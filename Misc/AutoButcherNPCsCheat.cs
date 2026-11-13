using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;
using Terraria.UtilClient.Common.Helpers.Game;

namespace Terraria.UtilClient.Cheats.Categories.Misc;

public sealed class AutoButcherNPCsCheat : TickingCheat<AutoButcherNPCsCheat.CheatSettings>
{
	public class CheatSettings : USettings, INPCFilterSettings
	{
		public NPCHelper.NPCTypeFilter Filter { get; set; } = NPCHelper.NPCTypeFilter.Hostile | NPCHelper.NPCTypeFilter.Boss;

		public bool IncludeInvincibleNPCs { get; set; }

		public bool IncludeMultiPartNPCs { get; set; } = true;
	}

	public override string Name => "Auto Butcher NPCs";

	public override CheatCategory Category => CheatCategory.Misc;

	public override bool SaveEnabled => false;

	public override bool RateConfigurable => true;

	protected override uint DefaultUpdateRate => 1u;

	public override string CommandOverride => "autobutcher";

	protected override void OnUpdate()
	{
		ButcherHelpers.ButcherAll(auto: true, base.Settings);
	}
}
