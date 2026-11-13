using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI.Chat;

namespace Terraria.Unified.Features;

/// <summary>
/// HERO's Mod. Se abre/cierra con ~ (tilde) o F8.
///
/// - Un jugador (netMode 0): las acciones se aplican directamente al juego (igual que antes).
/// - Servidor, modo COMANDOS: las acciones se envian como comandos de chat a TShock, que decide
///   segun los permisos. Las acciones solo-cliente (vuelo, godmode local...) quedan apagadas.
/// - Servidor, modo DIRECTO (vanilla / anarquicos): el cliente actua por si mismo (vuelo, godmode,
///   items, jefes, teleports, butcher...) y sincroniza vida/buffs. Hora, clima y moderar a otros
///   jugadores NO se pueden hacer asi: necesitan TShock (modo COMANDOS) o ser el host.
///   El boton de arriba cambia de modo.
/// </summary>
internal static partial class HerosModPanel
{
    // Fuente de entidad usada por las acciones de invocación añadidas desde Zaza.
    private sealed class PanelEntitySource : IEntitySource
    {
        public string Context => "HerosModPanel";
    }

    private enum Tab { Jugador, Mundo, Items, EditorItems, EditorVida, WorldEdit, Jefes, Teleport, Jugadores, Herramientas, Inventario, Guia, Perfiles, Personalizar, NPCs, Proyectiles, AntiLag, FPSBoost, Cheats, Logs }

    // Dimensiones ampliadas para que los controles respiren y sean más fáciles de leer.
    private const int Width = 820;
    private const int Height = 850;
    // Dimensiones adaptables: conserva el diseño, pero evita que el panel exceda la pantalla.
    private static int VisiblePanelWidth => Math.Max(280, Math.Min(Width, Main.screenWidth - 12));
    private static int VisiblePanelHeight => Math.Max(220, Math.Min(Height, Main.screenHeight - 12));
    private const int Pad = 12;
    private const int RowH = 28;

    public static bool Visible { get; set; }

    // Consola independiente del panel HERO. No concede permisos: el servidor decide qué comandos acepta.
    private static bool commandConsoleVisible;
    // HUD superior opcional con información local de conexión, mundo y personaje.
    private static bool serverHudEnabled = false;
    // Posición del HUD: 0 arriba-izquierda, 1 arriba-derecha, 2 abajo-izquierda, 3 abajo-derecha, 4 centro-arriba, 5 centro-abajo.
    private static int serverHudPosition = 0;
    // Categorías configurables del HUD; las cuatro originales siguen activadas por defecto.
    private static bool hudShowConnection = true;
    private static bool hudShowPerformance = true;
    private static bool hudShowVitals = true;
    private static bool hudShowPositionTime = true;
    private static bool hudShowBiome;
    private static bool hudShowVelocity;
    private static bool hudShowInventory;
    private static bool hudShowCounts;
    private static bool commandConsolePreviousPanelVisible;
    private static string commandConsoleInput = string.Empty;
    private static string commandConsoleOutput = "Consola: Ctrl derecho abre/cierra · Alt derecho o Enter ejecuta · Esc cierra.";
    private static int commandConsoleOutputTimer;

    // Registro de eventos HERO en el chat, sin requerir que el panel esté abierto.
    private static bool eventLogEnabled = true;
    private static bool eventLogPlayersEnabled = true;
    private static bool eventLogDeathsEnabled = true;
    private static bool eventLogBossesEnabled = true;
    private static bool eventLogWorldEnabled = true;
    private static bool eventLogCombatEnabled = true;
    private static bool eventLogSystemEnabled = true;
    // Sistema de registro HERO ampliado: historial en memoria y archivo rotativo por día.
    private static bool eventLogFileEnabled = true;
    private static bool eventLogDebugEnabled;
    private static readonly Queue<string> herosLogHistory = new();
    private static readonly object herosLogSync = new();
    private const int HerosLogHistoryLimit = 160;
    private static int herosLogScroll;
    private static bool eventLogNPCEnabled = true;
    private static bool eventLogItemsEnabled;
    private static bool eventLogPerformanceEnabled = true;
    private static int eventLogLastNpcCount;
    private static int eventLogLastProjectileCount;
    private static int eventLogLastDustCount;
    private static int eventLogLastGoreCount;
    private static int eventLogLastLife;
    private static int eventLogLastMana;
    private static bool eventLogInitialized;
    private static bool eventLogLastDayTime;
    private static bool eventLogLastRaining;
    private static bool eventLogLastLowLife;
    private static bool eventLogLastLowMana;
    private static string eventLogLastBiome = string.Empty;
    private static int eventLogLastActiveBossCount;
    private static readonly bool[] eventLogPlayerActive = new bool[256];
    private static readonly bool[] eventLogPlayerDead = new bool[256];
    private static readonly string[] eventLogPlayerNames = new string[256];
    private static readonly int[] eventLogBossTypes = new int[Main.maxNPCs];

    private enum ServerMode { Comandos, Directo }

    /// <summary>
    /// Comandos = mandar comandos de chat a TShock (el servidor decide segun tus permisos).
    /// Directo  = el cliente actua por si mismo (servidores vanilla / anarquicos).
    /// </summary>
    private static ServerMode serverMode = ServerMode.Comandos;

    private static bool InMultiplayer => Main.netMode == 1;

    /// <summary>true = en un servidor y usando comandos de TShock.</summary>
    private static bool IsServer => Main.netMode == 1 && serverMode == ServerMode.Comandos;

    private static Tab currentTab = Tab.Jugador;
    private static bool englishUi;
    // Apariencia del panel; solo cambia el dibujo, no altera las funciones del mod.
    private static int uiTheme = 6;
    private static bool uiHighContrast;
    private static readonly Color[] UiAccents =
    {
        new Color(70, 130, 180), // Azul
        new Color(80, 165, 115), // Verde
        new Color(160, 105, 205), // Violeta
        new Color(210, 125, 65), // Naranja
        new Color(205, 80, 95), // Rojo
        new Color(255, 105, 190), // Anime Sakura
        new Color(110, 220, 255), // Anime Cyan
    };
    private static Vector2 position = new(120f, 80f);
    private static bool dragging;
    private static Vector2 dragOffset;

    private static bool godmodeEnabled;
    // Toggles portados de las categorías LocalPlayer del código de referencia.
    private static bool antiDebuffEnabled;
    private static bool noFallDamageEnabled;
    private static bool noKnockbackEnabled;
    private static bool infiniteBreathEnabled;
    // Opciones de NPC/tiendas aplicadas de forma reversible mediante reflexión para mantener compatibilidad.
    private static bool npcShopFreeEnabled;
    private static bool luckBoostEnabled;
    private static bool luckOriginalCaptured;
    private static float luckOriginalValue;
    private static object luckPlayerRef;
    private static object shoppingSettingsRef;
    private static object shoppingAdjustmentOriginal;
    private static bool shoppingAdjustmentCaptured;
    private static readonly Dictionary<object, object> shopPriceOriginals = new();
    // Modo fantasma visual local: oculta al jugador sin usar el estado de muerte/ghost de Terraria.
    private static bool ghostModeEnabled;
    private static bool adventureUnrestrictedEnabled;
    private static bool noSpawnsEnabled;
    private static bool flyEnabled;
    private const float FlySpeed = 12f;

    private static int activeField;
    private static string itemQuery = "";
    private static string itemAmount = "1";
    private static string itemPrefix = "-1";
    // Herramientas adicionales de moderación/comandos (requieren servidor con TShock/plugins).
    private static string moderationTarget = "";
    private static string moderationReason = "";
    private static int toolsPage;
    private static string toolSearchQuery = string.Empty;
    private static string editorSlot = "1";
    private static string schematicFileName = "construccion";
    private static string editorDamage = "";
    private static string editorDefense = "";
    private static string editorUseTime = "";
    private static string editorStack = "";
    private static string editorValue = "";
    private static string editorType = "1";
    private static string editorPrefix = "0";
    private static string editorKnockback = "";
    private static string editorCrit = "";
    private static string editorUseAnimation = "";
    private static string editorShootSpeed = "";
    private static string editorProjectile = "0";
    private static int editorProjectileMultiplier = 1;
    private static bool radialProjectileMode;
    private static readonly HashSet<int> multipliedProjectileIdentities = new();
    private static bool infiniteMinionsEnabled;
    private static bool lifeLockEnabled;
    private static string editorRarity = "";
    private static string editorMana = "0";
    private static string editorUseStyle = "1";
    private static string editorAmmo = "0";
    private static string editorScale = "1";
    private static bool editorAutoReuse;
    private static bool editorConsumable;
    private static bool editorFavorite;
    // Propiedades adicionales portadas del editor de Zaza, sin reemplazar las actuales.
    private static bool editorAdvancedPage;
    private static bool editorExtraPage;
    private static bool editorChannel;
    private static bool editorMelee = true;
    private static bool editorMagic;
    private static bool editorRanged;
    private static bool editorSummon;
    private static bool editorNoWet;
    private static bool editorNotAmmo;
    private static string editorFishingPole = "0";
    private static string editorBait = "0";
    private static string editorMakeNpc = "0";
    private static string editorTileWand = "-1";
    private static string editorPick = "0";
    private static string editorAxe = "0";
    private static string editorHealLife = "0";
    private static string editorHealMana = "0";
    private static string editorUseAmmo = "0";
    private static string editorCreateTile = "-1";
    private static string editorCreateWall = "-1";
    private static string editorBuffType = "0";
    private static string editorBuffTime = "0";
    private static string editorMountType = "-1";
    private static bool editorUseTurn;
    private static bool editorNoMelee;
    private static bool editorNoUseGraphic;
    private static string teleportX = "";
    private static string teleportY = "";
    private static bool worldEditSetPos1Next = true;
    private static string lifeCurrent = "";
    private static string lifeMaximum = "";
    private static string manaCurrent = "";
    private static string manaMaximum = "";
    private static string lifeDefense = "";
    private static int defenseOverride;
    private static bool defenseOverrideEnabled;
    private static Point? worldPos1;
    private static Point? worldPos2;
    private static string worldTileType = "1";
    private static string worldReplaceType = "0";
    private static string worldWallType = "1";
    private static string worldReplaceWallType = "0";
    private static string worldSchematicStatus = "Marca POS1 y POS2 para exportar.";
    private static int worldEditUndoType = -1;
    private static readonly List<(int x, int y, Tile tile)> worldEditUndo = new();
    // Portapapeles de WorldEdit en memoria: conserva tiles, paredes y líquidos.
    private static Tile[,] worldEditClipboard;
    private static int worldEditClipboardWidth;
    private static int worldEditClipboardHeight;

    private static readonly (string label, string command)[] extraTools = new (string, string)[]
    {
        ("Kick jugador", "/kick {target} {reason}"),
        ("Ban jugador", "/ban {target} {reason}"),
        ("Silenciar", "/mute {target}"),
        ("Quitar silencio", "/unmute {target}"),
        ("Teletransportarme", "/tp {target}"),
        ("Traer jugador", "/tphere {target}"),
        ("Eliminar jugador", "/kill {target}"),
        ("Curar jugador", "/heal {target}"),
        ("Slap / empujar", "/slap {target}"),
        ("Ver usuario", "/userinfo {target}"),
        ("Lista de jugadores", "/who"),
        ("Quién está jugando", "/playing"),
        ("Ver reglas", "/rules"),
        ("Mensaje del día", "/motd"),
        ("Ayuda del servidor", "/help"),
        ("Ir al spawn", "/spawn"),
        ("Volver a casa", "/home"),
        ("Volver atrás", "/back"),
        ("Listar warps", "/warp list"),
        ("Usar warp", "/warp {target}"),
        ("Eliminar enemigos", "/butcher"),
        ("Asentar líquidos", "/settle"),
        ("Poner de día", "/time day"),
        ("Poner de noche", "/time night"),
        ("Poner mediodía", "/time noon"),
        ("Poner medianoche", "/time midnight"),
        ("Iniciar lluvia", "/rain start"),
        ("Detener lluvia", "/rain stop"),
        ("Luna de sangre", "/bloodmoon"),
        ("Eclipse solar", "/eclipse"),
        ("Invasión goblin", "/invasion goblin"),
        ("Invasión helada", "/invasion frost"),
        ("Invasión pirata", "/invasion pirate"),
        ("Invasión marciana", "/invasion martian"),
        ("Luna de calabaza", "/invasion pumpkin"),
        ("Luna helada", "/invasion frostmoon"),
        ("Evento Solar", "/invasion solar"),
        ("Evento Vortex", "/invasion vortex"),
        ("Evento Nebula", "/invasion nebula"),
        ("Evento Stardust", "/invasion stardust"),
        ("Crecer plantas", "/grow"),
        ("Invocar meteorito", "/dropmeteor"),
        ("Activar Hardmode", "/hardmode"),
        ("Listar grupos", "/group list"),
        ("Listar usuarios", "/user list"),
        ("Listar regiones", "/region list"),
        ("Info de región", "/region info {target}"),
        ("Cambiar clave servidor", "/serverpassword {target}"),
        ("Buscar actualizaciones", "/checkupdates"),
        ("Info del mundo", "/world"),
        ("MODERACIÓN · Kick jugador", "/kick {target} {reason}"),
        ("MODERACIÓN · Ban jugador", "/ban {target} {reason}"),
        ("MODERACIÓN · Silenciar", "/mute {target}"),
        ("MODERACIÓN · Quitar silencio", "/unmute {target}"),
        ("MODERACIÓN · Perfil jugador", "/userinfo {target}"),
        ("MODERACIÓN · Lista conectados", "/playing"),
        ("MODERACIÓN · Kick global (plugin)", "/kickall {reason}"),
        ("MODERACIÓN · Advertencia (plugin)", "/warn {target} {reason}"),
        ("MODERACIÓN · Historial (plugin)", "/history {target}"),
        ("MODERACIÓN · Ayuda moderación", "/help"),
        ("USUARIOS · Listar usuarios", "/user list"),
        ("USUARIOS · Ver usuario", "/user info {target}"),
        ("USUARIOS · Crear usuario (admin)", "/user add {target}"),
        ("USUARIOS · Eliminar usuario (admin)", "/user del {target}"),
        ("USUARIOS · Listar grupos", "/group list"),
        ("USUARIOS · Ver grupo (plugin)", "/group {target}"),
        ("USUARIOS · Asignar grupo (plugin)", "/user group {target}"),
        ("USUARIOS · Permisos grupo (plugin)", "/group perm {target}"),
        ("USUARIOS · Añadir permiso (plugin)", "/group addperm {target}"),
        ("USUARIOS · Quitar permiso (plugin)", "/group delperm {target}"),
        ("REGIONES · Listar regiones", "/region list"),
        ("REGIONES · Info región", "/region info {target}"),
        ("REGIONES · Proteger región (plugin)", "/region protect {target}"),
        ("REGIONES · Eliminar región (admin)", "/region delete {target}"),
        ("REGIONES · Ajustar región (plugin)", "/region set {target}"),
        ("REGIONES · Añadir miembro (plugin)", "/region add {target}"),
        ("REGIONES · Quitar miembro (plugin)", "/region remove {target}"),
        ("REGIONES · Listar regiones", "/region list"),
        ("REGIONES · Ver protección (plugin)", "/region info {target}"),
        ("REGIONES · Ayuda regiones", "/help region"),
        ("TIEMPO · Poner día", "/time day"),
        ("TIEMPO · Poner noche", "/time night"),
        ("TIEMPO · Mediodía", "/time noon"),
        ("TIEMPO · Medianoche", "/time midnight"),
        ("TIEMPO · Consultar hora (plugin)", "/time"),
        ("TIEMPO · Hora personalizada (plugin)", "/time {target}"),
        ("TIEMPO · Restablecer día", "/time day"),
        ("TIEMPO · Cambiar a noche", "/time night"),
        ("TIEMPO · Ayuda tiempo", "/help time"),
        ("TIEMPO · Info mundo", "/world"),
        ("CLIMA · Iniciar lluvia", "/rain start"),
        ("CLIMA · Detener lluvia", "/rain stop"),
        ("CLIMA · Clima (plugin)", "/rain"),
        ("CLIMA · Iniciar tormenta", "/rain start"),
        ("CLIMA · Parar tormenta", "/rain stop"),
        ("CLIMA · Luna de sangre", "/bloodmoon"),
        ("CLIMA · Eclipse solar", "/eclipse"),
        ("CLIMA · Info clima (plugin)", "/world"),
        ("CLIMA · Ayuda clima", "/help rain"),
        ("CLIMA · Estado mundo", "/world"),
        ("EVENTOS · Invasión goblin", "/invasion goblin"),
        ("EVENTOS · Invasión pirata", "/invasion pirate"),
        ("EVENTOS · Invasión helada", "/invasion frost"),
        ("EVENTOS · Invasión marciana", "/invasion martian"),
        ("EVENTOS · Luna calabaza", "/invasion pumpkin"),
        ("EVENTOS · Luna helada", "/invasion frostmoon"),
        ("EVENTOS · Evento Solar", "/invasion solar"),
        ("EVENTOS · Evento Vortex", "/invasion vortex"),
        ("EVENTOS · Evento Nebula", "/invasion nebula"),
        ("EVENTOS · Evento Stardust", "/invasion stardust"),
        ("PROGRESIÓN · Activar Hardmode", "/hardmode"),
        ("PROGRESIÓN · Crecer plantas", "/grow"),
        ("PROGRESIÓN · Invocar meteorito", "/dropmeteor"),
        ("PROGRESIÓN · Asentar líquidos", "/settle"),
        ("PROGRESIÓN · Eliminar enemigos", "/butcher"),
        ("PROGRESIÓN · Progreso (plugin)", "/progress"),
        ("PROGRESIÓN · Estado mundo", "/world"),
        ("PROGRESIÓN · Ayuda progresión", "/help"),
        ("PROGRESIÓN · Info mundo", "/world"),
        ("PROGRESIÓN · Actualizar datos (plugin)", "/world"),
        ("TELETRANSPORTE · Ir al spawn", "/spawn"),
        ("TELETRANSPORTE · Volver a casa", "/home"),
        ("TELETRANSPORTE · Volver atrás", "/back"),
        ("TELETRANSPORTE · Listar warps", "/warp list"),
        ("TELETRANSPORTE · Usar warp", "/warp {target}"),
        ("TELETRANSPORTE · Teletransportar", "/tp {target}"),
        ("TELETRANSPORTE · Traer jugador", "/tphere {target}"),
        ("TELETRANSPORTE · TP aleatorio (plugin)", "/tpr"),
        ("TELETRANSPORTE · Guardar hogar (plugin)", "/home set"),
        ("TELETRANSPORTE · Ayuda teletransporte", "/help tp"),
        ("JUGADOR · Curar jugador", "/heal {target}"),
        ("JUGADOR · Eliminar jugador", "/kill {target}"),
        ("JUGADOR · Empujar jugador", "/slap {target}"),
        ("JUGADOR · Ver perfil", "/userinfo {target}"),
        ("JUGADOR · Cambiar equipo (plugin)", "/team {target}"),
        ("JUGADOR · Dificultad (plugin)", "/difficulty"),
        ("JUGADOR · Modo dios (plugin)", "/godmode {target}"),
        ("JUGADOR · Reaparecer (plugin)", "/respawn {target}"),
        ("JUGADOR · Estadísticas (plugin)", "/stats {target}"),
        ("JUGADOR · Ayuda jugador", "/help"),
        ("OBJETOS · Dar objeto (plugin)", "/give {target}"),
        ("OBJETOS · Crear objeto (plugin)", "/item {target}"),
        ("OBJETOS · Dar monedas (plugin)", "/give {target}"),
        ("OBJETOS · Dar munición (plugin)", "/give {target}"),
        ("OBJETOS · Dar pociones (plugin)", "/give {target}"),
        ("OBJETOS · Dar materiales (plugin)", "/give {target}"),
        ("OBJETOS · Dar bloques (plugin)", "/give {target}"),
        ("OBJETOS · Dar accesorio (plugin)", "/give {target}"),
        ("OBJETOS · Dar arma (plugin)", "/give {target}"),
        ("OBJETOS · Ayuda objetos", "/help item"),
        ("NPC · Eliminar enemigos", "/butcher"),
        ("NPC · Invocar NPC (plugin)", "/spawnmob {target}"),
        ("NPC · Eliminar NPC (plugin)", "/butcher"),
        ("NPC · Invocar jefe (plugin)", "/spawnmob {target}"),
        ("NPC · Lista NPC (plugin)", "/npc list"),
        ("NPC · Limpiar NPC", "/butcher"),
        ("NPC · Spawn rate (plugin)", "/spawnrate"),
        ("NPC · Máximo NPC (plugin)", "/maxspawns"),
        ("NPC · Ayuda NPC", "/help spawnmob"),
        ("NPC · Estado mundo", "/world"),
        ("SERVIDOR · Lista jugadores", "/who"),
        ("SERVIDOR · Quién juega", "/playing"),
        ("SERVIDOR · Ver reglas", "/rules"),
        ("SERVIDOR · Mensaje del día", "/motd"),
        ("SERVIDOR · Ayuda general", "/help"),
        ("SERVIDOR · Info mundo", "/world"),
        ("SERVIDOR · Buscar actualizaciones", "/checkupdates"),
        ("SERVIDOR · Info servidor (plugin)", "/serverinfo"),
        ("SERVIDOR · Versión servidor (plugin)", "/version"),
        ("SERVIDOR · Lista comandos", "/help"),
        ("CONFIGURACIÓN · Cambiar contraseña", "/serverpassword {target}"),
        ("CONFIGURACIÓN · Recargar config (plugin)", "/reload"),
        ("CONFIGURACIÓN · Lista blanca (plugin)", "/whitelist"),
        ("CONFIGURACIÓN · Añadir lista blanca (plugin)", "/whitelist add {target}"),
        ("CONFIGURACIÓN · Quitar lista blanca (plugin)", "/whitelist remove {target}"),
        ("CONFIGURACIÓN · Cambiar MOTD (plugin)", "/motd {target}"),
        ("CONFIGURACIÓN · Editar reglas (plugin)", "/rules {target}"),
        ("CONFIGURACIÓN · Ver permisos (plugin)", "/checkperm {target}"),
        ("CONFIGURACIÓN · Ayuda admin", "/help"),
        ("CONFIGURACIÓN · Buscar actualizaciones", "/checkupdates"),
        ("ECONOMÍA · Ver saldo (plugin)", "/balance {target}"),
        ("ECONOMÍA · Consulta economía (plugin)", "/eco {target}"),
        ("ECONOMÍA · Dar dinero (plugin)", "/eco give {target}"),
        ("ECONOMÍA · Quitar dinero (plugin)", "/eco take {target}"),
        ("ECONOMÍA · Establecer saldo (plugin)", "/eco set {target}"),
        ("ECONOMÍA · Ranking dinero (plugin)", "/baltop"),
        ("ECONOMÍA · Tienda (plugin)", "/shop"),
        ("ECONOMÍA · Ver precio (plugin)", "/price {target}"),
        ("ECONOMÍA · Transferir dinero (plugin)", "/pay {target}"),
        ("ECONOMÍA · Ayuda economía (plugin)", "/help eco"),
        ("UTILIDADES · Ver ping (plugin)", "/ping {target}"),
        ("UTILIDADES · Limpiar chat (plugin)", "/clear"),
        ("UTILIDADES · Anuncio (plugin)", "/announce {target}"),
        ("UTILIDADES · Mensaje global (plugin)", "/broadcast {target}"),
        ("UTILIDADES · Ver logs (plugin)", "/logs"),
        ("UTILIDADES · Historial (plugin)", "/history {target}"),
        ("UTILIDADES · Estadísticas (plugin)", "/stats"),
        ("UTILIDADES · Lista plugins (plugin)", "/plugins"),
        ("UTILIDADES · Versión (plugin)", "/version"),
        ("UTILIDADES · Ayuda utilidades", "/help")
    };


    private static string statusMessage = "HERO's Mod listo.";
    private static int statusTimer;

    // Anti-Lag local adaptativo. No modifica el servidor y no puede impedir cada spawn antes de ocurrir.
    private static bool antiLagEnabled;
    private static bool antiEffectsEnabled;
    private static int antiEffectsFrameCounter;
    private static int antiLagFrameCounter;
    private static long antiLagParticlesCleared;
    private static long antiLagGoreCleared;
    private static int antiLagProjectileLimit = 250;
    private static bool antiLagProjectilesEnabled;
    private static int antiLagProjectileFrameCounter;
    private static long antiLagProjectilesRemoved;
    private static bool fpsStabilizerEnabled;
    private static int fpsStabilizerFrameCounter;
    private static int fpsStabilizerThreshold = 45;
    private static long fpsStabilizerInterventions;
    private static long antiLagEmergencyInterventions;
    private static bool muteGameSoundsEnabled;
    private static float soundVolumeBeforeMute = 1f;
    private static bool settingsLoaded;
    private static int settingsSaveCounter;
    private static string lastSavedSettingsSnapshot;
    private static int antiLagLastProjectileCount;
    private static int antiLagLastDustCount;
    private static int antiLagLastGoreCount;

    // ---- Navegador de items (pestana Items) ----
    private const int CellSize = 40;
    private const int BarW = 12;
    private static string[] itemNames;
    private static List<int> allItemIds;
    private static readonly List<int> browserIds = new();
    private static string browserFilterKey;
    private static float browserScroll;
    private static bool browserDragging;
    private static float browserDragStartY;
    private static float browserDragStartScroll;
    private static string hoverTip;

    // Portapapeles local: guarda una copia de los objetos del inventario para pegarla
    // Ãºnicamente en espacios vacÃ­os. No sobrescribe objetos existentes.
    private static Item[] inventoryClipboard;
    private static string inventoryClipboardOwner = "";
    private static int inventoryPage;

    private static bool Clicked => Main.mouseLeft && Main.mouseLeftRelease;

    private static void DisableAllActiveFeatures()
    {
        // Apaga funciones activas sin borrar datos, selecciones, clips ni ajustes guardados.
        godmodeEnabled = false;
        npcShopFreeEnabled = false;
        luckBoostEnabled = false;
        ghostModeEnabled = false;
        adventureUnrestrictedEnabled = false;
        noSpawnsEnabled = false;
        flyEnabled = false;
        radialProjectileMode = false;
        infiniteMinionsEnabled = false;
        lifeLockEnabled = false;
        defenseOverrideEnabled = false;
        antiLagEnabled = false;
        antiEffectsEnabled = false;
        antiLagProjectilesEnabled = false;
        fpsStabilizerEnabled = false;
        editorAutoReuse = false;
        editorConsumable = false;
        editorFavorite = false;
        editorChannel = false;
        editorMelee = false;
        editorMagic = false;
        editorRanged = false;
        editorSummon = false;
        editorNoWet = false;
        editorNotAmmo = false;
        editorUseTurn = false;
        editorNoMelee = false;
        editorNoUseGraphic = false;

        // Restaurar los valores que estas funciones modifican temporalmente.
        try
        {
            if (muteGameSoundsEnabled)
                Main.soundVolume = Math.Clamp(soundVolumeBeforeMute, 0f, 1f);
        }
        catch { }
        muteGameSoundsEnabled = false;
        try { RestoreShopPrices(); } catch { }
        try { RestoreLuckValue(); } catch { }
        try
        {
            if (!IsServer && Main.LocalPlayer != null && Main.LocalPlayer.active)
                Main.LocalPlayer.creativeGodMode = false;
        }
        catch { }

        journeyResearchRunning = false;
        journeyResearchTracker = null;
        journeyResearchSetter = null;
        SetStatus("Todas las funciones activas se desactivaron. No se borraron selecciones, clips ni ajustes guardados.");
        SaveSettingsIfChanged(true);
    }

    public static void UpdateLogic()
    {
        if (Main.dedServ || Main.gameMenu)
            return;

        EnsureSettingsLoaded();
        ProcessJourneyResearchUnlock();
        ApplySoundMuteSetting();
        ApplyNpcShopFreeSetting();
        ApplyLuckBoostSetting();
        if (++settingsSaveCounter >= 30)
        {
            settingsSaveCounter = 0;
            SaveSettingsIfChanged();
        }

        // Anti-efectos visuales local: limpia polvo y gore periódicamente sin tocar el servidor.
        if (antiEffectsEnabled && ++antiEffectsFrameCounter >= 10)
        {
            antiEffectsFrameCounter = 0;
            ClearAntiLagDust();
            ClearAntiLagGore();
        }

        // Contadores baratos y revisión frecuente: actúa antes de que la carga crezca demasiado.
        if (antiLagEnabled && ++antiLagFrameCounter >= 20)
        {
            antiLagFrameCounter = 0;
            antiLagLastDustCount = CountActiveDust();
            antiLagLastGoreCount = CountActiveGore();
            if (antiLagLastDustCount > 350 || (fpsStabilizerEnabled && Main.frameRate > 0 && Main.frameRate < fpsStabilizerThreshold))
            {
                ClearAntiLagDust();
                ClearAntiLagGore();
            }
        }

        if ((antiLagProjectilesEnabled || fpsStabilizerEnabled) && ++antiLagProjectileFrameCounter >= 5)
        {
            antiLagProjectileFrameCounter = 0;
            antiLagLastProjectileCount = CountActiveProjectiles();
            bool lowFps = Main.frameRate > 0 && Main.frameRate < fpsStabilizerThreshold;
            bool emergency = (Main.frameRate > 0 && Main.frameRate < 20)
                || antiLagLastProjectileCount >= antiLagProjectileLimit * 2;
            if (antiLagProjectilesEnabled && antiLagLastProjectileCount > antiLagProjectileLimit)
            {
                int target = emergency ? Math.Max(40, antiLagProjectileLimit / 2) : antiLagProjectileLimit;
                int removed = TrimProjectilesForPerformance(false, target);
                if (emergency && removed > 0) antiLagEmergencyInterventions++;
            }
            if (fpsStabilizerEnabled && lowFps)
            {
                int dustCleared = ClearAntiLagDust();
                int goreCleared = ClearAntiLagGore();
                int projectilesRemoved = antiLagProjectilesEnabled
                    ? TrimProjectilesForPerformance(false, Math.Max(40, antiLagProjectileLimit / 2))
                    : 0;
                if (dustCleared > 0 || goreCleared > 0 || projectilesRemoved > 0)
                    fpsStabilizerInterventions++;
            }
        }

        var local = Main.LocalPlayer;

        // Función adaptada de HERO: amplía el alcance de colocación/minado solo para el jugador local.
        ApplyExtendedReach();

        // Adaptaciones locales de cheats básicos de la referencia: solo afectan al personaje local.
        if (local != null && local.active && !local.dead)
        {
            if (noFallDamageEnabled) local.noFallDmg = true;
            if (noKnockbackEnabled) local.noKnockback = true;
            if (infiniteBreathEnabled) local.breath = local.breathMax;
            if (antiDebuffEnabled)
            {
                for (int i = local.buffType.Length - 1; i >= 0; i--)
                {
                    int buff = local.buffType[i];
                    if (buff > 0 && buff < Main.debuff.Length && Main.debuff[buff])
                        local.DelBuff(i);
                }
            }
        }

        // Multiplica proyectiles nuevos del arma equipada una sola vez por identidad.
        ApplyProjectileMultiplier(local);
        if (infiniteMinionsEnabled && local.active && !local.dead)
            local.maxMinions = 9999;
        if (lifeLockEnabled && local.active && !local.dead && !IsServer)
        {
            local.statLife = Math.Max(1, local.statLifeMax2);
            local.statMana = Math.Max(0, local.statManaMax2);
        }

        // En servidor las funciones solo-cliente quedan apagadas (el servidor manda).
        if (IsServer && flyEnabled)
        {
            flyEnabled = false;
        }

        if (defenseOverrideEnabled && local.active && !IsServer)
        {
            // Refuerza la defensa configurada en el editor local; el servidor puede sobrescribirla.
            local.statDefense = defenseOverride;
        }

        if (godmodeEnabled && local.active)
        {
            // God Mode local sin el bloqueo por modo de panel/red.
            // En multijugador, la invulnerabilidad real requiere que el servidor aplique esta lógica.
            local.creativeGodMode = true;
            local.statLife = Math.Max(1, local.statLifeMax2);
            local.statMana = Math.Max(0, local.statManaMax2);
            local.immune = true;
            local.immuneTime = Math.Max(local.immuneTime, 2);
            for (int i = local.buffType.Length - 1; i >= 0; i--)
            {
                int buff = local.buffType[i];
                if (buff > 0 && buff < Main.debuff.Length && Main.debuff[buff])
                    local.DelBuff(i);
            }
        }
        else if (local.active && !godmodeEnabled)
        {
            local.creativeGodMode = false;
        }

        if (adventureUnrestrictedEnabled && local.active)
        {
            ApplyUnrestrictedAdventurePermissions(true);
        }

        if (flyEnabled && local.active && activeField == 0 && !Main.drawingPlayerChat)
        {
            var ks = Main.keyState;
            Vector2 dir = Vector2.Zero;
            if (ks.IsKeyDown(Keys.W) || ks.IsKeyDown(Keys.Up)) dir.Y -= 1f;
            if (ks.IsKeyDown(Keys.S) || ks.IsKeyDown(Keys.Down)) dir.Y += 1f;
            if (ks.IsKeyDown(Keys.A) || ks.IsKeyDown(Keys.Left)) dir.X -= 1f;
            if (ks.IsKeyDown(Keys.D) || ks.IsKeyDown(Keys.Right)) dir.X += 1f;
            if (ks.IsKeyDown(Keys.Space)) dir.Y -= 1f;
            if (ks.IsKeyDown(Keys.LeftShift)) dir.Y += 1f;

            if (dir != Vector2.Zero)
            {
                dir.Normalize();
                local.velocity = dir * FlySpeed;
            }
            else
            {
                local.velocity *= 0.85f;
            }
        }
    }


    private static string HerosLogDirectory
    {
        get
        {
            string root = string.IsNullOrWhiteSpace(Main.SavePath) ? AppContext.BaseDirectory : Main.SavePath;
            return Path.Combine(root, "HERO", "Logs");
        }
    }

    private static void WriteHerosLog(string message, Color color, string category = "EVENTO")
    {
        string safeMessage = message ?? string.Empty;
        string line = DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture) +
            " [" + (category ?? "EVENTO") + "] " + safeMessage;
        lock (herosLogSync)
        {
            herosLogHistory.Enqueue(line);
            while (herosLogHistory.Count > HerosLogHistoryLimit)
                herosLogHistory.Dequeue();
            if (eventLogFileEnabled)
            {
                try
                {
                    Directory.CreateDirectory(HerosLogDirectory);
                    string path = Path.Combine(HerosLogDirectory, "heros-" + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ".log");
                    File.AppendAllText(path, line + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    if (eventLogDebugEnabled)
                        try { Console.Error.WriteLine("[HERO LOG] " + ex); } catch { }
                }
            }
        }
        if (!Main.dedServ && !Main.gameMenu && Main.netMode != 2)
            Main.NewText(safeMessage, color);
    }

    private static string[] GetHerosLogSnapshot()
    {
        lock (herosLogSync)
            return herosLogHistory.ToArray();
    }

    private static void UpdateEventChatLog()
    {
        if (!eventLogEnabled || Main.dedServ || Main.gameMenu || Main.netMode == 2)
            return;

        Player local = Main.LocalPlayer;
        string biome = local == null || !local.active ? "Sin personaje" :
            local.ZoneDungeon ? "Mazmorra" :
            local.ZoneUnderworldHeight ? "Inframundo" :
            local.ZoneJungle ? "Jungla" :
            local.ZoneSnow ? "Nieve" :
            local.ZoneDesert ? "Desierto" :
            local.ZoneBeach ? "Playa" :
            local.ZoneCorrupt ? "Corrupción" :
            local.ZoneCrimson ? "Carmesí" :
            local.ZoneHallow ? "Sagrado" : "Superficie/subsuelo/otro";

        int activeBossCount = 0;
        for (int i = 0; i < Main.npc.Length && i < eventLogBossTypes.Length; i++)
        {
            NPC n = Main.npc[i];
            if (n != null && n.active && n.boss)
                activeBossCount++;
        }

        int activeNpcCount = 0;
        int activeProjectileCount = 0;
        int activeDustCount = 0;
        int activeGoreCount = 0;
        for (int i = 0; i < Main.npc.Length; i++)
            if (Main.npc[i] != null && Main.npc[i].active) activeNpcCount++;
        for (int i = 0; i < Main.projectile.Length; i++)
            if (Main.projectile[i] != null && Main.projectile[i].active) activeProjectileCount++;
        for (int i = 0; i < Main.dust.Length; i++)
            if (Main.dust[i] != null && Main.dust[i].active) activeDustCount++;
        for (int i = 0; i < Main.gore.Length; i++)
            if (Main.gore[i] != null && Main.gore[i].active) activeGoreCount++;

        // Inicializa el estado para evitar falsos anuncios al cargar el mundo.
        if (!eventLogInitialized)
        {
            for (int i = 0; i < Main.player.Length && i < eventLogPlayerActive.Length; i++)
            {
                Player p = Main.player[i];
                eventLogPlayerActive[i] = p != null && p.active;
                eventLogPlayerDead[i] = p != null && p.dead;
                eventLogPlayerNames[i] = p?.name ?? ("Jugador " + i);
            }
            for (int i = 0; i < Main.npc.Length && i < eventLogBossTypes.Length; i++)
            {
                NPC n = Main.npc[i];
                eventLogBossTypes[i] = n != null && n.active && n.boss ? n.type : 0;
            }
            eventLogLastDayTime = Main.dayTime;
            eventLogLastRaining = Main.raining;
            eventLogLastLowLife = local != null && local.active && local.statLife > 0 && local.statLifeMax2 > 0 && local.statLife <= Math.Max(1, local.statLifeMax2 / 4);
            eventLogLastLowMana = local != null && local.active && local.statManaMax2 > 0 && local.statMana <= Math.Max(1, local.statManaMax2 / 4);
            eventLogLastBiome = biome;
            eventLogLastActiveBossCount = activeBossCount;
            eventLogInitialized = true;
            if (eventLogSystemEnabled)
                WriteHerosLog("[HERO | SISTEMA] Registro ampliado activo: jugadores, muertes, jefes, mundo, clima, biomas y alertas de combate.", new Color(120, 200, 255));
            return;
        }

        // Entradas y salidas de jugadores visibles para este cliente.
        if (eventLogPlayersEnabled)
        {
            for (int i = 0; i < Main.player.Length && i < eventLogPlayerActive.Length; i++)
            {
                Player p = Main.player[i];
                bool active = p != null && p.active;
                string name = p?.name;
                if (active && !eventLogPlayerActive[i])
                    WriteHerosLog("[HERO | ENTRADA] " + (string.IsNullOrWhiteSpace(name) ? "Jugador " + i : name) + " entró al mundo.", new Color(100, 220, 150));
                else if (!active && eventLogPlayerActive[i] && i != Main.myPlayer)
                    WriteHerosLog("[HERO | SALIDA] " + (string.IsNullOrWhiteSpace(eventLogPlayerNames[i]) ? "Jugador " + i : eventLogPlayerNames[i]) + " salió del mundo.", new Color(235, 170, 100));
                if (active && !string.IsNullOrWhiteSpace(name)) eventLogPlayerNames[i] = name;
                eventLogPlayerActive[i] = active;
            }
        }
        else
        {
            for (int i = 0; i < Main.player.Length && i < eventLogPlayerActive.Length; i++)
            {
                Player p = Main.player[i];
                eventLogPlayerActive[i] = p != null && p.active;
                if (p != null && p.active && !string.IsNullOrWhiteSpace(p.name)) eventLogPlayerNames[i] = p.name;
            }
        }

        if (eventLogDeathsEnabled)
        {
            for (int i = 0; i < Main.player.Length && i < eventLogPlayerDead.Length; i++)
            {
                Player p = Main.player[i];
                bool active = p != null && p.active;
                bool dead = active && p.dead;
                if (active && dead && !eventLogPlayerDead[i])
                    WriteHerosLog("[HERO | MUERTE] " + (string.IsNullOrWhiteSpace(p.name) ? "Jugador " + i : p.name) + " murió.", new Color(255, 100, 100));
                eventLogPlayerDead[i] = dead;
            }
        }
        else
        {
            for (int i = 0; i < Main.player.Length && i < eventLogPlayerDead.Length; i++)
                eventLogPlayerDead[i] = Main.player[i] != null && Main.player[i].active && Main.player[i].dead;
        }

        if (eventLogBossesEnabled)
        {
            for (int i = 0; i < Main.npc.Length && i < eventLogBossTypes.Length; i++)
            {
                NPC n = Main.npc[i];
                int type = n != null && n.active && n.boss ? n.type : 0;
                if (type != 0 && eventLogBossTypes[i] != type)
                    WriteHerosLog("[HERO | JEFE] Apareció: " + n.FullName + " (ID " + n.type + ", vida " + n.life + "/" + n.lifeMax + ").", new Color(255, 205, 90));
                else if (type == 0 && eventLogBossTypes[i] != 0)
                    WriteHerosLog("[HERO | JEFE] El jefe ID " + eventLogBossTypes[i] + " ya no está activo; pudo ser derrotado o desaparecer.", new Color(255, 205, 90));
                eventLogBossTypes[i] = type;
            }
        }
        else
        {
            for (int i = 0; i < Main.npc.Length && i < eventLogBossTypes.Length; i++)
            {
                NPC n = Main.npc[i];
                eventLogBossTypes[i] = n != null && n.active && n.boss ? n.type : 0;
            }
        }

        if (eventLogWorldEnabled)
        {
            if (Main.dayTime != eventLogLastDayTime)
                WriteHerosLog(Main.dayTime ? "[HERO | MUNDO] Comienza el día." : "[HERO | MUNDO] Comienza la noche.", new Color(255, 220, 130));
            if (Main.raining != eventLogLastRaining)
                WriteHerosLog(Main.raining ? "[HERO | CLIMA] Ha comenzado la lluvia." : "[HERO | CLIMA] La lluvia ha terminado.", new Color(120, 190, 255));
            if (!string.Equals(biome, eventLogLastBiome, StringComparison.Ordinal) && local != null && local.active)
                WriteHerosLog("[HERO | BIOMA] Zona actual: " + biome + ".", new Color(100, 220, 210));
        }
        eventLogLastDayTime = Main.dayTime;
        eventLogLastRaining = Main.raining;
        eventLogLastBiome = biome;

        // Alertas por umbral: no repite mensajes cada fotograma mientras la vida/maná siga bajo.
        if (local != null && local.active && eventLogCombatEnabled)
        {
            bool lowLife = local.statLife > 0 && local.statLifeMax2 > 0 && local.statLife <= Math.Max(1, local.statLifeMax2 / 4);
            bool lowMana = local.statManaMax2 > 0 && local.statMana <= Math.Max(1, local.statManaMax2 / 4);
            if (lowLife && !eventLogLastLowLife)
                WriteHerosLog("[HERO | COMBATE] ¡Vida crítica! " + local.statLife + "/" + local.statLifeMax2 + " vida; defensa " + local.statDefense + ".", new Color(255, 80, 80));
            if (lowMana && !eventLogLastLowMana)
                WriteHerosLog("[HERO | COMBATE] Maná bajo: " + local.statMana + "/" + local.statManaMax2 + ".", new Color(100, 150, 255));
            if (!lowLife && local.statLifeMax2 > 0 && local.statLife > local.statLifeMax2 * 0.35f) eventLogLastLowLife = false;
            else if (lowLife) eventLogLastLowLife = true;
            if (!lowMana && local.statManaMax2 > 0 && local.statMana > local.statManaMax2 * 0.35f) eventLogLastLowMana = false;
            else if (lowMana) eventLogLastLowMana = true;
        }
        else if (local != null && local.active)
        {
            eventLogLastLowLife = local.statLife > 0 && local.statLifeMax2 > 0 && local.statLife <= Math.Max(1, local.statLifeMax2 / 4);
            eventLogLastLowMana = local.statManaMax2 > 0 && local.statMana <= Math.Max(1, local.statManaMax2 / 4);
        }

        if (eventLogWorldEnabled && activeBossCount != eventLogLastActiveBossCount)
        {
            WriteHerosLog("[HERO | MUNDO] Cantidad de jefes activos: " + activeBossCount + ".", new Color(255, 190, 100));
        }
        eventLogLastActiveBossCount = activeBossCount;

        if (eventLogNPCEnabled && Math.Abs(activeNpcCount - eventLogLastNpcCount) >= 3)
            WriteHerosLog("[HERO | NPC] Entidades activas: " + activeNpcCount + " (jefes: " + activeBossCount + ").", new Color(180, 190, 220), "NPC");
        if (eventLogPerformanceEnabled &&
            (Math.Abs(activeProjectileCount - eventLogLastProjectileCount) >= 25 ||
             Math.Abs(activeDustCount - eventLogLastDustCount) >= 80 ||
             Math.Abs(activeGoreCount - eventLogLastGoreCount) >= 30))
            WriteHerosLog("[HERO | RENDIMIENTO] Proyectiles: " + activeProjectileCount +
                ", partículas: " + activeDustCount + ", gore: " + activeGoreCount + ".", new Color(120, 200, 255), "PERFORMANCE");
        if (local != null && local.active && eventLogItemsEnabled &&
            (local.statLife != eventLogLastLife || local.statMana != eventLogLastMana))
            WriteHerosLog("[HERO | ESTADO] Vida " + local.statLife + "/" + local.statLifeMax2 +
                ", maná " + local.statMana + "/" + local.statManaMax2 + ".", new Color(160, 190, 255), "PLAYER");
        eventLogLastNpcCount = activeNpcCount;
        eventLogLastProjectileCount = activeProjectileCount;
        eventLogLastDustCount = activeDustCount;
        eventLogLastGoreCount = activeGoreCount;
        if (local != null && local.active)
        {
            eventLogLastLife = local.statLife;
            eventLogLastMana = local.statMana;
        }
    }

    public static void Draw(SpriteBatch sb)
    {
        if (Main.dedServ || Main.gameMenu)
            return;

        UpdateEventChatLog();
        UpdateLogic();
        HandleToggleKey();

        var pixel = TextureAssets.MagicPixel.Value;
        DrawWorldSelectionOverlay(sb, pixel);

        // La HUD flotante queda desactivada; sus datos se consultan desde la categoría Información.

        // Al abrir la consola se oculta visualmente HERO para que solo aparezca la consola.
        if (commandConsoleVisible)
        {
            DrawCommandConsole(sb, pixel);
            HandleCommandConsoleTyping();
            return;
        }

        if (!Visible)
            return;

        hoverTip = null;
        var mouse = new Point(Main.mouseX, Main.mouseY);
        int safePanelHeight = VisiblePanelHeight;
        // El diseño interno usa Width como referencia; mantener el ancho fijo evita desalinear controles.
        int safePanelWidth = VisiblePanelWidth;
        var panelRect = new Rectangle(
            Math.Clamp((int)position.X, 0, Math.Max(0, Main.screenWidth - safePanelWidth)),
            Math.Clamp((int)position.Y, 0, Math.Max(0, Main.screenHeight - safePanelHeight)),
            safePanelWidth, safePanelHeight);
        position = new Vector2(panelRect.X, panelRect.Y);

        if (panelRect.Contains(mouse))
            Main.LocalPlayer.mouseInterface = true;

        var titleRect = new Rectangle(panelRect.X, panelRect.Y, panelRect.Width, 40);
        if (dragging)
        {
            if (Main.mouseLeft)
            {
                position = new Vector2(mouse.X, mouse.Y) - dragOffset;
                position.X = Math.Clamp(position.X, 0f, Math.Max(0f, Main.screenWidth - panelRect.Width));
                position.Y = Math.Clamp(position.Y, 0f, Math.Max(0f, Main.screenHeight - panelRect.Height));
            }
            else
            {
                dragging = false;
            }
        }
        else if (Clicked && titleRect.Contains(mouse))
        {
            dragging = true;
            dragOffset = new Vector2(mouse.X, mouse.Y) - position;
            Main.mouseLeftRelease = false;
        }

        // Nuevo marco visual: sombra, borde, cabecera y área de contenido separadas.
        sb.Draw(pixel, new Rectangle(panelRect.X + 5, panelRect.Y + 6, panelRect.Width, panelRect.Height), new Color(0, 0, 0, 105));
        sb.Draw(pixel, panelRect, new Color(8, 10, 18, 245));
        sb.Draw(pixel, new Rectangle(panelRect.X, panelRect.Y, panelRect.Width, 2), UiAccent);
        sb.Draw(pixel, new Rectangle(panelRect.X, panelRect.Y + 2, panelRect.Width, panelRect.Height - 2), UiPanelColor);
        sb.Draw(pixel, titleRect, UiTitleColor);
        sb.Draw(pixel, new Rectangle(panelRect.X, panelRect.Y + 40, panelRect.Width, 1), UiAccent * 0.75f);
        // El botón ES/EN ocupa de panelRect.X + Pad a panelRect.X + Pad + 54; el título va a su derecha.
        Utils.DrawBorderString(sb, "HERO's Mod", new Vector2(panelRect.X + Pad + 66, panelRect.Y + 5), Color.White, 1.0f);
        Utils.DrawBorderString(sb, "CENTRO DE CONTROL  /  TERRARIA", new Vector2(panelRect.X + Pad + 66 + 130, panelRect.Y + 12), new Color(177, 185, 205), 0.58f);

        string modeText = !InMultiplayer
            ? (englishUi ? "MODE: SINGLE PLAYER" : "MODO: UN JUGADOR")
            : (serverMode == ServerMode.Comandos
                ? (englishUi ? "PERMISSION SERVER (TShock)" : "SERVIDOR CON PERMISOS (TShock)")
                : (englishUi ? "ANARCHY / DIRECT" : "ANÁRQUICO / DIRECTO"));
        Color modeColor = !InMultiplayer ? new Color(35, 95, 75) : (serverMode == ServerMode.Comandos ? new Color(35, 82, 125) : new Color(125, 78, 30));
        if (DrawButton(sb, pixel, new Rectangle(panelRect.Right - 218, panelRect.Y + 8, 180, 24), modeText, mouse, modeColor))
        {
            ToggleServerMode();
        }

        if (DrawButton(sb, pixel, new Rectangle(panelRect.Right - 335, panelRect.Y + 8, 108, 24), (englishUi ? "Disable all" : "Desactivar todo"), mouse, new Color(145, 55, 55)))
        {
            DisableAllActiveFeatures();
        }

        if (DrawButton(sb, pixel, new Rectangle(panelRect.X + Pad, panelRect.Y + 8, 54, 24), englishUi ? "EN" : "ES", mouse, new Color(75, 90, 125)))
            englishUi = !englishUi;

        var closeRect = new Rectangle(panelRect.Right - 30, panelRect.Y + 8, 22, 24);
        if (DrawButton(sb, pixel, closeRect, "×", mouse, new Color(150, 45, 60)))
        {
            Visible = false;
            return;
        }

        // Navegación agrupada por tarea para que las funciones sean más fáciles de encontrar.
        Tab[][] tabGroups =
        [
            [Tab.Jugador, Tab.EditorVida, Tab.Inventario, Tab.Items, Tab.EditorItems],
            [Tab.Mundo, Tab.WorldEdit, Tab.Teleport, Tab.Perfiles, Tab.Guia],
            [Tab.Jugadores, Tab.Jefes, Tab.Herramientas, Tab.Personalizar, Tab.Cheats],
            [Tab.NPCs, Tab.Proyectiles, Tab.AntiLag, Tab.FPSBoost, Tab.Logs]
        ];
        string[] groupLabels = englishUi ? ["CHARACTER / INVENTORY", "WORLD / BUILDING", "ADMIN / CHEATS", "ENTITIES / PERFORMANCE / LOGS"] : ["PERSONAJE / INVENTARIO", "MUNDO / CONSTRUCCIÓN", "ADMIN / CATEGORÍAS", "ENTIDADES / RENDIMIENTO / REGISTROS"];
        string[] tabLabels = englishUi
            ? ["Player", "Life", "Inventory", "Items", "Item Editor", "World", "WorldEdit", "Teleport", "Profiles", "Guide", "Players", "Bosses", "Tools", "Customize", "Cheats", "NPCs", "Projectiles", "Anti-Lag", "FPS Boost", "Logs"]
            : ["Jugador", "Vida", "Inventario", "Items", "Editor items", "Mundo", "WorldEdit", "Teleport", "Perfiles", "Guía", "Jugadores", "Jefes", "Herramientas", "Personalizar", "Categorías", "NPCs", "Proyectiles", "Anti-Lag", "FPS Boost", "Registros"];
        int tabCols = 5;
        int tabWidth = (Width - (Pad * 2) - ((tabCols - 1) * 6)) / tabCols;
        // Separadores finos y etiquetas de sección para una navegación más parecida a Terraria/Calamity.
        // Navegación compacta en pantallas bajas para reservar espacio al contenido.
        bool compactNavigation = Main.screenHeight < 760;
        int navRowStep = compactNavigation ? 25 : 34;
        int navButtonTop = compactNavigation ? 48 : 55;
        int navButtonHeight = compactNavigation ? 19 : 22;
        // Los grupos no tienen el mismo número de pestañas (5, 5, 4, 4), así que el índice de la
        // etiqueta debe ser un contador acumulado; row * tabCols + col se salía del arreglo en la
        // última fila y lanzaba IndexOutOfRangeException, abortando el resto del dibujo del juego.
        int labelCounter = 0;
        for (int row = 0; row < tabGroups.Length; row++)
        {
            int labelY = panelRect.Y + 44 + row * navRowStep;
            if (!compactNavigation)
                Utils.DrawBorderString(sb, groupLabels[row], new Vector2(panelRect.X + Pad + 2, labelY), new Color(190, 175, 220), 0.52f);
            for (int col = 0; col < tabGroups[row].Length; col++)
            {
                Tab tab = tabGroups[row][col];
                int labelIndex = Math.Min(labelCounter++, tabLabels.Length - 1);
                var tabRect = new Rectangle(panelRect.X + Pad + col * (tabWidth + 6),
                    panelRect.Y + navButtonTop + row * navRowStep, tabWidth, navButtonHeight);
                bool selected = currentTab == tab;
                Color tabColor = selected ? UiAccent : UiTabColor;
                if (DrawButton(sb, pixel, tabRect, tabLabels[labelIndex], mouse, tabColor) && !selected)
                {
                    currentTab = tab;
                    activeField = 0;
                }
            }
        }

        int contentY = panelRect.Y + (compactNavigation ? 151 : 185);
        // En pantallas muy bajas, reservar un área de contenido no negativa.
        contentY = Math.Min(contentY, panelRect.Bottom - 28);
        var contentRect = new Rectangle(panelRect.X + 5, contentY - 3, panelRect.Width - 10, Math.Max(1, panelRect.Bottom - contentY - 8));
        sb.Draw(pixel, contentRect, new Color(13, 15, 24, 210));
        sb.Draw(pixel, new Rectangle(contentRect.X, contentRect.Y, 2, contentRect.Height), UiAccent * 0.65f);
        // Red de seguridad: si una pestaña falla, el error se muestra en el panel y NO se propaga al
        // dibujado del juego (que dejaba sin cursor, HUD ni minimapa).
        try
        {
            switch (currentTab)
            {
                case Tab.Jugador:
                    DrawPlayerTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.Mundo:
                    DrawWorldTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.Items:
                    DrawItemsTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.EditorItems:
                    DrawItemEditorTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.EditorVida:
                    DrawLifeEditorTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.WorldEdit:
                    DrawWorldEditTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.Jefes:
                    DrawBossesTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.Teleport:
                    DrawTeleportTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.Jugadores:
                    DrawPlayersTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.NPCs:
                    DrawNPCsTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.Proyectiles:
                    DrawProjectilesTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.AntiLag:
                    DrawAntiLagTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.FPSBoost:
                    DrawFPSBoostTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.Inventario:
                    DrawInventoryTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.Herramientas:
                    DrawToolsTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.Guia:
                    DrawGuideTab(sb, pixel, panelRect, ref contentY);
                    break;
                case Tab.Perfiles:
                    DrawProfilesTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.Personalizar:
                    DrawCustomizeTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.Cheats:
                    DrawCheatsTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
                case Tab.Logs:
                    DrawLogsTab(sb, pixel, panelRect, ref contentY, mouse);
                    break;
            }
        }
        catch (Exception tabException)
        {
            activeField = 0;
            Utils.DrawBorderString(sb, "Error en la pestaña " + currentTab + ": " + tabException.GetType().Name,
                new Vector2(panelRect.X + Pad, contentY), new Color(255, 120, 120), 0.75f);
            Utils.DrawBorderString(sb, tabException.Message ?? string.Empty,
                new Vector2(panelRect.X + Pad, contentY + 22), new Color(255, 190, 150), 0.6f);
            try { Console.Error.WriteLine("[HerosModPanel] " + currentTab + ": " + tabException); } catch { }
        }

        if (statusTimer > 0)
        {
            statusTimer--;
            Utils.DrawBorderString(sb, statusMessage, new Vector2(panelRect.X + Pad, panelRect.Bottom - 24), new Color(150, 255, 150), 0.75f);
        }

        if (activeField != 0)
        {
            HandleTyping();
        }

        // Tooltip al final para que quede encima de todo.
        if (hoverTip != null)
        {
            DrawTooltip(sb, pixel, hoverTip, mouse);
        }
    }

    // ------------------------------------------------------------------ Jugador

    // Desbloquea los permisos de los poderes creativos en mundos Journey/Viaje.
    // Reflection evita acoplar el panel a una firma concreta entre revisiones de Unified.
    private static bool ApplyUnrestrictedAdventurePermissions(bool enable)
    {
        // Busca la API en el mismo ensamblado cargado de Terraria; Type.GetType con nombre de
        // ensamblado fijo falla en algunas compilaciones de Terraria Unified.
        try
        {
            Type managerType = typeof(Player).Assembly.GetType("Terraria.GameContent.Creative.CreativePowerManager")
                ?? Type.GetType("Terraria.GameContent.Creative.CreativePowerManager, Terraria")
                ?? AppDomain.CurrentDomain.GetAssemblies()
                    .Select(a => a.GetType("Terraria.GameContent.Creative.CreativePowerManager", false))
                    .FirstOrDefault(t => t != null);
            if (managerType == null)
                return false;

            object manager = null;
            PropertyInfo instanceProperty = managerType.GetProperty("Instance", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (instanceProperty != null)
                manager = instanceProperty.GetValue(null);
            if (manager == null)
            {
                FieldInfo instanceField = managerType.GetField("Instance", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                if (instanceField != null)
                    manager = instanceField.GetValue(null);
            }
            if (manager == null)
                return false;

            bool changed = false;
            var visited = new HashSet<object>();
            var pending = new Stack<object>();
            pending.Push(manager);

            while (pending.Count > 0)
            {
                object current = pending.Pop();
                if (current == null || current is string || current.GetType().IsPrimitive || current.GetType().IsEnum)
                    continue;
                if (!current.GetType().IsValueType && !visited.Add(current))
                    continue;

                Type type = current.GetType();
                bool isCreativePower = type.Name.IndexOf("CreativePower", StringComparison.OrdinalIgnoreCase) >= 0
                    || type.GetInterfaces().Any(i => i.Name == "ICreativePower");
                if (isCreativePower)
                {
                    Type permissionType = type.GetProperty("CurrentPermissionLevel", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.PropertyType
                        ?? type.GetMethod("SetPermissionLevel", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetParameters().FirstOrDefault()?.ParameterType;
                    if (permissionType != null && permissionType.IsEnum)
                    {
                        string[] names = Enum.GetNames(permissionType);
                        string wanted = enable
                            ? names.FirstOrDefault(n => n.IndexOf("Everyone", StringComparison.OrdinalIgnoreCase) >= 0
                                && n.IndexOf("Locked", StringComparison.OrdinalIgnoreCase) < 0)
                                ?? names.FirstOrDefault(n => n.IndexOf("CanBeChangedByEveryone", StringComparison.OrdinalIgnoreCase) >= 0)
                            : names.FirstOrDefault(n => n.IndexOf("CanBeChangedByHostAlone", StringComparison.OrdinalIgnoreCase) >= 0)
                                ?? names.FirstOrDefault(n => n.IndexOf("Host", StringComparison.OrdinalIgnoreCase) >= 0
                                    && n.IndexOf("Locked", StringComparison.OrdinalIgnoreCase) < 0);
                        if (wanted != null)
                        {
                            object value = Enum.Parse(permissionType, wanted);
                            try
                            {
                                PropertyInfo permissionProperty = type.GetProperty("CurrentPermissionLevel", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                if (permissionProperty?.CanWrite == true)
                                {
                                    permissionProperty.SetValue(current, value);
                                    changed = true;
                                }
                            }
                            catch { }

                            try
                            {
                                MethodInfo setter = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                    .FirstOrDefault(m => m.Name == "SetPermissionLevel" && m.GetParameters().Length == 1
                                        && m.GetParameters()[0].ParameterType == permissionType);
                                if (setter != null)
                                {
                                    setter.Invoke(current, new[] { value });
                                    changed = true;
                                }
                            }
                            catch { }
                        }
                    }
                }

                if (current is IDictionary dictionary)
                {
                    foreach (DictionaryEntry entry in dictionary)
                        if (entry.Value != null) pending.Push(entry.Value);
                }
                else if (current is IEnumerable enumerable && current is not string)
                {
                    foreach (object entry in enumerable)
                        if (entry != null) pending.Push(entry);
                }

                const BindingFlags instanceFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                foreach (FieldInfo field in type.GetFields(instanceFlags))
                {
                    if (field.FieldType.IsPrimitive || field.FieldType.IsEnum || field.FieldType == typeof(string))
                        continue;
                    try
                    {
                        object value = field.GetValue(current);
                        if (value != null && !ReferenceEquals(value, current))
                            pending.Push(value);
                    }
                    catch { }
                }

                foreach (PropertyInfo property in type.GetProperties(instanceFlags))
                {
                    if (!property.CanRead || property.GetIndexParameters().Length != 0
                        || property.PropertyType.IsPrimitive || property.PropertyType.IsEnum
                        || property.PropertyType == typeof(string))
                        continue;
                    try
                    {
                        object value = property.GetValue(current);
                        if (value != null && !ReferenceEquals(value, current))
                            pending.Push(value);
                    }
                    catch { }
                }
            }

            return changed;
        }
        catch
        {
            return false;
        }
    }

    // Intenta activar Journey Mode tanto en el mundo como en el personaje.
    // Terraria puede exigir un mundo/personaje creado en Viaje y el servidor puede rechazar el cambio.
    private static bool TryEnableJourneyMode()
    {
        bool changed = false;
        try
        {
            const BindingFlags staticFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo worldModeField = typeof(Main).GetField("GameMode", staticFlags);
            if (worldModeField != null && worldModeField.FieldType == typeof(int))
            {
                worldModeField.SetValue(null, 3);
                changed = true;
            }
            else
            {
                PropertyInfo worldModeProperty = typeof(Main).GetProperty("GameMode", staticFlags);
                if (worldModeProperty?.CanWrite == true && worldModeProperty.PropertyType == typeof(int))
                {
                    worldModeProperty.SetValue(null, 3);
                    changed = true;
                }
            }
        }
        catch { }

        try
        {
            Player player = Main.LocalPlayer;
            if (player != null && player.active)
            {
                const BindingFlags instanceFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                FieldInfo difficultyField = typeof(Player).GetField("difficulty", instanceFlags);
                if (difficultyField != null)
                {
                    difficultyField.SetValue(player, Convert.ChangeType(3, difficultyField.FieldType, CultureInfo.InvariantCulture));
                    changed = true;
                }
                else
                {
                    PropertyInfo difficultyProperty = typeof(Player).GetProperty("difficulty", instanceFlags);
                    if (difficultyProperty?.CanWrite == true)
                    {
                        difficultyProperty.SetValue(player, Convert.ChangeType(3, difficultyProperty.PropertyType, CultureInfo.InvariantCulture));
                        changed = true;
                    }
                }
            }
        }
        catch { }
        return changed;
    }

    private static void ApplyProjectileMultiplier(Player local)
    {
        if (local == null || !local.active || local.dead || editorProjectileMultiplier <= 1)
            return;

        Item held = local.HeldItem;
        if (held == null || held.IsAir || held.shoot <= 0)
            return;

        // Unified no expone Projectile.identity ni GetSource_FromThis en esta versión.
        // Usamos el índice de la ranura y retiramos entradas cuando la bala desaparece.
        for (int n = multipliedProjectileIdentities.Count - 1; n >= 0; n--)
        {
            int tracked = multipliedProjectileIdentities.ElementAt(n);
            if (tracked < 0 || tracked >= Main.maxProjectiles || Main.projectile[tracked] == null || !Main.projectile[tracked].active)
                multipliedProjectileIdentities.Remove(tracked);
        }

        int totalShots = radialProjectileMode
            ? Math.Max(1, editorProjectileMultiplier)
            : Math.Max(1, editorProjectileMultiplier);
        int copies = Math.Max(0, totalShots - 1);
        for (int i = 0; i < Main.maxProjectiles; i++)
        {
            Projectile source = Main.projectile[i];
            if (source == null || !source.active || source.owner != local.whoAmI
                || source.type != held.shoot || source.minion || source.sentry || source.aiStyle == 7)
                continue;
            if (!multipliedProjectileIdentities.Add(source.whoAmI))
                continue;

            if (radialProjectileMode)
                source.position = new Vector2(local.Center.X - source.width * 0.5f,
                    local.Center.Y - source.height * 0.5f);

            for (int n = 1; n <= copies; n++)
            {
                Vector2 spawnPosition = source.position;
                Vector2 spawnVelocity = source.velocity;

                if (radialProjectileMode)
                {
                    // Coloca los disparos alrededor del jugador y distribuye sus direcciones en círculo.
                    spawnPosition = new Vector2(
                        local.Center.X - source.width * 0.5f,
                        local.Center.Y - source.height * 0.5f);
                    double angle = (Math.PI * 2.0 * n) / totalShots;
                    float cos = (float)Math.Cos(angle);
                    float sin = (float)Math.Sin(angle);
                    spawnVelocity = new Vector2(
                        source.velocity.X * cos - source.velocity.Y * sin,
                        source.velocity.X * sin + source.velocity.Y * cos);
                }

                int spawned = Projectile.NewProjectile(new PanelEntitySource(), spawnPosition, spawnVelocity,
                    source.type, source.damage, source.knockBack, source.owner, source.ai[0], source.ai[1], source.ai[2]);
                if (spawned >= 0 && spawned < Main.maxProjectiles)
                    multipliedProjectileIdentities.Add(Main.projectile[spawned].whoAmI);
            }
        }
    }

    private static object journeyResearchTracker;
    private static MethodInfo journeyResearchSetter;
    private static int journeyResearchNextItem;
    private static int journeyResearchTotal;
    private static int journeyResearchCompleted;
    private static int journeyResearchFrameDelay;
    private static bool journeyResearchRunning;

    // Solo localiza el tracker y prepara el trabajo; no procesa todos los objetos en un clic.
    private static bool StartJourneyResearchUnlock(Player player)
    {
        if (player == null) return false;
        try
        {
            var visited = new HashSet<object>();
            var pending = new Stack<object>();
            pending.Push(player);
            const int maxVisitedObjects = 2048;
            int visitedCount = 0;

            while (pending.Count > 0 && visitedCount < maxVisitedObjects)
            {
                object current = pending.Pop();
                if (current == null || !visited.Add(current)) continue;
                visitedCount++;
                Type type = current.GetType();

                if (type.Name.IndexOf("ItemSacrificesTracker", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    MethodInfo setter = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .FirstOrDefault(m => m.Name == "SetSacrificeCount" && m.GetParameters().Length == 2
                            && m.GetParameters().All(p => p.ParameterType == typeof(int)));
                    if (setter == null) return false;

                    journeyResearchTracker = current;
                    journeyResearchSetter = setter;
                    journeyResearchNextItem = 1;
                    journeyResearchTotal = Math.Max(1, ItemID.Count - 1);
                    journeyResearchCompleted = 0;
                    journeyResearchFrameDelay = 0;
                    journeyResearchRunning = true;
                    SetStatus("Investigación iniciada: 0/" + journeyResearchTotal + " objetos; faltan " + journeyResearchTotal + ".");
                    return true;
                }

                if (type == typeof(string) || type.IsPrimitive || type.IsEnum ||
                    type.Namespace == null || type.Namespace.StartsWith("System", StringComparison.Ordinal))
                    continue;

                foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    Type fieldType = field.FieldType;
                    if (fieldType.IsPrimitive || fieldType.IsEnum || fieldType == typeof(string) ||
                        fieldType.Namespace != null && fieldType.Namespace.StartsWith("System", StringComparison.Ordinal))
                        continue;
                    try
                    {
                        object value = field.GetValue(current);
                        if (value != null && !ReferenceEquals(value, current) && !visited.Contains(value))
                            pending.Push(value);
                    }
                    catch { }
                    if (pending.Count >= maxVisitedObjects) break;
                }
            }
        }
        catch { }
        return false;
    }

    // Procesa pocos IDs por tanda para evitar congelar el juego en un solo frame.
    private static void ProcessJourneyResearchUnlock()
    {
        if (!journeyResearchRunning) return;
        if (journeyResearchTracker == null || journeyResearchSetter == null)
        {
            journeyResearchRunning = false;
            SetStatus("Investigación detenida: no se encontró el registro de objetos.");
            return;
        }

        if (++journeyResearchFrameDelay < 6) return;
        journeyResearchFrameDelay = 0;

        int batchSize = 5;
        int batchEnd = Math.Min(journeyResearchTotal + 1, journeyResearchNextItem + batchSize);
        for (; journeyResearchNextItem < batchEnd; journeyResearchNextItem++)
        {
            try
            {
                journeyResearchSetter.Invoke(journeyResearchTracker, new object[] { journeyResearchNextItem, 999999 });
                journeyResearchCompleted++;
            }
            catch
            {
                // Algunos IDs no admiten investigación; se omiten y se continúa.
            }
        }

        int remaining = Math.Max(0, journeyResearchTotal - journeyResearchNextItem + 1);
        if (journeyResearchNextItem > journeyResearchTotal)
        {
            journeyResearchRunning = false;
            SetStatus("Investigación terminada: " + journeyResearchCompleted + "/" + journeyResearchTotal +
                " objetos procesados; faltan 0 por revisar.");
            journeyResearchTracker = null;
            journeyResearchSetter = null;
        }
        else
        {
            SetStatus("Desbloqueando objetos: " + Math.Min(journeyResearchNextItem - 1, journeyResearchTotal) +
                "/" + journeyResearchTotal + "; faltan " + remaining + " por revisar.");
        }
    }

    private static void DrawPlayerTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, "Modificaciones del Jugador");
        int btnW = (Width - (Pad * 2) - 8) / 2;

        int clientUtilityW = (Width - Pad * 2 - 8) / 2;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, clientUtilityW, RowH), "Cliente: limpiar efectos negativos", mouse, new Color(75, 95, 125)))
        {
            if (IsServer)
            {
                SetStatus("Esta herramienta es solo para el cliente; úsala desde un jugador conectado.");
            }
            else
            {
                Player localPlayer = Main.LocalPlayer;
                int removedDebuffs = 0;
                for (int buffIndex = 0; buffIndex < localPlayer.buffType.Length; buffIndex++)
                {
                    int buffType = localPlayer.buffType[buffIndex];
                    if (buffType > 0 && localPlayer.buffTime[buffIndex] > 0 && Main.debuff[buffType])
                    {
                        localPlayer.DelBuff(buffIndex);
                        removedDebuffs++;
                        buffIndex--;
                    }
                }
                SetStatus($"Limpieza local completada: {removedDebuffs} efectos negativos eliminados. En multijugador el servidor puede restaurarlos.");
            }
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + clientUtilityW + 8, y, clientUtilityW, RowH), "Cliente: restaurar vida/maná", mouse, new Color(45, 105, 85)))
        {
            if (IsServer)
            {
                SetStatus("La restauración local no se puede ejecutar desde el servidor dedicado.");
            }
            else
            {
                Player localPlayer = Main.LocalPlayer;
                localPlayer.statLife = localPlayer.statLifeMax2;
                localPlayer.statMana = localPlayer.statManaMax2;
                SyncSelf();
                SetStatus("Vida y maná restaurados localmente; en multijugador el servidor decide el estado final.");
            }
        }
        y += RowH + 6;

        string godText = godmodeEnabled ? "God Mode: ACTIVADO" : "God Mode: desactivado";
        Color godColor = godmodeEnabled ? new Color(45, 130, 80) : UiTabColor;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, btnW, RowH), godText, mouse, godColor))
        {
            godmodeEnabled = !godmodeEnabled;
            var p = Main.LocalPlayer;
            p.creativeGodMode = godmodeEnabled;
            if (godmodeEnabled)
            {
                p.statLife = Math.Max(1, p.statLifeMax2);
                p.statMana = Math.Max(0, p.statManaMax2);
                p.immune = true;
                p.immuneTime = Math.Max(p.immuneTime, 2);
            }
            else
            {
                p.creativeGodMode = false;
                p.immune = false;
                p.immuneTime = 0;
            }
            SyncSelf();
            SetStatus(godmodeEnabled
                ? "God Mode activado. En multijugador, el servidor puede limitar la invulnerabilidad."
                : "God Mode desactivado.");
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + btnW + 8, y, btnW, RowH), "DESACTIVAR GOD MODE", mouse, new Color(135, 55, 60)))
        {
            godmodeEnabled = false;
            Player p = Main.LocalPlayer;
            p.creativeGodMode = false;
            p.immune = false;
            p.immuneTime = 0;
            SyncSelf();
            SetStatus("God Mode desactivado manualmente.");
        }
        y += RowH + 6;

        string flyText = flyEnabled ? "Vuelo Libre: [ON]" : "Vuelo Libre: [OFF]";
        Color flyColor = flyEnabled ? new Color(40, 100, 160) : new Color(50, 55, 70);
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, btnW, RowH), flyText, mouse, flyColor))
        {
            if (IsServer)
            {
                NotInServer("Vuelo");
            }
            else
            {
                flyEnabled = !flyEnabled;
                SetStatus(flyEnabled ? "Vuelo: WASD + Espacio (subir) + Shift (bajar)." : "Vuelo desactivado.");
            }
        }
        int extraToggleW = (Width - Pad * 2 - 8) / 2;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + extraToggleW + 8, y, extraToggleW, RowH), infiniteMinionsEnabled ? "Minions infinitos: ACTIVADO" : "Minions infinitos: desactivado", mouse, infiniteMinionsEnabled ? new Color(40, 115, 75) : UiTabColor))
        {
            infiniteMinionsEnabled = !infiniteMinionsEnabled;
            if (infiniteMinionsEnabled) Main.LocalPlayer.maxMinions = 9999;
            SetStatus(infiniteMinionsEnabled ? "Límite local de minions elevado; el servidor puede rechazarlo." : "Minions infinitos desactivados.");
        }
        y += RowH + 6;

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, btnW, RowH), "Reiniciar registro de proyectiles", mouse))
        {
            multipliedProjectileIdentities.Clear();
            SetStatus("Registro de proyectiles reiniciado.");
        }
        string ghostText = ghostModeEnabled ? "Modo Fantasma: ACTIVADO" : "Modo Fantasma: desactivado";
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + btnW + 8, y, btnW, RowH), ghostText, mouse, ghostModeEnabled ? new Color(85, 95, 145) : UiTabColor))
        {
            ghostModeEnabled = !ghostModeEnabled;
            if (Main.LocalPlayer.active)
            {
                // Cambia el estado ghost real de Terraria solo al pulsar el botón; no lo fuerza cada tick.
                Main.LocalPlayer.ghost = ghostModeEnabled;
            }
            SetStatus(ghostModeEnabled
                ? "Estado Ghost de Terraria activado localmente. En multijugador el servidor puede rechazarlo o sobrescribirlo."
                : "Estado Ghost desactivado localmente.");
        }
        y += RowH + 6;

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, Width - Pad * 2, RowH), "Activar Journey Mode + desbloquear objetos poco a poco", mouse, new Color(40, 115, 75)))
        {
            bool applied = TryEnableJourneyMode();
            bool started = journeyResearchRunning || StartJourneyResearchUnlock(Main.LocalPlayer);
            SetStatus(started
                ? "Journey Mode: " + (applied ? "cambio intentado. " : "cambio no confirmado. ") +
                    "Investigación progresiva: " + Math.Min(journeyResearchNextItem - 1, journeyResearchTotal) +
                    "/" + journeyResearchTotal + "; faltan " + Math.Max(0, journeyResearchTotal - journeyResearchNextItem + 1) + "."
                : "No se encontró el registro de investigación. Terraria puede requerir personaje y mundo creados en Modo Viaje.");
        }
        y += RowH + 6;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, Width - Pad * 2, RowH), "Desbloquear investigación de TODOS los objetos (progresivo)", mouse, new Color(75, 85, 135)))
        {
            bool started = journeyResearchRunning || StartJourneyResearchUnlock(Main.LocalPlayer);
            SetStatus(started
                ? "Investigación progresiva: " + Math.Min(journeyResearchNextItem - 1, journeyResearchTotal) +
                    "/" + journeyResearchTotal + "; faltan " + Math.Max(0, journeyResearchTotal - journeyResearchNextItem + 1) + " por revisar."
                : "No se encontró el registro de investigación de Viaje en esta versión.");
        }
        y += RowH + 6;

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, btnW, RowH), "Curar Full Vida/ManÃ¡", mouse))
        {
            if (IsServer)
            {
                NotInServer("Curar");
            }
            else
            {
                var p = Main.LocalPlayer;
                p.statLife = p.statLifeMax2;
                p.statMana = p.statManaMax2;
                SyncSelf();
                SetStatus("Â¡Salud y manÃ¡ restaurados al mÃ¡ximo!");
            }
        }

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + btnW + 8, y, btnW, RowH), "Limpiar Debuffs", mouse))
        {
            if (IsServer)
            {
                NotInServer("Limpiar debuffs");
            }
            else
            {
                var p = Main.LocalPlayer;
                for (int i = 0; i < p.buffType.Length; i++)
                {
                    if (p.buffType[i] > 0 && Main.debuff[p.buffType[i]])
                    {
                        p.DelBuff(i);
                    }
                }
                SyncSelf();
                SetStatus("Efectos negativos eliminados.");
            }
        }
        y += RowH + 6;

        string spawnText = noSpawnsEnabled ? "Spawn Enemigos: [OFF]" : "Spawn Enemigos: [ON]";
        Color spawnColor = noSpawnsEnabled ? new Color(140, 40, 40) : new Color(50, 55, 70);
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, btnW, RowH), spawnText, mouse, spawnColor))
        {
            if (InMultiplayer)
            {
                SetStatus("El spawn de enemigos lo controla el servidor (config de TShock).");
            }
            else
            {
                SetStatus("Esta versiÃ³n de Unified no expone la configuraciÃ³n de spawn; no se modificÃ³ el mundo.");
            }
        }
        y += RowH + 14;

        DrawSectionHeader(sb, panel, ref y, "Buffs RÃ¡pidos (10 minutos)");
        (int id, string name)[] buffs =
        {
            (BuffID.Regeneration, "RegeneraciÃ³n"),
            (BuffID.Swiftness, "Velocidad"),
            (BuffID.Ironskin, "Piel de Hierro"),
            (BuffID.ManaRegeneration, "ManÃ¡ Regen"),
            (BuffID.Spelunker, "Spelunker"),
            (BuffID.Shine, "Resplandor"),
        };
        int bw = (Width - (Pad * 2) - 16) / 3;
        for (int i = 0; i < buffs.Length; i++)
        {
            int col = i % 3;
            int row = i / 3;
            var rect = new Rectangle(panel.X + Pad + col * (bw + 8), y + row * (RowH + 6), bw, RowH);
            if (DrawButton(sb, pixel, rect, buffs[i].name, mouse, new Color(60, 90, 70)))
            {
                if (IsServer)
                {
                    SendCmd($"/buff {buffs[i].id} 600");
                }
                else
                {
                    Main.LocalPlayer.AddBuff(buffs[i].id, 36000);
                    SyncSelf();
                    SetStatus($"Buff aplicado: {buffs[i].name} (10 min)");
                }
            }
        }
        y += 2 * (RowH + 6) + 8;
    }

    // ------------------------------------------------------------------ Mundo

    private static void DrawWorldTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, "Control de Tiempo y Clima");
        int btnW = (Width - (Pad * 2) - 8) / 2;

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, btnW, RowH), "Establecer: DÃ­a", mouse))
        {
            if (IsServer)
            {
                SendCmd("/time day");
            }
            else if (InMultiplayer)
            {
                NoRemoteWorld("Hora");
            }
            else
            {
                Main.dayTime = true;
                Main.time = 0;
                SetStatus("Hora cambiada al DÃ­a.");
            }
        }

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + btnW + 8, y, btnW, RowH), "Establecer: Noche", mouse))
        {
            if (IsServer)
            {
                SendCmd("/time night");
            }
            else if (InMultiplayer)
            {
                NoRemoteWorld("Hora");
            }
            else
            {
                Main.dayTime = false;
                Main.time = 0;
                SetStatus("Hora cambiada a la Noche.");
            }
        }
        y += RowH + 6;

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, btnW, RowH), "Establecer: MediodÃ­a", mouse))
        {
            if (IsServer)
            {
                SendCmd("/time noon");
            }
            else if (InMultiplayer)
            {
                NoRemoteWorld("Hora");
            }
            else
            {
                Main.dayTime = true;
                Main.time = 27000.0;
                SetStatus("Hora cambiada al MediodÃ­a.");
            }
        }

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + btnW + 8, y, btnW, RowH), "Establecer: Medianoche", mouse))
        {
            if (IsServer)
            {
                SendCmd("/time midnight");
            }
            else if (InMultiplayer)
            {
                NoRemoteWorld("Hora");
            }
            else
            {
                Main.dayTime = false;
                Main.time = 16200.0;
                SetStatus("Hora cambiada a la Medianoche.");
            }
        }
        y += RowH + 6;

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, btnW, RowH), "Alternar Lluvia", mouse))
        {
            if (IsServer)
            {
                SendCmd(Main.raining ? "/rain stop" : "/rain start");
            }
            else if (InMultiplayer)
            {
                NoRemoteWorld("Clima");
            }
            else if (Main.raining)
            {
                Main.StopRain();
                SetStatus("Lluvia detenida.");
            }
            else
            {
                Main.StartRain();
                SetStatus("Lluvia iniciada.");
            }
        }

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + btnW + 8, y, btnW, RowH), "Matar Enemigos (Butcher)", mouse))
        {
            if (IsServer)
            {
                SendCmd("/butcher");
            }
            else
            {
                int count = 0;
                foreach (var npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && !npc.townNPC && npc.lifeMax > 5)
                    {
                        if (!InMultiplayer)
                        {
                            npc.life = 0;
                            npc.active = false;
                            count++;
                        }
                    }
                }
                SetStatus($"Se eliminaron {count} enemigos activos.");
            }
        }
        y += RowH + 12;
    }

    // ------------------------------------------------------------------ Items

    private static void DrawItemsTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, "Generador de Objetos (Spawn de Ãtems)");

        EnsureItemNames();
        RefreshBrowser();

        Utils.DrawBorderString(sb, "Nombre o ID del Objeto:", new Vector2(panel.X + Pad, y), Color.White, 0.75f);
        Utils.DrawBorderString(sb, $"{browserIds.Count} resultados", new Vector2(panel.Right - Pad - 110, y), Color.Gray, 0.7f);
        y += 20;

        var queryRect = new Rectangle(panel.X + Pad, y, Width - (Pad * 2), RowH);
        DrawTextField(sb, pixel, queryRect, 1, itemQuery, mouse, "Escribe para buscar (nombre o ID)...");
        y += RowH + 10;

        Utils.DrawBorderString(sb, "Cantidad:", new Vector2(panel.X + Pad, y), Color.White, 0.75f);
        Utils.DrawBorderString(sb, "Prefijo (ID, -1=aleatorio, 0=ninguno):", new Vector2(panel.X + Pad + 110, y), Color.White, 0.75f);
        y += 20;

        var amountRect = new Rectangle(panel.X + Pad, y, 100, RowH);
        DrawTextField(sb, pixel, amountRect, 2, itemAmount, mouse);

        var prefixRect = new Rectangle(panel.X + Pad + 110, y, 100, RowH);
        DrawTextField(sb, pixel, prefixRect, 3, itemPrefix, mouse);
        y += RowH + 10;

        var giveBtnRect = new Rectangle(panel.X + Pad, y, Width - (Pad * 2), RowH);
        if (DrawButton(sb, pixel, giveBtnRect, "Obtener Objeto (primer resultado / nombre escrito)", mouse, new Color(40, 110, 160)))
        {
            SpawnItemAction();
        }
        y += RowH + 12;

        // Navegador con iconos: clic en un item para obtenerlo.
        int browserH = Math.Max(CellSize * 2, panel.Bottom - 30 - y);
        var browserRect = new Rectangle(panel.X + Pad, y, Width - (Pad * 2), browserH);
        DrawItemBrowser(sb, pixel, browserRect, mouse);
    }

    private static void EnsureItemNames()
    {
        if (itemNames != null)
            return;

        itemNames = new string[ItemID.Count];
        allItemIds = new List<int>();
        for (int i = 1; i < ItemID.Count; i++)
        {
            string name = Lang.GetItemNameValue(i);
            itemNames[i] = name ?? "";
            if (!string.IsNullOrWhiteSpace(itemNames[i]))
                allItemIds.Add(i);
        }
    }

    private static void RefreshBrowser()
    {
        string key = (itemQuery ?? "").Trim();
        if (key == browserFilterKey)
            return;

        browserFilterKey = key;
        browserScroll = 0f;
        browserIds.Clear();

        bool isNumber = int.TryParse(key, out int number);
        foreach (int id in allItemIds)
        {
            if (key.Length == 0
                || (isNumber && id == number)
                || itemNames[id].Contains(key, StringComparison.OrdinalIgnoreCase))
            {
                browserIds.Add(id);
            }
        }
    }

    private static void DrawItemBrowser(SpriteBatch sb, Texture2D pixel, Rectangle area, Point mouse)
    {
        sb.Draw(pixel, area, new Color(20, 22, 28));

        int cols = Math.Max(1, (area.Width - BarW - 4) / CellSize);
        int rows = (browserIds.Count + cols - 1) / cols;
        float maxScroll = Math.Max(0f, rows * CellSize + 4 - area.Height);
        bool hoverArea = area.Contains(mouse);

        if (hoverArea)
        {
            int wheel = PlayerInput.ScrollWheelDeltaForUI;
            if (wheel != 0)
            {
                browserScroll -= wheel / 120f * CellSize;
                PlayerInput.ScrollWheelDeltaForUI = 0;
            }
        }

        // Barra de scroll (arrastrable).
        var track = new Rectangle(area.Right - BarW, area.Y, BarW, area.Height);
        int thumbH = maxScroll > 0f ? Math.Max(24, (int)(area.Height * (area.Height / (area.Height + maxScroll)))) : area.Height;

        if (browserDragging)
        {
            if (Main.mouseLeft)
            {
                float trackLen = area.Height - thumbH;
                if (trackLen > 0f)
                    browserScroll = browserDragStartScroll + (Main.mouseY - browserDragStartY) * maxScroll / trackLen;
            }
            else
            {
                browserDragging = false;
            }
        }
        else if (Clicked && maxScroll > 0f && track.Contains(mouse))
        {
            browserDragging = true;
            browserDragStartY = Main.mouseY;
            browserDragStartScroll = browserScroll;
            Main.mouseLeftRelease = false;
        }

        browserScroll = Math.Clamp(browserScroll, 0f, maxScroll);

        int firstRow = (int)(browserScroll / CellSize);
        int lastRow = (int)((browserScroll + area.Height) / CellSize);

        for (int row = firstRow; row <= lastRow; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int idx = row * cols + col;
                if (idx >= browserIds.Count)
                    break;

                var cell = new Rectangle(area.X + 2 + col * CellSize, area.Y + 2 + (int)(row * CellSize - browserScroll), CellSize - 2, CellSize - 2);

                // Solo se dibujan celdas completas dentro del area (sin recorte).
                if (cell.Y < area.Y || cell.Bottom > area.Bottom)
                    continue;

                int id = browserIds[idx];
                bool hover = hoverArea && !browserDragging && mouse.X < area.Right - BarW && cell.Contains(mouse);

                sb.Draw(pixel, cell, hover ? new Color(75, 85, 110) : new Color(50, 55, 68));
                DrawItemIcon(sb, id, cell);

                if (hover)
                {
                    hoverTip = $"{itemNames[id]} (ID {id})";
                    if (Clicked)
                    {
                        Main.mouseLeftRelease = false;
                        GiveItemById(id);
                    }
                }
            }
        }

        if (maxScroll > 0f)
        {
            sb.Draw(pixel, track, Color.Black * 0.4f);
            int thumbY = area.Y + (int)((area.Height - thumbH) * (browserScroll / maxScroll));
            sb.Draw(pixel, new Rectangle(track.X + 2, thumbY, BarW - 4, thumbH), browserDragging ? new Color(110, 130, 190) : new Color(75, 85, 110));
        }

        if (browserIds.Count == 0)
        {
            Utils.DrawBorderString(sb, "Sin resultados", new Vector2(area.X + 10, area.Y + 10), Color.Gray, 0.8f);
        }
    }

    private static void DrawItemIcon(SpriteBatch sb, int id, Rectangle cell)
    {
        Main.instance.LoadItem(id);
        var tex = TextureAssets.Item[id].Value;
        if (tex == null)
            return;

        var anim = Main.itemAnimations[id];
        var frame = anim != null ? anim.GetFrame(tex) : new Rectangle(0, 0, tex.Width, tex.Height);

        float scale = Math.Min(1f, 30f / Math.Max(frame.Width, frame.Height));
        var center = new Vector2(cell.X + cell.Width / 2f, cell.Y + cell.Height / 2f);
        var origin = new Vector2(frame.Width / 2f, frame.Height / 2f);

        sb.Draw(tex, center, frame, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
    }

    private static void DrawTooltip(SpriteBatch sb, Texture2D pixel, string text, Point mouse)
    {
        var font = FontAssets.MouseText.Value;
        float scale = 0.8f;
        var size = font.MeasureString(text) * scale;
        int w = (int)size.X + 12;
        int h = (int)size.Y + 8;
        int x = Math.Min(mouse.X + 16, Main.screenWidth - w - 4);
        int ty = Math.Min(mouse.Y + 16, Main.screenHeight - h - 4);

        sb.Draw(pixel, new Rectangle(x, ty, w, h), new Color(10, 12, 20) * 0.95f);
        Utils.DrawBorderString(sb, text, new Vector2(x + 6, ty + 4), Color.White, scale);
    }

    // ------------------------------------------------------------------ Jefes

    private static void DrawBossesTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, "Invocador de jefes — sin cooldown ni límite de uso");
        int gap = 5;
        int columns = 4;
        int bossButtonHeight = Math.Max(22, RowH - 5);
        int bossRowGap = 3;
        int btnW = (Width - Pad * 2 - gap * (columns - 1)) / columns;
        int row = 0;

        // Invocación directa desde la interfaz: no necesita comandos /spawnboss ni plugins.
        // Se conserva el mismo método de invocación para permitir repetir los clics sin cooldown.
        (int id, string name)[] bosses =
        {
            (50, "Rey Slime"),
            (4, "Ojo de Cthulhu"),
            (13, "Devoramundos"),
            (266, "Cerebro de Cthulhu"),
            (35, "Skeletron"),
            (222, "Abeja Reina"),
            (113, "Muro de Carne"),
            (657, "Reina Slime"),
            (125, "Los Gemelos"),
            (134, "Destructor"),
            (127, "Skeletron Prime"),
            (262, "Plantera"),
            (245, "Golem"),
            (370, "Duque Fishron"),
            (439, "Cultista Lunático"),
            (551, "Betsy"),
            (636, "Emperatriz de la Luz"),
            (668, "Ciervíclope"),
            (398, "Señor de la Luna")
        };

        foreach (var boss in bosses)
        {
            int col = row % columns;
            int line = row / columns;
            var rect = new Rectangle(panel.X + Pad + col * (btnW + gap), y + line * (bossButtonHeight + bossRowGap), btnW, bossButtonHeight);
            if (DrawButton(sb, pixel, rect, boss.name, mouse))
                SpawnBoss(boss.id, boss.name, "");
            row++;
        }
        int bossRows = (row + columns - 1) / columns;
        y += bossRows * (bossButtonHeight + bossRowGap) + 7;

        int activeBosses = 0;
        foreach (NPC npc in Main.npc)
        {
            if (npc != null && npc.active && npc.boss)
                activeBosses++;
        }
        Utils.DrawBorderString(sb, $"Jefes activos: {activeBosses}  ·  Botones directos en un jugador; multijugador depende del servidor.",
            new Vector2(panel.X + Pad, y), activeBosses > 0 ? new Color(255, 190, 105) : Color.LightGray, 0.62f);
        y += 20;

        DrawSectionHeader(sb, panel, ref y, "Progreso de jefes del mundo");
        DrawBossState(sb, panel, ref y, "Rey Slime", NPC.downedSlimeKing);
        DrawBossState(sb, panel, ref y, "Ojo de Cthulhu", NPC.downedBoss1);
        DrawBossState(sb, panel, ref y, "Devoramundos / Cerebro", NPC.downedBoss2);
        DrawBossState(sb, panel, ref y, "Skeletron", NPC.downedBoss3);
        DrawBossState(sb, panel, ref y, "Abeja Reina", NPC.downedQueenBee);
        DrawBossState(sb, panel, ref y, "Jefes mecánicos", NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3);
        DrawBossState(sb, panel, ref y, "Plantera", NPC.downedPlantBoss);
        DrawBossState(sb, panel, ref y, "Golem", NPC.downedGolemBoss);
        DrawBossState(sb, panel, ref y, "Señor de la Luna", NPC.downedMoonlord);
    }

    private static void DrawBossState(SpriteBatch sb, Rectangle panel, ref int y, string name, bool defeated)
    {
        if (y > panel.Bottom - 36)
            return;
        Color stateColor = defeated ? new Color(115, 220, 130) : new Color(230, 175, 100);
        string state = defeated ? "Derrotado" : "Pendiente";
        Utils.DrawBorderString(sb, name, new Vector2(panel.X + Pad, y), Color.White, 0.65f);
        Utils.DrawBorderString(sb, state, new Vector2(panel.X + Width - 112, y), stateColor, 0.65f);
        y += 18;
    }

    // Mini mapa del panel: muestrea tiles alrededor del jugador y permite elegir coordenadas.
    private static void DrawWorldMiniMap(SpriteBatch sb, Texture2D pixel, Rectangle area, Point mouse, bool worldEdit)
    {
        if (area.Width <= 0 || area.Height <= 0) return;
        sb.Draw(pixel, area, new Color(8, 12, 22));
        // Celdas adaptables al alto disponible para mejorar la legibilidad del minimapa.
        int cell = area.Height >= 250 ? 12 : area.Height >= 150 ? 10 : 8, span = 2;
        int cols = Math.Max(1, area.Width / cell), rows = Math.Max(1, area.Height / cell);
        int cx = (int)(Main.LocalPlayer.Center.X / 16f), cy = (int)(Main.LocalPlayer.Center.Y / 16f);
        int left = cx - cols * span / 2, top = cy - rows * span / 2;
        for (int gy = 0; gy < rows; gy++)
            for (int gx = 0; gx < cols; gx++)
            {
                int tx = Math.Clamp(left + gx * span, 1, Main.maxTilesX - 2);
                int ty = Math.Clamp(top + gy * span, 1, Main.maxTilesY - 2);
                Tile t = Main.tile[tx, ty];
                Color c = new Color(35, 40, 48);
                if (t != null && t.HasTile)
                    c = t.TileType switch
                    {
                        TileID.Dirt or TileID.Grass or TileID.CorruptGrass or TileID.CrimsonGrass or TileID.JungleGrass => new Color(83, 145, 70),
                        TileID.Stone or TileID.Ebonstone or TileID.Crimstone => new Color(105, 105, 115),
                        TileID.Sand or TileID.HardenedSand => new Color(205, 184, 118),
                        TileID.SnowBlock or TileID.IceBlock => new Color(170, 210, 225),
                        TileID.Ash or TileID.Hellstone => new Color(190, 80, 55),
                        _ => new Color(120, 125, 135)
                    };
                sb.Draw(pixel, new Rectangle(area.X + gx * cell, area.Y + gy * cell, cell - 1, cell - 1), c);
            }
        sb.Draw(pixel, new Rectangle(area.X + cols / 2 * cell - 2, area.Y + rows / 2 * cell - 2, 5, 5), Color.Cyan);
        if (worldEdit)
        {
            if (worldPos1.HasValue && worldPos2.HasValue)
                DrawMiniMapSelection(sb, pixel, area, left, top, worldPos1.Value, worldPos2.Value, span, cell);
            if (worldPos1.HasValue) DrawMapMarker(sb, pixel, area, left, top, worldPos1.Value, span, cell, Color.LimeGreen);
            if (worldPos2.HasValue) DrawMapMarker(sb, pixel, area, left, top, worldPos2.Value, span, cell, Color.Orange);
        }
        if (!area.Contains(mouse) || !Clicked) return;
        int col = Math.Clamp((mouse.X - area.X) / cell, 0, cols - 1);
        int row = Math.Clamp((mouse.Y - area.Y) / cell, 0, rows - 1);
        int wx = Math.Clamp(left + col * span, 1, Main.maxTilesX - 2);
        int wy = Math.Clamp(top + row * span, 1, Main.maxTilesY - 2);
        Main.mouseLeftRelease = false;
        if (worldEdit)
        {
            if (worldEditSetPos1Next) { worldPos1 = new Point(wx, wy); worldEditSetPos1Next = false; SetStatus($"POS1: {wx}, {wy}"); }
            else { worldPos2 = new Point(wx, wy); worldEditSetPos1Next = true; SetStatus($"POS2: {wx}, {wy}"); }
        }
        else
        {
            // En la pestaña Teleport, un clic en el minimapa ejecuta el TP directamente.
            // Se mantienen las coordenadas y el botón manual para no quitar la función anterior.
            teleportX = wx.ToString();
            teleportY = wy.ToString();
            TeleportToCoordinates();
        }
    }

    private static void DrawMiniMapSelection(SpriteBatch sb, Texture2D pixel, Rectangle area, int left, int top,
        Point p1, Point p2, int span, int cell)
    {
        int minX = Math.Min(p1.X, p2.X), maxX = Math.Max(p1.X, p2.X);
        int minY = Math.Min(p1.Y, p2.Y), maxY = Math.Max(p1.Y, p2.Y);
        int x1 = area.X + (minX - left) / span * cell;
        int y1 = area.Y + (minY - top) / span * cell;
        int x2 = area.X + (maxX - left) / span * cell + cell - 1;
        int y2 = area.Y + (maxY - top) / span * cell + cell - 1;
        int thickness = 2;
        Color outline = new Color(90, 245, 150, 230);
        if (x2 < area.X || x1 >= area.Right || y2 < area.Y || y1 >= area.Bottom) return;
        x1 = Math.Clamp(x1, area.X, area.Right - 1);
        x2 = Math.Clamp(x2, area.X, area.Right - 1);
        y1 = Math.Clamp(y1, area.Y, area.Bottom - 1);
        y2 = Math.Clamp(y2, area.Y, area.Bottom - 1);
        sb.Draw(pixel, new Rectangle(x1, y1, Math.Max(1, x2 - x1 + 1), thickness), outline);
        sb.Draw(pixel, new Rectangle(x1, Math.Max(area.Y, y2 - thickness + 1), Math.Max(1, x2 - x1 + 1), thickness), outline);
        sb.Draw(pixel, new Rectangle(x1, y1, thickness, Math.Max(1, y2 - y1 + 1)), outline);
        sb.Draw(pixel, new Rectangle(Math.Max(area.X, x2 - thickness + 1), y1, thickness, Math.Max(1, y2 - y1 + 1)), outline);
    }

    // Dibuja la selección en coordenadas de mundo incluso cuando el panel está cerrado.
    private static void DrawWorldSelectionOverlay(SpriteBatch sb, Texture2D pixel)
    {
        if (!worldPos1.HasValue || !worldPos2.HasValue || Main.gameMenu || Main.dedServ)
            return;

        int x1 = Math.Clamp(Math.Min(worldPos1.Value.X, worldPos2.Value.X), 1, Main.maxTilesX - 2);
        int x2 = Math.Clamp(Math.Max(worldPos1.Value.X, worldPos2.Value.X) + 1, 1, Main.maxTilesX - 1);
        int y1 = Math.Clamp(Math.Min(worldPos1.Value.Y, worldPos2.Value.Y), 1, Main.maxTilesY - 2);
        int y2 = Math.Clamp(Math.Max(worldPos1.Value.Y, worldPos2.Value.Y) + 1, 1, Main.maxTilesY - 1);

        Matrix transform = Main.GameViewMatrix.TransformationMatrix;
        Vector2 a = Vector2.Transform(new Vector2(x1 * 16f, y1 * 16f), transform);
        Vector2 b = Vector2.Transform(new Vector2(x2 * 16f, y2 * 16f), transform);
        int left = (int)MathF.Floor(Math.Min(a.X, b.X));
        int top = (int)MathF.Floor(Math.Min(a.Y, b.Y));
        int right = (int)MathF.Ceiling(Math.Max(a.X, b.X));
        int bottom = (int)MathF.Ceiling(Math.Max(a.Y, b.Y));
        if (right <= 0 || bottom <= 0 || left >= Main.screenWidth || top >= Main.screenHeight)
            return;

        left = Math.Clamp(left, 0, Main.screenWidth - 1);
        right = Math.Clamp(right, left + 1, Main.screenWidth);
        top = Math.Clamp(top, 0, Main.screenHeight - 1);
        bottom = Math.Clamp(bottom, top + 1, Main.screenHeight);
        int width = right - left, height = bottom - top, thickness = 2;
        Color fill = new Color(35, 190, 95, 28);
        Color edge = new Color(80, 255, 145, 220);
        sb.Draw(pixel, new Rectangle(left, top, width, height), fill);
        sb.Draw(pixel, new Rectangle(left, top, width, Math.Min(thickness, height)), edge);
        sb.Draw(pixel, new Rectangle(left, Math.Max(top, bottom - thickness), width, Math.Min(thickness, height)), edge);
        sb.Draw(pixel, new Rectangle(left, top, Math.Min(thickness, width), height), edge);
        sb.Draw(pixel, new Rectangle(Math.Max(left, right - thickness), top, Math.Min(thickness, width), height), edge);
    }

    private static void DrawMapMarker(SpriteBatch sb, Texture2D pixel, Rectangle area, int left, int top, Point p, int span, int cell, Color color)
    {
        int x = area.X + (p.X - left) / span * cell, y = area.Y + (p.Y - top) / span * cell;
        if (x >= area.X && x < area.Right && y >= area.Y && y < area.Bottom)
            sb.Draw(pixel, new Rectangle(x, y, cell, cell), color);
    }

    private static void TeleportToCoordinates()
    {
        if (!int.TryParse(teleportX, out int x) || !int.TryParse(teleportY, out int y)
            || x < 1 || x >= Main.maxTilesX - 1 || y < 1 || y >= Main.maxTilesY - 1)
        { SetStatus("Coordenadas inválidas."); return; }
        // TP local directo desde la interfaz; no requiere /tppos ni comandos de chat.
        TeleportPlayer(x * 16, y * 16, $"Coordenadas {x}, {y}");
    }

    // ------------------------------------------------------------------ Teleport

    private static void DrawTeleportTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, "Puntos de Teletransporte");
        int btnW = (Width - (Pad * 2) - 8) / 2;

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, btnW, RowH), "Spawn del Mundo", mouse))
            TeleportPlayer(Main.spawnTileX * 16, Main.spawnTileY * 16, "Spawn");

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + btnW + 8, y, btnW, RowH), "Inframundo (Hell)", mouse))
            TeleportPlayer(Main.maxTilesX / 2 * 16, (Main.maxTilesY - 120) * 16, "Inframundo");
        y += RowH + 6;

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, btnW, RowH), "Mazmorra (Dungeon)", mouse))
            TeleportPlayer(Main.dungeonX * 16, Main.dungeonY * 16, "Mazmorra");
        y += RowH + 8;
        Utils.DrawBorderString(sb, "Destino X (tiles):", new Vector2(panel.X + Pad, y), Color.White, 0.7f);
        Utils.DrawBorderString(sb, "Destino Y (tiles):", new Vector2(panel.X + 230, y), Color.White, 0.7f);
        y += 17;
        if (string.IsNullOrEmpty(teleportX)) teleportX = ((int)(Main.LocalPlayer.Center.X / 16f)).ToString();
        if (string.IsNullOrEmpty(teleportY)) teleportY = ((int)(Main.LocalPlayer.Center.Y / 16f)).ToString();
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 150, 24), 26, teleportX, mouse, "X");
        DrawTextField(sb, pixel, new Rectangle(panel.X + 230, y, 150, 24), 27, teleportY, mouse, "Y");
        y += 29;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, Width - Pad * 2, RowH), "Teletransportar a X/Y", mouse, new Color(45, 100, 150)))
            TeleportToCoordinates();
        y += RowH + 6;
        Utils.DrawBorderString(sb, "Mini mapa · clic para elegir destino", new Vector2(panel.X + Pad, y), new Color(145, 200, 240), 0.68f);
        y += 18;
        DrawWorldMiniMap(sb, pixel, new Rectangle(panel.X + Pad, y, Width - Pad * 2, Math.Max(50, panel.Bottom - y - 35)), mouse, false);
    }

    // ------------------------------------------------------------------ Jugadores

    private static Rectangle Slot(int startX, int rowY, int w, int gap, int k)
    {
        return new Rectangle(startX + k * (w + gap), rowY, w, RowH);
    }

    private static void DrawPlayersTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, "Jugadores Conectados (ModeraciÃ³n)");

        int nameW = 120;
        int gap = 4;
        int actionW = (Width - (Pad * 2) - nameW - gap * 5) / 6;
        int startX = panel.X + Pad + nameW;
        int shown = 0;

        for (int i = 0; i < Main.player.Length; i++)
        {
            var target = Main.player[i];
            if (target == null || !target.active)
                continue;

            // Cada jugador ocupa dos líneas para que los botones no se salgan del panel.
            if (y > panel.Bottom - (RowH * 2) - 48)
                break;

            bool isSelf = i == Main.myPlayer;
            string label = isSelf ? $"{target.name} (TÃº)" : target.name;
            Utils.DrawBorderString(sb, label, new Vector2(panel.X + Pad, y + 5), Color.White, 0.7f);

            if (!isSelf)
            {
                if (DrawButton(sb, pixel, Slot(startX, y, actionW, gap, 0), "Ir", mouse, new Color(40, 100, 140)))
                {
                    if (IsServer)
                        SendCmd($"/tp {Quote(target.name)}");
                    else
                        TeleportToPlayer(target);
                }

                if (DrawButton(sb, pixel, Slot(startX, y, actionW, gap, 1), "Traer", mouse, new Color(40, 100, 140)))
                {
                    if (IsServer)
                        SendCmd($"/tphere {Quote(target.name)}");
                    else if (InMultiplayer)
                        NeedsCommands("Traer");
                    else
                        TeleportToMe(target);
                }

                if (DrawButton(sb, pixel, Slot(startX, y, actionW, gap, 2), "Sanar", mouse, new Color(40, 140, 70)))
                {
                    if (InMultiplayer)
                        NeedsCommands("Sanar a otro jugador");
                    else
                        HealPlayer(target);
                }

                if (DrawButton(sb, pixel, Slot(startX, y, actionW, gap, 3), "Matar", mouse, new Color(150, 40, 40)))
                {
                    if (IsServer)
                        SendCmd($"/kill {Quote(target.name)}");
                    else if (InMultiplayer)
                        NeedsCommands("Matar");
                    else
                        KillPlayerAction(target);
                }

                if (DrawButton(sb, pixel, Slot(startX, y, actionW, gap, 4), "Kick", mouse, new Color(160, 90, 30)))
                {
                    if (IsServer)
                        SendCmd($"/kick {Quote(target.name)}");
                    else
                        NeedsCommands("Kick");
                }

                if (DrawButton(sb, pixel, Slot(startX, y, actionW, gap, 5), "Mute", mouse, new Color(110, 70, 140)))
                {
                    if (IsServer)
                        SendCmd($"/mute {Quote(target.name)}");
                    else
                        NeedsCommands("Mute");
                }
            }

            y += RowH + 3;
            if (!isSelf)
            {
                int giveButtonW = (Width - Pad * 2 - 6) / 2;
                var giveRect = new Rectangle(panel.X + Pad, y, giveButtonW, RowH);
                var spamRect = new Rectangle(panel.X + Pad + giveButtonW + 6, y, giveButtonW, RowH);

                if (DrawButton(sb, pixel, giveRect, "Dar ítem seleccionado", mouse, new Color(35, 115, 75)))
                    GiveItemToRemotePlayer(target, false);

                if (DrawButton(sb, pixel, spamRect, "Lote de ítem x10", mouse, new Color(135, 85, 35)))
                    GiveItemToRemotePlayer(target, true);
            }
            else
            {
                Utils.DrawBorderString(sb, "Usa la pestaña Ítems para darte objetos.", new Vector2(panel.X + Pad, y + 5), Color.Gray, 0.62f);
            }

            y += RowH + 7;
            shown++;
        }

        if (shown == 0)
        {
            Utils.DrawBorderString(sb, "No hay jugadores conectados.", new Vector2(panel.X + Pad, y + 5), Color.Gray, 0.75f);
            y += RowH;
        }

        y += 8;
        string note = IsServer
            ? "Modo COMANDOS: todo va por comandos de TShock (segun tus permisos). Ban: usa /ban en el chat."
            : (InMultiplayer
                ? "Modo DIRECTO: solo 'Ir'. Traer/Matar/Kick/Mute necesitan TShock (modo COMANDOS)."
                : "Kick/Mute/Ir/Traer solo aplican en servidor.");
        Utils.DrawBorderString(sb, note, new Vector2(panel.X + Pad, panel.Bottom - 46), new Color(200, 180, 100), 0.65f);
    }


    private static void GiveItemToRemotePlayer(Player target, bool tenfoldBatch)
    {
        if (target == null || !target.active)
        {
            SetStatus("El jugador seleccionado ya no está conectado.");
            return;
        }

        if (!IsServer || !InMultiplayer)
        {
            SetStatus("Dar objetos a otro jugador requiere multijugador con TShock y permisos de /give.");
            return;
        }

        string query = (itemQuery ?? "").Trim();
        int itemId = ResolveItemId(query);
        if (itemId <= 0 || itemId >= ItemID.Count)
        {
            SetStatus("Primero ve a Ítems y escribe un nombre o ID válido; después vuelve aquí.");
            return;
        }

        int requested = int.TryParse(itemAmount, out int parsedAmount) && parsedAmount > 0
            ? Math.Clamp(parsedAmount, 1, 9999)
            : 1;
        int amount = tenfoldBatch ? (int)Math.Min(9999L, (long)requested * 10L) : requested;

        // Un solo comando por clic: el servidor valida permisos y entrega el objeto.
        // El lote x10 está limitado para evitar inundar el chat de comandos.
        SendCmd($"/give {Quote(target.name)} {itemId} {amount}");
        SetStatus($"Solicitud enviada: {amount} × {Lang.GetItemNameValue(itemId)} para {target.name}. Requiere TShock y permiso /give.");
    }

    // HUD compacto, dibujado aunque el panel principal esté cerrado.
    private static void DrawServerHud(SpriteBatch sb, Texture2D pixel)
    {
        Player p = Main.LocalPlayer;
        if (p == null || !p.active) return;
        var hudLines = new List<(string text, Color color, float scale)>();
        string connection = Main.netMode == 0 ? "Un jugador" : Main.netMode == 1 ? "Multijugador" : "Servidor";
        string world = string.IsNullOrWhiteSpace(Main.worldName) ? "Mundo sin nombre" : Main.worldName;
        if (hudShowConnection) hudLines.Add(("HERO | " + connection + " | " + world, new Color(110, 220, 255), 0.68f));
        if (hudShowPerformance)
        {
            string mode = Main.masterMode ? "Maestro" : Main.expertMode ? "Experto" : "Normal";
            hudLines.Add(($"Ping: {GetPingLabel()} | FPS: {Main.frameRate} | Modo: {mode}", Color.White, 0.64f));
        }
        if (hudShowVitals) hudLines.Add(($"Vida: {p.statLife}/{p.statLifeMax2} | Maná: {p.statMana}/{p.statManaMax2} | DEF: {p.statDefense}", Color.White, 0.64f));
        if (hudShowPositionTime) hudLines.Add(($"X: {(int)(p.position.X / 16f)} Y: {(int)(p.position.Y / 16f)} | {(Main.dayTime ? "Día" : "Noche")}", new Color(210, 220, 235), 0.62f));
        if (hudShowBiome)
        {
            string biome = p.ZoneDungeon ? "Mazmorra" : p.ZoneUnderworldHeight ? "Inframundo" : p.ZoneJungle ? "Jungla" :
                p.ZoneSnow ? "Nieve" : p.ZoneDesert ? "Desierto" : p.ZoneBeach ? "Playa" :
                p.ZoneCorrupt ? "Corrupción" : p.ZoneCrimson ? "Carmesí" : p.ZoneHallow ? "Sagrado" :
                (p.position.Y / 16f < Main.worldSurface ? "Superficie" : "Subsuelo");
            hudLines.Add(("Bioma: " + biome, new Color(190, 220, 190), 0.62f));
        }
        if (hudShowVelocity) hudLines.Add(($"Velocidad: X {p.velocity.X:0.00} | Y {p.velocity.Y:0.00}", Color.LightGray, 0.62f));
        if (hudShowInventory)
        {
            int occupied = p.inventory.Count(item => item != null && !item.IsAir);
            Item held = p.inventory != null && p.selectedItem >= 0 && p.selectedItem < p.inventory.Length ? p.inventory[p.selectedItem] : null;
            hudLines.Add(($"Inventario: {occupied}/{p.inventory.Length} espacios | Mano: {(held == null || held.IsAir ? "vacía" : held.Name)}", Color.LightGray, 0.62f));
        }
        if (hudShowCounts)
        {
            int hostile = Main.npc.Count(n => n != null && n.active && n.CanBeChasedBy());
            hudLines.Add(($"NPC activos: {Main.npc.Count(n => n != null && n.active)} | Enemigos: {hostile} | Proyectiles: {CountActiveProjectiles()}", Color.LightGray, 0.62f));
        }
        int width = Math.Min(560, Math.Max(260, Main.screenWidth - 24));
        int height = Math.Max(48, 24 + hudLines.Count * 21);
        int margin = 12;
        int maxX = Math.Max(margin, Main.screenWidth - width - margin);
        int maxY = Math.Max(margin, Main.screenHeight - height - margin);
        int x = serverHudPosition switch
        {
            1 => maxX,
            3 => maxX,
            4 => Math.Max(margin, (Main.screenWidth - width) / 2),
            5 => Math.Max(margin, (Main.screenWidth - width) / 2),
            _ => margin
        };
        int y = serverHudPosition switch
        {
            2 => maxY,
            3 => maxY,
            5 => maxY,
            _ => margin
        };
        Rectangle box = new Rectangle(x, y, width, height);
        sb.Draw(pixel, box, new Color(7, 10, 18, 220));
        sb.Draw(pixel, new Rectangle(x, y, width, 3), new Color(110, 220, 255));
        sb.Draw(pixel, new Rectangle(x, y + height - 2, width, 2), UiAccent * 0.9f);
        int lineY = y + 7;
        foreach (var line in hudLines)
        {
            string display = line.text;
            while (display.Length > 1 && FontAssets.MouseText.Value.MeasureString(display).X * line.scale > width - 18)
                display = display.Substring(0, display.Length - 1);
            Utils.DrawBorderString(sb, display, new Vector2(x + 9, lineY), line.color, line.scale);
            lineY += 21;
        }
        // El HUD es solo informativo: no captura el ratón ni bloquea menús/interacciones.
    }

    private static string GetServerInfoSummary()
    {
        Player p = Main.LocalPlayer;
        string connection = Main.netMode == 0 ? "Un jugador" : Main.netMode == 1 ? "Multijugador cliente" : "Servidor";
        if (p == null || !p.active) return $"Conexión={connection} · Mundo={Main.worldName} · Ping={GetPingLabel()} · FPS={Main.frameRate}";
        return $"Conexión={connection} · Mundo={Main.worldName} · Ping={GetPingLabel()} · FPS={Main.frameRate} · Vida={p.statLife}/{p.statLifeMax2} · Maná={p.statMana}/{p.statManaMax2} · X={(int)(p.position.X / 16f)} Y={(int)(p.position.Y / 16f)}";
    }

    // Busca latencia mediante reflexión para no depender de una API concreta de cada versión.
    private static string GetPingLabel()
    {
        try
        {
            Type t = typeof(Main).Assembly.GetType("Terraria.Netplay");
            object root = null;
            if (t != null)
            {
                FieldInfo f = t.GetField("Connection", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                if (f != null) root = f.GetValue(null);
                if (root == null)
                {
                    PropertyInfo p = t.GetProperty("Connection", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                    if (p != null) root = p.GetValue(null);
                }
            }
            Queue<object> queue = new Queue<object>();
            HashSet<object> seen = new HashSet<object>();
            if (root != null) queue.Enqueue(root);
            if (Main.instance != null) queue.Enqueue(Main.instance);
            int inspected = 0;
            while (queue.Count > 0 && inspected++ < 24)
            {
                object obj = queue.Dequeue();
                if (obj == null || obj is string || obj.GetType().IsPrimitive || !seen.Add(obj)) continue;
                foreach (MemberInfo m in obj.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    try
                    {
                        object value = null;
                        if (m is FieldInfo fi) value = fi.GetValue(obj);
                        else if (m is PropertyInfo pi && pi.GetIndexParameters().Length == 0 && pi.GetMethod != null) value = pi.GetValue(obj);
                        else continue;
                        string name = m.Name.ToLowerInvariant();
                        if ((name.Contains("ping") || name.Contains("latency") || name.Contains("roundtrip")) && value != null)
                        {
                            if (value is int n && n >= 0 && n < 120000) return n + " ms";
                            if (value is long l && l >= 0 && l < 120000) return l + " ms";
                            if (value is float f && f >= 0 && f < 120000) return ((int)f) + " ms";
                        }
                        if (value != null && !(value is string) && !value.GetType().IsPrimitive && queue.Count < 24 &&
                            (name.Contains("connection") || name.Contains("socket") || name.Contains("net") || name.Contains("diagnostic") || name.Contains("ping"))) queue.Enqueue(value);
                    }
                    catch { }
                }
            }
        }
        catch { }
        return Main.netMode == 0 ? "N/A (local)" : "N/D";
    }

    // ------------------------------------------------------------------ Informacion del mundo y jugador

    private static void DrawInfoTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y)
    {
        var p = Main.LocalPlayer;
        DrawSectionHeader(sb, panel, ref y, "Resumen del mundo y conexión");
        // La información que antes aparecía en una HUD flotante se consulta aquí.
        // Se evita dibujar una ventana independiente para no interferir con menús o conexiones.
        DrawInfoLine(sb, panel, ref y, "Conexión", Main.netMode == 0 ? "Un jugador" : Main.netMode == 1 ? "Multijugador cliente" : "Servidor dedicado");
        DrawInfoLine(sb, panel, ref y, "Ping", GetPingLabel());
        DrawInfoLine(sb, panel, ref y, "FPS", Main.frameRate.ToString());
        DrawInfoLine(sb, panel, ref y, "Velocidad", $"{p.velocity.Length():0.0} px/tick");
        Item selectedInfoItem = p.inventory[Math.Clamp(p.selectedItem, 0, p.inventory.Length - 1)];
        DrawInfoLine(sb, panel, ref y, "Objeto seleccionado", selectedInfoItem == null || selectedInfoItem.IsAir ? "Ninguno" : $"{selectedInfoItem.Name} (ID {selectedInfoItem.type})");
        DrawInfoLine(sb, panel, ref y, "NPC activos", Main.npc.Count(n => n != null && n.active).ToString());
        DrawInfoLine(sb, panel, ref y, "Enemigos", Main.npc.Count(n => n != null && n.active && n.damage > 0 && !n.friendly).ToString());
        DrawInfoLine(sb, panel, ref y, "Proyectiles", Main.projectile.Count(pr => pr != null && pr.active).ToString());
        string mode = Main.masterMode ? "Maestro" : Main.expertMode ? "Experto" : "Normal";
        DrawInfoLine(sb, panel, ref y, "Mundo", string.IsNullOrWhiteSpace(Main.worldName) ? "(sin nombre)" : Main.worldName);
        DrawInfoLine(sb, panel, ref y, "TamaÃ±o", $"{Main.maxTilesX} x {Main.maxTilesY} tiles");
        DrawInfoLine(sb, panel, ref y, "Dificultad", mode);
        DrawInfoLine(sb, panel, ref y, "Hardmode", Main.hardMode ? "Activado" : "No activado");
        DrawInfoLine(sb, panel, ref y, "Hora", Main.dayTime ? "DÃ­a" : "Noche");
        DrawInfoLine(sb, panel, ref y, "Clima", Main.raining ? "Lluvia" : "Sin lluvia");
        y += 5;
        DrawSectionHeader(sb, panel, ref y, "Tu personaje");
        DrawInfoLine(sb, panel, ref y, "Vida", $"{p.statLife}/{p.statLifeMax2}");
        DrawInfoLine(sb, panel, ref y, "ManÃ¡", $"{p.statMana}/{p.statManaMax2}");
        DrawInfoLine(sb, panel, ref y, "Defensa", p.statDefense.ToString());
        DrawInfoLine(sb, panel, ref y, "PosiciÃ³n", $"X {(int)(p.position.X / 16f)}  Y {(int)(p.position.Y / 16f)}");
        DrawInfoLine(sb, panel, ref y, "Bioma", p.ZoneDungeon ? "Mazmorra" : p.ZoneUnderworldHeight ? "Inframundo" :
            p.ZoneJungle ? "Jungla" : p.ZoneSnow ? "Nieve" : p.ZoneDesert ? "Desierto" :
            p.ZoneBeach ? "Playa" : p.ZoneCorrupt ? "CorrupciÃ³n" : p.ZoneCrimson ? "CarmesÃ­" :
            p.ZoneHallow ? "Sagrado" : (p.position.Y / 16f < Main.worldSurface ? "Superficie/otro" : "Subsuelo/otro"));
        Utils.DrawBorderString(sb, "Los datos mostrados son locales; el servidor controla el estado autoritativo.",
            new Vector2(panel.X + Pad, panel.Bottom - 48), new Color(220, 190, 120), 0.62f);
    }

    private static void DrawInfoLine(SpriteBatch sb, Rectangle panel, ref int y, string label, string value)
    {
        Utils.DrawBorderString(sb, label + ":", new Vector2(panel.X + Pad, y), new Color(155, 190, 230), 0.72f);
        Utils.DrawBorderString(sb, value ?? "-", new Vector2(panel.X + 150, y), Color.White, 0.72f);
        y += 20;
    }

    // ------------------------------------------------------------------ Inventario: inspeccion y portapapeles

    private static void DrawInventoryTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        var p = Main.LocalPlayer;
        DrawSectionHeader(sb, panel, ref y, "Inventario del personaje local");
        int half = (Width - Pad * 2 - 8) / 2;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, half, RowH), "Copiar inventario", mouse,
            new Color(45, 105, 135)))
        {
            inventoryClipboard = new Item[p.inventory.Length];
            for (int i = 0; i < p.inventory.Length; i++)
                inventoryClipboard[i] = p.inventory[i]?.Clone() ?? new Item();
            inventoryClipboardOwner = p.name ?? "Jugador";
            SetStatus($"Inventario copiado: {inventoryClipboardOwner}.");
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + half + 8, y, half, RowH), "Pegar en huecos", mouse,
            new Color(65, 115, 75)))
        {
            if (inventoryClipboard == null)
            {
                SetStatus("Primero copia un inventario.");
            }
            else
            {
                int pasted = 0;
                int n = Math.Min(p.inventory.Length, inventoryClipboard.Length);
                for (int i = 0; i < n; i++)
                {
                    if (p.inventory[i] == null || p.inventory[i].IsAir)
                    {
                        if (inventoryClipboard[i] != null && !inventoryClipboard[i].IsAir)
                        {
                            p.inventory[i] = inventoryClipboard[i].Clone();
                            pasted++;
                        }
                    }
                }
                if (pasted > 0)
                    SyncSelf();
                SetStatus($"Pegados {pasted} espacios del inventario copiado. No se sobrescribieron objetos.");
            }
        }
        y += RowH + 6;
        DrawInfoLine(sb, panel, ref y, "Portapapeles", inventoryClipboard == null ? "VacÃ­o" : inventoryClipboardOwner);
        DrawInfoLine(sb, panel, ref y, "Regla", "Solo pega en espacios vacÃ­os");
        y += 3;
        DrawSectionHeader(sb, panel, ref y, "Contenido (mochila)");
        int first = inventoryPage * 16;
        int last = Math.Min(p.inventory.Length, first + 16);
        int leftX = panel.X + Pad;
        int rightX = panel.X + Width / 2;
        for (int i = first; i < last; i++)
        {
            Item item = p.inventory[i];
            string itemName = item == null || item.IsAir ? "(vacÃ­o)" : item.Name;
            string stack = item == null || item.IsAir ? "" : $" x{item.stack}";
            int col = (i - first) / 8;
            int row = (i - first) % 8;
            int x = col == 0 ? leftX : rightX;
            int yy = y + row * 22;
            Utils.DrawBorderString(sb, $"{i + 1}. {itemName}{stack}",
                new Vector2(x, yy), item == null || item.IsAir ? Color.Gray : Color.White, 0.64f);
        }
        int navY = panel.Bottom - 76;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, navY, 120, 22), "Anterior", mouse))
            inventoryPage = Math.Max(0, inventoryPage - 1);
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + 128, navY, 120, 22), "Siguiente", mouse))
            inventoryPage = Math.Min(Math.Max(0, (p.inventory.Length - 1) / 16), inventoryPage + 1);
        Utils.DrawBorderString(sb, $"PÃ¡gina {inventoryPage + 1}", new Vector2(panel.X + Pad + 260, navY + 4),
            Color.LightGray, 0.7f);
        Utils.DrawBorderString(sb, "La copia es local y no lee el inventario privado de otros jugadores.",
            new Vector2(panel.X + Pad, panel.Bottom - 46), new Color(220, 190, 120), 0.62f);
    }

    // ------------------------------------------------------------------ Guia rapida

    private static void DrawGuideTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y)
    {
        DrawSectionHeader(sb, panel, ref y, "GuÃ­a rÃ¡pida: crafteos, buffs y progresiÃ³n");
        string[] tips =
        {
            "Crafteos: consulta el objeto en el menÃº de fabricaciÃ³n del juego",
            "y acÃ©rcate a la estaciÃ³n requerida para ver recetas disponibles.",
            "Materiales: guarda minerales, barras, gemas, telaraÃ±as y almas;",
            "muchas recetas dependen del progreso del mundo.",
            "",
            "BUFFS ÃšTILES",
            "RegeneraciÃ³n â€” recuperaciÃ³n de vida.",
            "Piel de hierro â€” aumenta la defensa.",
            "Velocidad â€” aumenta la velocidad de movimiento.",
            "RegeneraciÃ³n de manÃ¡ â€” acelera la recuperaciÃ³n de manÃ¡.",
            "EspeleÃ³logo â€” resalta minerales y tesoros.",
            "Resplandor â€” ilumina el entorno.",
            "",
            "PROGRESIÃ“N",
            "Pre-Hardmode: Rey Slime, Ojo de Cthulhu, jefes malignos,",
            "Abeja Reina, EsqueletrÃ³n y Muro de Carne.",
            "Hardmode: jefes mecÃ¡nicos, Plantera, Golem y eventos finales.",
            "",
            "Los drops y recetas pueden variar segÃºn versiÃ³n/mods.",
            "Esta pestaÃ±a es informativa; no altera el mundo."
        };
        foreach (string tip in tips)
        {
            if (y > panel.Bottom - 40)
                break;
            Color color = tip == "BUFFS ÃšTILES" || tip == "PROGRESIÃ“N"
                ? new Color(130, 180, 240) : (string.IsNullOrEmpty(tip) ? Color.Gray : Color.White);
            Utils.DrawBorderString(sb, tip, new Vector2(panel.X + Pad, y), color, 0.68f);
            y += 19;
        }
    }



    // ------------------------------------------------------------------ Editor local de objetos

    private static void DrawItemEditorTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, "Editor de objetos · inventario local");
        var player = Main.LocalPlayer;
        int selectedSlot = Math.Clamp(player.selectedItem, 0, player.inventory.Length - 1);
        Item heldItem = player.inventory[selectedSlot];
        string heldName = heldItem == null || heldItem.IsAir ? "Mano vacía" : heldItem.Name + " · ID " + heldItem.type;
        DrawInfoLine(sb, panel, ref y, "Objeto equipado actualmente", "Ranura " + (selectedSlot + 1) + ": " + heldName);
        int slotButtonW = (Width - Pad * 2 - 8) / 2;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, Width - Pad * 2, 24),
            editorExtraPage ? "Editor Zaza: PROPIEDADES EXTRA (clic para volver)" : (editorAdvancedPage ? "Editor Zaza: AVANZADO (clic para página extra)" : "Editor Zaza: abrir propiedades AVANZADAS"), mouse, editorAdvancedPage ? UiAccent : UiTabColor))
        {
            if (!editorAdvancedPage && !editorExtraPage) editorAdvancedPage = true;
            else if (editorAdvancedPage) { editorAdvancedPage = false; editorExtraPage = true; }
            else { editorExtraPage = false; editorAdvancedPage = false; }
            activeField = 0;
        }
        y += 30;

        // Preferencia del editor. Terraria Unified 1.4.5.8 no expone Item.timesToShoot;
        // el valor se conserva al cargar otra vez la ranura, pero requiere soporte en Player.cs
        // para modificar realmente los proyectiles generados.
        Utils.DrawBorderString(sb, "Multiplicador de disparo (experimental):", new Vector2(panel.X + Pad, y), UiAccent, 0.68f);
        y += 17;
        int[] projectileMultipliers = { 1, 2, 3, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096 };
        int multiplierCols = 5;
        int multiplierGap = 5;
        int multiplierW = (Width - Pad * 2 - multiplierGap * (multiplierCols - 1)) / multiplierCols;
        for (int i = 0; i < projectileMultipliers.Length; i++)
        {
            int multiplier = projectileMultipliers[i];
            int row = i / multiplierCols;
            int col = i % multiplierCols;
            var multiplierRect = new Rectangle(
                panel.X + Pad + col * (multiplierW + multiplierGap),
                y + row * 25,
                multiplierW,
                22);
            if (DrawButton(sb, pixel, multiplierRect, "×" + multiplier, mouse,
                editorProjectileMultiplier == multiplier ? UiAccent : UiTabColor))
            {
                editorProjectileMultiplier = multiplier;
                SetStatus($"Multiplicador ×{multiplier} seleccionado. Sin límite fijo de copias; valores altos pueden provocar lag o alcanzar el límite interno de Terraria.");
            }
        }
        y += 55;

        string radialText = radialProjectileMode ? "Disparo circular: ACTIVADO" : "Disparo normal: ACTIVADO";
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, Width - Pad * 2, RowH), radialText, mouse,
            radialProjectileMode ? new Color(35, 105, 70) : UiTabColor))
        {
            radialProjectileMode = !radialProjectileMode;
            multipliedProjectileIdentities.Clear();
            SetStatus(radialProjectileMode
                ? "Patrón circular activado: ×2, ×3 y ×4 salen distribuidos alrededor del jugador."
                : "Patrón circular desactivado; se conserva el patrón normal del arma.");
        }
        y += RowH + 8;

        if (editorExtraPage)
        {
            DrawSectionHeader(sb, panel, ref y, "Propiedades extra de combate y utilidad");
            int toggleWExtra = (Width - Pad * 2 - 12) / 3;
            bool[] flags = { editorChannel, editorMelee, editorMagic, editorRanged, editorSummon, editorNoWet, editorNotAmmo };
            string[] labels = { "Channel", "Melee", "Magic", "Ranged", "Summon", "NoWet", "NotAmmo" };
            for (int k = 0; k < flags.Length; k++)
            {
                int col = k % 3, row = k / 3;
                var rect = new Rectangle(panel.X + Pad + col * (toggleWExtra + 6), y + row * 29, toggleWExtra, 25);
                if (DrawButton(sb, pixel, rect, labels[k] + ": " + (flags[k] ? "SÍ" : "NO"), mouse, flags[k] ? UiAccent : UiTabColor))
                {
                    switch (k) { case 0: editorChannel = !editorChannel; break; case 1: editorMelee = !editorMelee; break; case 2: editorMagic = !editorMagic; break; case 3: editorRanged = !editorRanged; break; case 4: editorSummon = !editorSummon; break; case 5: editorNoWet = !editorNoWet; break; case 6: editorNotAmmo = !editorNotAmmo; break; }
                }
            }
            y += 68;
            Utils.DrawBorderString(sb, "Poder de pesca", new Vector2(panel.X + Pad, y), Color.White, 0.68f);
            Utils.DrawBorderString(sb, "Poder de cebo", new Vector2(panel.X + 195, y), Color.White, 0.68f);
            y += 17;
            DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 125, 24), 50, editorFishingPole, mouse, "fishingPole");
            DrawTextField(sb, pixel, new Rectangle(panel.X + 195, y, 125, 24), 51, editorBait, mouse, "bait");
            y += 30;
            Utils.DrawBorderString(sb, "ID NPC al usar", new Vector2(panel.X + Pad, y), Color.White, 0.68f);
            Utils.DrawBorderString(sb, "ID herramienta de tiles", new Vector2(panel.X + 230, y), Color.White, 0.68f);
            y += 17;
            DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 150, 24), 52, editorMakeNpc, mouse, "makeNPC");
            DrawTextField(sb, pixel, new Rectangle(panel.X + 230, y, 150, 24), 53, editorTileWand, mouse, "tileWand (-1 ninguno)");
            y += 34;
            int extraButtonW = (Width - Pad * 2 - 8) / 2;
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, extraButtonW, RowH), "Cargar propiedades extra", mouse, new Color(45, 90, 130))) LoadItemEditorSlot();
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + extraButtonW + 8, y, extraButtonW, RowH), "Aplicar propiedades extra", mouse, new Color(55, 115, 75))) ApplyItemEditorSlot();
            y += RowH + 10;
            Utils.DrawBorderString(sb, "Página adicional: categorías de daño, canalización, pesca y herramientas.", new Vector2(panel.X + Pad, y), Color.LightGray, 0.62f);
            return;
        }

        if (editorAdvancedPage)
        {
            DrawSectionHeader(sb, panel, ref y, "Propiedades adicionales del objeto (Zaza Mod)");
            Utils.DrawBorderString(sb, "Pico", new Vector2(panel.X + Pad, y), Color.White, 0.68f);
            Utils.DrawBorderString(sb, "Hacha", new Vector2(panel.X + 195, y), Color.White, 0.68f);
            Utils.DrawBorderString(sb, "Curación vida", new Vector2(panel.X + 365, y), Color.White, 0.68f);
            y += 17;
            DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 125, 24), 40, editorPick, mouse, "pick");
            DrawTextField(sb, pixel, new Rectangle(panel.X + 195, y, 125, 24), 41, editorAxe, mouse, "axe");
            DrawTextField(sb, pixel, new Rectangle(panel.X + 365, y, 125, 24), 42, editorHealLife, mouse, "healLife");
            y += 30;
            Utils.DrawBorderString(sb, "Curación maná", new Vector2(panel.X + Pad, y), Color.White, 0.68f);
            Utils.DrawBorderString(sb, "Tipo de munición", new Vector2(panel.X + 195, y), Color.White, 0.68f);
            Utils.DrawBorderString(sb, "ID de montura", new Vector2(panel.X + 365, y), Color.White, 0.68f);
            y += 17;
            DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 125, 24), 43, editorHealMana, mouse, "healMana");
            DrawTextField(sb, pixel, new Rectangle(panel.X + 195, y, 125, 24), 44, editorUseAmmo, mouse, "useAmmo");
            DrawTextField(sb, pixel, new Rectangle(panel.X + 365, y, 125, 24), 45, editorMountType, mouse, "mountType (-1 ninguno)");
            y += 30;
            Utils.DrawBorderString(sb, "Crear bloque (tile ID)", new Vector2(panel.X + Pad, y), Color.White, 0.68f);
            Utils.DrawBorderString(sb, "Crear pared (wall ID)", new Vector2(panel.X + 230, y), Color.White, 0.68f);
            y += 17;
            DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 150, 24), 46, editorCreateTile, mouse, "tile ID (-1 ninguno)");
            DrawTextField(sb, pixel, new Rectangle(panel.X + 230, y, 150, 24), 47, editorCreateWall, mouse, "wall ID (-1 ninguno)");
            y += 30;
            Utils.DrawBorderString(sb, "Tipo de buff", new Vector2(panel.X + Pad, y), Color.White, 0.68f);
            Utils.DrawBorderString(sb, "Duración buff (ticks)", new Vector2(panel.X + 230, y), Color.White, 0.68f);
            y += 17;
            DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 150, 24), 48, editorBuffType, mouse, "buff ID (0 ninguno)");
            DrawTextField(sb, pixel, new Rectangle(panel.X + 230, y, 150, 24), 49, editorBuffTime, mouse, "buffTime");
            y += 32;
            int toggleWAdvanced = (Width - Pad * 2 - 12) / 3;
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, toggleWAdvanced, 25), "UseTurn: " + (editorUseTurn ? "SÍ" : "NO"), mouse, editorUseTurn ? UiAccent : UiTabColor)) editorUseTurn = !editorUseTurn;
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + toggleWAdvanced + 6, y, toggleWAdvanced, 25), "NoMelee: " + (editorNoMelee ? "SÍ" : "NO"), mouse, editorNoMelee ? UiAccent : UiTabColor)) editorNoMelee = !editorNoMelee;
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + (toggleWAdvanced + 6) * 2, y, toggleWAdvanced, 25), "Sin gráfico: " + (editorNoUseGraphic ? "SÍ" : "NO"), mouse, editorNoUseGraphic ? UiAccent : UiTabColor)) editorNoUseGraphic = !editorNoUseGraphic;
            y += 34;
            int advButtonW = (Width - Pad * 2 - 8) / 2;
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, advButtonW, RowH), "Cargar datos", mouse, new Color(45, 90, 130))) LoadItemEditorSlot();
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + advButtonW + 8, y, advButtonW, RowH), "Aplicar propiedades", mouse, new Color(55, 115, 75))) ApplyItemEditorSlot();
            y += RowH + 8;
            Utils.DrawBorderString(sb, "Estos campos se aplican al objeto de la ranura seleccionada.", new Vector2(panel.X + Pad, y), Color.LightGray, 0.62f);
            Utils.DrawBorderString(sb, "En multijugador el servidor puede ignorar o sobrescribir cambios locales.", new Vector2(panel.X + Pad, panel.Bottom - 45), new Color(235, 195, 120), 0.62f);
            return;
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, slotButtonW, RowH), "Editar objeto equipado", mouse, UiAccent))
        {
            editorSlot = (selectedSlot + 1).ToString();
            LoadItemEditorSlot();
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + slotButtonW + 8, y, slotButtonW, RowH), "Cargar ranura", mouse))
            LoadItemEditorSlot();
        y += RowH + 7;
        Utils.DrawBorderString(sb, "Ranura de mochila (1-50):", new Vector2(panel.X + Pad, y), Color.White, 0.72f);
        Utils.DrawBorderString(sb, "Cantidad:", new Vector2(panel.X + 230, y), Color.White, 0.72f);
        Utils.DrawBorderString(sb, "Daño:", new Vector2(panel.X + 390, y), Color.White, 0.72f);
        y += 18;
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 95, 26), 6, editorSlot, mouse, "1");
        DrawTextField(sb, pixel, new Rectangle(panel.X + 230, y, 110, 26), 10, editorStack, mouse, "cantidad");
        DrawTextField(sb, pixel, new Rectangle(panel.X + 390, y, 110, 26), 7, editorDamage, mouse, "daño");
        y += 34;

        Utils.DrawBorderString(sb, "Defensa:", new Vector2(panel.X + Pad, y), Color.White, 0.72f);
        Utils.DrawBorderString(sb, "UseTime:", new Vector2(panel.X + 230, y), Color.White, 0.72f);
        Utils.DrawBorderString(sb, "Valor (cobre):", new Vector2(panel.X + 390, y), Color.White, 0.72f);
        y += 18;
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 110, 26), 8, editorDefense, mouse, "defensa");
        DrawTextField(sb, pixel, new Rectangle(panel.X + 230, y, 110, 26), 9, editorUseTime, mouse, "useTime");
        DrawTextField(sb, pixel, new Rectangle(panel.X + 390, y, 140, 26), 11, editorValue, mouse, "valor");
        y += 36;

        int bw = (Width - Pad * 2 - 8) / 2;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, RowH), "Cargar datos de la ranura", mouse, new Color(45, 90, 130)))
            LoadItemEditorSlot();
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + 8, y, bw, RowH), "Aplicar cambios", mouse, new Color(55, 115, 75)))
            ApplyItemEditorSlot();
        y += RowH + 8;

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, RowH), "Duplicar objeto en hueco vacío", mouse))
            DuplicateItemEditorSlot();
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + 8, y, bw, RowH), "Reparar / restablecer objeto", mouse))
            ResetItemEditorSlot();
        y += RowH + 10;

        Utils.DrawBorderString(sb, "ID del objeto:", new Vector2(panel.X + Pad, y), UiAccent, 0.72f);
        Utils.DrawBorderString(sb, "Prefijo (0 = ninguno):", new Vector2(panel.X + 230, y), UiAccent, 0.72f);
        y += 18;
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 110, 26), 14, editorType, mouse, "ID");
        DrawTextField(sb, pixel, new Rectangle(panel.X + 230, y, 130, 26), 15, editorPrefix, mouse, "prefijo");
        y += 34;
        int bwExtra = (Width - Pad * 2 - 8) / 2;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bwExtra, RowH), "Crear / reemplazar ranura", mouse, new Color(45, 105, 145)))
            CreateItemEditorSlot();
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bwExtra + 8, y, bwExtra, RowH), "Vaciar ranura", mouse, new Color(135, 55, 55)))
            ClearItemEditorSlot();
        y += RowH + 8;
        Utils.DrawBorderString(sb, "Retroceso:", new Vector2(panel.X + Pad, y), Color.White, 0.68f);
        Utils.DrawBorderString(sb, "Crítico:", new Vector2(panel.X + 195, y), Color.White, 0.68f);
        Utils.DrawBorderString(sb, "Rareza:", new Vector2(panel.X + 365, y), Color.White, 0.68f);
        y += 17;
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 125, 24), 20, editorKnockback, mouse, "knockback");
        DrawTextField(sb, pixel, new Rectangle(panel.X + 195, y, 100, 24), 21, editorCrit, mouse, "crítico");
        DrawTextField(sb, pixel, new Rectangle(panel.X + 365, y, 100, 24), 22, editorRarity, mouse, "rareza");
        y += 30;
        Utils.DrawBorderString(sb, "Animación:", new Vector2(panel.X + Pad, y), Color.White, 0.68f);
        Utils.DrawBorderString(sb, "Velocidad proyectil:", new Vector2(panel.X + 230, y), Color.White, 0.68f);
        Utils.DrawBorderString(sb, "ID proyectil (0 = ninguno):", new Vector2(panel.X + 400, y), Color.White, 0.62f);
        y += 17;
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 125, 24), 23, editorUseAnimation, mouse, "useAnimation");
        DrawTextField(sb, pixel, new Rectangle(panel.X + 230, y, 140, 24), 24, editorShootSpeed, mouse, "shootSpeed");
        DrawTextField(sb, pixel, new Rectangle(panel.X + 400, y, 120, 24), 25, editorProjectile, mouse, "ID proyectil");
        y += 30;

        // Propiedades adicionales del objeto, al estilo de un editor avanzado.
        Utils.DrawBorderString(sb, "Maná:", new Vector2(panel.X + Pad, y), Color.White, 0.68f);
        Utils.DrawBorderString(sb, "Estilo de uso:", new Vector2(panel.X + 195, y), Color.White, 0.68f);
        Utils.DrawBorderString(sb, "ID de munición:", new Vector2(panel.X + 365, y), Color.White, 0.68f);
        y += 17;
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 125, 24), 35, editorMana, mouse, "mana");
        DrawTextField(sb, pixel, new Rectangle(panel.X + 195, y, 125, 24), 36, editorUseStyle, mouse, "useStyle");
        DrawTextField(sb, pixel, new Rectangle(panel.X + 365, y, 125, 24), 37, editorAmmo, mouse, "ammo ID");
        y += 29;
        Utils.DrawBorderString(sb, "Escala:", new Vector2(panel.X + Pad, y), Color.White, 0.68f);
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y + 15, 125, 24), 38, editorScale, mouse, "scale");
        int toggleW = (Width - Pad * 2 - 12) / 3;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + 195, y + 15, toggleW, 24), "Auto uso: " + (editorAutoReuse ? "SÍ" : "NO"), mouse, editorAutoReuse ? UiAccent : UiTabColor))
            editorAutoReuse = !editorAutoReuse;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + 195 + toggleW + 6, y + 15, toggleW, 24), "Consumible: " + (editorConsumable ? "SÍ" : "NO"), mouse, editorConsumable ? UiAccent : UiTabColor))
            editorConsumable = !editorConsumable;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + 195 + (toggleW + 6) * 2, y + 15, toggleW, 24), "Favorito: " + (editorFavorite ? "SÍ" : "NO"), mouse, editorFavorite ? UiAccent : UiTabColor))
            editorFavorite = !editorFavorite;
        y += 45;

        // Presets de un clic: solo rellenan los campos; el usuario confirma con Aplicar cambios.
        int presetW = (Width - Pad * 2 - 12) / 3;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, presetW, 24), "Preset: arma", mouse))
        {
            editorDamage = "500"; editorDefense = "0"; editorUseTime = "1"; editorUseAnimation = "1";
            editorCrit = "100"; editorKnockback = "20"; editorShootSpeed = "20"; editorAutoReuse = true;
            editorMana = "0"; editorScale = "1";
            SetStatus("Preset de arma cargado en los campos; pulsa Aplicar cambios.");
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + presetW + 6, y, presetW, 24), "Preset: tanque", mouse))
        {
            editorDamage = "1"; editorDefense = "1000"; editorUseTime = "20"; editorUseAnimation = "20";
            editorCrit = "0"; editorKnockback = "0"; editorShootSpeed = "0"; editorAutoReuse = false;
            editorMana = "0"; editorScale = "1";
            SetStatus("Preset de tanque cargado en los campos; pulsa Aplicar cambios.");
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + (presetW + 6) * 2, y, presetW, 24), "Preset: rápido", mouse))
        {
            editorUseTime = "1"; editorUseAnimation = "1"; editorAutoReuse = true; editorShootSpeed = "30";
            SetStatus("Preset de uso rápido cargado en los campos; pulsa Aplicar cambios.");
        }
        y += 30;

        var p = Main.LocalPlayer;
        int slot = ParseEditorSlot();
        if (slot >= 0 && slot < p.inventory.Length)
        {
            Item it = p.inventory[slot];
            if (it != null && !it.IsAir)
            {
                DrawInfoLine(sb, panel, ref y, "Objeto seleccionado", $"{it.Name} (ID {it.type})");
                DrawInfoLine(sb, panel, ref y, "Estadísticas actuales", $"Daño {it.damage} · Defensa {it.defense} · UseTime {it.useTime}");
                DrawInfoLine(sb, panel, ref y, "Cantidad / valor", $"{it.stack} / {it.value} monedas de cobre");
                DrawInfoLine(sb, panel, ref y, "Combate", $"Crítico {it.crit}% · Retroceso {it.knockBack:0.##} · Rareza {it.rare}");
                DrawInfoLine(sb, panel, ref y, "Uso / proyectil", $"Animación {it.useAnimation} · Velocidad {it.shootSpeed:0.##} · ID {it.shoot}");
            }
            else
            {
                Utils.DrawBorderString(sb, "La ranura seleccionada está vacía.", new Vector2(panel.X + Pad, y), Color.Gray, 0.72f);
                y += 20;
            }
        }

        Utils.DrawBorderString(sb, "Limitación: cambios locales; un servidor puede rechazarlos o sobrescribirlos.",
            new Vector2(panel.X + Pad, panel.Bottom - 45), new Color(235, 195, 120), 0.62f);
    }

    private static int ParseEditorSlot()
    {
        if (!int.TryParse(editorSlot, out int slot))
            return -1;

        // El editor debe limitarse a las 50 ranuras de la mochila principal.
        // Evita modificar por accidente monedas, munición, armadura u otros slots internos.
        int inventorySlots = Math.Min(50, Main.LocalPlayer.inventory.Length);
        return slot >= 1 && slot <= inventorySlots ? slot - 1 : -1;
    }

    private static void LoadItemEditorSlot()
    {
        int slot = ParseEditorSlot();
        var p = Main.LocalPlayer;
        if (slot < 0 || slot >= p.inventory.Length || p.inventory[slot] == null || p.inventory[slot].IsAir)
        {
            SetStatus("La ranura indicada no contiene un objeto válido.");
            return;
        }
        Item it = p.inventory[slot];
        editorDamage = it.damage.ToString();
        editorDefense = it.defense.ToString();
        editorUseTime = it.useTime.ToString();
        editorStack = it.stack.ToString();
        editorValue = it.value.ToString();
        editorType = it.type.ToString();
        editorPrefix = it.prefix.ToString();
        editorKnockback = it.knockBack.ToString(System.Globalization.CultureInfo.InvariantCulture);
        editorCrit = it.crit.ToString();
        editorUseAnimation = it.useAnimation.ToString();
        editorShootSpeed = it.shootSpeed.ToString(System.Globalization.CultureInfo.InvariantCulture);
        editorProjectile = it.shoot.ToString();
        // No reiniciar la preferencia al recargar datos: el usuario puede volver a cargar
        // el objeto sin perder la selección del multiplicador en esta sesión.
        editorRarity = it.rare.ToString();
        editorMana = it.mana.ToString();
        editorUseStyle = it.useStyle.ToString();
        editorAmmo = it.useAmmo.ToString();
        editorScale = it.scale.ToString(System.Globalization.CultureInfo.InvariantCulture);
        editorAutoReuse = it.autoReuse;
        editorConsumable = it.consumable;
        editorFavorite = it.favorited;
        editorPick = it.pick.ToString();
        editorAxe = it.axe.ToString();
        editorHealLife = it.healLife.ToString();
        editorHealMana = it.healMana.ToString();
        editorUseAmmo = it.useAmmo.ToString();
        editorCreateTile = it.createTile.ToString();
        editorCreateWall = it.createWall.ToString();
        editorBuffType = it.buffType.ToString();
        editorBuffTime = it.buffTime.ToString();
        editorMountType = it.mountType.ToString();
        editorUseTurn = it.useTurn;
        editorNoMelee = it.noMelee;
        editorNoUseGraphic = it.noUseGraphic;
        editorChannel = it.channel;
        editorMelee = it.melee;
        editorMagic = it.magic;
        editorRanged = it.ranged;
        editorSummon = it.summon;
        editorNoWet = it.noWet;
        editorNotAmmo = it.notAmmo;
        editorFishingPole = it.fishingPole.ToString();
        editorBait = it.bait.ToString();
        editorMakeNpc = it.makeNPC.ToString();
        editorTileWand = it.tileWand.ToString();
        SetStatus($"Datos cargados: {it.Name}.");
    }

    private static void ApplyItemEditorSlot()
    {
        int slot = ParseEditorSlot();
        var p = Main.LocalPlayer;
        if (slot < 0 || slot >= p.inventory.Length || p.inventory[slot] == null || p.inventory[slot].IsAir)
        {
            SetStatus("Selecciona una ranura con un objeto.");
            return;
        }

        Item it = p.inventory[slot];
        if (int.TryParse(editorType, out int requestedType) && requestedType > 0 && requestedType < ItemID.Count && requestedType != it.type)
        {
            it.SetDefaults(requestedType);
            it.stack = 1;
        }
        if (int.TryParse(editorPrefix, out int requestedPrefix))
        {
            int safePrefix = Math.Clamp(requestedPrefix, 0, PrefixID.Count - 1);
            // Item.Prefix aplica también los modificadores de daño, velocidad, crítico, etc.
            // Asignar directamente la propiedad prefix solo cambia el identificador y deja
            // las estadísticas desincronizadas. El prefijo 0 significa "sin prefijo".
            if (safePrefix == 0)
                it.prefix = 0;
            else
                it.Prefix(safePrefix);
        }
        if (int.TryParse(editorDamage, out int damage)) it.damage = Math.Clamp(damage, 0, 1000000);
        if (int.TryParse(editorDefense, out int defense)) it.defense = Math.Clamp(defense, 0, 100000);
        if (int.TryParse(editorUseTime, out int useTime)) it.useTime = Math.Clamp(useTime, 1, 10000);
        if (int.TryParse(editorStack, out int stack)) it.stack = Math.Clamp(stack, 1, Math.Max(1, it.maxStack));
        if (int.TryParse(editorValue, out int value)) it.value = Math.Clamp(value, 0, int.MaxValue);
        if (float.TryParse(editorKnockback, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float knockback))
            it.knockBack = Math.Clamp(knockback, 0f, 10000f);
        if (int.TryParse(editorCrit, out int crit)) it.crit = Math.Clamp(crit, 0, 1000);
        if (int.TryParse(editorUseAnimation, out int animation)) it.useAnimation = Math.Clamp(animation, 1, 10000);
        if (float.TryParse(editorShootSpeed, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float shootSpeed))
            it.shootSpeed = Math.Clamp(shootSpeed, 0f, 10000f);
        // El multiplicador de disparo requiere un hook compatible en Player.ItemCheck_Shoot.
        if (int.TryParse(editorProjectile, out int projectileId))
        {
            if (projectileId >= 0 && projectileId < ProjectileID.Count)
                it.shoot = projectileId;
            else
                SetStatus($"ID de proyectil fuera de rango (0-{ProjectileID.Count - 1}).");
        }
        if (int.TryParse(editorRarity, out int rarity)) it.rare = (sbyte)Math.Clamp(rarity, -1, 11);
        if (int.TryParse(editorMana, out int mana)) it.mana = Math.Clamp(mana, 0, 100000);
        if (int.TryParse(editorUseStyle, out int useStyle)) it.useStyle = Math.Clamp(useStyle, 0, 20);
        if (int.TryParse(editorAmmo, out int ammo)) it.useAmmo = Math.Max(0, ammo);
        if (float.TryParse(editorScale, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float scale))
            it.scale = Math.Clamp(scale, 0.1f, 100f);
        it.autoReuse = editorAutoReuse;
        it.consumable = editorConsumable;
        it.favorited = editorFavorite;
        if (int.TryParse(editorPick, out int pick)) it.pick = Math.Clamp(pick, 0, 100000);
        if (int.TryParse(editorAxe, out int axe)) it.axe = Math.Clamp(axe, 0, 100000);
        if (int.TryParse(editorHealLife, out int healLife)) it.healLife = Math.Clamp(healLife, 0, 1000000);
        if (int.TryParse(editorHealMana, out int healMana)) it.healMana = Math.Clamp(healMana, 0, 1000000);
        if (int.TryParse(editorUseAmmo, out int useAmmo)) it.useAmmo = Math.Max(0, useAmmo);
        if (int.TryParse(editorCreateTile, out int createTile)) it.createTile = Math.Clamp(createTile, -1, TileID.Count - 1);
        if (int.TryParse(editorCreateWall, out int createWall)) it.createWall = Math.Clamp(createWall, -1, WallID.Count - 1);
        if (int.TryParse(editorBuffType, out int buffType)) it.buffType = Math.Clamp(buffType, 0, BuffID.Count - 1);
        if (int.TryParse(editorBuffTime, out int buffTime)) it.buffTime = Math.Clamp(buffTime, 0, 3600000);
        if (int.TryParse(editorMountType, out int mountType)) it.mountType = Math.Clamp(mountType, -1, MountID.Count - 1);
        it.useTurn = editorUseTurn;
        it.noMelee = editorNoMelee;
        it.noUseGraphic = editorNoUseGraphic;
        it.channel = editorChannel;
        it.melee = editorMelee;
        it.magic = editorMagic;
        it.ranged = editorRanged;
        it.summon = editorSummon;
        it.noWet = editorNoWet;
        it.notAmmo = editorNotAmmo;
        if (int.TryParse(editorFishingPole, out int fishingPole)) it.fishingPole = Math.Clamp(fishingPole, 0, 100000);
        if (int.TryParse(editorBait, out int bait)) it.bait = Math.Clamp(bait, 0, 100000);
        if (int.TryParse(editorMakeNpc, out int makeNpc)) it.makeNPC = (short)Math.Clamp(makeNpc, 0, Math.Min(short.MaxValue, NPCID.Count - 1));
        if (int.TryParse(editorTileWand, out int tileWand)) it.tileWand = Math.Clamp(tileWand, -1, TileID.Count - 1);
        SyncInventorySlot(p, slot, it);
        if (Main.netMode == 0)
            SetStatus($"Editado: {it.Name}. Propiedades aplicadas; el multiplicador ×{editorProjectileMultiplier} aún no afecta el disparo sin integración en Player.cs.");
        else
            SetStatus($"Cambios aplicados localmente y enviados para sincronización; el servidor puede rechazarlos o sobrescribirlos. El multiplicador ×{editorProjectileMultiplier} aún no afecta el disparo.");
    }

    private static void DuplicateItemEditorSlot()
    {
        int slot = ParseEditorSlot();
        var p = Main.LocalPlayer;
        if (slot < 0 || slot >= p.inventory.Length || p.inventory[slot] == null || p.inventory[slot].IsAir)
        {
            SetStatus("Selecciona primero un objeto válido.");
            return;
        }
        for (int i = 0; i < p.inventory.Length; i++)
        {
            if (p.inventory[i] == null || p.inventory[i].IsAir)
            {
                p.inventory[i] = p.inventory[slot].Clone();
                SyncInventorySlot(p, i, p.inventory[i]);
                SetStatus($"Copia creada en la ranura {i + 1}.");
                return;
            }
        }
        SetStatus("No hay espacios vacíos en la mochila.");
    }

    private static void CreateItemEditorSlot()
    {
        int slot = ParseEditorSlot();
        var p = Main.LocalPlayer;
        if (slot < 0 || slot >= p.inventory.Length)
        {
            SetStatus("Ranura inválida. Usa un número entre 1 y 50.");
            return;
        }
        if (!int.TryParse(editorType, out int type) || type <= 0 || type >= ItemID.Count)
        {
            SetStatus("ID de objeto inválido.");
            return;
        }
        Item created = new Item();
        created.SetDefaults(type);
        created.stack = 1;
        if (int.TryParse(editorStack, out int amount))
            created.stack = Math.Clamp(amount, 1, Math.Max(1, created.maxStack));
        if (int.TryParse(editorPrefix, out int prefix))
        {
            prefix = Math.Clamp(prefix, 0, PrefixID.Count - 1);
            if (prefix > 0)
                created.Prefix(prefix);
        }
        p.inventory[slot] = created;
        SyncInventorySlot(p, slot, created);

        // Recargar todos los campos desde el objeto recién creado evita que "Aplicar cambios"
        // reutilice datos antiguos del objeto anterior que estaba seleccionado en el editor.
        LoadItemEditorSlot();
        SetStatus($"Objeto creado en ranura {slot + 1}: {created.Name}.");
    }

    private static void ClearItemEditorSlot()
    {
        int slot = ParseEditorSlot();
        var p = Main.LocalPlayer;
        if (slot < 0 || slot >= p.inventory.Length)
        {
            SetStatus("Ranura inválida.");
            return;
        }
        p.inventory[slot].TurnToAir();
        SyncInventorySlot(p, slot, p.inventory[slot]);
        SetStatus($"Ranura {slot + 1} vaciada.");
    }

    private static void ResetItemEditorSlot()
    {
        int slot = ParseEditorSlot();
        var p = Main.LocalPlayer;
        if (slot < 0 || slot >= p.inventory.Length || p.inventory[slot] == null || p.inventory[slot].IsAir)
        {
            SetStatus("Selecciona primero un objeto válido.");
            return;
        }
        int type = p.inventory[slot].type;
        int stack = p.inventory[slot].stack;
        p.inventory[slot].SetDefaults(type);
        p.inventory[slot].stack = Math.Clamp(stack, 1, Math.Max(1, p.inventory[slot].maxStack));
        SyncInventorySlot(p, slot, p.inventory[slot]);
        LoadItemEditorSlot();
        SetStatus("Estadísticas restauradas a los valores base del objeto.");
    }


    // ------------------------------------------------------------------ Editor de vida y maná

    private static void DrawLifeEditorTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        var p = Main.LocalPlayer;
        DrawSectionHeader(sb, panel, ref y, "Editor avanzado de vida y maná");
        int lifeToggleW = (Width - Pad * 2 - 8) / 2;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, lifeToggleW, 25), lifeLockEnabled ? "Vida/Maná infinitos: ACTIVADO" : "Vida/Maná infinitos: desactivado", mouse, lifeLockEnabled ? new Color(40, 115, 75) : UiTabColor))
        {
            if (IsServer) SetStatus("El servidor controla la vida en modo COMANDOS; usa DIRECTO solo donde esté permitido.");
            else { lifeLockEnabled = !lifeLockEnabled; SetStatus(lifeLockEnabled ? "Vida y maná se restaurarán continuamente mientras estés vivo." : "Restauración continua desactivada."); }
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + lifeToggleW + 8, y, lifeToggleW, 25), "God Mode: alternar", mouse, godmodeEnabled ? UiAccent : UiTabColor))
        {
            godmodeEnabled = !godmodeEnabled;
            Main.LocalPlayer.creativeGodMode = godmodeEnabled;
            Main.LocalPlayer.immune = godmodeEnabled;
            Main.LocalPlayer.immuneTime = godmodeEnabled ? Math.Max(Main.LocalPlayer.immuneTime, 2) : 0;
            SyncSelf();
            SetStatus(godmodeEnabled ? "God Mode local activado." : "God Mode local desactivado.");
        }
        y += 31;
        if (DrawButton(sb, pixel, new Rectangle(panel.Right - Pad - 155, y - 22, 155, 22), "Leer valores actuales", mouse, UiAccent))
        {
            lifeCurrent = p.statLife.ToString();
            lifeMaximum = p.statLifeMax2.ToString();
            manaCurrent = p.statMana.ToString();
            manaMaximum = p.statManaMax2.ToString();
            SetStatus("Campos actualizados con los valores actuales del personaje.");
        }
        DrawInfoLine(sb, panel, ref y, "Estado actual", $"Vida {p.statLife}/{p.statLifeMax2}  ·  Maná {p.statMana}/{p.statManaMax2}");
        DrawInfoLine(sb, panel, ref y, "Límites base", $"Vida base {p.statLifeMax}  ·  Maná base {p.statManaMax}  ·  Defensa {p.statDefense}");
        y += 4;

        if (string.IsNullOrEmpty(lifeCurrent)) lifeCurrent = p.statLife.ToString();
        if (string.IsNullOrEmpty(lifeMaximum)) lifeMaximum = p.statLifeMax2.ToString();
        if (string.IsNullOrEmpty(manaCurrent)) manaCurrent = p.statMana.ToString();
        if (string.IsNullOrEmpty(manaMaximum)) manaMaximum = p.statManaMax2.ToString();
        if (string.IsNullOrEmpty(lifeDefense)) lifeDefense = p.statDefense.ToString();

        Utils.DrawBorderString(sb, "Vida actual", new Vector2(panel.X + Pad, y), Color.White, 0.72f);
        Utils.DrawBorderString(sb, "Vida máxima (sin tope artificial)", new Vector2(panel.X + 230, y), Color.White, 0.72f);
        y += 18;
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 150, 26), 16, lifeCurrent, mouse, "vida actual");
        DrawTextField(sb, pixel, new Rectangle(panel.X + 230, y, 210, 26), 17, lifeMaximum, mouse, "hasta Int32.MaxValue");
        y += 34;

        Utils.DrawBorderString(sb, "Maná actual", new Vector2(panel.X + Pad, y), Color.White, 0.72f);
        Utils.DrawBorderString(sb, "Maná máximo", new Vector2(panel.X + 230, y), Color.White, 0.72f);
        y += 18;
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, 150, 26), 18, manaCurrent, mouse, "maná actual");
        DrawTextField(sb, pixel, new Rectangle(panel.X + 230, y, 210, 26), 19, manaMaximum, mouse, "0 - 999999");
        y += 34;

        Utils.DrawBorderString(sb, "Defensa personalizada", new Vector2(panel.X + Pad, y), Color.White, 0.72f);
        DrawTextField(sb, pixel, new Rectangle(panel.X + 230, y - 3, 210, 26), 39, lifeDefense, mouse, "0 - 1000000");
        y += 32;

        int bw = (Width - Pad * 2 - 8) / 2;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, RowH), "Aplicar vida, maná y defensa", mouse, new Color(45, 115, 75)))
            ApplyLifeEditor();
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + 8, y, bw, RowH), "Curar al máximo", mouse, new Color(45, 90, 135)))
        {
            p.statLife = p.statLifeMax2;
            p.statMana = p.statManaMax2;
            lifeCurrent = p.statLife.ToString();
            lifeMaximum = p.statLifeMax2.ToString();
            manaCurrent = p.statMana.ToString();
            manaMaximum = p.statManaMax2.ToString();
            lifeDefense = p.statDefense.ToString();
            SyncSelf();
            SetStatus("Vida y maná restaurados al máximo configurado.");
        }
        y += RowH + 8;

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, RowH), "Vida al 50%", mouse))
        {
            p.statLife = Math.Max(1, p.statLifeMax2 / 2);
            lifeCurrent = p.statLife.ToString();
            SyncSelf();
            SetStatus("Vida ajustada al 50%.");
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + 8, y, bw, RowH), "Maná al 50%", mouse))
        {
            p.statMana = Math.Max(0, p.statManaMax2 / 2);
            manaCurrent = p.statMana.ToString();
            SyncSelf();
            SetStatus("Maná ajustado al 50%.");
        }
        y += RowH + 8;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, RowH), "Máximos estándar (500/200)", mouse))
        {
            p.statLifeMax = 500; p.statLifeMax2 = 500; p.statManaMax = 200; p.statManaMax2 = 200;
            p.statLife = Math.Min(p.statLife, 500); p.statMana = Math.Min(p.statMana, 200);
            lifeCurrent = p.statLife.ToString(); lifeMaximum = "500"; manaCurrent = p.statMana.ToString(); manaMaximum = "200";
            SyncSelf(); SetStatus("Máximos estándar aplicados localmente.");
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + 8, y, bw, RowH), "Máximos altos (1000/500)", mouse))
        {
            p.statLifeMax = 1000; p.statLifeMax2 = 1000; p.statManaMax = 500; p.statManaMax2 = 500;
            p.statLife = Math.Min(p.statLife, 1000); p.statMana = Math.Min(p.statMana, 500);
            lifeCurrent = p.statLife.ToString(); lifeMaximum = "1000"; manaCurrent = p.statMana.ToString(); manaMaximum = "500";
            SyncSelf(); SetStatus("Máximos altos aplicados localmente.");
        }
        y += RowH + 8;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, RowH), "Vida crítica (1 HP)", mouse, new Color(145, 55, 55)))
        {
            p.statLife = 1; lifeCurrent = "1"; SyncSelf(); SetStatus("Vida fijada en 1 HP.");
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + 8, y, bw, RowH), "Restablecer máximos base", mouse))
        {
            p.statLifeMax = 100; p.statLifeMax2 = 100; p.statManaMax = 20; p.statManaMax2 = 20;
            p.statLife = Math.Min(p.statLife, 100); p.statMana = Math.Min(p.statMana, 20);
            lifeCurrent = p.statLife.ToString(); lifeMaximum = "100"; manaCurrent = p.statMana.ToString(); manaMaximum = "20";
            SyncSelf(); SetStatus("Máximos base restablecidos localmente.");
        }
        y += RowH + 8;
        Utils.DrawBorderString(sb, "Nota: en multijugador el servidor puede corregir o rechazar cambios locales.",
            new Vector2(panel.X + Pad, y), new Color(235, 195, 120), 0.66f);
    }

    private static void ApplyLifeEditor()
    {
        var p = Main.LocalPlayer;
        if (!int.TryParse(lifeMaximum, out int maxLife) || maxLife < 1
            || !int.TryParse(lifeCurrent, out int currentLife) || currentLife < 0 || currentLife > maxLife
            || !int.TryParse(manaMaximum, out int maxMana) || maxMana < 0 || maxMana > 999999
            || !int.TryParse(manaCurrent, out int currentMana) || currentMana < 0 || currentMana > maxMana
            || !int.TryParse(lifeDefense, out int newDefense) || newDefense < 0 || newDefense > 1000000)
        {
            SetStatus("Valores inválidos: la vida máxima debe ser mayor que 0, la vida/maná actual no pueden superar su máximo y la defensa debe estar entre 0 y 1000000.");
            return;
        }

        // Cambia los máximos base del personaje local. En multijugador el servidor puede
        // ignorarlos o recalcularlos, ya que no es una API administrativa del servidor.
        p.statLifeMax = maxLife;
        p.statManaMax = maxMana;
        p.statLifeMax2 = maxLife;
        p.statManaMax2 = maxMana;
        p.statLife = currentLife;
        p.statMana = currentMana;
        defenseOverride = newDefense;
        defenseOverrideEnabled = true;
        p.statDefense = newDefense;
        lifeCurrent = p.statLife.ToString();
        lifeMaximum = p.statLifeMax2.ToString();
        manaCurrent = p.statMana.ToString();
        manaMaximum = p.statManaMax2.ToString();
        lifeDefense = newDefense.ToString();
        SyncSelf();
        SetStatus("Vida, maná y defensa aplicados localmente. Valores extremos pueden causar errores de juego; el servidor puede sobrescribirlos.");
    }


    // ------------------------------------------------------------------ WorldEdit basico local

    private static int worldEditUiPage;
    private static int worldEditCatalogPage;
    private static bool worldEditCatalogWalls;

    private static void DrawWorldEditTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, "WorldEdit · editor organizado por páginas");
        int bx = panel.X + Pad;
        int bw = (Width - Pad * 2 - 8) / 2;
        int navGap = 4;
        int navW = (Width - Pad * 2 - navGap * 5) / 6;
        string[] pageNames = { "Selección", "Bloques", "Paredes/ID", "Copiar/Líquidos", "Archivos", "Catálogo IDs" };
        for (int page = 0; page < pageNames.Length; page++)
        {
            var navRect = new Rectangle(bx + page * (navW + navGap), y, navW, 25);
            if (DrawButton(sb, pixel, navRect, pageNames[page], mouse,
                worldEditUiPage == page ? UiAccent : new Color(45, 50, 68)))
                worldEditUiPage = page;
        }
        y += 32;

        if (worldEditUiPage == 0)
        {
            string p1 = worldPos1.HasValue ? $"{worldPos1.Value.X}, {worldPos1.Value.Y}" : "Sin seleccionar";
            string p2 = worldPos2.HasValue ? $"{worldPos2.Value.X}, {worldPos2.Value.Y}" : "Sin seleccionar";
            DrawInfoLine(sb, panel, ref y, "POS1", p1);
            DrawInfoLine(sb, panel, ref y, "POS2", p2);
            if (worldPos1.HasValue && worldPos2.HasValue)
            {
                int areaWidth = Math.Abs(worldPos1.Value.X - worldPos2.Value.X) + 1;
                int areaHeight = Math.Abs(worldPos1.Value.Y - worldPos2.Value.Y) + 1;
                long areaTiles = (long)areaWidth * areaHeight;
                DrawInfoLine(sb, panel, ref y, "Área seleccionada", $"{areaWidth} × {areaHeight} tiles ({areaTiles:N0})");
            }
            else DrawInfoLine(sb, panel, ref y, "Área seleccionada", "Marca POS1 y POS2 para calcularla");

            if (DrawButton(sb, pixel, new Rectangle(bx, y, bw, RowH), "Marcar POS1 aquí", mouse, UiAccent))
            {
                Player p = Main.LocalPlayer;
                worldPos1 = new Point((int)(p.Center.X / 16f), (int)(p.Center.Y / 16f));
                SetStatus($"POS1: {worldPos1.Value.X}, {worldPos1.Value.Y}");
            }
            if (DrawButton(sb, pixel, new Rectangle(bx + bw + 8, y, bw, RowH), "Marcar POS2 aquí", mouse, UiAccent))
            {
                Player p = Main.LocalPlayer;
                worldPos2 = new Point((int)(p.Center.X / 16f), (int)(p.Center.Y / 16f));
                SetStatus($"POS2: {worldPos2.Value.X}, {worldPos2.Value.Y}");
            }
            y += RowH + 7;
            if (DrawButton(sb, pixel, new Rectangle(bx, y, bw, RowH), "Ampliar selección ×2", mouse, new Color(65, 90, 125)))
                ScaleWorldEditSelection(2);
            if (DrawButton(sb, pixel, new Rectangle(bx + bw + 8, y, bw, RowH), "Ampliar selección ×3", mouse, new Color(65, 90, 125)))
                ScaleWorldEditSelection(3);
            y += RowH + 6;
            Utils.DrawBorderString(sb, "Minimapa ampliado · clic alterna POS1/POS2", new Vector2(bx, y), new Color(145, 200, 240), 0.68f);
            y += 19;
            // El mapa crece en pantallas grandes y se oculta en pantallas muy bajas si no
            // cabe completo. Así ningún control de WorldEdit se dibuja fuera del panel.
            int mapSpace = panel.Bottom - y - RowH - 38;
            if (mapSpace >= 64)
            {
                int availableMapHeight = Math.Clamp(mapSpace, 64, 320);
                DrawWorldMiniMap(sb, pixel, new Rectangle(bx, y, Math.Max(1, panel.Width - Pad * 2), availableMapHeight), mouse, true);
                y += availableMapHeight + 5;
            }
            else
            {
                Utils.DrawBorderString(sb, "Pantalla baja: minimapa oculto para evitar que la UI se salga. Usa POS1/POS2 aquí arriba.",
                    new Vector2(bx, y), new Color(145, 200, 240), 0.56f);
                y += 16;
            }

            if (y + RowH <= panel.Bottom - 28 &&
                DrawButton(sb, pixel, new Rectangle(bx, y, Math.Max(1, panel.Width - Pad * 2), RowH), "Borrar selección POS1/POS2", mouse, new Color(100, 65, 70)))
            {
                worldPos1 = null;
                worldPos2 = null;
                SetStatus("Selección borrada. Marca POS1 y POS2 otra vez.");
            }
        }
        else if (worldEditUiPage == 1)
        {
            int idFieldGap = 12;
            int idFieldWidth = (Width - Pad * 2 - idFieldGap) / 2;
            Utils.DrawBorderString(sb, "Tile ID (0 = aire):", new Vector2(bx, y), Color.White, 0.68f);
            Utils.DrawBorderString(sb, "ID objetivo:", new Vector2(bx + idFieldWidth + idFieldGap, y), Color.White, 0.68f);
            y += 18;
            DrawTextField(sb, pixel, new Rectangle(bx, y, idFieldWidth, 25), 12, worldTileType, mouse, "1");
            DrawTextField(sb, pixel, new Rectangle(bx + idFieldWidth + idFieldGap, y, idFieldWidth, 25), 13, worldReplaceType, mouse, "0");
            y += 32;
            if (DrawButton(sb, pixel, new Rectangle(bx, y, bw, RowH), "Rellenar selección", mouse, new Color(50, 105, 70))) ApplyWorldEdit("fill");
            if (DrawButton(sb, pixel, new Rectangle(bx + bw + 8, y, bw, RowH), "Vaciar tiles", mouse, new Color(135, 65, 60))) ApplyWorldEdit("clear");
            y += RowH + 5;
            if (DrawButton(sb, pixel, new Rectangle(bx, y, bw, RowH), "Reemplazar ID → ID objetivo", mouse, new Color(80, 75, 130))) ApplyWorldEdit("replace");
            if (DrawButton(sb, pixel, new Rectangle(bx + bw + 8, y, bw, RowH), "Quitar bloques por ID objetivo", mouse, new Color(145, 55, 55))) ApplyWorldEdit("removeid");
            y += RowH + 5;
            if (DrawButton(sb, pixel, new Rectangle(bx, y, bw, RowH), "Contorno de tiles", mouse, new Color(65, 105, 135))) ApplyWorldEdit("outline");
            if (DrawButton(sb, pixel, new Rectangle(bx + bw + 8, y, bw, RowH), "Rellenar solo aire", mouse, new Color(45, 115, 85))) ApplyWorldEdit("airfill");
            y += RowH + 5;
            if (DrawButton(sb, pixel, new Rectangle(bx, y, bw, RowH), "Vaciar interior", mouse, new Color(120, 75, 65))) ApplyWorldEdit("hollow");
            if (DrawButton(sb, pixel, new Rectangle(bx + bw + 8, y, bw, RowH), "Borrar tiles + paredes", mouse, new Color(145, 55, 55))) ApplyWorldEdit("clearall");
        }
        else if (worldEditUiPage == 2)
        {
            DrawSectionHeader(sb, panel, ref y, "Paredes · edición por ID");
            int wallFieldGap = 12;
            int wallFieldWidth = (Width - Pad * 2 - wallFieldGap) / 2;
            Utils.DrawBorderString(sb, "Wall ID:", new Vector2(bx, y), Color.White, 0.68f);
            Utils.DrawBorderString(sb, "Reemplazar wall ID:", new Vector2(bx + wallFieldWidth + wallFieldGap, y), Color.White, 0.68f);
            y += 18;
            DrawTextField(sb, pixel, new Rectangle(bx, y, wallFieldWidth, 25), 27, worldWallType, mouse, "1");
            DrawTextField(sb, pixel, new Rectangle(bx + wallFieldWidth + wallFieldGap, y, wallFieldWidth, 25), 28, worldReplaceWallType, mouse, "0");
            y += 34;
            if (DrawButton(sb, pixel, new Rectangle(bx, y, bw, RowH), "Rellenar paredes", mouse, new Color(50, 105, 70))) ApplyWorldEdit("wallfill");
            if (DrawButton(sb, pixel, new Rectangle(bx + bw + 8, y, bw, RowH), "Borrar paredes", mouse, new Color(135, 65, 60))) ApplyWorldEdit("wallclear");
            y += RowH + 6;
            if (DrawButton(sb, pixel, new Rectangle(bx, y, bw, RowH), "Reemplazar wall ID", mouse, new Color(80, 75, 130))) ApplyWorldEdit("wallreplace");
            y += RowH + 6;
            if (DrawButton(sb, pixel, new Rectangle(bx, y, bw, RowH), "Quitar paredes por ID", mouse, new Color(145, 55, 55))) ApplyWorldEdit("wallremoveid");
            if (DrawButton(sb, pixel, new Rectangle(bx + bw + 8, y, bw, RowH), "Undo última acción", mouse, new Color(75, 85, 115))) UndoWorldEdit();
            y += RowH + 8;
            Utils.DrawBorderString(sb, "Los cambios se guardan en la pila Undo de WorldEdit.", new Vector2(bx, y), new Color(190, 205, 225), 0.62f);
        }
        else if (worldEditUiPage == 3)
        {
            DrawSectionHeader(sb, panel, ref y, "Copiar y pegar · portapapeles");
            if (DrawButton(sb, pixel, new Rectangle(bx, y, bw, RowH), "Copiar POS1 → POS2", mouse, new Color(65, 95, 145))) CopyWorldEditSelection();
            if (DrawButton(sb, pixel, new Rectangle(bx + bw + 8, y, bw, RowH), "Pegar en POS1", mouse, new Color(55, 115, 80))) PasteWorldEditClipboard();
            y += RowH + 5;
            string clipboardInfo = worldEditClipboard == null
                ? "Portapapeles vacío · copia una selección primero"
                : $"Portapapeles: {worldEditClipboardWidth} × {worldEditClipboardHeight} tiles";
            Utils.DrawBorderString(sb, clipboardInfo, new Vector2(bx, y), new Color(185, 220, 255), 0.62f);
            y += 23;
            DrawSectionHeader(sb, panel, ref y, "Líquidos · rellenar selección");
            if (DrawButton(sb, pixel, new Rectangle(bx, y, bw, RowH), "Rellenar agua", mouse, new Color(45, 105, 175))) ApplyWorldEdit("liquidwater");
            if (DrawButton(sb, pixel, new Rectangle(bx + bw + 8, y, bw, RowH), "Vaciar líquidos", mouse, new Color(85, 95, 110))) ApplyWorldEdit("liquidclear");
            y += RowH + 6;
            if (DrawButton(sb, pixel, new Rectangle(bx, y, bw, RowH), "Rellenar lava", mouse, new Color(170, 70, 45))) ApplyWorldEdit("liquidlava");
            if (DrawButton(sb, pixel, new Rectangle(bx + bw + 8, y, bw, RowH), "Rellenar miel", mouse, new Color(180, 135, 45))) ApplyWorldEdit("liquidhoney");
            y += RowH + 6;
            if (DrawButton(sb, pixel, new Rectangle(bx, y, bw, RowH), "Rellenar shimmer", mouse, new Color(135, 85, 185))) ApplyWorldEdit("liquidshimmer");
            y += RowH + 8;
            Utils.DrawBorderString(sb, "Usa POS1 y POS2 para definir el área de los líquidos.", new Vector2(bx, y), new Color(190, 205, 225), 0.62f);
        }
        else if (worldEditUiPage == 4)
        {
            Utils.DrawBorderString(sb, "Archivo de construcción (.hws)", new Vector2(bx, y), UiAccent, 0.72f);
            y += 22;
            DrawTextField(sb, pixel, new Rectangle(bx, y, 220, 25), 26, schematicFileName, mouse, "nombre del archivo");
            int fileButtonW = (Width - Pad * 2 - 240 - 8) / 2;
            if (DrawButton(sb, pixel, new Rectangle(bx + 228, y, fileButtonW, 25), "Exportar", mouse, UiAccent)) ExportWorldSchematic();
            if (DrawButton(sb, pixel, new Rectangle(bx + 228 + fileButtonW + 8, y, fileButtonW, 25), "Importar en POS1", mouse)) ImportWorldSchematic();
            y += 34;
            Utils.DrawBorderString(sb, worldSchematicStatus, new Vector2(bx, y), new Color(190, 220, 255), 0.62f);
            y += 22;
            Utils.DrawBorderString(sb, "Exporta la selección actual e importa la construcción desde POS1.", new Vector2(bx, y), Color.LightGray, 0.62f);
            y += 22;
            Utils.DrawBorderString(sb, "En multijugador el servidor puede revertir cambios locales.", new Vector2(bx, y), new Color(235, 195, 120), 0.62f);
        }
        else if (worldEditUiPage == 5)
        {
            DrawSectionHeader(sb, panel, ref y, "Catálogo completo de IDs · Terraria");
            int toggleW = (Width - Pad * 2 - 8) / 2;
            if (DrawButton(sb, pixel, new Rectangle(bx, y, toggleW, RowH),
                worldEditCatalogWalls ? "Ver bloques" : "Ver paredes", mouse, UiAccent))
            {
                worldEditCatalogWalls = !worldEditCatalogWalls;
                worldEditCatalogPage = 0;
            }
            Type idType = worldEditCatalogWalls ? typeof(WallID) : typeof(TileID);
            var idEntries = idType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Where(f => f.IsLiteral && !f.IsInitOnly && (f.FieldType == typeof(ushort) || f.FieldType == typeof(int)))
                .Select(f => new { Name = f.Name, Value = Convert.ToInt32(f.GetRawConstantValue()) })
                .Where(v => v.Value >= 0 && v.Value < (worldEditCatalogWalls ? WallID.Count : TileID.Count))
                .GroupBy(v => v.Value).Select(g => g.OrderBy(v => v.Name.Length).First())
                .OrderBy(v => v.Value).ToList();
            int pageSize = 8;
            int maxPage = Math.Max(0, (idEntries.Count - 1) / pageSize);
            worldEditCatalogPage = Math.Clamp(worldEditCatalogPage, 0, maxPage);
            y += RowH + 5;
            Utils.DrawBorderString(sb, $"{(worldEditCatalogWalls ? "PAREDES" : "BLOQUES")} · {idEntries.Count} IDs · página {worldEditCatalogPage + 1}/{maxPage + 1}",
                new Vector2(bx, y), new Color(190, 220, 255), 0.65f);
            y += 21;
            for (int i = worldEditCatalogPage * pageSize; i < Math.Min(idEntries.Count, (worldEditCatalogPage + 1) * pageSize); i++)
            {
                var entry = idEntries[i];
                Utils.DrawBorderString(sb, $"{entry.Value,5}  ·  {entry.Name}", new Vector2(bx, y), Color.White, 0.62f);
                y += 19;
            }
            y += 3;
            if (DrawButton(sb, pixel, new Rectangle(bx, y, toggleW, RowH), "Anterior", mouse, new Color(55, 75, 105)))
                worldEditCatalogPage = Math.Max(0, worldEditCatalogPage - 1);
            if (DrawButton(sb, pixel, new Rectangle(bx + toggleW + 8, y, toggleW, RowH), "Siguiente", mouse, new Color(55, 75, 105)))
                worldEditCatalogPage = Math.Min(maxPage, worldEditCatalogPage + 1);
            y += RowH + 5;
            Utils.DrawBorderString(sb, worldEditCatalogWalls
                ? "Borrar por ID: en Paredes/ID escribe el ID en Wall ID y pulsa Quitar paredes por ID."
                : "Borrar por ID: en Bloques escribe el ID en ID objetivo y pulsa Quitar bloques por ID objetivo.",
                new Vector2(bx, y), new Color(190, 205, 225), 0.58f);
        }
    }

    private static void CopyWorldEditSelection()
    {
        if (!worldPos1.HasValue || !worldPos2.HasValue)
        {
            SetStatus("Marca POS1 y POS2 antes de copiar.");
            return;
        }

        int x1 = Math.Clamp(Math.Min(worldPos1.Value.X, worldPos2.Value.X), 1, Main.maxTilesX - 2);
        int x2 = Math.Clamp(Math.Max(worldPos1.Value.X, worldPos2.Value.X), 1, Main.maxTilesX - 2);
        int y1 = Math.Clamp(Math.Min(worldPos1.Value.Y, worldPos2.Value.Y), 1, Main.maxTilesY - 2);
        int y2 = Math.Clamp(Math.Max(worldPos1.Value.Y, worldPos2.Value.Y), 1, Main.maxTilesY - 2);
        int width = x2 - x1 + 1;
        int height = y2 - y1 + 1;
        long area = (long)width * height;
        if (area > 100000)
        {
            SetStatus("Copia demasiado grande: máximo 100 000 tiles.");
            return;
        }

        try
        {
            var buffer = new Tile[width, height];
            for (int yy = 0; yy < height; yy++)
                for (int xx = 0; xx < width; xx++)
                {
                    Tile source = Main.tile[x1 + xx, y1 + yy];
                    buffer[xx, yy] = source == null ? new Tile() : (Tile)source.Clone();
                }

            worldEditClipboard = buffer;
            worldEditClipboardWidth = width;
            worldEditClipboardHeight = height;
            worldSchematicStatus = $"Copiado a memoria: {width} × {height} ({area:N0} tiles).";
            SetStatus(worldSchematicStatus + " Pega colocando POS1 en la esquina destino.");
        }
        catch (Exception ex)
        {
            SetStatus("No se pudo copiar la selección: " + ex.GetType().Name);
        }
    }

    private static void PasteWorldEditClipboard()
    {
        if (worldEditClipboard == null || worldEditClipboardWidth < 1 || worldEditClipboardHeight < 1)
        {
            SetStatus("El portapapeles está vacío. Marca POS1/POS2 y pulsa Copiar.");
            return;
        }
        if (!worldPos1.HasValue)
        {
            SetStatus("Marca POS1 donde quieres pegar la construcción.");
            return;
        }

        int startX = worldPos1.Value.X;
        int startY = worldPos1.Value.Y;
        if (startX < 1 || startY < 1 || startX + worldEditClipboardWidth >= Main.maxTilesX - 1 ||
            startY + worldEditClipboardHeight >= Main.maxTilesY - 1)
        {
            SetStatus("La construcción copiada no cabe en el mundo desde POS1.");
            return;
        }

        long area = (long)worldEditClipboardWidth * worldEditClipboardHeight;
        if (area > 100000)
        {
            SetStatus("Pegado demasiado grande: máximo 100 000 tiles.");
            return;
        }

        try
        {
            worldEditUndo.Clear();
            int changed = 0;
            for (int yy = 0; yy < worldEditClipboardHeight; yy++)
                for (int xx = 0; xx < worldEditClipboardWidth; xx++)
                {
                    int worldX = startX + xx;
                    int worldY = startY + yy;
                    Tile current = Main.tile[worldX, worldY];
                    Tile source = worldEditClipboard[xx, yy];
                    if (current == null || source == null) continue;
                    worldEditUndo.Add((worldX, worldY, (Tile)current.Clone()));
                    current.CopyFrom(source);
                    changed++;
                }

            worldSchematicStatus = $"Pegado en POS1: {worldEditClipboardWidth} × {worldEditClipboardHeight}.";
            SetStatus($"Pegado local completado: {changed:N0} tiles." + GetWorldEditAuthorityNotice());
        }
        catch (Exception ex)
        {
            SetStatus("Error al pegar: " + ex.GetType().Name + ". Usa Undo si la acción llegó a comenzar.");
        }
    }

    private static string GetSchematicPath()
    {
        string name = Path.GetFileNameWithoutExtension(schematicFileName ?? "construccion");
        foreach (char invalid in Path.GetInvalidFileNameChars())
            name = name.Replace(invalid, '_');
        if (string.IsNullOrWhiteSpace(name)) name = "construccion";
        string directory = Path.Combine(Main.SavePath, "HerosMod", "Schematics");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, name + ".hws");
    }

    private static void ExportWorldSchematic()
    {
        if (!worldPos1.HasValue || !worldPos2.HasValue) { SetStatus("Marca POS1 y POS2 antes de exportar."); return; }
        try
        {
            int x1 = Math.Clamp(Math.Min(worldPos1.Value.X, worldPos2.Value.X), 1, Main.maxTilesX - 2);
            int x2 = Math.Clamp(Math.Max(worldPos1.Value.X, worldPos2.Value.X), 1, Main.maxTilesX - 2);
            int y1 = Math.Clamp(Math.Min(worldPos1.Value.Y, worldPos2.Value.Y), 1, Main.maxTilesY - 2);
            int y2 = Math.Clamp(Math.Max(worldPos1.Value.Y, worldPos2.Value.Y), 1, Main.maxTilesY - 2);
            long area = (long)(x2 - x1 + 1) * (y2 - y1 + 1);
            if (area > 500000) { SetStatus("Selección demasiado grande: máximo 500 000 tiles."); return; }
            string path = GetSchematicPath();
            using (var writer = new StreamWriter(path, false))
            {
                writer.WriteLine("HEROSMOD_SCHEMATIC_1");
                writer.WriteLine((x2 - x1 + 1) + "," + (y2 - y1 + 1));
                for (int yy = y1; yy <= y2; yy++)
                {
                    for (int xx = x1; xx <= x2; xx++)
                    {
                        Tile tile = Main.tile[xx, yy];
                        writer.Write(tile != null && tile.HasTile ? tile.TileType + 1 : 0);
                        if (xx < x2) writer.Write(',');
                    }
                    writer.WriteLine();
                }
            }
            worldSchematicStatus = "Exportado: " + Path.GetFileName(path) + " (" + area.ToString("N0") + " tiles)";
            SetStatus("Archivo guardado en " + path);
        }
        catch (Exception ex) { worldSchematicStatus = "Error al exportar: " + ex.Message; SetStatus("No se pudo exportar el archivo."); }
    }

    private static void ImportWorldSchematic()
    {
        if (!worldPos1.HasValue) { SetStatus("Marca POS1 como esquina superior izquierda."); return; }
        try
        {
            string path = GetSchematicPath();
            if (!File.Exists(path)) { worldSchematicStatus = "No existe " + Path.GetFileName(path); SetStatus(worldSchematicStatus); return; }
            int width, height;
            using (var reader = new StreamReader(path))
            {
                if (reader.ReadLine() != "HEROSMOD_SCHEMATIC_1") throw new InvalidDataException("Formato .hws no reconocido.");
                string[] dims = (reader.ReadLine() ?? "").Split(',');
                if (dims.Length != 2 || !int.TryParse(dims[0], out width) || !int.TryParse(dims[1], out height) ||
                    width < 1 || height < 1 || (long)width * height > 500000)
                    throw new InvalidDataException("Dimensiones inválidas o demasiado grandes.");
                if (worldPos1.Value.X < 1 || worldPos1.Value.Y < 1 ||
                    worldPos1.Value.X + width >= Main.maxTilesX - 1 || worldPos1.Value.Y + height >= Main.maxTilesY - 1)
                    throw new InvalidDataException("La construcción no cabe desde POS1.");
                for (int row = 0; row < height; row++)
                {
                    string[] cells = (reader.ReadLine() ?? "").Split(',');
                    if (cells.Length != width) throw new InvalidDataException("Fila incompleta: " + (row + 1));
                    for (int col = 0; col < width; col++)
                        if (!int.TryParse(cells[col], out int encoded) || encoded < 0 || encoded > TileID.Count)
                            throw new InvalidDataException("ID de tile inválido.");
                }
            }
            int changed = 0;
            using (var reader = new StreamReader(path))
            {
                reader.ReadLine();
                reader.ReadLine();
                for (int row = 0; row < height; row++)
                {
                    string[] cells = (reader.ReadLine() ?? "").Split(',');
                    for (int col = 0; col < width; col++)
                    {
                        int encoded = int.Parse(cells[col]);
                        int x = worldPos1.Value.X + col;
                        int yy = worldPos1.Value.Y + row;
                        if (Main.tile[x, yy] == null) continue;
                        WorldGen.KillTile(x, yy, false, false, true);
                        if (encoded > 0) WorldGen.PlaceTile(x, yy, encoded - 1, true, true);
                        changed++;
                    }
                }
            }
            worldSchematicStatus = "Importado " + Path.GetFileName(path) + " · " + width + "×" + height;
            SetStatus("Construcción importada localmente en POS1: " + changed.ToString("N0") + " tiles." + GetWorldEditAuthorityNotice());
        }
        catch (Exception ex) { worldSchematicStatus = "Error al importar: " + ex.Message; SetStatus("No se pudo importar el archivo."); }
    }

    private static void ScaleWorldEditSelection(int multiplier)
    {
        if (!worldPos1.HasValue || !worldPos2.HasValue)
        {
            SetStatus("Marca POS1 y POS2 antes de ampliar la selección.");
            return;
        }

        multiplier = Math.Clamp(multiplier, 2, 3);
        Point first = worldPos1.Value;
        Point second = worldPos2.Value;
        int width = Math.Abs(second.X - first.X) + 1;
        int height = Math.Abs(second.Y - first.Y) + 1;
        int signX = second.X >= first.X ? 1 : -1;
        int signY = second.Y >= first.Y ? 1 : -1;
        long scaledArea = (long)width * multiplier * height * multiplier;
        if (scaledArea > 100000)
        {
            SetStatus("La selección ampliada superaría 100 000 tiles. Reduce POS1/POS2 primero.");
            return;
        }

        int newX = first.X + signX * (width * multiplier - 1);
        int newY = first.Y + signY * (height * multiplier - 1);
        if (newX < 1 || newX >= Main.maxTilesX - 1 || newY < 1 || newY >= Main.maxTilesY - 1)
        {
            SetStatus("No se puede ampliar ×" + multiplier + ": el área saldría de los límites del mundo.");
            return;
        }

        worldPos2 = new Point(newX, newY);
        SetStatus($"Selección ampliada ×{multiplier}: {width * multiplier} × {height * multiplier}. Pulsa Fill para colocar el bloque seleccionado.");
    }

    // En un cliente multijugador solo podemos modificar la copia local del mundo.
    // Este aviso evita presentar los cambios como confirmados por un servidor remoto.
    private static string GetWorldEditAuthorityNotice()
    {
        return Main.netMode == 1
            ? " Aviso: cambio local del cliente; el servidor remoto no lo ha confirmado y puede revertirlo."
            : string.Empty;
    }

    private static void ApplyWorldEdit(string operation)
    {
        // Quitar bloques por ID no requiere POS1/POS2: trabaja en un área local
        // centrada en el jugador para evitar escanear o borrar accidentalmente todo el mundo.
        const int removeIdRadius = 40;
        bool removeIdNearPlayer = operation == "removeid";
        if (!removeIdNearPlayer && (!worldPos1.HasValue || !worldPos2.HasValue))
        {
            SetStatus("Marca POS1 y POS2 antes de editar.");
            return;
        }

        int playerTileX = (int)(Main.LocalPlayer.Center.X / 16f);
        int playerTileY = (int)(Main.LocalPlayer.Center.Y / 16f);
        int x1 = removeIdNearPlayer
            ? Math.Clamp(playerTileX - removeIdRadius, 1, Main.maxTilesX - 2)
            : Math.Clamp(Math.Min(worldPos1.Value.X, worldPos2.Value.X), 1, Main.maxTilesX - 2);
        int x2 = removeIdNearPlayer
            ? Math.Clamp(playerTileX + removeIdRadius, 1, Main.maxTilesX - 2)
            : Math.Clamp(Math.Max(worldPos1.Value.X, worldPos2.Value.X), 1, Main.maxTilesX - 2);
        int y1 = removeIdNearPlayer
            ? Math.Clamp(playerTileY - removeIdRadius, 1, Main.maxTilesY - 2)
            : Math.Clamp(Math.Min(worldPos1.Value.Y, worldPos2.Value.Y), 1, Main.maxTilesY - 2);
        int y2 = removeIdNearPlayer
            ? Math.Clamp(playerTileY + removeIdRadius, 1, Main.maxTilesY - 2)
            : Math.Clamp(Math.Max(worldPos1.Value.Y, worldPos2.Value.Y), 1, Main.maxTilesY - 2);
        long selectedArea = (long)(x2 - x1 + 1) * (y2 - y1 + 1);
        if (selectedArea > 100000)
        {
            SetStatus("Selección demasiado grande para editar de una vez (máximo 100 000 tiles). Reduce el área.");
            return;
        }

        int tileId = int.TryParse(worldTileType, out int parsedTile) ? parsedTile : 1;
        int replaceId = int.TryParse(worldReplaceType, out int parsedReplace) ? parsedReplace : 0;
        int wallId = int.TryParse(worldWallType, out int parsedWall) ? parsedWall : 1;
        int replaceWallId = int.TryParse(worldReplaceWallType, out int parsedReplaceWall) ? parsedReplaceWall : 0;
        bool tileOperation = operation == "fill" || operation == "replace" || operation == "outline" ||
            operation == "clear" || operation == "airfill" || operation == "hollow" || operation == "clearall" || operation == "removeid";
        bool wallOperation = operation == "wallfill" || operation == "wallclear" ||
            operation == "wallreplace" || operation == "wallremoveid" || operation == "clearall";
        bool liquidOperation = operation.StartsWith("liquid", StringComparison.Ordinal);

        if (tileOperation && (tileId < 0 || tileId >= TileID.Count || replaceId < 0 || replaceId >= TileID.Count))
        {
            SetStatus("Tile ID fuera de rango.");
            return;
        }
        if (wallOperation && (wallId < 0 || wallId >= WallID.Count || replaceWallId < 0 || replaceWallId >= WallID.Count))
        {
            SetStatus("Wall ID fuera de rango.");
            return;
        }

        worldEditUndo.Clear();
        int changed = 0;
        for (int x = x1; x <= x2; x++)
            for (int yy = y1; yy <= y2; yy++)
            {
                Tile tile = Main.tile[x, yy];
                if (tile == null)
                    continue;

                if ((operation == "replace" || operation == "removeid") && (!tile.HasTile || tile.TileType != replaceId))
                    continue;
                if ((operation == "wallreplace" && tile.WallType != replaceWallId) ||
                    (operation == "wallremoveid" && tile.WallType != wallId))
                    continue;
                if ((operation == "wallclear" || operation == "wallremoveid") && tile.WallType == 0)
                    continue;
                if (operation == "clear" && !tile.HasTile)
                    continue;
                if (operation == "clearall" && !tile.HasTile && tile.WallType == 0)
                    continue;
                if (operation == "airfill" && tile.HasTile)
                    continue;
                if (operation == "hollow" && (x == x1 || x == x2 || yy == y1 || yy == y2))
                    continue;
                if (operation == "hollow" && !tile.HasTile)
                    continue;
                if (operation == "outline" && x != x1 && x != x2 && yy != y1 && yy != y2)
                    continue;

                worldEditUndo.Add((x, yy, (Tile)tile.Clone()));
                switch (operation)
                {
                    case "liquidwater":
                        tile.LiquidType = LiquidID.Water;
                        tile.LiquidAmount = 255;
                        break;
                    case "liquidlava":
                        tile.LiquidType = LiquidID.Lava;
                        tile.LiquidAmount = 255;
                        break;
                    case "liquidhoney":
                        tile.LiquidType = LiquidID.Honey;
                        tile.LiquidAmount = 255;
                        break;
                    case "liquidshimmer":
                        tile.LiquidType = LiquidID.Shimmer;
                        tile.LiquidAmount = 255;
                        break;
                    case "liquidclear":
                        tile.LiquidAmount = 0;
                        break;
                    case "removeid":
                        WorldGen.KillTile(x, yy, false, false, true);
                        break;
                    case "clear":
                    case "hollow":
                    case "clearall":
                        if (tile.HasTile)
                            WorldGen.KillTile(x, yy, false, false, true);
                        if (operation == "clearall")
                            tile.WallType = 0;
                        break;
                    case "fill":
                        if (tileId == 0)
                            WorldGen.KillTile(x, yy, false, false, true);
                        else
                            WorldGen.PlaceTile(x, yy, tileId, true, true);
                        break;
                    case "airfill":
                        if (tileId > 0 && !tile.HasTile)
                            WorldGen.PlaceTile(x, yy, tileId, true, true);
                        break;
                    case "replace":
                        WorldGen.KillTile(x, yy, false, false, true);
                        if (tileId > 0)
                            WorldGen.PlaceTile(x, yy, tileId, true, true);
                        break;
                    case "outline":
                        if (tileId == 0)
                            WorldGen.KillTile(x, yy, false, false, true);
                        else
                            WorldGen.PlaceTile(x, yy, tileId, true, true);
                        break;
                    case "wallclear":
                    case "wallremoveid":
                        tile.WallType = 0;
                        break;
                    case "wallfill":
                        tile.WallType = (ushort)wallId;
                        break;
                    case "wallreplace":
                        tile.WallType = (ushort)wallId;
                        break;
                }
                changed++;
            }

        if (changed == 0)
        {
            worldEditUndo.Clear();
            SetStatus("No hubo cambios en la selección.");
            return;
        }
        SetStatus((removeIdNearPlayer
            ? $"WorldEdit quitar ID {replaceId}: {changed:N0} bloques eliminados en radio de {removeIdRadius} tiles alrededor del jugador. Undo disponible."
            : liquidOperation
                ? $"WorldEdit líquidos {operation}: {changed:N0} celdas procesadas. Undo disponible para la última operación."
                : $"WorldEdit {operation}: {changed:N0} celdas modificadas. Undo disponible para la última operación.") + GetWorldEditAuthorityNotice());
    }

    private static void UndoWorldEdit()
    {
        if (worldEditUndo.Count == 0)
        {
            SetStatus("No hay una operación para deshacer.");
            return;
        }
        foreach (var entry in worldEditUndo)
        {
            if (entry.x < 0 || entry.x >= Main.maxTilesX || entry.y < 0 || entry.y >= Main.maxTilesY)
                continue;
            Tile current = Main.tile[entry.x, entry.y];
            if (current != null && entry.tile != null)
                current.CopyFrom(entry.tile);
        }
        int count = worldEditUndo.Count;
        worldEditUndo.Clear();
        SetStatus($"Undo local completado: restaurados {count} tiles." + GetWorldEditAuthorityNotice());
    }

    // ------------------------------------------------------------------ Herramientas adicionales: 200 acciones categorizadas

    private static void DrawToolsTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, "Centro de herramientas · catálogo con búsqueda y páginas");
        int fieldWidth = Math.Min(360, panel.Width - Pad * 2);
        Utils.DrawBorderString(sb, "Buscar herramientas:", new Vector2(panel.X + Pad, y), Color.White, 0.66f);
        y += 17;
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, fieldWidth, 25), 50, toolSearchQuery, mouse, "Buscar por nombre...");
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + fieldWidth + 8, y, 90, 25), "Limpiar", mouse))
        {
            toolSearchQuery = string.Empty;
            activeField = 0;
            toolsPage = 0;
        }
        y += 31;

        Utils.DrawBorderString(sb, "Objetivo:", new Vector2(panel.X + Pad, y), Color.White, 0.62f);
        Utils.DrawBorderString(sb, "Motivo:", new Vector2(panel.X + 330, y), Color.White, 0.62f);
        y += 16;
        int targetW = Math.Min(300, (panel.Width - Pad * 2 - 8) / 2);
        int reasonX = panel.X + Pad + targetW + 8;
        int reasonW = Math.Max(100, panel.Right - Pad - reasonX);
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad, y, targetW, 24), 4, moderationTarget, mouse, "Nombre o argumento...");
        DrawTextField(sb, pixel, new Rectangle(reasonX, y, reasonW, 24), 5, moderationReason, mouse, "Motivo opcional...");
        y += 29;

        var filtered = string.IsNullOrWhiteSpace(toolSearchQuery)
            ? extraTools.Select((tool, index) => (tool, index)).ToArray()
            : extraTools.Select((tool, index) => (tool, index))
                .Where(entry => entry.tool.label.Contains(toolSearchQuery, StringComparison.OrdinalIgnoreCase)
                    || entry.tool.command.Contains(toolSearchQuery, StringComparison.OrdinalIgnoreCase))
                .ToArray();

        int pageSize = 8;
        int pageCount = Math.Max(1, (filtered.Length + pageSize - 1) / pageSize);
        toolsPage = Math.Clamp(toolsPage, 0, pageCount - 1);
        int first = toolsPage * pageSize;
        int cols = 2;
        int buttonW = (panel.Width - Pad * 2 - 8) / cols;
        int rowStep = 27;
        int availableBottom = panel.Bottom - 77;
        int availableRows = Math.Max(1, Math.Min(4, (availableBottom - y) / rowStep));
        pageSize = availableRows * cols;
        pageCount = Math.Max(1, (filtered.Length + pageSize - 1) / pageSize);
        toolsPage = Math.Clamp(toolsPage, 0, pageCount - 1);
        first = toolsPage * pageSize;

        for (int localIndex = 0; localIndex < pageSize; localIndex++)
        {
            int filteredIndex = first + localIndex;
            if (filteredIndex >= filtered.Length)
                break;
            var entry = filtered[filteredIndex];
            int col = localIndex % cols;
            int row = localIndex / cols;
            var rect = new Rectangle(panel.X + Pad + col * (buttonW + 8), y + row * rowStep, buttonW, 23);
            string toolLabel = entry.tool.label;
            Color color = toolLabel.StartsWith("MODERACIÓN") || toolLabel.StartsWith("USUARIOS") || toolLabel.StartsWith("REGIONES") ? new Color(112, 55, 62)
                : toolLabel.StartsWith("TIEMPO") || toolLabel.StartsWith("CLIMA") || toolLabel.StartsWith("EVENTOS") || toolLabel.StartsWith("PROGRESIÓN") ? new Color(45, 86, 120)
                : toolLabel.StartsWith("TELETRANSPORTE") || toolLabel.StartsWith("JUGADOR") ? new Color(48, 91, 75)
                : toolLabel.StartsWith("OBJETOS") || toolLabel.StartsWith("NPC") ? new Color(92, 76, 43)
                : toolLabel.StartsWith("SERVIDOR") || toolLabel.StartsWith("CONFIGURACIÓN") ? new Color(75, 75, 88)
                : toolLabel.StartsWith("ECONOMÍA") ? new Color(92, 58, 112)
                : new Color(50, 55, 68);
            if (DrawButton(sb, pixel, rect, $"{entry.index + 1:000}. {entry.tool.label}", mouse, color))
                RunExtraTool(entry.index);
        }

        int navY = panel.Bottom - 70;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, navY, 105, 22), "Anterior", mouse))
            toolsPage = Math.Max(0, toolsPage - 1);
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + 112, navY, 105, 22), "Siguiente", mouse))
            toolsPage = Math.Min(pageCount - 1, toolsPage + 1);
        string countText = string.IsNullOrWhiteSpace(toolSearchQuery)
            ? $"{filtered.Length} herramientas"
            : $"{filtered.Length} resultados";
        Utils.DrawBorderString(sb, $"Página {toolsPage + 1}/{pageCount} · {countText}",
            new Vector2(panel.X + Pad + 230, navY + 4), new Color(180, 205, 235), 0.65f);
        Utils.DrawBorderString(sb, "Acciones de servidor requieren permisos y comandos compatibles.",
            new Vector2(panel.X + Pad, panel.Bottom - 43), new Color(235, 195, 120), 0.61f);
    }


    private static void RunExtraTool(int index)
    {
        if (index < 0 || index >= extraTools.Length)
            return;

        if (Main.netMode != 1 || serverMode != ServerMode.Comandos)
        {
            SetStatus("Estas herramientas requieren multijugador y modo COMANDOS (TShock/plugins).");
            return;
        }

        string template = extraTools[index].command;
        if (template.Contains("{target}") && string.IsNullOrWhiteSpace(moderationTarget))
        {
            SetStatus("Escribe primero el objetivo en el campo superior.");
            return;
        }

        string target = Quote(moderationTarget.Trim());
        string reason = string.IsNullOrWhiteSpace(moderationReason) ? "" : Quote(moderationReason.Trim());
        string command = template.Replace("{target}", target).Replace("{reason}", reason);
        while (command.Contains("  "))
            command = command.Replace("  ", " ");
        command = command.Trim();

        SendCmd(command);
        SetStatus($"Herramienta enviada: {extraTools[index].label}. El servidor debe permitir el comando.");
    }


    // Herramientas inspiradas en Zaza: administración local de NPC y monitor de proyectiles.
    // En servidores remotos, el servidor conserva la autoridad sobre las entidades.
    private static void DrawNPCsTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, "Administrador de NPC · Tiendas y suerte");
        int optionGap = 6;
        int optionW = (panel.Width - Pad * 2 - optionGap) / 2;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, optionW, RowH),
            "Tienda gratis: " + (npcShopFreeEnabled ? "ACTIVA" : "OFF"), mouse,
            npcShopFreeEnabled ? new Color(35, 125, 75) : new Color(55, 65, 85)))
        {
            npcShopFreeEnabled = !npcShopFreeEnabled;
            if (!npcShopFreeEnabled) RestoreShopPrices();
            SetStatus(npcShopFreeEnabled
                ? "Tienda gratis activada. Abre una tienda NPC; el resultado depende de la versión y de la autoridad del servidor."
                : "Tienda gratis desactivada; se restauraron los precios que el panel pudo guardar.");
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + optionW + optionGap, y, optionW, RowH),
            "Suerte máxima: " + (luckBoostEnabled ? "ACTIVA" : "OFF"), mouse,
            luckBoostEnabled ? new Color(35, 125, 75) : new Color(55, 65, 85)))
        {
            if (!luckBoostEnabled)
            {
                luckOriginalCaptured = false;
                luckPlayerRef = null;
            }
            luckBoostEnabled = !luckBoostEnabled;
            SetStatus(luckBoostEnabled
                ? "Suerte aumentada activada; se aplicará localmente mientras el panel esté activo."
                : "Suerte aumentada desactivada; se intentará restaurar el valor anterior.");
        }
        y += RowH + 5;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, optionW, RowH), "Restaurar suerte base", mouse, new Color(75, 85, 115)))
        {
            luckBoostEnabled = false;
            RestoreLuckValue();
            SetStatus("Se desactivó la suerte aumentada y se restauró el valor guardado cuando fue posible.");
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + optionW + optionGap, y, optionW, RowH), "Restaurar precios guardados", mouse, new Color(85, 75, 100)))
        {
            npcShopFreeEnabled = false;
            RestoreShopPrices();
            SetStatus("Se desactivó tienda gratis y se restauraron los precios guardados por el panel.");
        }
        y += RowH + 7;
        // Contador en vivo: muestra el valor que Terraria expone para el jugador local.
        var luckCounterRect = new Rectangle(panel.X + Pad, y, panel.Width - Pad * 2, 25);
        sb.Draw(pixel, luckCounterRect, new Color(22, 29, 43, 230));
        sb.Draw(pixel, new Rectangle(luckCounterRect.X, luckCounterRect.Y, 3, luckCounterRect.Height), new Color(105, 210, 150));
        Utils.DrawBorderString(sb, GetLuckCounterText(), new Vector2(luckCounterRect.X + 9, luckCounterRect.Y + 4),
            luckBoostEnabled ? new Color(130, 255, 170) : new Color(205, 220, 240), 0.64f);
        y += 31;
        Utils.DrawBorderString(sb, "Invocación rápida cerca de tu personaje:", new Vector2(panel.X + Pad, y), Color.White, 0.72f);
        y += 24;
        int gap = 8;
        int bw = (Width - Pad * 2 - gap) / 2;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, RowH), "Invocar Slime azul", mouse, new Color(45, 90, 145)))
            SpawnPanelNpc(1);
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + gap, y, bw, RowH), "Invocar Zombie", mouse, new Color(90, 100, 65)))
            SpawnPanelNpc(3);
        y += RowH + 6;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, RowH), "Invocar Guía", mouse, new Color(70, 115, 80)))
            SpawnPanelNpc(22);
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + gap, y, bw, RowH), "Invocar Conejo", mouse, new Color(130, 90, 105)))
            SpawnPanelNpc(46);
        y += RowH + 8;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, Width - Pad * 2, RowH), "Eliminar NPC hostiles locales", mouse, new Color(145, 55, 45)))
            RemovePanelHostileNpcs();
        y += RowH + 12;
        DrawSectionHeader(sb, panel, ref y, "NPC activos");
        int shown = 0;
        for (int i = 0; i < Main.npc.Length && shown < 10; i++)
        {
            NPC npc = Main.npc[i];
            if (npc == null || !npc.active) continue;
            if (y + 22 > panel.Bottom - 42) break;
            string name = Lang.GetNPCNameValue(npc.type);
            if (string.IsNullOrWhiteSpace(name)) name = "NPC " + npc.type;
            Utils.DrawBorderString(sb, $"#{i} {name} · vida {npc.life}/{Math.Max(1, npc.lifeMax)}",
                new Vector2(panel.X + Pad, y + 3), Color.White, 0.65f);
            y += 21;
            shown++;
        }
        if (shown == 0)
            Utils.DrawBorderString(sb, "No hay NPC activos.", new Vector2(panel.X + Pad, y), Color.Gray, 0.7f);
        Utils.DrawBorderString(sb, "En multijugador, los cambios locales pueden ser rechazados por el servidor.",
            new Vector2(panel.X + Pad, panel.Bottom - 36), new Color(230, 190, 100), 0.58f);
    }

    // Aplica la opción de tienda sin depender de nombres de API concretos entre versiones.
    // Los precios originales se guardan y restauran al apagar la opción.
    private static object GetReflectedMember(object target, string name)
    {
        if (target == null) return null;
        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        Type type = target.GetType();
        try
        {
            FieldInfo field = type.GetField(name, flags);
            if (field != null) return field.GetValue(target);
        }
        catch { }
        try
        {
            PropertyInfo property = type.GetProperty(name, flags);
            if (property != null && property.CanRead && property.GetIndexParameters().Length == 0)
                return property.GetValue(target);
        }
        catch { }
        return null;
    }

    private static bool SetReflectedMember(object target, string name, object value)
    {
        if (target == null) return false;
        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        Type type = target.GetType();
        try
        {
            FieldInfo field = type.GetField(name, flags);
            if (field != null && !field.IsInitOnly)
            {
                Type targetType = Nullable.GetUnderlyingType(field.FieldType) ?? field.FieldType;
                object converted = value == null ? null : Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
                field.SetValue(target, converted);
                return true;
            }
        }
        catch { }
        try
        {
            PropertyInfo property = type.GetProperty(name, flags);
            if (property != null && property.CanWrite && property.GetIndexParameters().Length == 0)
            {
                Type targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                object converted = value == null ? null : Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
                property.SetValue(target, converted);
                return true;
            }
        }
        catch { }
        return false;
    }

    private static object GetStaticReflectedMember(Type type, string name)
    {
        if (type == null) return null;
        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        try
        {
            FieldInfo field = type.GetField(name, flags);
            if (field != null) return field.GetValue(null);
        }
        catch { }
        try
        {
            PropertyInfo property = type.GetProperty(name, flags);
            if (property != null && property.CanRead && property.GetIndexParameters().Length == 0)
                return property.GetValue(null);
        }
        catch { }
        return null;
    }

    private static void ApplyNpcShopFreeSetting()
    {
        if (!npcShopFreeEnabled) return;
        try
        {
            // Main.instance.shop[Main.npcShop].item[] es la tienda abierta en Terraria vanilla.
            object mainInstance = Main.instance;
            object shopCollection = GetReflectedMember(mainInstance, "shop");
            object shopIndexValue = GetStaticReflectedMember(typeof(Main), "npcShop");
            int shopIndex = shopIndexValue is int index ? index : -1;
            if (shopCollection is Array shops && shopIndex >= 0 && shopIndex < shops.Length)
            {
                object shop = shops.GetValue(shopIndex);
                object itemsObject = GetReflectedMember(shop, "item");
                if (itemsObject is Array items)
                {
                    foreach (object item in items)
                    {
                        if (item == null || GetReflectedMember(item, "IsAir") is bool isAir && isAir) continue;
                        object original = GetReflectedMember(item, "shopCustomPrice");
                        if (!shopPriceOriginals.ContainsKey(item))
                            shopPriceOriginals[item] = original ?? DBNull.Value;
                        // 0 indica precio gratuito en las versiones que exponen shopCustomPrice.
                        SetReflectedMember(item, "shopCustomPrice", 0);
                    }
                }
            }

            // Fallback para versiones donde la tienda usa PriceAdjustment.
            Player player = Main.LocalPlayer;
            object settings = GetReflectedMember(player, "currentShoppingSettings");
            if (settings != null)
            {
                if (!ReferenceEquals(settings, shoppingSettingsRef))
                {
                    shoppingSettingsRef = settings;
                    shoppingAdjustmentOriginal = GetReflectedMember(settings, "PriceAdjustment");
                    shoppingAdjustmentCaptured = shoppingAdjustmentOriginal != null;
                }
                SetReflectedMember(settings, "PriceAdjustment", 0f);
            }
        }
        catch
        {
            // No propagar fallos de reflexión al hilo de dibujo/juego.
        }
    }

    private static void RestoreShopPrices()
    {
        foreach (KeyValuePair<object, object> entry in shopPriceOriginals)
        {
            try
            {
                if (entry.Value == DBNull.Value)
                    SetReflectedMember(entry.Key, "shopCustomPrice", null);
                else
                    SetReflectedMember(entry.Key, "shopCustomPrice", entry.Value);
            }
            catch { }
        }
        shopPriceOriginals.Clear();
        if (shoppingAdjustmentCaptured && shoppingSettingsRef != null && shoppingAdjustmentOriginal != null)
            SetReflectedMember(shoppingSettingsRef, "PriceAdjustment", shoppingAdjustmentOriginal);
        shoppingSettingsRef = null;
        shoppingAdjustmentOriginal = null;
        shoppingAdjustmentCaptured = false;
    }

    private static void ApplyLuckBoostSetting()
    {
        if (!luckBoostEnabled)
        {
            RestoreLuckValue();
            return;
        }

        try
        {
            Player player = Main.LocalPlayer;
            if (player == null || !player.active) return;
            object current = GetReflectedMember(player, "luck");
            if (current == null) return;
            if (!luckOriginalCaptured || !ReferenceEquals(luckPlayerRef, player))
            {
                if (float.TryParse(Convert.ToString(current, CultureInfo.InvariantCulture), NumberStyles.Float,
                    CultureInfo.InvariantCulture, out float original))
                {
                    luckOriginalValue = original;
                    luckOriginalCaptured = true;
                    luckPlayerRef = player;
                }
            }
            // Terraria normalmente limita la suerte a un rango aproximado de -1 a +1.
            SetReflectedMember(player, "luck", 1f);
        }
        catch { }
    }

    private static void RestoreLuckValue()
    {
        if (!luckOriginalCaptured || luckPlayerRef == null) return;
        try { SetReflectedMember(luckPlayerRef, "luck", luckOriginalValue); }
        catch { }
        luckOriginalCaptured = false;
        luckPlayerRef = null;
    }

    private static string GetLuckCounterText()
    {
        try
        {
            Player player = Main.LocalPlayer;
            if (player == null || !player.active) return "Suerte actual: no disponible";
            object value = GetReflectedMember(player, "luck");
            if (value == null) return "Suerte actual: API no disponible";
            float luck = Convert.ToSingle(value, CultureInfo.InvariantCulture);
            string mode = luckBoostEnabled ? "MAX ACTIVADA" : "normal";
            return $"SUERTE ACTUAL: {luck:+0.000;-0.000;0.000}  |  Modo: {mode}";
        }
        catch
        {
            return "Suerte actual: no disponible en esta versión";
        }
    }

    private static void SpawnPanelNpc(int id)
    {
        if (IsServer)
        {
            SendCmd($"/spawnmob {id} 1");
            SetStatus("Solicitud de invocación enviada al servidor.");
            return;
        }
        Player player = Main.LocalPlayer;
        int index = NPC.NewNPC(new PanelEntitySource(), (int)player.Center.X, (int)player.Center.Y, id);
        SetStatus(index >= 0 && index < Main.npc.Length
            ? $"Invocación solicitada: {Lang.GetNPCNameValue(id)}."
            : "No se pudo invocar el NPC.");
    }

    private static void RemovePanelHostileNpcs()
    {
        if (IsServer)
        {
            SendCmd("/butcher");
            SetStatus("Comando /butcher enviado al servidor.");
            return;
        }
        int removed = 0;
        foreach (NPC npc in Main.npc)
        {
            if (npc != null && npc.active && !npc.friendly && !npc.townNPC && npc.lifeMax > 5)
            {
                npc.active = false;
                removed++;
            }
        }
        SetStatus($"NPC hostiles desactivados localmente: {removed}.");
    }

    private static void DrawProjectilesTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, "Monitor de proyectiles");
        int active = 0;
        foreach (Projectile projectile in Main.projectile)
            if (projectile != null && projectile.active) active++;
        Utils.DrawBorderString(sb, $"Proyectiles activos: {active}", new Vector2(panel.X + Pad, y), Color.LightGray, 0.78f);
        y += 30;
        int bw = (Width - Pad * 2 - 8) / 2;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, RowH), "Eliminar todos (local)", mouse, new Color(145, 50, 45)))
        {
            if (IsServer)
                SetStatus("La limpieza de proyectiles requiere soporte del servidor; no se envió un comando inventado.");
            else
            {
                int removed = 0;
                foreach (Projectile projectile in Main.projectile)
                    if (projectile != null && projectile.active) { projectile.active = false; removed++; }
                SetStatus($"Proyectiles desactivados localmente: {removed}.");
            }
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + 8, y, bw, RowH), "Solo no amistosos", mouse, new Color(120, 70, 45)))
        {
            if (IsServer)
                SetStatus("Esta acción requiere soporte del servidor.");
            else
            {
                int removed = 0;
                foreach (Projectile projectile in Main.projectile)
                    if (projectile != null && projectile.active && !projectile.friendly) { projectile.active = false; removed++; }
                SetStatus($"Proyectiles no amistosos desactivados localmente: {removed}.");
            }
        }
        y += RowH + 14;
        DrawSectionHeader(sb, panel, ref y, "Proyectiles activos (hasta 12)");
        int shown = 0;
        for (int i = 0; i < Main.projectile.Length && shown < 12; i++)
        {
            Projectile projectile = Main.projectile[i];
            if (projectile == null || !projectile.active) continue;
            if (y + 22 > panel.Bottom - 40) break;
            Utils.DrawBorderString(sb, $"#{i} · tipo {projectile.type} · daño {projectile.damage} · dueño {projectile.owner}",
                new Vector2(panel.X + Pad, y + 3), Color.White, 0.64f);
            y += 21;
            shown++;
        }
        if (shown == 0)
            Utils.DrawBorderString(sb, "No hay proyectiles activos.", new Vector2(panel.X + Pad, y), Color.Gray, 0.7f);
        Utils.DrawBorderString(sb, "Las acciones locales no garantizan cambios en servidores remotos.",
            new Vector2(panel.X + Pad, panel.Bottom - 36), new Color(230, 190, 100), 0.58f);
    }

    // ------------------------------------------------------------------ Personalización de la interfaz
    private static void DrawAntiLagTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, "Anti-Lag avanzado (solo cliente)");
        Utils.DrawBorderString(sb,
            "Limpia polvo y puede retirar proyectiles locales cuando hay demasiados.",
            new Vector2(panel.X + Pad, y), Color.LightGray, 0.68f);
        y += 30;

        int bw = (Width - (Pad * 2) - 8) / 2;
        string effectsText = antiEffectsEnabled ? "Anti-efectos: ACTIVADO" : "Anti-efectos: desactivado";
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, RowH), effectsText, mouse,
            antiEffectsEnabled ? new Color(35, 105, 70) : UiTabColor))
        {
            antiEffectsEnabled = !antiEffectsEnabled;
            antiEffectsFrameCounter = 0;
            SetStatus(antiEffectsEnabled
                ? "Anti-efectos activado: limpia polvo y gore local cada 10 actualizaciones."
                : "Anti-efectos desactivado.");
        }

        y += RowH + 8;
        string toggleText = antiLagEnabled ? "Limpieza de polvo: ACTIVADA" : "Limpieza de polvo: desactivada";
        Color toggleColor = antiLagEnabled ? new Color(35, 105, 70) : UiTabColor;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, RowH), toggleText, mouse, toggleColor))
        {
            antiLagEnabled = !antiLagEnabled;
            antiLagFrameCounter = 0;
            SetStatus(antiLagEnabled
                ? "Limpieza de polvo activada: cada 30 actualizaciones."
                : "Limpieza de polvo desactivada.");
        }

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + 8, y, bw, RowH), "Limpiar polvo ahora", mouse, new Color(55, 75, 115)))
        {
            int cleared = ClearAntiLagDust();
            SetStatus($"Partículas de polvo limpiadas: {cleared}.");
        }
        y += RowH + 8;

        string projectileToggle = antiLagProjectilesEnabled
            ? "Control de proyectiles: ACTIVADO"
            : "Control de proyectiles: desactivado";
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, RowH), projectileToggle, mouse,
            antiLagProjectilesEnabled ? new Color(145, 75, 40) : UiTabColor))
        {
            antiLagProjectilesEnabled = !antiLagProjectilesEnabled;
            antiLagProjectileFrameCounter = 0;
            SetStatus(antiLagProjectilesEnabled
                ? $"Control local activado: intentará reducir proyectiles por encima de {antiLagProjectileLimit}."
                : "Control automático de proyectiles desactivado.");
        }

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + 8, y, bw, RowH), "Eliminar proyectiles ahora", mouse, new Color(145, 50, 45)))
        {
            if (IsServer)
                SetStatus("En modo COMANDOS, el servidor controla los proyectiles; no se modificaron.");
            else
            {
                int removed = TrimProjectilesForPerformance(true);
                SetStatus($"Limpieza local: {removed} proyectiles eliminados.");
            }
        }
        y += RowH + 10;

        int activeProjectiles = 0;
        foreach (Projectile projectile in Main.projectile)
            if (projectile != null && projectile.active) activeProjectiles++;
        Utils.DrawBorderString(sb, $"Proyectiles activos: {activeProjectiles} / límite automático {antiLagProjectileLimit}",
            new Vector2(panel.X + Pad, y), activeProjectiles > antiLagProjectileLimit ? new Color(255, 145, 90) : Color.LightGray, 0.72f);
        y += 21;
        Utils.DrawBorderString(sb, $"Proyectiles retirados esta sesión: {antiLagProjectilesRemoved}",
            new Vector2(panel.X + Pad, y), Color.LightGray, 0.72f);
        y += 23;
        Utils.DrawBorderString(sb, $"Polvo limpiado: {antiLagParticlesCleared} · Gore limpiado: {antiLagGoreCleared}",
            new Vector2(panel.X + Pad, y), Color.LightGray, 0.68f);
        y += 22;
        Utils.DrawBorderString(sb, $"Intervenciones de emergencia: {antiLagEmergencyInterventions}",
            new Vector2(panel.X + Pad, y), Color.LightGray, 0.68f);
        y += 24;

        Utils.DrawBorderString(sb, "Límite de proyectiles:", new Vector2(panel.X + Pad, y), Color.White, 0.7f);
        y += 20;
        int[] limits = { 100, 150, 250, 400 };
        int lw = (Width - Pad * 2 - 18) / 4;
        for (int i = 0; i < limits.Length; i++)
        {
            int limit = limits[i];
            Rectangle limitRect = new Rectangle(panel.X + Pad + i * (lw + 6), y, lw, 24);
            if (DrawButton(sb, pixel, limitRect, limit.ToString(), mouse,
                antiLagProjectileLimit == limit ? UiAccent : UiTabColor))
            {
                antiLagProjectileLimit = limit;
                SetStatus($"Límite Anti-Lag ajustado a {limit} proyectiles.");
            }
        }
        y += 31;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, RowH), "Limpiar polvo + gore", mouse, new Color(55, 75, 115)))
        {
            int dust = ClearAntiLagDust();
            int gore = ClearAntiLagGore();
            SetStatus($"Limpieza visual: {dust} partículas y {gore} efectos gore.");
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + 8, y, bw, RowH), "Modo emergencia", mouse, new Color(145, 50, 45)))
        {
            int dust = ClearAntiLagDust();
            int gore = ClearAntiLagGore();
            int removed = TrimProjectilesForPerformance(false, Math.Max(40, antiLagProjectileLimit / 2));
            antiLagEmergencyInterventions++;
            SetStatus($"Emergencia: retirados {removed} proyectiles, {dust} partículas y {gore} efectos.");
        }
        y += RowH + 8;
        Utils.DrawBorderString(sb,
            "Solo cliente: esto limita y limpia entidades ya creadas; no puede garantizar que un servidor/mod no vuelva a generarlas.",
            new Vector2(panel.X + Pad, y), new Color(235, 190, 120), 0.62f);
        y += 30;
    }

    private static void DrawFPSBoostTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, "FPS Boost · estabilizador adaptativo");
        DrawSectionHeader(sb, panel, ref y, "FPS Boost · estabilizador adaptativo");
        Utils.DrawBorderString(sb,
            "Actúa cuando detecta una caída de FPS; no cambia archivos ni ajustes gráficos permanentes.",
            new Vector2(panel.X + Pad, y), Color.LightGray, 0.64f);
        y += 28;

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, Width - Pad * 2, RowH),
            "Anti-sonidos del juego: " + (muteGameSoundsEnabled ? "ACTIVADO" : "desactivado"), mouse,
            muteGameSoundsEnabled ? new Color(145, 75, 40) : UiTabColor))
        {
            if (!muteGameSoundsEnabled)
            {
                soundVolumeBeforeMute = Math.Clamp(Main.soundVolume, 0f, 1f);
                muteGameSoundsEnabled = true;
                Main.soundVolume = 0f;
            }
            else
            {
                muteGameSoundsEnabled = false;
                Main.soundVolume = Math.Clamp(soundVolumeBeforeMute, 0f, 1f);
            }
            SaveSettingsIfChanged(force: true);
            SetStatus(muteGameSoundsEnabled ? "Sonidos del juego silenciados; la música puede tener control separado." : "Sonido del juego restaurado al volumen guardado.");
        }
        y += RowH + 8;

        int bw = (Width - Pad * 2 - 8) / 2;
        string state = fpsStabilizerEnabled ? "Estabilizador automático: ACTIVADO" : "Estabilizador automático: desactivado";
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, RowH), state, mouse,
            fpsStabilizerEnabled ? new Color(35, 105, 70) : UiTabColor))
        {
            fpsStabilizerEnabled = !fpsStabilizerEnabled;
            fpsStabilizerFrameCounter = 0;
            SetStatus(fpsStabilizerEnabled
                ? $"Estabilizador activado: actuará si los FPS bajan de {fpsStabilizerThreshold}."
                : "Estabilizador automático desactivado.");
        }

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + 8, y, bw, RowH), "Aplicar perfil equilibrado", mouse, new Color(45, 85, 135)))
        {
            fpsStabilizerEnabled = true;
            antiLagEnabled = false; // El estabilizador solo limpia cuando detecta una caída.
            antiLagProjectilesEnabled = true;
            fpsStabilizerFrameCounter = 0;
            SetStatus("Perfil equilibrado activado: limpieza adaptativa de efectos y control de proyectiles.");
        }
        y += RowH + 10;

        Utils.DrawBorderString(sb, "Umbral de intervención:", new Vector2(panel.X + Pad, y), Color.White, 0.72f);
        y += 20;
        int[] thresholds = { 30, 45, 60 };
        for (int i = 0; i < thresholds.Length; i++)
        {
            int threshold = thresholds[i];
            int tw = (Width - Pad * 2 - 12) / 3;
            var rect = new Rectangle(panel.X + Pad + i * (tw + 6), y, tw, 25);
            if (DrawButton(sb, pixel, rect, threshold + " FPS", mouse,
                fpsStabilizerThreshold == threshold ? UiAccent : UiTabColor))
            {
                fpsStabilizerThreshold = threshold;
                SetStatus($"Umbral del estabilizador: {threshold} FPS.");
            }
        }
        y += 34;

        int activeProjectiles = 0;
        foreach (Projectile projectile in Main.projectile)
            if (projectile != null && projectile.active) activeProjectiles++;

        Color fpsColor = Main.frameRate < fpsStabilizerThreshold
            ? new Color(255, 145, 90)
            : new Color(125, 225, 155);
        Utils.DrawBorderString(sb, $"FPS actuales: {Main.frameRate}", new Vector2(panel.X + Pad, y), fpsColor, 0.85f);
        y += 23;
        Utils.DrawBorderString(sb, $"Proyectiles activos: {activeProjectiles} / límite {antiLagProjectileLimit}", new Vector2(panel.X + Pad, y), Color.LightGray, 0.72f);
        y += 21;
        Utils.DrawBorderString(sb, $"Polvo: {CountActiveDust()} · Gore: {CountActiveGore()}", new Vector2(panel.X + Pad, y), Color.LightGray, 0.72f);
        y += 21;
        Utils.DrawBorderString(sb, $"Intervenciones adaptativas: {fpsStabilizerInterventions}", new Vector2(panel.X + Pad, y), Color.LightGray, 0.72f);
        y += 25;

        int half = (Width - Pad * 2 - 8) / 2;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, half, RowH), "Limpiar efectos ahora", mouse, new Color(55, 75, 115)))
        {
            int cleared = ClearAntiLagDust();
            SetStatus($"Se limpiaron {cleared} partículas de polvo locales.");
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + half + 8, y, half, RowH), "Control proyectiles: " + (antiLagProjectilesEnabled ? "ON" : "OFF"), mouse,
            antiLagProjectilesEnabled ? new Color(145, 75, 40) : UiTabColor))
        {
            antiLagProjectilesEnabled = !antiLagProjectilesEnabled;
            antiLagProjectileFrameCounter = 0;
            SetStatus(antiLagProjectilesEnabled ? "Control automático de proyectiles activado." : "Control automático de proyectiles desactivado.");
        }
        y += RowH + 9;
        Utils.DrawBorderString(sb,
            "Aviso: si los FPS caen, puede eliminar polvo y proyectiles locales; esto puede borrar ataques visibles. En multijugador el servidor puede volver a enviarlos.",
            new Vector2(panel.X + Pad, y), new Color(235, 190, 120), 0.62f);
        y += 32;
    }

    // Ajustes locales persistentes: se guardan en un archivo de configuración del usuario,
    // no en el mundo/servidor. Se persisten solo opciones explícitas, nunca estados temporales de UI.
    private static readonly string[] PersistentBooleanSettings =
    {
        nameof(uiHighContrast), nameof(godmodeEnabled), nameof(adventureUnrestrictedEnabled), nameof(noSpawnsEnabled),
        nameof(flyEnabled), nameof(infiniteMinionsEnabled), nameof(lifeLockEnabled),
        nameof(defenseOverrideEnabled), nameof(antiLagEnabled), nameof(antiLagProjectilesEnabled),
        nameof(fpsStabilizerEnabled), nameof(muteGameSoundsEnabled),
        nameof(editorAutoReuse), nameof(editorConsumable), nameof(editorFavorite),
        nameof(editorChannel), nameof(editorMelee), nameof(editorMagic), nameof(editorRanged),
        nameof(editorSummon), nameof(editorNoWet), nameof(editorNotAmmo),
        nameof(editorUseTurn), nameof(editorNoMelee), nameof(editorNoUseGraphic)
    };

    private static string SettingsFilePath => Path.Combine(Main.SavePath, "HerosModPanel.settings");

    private static void EnsureSettingsLoaded()
    {
        if (settingsLoaded) return;
        settingsLoaded = true;
        try
        {
            string path = SettingsFilePath;
            if (File.Exists(path))
            {
                foreach (string rawLine in File.ReadAllLines(path))
                {
                    int split = rawLine.IndexOf('=');
                    if (split <= 0) continue;
                    string key = rawLine.Substring(0, split).Trim();
                    string value = rawLine.Substring(split + 1).Trim();
                    if (key == "antiLagProjectileLimit" && int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int limit))
                        antiLagProjectileLimit = limit is 100 or 150 or 250 or 400 ? limit : 250;
                    else if (key == "fpsStabilizerThreshold" && int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int threshold))
                        fpsStabilizerThreshold = threshold is 30 or 45 or 60 ? threshold : 45;
                    else if (key == "editorProjectileMultiplier" && int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int multiplier))
                        editorProjectileMultiplier = Math.Clamp(multiplier, 1, 128);
                    else if (key == "uiTheme" && int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int theme))
                        uiTheme = Math.Clamp(theme, 0, UiAccents.Length - 1);
                    else if (key == "defenseOverride" && int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int defense))
                        defenseOverride = Math.Clamp(defense, 0, 9999);
                    else if (key == "serverMode" && Enum.TryParse(value, true, out ServerMode loadedMode))
                        serverMode = loadedMode;
                    else if (key == "soundVolumeBeforeMute" && float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float volume))
                        soundVolumeBeforeMute = Math.Clamp(volume, 0f, 1f);
                    else if (PersistentBooleanSettings.Contains(key))
                    {
                        FieldInfo field = typeof(HerosModPanel).GetField(key, BindingFlags.Static | BindingFlags.NonPublic);
                        if (field != null && field.FieldType == typeof(bool) && bool.TryParse(value, out bool enabled))
                            field.SetValue(null, enabled);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            statusMessage = "No se pudieron cargar los ajustes: " + ex.Message;
        }
        lastSavedSettingsSnapshot = BuildSettingsSnapshot();
    }

    private static void ApplySoundMuteSetting()
    {
        if (muteGameSoundsEnabled && Main.soundVolume != 0f)
            Main.soundVolume = 0f;
    }

    private static string BuildSettingsSnapshot()
    {
        var lines = new List<string>();
        foreach (string name in PersistentBooleanSettings)
        {
            FieldInfo field = typeof(HerosModPanel).GetField(name, BindingFlags.Static | BindingFlags.NonPublic);
            if (field != null) lines.Add(name + "=" + (bool)field.GetValue(null));
        }
        lines.Add("antiLagProjectileLimit=" + antiLagProjectileLimit.ToString(CultureInfo.InvariantCulture));
        lines.Add("fpsStabilizerThreshold=" + fpsStabilizerThreshold.ToString(CultureInfo.InvariantCulture));
        lines.Add("editorProjectileMultiplier=" + Math.Clamp(editorProjectileMultiplier, 1, 128).ToString(CultureInfo.InvariantCulture));
        lines.Add("uiTheme=" + uiTheme.ToString(CultureInfo.InvariantCulture));
        lines.Add("defenseOverride=" + defenseOverride.ToString(CultureInfo.InvariantCulture));
        lines.Add("serverMode=" + serverMode.ToString());
        lines.Add("soundVolumeBeforeMute=" + soundVolumeBeforeMute.ToString(CultureInfo.InvariantCulture));
        return string.Join("\n", lines);
    }

    private static void SaveSettingsIfChanged(bool force = false)
    {
        if (!settingsLoaded) return;
        string snapshot = BuildSettingsSnapshot();
        if (!force && string.Equals(snapshot, lastSavedSettingsSnapshot, StringComparison.Ordinal)) return;
        try
        {
            string path = SettingsFilePath;
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
            File.WriteAllText(path, snapshot + Environment.NewLine);
            lastSavedSettingsSnapshot = snapshot;
        }
        catch (Exception ex)
        {
            statusMessage = "No se pudieron guardar los ajustes: " + ex.Message;
        }
    }

    private static int CountActiveDust()
    {
        int count = 0;
        for (int i = 0; i < Main.dust.Length; i++)
            if (Main.dust[i].active) count++;
        return count;
    }

    private static int CountActiveGore()
    {
        int count = 0;
        for (int i = 0; i < Main.gore.Length; i++)
            if (Main.gore[i].active) count++;
        return count;
    }

    private static int CountActiveProjectiles()
    {
        int count = 0;
        for (int i = 0; i < Main.projectile.Length; i++)
            if (Main.projectile[i] != null && Main.projectile[i].active) count++;
        return count;
    }

    private static int ClearAntiLagDust()
    {
        int cleared = 0;
        for (int i = 0; i < Main.dust.Length; i++)
        {
            if (Main.dust[i].active)
            {
                Main.dust[i].active = false;
                cleared++;
            }
        }
        antiLagParticlesCleared += cleared;
        return cleared;
    }

    private static int ClearAntiLagGore()
    {
        int cleared = 0;
        for (int i = 0; i < Main.gore.Length; i++)
        {
            if (Main.gore[i].active)
            {
                Main.gore[i].active = false;
                cleared++;
            }
        }
        antiLagGoreCleared += cleared;
        return cleared;
    }

    // Retira entidades locales hasta un objetivo; force=true elimina todos los proyectiles.
    // Prioriza hostiles y no amistosos para preservar ataques amistosos cuando sea posible.
    private static int TrimProjectilesForPerformance(bool force, int targetOverride = -1)
    {
        int active = 0;
        for (int i = 0; i < Main.projectile.Length; i++)
            if (Main.projectile[i] != null && Main.projectile[i].active) active++;

        int target = force ? 0 : (targetOverride >= 0 ? targetOverride : antiLagProjectileLimit);
        if (active <= target) return 0;

        int removed = 0;
        for (int pass = 0; pass < (force ? 1 : 4) && active > target; pass++)
        {
            for (int i = 0; i < Main.projectile.Length && active > target; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile == null || !projectile.active) continue;

                bool eligible = force
                    || (pass == 0 && projectile.hostile)
                    || (pass == 1 && !projectile.friendly)
                    || (pass == 2 && projectile.owner != Main.myPlayer)
                    || pass == 3;

                if (!eligible) continue;
                projectile.active = false;
                active--;
                removed++;
            }
        }

        antiLagProjectilesRemoved += removed;
        return removed;
    }

    private static void DrawCustomizeTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, "Personalizar interfaz");
        Utils.DrawBorderString(sb, "Cambia los colores del panel sin desactivar ni reemplazar ninguna función.",
            new Vector2(panel.X + Pad, y), Color.LightGray, 0.68f);
        y += 28;

        string[] themeNames = { "Azul", "Verde", "Violeta", "Naranja", "Rojo", "Anime Sakura", "Anime Cyan" };
        int gap = 7;
        int bw = (Width - Pad * 2 - gap * (themeNames.Length - 1)) / themeNames.Length;
        for (int i = 0; i < themeNames.Length; i++)
        {
            var rect = new Rectangle(panel.X + Pad + i * (bw + gap), y, bw, 32);
            if (DrawButton(sb, pixel, rect, themeNames[i], mouse, UiAccents[i]))
            {
                uiTheme = i;
                SetStatus($"Tema aplicado: {themeNames[i]}.");
            }
            if (uiTheme == i)
                sb.Draw(pixel, new Rectangle(rect.X, rect.Bottom - 3, rect.Width, 3), Color.White);
        }
        y += 44;

        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, 245, RowH + 4),
            uiHighContrast ? "Contraste alto: ACTIVADO" : "Contraste alto: desactivado", mouse,
            uiHighContrast ? UiAccent : null))
        {
            uiHighContrast = !uiHighContrast;
            SetStatus(uiHighContrast ? "Contraste alto activado." : "Contraste alto desactivado.");
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + 253, y, 245, RowH + 4),
            "Restablecer apariencia", mouse))
        {
            uiTheme = 0;
            uiHighContrast = false;
            SetStatus("Apariencia predeterminada restaurada.");
        }
        y += RowH + 18;

        DrawSectionHeader(sb, panel, ref y, "Vista previa");
        var preview = new Rectangle(panel.X + Pad, y, Width - Pad * 2, 76);
        sb.Draw(pixel, preview, UiPanelColor);
        sb.Draw(pixel, new Rectangle(preview.X, preview.Y, preview.Width, 23), UiTitleColor);
        Utils.DrawBorderString(sb, "HERO's Mod · Vista previa", new Vector2(preview.X + 9, preview.Y + 4), Color.White, 0.75f);
        sb.Draw(pixel, new Rectangle(preview.X + 9, preview.Y + 34, 135, 27), UiAccent);
        Utils.DrawBorderString(sb, "Botón destacado", new Vector2(preview.X + 18, preview.Y + 39), Color.White, 0.68f);
        sb.Draw(pixel, new Rectangle(preview.X + 153, preview.Y + 34, 135, 27), UiTabColor);
        Utils.DrawBorderString(sb, "Botón normal", new Vector2(preview.X + 162, preview.Y + 39), Color.White, 0.68f);
        y += 91;

        Utils.DrawBorderString(sb, "Los cambios son visuales y duran mientras siga abierta esta sesión del juego.",
            new Vector2(panel.X + Pad, y), new Color(235, 195, 120), 0.66f);
        y += 20;
        Utils.DrawBorderString(sb, "Consejo: arrastra la barra superior para mover el panel y usa F8 para abrir/cerrar.",
            new Vector2(panel.X + Pad, y), Color.LightGray, 0.66f);
    }

    // ------------------------------------------------------------------ dibujo comun

    private static void DrawSectionHeader(SpriteBatch sb, Rectangle panel, ref int y, string title)
    {
        string fittedTitle = title ?? "";
        var font = FontAssets.MouseText.Value;
        float maxWidth = Math.Max(20, panel.Width - Pad * 2);
        while (fittedTitle.Length > 1 && font.MeasureString(fittedTitle).X * 0.82f > maxWidth)
            fittedTitle = fittedTitle.Substring(0, fittedTitle.Length - 2) + "…";
        Utils.DrawBorderString(sb, fittedTitle, new Vector2(panel.X + Pad, y), UiAccent, 0.82f);
        int ruleY = y + 19;
        sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(panel.X + Pad, ruleY, Math.Max(1, panel.Width - Pad * 2), 1), UiAccent * 0.38f);
        y += 23;
    }

    private static Color UiAccent => UiAccents[Math.Clamp(uiTheme, 0, UiAccents.Length - 1)];
    // Paleta inspirada en Terraria/Calamity: obsidiana, violeta profundo y cian arcano.
    private static Color UiPanelColor => uiHighContrast ? new Color(8, 15, 25) : new Color(17, 20, 34);
    private static Color UiTitleColor => uiHighContrast ? new Color(12, 42, 58) : new Color(26, 42, 62);
    private static Color UiTabColor => uiHighContrast ? new Color(12, 24, 38) : new Color(28, 31, 48);

    private static bool DrawButton(SpriteBatch sb, Texture2D pixel, Rectangle rect, string text, Point mouse, Color? customBg = null)
    {
        bool hover = rect.Contains(mouse);
        Color baseColor = customBg ?? UiTabColor;
        Color bg = hover ? Color.Lerp(baseColor, UiAccent, 0.48f) : baseColor;
        // Marco doble inspirado en los paneles de Terraria/Calamity: borde oscuro y filo de acento.
        sb.Draw(pixel, rect, new Color(7, 8, 14, 235));
        if (rect.Width > 2 && rect.Height > 2)
            sb.Draw(pixel, new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2), bg);
        if (rect.Width > 4 && rect.Height > 3)
        {
            sb.Draw(pixel, new Rectangle(rect.X + 2, rect.Y + 2, rect.Width - 4, 1), hover ? Color.White * 0.8f : UiAccent * 0.8f);
            sb.Draw(pixel, new Rectangle(rect.X + 2, rect.Bottom - 2, rect.Width - 4, 1), new Color(0, 0, 0, 150));
        }

        var font = FontAssets.MouseText.Value;
        string fittedText = text ?? "";
        float scale = 0.7f;
        float maxTextWidth = Math.Max(8, rect.Width - 10);
        while (fittedText.Length > 1 && font.MeasureString(fittedText).X * scale > maxTextWidth && scale > 0.46f)
            scale -= 0.04f;
        if (font.MeasureString(fittedText).X * scale > maxTextWidth)
        {
            while (fittedText.Length > 1 && font.MeasureString(fittedText + "…").X * scale > maxTextWidth)
                fittedText = fittedText.Substring(0, fittedText.Length - 1);
            fittedText += "…";
        }
        var size = font.MeasureString(fittedText) * scale;
        Vector2 textPos = new Vector2(rect.X + (rect.Width - size.X) / 2f, rect.Y + (rect.Height - size.Y) / 2f + 2f);
        Utils.DrawBorderString(sb, fittedText, textPos, Color.White, scale);

        if (hover && Clicked)
        {
            Main.mouseLeftRelease = false;
            activeField = 0;
            return true;
        }
        return false;
    }

    private static void DrawTextField(SpriteBatch sb, Texture2D pixel, Rectangle rect, int fieldId, string text, Point mouse, string hint = null)
    {
        bool active = activeField == fieldId;
        sb.Draw(pixel, rect, active ? new Color(40, 48, 65) : new Color(20, 22, 28));

        string display = text;
        if (active && (Main.timeForVisualEffects % 60 < 30))
            display += "|";

        if (hint != null && string.IsNullOrEmpty(text) && !active)
            Utils.DrawBorderString(sb, hint, new Vector2(rect.X + 6, rect.Y + 4), Color.Gray, 0.8f);
        else
            Utils.DrawBorderString(sb, display, new Vector2(rect.X + 6, rect.Y + 4), Color.White, 0.8f);

        if (rect.Contains(mouse) && Clicked)
        {
            Main.mouseLeftRelease = false;
            activeField = fieldId;
            Main.clrInput();
        }
    }

    private static void HandleToggleKey()
    {
        if (!Main.drawingPlayerChat && Main.keyState.IsKeyDown(Keys.RightControl) && Main.oldKeyState.IsKeyUp(Keys.RightControl))
        {
            commandConsoleVisible = !commandConsoleVisible;
            if (commandConsoleVisible)
            {
                commandConsolePreviousPanelVisible = Visible;
                activeField = 0;
                Visible = false;
                commandConsoleInput = string.Empty;
                Main.clrInput();
            }
            else
            {
                Visible = commandConsolePreviousPanelVisible;
                Main.clrInput();
            }
        }

        if (commandConsoleVisible || Main.drawingPlayerChat || activeField != 0)
            return;

        bool tilde = Main.keyState.IsKeyDown(Keys.OemTilde) && Main.oldKeyState.IsKeyUp(Keys.OemTilde);
        bool f8 = Main.keyState.IsKeyDown(Keys.F8) && Main.oldKeyState.IsKeyUp(Keys.F8);
        if (tilde || f8)
        {
            Visible = !Visible;
        }
    }

    private static void DrawCommandConsole(SpriteBatch sb, Texture2D pixel)
    {
        int width = Math.Min(760, Main.screenWidth - 32);
        int height = 190;
        int x = Math.Max(16, (Main.screenWidth - width) / 2);
        int y = Math.Max(24, Main.screenHeight - height - 70);
        var panel = new Rectangle(x, y, width, height);
        sb.Draw(pixel, panel, new Color(7, 9, 15, 245));
        sb.Draw(pixel, new Rectangle(x, y, width, 3), new Color(110, 220, 255));
        sb.Draw(pixel, new Rectangle(x + 1, y + 1, width - 2, height - 2), new Color(17, 22, 34, 245));
        Utils.DrawBorderString(sb, "CONSOLA · TERRARIA UNIFIED", new Vector2(x + 12, y + 10), new Color(110, 220, 255), 0.82f);
        Utils.DrawBorderString(sb, "Ctrl derecho: cerrar  ·  Alt derecho/Enter: ejecutar  ·  Esc: cerrar", new Vector2(x + 12, y + 34), Color.LightGray, 0.62f);
        string output = commandConsoleOutput ?? string.Empty;
        var font = FontAssets.MouseText.Value;
        float maxOutputWidth = width - 24;
        var outputLines = new List<string>();
        string currentLine = string.Empty;
        foreach (string word in output.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            string candidate = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
            if (font.MeasureString(candidate).X * 0.62f > maxOutputWidth && !string.IsNullOrEmpty(currentLine))
            {
                outputLines.Add(currentLine);
                currentLine = word;
                if (outputLines.Count >= 3) break;
            }
            else currentLine = candidate;
        }
        if (outputLines.Count < 3 && !string.IsNullOrEmpty(currentLine)) outputLines.Add(currentLine);
        if (outputLines.Count == 3 && output.Length > string.Join(" ", outputLines).Length)
            outputLines[2] = outputLines[2].TrimEnd('.') + " …";
        for (int i = 0; i < outputLines.Count && i < 3; i++)
            Utils.DrawBorderString(sb, outputLines[i], new Vector2(x + 12, y + 59 + i * 18), Color.White, 0.62f);
        var inputRect = new Rectangle(x + 10, y + height - 38, width - 20, 27);
        sb.Draw(pixel, inputRect, new Color(35, 43, 59));
        string input = "> " + commandConsoleInput;
        if (Main.timeForVisualEffects % 60 < 30) input += "|";
        Utils.DrawBorderString(sb, input, new Vector2(inputRect.X + 7, inputRect.Y + 5), Color.White, 0.72f);
        Main.LocalPlayer.mouseInterface = true;
    }

    private static void HandleCommandConsoleTyping()
    {
        PlayerInput.WritingText = true;
        Main.instance.HandleIME();
        commandConsoleInput = Main.GetInputText(commandConsoleInput ?? string.Empty);

        if (Main.inputTextEscape)
        {
            commandConsoleVisible = false;
            Visible = commandConsolePreviousPanelVisible;
            commandConsoleInput = string.Empty;
            Main.clrInput();
            return;
        }

        bool rightAltPressed = Main.keyState.IsKeyDown(Keys.RightAlt) && Main.oldKeyState.IsKeyUp(Keys.RightAlt);
        if (!Main.inputTextEnter && !rightAltPressed)
            return;

        string command = (commandConsoleInput ?? string.Empty).Trim();
        commandConsoleInput = string.Empty;
        Main.clrInput();
        if (command.Length == 0)
            return;

        // Los comandos con punto son funciones locales de HERO; no se envían al servidor.
        if (command.StartsWith(".", StringComparison.Ordinal))
        {
            ExecuteLocalConsoleCommand(command);
            return;
        }

        if (Main.netMode == 1)
        {
            // Los comandos normales usan el canal de chat existente; el servidor valida permisos.
            SendCmd(command.StartsWith("/", StringComparison.Ordinal) ? command : "/" + command);
            commandConsoleOutput = "Enviado al servidor: " + command;
        }
        else
        {
            commandConsoleOutput = "Comandos locales: usa .help. Los comandos / requieren un servidor compatible.";
        }
    }

    private static void ExecuteLocalConsoleCommand(string rawCommand)
    {
        string[] parts = (rawCommand ?? string.Empty).Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string command = parts.Length > 0 ? parts[0].ToLowerInvariant() : string.Empty;
        string argument = parts.Length > 1 ? parts[1].ToLowerInvariant() : string.Empty;
        bool? requestedState = argument switch
        {
            "on" or "1" or "true" or "activar" or "si" or "sí" => true,
            "off" or "0" or "false" or "desactivar" or "no" => false,
            _ => null
        };

        bool Toggle(bool current) => requestedState ?? !current;
        switch (command)
        {
            case ".cheats":
            case ".categorias":
                commandConsoleOutput = "CATEGORÍAS HERO: .category jugador|movimiento|combate|visual|mundo|utilidades|rendimiento|todo. Abre HERO > Categorías para controles gráficos. Los toggles disponibles son .god .fly .ghost .minions .lifelock .luck .antilag .effects .projectiles .fps .mute .shopfree.";
                break;
            case ".category":
            case ".categoria":
                commandConsoleOutput = parts.Length < 2
                    ? "Categorías: jugador, movimiento, combate, visual, mundo, utilidades, rendimiento, registros. Usa .cheats para ver los toggles disponibles."
                    : "Categoría seleccionada para consulta: " + argument + ". Abre la pestaña Categorías para ver sus controles.";
                break;
            case ".help":
            case ".ayuda":
                commandConsoleOutput = ".helpall = lista completa · .toggle <función> [on/off] · .hudpos <posición> · .buff <ID> [segundos] · .log [on/off] · .logall · .logworld · .logcombat · .logsystem · .lognpc · .logperformance · .logitems · .logfile · .logstatus · .logclear · .help2";
                break;
            case ".log":
            case ".eventlog":
                eventLogEnabled = requestedState ?? !eventLogEnabled;
                if (eventLogEnabled) eventLogInitialized = false;
                commandConsoleOutput = "Registro de eventos HERO en chat " + (eventLogEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".logplayers":
                eventLogPlayersEnabled = requestedState ?? !eventLogPlayersEnabled;
                commandConsoleOutput = "Registro de entradas/salidas " + (eventLogPlayersEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".logdeaths":
                eventLogDeathsEnabled = requestedState ?? !eventLogDeathsEnabled;
                commandConsoleOutput = "Registro de muertes " + (eventLogDeathsEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".logbosses":
                eventLogBossesEnabled = requestedState ?? !eventLogBossesEnabled;
                commandConsoleOutput = "Registro de aparición de jefes " + (eventLogBossesEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".logworld":
                eventLogWorldEnabled = requestedState ?? !eventLogWorldEnabled;
                commandConsoleOutput = "Registro de mundo, clima y biomas " + (eventLogWorldEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".logcombat":
                eventLogCombatEnabled = requestedState ?? !eventLogCombatEnabled;
                commandConsoleOutput = "Alertas de combate " + (eventLogCombatEnabled ? "ACTIVADAS." : "DESACTIVADAS.");
                break;
            case ".logsystem":
                eventLogSystemEnabled = requestedState ?? !eventLogSystemEnabled;
                commandConsoleOutput = "Mensajes del sistema " + (eventLogSystemEnabled ? "ACTIVADOS." : "DESACTIVADOS.");
                break;
            case ".logfile":
                eventLogFileEnabled = requestedState ?? !eventLogFileEnabled;
                commandConsoleOutput = "Registro en archivo " + (eventLogFileEnabled ? "ACTIVADO: " + HerosLogDirectory : "DESACTIVADO") + ".";
                break;
            case ".logdebug":
                eventLogDebugEnabled = requestedState ?? !eventLogDebugEnabled;
                commandConsoleOutput = "Diagnóstico interno " + (eventLogDebugEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".lognpc":
                eventLogNPCEnabled = requestedState ?? !eventLogNPCEnabled;
                commandConsoleOutput = "Registro de entidades/NPC " + (eventLogNPCEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".logperformance":
                eventLogPerformanceEnabled = requestedState ?? !eventLogPerformanceEnabled;
                commandConsoleOutput = "Registro de rendimiento " + (eventLogPerformanceEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".logitems":
                eventLogItemsEnabled = requestedState ?? !eventLogItemsEnabled;
                commandConsoleOutput = "Registro de cambios de vida/maná " + (eventLogItemsEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".logclear":
                lock (herosLogSync) herosLogHistory.Clear();
                commandConsoleOutput = "Historial de registros en memoria limpiado. Los archivos diarios se conservaron.";
                break;
            case ".logstatus":
                commandConsoleOutput = "LOG: eventos=" + eventLogEnabled + ", jugadores=" + eventLogPlayersEnabled +
                    ", muertes=" + eventLogDeathsEnabled + ", jefes=" + eventLogBossesEnabled +
                    ", mundo=" + eventLogWorldEnabled + ", combate=" + eventLogCombatEnabled +
                    ", NPC=" + eventLogNPCEnabled + ", rendimiento=" + eventLogPerformanceEnabled +
                    ", archivo=" + eventLogFileEnabled + ".";
                break;
            case ".logall":
                eventLogNPCEnabled = true;
                eventLogPerformanceEnabled = true;

                eventLogPlayersEnabled = true;
                eventLogDeathsEnabled = true;
                eventLogBossesEnabled = true;
                eventLogWorldEnabled = true;
                eventLogCombatEnabled = true;
                eventLogSystemEnabled = true;
                eventLogEnabled = true;
                eventLogInitialized = false;
                commandConsoleOutput = "Registro ampliado activado: jugadores, muertes, jefes, mundo, clima, biomas y alertas de combate.";
                break;
            case ".helpall":
            case ".helpfull":
            case ".comandos":
                commandConsoleOutput = "Ayuda por categorías: .help2 / .help3 · registro: .log .logplayers .logdeaths .logbosses .lognpc .logperformance .logfile · HUD: .hud .hudpos .hudinfo · informes: .report .diagnostics .status · jugador: .god .fly .heal .ghost .buff .debuffclear · mundo: .world .biome .clock .nearby .clearhostiles · datos: .server .ping .pos .life .mana .inventory .buffs .counts .players .equipment · funciones: .helpfeatures";
                break;
            case ".helpinfo":
            case ".helpdatos":
                commandConsoleOutput = "DATOS: .report = informe combinado del mundo/personaje · .diagnostics = rendimiento y entidades · .server = conexión y personaje · .ping = latencia disponible · .world = dificultad/progreso · .biome = biomas · .clock = hora/clima · .inventory = espacios/objeto equipado · .buffs = buffs activos · .counts = FPS y entidades · .players = jugadores visibles · .equipment = armadura/equipo · .status = estado de toggles.";
                break;
            case ".helpfeatures":
            case ".helpfunciones":
                commandConsoleOutput = "TOGGLES: .toggle god|nofall|noknockback|antidebuff|breath|fly|ghost|minions|lifelock|luck|antilag|effects|projectiles|fps|mute|shopfree [on/off] · .hud [on/off] · .hudpos 0-5 · .hudinfo <categoria> [on/off] · .buff <ID> [segundos] · .heal · .debuffclear · .clearhostiles. En multijugador el servidor puede revertir cambios locales.";
                break;
            case ".hudinfo":
                {
                    if (parts.Length < 2)
                    {
                        commandConsoleOutput = "Uso: .hudinfo <conexion|rendimiento|vida|posicion|bioma|velocidad|inventario|conteos> [on/off]. Controla qué datos salen en el HUD.";
                        break;
                    }
                    string category = argument;
                    bool? value = parts.Length > 2 ? parts[2].ToLowerInvariant() switch
                    {
                        "on" or "1" or "true" or "activar" or "si" or "sí" => true,
                        "off" or "0" or "false" or "desactivar" or "no" => false,
                        _ => null
                    } : null;
                    bool known = true;
                    switch (category)
                    {
                        case "conexion": hudShowConnection = value ?? !hudShowConnection; break;
                        case "rendimiento": hudShowPerformance = value ?? !hudShowPerformance; break;
                        case "vida": hudShowVitals = value ?? !hudShowVitals; break;
                        case "posicion": hudShowPositionTime = value ?? !hudShowPositionTime; break;
                        case "bioma": hudShowBiome = value ?? !hudShowBiome; break;
                        case "velocidad": hudShowVelocity = value ?? !hudShowVelocity; break;
                        case "inventario": hudShowInventory = value ?? !hudShowInventory; break;
                        case "conteos": hudShowCounts = value ?? !hudShowCounts; break;
                        default: known = false; break;
                    }
                    commandConsoleOutput = known
                        ? "HUD info " + category + "=" + (category switch
                        {
                            "conexion" => hudShowConnection,
                            "rendimiento" => hudShowPerformance,
                            "vida" => hudShowVitals,
                            "posicion" => hudShowPositionTime,
                            "bioma" => hudShowBiome,
                            "velocidad" => hudShowVelocity,
                            "inventario" => hudShowInventory,
                            "conteos" => hudShowCounts,
                            _ => false
                        } ? "ON" : "OFF") + "."
                        : "Categoría desconocida. Usa conexion, rendimiento, vida, posicion, bioma, velocidad, inventario o conteos.";
                    break;
                }
            case ".report":
            case ".informe":
                {
                    Player rp = Main.LocalPlayer;
                    int activeNpcCount = Main.npc.Count(n => n != null && n.active);
                    int hostileCount = Main.npc.Count(n => n != null && n.active && n.CanBeChasedBy());
                    int itemCount = rp != null && rp.active ? rp.inventory.Count(it => it != null && !it.IsAir) : 0;
                    commandConsoleOutput = $"INFORME | Mundo={Main.worldName} ({Main.maxTilesX}x{Main.maxTilesY}) | Modo={(Main.masterMode ? "Maestro" : Main.expertMode ? "Experto" : "Normal")} | Hardmode={Main.hardMode} | Hora={(Main.dayTime ? "Día" : "Noche")} | Clima={(Main.raining ? "Lluvia" : "despejado")} | Ping={GetPingLabel()} | FPS={Main.frameRate} | NPC={activeNpcCount} enemigos={hostileCount} proyectiles={CountActiveProjectiles()} | Inventario={itemCount} objetos | HUD={(serverHudEnabled ? "ON" : "OFF")}";
                    break;
                }
            case ".diagnostics":
            case ".diag":
                commandConsoleOutput = $"DIAGNÓSTICO | Terraria={Main.versionNumber} | Red={Main.netMode} | Pantalla={Main.screenWidth}x{Main.screenHeight} | FPS={Main.frameRate} | NPC={Main.npc.Count(n => n != null && n.active)} | Proyectiles={CountActiveProjectiles()} | Polvo={CountActiveDust()} | Gore={CountActiveGore()} | Jugadores={Main.player.Count(pl => pl != null && pl.active)} | Ping={GetPingLabel()}";
                break;
            case ".hudpos":
            case ".hudposition":
                {
                    if (parts.Length < 2)
                    {
                        commandConsoleOutput = "Posición actual del HUD: " + serverHudPosition + " (" +
                            new[] { "arriba izq.", "arriba der.", "abajo izq.", "abajo der.", "centro arriba", "centro abajo" }[Math.Clamp(serverHudPosition, 0, 5)] +
                            "). Uso: .hudpos 0-5 o nombre de posición.";
                        break;
                    }
                    string pos = argument.Replace("-", "").Replace("_", "").Replace(" ", "");
                    int position = pos switch
                    {
                        "0" or "arribaizq" or "izquierdaarriba" or "topleft" => 0,
                        "1" or "arribader" or "derechaarriba" or "topright" => 1,
                        "2" or "abajoizq" or "izquierdaabajo" or "bottomleft" => 2,
                        "3" or "abajoder" or "derechaabajo" or "bottomright" => 3,
                        "4" or "centroarriba" or "topcenter" => 4,
                        "5" or "centroabajo" or "bottomcenter" => 5,
                        _ => -1
                    };
                    if (position < 0)
                        commandConsoleOutput = "Posición inválida. Usa .hudpos 0-5; 0=arriba izq., 1=arriba der., 2=abajo izq., 3=abajo der., 4=centro arriba, 5=centro abajo.";
                    else
                    {
                        serverHudPosition = position;
                        serverHudEnabled = true;
                        commandConsoleOutput = "HUD movido a: " + new[] { "arriba izquierda", "arriba derecha", "abajo izquierda", "abajo derecha", "centro arriba", "centro abajo" }[position] + ".";
                    }
                    break;
                }
            case ".toggle":
                {
                    if (parts.Length < 2)
                    {
                        commandConsoleOutput = "Uso: .toggle <god|reach|nofall|noknockback|antidebuff|breath|fly|ghost|minions|lifelock|luck|antilag|effects|projectiles|fps|mute|shopfree> [on/off]";
                        break;
                    }
                    string feature = argument;
                    bool? toggleRequestedState = parts.Length > 2 ? parts[2].ToLowerInvariant() switch
                    {
                        "on" or "1" or "true" or "activar" or "si" or "sí" => true,
                        "off" or "0" or "false" or "desactivar" or "no" => false,
                        _ => null
                    } : null;
                    bool ToggleFeature(bool current) => toggleRequestedState ?? !current;
                    bool blocked = false;
                    switch (feature)
                    {
                        case "reach": extendedReachEnabled = ToggleFeature(extendedReachEnabled); ApplyExtendedReach(); break;
                        case "nofall": noFallDamageEnabled = ToggleFeature(noFallDamageEnabled); break;
                        case "noknockback": noKnockbackEnabled = ToggleFeature(noKnockbackEnabled); break;
                        case "antidebuff": antiDebuffEnabled = ToggleFeature(antiDebuffEnabled); break;
                        case "breath": infiniteBreathEnabled = ToggleFeature(infiniteBreathEnabled); break;
                        case "god":
                            godmodeEnabled = ToggleFeature(godmodeEnabled);
                            if (Main.LocalPlayer != null && Main.LocalPlayer.active)
                            {
                                Main.LocalPlayer.creativeGodMode = godmodeEnabled;
                                Main.LocalPlayer.immune = godmodeEnabled;
                                Main.LocalPlayer.immuneTime = godmodeEnabled ? Math.Max(Main.LocalPlayer.immuneTime, 2) : 0;
                            }
                            break;
                        case "fly":
                            flyEnabled = ToggleFeature(flyEnabled);
                            if (IsServer && flyEnabled)
                            {
                                flyEnabled = false;
                                commandConsoleOutput = "Vuelo bloqueado en modo COMANDOS: el servidor controla el movimiento.";
                                blocked = true;
                            }
                            break;
                        case "ghost":
                            ghostModeEnabled = ToggleFeature(ghostModeEnabled);
                            if (Main.LocalPlayer != null && Main.LocalPlayer.active) Main.LocalPlayer.ghost = ghostModeEnabled;
                            break;
                        case "minions":
                            infiniteMinionsEnabled = ToggleFeature(infiniteMinionsEnabled);
                            if (Main.LocalPlayer != null && Main.LocalPlayer.active && infiniteMinionsEnabled) Main.LocalPlayer.maxMinions = 9999;
                            break;
                        case "lifelock":
                            if (IsServer && toggleRequestedState != false && !lifeLockEnabled)
                            {
                                commandConsoleOutput = "Vida infinita bloqueada en modo COMANDOS porque el servidor controla la vida.";
                                blocked = true;
                                break;
                            }
                            lifeLockEnabled = ToggleFeature(lifeLockEnabled);
                            break;
                        case "luck": luckBoostEnabled = ToggleFeature(luckBoostEnabled); break;
                        case "antilag": antiLagEnabled = ToggleFeature(antiLagEnabled); break;
                        case "effects": antiEffectsEnabled = ToggleFeature(antiEffectsEnabled); break;
                        case "projectiles": antiLagProjectilesEnabled = ToggleFeature(antiLagProjectilesEnabled); break;
                        case "fps": fpsStabilizerEnabled = ToggleFeature(fpsStabilizerEnabled); break;
                        case "mute":
                            muteGameSoundsEnabled = ToggleFeature(muteGameSoundsEnabled);
                            ApplySoundMuteSetting();
                            break;
                        case "shopfree":
                            npcShopFreeEnabled = ToggleFeature(npcShopFreeEnabled);
                            ApplyNpcShopFreeSetting();
                            break;
                        default:
                            commandConsoleOutput = "Función desconocida: " + feature + ". Usa .helpall.";
                            blocked = true;
                            break;
                    }
                    if (!blocked)
                    {
                        bool enabled = feature switch
                        {
                            "god" => godmodeEnabled,
                            "reach" => extendedReachEnabled,
                            "nofall" => noFallDamageEnabled,
                            "noknockback" => noKnockbackEnabled,
                            "antidebuff" => antiDebuffEnabled,
                            "breath" => infiniteBreathEnabled,
                            "fly" => flyEnabled,
                            "ghost" => ghostModeEnabled,
                            "minions" => infiniteMinionsEnabled,
                            "lifelock" => lifeLockEnabled,
                            "luck" => luckBoostEnabled,
                            "antilag" => antiLagEnabled,
                            "effects" => antiEffectsEnabled,
                            "projectiles" => antiLagProjectilesEnabled,
                            "fps" => fpsStabilizerEnabled,
                            "mute" => muteGameSoundsEnabled,
                            "shopfree" => npcShopFreeEnabled,
                            _ => false
                        };
                        commandConsoleOutput = "toggle " + feature + "=" + (enabled ? "ON" : "OFF") + ".";
                    }
                    break;
                }
            case ".killprojs":
            case ".clearprojectiles":
                {
                    int removed = KillOwnedProjectilesLocal();
                    commandConsoleOutput = "Proyectiles propios eliminados localmente: " + removed + ".";
                    break;
                }
            case ".help2":
                commandConsoleOutput = ".god .fly .heal .ghost .minions .lifelock .luck .debuffclear .nearby .players .equipment .help3";
                break;
            case ".god":
                godmodeEnabled = Toggle(godmodeEnabled);
                if (Main.LocalPlayer != null && Main.LocalPlayer.active)
                {
                    Main.LocalPlayer.creativeGodMode = godmodeEnabled;
                    Main.LocalPlayer.immune = godmodeEnabled;
                    Main.LocalPlayer.immuneTime = godmodeEnabled ? Math.Max(Main.LocalPlayer.immuneTime, 2) : 0;
                }
                commandConsoleOutput = "God Mode local " + (godmodeEnabled ? "ACTIVADO" : "DESACTIVADO") +
                    (Main.netMode == 1 ? " · el servidor puede ignorarlo o revertirlo." : ".");
                break;
            case ".fly":
                flyEnabled = Toggle(flyEnabled);
                if (IsServer && flyEnabled)
                {
                    flyEnabled = false;
                    commandConsoleOutput = "Vuelo bloqueado en modo COMANDOS: el servidor controla el movimiento. Usa solo donde esté permitido.";
                }
                else
                    commandConsoleOutput = "Vuelo local " + (flyEnabled ? "ACTIVADO (WASD, Espacio sube, Shift baja)." : "DESACTIVADO.");
                break;
            case ".heal":
            case ".curar":
                if (Main.LocalPlayer != null && Main.LocalPlayer.active)
                {
                    Main.LocalPlayer.statLife = Main.LocalPlayer.statLifeMax2;
                    Main.LocalPlayer.statMana = Main.LocalPlayer.statManaMax2;
                    Main.LocalPlayer.HealEffect(Main.LocalPlayer.statLifeMax2);
                    Main.LocalPlayer.ManaEffect(Main.LocalPlayer.statManaMax2);
                    if (Main.netMode == 1) SyncSelf();
                    commandConsoleOutput = "Vida y maná restaurados localmente; el servidor puede sobrescribirlos.";
                }
                else commandConsoleOutput = "No hay personaje activo.";
                break;
            case ".ghost":
            case ".fantasma":
                ghostModeEnabled = Toggle(ghostModeEnabled);
                if (Main.LocalPlayer != null && Main.LocalPlayer.active)
                    Main.LocalPlayer.ghost = ghostModeEnabled;
                commandConsoleOutput = "Modo fantasma local " + (ghostModeEnabled ? "ACTIVADO." : "DESACTIVADO.") +
                    (Main.netMode == 1 ? " El servidor puede corregirlo." : string.Empty);
                break;
            case ".minions":
                infiniteMinionsEnabled = Toggle(infiniteMinionsEnabled);
                if (Main.LocalPlayer != null && Main.LocalPlayer.active && infiniteMinionsEnabled)
                    Main.LocalPlayer.maxMinions = 9999;
                commandConsoleOutput = "Límite local de minions " + (infiniteMinionsEnabled ? "elevado." : "restauración desactivada.") +
                    (Main.netMode == 1 ? " El servidor puede rechazarlo." : string.Empty);
                break;
            case ".lifelock":
                if (IsServer && requestedState != false)
                {
                    commandConsoleOutput = "Vida infinita bloqueada en modo COMANDOS porque el servidor controla la vida.";
                    break;
                }
                lifeLockEnabled = Toggle(lifeLockEnabled);
                commandConsoleOutput = "Restauración continua de vida/maná " + (lifeLockEnabled ? "ACTIVADA." : "DESACTIVADA.");
                break;
            case ".luck":
                luckBoostEnabled = Toggle(luckBoostEnabled);
                commandConsoleOutput = "Aumento de suerte local " + (luckBoostEnabled ? "ACTIVADO." : "DESACTIVADO.") +
                    (Main.netMode == 1 ? " Puede ser sobrescrito por el servidor." : string.Empty);
                break;
            case ".antilag":
                antiLagEnabled = Toggle(antiLagEnabled);
                commandConsoleOutput = "Anti-Lag " + (antiLagEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".effects":
            case ".antieffects":
                antiEffectsEnabled = Toggle(antiEffectsEnabled);
                commandConsoleOutput = "Anti-efectos " + (antiEffectsEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".projectiles":
            case ".antiprojectiles":
                antiLagProjectilesEnabled = Toggle(antiLagProjectilesEnabled);
                commandConsoleOutput = "Control de proyectiles " + (antiLagProjectilesEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".fps":
                fpsStabilizerEnabled = Toggle(fpsStabilizerEnabled);
                commandConsoleOutput = "Estabilizador FPS " + (fpsStabilizerEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".mute":
                muteGameSoundsEnabled = Toggle(muteGameSoundsEnabled);
                ApplySoundMuteSetting();
                commandConsoleOutput = "Silencio del juego " + (muteGameSoundsEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".shopfree":
                npcShopFreeEnabled = Toggle(npcShopFreeEnabled);
                ApplyNpcShopFreeSetting();
                commandConsoleOutput = "Precios gratis de tiendas NPC " + (npcShopFreeEnabled ? "ACTIVADOS localmente." : "DESACTIVADOS.");
                break;
            case ".buff":
                if (parts.Length < 2 || !int.TryParse(parts[1], out int buffId) || buffId < 1 || buffId >= BuffID.Count)
                {
                    commandConsoleOutput = "Uso: .buff ID [segundos] · ID válido: 1-" + (BuffID.Count - 1);
                    break;
                }
                int seconds = 600;
                if (parts.Length > 2 && (!int.TryParse(parts[2], out seconds) || seconds < 1))
                {
                    commandConsoleOutput = "Duración inválida. Usa segundos positivos, por ejemplo .buff  buffID 600.";
                    break;
                }
                seconds = Math.Min(seconds, 36000);
                Main.LocalPlayer.AddBuff(buffId, seconds * 60);
                commandConsoleOutput = $"Buff local aplicado: ID {buffId}, {seconds} s." + (Main.netMode == 1 ? " El servidor puede quitarlo." : string.Empty);
                break;
            case ".clearhostiles":
                if (Main.netMode == 2)
                {
                    commandConsoleOutput = "Esta limpieza requiere un cliente con mundo cargado.";
                    break;
                }
                RemovePanelHostileNpcs();
                commandConsoleOutput = "Limpieza local de enemigos solicitada; el servidor puede restaurarlos.";
                break;
            case ".help3":
                commandConsoleOutput = ".counts = partículas/proyectiles/NPCs · .nearby = enemigos cercanos · .players = jugadores locales · .inventory = resumen inventario · .buffs = buffs activos · .debuffclear = quitar debuffs locales";
                break;
            case ".world":
                commandConsoleOutput = $"Mundo={Main.worldName} · Tamaño={Main.maxTilesX}x{Main.maxTilesY} tiles · Modo={(Main.masterMode ? "Maestro" : Main.expertMode ? "Experto" : "Normal")} · Hardmode={(Main.hardMode ? "Sí" : "No")} · Invasión={Main.invasionType}";
                break;
            case ".biome":
                {
                    var p = Main.LocalPlayer;
                    if (p == null || !p.active) { commandConsoleOutput = "No hay personaje activo."; break; }
                    var biomes = new List<string>();
                    if (p.ZoneDesert) biomes.Add("Desierto");
                    if (p.ZoneJungle) biomes.Add("Jungla");
                    if (p.ZoneSnow) biomes.Add("Nieve");
                    if (p.ZoneBeach) biomes.Add("Playa");
                    if (p.ZoneDungeon) biomes.Add("Mazmorra");
                    if (p.ZoneUnderworldHeight) biomes.Add("Inframundo");
                    if (p.ZoneCorrupt) biomes.Add("Corrupción");
                    if (p.ZoneCrimson) biomes.Add("Carmesí");
                    if (p.ZoneHallow) biomes.Add("Sagrado");
                    if (p.ZoneGlowshroom) biomes.Add("Hongos brillantes");
                    if (p.ZoneSkyHeight) biomes.Add("Cielo");
                    commandConsoleOutput = "Biomas: " + (biomes.Count == 0 ? "Superficie/subsuelo u otro" : string.Join(", ", biomes));
                    break;
                }
            case ".clock":
                commandConsoleOutput = $"Hora del mundo: {(Main.dayTime ? "Día" : "Noche")} · tick={(long)Main.time} · lluvia={(Main.raining ? "Sí" : "No")} · viento={Main.windSpeedCurrent:0.00}";
                break;
            case ".inventory":
                {
                    var p = Main.LocalPlayer;
                    if (p == null || !p.active) { commandConsoleOutput = "No hay personaje activo."; break; }
                    int occupied = 0, empty = 0, stacks = 0;
                    foreach (var item in p.inventory)
                    {
                        if (item != null && !item.IsAir) { occupied++; stacks += Math.Max(1, item.stack); }
                        else empty++;
                    }
                    var held = p.HeldItem;
                    commandConsoleOutput = $"Inventario: {occupied} espacios ocupados, {empty} vacíos, {stacks} objetos sumando stacks · equipado={(held == null || held.IsAir ? "mano vacía" : held.Name + " x" + held.stack)}";
                    break;
                }
            case ".buffs":
                {
                    var p = Main.LocalPlayer;
                    if (p == null || !p.active) { commandConsoleOutput = "No hay personaje activo."; break; }
                    var activeBuffs = new List<string>();
                    for (int i = 0; i < p.buffType.Length; i++)
                        if (p.buffType[i] > 0 && p.buffTime[i] > 0) activeBuffs.Add(p.buffType[i] + "(" + (p.buffTime[i] / 60) + "s)");
                    commandConsoleOutput = activeBuffs.Count == 0 ? "No tienes buffs activos." : "Buffs activos (ID y segundos): " + string.Join(", ", activeBuffs.Take(12)) + (activeBuffs.Count > 12 ? $" … +{activeBuffs.Count - 12}" : "");
                    break;
                }
            case ".debuffclear":
                {
                    var p = Main.LocalPlayer;
                    if (p == null || !p.active) { commandConsoleOutput = "No hay personaje activo."; break; }
                    int removed = 0;
                    for (int i = p.buffType.Length - 1; i >= 0; i--)
                    {
                        int id = p.buffType[i];
                        if (id > 0 && id < Main.debuff.Length && Main.debuff[id]) { p.DelBuff(i); removed++; }
                    }
                    if (Main.netMode == 1) SyncSelf();
                    commandConsoleOutput = $"Debuffs locales eliminados: {removed}. El servidor puede volver a aplicarlos.";
                    break;
                }
            case ".nearby":
                {
                    var p = Main.LocalPlayer;
                    if (p == null || !p.active) { commandConsoleOutput = "No hay personaje activo."; break; }
                    int nearby = 0, bosses = 0;
                    foreach (var npc in Main.npc)
                    {
                        if (npc == null || !npc.active || npc.friendly || npc.lifeMax <= 5) continue;
                        if (Vector2.DistanceSquared(npc.Center, p.Center) <= 60f * 16f * 60f * 16f)
                        {
                            nearby++;
                            if (npc.boss) bosses++;
                        }
                    }
                    commandConsoleOutput = $"Enemigos a 60 tiles: {nearby} · jefes cercanos: {bosses}.";
                    break;
                }
            case ".players":
                {
                    var online = Main.player.Where(p => p != null && p.active).Select(p => p.name).ToArray();
                    commandConsoleOutput = "Jugadores visibles para este cliente (" + online.Length + "): " + (online.Length == 0 ? "ninguno" : string.Join(", ", online.Take(8))) + (online.Length > 8 ? $" … +{online.Length - 8}" : "");
                    break;
                }
            case ".equipment":
                {
                    var p = Main.LocalPlayer;
                    if (p == null || !p.active) { commandConsoleOutput = "No hay personaje activo."; break; }
                    var worn = new List<string>();
                    foreach (var item in p.armor)
                        if (item != null && !item.IsAir) worn.Add(item.Name + " x" + item.stack);
                    commandConsoleOutput = worn.Count == 0 ? "Equipo/armadura vacíos." : "Equipo: " + string.Join(", ", worn.Take(8)) + (worn.Count > 8 ? $" … +{worn.Count - 8}" : "");
                    break;
                }
            case ".counts":
                {
                    int activeNpcs = Main.npc.Count(n => n != null && n.active);
                    int activePlayers = Main.player.Count(p => p != null && p.active);
                    commandConsoleOutput = $"FPS={Main.frameRate} · proyectiles={CountActiveProjectiles()} · polvo={CountActiveDust()} · gore={CountActiveGore()} · NPCs={activeNpcs} · jugadores={activePlayers}";
                    break;
                }
            case ".hud":
            case ".serverhud":
                serverHudEnabled = requestedState ?? !serverHudEnabled;
                commandConsoleOutput = "HUD superior " + (serverHudEnabled ? "ACTIVADO." : "DESACTIVADO.");
                break;
            case ".server":
            case ".info":
                commandConsoleOutput = GetServerInfoSummary();
                break;
            case ".ping":
                commandConsoleOutput = "Ping: " + GetPingLabel() + " · la versión del juego puede no exponer la latencia al cliente.";
                break;
            case ".pos":
                {
                    var p = Main.LocalPlayer;
                    commandConsoleOutput = p != null && p.active ? $"Posición X={(int)(p.position.X / 16f)} Y={(int)(p.position.Y / 16f)} · velocidad X={p.velocity.X:0.0} Y={p.velocity.Y:0.0}" : "No hay personaje activo.";
                    break;
                }
            case ".life":
                commandConsoleOutput = Main.LocalPlayer != null && Main.LocalPlayer.active ? $"Vida {Main.LocalPlayer.statLife}/{Main.LocalPlayer.statLifeMax2} · Defensa {Main.LocalPlayer.statDefense}" : "No hay personaje activo.";
                break;
            case ".mana":
                commandConsoleOutput = Main.LocalPlayer != null && Main.LocalPlayer.active ? $"Maná {Main.LocalPlayer.statMana}/{Main.LocalPlayer.statManaMax2}" : "No hay personaje activo.";
                break;
            case ".status":
                commandConsoleOutput = $"hud={(serverHudEnabled ? "ON" : "OFF")} god={(godmodeEnabled ? "ON" : "OFF")} fly={(flyEnabled ? "ON" : "OFF")} ghost={(ghostModeEnabled ? "ON" : "OFF")} minions={(infiniteMinionsEnabled ? "ON" : "OFF")} lifelock={(lifeLockEnabled ? "ON" : "OFF")} luck={(luckBoostEnabled ? "ON" : "OFF")} antilag={(antiLagEnabled ? "ON" : "OFF")} effects={(antiEffectsEnabled ? "ON" : "OFF")} projectiles={(antiLagProjectilesEnabled ? "ON" : "OFF")} fps={(fpsStabilizerEnabled ? "ON" : "OFF")} mute={(muteGameSoundsEnabled ? "ON" : "OFF")} shopfree={(npcShopFreeEnabled ? "ON" : "OFF")}";
                break;
            default:
                commandConsoleOutput = "Comando local desconocido: " + command + " · escribe .help";
                break;
        }
    }

    private static void HandleTyping()
    {
        PlayerInput.WritingText = true;
        Main.instance.HandleIME();

        if (activeField == 1)
            itemQuery = Main.GetInputText(itemQuery);
        else if (activeField == 2)
            itemAmount = Main.GetInputText(itemAmount);
        else if (activeField == 3)
            itemPrefix = Main.GetInputText(itemPrefix);
        else if (activeField == 4)
            moderationTarget = Main.GetInputText(moderationTarget);
        else if (activeField == 5)
            moderationReason = Main.GetInputText(moderationReason);
        else if (activeField == 6)
            editorSlot = Main.GetInputText(editorSlot);
        else if (activeField == 7)
            editorDamage = Main.GetInputText(editorDamage);
        else if (activeField == 50)
            toolSearchQuery = Main.GetInputText(toolSearchQuery ?? string.Empty);
        else if (activeField == 8)
            editorDefense = Main.GetInputText(editorDefense);
        else if (activeField == 9)
            editorUseTime = Main.GetInputText(editorUseTime);
        else if (activeField == 10)
            editorStack = Main.GetInputText(editorStack);
        else if (activeField == 11)
            editorValue = Main.GetInputText(editorValue);
        else if (activeField == 12)
            worldTileType = Main.GetInputText(worldTileType);
        else if (activeField == 13)
            worldReplaceType = Main.GetInputText(worldReplaceType);
        else if (activeField == 14)
            editorType = Main.GetInputText(editorType);
        else if (activeField == 15)
            editorPrefix = Main.GetInputText(editorPrefix);
        else if (activeField == 16)
            lifeCurrent = Main.GetInputText(lifeCurrent);
        else if (activeField == 17)
            lifeMaximum = Main.GetInputText(lifeMaximum);
        else if (activeField == 18)
            manaCurrent = Main.GetInputText(manaCurrent);
        else if (activeField == 19)
            manaMaximum = Main.GetInputText(manaMaximum);
        else if (activeField == 39)
            lifeDefense = Main.GetInputText(lifeDefense);
        else if (activeField == 20)
            editorKnockback = Main.GetInputText(editorKnockback);
        else if (activeField == 21)
            editorCrit = Main.GetInputText(editorCrit);
        else if (activeField == 22)
            editorRarity = Main.GetInputText(editorRarity);
        else if (activeField == 23)
            editorUseAnimation = Main.GetInputText(editorUseAnimation);
        else if (activeField == 24)
            editorShootSpeed = Main.GetInputText(editorShootSpeed);
        else if (activeField == 25)
            editorProjectile = Main.GetInputText(editorProjectile);
        else if (activeField == 35)
            editorMana = Main.GetInputText(editorMana);
        else if (activeField == 36)
            editorUseStyle = Main.GetInputText(editorUseStyle);
        else if (activeField == 37)
            editorAmmo = Main.GetInputText(editorAmmo);
        else if (activeField == 38)
            editorScale = Main.GetInputText(editorScale);
        else if (activeField == 40)
            editorPick = Main.GetInputText(editorPick);
        else if (activeField == 41)
            editorAxe = Main.GetInputText(editorAxe);
        else if (activeField == 42)
            editorHealLife = Main.GetInputText(editorHealLife);
        else if (activeField == 43)
            editorHealMana = Main.GetInputText(editorHealMana);
        else if (activeField == 44)
            editorUseAmmo = Main.GetInputText(editorUseAmmo);
        else if (activeField == 45)
            editorMountType = Main.GetInputText(editorMountType);
        else if (activeField == 46)
            editorCreateTile = Main.GetInputText(editorCreateTile);
        else if (activeField == 47)
            editorCreateWall = Main.GetInputText(editorCreateWall);
        else if (activeField == 48)
            editorBuffType = Main.GetInputText(editorBuffType);
        else if (activeField == 49)
            editorBuffTime = Main.GetInputText(editorBuffTime);
        else if (activeField == 50)
            editorFishingPole = Main.GetInputText(editorFishingPole);
        else if (activeField == 51)
            editorBait = Main.GetInputText(editorBait);
        else if (activeField == 52)
            editorMakeNpc = Main.GetInputText(editorMakeNpc);
        else if (activeField == 53)
            editorTileWand = Main.GetInputText(editorTileWand);
        else if (activeField == 26)
            schematicFileName = Main.GetInputText(schematicFileName);
        else if (activeField == 26)
            schematicFileName = Main.GetInputText(schematicFileName);
        else if (activeField == 26)
            teleportX = Main.GetInputText(teleportX);
        else if (activeField == 27)
            teleportY = Main.GetInputText(teleportY);
        else if (activeField == 31)
            profileNameInput = Main.GetInputText(profileNameInput);
        else if (activeField == 32)
            bookmarkXInput = Main.GetInputText(bookmarkXInput);
        else if (activeField == 33)
            bookmarkYInput = Main.GetInputText(bookmarkYInput);
        else if (activeField == 34)
            bookmarkNameInput = Main.GetInputText(bookmarkNameInput);

        if (Main.inputTextEnter || Main.inputTextEscape)
        {
            activeField = 0;
            Main.clrInput();
        }
    }

    // ------------------------------------------------------------------ servidor

    private static string Quote(string name)
    {
        return "\"" + (name ?? "").Replace("\"", "") + "\"";
    }

    private static void SendCmd(string command)
    {
        if (Main.netMode != 1)
        {
            SetStatus("Sin conexiÃ³n a un servidor.");
            return;
        }

        ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage(command));
        SetStatus($"Comando enviado: {command}");
    }

    /// <summary>Accion solo-cliente bloqueada en modo Comandos (para respetar al servidor).</summary>
    private static void NotInServer(string what)
    {
        SetStatus($"{what}: solo en un jugador o en modo DIRECTO (arriba).");
    }

    /// <summary>Accion que en servidor solo existe como comando de TShock.</summary>
    private static void NeedsCommands(string what)
    {
        SetStatus($"{what}: solo con TShock. Cambia a modo COMANDOS (arriba).");
    }

    /// <summary>Hora/clima: un cliente no puede cambiarlos en un servidor sin comandos.</summary>
    private static void NoRemoteWorld(string what)
    {
        SetStatus($"{what}: un servidor vanilla no lo permite desde el cliente. Usa TShock (modo COMANDOS) o el host.");
    }

    private static void ToggleServerMode()
    {
        if (!InMultiplayer)
        {
            SetStatus("El modo de servidor se usa al entrar a un servidor.");
            return;
        }

        if (serverMode == ServerMode.Comandos)
        {
            serverMode = ServerMode.Directo;
            SetStatus("Modo DIRECTO: acciones del cliente. Con anticheat (TShock) pueden kickearte; Ãºsalo donde estÃ© permitido.");
        }
        else
        {
            serverMode = ServerMode.Comandos;
            SetStatus("Modo COMANDOS: todo va por comandos de TShock.");
        }
    }

    /// <summary>Sincroniza vida, mana y buffs propios con el servidor (solo multijugador).</summary>
    private static void SyncSelf()
    {
        if (Main.netMode != 1)
            return;

        NetMessage.SendData(MessageID.PlayerLifeMana, -1, -1, null, Main.myPlayer);
        NetMessage.SendData(MessageID.PlayerBuffs, -1, -1, null, Main.myPlayer);
    }

    // ------------------------------------------------------------------ acciones

    private static int ResolveItemId(string query)
    {
        int id = 0;

        if (int.TryParse(query, out int parsedId))
        {
            id = parsedId;
        }
        else if (ItemID.Search.TryGetId(query, out int searchId))
        {
            id = searchId;
        }
        else
        {
            for (int i = 1; i < ItemID.Count; i++)
            {
                string name = Lang.GetItemNameValue(i);
                if (name.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    id = i;
                    break;
                }
            }
        }

        return id;
    }

    private static void SpawnItemAction()
    {
        if (string.IsNullOrWhiteSpace(itemQuery)) return;

        int id = ResolveItemId(itemQuery);

        if (id > 0 && id < ItemID.Count)
        {
            GiveItemById(id);
        }
        else
        {
            SetStatus($"No se encontrÃ³ el objeto: '{itemQuery}'");
        }
    }

    private static void GiveItemById(int id, int? forcedCount = null, int? forcedPrefix = null)
    {
        if (id <= 0 || id >= ItemID.Count)
            return;

        int count = forcedCount.HasValue
            ? Math.Clamp(forcedCount.Value, 1, 9999)
            : (int.TryParse(itemAmount, out int parsedCount) && parsedCount > 0 ? Math.Min(parsedCount, 9999) : 1);
        int prefixValue = forcedPrefix ?? (int.TryParse(itemPrefix, out int parsedPrefix) ? parsedPrefix : -1);
        Player player = Main.LocalPlayer;

        // Entrega directamente al inventario; no requiere escribir /item.
        // En multijugador se envía la sincronización estándar del inventario del jugador.
        Item template = new Item();
        template.SetDefaults(id);
        if (prefixValue > 0)
            template.Prefix(prefixValue);

        int remaining = count;
        int changedSlots = 0;
        int inventoryLimit = Math.Min(50, player.inventory.Length);

        // Primero completar pilas existentes del mismo objeto y prefijo.
        for (int slot = 0; slot < inventoryLimit && remaining > 0; slot++)
        {
            Item current = player.inventory[slot];
            if (current == null || current.IsAir || current.type != template.type || current.prefix != template.prefix)
                continue;

            int room = Math.Max(0, current.maxStack - current.stack);
            if (room == 0)
                continue;

            int added = Math.Min(room, remaining);
            current.stack += added;
            remaining -= added;
            changedSlots++;
            SyncInventorySlot(player, slot, current);
        }

        // Después usar espacios vacíos del inventario principal (sin tocar monedas/munición).
        for (int slot = 0; slot < inventoryLimit && remaining > 0; slot++)
        {
            Item current = player.inventory[slot];
            if (current != null && !current.IsAir)
                continue;

            Item created = template.Clone();
            int added = Math.Min(Math.Max(1, created.maxStack), remaining);
            created.stack = added;
            player.inventory[slot] = created;
            remaining -= added;
            changedSlots++;
            SyncInventorySlot(player, slot, created);
        }

        if (Main.netMode == 0)
        {
            SetStatus(remaining == 0
                ? $"Objeto añadido al inventario: {Lang.GetItemNameValue(id)} (x{count})."
                : $"Inventario casi lleno: se añadieron {count - remaining}/{count} de {Lang.GetItemNameValue(id)}.");
        }
        else
        {
            SetStatus(remaining == 0
                ? $"Entrega enviada al inventario: {Lang.GetItemNameValue(id)} (x{count}). El servidor puede rechazar cambios directos."
                : $"Inventario lleno: se enviaron {count - remaining}/{count} de {Lang.GetItemNameValue(id)}. El servidor puede rechazar cambios directos.");
        }
    }

    private static void SyncInventorySlot(Player player, int slot, Item item)
    {
        if (Main.netMode != 1 || player == null || item == null)
            return;

        NetMessage.SendData(MessageID.SyncEquipment, -1, -1, null,
            player.whoAmI, slot, item.type, item.prefix, item.stack);
    }

    private static void SpawnBoss(int npcId, string bossName, string tshockName)
    {
        // Validación defensiva: una invocación inválida no debe tumbar el cliente.
        if (npcId <= 0)
        {
            SetStatus($"No se puede invocar {bossName}: ID de NPC inválido.");
            return;
        }

        // En un jugador, usar la API vanilla con protección ante IDs no disponibles.
        if (Main.netMode == 0)
        {
            try
            {
                NPC.SpawnOnPlayer(Main.myPlayer, npcId);
                SetStatus($"Solicitud de invocación enviada: {bossName}. Si no aparece, revisa la versión/mods.");
            }
            catch (Exception ex)
            {
                SetStatus($"No se pudo invocar {bossName}: {ex.GetType().Name}. El juego continúa.");
            }
            return;
        }

        // Si el servidor usa TShock y el panel está en modo COMANDOS, pedirle
        // al servidor que cree el jefe. /spawnmob es ejecutado por el servidor,
        // que conserva la autoridad de NPC y sincroniza la entidad a los clientes.
        if (IsServer)
        {
            // Preferir /spawnboss para los jefes que TShock reconoce por alias;
            // para jefes/eventos no cubiertos, intentar /spawnmob con el ID vanilla.
            string bossAlias = npcId switch
            {
                50 => "king slime",
                4 => "eye of cthulhu",
                13 => "eater of worlds",
                266 => "brain of cthulhu",
                35 => "skeletron",
                222 => "queen bee",
                113 => "wall of flesh",
                657 => "queen slime",
                125 => "the twins",
                134 => "the destroyer",
                127 => "skeletron prime",
                262 => "plantera",
                245 => "golem",
                370 => "duke fishron",
                439 => "lunatic cultist",
                398 => "moon lord",
                _ => ""
            };

            string command = !string.IsNullOrEmpty(bossAlias)
                ? $"/spawnboss {bossAlias} 1"
                : $"/spawnmob {npcId} 1";
            SendCmd(command);
            SetStatus($"Solicitud enviada al servidor: {command}. Si no aparece, comprueba el permiso tshock.npc.spawnboss o tshock.npc.spawnmob.");
            return;
        }

        // En servidor vanilla/anárquico sin plugins no se puede forzar la aparición
        // desde el cliente. Como alternativa, entregar el objeto invocador vanilla.
        int summonItemId = npcId switch
        {
            50 => 560,  // Slime Crown
            4 => 43,   // Suspicious Looking Eye
            13 => 70,   // Worm Food
            266 => 1331, // Bloody Spine
            222 => 1133, // Abeemination
            657 => 4988, // Gelatin Crystal
            125 => 544,  // Mechanical Eye
            134 => 556,  // Mechanical Worm
            127 => 557,  // Mechanical Skull
            398 => 3601, // Celestial Sigil
            _ => 0
        };

        if (summonItemId > 0)
        {
            GiveItemById(summonItemId, 1, 0);
            SetStatus($"No hay comando de servidor activo: se entregó el invocador de {bossName}. Úsalo desde la barra rápida; el servidor puede rechazarlo.");
            return;
        }

        SetStatus($"Para invocar {bossName} directamente en multijugador, activa modo COMANDOS con TShock y permiso /spawnmob, o instala soporte de invocación en el servidor. Un cliente no puede forzar NPC en un servidor vanilla.");
    }

    private static void TeleportPlayer(int worldX, int worldY, string destination)
    {
        var p = Main.LocalPlayer;
        p.Teleport(new Vector2(worldX, worldY - p.height), 1);
        SetStatus($"Teletransportado a: {destination}");
    }

    private static void TeleportToPlayer(Player target)
    {
        Main.LocalPlayer.Teleport(target.position, 1);
        SetStatus($"Teletransportado hacia {target.name}.");
    }

    private static void TeleportToMe(Player target)
    {
        target.Teleport(Main.LocalPlayer.Center, 1);
        SetStatus($"{target.name} fue traÃ­do hacia ti.");
    }

    private static void HealPlayer(Player target)
    {
        target.statLife = target.statLifeMax2;
        target.statMana = target.statManaMax2;
        SetStatus($"{target.name} fue sanado por completo.");
    }

    private static void KillPlayerAction(Player target)
    {
        target.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason($"{target.name} fue eliminado por un administrador."), 1000.0, 0);
        SetStatus($"{target.name} fue eliminado.");
    }

    private static void SetStatus(string msg)
    {
        statusMessage = msg;
        statusTimer = 240;
    }
}