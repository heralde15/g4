using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.UI.Chat;

namespace Terraria.ZazaMod.UI;

internal class ModsVitalsUI
{
	public static void Draw()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (Main.gameMenu || Main.mapFullscreen || !Mods.playerVitalStatsUI)
			{
				return;
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, Main.UIScaleMatrix);
			PlayerInput.SetZoom_World();
			int screenWidth = Main.screenWidth;
			int screenHeight = Main.screenHeight;
			Vector2 screenPosition = Main.screenPosition;
			PlayerInput.SetZoom_UI();
			for (int i = 0; i <= 255; i++)
			{
				Player player = Main.player[i];
				if (player.active && player.whoAmI != Main.myPlayer)
				{
					Draw_Inner(player, screenWidth, screenHeight, screenPosition);
				}
			}
			Draw_Inner(Main.LocalPlayer, screenWidth, screenHeight, screenPosition);
		}
		catch
		{
		}
	}

	private static void Draw_Inner(Player player, int screenWidth, int screenHeight, Vector2 screenPosition)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		if (!player.dead)
		{
			float uIScale = Main.UIScale;
			SpriteViewMatrix gameViewMatrix = Main.GameViewMatrix;
			Color color = new Color(255f, 255f, 255f) * 0.65f;
			string theText = $"   {player.statLife}/{player.statLifeMax2}    {player.statMana}/{player.statManaMax2}    {player.statDefense}";
			string text = $"{player.statLife}/{player.statLifeMax2} ";
			string text2 = $"{player.statMana}/{player.statManaMax2} ";
			_ = $"{player.statDefense}";
			Vector2 namePlatePos = FontAssets.DeathText.Value.MeasureString(theText) / 2f;
			namePlatePos.X -= FontAssets.DeathText.Value.MeasureString(theText).X / (5f * gameViewMatrix.Zoom.X) * (gameViewMatrix.Zoom.X / 2f);
			float num = 42f;
			if (Main.LocalPlayer.gravDir == -1f)
			{
				num += 24f;
			}
			if (player.statLife < player.statLifeMax2 && player.whoAmI != Main.myPlayer)
			{
				num += ((Main.LocalPlayer.gravDir == -1f) ? 8f : 16f);
			}
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector((float)(screenWidth / 2) + screenPosition.X, (float)(screenHeight / 2) + screenPosition.Y);
			Vector2 position = player.Bottom;
			position += (position - vector) * (gameViewMatrix.Zoom - Vector2.One);
			float num2 = position.X - vector.X;
			float num3 = position.Y - namePlatePos.Y - 2f + num - vector.Y;
			Math.Sqrt(num2 * num2 + num3 * num3);
			int num5 = screenHeight;
			if (screenHeight > screenWidth)
			{
				num5 = screenWidth;
			}
			num5 = num5 / 2 - 50;
			if (num5 < 100)
			{
				num5 = 100;
			}
			namePlatePos.X = position.X - namePlatePos.X / 2f - screenPosition.X;
			namePlatePos.Y = position.Y - namePlatePos.Y - 2f + num - screenPosition.Y;
			Vector2 measurement = FontAssets.DeathText.Value.MeasureString(theText) / 2f;
			namePlatePos += measurement / 2f;
			namePlatePos *= 1f / uIScale;
			namePlatePos -= measurement / 2f;
			if (Main.LocalPlayer.gravDir == -1f)
			{
				namePlatePos.Y = (float)screenHeight / Main.UIScale - namePlatePos.Y;
			}
			namePlatePos = namePlatePos.Floor();
			ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, theText, namePlatePos, color, 0f, Vector2.Zero, Vector2.One);
			Main.spriteBatch.Draw((player.statLifeMax > 400) ? TextureAssets.Heart2.Value : TextureAssets.Heart.Value, namePlatePos, (Rectangle?)null, color, 0f, Vector2.Zero, 0.9f, (SpriteEffects)0, 0f);
			Vector2 vector3 = default(Vector2);
			((Vector2)(ref vector3))._002Ector(namePlatePos.X + 21f + FontAssets.DeathText.Value.MeasureString(text).X * 0.4f, namePlatePos.Y - 1.5f);
			Main.spriteBatch.Draw(TextureAssets.Mana.Value, vector3, (Rectangle?)null, color, 0f, Vector2.Zero, 0.9f, (SpriteEffects)0, 0f);
			Vector2 vector4 = default(Vector2);
			((Vector2)(ref vector4))._002Ector(vector3.X + 21f + FontAssets.DeathText.Value.MeasureString(text2).X * 0.4f, namePlatePos.Y);
			Main.spriteBatch.Draw(TextureAssets.Extra[58].Value, vector4, (Rectangle?)null, color, 0f, Vector2.Zero, 0.5f, (SpriteEffects)0, 0f);
		}
	}
}
