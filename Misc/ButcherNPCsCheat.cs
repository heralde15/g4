using System.ComponentModel;
using System.Runtime.CompilerServices;
using Terraria.UtilClient.Chat;
using Terraria.UtilClient.Cheats.Types;
using Terraria.UtilClient.Common.Helpers.Game;
using Terraria.UtilClient.Localization;

namespace Terraria.UtilClient.Cheats.Categories.Misc;

public sealed class ButcherNPCsCheat : SyncRunActionCheat
{
	[CompilerGenerated]
	private NPCHelper.NPCTypeFilter? _003CFilterOverride_003Ek__BackingField;

	public override string Name => "Butcher NPCs";

	public override CheatCategory Category => CheatCategory.Misc;

	public override string CommandOverride => "butcher";

	public NPCHelper.NPCTypeFilter? FilterOverride
	{
		[CompilerGenerated]
		get
		{
			return _003CFilterOverride_003Ek__BackingField;
		}
		set
		{
			if (value.HasValue)
			{
				NPCHelper.NPCTypeFilter filter = value.GetValueOrDefault();
				if ((filter & ~NPCHelper.NPCTypeFilter.All) != NPCHelper.NPCTypeFilter.None)
				{
					throw new InvalidEnumArgumentException("value");
				}
			}
			_003CFilterOverride_003Ek__BackingField = value;
		}
	}

	public bool? AllowInvincibleOverride { get; set; }

	protected override void RunAction()
	{
		AutoButcherNPCsCheat.CheatSettings settings = BuiltInCheats.AutoButcherNPCs.Settings;
		NPCHelper.NPCTypeFilter filter = FilterOverride ?? settings.Filter;
		bool allowInvincible = AllowInvincibleOverride ?? settings.IncludeInvincibleNPCs;
		FilterOverride = null;
		AllowInvincibleOverride = null;
		int butchered = ButcherHelpers.ButcherAll(auto: false, filter, allowInvincible, settings.IncludeMultiPartNPCs);
		ChatLogging.LogInfoOrErrorIfCondition(ULang.TryGetPluralOrNoneByCount("UtilClientCheat.NpcButcher", butchered), butchered > 0);
	}
}
