using System.ComponentModel.DataAnnotations;
using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.Visual;

public sealed class SmoothScrollingChEaT : ToggleCheat<SmoothScrollingChEaT.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		[Range(0.0, 3.4028234663852886E+38, MinimumIsExclusive = true, MaximumIsExclusive = false)]
		public float ScrollMultiplier = 1f;

		[Range(-3.4028234663852886E+38, 1.0, MinimumIsExclusive = false, MaximumIsExclusive = true)]
		public float ScrollSmoothness = 0.1f;

		[Range(0.25, 119.94999694824219)]
		public float SmoothingCutoff = 2f;
	}

	public override string Name => "Smooth Scrolling";

	public override CheatCategory Category => CheatCategory.Visual;

	protected override bool PanicModeAffected => false;
}
