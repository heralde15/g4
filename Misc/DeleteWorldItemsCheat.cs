using Terraria.UtilClient.Chat;
using Terraria.UtilClient.Cheats.Types;
using Terraria.UtilClient.Common.Helpers.Game;
using Terraria.UtilClient.Localization;

namespace Terraria.UtilClient.Cheats.Categories.Misc;

public sealed class DeleteWorldItemsCheat : SyncRunActionCheat
{
	public override string Name => "Delete World Items";

	public override CheatCategory Category => CheatCategory.Misc;

	protected override void RunAction()
	{
		int deleted = NetHelper.DeleteAllItems();
		ChatLogging.LogInfoOrErrorIfCondition(ULang.TryGetPluralOrNoneByCount("UtilClientCheat.DeleteItems", deleted), deleted > 0);
	}
}
