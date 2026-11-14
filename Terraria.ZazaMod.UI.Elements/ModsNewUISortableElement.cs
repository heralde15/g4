using Terraria.UI;

namespace Terraria.ZazaMod.UI.Elements;

public class ModsNewUISortableElement : UIElement
{
	public int OrderIndex;

	public bool SingleLine;

	public bool FullWidth;

	public bool Thick;

	public bool FullLine
	{
		get
		{
			if (FullWidth && SingleLine)
			{
				return !Thick;
			}
			return false;
		}
	}

	public bool ThickFullLine
	{
		get
		{
			if (FullWidth && SingleLine)
			{
				return Thick;
			}
			return false;
		}
	}

	public bool HalfSingleLine
	{
		get
		{
			if (SingleLine && !FullWidth)
			{
				return !Thick;
			}
			return false;
		}
	}

	public bool ThickHalfSingleLine
	{
		get
		{
			if (SingleLine && !FullWidth)
			{
				return Thick;
			}
			return false;
		}
	}

	public ModsNewUISortableElement(int index)
	{
		OrderIndex = index;
	}

	public override int CompareTo(object obj)
	{
		if (obj is ModsNewUISortableElement uISortableElement)
		{
			return OrderIndex.CompareTo(uISortableElement.OrderIndex);
		}
		return base.CompareTo(obj);
	}
}
