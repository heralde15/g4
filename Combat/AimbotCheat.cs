using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Terraria.GameInput;
using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.Combat;

public sealed class AimbotCheat : ToggleCheat<AimbotCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		public bool TargetPlayers;

		[JsonConverter(typeof(StringEnumConverter))]
		public LockOnHelper.LockOnMode Mode = LockOnHelper.LockOnMode.ThreeDS;
	}

	public override string Name => "Aimbot";

	public override CheatCategory Category => CheatCategory.Combat;
}
