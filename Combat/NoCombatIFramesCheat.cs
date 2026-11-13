using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.Combat;

public sealed class NoCombatIFramesCheat : ToggleCheat<NoCombatIFramesCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		public bool NPCs = true;

		public bool LocalPlayer;
	}

	public override string Name => "No Combat Immunity Frames";

	public override CheatCategory Category => CheatCategory.Combat;
}
