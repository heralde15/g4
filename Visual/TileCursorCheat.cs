using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;
using Terraria.UtilClient.Rendering;

namespace Terraria.UtilClient.Cheats.Categories.Visual;

public sealed class TileCursorCheat : ToggleCheat<TileCursorCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings, ITileOverlaySettings
	{
		[CompilerGenerated]
		private Color _003CColor_003Ek__BackingField;

		public TileOverlays.TileOverlayTexture Texture { get; set; }

		public Color Color
		{
			[CompilerGenerated]
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _003CColor_003Ek__BackingField;
			}
			[CompilerGenerated]
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				_003CColor_003Ek__BackingField = value;
			}
		}

		public byte OutlineThickness { get; set; }

		public CheatSettings()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			Texture = TileOverlays.TileOverlayTexture.Ruler;
			Color mediumPurple = Color.MediumPurple;
			((Color)(ref mediumPurple)).A = 100;
			Color = mediumPurple;
			OutlineThickness = 2;
			base._002Ector();
		}
	}

	public override string Name => "Tile Cursor";

	public override CheatCategory Category => CheatCategory.Visual;
}
