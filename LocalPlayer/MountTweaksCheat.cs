using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.LocalPlayer;

public sealed class MountTweaksCheat : ToggleCheat<MountTweaksCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		public bool AllMountsCanGrapple = true;

		public bool AllMountsCanDash = true;

		public bool AllTransformMountsCanUseItems = true;

		public bool DontDismountOnCC = true;
	}

	public override string Name => "Mount Tweaks";

	public override CheatCategory Category => CheatCategory.LocalPlayer;
}
