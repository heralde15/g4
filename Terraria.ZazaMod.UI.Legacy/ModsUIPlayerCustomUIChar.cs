using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria.ZazaMod.UI.Legacy;

public class ModsUIPlayerCustomUIChar : UIState
{
	public static UIElement baseElement;

	public static ModsUIPlayerCustomUIChar uiStateDrawSelectedUiChar;

	public static UISlicedImage uiImagePanel;

	private static int previousWhoAmI;

	private static Player uiPlayer;

	private static UICharacter uiCharacterDraw;

	public override void OnInitialize()
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		base.OnInitialize();
		baseElement.Width.Set(100f, 0f);
		baseElement.Height.Set(100f, 0f);
		baseElement.SetPadding(0f);
		UISlicedImage uISlicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", (AssetRequestMode)1))
		{
			Width = StyleDimension.FromPixelsAndPercent(70f, 0f),
			Height = StyleDimension.FromPixelsAndPercent(80f, 0f),
			HAlign = 0.5f,
			VAlign = 0.5f
		};
		uISlicedImage.SetPadding(10f);
		uISlicedImage.SetSliceDepths(10);
		uISlicedImage.Color = new Color(120, 50, 50) * 0.7f;
		baseElement.Append(uISlicedImage);
		Append(baseElement);
	}

	public override void Update(GameTime gameTime)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		base.Update(gameTime);
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)Main.screenWidth / 2f, (float)(Main.screenHeight / 2 - 40));
		float num = (float)TextureAssets.InventoryBack9.Width() * 0.82f;
		float num2 = (float)TextureAssets.InventoryBack9.Width() * 0.82f * 11f + 40f;
		float num3 = num * 10f + 36f;
		Vector2 vector2 = default(Vector2);
		((Vector2)(ref vector2))._002Ector(vector.X + num2 / 2f, vector.Y + Mods.panelHeight / 2f - num3 - 25f - 101.5f);
		vector2.X -= (num + 4f) * 1.5f + 1f + (num + 4f) / 2f;
		baseElement.Left.Set(vector2.X, 0f);
		baseElement.Top.Set(vector2.Y, 0f);
		Player player = Main.player[Mods.storePlrWhoAmI];
		uiPlayer.skinVariant = player.skinVariant;
		uiPlayer.eyeColor = player.eyeColor;
		uiPlayer.hairColor = player.hairColor;
		uiPlayer.hair = player.hair;
		uiPlayer.skinColor = player.skinColor;
		uiPlayer.shirtColor = player.shirtColor;
		uiPlayer.underShirtColor = player.underShirtColor;
		uiPlayer.shoeColor = player.shoeColor;
		uiPlayer.pantsColor = player.pantsColor;
		uiPlayer.armor = player.armor;
		uiPlayer.dye = player.dye;
		uiPlayer.miscEquips = player.miscEquips;
		uiPlayer.miscDyes = player.miscDyes;
		uiPlayer.direction = 1;
		uiPlayer.gravDir = 1f;
		uiPlayer.socialIgnoreLight = true;
		if (Mods.storePlrWhoAmI != previousWhoAmI)
		{
			previousWhoAmI = Mods.storePlrWhoAmI;
			if (uiCharacterDraw != null)
			{
				baseElement.RemoveChild(uiCharacterDraw);
			}
			uiCharacterDraw = new UICharacter(uiPlayer, animated: true, hasBackPanel: false, 1.2f);
			uiCharacterDraw.HAlign = 0.5f;
			uiCharacterDraw.VAlign = 0.5f;
			baseElement.Append(uiCharacterDraw);
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
	}

	static ModsUIPlayerCustomUIChar()
	{
		baseElement = new UIElement();
		previousWhoAmI = -1;
		uiPlayer = new Player();
		uiStateDrawSelectedUiChar = new ModsUIPlayerCustomUIChar();
	}
}
