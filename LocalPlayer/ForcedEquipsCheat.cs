using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.LocalPlayer;

public class ForcedEquipsCheat : ToggleCheat<ForcedEquipsCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		public Dictionary<short, ushort> ItemTypesToAmount = new Dictionary<short, ushort>();
	}

	public override string Name => "Forced Equips";

	public override CheatCategory Category => CheatCategory.LocalPlayer;

	protected override void OnEnabled()
	{
		Filter();
	}

	protected override void OnReload()
	{
		Filter();
	}

	public void Apply()
	{
		Player player = Main.LocalPlayer;
		foreach (var (type, amount) in base.Settings.ItemTypesToAmount)
		{
			Item item = ContentSamples.ItemsByType[type];
			for (int i = 0; i < amount; i++)
			{
				player.GrantArmorBenefits(item, force: true);
				player.ApplyEquipFunctional(0, item, force: true);
			}
			if (type == 4743)
			{
				player.hasFootball = true;
			}
			if (type == 4760)
			{
				player.hasRaisableShield = true;
			}
		}
	}

	private static bool IsItemValid(int id)
	{
		if (id < 1 || id >= ItemID.Count)
		{
			return false;
		}
		Item item = new Item();
		item.SetDefaults(id);
		if (item.accessory)
		{
			return true;
		}
		if (item != null && !item.consumable && item.ammo == 0 && item.dye == 0 && item.makeNPC == 0 && item.bait == 0 && item.createTile == -1 && item.paint <= 0)
		{
			return item.paintCoating <= 0;
		}
		return false;
	}

	private void Filter()
	{
		Dictionary<short, ushort> validated = new Dictionary<short, ushort>();
		foreach (var (type, amount) in base.Settings.ItemTypesToAmount)
		{
			if (IsItemValid(type))
			{
				ushort validatedAmount = Math.Clamp(amount, (ushort)1, (ushort)500);
				validated.Add(type, validatedAmount);
			}
		}
		base.Settings.ItemTypesToAmount = validated;
	}
}
