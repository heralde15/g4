using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace Terraria.Unified.Features;

/// <summary>
/// Modulo complementario de HERO's Mod: perfiles de estadísticas temporales y
/// marcadores de teletransporte. Los perfiles viven en memoria y se pierden al
/// cerrar el juego. En multijugador el servidor puede corregir estadísticas.
/// </summary>
internal static partial class HerosModPanel
{
    private sealed class PlayerProfileSnapshot
    {
        public bool Valid;
        public string Name = "";
        public int Life;
        public int MaxLife;
        public int Mana;
        public int MaxMana;
        public int SelectedSlot;
        public int Defense;
        public int TileX;
        public int TileY;
        public float MoveSpeed;
        public float RunAcceleration;
        public float MaxRunSpeed;
        public bool NoKnockback;
        public bool FireWalk;
        public bool LavaImmune;
        public bool FallImmune;
    }

    private readonly record struct TeleportBookmark(bool Valid, string Name, int X, int Y);

    private static readonly PlayerProfileSnapshot[] profileSlots =
    {
        new(), new(), new(), new()
    };

    private static TeleportBookmark[] teleportBookmarks =
    {
        new(false, "", 0, 0),
        new(false, "", 0, 0),
        new(false, "", 0, 0),
        new(false, "", 0, 0),
        new(false, "", 0, 0),
        new(false, "", 0, 0)
    };

    private static string profileNameInput = "Mi perfil";
    private static string bookmarkNameInput = "Marcador";
    private static string bookmarkXInput = "";
    private static string bookmarkYInput = "";
    private static int profilePage;
    private static int bookmarkPage;
    private static int selectedProfile = -1;
    private static int selectedBookmark = -1;
    private static bool profileConfirmRestore;
    private static int profileNoticeTimer;

    private static bool showBookmarkPage;

    private static void DrawProfilesTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        Player player = Main.LocalPlayer;
        DrawSectionHeader(sb, panel, ref y, showBookmarkPage ? "Marcadores de teletransporte" : "Perfiles del jugador");
        if (DrawButton(sb, pixel, new Rectangle(panel.Right - 170, y - 21, 160, 22),
            showBookmarkPage ? "Volver a perfiles" : "Abrir marcadores", mouse, new Color(50, 80, 115)))
            showBookmarkPage = !showBookmarkPage;

        if (!showBookmarkPage)
        {
            Utils.DrawBorderString(sb, "4 espacios temporales: guardan estadísticas y posición en tiles.",
                new Vector2(panel.X + Pad, y), new Color(190, 205, 225), 0.68f);
            y += 22;
            Utils.DrawBorderString(sb, "Nombre:", new Vector2(panel.X + Pad, y + 4), Color.White, 0.68f);
            DrawTextField(sb, pixel, new Rectangle(panel.X + Pad + 58, y, 235, 24), 31, profileNameInput, mouse, "Mi perfil");
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + 302, y, 145, 24), "Guardar espacio", mouse, new Color(45, 105, 75)))
                SavePlayerProfile(profilePage);
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + 455, y, 153, 24), "Restaurar", mouse, new Color(55, 85, 130)))
                RestorePlayerProfile(profilePage);
            y += 30;

            for (int i = 0; i < profileSlots.Length; i++)
            {
                PlayerProfileSnapshot p = profileSlots[i];
                int rowY = y + i * 31;
                bool selected = i == profilePage;
                var row = new Rectangle(panel.X + Pad, rowY, Width - Pad * 2, 27);
                sb.Draw(pixel, row, selected ? new Color(56, 80, 105) : new Color(38, 42, 52));
                string summary = p.Valid
                    ? $"{i + 1}. {p.Name} | HP {p.Life}/{p.MaxLife} | MP {p.Mana}/{p.MaxMana} | Pos {p.TileX},{p.TileY}"
                    : $"{i + 1}. (espacio vacío)";
                Utils.DrawBorderString(sb, summary, new Vector2(row.X + 7, row.Y + 5), Color.White, 0.68f);
                if (row.Contains(mouse) && Clicked)
                {
                    Main.mouseLeftRelease = false;
                    profilePage = i;
                    selectedProfile = i;
                    if (p.Valid) profileNameInput = p.Name;
                }
            }
            y += profileSlots.Length * 31 + 5;
            DrawSectionHeader(sb, panel, ref y, "Acciones rápidas");
            int bw = (Width - Pad * 2 - 12) / 3;
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, 25), "Curar al máximo", mouse, new Color(45, 105, 65)))
            {
                player.statLife = player.statLifeMax2;
                player.statMana = player.statManaMax2;
                SyncSelf();
                SetStatus("Vida y maná restaurados localmente.");
            }
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + 6, y, bw, 25), "Vida al 50%", mouse))
            {
                player.statLife = Math.Max(1, player.statLifeMax2 / 2);
                SyncSelf();
                SetStatus("Vida ajustada al 50%.");
            }
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + (bw + 6) * 2, y, bw, 25), "Maná al 50%", mouse))
            {
                player.statMana = Math.Max(0, player.statManaMax2 / 2);
                SyncSelf();
                SetStatus("Maná ajustado al 50%.");
            }
            y += 31;
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, y, bw, 25), "Quitar buffs", mouse, new Color(110, 65, 55)))
            {
                for (int i = player.buffType.Length - 1; i >= 0; i--)
                    if (player.buffType[i] > 0) player.DelBuff(i);
                SyncSelf();
                SetStatus("Se quitaron los buffs activos del jugador local.");
            }
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + bw + 6, y, bw, 25), "Volver a posición", mouse))
            {
                if (profileSlots[profilePage].Valid)
                {
                    teleportX = profileSlots[profilePage].TileX.ToString();
                    teleportY = profileSlots[profilePage].TileY.ToString();
                    TeleportToCoordinates();
                }
                else SetStatus("Guarda primero ese perfil.");
            }
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + (bw + 6) * 2, y, bw, 25), "Limpiar selección", mouse))
            {
                selectedProfile = -1;
                profileConfirmRestore = false;
                SetStatus("Selección del perfil limpiada.");
            }
            y += 32;
            Utils.DrawBorderString(sb, "Los perfiles no se guardan en disco. En servidores, algunos valores pueden revertirse.",
                new Vector2(panel.X + Pad, y), new Color(235, 195, 120), 0.62f);
            return;
        }

        Utils.DrawBorderString(sb, "Coordenadas en tiles. El servidor puede requerir permisos para teletransportarte.",
            new Vector2(panel.X + Pad, y), new Color(190, 205, 225), 0.66f);
        y += 23;
        Utils.DrawBorderString(sb, "X:", new Vector2(panel.X + Pad, y + 4), Color.White, 0.7f);
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad + 20, y, 75, 24), 32, bookmarkXInput, mouse, "X");
        Utils.DrawBorderString(sb, "Y:", new Vector2(panel.X + Pad + 102, y + 4), Color.White, 0.7f);
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad + 122, y, 75, 24), 33, bookmarkYInput, mouse, "Y");
        DrawTextField(sb, pixel, new Rectangle(panel.X + Pad + 205, y, 175, 24), 34, bookmarkNameInput, mouse, "Nombre");
        if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + 388, y, 220, 24), "Guardar / reemplazar", mouse, new Color(45, 105, 75)))
            SaveTeleportBookmark();
        y += 31;

        for (int i = 0; i < teleportBookmarks.Length; i++)
        {
            TeleportBookmark b = teleportBookmarks[i];
            int rowY = y + i * 34;
            string label = b.Valid ? $"{i + 1}. {b.Name} ({b.X}, {b.Y})" : $"{i + 1}. (vacío)";
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad, rowY, 365, 27), label, mouse,
                selectedBookmark == i ? new Color(65, 90, 125) : new Color(42, 47, 57)))
            {
                selectedBookmark = i;
                if (b.Valid)
                {
                    bookmarkXInput = b.X.ToString();
                    bookmarkYInput = b.Y.ToString();
                    bookmarkNameInput = b.Name;
                }
            }
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + 373, rowY, 110, 27), "Ir", mouse, new Color(45, 105, 75)))
                GoToTeleportBookmark(i);
            if (DrawButton(sb, pixel, new Rectangle(panel.X + Pad + 491, rowY, 117, 27), "Eliminar", mouse, new Color(110, 55, 55)))
            {
                teleportBookmarks[i] = new TeleportBookmark(false, "", 0, 0);
                if (selectedBookmark == i) selectedBookmark = -1;
                SetStatus($"Marcador {i + 1} eliminado.");
            }
        }
        y += teleportBookmarks.Length * 34 + 2;
        Utils.DrawBorderString(sb, "Consejo: selecciona un marcador para editar sus coordenadas y guardarlo encima.",
            new Vector2(panel.X + Pad, y), new Color(190, 205, 225), 0.62f);
    }

    private static void SavePlayerProfile(int index)
    {
        if (index < 0 || index >= profileSlots.Length)
        {
            SetStatus("Espacio de perfil no válido.");
            return;
        }

        Player p = Main.LocalPlayer;
        PlayerProfileSnapshot snapshot = profileSlots[index];
        snapshot.Valid = true;
        snapshot.Name = string.IsNullOrWhiteSpace(profileNameInput) ? $"Perfil {index + 1}" : profileNameInput.Trim();
        snapshot.Life = p.statLife;
        snapshot.MaxLife = p.statLifeMax;
        snapshot.Mana = p.statMana;
        snapshot.MaxMana = p.statManaMax;
        snapshot.SelectedSlot = p.selectedItem;
        snapshot.Defense = p.statDefense;
        snapshot.TileX = (int)(p.position.X / 16f);
        snapshot.TileY = (int)(p.position.Y / 16f);
        snapshot.MoveSpeed = p.moveSpeed;
        snapshot.RunAcceleration = p.runAcceleration;
        snapshot.MaxRunSpeed = p.maxRunSpeed;
        snapshot.NoKnockback = p.noKnockback;
        snapshot.FireWalk = p.fireWalk;
        snapshot.LavaImmune = p.lavaImmune;
        snapshot.FallImmune = p.noFallDmg;
        SetStatus($"Perfil '{snapshot.Name}' guardado en el espacio {index + 1} (memoria temporal).");
    }

    private static void RestorePlayerProfile(int index)
    {
        if (index < 0 || index >= profileSlots.Length || !profileSlots[index].Valid)
        {
            SetStatus("Ese espacio de perfil está vacío.");
            return;
        }

        Player p = Main.LocalPlayer;
        PlayerProfileSnapshot snapshot = profileSlots[index];
        p.statLifeMax = snapshot.MaxLife;
        p.statManaMax = snapshot.MaxMana;
        p.statLife = Math.Clamp(snapshot.Life, 1, Math.Max(1, p.statLifeMax2));
        p.statMana = Math.Clamp(snapshot.Mana, 0, Math.Max(0, p.statManaMax2));
        p.statDefense = snapshot.Defense;
        p.moveSpeed = snapshot.MoveSpeed;
        p.runAcceleration = snapshot.RunAcceleration;
        p.maxRunSpeed = snapshot.MaxRunSpeed;
        p.noKnockback = snapshot.NoKnockback;
        p.fireWalk = snapshot.FireWalk;
        p.lavaImmune = snapshot.LavaImmune;
        p.noFallDmg = snapshot.FallImmune;
        SyncSelf();
        SetStatus($"Perfil '{snapshot.Name}' restaurado localmente. El servidor puede sobrescribir algunos valores.");
    }

    private static void SaveTeleportBookmark()
    {
        if (!int.TryParse(bookmarkXInput, out int x) || !int.TryParse(bookmarkYInput, out int y))
        {
            SetStatus("Escribe coordenadas X e Y enteras.");
            return;
        }

        x = Math.Clamp(x, 0, Math.Max(0, Main.maxTilesX - 1));
        y = Math.Clamp(y, 0, Math.Max(0, Main.maxTilesY - 1));
        int index = selectedBookmark >= 0 && selectedBookmark < teleportBookmarks.Length
            ? selectedBookmark
            : Array.FindIndex(teleportBookmarks, b => !b.Valid);
        if (index < 0)
        {
            SetStatus("No hay espacios vacíos. Selecciona un marcador para reemplazarlo.");
            return;
        }

        string name = string.IsNullOrWhiteSpace(bookmarkNameInput) ? $"Marcador {index + 1}" : bookmarkNameInput.Trim();
        teleportBookmarks[index] = new TeleportBookmark(true, name, x, y);
        selectedBookmark = index;
        SetStatus($"Marcador '{name}' guardado en ({x}, {y}).");
    }

    private static void GoToTeleportBookmark(int index)
    {
        if (index < 0 || index >= teleportBookmarks.Length || !teleportBookmarks[index].Valid)
        {
            SetStatus("Ese marcador está vacío.");
            return;
        }

        TeleportBookmark b = teleportBookmarks[index];
        teleportX = b.X.ToString();
        teleportY = b.Y.ToString();
        TeleportToCoordinates();
        SetStatus($"Solicitado teletransporte a '{b.Name}' ({b.X}, {b.Y}).");
    }
}