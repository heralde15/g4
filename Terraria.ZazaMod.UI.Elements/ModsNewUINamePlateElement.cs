using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria.ZazaMod.UI.Elements;

public class ModsNewUINamePlateElement : UIElement
{
	public UIText _uIText;

	private int playerIndex => ModsNewUIInventoryViewer.targetPlayer;

	public UIElement ProvideUIElement()
	{
		UIElement uIElement = new UIText(Main.player[playerIndex].name)
		{
			NoSnippets = true
		};
		uIElement.HAlign = 0.5f;
		uIElement.VAlign = 0.5f;
		uIElement.Top = new StyleDimension(2f, 0f);
		uIElement.IgnoresMouseInteraction = true;
		UIElement uIElement2 = new UIElement();
		uIElement2.Width = new StyleDimension(0f, 1f);
		uIElement2.Height = new StyleDimension(24f, 0f);
		uIElement2.Append(uIElement);
		_uIText = (UIText)uIElement;
		return uIElement2;
	}
}
