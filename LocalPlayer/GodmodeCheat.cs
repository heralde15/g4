using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.LocalPlayer;

public sealed class GodmodeCheat : ToggleCheat<GodmodeCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		public bool ReceiveDamage;

		public bool ImmediateRegen;

		public bool RegenOnDeath;

		public bool DodgeDamage;
	}

	public override string Name => "Godmode";

	public override CheatCategory Category => CheatCategory.LocalPlayer;

	public override string CommandOverride => "god";
}
