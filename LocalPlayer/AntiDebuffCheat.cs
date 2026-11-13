using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.LocalPlayer;

public sealed class AntiDebuffCheat : ToggleCheat<AntiDebuffCheat.CheatSettings>
{
	public enum AntiDebuffMode
	{
		Ignore,
		NoAdd
	}

	public sealed class CheatSettings : USettings
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public AntiDebuffMode Mode;
	}

	public override string Name => "Anti Debuff";

	public override CheatCategory Category => CheatCategory.LocalPlayer;
}
