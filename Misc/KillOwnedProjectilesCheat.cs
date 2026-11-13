using Terraria.UtilClient.Chat;
using Terraria.UtilClient.Cheats.Types;
using Terraria.UtilClient.Common.Helpers.Game;
using Terraria.UtilClient.Localization;

namespace Terraria.UtilClient.Cheats.Categories.Misc;

public class KillOwnedProjectilesCheat : SyncRunActionCheat
{
	public override string Name => "Kill Owned Projectiles";

	public override CheatCategory Category => CheatCategory.Misc;

	public override string CommandOverride => "killprojs";

	protected override void RunAction()
	{
		int killed = NetHelper.KillOwnedProjectiles();
		ChatLogging.LogInfoOrErrorIfCondition(ULang.TryGetPluralOrNoneByCount("UtilClientCheat.KillProjectiles", killed), killed > 0);
	}
}
