using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.Items;
using Terraria.GameContent.UI.Chat;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.ZazaMod.ItemModSystem;

namespace Terraria.ZazaMod.UI.Elements;

public class ModsNewUIListItem : UIElement
{
	public Color _color;

	private string _name;

	public Func<bool> _DisableCondition;

	private Func<string> _HoverText;

	public ModsNewUIListItem(string name, Color color, Func<bool> disableCon = null, Func<string> hoverText = null)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		base._002Ector();
		_name = name;
		_color = color;
		_DisableCondition = ((disableCon != null) ? disableCon : ((Func<bool>)(() => false)));
		_HoverText = ((hoverText != null) ? hoverText : ((Func<string>)(() => "")));
		base.OnUpdate += UpdateHovering;
		base.OnMouseOver += MouseOver;
	}

	private void MouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		if (!_DisableCondition())
		{
			SoundEngine.PlaySound(12);
		}
	}

	private void UpdateHovering(UIElement element)
	{
		if (element.IsMouseHovering && !string.IsNullOrEmpty(_HoverText()))
		{
			Main.instance.MouseText(_HoverText(), 0, 0);
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		float num = 6f;
		base.DrawSelf(spriteBatch);
		CalculatedStyle dimensions = GetDimensions();
		float num2 = dimensions.Width + 1f;
		Vector2 val = new Vector2(dimensions.X, dimensions.Y);
		bool flag = PlayerInput.ListeningTrigger == _name;
		Vector2 baseScale = default(Vector2);
		((Vector2)(ref baseScale))._002Ector(0.8f);
		Color value = (flag ? Color.Gold : (base.IsMouseHovering ? Color.White : Color.Silver));
		value = Color.Lerp(value, Color.White, base.IsMouseHovering ? 0.5f : 0f);
		Color color = (_DisableCondition() ? _color.MultiplyRGBA(new Color(140, 140, 140)) : (base.IsMouseHovering ? _color : _color.MultiplyRGBA(new Color(180, 180, 180))));
		Vector2 position = val;
		Utils.DrawSettingsPanel(spriteBatch, position, num2, color);
		position.X += 8f;
		position.Y += 2f + num;
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, _name, position, value, 0f, Vector2.Zero, baseScale, num2);
		position.X -= 17f;
		string text = GenInput(_name);
		if (string.IsNullOrEmpty(text))
		{
			switch (_name)
			{
			case "Fill Selection":
				text = "<No Tile/Wall Held>";
				break;
			case "Remove From Selection":
				text = "<No Tile Held>";
				break;
			case "Seed":
				text = "<Unavailble On Servers>";
				break;
			case "Dungeon Position":
			case "Smashed Orb Count":
			case "Smashed Altar Count":
				text = "<Unavailble>";
				break;
			default:
				text = "<Unknown...>";
				break;
			}
			if (!flag)
			{
				((Color)(ref value))._002Ector(80, 80, 80);
			}
		}
		Vector2 stringSize = ChatManager.GetStringSize(FontAssets.ItemStack.Value, text, baseScale);
		((Vector2)(ref position))._002Ector(dimensions.X + dimensions.Width - stringSize.X - 10f, dimensions.Y + 2f + num);
		GlyphTagHandler.GlyphsScale = 0.85f;
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text, position, value, 0f, Vector2.Zero, baseScale, num2);
		GlyphTagHandler.GlyphsScale = 1f;
	}

	private string GenInput(string name)
	{
		try
		{
			switch (name)
			{
			case "Fullbright":
				return GetFullbrightPercentage();
			case "Item Drop Mode":
				return GetItemDropModeName();
			case "Buff Loss Mode":
				return GetPersistBuffModeName();
			case "Surface Friction":
				return GetFrictionOverrideName();
			case "Conveyer Belt Handling":
				return GetConveyerHandlingName();
			case "Force Dash Type":
				return GetDashTypeName();
			case "Luck Override":
				return GetDreamLuckModeName();
			case "Walking Particles":
				return GetWalkParticleName();
			case "Death Particle":
				return GetDeathParticleName();
			case "Death Effect":
				return GetDeathEffectName();
			case "Death Sound":
				return GetDeathSoundName();
			case "Lucy Messages":
				return GetLucyMessageModeName();
			case "Lucy Message on Death":
				return GetLucyDeathMessageModeName();
			case "Special Paint Pattern":
				return GetRainbowPaintName();
			case "Brush Pattern":
				return GetBrushPatternName();
			case "Force Soundtrack":
				return GetForcedSoundtrackName();
			case "Hitbox Style":
				return GetHitboxesTextureName();
			case "Lock-On Target":
				return LockOnHelper.targetPlayers ? "Players" : "NPCs";
			case "Packet Mode":
				return Mods.tilePacketMaxMode ? "Packet Maximum" : "Packet 100/ms";
			case "Selection Packet Type":
				return (!Mods.selectionTileSyncingMode) ? "Singular" : "Chunk";
			case "Fill Selection":
			case "Remove From Selection":
				return GetSelectionFillModeText();
			case "Brush Shape":
				return Mods.brushCircle ? "Circle" : "Square";
			case "Item Name":
				return ((Main.LocalPlayer.HeldItem.Name != "") ? Main.LocalPlayer.HeldItem.Name : "None") + (Mods.showIDs ? "" : (" (ID: " + Main.LocalPlayer.HeldItem.netID + ")"));
			case "Prefix":
				return ((Lang.prefix[Main.LocalPlayer.HeldItem.prefix].Value.Length > 0) ? Lang.prefix[Main.LocalPlayer.HeldItem.prefix].Value : "None") + " (ID: " + Main.LocalPlayer.HeldItem.prefix + ")";
			case "Use Style":
				return ((Main.LocalPlayer.HeldItem.useStyle >= 0 && Main.LocalPlayer.HeldItem.useStyle < ItemUseStyleID.Count) ? ItemUseStyleID.Search.GetName(Main.LocalPlayer.HeldItem.useStyle) : "None") + $" (ID: {Main.LocalPlayer.HeldItem.useStyle})";
			case "Projectile":
				return ((Lang.GetProjectileName(Main.LocalPlayer.HeldItem.shoot).Value != "") ? Lang.GetProjectileName(Main.LocalPlayer.HeldItem.shoot).Value : "None") + " (ID: " + Main.LocalPlayer.HeldItem.shoot + ")";
			case "Use Ammo":
				return ((Lang.GetItemName(Main.LocalPlayer.HeldItem.useAmmo).Value != "") ? Lang.GetItemName(Main.LocalPlayer.HeldItem.useAmmo).Value : "None") + " (ID: " + Main.LocalPlayer.HeldItem.useAmmo + ")";
			case "Shoot Style":
			{
				ItemShootBehavior shootStyle = (ItemShootBehavior)Main.LocalPlayer.HeldItem.itemShootBehavior;
				return ((shootStyle.GetType().GetEnumName(shootStyle) != null) ? shootStyle.GetType().GetEnumName(shootStyle) : "") + " (ID: " + Main.LocalPlayer.HeldItem.itemShootBehavior + ")";
			}
			case "Create Tile":
				return ((Main.LocalPlayer.HeldItem.createTile >= 0 && Main.LocalPlayer.HeldItem.createTile < TileID.Count) ? TileID.Search.GetName(Main.LocalPlayer.HeldItem.createTile) : "None") + $" (ID: {Main.LocalPlayer.HeldItem.createTile})";
			case "Create Wall":
				return ((Main.LocalPlayer.HeldItem.createWall > 0 && Main.LocalPlayer.HeldItem.createWall < WallID.Count) ? WallID.Search.GetName(Main.LocalPlayer.HeldItem.createWall) : "None") + $" (ID: {Main.LocalPlayer.HeldItem.createWall})";
			case "Place Style":
				return Main.LocalPlayer.HeldItem.placeStyle.ToString();
			case "World Name":
				return Main.worldName;
			case "World Size":
				return GetWorldSizeName();
			case "World Difficulty":
				return GetWorldDifficultyName();
			case "World ID":
				return Main.worldID.ToString();
			case "World GUID":
				return Main.ActiveWorldFileData.UniqueId.ToString();
			case "World Generator Version":
				return Main.ActiveWorldFileData.WorldGeneratorVersion.ToString();
			case "Seed":
				return (Main.netMode != 0) ? "" : ((WorldGen.currentWorldSeed.Length > 0) ? WorldGen.currentWorldSeed : "No Seed (World too old?)");
			case "World Evil":
				return WorldGen.crimson ? "Crimson" : "Corruption";
			case "World Infection":
				return GetWorldPurityStatus();
			case "Moon Phase":
				return GetMoonPhaseName();
			case "Moon Type":
				return Main.moonType.ToString();
			case "Spawn Position":
				return "X: " + Main.spawnTileX + " Y: " + Main.spawnTileY;
			case "Dungeon Position":
				return Mods.DoWeHaveDungeonPosition ? ("X: " + Main.dungeonX + " Y: " + Main.dungeonY) : "";
			case "Ice Background Style":
				return Main.iceBackStyle.ToString();
			case "Jungle Background Style":
				return Main.jungleBackStyle.ToString();
			case "Hell Background Style":
				return Main.hellBackStyle.ToString();
			case "World Surface":
				return "Y: " + Main.worldSurface;
			case "Rock Layer":
				return "Y: " + Main.rockLayer;
			case "Max Raining":
				return Main.maxRaining.ToString();
			case "Sandstorm Severity":
				return Sandstorm.IntendedSeverity.ToString("0.000");
			case "Invasion Type":
				return Main.invasionType.ToString();
			case "Enchanted Sundial Cooldown":
				return Main.sundialCooldown.ToString();
			case "Enchanted Moondial Cooldown":
				return Main.moondialCooldown.ToString();
			case "Copper Ore":
				return (WorldGen.SavedOreTiers.Copper == 7) ? "Copper" : ((WorldGen.SavedOreTiers.Copper == 166) ? "Tin" : "Unknown");
			case "Iron Ore":
				return (WorldGen.SavedOreTiers.Iron == 6) ? "Iron" : ((WorldGen.SavedOreTiers.Iron == 167) ? "Lead" : "Unknown");
			case "Silver Ore":
				return (WorldGen.SavedOreTiers.Silver == 9) ? "Silver" : ((WorldGen.SavedOreTiers.Silver == 168) ? "Tungsten" : "Unknown");
			case "Gold Ore":
				return (WorldGen.SavedOreTiers.Gold == 8) ? "Gold" : ((WorldGen.SavedOreTiers.Gold == 169) ? "Platinum" : "Unknown");
			case "Cobalt Ore":
				return (WorldGen.SavedOreTiers.Cobalt == 107) ? "Cobalt" : ((WorldGen.SavedOreTiers.Cobalt == 221) ? "Palladium" : "Unknown");
			case "Mythril Ore":
				return (WorldGen.SavedOreTiers.Mythril == 108) ? "Mythril" : ((WorldGen.SavedOreTiers.Mythril == 222) ? "Orichalcum" : "Unknown");
			case "Adamantite Ore":
				return (WorldGen.SavedOreTiers.Adamantite == 111) ? "Adamantite" : ((WorldGen.SavedOreTiers.Adamantite == 223) ? "Titanium" : "Unknown");
			case "Smashed Orb Count":
				return (Main.netMode == 0) ? WorldGen.shadowOrbCount.ToString() : "";
			case "Smashed Altar Count":
				return (Main.netMode == 0) ? WorldGen.altarCount.ToString() : "";
			case "Generating Hardmode":
				return WorldGen.IsGeneratingHardMode ? "True" : "False";
			case "Current Preset":
				return $"{ItemMods.ActivePresetName} (#{ItemMods.ActivePresetIndex})";
			case "Preset Author":
				return ItemMods.ActivePreset.Author;
			default:
				return "<Wrong UIElement...>";
			}
		}
		catch
		{
			return "Error!";
		}
	}

	private string GetDeathParticleName()
	{
		return Mods.deathParticleCosmeticId switch
		{
			-1 => "None", 
			0 => "Keybrand", 
			1 => "Flame Waders", 
			2 => "Stellar Tune", 
			3 => "WoF Goat Mount Flames", 
			4 => "Black Lightning", 
			5 => "Rainbow Rod Hit", 
			6 => "Black Lightning Small", 
			7 => "Stardust Punch", 
			8 => "Princess Weapon", 
			9 => "Paladin's Hammer", 
			10 => "Night's Edge", 
			11 => "Silver Bullet Sparkle", 
			12 => "True Night's Edge", 
			13 => "Excalibur", 
			14 => "True Excalibur", 
			15 => "Terra Blade", 
			16 => "Leaf Crystal Passive", 
			17 => "Leaf Crystal Shot", 
			18 => "Ash Tree Shake", 
			19 => "Pet Exchange", 
			20 => "Slap Hand SFX", 
			21 => "Flymeal SFX", 
			22 => "Gas Trap", 
			23 => "Item Transfer", 
			24 => "Shimmer Arrow", 
			25 => "Town Slime Transform", 
			26 => "Loadout Change", 
			27 => "Shimmer Block", 
			28 => "Digestion", 
			29 => "Waffle Iron SFX", 
			30 => "Poo Fly", 
			31 => "Shimmer Town NPC", 
			32 => "Shimmer Town NPC Send", 
			_ => Mods.deathParticleCosmeticId.ToString(), 
		};
	}

	private string GetDeathEffectName()
	{
		switch (Mods.deathGoreCosmeticId)
		{
		case -1:
			return "None";
		case 11:
		case 12:
		case 13:
			return "Cloud Smoke";
		case 16:
			return "Yellow Stars";
		case 17:
			return "Blue Stars";
		case -3:
			return "Yellow & Blue Stars";
		case 61:
		case 62:
		case 63:
			return "Smoke";
		case 99:
			return "Dark Smoke";
		case 276:
		case 277:
		case 278:
		case 279:
		case 280:
		case 281:
		case 282:
			return "Confetti";
		case 375:
		case 376:
		case 377:
			return "Blue Smoke";
		case 411:
			return "Tiny Bubbles";
		case 412:
			return "Small Bubbles";
		case 413:
			return "Medium Bubbles";
		case 414:
			return "Large Bubbles";
		case 415:
			return "Huge Bubbles";
		case 570:
		case 571:
		case 572:
			return "Music Notes";
		case 825:
		case 826:
		case 827:
			return "Lingering Smoke";
		case 861:
		case 862:
			return "Large Smoke";
		case 907:
			return "Pink Balloons";
		case 908:
			return "Blue Balloons";
		case 909:
			return "Green Balloons";
		case -2:
			return "All Balloons";
		case 1201:
			return "Chum";
		case 1218:
			return "Coins";
		default:
			return Mods.deathGoreCosmeticId.ToString();
		}
	}

	private string GetDeathSoundName()
	{
		return Mods.deathSoundCosmeticId switch
		{
			20 => "Slap Hand SFX", 
			21 => "Flymeal SFX", 
			29 => "Waffle Iron SFX", 
			_ => Mods.deathSoundCosmeticId.ToString(), 
		};
	}

	private string GetWalkParticleName()
	{
		return Mods.walkParticleType switch
		{
			0 => "None", 
			1 => "Aetherium Block", 
			2 => "Flames", 
			3 => "Flames (Dyed)", 
			_ => "Invalid", 
		};
	}

	private string GetLucyMessageModeName()
	{
		return Mods.lucyMessagesMode switch
		{
			0 => "No Override", 
			1 => "Forced Off", 
			2 => "Forced On", 
			3 => "Forced On (Everyone)", 
			_ => Mods.lucyMessagesMode.ToString(), 
		};
	}

	private string GetLucyDeathMessageModeName()
	{
		return Mods.lucyDeathMessageMode switch
		{
			0 => "Disabled", 
			1 => "Self", 
			2 => "Others", 
			3 => "Everyone", 
			_ => Mods.lucyDeathMessageMode.ToString(), 
		};
	}

	private string GetForcedSoundtrackName()
	{
		return Mods.forceSoundtrackId switch
		{
			0 => "No Override", 
			1 => "Terraria", 
			2 => "Terraria: Otherworld", 
			_ => Mods.forceSoundtrackId.ToString(), 
		};
	}

	private string GetFullbrightPercentage()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		switch (Mods.fullBrightLevel)
		{
		case 1:
			Mods.fullBrightBrightness = new Vector3(0.5f, 0.5f, 0.5f);
			return "50%";
		case 2:
			Mods.fullBrightBrightness = new Vector3(1f, 1f, 1f);
			return "100%";
		default:
			return "Off";
		}
	}

	private string GetHitboxesTextureName()
	{
		return Mods.hitBoxesTexture switch
		{
			1 => "Border w/ Overlay", 
			2 => "Flat Overlay", 
			_ => "Border", 
		};
	}

	private string GetItemDropModeName()
	{
		return Mods.deathItemDropMode switch
		{
			1 => "Duplicate Inventory", 
			2 => "Don't Drop Anything", 
			_ => "Drop Inventory (Vanilla)", 
		};
	}

	private string GetPersistBuffModeName()
	{
		return Mods.persistBuffsMode switch
		{
			1 => "Keep Buffs", 
			2 => "Keep Buffs (Long)", 
			3 => "Keep Debuffs", 
			4 => "Keep Debuffs (Long)", 
			5 => "Keep All", 
			_ => "Lose All (Vanilla)", 
		};
	}

	private string GetFrictionOverrideName()
	{
		return Mods.surfaceFrictionMode switch
		{
			1 => "None", 
			2 => "Icy", 
			3 => "No Friction", 
			_ => "Normal", 
		};
	}

	private string GetConveyerHandlingName()
	{
		return Mods.conveyerBeltMode switch
		{
			1 => "Ignored", 
			2 => "Force Clockwise", 
			3 => "Force Counter-Clockwise", 
			_ => "Normal", 
		};
	}

	private string GetRainbowPaintName()
	{
		return Mods.rainbowPaintType switch
		{
			0 => "Disabled", 
			1 => "Diagonal Rainbow", 
			2 => "Horizontal Rainbow", 
			3 => "Veritcal Rainbow", 
			4 => "Spiral Circles", 
			5 => "Swirly Rainbow", 
			6 => "Rainbow Rhombus", 
			7 => "Rainbow Rings", 
			8 => "Distorted Vortex", 
			9 => "Wavy Checkerboard", 
			10 => "Rainbow Diamonds", 
			11 => "Noisy Rainbow", 
			12 => "Swirly Rings", 
			13 => "Noisy Fractal", 
			14 => "Layered Swirls", 
			15 => "Chaotic Swirl", 
			16 => ModsNewUICheatPanel.amongUsSusString, 
			_ => $"{Mods.rainbowPaintType}", 
		};
	}

	private string GetBrushPatternName()
	{
		return Mods.brushPattern switch
		{
			0 => "Normal", 
			1 => "Alternating Dither", 
			2 => "Inverted Alternating Dither", 
			3 => "Two-Spaced H. Dither", 
			4 => "Inverted Two-Spaced H. Dither", 
			5 => "Three-Spaced H. Dither", 
			6 => "Inverted Three-Spaced H. Dither", 
			7 => "Two-Spaced V. Dither", 
			8 => "Inverted Two-Spaced V. Dither", 
			9 => "Three-Spaced V. Dither", 
			10 => "Inverted Three-Spaced V. Dither", 
			11 => "Right Diagonal", 
			12 => "Inverted Right Diagonal", 
			13 => "Left Diagonal", 
			14 => "Inverted Left Diagonal", 
			15 => "Alternating U-D Dither", 
			16 => "Inverted Alternating U-D Dither", 
			17 => "Right Three-Spaced Diagonal", 
			18 => "Inv. Right Three-Spaced Diagonal", 
			19 => "Left Three-Spaced Diagonal", 
			20 => "Inv. Left Three-Spaced Diagonal", 
			21 => "R-Alt. Two-Spaced U-D Dither", 
			22 => "Inv. R-Alt. Two-Spaced U-D Dither", 
			23 => "L-Alt. Two-Spaced U-D Dither", 
			24 => "Inv. L-Alt. Two-Spaced U-D Dither", 
			25 => "Horizontal Lines", 
			26 => "Inverted Horizontal Lines", 
			27 => "Vertical Lines", 
			28 => "Inverted Vertical Lines", 
			29 => "Cross-Stitch", 
			30 => "Every-Other", 
			31 => "Empty Diamonds", 
			32 => "Inverted Empty Diamonds", 
			33 => "Right Snakes", 
			34 => "Inverted Right Snakes", 
			35 => "Left Snakes", 
			36 => "Inverted Left Snakes", 
			37 => "Coins", 
			38 => "Inverted Coins", 
			39 => "Empty Squares", 
			40 => "Inverted Empty Squares", 
			52 => ModsNewUICheatPanel.amongUsSusString2, 
			_ => $"{Mods.brushPattern}", 
		};
	}

	private string GetDreamLuckModeName()
	{
		return Mods.dreamLuckMode switch
		{
			1 => "Max Vanilla Luck", 
			2 => "Dream Luck", 
			_ => "No Override", 
		};
	}

	private string GetDashTypeName()
	{
		return Mods.forceDashType switch
		{
			1 => Lang.GetItemNameValue(977), 
			2 => Lang.GetItemNameValue(3097), 
			3 => Lang.GetBuffName(172), 
			4 => "Unknown Dash Type (4)", 
			5 => "Crystal Assassin", 
			_ => "No Override", 
		};
	}

	private string GetSelectionFillModeText()
	{
		Player plr = Main.LocalPlayer;
		if (plr.HeldItem.createTile <= -1 && plr.HeldItem.createWall <= -1 && plr.HeldItem.type != 4262 && plr.HeldItem.type != 496)
		{
			return "";
		}
		return "(" + Main.LocalPlayer.HeldItem.Name + ")";
	}

	public static string GetWorldSizeName()
	{
		return Main.maxTilesX switch
		{
			4200 => Language.GetText("UI.WorldSizeSmall").Value, 
			5000 => "Pre-Alpha", 
			6400 => Language.GetText("UI.WorldSizeMedium").Value, 
			8400 => Language.GetText("UI.WorldSizeLarge").Value, 
			1800 => Lang.prefix[7].Value, 
			1750 => Language.GetText("Legacy Tiny").Value, 
			12800 => Language.GetText("Huge").Value, 
			_ => $"Custom: {Main.maxTilesX}x{Main.maxTilesY}", 
		};
	}

	public static string GetWorldDifficultyName(bool ignoreLegendary = false)
	{
		if (Main.GameMode == 3)
		{
			return Language.GetTextValue("UI.Creative");
		}
		int num = 1;
		switch (Main.GameMode)
		{
		case 1:
			num = 2;
			break;
		case 2:
			num = 3;
			break;
		}
		if (Main.getGoodWorld && !ignoreLegendary)
		{
			num++;
		}
		return num switch
		{
			2 => Language.GetTextValue("UI.Expert"), 
			3 => Language.GetTextValue("UI.Master"), 
			4 => Language.GetTextValue("UI.Legendary"), 
			_ => Language.GetTextValue("UI.Normal"), 
		};
	}

	public static string GetWorldPurityStatus()
	{
		byte tBlood = WorldGen.tBlood;
		byte tEvil = WorldGen.tEvil;
		byte tGood = WorldGen.tGood;
		if (tGood > 0 && tEvil > 0 && tBlood > 0)
		{
			return tEvil + "% Corrupted & " + tBlood + "% Crimson & " + tGood + "% Hallowed";
		}
		if (tGood > 0 && tEvil > 0)
		{
			return tEvil + "% Corrupted & " + tGood + "% Hallowed";
		}
		if (tGood > 0 && tBlood > 0)
		{
			return tBlood + "% Crimson & " + tGood + "% Hallowed";
		}
		if (tEvil > 0 && tBlood > 0)
		{
			return tEvil + "% Corrupted & " + tBlood + "% Crimson";
		}
		if (tEvil > 0)
		{
			return tEvil + "% Corrupted";
		}
		if (tBlood > 0)
		{
			return tBlood + "% Crimson";
		}
		if (tGood > 0)
		{
			return tGood + "% Hallowed";
		}
		if (tGood <= 0 && tEvil <= 0 && tBlood <= 0)
		{
			return "100% Pure";
		}
		return "World Purity Status Is Unknown";
	}

	public static string GetMoonPhaseName()
	{
		if (Main.moonPhase == 0)
		{
			return Language.GetTextValue("GameUI.FullMoon");
		}
		if (Main.moonPhase == 1)
		{
			return Language.GetTextValue("GameUI.WaningGibbous");
		}
		if (Main.moonPhase == 2)
		{
			return Language.GetTextValue("GameUI.ThirdQuarter");
		}
		if (Main.moonPhase == 3)
		{
			return Language.GetTextValue("GameUI.WaningCrescent");
		}
		if (Main.moonPhase == 4)
		{
			return Language.GetTextValue("GameUI.NewMoon");
		}
		if (Main.moonPhase == 5)
		{
			return Language.GetTextValue("GameUI.WaxingCrescent");
		}
		if (Main.moonPhase == 6)
		{
			return Language.GetTextValue("GameUI.FirstQuarter");
		}
		if (Main.moonPhase == 7)
		{
			return Language.GetTextValue("GameUI.WaxingGibbous");
		}
		return "Invalid";
	}
}
