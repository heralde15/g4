using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.ZazaMod.UI;

public class ModsNewUIInventoryViewerButton : UIPanel, IManuallyOrderedUIElement
{
	public string _text;

	private string _altText;

	public string _mouseOverText;

	private bool _hideMouseOver;

	private Func<bool> _altTextCondition;

	private UITextPanel<string> _textPanel;

	public int OrderInUIList { get; set; }

	public ModsNewUIInventoryViewerButton(string text, string mouseOverText = "", string altText = "", Func<bool> altTextCon = null)
	{
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		base._002Ector();
		_text = text;
		_altText = altText;
		_mouseOverText = mouseOverText;
		_altTextCondition = ((altTextCon != null) ? altTextCon : ((Func<bool>)(() => false)));
		SetPadding(0f);
		PaddingLeft = 10f;
		PaddingRight = 10f;
		Width.Set(-14f, 1f);
		Height.Set(32f, 0f);
		Left.Set(5f, 0f);
		base.OnMouseOver += MouseOver;
		base.OnMouseOut += MouseOut;
		BorderColor = new Color(89, 116, 213, 255);
		Append(_textPanel = new UITextPanel<string>(_text)
		{
			IgnoresMouseInteraction = true,
			DrawPanel = false,
			HAlign = 0.5f,
			Top = new StyleDimension(-4f, 0f)
		});
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		base.DrawSelf(spriteBatch);
		_textPanel.SetText(_altTextCondition() ? _altText : _text);
		if (base.IsMouseHovering && !_hideMouseOver)
		{
			DrawMouseOver();
		}
	}

	private void DrawMouseOver()
	{
		Main.instance.MouseText(_mouseOverText, 0, 0);
	}

	public override int CompareTo(object obj)
	{
		if (obj is IManuallyOrderedUIElement manuallyOrderedUIElement)
		{
			return OrderInUIList.CompareTo(manuallyOrderedUIElement.OrderInUIList);
		}
		return base.CompareTo(obj);
	}

	private void MouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		BorderColor = Colors.FancyUIFatButtonMouseOver;
	}

	private void MouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		BorderColor = new Color(89, 116, 213, 255);
	}
}
