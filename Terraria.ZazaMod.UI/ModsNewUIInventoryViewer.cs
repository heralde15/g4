using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.ZazaMod.UI.Elements;

namespace Terraria.ZazaMod.UI;

public class ModsNewUIInventoryViewer : UIState
{
	private UIElement _bestiarySpace;

	private ModsNewUIInventoryViewerButtons _infoButtons;

	public static ModsNewUIInventoryViewer instance;

	public static int targetPlayer = Main.myPlayer;

	public static int temporaryLoadoutIndex = -1;

	private UIPanel _backPanel;

	private UIElement _outerContainer;

	public static bool mousedOver;

	public static Player ourPlayer = new Player();

	public bool IsActive => Main.player[targetPlayer].active;

	public override void OnInitialize()
	{
		BuildPage();
	}

	private void BuildPage()
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		RemoveAllChildren();
		int num = Utils.ToInt(value: true) * 100;
		UIElement uIElement = new UIElement();
		uIElement.Width.Set(0f, 0.875f);
		uIElement.MaxWidth.Set(800f + (float)num, 0f);
		uIElement.MinWidth.Set(600f + (float)num, 0f);
		uIElement.Top.Set(120f, 0f);
		uIElement.Height.Set(-120f, 1f);
		uIElement.HAlign = 0.5f;
		_outerContainer = uIElement;
		Append(uIElement);
		UIPanel uIPanel = new UIPanel();
		uIPanel.Width.Set(0f, 1f);
		uIPanel.Height.Set(-90f, 1f);
		uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
		uIElement.Append(uIPanel);
		uIPanel.PaddingTop -= 4f;
		uIPanel.PaddingBottom -= 4f;
		int num2 = 24;
		UIElement uIElement2 = new UIElement
		{
			Width = new StyleDimension(0f, 1f),
			Height = new StyleDimension(num2, 0f),
			VAlign = 0f
		};
		uIElement2.SetPadding(0f);
		uIPanel.Append(uIElement2);
		_backPanel = uIPanel;
		ModsNewUIInventoryViewerButtons uIBestiaryEntryInfoPage = new ModsNewUIInventoryViewerButtons
		{
			Height = new StyleDimension(12f, 1f),
			HAlign = 1f
		};
		int num3 = 4;
		UIElement uIElement3 = new UIElement
		{
			Width = new StyleDimension(0f, 1f),
			Height = new StyleDimension(-num2 - 6 - num3, 1f),
			VAlign = 1f,
			Top = new StyleDimension(-num3, 0f)
		};
		uIElement3.SetPadding(0f);
		uIPanel.Append(uIElement3);
		UIElement uIElement5 = new UIElement
		{
			Width = new StyleDimension(-12f - uIBestiaryEntryInfoPage.Width.Pixels, 1f),
			Height = new StyleDimension(-4f, 1f),
			VAlign = 1f
		};
		uIElement3.Append(uIElement5);
		uIElement5.SetPadding(0f);
		_bestiarySpace = uIElement5;
		uIElement3.Append(uIBestiaryEntryInfoPage);
		_infoButtons = uIBestiaryEntryInfoPage;
		AddBackAndForwardButtons(uIElement2);
		MakeExitButton(uIElement2);
	}

	private void AddBackAndForwardButtons(UIElement innerTopContainer)
	{
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		UIImageButton uIImageButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Back", (AssetRequestMode)1));
		uIImageButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Border", (AssetRequestMode)1));
		uIImageButton.SetVisibility(1f, 1f);
		uIImageButton.OnLeftClick += UIImageButton_OnLeftClick;
		innerTopContainer.Append(uIImageButton);
		UIImageButton uIImageButton2 = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Forward", (AssetRequestMode)1))
		{
			Left = new StyleDimension(uIImageButton.Width.Pixels + 1f, 0f)
		};
		uIImageButton2.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Border", (AssetRequestMode)1));
		uIImageButton2.SetVisibility(1f, 1f);
		uIImageButton2.OnLeftClick += UIImageButton2_OnLeftClick;
		innerTopContainer.Append(uIImageButton2);
		UIPanel uIPanel = new UIPanel
		{
			Left = new StyleDimension(uIImageButton.Width.Pixels + 1f + uIImageButton2.Width.Pixels + 3f, 0f),
			Width = new StyleDimension(135f, 0f),
			Height = new StyleDimension(0f, 1f),
			VAlign = 0.5f
		};
		uIPanel.BackgroundColor = new Color(35, 40, 83);
		uIPanel.BorderColor = new Color(35, 40, 83);
		uIPanel.SetPadding(0f);
		innerTopContainer.Append(uIPanel);
		UIText uIText = new UIText("< Previous   Next >", 0.8f)
		{
			HAlign = 0.5f,
			VAlign = 0.5f
		};
		uIPanel.Append(uIText);
	}

	private void UIImageButton2_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		temporaryLoadoutIndex = -1;
		SoundEngine.PlaySound(12);
		FindNextPlayer();
		_infoButtons.UpdateNameplateAndFlavorText();
	}

	private void UIImageButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		temporaryLoadoutIndex = -1;
		SoundEngine.PlaySound(12);
		FindNextPlayer(back: true);
		_infoButtons.UpdateNameplateAndFlavorText();
	}

	private void FindNextPlayer(bool back = false)
	{
		int loopCount = 0;
		if (!back)
		{
			while (true)
			{
				targetPlayer++;
				if (targetPlayer > 254)
				{
					targetPlayer = 0;
				}
				if (!IsActive && string.IsNullOrEmpty(Main.player[targetPlayer].name))
				{
					loopCount++;
					if (loopCount > 255)
					{
						targetPlayer = Main.myPlayer;
						break;
					}
					continue;
				}
				break;
			}
			return;
		}
		targetPlayer--;
		while (true)
		{
			targetPlayer--;
			if (targetPlayer < 0)
			{
				targetPlayer = 254;
			}
			if (!IsActive && string.IsNullOrEmpty(Main.player[targetPlayer].name))
			{
				loopCount++;
				if (loopCount > 255)
				{
					targetPlayer = Main.myPlayer;
					break;
				}
				continue;
			}
			break;
		}
	}

	public override void LeftClick(UIMouseEvent evt)
	{
		base.LeftClick(evt);
		AttemptStoppingUsingSearchbar(evt);
	}

	public override void RightClick(UIMouseEvent evt)
	{
		base.RightClick(evt);
		AttemptStoppingUsingSearchbar(evt);
	}

	private void AttemptStoppingUsingSearchbar(UIMouseEvent evt)
	{
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (Mods.customUI2.CurrentState != instance)
		{
			mousedOver = false;
		}
		targetPlayer = Math.Clamp(targetPlayer, 0, 255);
		CloneFromIndex();
		_ = _outerContainer;
		if (_backPanel.IsMouseHovering)
		{
			Main.LocalPlayer.mouseInterface = true;
			mousedOver = true;
		}
	}

	private void SwitchOurPlayersLoadout()
	{
		ourPlayer.Loadouts[ourPlayer.CurrentLoadoutIndex].Swap(ourPlayer);
		ourPlayer.Loadouts[temporaryLoadoutIndex].Swap(ourPlayer);
		ourPlayer.CurrentLoadoutIndex = temporaryLoadoutIndex;
		ourPlayer.CloneLoadouts(Main.player[targetPlayer].SafeSerializedClone());
	}

	private void CloneFromIndex()
	{
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[targetPlayer];
		ourPlayer.name = player.name;
		ourPlayer.joining = player.joining;
		ourPlayer.spawned = player.spawned;
		ourPlayer.team = player.team;
		ourPlayer.hostile = player.hostile;
		ourPlayer.difficulty = player.difficulty;
		ourPlayer.statLife = player.statLife;
		ourPlayer.statMana = player.statMana;
		ourPlayer.statLifeMax = player.statLifeMax;
		ourPlayer.statLifeMax2 = player.statLifeMax2;
		ourPlayer.statManaMax = player.statManaMax;
		ourPlayer.statLifeMax2 = player.statLifeMax2;
		ourPlayer.skinVariant = player.skinVariant;
		ourPlayer.eyeColor = player.eyeColor;
		ourPlayer.hairColor = player.hairColor;
		ourPlayer.hair = player.hair;
		ourPlayer.skinColor = player.skinColor;
		ourPlayer.shirtColor = player.shirtColor;
		ourPlayer.underShirtColor = player.underShirtColor;
		ourPlayer.shoeColor = player.shoeColor;
		ourPlayer.pantsColor = player.pantsColor;
		ourPlayer.hairDye = player.hairDye;
		ourPlayer.hairDyeColor = player.hairDyeColor;
		ourPlayer.hairDyeVar = player.hairDyeVar;
		ourPlayer.inventory = player.inventory;
		ourPlayer.buffType = player.buffType;
		ourPlayer.buffTime = player.buffTime;
		ourPlayer.selectedItem = player.selectedItem;
		Player clonedPlayer = player.SafeSerializedClone();
		ourPlayer.trashItem = player.trashItem.Clone();
		ourPlayer.Loadouts = clonedPlayer.Loadouts;
		ourPlayer.armor = (Item[])clonedPlayer.armor.Clone();
		ourPlayer.dye = (Item[])clonedPlayer.dye.Clone();
		ourPlayer.miscEquips = clonedPlayer.miscEquips;
		ourPlayer.miscDyes = clonedPlayer.miscDyes;
		ourPlayer.hideMisc = clonedPlayer.hideMisc;
		ourPlayer.hideVisibleAccessory = (bool[])clonedPlayer.hideVisibleAccessory.Clone();
		ourPlayer.CurrentLoadoutIndex = clonedPlayer.CurrentLoadoutIndex;
		if (temporaryLoadoutIndex != -1)
		{
			SwitchOurPlayersLoadout();
		}
		ourPlayer.wereWolf = player.wereWolf;
		ourPlayer.hideWolf = player.hideWolf;
		ourPlayer.forceWerewolf = player.forceWerewolf;
		ourPlayer.merman = player.merman;
		ourPlayer.hideMerman = player.hideMerman;
		ourPlayer.forceMerman = player.forceMerman;
		ourPlayer.IsZazaUser = player.IsZazaUser;
		ourPlayer.zone1 = player.zone1;
		ourPlayer.zone2 = player.zone2;
		ourPlayer.zone3 = player.zone3;
		ourPlayer.zone4 = player.zone4;
		ourPlayer.zone5 = player.zone5;
		ourPlayer.HasRgbMapOutline = player.HasRgbMapOutline;
		ourPlayer.HasRoaringKnightAfterimages = player.HasRoaringKnightAfterimages;
		ourPlayer.HasOutlineEffect = player.HasOutlineEffect;
		ourPlayer.hideVisibleAccessory[2] = player.hideVisibleAccessory[2];
		ourPlayer.zone1 = player.zone1;
		ourPlayer.zone2 = player.zone2;
		ourPlayer.zone3 = player.zone3;
		ourPlayer.zone4 = player.zone4;
		ourPlayer.zone5 = player.zone5;
		ourPlayer.extraAccessory = player.extraAccessory;
		ourPlayer.usedAegisCrystal = player.usedAegisCrystal;
		ourPlayer.usedAegisFruit = player.usedAegisFruit;
		ourPlayer.usedArcaneCrystal = player.usedArcaneCrystal;
		ourPlayer.usedAmbrosia = player.usedAmbrosia;
		ourPlayer.usedGalaxyPearl = player.usedGalaxyPearl;
		ourPlayer.usedGummyWorm = player.usedGummyWorm;
		ourPlayer.unlockedBiomeTorches = player.unlockedBiomeTorches;
		ourPlayer.UsingBiomeTorches = player.UsingBiomeTorches;
		ourPlayer.unlockedSuperCart = player.unlockedSuperCart;
		ourPlayer.UsingSuperCart = player.UsingSuperCart;
		ourPlayer.ateArtisanBread = player.ateArtisanBread;
		if (!IsActive || temporaryLoadoutIndex != -1)
		{
			ourPlayer.wereWolf = false;
			ourPlayer.hideWolf = false;
			ourPlayer.forceWerewolf = false;
			ourPlayer.merman = false;
			ourPlayer.hideMerman = false;
			ourPlayer.forceMerman = false;
			if (!IsActive)
			{
				ourPlayer.zone5 = (byte)0;
			}
		}
		else
		{
			if ((ourPlayer.wereWolf || ourPlayer.forceWerewolf) && !ourPlayer.hideWolf)
			{
				ourPlayer.legs = 20;
				ourPlayer.body = 21;
				ourPlayer.head = 38;
			}
			if ((ourPlayer.merman || ourPlayer.forceMerman) && !ourPlayer.hideMerman)
			{
				ourPlayer.head = 39;
				ourPlayer.legs = 21;
				ourPlayer.body = 22;
			}
		}
		ourPlayer.direction = 1;
		ourPlayer.gravDir = 1f;
		ourPlayer.socialIgnoreLight = true;
	}

	private void MakeExitButton(UIElement outerContainer)
	{
		ModsNewUIInventoryViewerButton button = new ModsNewUIInventoryViewerButton("")
		{
			Width = new StyleDimension(80f, 0f),
			Height = new StyleDimension(0f, 1f),
			VAlign = 0.5f,
			HAlign = 0.994f
		};
		button.OnMouseOver += FadedMouseOver;
		button.OnMouseOut += FadedMouseOut;
		UIText uIText = new UIText(Language.GetTextValue("UI.Back"))
		{
			VAlign = 0.5f,
			HAlign = 0.5f,
			IgnoresMouseInteraction = true
		};
		button.Append(uIText);
		button.OnLeftMouseDown += Click_GoBack;
		outerContainer.Append(button);
	}

	private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(11);
		Mods.customUI2.SetState(ModsNewUICheatPanel.uiStateNewCheatPanel);
		temporaryLoadoutIndex = -1;
	}

	private void DrawInventoryContents(SpriteBatch spriteBatch)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0815: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1243: Unknown result type (might be due to invalid IL or missing references)
		//IL_1245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d49: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_121b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1227: Unknown result type (might be due to invalid IL or missing references)
		//IL_122d: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1201: Unknown result type (might be due to invalid IL or missing references)
		//IL_1861: Unknown result type (might be due to invalid IL or missing references)
		//IL_1891: Unknown result type (might be due to invalid IL or missing references)
		//IL_1898: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_19bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_19cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_18fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_18fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1908: Unknown result type (might be due to invalid IL or missing references)
		//IL_190d: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a20: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a31: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_157b: Unknown result type (might be due to invalid IL or missing references)
		//IL_157d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1584: Unknown result type (might be due to invalid IL or missing references)
		//IL_158a: Unknown result type (might be due to invalid IL or missing references)
		//IL_145e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1465: Unknown result type (might be due to invalid IL or missing references)
		//IL_146f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1476: Unknown result type (might be due to invalid IL or missing references)
		//IL_1480: Unknown result type (might be due to invalid IL or missing references)
		//IL_1487: Unknown result type (might be due to invalid IL or missing references)
		//IL_1495: Unknown result type (might be due to invalid IL or missing references)
		//IL_149f: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1599: Unknown result type (might be due to invalid IL or missing references)
		//IL_159b: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fc: Unknown result type (might be due to invalid IL or missing references)
		float num = _backPanel.GetOuterDimensions().X;
		float num2 = _backPanel.Parent.Top.Pixels + 18f;
		int num3 = Main.screenWidth;
		int num4 = 0;
		int screenWidth = Main.screenWidth;
		int num6 = 0;
		float scale = ((Main.UIScale > 1f) ? Main.UIScale : 1f);
		int spacing = (int)(47f / scale);
		int scaledNum2 = (int)(num2 / scale);
		Main.inventoryScale = 0.85f / scale;
		new Vector2(num, num2);
		new Vector2((float)num3, (float)num4);
		new Vector2((float)screenWidth, (float)num6);
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				int num7 = (int)(20f + (float)(i * 56) * Main.inventoryScale) + (int)num;
				int num8 = (int)(20f + (float)(j * 56) * Main.inventoryScale) + (int)num2;
				int num9 = i + j * 10;
				new Color(100, 100, 100, 100);
				if (Main.mouseX >= num7 && (float)Main.mouseX <= (float)num7 + (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale && Main.mouseY >= num8 && (float)Main.mouseY <= (float)num8 + (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale && !PlayerInput.IgnoreMouseInterface)
				{
					Main.player[Main.myPlayer].mouseInterface = true;
					ItemSlot.OverrideHoverFavoriteAndShift(ourPlayer.inventory, 0, num9);
					if (ourPlayer.inventoryChestStack[num9] && (ourPlayer.inventory[num9].type == 0 || ourPlayer.inventory[num9].stack == 0))
					{
						ourPlayer.inventoryChestStack[num9] = false;
					}
					Main.instance.MouseText("", 0, 0);
					ItemSlot.MouseHover(ourPlayer.inventory, 0, num9);
					if (Main.cursorOverride == 2)
					{
						ItemSlot.LeftClick(ref ourPlayer.inventory[num9]);
					}
					if (Main.cursorOverride == 7 && targetPlayer != Main.myPlayer)
					{
						ItemSlot.LeftClickSteal(ref ourPlayer.inventory[num9]);
					}
				}
				if (i == 9)
				{
					i = 9;
				}
				if (j == 4)
				{
					j = 4;
				}
				ItemSlot.DrawAsOther(spriteBatch, ourPlayer.inventory, (ourPlayer.selectedItem != num9) ? 1 : 13, num9, new Vector2((float)num7, (float)num8), targetPlayer);
			}
		}
		int num9998 = (int)(20f + 504f * Main.inventoryScale) + (int)num;
		int num9999 = (int)(20f + 280f * Main.inventoryScale) + (int)num2;
		if (Main.mouseX >= num9998 && (float)Main.mouseX <= (float)num9998 + (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale && Main.mouseY >= num9999 && (float)Main.mouseY <= (float)num9999 + (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale && !PlayerInput.IgnoreMouseInterface)
		{
			Main.player[Main.myPlayer].mouseInterface = true;
			ItemSlot.OverrideHoverFavoriteAndShift(ourPlayer.trashItem, 6);
			Main.instance.MouseText("", 0, 0);
			ItemSlot.MouseHover(ref ourPlayer.trashItem, 6);
			if (Main.cursorOverride == 2)
			{
				ItemSlot.LeftClick(ref ourPlayer.trashItem, 6);
			}
			if (targetPlayer != Main.myPlayer)
			{
				ItemSlot.LeftClickSteal(ref ourPlayer.trashItem);
			}
		}
		ItemSlot.DrawAsOtherDirect(spriteBatch, ref ourPlayer.trashItem, 6, new Vector2((float)num9998, (float)num9999), ourPlayer);
		if (ourPlayer.selectedItem == 58)
		{
			int num10000 = (int)(20f + 560f * Main.inventoryScale) + (int)num;
			int num10001 = (int)(20f + num2);
			if (Main.mouseX >= num10000 && (float)Main.mouseX <= (float)num10000 + (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale && Main.mouseY >= num10001 && (float)Main.mouseY <= (float)num10001 + (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale && !PlayerInput.IgnoreMouseInterface)
			{
				Main.player[Main.myPlayer].mouseInterface = true;
				ItemSlot.OverrideHoverFavoriteAndShift(ourPlayer.inventory, 0, 58);
				if (ourPlayer.inventoryChestStack[58] && (ourPlayer.inventory[58].type == 0 || ourPlayer.inventory[58].stack == 0))
				{
					ourPlayer.inventoryChestStack[58] = false;
				}
				Main.instance.MouseText("", 0, 0);
				ItemSlot.MouseHover(ourPlayer.inventory, 0, 58);
				if (Main.cursorOverride == 2)
				{
					ItemSlot.LeftClick(ref ourPlayer.inventory[58]);
				}
				if (targetPlayer != Main.myPlayer)
				{
					ItemSlot.LeftClickSteal(ref ourPlayer.inventory[58]);
				}
			}
			ItemSlot.DrawAsOther(spriteBatch, ourPlayer.inventory, 13, 58, new Vector2((float)num10000, (float)num10001), targetPlayer);
		}
		Vector2 vector2 = FontAssets.MouseText.Value.MeasureString("Coins");
		Vector2 vector3 = FontAssets.MouseText.Value.MeasureString(Lang.inter[26].Value);
		float num10002 = vector2.X / vector3.X / scale;
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, Lang.inter[26].Value, new Vector2(num + 496f / scale, num2 + 84f / scale + (vector2.Y - vector2.Y * num10002) / 2f), new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor), 0f, default(Vector2), 0.75f * num10002, (SpriteEffects)0, 0f);
		Main.inventoryScale = 0.6f / scale;
		for (int k = 0; k < 4; k++)
		{
			int num10003 = (int)(num + 497f / scale);
			int num10004 = (int)num2 + (int)(85f / scale + (float)(k * 56) * Main.inventoryScale + 20f);
			int slot = k + 50;
			new Color(100, 100, 100, 100);
			if (Main.mouseX >= num10003 && (float)Main.mouseX <= (float)num10003 + (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale && Main.mouseY >= num10004 && (float)Main.mouseY <= (float)num10004 + (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale && !PlayerInput.IgnoreMouseInterface)
			{
				Main.player[Main.myPlayer].mouseInterface = true;
				ItemSlot.OverrideHoverFavoriteAndShift(ourPlayer.inventory, 1, slot);
				Main.instance.MouseText("", 0, 0);
				ItemSlot.MouseHover(ourPlayer.inventory, 1, slot);
				if (Main.cursorOverride == 2)
				{
					ItemSlot.LeftClick(ref ourPlayer.inventory[slot], 1);
				}
				if (targetPlayer != Main.myPlayer)
				{
					ItemSlot.LeftClickSteal(ref ourPlayer.inventory[slot]);
				}
			}
			ItemSlot.Draw(spriteBatch, ourPlayer.inventory, 1, slot, new Vector2((float)num10003, (float)num10004));
		}
		Vector2 vector4 = FontAssets.MouseText.Value.MeasureString("Ammo");
		Vector2 vector5 = FontAssets.MouseText.Value.MeasureString(Lang.inter[27].Value);
		float num10005 = vector4.X / vector5.X / scale;
		DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.Value, Lang.inter[27].Value, new Vector2(num + 532f / scale, num2 + 84f / scale + (vector4.Y - vector4.Y * num10005) / 2f), new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor), 0f, default(Vector2), 0.75f * num10005, (SpriteEffects)0, 0f);
		Main.inventoryScale = 0.6f / scale;
		for (int l = 0; l < 4; l++)
		{
			int num10006 = (int)(num + 534f / scale);
			int num10007 = (int)num2 + (int)(85f / scale + (float)(l * 56) * Main.inventoryScale + 20f);
			int slot2 = 54 + l;
			new Color(100, 100, 100, 100);
			if (Main.mouseX >= num10006 && (float)Main.mouseX <= (float)num10006 + (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale && Main.mouseY >= num10007 && (float)Main.mouseY <= (float)num10007 + (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale && !PlayerInput.IgnoreMouseInterface)
			{
				Main.player[Main.myPlayer].mouseInterface = true;
				ItemSlot.OverrideHoverFavoriteAndShift(ourPlayer.inventory, 2, slot2);
				Main.instance.MouseText("", 0, 0);
				ItemSlot.MouseHover(ourPlayer.inventory, 2, slot2);
				if (Main.cursorOverride == 2)
				{
					ItemSlot.LeftClick(ref ourPlayer.inventory[slot2], 2);
				}
				if (targetPlayer != Main.myPlayer)
				{
					ItemSlot.LeftClickSteal(ref ourPlayer.inventory[slot2]);
				}
			}
			ItemSlot.Draw(spriteBatch, ourPlayer.inventory, 2, slot2, new Vector2((float)num10006, (float)num10007));
		}
		Main.inventoryScale = 0.85f / scale;
		int num10008 = 8 + ourPlayer.GetAmountOfExtraAccessorySlotsToShow();
		int num10009 = 174;
		int num10010 = 4;
		if (Main.mouseX > Main.screenWidth - 64 - 28 && Main.mouseX < (int)((float)(Main.screenWidth - 64 - 28) + 56f * Main.inventoryScale) && Main.mouseY > num10009 && Main.mouseY < (int)((float)num10009 + 448f * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
		{
			Main.player[Main.myPlayer].mouseInterface = true;
		}
		float num10011 = Main.inventoryScale;
		bool flag4 = false;
		_ = num10008 - 1;
		bool flag5 = ourPlayer.CanDemonHeartAccessoryBeShown();
		bool flag6 = ourPlayer.CanMasterModeAccessoryBeShown();
		Color color = Main.inventoryBack;
		Color color2 = default(Color);
		((Color)(ref color2))._002Ector(80, 80, 80, 80);
		int num10012 = -1;
		Rectangle rectangle = default(Rectangle);
		for (int m = 0; m < 10; m++)
		{
			if ((m == 8 && !flag5) || (m == 9 && !flag6))
			{
				continue;
			}
			num10012++;
			if (!ourPlayer.IsItemSlotUnlockedAndUsable(m))
			{
				flag4 = true;
			}
			int num10013 = (int)num + 608;
			int num10014 = (int)(264f / scale + (float)scaledNum2 + (float)(num10012 * 56) * Main.inventoryScale);
			new Color(100, 100, 100, 100);
			int num10015 = (int)(num + 608f + 34f / scale);
			int num10016 = (int)(262f / scale + (float)scaledNum2 + (float)(num10012 * 56) * Main.inventoryScale);
			int context2 = 8;
			if (m > 2)
			{
				num10014 += num10010;
				num10016 += num10010;
				context2 = 10;
			}
			Texture2D value3 = TextureAssets.InventoryTickOn.Value;
			if (ourPlayer.hideVisibleAccessory[m])
			{
				value3 = TextureAssets.InventoryTickOff.Value;
			}
			((Rectangle)(ref rectangle))._002Ector(num10015, num10016, value3.Width, value3.Height);
			int num10017 = 0;
			if (m > 2 && ((Rectangle)(ref rectangle)).Contains(new Point(Main.mouseX, Main.mouseY)) && !PlayerInput.IgnoreMouseInterface)
			{
				Main.player[Main.myPlayer].mouseInterface = true;
				num10017 = ((!ourPlayer.hideVisibleAccessory[m]) ? 1 : 2);
			}
			else if (Main.mouseX >= num10013 && (float)Main.mouseX <= (float)num10013 + (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale && Main.mouseY >= num10014 && (float)Main.mouseY <= (float)num10014 + (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale && !PlayerInput.IgnoreMouseInterface)
			{
				Main.armorHide = true;
				Main.player[Main.myPlayer].mouseInterface = true;
				Main.instance.MouseText("", 0, 0);
				ItemSlot.OverrideHoverFavoriteAndShift(ourPlayer.armor, context2, m);
				ItemSlot.MouseHover(ourPlayer.armor, context2, m);
				if (Main.cursorOverride == 2)
				{
					ItemSlot.LeftClick(ref ourPlayer.armor[m], context2);
				}
				if (targetPlayer != Main.myPlayer)
				{
					ItemSlot.LeftClickSteal(ref ourPlayer.armor[m]);
				}
			}
			if (flag4)
			{
				Main.inventoryBack = color2;
			}
			ItemSlot.DrawAsOtherDirect(spriteBatch, ourPlayer.armor, context2, m, new Vector2((float)num10013, (float)num10014), ourPlayer);
			if (m > 2)
			{
				spriteBatch.Draw(value3, new Vector2((float)num10015, (float)num10016), Color.White * 0.7f);
				if (num10017 > 0)
				{
					Main.HoverItem = new Item();
					Main.hoverItemName = Lang.inter[58 + num10017].Value;
					Main.instance.MouseText(Main.hoverItemName, 0, 0);
				}
			}
		}
		Main.inventoryBack = color;
		if (Main.mouseX > Main.screenWidth - 64 - 28 - 47 && Main.mouseX < (int)((float)(Main.screenWidth - 64 - 20 - 47) + 56f * Main.inventoryScale) && Main.mouseY > num10009 && Main.mouseY < (int)((float)num10009 + 168f * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
		{
			Main.player[Main.myPlayer].mouseInterface = true;
		}
		num10012 = -1;
		for (int n = 10; n < 20; n++)
		{
			if ((n == 18 && !flag5) || (n == 19 && !flag6))
			{
				continue;
			}
			num10012++;
			bool num10018 = ourPlayer.IsItemSlotUnlockedAndUsable(n);
			flag4 = !num10018;
			if (!num10018)
			{
				_ = !Main.mouseItem.IsAir;
			}
			else
				_ = 0;
			int num10019 = (int)(num + 608f - (float)spacing);
			int num10020 = (int)(264f / scale + (float)scaledNum2 + (float)(num10012 * 56) * Main.inventoryScale);
			new Color(100, 100, 100, 100);
			if (n > 12)
			{
				num10020 += num10010;
			}
			int context3 = 9;
			if (n > 12)
			{
				context3 = 11;
			}
			if (Main.mouseX >= num10019 && (float)Main.mouseX <= (float)num10019 + (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale && Main.mouseY >= num10020 && (float)Main.mouseY <= (float)num10020 + (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale && !PlayerInput.IgnoreMouseInterface)
			{
				Main.player[Main.myPlayer].mouseInterface = true;
				Main.armorHide = true;
				Main.instance.MouseText("", 0, 0);
				ItemSlot.OverrideHoverFavoriteAndShift(ourPlayer.armor, context3, n);
				ItemSlot.MouseHover(ourPlayer.armor, context3, n);
				if (Main.cursorOverride == 2)
				{
					ItemSlot.LeftClick(ref ourPlayer.armor[n], context3);
				}
				if (targetPlayer != Main.myPlayer)
				{
					ItemSlot.LeftClickSteal(ref ourPlayer.armor[n]);
				}
			}
			if (flag4)
			{
				Main.inventoryBack = color2;
			}
			ItemSlot.DrawAsOtherDirect(spriteBatch, ourPlayer.armor, context3, n, new Vector2((float)num10019, (float)num10020), ourPlayer);
		}
		Main.inventoryBack = color;
		if (Main.mouseX > Main.screenWidth - 64 - 28 - 47 && Main.mouseX < (int)((float)(Main.screenWidth - 64 - 20 - 47) + 56f * Main.inventoryScale) && Main.mouseY > num10009 && Main.mouseY < (int)((float)num10009 + 168f * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
		{
			Main.player[Main.myPlayer].mouseInterface = true;
		}
		num10012 = -1;
		for (int num10021 = 0; num10021 < 10; num10021++)
		{
			if ((num10021 == 8 && !flag5) || (num10021 == 9 && !flag6))
			{
				continue;
			}
			num10012++;
			bool num10022 = ourPlayer.IsItemSlotUnlockedAndUsable(num10021);
			flag4 = !num10022;
			if (!num10022)
			{
				_ = !Main.mouseItem.IsAir;
			}
			else
				_ = 0;
			int num10023 = (int)(num + 608f - (float)spacing - (float)spacing);
			int num10024 = (int)(264f / scale + (float)scaledNum2 + (float)(num10012 * 56) * Main.inventoryScale);
			new Color(100, 100, 100, 100);
			if (num10021 > 2)
			{
				num10024 += num10010;
			}
			if (Main.mouseX >= num10023 && (float)Main.mouseX <= (float)num10023 + (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale && Main.mouseY >= num10024 && (float)Main.mouseY <= (float)num10024 + (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale && !PlayerInput.IgnoreMouseInterface)
			{
				Main.player[Main.myPlayer].mouseInterface = true;
				Main.armorHide = true;
				Main.instance.MouseText("", 0, 0);
				ItemSlot.OverrideHoverFavoriteAndShift(ourPlayer.dye, 12, num10021);
				ItemSlot.MouseHover(ourPlayer.dye, 12, num10021);
				if (Main.cursorOverride == 2)
				{
					ItemSlot.LeftClick(ref ourPlayer.dye[num10021], 12);
				}
				if (targetPlayer != Main.myPlayer)
				{
					ItemSlot.LeftClickSteal(ref ourPlayer.dye[num10021]);
				}
			}
			if (flag4)
			{
				Main.inventoryBack = color2;
			}
			ItemSlot.DrawAsOtherDirect(spriteBatch, ourPlayer.dye, 12, num10021, new Vector2((float)num10023, (float)num10024), ourPlayer);
		}
		Main.inventoryBack = color;
		Main.inventoryScale = num10011;
		Point value4 = default(Point);
		((Point)(ref value4))._002Ector(Main.mouseX, Main.mouseY);
		Item[] inv = ourPlayer.miscEquips;
		int num10025 = (int)num + 608 - spacing - spacing - spacing;
		int num10026 = (int)(311f / scale + (float)scaledNum2 + 0f * Main.inventoryScale);
		Rectangle r = default(Rectangle);
		((Rectangle)(ref r))._002Ector(0, 0, (int)((float)TextureAssets.InventoryBack.Width() * Main.inventoryScale), (int)((float)TextureAssets.InventoryBack.Height() * Main.inventoryScale));
		Rectangle r2 = default(Rectangle);
		for (int num10027 = 0; num10027 < 2; num10027++)
		{
			switch (num10027)
			{
			case 0:
				inv = ourPlayer.miscEquips;
				break;
			case 1:
				inv = ourPlayer.miscDyes;
				break;
			}
			r.X = num10025 + num10027 * -spacing;
			for (int num10028 = 0; num10028 < 5; num10028++)
			{
				int context4 = 0;
				int num10029 = -1;
				bool flag7 = false;
				switch (num10028)
				{
				case 0:
					context4 = 19;
					num10029 = 0;
					break;
				case 1:
					context4 = 20;
					num10029 = 1;
					break;
				case 2:
					context4 = 18;
					flag7 = ourPlayer.unlockedSuperCart;
					break;
				case 3:
					context4 = 17;
					break;
				case 4:
					context4 = 16;
					break;
				}
				if (num10027 == 1)
				{
					context4 = 33;
					num10029 = -1;
					flag7 = false;
				}
				r.Y = num10026 + num10028 * spacing;
				bool flag8 = false;
				Texture2D value5 = TextureAssets.InventoryTickOn.Value;
				((Rectangle)(ref r2))._002Ector((int)((float)((Rectangle)(ref r)).Left + 34f / scale), ((Rectangle)(ref r)).Top - 2, value5.Width, value5.Height);
				int num10030 = 0;
				if (num10029 != -1)
				{
					if (ourPlayer.hideMisc[num10029])
					{
						value5 = TextureAssets.InventoryTickOff.Value;
					}
					if (((Rectangle)(ref r2)).Contains(value4) && !PlayerInput.IgnoreMouseInterface)
					{
						Main.player[Main.myPlayer].mouseInterface = true;
						flag8 = true;
						num10030 = ((!ourPlayer.hideMisc[num10029]) ? 1 : 2);
					}
				}
				if (flag7)
				{
					value5 = TextureAssets.Extra[255].Value;
					if (!ourPlayer.enabledSuperCart)
					{
						value5 = TextureAssets.Extra[256].Value;
					}
					((Rectangle)(ref r2))._002Ector(r2.X + r2.Width / 2, r2.Y + r2.Height / 2, r2.Width, r2.Height);
					((Rectangle)(ref r2)).Offset(-r2.Width / 2, -r2.Height / 2);
					if (((Rectangle)(ref r2)).Contains(value4) && !PlayerInput.IgnoreMouseInterface)
					{
						Main.player[Main.myPlayer].mouseInterface = true;
						flag8 = true;
						num10030 = ((!ourPlayer.enabledSuperCart) ? 1 : 2);
					}
				}
				if (((Rectangle)(ref r)).Contains(value4) && !flag8 && !PlayerInput.IgnoreMouseInterface)
				{
					Main.player[Main.myPlayer].mouseInterface = true;
					Main.armorHide = true;
					Main.instance.MouseText("", 0, 0);
					ItemSlot.OverrideHoverFavoriteAndShift(inv, context4, num10028);
					ItemSlot.MouseHover(inv, context4, num10028);
					if (Main.cursorOverride == 2)
					{
						ItemSlot.LeftClick(ref inv[num10028], context4);
					}
					if (targetPlayer != Main.myPlayer)
					{
						ItemSlot.LeftClickSteal(ref inv[num10028]);
					}
				}
				ItemSlot.Draw(spriteBatch, inv, context4, num10028, r.TopLeft());
				if (num10029 != -1)
				{
					spriteBatch.Draw(value5, r2.TopLeft(), Color.White * 0.7f);
					if (num10030 > 0)
					{
						Main.HoverItem = new Item();
						Main.hoverItemName = Lang.inter[58 + num10030].Value;
						Main.instance.MouseText(Main.hoverItemName, 0, 0);
					}
				}
				if (flag7)
				{
					spriteBatch.Draw(value5, r2.TopLeft(), Color.White);
					if (num10030 > 0)
					{
						Main.HoverItem = new Item();
						Main.hoverItemName = Language.GetTextValue((num10030 == 1) ? "GameUI.SuperCartDisabled" : "GameUI.SuperCartEnabled");
						Main.instance.MouseText(Main.hoverItemName, 0, 0);
					}
				}
			}
		}
		num10026 += 247;
		num10025 += 8;
		int num10031 = (int)(num + 5f);
		int num10032 = (int)(264f + (float)scaledNum2 + 0f * Main.inventoryScale);
		int num10033 = -1;
		int num10034 = 0;
		int num10035 = 11;
		int num10036 = 38;
		int num10037 = 47;
		Point point = default(Point);
		for (int num10038 = 0; num10038 < Player.maxBuffs; num10038++)
		{
			if (ourPlayer.buffType[num10038] != 0)
			{
				int num10039 = num10034 % num10035;
				int num10040 = num10034 / num10035;
				((Point)(ref point))._002Ector(num10031 + num10039 * num10036, num10032 + num10040 * num10037);
				num10033 = Main.DrawBuffIconTarget(num10033, num10038, point.X, point.Y, targetPlayer);
				num10034++;
			}
		}
		if (num10033 >= 0)
		{
			int num10041 = ourPlayer.buffType[num10033];
			if (num10041 > 0)
			{
				string buffName = Lang.GetBuffName(num10041) + (Mods.showIDs ? (" (ID: " + num10041 + ")") : "");
				string buffTooltip = Main.GetBuffTooltip(ourPlayer, num10041);
				Main.instance.MouseText("", 0, 0);
				if (targetPlayer == Main.myPlayer && num10041 == 147)
				{
					Main.bannerMouseOver = true;
				}
				if (Main.meleeBuff.IndexInRange(num10041) && Main.meleeBuff[num10041])
				{
					Main.instance.MouseTextHackZoom(buffName, -10, 0, buffTooltip);
				}
				else
				{
					Main.instance.MouseTextHackZoom(buffName, buffTooltip);
				}
			}
		}
		int numm = 10;
		if (!flag5)
		{
			numm--;
		}
		if (!flag6)
		{
			numm--;
		}
		int x = (int)num + 618;
		int numm2 = (int)((float)(152 + scaledNum2 - 2) + 0f * Main.inventoryScale);
		int numm3 = (int)((float)(152 + scaledNum2 - 2) + (float)(numm * 56) * Main.inventoryScale);
		Texture2D valuee = TextureAssets.Extra[259].Value;
		Rectangle rectanglee = default(Rectangle);
		((Rectangle)(ref rectanglee))._002Ector(x, numm2 + 2, 4, numm3 - numm2);
		ItemSlot.GetLoadoutColor(ourPlayer.CurrentLoadoutIndex);
		int numm4 = ourPlayer.Loadouts.Length;
		int numm5 = 32;
		int numm6 = 4;
		_ = FontAssets.ItemStack.Value;
		Rectangle rectanglee2 = default(Rectangle);
		for (int num10042 = 0; num10042 < numm4; num10042++)
		{
			((Rectangle)(ref rectanglee2))._002Ector(rectanglee.X + rectanglee.Width, rectanglee.Y + (numm5 + numm6) * num10042, 32, numm5);
			Color loadoutColor = ItemSlot.GetLoadoutColor(num10042);
			_ = ourPlayer.Loadouts[num10042];
			int frameX = ((num10042 == ourPlayer.CurrentLoadoutIndex) ? 1 : 0);
			bool flag9 = false;
			if (((Rectangle)(ref rectanglee2)).Contains(Main.MouseScreen.ToPoint()))
			{
				flag9 = true;
				loadoutColor = Color.Lerp(loadoutColor, Color.White, 0.8f);
				Main.LocalPlayer.mouseInterface = true;
				if (!Main.mouseText)
				{
					Main.instance.MouseText(Language.GetTextValue("UI.Loadout" + (num10042 + 1)), 0, 0);
					Main.mouseText = true;
				}
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					if (num10042 == Main.player[targetPlayer].CurrentLoadoutIndex)
					{
						temporaryLoadoutIndex = -1;
						SoundEngine.PlaySound(12);
					}
					else
					{
						temporaryLoadoutIndex = num10042;
						SoundEngine.PlaySound(12);
						SwitchOurPlayersLoadout();
					}
				}
			}
			Rectangle rectanglee3 = valuee.Frame(3, 3, frameX, num10042);
			spriteBatch.Draw(valuee, ((Rectangle)(ref rectanglee2)).Center.ToVector2(), (Rectangle?)rectanglee3, Color.White, 0f, rectanglee3.Size() / 2f, 1f, (SpriteEffects)0, 0f);
			if (flag9)
			{
				rectanglee3 = valuee.Frame(3, 3, 2, num10042);
				spriteBatch.Draw(valuee, ((Rectangle)(ref rectanglee2)).Center.ToVector2(), (Rectangle?)rectanglee3, Main.OurFavoriteColor, 0f, rectanglee3.Size() / 2f, 1f, (SpriteEffects)0, 0f);
			}
		}
	}

	private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
		((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
	}

	private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
		((UIPanel)evt.Target).BorderColor = new Color(89, 116, 213, 255);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		DrawInventoryContents(spriteBatch);
	}

	public override void ScrollWheel(UIScrollWheelEvent evt)
	{
		base.ScrollWheel(evt);
	}
}
