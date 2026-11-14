using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.ZazaMod.UI.Elements;

public class ModsNewUICharacterListItem : UIPanel
{
	public Player ourPlayer;

	private Asset<Texture2D> _dividerTexture;

	private Asset<Texture2D> _innerPanelTexture;

	private Asset<Texture2D> _buttonTrashTexture;

	private Asset<Texture2D> _spectateButtonTexture;

	private UICharacter _playerPanel;

	private UIText _buttonLabel;

	public UIImageButton playerItem;

	private UIImageButton _trashButton;

	private UIImageButton _spectateButton;

	private UIImage _zazaIcon;

	public int playerIndex;

	private UIElement _goldenOutline;

	private UIElement _specOn;

	public bool IsActive
	{
		get
		{
			if (!Main.player[playerIndex].active)
			{
				if (Main.netMode == 1)
				{
					return playerIndex == 255;
				}
				return false;
			}
			return true;
		}
	}

	public ModsNewUICharacterListItem(int index)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		base._002Ector();
		BackgroundColor = new Color(63, 82, 151) * 0.7f;
		BorderColor = new Color(89, 116, 213) * 0.7f;
		_dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider", (AssetRequestMode)1);
		_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", (AssetRequestMode)1);
		_buttonTrashTexture = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", (AssetRequestMode)1);
		_spectateButtonTexture = Main.Assets.Request<Texture2D>("Images/Inventory_Tick_Off", (AssetRequestMode)1);
		Height.Set(70f, 0f);
		Width.Set(0f, 1f);
		SetPadding(6f);
		if (index < 0 || index > 255)
		{
			index = 0;
		}
		ourPlayer = new Player();
		playerIndex = index;
		CloneFromIndex();
		UpdateBackgroundColor();
		UIImageButton uIImageButton5 = new UIImageButton(_buttonTrashTexture)
		{
			VAlign = -0.05f,
			HAlign = 1f,
			MarginTop = 100000000f
		};
		uIImageButton5.OnLeftDoubleClick += TrashButtonDoubleClick;
		_trashButton = uIImageButton5;
		Append(uIImageButton5);
		UIImageButton uIImageButton6 = new UIImageButton(_spectateButtonTexture)
		{
			VAlign = 0.075f,
			HAlign = 0.95f,
			MarginTop = 100000000f
		};
		UIImage specOn = new UIImage(Main.Assets.Request<Texture2D>("Images/Inventory_Tick_On", (AssetRequestMode)1))
		{
			IgnoresMouseInteraction = true
		};
		_specOn = specOn;
		uIImageButton6.Append(_specOn);
		uIImageButton6.OnLeftClick += SpectateButtonLeftClick;
		uIImageButton6.OnRightClick += SpectateButtonRightClick;
		_spectateButton = uIImageButton6;
		Append(uIImageButton6);
		UIImage zazaIcon = new UIImage(Main.Assets.Request<Texture2D>("Images/Item_2737", (AssetRequestMode)1))
		{
			ImageScale = 0.75f,
			VAlign = -0.3f,
			HAlign = 0.92f,
			MarginTop = 100000000f
		};
		zazaIcon.Color = Color.Lerp(Color.White, Color.LightCyan, 0.5f);
		_zazaIcon = zazaIcon;
		Append(zazaIcon);
		_playerPanel = new UICharacter(ourPlayer);
		_playerPanel.Left.Set(4f, 0f);
		UIImage element = (UIImage)(_goldenOutline = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/IconCompletion", (AssetRequestMode)1))
		{
			HAlign = 0.5f,
			VAlign = 0.5f,
			Top = new StyleDimension(-10f, 0f),
			Left = new StyleDimension(-3f, 0f),
			IgnoresMouseInteraction = true
		});
		_playerPanel.Append(element);
		_playerPanel.OnLeftClick += delegate
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			if (IsActive && playerIndex != Main.myPlayer && PlayerInput.Triggers.JustReleased.MouseLeft && playerIndex != 255)
			{
				Main.LocalPlayer.position = Main.player[playerIndex].position;
				Main.LocalPlayer.oldPosition = Main.LocalPlayer.position;
				Main.LocalPlayer.velocity = Vector2.Zero;
				Main.LocalPlayer.fallStart = (int)Main.LocalPlayer.Bottom.Y / 16;
				Mods.Instance.OldCenterMod = Main.LocalPlayer.Center;
				Main.LocalPlayer.noFallDmg = true;
				NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
			}
		};
		if (playerIndex == Main.myPlayer)
		{
			_goldenOutline.MarginTop = 0f;
		}
		else
		{
			_goldenOutline.MarginTop = 100000000f;
		}
		Append(_playerPanel);
		base.OnLeftClick += OpenInvView;
	}

	private void CloneFromIndex()
	{
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[playerIndex];
		if (playerIndex != 255 || (playerIndex == 255 && Main.netMode != 1))
		{
			ourPlayer.name = player.name;
			ourPlayer.whoAmI = player.whoAmI;
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
			ourPlayer.armor = player.armor;
			ourPlayer.dye = player.dye;
			ourPlayer.miscEquips = player.miscEquips;
			ourPlayer.miscDyes = player.miscDyes;
			ourPlayer.hideMisc = player.hideMisc;
			ourPlayer.hideVisibleAccessory = player.hideVisibleAccessory;
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
			if (!IsActive)
			{
				ourPlayer.wereWolf = false;
				ourPlayer.hideWolf = false;
				ourPlayer.forceWerewolf = false;
				ourPlayer.merman = false;
				ourPlayer.hideMerman = false;
				ourPlayer.forceMerman = false;
				ourPlayer.zone5 = (byte)0;
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
		}
		else
		{
			ourPlayer.name = "Server";
			ourPlayer.statLifeMax = 1000000;
			ourPlayer.statLifeMax2 = ourPlayer.statLifeMax;
			ourPlayer.statLife = ourPlayer.statLifeMax;
			ourPlayer.statManaMax = 1000000;
			ourPlayer.statManaMax2 = ourPlayer.statManaMax;
			ourPlayer.statMana = ourPlayer.statManaMax;
		}
		ourPlayer.direction = 1;
		ourPlayer.gravDir = 1f;
		ourPlayer.socialIgnoreLight = true;
	}

	private void UpdateBackgroundColor()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		Color color = (Color)(IsActive ? Color.White : new Color(150, 150, 150));
		if (!base.IsMouseHovering)
		{
			BackgroundColor = ((IsActive && ourPlayer.IsZazaSpecial) ? Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.25f % 1f, 0.5f, 0.5f) : (new Color(63, 82, 151) * 0.7f));
			BorderColor = ((IsActive && ourPlayer.IsZazaSpecial) ? Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.25f % 1f, 0.85f, 0.5f) : (new Color(89, 116, 213) * 0.7f));
			if (!IsActive)
			{
				BackgroundColor = BackgroundColor.MultiplyRGB(color);
				((Color)(ref BackgroundColor)).A = (byte)((float)(int)((Color)(ref BackgroundColor)).A * 0.7f);
				BorderColor = BorderColor.MultiplyRGB(color);
				((Color)(ref BorderColor)).A = (byte)((float)(int)((Color)(ref BorderColor)).A * 0.7f);
			}
		}
		else
		{
			BackgroundColor = ((IsActive && ourPlayer.IsZazaSpecial) ? Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.25f % 1f, 0.5f, 0.5f) : (new Color(73, 94, 171) * 0.8f));
			BorderColor = ((IsActive && ourPlayer.IsZazaSpecial) ? Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.25f % 1f, 0.85f, 0.5f) : (new Color(89, 116, 213) * 0.8f));
			if (!IsActive)
			{
				BackgroundColor = BackgroundColor.MultiplyRGB(new Color(175, 175, 175));
				((Color)(ref BackgroundColor)).A = (byte)((float)(int)((Color)(ref BackgroundColor)).A * 0.8f);
				BorderColor = BorderColor.MultiplyRGB(new Color(175, 175, 175));
				((Color)(ref BorderColor)).A = (byte)((float)(int)((Color)(ref BorderColor)).A * 0.8f);
			}
		}
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		_playerPanel.SetAnimated(animated: true);
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		base.MouseOut(evt);
		_playerPanel.SetAnimated(animated: false);
	}

	private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		Color color = ((!IsActive) ? Color.Gray : (ourPlayer.IsZazaSpecial ? Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.25f % 1f, 1f, 0.5f) : Color.White));
		spriteBatch.Draw(_innerPanelTexture.Value, position, (Rectangle?)new Rectangle(0, 0, 8, _innerPanelTexture.Height()), color);
		spriteBatch.Draw(_innerPanelTexture.Value, new Vector2(position.X + 8f, position.Y), (Rectangle?)new Rectangle(8, 0, 8, _innerPanelTexture.Height()), color, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(_innerPanelTexture.Value, new Vector2(position.X + width - 8f, position.Y), (Rectangle?)new Rectangle(16, 0, 8, _innerPanelTexture.Height()), color);
	}

	public override void Update(GameTime gameTime)
	{
		try
		{
			base.Update(gameTime);
			if (Mods.spectatePlayerTarget == playerIndex && playerIndex != 255)
			{
				_specOn.MarginTop = 0f;
			}
			else
			{
				_specOn.MarginTop = 100000000f;
			}
			if (_playerPanel.IsMouseHovering && playerIndex != Main.myPlayer && IsActive && playerIndex != 255)
			{
				Main.instance.MouseText("Left click to teleport to this player.", 0, 0);
			}
			if (_zazaIcon.IsMouseHovering && playerIndex != Main.myPlayer && IsActive && playerIndex != 255)
			{
				Main.instance.MouseText("ZazaMod User", 0, 0);
			}
			if (_trashButton.IsMouseHovering && playerIndex != Main.myPlayer && IsActive && !ourPlayer.IsZazaUser)
			{
				Main.instance.MouseText("Double click to send a mobile-crashing whisper message.", 0, 0);
			}
			if (_spectateButton.IsMouseHovering && playerIndex != Main.myPlayer && IsActive && playerIndex != 255)
			{
				if (Mods.spectatePlayerTarget == playerIndex)
				{
					_specOn.MarginTop = 100000000f;
				}
				else
				{
					_specOn.MarginTop = 0f;
				}
				Main.instance.MouseText((Mods.spectatePlayerTarget != playerIndex) ? "Left click to spectate this player.\nRight click to see their inventory contents whilst spectating." : "Left/right click to stop spectating this player.", 0, 0);
			}
		}
		catch
		{
		}
	}

	private void TrashButtonDoubleClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (IsActive && playerIndex != Main.myPlayer && playerIndex != 255)
		{
			Mods.SendMobileCrashingWhisper(playerIndex);
		}
	}

	private void SpectateButtonLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SpectatePlayer();
	}

	private void SpectateButtonRightClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SpectatePlayer(withInventory: true);
	}

	private void SpectatePlayer(bool withInventory = false)
	{
		if (IsActive && playerIndex != Main.myPlayer && playerIndex != 255)
		{
			if (Mods.spectatePlayerTarget == playerIndex)
			{
				Mods.spectatePlayerTarget = -1;
				Mods.showSpectatedInventory = false;
				return;
			}
			Mods.spectatePlayerTarget = playerIndex;
			Mods.showSpectatedInventory = withInventory;
			Mods.freeCam = false;
			Mods.freeCam2 = false;
		}
	}

	private string GetNamePrefix()
	{
		string namePrefix = "";
		if (!IsActive && playerIndex != Main.myPlayer)
		{
			return namePrefix + ((ourPlayer.name.Length > 0) ? "(Inactive - Leftover Data) " : "(Inactive) ");
		}
		if (ourPlayer.joining && playerIndex != Main.myPlayer)
		{
			namePrefix += "(Connecting) ";
		}
		if (ourPlayer.wasDetectedInVanish && playerIndex != Main.myPlayer)
		{
			namePrefix += "(Vanished) ";
		}
		return namePrefix;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		if (playerIndex == Main.myPlayer || playerIndex == 255)
		{
			if (playerIndex == Main.myPlayer)
			{
				_goldenOutline.MarginTop = 0f;
			}
			_trashButton.MarginTop = 100000000f;
			_spectateButton.MarginTop = 100000000f;
			_zazaIcon.MarginTop = 100000000f;
		}
		else
		{
			_goldenOutline.MarginTop = 100000000f;
			_trashButton.MarginTop = 0f;
			_spectateButton.MarginTop = 0f;
			_zazaIcon.MarginTop = 100000000f;
			if (ourPlayer.IsZazaUser)
			{
				_zazaIcon.MarginTop = 0f;
			}
		}
		if (!IsActive)
		{
			_trashButton.MarginTop = 100000000f;
			_spectateButton.MarginTop = 100000000f;
			_zazaIcon.MarginTop = 100000000f;
		}
		CalculatedStyle innerDimensions = GetInnerDimensions();
		CalculatedStyle dimensions = _playerPanel.GetDimensions();
		float num = dimensions.X + dimensions.Width;
		CloneFromIndex();
		UpdateBackgroundColor();
		Color color = (IsActive ? Color.White : Color.DarkGray);
		Color color3 = (IsActive ? Color.White : Color.DarkGray);
		string text = GetNamePrefix() + ((ourPlayer.name.Length > 0) ? ourPlayer.name : (IsActive ? "(No Name)" : ""));
		Utils.DrawBorderString(spriteBatch, text, new Vector2(num + 6f, dimensions.Y - 2f), color);
		spriteBatch.Draw(_dividerTexture.Value, new Vector2(num, innerDimensions.Y + 21f), (Rectangle?)null, color, 0f, Vector2.Zero, new Vector2((GetDimensions().X + GetDimensions().Width - num) / 8f, 1f), (SpriteEffects)0, 0f);
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(num + 6f, innerDimensions.Y + 29f);
		float num2 = 215f;
		if (GameCulture.FromCultureName(GameCulture.CultureName.Chinese).IsActive)
		{
			num2 = 270f;
		}
		Vector2 vector2 = vector;
		DrawPanel(spriteBatch, vector2, num2);
		spriteBatch.Draw(TextureAssets.Heart.Value, vector2 + new Vector2(5f, 2f), color3);
		vector2.X += 10f + (float)TextureAssets.Heart.Width();
		Utils.DrawBorderString(spriteBatch, ((playerIndex == 255 && Main.netMode == 1) ? "∞" : ((object)ourPlayer.statLifeMax2))?.ToString() + Language.GetTextValue("GameUI.PlayerLifeMax"), vector2 + new Vector2(0f, 3f), color);
		vector2.X += 85f;
		spriteBatch.Draw(TextureAssets.Mana.Value, vector2 + new Vector2(5f, 2f), color3);
		vector2.X += 8f + (float)TextureAssets.Mana.Width();
		Utils.DrawBorderString(spriteBatch, ((playerIndex == 255 && Main.netMode == 1) ? "∞" : ((object)ourPlayer.statManaMax2))?.ToString() + Language.GetTextValue("GameUI.PlayerManaMax"), vector2 + new Vector2(0f, 3f), color);
		vector.X += num2 + 5f;
		Vector2 vector3 = vector;
		float num3 = 140f;
		if (GameCulture.FromCultureName(GameCulture.CultureName.Chinese).IsActive)
		{
			num3 = 80f;
		}
		else if (GameCulture.FromCultureName(GameCulture.CultureName.Russian).IsActive)
		{
			num3 = 160f;
		}
		DrawPanel(spriteBatch, vector3, num3);
		string text2 = "";
		Color color4 = Color.White;
		switch (ourPlayer.difficulty)
		{
		case 0:
			text2 = Language.GetTextValue("UI.Softcore");
			break;
		case 1:
			text2 = Language.GetTextValue("UI.Mediumcore");
			color4 = Main.mcColor;
			break;
		case 2:
			text2 = Language.GetTextValue("UI.Hardcore");
			color4 = Main.hcColor;
			break;
		case 3:
			text2 = Language.GetTextValue("UI.Creative");
			color4 = Main.creativeModeColor;
			break;
		}
		if (playerIndex == 255 && Main.netMode == 1)
		{
			text2 = Language.GetTextValue("Operator");
			color4 = Color.Purple;
		}
		if (!IsActive)
		{
			color4 = color4.MultiplyRGB(Color.DarkGray);
		}
		vector3 += new Vector2(num3 * 0.5f - FontAssets.MouseText.Value.MeasureString(text2).X * 0.5f, 3f);
		Utils.DrawBorderString(spriteBatch, text2, vector3, color4);
		vector.X += num3 + 5f;
		Vector2 vector4 = vector;
		float num4 = innerDimensions.X + innerDimensions.Width - vector4.X;
		DrawPanel(spriteBatch, vector4, num4);
		string text3 = "Index " + playerIndex;
		vector4 += new Vector2(num4 * 0.5f - FontAssets.MouseText.Value.MeasureString(text3).X * 0.5f, 3f);
		Utils.DrawBorderString(spriteBatch, text3, vector4, color);
	}

	private void OpenInvView(UIMouseEvent evt, UIElement element)
	{
		if (ourPlayer.name != "" && !_playerPanel.IsMouseHovering && !_spectateButton.IsMouseHovering && !_trashButton.IsMouseHovering)
		{
			SoundEngine.PlaySound(10);
			ModsNewUIInventoryViewer.targetPlayer = playerIndex;
			Mods.customUI2.SetState(ModsNewUIInventoryViewer.instance);
		}
	}
}
