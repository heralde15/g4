using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ZazaMod.UI.Elements;

namespace Terraria.ZazaMod.UI;

public class ModsNewUIFlavorTextElement : UIElement
{
	public UIText _uIText;

	public static int playerIndex => ModsNewUIInventoryViewerButtons.playerIndex;

	public UIElement ProvideUIElement()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		UIPanel obj = new UIPanel(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel", (AssetRequestMode)1), null, 12, 7)
		{
			Width = new StyleDimension(-11f, 1f),
			Height = new StyleDimension(109f, 0f),
			IgnoresMouseInteraction = true,
			BackgroundColor = new Color(43, 56, 101),
			BorderColor = Color.Transparent,
			Left = new StyleDimension(3f, 0f),
			PaddingLeft = 4f,
			PaddingRight = 4f
		};
		UIText uIText = new UIText(GenerateStuff(), 0.8f)
		{
			HAlign = 0f,
			VAlign = 0f,
			IgnoresMouseInteraction = true,
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			IsWrapped = true
		};
		AddDynamicResize(obj, uIText);
		_uIText = uIText;
		obj.Append(uIText);
		return obj;
	}

	private static void AddDynamicResize(UIElement container, UIText text)
	{
		text.OnInternalTextChange += delegate
		{
			container.Height = new StyleDimension(text.MinHeight.Pixels, 0f);
		};
	}

	public string GenerateStuff()
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		List<string> playerStuff = new List<string>();
		Player player = Main.player[playerIndex];
		playerStuff.Add($"Index: {playerIndex}");
		playerStuff.Add(Lang.inter[0].Value + " " + player.statLife + "/" + player.statLifeMax2);
		playerStuff.Add(Lang.inter[2].Value + ": " + player.statMana + "/" + player.statManaMax2);
		if (player.position.X < Main.leftWorld || player.BottomRight.X > Main.rightWorld || player.position.Y < Main.topWorld || player.BottomRight.Y > Main.bottomWorld)
		{
			playerStuff.Add((player.active ? "" : "Last ") + "Position: Unavailable!");
		}
		else
		{
			playerStuff.Add($"{(player.active ? "" : "Last ")}Position: {(int)player.Bottom.X / 16}, {(int)player.Bottom.Y / 16}");
		}
		if (player.active)
		{
			if (playerIndex != Main.myPlayer)
			{
				if (player.IsZazaUser)
				{
					playerStuff.Add("ZazaMod User");
				}
				if (player.isProjectileSpammer)
				{
					playerStuff.Add("Projectile Spammer");
				}
				if (player.isParticleSpammer)
				{
					playerStuff.Add("Particle Spammer");
				}
				if (player.isSoundSpammer)
				{
					playerStuff.Add("Sound Spammer");
				}
				if (player.isNebulaSpammer)
				{
					playerStuff.Add("Nebula Spammer");
				}
			}
		}
		else
		{
			playerStuff.Add("Inactive");
		}
		return string.Join("\n", playerStuff);
	}
}
