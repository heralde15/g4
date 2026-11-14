using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.ZazaMod.UI.Legacy;

public class ModsUIWorldStats : UIState
{
	public static UIList worldStatusListing;

	public static UIElement worldStatusBackgroundPanel;

	public static ModsUIWorldStats uiStateWorldInformation;

	public static UIList bossList1;

	private static bool[] storedBosses;

	public static UIList bossList2;

	private static UIList worldDataList;

	private static string[] storedWorldData;

	public bool AllPreBossesDefeated
	{
		get
		{
			bool[] array = new bool[7]
			{
				NPC.downedSlimeKing,
				NPC.downedBoss1,
				NPC.downedBoss2,
				NPC.downedBoss3,
				NPC.downedQueenBee,
				NPC.downedDeerclops,
				Main.hardMode
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i])
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool AllPostBossesDefeated
	{
		get
		{
			bool[] array = new bool[10]
			{
				NPC.downedQueenSlime,
				NPC.downedMechBoss1,
				NPC.downedMechBoss2,
				NPC.downedMechBoss3,
				NPC.downedPlantBoss,
				NPC.downedGolemBoss,
				NPC.downedFishron,
				NPC.downedEmpressOfLight,
				NPC.downedAncientCultist,
				NPC.downedMoonlord
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i])
				{
					return false;
				}
			}
			return true;
		}
	}

	public static string WorldGameMode
	{
		get
		{
			if (Main.GameMode == 0)
			{
				return "Normal";
			}
			if (Main.GameMode == 1)
			{
				return "Expert";
			}
			if (Main.GameMode == 2)
			{
				return "Master";
			}
			if (Main.GameMode == 3)
			{
				return "Journey";
			}
			return "Unknown";
		}
	}

	public static string WorldSize
	{
		get
		{
			if (Main.maxTilesX == 1750)
			{
				return "Tiny (Legacy)";
			}
			if (Main.maxTilesX == 1800)
			{
				return "Tiny";
			}
			if (Main.maxTilesX == 4200)
			{
				return "Small";
			}
			if (Main.maxTilesX == 6400)
			{
				return "Medium";
			}
			if (Main.maxTilesX == 8400)
			{
				return "Large";
			}
			if (Main.maxTilesX == 12800)
			{
				return "Huge";
			}
			if (Main.maxTilesX > 12800)
			{
				return "Unsupported XXL Size";
			}
			return "Your World Is Fucked Up";
		}
	}

	public static string GetWorldPurityStatus
	{
		get
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
			return "World Purity Status Is Unknown;";
		}
	}

	public override void OnInitialize()
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		base.OnInitialize();
		bool fakeBool = false;
		worldStatusBackgroundPanel.Width.Set(515f, 0f);
		worldStatusBackgroundPanel.Height.Set(585f, 0f);
		worldStatusBackgroundPanel.SetPadding(10f);
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
		worldStatusBackgroundPanel.Append(uISlicedImage);
		worldStatusListing.Width.Set(0f, 1f);
		worldStatusListing.Height.Set(0f, 1f);
		worldStatusListing.HAlign = 0.5f;
		worldStatusListing.VAlign = 0.5f;
		worldStatusListing.ListPadding = 5f;
		worldStatusListing.ManualSortMethod = ManualIfnoSortingMethod;
		uISlicedImage.Append(worldStatusListing);
		UIScrollbar uIScrollbar = new UIScrollbar();
		uIScrollbar.SetView(100f, 1000f);
		uIScrollbar.Height.Set(0f, 1f);
		uIScrollbar.HAlign = 1f;
		uISlicedImage.Append(uIScrollbar);
		worldStatusListing.SetScrollbar(uIScrollbar);
		Color backgroundColor = new Color(9, 46, 89) * 0.75f;
		Color borderColor = new Color(0, 0, 0) * 0.75f;
		UITextPanel<LocalizedText> uitextPanel = new UITextPanel<LocalizedText>(Language.GetText("Pre-Hardmode Bosses"), 0.4f, large: true);
		uitextPanel.Width.Set(40f, 0f);
		uitextPanel.Height.Set(40f, 0f);
		uitextPanel.BackgroundColor = backgroundColor;
		uitextPanel.BorderColor = borderColor;
		uitextPanel.OnUpdate += delegate
		{
			UpdateButtonVariables(uitextPanel, ref fakeBool);
		};
		uitextPanel.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		worldStatusListing.Add(uitextPanel);
		UITextPanel<LocalizedText> uitextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("Post-Hardmode Bosses"), 0.4f, large: true);
		uitextPanel2.MarginTop = 0f - uitextPanel.Width.Pixels + (0f - uitextPanel2.Width.Pixels) + -5f;
		uitextPanel2.Width.Set(40f, 0f);
		uitextPanel2.Height.Set(40f, 0f);
		uitextPanel2.BackgroundColor = backgroundColor;
		uitextPanel2.BorderColor = borderColor;
		uitextPanel2.HAlign = 0.9f;
		uitextPanel2.OnUpdate += delegate
		{
			UpdateButtonVariables(uitextPanel2, ref fakeBool);
		};
		uitextPanel2.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		worldStatusListing.Add(uitextPanel2);
		UIPanel uIPanel = new UIPanel();
		uIPanel.Width.Set(uitextPanel.TextSize.X + uitextPanel.Width.Pixels * uitextPanel.TextScale + 6.5f, 0f);
		uIPanel.Height.Set(520f, 0f);
		uIPanel.BackgroundColor = backgroundColor;
		uIPanel.BorderColor = borderColor;
		worldStatusListing.Add(uIPanel);
		UIPanel uIPanel2 = new UIPanel();
		uIPanel2.MarginTop = -525f;
		uIPanel2.Width.Set(uitextPanel2.TextSize.X + uitextPanel.Width.Pixels * uitextPanel2.TextScale + 6.5f, 0f);
		uIPanel2.Height.Set(520f, 0f);
		uIPanel2.BackgroundColor = backgroundColor;
		uIPanel2.BorderColor = borderColor;
		uIPanel2.HAlign = 0.9f;
		worldStatusListing.Add(uIPanel2);
		bossList1.Width.Set(0f, 1f);
		bossList1.Height.Set(0f, 1f);
		bossList1.HAlign = 0.5f;
		bossList1.VAlign = 0.5f;
		bossList1.ListPadding = 5f;
		bossList1.ManualSortMethod = ManualIfnoSortingMethod;
		uIPanel.Append(bossList1);
		bossList2.Width.Set(0f, 1f);
		bossList2.Height.Set(0f, 1f);
		bossList2.HAlign = 0.5f;
		bossList2.VAlign = 0.5f;
		bossList2.ListPadding = 5f;
		bossList2.ManualSortMethod = ManualIfnoSortingMethod;
		uIPanel2.Append(bossList2);
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("World Information"), 0.4f, large: true);
		uITextPanel.MarginTop = 10f;
		uITextPanel.Width.Set(40f, 0f);
		uITextPanel.Height.Set(40f, 0f);
		uITextPanel.BackgroundColor = backgroundColor;
		uITextPanel.BorderColor = borderColor;
		uITextPanel.HAlign = 0.45f;
		worldStatusListing.Add(uITextPanel);
		UIPanel uIPanel3 = new UIPanel();
		uIPanel3.Width.Set(0f, 0.945f);
		uIPanel3.Height.Set(220f, 0f);
		uIPanel3.BackgroundColor = backgroundColor;
		uIPanel3.BorderColor = borderColor;
		worldStatusListing.Add(uIPanel3);
		worldDataList.Width.Set(0f, 1f);
		worldDataList.Height.Set(0f, 1f);
		worldDataList.HAlign = 0.5f;
		worldDataList.VAlign = 0.5f;
		worldDataList.ListPadding = 5f;
		worldDataList.ManualSortMethod = ManualIfnoSortingMethod;
		uIPanel3.Append(worldDataList);
		Append(worldStatusBackgroundPanel);
		UpdateBossList();
		UpdateWorldDataList();
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
	}

	private void ManualIfnoSortingMethod(List<UIElement> list)
	{
	}

	private void UpdateButtonVariables(UITextPanel<LocalizedText> theButtonInQuestion, ref bool inputBool)
	{
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Quick Moondial")
		{
			Main.instance.MouseText("Moondial", 0, 0);
		}
		else if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Quick Sundial")
		{
			Main.instance.MouseText("Sundial", 0, 0);
		}
		else if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Pre-Hardmode Bosses")
		{
			Main.instance.MouseText("Spawn All Pre-Hardmode Bosses", 0, 0);
		}
		else if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Post-Hardmode Bosses")
		{
			Main.instance.MouseText("Spawn All Post-Hardmode Bosses", 0, 0);
		}
		else if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text.Contains("World Size"))
		{
			Main.instance.MouseText(Main.maxTilesX + " x " + Main.maxTilesY, 0, 0);
		}
		else if (theButtonInQuestion.IsMouseHovering)
		{
			Main.instance.MouseText("Spawn Boss", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.BackgroundColor != (inputBool ? (new Color(9, 46, 75) * 0.75f) : (new Color(9, 46, 110) * 0.75f)))
		{
			SoundEngine.PlaySound(12, 0);
			theButtonInQuestion.BackgroundColor = (theButtonInQuestion.BackgroundColor = (inputBool ? (new Color(9, 46, 75) * 0.75f) : (new Color(9, 46, 110) * 0.75f)));
			theButtonInQuestion.BorderColor = Main.OurFavoriteColor;
		}
		else if (!theButtonInQuestion.IsMouseHovering && theButtonInQuestion.BackgroundColor != (inputBool ? (new Color(9, 46, 66) * 0.75f) : (new Color(9, 46, 89) * 0.75f)))
		{
			theButtonInQuestion.BackgroundColor = (inputBool ? (new Color(9, 46, 66) * 0.75f) : (new Color(9, 46, 89) * 0.75f));
			theButtonInQuestion.BorderColor = new Color(0, 0, 0) * 0.75f;
		}
	}

	private void UpdateButtonOnClick(UIMouseEvent evt, UIElement listeningElement, ref bool boolean)
	{
		//IL_0c94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0793: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0884: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d92: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		if (!(evt.Target is UITextPanel<LocalizedText>))
		{
			return;
		}
		Vector2 position = default(Vector2);
		((Vector2)(ref position))._002Ector((float)((Main.LocalPlayer.direction == 1) ? ((int)Main.LocalPlayer.position.X + 300) : ((int)Main.LocalPlayer.position.X - 300)), (float)((int)Main.LocalPlayer.position.Y - 100));
		string text = ((UITextPanel<LocalizedText>)evt.Target).Text;
		SoundEngine.PlaySound(boolean ? 10 : 11, 0);
		int num = -1;
		switch (text)
		{
		case "Pre-Hardmode Bosses":
			if (Main.netMode == 0)
			{
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 50);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 4);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, (!WorldGen.crimson) ? 13 : 266);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 35);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 222);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 668);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 113);
			}
			else
			{
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 50f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 4f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 13f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 266f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 35f);
				NetMessage.SendData(51, -1, -1, null, Main.myPlayer, 1f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 222f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 668f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 113f);
			}
			return;
		case "Post-Hardmode Bosses":
			if (Main.netMode == 0)
			{
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 657);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 125);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 126);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 134);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 127);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 262);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 245);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 636);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 439);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 398);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 370);
			}
			else
			{
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, -8f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 657f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 125f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 126f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 134f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 127f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 262f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 245f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 636f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 439f);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 398f);
				Projectile projectile = Main.projectile[Projectile.NewProjectile(Main.LocalPlayer.GetProjectileSource_Misc(360), position, Vector2.Zero, 360, 0, 0f, Main.myPlayer)];
				NetMessage.SendData(27, -1, -1, null, projectile.whoAmI);
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 370f);
				projectile.Kill();
			}
			return;
		case "Quick Sundial":
			if (Main.netMode == 0)
			{
				Main.Sundialing();
				Main.sundialCooldown = 0;
			}
			else
			{
				NetMessage.SendData(51, -1, -1, null, Main.myPlayer, 3f);
			}
			return;
		case "Quick Moondial":
			if (Main.netMode == 0)
			{
				Main.Moondialing();
				Main.moondialCooldown = 0;
			}
			else
			{
				NetMessage.SendData(51, -1, -1, null, Main.myPlayer, 6f);
			}
			return;
		case "King Slime":
			num = 50;
			break;
		case "Eye Of Cthulhu":
			num = 4;
			break;
		case "Eater Of Worlds":
			num = 13;
			break;
		case "Brain Of Cthulhu":
			num = 266;
			break;
		case "Skeletron":
			num = 35;
			break;
		case "Queen Bee":
			num = 222;
			break;
		case "Deerclops":
			num = 668;
			break;
		case "Wall Of Flesh":
			num = 113;
			break;
		case "Queen Slime":
			num = 657;
			break;
		case "Twins":
			num = 125;
			break;
		case "Destroyer":
			num = 134;
			break;
		case "Skeletron Prime":
			num = 127;
			break;
		case "Plantera":
			num = 262;
			break;
		case "Golem":
			num = 245;
			break;
		case "Empress Of Light":
			num = 636;
			break;
		case "Lunatic Cultists":
			num = 439;
			break;
		case "Moon Lord":
			num = 398;
			break;
		case "Duke Fishron":
			num = 370;
			break;
		}
		if (Main.netMode == 0)
		{
			if (num == 125)
			{
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 125);
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, 126);
			}
			else
			{
				NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)position.X, (int)position.Y, num);
			}
			return;
		}
		switch (num)
		{
		case 35:
			NetMessage.SendData(61, -1, -1, null, Main.myPlayer, num);
			NetMessage.SendData(51, -1, -1, null, Main.myPlayer, 1f);
			break;
		case 125:
			NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 125f);
			NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 126f);
			break;
		case 370:
		{
			Projectile projectile2 = Main.projectile[Projectile.NewProjectile(Main.LocalPlayer.GetProjectileSource_Misc(360), position, Vector2.Zero, 360, 0, 0f, Main.myPlayer)];
			NetMessage.SendData(27, -1, -1, null, projectile2.whoAmI);
			NetMessage.SendData(61, -1, -1, null, Main.myPlayer, 370f);
			projectile2.Kill();
			break;
		}
		case 398:
			NetMessage.SendData(61, -1, -1, null, Main.myPlayer, -8f);
			break;
		default:
			NetMessage.SendData(61, -1, -1, null, Main.myPlayer, num);
			break;
		case -1:
			break;
		}
	}

	static ModsUIWorldStats()
	{
		storedWorldData = new string[10];
		bossList2 = new UIList();
		worldDataList = new UIList();
		storedBosses = new bool[17];
		worldStatusListing = new UIList();
		bossList1 = new UIList();
		worldStatusBackgroundPanel = new UIElement();
		uiStateWorldInformation = new ModsUIWorldStats();
	}

	public override void Update(GameTime gameTime)
	{
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		if (ModsUICommandPanelButtons.PlayerPressedHotbarKeyV2())
		{
			ModsUICommandPanelButtons.sharedPrevSelectedItem = Main.LocalPlayer.selectedItem;
		}
		if (Mods.bossAndWorldProgression && Main.LocalPlayer.mouseInterface)
		{
			Main.LocalPlayer.selectedItem = ModsUICommandPanelButtons.sharedPrevSelectedItem;
		}
		if (NPC.downedSlimeKing != storedBosses[0] || NPC.downedBoss1 != storedBosses[1] || NPC.downedBoss2 != storedBosses[2] || NPC.downedBoss3 != storedBosses[3] || NPC.downedQueenBee != storedBosses[4] || NPC.downedDeerclops != storedBosses[5] || Main.hardMode != storedBosses[6])
		{
			storedBosses[0] = NPC.downedSlimeKing;
			storedBosses[1] = NPC.downedBoss1;
			storedBosses[2] = NPC.downedBoss2;
			storedBosses[3] = NPC.downedBoss3;
			storedBosses[4] = NPC.downedQueenBee;
			storedBosses[5] = NPC.downedDeerclops;
			storedBosses[6] = Main.hardMode;
			UpdateBossList();
		}
		if (NPC.downedQueenSlime != storedBosses[7] || NPC.downedMechBoss1 != storedBosses[8] || NPC.downedMechBoss2 != storedBosses[9] || NPC.downedMechBoss3 != storedBosses[10] || NPC.downedPlantBoss != storedBosses[11] || NPC.downedGolemBoss != storedBosses[12] || NPC.downedFishron != storedBosses[13] || NPC.downedEmpressOfLight != storedBosses[14] || NPC.downedAncientCultist != storedBosses[15] || NPC.downedMoonlord != storedBosses[16])
		{
			storedBosses[7] = NPC.downedQueenSlime;
			storedBosses[8] = NPC.downedMechBoss1;
			storedBosses[9] = NPC.downedMechBoss2;
			storedBosses[10] = NPC.downedMechBoss3;
			storedBosses[11] = NPC.downedPlantBoss;
			storedBosses[12] = NPC.downedGolemBoss;
			storedBosses[13] = NPC.downedFishron;
			storedBosses[14] = NPC.downedEmpressOfLight;
			storedBosses[15] = NPC.downedAncientCultist;
			storedBosses[16] = NPC.downedMoonlord;
			UpdateBossList();
		}
		if (storedWorldData[0] != Main.worldName || storedWorldData[1] != WorldSize || storedWorldData[2] != WorldGameMode || storedWorldData[3] != GetWorldPurityStatus || storedWorldData[4] != Main.sundialCooldown.ToString() || storedWorldData[5] != Main.moondialCooldown.ToString())
		{
			storedWorldData[0] = Main.worldName;
			storedWorldData[1] = WorldSize;
			storedWorldData[2] = WorldGameMode;
			storedWorldData[3] = GetWorldPurityStatus;
			storedWorldData[4] = Main.sundialCooldown.ToString();
			storedWorldData[5] = Main.moondialCooldown.ToString();
			UpdateWorldDataList();
		}
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)Main.screenWidth / 2f - (float)((int)Mods.panelWidth / 2) + 15f, (float)(Main.screenHeight / 2 - 40));
		worldStatusBackgroundPanel.Left.Set(vector.X, 0f);
		worldStatusBackgroundPanel.Top.Set(vector.Y - Mods.panelHeight / 2f + 75f, 0f);
		base.Update(gameTime);
	}

	private void UpdateBossList()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Unknown result type (might be due to invalid IL or missing references)
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0920: Unknown result type (might be due to invalid IL or missing references)
		//IL_0921: Unknown result type (might be due to invalid IL or missing references)
		//IL_092d: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1000: Unknown result type (might be due to invalid IL or missing references)
		//IL_100c: Unknown result type (might be due to invalid IL or missing references)
		//IL_100d: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10da: Unknown result type (might be due to invalid IL or missing references)
		//IL_1199: Unknown result type (might be due to invalid IL or missing references)
		//IL_119a: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1266: Unknown result type (might be due to invalid IL or missing references)
		//IL_1267: Unknown result type (might be due to invalid IL or missing references)
		//IL_1273: Unknown result type (might be due to invalid IL or missing references)
		//IL_1274: Unknown result type (might be due to invalid IL or missing references)
		//IL_1333: Unknown result type (might be due to invalid IL or missing references)
		//IL_1334: Unknown result type (might be due to invalid IL or missing references)
		//IL_1340: Unknown result type (might be due to invalid IL or missing references)
		//IL_1341: Unknown result type (might be due to invalid IL or missing references)
		//IL_1400: Unknown result type (might be due to invalid IL or missing references)
		//IL_1401: Unknown result type (might be due to invalid IL or missing references)
		//IL_140d: Unknown result type (might be due to invalid IL or missing references)
		//IL_140e: Unknown result type (might be due to invalid IL or missing references)
		//IL_156d: Unknown result type (might be due to invalid IL or missing references)
		//IL_156e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1575: Unknown result type (might be due to invalid IL or missing references)
		//IL_1576: Unknown result type (might be due to invalid IL or missing references)
		//IL_14cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_14da: Unknown result type (might be due to invalid IL or missing references)
		//IL_14db: Unknown result type (might be due to invalid IL or missing references)
		//IL_161d: Unknown result type (might be due to invalid IL or missing references)
		//IL_161e: Unknown result type (might be due to invalid IL or missing references)
		//IL_162a: Unknown result type (might be due to invalid IL or missing references)
		//IL_162b: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_16eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1884: Unknown result type (might be due to invalid IL or missing references)
		//IL_1885: Unknown result type (might be due to invalid IL or missing references)
		//IL_1891: Unknown result type (might be due to invalid IL or missing references)
		//IL_1892: Unknown result type (might be due to invalid IL or missing references)
		//IL_1951: Unknown result type (might be due to invalid IL or missing references)
		//IL_1952: Unknown result type (might be due to invalid IL or missing references)
		//IL_195e: Unknown result type (might be due to invalid IL or missing references)
		//IL_195f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c93: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d53: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d60: Unknown result type (might be due to invalid IL or missing references)
		bool fakeBool = false;
		if (bossList1.Count > 0)
		{
			bossList1.Clear();
		}
		if (bossList2.Count > 0)
		{
			bossList2.Clear();
		}
		Color backgroundColor = new Color(9, 46, 89) * 0.75f;
		Color borderColor = new Color(0, 0, 0) * 0.75f;
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("Defeated"), 0.32f, large: true);
		uITextPanel.Width.Set(20f, 0f);
		uITextPanel.Height.Set(10f, 0f);
		uITextPanel.BackgroundColor = backgroundColor;
		uITextPanel.BorderColor = borderColor;
		uITextPanel.HAlign = 0.5f;
		uITextPanel.MarginBottom = 5f;
		bossList1.Add(uITextPanel);
		if (NPC.downedSlimeKing)
		{
			UITextPanel<LocalizedText> uitextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("King Slime"), 0.32f, large: true);
			uitextPanel2.Width.Set(0f, 1f);
			uitextPanel2.Height.Set(20f, 0f);
			uitextPanel2.BackgroundColor = backgroundColor;
			uitextPanel2.BorderColor = borderColor;
			uitextPanel2.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel2, ref fakeBool);
			};
			uitextPanel2.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList1.Add(uitextPanel2);
		}
		if (NPC.downedBoss1)
		{
			UITextPanel<LocalizedText> uitextPanel3 = new UITextPanel<LocalizedText>(Language.GetText("Eye Of Cthulhu"), 0.32f, large: true);
			uitextPanel3.Width.Set(0f, 1f);
			uitextPanel3.Height.Set(20f, 0f);
			uitextPanel3.BackgroundColor = backgroundColor;
			uitextPanel3.BorderColor = borderColor;
			uitextPanel3.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel3, ref fakeBool);
			};
			uitextPanel3.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList1.Add(uitextPanel3);
		}
		if (NPC.downedBoss2)
		{
			UITextPanel<LocalizedText> uitextPanel4 = new UITextPanel<LocalizedText>(Language.GetText((!WorldGen.crimson) ? "Eater Of Worlds" : "Brain Of Cthulhu"), 0.32f, large: true);
			uitextPanel4.Width.Set(0f, 1f);
			uitextPanel4.Height.Set(20f, 0f);
			uitextPanel4.BackgroundColor = backgroundColor;
			uitextPanel4.BorderColor = borderColor;
			uitextPanel4.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel4, ref fakeBool);
			};
			uitextPanel4.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList1.Add(uitextPanel4);
		}
		if (NPC.downedBoss3)
		{
			UITextPanel<LocalizedText> uitextPanel5 = new UITextPanel<LocalizedText>(Language.GetText("Skeletron"), 0.32f, large: true);
			uitextPanel5.Width.Set(0f, 1f);
			uitextPanel5.Height.Set(20f, 0f);
			uitextPanel5.BackgroundColor = backgroundColor;
			uitextPanel5.BorderColor = borderColor;
			uitextPanel5.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel5, ref fakeBool);
			};
			uitextPanel5.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList1.Add(uitextPanel5);
		}
		if (NPC.downedQueenBee)
		{
			UITextPanel<LocalizedText> uitextPanel6 = new UITextPanel<LocalizedText>(Language.GetText("Queen Bee"), 0.32f, large: true);
			uitextPanel6.Width.Set(0f, 1f);
			uitextPanel6.Height.Set(20f, 0f);
			uitextPanel6.BackgroundColor = backgroundColor;
			uitextPanel6.BorderColor = borderColor;
			uitextPanel6.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel6, ref fakeBool);
			};
			uitextPanel6.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList1.Add(uitextPanel6);
		}
		if (NPC.downedDeerclops)
		{
			UITextPanel<LocalizedText> uitextPanel7 = new UITextPanel<LocalizedText>(Language.GetText("Deerclops"), 0.32f, large: true);
			uitextPanel7.Width.Set(0f, 1f);
			uitextPanel7.Height.Set(20f, 0f);
			uitextPanel7.BackgroundColor = backgroundColor;
			uitextPanel7.BorderColor = borderColor;
			uitextPanel7.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel7, ref fakeBool);
			};
			uitextPanel7.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList1.Add(uitextPanel7);
		}
		if (Main.hardMode)
		{
			UITextPanel<LocalizedText> uitextPanel8 = new UITextPanel<LocalizedText>(Language.GetText("Wall Of Flesh"), 0.32f, large: true);
			uitextPanel8.Width.Set(0f, 1f);
			uitextPanel8.Height.Set(20f, 0f);
			uitextPanel8.BackgroundColor = backgroundColor;
			uitextPanel8.BorderColor = borderColor;
			uitextPanel8.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel8, ref fakeBool);
			};
			uitextPanel8.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList1.Add(uitextPanel8);
		}
		UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("Undefeated"), 0.32f, large: true);
		uITextPanel2.Width.Set(20f, 0f);
		uITextPanel2.Height.Set(10f, 0f);
		uITextPanel2.BackgroundColor = backgroundColor;
		uITextPanel2.BorderColor = borderColor;
		uITextPanel2.HAlign = 0.5f;
		uITextPanel2.MarginTop = 15f;
		uITextPanel2.MarginBottom = 5f;
		bossList1.Add(uITextPanel2);
		if (!NPC.downedSlimeKing)
		{
			UITextPanel<LocalizedText> uitextPanel10 = new UITextPanel<LocalizedText>(Language.GetText("King Slime"), 0.32f, large: true);
			uitextPanel10.Width.Set(0f, 1f);
			uitextPanel10.Height.Set(20f, 0f);
			uitextPanel10.BackgroundColor = backgroundColor;
			uitextPanel10.BorderColor = borderColor;
			uitextPanel10.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel10, ref fakeBool);
			};
			uitextPanel10.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList1.Add(uitextPanel10);
		}
		if (!NPC.downedBoss1)
		{
			UITextPanel<LocalizedText> uitextPanel11 = new UITextPanel<LocalizedText>(Language.GetText("Eye Of Cthulhu"), 0.32f, large: true);
			uitextPanel11.Width.Set(0f, 1f);
			uitextPanel11.Height.Set(20f, 0f);
			uitextPanel11.BackgroundColor = backgroundColor;
			uitextPanel11.BorderColor = borderColor;
			uitextPanel11.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel11, ref fakeBool);
			};
			uitextPanel11.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList1.Add(uitextPanel11);
		}
		if (!NPC.downedBoss2)
		{
			UITextPanel<LocalizedText> uitextPanel12 = new UITextPanel<LocalizedText>(Language.GetText((!WorldGen.crimson) ? "Eater Of Worlds" : "Brain Of Cthulhu"), 0.32f, large: true);
			uitextPanel12.Width.Set(0f, 1f);
			uitextPanel12.Height.Set(20f, 0f);
			uitextPanel12.BackgroundColor = backgroundColor;
			uitextPanel12.BorderColor = borderColor;
			uitextPanel12.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel12, ref fakeBool);
			};
			uitextPanel12.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList1.Add(uitextPanel12);
		}
		if (!NPC.downedBoss3)
		{
			UITextPanel<LocalizedText> uitextPanel13 = new UITextPanel<LocalizedText>(Language.GetText("Skeletron"), 0.32f, large: true);
			uitextPanel13.Width.Set(0f, 1f);
			uitextPanel13.Height.Set(20f, 0f);
			uitextPanel13.BackgroundColor = backgroundColor;
			uitextPanel13.BorderColor = borderColor;
			uitextPanel13.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel13, ref fakeBool);
			};
			uitextPanel13.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList1.Add(uitextPanel13);
		}
		if (!NPC.downedQueenBee)
		{
			UITextPanel<LocalizedText> uitextPanel14 = new UITextPanel<LocalizedText>(Language.GetText("Queen Bee"), 0.32f, large: true);
			uitextPanel14.Width.Set(0f, 1f);
			uitextPanel14.Height.Set(20f, 0f);
			uitextPanel14.BackgroundColor = backgroundColor;
			uitextPanel14.BorderColor = borderColor;
			uitextPanel14.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel14, ref fakeBool);
			};
			uitextPanel14.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList1.Add(uitextPanel14);
		}
		if (!NPC.downedDeerclops)
		{
			UITextPanel<LocalizedText> uitextPanel15 = new UITextPanel<LocalizedText>(Language.GetText("Deerclops"), 0.32f, large: true);
			uitextPanel15.Width.Set(0f, 1f);
			uitextPanel15.Height.Set(20f, 0f);
			uitextPanel15.BackgroundColor = backgroundColor;
			uitextPanel15.BorderColor = borderColor;
			uitextPanel15.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel15, ref fakeBool);
			};
			uitextPanel15.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList1.Add(uitextPanel15);
		}
		if (!Main.hardMode)
		{
			UITextPanel<LocalizedText> uitextPanel16 = new UITextPanel<LocalizedText>(Language.GetText("Wall Of Flesh"), 0.32f, large: true);
			uitextPanel16.Width.Set(0f, 1f);
			uitextPanel16.Height.Set(20f, 0f);
			uitextPanel16.BackgroundColor = backgroundColor;
			uitextPanel16.BorderColor = borderColor;
			uitextPanel16.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel16, ref fakeBool);
			};
			uitextPanel16.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList1.Add(uitextPanel16);
		}
		UITextPanel<LocalizedText> uITextPanel3 = new UITextPanel<LocalizedText>(Language.GetText("Defeated"), 0.32f, large: true);
		uITextPanel3.Width.Set(20f, 0f);
		uITextPanel3.Height.Set(10f, 0f);
		uITextPanel3.BackgroundColor = backgroundColor;
		uITextPanel3.BorderColor = borderColor;
		uITextPanel3.HAlign = 0.5f;
		uITextPanel3.MarginBottom = 5f;
		bossList2.Add(uITextPanel3);
		if (NPC.downedQueenSlime)
		{
			UITextPanel<LocalizedText> uitextPanel252 = new UITextPanel<LocalizedText>(Language.GetText("Queen Slime"), 0.32f, large: true);
			uitextPanel252.Width.Set(0f, 1f);
			uitextPanel252.Height.Set(20f, 0f);
			uitextPanel252.BackgroundColor = backgroundColor;
			uitextPanel252.BorderColor = borderColor;
			uitextPanel252.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel252, ref fakeBool);
			};
			uitextPanel252.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel252);
		}
		if (NPC.downedMechBoss1)
		{
			UITextPanel<LocalizedText> uitextPanel253 = new UITextPanel<LocalizedText>(Language.GetText("Twins"), 0.32f, large: true);
			uitextPanel253.Width.Set(0f, 1f);
			uitextPanel253.Height.Set(20f, 0f);
			uitextPanel253.BackgroundColor = backgroundColor;
			uitextPanel253.BorderColor = borderColor;
			uitextPanel253.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel253, ref fakeBool);
			};
			uitextPanel253.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel253);
		}
		if (NPC.downedMechBoss2)
		{
			UITextPanel<LocalizedText> uitextPanel254 = new UITextPanel<LocalizedText>(Language.GetText("Destroyer"), 0.32f, large: true);
			uitextPanel254.Width.Set(0f, 1f);
			uitextPanel254.Height.Set(20f, 0f);
			uitextPanel254.BackgroundColor = backgroundColor;
			uitextPanel254.BorderColor = borderColor;
			uitextPanel254.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel254, ref fakeBool);
			};
			uitextPanel254.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel254);
		}
		if (NPC.downedMechBoss3)
		{
			UITextPanel<LocalizedText> uitextPanel255 = new UITextPanel<LocalizedText>(Language.GetText("Skeletron Prime"), 0.32f, large: true);
			uitextPanel255.Width.Set(0f, 1f);
			uitextPanel255.Height.Set(20f, 0f);
			uitextPanel255.BackgroundColor = backgroundColor;
			uitextPanel255.BorderColor = borderColor;
			uitextPanel255.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel255, ref fakeBool);
			};
			uitextPanel255.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel255);
		}
		if (NPC.downedPlantBoss)
		{
			UITextPanel<LocalizedText> uitextPanel256 = new UITextPanel<LocalizedText>(Language.GetText("Plantera"), 0.32f, large: true);
			uitextPanel256.Width.Set(0f, 1f);
			uitextPanel256.Height.Set(20f, 0f);
			uitextPanel256.BackgroundColor = backgroundColor;
			uitextPanel256.BorderColor = borderColor;
			uitextPanel256.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel256, ref fakeBool);
			};
			uitextPanel256.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel256);
		}
		if (NPC.downedGolemBoss)
		{
			UITextPanel<LocalizedText> uitextPanel257 = new UITextPanel<LocalizedText>(Language.GetText("Golem"), 0.32f, large: true);
			uitextPanel257.Width.Set(0f, 1f);
			uitextPanel257.Height.Set(20f, 0f);
			uitextPanel257.BackgroundColor = backgroundColor;
			uitextPanel257.BorderColor = borderColor;
			uitextPanel257.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel257, ref fakeBool);
			};
			uitextPanel257.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel257);
		}
		if (NPC.downedFishron)
		{
			UITextPanel<LocalizedText> boss21 = new UITextPanel<LocalizedText>(Language.GetText("Duke Fishron"), 0.32f, large: true);
			boss21.Width.Set(0f, 1f);
			boss21.Height.Set(20f, 0f);
			boss21.BackgroundColor = backgroundColor;
			boss21.BorderColor = borderColor;
			boss21.OnUpdate += delegate
			{
				UpdateButtonVariables(boss21, ref fakeBool);
			};
			boss21.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(boss21);
		}
		if (NPC.downedEmpressOfLight)
		{
			UITextPanel<LocalizedText> uitextPanel258 = new UITextPanel<LocalizedText>(Language.GetText("Empress Of Light"), 0.32f, large: true);
			uitextPanel258.Width.Set(0f, 1f);
			uitextPanel258.Height.Set(20f, 0f);
			uitextPanel258.BackgroundColor = backgroundColor;
			uitextPanel258.BorderColor = borderColor;
			uitextPanel258.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel258, ref fakeBool);
			};
			uitextPanel258.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel258);
		}
		if (NPC.downedAncientCultist)
		{
			UITextPanel<LocalizedText> uitextPanel259 = new UITextPanel<LocalizedText>(Language.GetText("Lunatic Cultists"), 0.32f, large: true);
			uitextPanel259.Width.Set(0f, 1f);
			uitextPanel259.Height.Set(20f, 0f);
			uitextPanel259.BackgroundColor = backgroundColor;
			uitextPanel259.BorderColor = borderColor;
			uitextPanel259.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel259, ref fakeBool);
			};
			uitextPanel259.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel259);
		}
		if (NPC.downedMoonlord)
		{
			UITextPanel<LocalizedText> uitextPanel260 = new UITextPanel<LocalizedText>(Language.GetText("Moon Lord"), 0.32f, large: true);
			uitextPanel260.Width.Set(0f, 1f);
			uitextPanel260.Height.Set(20f, 0f);
			uitextPanel260.BackgroundColor = backgroundColor;
			uitextPanel260.BorderColor = borderColor;
			uitextPanel260.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel260, ref fakeBool);
			};
			uitextPanel260.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel260);
		}
		UITextPanel<LocalizedText> uITextPanel4 = new UITextPanel<LocalizedText>(Language.GetText("Undefeated"), 0.32f, large: true);
		uITextPanel4.Width.Set(20f, 0f);
		uITextPanel4.Height.Set(10f, 0f);
		uITextPanel4.BackgroundColor = backgroundColor;
		uITextPanel4.BorderColor = borderColor;
		uITextPanel4.HAlign = 0.5f;
		uITextPanel4.MarginTop = 15f;
		uITextPanel4.MarginBottom = 5f;
		bossList2.Add(uITextPanel4);
		if (!NPC.downedQueenSlime)
		{
			UITextPanel<LocalizedText> uitextPanel261 = new UITextPanel<LocalizedText>(Language.GetText("Queen Slime"), 0.32f, large: true);
			uitextPanel261.Width.Set(0f, 1f);
			uitextPanel261.Height.Set(20f, 0f);
			uitextPanel261.BackgroundColor = backgroundColor;
			uitextPanel261.BorderColor = borderColor;
			uitextPanel261.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel261, ref fakeBool);
			};
			uitextPanel261.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel261);
		}
		if (!NPC.downedMechBoss1)
		{
			UITextPanel<LocalizedText> uitextPanel262 = new UITextPanel<LocalizedText>(Language.GetText("Twins"), 0.32f, large: true);
			uitextPanel262.Width.Set(0f, 1f);
			uitextPanel262.Height.Set(20f, 0f);
			uitextPanel262.BackgroundColor = backgroundColor;
			uitextPanel262.BorderColor = borderColor;
			uitextPanel262.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel262, ref fakeBool);
			};
			uitextPanel262.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel262);
		}
		if (!NPC.downedMechBoss2)
		{
			UITextPanel<LocalizedText> uitextPanel263 = new UITextPanel<LocalizedText>(Language.GetText("Destroyer"), 0.32f, large: true);
			uitextPanel263.Width.Set(0f, 1f);
			uitextPanel263.Height.Set(20f, 0f);
			uitextPanel263.BackgroundColor = backgroundColor;
			uitextPanel263.BorderColor = borderColor;
			uitextPanel263.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel263, ref fakeBool);
			};
			uitextPanel263.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel263);
		}
		if (!NPC.downedMechBoss3)
		{
			UITextPanel<LocalizedText> uitextPanel264 = new UITextPanel<LocalizedText>(Language.GetText("Skeletron Prime"), 0.32f, large: true);
			uitextPanel264.Width.Set(0f, 1f);
			uitextPanel264.Height.Set(20f, 0f);
			uitextPanel264.BackgroundColor = backgroundColor;
			uitextPanel264.BorderColor = borderColor;
			uitextPanel264.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel264, ref fakeBool);
			};
			uitextPanel264.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel264);
		}
		if (!NPC.downedPlantBoss)
		{
			UITextPanel<LocalizedText> uitextPanel265 = new UITextPanel<LocalizedText>(Language.GetText("Plantera"), 0.32f, large: true);
			uitextPanel265.Width.Set(0f, 1f);
			uitextPanel265.Height.Set(20f, 0f);
			uitextPanel265.BackgroundColor = backgroundColor;
			uitextPanel265.BorderColor = borderColor;
			uitextPanel265.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel265, ref fakeBool);
			};
			uitextPanel265.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel265);
		}
		if (!NPC.downedGolemBoss)
		{
			UITextPanel<LocalizedText> uitextPanel266 = new UITextPanel<LocalizedText>(Language.GetText("Golem"), 0.32f, large: true);
			uitextPanel266.Width.Set(0f, 1f);
			uitextPanel266.Height.Set(20f, 0f);
			uitextPanel266.BackgroundColor = backgroundColor;
			uitextPanel266.BorderColor = borderColor;
			uitextPanel266.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel266, ref fakeBool);
			};
			uitextPanel266.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel266);
		}
		if (!NPC.downedFishron)
		{
			UITextPanel<LocalizedText> boss31 = new UITextPanel<LocalizedText>(Language.GetText("Duke Fishron"), 0.32f, large: true);
			boss31.Width.Set(0f, 1f);
			boss31.Height.Set(20f, 0f);
			boss31.BackgroundColor = backgroundColor;
			boss31.BorderColor = borderColor;
			boss31.OnUpdate += delegate
			{
				UpdateButtonVariables(boss31, ref fakeBool);
			};
			boss31.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(boss31);
		}
		if (!NPC.downedEmpressOfLight)
		{
			UITextPanel<LocalizedText> uitextPanel267 = new UITextPanel<LocalizedText>(Language.GetText("Empress Of Light"), 0.32f, large: true);
			uitextPanel267.Width.Set(0f, 1f);
			uitextPanel267.Height.Set(20f, 0f);
			uitextPanel267.BackgroundColor = backgroundColor;
			uitextPanel267.BorderColor = borderColor;
			uitextPanel267.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel267, ref fakeBool);
			};
			uitextPanel267.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel267);
		}
		if (!NPC.downedAncientCultist)
		{
			UITextPanel<LocalizedText> uitextPanel268 = new UITextPanel<LocalizedText>(Language.GetText("Lunatic Cultists"), 0.32f, large: true);
			uitextPanel268.Width.Set(0f, 1f);
			uitextPanel268.Height.Set(20f, 0f);
			uitextPanel268.BackgroundColor = backgroundColor;
			uitextPanel268.BorderColor = borderColor;
			uitextPanel268.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel268, ref fakeBool);
			};
			uitextPanel268.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel268);
		}
		if (!NPC.downedMoonlord)
		{
			UITextPanel<LocalizedText> uitextPanel269 = new UITextPanel<LocalizedText>(Language.GetText("Moon Lord"), 0.32f, large: true);
			uitextPanel269.Width.Set(0f, 1f);
			uitextPanel269.Height.Set(20f, 0f);
			uitextPanel269.BackgroundColor = backgroundColor;
			uitextPanel269.BorderColor = borderColor;
			uitextPanel269.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel269, ref fakeBool);
			};
			uitextPanel269.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
			bossList2.Add(uitextPanel269);
		}
	}

	private void UpdateWorldDataList()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		bool fakeBool = false;
		if (worldDataList.Count > 0)
		{
			worldDataList.Clear();
		}
		Color backgroundColor = new Color(9, 46, 89) * 0.75f;
		Color borderColor = new Color(0, 0, 0) * 0.75f;
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("World Name: " + Main.worldName), 0.32f, large: true);
		uITextPanel.Width.Set(0f, 0.95f);
		uITextPanel.Height.Set(5f, 0f);
		uITextPanel.BackgroundColor = backgroundColor;
		uITextPanel.BorderColor = borderColor;
		uITextPanel.HAlign = 0.5f;
		worldDataList.Add(uITextPanel);
		UITextPanel<LocalizedText> uitextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("World Size: " + WorldSize), 0.32f, large: true);
		uitextPanel2.Width.Set(0f, 0.95f);
		uitextPanel2.Height.Set(5f, 0f);
		uitextPanel2.BackgroundColor = backgroundColor;
		uitextPanel2.BorderColor = borderColor;
		uitextPanel2.HAlign = 0.5f;
		uitextPanel2.OnUpdate += delegate
		{
			UpdateButtonVariables(uitextPanel2, ref fakeBool);
		};
		worldDataList.Add(uitextPanel2);
		UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("Game Mode: " + WorldGameMode), 0.32f, large: true);
		uITextPanel2.Width.Set(0f, 0.95f);
		uITextPanel2.Height.Set(5f, 0f);
		uITextPanel2.BackgroundColor = backgroundColor;
		uITextPanel2.BorderColor = borderColor;
		uITextPanel2.HAlign = 0.5f;
		worldDataList.Add(uITextPanel2);
		UITextPanel<LocalizedText> uITextPanel3 = new UITextPanel<LocalizedText>(Language.GetText("Infection: " + GetWorldPurityStatus), 0.32f, large: true);
		uITextPanel3.Width.Set(0f, 0.95f);
		uITextPanel3.Height.Set(5f, 0f);
		uITextPanel3.BackgroundColor = backgroundColor;
		uITextPanel3.BorderColor = borderColor;
		uITextPanel3.HAlign = 0.5f;
		worldDataList.Add(uITextPanel3);
		UITextPanel<LocalizedText> uitextPanel8 = new UITextPanel<LocalizedText>(Language.GetText((Main.sundialCooldown > 0) ? ("Sundial Cooldown: " + Main.sundialCooldown) : "Quick Sundial"), 0.32f, large: true);
		uitextPanel8.Width.Set(0f, 0.47f);
		uitextPanel8.Height.Set(5f, 0f);
		uitextPanel8.BackgroundColor = backgroundColor;
		uitextPanel8.BorderColor = borderColor;
		uitextPanel8.HAlign = 0.05f;
		if (Main.sundialCooldown <= 0)
		{
			uitextPanel8.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel8, ref fakeBool);
			};
			uitextPanel8.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
		}
		worldDataList.Add(uitextPanel8);
		UITextPanel<LocalizedText> uitextPanel9 = new UITextPanel<LocalizedText>(Language.GetText((Main.moondialCooldown > 0) ? ("Moondial Cooldown: " + Main.moondialCooldown) : "Quick Moondial"), 0.32f, large: true);
		uitextPanel9.MarginTop = (0f - uitextPanel8.Height.Pixels) * 2f + (0f - uitextPanel8.TextSize.Y) * 2f + -9f;
		uitextPanel9.Width.Set(0f, 0.47f);
		uitextPanel9.Height.Set(5f, 0f);
		uitextPanel9.BackgroundColor = backgroundColor;
		uitextPanel9.BorderColor = borderColor;
		uitextPanel9.HAlign = 0.95f;
		if (Main.moondialCooldown <= 0)
		{
			uitextPanel9.OnUpdate += delegate
			{
				UpdateButtonVariables(uitextPanel9, ref fakeBool);
			};
			uitextPanel9.OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
			{
				UpdateButtonOnClick(evt, element, ref fakeBool);
			};
		}
		worldDataList.Add(uitextPanel9);
	}
}
