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
}
