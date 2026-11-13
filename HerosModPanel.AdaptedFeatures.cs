using System;
using Terraria;

namespace Terraria.Unified.Features;

internal static partial class HerosModPanel
{
    // Extended Tile Reach: modifica solo el alcance local y restaura los valores originales.
    private static bool extendedReachEnabled;
    private static bool extendedReachOriginalCaptured;
    private static int extendedReachOriginalX;
    private static int extendedReachOriginalY;
    private const int ExtendedReachValue = 60;

    // Protecciones locales reversibles: conserva los valores previos de cada personaje.
    private static Player noFallDamagePlayer;
    private static bool noFallDamageOriginal;
    private static Player noKnockbackPlayer;
    private static bool noKnockbackOriginal;

    private static void SetNoFallDamage(bool enabled)
    {
        noFallDamageEnabled = enabled;
        ApplyNoFallDamage();
    }

    private static void ApplyNoFallDamage()
    {
        Player local = Main.LocalPlayer;
        if (noFallDamagePlayer != null && (!noFallDamageEnabled || !ReferenceEquals(noFallDamagePlayer, local)))
            RestoreNoFallDamage();
        if (!noFallDamageEnabled || local == null || !local.active)
            return;
        if (!ReferenceEquals(noFallDamagePlayer, local))
        {
            noFallDamagePlayer = local;
            noFallDamageOriginal = local.noFallDmg;
        }
        local.noFallDmg = true;
    }

    private static void RestoreNoFallDamage()
    {
        if (noFallDamagePlayer != null)
            noFallDamagePlayer.noFallDmg = noFallDamageOriginal;
        noFallDamagePlayer = null;
        noFallDamageOriginal = false;
    }

    private static void SetNoKnockback(bool enabled)
    {
        noKnockbackEnabled = enabled;
        ApplyNoKnockback();
    }

    private static void ApplyNoKnockback()
    {
        Player local = Main.LocalPlayer;
        if (noKnockbackPlayer != null && (!noKnockbackEnabled || !ReferenceEquals(noKnockbackPlayer, local)))
            RestoreNoKnockback();
        if (!noKnockbackEnabled || local == null || !local.active)
            return;
        if (!ReferenceEquals(noKnockbackPlayer, local))
        {
            noKnockbackPlayer = local;
            noKnockbackOriginal = local.noKnockback;
        }
        local.noKnockback = true;
    }

    private static void RestoreNoKnockback()
    {
        if (noKnockbackPlayer != null)
            noKnockbackPlayer.noKnockback = noKnockbackOriginal;
        noKnockbackPlayer = null;
        noKnockbackOriginal = false;
    }

    // Conserva el límite de invocaciones del personaje para restaurarlo al apagar la función.
    private static Player infiniteMinionsPlayer;
    private static int infiniteMinionsOriginalMax;

    private static void ApplyExtendedReach()
    {
        if (!extendedReachEnabled)
        {
            RestoreExtendedReach();
            return;
        }

        if (Main.dedServ || Main.gameMenu || Main.LocalPlayer == null || !Main.LocalPlayer.active)
            return;

        if (!extendedReachOriginalCaptured)
        {
            extendedReachOriginalX = Player.tileRangeX;
            extendedReachOriginalY = Player.tileRangeY;
            extendedReachOriginalCaptured = true;
        }

        Player.tileRangeX = Math.Max(extendedReachOriginalX, ExtendedReachValue);
        Player.tileRangeY = Math.Max(extendedReachOriginalY, ExtendedReachValue);
    }

    private static void RestoreExtendedReach()
    {
        if (extendedReachOriginalCaptured)
        {
            Player.tileRangeX = extendedReachOriginalX;
            Player.tileRangeY = extendedReachOriginalY;
        }

        extendedReachOriginalCaptured = false;
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
