using System.ComponentModel.DataAnnotations;
using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.Visual;

public sealed class FullbrightCheat : ToggleCheat<FullbrightCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		[Range(0.0, 1.0)]
		public float Brightness = 0.5f;
	}

	public override string Name => "Fullbright";

	public override CheatCategory Category => CheatCategory.Visual;
}
