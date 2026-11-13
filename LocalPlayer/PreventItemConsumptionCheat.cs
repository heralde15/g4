using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.LocalPlayer;

public sealed class PreventItemConsumptionCheat : ToggleCheat<PreventItemConsumptionCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		public bool Potions = true;

		public bool Placeables = true;

		public bool Weapons = true;

		public bool Ammo = true;

		public bool Money = true;

		public bool Wiring = true;

		public bool Other = true;
	}

	public override string Name => "Prevent Item Consumption";

	public override CheatCategory Category => CheatCategory.LocalPlayer;
}
