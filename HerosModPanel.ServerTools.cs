using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;

namespace Terraria.Unified.Features;

internal static partial class HerosModPanel
{
    // Herramientas de reconocimiento pasivo: solo consultan datos que el cliente ya recibió.
    // No envían paquetes especiales ni intentan enumerar archivos/plugins del servidor remoto.

    private static string GetHeroServerPluginScan()
    {
        if (Main.gameMenu)
            return "HERO SCAN: entra a un mundo o servidor antes de ejecutar el análisis.";

        string networkMode = Main.netMode switch
        {
            0 => "Un jugador",
            1 => "Cliente conectado a servidor",
            2 => "Servidor dedicado",
            _ => "Desconocido (" + Main.netMode + ")"
        };
        int visiblePlayers = Main.player.Count(p => p != null && p.active);
        string serverInfo = GetServerInfoSummary();

        return "HERO SERVER SCAN | Red=" + networkMode +
            " | Jugadores visibles=" + visiblePlayers +
            " | Terraria=" + Main.versionNumber +
            " | Mundo=" + (string.IsNullOrWhiteSpace(Main.worldName) ? "sin nombre" : Main.worldName) +
            " | Datos=" + serverInfo +
            " | Plugins: el protocolo normal de Terraria no expone un listado de plugins del servidor. " +
            "Solo pueden identificarse cuando el servidor los anuncia en mensajes o interfaces públicas; " +
            "este análisis no inventa nombres ni envía sondeos.";
    }

    private static string GetHeroBossScan()
    {
        List<string> bosses = new();
        for (int i = 0; i < Main.npc.Length; i++)
        {
            NPC npc = Main.npc[i];
            if (npc == null || !npc.active || !npc.boss)
                continue;

            string name = string.IsNullOrWhiteSpace(npc.FullName) ? "NPC tipo " + npc.type : npc.FullName;
            bosses.Add(name + " (" + Math.Max(0, npc.life) + "/" + Math.Max(1, npc.lifeMax) + " HP)");
        }

        return bosses.Count == 0
            ? "HERO BOSS SCAN: no hay jefes activos visibles."
            : "HERO BOSS SCAN (" + bosses.Count + "): " + string.Join(" | ", bosses.Take(12)) +
                (bosses.Count > 12 ? " | +" + (bosses.Count - 12) + " más" : string.Empty);
    }

    private static string GetHeroHostileScan(string radiusArgument)
    {
        Player player = Main.LocalPlayer;
        if (player == null || !player.active)
            return "HERO HOSTILE SCAN: no hay personaje activo.";

        int radius = 80;
        if (!string.IsNullOrWhiteSpace(radiusArgument) &&
            int.TryParse(radiusArgument, out int parsedRadius))
            radius = Math.Clamp(parsedRadius, 10, 500);

        float maxDistanceSquared = radius * 16f * radius * 16f;
        List<(string Name, double Distance, bool Boss, int Life, int LifeMax)> found = new();

        for (int i = 0; i < Main.npc.Length; i++)
        {
            NPC npc = Main.npc[i];
            if (npc == null || !npc.active || !npc.CanBeChasedBy())
                continue;

            float distanceSquared = Vector2.DistanceSquared(player.Center, npc.Center);
            if (distanceSquared > maxDistanceSquared)
                continue;

            string name = string.IsNullOrWhiteSpace(npc.FullName) ? "NPC tipo " + npc.type : npc.FullName;
            found.Add((name, Math.Sqrt(distanceSquared) / 16.0, npc.boss, npc.life, npc.lifeMax));
        }

        var ordered = found.OrderBy(n => n.Distance).Take(10).ToArray();
        if (ordered.Length == 0)
            return "HERO HOSTILE SCAN: no hay enemigos visibles en " + radius + " tiles.";

        return "HERO HOSTILE SCAN (" + found.Count + " en " + radius + " tiles): " +
            string.Join(" | ", ordered.Select(n =>
                (n.Boss ? "[JEFE] " : string.Empty) + n.Name + " a " +
                n.Distance.ToString("0") + " tiles (" + Math.Max(0, n.Life) + "/" +
                Math.Max(1, n.LifeMax) + " HP)"));
    }

    private static string GetHeroWorldScan()
    {
        if (Main.gameMenu)
            return "HERO WORLD SCAN: no hay mundo cargado.";

        string difficulty = Main.masterMode ? "Maestro" : Main.expertMode ? "Experto" : "Normal";
        string network = Main.netMode == 0 ? "Un jugador" : Main.netMode == 1 ? "Multijugador" : "Servidor";
        int activeNpcs = Main.npc.Count(n => n != null && n.active);
        int activePlayers = Main.player.Count(p => p != null && p.active);
        int hostileNpcs = Main.npc.Count(n => n != null && n.active && n.CanBeChasedBy());

        return "HERO WORLD SCAN | Mundo=" + Main.worldName +
            " | Tamaño=" + Main.maxTilesX + "x" + Main.maxTilesY +
            " | Dificultad=" + difficulty +
            " | Hardmode=" + (Main.hardMode ? "Sí" : "No") +
            " | Hora=" + (Main.dayTime ? "Día" : "Noche") +
            " | Clima=" + (Main.raining ? "Lluvia" : "Despejado") +
            " | Red=" + network +
            " | Jugadores=" + activePlayers +
            " | NPC=" + activeNpcs +
            " | Hostiles=" + hostileNpcs +
            " | FPS=" + Main.frameRate;
    }
}
