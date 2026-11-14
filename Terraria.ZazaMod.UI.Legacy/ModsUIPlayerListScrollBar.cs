using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.ZazaMod.UI.Legacy;

public class ModsUIPlayerListScrollBar : UIState
{
	public static UIElement playerListUiElement;

	public static UIList playerListUIList;

	public static int previousPlayerCount;

	public static UITextPanel<LocalizedText> PlayerNamePanel;

	public static Dictionary<int, UITextPanel<LocalizedText>> nameTextPanels;

	public static bool uiCharAnimated;

	private static Player[] uiCharacter;

	private static UICharacter[] drawnPlayer;

	public static ModsUIPlayerListScrollBar uiStatePlayerList;

	public static bool isHoveringOnUiCharacter;

	private static int prevSelectedItem;

	public override void OnInitialize()
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		base.OnInitialize();
		playerListUiElement.Width.Set(515f, 0f);
		playerListUiElement.Height.Set(585f, 0f);
		playerListUiElement.SetPadding(10f);
		UISlicedImage uISlicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", (AssetRequestMode)1))
		{
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			HAlign = 0.5f,
			VAlign = 0.5f
		};
		uISlicedImage.SetPadding(10f);
		uISlicedImage.SetSliceDepths(10);
		uISlicedImage.Color = new Color(11, 84, 95) * 0.55f;
		playerListUiElement.Append(uISlicedImage);
		playerListUIList.Width.Set(0f, 1f);
		playerListUIList.Height.Set(0f, 1f);
		playerListUIList.HAlign = 0.5f;
		playerListUIList.VAlign = 0.5f;
		playerListUIList.ListPadding = 5f;
		playerListUIList.ManualSortMethod = ManualIfnoSortingMethod;
		uISlicedImage.Append(playerListUIList);
		UIScrollbar uIScrollbar = new UIScrollbar();
		uIScrollbar.SetView(500f, 1000f);
		uIScrollbar.Height.Set(0f, 1f);
		uIScrollbar.HAlign = 1f;
		uISlicedImage.Append(uIScrollbar);
		playerListUIList.SetScrollbar(uIScrollbar);
		Append(playerListUiElement);
	}

	public override void Update(GameTime gameTime)
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		base.Update(gameTime);
		if (ModsUICommandPanelButtons.PlayerPressedHotbarKeyV2())
		{
			ModsUICommandPanelButtons.sharedPrevSelectedItem = Main.LocalPlayer.selectedItem;
		}
		if (Mods.playerInspection && Main.LocalPlayer.mouseInterface)
		{
			Main.LocalPlayer.selectedItem = ModsUICommandPanelButtons.sharedPrevSelectedItem;
		}
		if (isHoveringOnUiCharacter)
		{
			Main.instance.MouseText("Left Click To Teleport", 0, 0);
		}
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)Main.screenWidth / 2f - (float)((int)Mods.panelWidth / 2) + 15f, (float)(Main.screenHeight / 2 - 40));
		playerListUiElement.Left.Set(vector.X, 0f);
		playerListUiElement.Top.Set(vector.Y - Mods.panelHeight / 2f + 75f, 0f);
		int num = 0;
		Player[] player = Main.player;
		for (int i = 0; i < player.Length; i++)
		{
			if (player[i].active)
			{
				num++;
			}
		}
		try
		{
			Player[] player2;
			if (previousPlayerCount != num)
			{
				previousPlayerCount = num;
				playerListUIList.Clear();
				nameTextPanels.Clear();
				UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("Players Online: " + previousPlayerCount), 0.3f, large: true);
				uITextPanel.MarginTop = 5f;
				uITextPanel.Width.Set(uITextPanel.TextSize.X + 70f, 0f);
				uITextPanel.Height.Set(uITextPanel.TextSize.Y + 28f, 0f);
				uITextPanel.HAlign = 0.5f;
				uITextPanel.BackgroundColor = new Color(9, 46, 89) * 0.75f;
				uITextPanel.BorderColor = new Color(0, 0, 0) * 0.75f;
				uITextPanel.MarginBottom = 15f;
				playerListUIList.Add(uITextPanel);
				player2 = Main.player;
				foreach (Player player3 in player2)
				{
					if (player3.active)
					{
						UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText(" ) " + player3.name), 0.35f, large: true);
						uITextPanel2.Width.Set(0f, 0.945f);
						uITextPanel2.Height.Set(50f, 0f);
						uITextPanel2.BackgroundColor = new Color(9, 46, 89) * 0.75f;
						uITextPanel2.BorderColor = new Color(0, 0, 0) * 0.75f;
						uITextPanel2.OnMouseOver += FadedMouseOver;
						uITextPanel2.OnMouseOut += FadedMouseOut;
						uITextPanel2.OnLeftClick += ButtonLeftMouse;
						nameTextPanels.Add(player3.whoAmI, uITextPanel2);
						playerListUIList.Add(nameTextPanels[player3.whoAmI]);
						UiCharacterDrawV2(uITextPanel2, player3, player3.whoAmI, updateOnly: false);
					}
				}
			}
			player2 = Main.player;
			foreach (Player player4 in player2)
			{
				if (player4.active)
				{
					if (!nameTextPanels.ContainsKey(player4.whoAmI))
					{
						previousPlayerCount = 999;
						break;
					}
					UiCharacterDrawV2(nameTextPanels[player4.whoAmI], player4, player4.whoAmI, updateOnly: true);
					if (!nameTextPanels[player4.whoAmI].IsMouseHovering && drawnPlayer[player4.whoAmI].IsAnimated)
					{
						drawnPlayer[player4.whoAmI].SetAnimated(animated: false);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Main.NewText("Error:         " + ex);
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
	}

	static ModsUIPlayerListScrollBar()
	{
		playerListUiElement = new UIElement();
		playerListUIList = new UIList();
		nameTextPanels = new Dictionary<int, UITextPanel<LocalizedText>>();
		uiCharacter = new Player[255];
		drawnPlayer = new UICharacter[255];
		uiStatePlayerList = new ModsUIPlayerListScrollBar();
	}

	private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOut(evt);
		SoundEngine.PlaySound(12, 0);
		if (evt.Target is UITextPanel<LocalizedText>)
		{
			((UITextPanel<LocalizedText>)evt.Target).BackgroundColor = new Color(9, 46, 110) * 0.75f;
			((UITextPanel<LocalizedText>)evt.Target).BorderColor = Main.OurFavoriteColor;
		}
		else if (evt.Target is UICharacter)
		{
			isHoveringOnUiCharacter = true;
		}
	}

	private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOut(evt);
		if (evt.Target is UITextPanel<LocalizedText>)
		{
			((UITextPanel<LocalizedText>)evt.Target).BackgroundColor = new Color(9, 46, 89) * 0.75f;
			((UITextPanel<LocalizedText>)evt.Target).BorderColor = new Color(0, 0, 0) * 0.75f;
		}
		else if (evt.Target is UICharacter)
		{
			isHoveringOnUiCharacter = false;
		}
	}

	private void ButtonLeftMouse(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		if (evt.Target is UITextPanel<LocalizedText>)
		{
			string text = ((UITextPanel<LocalizedText>)evt.Target).Text;
			SoundEngine.PlaySound(10, 0);
			Player[] player = Main.player;
			foreach (Player player2 in player)
			{
				if (player2.active && player2.name == text.Substring(3))
				{
					SoundEngine.PlaySound(10, 0, -1, 37);
					Mods.storePlrWhoAmI = player2.whoAmI;
					Mods.showPlayerInfo = true;
				}
			}
		}
		else
		{
			if (!(evt.Target is UICharacter))
			{
				return;
			}
			UITextPanel<LocalizedText> uITextPanel = (UITextPanel<LocalizedText>)evt.Target.Parent;
			Player[] player = Main.player;
			foreach (Player player3 in player)
			{
				if (player3.active && player3.name == uITextPanel.Text.Substring(3))
				{
					Main.LocalPlayer.position = player3.position;
					Main.LocalPlayer.velocity = Vector2.Zero;
				}
			}
		}
	}

	private void UiCharacterDrawV2(UITextPanel<LocalizedText> element, Player plr, int iteration, bool updateOnly)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		if (uiCharacter[iteration] == null)
		{
			uiCharacter[iteration] = new Player();
		}
		uiCharacter[iteration].skinVariant = plr.skinVariant;
		uiCharacter[iteration].eyeColor = plr.eyeColor;
		uiCharacter[iteration].hairColor = plr.hairColor;
		uiCharacter[iteration].hair = plr.hair;
		uiCharacter[iteration].skinColor = plr.skinColor;
		uiCharacter[iteration].shirtColor = plr.shirtColor;
		uiCharacter[iteration].underShirtColor = plr.underShirtColor;
		uiCharacter[iteration].shoeColor = plr.shoeColor;
		uiCharacter[iteration].pantsColor = plr.pantsColor;
		uiCharacter[iteration].armor = plr.armor;
		uiCharacter[iteration].dye = plr.dye;
		uiCharacter[iteration].direction = 1;
		uiCharacter[iteration].gravDir = 1f;
		uiCharacter[iteration].socialIgnoreLight = true;
		if (!updateOnly)
		{
			drawnPlayer[iteration] = new UICharacter(uiCharacter[iteration], animated: false, hasBackPanel: false, 0.75f);
			drawnPlayer[iteration].HAlign = 0.5f;
			drawnPlayer[iteration].Left.Set((0f - element.TextSize.X) / 2f - 10f, 0f);
			drawnPlayer[iteration].Top.Set(-5f, 0f);
			element.Append(drawnPlayer[iteration]);
		}
		if (nameTextPanels[iteration].IsMouseHovering && !drawnPlayer[iteration].IsAnimated)
		{
			drawnPlayer[iteration].SetAnimated(animated: true);
		}
	}

	private void ManualIfnoSortingMethod(List<UIElement> list)
	{
	}
}
