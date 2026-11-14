using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.IO;
using Terraria.UI;

namespace Terraria.ZazaMod.UI.Elements;

public class ModsNewUIInventoryViewerButtons : UIPanel
{
	private UIList _list;

	private ModsNewUINamePlateElement _nameplate;

	private ModsNewUIPlayerPortaitElement _portrait;

	private ModsNewUIFlavorTextElement _flavorText;

	private ModsNewUIInventoryViewerButton _teleportButton;

	private UIElement _spectateButton;

	private UIElement _saveAsFileButton;

	private UIElement _whisperCrashButton;

	private UIElement _copyAppearenceButton;

	private UIElement _copyInventoryButton;

	private UIElement _annoyButton1;

	private UIElement _annoyButton2;

	private UIElement _annoyButton3;

	public static bool savingPlayer;

	public static Player ourPlayer => ModsNewUIInventoryViewer.ourPlayer;

	public static int playerIndex => ModsNewUIInventoryViewer.targetPlayer;

	public bool IsActive => Main.player[playerIndex].active;

	public ModsNewUIInventoryViewerButtons()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		base._002Ector();
		CloneFromIndex();
		Width.Set(230f, 0f);
		Height.Set(0f, 1f);
		SetPadding(0f);
		BorderColor = new Color(89, 116, 213, 255);
		BackgroundColor = new Color(73, 94, 171);
		UIList uIList = new UIList
		{
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f)
		};
		uIList.SetPadding(2f);
		uIList.PaddingBottom = 4f;
		uIList.PaddingTop = 4f;
		Append(uIList);
		_list = uIList;
		uIList.ListPadding = 4f;
		uIList.ManualSortMethod = ManualIfnoSortingMethod;
		AppendBorderOverEverything();
		FillInfoForEntry();
	}

	public void UpdateNameplateAndFlavorText()
	{
		if (_nameplate != null && _nameplate._uIText != null)
		{
			_nameplate._uIText.SetText(ourPlayer.name);
		}
		if (_flavorText != null && _flavorText._uIText != null)
		{
			_flavorText._uIText.SetText(_flavorText.GenerateStuff());
		}
	}

	private void AppendBorderOverEverything()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		UIPanel uIPanel = new UIPanel
		{
			Width = new StyleDimension(0f, 1f),
			Height = new StyleDimension(0f, 1f),
			IgnoresMouseInteraction = true
		};
		uIPanel.BorderColor = new Color(89, 116, 213, 255);
		uIPanel.BackgroundColor = Color.Transparent;
		Append(uIPanel);
	}

	private void ManualIfnoSortingMethod(List<UIElement> list)
	{
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		try
		{
			CloneFromIndex();
			UpdateUIList();
			UpdateBackrgoundImage();
		}
		catch
		{
			ModsNewUICheatPanel.RefreshUI();
		}
	}

	public override void Recalculate()
	{
		base.Recalculate();
	}

	public void FillInfoForEntry()
	{
		_list.Clear();
		ModsNewUINamePlateElement nameplate = new ModsNewUINamePlateElement();
		ModsNewUIPlayerPortaitElement portrait = new ModsNewUIPlayerPortaitElement();
		ModsNewUIFlavorTextElement flavorText = new ModsNewUIFlavorTextElement();
		ModsNewUIInventoryViewerButton teleportButton = new ModsNewUIInventoryViewerButton("Teleport");
		teleportButton.OnLeftClick += TeleportButton_OnLeftClick;
		ModsNewUIInventoryViewerButton spectateButton = new ModsNewUIInventoryViewerButton("Spectate", "", "Stop Spectating", () => Mods.spectatePlayerTarget == playerIndex || (playerIndex == Main.myPlayer && Mods.spectatePlayerTarget != -1) || (Mods.spectatePlayerTarget != -1 && !IsActive));
		spectateButton.OnLeftClick += SpectateButton_OnLeftClick;
		spectateButton.OnRightClick += SpectateButton_OnRightClick;
		ModsNewUIInventoryViewerButton copyAppearenceButton = new ModsNewUIInventoryViewerButton("Copy Appearence", "", "Restore Appearence", () => Mods.isAppearanceBackedUp);
		copyAppearenceButton.OnLeftClick += CopyAppearenceButton_OnLeftClick;
		ModsNewUIInventoryViewerButton copyInventoryButton = new ModsNewUIInventoryViewerButton("Copy Inventory", "", "Restore Inventory", () => Mods.isInventoryBackedUp);
		copyInventoryButton.OnLeftClick += CopyInventoryButton_OnLeftClick;
		ModsNewUIInventoryViewerButton savePlayerButton = new ModsNewUIInventoryViewerButton("Save .plr (Double Click)");
		savePlayerButton.OnLeftDoubleClick += SavePlayerButton_OnLeftDoubleClick;
		ModsNewUIInventoryViewerButton whisperCrashButton = new ModsNewUIInventoryViewerButton("Whisper Crash Message");
		whisperCrashButton.OnLeftClick += WhisperCrashButton_OnLeftClick;
		ModsNewUIInventoryViewerButton annoyButton = new ModsNewUIInventoryViewerButton("Annoy (Sound)", "", "Stop Annoying (Sound)", () => Mods.annoyPlayerV2[playerIndex]);
		annoyButton.OnLeftClick += AnnoyButton_OnLeftClick;
		ModsNewUIInventoryViewerButton annoyButton2 = new ModsNewUIInventoryViewerButton("Annoy (Shimmer)", "", "Stop Annoying (Shimmer)", () => Mods.annoyPlayerShimmer[playerIndex]);
		annoyButton2.OnLeftClick += AnnoyButton2_OnLeftClick;
		ModsNewUIInventoryViewerButton annoyButton3 = new ModsNewUIInventoryViewerButton("Annoy (Gas)", "", "Stop Annoying (Gas)", () => Mods.annoyPlayer3[playerIndex]);
		annoyButton3.OnLeftClick += AnnoyButton3_OnLeftClick;
		_nameplate = nameplate;
		_portrait = portrait;
		_flavorText = flavorText;
		_teleportButton = teleportButton;
		_spectateButton = spectateButton;
		_saveAsFileButton = savePlayerButton;
		_whisperCrashButton = whisperCrashButton;
		_copyAppearenceButton = copyAppearenceButton;
		_copyInventoryButton = copyInventoryButton;
		_annoyButton1 = annoyButton;
		_annoyButton2 = annoyButton2;
		_annoyButton3 = annoyButton3;
		_list.Add(nameplate.ProvideUIElement());
		AddHorizontalSeperator(_list);
		_list.Add(portrait.ProvideUIElement());
		AddHorizontalSeperator(_list);
		_list.Add(flavorText.ProvideUIElement());
		AddHorizontalSeperator(_list);
		_list.Add(teleportButton);
		_list.Add(spectateButton);
		_list.Add(copyAppearenceButton);
		_list.Add(copyInventoryButton);
		_list.Add(savePlayerButton);
		_list.Add(annoyButton);
		_list.Add(annoyButton2);
		_list.Add(annoyButton3);
		_list.Add(whisperCrashButton);
	}

	private void WhisperCrashButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (IsActive)
		{
			SoundEngine.PlaySound(12);
			Mods.SendMobileCrashingWhisper(playerIndex);
		}
	}

	private async void SavePlayerButton_OnLeftDoubleClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (savingPlayer)
		{
			return;
		}
		savingPlayer = true;
		await Task.Run(delegate
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (playerIndex != Main.myPlayer && ourPlayer.name != "")
				{
					PlayerFileData.CreateAndSave(ourPlayer, saveInSsc: true);
					Main.NewText("Saved " + PlayerFileName(ourPlayer.name) + ".plr.", Mods.zazasFavoriteColor);
				}
			}
			catch (Exception)
			{
			}
		});
		savingPlayer = false;
	}

	private string PlayerFileName(string playerName)
	{
		char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
		string text = "";
		playerName = playerName.Replace(".", "_");
		playerName = playerName.Replace("*", "_");
		string text2 = playerName;
		foreach (char c in text2)
		{
			text += ((!Enumerable.Contains(invalidFileNameChars, c)) ? ((c != ' ') ? c : '_') : '-');
		}
		return text;
	}

	private void CopyInventoryButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (Mods.isInventoryBackedUp || IsActive)
		{
			Mods.CopyInventoryFrom(ourPlayer);
		}
	}

	private void CopyAppearenceButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (Mods.isAppearanceBackedUp || IsActive)
		{
			Mods.CopyAppearenceFrom(ourPlayer);
		}
	}

	private void AnnoyButton3_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (IsActive)
		{
			SoundEngine.PlaySound(12);
			Mods.annoyPlayer3[playerIndex] = !Mods.annoyPlayer3[playerIndex];
		}
	}

	private void AnnoyButton2_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (IsActive)
		{
			SoundEngine.PlaySound(12);
			Mods.annoyPlayerShimmer[playerIndex] = !Mods.annoyPlayerShimmer[playerIndex];
		}
	}

	private void AnnoyButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (IsActive)
		{
			SoundEngine.PlaySound(12);
			Mods.annoyPlayerV2[playerIndex] = !Mods.annoyPlayerV2[playerIndex];
		}
	}

	private void AddHorizontalSeperator(UIList list)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		UIHorizontalSeparator uIHorizontalSeparator3 = new UIHorizontalSeparator
		{
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Color = new Color(89, 116, 213, 255) * 0.9f
		};
		list.Add(uIHorizontalSeparator3);
	}

	private void SpectateButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SpectatePlayer();
	}

	private void SpectateButton_OnRightClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SpectatePlayer(withInventory: true);
	}

	private void SpectatePlayer(bool withInventory = false)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		if (Mods.spectatePlayerTarget == playerIndex || playerIndex == Main.myPlayer)
		{
			Mods.spectatePlayerTarget = -1;
			Mods.showSpectatedInventory = false;
			SoundEngine.PlaySound(12);
		}
		else if (!(ourPlayer.position.X < Main.leftWorld) && !(ourPlayer.position.Y < Main.topWorld) && !(ourPlayer.BottomRight.X > Main.rightWorld) && !(ourPlayer.BottomRight.Y > Main.bottomWorld))
		{
			Mods.spectatePlayerTarget = playerIndex;
			Mods.showSpectatedInventory = withInventory;
			Mods.freeCam = false;
			Mods.freeCam2 = false;
			SoundEngine.PlaySound(12);
		}
	}

	private void TeleportButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		if (playerIndex != Main.myPlayer && PlayerInput.Triggers.JustReleased.MouseLeft && playerIndex != 255 && !(ourPlayer.position.X < Main.leftWorld) && !(ourPlayer.position.Y < Main.topWorld) && !(ourPlayer.BottomRight.X > Main.rightWorld) && !(ourPlayer.BottomRight.Y > Main.bottomWorld))
		{
			SoundEngine.PlaySound(12);
			Main.LocalPlayer.position = Main.player[playerIndex].position;
			Main.LocalPlayer.oldPosition = Main.LocalPlayer.position;
			Main.LocalPlayer.velocity = Vector2.Zero;
			Main.LocalPlayer.fallStart = (int)Main.LocalPlayer.Bottom.Y / 16;
			Mods.Instance.OldCenterMod = Main.LocalPlayer.Center;
			Main.LocalPlayer.noFallDmg = true;
			NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
		}
	}

	private BestiaryUICollectionInfo GetUICollectionInfo(BestiaryEntry entry, ExtraBestiaryInfoPageInformation extraInfo)
	{
		BestiaryUICollectionInfo result = entry.UIInfoProvider?.GetEntryUICollectionInfo() ?? default(BestiaryUICollectionInfo);
		result.OwnerEntry = entry;
		return result;
	}

	public void UpdateUIList()
	{
		_list.Clear();
		_list.Add(_nameplate.ProvideUIElement());
		AddHorizontalSeperator(_list);
		_list.Add(_portrait.ProvideUIElement());
		AddHorizontalSeperator(_list);
		_list.Add(_flavorText.ProvideUIElement());
		AddHorizontalSeperator(_list);
		if (playerIndex != Main.myPlayer)
		{
			_list.Add(_teleportButton);
		}
		_teleportButton._text = (IsActive ? "Teleport" : "Teleport to Last Pos");
		_teleportButton._mouseOverText = (IsActive ? "" : "Teleport to this player's last position.");
		if ((IsActive && playerIndex != Main.myPlayer) || Mods.spectatePlayerTarget != -1)
		{
			_list.Add(_spectateButton);
		}
		if (IsActive || Mods.isAppearanceBackedUp)
		{
			_list.Add(_copyAppearenceButton);
		}
		if (IsActive || Mods.isInventoryBackedUp)
		{
			_list.Add(_copyInventoryButton);
		}
		if (playerIndex != Main.myPlayer && ourPlayer.name != "")
		{
			_list.Add(_saveAsFileButton);
		}
		if (IsActive && ourPlayer.name != "")
		{
			_list.Add(_annoyButton1);
			_list.Add(_annoyButton2);
			_list.Add(_annoyButton3);
		}
		if (IsActive && playerIndex != Main.myPlayer)
		{
			_list.Add(_whisperCrashButton);
		}
	}

	private void CloneFromIndex()
	{
		if (_nameplate != null && _nameplate._uIText != null)
		{
			_nameplate._uIText.SetText(ourPlayer.name);
		}
		if (_flavorText != null && _flavorText._uIText != null)
		{
			_flavorText._uIText.SetText(_flavorText.GenerateStuff());
		}
	}

	private void UpdateBackrgoundImage()
	{
		Asset<Texture2D> asset = TextureAssets.MapBGs[0];
		int num = -1;
		if (ourPlayer.ZoneUnderworldHeight)
		{
			num = 2;
		}
		else if (ourPlayer.ZoneDungeon)
		{
			num = 4;
		}
		else if (ourPlayer.ZoneLihzhardTemple)
		{
			num = 13;
		}
		else if (ourPlayer.ZoneDirtLayerHeight || ourPlayer.ZoneRockLayerHeight)
		{
			if (ourPlayer.ZoneHive)
			{
				num = 15;
			}
			if (ourPlayer.ZoneGranite)
			{
				num = 16;
			}
			if (ourPlayer.ZoneMarble)
			{
				num = 17;
			}
			else if (!ourPlayer.ZoneHive && !ourPlayer.ZoneGranite && !ourPlayer.ZoneMarble)
			{
				num = (ourPlayer.ZoneGlowshroom ? 20 : (ourPlayer.ZoneCorrupt ? (ourPlayer.ZoneDesert ? 39 : ((!ourPlayer.ZoneSnow) ? 22 : 33)) : (ourPlayer.ZoneCrimson ? (ourPlayer.ZoneDesert ? 40 : ((!ourPlayer.ZoneSnow) ? 23 : 34)) : (ourPlayer.ZoneHallow ? (ourPlayer.ZoneDesert ? 41 : ((!ourPlayer.ZoneSnow) ? 21 : 35)) : (ourPlayer.ZoneSnow ? 3 : (ourPlayer.ZoneJungle ? 12 : (ourPlayer.ZoneDesert ? 14 : ((!ourPlayer.ZoneRockLayerHeight) ? 1 : 31))))))));
			}
		}
		else if (ourPlayer.ZoneGlowshroom)
		{
			num = 19;
		}
		else if (ourPlayer.ZoneSkyHeight)
		{
			num = 32;
		}
		else if (ourPlayer.ZoneCorrupt)
		{
			num = ((!ourPlayer.ZoneDesert) ? 5 : 36);
		}
		else if (ourPlayer.ZoneCrimson)
		{
			num = ((!ourPlayer.ZoneDesert) ? 6 : 37);
		}
		else if (ourPlayer.ZoneHallow)
		{
			num = ((!ourPlayer.ZoneDesert) ? 7 : 38);
		}
		else if (ourPlayer.ZoneBeach)
		{
			num = 10;
		}
		else if (ourPlayer.ZoneSnow)
		{
			num = 11;
		}
		else if (ourPlayer.ZoneJungle)
		{
			num = 8;
		}
		else if (ourPlayer.ZoneDesert)
		{
			num = 9;
		}
		else if (ourPlayer.ZoneGraveyard)
		{
			num = 26;
		}
		if (num > -1)
		{
			asset = TextureAssets.MapBGs[num];
		}
		ModsNewUIPlayerPortait.SetPortraitBG(asset);
	}
}
