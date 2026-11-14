using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.ZazaMod.ItemModSystem;

public class ItemMods
{
	public struct ItemModData(short netId)
	{
		public short NetID = netId;

		public int? Damage = null;

		public int? CritChance = null;

		public float? Knockback = null;

		public float? Scale = null;

		public int? ManaCost = null;

		public int? UseTime = null;

		public int? AnimationTime = null;

		public int? UseStyle = null;

		public int? HoldStyle = null;

		public bool? AutoReusable = null;

		public bool? TurnWhileUsing = null;

		public bool? ChannelUse = null;

		public int? ReuseDelay = null;

		public int? ShotProjectile = null;

		public float? ProjectileSpeed = null;

		public int? AmmoUsed = null;

		public int? AmmoType = null;

		public bool? NeverGoesInAmmoSlot = null;

		public ushort? TimesToShoot = null;

		public int? ShootStyle = null;

		public bool? ShootsEveryUse = null;

		public int? HealedLife = null;

		public int? HealedMana = null;

		public int? BuffAdded = null;

		public int? BuffDuration = null;

		public int? PickaxePower = null;

		public int? AxePower = null;

		public int? HammerPower = null;

		public int? FishingPower = null;

		public int? BaitPower = null;

		public int? Rarity = null;

		public bool? ExpertItem = null;

		public bool? OnlyUseableInExpert = null;

		public bool? IsQuestFish = null;

		public bool? UniqueItemStack = null;

		public bool? ConsumableStack = null;

		public int? MaxStackSize = null;

		public int? SellValue = null;

		public int? CreatesTile = null;

		public int? CreatesWall = null;

		public int? PlacementStyle = null;

		public int? TileRangeModifier = null;

		public int? ItemIdForWand = null;

		public bool? IsMelee = null;

		public bool? IsRanged = null;

		public bool? IsMagic = null;

		public bool? IsSummoner = null;

		public bool? IsSentry = null;

		public bool? IsDD2Sentry = null;

		public int? ArmorHeadSlot = null;

		public int? ArmorBodySlot = null;

		public int? ArmorLegSlot = null;

		public bool? IsAccessory = null;

		public bool? IsVanityItem = null;

		public sbyte? WingType = null;

		public int? Defense = null;

		public int? LifeRegenBonus = null;

		public int? ManaBonus = null;

		public bool? DoesNoMeleeDamage = null;

		public bool? NoUseGraphic = null;

		public bool? CanHitInvincibleNPCs = null;

		public string CustomName = null;

		public Color? Color = null;

		public bool IsValid = false;

		public bool BelongsToDynamicCollection = false;

		public string PresetName = null;

		public bool FromOtherClient = false;

		public void SetDefaultValues()
		{
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			Damage = -1;
			CritChance = 0;
			Knockback = 0f;
			Scale = 1f;
			ManaCost = 0;
			UseTime = 20;
			AnimationTime = 20;
			UseStyle = 0;
			HoldStyle = 0;
			AutoReusable = false;
			TurnWhileUsing = false;
			ChannelUse = false;
			ReuseDelay = 0;
			ShotProjectile = 0;
			ProjectileSpeed = 0f;
			AmmoUsed = AmmoID.None;
			AmmoType = AmmoID.None;
			NeverGoesInAmmoSlot = false;
			TimesToShoot = (ushort)1;
			ShootStyle = 0;
			ShootsEveryUse = false;
			HealedLife = 0;
			HealedMana = 0;
			BuffAdded = 0;
			BuffDuration = 0;
			PickaxePower = 0;
			AxePower = 0;
			HammerPower = 0;
			FishingPower = 0;
			BaitPower = 0;
			Rarity = 0;
			ExpertItem = false;
			OnlyUseableInExpert = false;
			IsQuestFish = false;
			UniqueItemStack = false;
			ConsumableStack = false;
			MaxStackSize = 1;
			SellValue = 0;
			CreatesTile = -1;
			CreatesWall = -1;
			PlacementStyle = 0;
			TileRangeModifier = 0;
			ItemIdForWand = -1;
			IsMelee = false;
			IsRanged = false;
			IsMagic = false;
			IsSummoner = false;
			IsSentry = false;
			IsDD2Sentry = false;
			ArmorHeadSlot = -1;
			ArmorBodySlot = -1;
			ArmorLegSlot = -1;
			IsAccessory = false;
			IsVanityItem = false;
			WingType = (sbyte)(-1);
			Defense = 0;
			LifeRegenBonus = 0;
			ManaBonus = 0;
			DoesNoMeleeDamage = false;
			NoUseGraphic = false;
			CanHitInvincibleNPCs = false;
			CustomName = null;
			Color = Color.White;
		}

		public void ApplyToItem(Item item)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0782: Unknown result type (might be due to invalid IL or missing references)
			//IL_0787: Unknown result type (might be due to invalid IL or missing references)
			if (item.modifiedByServer)
			{
				Main.NewText("Warning: " + item.Name + " was modified by the server. Applying item mods to it will override those modifications. If you're sure about this, apply the item mod again.\nThe \"Modified By Server\" flag has been removed.", Mods.infoColor);
				item.modifiedByServer = false;
				return;
			}
			if (Damage.HasValue)
			{
				item.damage = Damage.Value;
			}
			if (CritChance.HasValue)
			{
				item.crit = CritChance.Value;
			}
			if (Knockback.HasValue)
			{
				item.knockBack = Knockback.Value;
			}
			if (Scale.HasValue)
			{
				item.scale = Scale.Value;
			}
			if (ManaCost.HasValue)
			{
				item.mana = ManaCost.Value;
			}
			if (UseTime.HasValue)
			{
				item.useTime = UseTime.Value;
			}
			if (AnimationTime.HasValue)
			{
				item.useAnimation = AnimationTime.Value;
			}
			if (UseStyle.HasValue)
			{
				item.useStyle = UseStyle.Value;
			}
			if (HoldStyle.HasValue)
			{
				item.holdStyle = HoldStyle.Value;
			}
			if (AutoReusable.HasValue)
			{
				item.autoReuse = AutoReusable.Value;
			}
			if (TurnWhileUsing.HasValue)
			{
				item.useTurn = TurnWhileUsing.Value;
			}
			if (ChannelUse.HasValue)
			{
				item.channel = ChannelUse.Value;
			}
			if (ReuseDelay.HasValue)
			{
				item.reuseDelay = ReuseDelay.Value;
			}
			if (ShotProjectile.HasValue)
			{
				item.shoot = ShotProjectile.Value;
			}
			if (ProjectileSpeed.HasValue)
			{
				item.shootSpeed = ProjectileSpeed.Value;
			}
			if (AmmoUsed.HasValue)
			{
				item.useAmmo = AmmoUsed.Value;
			}
			if (AmmoType.HasValue)
			{
				item.ammo = AmmoType.Value;
			}
			if (NeverGoesInAmmoSlot.HasValue)
			{
				item.notAmmo = NeverGoesInAmmoSlot.Value;
			}
			if (TimesToShoot.HasValue)
			{
				item.timesToShoot = TimesToShoot.Value;
			}
			if (ShootStyle.HasValue)
			{
				item.itemShootBehavior = ShootStyle.Value;
			}
			if (ShootsEveryUse.HasValue)
			{
				item.shootsEveryUse = ShootsEveryUse.Value;
			}
			if (HealedLife.HasValue)
			{
				item.healLife = HealedLife.Value;
			}
			if (HealedMana.HasValue)
			{
				item.healMana = HealedMana.Value;
			}
			if (BuffAdded.HasValue)
			{
				item.buffType = BuffAdded.Value;
			}
			if (BuffDuration.HasValue)
			{
				item.buffTime = BuffDuration.Value;
			}
			if (PickaxePower.HasValue)
			{
				item.pick = PickaxePower.Value;
			}
			if (AxePower.HasValue)
			{
				item.axe = AxePower.Value;
			}
			if (HammerPower.HasValue)
			{
				item.hammer = HammerPower.Value;
			}
			if (FishingPower.HasValue)
			{
				item.fishingPole = FishingPower.Value;
			}
			if (BaitPower.HasValue)
			{
				item.bait = BaitPower.Value;
			}
			if (Rarity.HasValue)
			{
				item.rare = Rarity.Value;
			}
			if (ExpertItem.HasValue)
			{
				item.expert = ExpertItem.Value;
			}
			if (OnlyUseableInExpert.HasValue)
			{
				item.expertOnly = OnlyUseableInExpert.Value;
			}
			if (IsQuestFish.HasValue)
			{
				item.questItem = IsQuestFish.Value;
			}
			if (UniqueItemStack.HasValue)
			{
				item.uniqueStack = UniqueItemStack.Value;
			}
			if (ConsumableStack.HasValue)
			{
				item.consumable = ConsumableStack.Value;
			}
			if (MaxStackSize.HasValue)
			{
				item.maxStack = MaxStackSize.Value;
			}
			if (SellValue.HasValue)
			{
				item.value = SellValue.Value;
			}
			if (CreatesTile.HasValue)
			{
				item.createTile = CreatesTile.Value;
			}
			if (CreatesWall.HasValue)
			{
				item.createWall = CreatesWall.Value;
			}
			if (PlacementStyle.HasValue)
			{
				item.placeStyle = PlacementStyle.Value;
			}
			if (TileRangeModifier.HasValue)
			{
				item.tileBoost = TileRangeModifier.Value;
			}
			if (ItemIdForWand.HasValue)
			{
				item.tileWand = ItemIdForWand.Value;
			}
			if (IsMelee.HasValue)
			{
				item.melee = IsMelee.Value;
			}
			if (IsRanged.HasValue)
			{
				item.ranged = IsRanged.Value;
			}
			if (IsMagic.HasValue)
			{
				item.magic = IsMagic.Value;
			}
			if (IsSummoner.HasValue)
			{
				item.summon = IsSummoner.Value;
			}
			if (IsSentry.HasValue)
			{
				item.sentry = IsSentry.Value;
			}
			if (IsDD2Sentry.HasValue)
			{
				item.DD2Summon = IsDD2Sentry.Value;
			}
			if (ArmorHeadSlot.HasValue)
			{
				item.headSlot = ArmorHeadSlot.Value;
			}
			if (ArmorBodySlot.HasValue)
			{
				item.bodySlot = ArmorBodySlot.Value;
			}
			if (ArmorLegSlot.HasValue)
			{
				item.legSlot = ArmorLegSlot.Value;
			}
			if (IsAccessory.HasValue)
			{
				item.accessory = IsAccessory.Value;
			}
			if (IsVanityItem.HasValue)
			{
				item.vanity = IsVanityItem.Value;
			}
			if (WingType.HasValue)
			{
				item.wingSlot = WingType.Value;
			}
			if (Defense.HasValue)
			{
				item.defense = Defense.Value;
			}
			if (LifeRegenBonus.HasValue)
			{
				item.lifeRegen = LifeRegenBonus.Value;
			}
			if (ManaBonus.HasValue)
			{
				item.manaIncrease = ManaBonus.Value;
			}
			if (DoesNoMeleeDamage.HasValue)
			{
				item.noMelee = DoesNoMeleeDamage.Value;
			}
			if (NoUseGraphic.HasValue)
			{
				item.noUseGraphic = NoUseGraphic.Value;
			}
			if (CanHitInvincibleNPCs.HasValue)
			{
				item.hitsInvincibleNpc = CanHitInvincibleNPCs.Value;
			}
			if (!string.IsNullOrWhiteSpace(CustomName))
			{
				item.SetNameOverride(CustomName);
			}
			if (Color.HasValue)
			{
				item.color = Color.Value;
			}
			item.ItemMod = this;
		}

		public string WriteToString(Item i)
		{
			MemoryStream memoryStream = new MemoryStream(new byte[1024]);
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write7BitEncodedInt(i.damage);
			binaryWriter.Write((byte)i.crit);
			binaryWriter.Write(i.knockBack);
			binaryWriter.Write((short)i.mana);
			binaryWriter.Write((short)i.useTime);
			binaryWriter.Write((short)i.useAnimation);
			binaryWriter.Write((sbyte)i.useStyle);
			binaryWriter.Write((sbyte)i.holdStyle);
			BitsByte b = (byte)0;
			b[0] = i.autoReuse;
			b[1] = i.useTurn;
			b[2] = i.channel;
			b[3] = i.notAmmo;
			b[4] = i.shootsEveryUse;
			binaryWriter.Write(b);
			binaryWriter.Write((short)i.reuseDelay);
			binaryWriter.Write((short)i.shoot);
			binaryWriter.Write(i.shootSpeed);
			binaryWriter.Write((short)i.useAmmo);
			binaryWriter.Write((short)i.ammo);
			binaryWriter.Write(i.timesToShoot);
			binaryWriter.Write((byte)i.itemShootBehavior);
			binaryWriter.Write7BitEncodedInt(i.healLife);
			binaryWriter.Write7BitEncodedInt(i.healMana);
			binaryWriter.Write((short)i.buffType);
			binaryWriter.Write7BitEncodedInt(i.buffTime);
			binaryWriter.Write((short)i.pick);
			binaryWriter.Write((short)i.axe);
			binaryWriter.Write((short)i.hammer);
			binaryWriter.Write((short)i.fishingPole);
			binaryWriter.Write((short)i.bait);
			binaryWriter.Write((sbyte)i.rare);
			BitsByte b2 = (byte)0;
			b2[0] = i.expert;
			b2[1] = i.expertOnly;
			b2[2] = i.questItem;
			b2[3] = i.uniqueStack;
			b2[4] = i.consumable;
			binaryWriter.Write(b2);
			binaryWriter.Write7BitEncodedInt(i.maxStack);
			binaryWriter.Write7BitEncodedInt(i.value);
			binaryWriter.Write((short)i.createTile);
			binaryWriter.Write((short)i.createWall);
			binaryWriter.Write((short)i.placeStyle);
			binaryWriter.Write((short)i.tileBoost);
			binaryWriter.Write((short)i.tileWand);
			BitsByte b3 = (byte)0;
			b3[0] = i.melee;
			b3[1] = i.ranged;
			b3[2] = i.magic;
			b3[3] = i.summon;
			b3[4] = i.sentry;
			b3[5] = i.noMelee;
			b3[6] = i.noUseGraphic;
			b3[7] = i.hitsInvincibleNpc;
			binaryWriter.Write(b3);
			binaryWriter.Write((short)i.headSlot);
			binaryWriter.Write((short)i.bodySlot);
			binaryWriter.Write((short)i.legSlot);
			BitsByte b4 = (byte)0;
			b4[0] = i.accessory;
			b4[1] = i.vanity;
			binaryWriter.Write(b4);
			binaryWriter.Write(i.wingSlot);
			binaryWriter.Write7BitEncodedInt(i.defense);
			binaryWriter.Write7BitEncodedInt(i.lifeRegen);
			binaryWriter.Write7BitEncodedInt(i.manaIncrease);
			binaryWriter.Write(string.IsNullOrEmpty(i.NameOverride) ? "" : ((i.NameOverride.Length > 80) ? i.NameOverride.Substring(0, 80) : i.NameOverride));
			binaryWriter.Write(i.ItemMod.FromOtherClient ? i.ItemMod.PresetName : Main.LocalPlayer.name);
			binaryWriter.Write7BitEncodedInt(i.stack);
			binaryWriter.Write(i.prefix);
			int pos = (int)binaryWriter.BaseStream.Position;
			BinaryReader r = new BinaryReader(memoryStream);
			r.BaseStream.Position = 0L;
			byte[] bytes = new byte[pos];
			for (int j = 0; j < pos; j++)
			{
				bytes[j] = r.ReadByte();
			}
			string b64 = Convert.ToBase64String(bytes);
			return b64.Length.ToString("D4") + b64;
		}
	}

	public static string PresetsPath = Path.Combine(Main.SavePath, "Item Mods");

	public static int ActivePresetIndex = 0;

	public static List<ItemModPreset> Presets = new List<ItemModPreset>
	{
		new ItemModPreset("None", isLocked: true, "Default Item Mods Preset")
	};

	public static ItemModPreset ActivePreset
	{
		get
		{
			if (ActivePresetIndex >= Presets.Count)
			{
				return Presets[0];
			}
			return Presets[ActivePresetIndex];
		}
	}

	public static string ActivePresetName => ActivePreset.Name;

	public static ItemModData FromString(string dataAsString, Item item)
	{
		ItemModData i = default(ItemModData);
		try
		{
			BinaryReader r = new BinaryReader(new MemoryStream(Convert.FromBase64String(dataAsString)));
			i.Damage = r.Read7BitEncodedInt();
			i.CritChance = r.ReadByte();
			i.Knockback = r.ReadSingle();
			i.ManaCost = r.ReadInt16();
			i.UseTime = r.ReadInt16();
			i.AnimationTime = r.ReadInt16();
			i.UseStyle = r.ReadSByte();
			i.HoldStyle = r.ReadSByte();
			BitsByte b = r.ReadByte();
			i.AutoReusable = b[0];
			i.TurnWhileUsing = b[1];
			i.ChannelUse = b[2];
			i.NeverGoesInAmmoSlot = b[3];
			i.ShootsEveryUse = b[4];
			i.ReuseDelay = r.ReadInt16();
			i.ShotProjectile = r.ReadInt16();
			i.ProjectileSpeed = r.ReadSingle();
			i.AmmoUsed = r.ReadInt16();
			i.AmmoType = r.ReadInt16();
			i.TimesToShoot = r.ReadUInt16();
			i.ShootStyle = r.ReadByte();
			i.HealedLife = r.Read7BitEncodedInt();
			i.HealedMana = r.Read7BitEncodedInt();
			i.BuffAdded = r.ReadInt16();
			i.BuffDuration = r.Read7BitEncodedInt();
			i.PickaxePower = r.ReadInt16();
			i.AxePower = r.ReadInt16();
			i.HammerPower = r.ReadInt16();
			i.FishingPower = r.ReadInt16();
			i.BaitPower = r.ReadInt16();
			i.Rarity = r.ReadSByte();
			BitsByte b2 = r.ReadByte();
			i.ExpertItem = b2[0];
			i.OnlyUseableInExpert = b2[1];
			i.IsQuestFish = b2[2];
			i.UniqueItemStack = b2[3];
			i.ConsumableStack = b2[4];
			i.MaxStackSize = r.Read7BitEncodedInt();
			i.SellValue = r.Read7BitEncodedInt();
			i.CreatesTile = r.ReadInt16();
			i.CreatesWall = r.ReadInt16();
			i.PlacementStyle = r.ReadInt16();
			i.TileRangeModifier = r.ReadInt16();
			i.ItemIdForWand = r.ReadInt16();
			BitsByte b3 = r.ReadByte();
			i.IsMelee = b3[0];
			i.IsRanged = b3[1];
			i.IsMagic = b3[2];
			i.IsSummoner = b3[3];
			i.IsSentry = b3[4];
			i.DoesNoMeleeDamage = b3[5];
			i.NoUseGraphic = b3[6];
			i.CanHitInvincibleNPCs = b3[7];
			i.ArmorHeadSlot = r.ReadInt16();
			i.ArmorBodySlot = r.ReadInt16();
			i.ArmorLegSlot = r.ReadInt16();
			BitsByte b4 = r.ReadByte();
			i.IsAccessory = b4[0];
			i.IsVanityItem = b4[1];
			i.WingType = r.ReadSByte();
			i.Defense = r.Read7BitEncodedInt();
			i.LifeRegenBonus = r.Read7BitEncodedInt();
			i.ManaBonus = r.Read7BitEncodedInt();
			string name = r.ReadString();
			if (!string.IsNullOrEmpty(name))
			{
				i.CustomName = name;
			}
			i.PresetName = r.ReadString();
			if (r.BaseStream.Position < r.BaseStream.Length)
			{
				item.stack = r.Read7BitEncodedInt();
				item.prefix = r.ReadByte();
			}
			i.IsValid = true;
			i.FromOtherClient = true;
			item.ItemMod = i;
			i.ApplyToItem(item);
		}
		catch
		{
		}
		return i;
	}

	public static void RefreshAllPresets()
	{
		try
		{
			string LastPresetName = ActivePresetName;
			Presets.RemoveRange(1, Presets.Count - 1);
			Presets[0].RefreshPreset();
			string[] presets = Directory.GetDirectories(PresetsPath);
			for (int i = 0; i < presets.Length; i++)
			{
				bool validDir = false;
				string[] files = Directory.GetFiles(presets[i]);
				for (int j = 0; j < files.Length; j++)
				{
					if (files[j] == Path.Combine(PresetsPath, presets[i], "_PresetData.json") && files[j] != Path.Combine(PresetsPath, Presets[0].Name, "_PresetData.json"))
					{
						validDir = true;
						break;
					}
				}
				if (validDir)
				{
					ItemModPreset preset = new ItemModPreset();
					preset.Name = presets[i].Substring(presets[i].LastIndexOf(Path.DirectorySeparatorChar) + 1);
					preset.DataLocked = false;
					if (preset.LoadPresetFromPath(presets[i]))
					{
						Presets.Add(preset);
					}
				}
			}
			for (int k = 0; k < Presets.Count; k++)
			{
				if (Presets[k].Name == LastPresetName)
				{
					ActivePresetIndex = k;
					break;
				}
			}
			if (ActivePresetIndex >= Presets.Count)
			{
				ActivePresetIndex = 0;
			}
			if (Program.IsMainThread && Main._isAsyncLoadComplete)
			{
				if (!Main.CreativeMenu._initialized)
				{
					Main.CreativeMenu.Initialize();
					Main.CreativeMenu._uiState.InitializePage();
				}
				Main.CreativeMenu._uiState._infiniteItemsWindow.UpdateContents();
			}
		}
		catch (Exception ex)
		{
			Main.NewText($"Refresh all presets error!\n{ex}");
			Mods.LogException(ex);
		}
	}

	public static bool AutoApplyPreset()
	{
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string folderedName = ItemModPreset.CleanupName(Main.LocalPlayer.name);
			for (int i = 0; i < Presets.Count; i++)
			{
				if ((Main.LocalPlayer.name == Presets[i].Name || folderedName == Presets[i].Name) && Presets[i].AutoApply && !Presets[i].DataLocked)
				{
					Presets[i].Apply();
					ActivePresetIndex = i;
					Main.NewText($"Auto-applied preset \"{Presets[i].Name}\" ({Presets[i].Items.Count} item mods).", Mods.zazasFavoriteColor);
					return true;
				}
			}
		}
		catch
		{
		}
		return false;
	}

	public static bool AddNewPreset(string name, out int newIndex)
	{
		newIndex = 0;
		if (PresetExists(name))
		{
			return false;
		}
		try
		{
			ItemModPreset preset = new ItemModPreset(name);
			if (preset.DataLocked)
			{
				return false;
			}
			Presets.Add(preset);
			newIndex = Presets.IndexOf(preset);
		}
		catch
		{
			return false;
		}
		return true;
	}

	public static bool PresetExists(string name)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return true;
			}
			string folderName = ItemModPreset.CleanupName(name);
			for (int i = 0; i < Presets.Count; i++)
			{
				if (Presets[i].Name == name || Presets[i].Name == folderName)
				{
					return true;
				}
			}
		}
		catch
		{
			return false;
		}
		return false;
	}

	public static bool DeleteCurrentPreset()
	{
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		string name = ActivePreset.Name;
		try
		{
			if (ActivePreset.DataLocked)
			{
				if (Directory.Exists(ActivePreset.FilePath))
				{
					ActivePreset.Delete();
				}
				ActivePreset.Items.Clear();
				ActivePreset.LoadPreset(Path.DirectorySeparatorChar + ActivePresetName);
				if (!Directory.Exists(ActivePreset.FilePath))
				{
					Directory.CreateDirectory(ActivePreset.FilePath);
				}
				return true;
			}
			if (Directory.Exists(ActivePreset.FilePath))
			{
				ActivePreset.Delete();
			}
			Presets.Remove(ActivePreset);
			if (ActivePresetIndex >= Presets.Count)
			{
				ActivePresetIndex = 0;
			}
			return true;
		}
		catch (Exception value)
		{
			Main.NewText($"Deleting Item Mod Preset \"{name}\" error!\n{value}", Mods.errorColor);
			return false;
		}
	}

	public static List<Item> GetEveryItemMod(bool dynamicCollection = false)
	{
		List<Item> items = new List<Item>();
		for (int i = 0; i < Presets.Count; i++)
		{
			ItemModPreset preset = Presets[i];
			for (int j = 0; j < preset.Items.Count; j++)
			{
				Item item = new Item(preset.Items[j].NetID);
				preset.Items[j].ApplyToItem(item);
				item.ItemMod.BelongsToDynamicCollection = dynamicCollection;
				items.Add(item);
			}
		}
		return items;
	}
}
