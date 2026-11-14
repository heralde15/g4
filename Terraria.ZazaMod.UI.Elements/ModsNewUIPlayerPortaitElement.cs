using Terraria.UI;

namespace Terraria.ZazaMod.UI.Elements;

internal class ModsNewUIPlayerPortaitElement : UIElement
{
	private static Player ourPlayer => ModsNewUIInventoryViewerButtons.ourPlayer;

	public UIElement ProvideUIElement()
	{
		UIElement uIElement = new UIElement();
		uIElement.Width = new StyleDimension(0f, 1f);
		uIElement.Height = new StyleDimension(112f, 0f);
		uIElement.IgnoresMouseInteraction = true;
		uIElement.SetPadding(0f);
		ModsNewUIPlayerPortait element = new ModsNewUIPlayerPortait(ourPlayer)
		{
			Left = new StyleDimension(4f, 0f),
			HAlign = 0f,
			IgnoresMouseInteraction = true
		};
		uIElement.Append(element);
		return uIElement;
	}
}
