using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.LocalPlayer;

public class ExtendedReachCheat : ToggleCheat<ExtendedReachCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		public int TileRangeX = 18;

		public int TileRangeY = 16;
	}

	public override string Name => "Extended Tile Reach";

	public override CheatCategory Category => CheatCategory.LocalPlayer;

	public override string CommandOverride => "tilereach";
}
