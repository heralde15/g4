using Microsoft.Xna.Framework;

namespace Terraria.UI;

public class UIScrollWheelEvent : UIMouseEvent
{
	public readonly int ScrollWheelValue;

	public UIScrollWheelEvent(UIElement target, Vector2 mousePosition, int scrollWheelValue)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		base._002Ector(target, mousePosition);
		ScrollWheelValue = scrollWheelValue;
	}
}
