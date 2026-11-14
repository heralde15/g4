using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.ZazaMod.UI.Elements;

public class ModsNewUISimpleListItem : UIElement
{
	public Color _color;

	private Func<string> _GetTextFunction;

	public Func<bool> _DisableCondition;

	private Func<string> _HoverText;

	public ModsNewUISimpleListItem(Func<string> getText, Color color, Func<bool> disableCon = null, Func<string> hoverText = null)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		base._002Ector();
		_color = color;
		_GetTextFunction = ((getText != null) ? getText : ((Func<string>)(() => "???")));
		_DisableCondition = ((disableCon != null) ? disableCon : ((Func<bool>)(() => false)));
		_HoverText = ((hoverText != null) ? hoverText : ((Func<string>)(() => "")));
		base.OnUpdate += UpdateHovering;
		base.OnMouseOver += MouseOver;
	}

	private void MouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		if (!_DisableCondition())
		{
			SoundEngine.PlaySound(12);
		}
	}

	private void UpdateHovering(UIElement element)
	{
		if (element.IsMouseHovering && !string.IsNullOrEmpty(_HoverText()))
		{
			Main.instance.MouseText(_HoverText(), 0, 0);
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		float num = 6f;
		float scaledHeight = ((Height.Pixels > 0f) ? (Height.Pixels / (float)TextureAssets.SettingsPanel.Value.Height) : 1f);
		num *= scaledHeight * scaledHeight;
		base.DrawSelf(spriteBatch);
		CalculatedStyle dimensions = GetDimensions();
		float num2 = dimensions.Width + 1f;
		Vector2 val = new Vector2(dimensions.X, dimensions.Y);
		Vector2 baseScale = default(Vector2);
		((Vector2)(ref baseScale))._002Ector(0.8f);
		Color value = (base.IsMouseHovering ? Color.White : Color.Silver);
		value = (_DisableCondition() ? Color.Lerp(value, new Color(160, 160, 160), 0f) : Color.Lerp(value, Color.White, base.IsMouseHovering ? 0.5f : 0f));
		Color color = ((!_DisableCondition()) ? (base.IsMouseHovering ? _color : _color.MultiplyRGBA(new Color(180, 180, 180))) : ((base.IsMouseHovering && !_DisableCondition()) ? _color.MultiplyRGBA(new Color(220, 220, 220)) : _color.MultiplyRGBA(new Color(140, 140, 140))));
		Vector2 position = val;
		Utils.DrawSettings2Panel(spriteBatch, position, num2, color, scaledHeight);
		position.X += 8f;
		position.Y += 2f + num;
		string text = _GetTextFunction();
		Vector2 stringSize = ChatManager.GetStringSize(FontAssets.ItemStack.Value, text, baseScale);
		position.X = dimensions.X + dimensions.Width / 2f - stringSize.X / 2f;
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text, position, value, 0f, Vector2.Zero, baseScale, num2);
	}
}
