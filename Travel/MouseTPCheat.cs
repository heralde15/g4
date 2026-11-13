using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.Travel;

public sealed class MouseTPCheat : ToggleCheat<MouseTPCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		public bool WithEffects;

		public bool LerpScreen;
	}

	public override string Name => "Mouse TP";

	public override CheatCategory Category => CheatCategory.Travel;

	protected override bool PanicModeAffected => false;
}
