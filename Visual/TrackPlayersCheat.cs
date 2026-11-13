using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.Visual;

public sealed class TrackPlayersCheat : ToggleCheat<TrackPlayersCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		public bool EnableTeleporting;

		public bool TeleportWithoutWormhole;

		public bool SilentTeleport;
	}

	public override string Name => "Track Players";

	public override CheatCategory Category => CheatCategory.Visual;
}
