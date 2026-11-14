using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.ZazaMod.UI.Elements;

public class ModsNewUIToggleListItem : UIElement
{
	private Color _color;

	private Func<string> _TextDisplayFunction;

	private Func<bool> _IsOnFunction;

	public Func<bool> _DisableCondition;

	private Asset<Texture2D> _toggleTexture;

	private Func<string> _HoverText;

	public ModsNewUIToggleListItem(Func<string> getText, Func<bool> getStatus, Color color, Func<bool> disableCon = null, Func<string> hoverText = null)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		base._002Ector();
		_color = color;
		_toggleTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle", (AssetRequestMode)1);
		_TextDisplayFunction = ((getText != null) ? getText : ((Func<string>)(() => "???")));
		_IsOnFunction = ((getStatus != null) ? getStatus : ((Func<bool>)(() => false)));
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		float num = 6f;
		base.DrawSelf(spriteBatch);
		CalculatedStyle dimensions = GetDimensions();
		float num2 = dimensions.Width + 1f;
		Vector2 val = new Vector2(dimensions.X, dimensions.Y);
		Vector2 baseScale = default(Vector2);
		((Vector2)(ref baseScale))._002Ector(0.8f);
		Color value = (base.IsMouseHovering ? Color.White : Color.Silver);
		value = (_DisableCondition() ? Color.Lerp(value, new Color(160, 160, 160), 0f) : Color.Lerp(value, Color.White, base.IsMouseHovering ? 0.5f : 0f));
		Color color = (_DisableCondition() ? _color.MultiplyRGBA(new Color(140, 140, 140)) : (base.IsMouseHovering ? _color : _color.MultiplyRGBA(new Color(180, 180, 180))));
		Vector2 position = val;
		Utils.DrawSettingsPanel(spriteBatch, position, num2, color);
		position.X += 8f;
		position.Y += 2f + num;
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, _TextDisplayFunction(), position, value, 0f, Vector2.Zero, baseScale, num2);
		position.X -= 17f;
		Rectangle value2 = default(Rectangle);
		((Rectangle)(ref value2))._002Ector(_IsOnFunction() ? ((_toggleTexture.Width() - 2) / 2 + 2) : 0, 0, (_toggleTexture.Width() - 2) / 2, _toggleTexture.Height());
		Vector2 vector2 = default(Vector2);
		((Vector2)(ref vector2))._002Ector((float)value2.Width, 0f);
		Vector2 val2 = default(Vector2);
		((Vector2)(ref val2))._002Ector(dimensions.X + dimensions.Width - vector2.X - 10f, dimensions.Y + 2f + num);
		spriteBatch.Draw(_toggleTexture.Value, val2, (Rectangle?)value2, Color.White, 0f, Vector2.Zero, Vector2.One, (SpriteEffects)0, 0f);
	}
}
