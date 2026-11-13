using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;

namespace Terraria.Unified.Features;

internal static partial class HerosModPanel
{
    private static bool noclipEnabled;
    private static float noclipSpeed = 12f;
    private static bool noclipOriginalCaptured;
    private static bool noclipOriginalCollision;
    private static FieldInfo noclipCollisionField;
    private static PropertyInfo noclipCollisionProperty;
    private static bool autoButcherEnabled;
    private static int autoButcherInterval = 30;
    private static int autoButcherTimer;

    private static bool TrySetNoTileCollision(Player player, bool value, out bool previous)
    {
        previous = false;
        if (player == null) return false;
        Type type = player.GetType();
        noclipCollisionField ??= type.GetField("noTileCollide", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        noclipCollisionProperty ??= type.GetProperty("noTileCollide", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        try
        {
            if (noclipCollisionField != null && noclipCollisionField.FieldType == typeof(bool))
            {
                previous = (bool)noclipCollisionField.GetValue(player);
                noclipCollisionField.SetValue(player, value);
                return true;
            }
            if (noclipCollisionProperty != null && noclipCollisionProperty.PropertyType == typeof(bool) && noclipCollisionProperty.CanWrite)
            {
                previous = (bool)noclipCollisionProperty.GetValue(player);
                noclipCollisionProperty.SetValue(player, value);
                return true;
            }
        }
        catch { }
        return false;
    }

    private static void UpdateNoclip()
    {
        Player player = Main.LocalPlayer;
        if (player == null || !player.active || player.dead || Main.gameMenu) return;
        if (!noclipEnabled)
        {
            if (noclipOriginalCaptured)
            {
                TrySetNoTileCollision(player, noclipOriginalCollision, out _);
                noclipOriginalCaptured = false;
            }
            return;
        }

        if (!noclipOriginalCaptured)
        {
            if (!TrySetNoTileCollision(player, true, out noclipOriginalCollision))
            {
                noclipEnabled = false;
                commandConsoleOutput = "Noclip no disponible: el motor no expone noTileCollide.";
                return;
            }
            noclipOriginalCaptured = true;
        }
        else
        {
            TrySetNoTileCollision(player, true, out _);
        }

        if (activeField != 0 || Main.drawingPlayerChat) return;
        KeyboardState keys = Main.keyState;
        Vector2 direction = Vector2.Zero;
        if (keys.IsKeyDown(Keys.W) || keys.IsKeyDown(Keys.Up)) direction.Y -= 1f;
        if (keys.IsKeyDown(Keys.S) || keys.IsKeyDown(Keys.Down)) direction.Y += 1f;
        if (keys.IsKeyDown(Keys.A) || keys.IsKeyDown(Keys.Left)) direction.X -= 1f;
        if (keys.IsKeyDown(Keys.D) || keys.IsKeyDown(Keys.Right)) direction.X += 1f;
        if (keys.IsKeyDown(Keys.Space)) direction.Y -= 1f;
        if (keys.IsKeyDown(Keys.LeftShift)) direction.Y += 1f;
        if (direction != Vector2.Zero)
        {
            direction.Normalize();
            player.velocity = direction * noclipSpeed;
        }
        else
        {
            player.velocity = Vector2.Zero;
        }
    }

    private static void SetNoclip(bool enabled)
    {
        noclipEnabled = enabled;
        if (!enabled && Main.LocalPlayer != null && noclipOriginalCaptured)
        {
            TrySetNoTileCollision(Main.LocalPlayer, noclipOriginalCollision, out _);
            noclipOriginalCaptured = false;
        }
    }

    private static bool TeleportLocalToWorldPoint(Vector2 worldPosition, string label)
    {
        Player player = Main.LocalPlayer;
        if (player == null || !player.active || Main.gameMenu) return false;
        if (Main.netMode == 1 && IsServer)
        {
            commandConsoleOutput = "Teletransporte local bloqueado en modo COMANDOS; usa los comandos autorizados del servidor.";
            return false;
        }
        Vector2 destination = worldPosition - new Vector2(player.width * 0.5f, player.height);
        destination.X = Math.Clamp(destination.X, 0f, Math.Max(0f, Main.maxTilesX * 16f - player.width));
        destination.Y = Math.Clamp(destination.Y, 0f, Math.Max(0f, Main.maxTilesY * 16f - player.height));
        player.Teleport(destination, 1);
        player.velocity = Vector2.Zero;
        if (Main.netMode == 1) SyncSelf();
        commandConsoleOutput = "Teletransportado: " + label + ".";
        return true;
    }

    private static void TeleportToMouse()
    {
        TeleportLocalToWorldPoint(Main.MouseWorld, "posición del cursor");
    }

    private static void TeleportToMapCoordinates(int tileX, int tileY)
    {
        if (tileX < 0 || tileY < 0 || tileX >= Main.maxTilesX || tileY >= Main.maxTilesY)
        {
            commandConsoleOutput = "Coordenadas fuera del mundo.";
            return;
        }
        TeleportLocalToWorldPoint(new Vector2(tileX * 16f + 8f, tileY * 16f + 8f), "mapa (" + tileX + ", " + tileY + ")");
    }

    private static int ButcherHostileNPCsLocal()
    {
        if (Main.netMode != 0)
        {
            commandConsoleOutput = "Auto Butcher solo está disponible en un jugador; no envía daño falso a servidores.";
            return 0;
        }
        int killed = 0;
        for (int i = 0; i < Main.npc.Length; i++)
        {
            NPC npc = Main.npc[i];
            if (npc == null || !npc.active || npc.life <= 0 || npc.friendly || npc.townNPC || npc.immortal || npc.dontTakeDamage)
                continue;
            if (npc.realLife >= 0 && npc.realLife != npc.whoAmI)
                continue;
            npc.life = 0;
            npc.checkDead();
            if (npc.active)
                npc.active = false;
            killed++;
        }
        return killed;
    }

    private static void UpdateAutoButcher()
    {
        if (!autoButcherEnabled || Main.gameMenu || Main.netMode != 0)
            return;
        if (++autoButcherTimer < autoButcherInterval)
            return;
        autoButcherTimer = 0;
        int count = ButcherHostileNPCsLocal();
        if (count > 0)
            WriteHerosLog("Auto Butcher eliminó " + count + " NPC hostiles.", new Color(255, 150, 100), "NPC");
    }
}
