using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent;

namespace Terraria.Unified.Features;

internal static partial class HerosModPanel
{
    // Extended Tile Reach: modifica solo el alcance local y restaura los valores originales.
    private static bool extendedReachEnabled;
    private static bool extendedReachOriginalCaptured;
    private static int extendedReachOriginalX;
    private static int extendedReachOriginalY;
    private const int ExtendedReachValue = 60;

    // Limpieza automática de objetos tirados: solo en un jugador para no alterar servidores.
    private static bool autoClearWorldItemsEnabled;
    private static int autoClearWorldItemsTimer;
    private const int AutoClearWorldItemsInterval = 600;

    // Accesorios configurados manualmente; se aplican solo al jugador local.
    private static bool forcedEquipsEnabled;
    private static readonly Dictionary<int, int> forcedEquipItems = new();

    // Desactiva los frames de invulnerabilidad del jugador local (no afecta NPCs).
    private static bool noCombatIFramesEnabled;

    // Retira ganchos de agarre propios cuando regresan junto al personaje.
    private static bool autoKillHooksEnabled;

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

    private static int ClearWorldItemsLocal()
    {
        if (Main.dedServ || Main.gameMenu || Main.netMode != 0)
            return 0;

        int removed = 0;
        for (int i = 0; i < Main.item.Length; i++)
        {
            WorldItem worldItem = Main.item[i];
            if (worldItem == null || !worldItem.active)
                continue;

            // WorldItem.active es de solo lectura en esta variante. Accedemos al Item
            // contenido mediante reflexión y usamos su método TurnToAir cuando exista.
            object containedItem = GetWorldItemContainedItem(worldItem);
            if (containedItem == null)
                continue;

            MethodInfo turnToAir = containedItem.GetType().GetMethod(
                "TurnToAir",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                Type.EmptyTypes,
                null);
            if (turnToAir != null)
            {
                turnToAir.Invoke(containedItem, null);
                removed++;
                continue;
            }

            FieldInfo activeField = containedItem.GetType().GetField(
                "active",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (activeField != null && activeField.FieldType == typeof(bool))
            {
                activeField.SetValue(containedItem, false);
                removed++;
            }
        }
        return removed;
    }

    private static object GetWorldItemContainedItem(WorldItem worldItem)
    {
        Type type = worldItem.GetType();
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        FieldInfo field = type.GetField("item", flags) ?? type.GetField("_item", flags);
        if (field != null)
            return field.GetValue(worldItem);

        PropertyInfo property = type.GetProperty("item", flags) ?? type.GetProperty("Item", flags);
        if (property != null && property.GetIndexParameters().Length == 0 && property.GetMethod != null)
            return property.GetValue(worldItem);

        return null;
    }

    private static bool RevealLocalMap()
    {
        if (Main.dedServ || Main.gameMenu)
            return false;

        // Revela únicamente la iluminación del mapa que el cliente ya conoce;
        // no solicita secciones ni envía paquetes al servidor.
        for (int x = 0; x < Main.maxTilesX; x++)
            for (int y = 0; y < Main.maxTilesY; y++)
                Main.Map.UpdateLighting(x, y, byte.MaxValue);

        Main.refreshMap = true;
        return true;
    }

    private static void ApplyForcedEquips()
    {
        if (!forcedEquipsEnabled || Main.LocalPlayer == null || !Main.LocalPlayer.active || Main.LocalPlayer.dead)
            return;

        Player player = Main.LocalPlayer;
        foreach (KeyValuePair<int, int> entry in forcedEquipItems)
        {
            Item item = new Item();
            item.SetDefaults(entry.Key);
            if (item.IsAir || !item.accessory)
                continue;

            int amount = Math.Clamp(entry.Value, 1, 10);
            for (int i = 0; i < amount; i++)
            {
                // Estos métodos son internos/privados en esta variante del motor.
                // Se invocan por reflexión para mantener compatibilidad sin romper la compilación.
                InvokePlayerEquipMethod(player, "GrantArmorBenefits", item, true);
                InvokePlayerEquipMethod(player, "ApplyEquipFunctional", 0, item, true);
            }

            if (entry.Key == 4743)
                player.hasFootball = true;
            if (entry.Key == 4760)
                player.hasRaisableShield = true;
        }
    }

    private static void InvokePlayerEquipMethod(Player player, string methodName, params object[] arguments)
    {
        MethodInfo[] methods = typeof(Player).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (MethodInfo method in methods)
        {
            if (method.Name != methodName)
                continue;

            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length != arguments.Length)
                continue;

            bool matches = true;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (arguments[i] == null || !parameters[i].ParameterType.IsInstanceOfType(arguments[i]))
                {
                    matches = false;
                    break;
                }
            }

            if (!matches)
                continue;

            method.Invoke(player, arguments);
            return;
        }
    }

    private static void ApplyNoCombatIFrames()
    {
        if (!noCombatIFramesEnabled || Main.LocalPlayer == null || !Main.LocalPlayer.active)
            return;

        // Mantiene la opción estrictamente local. God Mode conserva prioridad si está activo.
        Main.LocalPlayer.immuneTime = 0;
        if (!godmodeEnabled)
            Main.LocalPlayer.immune = false;
    }

    private static void UpdateAdaptedUtilityFeatures()
    {
        if (autoKillHooksEnabled && Main.LocalPlayer != null && Main.LocalPlayer.active)
        {
            Player player = Main.LocalPlayer;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile hook = Main.projectile[i];
                if (hook != null && hook.active && hook.owner == player.whoAmI
                    && hook.type >= 0 && hook.type < Main.projHook.Length
                    && Main.projHook[hook.type]
                    && Math.Abs(player.Center.X - hook.Center.X) <= 32f
                    && Math.Abs(player.Center.Y - hook.Center.Y) <= 32f)
                    hook.Kill();
            }
        }

        if (!autoClearWorldItemsEnabled)
        {
            autoClearWorldItemsTimer = 0;
            return;
        }

        if (Main.netMode != 0 || Main.gameMenu)
        {
            autoClearWorldItemsEnabled = false;
            autoClearWorldItemsTimer = 0;
            return;
        }

        if (++autoClearWorldItemsTimer >= AutoClearWorldItemsInterval)
        {
            autoClearWorldItemsTimer = 0;
            ClearWorldItemsLocal();
        }
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
