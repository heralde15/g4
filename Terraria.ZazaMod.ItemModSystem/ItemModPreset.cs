using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.IO;

namespace Terraria.ZazaMod.ItemModSystem;

public class ItemModPreset
{
	public const string PresetDataFileName = "_PresetData.json";

	public const string DefaultPresetName = "Default Item Mods Preset";

	public const string DefaultAuthor = "Terrarian";

	public List<ItemMods.ItemModData> Items;

	public string Name;

	public bool DataLocked;

	public string Author;

	public bool AutoApply;

	public string FilePath => Path.Combine(ItemMods.PresetsPath, Name);

	public string PresetFilePath => Path.Combine(ItemMods.PresetsPath, Name, "_PresetData.json");

	public ItemModPreset()
	{
		Items = new List<ItemMods.ItemModData>();
		Name = "Default Item Mods Preset";
		DataLocked = true;
		Author = "Terrarian";
		base._002Ector();
	}

	public ItemModPreset(string author = "Terrarian", bool isLocked = false, string name = null)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		Items = new List<ItemMods.ItemModData>();
		Name = "Default Item Mods Preset";
		DataLocked = true;
		Author = "Terrarian";
		base._002Ector();
		Author = author;
		DataLocked = isLocked;
		string folderName = CleanupName(author);
		if (name != null)
		{
			Name = name;
		}
		else
		{
			Name = folderName;
		}
		if (!Directory.Exists(FilePath))
		{
			Directory.CreateDirectory(FilePath);
		}
		else if (!DataLocked)
		{
			Main.NewText("A folder with the name " + name + " already exists. Please make a preset using a different character name.", Mods.errorColor);
			DataLocked = true;
			return;
		}
		if (!DataLocked)
		{
			SavePresetData();
		}
		if (!LoadPreset(folderName))
		{
			throw new Exception("Failed to load " + folderName + ". The data is missing or invalid.");
		}
	}

	public static string CleanupName(string name)
	{
		char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
		string text = "";
		if (name.Length > 20)
		{
			name = name.Substring(0, 20);
		}
		name = name.Replace(".", "_");
		name = name.Replace("*", "_");
		string text2 = name;
		foreach (char c in text2)
		{
			text += ((!Enumerable.Contains(invalidFileNameChars, c)) ? ((c != ' ') ? c : '_') : '-');
		}
		return text;
	}

	public bool RefreshPreset()
	{
		bool result = LoadPresetFromPath(FilePath);
		if (Program.IsMainThread && Main._isAsyncLoadComplete)
		{
			if (!Main.CreativeMenu._initialized)
			{
				Main.CreativeMenu.Initialize();
				Main.CreativeMenu._uiState.InitializePage();
			}
			Main.CreativeMenu._uiState._infiniteItemsWindow.UpdateContents();
		}
		return result;
	}

	public bool LoadPreset(string preset = "Default Item Mods Preset")
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		if (Path.Exists(FilePath))
		{
			try
			{
				string[] itemMods = Directory.GetFiles(FilePath);
				if (itemMods.Length != 0)
				{
					Items.Clear();
					string[] array = itemMods;
					int id = default(int);
					foreach (string itemMod in array)
					{
						if (itemMod.EndsWith(".json"))
						{
							string itemName = Path.GetFileNameWithoutExtension(itemMod);
							if (itemName != null && ItemID.Search.TryGetId(itemName, ref id))
							{
								Items.Add(GetSingleItemMod(itemMod, id, Name));
							}
						}
					}
					if (!DataLocked)
					{
						LoadPresetData();
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				Main.NewText($"Loading Item Mod Preset \"{preset}\" error!\n{ex}", Mods.errorColor);
				Mods.LogException(ex);
				Items.Clear();
				return false;
			}
		}
		return false;
	}

	public bool LoadPresetFromPath(string presetPath = "")
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		if (!string.IsNullOrEmpty(presetPath) && Path.Exists(presetPath))
		{
			try
			{
				string[] itemMods = Directory.GetFiles(presetPath);
				if (itemMods.Length != 0)
				{
					Items.Clear();
					string[] array = itemMods;
					int id = default(int);
					foreach (string itemMod in array)
					{
						string itemName = Path.GetFileNameWithoutExtension(itemMod);
						if (itemName != null && ItemID.Search.TryGetId(itemName, ref id))
						{
							Items.Add(GetSingleItemMod(itemMod, id, Name));
						}
					}
					if (!DataLocked)
					{
						LoadPresetData();
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				Main.NewText($"Loading Item Mod Preset error!\n{ex}", Mods.errorColor);
				Mods.LogException(ex);
				Items.Clear();
				return false;
			}
		}
		return false;
	}

	public void Delete()
	{
		Directory.Delete(FilePath, recursive: true);
	}

	public void LoadPresetData()
	{
		if (!DataLocked)
		{
			Preferences preferences = new Preferences(PresetFilePath);
			if (!preferences.Load())
			{
				throw new IOException($"Preset is invalid - {Name} is missing the {"_PresetData.json"} file.");
			}
			if (!preferences.Contains("Author"))
			{
				throw new IOException("Preset is invalid - Preset data for " + Name + " does not contain the Author field.");
			}
			preferences.Get("Author", ref Author);
			if (Author.Length > 40)
			{
				Author = Author.Substring(0, 40);
			}
			if (!preferences.Contains("TiedToCharacter"))
			{
				throw new IOException("Preset is invalid - Preset data for " + Name + " does not contain the TiedToCharacter bool.");
			}
			preferences.Get("TiedToCharacter", ref AutoApply);
		}
	}

	public void SavePresetData()
	{
		if (!DataLocked)
		{
			Preferences preferences = new Preferences(PresetFilePath);
			preferences.Put("Author", Author);
			preferences.Put("TiedToCharacter", AutoApply);
			preferences.Save();
		}
	}

	public static ItemMods.ItemModData GetSingleItemMod(string path, int id, string name)
	{
		//IL_08a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Unknown result type (might be due to invalid IL or missing references)
		ItemMods.ItemModData data = new ItemMods.ItemModData((short)id);
		Preferences itemMod = new Preferences(path);
		if (!itemMod.Load())
		{
			return data;
		}
		if (itemMod.Contains("Damage"))
		{
			data.Damage = itemMod.Get("Damage", -1);
		}
		if (itemMod.Contains("CritChance"))
		{
			data.CritChance = itemMod.Get("CritChance", 0);
		}
		if (itemMod.Contains("Knockback"))
		{
			data.Knockback = itemMod.Get("Knockback", 0f);
		}
		if (itemMod.Contains("Scale"))
		{
			data.Scale = itemMod.Get("Scale", 1f);
		}
		if (itemMod.Contains("ManaCost"))
		{
			data.ManaCost = itemMod.Get("ManaCost", 0);
		}
		if (itemMod.Contains("UseTime"))
		{
			data.UseTime = itemMod.Get("UseTime", 20);
		}
		if (itemMod.Contains("AnimationTime"))
		{
			data.AnimationTime = itemMod.Get("AnimationTime", 20);
		}
		if (itemMod.Contains("UseStyle"))
		{
			data.UseStyle = itemMod.Get("UseStyle", 0);
		}
		if (itemMod.Contains("HoldStyle"))
		{
			data.HoldStyle = itemMod.Get("HoldStyle", 0);
		}
		if (itemMod.Contains("AutoReusable"))
		{
			itemMod.Get("AutoReusable", ref data.AutoReusable);
		}
		if (itemMod.Contains("TurnWhileUsing"))
		{
			itemMod.Get("TurnWhileUsing", ref data.TurnWhileUsing);
		}
		if (itemMod.Contains("ChannelUse"))
		{
			itemMod.Get("ChannelUse", ref data.ChannelUse);
		}
		if (itemMod.Contains("ReuseDelay"))
		{
			data.ReuseDelay = itemMod.Get("ReuseDelay", 0);
		}
		if (itemMod.Contains("ShotProjectile"))
		{
			data.ShotProjectile = itemMod.Get("ShotProjectile", (short)0);
		}
		if (itemMod.Contains("ProjectileSpeed"))
		{
			data.ProjectileSpeed = itemMod.Get("ProjectileSpeed", 0f);
		}
		if (itemMod.Contains("AmmoUsed"))
		{
			data.AmmoUsed = itemMod.Get("AmmoUsed", AmmoID.None);
		}
		if (itemMod.Contains("AmmoType"))
		{
			data.AmmoType = itemMod.Get("AmmoType", AmmoID.None);
		}
		if (itemMod.Contains("NeverGoesInAmmoSlot"))
		{
			itemMod.Get("NeverGoesInAmmoSlot", ref data.NeverGoesInAmmoSlot);
		}
		if (itemMod.Contains("TimesToShoot"))
		{
			data.TimesToShoot = itemMod.Get("TimesToShoot", (ushort)1);
		}
		if (itemMod.Contains("ShootStyle"))
		{
			data.ShootStyle = itemMod.Get("ShootStyle", 0);
		}
		if (itemMod.Contains("ShootsEveryUse"))
		{
			itemMod.Get("ShootsEveryUse", ref data.ShootsEveryUse);
		}
		if (itemMod.Contains("HealedLife"))
		{
			data.HealedLife = itemMod.Get("HealedLife", 0);
		}
		if (itemMod.Contains("HealedMana"))
		{
			data.HealedMana = itemMod.Get("HealedMana", 0);
		}
		if (itemMod.Contains("BuffAdded"))
		{
			data.BuffAdded = itemMod.Get("BuffAdded", 0);
		}
		if (itemMod.Contains("BuffDuration"))
		{
			data.BuffDuration = itemMod.Get("BuffDuration", 0);
		}
		if (itemMod.Contains("PickaxePower"))
		{
			data.PickaxePower = itemMod.Get("PickaxePower", 0);
		}
		if (itemMod.Contains("AxePower"))
		{
			data.AxePower = itemMod.Get("AxePower", 0);
		}
		if (itemMod.Contains("HammerPower"))
		{
			data.HammerPower = itemMod.Get("HammerPower", 0);
		}
		if (itemMod.Contains("FishingPower"))
		{
			data.FishingPower = itemMod.Get("FishingPower", 0);
		}
		if (itemMod.Contains("BaitPower"))
		{
			data.BaitPower = itemMod.Get("BaitPower", 0);
		}
		if (itemMod.Contains("Rarity"))
		{
			data.Rarity = itemMod.Get("Rarity", 0);
		}
		if (itemMod.Contains("ExpertItem"))
		{
			itemMod.Get("ExpertItem", ref data.ExpertItem);
		}
		if (itemMod.Contains("OnlyUseableInExpert"))
		{
			itemMod.Get("OnlyUseableInExpert", ref data.OnlyUseableInExpert);
		}
		if (itemMod.Contains("IsQuestFish"))
		{
			itemMod.Get("IsQuestFish", ref data.IsQuestFish);
		}
		if (itemMod.Contains("UniqueItemStack"))
		{
			itemMod.Get("UniqueItemStack", ref data.UniqueItemStack);
		}
		if (itemMod.Contains("ConsumableStack"))
		{
			itemMod.Get("ConsumableStack", ref data.ConsumableStack);
		}
		if (itemMod.Contains("MaxStackSize"))
		{
			data.MaxStackSize = itemMod.Get("MaxStackSize", 1);
		}
		if (itemMod.Contains("SellValue"))
		{
			data.SellValue = itemMod.Get("SellValue", 0);
		}
		if (itemMod.Contains("CreatesTile"))
		{
			data.CreatesTile = itemMod.Get("CreatesTile", -1);
		}
		if (itemMod.Contains("CreatesWall"))
		{
			data.CreatesWall = itemMod.Get("CreatesWall", -1);
		}
		if (itemMod.Contains("PlacementStyle"))
		{
			data.PlacementStyle = itemMod.Get("PlacementStyle", 0);
		}
		if (itemMod.Contains("TileRangeModifier"))
		{
			data.TileRangeModifier = itemMod.Get("TileRangeModifier", 0);
		}
		if (itemMod.Contains("ItemIdForWand"))
		{
			data.ItemIdForWand = itemMod.Get("ItemIdForWand", -1);
		}
		if (itemMod.Contains("IsMelee"))
		{
			itemMod.Get("IsMelee", ref data.IsMelee);
		}
		if (itemMod.Contains("IsRanged"))
		{
			itemMod.Get("IsRanged", ref data.IsRanged);
		}
		if (itemMod.Contains("IsMagic"))
		{
			itemMod.Get("IsMagic", ref data.IsMagic);
		}
		if (itemMod.Contains("IsSummoner"))
		{
			itemMod.Get("IsSummoner", ref data.IsSummoner);
		}
		if (itemMod.Contains("IsSentry"))
		{
			itemMod.Get("IsSentry", ref data.IsSentry);
		}
		if (itemMod.Contains("IsDD2Sentry"))
		{
			itemMod.Get("IsDD2Sentry", ref data.IsDD2Sentry);
		}
		if (itemMod.Contains("ArmorHeadSlot"))
		{
			data.ArmorHeadSlot = itemMod.Get("ArmorHeadSlot", -1);
		}
		if (itemMod.Contains("ArmorBodySlot"))
		{
			data.ArmorBodySlot = itemMod.Get("ArmorBodySlot", -1);
		}
		if (itemMod.Contains("ArmorLegSlot"))
		{
			data.ArmorLegSlot = itemMod.Get("ArmorLegSlot", -1);
		}
		if (itemMod.Contains("IsAccessory"))
		{
			itemMod.Get("IsAccessory", ref data.IsAccessory);
		}
		if (itemMod.Contains("IsVanityItem"))
		{
			itemMod.Get("IsVanityItem", ref data.IsVanityItem);
		}
		if (itemMod.Contains("WingType"))
		{
			data.WingType = itemMod.Get("WingType", (sbyte)(-1));
		}
		if (itemMod.Contains("Defense"))
		{
			data.Defense = itemMod.Get("Defense", 0);
		}
		if (itemMod.Contains("LifeRegenBonus"))
		{
			data.LifeRegenBonus = itemMod.Get("LifeRegenBonus", 0);
		}
		if (itemMod.Contains("ManaBonus"))
		{
			data.ManaBonus = itemMod.Get("ManaBonus", 0);
		}
		if (itemMod.Contains("DoesNoMeleeDamage"))
		{
			itemMod.Get("DoesNoMeleeDamage", ref data.DoesNoMeleeDamage);
		}
		if (itemMod.Contains("NoUseGraphic"))
		{
			itemMod.Get("NoUseGraphic", ref data.NoUseGraphic);
		}
		if (itemMod.Contains("CanHitInvincibleNPCs"))
		{
			itemMod.Get("CanHitInvincibleNPCs", ref data.CanHitInvincibleNPCs);
		}
		if (itemMod.Contains("Color"))
		{
			Dictionary<string, byte> itemColor = new Dictionary<string, byte>();
			itemMod.Get("Color", ref itemColor);
			Color color = Color.Transparent;
			if (itemColor.TryGetValue("R", out var value))
			{
				((Color)(ref color)).R = value;
			}
			if (itemColor.TryGetValue("G", out value))
			{
				((Color)(ref color)).G = value;
			}
			if (itemColor.TryGetValue("B", out value))
			{
				((Color)(ref color)).B = value;
			}
			if (itemColor.TryGetValue("A", out value))
			{
				((Color)(ref color)).A = value;
			}
			data.Color = color;
			itemColor.Clear();
		}
		if (itemMod.Contains("CustomName"))
		{
			itemMod.Get("CustomName", ref data.CustomName);
		}
		data.IsValid = true;
		data.PresetName = name;
		return data;
	}

	public void SaveItemMod(Item item)
	{
		Preferences preferences = new Preferences(Path.Combine(FilePath, ItemID.Search.GetName(item.netID)) + ".json");
		if (!Directory.Exists(FilePath))
		{
			Directory.CreateDirectory(FilePath);
		}
		new Item(item.netID).Prefix(item.prefix);
		preferences.Put("Damage", item.damage);
		preferences.Put("CritChance", item.crit);
		preferences.Put("Knockback", item.knockBack);
		preferences.Put("Scale", item.scale);
		preferences.Put("ManaCost", item.mana);
		preferences.Put("UseTime", item.useTime);
		preferences.Put("AnimationTime", item.useAnimation);
		preferences.Put("UseStyle", item.useStyle);
		preferences.Put("HoldStyle", item.holdStyle);
		preferences.Put("AutoReusable", item.autoReuse);
		preferences.Put("TurnWhileUsing", item.useTurn);
		preferences.Put("ChannelUse", item.channel);
		preferences.Put("ReuseDelay", item.reuseDelay);
		preferences.Put("ShotProjectile", item.shoot);
		preferences.Put("ProjectileSpeed", item.shootSpeed);
		preferences.Put("AmmoUsed", item.useAmmo);
		preferences.Put("AmmoType", item.ammo);
		preferences.Put("NeverGoesInAmmoSlot", item.notAmmo);
		preferences.Put("TimesToShoot", item.timesToShoot);
		preferences.Put("ShootStyle", item.itemShootBehavior);
		preferences.Put("ShootsEveryUse", item.shootsEveryUse);
		preferences.Put("HealedLife", item.healLife);
		preferences.Put("HealedMana", item.healMana);
		preferences.Put("BuffAdded", item.buffType);
		preferences.Put("BuffDuration", item.buffTime);
		preferences.Put("PickaxePower", item.pick);
		preferences.Put("AxePower", item.axe);
		preferences.Put("HammerPower", item.hammer);
		preferences.Put("FishingPower", item.fishingPole);
		preferences.Put("BaitPower", item.bait);
		preferences.Put("Rarity", item.rare);
		preferences.Put("ExpertItem", item.expert);
		preferences.Put("OnlyUseableInExpert", item.expertOnly);
		preferences.Put("IsQuestFish", item.questItem);
		preferences.Put("UniqueItemStack", item.uniqueStack);
		preferences.Put("ConsumableStack", item.consumable);
		preferences.Put("MaxStackSize", item.maxStack);
		preferences.Put("SellValue", item.value);
		preferences.Put("CreatesTile", item.createTile);
		preferences.Put("CreatesWall", item.createWall);
		preferences.Put("PlacementStyle", item.placeStyle);
		preferences.Put("TileRangeModifier", item.tileBoost);
		preferences.Put("ItemIdForWand", item.tileWand);
		preferences.Put("IsMelee", item.melee);
		preferences.Put("IsRanged", item.ranged);
		preferences.Put("IsMagic", item.magic);
		preferences.Put("IsSummoner", item.summon);
		preferences.Put("IsSentry", item.sentry);
		preferences.Put("IsDD2Sentry", item.DD2Summon);
		preferences.Put("ArmorHeadSlot", item.headSlot);
		preferences.Put("ArmorBodySlot", item.bodySlot);
		preferences.Put("ArmorLegSlot", item.legSlot);
		preferences.Put("IsAccessory", item.accessory);
		preferences.Put("IsVanityItem", item.vanity);
		preferences.Put("WingType", item.wingSlot);
		preferences.Put("Defense", item.defense);
		preferences.Put("LifeRegenBonus", item.lifeRegen);
		preferences.Put("ManaBonus", item.manaIncrease);
		preferences.Put("DoesNoMeleeDamage", item.noMelee);
		preferences.Put("NoUseGraphic", item.noUseGraphic);
		preferences.Put("CanHitInvincibleNPCs", item.hitsInvincibleNpc);
		preferences.Put("Color", new Dictionary<string, byte>
		{
			{
				"R",
				((Color)(ref item.color)).R
			},
			{
				"G",
				((Color)(ref item.color)).G
			},
			{
				"B",
				((Color)(ref item.color)).B
			},
			{
				"A",
				((Color)(ref item.color)).A
			}
		});
		preferences.Put("CustomName", string.IsNullOrEmpty(item.NameOverride) ? "" : item.NameOverride);
		preferences.Save();
		ItemMods.ItemModData data = GetSingleItemMod(Path.Combine(FilePath, ItemID.Search.GetName(item.netID)) + ".json", item.netID, Name);
		if (!HasItem(item.netID))
		{
			Items.Add(data);
		}
		else
		{
			UpdateItemInlist(data);
		}
		if (!Main.CreativeMenu._initialized)
		{
			Main.CreativeMenu.Initialize();
			Main.CreativeMenu._uiState.InitializePage();
		}
		Main.CreativeMenu._uiState._infiniteItemsWindow.UpdateContents();
	}

	public bool HasItem(int netId)
	{
		string text = default(string);
		if (!ItemID.Search.TryGetName(netId, ref text))
		{
			return false;
		}
		for (int i = 0; i < Items.Count; i++)
		{
			if (Items[i].NetID == netId)
			{
				return true;
			}
		}
		return false;
	}

	public void UpdateItemInlist(ItemMods.ItemModData data)
	{
		for (int i = 0; i < Items.Count; i++)
		{
			if (Items[i].NetID == data.NetID)
			{
				Items[i] = data;
				break;
			}
		}
	}

	public void Apply()
	{
		for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
		{
			Item item = Main.LocalPlayer.inventory[i];
			RefreshAndApply(item);
		}
		for (int j = 0; j < Main.LocalPlayer.armor.Length; j++)
		{
			Item item2 = Main.LocalPlayer.armor[j];
			RefreshAndApply(item2);
		}
		for (int k = 0; k < Main.LocalPlayer.dye.Length; k++)
		{
			Item item3 = Main.LocalPlayer.dye[k];
			RefreshAndApply(item3);
		}
		for (int l = 0; l < Main.LocalPlayer.bank.item.Length; l++)
		{
			Item item4 = Main.LocalPlayer.bank.item[l];
			RefreshAndApply(item4);
		}
		for (int m = 0; m < Main.LocalPlayer.bank2.item.Length; m++)
		{
			Item item5 = Main.LocalPlayer.bank2.item[m];
			RefreshAndApply(item5);
		}
		for (int n = 0; n < Main.LocalPlayer.bank3.item.Length; n++)
		{
			Item item6 = Main.LocalPlayer.bank3.item[n];
			RefreshAndApply(item6);
		}
		for (int num = 0; num < Main.LocalPlayer.bank4.item.Length; num++)
		{
			Item item7 = Main.LocalPlayer.bank4.item[num];
			RefreshAndApply(item7);
		}
		for (int num2 = 0; num2 < Main.LocalPlayer.Loadouts.Length; num2++)
		{
			for (int num3 = 0; num3 < Main.LocalPlayer.Loadouts[num2].Armor.Length; num3++)
			{
				Item item8 = Main.LocalPlayer.Loadouts[num2].Armor[num3];
				RefreshAndApply(item8);
			}
			for (int num4 = 0; num4 < Main.LocalPlayer.Loadouts[num2].Dye.Length; num4++)
			{
				Item item9 = Main.LocalPlayer.Loadouts[num2].Dye[num4];
				RefreshAndApply(item9);
			}
		}
	}

	public void RefreshAndApply(Item item)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (HasItem(item.netID))
		{
			if (item.modifiedByServer)
			{
				Main.NewText("Warning: " + item.Name + " was modified by the server. Applying item mods to it will override those modifications. If you're sure about this, apply the item mod again.\nThe \"Modified By Server\" flag has been removed.", Mods.infoColor);
				item.modifiedByServer = false;
			}
			else
			{
				item.Refresh(onlyIfVariantChanged: false);
				ApplyToItem(item);
			}
		}
	}

	public void ApplyToItem(Item item)
	{
		if (!HasItem(item.netID))
		{
			return;
		}
		for (int i = 0; i < Items.Count; i++)
		{
			if (Items[i].NetID == item.netID)
			{
				Items[i].ApplyToItem(item);
				break;
			}
		}
	}

	public bool ShouldAutoApply()
	{
		if ((Main.LocalPlayer.name == Name || CleanupName(Main.LocalPlayer.name) == Name) && AutoApply)
		{
			return !DataLocked;
		}
		return false;
	}
}
