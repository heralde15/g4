using System.Collections.Generic;
using Terraria.UtilClient.Cheats.Types;
using Terraria.UtilClient.Common.Helpers.Game;
using Terraria.UtilClient.Restrictions;

namespace Terraria.UtilClient.Cheats.Categories.Misc;

public sealed class AutoDeleteWorldItemsCheat : TickingCheat
{
	public override string Name => "Auto Delete World Items";

	public override CheatCategory Category => CheatCategory.Misc;

	public override IReadOnlyList<IRestriction> Restrictions { get; protected init; } = new _003C_003Ez__ReadOnlySingleElementList<IRestriction>(new NetRestriction(NetRestriction.Mode.Singleplayer));

	public override bool SaveEnabled => false;

	public override bool RateConfigurable => true;

	protected override uint DefaultUpdateRate => 15u;

	protected override void OnUpdate()
	{
		NetHelper.DeleteAllItems();
	}
}
