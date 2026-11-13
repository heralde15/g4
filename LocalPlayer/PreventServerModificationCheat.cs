using System;
using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.LocalPlayer;

public sealed class PreventServerModificationCheat : ToggleCheat<PreventServerModificationCheat.CheatSettings>
{
	[Flags]
	public enum PreventPacketTypes : short
	{
		None = 0,
		PlayerInfo = 1,
		Equipment = 2,
		Controls = 4,
		Health = 8,
		Mana = 0x10,
		Buffs = 0x20,
		Dodge = 0x40,
		Luck = 0x80,
		Zones = 0x100,
		ItemAnimation = 0x200,
		PlayerStealth = 0x400,
		TeleportEntity = 0x800,
		All = PlayerInfo | Equipment | Controls | Health | Mana | Buffs | Dodge | Luck | Zones | ItemAnimation | PlayerStealth
	}

	public sealed class CheatSettings : USettings
	{
		public bool SyncBack = true;

		public PreventPacketTypes PreventTypes = PreventPacketTypes.All;
	}

	public override string Name => "Prevent Server Modifications";

	public override CheatCategory Category => CheatCategory.LocalPlayer;

	public override string CommandOverride => "antiservermodif";
}
