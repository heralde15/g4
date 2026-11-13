using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.Travel;

public sealed class MapTPCheat : ToggleCheat<MapTPCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		public bool WithEffects;

		public bool UseMapPing;

		public bool DoesNotRapidfire;
	}

	public override string Name => "Map TP";

	public override CheatCategory Category => CheatCategory.Travel;

	protected override bool PanicModeAffected => false;
}
