using System;
using Terraria;
using Terraria.ID;

namespace Terraria.Unified.Features;

internal static partial class HerosModPanel
{
    private static bool autoFishEnabled;
    private static int autoFishCastTimer;
    private static int autoFishRodSlot = -1;

    private static void UpdateAutoFish()
    {
        Player player = Main.LocalPlayer;
        if (!autoFishEnabled || player == null || !player.active || player.dead || Main.gameMenu)
            return;

        if (autoFishRodSlot < 0 || autoFishRodSlot >= 10)
        {
            int bestPower = 0;
            autoFishRodSlot = -1;
            for (int i = 0; i < 10; i++)
            {
                Item candidate = player.inventory[i];
                if (candidate != null && !candidate.IsAir && candidate.fishingPole >= 5 && candidate.fishingPole > bestPower)
                {
                    bestPower = candidate.fishingPole;
                    autoFishRodSlot = i;
                }
            }

            if (autoFishRodSlot < 0)
            {
                autoFishEnabled = false;
                commandConsoleOutput = "Auto Fish: no encontré una caña en la barra rápida.";
                return;
            }

            player.changeItem = autoFishRodSlot;
            autoFishCastTimer = Main.rand.Next(30, 91);
        }

        if (player.selectedItem != autoFishRodSlot)
        {
            player.changeItem = autoFishRodSlot;
            return;
        }

        if (player.GetFishingConditions().BaitPower <= 0)
        {
            autoFishEnabled = false;
            autoFishRodSlot = -1;
            commandConsoleOutput = "Auto Fish detenido: no hay cebo disponible.";
            return;
        }

        Projectile bobber = null;
        for (int i = 0; i < Main.projectile.Length; i++)
        {
            Projectile projectile = Main.projectile[i];
            if (projectile != null && projectile.active && projectile.owner == player.whoAmI && projectile.bobber)
            {
                bobber = projectile;
                break;
            }
        }

        if (bobber != null)
        {
            int result = (int)bobber.localAI[1];
            if (bobber.ai[1] < 0f && result != 0)
                UseFishingRod(player);
            return;
        }

        if (autoFishCastTimer > 0)
        {
            autoFishCastTimer--;
            return;
        }

        UseFishingRod(player);
        autoFishCastTimer = Main.rand.Next(30, 91);
    }

    private static void UseFishingRod(Player player)
    {
        player.controlUseItem = true;
        player.releaseUseItem = true;
        player.ItemCheck();
        player.controlUseItem = false;
        player.releaseUseItem = true;
    }

    private static void StopAutoFish()
    {
        autoFishEnabled = false;
        autoFishCastTimer = 0;
        autoFishRodSlot = -1;
        if (Main.LocalPlayer != null)
        {
            Main.LocalPlayer.controlUseItem = false;
            Main.LocalPlayer.releaseUseItem = true;
        }
    }
}
