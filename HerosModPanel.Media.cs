using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using XnaColor = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;

namespace Terraria.Unified.Features;

internal static partial class HerosModPanel
{
    private sealed class HeroGifFrame
    {
        public Texture2D Texture;
        public int DurationMs;
    }

    private static readonly List<HeroGifFrame> heroGifFrames = new();
    private static string heroGifLoadedPath = string.Empty;
    private static int heroGifFrameIndex;
    private static long heroGifFrameStartedAt;

    // La consola usa una textura separada; cambiarla no afecta el fondo del panel.
    private static string consoleBackgroundImagePath = string.Empty;
    private static Texture2D consoleBackgroundTexture;
    private static bool consoleBackgroundEnabled;
    private static int consoleBackgroundOpacity = 50;
    private static string consoleBackgroundNotice = "Sin imagen personalizada para la consola.";

    private static void LoadAnimatedPanelBackground(string path)
    {
        if (string.Equals(heroGifLoadedPath, path, StringComparison.OrdinalIgnoreCase) &&
            heroGifFrames.Count > 0)
        {
            uiBackgroundEnabled = true;
            uiBackgroundNotice = "GIF animado listo.";
            return;
        }

        var decodedFrames = new List<HeroGifFrame>();
        try
        {
            var info = new FileInfo(path);
            if (!info.Exists || info.Length <= 0 || info.Length > 20 * 1024 * 1024)
                throw new InvalidDataException("El GIF debe pesar entre 1 byte y 20 MB.");

            using (Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(path))
            {
                int width = image.Width;
                int height = image.Height;
                int frameCount = image.Frames.Count;

                if (width < 1 || height < 1 || width > 2048 || height > 2048)
                    throw new InvalidDataException("El GIF no puede superar 2048 × 2048.");
                if (frameCount < 2)
                    throw new InvalidDataException("El archivo GIF no contiene animación.");
                if (frameCount > 50 || (long)width * height * frameCount > 24000000L)
                    throw new InvalidDataException("El GIF tiene demasiados fotogramas o consume demasiada memoria (máximo 50 fotogramas y 24 MP acumulados).");

                for (int i = 0; i < frameCount; i++)
                {
                    ImageFrame<Rgba32> sourceFrame = image.Frames[i];
                    Rgba32[] sourcePixels = new Rgba32[width * height];
                    sourceFrame.CopyPixelDataTo(sourcePixels);

                    XnaColor[] texturePixels = new XnaColor[sourcePixels.Length];
                    for (int pixelIndex = 0; pixelIndex < sourcePixels.Length; pixelIndex++)
                    {
                        Rgba32 color = sourcePixels[pixelIndex];
                        texturePixels[pixelIndex] = new XnaColor(color.R, color.G, color.B, color.A);
                    }

                    var texture = new Texture2D(Main.graphics.GraphicsDevice, width, height);
                    texture.SetData(texturePixels);

                    int delay = sourceFrame.Metadata.GetGifMetadata().FrameDelay;
                    int durationMs = Math.Clamp(delay <= 0 ? 100 : delay * 10, 20, 10000);
                    decodedFrames.Add(new HeroGifFrame { Texture = texture, DurationMs = durationMs });
                }
            }

            try
            {
                if (uiBackgroundTexture != null && !uiBackgroundTexture.IsDisposed)
                    uiBackgroundTexture.Dispose();
            }
            catch { }
            uiBackgroundTexture = null;
            UnloadAnimatedPanelBackground();
            heroGifFrames.AddRange(decodedFrames);
            decodedFrames.Clear();
            heroGifLoadedPath = path;
            heroGifFrameIndex = 0;
            heroGifFrameStartedAt = Environment.TickCount64;
            uiBackgroundImagePath = path;
            uiBackgroundEnabled = true;
            uiBackgroundNotice = "GIF animado cargado (" + heroGifFrames.Count + " fotogramas).";
            SetStatus(uiBackgroundNotice);
            SaveSettingsIfChanged(true);
        }
        catch (Exception ex)
        {
            foreach (HeroGifFrame frame in decodedFrames)
            {
                try { if (frame.Texture != null && !frame.Texture.IsDisposed) frame.Texture.Dispose(); }
                catch { }
            }
            uiBackgroundEnabled = false;
            uiBackgroundNotice = "No se pudo cargar el GIF: " + ex.Message;
            SetStatus(uiBackgroundNotice);
        }
    }

    private static void DrawAnimatedPanelBackground(SpriteBatch sb, Rectangle panel)
    {
        if (heroGifFrames.Count == 0 || !string.Equals(heroGifLoadedPath, uiBackgroundImagePath, StringComparison.OrdinalIgnoreCase))
            LoadAnimatedPanelBackground(uiBackgroundImagePath);

        if (heroGifFrames.Count == 0)
            return;

        long now = Environment.TickCount64;
        if (heroGifFrameStartedAt <= 0)
            heroGifFrameStartedAt = now;

        int guard = heroGifFrames.Count;
        while (guard-- > 0 && now - heroGifFrameStartedAt >= heroGifFrames[heroGifFrameIndex].DurationMs)
        {
            heroGifFrameStartedAt += heroGifFrames[heroGifFrameIndex].DurationMs;
            heroGifFrameIndex = (heroGifFrameIndex + 1) % heroGifFrames.Count;
        }

        Texture2D texture = heroGifFrames[heroGifFrameIndex].Texture;
        if (texture != null && !texture.IsDisposed)
            sb.Draw(texture, panel, XnaColor.White * (Math.Clamp(uiBackgroundOpacity, 10, 100) / 100f));
    }

    private static void UnloadAnimatedPanelBackground()
    {
        foreach (HeroGifFrame frame in heroGifFrames)
        {
            try { if (frame.Texture != null && !frame.Texture.IsDisposed) frame.Texture.Dispose(); }
            catch { }
        }
        heroGifFrames.Clear();
        heroGifLoadedPath = string.Empty;
        heroGifFrameIndex = 0;
        heroGifFrameStartedAt = 0;
    }

    private static void LoadConsoleBackground()
    {
        try
        {
            string path = (consoleBackgroundImagePath ?? string.Empty).Trim().Trim('"');
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                throw new FileNotFoundException("No se encontró el archivo indicado.");

            var info = new FileInfo(path);
            if (info.Length <= 0 || info.Length > 20 * 1024 * 1024)
                throw new InvalidDataException("La imagen debe pesar entre 1 byte y 20 MB.");

            string extension = Path.GetExtension(path).ToLowerInvariant();
            if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
                throw new InvalidDataException("Para la consola usa PNG o JPG.");

            Texture2D loaded;
            using (FileStream stream = File.OpenRead(path))
                loaded = Texture2D.FromStream(Main.graphics.GraphicsDevice, stream);

            if (loaded.Width > 8192 || loaded.Height > 8192)
            {
                loaded.Dispose();
                throw new InvalidDataException("La imagen supera el límite de 8192 × 8192.");
            }

            UnloadConsoleBackground();
            consoleBackgroundTexture = loaded;
            consoleBackgroundImagePath = path;
            consoleBackgroundEnabled = true;
            consoleBackgroundNotice = "Imagen de consola cargada correctamente.";
            SetStatus(consoleBackgroundNotice);
            SaveSettingsIfChanged(true);
        }
        catch (Exception ex)
        {
            consoleBackgroundEnabled = false;
            consoleBackgroundNotice = "Error en imagen de consola: " + ex.Message;
            SetStatus(consoleBackgroundNotice);
        }
    }

    private static void DrawConsoleCustomBackground(SpriteBatch sb, Rectangle panel)
    {
        if (!consoleBackgroundEnabled)
            return;

        if ((consoleBackgroundTexture == null || consoleBackgroundTexture.IsDisposed) &&
            !string.IsNullOrWhiteSpace(consoleBackgroundImagePath))
        {
            try
            {
                string path = consoleBackgroundImagePath;
                if (!File.Exists(path))
                    throw new FileNotFoundException("No se encontró la imagen guardada.");

                using FileStream stream = File.OpenRead(path);
                Texture2D loaded = Texture2D.FromStream(Main.graphics.GraphicsDevice, stream);
                if (loaded.Width > 8192 || loaded.Height > 8192)
                {
                    loaded.Dispose();
                    throw new InvalidDataException("La imagen supera el límite de 8192 × 8192.");
                }
                consoleBackgroundTexture = loaded;
            }
            catch (Exception ex)
            {
                consoleBackgroundEnabled = false;
                consoleBackgroundNotice = "No se pudo restaurar el fondo de consola: " + ex.Message;
                return;
            }
        }

        if (consoleBackgroundTexture != null && !consoleBackgroundTexture.IsDisposed)
            sb.Draw(consoleBackgroundTexture, panel,
                XnaColor.White * (Math.Clamp(consoleBackgroundOpacity, 10, 100) / 100f));
    }

    private static void UnloadConsoleBackground()
    {
        try
        {
            if (consoleBackgroundTexture != null && !consoleBackgroundTexture.IsDisposed)
                consoleBackgroundTexture.Dispose();
        }
        catch { }
        consoleBackgroundTexture = null;
    }
}
