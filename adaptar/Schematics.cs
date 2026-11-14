using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.OS;
using SkiaSharp;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Liquid;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameInput;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.Map;
using Terraria.ObjectData;
using Terraria.Utilities;
using Terraria.Utilities.FileBrowser;

namespace Terraria.ZazaMod.WorldEdit;

public class Schematics
{
	public class SchematicWorldPreviewTargetContent : ARenderTargetContentByRequest
	{
		protected override void HandleUseReqest(GraphicsDevice device, SpriteBatch spriteBatch)
		{
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			int schematicSizeX = Mods.schematicSizeX;
			int schemSY = Mods.schematicSizeY;
			int width = (schematicSizeX - schemX * 200) * 16 - ((schemX < schemSizeX - 1) ? 16 : 0);
			int height = (schemSY - schemY * 150) * 16 - ((schemY < schemSizeY - 1) ? 16 : 0);
			if (width > 4096)
			{
				width = 4096;
			}
			if (height > 4096)
			{
				height = 4096;
			}
			if (width <= 0 || height <= 0)
			{
				_wasPrepared = true;
				return;
			}
			int startX = schemX * 200;
			int startY = schemY * 150;
			int endX = startX + 200 - ((schemX < schemSizeX - 1) ? 1 : 0);
			int endY = startY + 150 - ((schemY < schemSizeY - 1) ? 1 : 0);
			PrepareARenderTarget_AndListenToEvents(ref _target, device, width, height, (RenderTargetUsage)1);
			device.SetRenderTarget(_target);
			device.Clear(Color.Transparent);
			spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend);
			int tileIndex = 0;
			Vector2 pos = default(Vector2);
			for (int i = 0; i < Mods.schematicSizeX; i++)
			{
				for (int j = 0; j < Mods.schematicSizeY; j++)
				{
					if (tileIndex >= Mods.copiedTiles.Count)
					{
						continue;
					}
					if (i < startX || j < startY || i > endX || j > endY)
					{
						tileIndex++;
						continue;
					}
					LegacyTile tile = Mods.copiedTiles[tileIndex];
					((Vector2)(ref pos))._002Ector((float)(i - startX) * 16f, (float)(j - startY) * 16f);
					if (!(pos.X > (float)width) && !(pos.Y > (float)height))
					{
						if (tile.wall > 0)
						{
							Main.instance.LoadWall(tile.wall);
							spriteBatch.Draw(Main.instance.TilePaintSystem.TryGetWallAndRequestIfNotReady(tile.wall, tile.wallColor()) ?? TextureAssets.Wall[tile.wall].Value, pos, (Rectangle?)new Rectangle(44, 44, 32, 32), Color.White, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
						}
						LegacyTile aboveTile = ((j == 0) ? new LegacyTile() : Mods.copiedTiles[tileIndex - 1]);
						LegacyTile belowTile = ((!tile.active() && height / 16 == 1) ? new LegacyTile(tile)
						{
							liquid = byte.MaxValue
						} : ((j < endY && j < Mods.schematicSizeY - 1) ? Mods.copiedTiles[tileIndex + 1] : new LegacyTile()));
						if (tile.liquid > 0 && ((tile.active() && Main.tileSolid[tile.type] && !TileID.Sets.Platforms[tile.type]) || !tile.active()))
						{
							oldDrawWater(spriteBatch, tile, aboveTile, belowTile, i - startX, j - startY);
						}
						if (tile.active())
						{
							Main.instance.LoadTiles(tile.type);
							spriteBatch.Draw(Main.instance.TilePaintSystem.TryGetTileAndRequestIfNotReady(tile.type, 0, tile.color()) ?? TextureAssets.Tile[tile.type].Value, pos, (Rectangle?)new Rectangle((int)tile.frameX, (int)tile.frameY, 16, 16), Color.White, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
						}
						if (tile.liquid > 0 && tile.active() && (!Main.tileSolid[tile.type] || TileID.Sets.Platforms[tile.type]))
						{
							oldDrawWater(spriteBatch, tile, aboveTile, belowTile, i - startX, j - startY);
						}
						tileIndex++;
					}
				}
			}
			spriteBatch.End();
			device.SetRenderTarget((RenderTarget2D)null);
			_wasPrepared = true;
		}

		private void oldDrawWater(SpriteBatch spriteBatch, LegacyTile tile, LegacyTile aboveTile, LegacyTile belowTile, int j, int i, int Style = 0, float Alpha = 1f)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_050f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0503: Unknown result type (might be due to invalid IL or missing references)
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_07df: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_082e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0830: Unknown result type (might be due to invalid IL or missing references)
			//IL_0831: Unknown result type (might be due to invalid IL or missing references)
			//IL_0836: Unknown result type (might be due to invalid IL or missing references)
			//IL_0845: Unknown result type (might be due to invalid IL or missing references)
			//IL_0851: Unknown result type (might be due to invalid IL or missing references)
			//IL_0857: Unknown result type (might be due to invalid IL or missing references)
			//IL_0873: Unknown result type (might be due to invalid IL or missing references)
			//IL_0878: Unknown result type (might be due to invalid IL or missing references)
			//IL_0907: Unknown result type (might be due to invalid IL or missing references)
			//IL_0909: Unknown result type (might be due to invalid IL or missing references)
			//IL_090a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0914: Unknown result type (might be due to invalid IL or missing references)
			//IL_091e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0927: Unknown result type (might be due to invalid IL or missing references)
			//IL_092d: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0575: Unknown result type (might be due to invalid IL or missing references)
			//IL_0577: Unknown result type (might be due to invalid IL or missing references)
			//IL_0579: Unknown result type (might be due to invalid IL or missing references)
			//IL_057e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0951: Unknown result type (might be due to invalid IL or missing references)
			//IL_0953: Unknown result type (might be due to invalid IL or missing references)
			//IL_0954: Unknown result type (might be due to invalid IL or missing references)
			//IL_095f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0971: Unknown result type (might be due to invalid IL or missing references)
			//IL_097d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0983: Unknown result type (might be due to invalid IL or missing references)
			//IL_0584: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Unknown result type (might be due to invalid IL or missing references)
			//IL_058b: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_0595: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0700: Unknown result type (might be due to invalid IL or missing references)
			//IL_070a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0718: Unknown result type (might be due to invalid IL or missing references)
			//IL_0722: Unknown result type (might be due to invalid IL or missing references)
			//IL_072b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0731: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0755: Unknown result type (might be due to invalid IL or missing references)
			//IL_075d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0762: Unknown result type (might be due to invalid IL or missing references)
			//IL_0767: Unknown result type (might be due to invalid IL or missing references)
			//IL_0768: Unknown result type (might be due to invalid IL or missing references)
			//IL_076d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0777: Unknown result type (might be due to invalid IL or missing references)
			//IL_0788: Unknown result type (might be due to invalid IL or missing references)
			//IL_079a: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
			float num = 0f;
			float num2 = 99999f;
			float num3 = 99999f;
			Vector2 vector = Vector2.Zero;
			if (tile == null)
			{
				tile = new LegacyTile();
			}
			if (aboveTile == null)
			{
				aboveTile = new LegacyTile();
			}
			if (belowTile == null)
			{
				belowTile = new LegacyTile();
			}
			_ = new Color[4];
			int num6 = 40;
			if (tile.liquid <= 0 || (tile.nactive() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]))
			{
				return;
			}
			Color color = Color.White;
			float num11 = 256 - tile.liquid;
			num11 /= 32f;
			bool flag = false;
			int num12 = 0;
			if (tile.lava())
			{
				float num13 = Math.Abs(j * 16 + 8);
				float num14 = Math.Abs(i * 16 + 8);
				if (num13 < (float)(Main.screenWidth * 2) && num14 < (float)(Main.screenHeight * 2))
				{
					float num15 = (float)Math.Sqrt(num13 * num13 + num14 * num14);
					float num16 = 1f - num15 / ((float)Main.screenWidth * 0.75f);
					if (num16 > 0f)
					{
						num += num16;
					}
				}
				if (num13 < num2)
				{
					num2 = num13;
				}
				if (num14 < num3)
				{
					num3 = num13;
				}
				num12 = 1;
			}
			else if (tile.honey())
			{
				num12 = 11;
			}
			else if (tile.shimmer())
			{
				num12 = 14;
				flag = true;
			}
			if (num12 == 0)
			{
				num12 = Style;
			}
			float num17 = 0.5f;
			if (num12 != 1 && num12 != 11)
			{
				num17 *= Alpha;
			}
			Vector2 vector2 = default(Vector2);
			((Vector2)(ref vector2))._002Ector((float)(j * 16), (float)(i * 16 + (int)num11 * 2));
			Rectangle value = default(Rectangle);
			((Rectangle)(ref value))._002Ector(0, 0, 16, 16 - (int)num11 * 2);
			bool flag2 = true;
			if (belowTile.liquid < 245 && (!belowTile.nactive() || !Main.tileSolid[belowTile.type] || Main.tileSolidTop[belowTile.type]))
			{
				float num18 = 256 - belowTile.liquid;
				num18 /= 32f;
				num17 = 0.5f * (8f - num11) / 4f;
				if ((double)num17 > 0.55)
				{
					num17 = 0.55f;
				}
				if ((double)num17 < 0.35)
				{
					num17 = 0.35f;
				}
				float num19 = num11 / 2f;
				if (belowTile.liquid < 200)
				{
					if (aboveTile.liquid > 0 && aboveTile.liquid > 0)
					{
						((Rectangle)(ref value))._002Ector(0, 4, 16, 16);
						num17 = 0.5f;
					}
					else if (aboveTile.liquid > 0)
					{
						((Vector2)(ref vector2))._002Ector((float)(j * 16), (float)(i * 16 + 4));
						((Rectangle)(ref value))._002Ector(0, 4, 16, 12);
						num17 = 0.5f;
					}
					else if (belowTile.liquid > 0)
					{
						((Vector2)(ref vector2))._002Ector((float)(j * 16), (float)(i * 16 + (int)num11 * 2 + (int)num18 * 2));
						((Rectangle)(ref value))._002Ector(0, 4, 16, 16 - (int)num11 * 2);
					}
					else
					{
						((Vector2)(ref vector2))._002Ector((float)(j * 16 + (int)num19), (float)(i * 16 + (int)num19 * 2 + (int)num18 * 2));
						((Rectangle)(ref value))._002Ector(0, 4, 16 - (int)num19 * 2, 16 - (int)num19 * 2);
					}
				}
				else
				{
					num17 = 0.5f;
					((Rectangle)(ref value))._002Ector(0, 4, 16, 16 - (int)num11 * 2 + (int)num18 * 2);
				}
			}
			else if (aboveTile.liquid > 32)
			{
				((Rectangle)(ref value))._002Ector(0, 4, value.Width, value.Height);
			}
			else if (num11 < 1f && aboveTile.nactive() && Main.tileSolid[aboveTile.type] && !Main.tileSolidTop[aboveTile.type])
			{
				((Vector2)(ref vector2))._002Ector((float)(j * 16), (float)(i * 16));
				((Rectangle)(ref value))._002Ector(0, 4, 16, 16);
			}
			if ((((Color)(ref color)).R > 20 || ((Color)(ref color)).B > 20 || ((Color)(ref color)).G > 20) && value.Y < 4)
			{
				int num20 = ((Color)(ref color)).R;
				if (((Color)(ref color)).G > num20)
				{
					num20 = ((Color)(ref color)).G;
				}
				if (((Color)(ref color)).B > num20)
				{
					num20 = ((Color)(ref color)).B;
				}
				num20 /= 30;
				if (Main.rand.Next(20000) < num20)
				{
					new Color(255, 255, 255);
					if (tile.honey())
					{
						new Color(255, 255, 50);
					}
				}
			}
			if (tile.honey())
			{
				num17 *= 1.6f;
				if (num17 > 1f)
				{
					num17 = 1f;
				}
			}
			if (tile.lava())
			{
				num17 *= 1.8f;
				if (num17 > 1f)
				{
					num17 = 1f;
				}
			}
			float num23 = (float)(int)((Color)(ref color)).R * num17;
			float num24 = (float)(int)((Color)(ref color)).G * num17;
			float num25 = (float)(int)((Color)(ref color)).B * num17;
			float num26 = (float)(int)((Color)(ref color)).A * num17;
			((Color)(ref color))._002Ector((int)(byte)num23, (int)(byte)num24, (int)(byte)num25, (int)(byte)num26);
			if (flag)
			{
				((Color)(ref color))._002Ector(((Color)(ref color)).ToVector4() * LiquidRenderer.GetShimmerBaseColor(j, i));
			}
			Color color2 = color;
			if (num12 != 1 && ((double)(int)((Color)(ref color2)).R > (double)num6 * 0.6 || (double)(int)((Color)(ref color2)).G > (double)num6 * 0.65 || (double)(int)((Color)(ref color2)).B > (double)num6 * 0.7))
			{
				for (int l = 0; l < 4; l++)
				{
					int num27 = 0;
					int num28 = 0;
					int width = 8;
					int height = 8;
					Color color3 = color2;
					Color color4 = Color.White;
					if (l == 0)
					{
						color4 = Color.White;
						if (value.Height < 8)
						{
							height = value.Height;
						}
					}
					if (l == 1)
					{
						color4 = Color.White;
						num27 = 8;
						if (value.Height < 8)
						{
							height = value.Height;
						}
					}
					if (l == 2)
					{
						color4 = Color.White;
						num28 = 8;
						height = 8 - (16 - value.Height);
					}
					if (l == 3)
					{
						color4 = Color.White;
						num27 = 8;
						num28 = 8;
						height = 8 - (16 - value.Height);
					}
					num23 = (float)(int)((Color)(ref color4)).R * num17;
					num24 = (float)(int)((Color)(ref color4)).G * num17;
					num25 = (float)(int)((Color)(ref color4)).B * num17;
					num26 = (float)(int)((Color)(ref color4)).A * num17;
					((Color)(ref color4))._002Ector((int)(byte)num23, (int)(byte)num24, (int)(byte)num25, (int)(byte)num26);
					((Color)(ref color3)).R = (byte)((((Color)(ref color2)).R * 3 + ((Color)(ref color4)).R * 2) / 5);
					((Color)(ref color3)).G = (byte)((((Color)(ref color2)).G * 3 + ((Color)(ref color4)).G * 2) / 5);
					((Color)(ref color3)).B = (byte)((((Color)(ref color2)).B * 3 + ((Color)(ref color4)).B * 2) / 5);
					((Color)(ref color3)).A = (byte)((((Color)(ref color2)).A * 3 + ((Color)(ref color4)).A * 2) / 5);
					if (flag)
					{
						((Color)(ref color3))._002Ector(((Color)(ref color3)).ToVector4() * LiquidRenderer.GetShimmerBaseColor(j, i));
					}
					spriteBatch.Draw(TextureAssets.Liquid[num12].Value, vector2 + new Vector2((float)num27, (float)num28) + vector, (Rectangle?)new Rectangle(value.X + num27, value.Y + num28, width, height), color3, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
					if (flag)
					{
						spriteBatch.Draw(TextureAssets.Liquid[num12].Value, vector2 + new Vector2((float)num27, (float)num28) + vector, (Rectangle?)new Rectangle(value.X + num27, value.Y + num28 + 36, width, height), LiquidRenderer.GetShimmerGlitterColor(flag2, j, i), 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
					}
				}
			}
			else
			{
				spriteBatch.Draw(TextureAssets.Liquid[num12].Value, vector2 + vector, (Rectangle?)value, color, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
				if (flag)
				{
					value.Y += 36;
					spriteBatch.Draw(TextureAssets.Liquid[num12].Value, vector2 + vector, (Rectangle?)value, LiquidRenderer.GetShimmerGlitterColor(flag2, j, i), 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
				}
			}
			if (belowTile.halfBrick())
			{
				color = Color.White;
				num23 = (float)(int)((Color)(ref color)).R * num17;
				num24 = (float)(int)((Color)(ref color)).G * num17;
				num25 = (float)(int)((Color)(ref color)).B * num17;
				num26 = (float)(int)((Color)(ref color)).A * num17;
				((Color)(ref color))._002Ector((int)(byte)num23, (int)(byte)num24, (int)(byte)num25, (int)(byte)num26);
				if (flag)
				{
					((Color)(ref color))._002Ector(((Color)(ref color)).ToVector4() * LiquidRenderer.GetShimmerBaseColor(j, i));
				}
				((Vector2)(ref vector2))._002Ector((float)(j * 16), (float)(i * 16 + 16));
				spriteBatch.Draw(TextureAssets.Liquid[num12].Value, vector2 + vector, (Rectangle?)new Rectangle(0, 4, 16, 8), color, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
				if (flag)
				{
					spriteBatch.Draw(TextureAssets.Liquid[num12].Value, vector2 + vector, (Rectangle?)new Rectangle(0, 40, 16, 8), LiquidRenderer.GetShimmerGlitterColor(flag2, j, i), 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
				}
			}
		}
	}

	public class SchematicMapPreviewTargetContent : ARenderTargetContentByRequest
	{
		protected override void HandleUseReqest(GraphicsDevice device, SpriteBatch spriteBatch)
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			int startX = schemMapX * 200;
			int startY = schemMapY * 150;
			int endX = startX + 200;
			int endY = startY + 150;
			if (endX > Mods.schematicSizeX)
			{
				endX = Mods.schematicSizeX;
			}
			if (endY > Mods.schematicSizeY)
			{
				endY = Mods.schematicSizeY;
			}
			int width = endX - startX;
			int height = endY - startY;
			if (width <= 0 || height <= 0)
			{
				_wasPrepared = true;
				return;
			}
			PrepareARenderTarget_AndListenToEvents(ref _target, device, width, height, (RenderTargetUsage)1);
			device.SetRenderTarget(_target);
			device.Clear(Color.Transparent);
			spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend);
			int tileIndex = 0;
			Vector2 pos = default(Vector2);
			for (int i = 0; i < Mods.schematicSizeX; i++)
			{
				for (int j = 0; j < Mods.schematicSizeY; j++)
				{
					if (tileIndex >= Mods.copiedTiles.Count)
					{
						continue;
					}
					if (i < startX || j < startY || i > endX || j > endY)
					{
						tileIndex++;
						continue;
					}
					LegacyTile tile = Mods.copiedTiles[tileIndex];
					((Vector2)(ref pos))._002Ector((float)(i - startX), (float)(j - startY));
					if (!(pos.X > (float)width) && !(pos.Y > (float)height))
					{
						MapTile mT = MapHelper.CreateMapTile_FromLegacyTile(tile, i, j);
						spriteBatch.Draw(TextureAssets.MagicPixel.Value, pos, (Rectangle?)new Rectangle(0, 0, 1, 1), MapHelper.GetMapTileXnaColor(ref mT, i, j), 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
						tileIndex++;
					}
				}
			}
			spriteBatch.End();
			device.SetRenderTarget((RenderTarget2D)null);
			_wasPrepared = true;
		}
	}

	public struct ColorTable
	{
		public ushort Wall;

		public List<Color> Colors;

		public ColorTable(ushort wallId)
		{
			Wall = 1;
			Colors = new List<Color>();
			Wall = wallId;
		}
	}

	private static readonly Color previewTint;

	private static Dictionary<Tuple<int, int>, Texture2D> worldTileTextures;

	private static Dictionary<Tuple<int, int>, Texture2D> worldWallTextures;

	public static bool worldCacheNeedsReset;

	public static bool mapCacheNeedsReset;

	private static readonly HashSet<Tuple<int, int>> uniqueTileTypes;

	private static readonly HashSet<Tuple<int, int>> uniqueWallTypes;

	public static SchematicWorldPreviewTargetContent WorldSchematicPreview;

	public static SchematicMapPreviewTargetContent MapSchematicPreview;

	private const byte schemMaxX = 10;

	private const byte schemMaxY = 12;

	private const byte schemMapMaxX = 50;

	private const byte schemMapMaxY = 20;

	private static int schemSizeX;

	private static int schemSizeY;

	private static int schemMapSizeX;

	private static int schemMapSizeY;

	private static int schemX;

	private static int schemY;

	private static int schemMapX;

	private static int schemMapY;

	private static RenderTarget2D[,] worldSchemTargets;

	private static RenderTarget2D mapSchemTarget;

	private static RenderTarget2D[,] mapSchemTargets;

	private static bool requestedWorldPreview;

	private static bool requestedMapPreview;

	private static bool previewRunTwo;

	public static bool HasTargets
	{
		get
		{
			for (int i = 0; i < schemSizeX; i++)
			{
				for (int j = 0; j < schemSizeY; j++)
				{
					if (worldSchemTargets[i, j] != null && !((GraphicsResource)worldSchemTargets[i, j]).IsDisposed)
					{
						return true;
					}
				}
			}
			for (int k = 0; k < schemMapSizeX; k++)
			{
				for (int l = 0; l < schemMapSizeY; l++)
				{
					if (mapSchemTargets[k, l] != null && !((GraphicsResource)mapSchemTargets[k, l]).IsDisposed)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public static async void SaveTEDITSchematic(string fileNameOrPath = "")
	{
		try
		{
			if (Mods.copyingFromFillOrDelMode)
			{
				Main.NewText("Please wait for the copying to finish.", Mods.errorColor);
				return;
			}
			if (Mods.savingTEDITSchematic)
			{
				Main.NewText("Already saving a schematic, please wait for the operation to finish.", Mods.errorColor);
				return;
			}
			Mods.savingTEDITSchematic = true;
			await Task.Run(delegate
			{
				//IL_0114: Unknown result type (might be due to invalid IL or missing references)
				//IL_0165: Unknown result type (might be due to invalid IL or missing references)
				//IL_039a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0290: Unknown result type (might be due to invalid IL or missing references)
				//IL_046b: Unknown result type (might be due to invalid IL or missing references)
				if (Mods.selectionMode && Mods.copiedTiles.Count < 1)
				{
					int num = Math.Min(Mods.selectionA.X, Mods.selectionB.X);
					int num2 = Math.Min(Mods.selectionA.Y, Mods.selectionB.Y);
					Mods.schematicSizeX = Math.Abs(Mods.selectionA.X - Mods.selectionB.X) + 1;
					Mods.schematicSizeY = Math.Abs(Mods.selectionA.Y - Mods.selectionB.Y) + 1;
					Mods.copiedTiles.Clear();
					for (int i = num; i < num + Mods.schematicSizeX; i++)
					{
						for (int j = num2; j < num2 + Mods.schematicSizeY; j++)
						{
							Tile tile = Main.tile[i, j];
							if (tile == null)
							{
								tile = default(Tile);
							}
							if (WorldGen.InWorld(i, j))
							{
								LegacyTile item = new LegacyTile(tile);
								Mods.copiedTiles.Add(item);
							}
						}
					}
					ResetPreview();
				}
				if (Mods.copiedTiles.Count < 1)
				{
					Main.NewText("You haven't loaded any schematics, nor have you copied any tiles.", Mods.errorColor);
					Mods.savingTEDITSchematic = false;
				}
				else
				{
					if (!Directory.Exists(Path.Combine(Main.SavePath, "Schematics")))
					{
						Directory.CreateDirectory(Path.Combine(Main.SavePath, "Schematics"));
					}
					bool flag = false;
					if (string.IsNullOrEmpty(fileNameOrPath))
					{
						Main.NewText("Saving as a T-Edit Schematic...", Mods.zazasFavoriteColor);
						fileNameOrPath = FileBrowser.OpenFileSavePanel("Save TEdit Schematic File", "TEditSch", (!Platform.IsLinux) ? Path.Combine(Main.SavePath, "Schematics") : null);
						flag = true;
					}
					if (fileNameOrPath != null)
					{
						if (!flag)
						{
							char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
							string text = "";
							if (fileNameOrPath.Length > 80)
							{
								fileNameOrPath = fileNameOrPath.Substring(0, 80);
							}
							fileNameOrPath = fileNameOrPath.Replace(".", "_");
							fileNameOrPath = fileNameOrPath.Replace("*", "_");
							string text2 = fileNameOrPath;
							foreach (char c in text2)
							{
								text += ((!Enumerable.Contains(invalidFileNameChars, c)) ? ((c != ' ') ? c : '_') : '-');
							}
							fileNameOrPath = text;
							if (fileNameOrPath.Length > 30)
							{
								Main.NewText("File name is too long. Please use a file name shorter than 30 characters. Make sure you didn't input a file path instead of a file name.", Mods.errorColor);
								Mods.savingTEDITSchematic = false;
							}
							int num3 = 1;
							string text3 = Path.GetFileNameWithoutExtension(Path.Combine(Main.SavePath, "Schematics", fileNameOrPath));
							while (Path.Exists(Path.Combine(Main.SavePath, "Schematics", text3)))
							{
								text3 += $"-{num3}";
								num3++;
							}
							fileNameOrPath = Path.Combine(Main.SavePath, "Schematics", text3);
						}
						if (!fileNameOrPath.ToLower().EndsWith(".teditsch"))
						{
							fileNameOrPath += ".TEditSch";
						}
						using FileStream output = new FileStream(fileNameOrPath, FileMode.Create);
						using BinaryWriter binaryWriter = new BinaryWriter(output);
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameOrPath);
						int schematicSizeX = Mods.schematicSizeX;
						int schematicSizeY = Mods.schematicSizeY;
						if (schematicSizeX >= 1000 || schematicSizeY >= 1000)
						{
							Main.NewText("Larger sizes take longer to save...", Mods.zazasFavoriteColor);
						}
						short value = 693;
						binaryWriter.Write(fileNameWithoutExtension);
						binaryWriter.Write(10279);
						binaryWriter.Write(value);
						binaryWriter.Write(Mods.TEditImportantFrames);
						binaryWriter.Write(schematicSizeX);
						binaryWriter.Write(schematicSizeY);
						SaveAllTileData(binaryWriter, schematicSizeX, schematicSizeY);
						binaryWriter.Write(0);
						short value2 = 0;
						binaryWriter.Write(value2);
						binaryWriter.Write(0);
						binaryWriter.Write(fileNameWithoutExtension);
						binaryWriter.Write(10279);
						binaryWriter.Write(schematicSizeX);
						binaryWriter.Write(schematicSizeY);
						Main.NewText($"Saved: {fileNameWithoutExtension}  |  Size: [X: {schematicSizeX} Y: {schematicSizeY}]  |  Version: {10279}", Mods.zazasFavoriteColor);
						Mods.savingTEDITSchematic = false;
					}
					Mods.savingTEDITSchematic = false;
				}
			});
			Mods.savingTEDITSchematic = false;
		}
		catch (Exception ex)
		{
			Main.NewText($"An error has occured while attempting to save TEditSch files\n{ex}", Mods.errorColor);
			Mods.savingTEDITSchematic = false;
			Mods.LogException(ex);
		}
		Mods.savingTEDITSchematic = false;
	}

	public static async void LoadTEDITSchematic(string filePath = "")
	{
		try
		{
			if (Mods.openingTEDITSchematic)
			{
				Main.NewText("Already selecting a schematic, please wait for the operation to finish.", Mods.errorColor);
				return;
			}
			Mods.openingTEDITSchematic = true;
			await Task.Run(delegate
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_014e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0127: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
				if (string.IsNullOrEmpty(filePath))
				{
					Main.NewText("Select a T-Edit Schematic...", Mods.zazasFavoriteColor);
					if (!Directory.Exists(Path.Combine(Main.SavePath, "Schematics")))
					{
						Directory.CreateDirectory(Path.Combine(Main.SavePath, "Schematics"));
					}
					filePath = FileBrowser.OpenFilePanel("Open TEdit Schematic File", "TEditSch", (!Platform.IsLinux) ? Path.Combine(Main.SavePath, "Schematics") : null);
				}
				else if (!Path.Exists(filePath))
				{
					Main.NewText("The specified file doesn't exist.", Mods.errorColor);
					Mods.openingTEDITSchematic = false;
					return;
				}
				if (filePath != null)
				{
					if (!filePath.ToLower().EndsWith(".teditsch"))
					{
						Main.NewText("This isn't a valid TEdit Schematic file. Schematic files use the .TEditSch extension.", Mods.errorColor);
						Mods.openingTEDITSchematic = false;
						return;
					}
					Mods.pasteDelayCounter = Mods.pasteDelay;
					using (FileStream input = new FileStream(filePath, FileMode.Open))
					{
						using BinaryReader binaryReader = new BinaryReader(input);
						string text = binaryReader.ReadString();
						int num = binaryReader.ReadInt32();
						if (num < 10279)
						{
							Main.NewText($"Schematic version {((num > 10000) ? (num - 10000) : num)} isn't compatible with ZazaMod.\nThe schematic you've selected was saved on an outdated world.\nTo updated this schematic upload it in a T-Edit world that was created during 1.4.4.9.", Mods.errorColor);
						}
						else if (num > 10279)
						{
							Main.NewText($"Schematic version {num} isn't compatible with ZazaMod.\nThe schematic you've selected was saved on an unsupported version.\nPlease use an older version of TEdit from 1.4.4.9 and re-create the schematic.", Mods.errorColor);
						}
						else
						{
							if (Mods.copiedTiles.Count > 0)
							{
								Mods.copiedTiles.Clear();
							}
							binaryReader.ReadBytes(89);
							int num2 = binaryReader.ReadInt32();
							int num3 = binaryReader.ReadInt32();
							Mods.copiedTiles = LoadAllTileData(binaryReader, num2, num3, text, num - ((num > 10000) ? 10000 : 0));
							ResetPreview();
							Main.NewText($"Selected: {text}  |  Size: [X: {num2}, Y: {num3}]  |  Version: {num}", Mods.zazasFavoriteColor);
							Mods.openingTEDITSchematic = false;
						}
					}
					Mods.openingTEDITSchematic = false;
					Mods.Instance.DisableBrushes();
					Mods.selectionMode = false;
					Mods.copyMode = false;
					Mods.drawPhotoMode = false;
					Mods.pasteMode = true;
					Mods.pasteDelayCounter = Mods.pasteDelay;
					Mods.motionlessPaste = false;
				}
				Mods.openingTEDITSchematic = false;
			});
		}
		catch (Exception ex)
		{
			Main.NewText($"An error has occured while attempting to load TEditSch files\n{ex}", Mods.errorColor);
			Mods.LogException(ex);
		}
		Mods.openingTEDITSchematic = false;
	}

	public static async void PasteTiles(Point mouseToTile)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		await Task.Run(delegate
		{
			//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_060e: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Mods.tileLoopCount = 0;
				if (Mods.autoExitPaste)
				{
					Mods.pasteMode = false;
				}
				if (Mods.openingTEDITSchematic)
				{
					Main.NewText("You can't paste while opening a schematic. Please close the dialog or select a schematic.", Mods.errorColor);
				}
				else
				{
					if (Mods.concurrentPastes < 6)
					{
						Mods.pasteDelayCounter = Mods.pasteDelay;
						int schematicSizeX = Mods.schematicSizeX;
						int schematicSizeY = Mods.schematicSizeY;
						List<LegacyTile> list = new List<LegacyTile>();
						for (int i = 0; i < Mods.copiedTiles.Count; i++)
						{
							LegacyTile legacyTile = new LegacyTile();
							legacyTile.CopyFrom(Mods.copiedTiles[i]);
							list.Add(legacyTile);
						}
						bool cancelThePaste = false;
						bool flag = false;
						lock (list)
						{
							Mods.concurrentPastes++;
							int totalPastedTiles;
							int pasteWidth;
							int pasteHeight;
							Stopwatch stopwatch;
							while (true)
							{
								totalPastedTiles = 0;
								pasteWidth = schematicSizeX;
								pasteHeight = schematicSizeY;
								int packetsSent = 0;
								_ = Main.LocalPlayer;
								if (Mods.cancelPaste)
								{
									Mods.cancelPaste = false;
								}
								if ((pasteWidth > 50 || pasteHeight > 50) && Main.netMode == 1 && !flag)
								{
									Main.NewText("Pasting tiles, this may take a while.", Mods.zazasFavoriteColor);
								}
								flag = true;
								if ((pasteWidth <= 0 && pasteHeight <= 0) || list.Count == 0)
								{
									Main.NewText("No tiles have been copied.", Mods.errorColor);
									Mods.pasteMode = false;
									Mods.concurrentPastes--;
								}
								else
								{
									bool flag2 = false;
									bool flag3 = false;
									bool flag4 = false;
									bool flag5 = false;
									if (Main.netMode == 1 && Mods.pasteInTShock)
									{
										for (int j = 0; j < list.Count; j++)
										{
											if (!flag4 && TileID.Sets.BasicChest[list[j].type])
											{
												flag4 = true;
											}
											if (!flag5 && TileObjectData.CustomPlace(list[j].type, 0))
											{
												flag5 = true;
											}
											if (!flag2 && (list[j].halfBrick() || list[j].slope() > 0))
											{
												flag2 = true;
											}
											if (!flag3 && TileID.Sets.Falling[list[j].type])
											{
												flag3 = true;
											}
										}
									}
									PasteTiles_BasicTiles_Walls(list, mouseToTile, pasteWidth, pasteHeight, out packetsSent, out totalPastedTiles);
									if (packetsSent != -1)
									{
										if (flag4 && Main.netMode != 0)
										{
											PasteTiles_Chests(list, mouseToTile, packetsSent, pasteWidth, pasteHeight, out packetsSent);
											if (packetsSent == -1)
											{
												return;
											}
										}
										if (flag5 && Main.netMode != 0 && Mods.pasteInTShock)
										{
											Thread.Sleep(100);
											for (int k = 0; k < 2; k++)
											{
												PasteTiles_TileObjects(list, mouseToTile, packetsSent, pasteWidth, pasteHeight, out packetsSent);
												if (packetsSent == -1)
												{
													return;
												}
											}
										}
										if (flag3 && Main.netMode != 0 && Mods.pasteInTShock)
										{
											Thread.Sleep(100);
											PasteTiles_RevertGravityBlocks(list, mouseToTile, pasteWidth, pasteHeight, out cancelThePaste);
											if (cancelThePaste)
											{
												return;
											}
										}
										if (flag2 && Main.netMode != 0 && Mods.pasteInTShock)
										{
											Thread.Sleep(100);
											PasteTiles_FixSlopes(list, mouseToTile, pasteWidth, pasteHeight, out cancelThePaste);
											if (cancelThePaste)
											{
												return;
											}
										}
										Thread.Sleep(100);
										PasteTiles_DoPaint(list, mouseToTile, pasteWidth, pasteHeight, out cancelThePaste);
										if (!cancelThePaste)
										{
											Thread.Sleep(100);
											PasteTiles_DoWiring(list, mouseToTile, pasteWidth, pasteHeight, out cancelThePaste);
											if (!cancelThePaste)
											{
												Thread.Sleep(100);
												stopwatch = new Stopwatch();
												stopwatch.Start();
												Main.QueueMainThreadAction(delegate
												{
													WorldGen.RangeFrame(mouseToTile.X - 2, mouseToTile.Y - 2, mouseToTile.X + pasteWidth + 2, mouseToTile.Y + pasteHeight + 2, resetFrame: false, noBreak: true);
												});
												if (Main.netMode != 0 && Mods.pasteInTShock && Main.netMode == 1)
												{
													for (int num = mouseToTile.X; num < mouseToTile.X + pasteWidth; num++)
													{
														for (int num2 = mouseToTile.Y; num2 < mouseToTile.Y + pasteHeight; num2++)
														{
															if (Mods.pasteInTShock && Mods.didTShockDisableYou)
															{
																num = mouseToTile.X;
																num2 = mouseToTile.Y;
																Thread.Sleep(100);
															}
															Tile tile = Main.tile[num, num2];
															NetMessage.SendObjectPlacement(Main.myPlayer, num, num2, tile.type, 0, 0, 0, 1);
														}
													}
												}
												if (Mods.pasteInTShock && Main.netMode == 1)
												{
													int num3 = 500;
													if (pasteWidth > 50 || pasteHeight > 50)
													{
														num3 = 2500;
													}
													int num4 = 0;
													while (num4 < num3)
													{
														if (!Mods.didTShockDisableYou)
														{
															Thread.Sleep(1);
															num4++;
															continue;
														}
														goto IL_04f3;
													}
												}
												break;
											}
										}
									}
								}
								return;
								IL_04f3:
								Main.NewText("Assumed TShock disabled you, pasting will restart after disable ends. Lower your packet rate to avoid this.", Mods.errorColor);
								while (Mods.didTShockDisableYou)
								{
									Thread.Sleep(20);
									if (Mods.cancelPaste)
									{
										Main.NewText("Caneled paste.", Mods.zazasFavoriteColor);
										Mods.cancelPaste = false;
										Mods.concurrentPastes--;
										NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
										NetMessage.SendData(13);
										Main.QueueMainThreadAction(delegate
										{
											WorldGen.RangeFrame(mouseToTile.X, mouseToTile.Y, mouseToTile.X + pasteWidth, mouseToTile.Y + pasteHeight, resetFrame: false, noBreak: true);
										});
										return;
									}
								}
								Thread.Sleep(250);
								packetsSent = 0;
							}
							stopwatch.Stop();
							Main.NewText($"Pasted {totalPastedTiles} tiles.", Mods.zazasFavoriteColor);
							Mods.concurrentPastes--;
							NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
							NetMessage.SendData(13);
							return;
						}
					}
					Main.NewText("Cannot have more than 6 pastes running at once!", Mods.errorColor);
					Mods.pasteMode = false;
				}
			}
			catch (Exception ex)
			{
				Mods.concurrentPastes--;
				Mods.pasteMode = false;
				Main.NewText($"Paste Tiles error!\n{ex}", Mods.errorColor);
				Mods.LogException(ex);
				NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
				NetMessage.SendData(13);
			}
		});
	}

	public static int PasteTiles_ConvertTiles(int x, int y, int packetsSent, Tile tile, Vector2 pastePos)
	{
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		bool needsTileRect = false;
		bool specialCase = false;
		int itemType = 0;
		int tileType = 0;
		switch (tile.type)
		{
		case 2:
		case 23:
		case 109:
		case 199:
			itemType = 2;
			break;
		case 661:
		case 662:
			itemType = 176;
			tileType = 59;
			needsTileRect = true;
			break;
		case 60:
		case 70:
			itemType = 176;
			tileType = 59;
			break;
		case 477:
			itemType = 2;
			tile.type = 2;
			needsTileRect = true;
			break;
		case 492:
			itemType = 2;
			tile.type = 109;
			needsTileRect = true;
			break;
		case 633:
			itemType = 172;
			tileType = 57;
			break;
		case 179:
		case 180:
		case 181:
		case 182:
		case 183:
		case 381:
		case 534:
		case 536:
		case 539:
		case 625:
		case 627:
			itemType = 3;
			tileType = 1;
			break;
		case 512:
		case 513:
		case 514:
		case 515:
		case 516:
		case 517:
		case 535:
		case 537:
		case 540:
		case 626:
		case 628:
			itemType = 129;
			tileType = 38;
			specialCase = true;
			break;
		}
		if (itemType > 0)
		{
			Mods.FakeNetMessage13(pastePos, 58);
			ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, itemType);
			NetMessage.SendData(17, -1, -1, null, 1, x, y, tileType);
			packetsSent++;
			if (specialCase)
			{
				switch (tile.type)
				{
				case 512:
					itemType = 4349;
					tileType = 179;
					break;
				case 513:
					itemType = 4350;
					tileType = 180;
					break;
				case 514:
					itemType = 4351;
					tileType = 181;
					break;
				case 515:
					itemType = 4352;
					tileType = 182;
					break;
				case 516:
					itemType = 4353;
					tileType = 183;
					break;
				case 517:
					itemType = 4354;
					tileType = 381;
					break;
				case 535:
					itemType = 4377;
					tileType = 534;
					break;
				case 537:
					itemType = 4378;
					tileType = 536;
					break;
				case 540:
					itemType = 4389;
					tileType = 539;
					break;
				case 626:
					itemType = 5127;
					tileType = 625;
					break;
				case 628:
					itemType = 5128;
					tileType = 627;
					break;
				}
				ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, itemType);
				NetMessage.SendData(17, -1, -1, null, 1, x, y, tileType);
				packetsSent++;
			}
			if (needsTileRect)
			{
				switch (tile.type)
				{
				case 2:
					ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, 62f);
					NetMessage.SendData(17, -1, -1, null, 1, x, y, 2f);
					tile.type = 477;
					break;
				case 109:
					ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, 369f);
					NetMessage.SendData(17, -1, -1, null, 1, x, y, 109f);
					tile.type = 492;
					break;
				case 661:
					ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, 195f);
					NetMessage.SendData(17, -1, -1, null, 1, x, y, 60f);
					tile.type = 661;
					break;
				case 662:
					ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, 195f);
					NetMessage.SendData(17, -1, -1, null, 1, x, y, 60f);
					tile.type = 662;
					break;
				}
				NetMessage.SendTileSquare(-1, x, y);
				packetsSent += 2;
			}
		}
		return packetsSent;
	}

	public static void PasteTiles_BasicTiles_Walls(List<LegacyTile> tilesToPaste, Point mouseToTile, int pasteWidth, int pasteHeight, out int packetsSent, out int totalPastedTiles)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		packetsSent = 0;
		totalPastedTiles = 0;
		Vector2 fakePos = default(Vector2);
		while (true)
		{
			int tileIndex = 0;
			bool warnedOfUnloadedSection = false;
			int i = mouseToTile.X;
			while (true)
			{
				if (i < mouseToTile.X + pasteWidth)
				{
					for (int j = mouseToTile.Y; j < mouseToTile.Y + pasteHeight; j++)
					{
						if (Mods.pasteInTShock && Mods.didTShockDisableYou)
						{
							Main.NewText("Assumed TShock disabled you, pasting will resume after disable ends. Lower your packet rate to avoid this.", Mods.errorColor);
							i = mouseToTile.X;
							j = mouseToTile.Y;
							tileIndex = 0;
							totalPastedTiles = 0;
							while (Mods.didTShockDisableYou)
							{
								Thread.Sleep(20);
								if (!Mods.cancelPaste)
								{
									continue;
								}
								goto IL_00b0;
							}
							goto IL_009d;
						}
						goto IL_00b0;
						IL_00b0:
						if (Mods.cancelPaste)
						{
							Main.NewText("Caneled paste.", Mods.zazasFavoriteColor);
							Mods.cancelPaste = false;
							Mods.concurrentPastes--;
							packetsSent = -1;
							NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
							NetMessage.SendData(13);
							Main.QueueMainThreadAction(delegate
							{
								WorldGen.RangeFrame(mouseToTile.X, mouseToTile.Y, mouseToTile.X + pasteWidth, mouseToTile.Y + pasteHeight, resetFrame: false, noBreak: true);
							});
							return;
						}
						if (Main.gameMenu)
						{
							Mods.concurrentPastes = 0;
							return;
						}
						Tile tile = Main.tile[i, j];
						LegacyTile oTile = tilesToPaste[tileIndex];
						if (!Main.sectionManager.SectionLoaded(Netplay.GetSectionX(i), Netplay.GetSectionY(j)))
						{
							if (!warnedOfUnloadedSection)
							{
								Main.NewText("Sections are not fully loaded, paste may be slow initially.", Mods.errorColor);
								warnedOfUnloadedSection = true;
							}
							Mods.LoadChunk(i, j);
							Thread.Sleep(1000);
						}
						if (oTile.wall > 0 && Main.netMode == 1 && Mods.pasteInTShock)
						{
							oTile.wall = Mods.ConvertUnsafeWall(oTile.wall);
						}
						if (Main.netMode == 1 && Mods.pasteInTShock)
						{
							LazyTileFixup(oTile);
						}
						LegacyTile copyTile = new LegacyTile(oTile);
						if (Main.netMode == 1 && Mods.pasteInTShock && tile.TileType != copyTile.type)
						{
							copyTile.type = ConvertGravityToNonGravity(copyTile.type);
						}
						bool newTileHasTile = copyTile.active();
						bool newTileHasWall = copyTile.wall > 0;
						bool sameTile = (tile.TileType == copyTile.type && tile.BlockType == (BlockType)copyTile.blockType()) & newTileHasTile;
						bool sameWall = (tile.WallType == copyTile.wall) & newTileHasWall;
						bool tileFullySame = (Mods.pasteInTShock ? IsTileTheSame(tile, copyTile) : tile.isTheSameAsLegacy(copyTile));
						bool removeIfTileAir = !Mods.blendAirTileMode || !Mods.IsTileAir(copyTile);
						if ((Mods.pasteInTShock && Main.netMode == 1) & removeIfTileAir)
						{
							if (tile.HasTile && !sameTile)
							{
								tile.ClearTile();
								NetMessage.SendData(17, -1, -1, null, 4, i, j);
							}
							if (tile.wall > 0 && !sameWall)
							{
								tile.Clear(TileDataType.Wall);
								tile.Clear(TileDataType.WallPaint);
								NetMessage.SendData(17, -1, -1, null, 2, i, j);
							}
						}
						if ((WorldGen.InWorld(i, j) && !tileFullySame) & removeIfTileAir)
						{
							CopyRelevantTileData(tile, copyTile);
							if (IsSameFurnitureTile(tile, copyTile) && Main.netMode == 1 && Mods.pasteInTShock)
							{
								tile.type = 0;
								tile.HasTile = false;
								tile.BlockType = BlockType.Solid;
								tile.TileFrameX = -1;
								tile.TileFrameY = -1;
								sameTile = false;
							}
							if (Mods.pasteInTShock && Main.netMode == 1)
							{
								bool tilePasted = false;
								((Vector2)(ref fakePos))._002Ector((float)((i + 2) * 16), (float)(j * 16));
								Mods.FakeNetMessage13(fakePos, 58);
								if (tile.WallType > 0 && Mods.wallToItemDictionarySearch.ContainsKey(tile.WallType) && !sameWall)
								{
									ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, Mods.wallToItemDictionarySearch[tile.wall]);
									NetMessage.SendData(17, -1, -1, null, 3, i, j, (int)tile.wall);
									packetsSent++;
									totalPastedTiles++;
									tilePasted = true;
								}
								Item toUse = GetItemDrop(i, j, copyTile, includeLargeObjectDrops: true);
								if (toUse.type == 496)
								{
									toUse.createTile = 127;
								}
								if (toUse.type == 4262)
								{
									toUse.createTile = 504;
								}
								if ((tile.HasTile && toUse.createTile != -1 && toUse.createTile == tile.TileType) || ShouldPlaceThisTile(tile))
								{
									packetsSent = PasteTiles_ConvertTiles(i, j, packetsSent, tile, fakePos);
									if (tile.HasTile && !toUse.IsAir)
									{
										Mods.FakeNetMessage13(fakePos, 58);
										ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, toUse.netID);
										NetMessage.SendData(17, -1, -1, null, 1, i, j, (int)tile.TileType, toUse.placeStyle);
										if (!tilePasted)
										{
											totalPastedTiles++;
											tilePasted = true;
										}
										packetsSent++;
										if (tile.BlockType != BlockType.Solid)
										{
											tile.BlockType = BlockType.Solid;
											NetMessage.SendData(17, -1, -1, null, 14, i, j, (int)tile.slope());
										}
									}
								}
								if (tile.TileType == 137 || tile.type == 314)
								{
									NetMessage.SendData(20, -1, -1, null, i, j, 1f, 1f);
								}
							}
							while (Mods.tilePacketMaxMode && (packetsSent > Mods.tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
							{
								packetsSent = 0;
								Thread.Sleep(500);
							}
							if (!Mods.tilePacketMaxMode && ((packetsSent > 0 && packetsSent % Mods.tilePacketRate == 0) || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
							{
								packetsSent = 0;
								Thread.Sleep(100);
							}
						}
						tileIndex++;
						Main.Map.UpdateLighting(i, j, byte.MaxValue);
						Main.refreshMap = true;
					}
					i++;
					continue;
				}
				Thread.Sleep(250);
				if (Mods.pasteInTShock && Mods.didTShockDisableYou)
				{
					Main.NewText("Assumed TShock disabled you, pasting will resume after disable ends. Lower your packet rate to avoid this.", Mods.errorColor);
					tileIndex = 0;
					totalPastedTiles = 0;
					while (Mods.didTShockDisableYou)
					{
						Thread.Sleep(20);
						if (Mods.cancelPaste)
						{
							Main.NewText("Caneled paste.", Mods.zazasFavoriteColor);
							Mods.cancelPaste = false;
							Mods.concurrentPastes--;
							packetsSent = -1;
							NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
							NetMessage.SendData(13);
							Main.QueueMainThreadAction(delegate
							{
								WorldGen.RangeFrame(mouseToTile.X, mouseToTile.Y, mouseToTile.X + pasteWidth, mouseToTile.Y + pasteHeight, resetFrame: false, noBreak: true);
							});
							return;
						}
					}
					packetsSent = 0;
					Thread.Sleep(250);
					break;
				}
				if (Main.netMode == 0 || Mods.pasteInTShock)
				{
					return;
				}
				int endX = mouseToTile.X + pasteWidth;
				int endY = mouseToTile.Y + pasteHeight;
				_ = mouseToTile;
				_ = mouseToTile;
				int xS = ((mouseToTile.X + 5 <= endX) ? 5 : (endX - mouseToTile.X));
				int yS = ((mouseToTile.Y + 5 <= endY) ? 5 : (endY - mouseToTile.Y));
				for (int i2 = mouseToTile.X; i2 < endX; i2 += xS)
				{
					for (int j2 = mouseToTile.Y; j2 < endY; j2 += yS)
					{
						xS = ((i2 + 5 <= endX) ? 5 : (endX - i2));
						yS = ((j2 + 5 <= endY) ? 5 : (endY - j2));
						NetMessage.SendTileSquare(-1, i2, j2, xS, yS);
						packetsSent++;
						if (Mods.cancelPaste)
						{
							Main.NewText("Caneled paste.", Mods.zazasFavoriteColor);
							Mods.cancelPaste = false;
							Mods.concurrentPastes--;
							packetsSent = -1;
							NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
							NetMessage.SendData(13);
							Main.QueueMainThreadAction(delegate
							{
								WorldGen.RangeFrame(mouseToTile.X, mouseToTile.Y, mouseToTile.X + pasteWidth, mouseToTile.Y + pasteHeight, resetFrame: false, noBreak: true);
							});
							return;
						}
						while (Mods.tilePacketMaxMode && (packetsSent > Mods.tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
						{
							packetsSent = 0;
							Thread.Sleep(500);
						}
						if (!Mods.tilePacketMaxMode && ((packetsSent > 0 && packetsSent % Mods.tilePacketRate == 0) || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
						{
							packetsSent = 0;
							Thread.Sleep(100);
						}
					}
				}
				return;
				IL_009d:
				packetsSent = 0;
				Thread.Sleep(250);
				break;
			}
		}
	}

	public static bool IsTileTheSame(Tile t, LegacyTile lT)
	{
		if (t.HasTile == lT.active() && t.IsActuated == lT.inActive() && t.TileType == lT.type && t.WallType == lT.wall && t.halfBrick() == lT.halfBrick())
		{
			return t.slope() == lT.slope();
		}
		return false;
	}

	public static bool IsSameFurnitureTile(Tile t, LegacyTile lT)
	{
		if (TileObjectData.CustomPlace(t.type, 0) && TileObjectData.CustomPlace(lT.type, 0) && t.type == lT.type && t.frameX == lT.frameX)
		{
			return t.frameY == lT.frameY;
		}
		return false;
	}

	public static void CopyRelevantTileData(Tile t, LegacyTile copy)
	{
		t.type = copy.type;
		t.wall = copy.wall;
		if (!Mods.pasteInTShock || Main.netMode != 1)
		{
			t.liquid = copy.liquid;
			t.liquidType(copy.liquidType());
			t.wire(copy.wire());
		}
		t.active(copy.active());
		if (!Mods.pasteInTShock || Main.netMode != 1)
		{
			t.halfBrick(copy.halfBrick());
			t.slope(copy.slope());
			t.actuator(copy.actuator());
			t.inActive(copy.inActive());
			t.color(copy.color());
			t.wallColor(copy.wallColor());
			t.wire2(copy.wire2());
			t.wire3(copy.wire3());
			t.wire4(copy.wire4());
			t.invisibleBlock(copy.invisibleBlock());
			t.invisibleWall(copy.invisibleWall());
			t.fullbrightBlock(copy.fullbrightBlock());
			t.fullbrightWall(copy.fullbrightWall());
		}
		t.frameX = copy.frameX;
		t.frameY = copy.frameY;
	}

	public static ushort ConvertGravityToNonGravity(ushort type)
	{
		return type switch
		{
			53 => 397, 
			112 => 398, 
			116 => 402, 
			234 => 399, 
			123 => 38, 
			224 => 273, 
			330 => 47, 
			331 => 46, 
			332 => 45, 
			333 => 177, 
			495 => 315, 
			_ => type, 
		};
	}

	public static void PasteTiles_Chests(List<LegacyTile> tilesToPaste, Point mouseToTile, int packets, int pasteWidth, int pasteHeight, out int packetsSent)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		packetsSent = 0;
		int tileIndex = 0;
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		for (int i = mouseToTile.X; i < mouseToTile.X + pasteWidth; i++)
		{
			for (int j = mouseToTile.Y; j < mouseToTile.Y + pasteHeight; j++)
			{
				while (Mods.pasteInTShock && Mods.didTShockDisableYou)
				{
					Main.NewText("Assumed TShock disabled you, pasting will resume after disable ends. Lower your tile packet rate to avoid this.", Mods.errorColor);
					i = mouseToTile.X;
					j = mouseToTile.Y;
					tileIndex = 0;
					while (Mods.didTShockDisableYou)
					{
						Thread.Sleep(20);
						if (Mods.cancelPaste)
						{
							goto end_IL_0051;
						}
					}
					packetsSent = 0;
					Thread.Sleep(250);
					continue;
					end_IL_0051:
					break;
				}
				if (Mods.cancelPaste)
				{
					Main.NewText("Caneled paste.", Mods.zazasFavoriteColor);
					Mods.cancelPaste = false;
					Mods.concurrentPastes--;
					NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
					NetMessage.SendData(13);
					Main.QueueMainThreadAction(delegate
					{
						WorldGen.RangeFrame(mouseToTile.X, mouseToTile.Y, mouseToTile.X + pasteWidth, mouseToTile.Y + pasteHeight, resetFrame: false, noBreak: true);
					});
					packetsSent = -1;
					return;
				}
				if (Main.gameMenu)
				{
					Mods.concurrentPastes = 0;
					packetsSent = -1;
					return;
				}
				if (tileIndex >= tilesToPaste.Count)
				{
					continue;
				}
				LegacyTile tile = tilesToPaste[tileIndex];
				if (tile.active() && (TileID.Sets.BasicChest[tile.type] || tile.type == 82 || tile.type == 227))
				{
					Item item = GetItemDrop(i, j, tile, includeLargeObjectDrops: true);
					if (WorldGen.InWorld(i, j, 10) && item.createTile > -1 && item.createTile < TileID.Count)
					{
						Mods.FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), 58);
						ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, item.netID);
						WorldGen.PlaceTile(i, j, tile.type, mute: true, forced: true, Main.myPlayer, item.placeStyle);
						packetsSent++;
					}
				}
				tileIndex++;
				while (Mods.tilePacketMaxMode && (packetsSent > Mods.tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
				{
					packetsSent = 0;
					Thread.Sleep(500);
				}
				if (!Mods.tilePacketMaxMode && ((packetsSent > 0 && packetsSent % Mods.tilePacketRate == 0) || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
				{
					packetsSent = 0;
					Thread.Sleep(100);
				}
				Main.Map.UpdateLighting(i, j, byte.MaxValue);
				Main.refreshMap = true;
			}
		}
		packetsSent += packets;
		stopwatch.Stop();
	}

	public static void PasteTiles_TileObjects(List<LegacyTile> tilesToPaste, Point mouseToTile, int packets, int pasteWidth, int pasteHeight, out int packetsSent)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		packetsSent = 0;
		int l = tilesToPaste.Count - 1;
		for (int i = mouseToTile.X - 1 + pasteWidth; i >= mouseToTile.X; i--)
		{
			for (int j = mouseToTile.Y - 1 + pasteHeight; j >= mouseToTile.Y; j--)
			{
				while (Mods.pasteInTShock && Mods.didTShockDisableYou)
				{
					Main.NewText("Assumed TShock disabled you, pasting will resume after disable ends. Lower your tile packet rate to avoid this.", Mods.errorColor);
					i = mouseToTile.X + pasteWidth - 1;
					j = mouseToTile.Y + pasteHeight - 1;
					l = tilesToPaste.Count - 1;
					while (Mods.didTShockDisableYou)
					{
						Thread.Sleep(20);
						if (Mods.cancelPaste)
						{
							goto end_IL_005d;
						}
					}
					packetsSent = 0;
					Thread.Sleep(250);
					continue;
					end_IL_005d:
					break;
				}
				if (Mods.cancelPaste)
				{
					Main.NewText("Caneled paste.", Mods.zazasFavoriteColor);
					Mods.cancelPaste = false;
					Mods.concurrentPastes--;
					NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
					NetMessage.SendData(13);
					Main.QueueMainThreadAction(delegate
					{
						WorldGen.RangeFrame(mouseToTile.X, mouseToTile.Y, mouseToTile.X + pasteWidth, mouseToTile.Y + pasteHeight, resetFrame: false, noBreak: true);
					});
					packetsSent = -1;
					return;
				}
				if (Main.gameMenu)
				{
					Mods.concurrentPastes = 0;
					packetsSent = -1;
					return;
				}
				if (l < tilesToPaste.Count)
				{
					LegacyTile tile = tilesToPaste[l];
					if (tile.active())
					{
						Item item = GetItemDrop(i, j, tile, includeLargeObjectDrops: true);
						if (WorldGen.InWorld(i, j, 10) && item.createTile > -1 && item.createTile < TileID.Count && TileObjectData.CustomPlace(item.createTile, item.placeStyle) && (item.createTile == tile.type || item.GetFlexibleTileWand() != null))
						{
							TileObject objectData = default(TileObject);
							if (Main.rand == null)
							{
								Main.rand = new UnifiedRandom();
							}
							TileObjectData data = TileObjectData.GetTileData(tile);
							if (data == null)
							{
								Main.NewText($"Error!\nGetTileData returned null for {TileID.Search.GetName((int)tile.type)} ({tile.type})!\nPlease report this as soon as possible!", Mods.errorColor);
							}
							if (data != null && TileObject.CanPlace(i, j, tile.type, item.placeStyle, Main.LocalPlayer.direction, out objectData))
							{
								int styleWrap = 0;
								int alt = -1;
								int direction = 0;
								int random = -1;
								int xCoord = item.createTile;
								int yCoord;
								switch (xCoord)
								{
								case 15:
								case 497:
									direction = ((tile.frameX == 18) ? 1 : (-1));
									alt = ((direction != -1) ? 1 : 0);
									break;
								case 136:
								case 442:
								case 630:
								case 631:
									alt = tile.frameX / 18;
									break;
								case 132:
									alt = tile.frameX / 72;
									break;
								case 55:
								case 395:
								case 425:
								case 510:
								case 511:
								case 573:
									alt = tile.frameX / 36;
									break;
								case 79:
								case 90:
								case 212:
									direction = ((tile.frameX != 18) ? 1 : (-1));
									alt = ((direction != -1) ? 1 : 0);
									break;
								case 105:
									direction = ((tile.frameY >= 162) ? 1 : (-1));
									alt = ((direction != -1) ? 1 : 0);
									break;
								case 388:
									direction = ((tile.frameY >= 94) ? 1 : (-1));
									alt = ((direction != -1) ? 5 : 0);
									break;
								case 349:
									direction = ((tile.frameY <= 53) ? 1 : (-1));
									alt = ((direction != -1) ? 5 : 0);
									break;
								case 453:
									direction = ((!int.IsEvenInteger(tile.frameX / 18)) ? 1 : (-1));
									alt = ((direction != -1) ? 1 : 0);
									break;
								case 470:
								case 506:
									direction = ((!int.IsEvenInteger(tile.frameX / 36)) ? 1 : (-1));
									alt = ((direction == 1) ? 1 : 0);
									break;
								case 545:
									direction = ((tile.frameY >= 54) ? 1 : (-1));
									alt = ((direction != -1) ? 1 : 0);
									break;
								case 617:
									direction = ((tile.frameY >= 72) ? 1 : (-1));
									alt = ((direction != -1) ? 1 : 0);
									break;
								case 520:
									direction = ((tile.frameX >= 18) ? 1 : (-1));
									alt = ((direction != -1) ? 1 : 0);
									break;
								case 650:
									yCoord = (item.placeStyle = (objectData.style = tile.frameX / 18));
									break;
								case 649:
								case 652:
									yCoord = (item.placeStyle = (objectData.style = tile.frameX / 36));
									break;
								case 647:
								case 648:
								case 651:
									yCoord = (item.placeStyle = (objectData.style = tile.frameX / 54 + ((tile.frameY >= 36) ? data.StyleWrapLimit : 0)));
									break;
								case 471:
									direction = ((tile.frameX >= 54) ? 1 : (-1));
									alt = ((direction != -1) ? 1 : 0);
									break;
								}
								if (data.RandomStyleRange > 0)
								{
									random = (data.StyleHorizontal ? (tile.frameX / data.CoordinateFullWidth) : (tile.frameY / data.CoordinateFullHeight));
								}
								objectData.random = random;
								int fY;
								if (data.AlternatesCount != 0)
								{
									fY = data.AlternatesCount;
									data = TileObjectData.GetTileData(item.createTile, item.placeStyle, alt);
								}
								if (alt > -1)
								{
									objectData.alternate = alt;
								}
								int style = data.CalculatePlacementStyle(objectData.style, objectData.alternate, objectData.random);
								if (data.StyleWrapLimit > 0)
								{
									styleWrap = style / data.StyleWrapLimit * data.StyleLineSkip;
									style %= data.StyleWrapLimit;
								}
								int fX;
								if (data.StyleHorizontal)
								{
									fX = data.CoordinateFullWidth * style;
									fY = data.CoordinateFullHeight * styleWrap;
									if (styleWrap == 0)
									{
										fY = data.Origin.Y * (data.CoordinateWidth + data.CoordinatePadding);
									}
								}
								else
								{
									fX = data.CoordinateFullWidth * styleWrap;
									fY = data.CoordinateFullHeight * style;
									if (style == 0)
									{
										fY = data.Origin.Y * (data.CoordinateWidth + data.CoordinatePadding);
									}
								}
								int targetFX = fX + data.Origin.X * (data.CoordinateWidth + data.CoordinatePadding);
								int targetFY = fY;
								int altTargetFY = fY + data.Origin.Y * (data.CoordinateWidth + data.CoordinatePadding);
								if (item.createTile == 10)
								{
									tile.frameX = (short)(tile.frameX / 54 * 54);
								}
								if (item.createTile == 240)
								{
									if (item.placeStyle > data.StyleWrapLimit)
									{
										targetFY = altTargetFY;
									}
									else
									{
										altTargetFY = targetFY;
									}
								}
								if (item.createTile == 471)
								{
									altTargetFY = targetFY;
								}
								if (item.createTile == 388)
								{
									targetFY += 2;
								}
								if (item.createTile == 55 || item.createTile == 425 || item.createTile == 573 || item.createTile == 510 || item.createTile == 511)
								{
									targetFX = 36 * alt;
									if (alt == 3)
									{
										targetFX += 18;
									}
								}
								if (tile.frameX == targetFX && (tile.frameY == targetFY || tile.frameY == altTargetFY) && Main.tile[i, j].type != tile.type)
								{
									WorldEditBase.PlaceDirectObject(i, j, tile.type, out xCoord, out yCoord, out objectData, item.placeStyle, direction, alt, random);
									Mods.FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), 58);
									ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, item.netID);
									if (!TileID.Sets.BasicChest[item.createTile] && item.createTile != 88)
									{
										TileObjectData.CallPostPlacementPlayerHook(i, j, item.createTile, item.placeStyle, -1, alt, objectData);
										if (!Main.tileContainer[item.createTile] && item.createTile != 423)
										{
											NetMessage.SendObjectPlacement(-1, i, j, objectData.type, item.placeStyle, objectData.alternate, objectData.random, direction);
											if (item.createTile == 15)
											{
												NetMessage.SendTileSquare(-1, i - 1, j - 1, 3);
											}
											if (item.createTile == 79 || item.createTile == 90)
											{
												NetMessage.SendTileSquare(-1, i, j, 5);
											}
										}
									}
									else if (item.createTile == 88)
									{
										NetMessage.SendData(34, -1, -1, null, 2, i, j, item.placeStyle);
									}
									else
									{
										WorldGen.PlaceTile(i, j, item.createTile, mute: true, forced: false, Main.myPlayer, item.placeStyle);
										packetsSent++;
									}
									packetsSent++;
								}
								while (Mods.tilePacketMaxMode && (packetsSent > Mods.tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
								{
									packetsSent = 0;
									Thread.Sleep(500);
								}
								if (!Mods.tilePacketMaxMode && ((packetsSent > 0 && packetsSent % Mods.tilePacketRate == 0) || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
								{
									packetsSent = 0;
									Thread.Sleep(100);
								}
							}
						}
					}
					l--;
					Main.Map.UpdateLighting(i, j, byte.MaxValue);
					Main.refreshMap = true;
				}
			}
		}
		packetsSent += packets;
	}

	public static void PasteTiles_RevertGravityBlocks(List<LegacyTile> tilesToPaste, Point mouseToTile, int pasteWidth, int pasteHeight, out bool cancelThePaste)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		cancelThePaste = false;
		int tileIndex = 0;
		int tilesSent = 0;
		Vector2 fakePos = default(Vector2);
		for (int i = mouseToTile.X; i < mouseToTile.X + pasteWidth; i++)
		{
			for (int j = mouseToTile.Y; j < mouseToTile.Y + pasteHeight; j++)
			{
				while (Mods.pasteInTShock && Mods.didTShockDisableYou)
				{
					Main.NewText("Assumed TShock disabled you, pasting will resume after disable ends. Lower your paint packet rate to avoid this.", Mods.errorColor);
					i = mouseToTile.X;
					j = mouseToTile.Y;
					tileIndex = 0;
					while (Mods.didTShockDisableYou)
					{
						Thread.Sleep(20);
						if (Mods.cancelPaste)
						{
							goto end_IL_0053;
						}
					}
					tilesSent = 0;
					Thread.Sleep(250);
					continue;
					end_IL_0053:
					break;
				}
				if (Mods.cancelPaste)
				{
					Main.NewText("Caneled paste.", Mods.zazasFavoriteColor);
					Mods.cancelPaste = false;
					Mods.concurrentPastes--;
					NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
					NetMessage.SendData(13);
					Main.QueueMainThreadAction(delegate
					{
						WorldGen.RangeFrame(mouseToTile.X, mouseToTile.Y, mouseToTile.X + pasteWidth, mouseToTile.Y + pasteHeight, resetFrame: false, noBreak: true);
					});
					cancelThePaste = true;
					return;
				}
				if (Main.gameMenu)
				{
					Mods.concurrentPastes = 0;
					cancelThePaste = true;
					return;
				}
				if (tileIndex >= tilesToPaste.Count)
				{
					continue;
				}
				Tile tile = Main.tile[i, j];
				LegacyTile lTile = tilesToPaste[tileIndex];
				if (TileID.Sets.Falling[lTile.type])
				{
					Item toUse = GetItemDrop(i, j, lTile, includeLargeObjectDrops: true);
					if ((tile.active() && toUse.createTile != -1 && toUse.createTile != tile.type) || ShouldPlaceThisTile(tile))
					{
						((Vector2)(ref fakePos))._002Ector((float)((i + 2) * 16), (float)(j * 16));
						tilesSent = PasteTiles_ConvertTiles(i, j, tilesSent, tile, fakePos);
						if (tile.active())
						{
							if (!toUse.IsAir)
							{
								tile.type = (ushort)toUse.createTile;
								Mods.FakeNetMessage13(fakePos, 58);
								ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, toUse.netID);
								NetMessage.SendData(17, -1, -1, null, 1, i, j, (int)tile.type, toUse.placeStyle);
								tilesSent++;
							}
							if (tile.halfBrick())
							{
								NetMessage.SendData(17, -1, -1, null, 7, i, j, 1f);
							}
							else if (tile.slope() > 0)
							{
								NetMessage.SendData(17, -1, -1, null, 14, i, j, (int)tile.slope());
							}
						}
					}
				}
				while (Mods.tilePacketMaxMode && (tilesSent > Mods.tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
				{
					tilesSent = 0;
					Thread.Sleep(500);
				}
				if (!Mods.tilePacketMaxMode && ((tilesSent > 0 && tilesSent % Mods.tilePacketRate == 0) || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
				{
					tilesSent = 0;
					Thread.Sleep(100);
				}
				tileIndex++;
				Main.Map.UpdateLighting(i, j, byte.MaxValue);
				Main.refreshMap = true;
			}
		}
		stopwatch.Stop();
	}

	public static void PasteTiles_FixSlopes(List<LegacyTile> tilesToPaste, Point mouseToTile, int pasteWidth, int pasteHeight, out bool cancelThePaste)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		cancelThePaste = false;
		int tileIndex = 0;
		int tilesSent = 0;
		for (int i = mouseToTile.X; i < mouseToTile.X + pasteWidth; i++)
		{
			for (int j = mouseToTile.Y; j < mouseToTile.Y + pasteHeight; j++)
			{
				while (Mods.pasteInTShock && Mods.didTShockDisableYou)
				{
					Main.NewText("Assumed TShock disabled you, pasting will resume after disable ends. Lower your paint packet rate to avoid this.", Mods.errorColor);
					i = mouseToTile.X;
					j = mouseToTile.Y;
					tileIndex = 0;
					while (Mods.didTShockDisableYou)
					{
						Thread.Sleep(20);
						if (Mods.cancelPaste)
						{
							goto end_IL_0053;
						}
					}
					tilesSent = 0;
					Thread.Sleep(250);
					continue;
					end_IL_0053:
					break;
				}
				if (Mods.cancelPaste)
				{
					Main.NewText("Caneled paste.", Mods.zazasFavoriteColor);
					Mods.cancelPaste = false;
					Mods.concurrentPastes--;
					NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
					NetMessage.SendData(13);
					Main.QueueMainThreadAction(delegate
					{
						WorldGen.RangeFrame(mouseToTile.X, mouseToTile.Y, mouseToTile.X + pasteWidth, mouseToTile.Y + pasteHeight, resetFrame: false, noBreak: true);
					});
					cancelThePaste = true;
					return;
				}
				if (Main.gameMenu)
				{
					Mods.concurrentPastes = 0;
					cancelThePaste = true;
					return;
				}
				if (tileIndex >= tilesToPaste.Count)
				{
					continue;
				}
				Tile tile = Main.tile[i, j];
				LegacyTile lTile = tilesToPaste[tileIndex];
				if (tile.active() && (!Mods.blendAirTileMode || (Mods.blendAirTileMode && lTile.active())))
				{
					if (tile.IsHalfBlock != lTile.halfBrick())
					{
						tile.IsHalfBlock = lTile.halfBrick();
						NetMessage.SendData(17, -1, -1, null, 7, i, j, 1f);
					}
					else if (tile.slope() != lTile.slope())
					{
						tile.slope(lTile.slope());
						NetMessage.SendData(17, -1, -1, null, 14, i, j, (int)tile.slope());
					}
				}
				while (Mods.tilePacketMaxMode && (tilesSent > Mods.tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
				{
					tilesSent = 0;
					Thread.Sleep(500);
				}
				if (!Mods.tilePacketMaxMode && ((tilesSent > 0 && tilesSent % Mods.tilePacketRate == 0) || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
				{
					tilesSent = 0;
					Thread.Sleep(100);
				}
				tileIndex++;
				Main.Map.UpdateLighting(i, j, byte.MaxValue);
				Main.refreshMap = true;
			}
		}
		stopwatch.Stop();
	}

	public static void PasteTiles_DoPaint(List<LegacyTile> tilesToPaste, Point mouseToTile, int pasteWidth, int pasteHeight, out bool cancelThePaste)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		cancelThePaste = false;
		int tileIndex = 0;
		int paintSent = 0;
		for (int i = mouseToTile.X; i < mouseToTile.X + pasteWidth; i++)
		{
			for (int j = mouseToTile.Y; j < mouseToTile.Y + pasteHeight; j++)
			{
				while (Mods.pasteInTShock && Mods.didTShockDisableYou)
				{
					Main.NewText("Assumed TShock disabled you, pasting will resume after disable ends. Lower your paint packet rate to avoid this.", Mods.errorColor);
					i = mouseToTile.X;
					j = mouseToTile.Y;
					tileIndex = 0;
					while (Mods.didTShockDisableYou)
					{
						Thread.Sleep(20);
						if (Mods.cancelPaste)
						{
							goto end_IL_0053;
						}
					}
					paintSent = 0;
					Thread.Sleep(250);
					continue;
					end_IL_0053:
					break;
				}
				if (Mods.cancelPaste)
				{
					Main.NewText("Caneled paste.", Mods.zazasFavoriteColor);
					Mods.cancelPaste = false;
					Mods.concurrentPastes--;
					NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
					NetMessage.SendData(13);
					Main.QueueMainThreadAction(delegate
					{
						WorldGen.RangeFrame(mouseToTile.X, mouseToTile.Y, mouseToTile.X + pasteWidth, mouseToTile.Y + pasteHeight, resetFrame: false, noBreak: true);
					});
					cancelThePaste = true;
					return;
				}
				if (Main.gameMenu)
				{
					Mods.concurrentPastes = 0;
					cancelThePaste = true;
					return;
				}
				if (tileIndex < tilesToPaste.Count)
				{
					Tile tile = Main.tile[i, j];
					LegacyTile lTile = tilesToPaste[tileIndex];
					if (tile.active() && tile.TileColor != lTile.color() && (!Mods.blendAirTileMode || (Mods.blendAirTileMode && lTile.active())))
					{
						Mods.FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), 58);
						tile.TileColor = lTile.color();
						ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, (tile.TileColor == 0) ? 1100 : 1071);
						NetMessage.SendData(63, -1, -1, null, i, j, (int)tile.color());
						paintSent++;
					}
					if (tile.active() && tile.fullbrightBlock() != lTile.fullbrightBlock() && (!Mods.blendAirTileMode || (Mods.blendAirTileMode && lTile.active())))
					{
						Mods.FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), 58);
						tile.IsTileFullbright = lTile.fullbrightBlock();
						ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, (!tile.IsTileFullbright) ? 1100 : 1071);
						NetMessage.SendData(63, -1, -1, null, i, j, tile.IsTileFullbright ? 1 : 0, 1f);
						paintSent++;
					}
					if (tile.active() && tile.invisibleBlock() != lTile.invisibleBlock() && (!Mods.blendAirTileMode || (Mods.blendAirTileMode && lTile.active())))
					{
						Mods.FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), 58);
						tile.IsTileInvisible = lTile.invisibleBlock();
						ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, (!tile.IsTileInvisible) ? 1100 : 1071);
						NetMessage.SendData(63, -1, -1, null, i, j, tile.IsTileInvisible ? 2 : 0, 1f);
						paintSent++;
					}
					if (tile.wall > 0 && tile.wallColor() != lTile.wallColor() && (!Mods.blendAirTileMode || (Mods.blendAirTileMode && lTile.wall > 0)))
					{
						Mods.FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), 58);
						tile.WallColor = lTile.wallColor();
						ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, (tile.WallColor == 0) ? 1100 : 1072);
						NetMessage.SendData(64, -1, -1, null, i, j, (int)tile.WallColor);
						paintSent++;
					}
					if (tile.wall > 0 && tile.fullbrightWall() != lTile.fullbrightWall() && (!Mods.blendAirTileMode || (Mods.blendAirTileMode && lTile.wall > 0)))
					{
						Mods.FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), 58);
						tile.IsWallFullbright = lTile.fullbrightWall();
						ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, (!tile.IsWallFullbright) ? 1100 : 1072);
						NetMessage.SendData(64, -1, -1, null, i, j, tile.IsWallFullbright ? 1 : 0, 1f);
						paintSent++;
					}
					if (tile.wall > 0 && tile.invisibleWall() != lTile.invisibleWall() && (!Mods.blendAirTileMode || (Mods.blendAirTileMode && lTile.wall > 0)))
					{
						Mods.FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), 58);
						tile.IsWallInvisible = lTile.invisibleWall();
						ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, (!tile.IsWallInvisible) ? 1100 : 1072);
						NetMessage.SendData(64, -1, -1, null, i, j, tile.IsWallInvisible ? 2 : 0, 1f);
						paintSent++;
					}
					while (Mods.tilePacketMaxMode && (paintSent > Mods.paintPacketRate || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
					{
						paintSent = 0;
						Thread.Sleep(500);
					}
					if (!Mods.tilePacketMaxMode && ((paintSent > 0 && paintSent % Mods.paintPacketRate == 0) || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
					{
						paintSent = 0;
						Thread.Sleep(100);
					}
					tileIndex++;
					Main.Map.UpdateLighting(i, j, byte.MaxValue);
					Main.refreshMap = true;
				}
			}
		}
		stopwatch.Stop();
	}

	public static void PasteTiles_DoWiring(List<LegacyTile> tilesToPaste, Point mouseToTile, int pasteWidth, int pasteHeight, out bool cancelThePaste)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		cancelThePaste = false;
		int tileIndex = 0;
		int wiresSent = 0;
		for (int i = mouseToTile.X; i < mouseToTile.X + pasteWidth; i++)
		{
			for (int j = mouseToTile.Y; j < mouseToTile.Y + pasteHeight; j++)
			{
				while (Mods.pasteInTShock && Mods.didTShockDisableYou)
				{
					Main.NewText("Assumed TShock disabled you, pasting will resume after disable ends. Lower your paint packet rate to avoid this.", Mods.errorColor);
					i = mouseToTile.X;
					j = mouseToTile.Y;
					tileIndex = 0;
					while (Mods.didTShockDisableYou)
					{
						Thread.Sleep(20);
						if (Mods.cancelPaste)
						{
							goto end_IL_0053;
						}
					}
					wiresSent = 0;
					Thread.Sleep(250);
					continue;
					end_IL_0053:
					break;
				}
				if (Mods.cancelPaste)
				{
					Main.NewText("Caneled paste.", Mods.zazasFavoriteColor);
					Mods.cancelPaste = false;
					Mods.concurrentPastes--;
					NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
					NetMessage.SendData(13);
					Main.QueueMainThreadAction(delegate
					{
						WorldGen.RangeFrame(mouseToTile.X, mouseToTile.Y, mouseToTile.X + pasteWidth, mouseToTile.Y + pasteHeight, resetFrame: false, noBreak: true);
					});
					cancelThePaste = true;
					return;
				}
				if (Main.gameMenu)
				{
					Mods.concurrentPastes = 0;
					cancelThePaste = true;
					return;
				}
				if (tileIndex >= tilesToPaste.Count)
				{
					continue;
				}
				Tile tile = Main.tile[i, j];
				LegacyTile lTile = tilesToPaste[tileIndex];
				if (lTile.wire() != tile.RedWire && (!Mods.blendAirTileMode || (Mods.blendAirTileMode && lTile.wire() && !tile.RedWire)))
				{
					if (!lTile.wire())
					{
						NetMessage.SendData(109, -1, -1, null, i, j, i, j, 33);
					}
					else
					{
						Mods.FakeNetMessage13(new Vector2((float)(i * 16), (float)(j * 16)), 58);
						ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, 3625f);
						NetMessage.SendData(17, -1, -1, null, 5, i, j);
					}
					wiresSent++;
					tile.RedWire = lTile.wire();
				}
				if (lTile.wire2() != tile.BlueWire && (!Mods.blendAirTileMode || (Mods.blendAirTileMode && lTile.wire2() && !tile.BlueWire)))
				{
					if (!lTile.wire2())
					{
						NetMessage.SendData(109, -1, -1, null, i, j, i, j, 36);
					}
					else
					{
						Mods.FakeNetMessage13(new Vector2((float)(i * 16), (float)(j * 16)), 58);
						ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, 3625f);
						NetMessage.SendData(17, -1, -1, null, 10, i, j);
					}
					wiresSent++;
					tile.BlueWire = lTile.wire2();
				}
				if (lTile.wire3() != tile.GreenWire && (!Mods.blendAirTileMode || (Mods.blendAirTileMode && lTile.wire3() && !tile.GreenWire)))
				{
					if (!lTile.wire3())
					{
						NetMessage.SendData(109, -1, -1, null, i, j, i, j, 34);
					}
					else
					{
						Mods.FakeNetMessage13(new Vector2((float)(i * 16), (float)(j * 16)), 58);
						ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, 3625f);
						NetMessage.SendData(17, -1, -1, null, 12, i, j);
					}
					wiresSent++;
					tile.GreenWire = lTile.wire3();
				}
				if (lTile.wire4() != tile.YellowWire && (!Mods.blendAirTileMode || (Mods.blendAirTileMode && lTile.wire4() && !tile.YellowWire)))
				{
					if (!lTile.wire4())
					{
						NetMessage.SendData(109, -1, -1, null, i, j, i, j, 40);
					}
					else
					{
						Mods.FakeNetMessage13(new Vector2((float)(i * 16), (float)(j * 16)), 58);
						ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, 3625f);
						NetMessage.SendData(17, -1, -1, null, 16, i, j);
					}
					wiresSent++;
					tile.YellowWire = lTile.wire4();
				}
				if ((lTile.actuator() != tile.HasActuator && (!Mods.blendAirTileMode || (Mods.blendAirTileMode && lTile.actuator() && !tile.HasActuator))) || (lTile.active() && tile.HasTile && lTile.inActive() && (!Mods.blendAirTileMode || (Mods.blendAirTileMode && lTile.inActive() && !tile.IsActuated))))
				{
					bool delActuator = lTile.active() && lTile.inActive() && !lTile.actuator();
					if (!lTile.actuator() && !lTile.inActive() && tile.HasActuator)
					{
						NetMessage.SendData(109, -1, -1, null, i, j, i, j, 48);
					}
					else
					{
						Mods.FakeNetMessage13(new Vector2((float)(i * 16), (float)(j * 16)), 58);
						ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, 849f);
						NetMessage.SendData(17, -1, -1, null, 8, i, j);
						wiresSent++;
						tile.HasActuator = true;
						if (lTile.active() && tile.HasTile && lTile.inActive())
						{
							NetMessage.SendData(17, -1, -1, null, 19, i, j);
							tile.IsActuated = true;
						}
						if (delActuator)
						{
							NetMessage.SendData(109, -1, -1, null, i, j, i, j, 48);
							tile.HasActuator = false;
						}
					}
				}
				while (Mods.tilePacketMaxMode && (wiresSent > Mods.tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
				{
					wiresSent = 0;
					Thread.Sleep(500);
				}
				if (!Mods.tilePacketMaxMode && ((wiresSent > 0 && wiresSent % Mods.tilePacketRate == 0) || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
				{
					wiresSent = 0;
					Thread.Sleep(100);
				}
				tileIndex++;
				Main.Map.UpdateLighting(i, j, byte.MaxValue);
				Main.refreshMap = true;
			}
		}
		stopwatch.Stop();
	}

	public static void LazyTileFixup(LegacyTile tile)
	{
		switch (tile.type)
		{
		case 83:
		case 84:
			tile.type = 82;
			break;
		case 4:
			if (tile.frameX > 65)
			{
				tile.frameX -= 65;
			}
			break;
		case 33:
		case 42:
		case 49:
		case 92:
		case 174:
		case 372:
		case 419:
		case 420:
		case 423:
		case 646:
			while (tile.frameX > 17)
			{
				tile.frameX -= 18;
			}
			break;
		case 35:
		case 95:
		case 100:
		case 173:
		case 411:
		case 564:
		case 565:
			while (tile.frameX > 35)
			{
				tile.frameX -= 36;
			}
			break;
		case 93:
			if ((tile.frameX > 17 && tile.frameX < 36) || tile.frameX > 53)
			{
				tile.frameX -= 18;
			}
			break;
		case 132:
		case 139:
			if ((tile.frameX > 35 && tile.frameX < 72) || tile.frameX > 107)
			{
				tile.frameX -= 36;
			}
			break;
		case 506:
			if (tile.frameX > 71)
			{
				tile.frameX -= 72;
			}
			break;
		case 11:
			if (tile.frameX == 0 || tile.frameX == 54 || tile.frameX == 72 || tile.frameX == 126)
			{
				tile.type = 10;
				tile.frameX = (short)(tile.frameX / 72 * 54);
				break;
			}
			tile.type = 0;
			tile.active(active: false);
			tile.frameX = 0;
			tile.frameY = 0;
			tile.ClearBlockPaintAndCoating();
			break;
		case 244:
		case 405:
		case 452:
			if (tile.frameX > 53)
			{
				tile.frameX -= 54;
			}
			break;
		case 34:
			if ((tile.frameX > 53 && tile.frameX < 108) || tile.frameX > 161)
			{
				tile.frameX -= 54;
			}
			break;
		case 136:
		case 144:
			if (tile.frameY > 17)
			{
				tile.frameY -= 18;
			}
			break;
		case 215:
		case 440:
		case 455:
			while (tile.frameY > 31)
			{
				tile.frameY -= 32;
			}
			break;
		case 209:
		case 406:
		case 410:
		case 480:
		case 509:
		case 657:
		case 658:
			while (tile.frameY > 53)
			{
				tile.frameY -= 54;
			}
			break;
		case 207:
			while (tile.frameY > 71)
			{
				tile.frameY -= 72;
			}
			break;
		case 386:
			if ((tile.frameX < 36 && tile.frameY >= 18) || (tile.frameX >= 36 && tile.frameY == 0))
			{
				tile.type = 387;
				tile.frameY = 0;
				if (tile.frameX >= 36)
				{
					tile.frameX -= 36;
				}
			}
			else
			{
				tile.type = 0;
				tile.active(active: false);
				tile.frameX = 0;
				tile.frameY = 0;
				tile.ClearBlockPaintAndCoating();
			}
			break;
		case 389:
			tile.type = 388;
			break;
		case 557:
			tile.type = 546;
			break;
		case 510:
		case 511:
			tile.frameY = (short)((!int.IsEvenInteger(tile.frameY / 18)) ? 18 : 0);
			break;
		case 445:
			if (tile.frameX > 0)
			{
				tile.frameX = 0;
			}
			if (tile.frameY > 0)
			{
				tile.frameY = 0;
			}
			break;
		case 50:
			if (tile.frameX > 72)
			{
				tile.frameX = 0;
			}
			break;
		case 185:
			if (tile.frameY > 17)
			{
				tile.type = 649;
				tile.frameY -= 18;
			}
			else
			{
				tile.type = 649;
			}
			break;
		case 233:
			if (tile.frameY > 35)
			{
				tile.type = 652;
				tile.frameY -= 36;
			}
			else
			{
				tile.type = 233;
			}
			break;
		case 186:
			tile.type = 186;
			break;
		case 187:
			tile.type = 187;
			break;
		case 668:
			tile.type = 0;
			break;
		case 624:
		case 656:
			tile.type = 0;
			tile.frameX = -1;
			tile.frameY = -1;
			tile.ClearTile();
			tile.ClearBlockPaintAndCoating();
			break;
		case 5:
		case 583:
		case 584:
		case 585:
		case 586:
		case 587:
		case 588:
		case 589:
		case 596:
		case 616:
			if (tile.frameX >= 22 && tile.frameX < 66 && tile.frameY >= 132 && tile.frameY < 198)
			{
				tile.type = 0;
				tile.frameX = -1;
				tile.frameY = -1;
				tile.ClearTile();
				tile.ClearBlockPaintAndCoating();
				break;
			}
			switch (tile.type)
			{
			case 5:
				tile.type = 20;
				tile.frameX = 0;
				break;
			case 596:
				tile.type = 595;
				tile.frameX = 0;
				break;
			case 616:
				tile.type = 615;
				tile.frameX = 0;
				break;
			case 583:
			case 584:
			case 585:
			case 586:
			case 587:
			case 588:
			case 589:
				tile.type = 590;
				tile.frameX = (short)((tile.type - 583) * 54);
				break;
			}
			tile.frameY = 18;
			break;
		case 323:
			tile.type = 20;
			tile.frameX = 378;
			break;
		}
	}

	public static bool ShouldPlaceThisTile(Tile tile)
	{
		switch (tile.type)
		{
		case 477:
		case 492:
		case 512:
		case 513:
		case 514:
		case 515:
		case 516:
		case 517:
		case 535:
		case 537:
		case 540:
		case 626:
		case 628:
		case 661:
		case 662:
			return true;
		default:
			return false;
		}
	}

	public static LegacyTile FetchSingleTileData(BinaryReader r, int schematicVersion, out int compression)
	{
		LegacyTile tile = new LegacyTile();
		byte b = 0;
		byte b2 = 0;
		byte b3 = 0;
		byte b4 = r.ReadByte();
		bool flag = false;
		bool flag2 = false;
		if ((b4 & 1) == 1)
		{
			flag = true;
			b3 = r.ReadByte();
		}
		if (flag && (b3 & 1) == 1)
		{
			flag2 = true;
			b2 = r.ReadByte();
		}
		if (((schematicVersion >= 269) & flag2) && (b2 & 1) == 1)
		{
			b = r.ReadByte();
		}
		if ((b4 & 2) == 2)
		{
			tile.active(active: true);
			int num;
			if ((b4 & 0x20) != 32)
			{
				num = r.ReadByte();
			}
			else
			{
				byte b5 = r.ReadByte();
				num = r.ReadByte();
				num = (num << 8) | b5;
			}
			tile.type = (ushort)num;
			if (!Main.tileFrameImportant[num])
			{
				tile.frameX = -1;
				tile.frameY = -1;
			}
			else
			{
				tile.frameX = r.ReadInt16();
				tile.frameY = r.ReadInt16();
				if (tile.type == 144)
				{
					tile.frameY = -1;
				}
			}
			if ((b2 & 8) == 8)
			{
				tile.color(r.ReadByte());
			}
		}
		if ((b4 & 4) == 4)
		{
			tile.wall = r.ReadByte();
			if ((b2 & 0x10) == 16)
			{
				tile.wallColor(r.ReadByte());
			}
		}
		byte b6 = (byte)((b4 & 0x18) >> 3);
		if (b6 != 0)
		{
			tile.liquid = r.ReadByte();
			if (schematicVersion >= 269 && (b2 & 0x80) == 128)
			{
				tile.shimmer(shimmer: true);
			}
			else if (b6 > 1)
			{
				if (b6 == 2)
				{
					tile.lava(lava: true);
				}
				else
				{
					tile.honey(honey: true);
				}
			}
		}
		if (b3 > 1)
		{
			if ((b3 & 2) == 2)
			{
				tile.wire(wire: true);
			}
			if ((b3 & 4) == 4)
			{
				tile.wire2(wire2: true);
			}
			if ((b3 & 8) == 8)
			{
				tile.wire3(wire3: true);
			}
			b3 = (byte)((b3 & 0x70) >> 4);
			if (b3 != 0 && (Main.tileSolid[tile.type] || TileID.Sets.NonSolidSaveSlopes[tile.type]))
			{
				if (b3 == 1)
				{
					tile.halfBrick(halfBrick: true);
				}
				else
				{
					b3--;
					tile.slope(b3);
				}
			}
		}
		if (b2 > 1)
		{
			if ((b2 & 2) == 2)
			{
				tile.actuator(actuator: true);
			}
			if ((b2 & 4) == 4)
			{
				tile.inActive(inActive: true);
			}
			if ((b2 & 0x20) == 32)
			{
				tile.wire4(wire4: true);
			}
			if (schematicVersion >= 222 && (b2 & 0x40) == 64)
			{
				tile.wall = (ushort)((r.ReadByte() << 8) | tile.wall);
			}
		}
		if (schematicVersion >= 269 && b > 1)
		{
			if ((b & 2) == 2)
			{
				tile.invisibleBlock(invisibleBlock: true);
			}
			if ((b & 4) == 4)
			{
				tile.invisibleWall(invisibleWall: true);
			}
			if ((b & 8) == 8)
			{
				tile.fullbrightBlock(fullbrightBlock: true);
			}
			if ((b & 0x10) == 16)
			{
				tile.fullbrightWall(fullbrightWall: true);
			}
		}
		byte b7 = (byte)((b4 & 0xC0) >> 6);
		compression = 0;
		switch (b7)
		{
		default:
			compression = r.ReadInt16();
			break;
		case 1:
			compression = r.ReadByte();
			break;
		case 0:
			compression = 0;
			break;
		}
		return tile;
	}

	public static List<LegacyTile> LoadAllTileData(BinaryReader r, int maxX, int maxY, string schematicName, int schematicVersion = 279)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		List<LegacyTile> list = new List<LegacyTile>();
		int compression = 0;
		Mods.schematicSizeX = maxX;
		Mods.schematicSizeY = maxY;
		for (int i = 0; i < maxX; i++)
		{
			for (int j = 0; j < maxY; j++)
			{
				try
				{
					LegacyTile item = FetchSingleTileData(r, schematicVersion, out compression);
					list.Add(item);
					while (compression > 0)
					{
						j++;
						if (j < maxY)
						{
							list.Add(item);
							compression--;
							continue;
						}
						break;
					}
				}
				catch (Exception ex)
				{
					Main.NewText($"Something broke in LoadAllTileData!\n{ex}", Mods.errorColor);
					Mods.LogException(ex);
					return list;
				}
			}
		}
		return list;
	}

	public static byte[] StoreSingleTileData(LegacyTile tile, BinaryWriter bw, out int dataIndex, out int headerIndex)
	{
		byte[] array = new byte[16];
		dataIndex = 4;
		byte b = 0;
		byte b2 = 0;
		byte b3 = 0;
		byte b4 = 0;
		if (tile.active() && tile.type <= TileID.Count && tile.type != 127)
		{
			b4 |= 2;
			array[dataIndex++] = (byte)tile.type;
			if (tile.type > 255)
			{
				array[dataIndex++] = (byte)(tile.type >> 8);
				b4 |= 0x20;
			}
			if (Main.tileFrameImportant[tile.type])
			{
				array[dataIndex++] = (byte)(tile.frameX & 0xFF);
				array[dataIndex++] = (byte)((tile.frameX & 0xFF00) >> 8);
				array[dataIndex++] = (byte)(tile.frameY & 0xFF);
				array[dataIndex++] = (byte)((tile.frameY & 0xFF00) >> 8);
			}
			if (tile.color() != 0 && tile.color() != 31)
			{
				byte b5 = tile.color();
				b2 |= 8;
				array[dataIndex++] = b5;
			}
		}
		if (tile.wall != 0 && tile.wall <= WallID.Count)
		{
			b4 |= 4;
			array[dataIndex++] = (byte)tile.wall;
			if (tile.wallColor() != 0 && tile.wallColor() != 31)
			{
				byte b6 = tile.wallColor();
				b2 |= 0x10;
				array[dataIndex++] = b6;
			}
		}
		if (tile.liquid != 0)
		{
			if (tile.liquidType() != 3)
			{
				b4 = ((tile.liquidType() == 1) ? ((byte)(b4 | 0x10)) : ((tile.liquidType() != 2) ? ((byte)(b4 | 8)) : ((byte)(b4 | 0x18))));
			}
			else
			{
				b2 |= 0x80;
				b4 |= 8;
			}
			array[dataIndex++] = tile.liquid;
		}
		if (tile.wire())
		{
			b3 |= 2;
		}
		if (tile.wire2())
		{
			b3 |= 4;
		}
		if (tile.wire3())
		{
			b3 |= 8;
		}
		int num = (tile.halfBrick() ? 16 : ((tile.slope() != 0) ? (tile.slope() + 1 << 4) : 0));
		b3 |= (byte)num;
		if (tile.actuator())
		{
			b2 |= 2;
		}
		if (tile.inActive())
		{
			b2 |= 4;
		}
		if (tile.wire4())
		{
			b2 |= 0x20;
		}
		if (tile.wall > 255)
		{
			b2 |= 0x40;
			array[dataIndex++] = (byte)(tile.wall >> 8);
		}
		if (tile.invisibleBlock())
		{
			b |= 2;
		}
		if (tile.invisibleWall())
		{
			b |= 4;
		}
		if (tile.fullbrightBlock() || tile.color() == 31)
		{
			b |= 8;
		}
		if (tile.fullbrightWall() || tile.wallColor() == 31)
		{
			b |= 0x10;
		}
		headerIndex = 3;
		if (b != 0)
		{
			b2 |= 1;
			array[headerIndex--] = b;
		}
		if (b2 != 0)
		{
			b3 |= 1;
			array[headerIndex--] = b2;
		}
		if (b3 != 0)
		{
			b4 |= 1;
			array[headerIndex--] = b3;
		}
		array[headerIndex] = b4;
		return array;
	}

	public static void SaveAllTileData(BinaryWriter bw, int maxX, int maxY)
	{
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		try
		{
			for (int i = 0; i < maxX; i++)
			{
				int num2;
				for (num2 = 0; num2 < maxY; num2++)
				{
					LegacyTile tile = Mods.copiedTiles[num];
					byte[] array = StoreSingleTileData(tile, bw, out var dataIndex, out var headerIndex);
					byte b = array[headerIndex];
					short num3 = 0;
					int num4 = num + 1;
					int num5 = maxY - num2 - 1;
					while (num5 > 0 && tile.isTheSameAs(Mods.copiedTiles[num4]) && TileID.Sets.AllowsSaveCompressionBatching[tile.type])
					{
						num3++;
						num5--;
						num4++;
						num++;
					}
					num2 += num3;
					if (num3 > 0)
					{
						array[dataIndex++] = (byte)(num3 & 0xFF);
						if (num3 <= 255)
						{
							b |= 0x40;
						}
						else
						{
							b |= 0x80;
							array[dataIndex++] = (byte)((num3 & 0xFF00) >> 8);
						}
					}
					array[headerIndex] = b;
					bw.Write(array, headerIndex, dataIndex - headerIndex);
					num++;
				}
			}
		}
		catch (Exception ex)
		{
			Main.NewText($"Something broke in SaveAllTileData!\n{ex}", Mods.errorColor);
			Mods.LogException(ex);
		}
	}

	public static void DrawSchematicPreview(SpriteBatch spriteBatch)
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!Main.gameMenu && Mods.pasteMode && !Mods.openingTEDITSchematic && Main.hasFocus)
			{
				int schematicSizeX = Mods.schematicSizeX;
				int schematicSizeY = Mods.schematicSizeY;
				if (Main.mapFullscreen)
				{
					spriteBatch.End();
					spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer, (Effect)null, Main.GameViewMatrix.EffectMatrix);
					DrawPreviewInMap(spriteBatch, schematicSizeX, schematicSizeY);
				}
				else
				{
					DrawPreviewInWorld(spriteBatch, schematicSizeX, schematicSizeY);
				}
				spriteBatch.End();
				spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, (Effect)null, Main.Transform);
			}
		}
		catch (ArgumentOutOfRangeException)
		{
		}
		catch (Exception ex2)
		{
			Mods.pasteMode = false;
			Main.NewText($"Drawing Schematic Preview error!\n{ex2}", Mods.errorColor);
			Mods.LogException(ex2);
		}
	}

	public static void RegisterRenderTargets()
	{
		Main.ContentThatNeedsRenderTargets.Add(WorldSchematicPreview);
		Main.ContentThatNeedsRenderTargets.Add(MapSchematicPreview);
	}

	public static void RequestCacheReset()
	{
		worldCacheNeedsReset = true;
		mapCacheNeedsReset = true;
	}

	public static void ResetPreview()
	{
		RequestCacheReset();
		requestedWorldPreview = false;
		requestedMapPreview = false;
		previewRunTwo = false;
		schemX = 0;
		schemY = 0;
		schemMapX = 0;
		schemMapY = 0;
		Main.RunOnMainThread(ResetRenderTargets);
	}

	private static void ResetRenderTargets()
	{
		for (int i = 0; i < schemSizeX; i++)
		{
			for (int j = 0; j < schemSizeY; j++)
			{
				if (worldSchemTargets[i, j] != null && !((GraphicsResource)worldSchemTargets[i, j]).IsDisposed)
				{
					((GraphicsResource)worldSchemTargets[i, j]).Dispose();
				}
			}
		}
		for (int k = 0; k < schemMapSizeX; k++)
		{
			for (int l = 0; l < schemMapSizeY; l++)
			{
				if (mapSchemTargets[k, l] != null && !((GraphicsResource)mapSchemTargets[k, l]).IsDisposed)
				{
					((GraphicsResource)mapSchemTargets[k, l]).Dispose();
				}
			}
		}
		schemSizeX = Mods.schematicSizeX / 200 + 1;
		schemSizeY = Mods.schematicSizeY / 150 + 1;
		schemMapSizeX = Mods.schematicSizeX / 200 + 1;
		schemMapSizeY = Mods.schematicSizeY / 150 + 1;
		if (Mods.schematicSizeX % 200 == 0)
		{
			schemSizeX--;
		}
		if (Mods.schematicSizeY % 150 == 0)
		{
			schemSizeY--;
		}
		if (Mods.schematicSizeX % 200 == 0)
		{
			schemMapSizeX--;
		}
		if (Mods.schematicSizeY % 150 == 0)
		{
			schemMapSizeY--;
		}
		if (schemSizeX > 10)
		{
			schemSizeX = 10;
		}
		if (schemSizeY > 12)
		{
			schemSizeY = 12;
		}
		if (schemMapSizeX > 50)
		{
			schemMapSizeX = 50;
		}
		if (schemMapSizeY > 20)
		{
			schemMapSizeY = 20;
		}
		worldSchemTargets = new RenderTarget2D[schemSizeX, schemSizeY];
		mapSchemTargets = new RenderTarget2D[schemMapSizeX, schemMapSizeY];
	}

	private static void RequestWorldPreview(ref bool cacheNeedsReset)
	{
		try
		{
			for (int i = 0; i < schemSizeX; i++)
			{
				for (int j = 0; j < schemSizeY; j++)
				{
					if (worldSchemTargets[i, j] == null)
					{
						cacheNeedsReset = true;
						requestedWorldPreview = false;
						break;
					}
				}
			}
			if (WorldSchematicPreview.IsReady)
			{
				cacheNeedsReset = false;
				requestedWorldPreview = false;
				if (!previewRunTwo)
				{
					cacheNeedsReset = true;
					previewRunTwo = true;
					return;
				}
				previewRunTwo = false;
				worldSchemTargets[schemX, schemY] = WorldSchematicPreview.GetTarget();
				if (schemX == schemSizeX || schemY == schemSizeY)
				{
					return;
				}
				schemX++;
				if (schemX >= schemSizeX)
				{
					schemY++;
					if (schemY >= schemSizeY && schemX < schemSizeX)
					{
						schemY = 0;
					}
					if (schemY < schemSizeY)
					{
						schemX = 0;
					}
				}
				if (schemX < schemSizeX && schemY < schemSizeY)
				{
					cacheNeedsReset = true;
					requestedWorldPreview = false;
					previewRunTwo = false;
				}
			}
			else if (!requestedWorldPreview)
			{
				WorldSchematicPreview.Reset();
				WorldSchematicPreview.Request();
				requestedWorldPreview = true;
			}
		}
		catch
		{
		}
	}

	private static void RequestMapPreview(ref bool cacheNeedsReset)
	{
		for (int i = 0; i < schemMapSizeX; i++)
		{
			for (int j = 0; j < schemMapSizeY; j++)
			{
				if (mapSchemTargets[i, j] == null)
				{
					cacheNeedsReset = true;
					requestedMapPreview = false;
					break;
				}
			}
		}
		if (MapSchematicPreview.IsReady)
		{
			cacheNeedsReset = false;
			requestedMapPreview = false;
			mapSchemTargets[schemMapX, schemMapY] = MapSchematicPreview.GetTarget();
			if (schemMapX == schemMapSizeX || schemMapY == schemMapSizeY)
			{
				return;
			}
			schemMapX++;
			if (schemMapX >= schemMapSizeX)
			{
				schemMapY++;
				if (schemMapY >= schemMapSizeY && schemMapX < schemMapSizeX)
				{
					schemMapY = 0;
				}
				if (schemMapY < schemMapSizeY)
				{
					schemMapX = 0;
				}
			}
			if (schemMapX < schemMapSizeX && schemMapY < schemMapSizeY)
			{
				cacheNeedsReset = true;
			}
		}
		else if (!requestedMapPreview)
		{
			MapSchematicPreview.Reset();
			MapSchematicPreview.Request();
			requestedMapPreview = true;
		}
	}

	private static void DrawPreviewInWorld(SpriteBatch spriteBatch, int schematicWidth, int schematicHeight)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		if (worldCacheNeedsReset)
		{
			RequestWorldPreview(ref worldCacheNeedsReset);
		}
		if (Mods.inWorldMapPreview && mapCacheNeedsReset)
		{
			RequestMapPreview(ref mapCacheNeedsReset);
		}
		Vector2 cameraPosition = Main.screenPosition;
		bool isPlayerUpsideDown = Main.player[Main.myPlayer].gravDir == -1f;
		float verticalFlipOffset = (isPlayerUpsideDown ? ((float)Main.screenHeight) : 0f);
		Main.instance.MouseText(Mods.PasteModeString(), 0, 0);
		if (Main.mouseRight && Main.mouseRightRelease)
		{
			Mods.motionlessPaste = !Mods.motionlessPaste;
			Main.NewText(Mods.motionlessPaste ? $"Locked paste position to {Mods.pasteImagePreviewPos.X}, {Mods.pasteImagePreviewPos.Y}." : "Paste position un-locked.", Mods.infoColor);
		}
		if (!Mods.motionlessPaste)
		{
			Mods.pasteImagePreviewPos = Main.MouseWorld.ToTileCoordinates();
		}
		Vector2 startPos = new Vector2((float)Mods.pasteImagePreviewPos.X * 16f, (float)Mods.pasteImagePreviewPos.Y * 16f) - cameraPosition;
		if (!Mods.inWorldMapPreview && worldSchemTargets[0, 0] != null && !((GraphicsResource)worldSchemTargets[0, 0]).IsDisposed)
		{
			Vector2 screenPos = default(Vector2);
			Vector2 dispPos = default(Vector2);
			for (int i = 0; i < schemSizeX; i++)
			{
				for (int j = 0; j < schemSizeY; j++)
				{
					((Vector2)(ref screenPos))._002Ector((float)(Mods.pasteImagePreviewPos.X * 16 + i * 3200), (float)(Mods.pasteImagePreviewPos.Y * 16 + j * 2400));
					((Vector2)(ref dispPos))._002Ector(startPos.X + (float)(i * 3200), startPos.Y + (float)(j * 2400));
					if (Mods.WithinScreenBounds(new Rectangle((int)screenPos.X, (int)screenPos.Y, 3200, 2400), -16))
					{
						if (worldSchemTargets[i, j] != null && !((GraphicsResource)worldSchemTargets[i, j]).IsDisposed)
						{
							spriteBatch.Draw((Texture2D)(object)worldSchemTargets[i, j], dispPos, (Rectangle?)new Rectangle(0, 0, ((Texture2D)worldSchemTargets[i, j]).Width, ((Texture2D)worldSchemTargets[i, j]).Height), previewTint, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
							continue;
						}
						worldCacheNeedsReset = true;
						requestedWorldPreview = false;
					}
				}
			}
		}
		else
		{
			Vector2 screenPos2 = default(Vector2);
			Vector2 dispPos2 = default(Vector2);
			for (int k = 0; k < schemMapSizeX; k++)
			{
				for (int l = 0; l < schemMapSizeY; l++)
				{
					((Vector2)(ref screenPos2))._002Ector((float)(Mods.pasteImagePreviewPos.X * 16 + k * 3200), (float)(Mods.pasteImagePreviewPos.Y * 16 + l * 2400));
					((Vector2)(ref dispPos2))._002Ector(startPos.X + (float)(k * 3200), startPos.Y + (float)(l * 2400));
					if (Mods.WithinScreenBounds(new Rectangle((int)screenPos2.X, (int)screenPos2.Y, 3200, 2400), -16))
					{
						if (mapSchemTargets[k, l] != null && !((GraphicsResource)mapSchemTargets[k, l]).IsDisposed)
						{
							spriteBatch.Draw((Texture2D)(object)mapSchemTargets[k, l], dispPos2, (Rectangle?)new Rectangle(0, 0, ((Texture2D)mapSchemTargets[k, l]).Width, ((Texture2D)mapSchemTargets[k, l]).Height), previewTint, 0f, Vector2.Zero, 16f, (SpriteEffects)0, 0f);
							continue;
						}
						mapCacheNeedsReset = true;
						requestedMapPreview = false;
					}
				}
			}
		}
		Rectangle value = Main.ReverseGravitySupport(new Rectangle(Mods.pasteImagePreviewPos.X * 16, Mods.pasteImagePreviewPos.Y * 16, schematicWidth * 16, schematicHeight * 16));
		Rectangle value2 = Main.ReverseGravitySupport(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth + 1, Main.screenHeight + 1));
		Rectangle result = default(Rectangle);
		Rectangle.Intersect(ref value2, ref value, ref result);
		if (result.Width != 0 && result.Height != 0)
		{
			((Rectangle)(ref result)).Offset(-value2.X, -value2.Y);
			if (isPlayerUpsideDown)
			{
				result.Y = (int)verticalFlipOffset - result.Y;
				result.Y += 1 - result.Height;
			}
			for (int m = 0; m < 2; m++)
			{
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)(float)result.X, (int)(float)(result.Y + ((m == 1) ? result.Height : (-2))), (int)(float)result.Width, 2), Mods.pasteBorderColor);
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)((float)result.X + (float)((m == 1) ? result.Width : (-2))), (int)(float)result.Y, 2, (int)(float)result.Height), Mods.pasteBorderColor);
			}
		}
	}

	private static void DrawPreviewInMap(SpriteBatch spriteBatch, int schematicSizeX, int schematicSizeY)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		if (mapCacheNeedsReset)
		{
			RequestMapPreview(ref mapCacheNeedsReset);
		}
		if (Mods.worldPreviewOnMap && worldCacheNeedsReset)
		{
			RequestWorldPreview(ref worldCacheNeedsReset);
		}
		Main.instance.MouseText(Mods.PasteModeString(), 0, 0);
		Point res = default(Point);
		((Point)(ref res))._002Ector(-1, -1);
		if (!Mods.motionlessPaste)
		{
			Mods.GetMapCoordinatesModV2(Main.mouseX, Main.mouseY, 0, out res);
		}
		else
		{
			res = Mods.pasteImagePreviewPos;
		}
		if (PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Activator") && PlayerInput.Triggers.JustPressed.MouseRight)
		{
			Mods.pasteImagePreviewPos = res;
			Mods.motionlessPaste = !Mods.motionlessPaste;
			Main.NewText(Mods.motionlessPaste ? $"Locked paste position to {res.X}, {res.Y}." : "Paste position un-locked.", Mods.infoColor);
		}
		if (res.X <= -1 || res.Y <= -1)
		{
			return;
		}
		Color color = default(Color);
		((Color)(ref color))._002Ector(210, 210, 210, 100);
		Mods.GetMapCoordinatesModV2(res.X, res.Y, 1, out var res2);
		if (!Mods.worldPreviewOnMap)
		{
			Vector2 dispPos = default(Vector2);
			for (int i = 0; i < schemMapSizeX; i++)
			{
				for (int j = 0; j < schemMapSizeY; j++)
				{
					((Vector2)(ref dispPos))._002Ector((float)res2.X + (float)(i * 3200) * Main.mapFullscreenScale / 16f, (float)res2.Y + (float)(j * 2400) * Main.mapFullscreenScale / 16f);
					if (Mods.WithinScreenBoundsMap(new Rectangle((int)dispPos.X, (int)dispPos.Y, (int)(3200f * Main.mapFullscreenScale / 16f), (int)(2400f * Main.mapFullscreenScale / 16f)), -1))
					{
						if (mapSchemTargets[i, j] != null && !((GraphicsResource)mapSchemTargets[i, j]).IsDisposed)
						{
							spriteBatch.Draw((Texture2D)(object)mapSchemTargets[i, j], dispPos, (Rectangle?)new Rectangle(0, 0, ((Texture2D)mapSchemTargets[i, j]).Width, ((Texture2D)mapSchemTargets[i, j]).Height), previewTint, 0f, Vector2.Zero, Main.mapFullscreenScale, (SpriteEffects)0, 0f);
							continue;
						}
						mapCacheNeedsReset = true;
						requestedMapPreview = false;
					}
				}
			}
		}
		else
		{
			Vector2 dispPos2 = default(Vector2);
			for (int k = 0; k < schemSizeX; k++)
			{
				for (int l = 0; l < schemSizeY; l++)
				{
					((Vector2)(ref dispPos2))._002Ector((float)res2.X + (float)(k * 3200) * Main.mapFullscreenScale / 16f, (float)res2.Y + (float)(l * 2400) * Main.mapFullscreenScale / 16f);
					if (Mods.WithinScreenBoundsMap(new Rectangle((int)dispPos2.X, (int)dispPos2.Y, (int)(3200f * Main.mapFullscreenScale / 16f), (int)(2400f * Main.mapFullscreenScale / 16f)), -4))
					{
						if (worldSchemTargets[k, l] != null && !((GraphicsResource)worldSchemTargets[k, l]).IsDisposed)
						{
							spriteBatch.Draw((Texture2D)(object)worldSchemTargets[k, l], dispPos2, (Rectangle?)new Rectangle(0, 0, ((Texture2D)worldSchemTargets[k, l]).Width, ((Texture2D)worldSchemTargets[k, l]).Height), color, 0f, Vector2.Zero, Main.mapFullscreenScale / 16f, (SpriteEffects)0, 0f);
							continue;
						}
						worldCacheNeedsReset = true;
						requestedWorldPreview = false;
					}
				}
			}
		}
		CaptureInterface.GetMapCoords(res.X, res.Y, 1, out var result2);
		CaptureInterface.GetMapCoords(res.X + schematicSizeX, res.Y + schematicSizeY, 1, out var result3);
		Rectangle value3 = default(Rectangle);
		((Rectangle)(ref value3))._002Ector(result2.X, result2.Y, result3.X - result2.X, result3.Y - result2.Y);
		Rectangle value4 = default(Rectangle);
		((Rectangle)(ref value4))._002Ector(0, 0, Main.screenWidth + 1, Main.screenHeight + 1);
		Rectangle result4 = default(Rectangle);
		Rectangle.Intersect(ref value4, ref value3, ref result4);
		if (result4.Width != 0 && result4.Height != 0)
		{
			((Rectangle)(ref result4)).Offset(-value4.X, -value4.Y);
			for (int m = 0; m < 2; m++)
			{
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(result4.X, result4.Y + ((m == 1) ? result4.Height : (-2)), result4.Width, 2), Mods.pasteBorderColor);
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(result4.X + ((m == 1) ? result4.Width : (-2)), result4.Y, 2, result4.Height), Mods.pasteBorderColor);
			}
		}
	}

	public static Item GetItemDrop(int x, int y, LegacyTile tileCache, bool includeLargeObjectDrops = false)
	{
		int dropItem = 0;
		int num = 0;
		if (includeLargeObjectDrops)
		{
			switch (tileCache.type)
			{
			case 21:
			case 467:
			{
				int chestStyle = tileCache.frameX / 36;
				if ((tileCache.type == 467 || chestStyle <= Chest.chestItemSpawn.Length) && (tileCache.type != 467 || chestStyle <= Chest.chestItemSpawn2.Length))
				{
					dropItem = ((tileCache.type != 467) ? Chest.chestItemSpawn[chestStyle] : Chest.chestItemSpawn2[chestStyle]);
				}
				break;
			}
			case 88:
				num = tileCache.frameX / 54;
				dropItem = WorldGen.GetDresserItemDrop(num);
				break;
			case 215:
				num = tileCache.frameX / 54;
				dropItem = WorldGen.GetCampfireItemDrop(num);
				break;
			}
		}
		switch (tileCache.type)
		{
		case 659:
			dropItem = 5349;
			break;
		case 667:
			dropItem = 5398;
			break;
		case 179:
		case 512:
			dropItem = 4349;
			break;
		case 180:
		case 513:
			dropItem = 4350;
			break;
		case 181:
		case 514:
			dropItem = 4351;
			break;
		case 182:
		case 515:
			dropItem = 4352;
			break;
		case 183:
		case 516:
			dropItem = 4353;
			break;
		case 381:
		case 517:
			dropItem = 4354;
			break;
		case 534:
		case 535:
			dropItem = 4377;
			break;
		case 536:
		case 537:
			dropItem = 4378;
			break;
		case 539:
		case 540:
			dropItem = 4389;
			break;
		case 625:
		case 626:
			dropItem = 5127;
			break;
		case 627:
		case 628:
			dropItem = 5128;
			break;
		case 0:
			dropItem = 2;
			break;
		case 2:
			dropItem = 62;
			break;
		case 109:
			dropItem = 369;
			break;
		case 199:
			dropItem = 2171;
			break;
		case 633:
			dropItem = 5214;
			break;
		case 426:
			dropItem = 3621;
			break;
		case 430:
			dropItem = 3633;
			break;
		case 431:
			dropItem = 3634;
			break;
		case 432:
			dropItem = 3635;
			break;
		case 433:
			dropItem = 3636;
			break;
		case 434:
			dropItem = 3637;
			break;
		case 427:
			dropItem = 3622;
			break;
		case 435:
			dropItem = 3638;
			break;
		case 436:
			dropItem = 3639;
			break;
		case 437:
			dropItem = 3640;
			break;
		case 438:
			dropItem = 3641;
			break;
		case 439:
			dropItem = 3642;
			break;
		case 446:
			dropItem = 3736;
			break;
		case 447:
			dropItem = 3737;
			break;
		case 448:
			dropItem = 3738;
			break;
		case 449:
			dropItem = 3739;
			break;
		case 450:
			dropItem = 3740;
			break;
		case 451:
			dropItem = 3741;
			break;
		case 368:
			dropItem = 3086;
			break;
		case 369:
			dropItem = 3087;
			break;
		case 367:
			dropItem = 3081;
			break;
		case 379:
			dropItem = 3214;
			break;
		case 353:
			dropItem = 2996;
			break;
		case 365:
			dropItem = 3077;
			break;
		case 366:
			dropItem = 3078;
			break;
		case 357:
			dropItem = 3066;
			break;
		case 1:
			dropItem = 3;
			break;
		case 442:
			dropItem = 3707;
			break;
		case 383:
			dropItem = 3360;
			break;
		case 315:
			dropItem = 2435;
			break;
		case 641:
			dropItem = 5306;
			break;
		case 330:
			dropItem = 71;
			break;
		case 331:
			dropItem = 72;
			break;
		case 332:
			dropItem = 73;
			break;
		case 333:
			dropItem = 74;
			break;
		case 408:
			dropItem = 3460;
			break;
		case 409:
			dropItem = 3461;
			break;
		case 669:
			dropItem = 5401;
			break;
		case 670:
			dropItem = 5402;
			break;
		case 671:
			dropItem = 5403;
			break;
		case 672:
			dropItem = 5404;
			break;
		case 673:
			dropItem = 5405;
			break;
		case 674:
			dropItem = 5406;
			break;
		case 675:
			dropItem = 5407;
			break;
		case 676:
			dropItem = 5408;
			break;
		case 677:
			dropItem = 5417;
			break;
		case 678:
			dropItem = 5419;
			break;
		case 679:
			dropItem = 5421;
			break;
		case 680:
			dropItem = 5423;
			break;
		case 681:
			dropItem = 5425;
			break;
		case 682:
			dropItem = 5427;
			break;
		case 683:
			dropItem = 5433;
			break;
		case 684:
			dropItem = 5435;
			break;
		case 685:
			dropItem = 5429;
			break;
		case 686:
			dropItem = 5431;
			break;
		case 687:
			dropItem = 5439;
			break;
		case 688:
			dropItem = 5440;
			break;
		case 689:
			dropItem = 5441;
			break;
		case 690:
			dropItem = 5442;
			break;
		case 691:
			dropItem = 5443;
			break;
		case 692:
			dropItem = 5444;
			break;
		case 666:
			dropItem = 5395;
			break;
		case 415:
			dropItem = 3573;
			break;
		case 416:
			dropItem = 3574;
			break;
		case 417:
			dropItem = 3575;
			break;
		case 418:
			dropItem = 3576;
			break;
		case 421:
			dropItem = 3609;
			break;
		case 422:
			dropItem = 3610;
			break;
		case 498:
			dropItem = 4139;
			break;
		case 424:
			dropItem = 3616;
			break;
		case 445:
			dropItem = 3725;
			break;
		case 429:
			dropItem = 3629;
			break;
		case 272:
			dropItem = 1344;
			break;
		case 273:
			dropItem = 2119;
			break;
		case 274:
			dropItem = 2120;
			break;
		case 618:
			dropItem = 4962;
			break;
		case 460:
			dropItem = 3756;
			break;
		case 541:
			dropItem = 4392;
			break;
		case 630:
			dropItem = 5137;
			break;
		case 631:
			dropItem = 5138;
			break;
		case 472:
			dropItem = 3951;
			break;
		case 473:
			dropItem = 3953;
			break;
		case 474:
			dropItem = 3955;
			break;
		case 478:
			dropItem = 4050;
			break;
		case 479:
			dropItem = 4051;
			break;
		case 496:
			dropItem = 4091;
			break;
		case 495:
			dropItem = 4090;
			break;
		case 346:
			dropItem = 2792;
			break;
		case 347:
			dropItem = 2793;
			break;
		case 348:
			dropItem = 2794;
			break;
		case 350:
			dropItem = 2860;
			break;
		case 336:
			dropItem = 2701;
			break;
		case 340:
			dropItem = 2751;
			break;
		case 341:
			dropItem = 2752;
			break;
		case 342:
			dropItem = 2753;
			break;
		case 343:
			dropItem = 2754;
			break;
		case 344:
			dropItem = 2755;
			break;
		case 351:
			dropItem = 2868;
			break;
		case 500:
			dropItem = 4229;
			break;
		case 501:
			dropItem = 4230;
			break;
		case 502:
			dropItem = 4231;
			break;
		case 503:
			dropItem = 4232;
			break;
		case 546:
		case 557:
			dropItem = 4422;
			break;
		case 561:
			dropItem = 4554;
			break;
		case 574:
			dropItem = 4717;
			break;
		case 575:
			dropItem = 4718;
			break;
		case 576:
			dropItem = 4719;
			break;
		case 577:
			dropItem = 4720;
			break;
		case 578:
			dropItem = 4721;
			break;
		case 562:
			dropItem = 4564;
			break;
		case 563:
			dropItem = 4547;
			break;
		case 251:
			dropItem = 1725;
			break;
		case 252:
			dropItem = 1727;
			break;
		case 253:
			dropItem = 1729;
			break;
		case 325:
			dropItem = 2692;
			break;
		case 370:
			dropItem = 3100;
			break;
		case 396:
			dropItem = 3271;
			break;
		case 400:
			dropItem = 3276;
			break;
		case 401:
			dropItem = 3277;
			break;
		case 403:
			dropItem = 3339;
			break;
		case 397:
			dropItem = 3272;
			break;
		case 398:
			dropItem = 3274;
			break;
		case 399:
			dropItem = 3275;
			break;
		case 402:
			dropItem = 3338;
			break;
		case 404:
			dropItem = 3347;
			break;
		case 407:
			dropItem = 3380;
			break;
		case 579:
			dropItem = 4761;
			break;
		case 593:
			dropItem = 4868;
			break;
		case 170:
			dropItem = 1872;
			break;
		case 284:
			dropItem = 2173;
			break;
		case 214:
			dropItem = 85;
			break;
		case 213:
			dropItem = 965;
			break;
		case 211:
			dropItem = 947;
			break;
		case 6:
			dropItem = 11;
			break;
		case 7:
			dropItem = 12;
			break;
		case 8:
			dropItem = 13;
			break;
		case 9:
			dropItem = 14;
			break;
		case 202:
			dropItem = 824;
			break;
		case 234:
			dropItem = 1246;
			break;
		case 226:
			dropItem = 1101;
			break;
		case 36:
			dropItem = 1869;
			break;
		case 311:
			dropItem = 2260;
			break;
		case 312:
			dropItem = 2261;
			break;
		case 313:
			dropItem = 2262;
			break;
		case 229:
			dropItem = 1125;
			break;
		case 230:
			dropItem = 1127;
			break;
		case 221:
			dropItem = 1104;
			break;
		case 222:
			dropItem = 1105;
			break;
		case 223:
			dropItem = 1106;
			break;
		case 248:
			dropItem = 1589;
			break;
		case 249:
			dropItem = 1591;
			break;
		case 250:
			dropItem = 1593;
			break;
		case 191:
			dropItem = 832;
			break;
		case 203:
			dropItem = 836;
			break;
		case 204:
			dropItem = 880;
			break;
		case 166:
			dropItem = 699;
			break;
		case 167:
			dropItem = 700;
			break;
		case 168:
			dropItem = 701;
			break;
		case 169:
			dropItem = 702;
			break;
		case 123:
			dropItem = 424;
			break;
		case 124:
			dropItem = 480;
			break;
		case 157:
			dropItem = 619;
			break;
		case 158:
			dropItem = 620;
			break;
		case 159:
			dropItem = 621;
			break;
		case 161:
			dropItem = 664;
			break;
		case 206:
			dropItem = 883;
			break;
		case 232:
			dropItem = 1150;
			break;
		case 198:
			dropItem = 775;
			break;
		case 314:
			dropItem = Minecart.GetTrackItem(tileCache);
			break;
		case 189:
			dropItem = 751;
			break;
		case 195:
			dropItem = 763;
			break;
		case 194:
			dropItem = 932;
			break;
		case 193:
			dropItem = 762;
			break;
		case 196:
			dropItem = 765;
			break;
		case 197:
			dropItem = 767;
			break;
		case 22:
			dropItem = 56;
			break;
		case 140:
			dropItem = 577;
			break;
		case 23:
			dropItem = 59;
			break;
		case 25:
			dropItem = 61;
			break;
		case 30:
			dropItem = 9;
			break;
		case 208:
			dropItem = 911;
			break;
		case 372:
			dropItem = 3117;
			break;
		case 646:
			dropItem = 5322;
			break;
		case 371:
			dropItem = 3113;
			break;
		case 174:
			dropItem = 713;
			break;
		case 37:
			dropItem = 116;
			break;
		case 38:
			dropItem = 129;
			break;
		case 39:
			dropItem = 131;
			break;
		case 40:
			dropItem = 133;
			break;
		case 41:
			dropItem = 134;
			break;
		case 43:
			dropItem = 137;
			break;
		case 44:
			dropItem = 139;
			break;
		case 45:
			dropItem = 141;
			break;
		case 46:
			dropItem = 143;
			break;
		case 47:
			dropItem = 145;
			break;
		case 48:
			dropItem = 147;
			break;
		case 49:
			dropItem = 148;
			break;
		case 51:
			dropItem = 150;
			break;
		case 53:
			dropItem = 169;
			break;
		case 151:
			dropItem = 607;
			break;
		case 152:
			dropItem = 609;
			break;
		case 56:
			dropItem = 173;
			break;
		case 57:
			dropItem = 172;
			break;
		case 58:
			dropItem = 174;
			break;
		case 70:
			dropItem = 194;
			break;
		case 75:
			dropItem = 192;
			break;
		case 76:
			dropItem = 214;
			break;
		case 78:
			dropItem = 222;
			break;
		case 81:
			dropItem = 275;
			break;
		case 80:
			dropItem = 276;
			break;
		case 188:
			dropItem = 276;
			break;
		case 107:
			dropItem = 364;
			break;
		case 108:
			dropItem = 365;
			break;
		case 111:
			dropItem = 366;
			break;
		case 150:
			dropItem = 604;
			break;
		case 112:
			dropItem = 370;
			break;
		case 116:
			dropItem = 408;
			break;
		case 117:
			dropItem = 409;
			break;
		case 118:
			dropItem = 412;
			break;
		case 119:
			dropItem = 413;
			break;
		case 120:
			dropItem = 414;
			break;
		case 121:
			dropItem = 415;
			break;
		case 122:
			dropItem = 416;
			break;
		case 136:
			dropItem = 538;
			break;
		case 385:
			dropItem = 3234;
			break;
		case 141:
			dropItem = 580;
			break;
		case 145:
			dropItem = 586;
			break;
		case 146:
			dropItem = 591;
			break;
		case 147:
			dropItem = 593;
			break;
		case 148:
			dropItem = 594;
			break;
		case 153:
			dropItem = 611;
			break;
		case 154:
			dropItem = 612;
			break;
		case 155:
			dropItem = 613;
			break;
		case 156:
			dropItem = 614;
			break;
		case 160:
			dropItem = 662;
			break;
		case 175:
			dropItem = 717;
			break;
		case 176:
			dropItem = 718;
			break;
		case 177:
			dropItem = 719;
			break;
		case 163:
			dropItem = 833;
			break;
		case 164:
			dropItem = 834;
			break;
		case 200:
			dropItem = 835;
			break;
		case 210:
			dropItem = 937;
			break;
		case 130:
			dropItem = 511;
			break;
		case 131:
			dropItem = 512;
			break;
		case 321:
			dropItem = 2503;
			break;
		case 322:
			dropItem = 2504;
			break;
		case 635:
			dropItem = 5215;
			break;
		case 54:
			dropItem = 170;
			break;
		case 326:
			dropItem = 2693;
			break;
		case 327:
			dropItem = 2694;
			break;
		case 458:
			dropItem = 3754;
			break;
		case 459:
			dropItem = 3755;
			break;
		case 345:
			dropItem = 2787;
			break;
		case 328:
			dropItem = 2695;
			break;
		case 329:
			dropItem = 2697;
			break;
		case 507:
			dropItem = 4277;
			break;
		case 508:
			dropItem = 4278;
			break;
		case 255:
		case 256:
		case 257:
		case 258:
		case 259:
		case 260:
		case 261:
			dropItem = 1970 + tileCache.type - 255;
			break;
		case 262:
		case 263:
		case 264:
		case 265:
		case 266:
		case 267:
		case 268:
			dropItem = 1970 + tileCache.type - 262;
			break;
		case 59:
			dropItem = 176;
			break;
		case 60:
			dropItem = 195;
			break;
		case 190:
		case 662:
			dropItem = 183;
			break;
		case 63:
			dropItem = 4642;
			break;
		case 64:
			dropItem = 4644;
			break;
		case 65:
			dropItem = 4643;
			break;
		case 66:
			dropItem = 4641;
			break;
		case 67:
			dropItem = 4640;
			break;
		case 68:
			dropItem = 4645;
			break;
		case 566:
			dropItem = 999;
			break;
		case 129:
			dropItem = 502;
			break;
		case 227:
			num = tileCache.frameX / 34;
			dropItem = 1107 + num;
			if (num >= 8 && num <= 11)
			{
				dropItem = 3385 + num - 8;
			}
			if (dropItem == 1113)
			{
				dropItem = 0;
			}
			break;
		case 4:
			num = tileCache.frameY / 22;
			dropItem = num switch
			{
				0 => 8, 
				8 => 523, 
				9 => 974, 
				10 => 1245, 
				11 => 1333, 
				12 => 2274, 
				13 => 3004, 
				14 => 3045, 
				15 => 3114, 
				16 => 4383, 
				17 => 4384, 
				18 => 4385, 
				19 => 4386, 
				20 => 4387, 
				21 => 4388, 
				22 => 5293, 
				23 => 5353, 
				_ => 426 + num, 
			};
			break;
		case 239:
			num = tileCache.frameX / 18;
			if (num == 0)
			{
				dropItem = 20;
			}
			if (num == 1)
			{
				dropItem = 703;
			}
			if (num == 2)
			{
				dropItem = 22;
			}
			if (num == 3)
			{
				dropItem = 704;
			}
			if (num == 4)
			{
				dropItem = 21;
			}
			if (num == 5)
			{
				dropItem = 705;
			}
			if (num == 6)
			{
				dropItem = 19;
			}
			if (num == 7)
			{
				dropItem = 706;
			}
			if (num == 8)
			{
				dropItem = 57;
			}
			if (num == 9)
			{
				dropItem = 117;
			}
			if (num == 10)
			{
				dropItem = 175;
			}
			if (num == 11)
			{
				dropItem = 381;
			}
			if (num == 12)
			{
				dropItem = 1184;
			}
			if (num == 13)
			{
				dropItem = 382;
			}
			if (num == 14)
			{
				dropItem = 1191;
			}
			if (num == 15)
			{
				dropItem = 391;
			}
			if (num == 16)
			{
				dropItem = 1198;
			}
			if (num == 17)
			{
				dropItem = 1006;
			}
			if (num == 18)
			{
				dropItem = 1225;
			}
			if (num == 19)
			{
				dropItem = 1257;
			}
			if (num == 20)
			{
				dropItem = 1552;
			}
			if (num == 21)
			{
				dropItem = 3261;
			}
			if (num == 22)
			{
				dropItem = 3467;
			}
			break;
		case 380:
			num = tileCache.frameY / 18;
			dropItem = 3215 + num;
			break;
		case 324:
			switch (tileCache.frameY / 22)
			{
			case 0:
				dropItem = 2625;
				break;
			case 1:
				dropItem = 2626;
				break;
			case 2:
				dropItem = 4072;
				break;
			case 3:
				dropItem = 4073;
				break;
			case 4:
				dropItem = 4071;
				break;
			}
			break;
		case 419:
			switch (tileCache.frameX / 18)
			{
			case 0:
				dropItem = 3602;
				break;
			case 1:
				dropItem = 3618;
				break;
			case 2:
				dropItem = 3663;
				break;
			}
			break;
		case 428:
			switch (tileCache.frameY / 18)
			{
			case 0:
				dropItem = 3630;
				break;
			case 1:
				dropItem = 3632;
				break;
			case 2:
				dropItem = 3631;
				break;
			case 3:
				dropItem = 3626;
				break;
			}
			break;
		case 420:
			switch (tileCache.frameY / 18)
			{
			case 0:
				dropItem = 3603;
				break;
			case 1:
				dropItem = 3604;
				break;
			case 2:
				dropItem = 3605;
				break;
			case 3:
				dropItem = 3606;
				break;
			case 4:
				dropItem = 3607;
				break;
			case 5:
				dropItem = 3608;
				break;
			}
			break;
		case 650:
			dropItem = 5324;
			break;
		case 476:
			dropItem = 4040;
			break;
		case 494:
			dropItem = 4089;
			break;
		case 423:
			switch (tileCache.frameY / 18)
			{
			case 0:
				dropItem = 3613;
				break;
			case 1:
				dropItem = 3614;
				break;
			case 2:
				dropItem = 3615;
				break;
			case 3:
				dropItem = 3726;
				break;
			case 4:
				dropItem = 3727;
				break;
			case 5:
				dropItem = 3728;
				break;
			case 6:
				dropItem = 3729;
				break;
			}
			break;
		case 520:
			dropItem = 4326;
			break;
		case 225:
			dropItem = 1129;
			break;
		case 178:
			switch (tileCache.frameX / 18)
			{
			case 0:
				dropItem = 181;
				break;
			case 1:
				dropItem = 180;
				break;
			case 2:
				dropItem = 177;
				break;
			case 3:
				dropItem = 179;
				break;
			case 4:
				dropItem = 178;
				break;
			case 5:
				dropItem = 182;
				break;
			case 6:
				dropItem = 999;
				break;
			}
			break;
		case 149:
			if (tileCache.frameX == 0 || tileCache.frameX == 54)
			{
				dropItem = 596;
			}
			else if (tileCache.frameX == 18 || tileCache.frameX == 72)
			{
				dropItem = 597;
			}
			else if (tileCache.frameX == 36 || tileCache.frameX == 90)
			{
				dropItem = 598;
			}
			break;
		case 13:
			dropItem = (tileCache.frameX / 18) switch
			{
				1 => 5320, 
				2 => 5321, 
				3 => 350, 
				4 => 351, 
				5 => 2234, 
				6 => 2244, 
				7 => 2257, 
				8 => 2258, 
				_ => 31, 
			};
			break;
		case 19:
			num = tileCache.frameY / 18;
			switch (num)
			{
			case 0:
				dropItem = 94;
				break;
			case 1:
				dropItem = 631;
				break;
			case 2:
				dropItem = 632;
				break;
			case 3:
				dropItem = 633;
				break;
			case 4:
				dropItem = 634;
				break;
			case 5:
				dropItem = 913;
				break;
			case 6:
				dropItem = 1384;
				break;
			case 7:
				dropItem = 1385;
				break;
			case 8:
				dropItem = 1386;
				break;
			case 9:
				dropItem = 1387;
				break;
			case 10:
				dropItem = 1388;
				break;
			case 11:
				dropItem = 1389;
				break;
			case 12:
				dropItem = 1418;
				break;
			case 13:
				dropItem = 1457;
				break;
			case 14:
				dropItem = 1702;
				break;
			case 15:
				dropItem = 1796;
				break;
			case 16:
				dropItem = 1818;
				break;
			case 17:
				dropItem = 2518;
				break;
			case 18:
				dropItem = 2549;
				break;
			case 19:
				dropItem = 2566;
				break;
			case 20:
				dropItem = 2581;
				break;
			case 21:
				dropItem = 2627;
				break;
			case 22:
				dropItem = 2628;
				break;
			case 23:
				dropItem = 2629;
				break;
			case 24:
				dropItem = 2630;
				break;
			case 25:
				dropItem = 2744;
				break;
			case 26:
				dropItem = 2822;
				break;
			case 27:
				dropItem = 3144;
				break;
			case 28:
				dropItem = 3146;
				break;
			case 29:
				dropItem = 3145;
				break;
			case 30:
			case 31:
			case 32:
			case 33:
			case 34:
			case 35:
				dropItem = 3903 + num - 30;
				break;
			case 36:
				dropItem = 3945;
				break;
			case 37:
				dropItem = 3957;
				break;
			case 38:
				dropItem = 4159;
				break;
			case 39:
				dropItem = 4180;
				break;
			case 40:
				dropItem = 4201;
				break;
			case 41:
				dropItem = 4222;
				break;
			case 42:
				dropItem = 4311;
				break;
			case 43:
				dropItem = 4416;
				break;
			case 44:
				dropItem = 4580;
				break;
			case 45:
				dropItem = 5162;
				break;
			case 46:
				dropItem = 5183;
				break;
			case 47:
				dropItem = 5204;
				break;
			case 48:
				dropItem = 5292;
				break;
			}
			break;
		case 33:
			num = tileCache.frameY / 22;
			dropItem = 105;
			switch (num)
			{
			case 1:
				dropItem = 1405;
				break;
			case 2:
				dropItem = 1406;
				break;
			case 3:
				dropItem = 1407;
				break;
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
				dropItem = 2045 + num - 4;
				break;
			case 14:
			case 15:
			case 16:
				dropItem = 2153 + num - 14;
				break;
			default:
				switch (num)
				{
				case 17:
					dropItem = 2236;
					break;
				case 18:
					dropItem = 2523;
					break;
				case 19:
					dropItem = 2542;
					break;
				case 20:
					dropItem = 2556;
					break;
				case 21:
					dropItem = 2571;
					break;
				case 22:
					dropItem = 2648;
					break;
				case 23:
					dropItem = 2649;
					break;
				case 24:
					dropItem = 2650;
					break;
				case 25:
					dropItem = 2651;
					break;
				case 26:
					dropItem = 2818;
					break;
				case 27:
					dropItem = 3171;
					break;
				case 28:
					dropItem = 3173;
					break;
				case 29:
					dropItem = 3172;
					break;
				case 30:
					dropItem = 3890;
					break;
				case 31:
					dropItem = 3936;
					break;
				case 32:
					dropItem = 3962;
					break;
				case 33:
					dropItem = 4150;
					break;
				case 34:
					dropItem = 4171;
					break;
				case 35:
					dropItem = 4192;
					break;
				case 36:
					dropItem = 4213;
					break;
				case 37:
					dropItem = 4303;
					break;
				case 38:
					dropItem = 4571;
					break;
				case 39:
					dropItem = 5153;
					break;
				case 40:
					dropItem = 5174;
					break;
				case 41:
					dropItem = 5195;
					break;
				}
				break;
			}
			break;
		case 137:
			num = tileCache.frameY / 18;
			if (num == 0)
			{
				dropItem = 539;
			}
			if (num == 1)
			{
				dropItem = 1146;
			}
			if (num == 2)
			{
				dropItem = 1147;
			}
			if (num == 3)
			{
				dropItem = 1148;
			}
			if (num == 4)
			{
				dropItem = 1149;
			}
			if (num == 5)
			{
				dropItem = 5135;
			}
			break;
		case 135:
			num = tileCache.frameY / 18;
			if (num == 0)
			{
				dropItem = 529;
			}
			if (num == 1)
			{
				dropItem = 541;
			}
			if (num == 2)
			{
				dropItem = 542;
			}
			if (num == 3)
			{
				dropItem = 543;
			}
			if (num == 4)
			{
				dropItem = 852;
			}
			if (num == 5)
			{
				dropItem = 853;
			}
			if (num == 6)
			{
				dropItem = 1151;
			}
			break;
		case 144:
			if (tileCache.frameX == 0)
			{
				dropItem = 583;
			}
			if (tileCache.frameX == 18)
			{
				dropItem = 584;
			}
			if (tileCache.frameX == 36)
			{
				dropItem = 585;
			}
			if (tileCache.frameX == 54)
			{
				dropItem = 4484;
			}
			if (tileCache.frameX == 72)
			{
				dropItem = 4485;
			}
			break;
		case 50:
			dropItem = ((tileCache.frameX != 90) ? 149 : 165);
			break;
		case 82:
		case 83:
		case 84:
			num = tileCache.frameX / 18;
			dropItem = 307 + num;
			if (num == 6)
			{
				dropItem = 2357;
			}
			break;
		case 10:
		{
			int doorStyle = tileCache.frameY / 54;
			doorStyle += tileCache.frameX / 54 * 36;
			switch (doorStyle)
			{
			case 0:
				dropItem = 25;
				break;
			case 9:
				dropItem = 837;
				break;
			case 10:
				dropItem = 912;
				break;
			case 12:
				dropItem = 1137;
				break;
			case 13:
				dropItem = 1138;
				break;
			case 14:
				dropItem = 1139;
				break;
			case 15:
				dropItem = 1140;
				break;
			case 16:
				dropItem = 1411;
				break;
			case 17:
				dropItem = 1412;
				break;
			case 18:
				dropItem = 1413;
				break;
			case 19:
				dropItem = 1458;
				break;
			case 20:
			case 21:
			case 22:
			case 23:
				dropItem = 1709 + doorStyle - 20;
				break;
			case 24:
				dropItem = 1793;
				break;
			case 25:
				dropItem = 1815;
				break;
			case 26:
				dropItem = 1924;
				break;
			case 27:
				dropItem = 2044;
				break;
			case 28:
				dropItem = 2265;
				break;
			case 29:
				dropItem = 2528;
				break;
			case 30:
				dropItem = 2561;
				break;
			case 31:
				dropItem = 2576;
				break;
			case 32:
				dropItem = 2815;
				break;
			case 33:
				dropItem = 3129;
				break;
			case 34:
				dropItem = 3131;
				break;
			case 35:
				dropItem = 3130;
				break;
			case 36:
				dropItem = 3888;
				break;
			case 37:
				dropItem = 3941;
				break;
			case 38:
				dropItem = 3967;
				break;
			case 39:
				dropItem = 4155;
				break;
			case 40:
				dropItem = 4176;
				break;
			case 41:
				dropItem = 4197;
				break;
			case 42:
				dropItem = 4218;
				break;
			case 43:
				dropItem = 4307;
				break;
			case 44:
				dropItem = 4415;
				break;
			case 45:
				dropItem = 4576;
				break;
			case 46:
				dropItem = 5158;
				break;
			case 47:
				dropItem = 5179;
				break;
			case 48:
				dropItem = 5200;
				break;
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				dropItem = 812 + doorStyle;
				break;
			default:
				dropItem = 649 + doorStyle;
				break;
			case 11:
				break;
			}
			break;
		}
		case 12:
			dropItem = 5286;
			break;
		case 14:
		{
			int val5 = tileCache.frameX / 18;
			int val6 = 0;
			while (val5 > 2)
			{
				val5 -= 3;
				val6++;
			}
			val5 = x - val5;
			dropItem = ((val6 >= 1 && val6 <= 3) ? (637 + val6) : ((val6 >= 15 && val6 <= 20) ? (1698 + val6) : ((val6 >= 4 && val6 <= 7) ? (823 + val6) : (val6 switch
			{
				8 => 917, 
				9 => 1144, 
				10 => 1397, 
				11 => 1400, 
				12 => 1403, 
				13 => 1460, 
				14 => 1510, 
				23 => 1926, 
				21 => 1794, 
				22 => 1816, 
				24 => 2248, 
				25 => 2259, 
				26 => 2532, 
				27 => 2550, 
				28 => 677, 
				29 => 2583, 
				30 => 2743, 
				31 => 2824, 
				32 => 3153, 
				33 => 3155, 
				34 => 3154, 
				_ => 32, 
			}))));
			break;
		}
		case 15:
		{
			int frameY = tileCache.frameY;
			int style = 0;
			while (frameY >= 40)
			{
				frameY -= 40;
				style++;
			}
			dropItem = WorldGen.GetItemDrop_Chair(style);
			break;
		}
		case 16:
		{
			int frameXdiv = tileCache.frameX / 18;
			int stylee = frameXdiv >> 1;
			dropItem = ((stylee == 1) ? 716 : 35);
			break;
		}
		case 17:
			dropItem = 33;
			break;
		case 18:
		{
			int frameXd = tileCache.frameX / 18;
			int styled = frameXd >> 1;
			dropItem = 36;
			if (styled >= 1 && styled <= 3)
			{
				dropItem = 634 + styled;
				break;
			}
			if (styled >= 4 && styled <= 8)
			{
				dropItem = 807 + styled;
				break;
			}
			switch (styled)
			{
			case 9:
				dropItem = 916;
				break;
			case 10:
				dropItem = 1145;
				break;
			case 11:
				dropItem = 1398;
				break;
			case 12:
				dropItem = 1401;
				break;
			case 13:
				dropItem = 1404;
				break;
			case 14:
				dropItem = 1461;
				break;
			case 15:
				dropItem = 1511;
				break;
			case 16:
				dropItem = 1795;
				break;
			case 17:
				dropItem = 1817;
				break;
			case 18:
				dropItem = 2229;
				break;
			case 19:
				dropItem = 2251;
				break;
			case 20:
				dropItem = 2252;
				break;
			case 21:
				dropItem = 2253;
				break;
			case 22:
				dropItem = 2534;
				break;
			case 23:
				dropItem = 673;
				break;
			case 24:
				dropItem = 2631;
				break;
			case 25:
				dropItem = 2632;
				break;
			case 26:
				dropItem = 2633;
				break;
			case 27:
				dropItem = 2826;
				break;
			case 28:
				dropItem = 3156;
				break;
			case 29:
				dropItem = 3158;
				break;
			case 30:
				dropItem = 3157;
				break;
			case 31:
				dropItem = 3909;
				break;
			case 32:
				dropItem = 3910;
				break;
			case 33:
				dropItem = 3949;
				break;
			case 34:
				dropItem = 3975;
				break;
			case 35:
				dropItem = 4163;
				break;
			case 36:
				dropItem = 4184;
				break;
			case 37:
				dropItem = 4205;
				break;
			case 38:
				dropItem = 4226;
				break;
			case 39:
				dropItem = 4315;
				break;
			case 40:
				dropItem = 4584;
				break;
			case 41:
				dropItem = 5166;
				break;
			case 42:
				dropItem = 5187;
				break;
			case 43:
				dropItem = 5208;
				break;
			}
			break;
		}
		case 20:
			dropItem = 27;
			break;
		case 27:
			dropItem = 63;
			break;
		case 29:
			dropItem = 87;
			break;
		case 34:
		{
			int var = x;
			int var2 = y;
			int var3 = 3;
			var = tileCache.frameX / 18;
			int var4 = 0;
			while (var >= var3)
			{
				var4++;
				var -= var3;
			}
			int var6 = 18 * var3 * var4;
			if (var >= var3)
			{
				var -= var3;
			}
			var = x - var;
			var2 = tileCache.frameY / 18;
			int var7 = 0;
			while (var2 >= 3)
			{
				var7++;
				var2 -= 3;
			}
			if (var6 >= 108)
			{
				var7 += 37 * (var6 / 108);
			}
			switch (var7)
			{
			case 1:
				dropItem = 107;
				break;
			case 2:
				dropItem = 108;
				break;
			case 3:
				dropItem = 710;
				break;
			case 4:
				dropItem = 711;
				break;
			case 5:
				dropItem = 712;
				break;
			case 6:
				dropItem = 1812;
				break;
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
			case 16:
			case 17:
				dropItem = 2055 + var7 - 7;
				break;
			case 18:
			case 19:
			case 20:
			case 21:
				dropItem = 2141 + var7 - 18;
				break;
			default:
				dropItem = var7 switch
				{
					22 => 2224, 
					23 => 2525, 
					24 => 2543, 
					25 => 2558, 
					26 => 2573, 
					27 => 2652, 
					28 => 2653, 
					29 => 2654, 
					30 => 2655, 
					31 => 2656, 
					32 => 2657, 
					33 => 2813, 
					34 => 3177, 
					35 => 3179, 
					36 => 3178, 
					37 => 3894, 
					38 => 3938, 
					39 => 3964, 
					40 => 4152, 
					41 => 4173, 
					42 => 4194, 
					43 => 4215, 
					44 => 4305, 
					45 => 4573, 
					46 => 5155, 
					47 => 5176, 
					48 => 5197, 
					_ => 106, 
				};
				break;
			}
			break;
		}
		case 35:
			dropItem = 1813;
			break;
		case 42:
		{
			int varia2 = tileCache.frameY / 18;
			int varia3 = 0;
			while (varia2 >= 2)
			{
				varia2 -= 2;
				varia3++;
			}
			if (varia3 == 0)
			{
				dropItem = 136;
				break;
			}
			if (varia3 == 7)
			{
				dropItem = 1431;
				break;
			}
			if (varia3 == 8)
			{
				dropItem = 1808;
				break;
			}
			if (varia3 == 9)
			{
				dropItem = 1859;
				break;
			}
			if (varia3 < 10)
			{
				dropItem = 1389 + varia3;
				break;
			}
			switch (varia3)
			{
			case 10:
				dropItem = 2032;
				break;
			case 11:
				dropItem = 2033;
				break;
			case 12:
				dropItem = 2034;
				break;
			case 13:
				dropItem = 2035;
				break;
			case 14:
				dropItem = 2036;
				break;
			case 15:
				dropItem = 2037;
				break;
			case 16:
				dropItem = 2038;
				break;
			case 17:
				dropItem = 2039;
				break;
			case 18:
				dropItem = 2040;
				break;
			case 19:
				dropItem = 2041;
				break;
			case 20:
				dropItem = 2042;
				break;
			case 21:
				dropItem = 2043;
				break;
			case 22:
			case 23:
			case 24:
			case 25:
				dropItem = 2145 + varia3 - 22;
				break;
			case 26:
				dropItem = 2226;
				break;
			case 27:
				dropItem = 2530;
				break;
			case 28:
				dropItem = 2546;
				break;
			case 29:
				dropItem = 2564;
				break;
			case 30:
				dropItem = 2579;
				break;
			case 31:
				dropItem = 2641;
				break;
			case 32:
				dropItem = 2642;
				break;
			case 33:
				dropItem = 2820;
				break;
			case 34:
				dropItem = 3138;
				break;
			case 35:
				dropItem = 3140;
				break;
			case 36:
				dropItem = 3139;
				break;
			case 37:
				dropItem = 3891;
				break;
			case 38:
				dropItem = 3943;
				break;
			case 39:
				dropItem = 3970;
				break;
			case 40:
				dropItem = 4157;
				break;
			case 41:
				dropItem = 4178;
				break;
			case 42:
				dropItem = 4199;
				break;
			case 43:
				dropItem = 4220;
				break;
			case 44:
				dropItem = 4309;
				break;
			case 45:
				dropItem = 4578;
				break;
			case 46:
				dropItem = 5160;
				break;
			case 47:
				dropItem = 5181;
				break;
			case 48:
				dropItem = 5202;
				break;
			}
			break;
		}
		case 55:
			dropItem = 171;
			break;
		case 77:
			dropItem = 221;
			break;
		case 79:
		{
			int valu2 = tileCache.frameY / 18;
			int valu3 = 0;
			while (valu2 > 1)
			{
				valu2 -= 2;
				valu3++;
			}
			switch (valu3)
			{
			case 0:
				dropItem = 224;
				break;
			case 4:
				dropItem = 920;
				break;
			case 9:
			case 10:
			case 11:
			case 12:
				dropItem = 1710 + valu3;
				break;
			case 5:
			case 6:
			case 7:
			case 8:
				dropItem = 1465 + valu3;
				break;
			default:
				dropItem = ((valu3 >= 13 && valu3 <= 18) ? (2066 + valu3 - 13) : (valu3 switch
				{
					19 => 2139, 
					20 => 2140, 
					21 => 2231, 
					22 => 2520, 
					23 => 2538, 
					24 => 2553, 
					25 => 2568, 
					26 => 2669, 
					27 => 2811, 
					28 => 3162, 
					29 => 3164, 
					30 => 3163, 
					31 => 3897, 
					32 => 3932, 
					33 => 3959, 
					34 => 4146, 
					35 => 4167, 
					36 => 4188, 
					37 => 4209, 
					38 => 4299, 
					39 => 4567, 
					40 => 5149, 
					41 => 5170, 
					42 => 5191, 
					_ => valu3 + 643, 
				}));
				break;
			}
			break;
		}
		case 85:
		{
			int num15 = tileCache.frameX / 18;
			int num16 = 0;
			while (num15 > 1)
			{
				num15 -= 2;
				num16++;
			}
			dropItem = 321;
			if (num16 >= 6 && num16 <= 10)
			{
				dropItem = 3229 + num16 - 6;
			}
			else if (num16 >= 1 && num16 <= 5)
			{
				dropItem = 1173 + num16 - 1;
			}
			break;
		}
		case 86:
			dropItem = 332;
			break;
		case 87:
		{
			int num13 = tileCache.frameX / 18;
			int num14 = 0;
			while (num13 > 2)
			{
				num13 -= 3;
				num14++;
			}
			if (num14 >= 1 && num14 <= 3)
			{
				dropItem = 640 + num14;
				break;
			}
			switch (num14)
			{
			case 4:
				dropItem = 919;
				break;
			case 5:
			case 6:
			case 7:
				dropItem = 2245 + num14 - 5;
				break;
			case 8:
			case 9:
			case 10:
				dropItem = 2254 + num14 - 8;
				break;
			default:
				dropItem = ((num14 >= 11 && num14 <= 20) ? (2376 + num14 - 11) : (num14 switch
				{
					21 => 2531, 
					22 => 2548, 
					23 => 2565, 
					24 => 2580, 
					25 => 2671, 
					26 => 2821, 
					27 => 3141, 
					28 => 3143, 
					29 => 3142, 
					30 => 3915, 
					31 => 3916, 
					32 => 3944, 
					33 => 3971, 
					34 => 4158, 
					35 => 4179, 
					36 => 4200, 
					37 => 4221, 
					38 => 4310, 
					39 => 4579, 
					40 => 5161, 
					41 => 5182, 
					42 => 5203, 
					_ => 333, 
				}));
				break;
			}
			break;
		}
		case 89:
		{
			int numm5 = tileCache.frameX / 18;
			int numm6 = 0;
			while (numm5 > 2)
			{
				numm5 -= 3;
				numm6++;
			}
			dropItem = WorldGen.GetItemDrop_Benches(numm6);
			break;
		}
		case 90:
		{
			int nuum2 = tileCache.frameY / 18;
			int nuum3 = 0;
			while (nuum2 > 1)
			{
				nuum2 -= 2;
				nuum3++;
			}
			switch (nuum3)
			{
			case 0:
				dropItem = 336;
				break;
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
				dropItem = 2072 + nuum3 - 1;
				break;
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
				dropItem = 2124 + nuum3 - 11;
				break;
			default:
				switch (nuum3)
				{
				case 16:
					dropItem = 2232;
					break;
				case 17:
					dropItem = 2519;
					break;
				case 18:
					dropItem = 2537;
					break;
				case 19:
					dropItem = 2552;
					break;
				case 20:
					dropItem = 2567;
					break;
				case 21:
					dropItem = 2658;
					break;
				case 22:
					dropItem = 2659;
					break;
				case 23:
					dropItem = 2660;
					break;
				case 24:
					dropItem = 2661;
					break;
				case 25:
					dropItem = 2662;
					break;
				case 26:
					dropItem = 2663;
					break;
				case 27:
					dropItem = 2810;
					break;
				case 28:
					dropItem = 3159;
					break;
				case 29:
					dropItem = 3161;
					break;
				case 30:
					dropItem = 3160;
					break;
				case 31:
					dropItem = 3895;
					break;
				case 32:
					dropItem = 3931;
					break;
				case 33:
					dropItem = 3958;
					break;
				case 34:
					dropItem = 4145;
					break;
				case 35:
					dropItem = 4166;
					break;
				case 36:
					dropItem = 4187;
					break;
				case 37:
					dropItem = 4208;
					break;
				case 38:
					dropItem = 4298;
					break;
				case 39:
					dropItem = 4566;
					break;
				case 40:
					dropItem = 5148;
					break;
				case 41:
					dropItem = 5169;
					break;
				case 42:
					dropItem = 5190;
					break;
				}
				break;
			}
			break;
		}
		case 91:
		{
			int numb = tileCache.frameY / 18;
			int numb2 = 0;
			while (numb >= 3)
			{
				numb -= 3;
				numb2++;
			}
			int numb4 = tileCache.frameX / 18;
			numb4 += numb2 * 111;
			dropItem = ((numb4 != 310) ? ((numb4 < 297) ? ((numb4 < 295) ? ((numb4 < 294) ? ((numb4 < 288) ? ((numb4 < 278) ? ((numb4 < 273) ? ((numb4 < 272) ? ((numb4 < 270) ? ((numb4 < 207) ? ((numb4 < 109) ? ((numb4 < 22) ? ((numb4 >= 10 && numb4 <= 15) ? (1441 + numb4) : ((numb4 >= 16 && numb4 <= 21) ? (1448 + numb4) : ((numb4 >= 7 && numb4 <= 9) ? (838 + numb4) : ((numb4 < 4 || numb4 > 6) ? (337 + numb4) : (785 + numb4))))) : (1593 + numb4)) : (2788 + numb4)) : (3183 + numb4)) : (3323 + numb4)) : 3780) : (3516 + numb4)) : (3559 + numb4)) : (4253 + numb4)) : 4602) : (4392 + numb4)) : (4668 + numb4)) : 5352);
			break;
		}
		case 92:
			dropItem = 341;
			break;
		case 93:
		{
			int num11 = tileCache.frameY;
			int num12 = 0;
			while (num11 >= 54)
			{
				num11 -= 54;
				num12++;
			}
			switch (num12)
			{
			case 0:
				dropItem = 342;
				break;
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
				dropItem = 2082 + num12 - 1;
				break;
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
			case 16:
				dropItem = 2129 + num12 - 11;
				break;
			default:
				switch (num12)
				{
				case 17:
					dropItem = 2225;
					break;
				case 18:
					dropItem = 2533;
					break;
				case 19:
					dropItem = 2547;
					break;
				case 20:
					dropItem = 2563;
					break;
				case 21:
					dropItem = 2578;
					break;
				case 22:
					dropItem = 2643;
					break;
				case 23:
					dropItem = 2644;
					break;
				case 24:
					dropItem = 2645;
					break;
				case 25:
					dropItem = 2646;
					break;
				case 26:
					dropItem = 2647;
					break;
				case 27:
					dropItem = 2819;
					break;
				case 28:
					dropItem = 3135;
					break;
				case 29:
					dropItem = 3137;
					break;
				case 30:
					dropItem = 3136;
					break;
				case 31:
					dropItem = 3892;
					break;
				case 32:
					dropItem = 3942;
					break;
				case 33:
					dropItem = 3969;
					break;
				case 34:
					dropItem = 4156;
					break;
				case 35:
					dropItem = 4177;
					break;
				case 36:
					dropItem = 4198;
					break;
				case 37:
					dropItem = 4219;
					break;
				case 38:
					dropItem = 4308;
					break;
				case 39:
					dropItem = 4577;
					break;
				case 40:
					dropItem = 5159;
					break;
				case 41:
					dropItem = 5180;
					break;
				case 42:
					dropItem = 5201;
					break;
				}
				break;
			}
			break;
		}
		case 94:
			dropItem = 352;
			break;
		case 95:
			dropItem = 344;
			break;
		case 96:
		{
			int nuumm3 = tileCache.frameX / 18;
			int nuumm4 = 0;
			while (nuumm3 > 1)
			{
				nuumm3 -= 2;
				nuumm4++;
			}
			dropItem = ((nuumm4 == 1) ? 1791 : 345);
			break;
		}
		case 97:
			dropItem = 346;
			break;
		case 98:
			dropItem = 347;
			break;
		case 100:
		{
			int nnum3 = 36;
			int nnum4 = tileCache.frameY;
			int nnum5 = 0;
			while (nnum4 >= nnum3)
			{
				nnum4 -= nnum3;
				nnum5++;
			}
			switch (nnum5)
			{
			case 0:
				dropItem = 349;
				break;
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
				dropItem = 2092 + nnum5 - 1;
				break;
			case 13:
			case 14:
			case 15:
			case 16:
				dropItem = 2149 + nnum5 - 13;
				break;
			default:
				switch (nnum5)
				{
				case 17:
					dropItem = 2227;
					break;
				case 18:
					dropItem = 2522;
					break;
				case 19:
					dropItem = 2541;
					break;
				case 20:
					dropItem = 2555;
					break;
				case 21:
					dropItem = 2570;
					break;
				case 22:
					dropItem = 2664;
					break;
				case 23:
					dropItem = 2665;
					break;
				case 24:
					dropItem = 2666;
					break;
				case 25:
					dropItem = 2667;
					break;
				case 26:
					dropItem = 2668;
					break;
				case 27:
					dropItem = 2825;
					break;
				case 28:
					dropItem = 3168;
					break;
				case 29:
					dropItem = 3170;
					break;
				case 30:
					dropItem = 3169;
					break;
				case 31:
					dropItem = 3893;
					break;
				case 32:
					dropItem = 3935;
					break;
				case 33:
					dropItem = 3961;
					break;
				case 34:
					dropItem = 4149;
					break;
				case 35:
					dropItem = 4170;
					break;
				case 36:
					dropItem = 4191;
					break;
				case 37:
					dropItem = 4212;
					break;
				case 38:
					dropItem = 4302;
					break;
				case 39:
					dropItem = 4570;
					break;
				case 40:
					dropItem = 5152;
					break;
				case 41:
					dropItem = 5173;
					break;
				case 42:
					dropItem = 5194;
					break;
				}
				break;
			}
			break;
		}
		case 101:
		{
			int nnuum = 0;
			for (int nnuum3 = tileCache.frameX / 18; nnuum3 >= 3; nnuum3 -= 3)
			{
				nnuum++;
			}
			switch (nnuum)
			{
			case 1:
				dropItem = 1414;
				break;
			case 2:
				dropItem = 1415;
				break;
			case 3:
				dropItem = 1416;
				break;
			case 4:
				dropItem = 1463;
				break;
			case 5:
				dropItem = 1512;
				break;
			case 6:
				dropItem = 2020;
				break;
			case 7:
				dropItem = 2021;
				break;
			case 8:
				dropItem = 2022;
				break;
			case 9:
				dropItem = 2023;
				break;
			case 10:
				dropItem = 2024;
				break;
			case 11:
				dropItem = 2025;
				break;
			case 12:
				dropItem = 2026;
				break;
			case 13:
				dropItem = 2027;
				break;
			case 14:
				dropItem = 2028;
				break;
			case 15:
				dropItem = 2029;
				break;
			case 16:
				dropItem = 2030;
				break;
			case 17:
				dropItem = 2031;
				break;
			case 18:
			case 19:
			case 20:
			case 21:
				dropItem = 2135 + nnuum - 18;
				break;
			case 22:
				dropItem = 2233;
				break;
			case 23:
				dropItem = 2536;
				break;
			case 24:
				dropItem = 2540;
				break;
			case 25:
				dropItem = 2554;
				break;
			case 26:
				dropItem = 2569;
				break;
			case 27:
				dropItem = 2670;
				break;
			case 28:
				dropItem = 2817;
				break;
			case 29:
				dropItem = 3165;
				break;
			case 30:
				dropItem = 3167;
				break;
			case 31:
				dropItem = 3166;
				break;
			case 32:
				dropItem = 3917;
				break;
			case 33:
				dropItem = 3933;
				break;
			case 34:
				dropItem = 3960;
				break;
			case 35:
				dropItem = 4147;
				break;
			case 36:
				dropItem = 4168;
				break;
			case 37:
				dropItem = 4189;
				break;
			case 38:
				dropItem = 4210;
				break;
			case 39:
				dropItem = 4300;
				break;
			case 40:
				dropItem = 4568;
				break;
			case 41:
				dropItem = 5150;
				break;
			case 42:
				dropItem = 5171;
				break;
			case 43:
				dropItem = 5192;
				break;
			default:
				dropItem = 354;
				break;
			}
			break;
		}
		case 102:
			dropItem = 355;
			break;
		case 103:
			switch (tileCache.frameX / 36)
			{
			case 0:
				dropItem = 356;
				break;
			case 1:
				dropItem = 2235;
				break;
			case 2:
				dropItem = 2242;
				break;
			case 3:
				dropItem = 2243;
				break;
			}
			break;
		case 104:
		{
			int num10 = tileCache.frameX / 36;
			if (num10 >= 1 && num10 <= 5)
			{
				dropItem = 2237 + num10 - 1;
				break;
			}
			switch (num10)
			{
			case 6:
				dropItem = 2560;
				break;
			case 7:
				dropItem = 2575;
				break;
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 22:
			case 23:
				dropItem = 2591 + num10 - 8;
				break;
			case 24:
				dropItem = 2809;
				break;
			case 25:
				dropItem = 3126;
				break;
			case 26:
				dropItem = 3128;
				break;
			case 27:
				dropItem = 3127;
				break;
			case 28:
				dropItem = 3898;
				break;
			case 29:
				dropItem = 3899;
				break;
			case 30:
				dropItem = 3900;
				break;
			case 31:
				dropItem = 3901;
				break;
			case 32:
				dropItem = 3902;
				break;
			case 33:
				dropItem = 3940;
				break;
			case 34:
				dropItem = 3966;
				break;
			case 35:
				dropItem = 4154;
				break;
			case 36:
				dropItem = 4175;
				break;
			case 37:
				dropItem = 4196;
				break;
			case 38:
				dropItem = 4217;
				break;
			case 39:
				dropItem = 4306;
				break;
			case 40:
				dropItem = 4575;
				break;
			case 41:
				dropItem = 5157;
				break;
			case 42:
				dropItem = 5178;
				break;
			case 43:
				dropItem = 5199;
				break;
			default:
				dropItem = 359;
				break;
			}
			break;
		}
		case 105:
		{
			int num8 = tileCache.frameX / 36;
			int num9 = tileCache.frameY / 54;
			num9 %= 3;
			num8 += num9 * 55;
			switch (num8)
			{
			case 0:
				dropItem = 360;
				break;
			case 1:
				dropItem = 52;
				break;
			case 43:
				dropItem = 1152;
				break;
			case 44:
				dropItem = 1153;
				break;
			case 45:
				dropItem = 1154;
				break;
			case 46:
				dropItem = 1408;
				break;
			case 47:
				dropItem = 1409;
				break;
			case 48:
				dropItem = 1410;
				break;
			case 49:
				dropItem = 1462;
				break;
			case 50:
				dropItem = 2672;
				break;
			case 51:
			case 52:
			case 53:
			case 54:
			case 55:
			case 56:
			case 57:
			case 58:
			case 59:
			case 60:
			case 61:
			case 62:
				dropItem = 3651 + num8 - 51;
				break;
			case 63:
			case 64:
			case 65:
			case 66:
			case 67:
			case 68:
			case 69:
			case 70:
			case 71:
			case 72:
			case 73:
			case 74:
			case 75:
				dropItem = 3708 + num8 - 63;
				break;
			default:
				dropItem = num8 switch
				{
					76 => 4397, 
					77 => 4360, 
					78 => 4342, 
					79 => 4466, 
					80 => 5317, 
					81 => 5318, 
					82 => 5319, 
					_ => 438 + num8 - 2, 
				};
				break;
			}
			break;
		}
		case 106:
			dropItem = 363;
			break;
		case 114:
			dropItem = 398;
			break;
		case 125:
			dropItem = 487;
			break;
		case 126:
			dropItem = 488;
			break;
		case 127:
			dropItem = 496;
			break;
		case 128:
			dropItem = 498;
			break;
		case 132:
			dropItem = 513;
			break;
		case 133:
			dropItem = ((tileCache.frameX >= 54) ? 1221 : 524);
			break;
		case 134:
			dropItem = ((tileCache.frameX >= 36) ? 1220 : 525);
			break;
		case 138:
			dropItem = 540;
			break;
		case 139:
		{
			int nnuumm3 = 0;
			for (int num4 = tileCache.frameY / 18; num4 >= 2; num4 -= 2)
			{
				nnuumm3++;
			}
			if (nnuumm3 == 28)
			{
				dropItem = 1963;
			}
			else if (nnuumm3 == 29)
			{
				dropItem = 1964;
			}
			else if (nnuumm3 == 30)
			{
				dropItem = 1965;
			}
			else if (nnuumm3 == 31)
			{
				dropItem = 2742;
			}
			else if (nnuumm3 == 32)
			{
				dropItem = 3044;
			}
			else if (nnuumm3 == 33)
			{
				dropItem = 3235;
			}
			else if (nnuumm3 == 34)
			{
				dropItem = 3236;
			}
			else if (nnuumm3 == 35)
			{
				dropItem = 3237;
			}
			else if (nnuumm3 == 36)
			{
				dropItem = 3370;
			}
			else if (nnuumm3 == 37)
			{
				dropItem = 3371;
			}
			else if (nnuumm3 == 38)
			{
				dropItem = 3796;
			}
			else if (nnuumm3 == 39)
			{
				dropItem = 3869;
			}
			else if (nnuumm3 == 43)
			{
				dropItem = 4077;
			}
			else if (nnuumm3 == 41)
			{
				dropItem = 4078;
			}
			else if (nnuumm3 == 42)
			{
				dropItem = 4079;
			}
			else if (nnuumm3 == 44)
			{
				dropItem = 4080;
			}
			else if (nnuumm3 == 45)
			{
				dropItem = 4081;
			}
			else if (nnuumm3 == 40)
			{
				dropItem = 4082;
			}
			else if (nnuumm3 == 46)
			{
				dropItem = 4237;
			}
			else if (nnuumm3 == 47)
			{
				dropItem = 4356;
			}
			else if (nnuumm3 == 48)
			{
				dropItem = 4357;
			}
			else if (nnuumm3 == 49)
			{
				dropItem = 4358;
			}
			else if (nnuumm3 == 50)
			{
				dropItem = 4421;
			}
			else if (nnuumm3 == 51)
			{
				dropItem = 4606;
			}
			else if (nnuumm3 == 52)
			{
				dropItem = 4979;
			}
			else if (nnuumm3 == 53)
			{
				dropItem = 4985;
			}
			else if (nnuumm3 == 54)
			{
				dropItem = 4990;
			}
			else if (nnuumm3 == 55)
			{
				dropItem = 4991;
			}
			else if (nnuumm3 == 56)
			{
				dropItem = 4992;
			}
			else if (nnuumm3 == 57)
			{
				dropItem = 5006;
			}
			else if (nnuumm3 == 58)
			{
				dropItem = 5014;
			}
			else if (nnuumm3 == 59)
			{
				dropItem = 5015;
			}
			else if (nnuumm3 == 60)
			{
				dropItem = 5016;
			}
			else if (nnuumm3 == 61)
			{
				dropItem = 5017;
			}
			else if (nnuumm3 == 62)
			{
				dropItem = 5018;
			}
			else if (nnuumm3 == 63)
			{
				dropItem = 5019;
			}
			else if (nnuumm3 == 64)
			{
				dropItem = 5020;
			}
			else if (nnuumm3 == 65)
			{
				dropItem = 5021;
			}
			else if (nnuumm3 == 66)
			{
				dropItem = 5022;
			}
			else if (nnuumm3 == 67)
			{
				dropItem = 5023;
			}
			else if (nnuumm3 == 68)
			{
				dropItem = 5024;
			}
			else if (nnuumm3 == 69)
			{
				dropItem = 5025;
			}
			else if (nnuumm3 == 70)
			{
				dropItem = 5026;
			}
			else if (nnuumm3 == 71)
			{
				dropItem = 5027;
			}
			else if (nnuumm3 == 72)
			{
				dropItem = 5028;
			}
			else if (nnuumm3 == 73)
			{
				dropItem = 5029;
			}
			else if (nnuumm3 == 74)
			{
				dropItem = 5030;
			}
			else if (nnuumm3 == 75)
			{
				dropItem = 5031;
			}
			else if (nnuumm3 == 76)
			{
				dropItem = 5032;
			}
			else if (nnuumm3 == 77)
			{
				dropItem = 5033;
			}
			else if (nnuumm3 == 78)
			{
				dropItem = 5034;
			}
			else if (nnuumm3 == 79)
			{
				dropItem = 5035;
			}
			else if (nnuumm3 == 80)
			{
				dropItem = 5036;
			}
			else if (nnuumm3 == 81)
			{
				dropItem = 5037;
			}
			else if (nnuumm3 == 82)
			{
				dropItem = 5038;
			}
			else if (nnuumm3 == 83)
			{
				dropItem = 5039;
			}
			else if (nnuumm3 == 84)
			{
				dropItem = 5040;
			}
			else if (nnuumm3 == 85)
			{
				dropItem = 5044;
			}
			else if (nnuumm3 == 86)
			{
				dropItem = 5112;
			}
			else if (nnuumm3 == 87)
			{
				dropItem = 5362;
			}
			else if (nnuumm3 >= 13)
			{
				dropItem = 1596 + nnuumm3 - 13;
			}
			break;
		}
		case 142:
			dropItem = 581;
			break;
		case 143:
			dropItem = 582;
			break;
		case 162:
			dropItem = 4391;
			break;
		case 172:
		{
			int integer3 = 38;
			int integer4 = tileCache.frameY;
			int integer5 = 0;
			while (integer4 >= integer3)
			{
				integer4 -= integer3;
				integer5++;
			}
			dropItem = 2827 + integer5;
			switch (integer5)
			{
			case 29:
				dropItem = 3147;
				break;
			case 30:
				dropItem = 3149;
				break;
			case 31:
				dropItem = 3148;
				break;
			case 32:
				dropItem = 3896;
				break;
			case 33:
				dropItem = 3946;
				break;
			case 34:
				dropItem = 3972;
				break;
			case 35:
				dropItem = 4160;
				break;
			case 36:
				dropItem = 4181;
				break;
			case 37:
				dropItem = 4202;
				break;
			case 38:
				dropItem = 4223;
				break;
			case 39:
				dropItem = 4312;
				break;
			case 40:
				dropItem = 4581;
				break;
			case 41:
				dropItem = 5163;
				break;
			case 42:
				dropItem = 5184;
				break;
			case 43:
				dropItem = 5205;
				break;
			}
			break;
		}
		case 173:
			dropItem = 714;
			break;
		case 184:
			dropItem = 713;
			break;
		case 192:
			dropItem = 933;
			break;
		case 207:
		{
			int integ11 = tileCache.frameX / 36;
			switch (integ11)
			{
			case 0:
				integ11 = 909;
				break;
			case 1:
				integ11 = 910;
				break;
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
				integ11 = 938 + integ11;
				break;
			case 8:
				integ11 = 4922;
				break;
			case 9:
				integ11 = 4417;
				break;
			}
			dropItem = integ11;
			break;
		}
		case 209:
		{
			int numver = tileCache.frameX / 18;
			int numver2 = 0;
			while (numver >= 4)
			{
				numver2++;
				numver -= 4;
			}
			dropItem = 928;
			switch (numver2)
			{
			case 1:
				dropItem = 1337;
				break;
			case 2:
				dropItem = 3369;
				break;
			case 3:
			case 4:
				dropItem = 3664;
				break;
			}
			break;
		}
		case 212:
			dropItem = 951;
			break;
		case 216:
		{
			int numv2 = tileCache.frameY;
			int numv3 = 0;
			while (numv2 >= 40)
			{
				numv2 -= 40;
				numv3++;
			}
			dropItem = 970 + numv3;
			break;
		}
		case 217:
			dropItem = 995;
			break;
		case 218:
			dropItem = 996;
			break;
		case 219:
			dropItem = 997;
			break;
		case 220:
			dropItem = 998;
			break;
		case 228:
			dropItem = 1120;
			break;
		case 235:
			dropItem = 1263;
			break;
		case 237:
			dropItem = 1292;
			break;
		case 240:
		{
			int numval = 0;
			int numval2 = 0;
			int numval3 = tileCache.frameX / 18;
			while (numval3 >= 3)
			{
				numval3 -= 3;
				numval++;
			}
			int numval5 = tileCache.frameY / 18;
			while (numval5 >= 3)
			{
				numval5 -= 3;
				numval2++;
			}
			numval += numval2 * 36;
			switch (numval)
			{
			case 37:
				dropItem = 1856;
				break;
			case 36:
				dropItem = 1855;
				break;
			case 38:
				dropItem = 1960;
				break;
			case 39:
				dropItem = 1961;
				break;
			case 40:
				dropItem = 1962;
				break;
			case 33:
				dropItem = 1574;
				break;
			case 34:
				dropItem = 1575;
				break;
			case 35:
				dropItem = 1576;
				break;
			case 18:
				dropItem = 1419;
				break;
			case 19:
				dropItem = 1420;
				break;
			case 20:
				dropItem = 1427;
				break;
			case 21:
				dropItem = 1428;
				break;
			case 54:
				dropItem = 2489;
				break;
			case 55:
				dropItem = 2589;
				break;
			case 56:
				dropItem = 3357;
				break;
			case 57:
				dropItem = 3358;
				break;
			case 58:
				dropItem = 3359;
				break;
			case 59:
				dropItem = 3595;
				break;
			case 60:
				dropItem = 3867;
				break;
			case 61:
				dropItem = 3866;
				break;
			case 62:
				dropItem = 3868;
				break;
			case 69:
				dropItem = 4660;
				break;
			case 70:
				dropItem = 4723;
				break;
			case 71:
				dropItem = 4724;
				break;
			case 72:
				dropItem = 4783;
				break;
			case 73:
				dropItem = 4958;
				break;
			case 74:
				dropItem = 5087;
				break;
			case 75:
				dropItem = 5108;
				break;
			case 92:
				dropItem = 5388;
				break;
			case 76:
			case 77:
			case 78:
			case 79:
			case 80:
			case 81:
			case 82:
			case 83:
			case 84:
			case 85:
			case 86:
			case 87:
			case 88:
			case 89:
			case 90:
			case 91:
				dropItem = 5218 + numval - 76;
				break;
			case 63:
			case 64:
			case 65:
			case 66:
			case 67:
			case 68:
				dropItem = 4626 + numval - 63;
				break;
			default:
				dropItem = ((numval >= 41 && numval <= 45) ? (2114 + numval - 41) : ((numval >= 46 && numval <= 53) ? (2442 + numval - 46) : ((numval >= 22 && numval <= 25) ? (1440 + numval - 22) : ((numval >= 26 && numval <= 29) ? (1496 + numval - 26) : ((numval < 30 || numval > 32) ? (1360 + numval) : (1538 + numval - 30))))));
				break;
			}
			break;
		}
		case 241:
			dropItem = 1417;
			break;
		case 242:
		{
			int numbered = 0;
			int numbered2 = tileCache.frameY / 18;
			int numbered3 = tileCache.frameX / 18;
			numbered = 27 * (numbered3 / 6) + (numbered2 >> 2);
			switch (numbered)
			{
			case 30:
				dropItem = 2995;
				break;
			case 65:
				dropItem = 5389;
				break;
			case 64:
				dropItem = 5310;
				break;
			case 63:
				dropItem = 5308;
				break;
			case 62:
				dropItem = 5274;
				break;
			case 50:
			case 51:
			case 52:
			case 53:
			case 54:
			case 55:
			case 56:
			case 57:
			case 58:
			case 59:
			case 60:
			case 61:
				dropItem = 5234 + numbered - 50;
				break;
			case 46:
			case 47:
			case 48:
			case 49:
				dropItem = 5121 + numbered - 46;
				break;
			default:
				switch (numbered)
				{
				case 45:
					dropItem = 5085;
					break;
				case 44:
					dropItem = 4725;
					break;
				case 43:
					dropItem = 4658;
					break;
				case 37:
				case 38:
				case 39:
				case 40:
				case 41:
				case 42:
					dropItem = 4632 + numbered - 37;
					break;
				case 31:
				case 32:
				case 33:
				case 34:
				case 35:
					dropItem = 3055 + numbered - 31;
					break;
				default:
					dropItem = ((numbered >= 27 && numbered <= 29) ? (2865 + numbered - 27) : ((numbered != 36) ? ((numbered != 26) ? ((numbered != 25) ? ((numbered < 22) ? ((numbered < 17) ? ((numbered != 16) ? ((numbered < 13) ? ((numbered < 6) ? (1421 + numbered) : (1433 + numbered - 6)) : (1500 + numbered - 13)) : 1573) : (1846 + numbered - 17)) : (2281 + numbered - 22)) : 2495) : 2497) : 3596));
					break;
				}
				break;
			}
			break;
		}
		case 243:
			dropItem = 1430;
			break;
		case 244:
			dropItem = 1449;
			break;
		case 245:
		{
			int valuu = 0;
			int valuu2 = tileCache.frameX / 18;
			while (valuu2 >= 2)
			{
				valuu2 -= 2;
				valuu++;
			}
			dropItem = ((valuu >= 19 && valuu <= 28) ? (5248 + valuu - 19) : ((valuu >= 15 && valuu <= 18) ? 5247 : (valuu switch
			{
				14 => 5246, 
				13 => 5086, 
				12 => 4728, 
				11 => 4727, 
				10 => 4726, 
				9 => 4659, 
				8 => 4639, 
				7 => 4638, 
				6 => 1577, 
				5 => 1495, 
				_ => 1474 + valuu, 
			})));
			break;
		}
		case 246:
		{
			int valyou = 0;
			int valyou2 = tileCache.frameY / 18;
			while (valyou2 >= 2)
			{
				valyou2 -= 2;
				valyou++;
			}
			switch (valyou)
			{
			case 16:
				dropItem = 1541;
				break;
			case 17:
				dropItem = 1542;
				break;
			case 18:
				dropItem = 1908;
				break;
			case 19:
				dropItem = 4661;
				break;
			case 20:
				dropItem = 4729;
				break;
			case 21:
			case 22:
			case 23:
			case 24:
			case 25:
			case 26:
			case 27:
			case 28:
			case 29:
			case 30:
			case 31:
			case 32:
			case 33:
			case 34:
			case 35:
			case 36:
				dropItem = 5258 + valyou - 21;
				break;
			default:
				dropItem = 1479 + valyou;
				break;
			}
			break;
		}
		case 247:
			dropItem = 1551;
			break;
		case 254:
			dropItem = 1828;
			break;
		case 269:
			dropItem = 1989;
			break;
		case 270:
			dropItem = 1993;
			break;
		case 271:
			dropItem = 2005;
			break;
		case 275:
			dropItem = 2162;
			break;
		case 276:
			dropItem = 2163;
			break;
		case 277:
			dropItem = 2164;
			break;
		case 278:
			dropItem = 2165;
			break;
		case 279:
			dropItem = 2166;
			break;
		case 280:
			dropItem = 2167;
			break;
		case 281:
			dropItem = 2168;
			break;
		case 282:
			dropItem = 250;
			break;
		case 283:
			dropItem = 2172;
			break;
		case 285:
			dropItem = 2174;
			break;
		case 286:
			dropItem = 2175;
			break;
		case 287:
			dropItem = 2177;
			break;
		case 288:
			dropItem = 2178;
			break;
		case 289:
			dropItem = 2179;
			break;
		case 290:
			dropItem = 2180;
			break;
		case 291:
			dropItem = 2181;
			break;
		case 292:
			dropItem = 2182;
			break;
		case 293:
			dropItem = 2183;
			break;
		case 294:
			dropItem = 2184;
			break;
		case 295:
			dropItem = 2185;
			break;
		case 296:
			dropItem = 2186;
			break;
		case 297:
			dropItem = 2187;
			break;
		case 298:
			dropItem = 2190;
			break;
		case 299:
			dropItem = 2191;
			break;
		case 300:
			dropItem = 2192;
			break;
		case 301:
			dropItem = 2193;
			break;
		case 302:
			dropItem = 2194;
			break;
		case 303:
			dropItem = 2195;
			break;
		case 304:
			dropItem = 2196;
			break;
		case 305:
			dropItem = 2197;
			break;
		case 306:
			dropItem = 2198;
			break;
		case 307:
			dropItem = 2203;
			break;
		case 308:
			dropItem = 2204;
			break;
		case 309:
			dropItem = 2206;
			break;
		case 310:
			dropItem = 2207;
			break;
		case 316:
			dropItem = 2439;
			break;
		case 317:
			dropItem = 2440;
			break;
		case 318:
			dropItem = 2441;
			break;
		case 319:
			dropItem = 2490;
			break;
		case 320:
			dropItem = 2496;
			break;
		case 334:
			dropItem = 2699;
			break;
		case 335:
			dropItem = 2700;
			break;
		case 337:
			dropItem = 2702 + tileCache.frameX / 36;
			break;
		case 338:
			dropItem = 2738;
			break;
		case 339:
			dropItem = 2741;
			break;
		case 349:
			dropItem = 470;
			break;
		case 354:
			dropItem = 2999;
			break;
		case 355:
			dropItem = 3000;
			break;
		case 356:
			dropItem = 3064;
			break;
		case 358:
			dropItem = 3070;
			break;
		case 359:
			dropItem = 3071;
			break;
		case 360:
			dropItem = 3072;
			break;
		case 361:
			dropItem = 3073;
			break;
		case 362:
			dropItem = 3074;
			break;
		case 363:
			dropItem = 3075;
			break;
		case 364:
			dropItem = 3076;
			break;
		case 373:
			dropItem = 3182;
			break;
		case 374:
			dropItem = 3184;
			break;
		case 375:
			dropItem = 3185;
			break;
		case 376:
			switch (tileCache.frameX / 36)
			{
			case 0:
				dropItem = 2334;
				break;
			case 1:
				dropItem = 2335;
				break;
			case 2:
				dropItem = 2336;
				break;
			case 3:
				dropItem = 3203;
				break;
			case 4:
				dropItem = 3204;
				break;
			case 5:
				dropItem = 3205;
				break;
			case 6:
				dropItem = 3206;
				break;
			case 7:
				dropItem = 3207;
				break;
			case 8:
				dropItem = 3208;
				break;
			case 9:
				dropItem = 3979;
				break;
			case 10:
				dropItem = 3980;
				break;
			case 11:
				dropItem = 3981;
				break;
			case 12:
				dropItem = 3982;
				break;
			case 13:
				dropItem = 3983;
				break;
			case 14:
				dropItem = 3984;
				break;
			case 15:
				dropItem = 3985;
				break;
			case 16:
				dropItem = 3986;
				break;
			case 17:
				dropItem = 3987;
				break;
			case 18:
				dropItem = 4405;
				break;
			case 19:
				dropItem = 4406;
				break;
			case 20:
				dropItem = 4407;
				break;
			case 21:
				dropItem = 4408;
				break;
			case 22:
				dropItem = 4877;
				break;
			case 23:
				dropItem = 4878;
				break;
			case 24:
				dropItem = 5002;
				break;
			case 25:
				dropItem = 5003;
				break;
			}
			break;
		case 377:
			dropItem = 3198;
			break;
		case 378:
			dropItem = 3202;
			break;
		case 384:
			dropItem = 3361;
			break;
		case 387:
			dropItem = 3239;
			break;
		case 388:
			dropItem = 3240;
			break;
		case 390:
			dropItem = 3253;
			break;
		case 391:
			dropItem = 3254;
			break;
		case 392:
			dropItem = 3255;
			break;
		case 393:
			dropItem = 3256;
			break;
		case 394:
			dropItem = 3257;
			break;
		case 395:
			dropItem = 3270;
			break;
		case 405:
			dropItem = 3364;
			break;
		case 406:
			dropItem = 3365;
			break;
		case 410:
			dropItem = 3536 + tileCache.frameX / 36;
			break;
		case 411:
			dropItem = 3545;
			break;
		case 412:
			dropItem = 3549;
			break;
		case 413:
			dropItem = 3565;
			break;
		case 414:
			dropItem = 3566;
			break;
		case 425:
			dropItem = 3617;
			break;
		case 440:
		{
			int valyuu = 0;
			int valyuu3 = tileCache.frameX / 18;
			while (valyuu3 >= 3)
			{
				valyuu3 -= 3;
				valyuu++;
			}
			switch (valyuu)
			{
			case 0:
				dropItem = 3644;
				break;
			case 1:
				dropItem = 3645;
				break;
			case 2:
				dropItem = 3646;
				break;
			case 3:
				dropItem = 3647;
				break;
			case 4:
				dropItem = 3648;
				break;
			case 5:
				dropItem = 3649;
				break;
			case 6:
				dropItem = 3650;
				break;
			}
			break;
		}
		case 441:
		{
			int aNumber3 = tileCache.frameX / 18;
			int aNumber4 = 0;
			while (aNumber3 > 1)
			{
				aNumber3 -= 2;
				aNumber4++;
			}
			switch (aNumber4)
			{
			case 0:
				dropItem = 3665;
				break;
			case 1:
				dropItem = 3666;
				break;
			case 3:
				dropItem = 3667;
				break;
			case 7:
				dropItem = 3668;
				break;
			case 8:
				dropItem = 3669;
				break;
			case 9:
				dropItem = 3670;
				break;
			case 10:
				dropItem = 3671;
				break;
			case 11:
				dropItem = 3672;
				break;
			case 12:
				dropItem = 3673;
				break;
			case 13:
				dropItem = 3674;
				break;
			case 14:
				dropItem = 3675;
				break;
			case 15:
				dropItem = 3676;
				break;
			case 16:
				dropItem = 3677;
				break;
			case 17:
				dropItem = 3678;
				break;
			case 18:
				dropItem = 3679;
				break;
			case 19:
				dropItem = 3680;
				break;
			case 20:
				dropItem = 3681;
				break;
			case 21:
				dropItem = 3682;
				break;
			case 22:
				dropItem = 3683;
				break;
			case 28:
				dropItem = 3684;
				break;
			case 29:
				dropItem = 3685;
				break;
			case 30:
				dropItem = 3686;
				break;
			case 31:
				dropItem = 3687;
				break;
			case 32:
				dropItem = 3688;
				break;
			case 33:
				dropItem = 3689;
				break;
			case 34:
				dropItem = 3690;
				break;
			case 35:
				dropItem = 3691;
				break;
			case 37:
				dropItem = 3692;
				break;
			case 39:
				dropItem = 3693;
				break;
			case 41:
				dropItem = 3694;
				break;
			case 42:
				dropItem = 3695;
				break;
			case 43:
				dropItem = 3696;
				break;
			case 44:
				dropItem = 3697;
				break;
			case 45:
				dropItem = 3698;
				break;
			case 46:
				dropItem = 3699;
				break;
			case 47:
				dropItem = 3700;
				break;
			case 48:
				dropItem = 3701;
				break;
			case 49:
				dropItem = 3702;
				break;
			case 50:
				dropItem = 3703;
				break;
			case 51:
				dropItem = 3704;
				break;
			}
			break;
		}
		case 443:
			dropItem = 3722;
			break;
		case 444:
			dropItem = 5066;
			break;
		case 452:
			dropItem = 3742;
			break;
		case 453:
			dropItem = ((tileCache.frameX / 18 < 2) ? 3744 : ((tileCache.frameX / 18 >= 4) ? 3743 : 3745));
			break;
		case 454:
			dropItem = 3746;
			break;
		case 455:
			dropItem = 3747;
			break;
		case 456:
			dropItem = 3748;
			break;
		case 457:
			dropItem = 3749;
			break;
		case 461:
			dropItem = 3782;
			break;
		case 462:
			dropItem = 3795;
			break;
		case 463:
			dropItem = 3813;
			break;
		case 464:
			dropItem = 3814;
			break;
		case 465:
			dropItem = 3815;
			break;
		case 466:
			dropItem = 3816;
			break;
		case 468:
		{
			int namber3 = tileCache.frameX / 18;
			int namber4 = 0;
			while (namber3 > 1)
			{
				namber3 -= 2;
				namber4++;
			}
			switch (namber4)
			{
			case 0:
				dropItem = 3886;
				break;
			case 1:
				dropItem = 3887;
				break;
			case 2:
				dropItem = 3950;
				break;
			case 3:
				dropItem = 3976;
				break;
			case 4:
				dropItem = -1;
				break;
			case 5:
				dropItem = 4164;
				break;
			case 6:
				dropItem = 4185;
				break;
			case 7:
				dropItem = 4206;
				break;
			case 8:
				dropItem = 4227;
				break;
			case 9:
				dropItem = 4266;
				break;
			case 10:
				dropItem = 4268;
				break;
			case 11:
				dropItem = 4585;
				break;
			case 12:
				dropItem = 4713;
				break;
			case 13:
				dropItem = -1;
				break;
			case 14:
				dropItem = 5167;
				break;
			case 15:
				dropItem = 5188;
				break;
			case 16:
				dropItem = 5209;
				break;
			}
			break;
		}
		case 469:
		{
			int nummberr5 = tileCache.frameX / 18;
			int nummberr6 = 0;
			while (nummberr5 > 2)
			{
				nummberr5 -= 3;
				nummberr6++;
			}
			dropItem = 3920;
			if (nummberr6 == 1)
			{
				dropItem = 3948;
			}
			if (nummberr6 == 2)
			{
				dropItem = 3974;
			}
			if (nummberr6 == 3)
			{
				dropItem = 4162;
			}
			if (nummberr6 == 4)
			{
				dropItem = 4183;
			}
			if (nummberr6 == 5)
			{
				dropItem = 4204;
			}
			if (nummberr6 == 6)
			{
				dropItem = 4225;
			}
			if (nummberr6 == 7)
			{
				dropItem = 4314;
			}
			if (nummberr6 == 8)
			{
				dropItem = 4583;
			}
			if (nummberr6 == 9)
			{
				dropItem = 5165;
			}
			if (nummberr6 == 10)
			{
				dropItem = 5186;
			}
			if (nummberr6 == 11)
			{
				dropItem = 5207;
			}
			break;
		}
		case 470:
			dropItem = ((tileCache.frameX / 72 != 1) ? 498 : 1989);
			break;
		case 471:
			dropItem = 2699;
			break;
		case 475:
			dropItem = 3977;
			break;
		case 480:
			dropItem = 4054;
			break;
		case 481:
			dropItem = 4238;
			break;
		case 482:
			dropItem = 4239;
			break;
		case 483:
			dropItem = 4240;
			break;
		case 484:
			dropItem = 4390;
			break;
		case 485:
			dropItem = 5067;
			break;
		case 486:
			dropItem = 4063;
			break;
		case 487:
			dropItem = WorldGen.GetItemDrop_PicnicTables(tileCache.frameX / 72);
			break;
		case 489:
			dropItem = 4074;
			break;
		case 490:
			dropItem = 4075;
			break;
		case 491:
			dropItem = 4076;
			break;
		case 493:
		{
			int nom4 = tileCache.frameX / 18;
			if (tileCache.frameX == -1)
			{
				nom4 = tileCache.frameX / 18;
			}
			switch (nom4)
			{
			case 0:
				dropItem = 4083;
				break;
			case 1:
				dropItem = 4084;
				break;
			case 2:
				dropItem = 4085;
				break;
			case 3:
				dropItem = 4086;
				break;
			case 4:
				dropItem = 4087;
				break;
			case 5:
				dropItem = 4088;
				break;
			}
			break;
		}
		case 497:
		{
			int valnum2 = tileCache.frameY;
			int valnum3 = 0;
			while (valnum2 >= 40)
			{
				valnum2 -= 40;
				valnum3++;
			}
			dropItem = WorldGen.GetItemDrop_Toilet(valnum3);
			break;
		}
		case 499:
			dropItem = 4142;
			break;
		case 504:
			dropItem = 4262;
			break;
		case 505:
			dropItem = 4275;
			break;
		case 506:
			dropItem = 4276;
			break;
		case 509:
			dropItem = 4318;
			break;
		case 510:
			dropItem = 4319;
			break;
		case 511:
			dropItem = 4320;
			break;
		case 521:
			dropItem = 4327;
			break;
		case 522:
			dropItem = 4328;
			break;
		case 523:
			dropItem = 4329;
			break;
		case 524:
			dropItem = 4330;
			break;
		case 525:
			dropItem = 4331;
			break;
		case 526:
			dropItem = 4332;
			break;
		case 527:
			dropItem = 4333;
			break;
		case 531:
			dropItem = 4355;
			break;
		case 532:
			dropItem = 4364;
			break;
		case 533:
			dropItem = 4376;
			break;
		case 538:
			dropItem = 4380;
			break;
		case 542:
			dropItem = 4396;
			break;
		case 543:
			dropItem = 4398;
			break;
		case 544:
			dropItem = 4399;
			break;
		case 545:
			dropItem = 4420;
			break;
		case 547:
		{
			int someVal = tileCache.frameX / 18;
			int someVal2 = 0;
			while (someVal >= 2)
			{
				someVal2++;
				someVal -= 2;
			}
			dropItem = 4430 + someVal2;
			break;
		}
		case 548:
		{
			int theStyle = tileCache.frameX / 18;
			int theStyle2 = 0;
			while (theStyle >= 3)
			{
				theStyle2++;
				theStyle -= 3;
			}
			dropItem = ((theStyle2 >= 7) ? (4902 + theStyle2 - 7) : (4435 + theStyle2));
			break;
		}
		case 550:
			dropItem = 4461;
			break;
		case 551:
			dropItem = 4462;
			break;
		case 552:
			dropItem = 4460;
			break;
		case 553:
			dropItem = 4473;
			break;
		case 554:
			dropItem = 4474;
			break;
		case 555:
			dropItem = 4475;
			break;
		case 556:
			dropItem = 4476;
			break;
		case 558:
			dropItem = 4481;
			break;
		case 559:
			dropItem = 4483;
			break;
		case 560:
			dropItem = 4599 + tileCache.frameX / 36;
			break;
		case 564:
			dropItem = 4553;
			break;
		case 565:
			dropItem = 4552;
			break;
		case 567:
			dropItem = 4609;
			break;
		case 568:
			dropItem = 4655;
			break;
		case 569:
			dropItem = 4656;
			break;
		case 570:
			dropItem = 4657;
			break;
		case 572:
		{
			int someValue2 = tileCache.frameY / 18;
			int someValue3 = 0;
			while (someValue2 >= 2)
			{
				someValue2 -= 2;
				someValue3++;
			}
			dropItem = 4695 + someValue3;
			break;
		}
		case 573:
			dropItem = 4710;
			break;
		case 580:
			dropItem = 4846;
			break;
		case 581:
			dropItem = 4848;
			break;
		case 582:
			dropItem = 4850;
			break;
		case 583:
		case 584:
		case 585:
		case 586:
		case 587:
		case 588:
		case 589:
			dropItem = 4851 + (tileCache.type - 583);
			break;
		case 590:
			dropItem = 4851 + tileCache.frameX / 54;
			break;
		case 591:
			dropItem = 4858 + tileCache.frameX / 36;
			break;
		case 592:
			dropItem = 4867;
			break;
		case 594:
			dropItem = 4869;
			break;
		case 595:
			dropItem = 4871;
			break;
		case 597:
			dropItem = TETeleportationPylon.GetPylonItemTypeFromTileStyle(tileCache.frameX / 54);
			break;
		case 598:
			dropItem = 4880;
			break;
		case 599:
			dropItem = 4882;
			break;
		case 600:
			dropItem = 4883;
			break;
		case 601:
			dropItem = 4884;
			break;
		case 602:
			dropItem = 4885;
			break;
		case 603:
			dropItem = 4886;
			break;
		case 604:
			dropItem = 4887;
			break;
		case 605:
			dropItem = 4888;
			break;
		case 606:
			dropItem = 4889;
			break;
		case 607:
			dropItem = 4890;
			break;
		case 608:
			dropItem = 4891;
			break;
		case 609:
			dropItem = 4892;
			break;
		case 610:
			dropItem = 4893;
			break;
		case 611:
			dropItem = 4894;
			break;
		case 612:
			dropItem = 4895;
			break;
		case 613:
		{
			int valz = tileCache.frameX / 18;
			int valz2 = 0;
			while (valz >= 3)
			{
				valz2++;
				valz -= 3;
			}
			dropItem = 4904 + valz2;
			break;
		}
		case 614:
			dropItem = 4906;
			break;
		case 615:
			dropItem = 4907;
			break;
		case 617:
		{
			int theValue = 0;
			for (int theValue3 = tileCache.frameX / 18; theValue3 >= 3; theValue3 -= 3)
			{
				theValue++;
			}
			dropItem = ((theValue == 27) ? 5110 : (4924 + theValue));
			break;
		}
		case 619:
			dropItem = 4963;
			break;
		case 620:
			dropItem = 4964;
			break;
		case 621:
			dropItem = 3750;
			break;
		case 622:
			dropItem = 5008;
			break;
		case 623:
		{
			int theValyou = tileCache.frameX / 18;
			int theValyou2 = 0;
			while (theValyou >= 2)
			{
				theValyou2++;
				theValyou -= 2;
			}
			dropItem = 5081 + theValyou2;
			break;
		}
		case 629:
			dropItem = 5133;
			break;
		case 632:
			dropItem = 5213;
			break;
		case 639:
			dropItem = 5287;
			break;
		case 640:
			dropItem = 5301;
			break;
		case 642:
			dropItem = 5296;
			break;
		case 643:
			dropItem = 5314;
			break;
		case 644:
			dropItem = 5315;
			break;
		case 645:
			dropItem = 5316;
			break;
		case 647:
		case 648:
			dropItem = 5330;
			break;
		case 649:
			dropItem = 5329;
			break;
		case 651:
			dropItem = 5330;
			break;
		case 652:
			dropItem = 5329;
			break;
		case 654:
			dropItem = 5327;
			break;
		case 657:
			dropItem = 5345;
			break;
		case 658:
			dropItem = 5347;
			break;
		case 660:
			dropItem = 5351;
			break;
		case 663:
			dropItem = 5381;
			break;
		case 664:
			dropItem = 5383;
			break;
		case 665:
			dropItem = 5384;
			break;
		}
		return new Item(dropItem);
	}

	public static async void LoadPhotosFromFile(string filePath = "")
	{
		try
		{
			if (!Mods.drawPhotoMode)
			{
				await Task.Run(delegate
				{
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Unknown result type (might be due to invalid IL or missing references)
					//IL_0102: Unknown result type (might be due to invalid IL or missing references)
					//IL_019d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0201: Unknown result type (might be due to invalid IL or missing references)
					//IL_056a: Unknown result type (might be due to invalid IL or missing references)
					//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
					//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
					//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
					//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
					if (Mods.openingTEDITSchematic)
					{
						Main.NewText("Already loading a schematic or photo, please wait for the operation to finish.", Mods.errorColor);
					}
					else
					{
						Mods.openingTEDITSchematic = true;
						if (string.IsNullOrEmpty(filePath))
						{
							Main.NewText("Select an image file...", Mods.zazasFavoriteColor);
							filePath = FileBrowser.OpenFilePanel("Select an image to paste", new ExtensionFilter[5]
							{
								new ExtensionFilter("PNG Files", "png"),
								new ExtensionFilter("JPEG Files", "jpg"),
								new ExtensionFilter("JPEG Files", "jpeg"),
								new ExtensionFilter("WEBP Files", "webp"),
								new ExtensionFilter("BMP Files", "bmp")
							});
						}
						else if (!Path.Exists(filePath))
						{
							Main.NewText("The specified file doesn't exist.", Mods.errorColor);
							Mods.openingTEDITSchematic = false;
							return;
						}
						if (filePath != null)
						{
							List<string> list = new List<string> { "png", "jpg", "jpeg", "webp", "bmp" };
							bool flag = false;
							for (int i = 0; i < list.Count; i++)
							{
								if (filePath.ToLower().EndsWith(list[i].ToLower()))
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								Main.NewText("The file is not one of the valid image formats: JPEG (jpg), PNG, WEBP, or BMP. Please use a valid image format.", Mods.errorColor);
								Mods.openingTEDITSchematic = false;
								return;
							}
							Mods.pasteDelayCounter = Mods.pasteDelay;
							if (Mods.pixelColorsInHex.Count > 0 && Mods.pixelColorsInHex.Capacity > 0)
							{
								Mods.pixelColorsInHex.Clear();
								Mods.pixelColorsInHex.TrimExcess();
							}
							using FileStream fileStream = File.OpenRead(filePath);
							SKBitmap val = SKBitmap.Decode((Stream)fileStream);
							if (val == null)
							{
								Main.NewText("That is not an image file!", Mods.errorColor);
								Mods.openingTEDITSchematic = false;
								return;
							}
							if (val.Width > 1024 || val.Height > 1024)
							{
								Main.NewText(filePath + " is too large. Please only use images under 1024x1024 in size!");
								Mods.drawPhotoMode = false;
								Mods.openingTEDITSchematic = false;
								return;
							}
							SKPixmap val2 = val.PeekPixels();
							Span<byte> pixelSpan = val2.GetPixelSpan();
							Mods.Instance.DisableBrushes();
							Mods.selectionMode = false;
							Mods.copyMode = false;
							Mods.pasteMode = false;
							Mods.drawPhotoWidth = val.Width;
							Mods.drawPhotoHeight = val.Height;
							Mods.schematicSizeX = Mods.drawPhotoWidth;
							Mods.schematicSizeY = Mods.drawPhotoHeight;
							Mods.copiedTiles.Clear();
							List<Color> list2 = new List<Color>();
							List<LegacyTile> list3 = new List<LegacyTile>();
							if (Mods.drawPhotoType != 2)
							{
								Color val3 = default(Color);
								for (int j = 0; j < val.Width; j++)
								{
									for (int k = 0; k < val.Height; k++)
									{
										int num = k * val2.RowBytes + j * 4;
										if (Mods.drawPhotoType == 0)
										{
											string item = $"#{pixelSpan[num + 2]:X2}{pixelSpan[num + 1]:X2}{pixelSpan[num]:X2}";
											Mods.pixelColorsInHex.Add(item);
										}
										else if (Mods.drawPhotoType == 1)
										{
											LegacyTile legacyTile = new LegacyTile();
											((Color)(ref val3))._002Ector((int)pixelSpan[num + 2], (int)pixelSpan[num + 1], (int)pixelSpan[num], (int)pixelSpan[num + 3]);
											if (list2.Contains(val3))
											{
												Mods.copiedTiles.Add(list3[list2.IndexOf(val3)]);
											}
											else
											{
												list2.Add(val3);
												if (((Color)(ref val3)).A > 30)
												{
													KeyValuePair<ColorTable, byte>? closestWallColor = GetClosestWallColor(val3);
													if (closestWallColor.HasValue)
													{
														legacyTile.wall = closestWallColor.Value.Key.Wall;
														legacyTile.wallColor(closestWallColor.Value.Value);
													}
												}
												list3.Add(legacyTile);
												Mods.copiedTiles.Add(legacyTile);
											}
										}
									}
								}
							}
							else if (Mods.drawPhotoType == 0)
							{
								for (int l = 0; l < Mods.pixelColorsInHex.Count; l++)
								{
									LegacyTile legacyTile2 = new LegacyTile();
									int num2 = ConvertBlockColorToClosestPixelColorV2(l);
									if (ATempFix(num2))
									{
										legacyTile2.active(active: true);
										legacyTile2.type = (ushort)num2;
									}
									else
									{
										legacyTile2.wall = (ushort)num2;
									}
									Mods.copiedTiles.Add(legacyTile2);
								}
							}
							else
							{
								Mods.convertingToSchematic = true;
								TEditImageToSchematic.ConvertPixelArt(val);
								while (Mods.convertingToSchematic)
								{
								}
							}
							ResetPreview();
							Mods.openingTEDITSchematic = false;
							Main.NewText($"Selected: {filePath}  |  Size: [X: {Mods.schematicSizeX}, Y: {Mods.schematicSizeY}]", Mods.zazasFavoriteColor);
							Mods.pasteMode = true;
							Mods.pasteDelayCounter = Mods.pasteDelay;
							fileStream.Dispose();
							((SKNativeObject)val2).Dispose();
							((SKNativeObject)val).Dispose();
						}
						Mods.openingTEDITSchematic = false;
					}
				});
			}
			else
			{
				Mods.drawPhotoMode = false;
			}
		}
		catch (Exception ex)
		{
			Mods.drawPhotoMode = false;
			Mods.openingTEDITSchematic = false;
			Main.NewText($"Load Photo error!\n{ex}", Mods.errorColor);
			Mods.LogException(ex);
		}
	}

	public static bool ATempFix(int i)
	{
		if (i != 51 && i != 124 && i != 150 && i != 213 && i != 214 && i != 248 && i != 336 && i != 340 && i != 341 && i != 342 && i != 343 && i != 344 && i != 351 && i != 353 && i != 365 && i != 366 && i != 379 && i != 449 && i != 450 && i != 451 && i != 561 && i != 574 && i != 575 && i != 576 && i != 577)
		{
			return i == 578;
		}
		return true;
	}

	public static int ConvertBlockColorToClosestPixelColorV2(int iteration)
	{
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		int num = int.MaxValue;
		int num2 = -1;
		for (int i = 0; i < Mods.hexValues.Length; i++)
		{
			int pixelColorDifferenceDistance = GetPixelColorDifferenceDistance(HexStringToMicrosoftColor(Mods.pixelColorsInHex[iteration]), Mods.hexValues[i]);
			if (pixelColorDifferenceDistance < num)
			{
				num2 = i;
				num = pixelColorDifferenceDistance;
			}
		}
		try
		{
			if (num2 <= 52)
			{
				return num2 + 1;
			}
			if (num2 > 52 && num2 < 313)
			{
				return num2 + 2;
			}
			if (num2 >= 313)
			{
				return Mods.indexToNonSolidTileID[num2];
			}
			Main.NewText("Error, Returned 0: " + num2, Mods.errorColor);
			return 0;
		}
		catch
		{
			Main.NewText("an error happened on , index: " + num2, Mods.errorColor);
			return 0;
		}
	}

	public static int GetPixelColorDifferenceDistance(Color current, Color match)
	{
		byte b = (byte)(((Color)(ref current)).R - ((Color)(ref match)).R);
		int num = ((Color)(ref current)).G - ((Color)(ref match)).G;
		int num2 = ((Color)(ref current)).B - ((Color)(ref match)).B;
		return b * b + num * num + num2 * num2;
	}

	public static Color HexStringToMicrosoftColor(string hexColor)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		int num = int.Parse(hexColor.Contains("#") ? hexColor.Replace("#", "") : hexColor, NumberStyles.HexNumber);
		byte r = (byte)((num & 0xFF0000) >> 16);
		byte g = (byte)((num & 0xFF00) >> 8);
		byte b = (byte)(num & 0xFF);
		return new Color((int)r, (int)g, (int)b);
	}

	public static void SetupWallColors()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			for (ushort i = 1; i < WallID.Count; i++)
			{
				ColorTable wTable = new ColorTable(i);
				for (byte j = 0; j < 31; j++)
				{
					wTable.Colors.Add(MapTileWallColor(i, j));
				}
				Mods.wallColors.Add(wTable);
			}
		}
		catch
		{
		}
	}

	public static Color MapTileWallColor(ushort wallId, byte wallColor)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int num2 = 255;
		int num3 = 0;
		int baseOption = 0;
		if (wallId > 0 && wallId < WallID.Count)
		{
			num3 = MapHelper.wallLookup[wallId];
			num = wallColor;
			switch (wallId)
			{
			case 21:
			case 88:
			case 89:
			case 90:
			case 91:
			case 92:
			case 93:
			case 168:
			case 241:
				num = 0;
				break;
			case 27:
				baseOption = 0;
				break;
			default:
				baseOption = 0;
				break;
			}
		}
		MapTile mTile = MapTile.Create((ushort)(num3 + baseOption), (byte)num2, (byte)num);
		return MapHelper.GetMapTileXnaColor(ref mTile, 0, 0);
	}

	public static KeyValuePair<ColorTable, byte>? GetClosestWallColor(Color color)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		for (int range = 0; range < 255; range++)
		{
			for (int i = 0; i < Mods.wallColors.Count; i++)
			{
				for (byte j = 0; j < 31; j++)
				{
					Color wColor = Mods.wallColors[i].Colors[j];
					if (ColorWithinRange(color, wColor, range))
					{
						return new KeyValuePair<ColorTable, byte>(Mods.wallColors[i], j);
					}
				}
			}
		}
		return null;
	}

	public static bool ColorWithinRange(Color color, Color wColor, int range)
	{
		if (((Color)(ref wColor)).R > ((Color)(ref color)).R - range && ((Color)(ref wColor)).R < ((Color)(ref color)).R + range && ((Color)(ref wColor)).B > ((Color)(ref color)).B - range && ((Color)(ref wColor)).B < ((Color)(ref color)).B + range && ((Color)(ref wColor)).G > ((Color)(ref color)).G - range && ((Color)(ref wColor)).G < ((Color)(ref color)).G + range && ((Color)(ref wColor)).A > 180)
		{
			return true;
		}
		return false;
	}

	static Schematics()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		previewTint = new Color(210, 210, 210, 100);
		worldTileTextures = new Dictionary<Tuple<int, int>, Texture2D>();
		worldWallTextures = new Dictionary<Tuple<int, int>, Texture2D>();
		worldCacheNeedsReset = false;
		mapCacheNeedsReset = false;
		uniqueTileTypes = new HashSet<Tuple<int, int>>();
		uniqueWallTypes = new HashSet<Tuple<int, int>>();
		WorldSchematicPreview = new SchematicWorldPreviewTargetContent();
		MapSchematicPreview = new SchematicMapPreviewTargetContent();
		schemSizeX = 1;
		schemSizeY = 1;
		schemMapSizeX = 1;
		schemMapSizeY = 1;
		schemX = 0;
		schemY = 0;
		schemMapX = 0;
		schemMapY = 0;
		worldSchemTargets = new RenderTarget2D[schemSizeX, schemSizeY];
		mapSchemTargets = new RenderTarget2D[schemMapSizeX, schemMapSizeY];
		requestedWorldPreview = false;
		requestedMapPreview = false;
		previewRunTwo = false;
	}
}
