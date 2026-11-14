using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using SkiaSharp;

namespace Terraria.ZazaMod.WorldEdit;

internal class TEditImageToSchematic
{
	public class TileWallData
	{
		public string Type { get; set; }

		public int Id { get; set; }

		public string Name { get; set; }

		public int Paint { get; set; }

		public string Color { get; set; }

		public bool BuildSafe { get; set; }
	}

	public static List<TileWallData> TileWallDataList { get; set; }

	public static Color[] Clrs { get; set; } = (Color[])(object)new Color[1];

	public static List<TileWallData> ClrsTileWallData { get; set; }

	public static async void ConvertPixelArt(SKBitmap btm, bool buildSchematic = true)
	{
		try
		{
			await Task.Run(delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				new List<Color>();
				Color val = Color.Lime;
				for (int i = 0; i < btm.Width; i++)
				{
					for (int j = 0; j < btm.Height; j++)
					{
						val = DirectColor(btm, i, j, includeTransparent: false);
						if (buildSchematic)
						{
							LegacyTile legacyTile = new LegacyTile();
							if (((Color)(ref val)).A < 25)
							{
								Mods.copiedTiles.Add(legacyTile);
							}
							else
							{
								string finalHtmlColor = $"#{((Color)(ref val)).A:X2}{((Color)(ref val)).R:X2}{((Color)(ref val)).G:X2}{((Color)(ref val)).B:X2}";
								TileWallData tileWallData = ClrsTileWallData.FirstOrDefault((TileWallData t) => t.Color.Equals(finalHtmlColor, StringComparison.OrdinalIgnoreCase));
								if (tileWallData != null)
								{
									legacyTile.wall = (ushort)tileWallData.Id;
									legacyTile.wallColor((byte)tileWallData.Paint);
									Mods.copiedTiles.Add(legacyTile);
								}
							}
						}
					}
				}
			});
		}
		catch (Exception)
		{
		}
		Mods.convertingToSchematic = false;
	}

	private static Color DirectColor(SKBitmap bmp, int x, int y, bool includeTransparent)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		SKPixmap pixmap = bmp.PeekPixels();
		Span<byte> pixels = pixmap.GetPixelSpan();
		int offset = y * pixmap.RowBytes + x * 4;
		Color color = default(Color);
		((Color)(ref color))._002Ector((int)pixels[offset + 2], (int)pixels[offset + 1], (int)pixels[offset], (int)pixels[offset + 3]);
		if (includeTransparent || ((Color)(ref color)).A != 0)
		{
			if (includeTransparent && ((Color)(ref color)).A == 0)
			{
				color = Color.Lime;
			}
			return Clr((Color[])(object)new Color[1] { color });
		}
		return color;
	}

	public static Color Clr(Color[] cs)
	{
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		int r = 0;
		int g = 0;
		int b = 0;
		for (int i = 0; i < cs.Length; i++)
		{
			r += ((Color)(ref cs[i])).R;
			g += ((Color)(ref cs[i])).G;
			b += ((Color)(ref cs[i])).B;
		}
		r /= cs.Length;
		g /= cs.Length;
		b /= cs.Length;
		int near = 1000;
		int ind = 0;
		for (int cl = 0; cl < Clrs.Length; cl++)
		{
			int valR = ((Color)(ref Clrs[cl])).R - r;
			int valG = ((Color)(ref Clrs[cl])).G - g;
			int valB = ((Color)(ref Clrs[cl])).B - b;
			if (valR < 0)
			{
				valR = -valR;
			}
			if (valG < 0)
			{
				valG = -valG;
			}
			if (valB < 0)
			{
				valB = -valB;
			}
			int total = valR + valG + valB;
			if (total < near)
			{
				ind = cl;
				near = total;
			}
		}
		return Clrs[ind];
	}

	public static async void BuildColorFilter()
	{
		TileWallDataList = new List<TileWallData>();
		using (Stream stream = FromEmbeddedPath("Terraria.ZazaMod.WorldEdit.MapColors.MapColors.xml"))
		{
			TileWallDataList = await ReadDataAsync(stream, null);
		}
		await Task.Run(delegate
		{
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			bool useTiles = true;
			bool useWalls = true;
			bool buildSafe = true;
			useTiles = false;
			useWalls = true;
			buildSafe = true;
			List<TileWallData> list = TileWallDataList.Where((TileWallData data) => ((useTiles && data.Type == "Tile") || (useWalls && data.Type == "Wall")) && (!buildSafe || data.BuildSafe)).ToList();
			if (false)
			{
				list = (from data in list
					group data by data.Color into @group
					select @group.First()).ToList();
			}
			int num = 0;
			Clrs = (Color[])(object)new Color[list.Count];
			foreach (TileWallData current in list)
			{
				try
				{
					Clrs[num] = HexStringToMicrosoftColor_WithAlpha(current.Color);
				}
				catch
				{
					Clrs[num] = Color.Transparent;
				}
				num++;
			}
			ClrsTileWallData = list;
		});
	}

	private static Color HexStringToMicrosoftColor_WithAlpha(string hexColor)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		string obj = (hexColor.Contains("#") ? hexColor.Replace("#", "") : hexColor);
		byte a = byte.Parse(obj.Substring(0, 2), NumberStyles.HexNumber);
		byte r = byte.Parse(obj.Substring(2, 2), NumberStyles.HexNumber);
		byte g = byte.Parse(obj.Substring(4, 2), NumberStyles.HexNumber);
		byte b = byte.Parse(obj.Substring(6, 2), NumberStyles.HexNumber);
		return new Color((int)r, (int)g, (int)b, (int)a);
	}

	public static async Task<List<TileWallData>> ReadDataAsync(Stream stream, IProgress<int> progress)
	{
		List<TileWallData> dataList = new List<TileWallData>();
		List<XElement> elements = (from e in (await Task.Run(() => XDocument.Load(stream))).Descendants()
			where e.Name == "Tile" || e.Name == "Wall"
			select e).ToList();
		int totalElements = elements.Count;
		for (int i = 0; i < totalElements; i++)
		{
			XElement element = elements[i];
			TileWallData data = new TileWallData
			{
				Type = element.Name.LocalName,
				Id = int.Parse(element.Attribute("Id").Value),
				Name = element.Attribute("Name").Value,
				Paint = int.Parse(element.Attribute("Paint").Value),
				Color = element.Attribute("Color").Value,
				BuildSafe = bool.Parse(element.Attribute("BuildSafe").Value)
			};
			dataList.Add(data);
			progress?.Report((i + 1) * 100 / totalElements);
		}
		return dataList;
	}

	public static Stream FromEmbeddedPath(string path)
	{
		return Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
	}
}
