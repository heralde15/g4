using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.ZazaMod.UI.Elements;

public class ModsNewUISliderItem : UIElement
{
	public Color _color;

	private Func<string> _TextDisplayFunction;

	public Func<float> _GetStatusFunction;

	private Action<float> _SlideKeyboardAction;

	private Func<string> _HoverText;

	private int _sliderIDInPage;

	public float perc;

	public ModsNewUISliderItem(Func<string> getText, Func<float> getStatus, Action<float> setStatusKeyboard, int sliderIDInPage, Color color, Func<string> hoverText = null)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		base._002Ector();
		_color = color;
		_TextDisplayFunction = ((getText != null) ? getText : ((Func<string>)(() => "???")));
		_GetStatusFunction = ((getStatus != null) ? getStatus : ((Func<float>)(() => 0f)));
		_SlideKeyboardAction = ((setStatusKeyboard != null) ? setStatusKeyboard : ((Action<float>)delegate
		{
		}));
		_sliderIDInPage = sliderIDInPage;
		perc = _GetStatusFunction();
		_HoverText = ((hoverText != null) ? hoverText : ((Func<string>)(() => "")));
		base.OnUpdate += UpdateHovering;
		base.OnMouseOver += MouseOver;
	}

	private void MouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
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
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		float num = 6f;
		base.DrawSelf(spriteBatch);
		int num2 = 0;
		IngameOptions.rightHover = -1;
		if (!Main.mouseLeft)
		{
			IngameOptions.rightLock = -1;
		}
		if (IngameOptions.rightLock == _sliderIDInPage)
		{
			num2 = 1;
		}
		else if (IngameOptions.rightLock != -1)
		{
			num2 = 2;
		}
		CalculatedStyle dimensions = GetDimensions();
		float num3 = dimensions.Width + 1f;
		Vector2 val = new Vector2(dimensions.X, dimensions.Y);
		bool flag = base.IsMouseHovering;
		if (num2 == 1)
		{
			flag = true;
		}
		if (num2 == 2)
		{
			flag = false;
		}
		Vector2 baseScale = default(Vector2);
		((Vector2)(ref baseScale))._002Ector(0.8f);
		Color value = (flag ? Color.White : Color.Silver);
		value = Color.Lerp(value, Color.White, flag ? 0.5f : 0f);
		Color color = (flag ? _color : _color.MultiplyRGBA(new Color(180, 180, 180)));
		Vector2 position = val;
		Utils.DrawSettingsPanel(spriteBatch, position, num3, color);
		position.X += 8f;
		position.Y += 2f + num;
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, _TextDisplayFunction(), position, value, 0f, Vector2.Zero, baseScale, num3);
		position.X -= 17f;
		TextureAssets.ColorBar.Frame();
		((Vector2)(ref position))._002Ector(dimensions.X + dimensions.Width - 10f, dimensions.Y + 10f + num);
		IngameOptions.valuePosition = position;
		float obj = IngameOptions.DrawValueBar(spriteBatch, 1f, _GetStatusFunction(), num2);
		if (IngameOptions.inBar || IngameOptions.rightLock == _sliderIDInPage)
		{
			perc = _GetStatusFunction();
			IngameOptions.rightHover = _sliderIDInPage;
			if (PlayerInput.Triggers.Current.MouseLeft && !PlayerInput.UsingGamepad && IngameOptions.rightLock == _sliderIDInPage && !Main.grabbingMap)
			{
				ModsNewUICheatPanel.draggingSlider = true;
				_SlideKeyboardAction(obj);
			}
		}
		if (IngameOptions.rightHover != -1 && IngameOptions.rightLock == -1)
		{
			IngameOptions.rightLock = IngameOptions.rightHover;
		}
	}
}
