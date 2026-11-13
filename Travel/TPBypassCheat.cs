using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.Travel;

public sealed class TPBypassCheat : ToggleCheat<TPBypassCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		public bool Rods = true;

		public bool Pylons = true;
	}

	public override string Name => "TP Bypass";

	public override CheatCategory Category => CheatCategory.Travel;
}
