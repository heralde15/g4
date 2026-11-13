using System;
using Terraria;

namespace Terraria.Unified.Features;

internal static partial class HerosModPanel
{
    // Extended Tile Reach: modifica solo el alcance local y restaura los valores originales.
    private static bool extendedReachEnabled;
    private static Player extendedReachPlayer;
    private static int extendedReachOriginalX;
    private static int extendedReachOriginalY;
    private const int ExtendedReachValue = 60;

    // Conserva el límite de invocaciones del personaje para restaurarlo al apagar la función.
    private static Player infiniteMinionsPlayer;
    private static int infiniteMinionsOriginalMax;

    private static void ApplyExtendedReach()
    {
        Player local = Main.LocalPlayer;

        // Si cambió el personaje o se desactivó el toggle, devuelve primero los valores anteriores.
        if (extendedReachPlayer != null && (!extendedReachEnabled || !ReferenceEquals(extendedReachPlayer, local)))
            RestoreExtendedReach();

        if (!extendedReachEnabled || local == null || !local.active)
            return;

        if (!ReferenceEquals(extendedReachPlayer, local))
        {
            extendedReachPlayer = local;
            extendedReachOriginalX = local.tileRangeX;
            extendedReachOriginalY = local.tileRangeY;
        }

        local.tileRangeX = Math.Max(extendedReachOriginalX, ExtendedReachValue);
        local.tileRangeY = Math.Max(extendedReachOriginalY, ExtendedReachValue);
    }

    private static void RestoreExtendedReach()
    {
        if (extendedReachPlayer != null)
        {
            extendedReachPlayer.tileRangeX = extendedReachOriginalX;
            extendedReachPlayer.tileRangeY = extendedReachOriginalY;
        }

        extendedReachPlayer = null;
        extendedReachOriginalX = 0;
        extendedReachOriginalY = 0;
    }

    private static void SetInfiniteMinions(bool enabled)
    {
        infiniteMinionsEnabled = enabled;
        ApplyInfiniteMinions();
    }

    private static void ApplyInfiniteMinions()
    {
        Player local = Main.LocalPlayer;
        if (infiniteMinionsPlayer != null && (!infiniteMinionsEnabled || !ReferenceEquals(infiniteMinionsPlayer, local)))
            RestoreInfiniteMinions();

        if (!infiniteMinionsEnabled || local == null || !local.active)
            return;

        if (!ReferenceEquals(infiniteMinionsPlayer, local))
        {
            infiniteMinionsPlayer = local;
            infiniteMinionsOriginalMax = local.maxMinions;
        }

        local.maxMinions = 9999;
    }

    private static void RestoreInfiniteMinions()
    {
        if (infiniteMinionsPlayer != null)
            infiniteMinionsPlayer.maxMinions = infiniteMinionsOriginalMax;

        infiniteMinionsPlayer = null;
        infiniteMinionsOriginalMax = 0;
    }

    // Acción manual: elimina únicamente proyectiles activos propiedad del jugador local.
    // No envía paquetes ni modifica proyectiles de otros jugadores.
    private static int KillOwnedProjectilesLocal()
    {
        if (Main.dedServ || Main.gameMenu || Main.LocalPlayer == null || !Main.LocalPlayer.active)
            return 0;

        int removed = 0;
        int owner = Main.myPlayer;
        for (int i = 0; i < Main.projectile.Length; i++)
        {
            Projectile projectile = Main.projectile[i];
            if (projectile == null || !projectile.active || projectile.owner != owner)
                continue;

            projectile.Kill();
            removed++;
        }

        return removed;
    }
}
