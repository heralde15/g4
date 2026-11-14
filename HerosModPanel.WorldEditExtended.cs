using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Terraria.Unified.Features;

internal static partial class HerosModPanel
{
    // Herramientas WorldEdit portadas a la arquitectura de HERO.
    // No dependen de Mods.*, NetHelper ni del protocolo privado de Zaza.
    private static int worldEditBrushSize = 3;
    private static int worldEditBrushShape;
    private static string worldEditImagePath = string.Empty;

    private static void DrawWorldEditExtendedTab(SpriteBatch sb, Texture2D pixel, Rectangle panel, ref int y, Point mouse)
    {
        int x = panel.X + Pad;
        int half = (panel.Width - Pad * 2 - 8) / 2;
        DrawSectionHeader(sb, panel, ref y, "Pinceles y conversión de imágenes");
        Utils.DrawBorderString(sb, "Pincel local · centro en el personaje", new Vector2(x, y), UiAccent, 0.68f);
        y += 22;
        Utils.DrawBorderString(sb, "Tamaño: " + worldEditBrushSize + " tiles", new Vector2(x, y + 4), Color.White, 0.68f);
        if (DrawButton(sb, pixel, new Rectangle(x + 190, y, 42, 25), "−", mouse, UiTabColor))
            worldEditBrushSize = Math.Max(1, worldEditBrushSize - 1);
        if (DrawButton(sb, pixel, new Rectangle(x + 238, y, 42, 25), "+", mouse, UiTabColor))
            worldEditBrushSize = Math.Min(32, worldEditBrushSize + 1);
        if (DrawButton(sb, pixel, new Rectangle(x + 288, y, Math.Max(80, panel.Width - Pad * 2 - 288), 25),
            worldEditBrushShape == 0 ? "Forma: círculo" : worldEditBrushShape == 1 ? "Forma: cuadrado" : "Forma: rombo",
            mouse, new Color(55, 75, 105)))
            worldEditBrushShape = (worldEditBrushShape + 1) % 3;
        y += 34;
        Utils.DrawBorderString(sb, "ID de bloque y pared: usa los campos de las páginas Bloques y Paredes/ID.", new Vector2(x, y), Color.LightGray, 0.58f);
        y += 21;
        if (DrawButton(sb, pixel, new Rectangle(x, y, half, RowH), "Pincel: colocar bloque", mouse, new Color(45, 115, 75)))
            ApplyWorldEditBrush(0);
        if (DrawButton(sb, pixel, new Rectangle(x + half + 8, y, half, RowH), "Pincel: colocar pared", mouse, new Color(55, 90, 145)))
            ApplyWorldEditBrush(1);
        y += RowH + 6;
        if (DrawButton(sb, pixel, new Rectangle(x, y, half, RowH), "Borrador: tiles", mouse, new Color(145, 65, 60)))
            ApplyWorldEditBrush(2);
        if (DrawButton(sb, pixel, new Rectangle(x + half + 8, y, half, RowH), "Deshacer pincel", mouse, new Color(75, 85, 115)))
            UndoWorldEdit();
        y += RowH + 12;

        DrawSectionHeader(sb, panel, ref y, "Imagen → construcción");
        Utils.DrawBorderString(sb, "Ruta local PNG/JPG/JPEG (máx. 20 MB y 256 × 256 px)", new Vector2(x, y), Color.White, 0.62f);
        y += 20;
        DrawTextField(sb, pixel, new Rectangle(x, y, panel.Width - Pad * 2, 26), 72, worldEditImagePath, mouse, "C:\\ruta\\imagen.png");
        y += 33;
        if (DrawButton(sb, pixel, new Rectangle(x, y, half, RowH), "Convertir imagen", mouse, new Color(75, 95, 150)))
            ImportWorldEditImage();
        if (DrawButton(sb, pixel, new Rectangle(x + half + 8, y, half, RowH), "Abrir carpeta de esquemas", mouse, new Color(55, 75, 105)))
        {
            try
            {
                string dir = Path.Combine(Main.SavePath, "HerosMod", "Schematics");
                Directory.CreateDirectory(dir);
                SetStatus("Carpeta de esquemas: " + dir);
            }
            catch (Exception ex) { SetStatus("No se pudo abrir la carpeta: " + ex.Message); }
        }
        y += RowH + 7;
        Utils.DrawBorderString(sb, "La imagen usa el Tile ID de la página Bloques. Los píxeles transparentes se omiten; el resultado es un mosaico monocromático editable con Undo.",
            new Vector2(x, y), new Color(190, 205, 225), 0.56f);
        y += 32;
        if (DrawButton(sb, pixel, new Rectangle(x, y, panel.Width - Pad * 2, RowH), "Exportar esquema avanzado (.hwsx)", mouse, UiAccent))
            ExportWorldSchematicExtended();
        y += RowH + 5;
        if (DrawButton(sb, pixel, new Rectangle(x, y, panel.Width - Pad * 2, RowH), "Importar esquema avanzado (.hwsx) en POS1", mouse, new Color(55, 115, 80)))
            ImportWorldSchematicExtended();
        y += RowH + 6;
        Utils.DrawBorderString(sb, "El formato avanzado conserva bloques, paredes y líquidos. Las operaciones son locales; en multijugador el servidor puede revertirlas.",
            new Vector2(x, y), new Color(235, 195, 120), 0.56f);
    }

    private static void ApplyWorldEditBrush(int mode)
    {
        if (Main.netMode == 2 || Main.LocalPlayer == null || !Main.LocalPlayer.active)
        {
            SetStatus("El pincel solo puede ejecutarse desde un cliente con personaje activo.");
            return;
        }
        int tileId = int.TryParse(worldTileType, out int t) ? t : 1;
        int wallId = int.TryParse(worldWallType, out int w) ? w : 1;
        if (mode == 0 && (tileId < 0 || tileId >= TileID.Count)) { SetStatus("Tile ID fuera de rango."); return; }
        if (mode == 1 && (wallId < 0 || wallId >= WallID.Count)) { SetStatus("Wall ID fuera de rango."); return; }

        int cx = (int)(Main.LocalPlayer.Center.X / 16f);
        int cy = (int)(Main.LocalPlayer.Center.Y / 16f);
        int radius = worldEditBrushSize / 2;
        int x1 = Math.Max(1, cx - radius), x2 = Math.Min(Main.maxTilesX - 2, cx + radius);
        int y1 = Math.Max(1, cy - radius), y2 = Math.Min(Main.maxTilesY - 2, cy + radius);
        worldEditUndo.Clear();
        int changed = 0;
        for (int tx = x1; tx <= x2; tx++)
        for (int ty = y1; ty <= y2; ty++)
        {
            int dx = Math.Abs(tx - cx), dy = Math.Abs(ty - cy);
            bool inside = worldEditBrushShape == 0 ? dx * dx + dy * dy <= radius * radius + 1
                : worldEditBrushShape == 1 ? true : dx + dy <= radius;
            if (!inside) continue;
            Tile tile = Main.tile[tx, ty];
            if (tile == null) continue;
            worldEditUndo.Add((tx, ty, (Tile)tile.Clone()));
            if (mode == 0)
            {
                if (tileId == 0) WorldGen.KillTile(tx, ty, false, false, true);
                else WorldGen.PlaceTile(tx, ty, tileId, true, true);
            }
            else if (mode == 1) tile.WallType = (ushort)wallId;
            else
            {
                if (tile.HasTile) WorldGen.KillTile(tx, ty, false, false, true);
                tile.WallType = 0;
                tile.LiquidAmount = 0;
            }
            changed++;
        }
        if (changed == 0) worldEditUndo.Clear();
        SetStatus("Pincel WorldEdit: " + changed + " celdas procesadas." + GetWorldEditAuthorityNotice());
    }

    private static string GetExtendedSchematicPath()
    {
        string safe = Path.GetFileNameWithoutExtension(schematicFileName ?? "construccion");
        foreach (char c in Path.GetInvalidFileNameChars()) safe = safe.Replace(c, '_');
        if (string.IsNullOrWhiteSpace(safe)) safe = "construccion";
        string dir = Path.Combine(Main.SavePath, "HerosMod", "Schematics");
        Directory.CreateDirectory(dir);
        return Path.Combine(dir, safe + ".hwsx");
    }

    private static void ExportWorldSchematicExtended()
    {
        if (!worldPos1.HasValue || !worldPos2.HasValue) { SetStatus("Marca POS1 y POS2 antes de exportar."); return; }
        try
        {
            int x1 = Math.Clamp(Math.Min(worldPos1.Value.X, worldPos2.Value.X), 1, Main.maxTilesX - 2);
            int x2 = Math.Clamp(Math.Max(worldPos1.Value.X, worldPos2.Value.X), 1, Main.maxTilesX - 2);
            int y1 = Math.Clamp(Math.Min(worldPos1.Value.Y, worldPos2.Value.Y), 1, Main.maxTilesY - 2);
            int y2 = Math.Clamp(Math.Max(worldPos1.Value.Y, worldPos2.Value.Y), 1, Main.maxTilesY - 2);
            int width = x2 - x1 + 1, height = y2 - y1 + 1;
            if ((long)width * height > 250000) { SetStatus("Esquema demasiado grande: máximo 250 000 tiles."); return; }
            string path = GetExtendedSchematicPath();
            using var stream = File.Create(path);
            using var writer = new BinaryWriter(stream);
            writer.Write("HERO_WORLD_EDIT_2");
            writer.Write(width); writer.Write(height);
            for (int yy = y1; yy <= y2; yy++)
            for (int xx = x1; xx <= x2; xx++)
            {
                Tile tile = Main.tile[xx, yy];
                writer.Write(tile != null && tile.HasTile);
                writer.Write(tile != null && tile.HasTile ? tile.TileType : (ushort)0);
                writer.Write(tile?.WallType ?? (ushort)0);
                writer.Write(tile?.LiquidType ?? (byte)0);
                writer.Write(tile?.LiquidAmount ?? (byte)0);
            }
            worldSchematicStatus = "Exportado esquema avanzado: " + Path.GetFileName(path);
            SetStatus("Esquema avanzado guardado: " + path);
        }
        catch (Exception ex) { SetStatus("Error al exportar esquema avanzado: " + ex.Message); }
    }

    private static void ImportWorldSchematicExtended()
    {
        if (!worldPos1.HasValue) { SetStatus("Marca POS1 antes de importar."); return; }
        try
        {
            string path = GetExtendedSchematicPath();
            if (!File.Exists(path)) { SetStatus("No existe " + Path.GetFileName(path)); return; }
            int width, height;
            using (var reader = new BinaryReader(File.OpenRead(path)))
            {
                if (reader.ReadString() != "HERO_WORLD_EDIT_2") throw new InvalidDataException("Formato HWSX no reconocido.");
                width = reader.ReadInt32(); height = reader.ReadInt32();
                if (width < 1 || height < 1 || (long)width * height > 250000 ||
                    worldPos1.Value.X < 1 || worldPos1.Value.Y < 1 ||
                    worldPos1.Value.X + width >= Main.maxTilesX - 1 || worldPos1.Value.Y + height >= Main.maxTilesY - 1)
                    throw new InvalidDataException("Dimensiones inválidas o esquema fuera del mundo.");
                long expected = (long)width * height * 7L;
                if (reader.BaseStream.Length - reader.BaseStream.Position != expected)
                    throw new InvalidDataException("El archivo está incompleto o contiene datos adicionales.");
            }

            worldEditUndo.Clear();
            int changed = 0;
            using (var reader = new BinaryReader(File.OpenRead(path)))
            {
                reader.ReadString(); reader.ReadInt32(); reader.ReadInt32();
                for (int yy = 0; yy < height; yy++)
                for (int xx = 0; xx < width; xx++)
                {
                    bool hasTile = reader.ReadBoolean();
                    ushort tileType = reader.ReadUInt16();
                    ushort wall = reader.ReadUInt16();
                    byte liquidType = reader.ReadByte();
                    byte liquidAmount = reader.ReadByte();
                    int wx = worldPos1.Value.X + xx, wy = worldPos1.Value.Y + yy;
                    Tile tile = Main.tile[wx, wy];
                    if (tile == null) continue;
                    worldEditUndo.Add((wx, wy, (Tile)tile.Clone()));
                    if (tile.HasTile) WorldGen.KillTile(wx, wy, false, false, true);
                    if (hasTile && tileType < TileID.Count) WorldGen.PlaceTile(wx, wy, tileType, true, true);
                    tile.WallType = wall < WallID.Count ? wall : (ushort)0;
                    tile.LiquidType = liquidType <= LiquidID.Shimmer ? liquidType : (byte)LiquidID.Water;
                    tile.LiquidAmount = liquidAmount;
                    changed++;
                }
            }
            worldSchematicStatus = "Importado esquema avanzado: " + width + " × " + height;
            SetStatus("Esquema HWSX importado: " + changed + " tiles." + GetWorldEditAuthorityNotice());
        }
        catch (Exception ex) { worldEditUndo.Clear(); SetStatus("Error al importar HWSX: " + ex.Message); }
    }

    private static void ImportWorldEditImage()
    {
        if (Main.netMode == 2 || Main.LocalPlayer == null || !Main.LocalPlayer.active)
        { SetStatus("La conversión de imágenes requiere un cliente con personaje activo."); return; }
        if (string.IsNullOrWhiteSpace(worldEditImagePath) || !File.Exists(worldEditImagePath))
        { SetStatus("Escribe una ruta de imagen válida."); return; }
        try
        {
            var info = new FileInfo(worldEditImagePath);
            if (info.Length > 20L * 1024 * 1024) throw new InvalidDataException("La imagen supera 20 MB.");
            using Image<Rgba32> image = Image.Load<Rgba32>(worldEditImagePath);
            if (image.Width < 1 || image.Height < 1 || image.Width > 256 || image.Height > 256)
                throw new InvalidDataException("La imagen debe medir como máximo 256 × 256 píxeles.");
            if (!int.TryParse(worldTileType, out int tileId) || tileId < 0 || tileId >= TileID.Count)
                throw new InvalidDataException("Configura un Tile ID válido en la página Bloques.");

            int originX = (int)(Main.LocalPlayer.Center.X / 16f);
            int originY = (int)(Main.LocalPlayer.Center.Y / 16f);
            if (originX + image.Width >= Main.maxTilesX - 1 || originY + image.Height >= Main.maxTilesY - 1)
                throw new InvalidDataException("La imagen no cabe en el mundo desde la posición actual.");

            worldEditUndo.Clear();
            int changed = 0;
            for (int py = 0; py < image.Height; py++)
            for (int px = 0; px < image.Width; px++)
            {
                Rgba32 pixelColor = image[px, py];
                if (pixelColor.A < 24) continue;
                int wx = originX + px, wy = originY + py;
                Tile tile = Main.tile[wx, wy];
                if (tile == null) continue;
                worldEditUndo.Add((wx, wy, (Tile)tile.Clone()));
                if (tile.HasTile) WorldGen.KillTile(wx, wy, false, false, true);
                if (tileId > 0) WorldGen.PlaceTile(wx, wy, tileId, true, true);
                changed++;
            }
            SetStatus("Imagen convertida a mosaico: " + image.Width + " × " + image.Height + ", " + changed + " píxeles opacos. Undo disponible." + GetWorldEditAuthorityNotice());
        }
        catch (Exception ex) { worldEditUndo.Clear(); SetStatus("No se pudo convertir la imagen: " + ex.Message); }
    }
}
