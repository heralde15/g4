using System.ComponentModel.DataAnnotations;
using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.Travel;

public sealed class NoclipCheat : ToggleCheat<NoclipCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		[Range(4.0, 100.0)]
		public float Speed = 25f;

		[Range(0, 60)]
		public int SyncRate = 10;

		public bool GhostMovement;

		public bool EnableSpeedControls = true;

		[Range(1.0, 20.0)]
		public float FastMultiplier = 2.56f;

		[Range(0.10000000149011612, 1.0)]
		public float SlowMultiplier = 0.4f;
	}

	public override string Name => "Noclip";

	public override CheatCategory Category => CheatCategory.Travel;

	public override bool SaveEnabled => false;
}
