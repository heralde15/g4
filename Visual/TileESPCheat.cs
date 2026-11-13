using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;
using Terraria.UtilClient.Config.JsonConverters;
using Terraria.UtilClient.Rendering;

namespace Terraria.UtilClient.Cheats.Categories.Visual;

public sealed class TileESPCheat : ToggleCheat<TileESPCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings, ITileOverlaySettings
	{
		[JsonProperty(ItemConverterType = typeof(ColorAlphaConverter))]
		public Dictionary<int, Color> TileColorSet = new Dictionary<int, Color>();

		public TileOverlays.TileOverlayTexture Texture { get; set; } = TileOverlays.TileOverlayTexture.SmartDig;

		Color ITileOverlaySettings.Color
		{
			get
			{
				throw new UnreachableException();
			}
		}

		public byte OutlineThickness { get; set; }
	}

	public override string Name => "Tile ESP";

	public override CheatCategory Category => CheatCategory.Visual;
}
