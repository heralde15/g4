using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria.Unified.Features;

internal static partial class HerosModPanel
{
    private enum HeroCheatGroup
    {
        Jugador,
        Movimiento,
        Combate,
        Visual,
        Mundo,
        Utilidades,
        Rendimiento
    }

    private static HeroCheatGroup selectedHeroCheatGroup;

    private static void ApplyConsoleToggle(string feature, bool enabled)
    {
        ExecuteLocalConsoleCommand(".toggle " + feature + (enabled ? " on" : " off"));
        WriteHerosLog("[HERO | CHEAT] " + feature + " = " + (enabled ? "ON" : "OFF"), new Color(120, 210, 255), "CHEAT");
    }

    private static void DrawCheatsTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, englishUi ? "FEATURES BY CATEGORY" : "FUNCIONES ORGANIZADAS POR CATEGORÍA");
        string[] names = englishUi
            ? new[] { "Player", "Movement", "Combat", "Visual", "World", "Utilities", "Performance" }
            : new[] { "Jugador", "Movimiento", "Combate", "Visual", "Mundo", "Utilidades", "Rendimiento" };
        int gap = 5;
        int buttonWidth = (panel.Width - Pad * 2 - gap * 3) / 4;
        for (int i = 0; i < names.Length; i++)
        {
            int row = i / 4;
            int col = i % 4;
            Rectangle rect = new(panel.X + Pad + col * (buttonWidth + gap), y + row * 25, buttonWidth, 22);
            bool selected = (int)selectedHeroCheatGroup == i;
            if (DrawButton(sb, pixel, rect, names[i], mouse, selected ? UiAccent : UiTabColor))
                selectedHeroCheatGroup = (HeroCheatGroup)i;
        }
        y += 56;

        string description = selectedHeroCheatGroup switch
        {
            HeroCheatGroup.Jugador => "Vida, protección y estado del personaje.",
            HeroCheatGroup.Movimiento => "Movimiento libre y desplazamiento local.",
            HeroCheatGroup.Combate => "Ayudas de combate y administración de efectos.",
            HeroCheatGroup.Visual => "Opciones visuales y reducción de efectos.",
            HeroCheatGroup.Mundo => "Opciones locales del mundo y herramientas de construcción.",
            HeroCheatGroup.Utilidades => "Funciones auxiliares y comodidad.",
            _ => "Herramientas para reducir carga y estabilizar FPS."
        };
        Utils.DrawBorderString(sb, description, new Vector2(panel.X + Pad, y), new Color(185, 195, 215), 0.58f);
        y += 22;

        int cols = 2;
        int width = (panel.Width - Pad * 2 - 8) / cols;
        int baseY = y;
        int actionRows = 0;
        void ActionButton(string label, string command, int index)
        {
            int row = index / cols;
            actionRows = Math.Max(actionRows, row + 1);
            int col = index % cols;
            Rectangle rect = new(panel.X + Pad + col * (width + 8), baseY + row * 31, width, 27);
            if (DrawButton(sb, pixel, rect, label, mouse, new Color(120, 75, 55)))
                ExecuteLocalConsoleCommand(command);
        }

        void ToggleButton(string label, string feature, bool current, int index)
        {
            int row = index / cols;
            actionRows = Math.Max(actionRows, row + 1);
            int col = index % cols;
            Rectangle rect = new(panel.X + Pad + col * (width + 8), baseY + row * 31, width, 27);
            if (DrawButton(sb, pixel, rect, label + (current ? "  [ON]" : "  [OFF]"), mouse,
                current ? new Color(35, 105, 78) : UiTabColor))
                ApplyConsoleToggle(feature, !current);
        }

        switch (selectedHeroCheatGroup)
        {
            case HeroCheatGroup.Jugador:
                ToggleButton("God Mode", "god", godmodeEnabled, 0);
                ToggleButton("Sin daño de caída", "nofall", noFallDamageEnabled, 1);
                ToggleButton("Sin retroceso", "noknockback", noKnockbackEnabled, 2);
                ToggleButton("Anti-debuff", "antidebuff", antiDebuffEnabled, 3);
                ToggleButton("Respiración infinita", "breath", infiniteBreathEnabled, 4);
                ToggleButton("Vida / maná bloqueados", "lifelock", lifeLockEnabled, 5);
                ToggleButton("Minions infinitos", "minions", infiniteMinionsEnabled, 6);
                ToggleButton("Suerte aumentada", "luck", luckBoostEnabled, 7);
                ToggleButton("Alcance extendido", "reach", extendedReachEnabled, 8);
                ToggleButton("Auto eliminar ganchos", "autokillhooks", autoKillHooksEnabled, 9);
                ActionButton("Curar vida y maná", ".heal", 10);
                ActionButton("Limpiar debuffs", ".debuffclear", 10);
                break;
            case HeroCheatGroup.Movimiento:
                ToggleButton("Vuelo libre", "fly", flyEnabled, 0);
                ToggleButton("Modo fantasma visual", "ghost", ghostModeEnabled, 1);
                ActionButton("Curar / restaurar", ".heal", 2);
                break;
            case HeroCheatGroup.Combate:
                ToggleButton("God Mode local", "god", godmodeEnabled, 0);
                ToggleButton("Bloqueo de vida", "lifelock", lifeLockEnabled, 1);
                ToggleButton("Limpiar efectos", "effects", antiEffectsEnabled, 2);
                ToggleButton("Reducir proyectiles", "projectiles", antiLagProjectilesEnabled, 3);
                ToggleButton("Sin i-frames (local)", "noiframes", noCombatIFramesEnabled, 4);
                ToggleButton("Forced Equips", "forceequips", forcedEquipsEnabled, 5);
                ActionButton("Ayuda Forced Equips", ".helpforceequips", 6);
                break;
            case HeroCheatGroup.Visual:
                ToggleButton("Modo fantasma", "ghost", ghostModeEnabled, 0);
                ToggleButton("Reducir efectos visuales", "effects", antiEffectsEnabled, 1);
                ToggleButton("Silenciar sonidos", "mute", muteGameSoundsEnabled, 2);
                ActionButton("Ver estado", ".status", 3);
                ActionButton("Informe completo", ".report", 4);
                break;
            case HeroCheatGroup.Mundo:
                ToggleButton("Tienda NPC gratis", "shopfree", npcShopFreeEnabled, 0);
                ToggleButton("Suerte aumentada", "luck", luckBoostEnabled, 1);
                ToggleButton("Auto eliminar ganchos", "autokillhooks", autoKillHooksEnabled, 2);
                ActionButton("Revelar mapa local", ".revealmap", 3);
                ActionButton("Eliminar objetos del suelo", ".clearitems", 4);
                ToggleButton("Auto limpiar objetos (SP)", "autoclearitems", autoClearWorldItemsEnabled, 5);
                break;
            case HeroCheatGroup.Utilidades:
                ToggleButton("Silenciar sonidos", "mute", muteGameSoundsEnabled, 0);
                ToggleButton("Tienda NPC gratis", "shopfree", npcShopFreeEnabled, 1);
                ToggleButton("Minions infinitos", "minions", infiniteMinionsEnabled, 2);
                ActionButton("Eliminar proyectiles propios", ".killprojs", 3);
                ActionButton("Limpiar debuffs", ".debuffclear", 4);
                ActionButton("Curar vida y maná", ".heal", 5);
                ActionButton("Ver posición", ".pos", 6);
                ActionButton("Ver inventario", ".inventory", 7);
                ActionButton("Revelar mapa", ".revealmap", 8);
                ActionButton("Limpiar objetos (SP)", ".clearitems", 9);
                ToggleButton("Auto limpiar objetos", "autoclearitems", autoClearWorldItemsEnabled, 10);
                ToggleButton("Auto eliminar ganchos", "autokillhooks", autoKillHooksEnabled, 11);
                break;
            case HeroCheatGroup.Rendimiento:
                ToggleButton("Anti-Lag", "antilag", antiLagEnabled, 0);
                ToggleButton("Estabilizador FPS", "fps", fpsStabilizerEnabled, 1);
                ToggleButton("Reducir efectos", "effects", antiEffectsEnabled, 2);
                ToggleButton("Reducir proyectiles", "projectiles", antiLagProjectilesEnabled, 3);
                break;
        }
        y += Math.Max(150, actionRows * 31 + 12);
        DrawSectionHeader(sb, panel, ref y, "CONSOLA RÁPIDA");
        string[] quick = { ".cheats", ".status", ".report", ".diagnostics", ".pos", ".players", ".logstatus", ".helpall" };
        int qWidth = (panel.Width - Pad * 2 - 7 * 4) / 8;
        for (int i = 0; i < quick.Length; i++)
        {
            Rectangle rect = new(panel.X + Pad + i * (qWidth + 4), y, qWidth, 24);
            if (DrawButton(sb, pixel, rect, quick[i], mouse))
            {
                ExecuteLocalConsoleCommand(quick[i]);
                commandConsoleVisible = true;
                commandConsoleOutputTimer = 240;
            }
        }
        y += 32;
        Utils.DrawBorderString(sb, "Las funciones se ejecutan con el sistema local de HERO; el servidor conserva el control de permisos.",
            new Vector2(panel.X + Pad, y), new Color(150, 165, 185), 0.52f);
    }

    private static void DrawLogsTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        DrawSectionHeader(sb, panel, ref y, englishUi ? "HERO EVENT LOG / DIAGNOSTICS" : "REGISTRO DE EVENTOS Y DIAGNÓSTICO HERO");
        int half = (panel.Width - Pad * 2 - 8) / 2;
        int settingsBaseY = y;
        void Setting(string label, bool value, Action toggle, int index)
        {
            int col = index % 2;
            int row = index / 2;
            Rectangle rect = new(panel.X + Pad + col * (half + 8), settingsBaseY + row * 28, half, 24);
            if (DrawButton(sb, pixel, rect, label + (value ? " [ON]" : " [OFF]"), mouse,
                value ? new Color(35, 95, 75) : UiTabColor))
                toggle();
        }
        Setting("Eventos generales", eventLogEnabled, () => { eventLogEnabled = !eventLogEnabled; if (eventLogEnabled) eventLogInitialized = false; }, 0);
        Setting("Entradas / salidas", eventLogPlayersEnabled, () => eventLogPlayersEnabled = !eventLogPlayersEnabled, 1);
        Setting("Muertes", eventLogDeathsEnabled, () => eventLogDeathsEnabled = !eventLogDeathsEnabled, 2);
        Setting("Jefes", eventLogBossesEnabled, () => eventLogBossesEnabled = !eventLogBossesEnabled, 3);
        Setting("Mundo / clima / bioma", eventLogWorldEnabled, () => eventLogWorldEnabled = !eventLogWorldEnabled, 4);
        Setting("Alertas de combate", eventLogCombatEnabled, () => eventLogCombatEnabled = !eventLogCombatEnabled, 5);
        Setting("NPC / entidades", eventLogNPCEnabled, () => eventLogNPCEnabled = !eventLogNPCEnabled, 6);
        Setting("Rendimiento", eventLogPerformanceEnabled, () => eventLogPerformanceEnabled = !eventLogPerformanceEnabled, 7);
        Setting("Cambios vida / maná", eventLogItemsEnabled, () => eventLogItemsEnabled = !eventLogItemsEnabled, 8);
        Setting("Guardar archivos", eventLogFileEnabled, () => eventLogFileEnabled = !eventLogFileEnabled, 9);
        y += 148;

        int actionWidth = (panel.Width - Pad * 2 - 16) / 3;
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, actionWidth, 25), "REGISTRO COMPLETO", mouse))
        {
            eventLogEnabled = eventLogPlayersEnabled = eventLogDeathsEnabled = eventLogBossesEnabled = true;
            eventLogWorldEnabled = eventLogCombatEnabled = eventLogNPCEnabled = eventLogPerformanceEnabled = true;
            eventLogFileEnabled = true;
            eventLogInitialized = false;
            WriteHerosLog("[HERO | SISTEMA] Todas las categorías de registro activadas.", new Color(120, 210, 255), "SYSTEM");
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + actionWidth + 8, y, actionWidth, 25), "LIMPIAR HISTORIAL", mouse))
        {
            lock (herosLogSync) herosLogHistory.Clear();
            WriteHerosLog("[HERO | SISTEMA] Historial en memoria limpiado.", new Color(160, 190, 220), "SYSTEM");
        }
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + (actionWidth + 8) * 2, y, actionWidth, 25), "ESTADO DEL LOG", mouse))
        {
            commandConsoleOutput = ".logstatus: eventos=" + eventLogEnabled + ", archivo=" + eventLogFileEnabled + ", NPC=" + eventLogNPCEnabled + ", rendimiento=" + eventLogPerformanceEnabled;
            commandConsoleVisible = true;
        }
        y += 34;
        DrawSectionHeader(sb, panel, ref y, "HISTORIAL RECIENTE  ·  " + GetHerosLogSnapshot().Length + " / " + HerosLogHistoryLimit);
        string[] entries = GetHerosLogSnapshot();
        int visibleLines = 10;
        int start = Math.Max(0, entries.Length - visibleLines - herosLogScroll);
        int end = Math.Min(entries.Length, start + visibleLines);
        for (int i = start; i < end; i++)
        {
            string entry = entries[i];
            var font = FontAssets.MouseText.Value;
            float maxWidth = panel.Width - Pad * 2;
            while (entry.Length > 3 && font.MeasureString(entry).X * 0.52f > maxWidth)
                entry = entry.Substring(0, entry.Length - 2) + "…";
            Utils.DrawBorderString(sb, entry, new Vector2(panel.X + Pad, y), new Color(190, 200, 215), 0.52f);
            y += 17;
        }
        if (entries.Length == 0)
            Utils.DrawBorderString(sb, "Todavía no hay eventos registrados.", new Vector2(panel.X + Pad, y), Color.Gray, 0.58f);
        Rectangle up = new(panel.Right - Pad - 52, panel.Bottom - 55, 24, 22);
        Rectangle down = new(panel.Right - Pad - 25, panel.Bottom - 55, 24, 22);
        if (DrawButton(sb, pixel, up, "▲", mouse)) herosLogScroll = Math.Min(Math.Max(0, entries.Length - visibleLines), herosLogScroll + 1);
        if (DrawButton(sb, pixel, down, "▼", mouse)) herosLogScroll = Math.Max(0, herosLogScroll - 1);
        Utils.DrawBorderString(sb, "Archivo: " + HerosLogDirectory, new Vector2(panel.X + Pad, panel.Bottom - 31), new Color(130, 155, 180), 0.48f);
    }
}
