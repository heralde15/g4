using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria.ZazaMod.UI.Elements;

public class ModsNewUIPlayerPortait : UIElement
{
	private static UIImage _uIImage;

	public ModsNewUIPlayerPortait(Player player)
	{
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		base._002Ector();
		Height.Set(112f, 0f);
		Width.Set(219f, 0f);
		SetPadding(0f);
		UIElement uIElement = new UIElement
		{
			Width = new StyleDimension(-4f, 1f),
			Height = new StyleDimension(-4f, 1f),
			IgnoresMouseInteraction = true,
			OverflowHidden = true,
			HAlign = 0.5f,
			VAlign = 0.5f
		};
		uIElement.SetPadding(0f);
		if (!TextureAssets.MapBGs[0].IsLoaded)
		{
			Main.Assets.Request<Texture2D>("Images/MapBG1", (AssetRequestMode)1);
		}
		uIElement.Append(_uIImage = new UIImage(TextureAssets.MapBGs[0])
		{
			HAlign = 0.5f,
			VAlign = 0.5f,
			ScaleToFit = true,
			IgnoresMouseInteraction = true,
			Width = new StyleDimension(0f, 1f),
			Height = new StyleDimension(0f, 1f),
			Color = Color.White
		});
		UICharacter element = new UICharacter(player, animated: true, hasBackPanel: false)
		{
			HAlign = 0.5f,
			VAlign = 1f,
			IgnoresMouseInteraction = true
		};
		element.SetAnimated(animated: true);
		uIElement.Append(element);
		Append(uIElement);
		Append(new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Portrait_Front", (AssetRequestMode)1))
		{
			VAlign = 0.5f,
			HAlign = 0.5f,
			IgnoresMouseInteraction = true
		});
	}

	public static void SetPortraitBG(Asset<Texture2D> asset)
	{
		_uIImage.SetImage(asset);
		_uIImage.Width = new StyleDimension(0f, 0.875f);
		_uIImage.Height = new StyleDimension(0f, 1f);
		_uIImage.IgnoresMouseInteraction = true;
	}
}
