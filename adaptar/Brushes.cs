using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ObjectData;

namespace Terraria.ZazaMod.WorldEdit;

public class Brushes
{
	public struct BrushData
	{
		public byte brushType;

		public int x;

		public int y;

		public ushort toPlace;

		public short style;

		public int itemNetId;

		public bool hasObject;

		public int alt;

		public int dir;

		public int rand;

		public BrushData(byte Brush, int xPos, int yPos, ushort tileWallId = 0, short placeStyle = 0, bool hasObj = false, int itemType = 0, int direction = -1, int alternate = 0, int random = 0)
		{
			alt = 0;
			dir = -1;
			rand = 0;
			brushType = Brush;
			x = xPos;
			y = yPos;
			toPlace = tileWallId;
			style = placeStyle;
			hasObject = hasObj;
			itemNetId = itemType;
			dir = direction;
			alt = alternate;
			rand = random;
		}

		public void Handle(ref int sentTilesAndWalls, ref int sentErases, ref int sentLiquids, ref int sentPaint)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			Mods.FakeNetMessage13(new Vector2((float)((x + 2) * 16), (float)(y * 16)), PlayerItemSlotID.InventoryMouseItem);
			if (brushType != 1)
			{
				ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, PlayerItemSlotID.InventoryMouseItem, 0f, itemNetId);
			}
			if (brushType == 0)
			{
				if (!hasObject)
				{
					if (!TileID.Sets.BasicChest[toPlace] && toPlace != 88)
					{
						NetMessage.SendData(17, -1, -1, null, 1, x, y, (int)toPlace, style);
						NetMessage.SendData(17, -1, -1, null, 14, x, y);
					}
					else if (toPlace == 88)
					{
						NetMessage.SendData(34, -1, -1, null, 2, x, y, style);
					}
					else
					{
						WorldGen.PlaceTile(x, y, toPlace, mute: true, forced: false, Main.myPlayer, style);
					}
				}
				else
				{
					TileObjectData.CallPostPlacementPlayerHook(data: new TileObject
					{
						type = toPlace,
						style = style,
						alternate = alt,
						random = rand
					}, tileX: x, tileY: y, type: toPlace, style: style, dir: dir, alternate: alt);
					if (!Main.tileContainer[toPlace] && toPlace != 423)
					{
						NetMessage.SendObjectPlacement(-1, x, y, toPlace, style, alt, rand, dir);
					}
				}
				sentTilesAndWalls++;
			}
			if (brushType == 1)
			{
				if (toPlace == 0)
				{
					NetMessage.SendData(17, -1, -1, null, 4, x, y);
				}
				else if (toPlace == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 2, x, y);
				}
				else
				{
					NetMessage.SendData(17, -1, -1, null, 4, x, y);
					NetMessage.SendData(17, -1, -1, null, 2, x, y);
					sentErases++;
				}
				sentErases++;
			}
			if (brushType == 2)
			{
				NetMessage.SendData(17, -1, -1, null, (style == 1) ? 22 : 3, x, y, (int)toPlace);
				sentTilesAndWalls++;
			}
			if (brushType == 3)
			{
				NetMessage.sendWater(x, y);
				sentLiquids++;
			}
			if (brushType == 4)
			{
				switch (style)
				{
				case 0:
					NetMessage.SendData(63, -1, -1, null, x, y, (int)toPlace);
					break;
				case 1:
					NetMessage.SendData(64, -1, -1, null, x, y, (int)toPlace);
					break;
				case 2:
					NetMessage.SendData(63, -1, -1, null, x, y, (int)toPlace, 1f);
					break;
				case 3:
					NetMessage.SendData(64, -1, -1, null, x, y, (int)toPlace, 1f);
					break;
				case 4:
					NetMessage.SendData(63, -1, -1, null, x, y);
					NetMessage.SendData(64, -1, -1, null, x, y);
					NetMessage.SendData(63, -1, -1, null, x, y, 0f, 1f);
					NetMessage.SendData(64, -1, -1, null, x, y, 0f, 1f);
					sentPaint += 3;
					break;
				}
				sentPaint++;
			}
		}
	}

	public static List<BrushData> tilesToBrush;

	public static readonly Dictionary<int, Func<int, int, bool>> Patterns;

	private static readonly int[,] AmongUsPattern;

	private static Texture2D texture;

	private static readonly Rectangle BrushSrcRect;

	public static bool TileValidForBrush(Point mouseToTile, int i, int j, int size)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (Mods.brushCircle)
		{
			size++;
			int num = i - mouseToTile.X;
			int dy = j - mouseToTile.Y;
			if (num * num + dy * dy >= size * size)
			{
				return false;
			}
		}
		if (Patterns == null || !Patterns.TryGetValue(Mods.brushPattern, out var pattern))
		{
			return true;
		}
		return pattern(i, j);
	}

	public static void TileBrush(Point mouseToTile, int size)
	{
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return;
		}
		int tileToBrush = Main.LocalPlayer.HeldItem.createTile;
		short placementStyle = (short)Main.LocalPlayer.HeldItem.placeStyle;
		int heldItemNetId = Main.LocalPlayer.HeldItem.netID;
		if (Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].type == 4262)
		{
			tileToBrush = 504;
		}
		if (Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].type == 496)
		{
			tileToBrush = 127;
		}
		if (tileToBrush == -1)
		{
			return;
		}
		for (int i = mouseToTile.X - size; i <= mouseToTile.X + size; i++)
		{
			for (int j = mouseToTile.Y - size; j <= mouseToTile.Y + size; j++)
			{
				if (!WorldGen.InWorld(i, j) || !TileValidForBrush(mouseToTile, i, j, size))
				{
					continue;
				}
				Tile tile = Main.tile[i, j];
				if (tile.type != tileToBrush && tile.type >= 0 && !TileObjectData.CustomPlace(tileToBrush, placementStyle))
				{
					tile.ClearTile();
				}
				if (tile.active() && (!tile.active() || tile.type == tileToBrush))
				{
					continue;
				}
				try
				{
					if (TileID.Sets.BasicChest[tileToBrush] || tileToBrush == 82 || tileToBrush == 227 || !TileObjectData.CustomPlace(tileToBrush, placementStyle))
					{
						if (TileID.Sets.BasicChest[tileToBrush] && !TileObject.CanPlace(i, j, tileToBrush, placementStyle, Main.LocalPlayer.direction, out var _))
						{
							continue;
						}
						WorldGen.PlaceTile(i, j, tileToBrush, mute: true, forced: true, Main.myPlayer, placementStyle);
						BrushData data = new BrushData(0, i, j, (ushort)tileToBrush, placementStyle, hasObj: false, heldItemNetId);
						if (Main.netMode != 0)
						{
							try
							{
								if (!TileID.Sets.BasicChest[tileToBrush] || !tilesToBrush.Contains(data))
								{
									tilesToBrush.Add(data);
								}
							}
							catch
							{
							}
						}
						goto IL_0327;
					}
					FlexibleTileWand flexibleTileWand = Main.LocalPlayer.HeldItem.GetFlexibleTileWand();
					int forcedRandom = -1;
					if (flexibleTileWand != null && flexibleTileWand.TryGetPlacementOption(Main.LocalPlayer, Player.FlexibleWandRandomSeed, Player.FlexibleWandCycleOffset, out var option, out var _))
					{
						tileToBrush = option.TileIdToPlace;
						placementStyle = (short)option.TileStyleToPlace;
					}
					TileObjectData data2 = WorldEditBase.PlaceDirectObject(i, j, tileToBrush, out var xCoord, out var yCoord, out var tObject, placementStyle, Main.LocalPlayer.direction, -1, forcedRandom);
					if (data2 != null)
					{
						if (Main.netMode != 0)
						{
							for (int k = xCoord; k < xCoord + data2.Width; k++)
							{
								for (int l = yCoord; l < yCoord + data2.Height; l++)
								{
									BrushData brushData2 = new BrushData(1, i, j, 0, 0);
									if (Main.tile[k, l].type != tileToBrush && Main.tile[k, l].active())
									{
										try
										{
											tilesToBrush.Add(brushData2);
										}
										catch
										{
										}
									}
								}
							}
						}
						WorldGen.SquareTileFrame(i, j, resetFrame: false, noBreak: true);
						BrushData brushData3 = new BrushData(0, i, j, (ushort)tileToBrush, placementStyle, hasObj: true, heldItemNetId, Main.LocalPlayer.direction, tObject.alternate, tObject.random);
						if (tileToBrush == 88)
						{
							brushData3.hasObject = false;
						}
						if (Main.netMode != 0)
						{
							try
							{
								tilesToBrush.Add(brushData3);
							}
							catch
							{
							}
						}
					}
					goto IL_0327;
					IL_0327:
					Main.Map.UpdateLighting(i, j, byte.MaxValue);
					Main.refreshMap = true;
				}
				catch (Exception)
				{
				}
			}
		}
	}

	public static void WallBrush(Point mouseToTile, int size)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return;
		}
		int wallType = (ushort)Main.LocalPlayer.HeldItem.createWall;
		if (wallType <= 0)
		{
			return;
		}
		for (int i = mouseToTile.X - size; i <= mouseToTile.X + size; i++)
		{
			for (int j = mouseToTile.Y - size; j <= mouseToTile.Y + size; j++)
			{
				if (!WorldGen.InWorld(i, j) || !TileValidForBrush(mouseToTile, i, j, size))
				{
					continue;
				}
				Tile tile = Main.tile[i, j];
				BrushData data = new BrushData(2, i, j, (ushort)wallType, 0, hasObj: false, Main.LocalPlayer.HeldItem.netID);
				if (!Main.sectionManager.SectionLoaded(Netplay.GetSectionX(i), Netplay.GetSectionY(j)))
				{
					Main.NewText("Tiles are not loaded, reveal map.", Mods.zazasFavoriteColor);
					return;
				}
				if (tile.wall != wallType && tile.wall > 0)
				{
					tile.Clear(TileDataType.Wall);
					data.style = 1;
				}
				if (tile.wall != 0)
				{
					continue;
				}
				tile.wall = (ushort)wallType;
				WorldGen.SquareWallFrame(i, j);
				if (Main.netMode != 0)
				{
					try
					{
						tilesToBrush.Add(data);
					}
					catch
					{
					}
				}
				Main.Map.UpdateLighting(i, j, byte.MaxValue);
				Main.refreshMap = true;
			}
		}
	}

	public static void EraseBrush(Point mouseToTile, int size)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return;
		}
		int createTile = Main.LocalPlayer.HeldItem.createTile;
		int createWall = Main.LocalPlayer.HeldItem.createWall;
		for (int i = mouseToTile.X - size; i <= mouseToTile.X + size; i++)
		{
			for (int j = mouseToTile.Y - size; j <= mouseToTile.Y + size; j++)
			{
				if (!WorldGen.InWorld(i, j))
				{
					continue;
				}
				bool flag = false;
				bool erasedTile = false;
				bool erasedWall = false;
				if (!TileValidForBrush(mouseToTile, i, j, size))
				{
					continue;
				}
				Tile tile = Main.tile[i, j];
				BrushData data = new BrushData(1, i, j, 0, 0);
				try
				{
					if (tile.active() && createWall <= 0 && (createTile == -1 || tile.type == createTile))
					{
						tile.frameX = -1;
						tile.frameY = -1;
						tile.ClearTile();
						tile.ClearBlockPaintAndCoating();
						WorldGen.SquareTileFrame(i, j);
						flag = true;
						erasedTile = true;
					}
					if (tile.wall > 0 && createTile == -1 && (createWall <= 0 || tile.wall == createWall))
					{
						tile.Clear(TileDataType.Wall);
						tile.ClearWallPaintAndCoating();
						WorldGen.SquareWallFrame(i, j);
						flag = true;
						erasedWall = true;
						data.toPlace = (ushort)((!erasedTile) ? 1u : 2u);
					}
					if ((erasedTile | erasedWall) && Main.netMode != 0)
					{
						try
						{
							tilesToBrush.Add(data);
						}
						catch
						{
						}
					}
				}
				catch (Exception)
				{
				}
				if (flag)
				{
					Main.Map.UpdateLighting(i, j, byte.MaxValue);
					Main.refreshMap = true;
				}
			}
		}
	}

	public static void LiquidAndDryBrush(Point mouseToTile, int size)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return;
		}
		for (int i = mouseToTile.X - size; i <= mouseToTile.X + size; i++)
		{
			for (int j = mouseToTile.Y - size; j <= mouseToTile.Y + size; j++)
			{
				int type = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].type;
				if (!WorldGen.InWorld(i, j) || !TileValidForBrush(mouseToTile, i, j, size))
				{
					continue;
				}
				Tile tile = Main.tile[i, j];
				BrushData data = new BrushData(3, i, j, 0, 0, hasObj: false, type);
				if (!Main.sectionManager.SectionLoaded(Netplay.GetSectionX(i), Netplay.GetSectionY(j)))
				{
					Main.NewText("Tiles are not loaded, reveal map.", Mods.zazasFavoriteColor);
					return;
				}
				try
				{
					bool modifiedLiquid = false;
					if (tile.LiquidAmount < byte.MaxValue && (!tile.HasTile || (tile.HasTile && (!Main.tileSolid[tile.TileType] || TileID.Sets.Platforms[tile.TileType] || tile.TileType == 546 || tile.IsActuated) && tile.TileType != 379)) && ItemID.Sets.LiquidBuckets[type])
					{
						switch (type)
						{
						case 206:
						case 3031:
							tile.LiquidType = 0;
							tile.LiquidAmount = byte.MaxValue;
							WorldGen.SquareTileFrame(i, j, resetFrame: false, noBreak: true);
							data.toPlace = 0;
							data.style = 255;
							data.itemNetId = type;
							modifiedLiquid = true;
							break;
						case 207:
						case 4820:
							tile.LiquidType = 1;
							tile.LiquidAmount = byte.MaxValue;
							WorldGen.SquareTileFrame(i, j, resetFrame: false, noBreak: true);
							data.toPlace = 1;
							data.style = 255;
							modifiedLiquid = true;
							break;
						case 1128:
						case 5302:
							tile.LiquidType = 2;
							tile.LiquidAmount = byte.MaxValue;
							WorldGen.SquareTileFrame(i, j, resetFrame: false, noBreak: true);
							data.toPlace = 2;
							data.style = 255;
							modifiedLiquid = true;
							break;
						case 5364:
							tile.LiquidType = 3;
							tile.LiquidAmount = byte.MaxValue;
							WorldGen.SquareTileFrame(i, j, resetFrame: false, noBreak: true);
							data.toPlace = 3;
							data.style = 255;
							modifiedLiquid = true;
							break;
						}
					}
					else if (tile.LiquidAmount > 0 && tile.LiquidAmount > 0 && ((type == 205 && tile.LiquidType != 3) || (type == 3032 && tile.LiquidType == 0) || (type == 4872 && tile.LiquidType == 1) || (type == 5303 && tile.LiquidType == 2) || type == 5304))
					{
						tile.LiquidAmount = 0;
						tile.LiquidType = 0;
						WorldGen.SquareTileFrame(i, j, resetFrame: false);
						data.toPlace = 0;
						data.style = 0;
						modifiedLiquid = true;
					}
					if (!modifiedLiquid)
					{
						continue;
					}
					Main.Map.UpdateLighting(i, j, byte.MaxValue);
					Main.refreshMap = true;
					if (Main.netMode != 0)
					{
						try
						{
							tilesToBrush.Add(data);
						}
						catch
						{
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}
	}

	public static void TilePaintBrush(Point mouseToTile, int size)
	{
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		byte mode = 0;
		byte paint = 0;
		byte paintCoating = 0;
		if (Mods.HoldingPaintBrush)
		{
			mode = 1;
		}
		if (Mods.HoldingPaintRoller)
		{
			mode = 2;
		}
		if (mode > 0)
		{
			Item item = Main.LocalPlayer.FindPaintOrCoating();
			if (item != null)
			{
				if (item.paintCoating != 0)
				{
					paintCoating = item.paintCoating;
				}
				if (item.paint != 0)
				{
					paint = item.paint;
				}
			}
			if (Mods.paintOverride > 0)
			{
				paint = Mods.paintOverride;
			}
			if (Mods.paintCoatOverride > 0)
			{
				paintCoating = Mods.paintCoatOverride;
			}
		}
		bool painted = false;
		for (int i = mouseToTile.X - size; i <= mouseToTile.X + size; i++)
		{
			for (int j = mouseToTile.Y - size; j <= mouseToTile.Y + size; j++)
			{
				painted = false;
				bool handledAlready = false;
				if (!WorldGen.InWorld(i, j) || !TileValidForBrush(mouseToTile, i, j, size))
				{
					continue;
				}
				BrushData data = new BrushData(4, i, j, 0, 0, hasObj: false, Main.LocalPlayer.HeldItem.netID);
				switch (mode)
				{
				case 1:
					if (paintCoating != 0 && (Mods.paintOnlyTile == -1 || Main.tile[i, j].type == Mods.paintOnlyTile) && WorldGen.paintCoatTile(i, j, paintCoating, broadcast: false, noEffect: true))
					{
						data.style = 2;
						data.toPlace = paintCoating;
						painted = true;
					}
					if (Mods.rainbowPaintType > 0)
					{
						paint = GetRainbowPaintColor(i, j);
					}
					if (paint != 0 && (Mods.paintOnlyTile == -1 || Main.tile[i, j].type == Mods.paintOnlyTile) && WorldGen.paintTile(i, j, paint, broadCast: false, noEffect: true))
					{
						data.style = 0;
						data.toPlace = paint;
						painted = true;
					}
					break;
				case 2:
					if (paintCoating != 0 && (Mods.paintOnlyWall == 0 || Main.tile[i, j].wall == Mods.paintOnlyWall) && WorldGen.paintCoatWall(i, j, paintCoating, broadcast: false, noEffect: true))
					{
						data.style = 3;
						data.toPlace = paintCoating;
						painted = true;
					}
					if (Mods.rainbowPaintType > 0)
					{
						paint = GetRainbowPaintColor(i, j);
					}
					if (paint != 0 && (Mods.paintOnlyWall == 0 || Main.tile[i, j].wall == Mods.paintOnlyWall) && WorldGen.paintWall(i, j, paint, broadCast: false, noEffect: true))
					{
						data.style = 1;
						data.toPlace = paint;
						painted = true;
					}
					break;
				default:
					if ((Mods.paintOnlyTile == -1 || Main.tile[i, j].type == Mods.paintOnlyTile) && WorldGen.paintTile(i, j, 0, broadCast: false, noEffect: true))
					{
						data.style = 0;
						data.toPlace = 0;
						painted = true;
						if (Main.netMode != 0)
						{
							try
							{
								tilesToBrush.Add(data);
							}
							catch
							{
							}
						}
						handledAlready = true;
					}
					if ((Mods.paintOnlyWall == 0 || Main.tile[i, j].wall == Mods.paintOnlyWall) && WorldGen.paintWall(i, j, 0, broadCast: false, noEffect: true))
					{
						data.style = 1;
						data.toPlace = 0;
						painted = true;
						if (Main.netMode != 0)
						{
							try
							{
								tilesToBrush.Add(data);
							}
							catch
							{
							}
						}
						handledAlready = true;
					}
					if ((Mods.paintOnlyTile == -1 || Main.tile[i, j].type == Mods.paintOnlyTile) && WorldGen.paintCoatTile(i, j, 0, broadcast: false, noEffect: true))
					{
						data.style = 2;
						data.toPlace = 0;
						painted = true;
						if (Main.netMode != 0)
						{
							try
							{
								tilesToBrush.Add(data);
							}
							catch
							{
							}
						}
						handledAlready = true;
					}
					if ((Mods.paintOnlyWall != 0 && Main.tile[i, j].wall != Mods.paintOnlyWall) || !WorldGen.paintCoatWall(i, j, 0, broadcast: false, noEffect: true))
					{
						break;
					}
					data.style = 3;
					data.toPlace = 0;
					painted = true;
					if (Main.netMode != 0)
					{
						try
						{
							tilesToBrush.Add(data);
						}
						catch
						{
						}
					}
					handledAlready = true;
					break;
				}
				if (!painted)
				{
					continue;
				}
				Main.Map.UpdateLighting(i, j, byte.MaxValue);
				Main.refreshMap = true;
				if (Main.netMode != 0 && !handledAlready)
				{
					try
					{
						tilesToBrush.Add(data);
					}
					catch
					{
					}
				}
			}
		}
	}

	public static byte GetRainbowPaintColor(int tileX, int tileY)
	{
		double worldCenterX = (double)Main.maxTilesX / 2.0;
		double worldCenterY = (double)Main.maxTilesY / 2.0;
		double offsetX = (double)tileX - worldCenterX;
		double offsetY = (double)tileY - worldCenterY;
		switch (Mods.rainbowPaintType)
		{
		case 1:
		{
			int patternIndex = (tileX % 52 + tileY % 52) % 26;
			if (patternIndex > 12)
			{
				patternIndex = 12 - (patternIndex - 12);
			}
			patternIndex = Math.Clamp(patternIndex, 1, 12);
			return (byte)(patternIndex + (Mods.rainbowPaintDeep ? 12 : 0));
		}
		case 2:
		{
			int patternIndex2 = tileX % 52 % 26;
			if (patternIndex2 > 12)
			{
				patternIndex2 = 12 - (patternIndex2 - 12);
			}
			patternIndex2 = Math.Clamp(patternIndex2, 1, 12);
			return (byte)(patternIndex2 + (Mods.rainbowPaintDeep ? 12 : 0));
		}
		case 3:
		{
			int patternIndex3 = tileY % 52 % 26;
			if (patternIndex3 > 12)
			{
				patternIndex3 = 12 - (patternIndex3 - 12);
			}
			patternIndex3 = Math.Clamp(patternIndex3, 1, 12);
			return (byte)(patternIndex3 + (Mods.rainbowPaintDeep ? 12 : 0));
		}
		case 4:
		{
			double angle2 = Math.Atan2(offsetY, offsetX);
			return (byte)Math.Clamp((int)((Math.Sin(offsetX * 0.08 + angle2 * 7.0) + Math.Cos(offsetX * 0.08 - angle2 * 5.0) + 2.0) / 4.0 * 30.0 + 1.0), 1, 31);
		}
		case 5:
		{
			double num2 = Math.Atan2(offsetY, offsetX);
			double distance2 = Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
			double swirlValue = Math.Sin(num2 * 6.0 + distance2 * 0.1);
			return (byte)(1 + (int)((swirlValue + 1.0) * 15.0) % 30);
		}
		case 6:
		{
			int absX = Math.Abs((int)offsetX);
			int absY = Math.Abs((int)offsetY);
			return (byte)(1 + ((absX < 2 || absY < 2) ? ((tileX + tileY) % 30) : ((absX + absY) / 5 % 30)));
		}
		case 7:
		{
			double mixValue = (Math.Sin((double)tileX * 0.1) + Math.Cos((double)tileY * 0.1) + 2.0) / 4.0;
			return (byte)(1 + (int)(mixValue * 29.0) % 30);
		}
		case 8:
		{
			double radius = Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
			double scale = 1.0 + Math.Sin(radius * 0.1) * 0.5;
			int gridX = (int)Math.Round(offsetX * scale / 8.0);
			int gridY = (int)Math.Round(offsetY * scale / 8.0);
			return (byte)(1 + ((gridX * 3 + gridY * 5) % 30 + 30) % 30);
		}
		case 9:
		{
			int waveX = (int)(Math.Sin(offsetY * 0.2) * 3.0);
			int waveY = (int)(Math.Sin(offsetX * 0.2) * 3.0);
			int idx = ((int)((offsetX + (double)waveX) / 5.0) + (int)((offsetY + (double)waveY) / 5.0)) % 30;
			return (byte)(1 + (idx + 30) % 30);
		}
		case 10:
		{
			double val = (Math.Sin(((double)tileY - worldCenterX) * 0.3) + Math.Cos(((double)tileX - worldCenterY) * 0.5) + 2.0) / 4.0;
			return (byte)(1 + (int)(val * 29.0));
		}
		case 11:
		{
			int bandX = (int)Math.Floor((double)(tileX - tileY) / 8.0);
			int bandY = (int)Math.Floor((double)(tileX + tileY) / 8.0);
			return (byte)(1 + ((bandX + bandY) % 30 + 30) % 30);
		}
		case 12:
		{
			double scaledX = offsetX / 100.0;
			double scaledY = offsetY / 100.0;
			double distance = Math.Sqrt(scaledX * scaledX + scaledY * scaledY);
			double angle = Math.Atan2(scaledY, scaledX) * 4.0;
			double patternValue = (Math.Sin(Math.Log(1.0 + distance) * 20.0) + Math.Cos(angle * 3.0) + 2.0) / 4.0;
			return (byte)(1 + (int)(patternValue * 29.0) % 30);
		}
		case 13:
		{
			double value = 0.0;
			double amplitude = 1.0;
			double frequency = 0.01;
			for (int j = 0; j < 4; j++)
			{
				value += amplitude * Math.Sin(frequency * (double)tileX * (double)(j + 1) + Math.Cos(frequency * (double)tileY * (double)(j + 1)));
				amplitude *= 0.5;
				frequency *= 2.0;
			}
			value = (value + 1.0) / 2.0;
			return (byte)(1 + (int)(value * 29.0) % 30);
		}
		case 14:
		{
			double lat = Math.Sin((double)tileX * 0.2) * Math.Cos((double)tileY * 0.2);
			lat += Math.Sin((double)(tileX + tileY) * 0.15) * Math.Cos((double)(tileX - tileY) * 0.15) * 0.7;
			lat += Math.Sin(Math.Sqrt(tileX * tileX + tileY * tileY) * 0.1) * 0.5;
			double chaos = (double)((tileX * 127 + tileY * 251) % 100) * 0.0002 - 0.1;
			lat += chaos;
			return (byte)(1 + (int)((lat + 1.0) * 14.5) % 30);
		}
		case 15:
		{
			double ax = (double)tileX * 0.01;
			double ay = (double)tileY * 0.01;
			for (int i = 0; i < 10; i++)
			{
				double num = Math.Sin(ay * 1.5) - Math.Cos(ax * 2.3);
				ay = Math.Sin(ax * 1.7) - Math.Cos(ay * 2.1);
				ax = num;
			}
			return (byte)(1 + (int)((ax + ay + 2.0) * 7.5) % 30);
		}
		case 16:
		{
			int localX = tileX % 5;
			int localY = tileY % 6;
			if (localX == 4 || localY == 5)
			{
				return 0;
			}
			int charIndexX = tileX / 5;
			int charIndexY = tileY / 6;
			int baseColor = (Mods.rainbowPaintDeep ? 12 : 0);
			int bodyColor = 1 + (charIndexX * 7 + charIndexY * 13) % 29;
			if (bodyColor >= 8)
			{
				bodyColor++;
			}
			bodyColor = Math.Clamp(bodyColor + baseColor, 1, 30);
			return AmongUsPattern[localY, localX] switch
			{
				-1 => 0, 
				0 => 8, 
				_ => (byte)bodyColor, 
			};
		}
		default:
			return 0;
		}
	}

	public static async void TileExploitBrushV2(Point mouseToTile, int size, ushort tileType = 10, short frameX = 36, short frameY = 612, bool isActive = true)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		for (int i = mouseToTile.X - size; i <= mouseToTile.X + size; i += 3)
		{
			for (int j = mouseToTile.Y + size; j >= mouseToTile.Y - size; j -= 3)
			{
				Mods.FakeNetMessage13(new Vector2((float)i * 16f, (float)j * 16f), 58);
				TileSyncExploit(new Point(i, j), tileType, frameX, frameY, isActive);
				NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
				while (Mods.quedPackets > Mods.tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS)
				{
					await Task.Delay(500);
				}
			}
		}
	}

	public static void TileSyncExploit(Point hatRackPos, ushort tileType = 10, short frameX = 36, short frameY = 612, bool tileActive = true)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		bool dothis = false;
		try
		{
			if (!WorldGen.InWorld(hatRackPos.X - 1, hatRackPos.Y - 3))
			{
				return;
			}
			for (int i = hatRackPos.X - 1; i <= hatRackPos.X + 1; i++)
			{
				for (int j = hatRackPos.Y - 3; j <= hatRackPos.Y; j++)
				{
					if (!WorldGen.InWorld(i, j))
					{
						continue;
					}
					Tile tile = Main.tile[i, j];
					if (i == hatRackPos.X - 1 && j == hatRackPos.Y - 3)
					{
						if (tile.type == 579 && tile.frameX == ((!Mods.onTshockServer) ? 927 : 0) && tile.frameY != 0)
						{
						}
					}
					else if (tile.type != tileType || tile.frameX != frameX || tile.frameY != frameY || tile.active() != tileActive)
					{
						dothis = true;
						break;
					}
				}
				if (dothis)
				{
					break;
				}
			}
			if (!dothis)
			{
				return;
			}
			for (int k = hatRackPos.X - 1; k <= hatRackPos.X + 1; k++)
			{
				for (int l = hatRackPos.Y - 3; l <= hatRackPos.Y; l++)
				{
					Tile tile2 = Main.tile[k, l];
					if (k != hatRackPos.X - 1 || l != hatRackPos.Y - 3)
					{
						tile2.active(tileActive);
						tile2.type = tileType;
						tile2.frameX = frameX;
						tile2.frameY = frameY;
					}
					else
					{
						tile2.active(active: true);
						tile2.type = 597;
						tile2.frameX = (short)((!Mods.onTshockServer) ? 927 : 0);
						tile2.frameY = 0;
					}
				}
			}
			NetMessage.SendTileSquare(Main.myPlayer, hatRackPos.X - 1, hatRackPos.Y - 3, 3, 4);
			NetMessage.SendData(17, -1, -1, null, 1, hatRackPos.X - 1, hatRackPos.Y - 3, 504f);
			NetMessage.SendData(17, -1, -1, null, 4, hatRackPos.X - 1, hatRackPos.Y - 3);
			Mods.noKillSoundTime = 5;
			if (Main.netMode == 0)
			{
				WorldGen.RangeFrame(hatRackPos.X - 1, hatRackPos.Y - 3, hatRackPos.X + 1, hatRackPos.Y, resetFrame: true);
			}
		}
		catch
		{
		}
	}

	public static async void HandleBrushNetMessagesV3()
	{
		Mods.handlingPackets = true;
		try
		{
			if (tilesToBrush.Count > 0 && ((Main.netMode == 1 && Netplay.Connection != null && Netplay.Connection.State == 10) || Main.netMode != 1) && ((Main.netMode == 2 && !Main.dedServ) ? 2000 : Mods.tilePacketRate) > 0)
			{
				int sentTilesAndWalls = 0;
				int sentErases = 0;
				int sentLiquids = 0;
				int sentPaint = 0;
				while (tilesToBrush.Count > 0)
				{
					BrushData data = tilesToBrush[0];
					if (Main.gameMenu)
					{
						Mods.handlingPackets = false;
						tilesToBrush.Clear();
						return;
					}
					while (((Main.netMode == 2 && !Main.dedServ) ? 2000 : Mods.tilePacketRate) < 1)
					{
						if (Main.gameMenu)
						{
							Mods.handlingPackets = false;
							tilesToBrush.Clear();
							return;
						}
					}
					if (!Main.sectionManager.SectionLoaded(Netplay.GetSectionX(data.x), Netplay.GetSectionY(data.y)))
					{
						Main.NewText($"Section at {data.x}, {data.y} isn't loaded. Attempting to load...", Mods.zazasFavoriteColor);
						Mods.LoadChunk(Netplay.GetSectionX(data.x), Netplay.GetSectionY(data.y));
						await Task.Delay(500);
					}
					data.Handle(ref sentTilesAndWalls, ref sentErases, ref sentLiquids, ref sentPaint);
					switch (data.brushType)
					{
					case 0:
					case 2:
						while (Mods.tilePacketMaxMode && (sentTilesAndWalls > Mods.tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
						{
							sentTilesAndWalls = 0;
							await Task.Delay(500);
						}
						if (!Mods.tilePacketMaxMode && (sentTilesAndWalls % Mods.tilePacketRate == 0 || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
						{
							sentTilesAndWalls = 0;
							await Task.Delay(100);
						}
						break;
					case 1:
						while (Mods.tilePacketMaxMode && (sentErases > Mods.erasePacketRate || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
						{
							sentErases = 0;
							await Task.Delay(500);
						}
						if (!Mods.tilePacketMaxMode && (sentErases % Mods.erasePacketRate == 0 || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
						{
							sentErases = 0;
							await Task.Delay(100);
						}
						break;
					case 3:
						while (Mods.tilePacketMaxMode && (sentLiquids > Mods.liquidPacketRate || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
						{
							sentLiquids = 0;
							await Task.Delay(500);
						}
						if (!Mods.tilePacketMaxMode && (sentLiquids % Mods.liquidPacketRate == 0 || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
						{
							sentLiquids = 0;
							await Task.Delay(100);
						}
						break;
					case 4:
						while (Mods.tilePacketMaxMode && (sentPaint > Mods.paintPacketRate || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
						{
							sentPaint = 0;
							await Task.Delay(500);
						}
						if (!Mods.tilePacketMaxMode && (sentPaint % Mods.paintPacketRate == 0 || Netplay.Connection.TimeOutTimer > 60 || Mods.IsLowFPS))
						{
							sentPaint = 0;
							await Task.Delay(100);
						}
						break;
					}
					try
					{
						tilesToBrush.RemoveUnorderedAt(0);
					}
					catch
					{
					}
				}
			}
		}
		catch (Exception ex)
		{
			Main.NewText($"Handling Brush Packets Error!\n{ex}", Mods.errorColor);
			tilesToBrush.Clear();
			Mods.LogException(ex);
		}
		Mods.handlingPackets = false;
		NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
		NetMessage.SendData(5, -1, -1, null, Main.myPlayer, PlayerItemSlotID.InventoryMouseItem, (int)Main.LocalPlayer.inventory[PlayerItemSlotID.InventoryMouseItem].prefix);
	}

	public static void Draw(SpriteBatch spriteBatch)
	{
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu || Mods.selectionMode || Mods.pasteMode)
		{
			return;
		}
		if (!Main.mapFullscreen)
		{
			PlayerInput.SetZoom_World();
		}
		Point point = Main.MouseWorld.ToTileCoordinates();
		Color color = new Color(0.9f, 0.9f, 0.9f, 0f) * 0.3f;
		try
		{
			if (texture == null)
			{
				texture = TextureAssets.MagicPixel.Value;
			}
			DrawBrushType(spriteBatch, Mods.IsInTileBrush, point, Mods.tileBrushSize, "Tile Brush", Main.LocalPlayer.HeldItem.Name, texture, color, Mods.brushCircle);
			DrawBrushType(spriteBatch, Mods.IsInEraseBrush, point, Mods.eraseBrushSize, "Erase " + Mods.EraseBrushString(), null, texture, color, Mods.brushCircle);
			DrawBrushType(spriteBatch, Mods.IsInWallBrush, point, Mods.wallBrushSize, "Wall Brush", Main.LocalPlayer.HeldItem.Name, texture, color, Mods.brushCircle);
			DrawBrushType(spriteBatch, Mods.IsInLiquidBrush, point, Mods.liquidBrushSize, "Liquid Brush", Main.LocalPlayer.HeldItem.Name, texture, color, Mods.brushCircle);
			if (Mods.IsInTilePaintBrush)
			{
				string paintText = GetPaintBrushName();
				DrawBrushType(spriteBatch, isActive: true, point, Mods.tilePaintBrushSize, "Super Paint " + Mods.BrushOrRollerOrSscraper(), paintText, texture, color, Mods.brushCircle);
			}
		}
		catch (Exception ex)
		{
			Mods.Instance.DisableBrushes();
			Main.NewText($"Drawing Brushes error!\n{ex}", Mods.errorColor);
			Mods.LogException(ex);
		}
	}

	private static string GetPaintBrushName()
	{
		if (Mods.rainbowPaintType > 0)
		{
			return "Rainbow Paint";
		}
		Item item = Main.LocalPlayer.FindPaintOrCoating();
		if (item == null || Mods.HoldingPaintScraper)
		{
			if (!Mods.HoldingPaintScraper)
			{
				return "No Paint...";
			}
			return Main.LocalPlayer.HeldItem.Name;
		}
		return item.Name;
	}

	private static void DrawBrushType(SpriteBatch spriteBatch, bool isActive, Point center, int brushSize, string brushName, string subName, Texture2D texture, Color color, bool useCircle = false)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		if (isActive && Main.hasFocus)
		{
			if (!Main.LocalPlayer.mouseInterface || Main.mapFullscreen)
			{
				string displayText = (string.IsNullOrEmpty(subName) ? brushName : (brushName + " (" + subName + ")"));
				Main.instance.MouseText(displayText, 0, 0);
			}
			if (Main.mapFullscreen)
			{
				DrawBrushOnMap(spriteBatch, center, brushSize, color, useCircle);
			}
			else
			{
				DrawBrushOnScreen(spriteBatch, center, brushSize, texture, color, useCircle);
			}
		}
	}

	private static void DrawBrushOnScreen(SpriteBatch spriteBatch, Point center, int brushSize, Texture2D texture, Color color, bool useCircle = false)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		if (useCircle)
		{
			brushSize++;
		}
		int brushSizeSq = brushSize * brushSize;
		bool invert = Main.player[Main.myPlayer].gravDir == -1f;
		for (int x = center.X - brushSize; x <= center.X + brushSize; x++)
		{
			int num = x - center.X;
			int dxSq = num * num;
			for (int y = center.Y - brushSize; y <= center.Y + brushSize; y++)
			{
				int num2 = y - center.Y;
				int dySq = num2 * num2;
				if ((!useCircle || dxSq + dySq < brushSizeSq) && Mods.WithinScreenBounds(x, y) && TileValidForBrush(center, x, y, brushSize))
				{
					Vector2 pos = new Vector2((float)x * 16f - 1f, (float)y * 16f - 1f) - Main.screenPosition;
					if (invert)
					{
						pos.Y = (float)Main.screenHeight - pos.Y - 18f;
					}
					spriteBatch.Draw(texture, pos, (Rectangle?)BrushSrcRect, color);
				}
			}
		}
	}

	private static void DrawBrushOnMap(SpriteBatch spriteBatch, Point center, int brushSize, Color color, bool useCircle = false)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		if (!Mods.GetMapCoordinatesModV2(Main.mouseX, Main.mouseY, 0, out var mapCoords) || mapCoords.X < 0 || mapCoords.Y < 0)
		{
			return;
		}
		int realSize = brushSize;
		if (useCircle)
		{
			brushSize++;
		}
		int brushSizeSq = brushSize * brushSize;
		Rectangle screenRect = default(Rectangle);
		((Rectangle)(ref screenRect))._002Ector(0, 0, Main.screenWidth, Main.screenHeight);
		Rectangle rect = default(Rectangle);
		Rectangle result = default(Rectangle);
		for (int dx = -realSize; dx <= realSize; dx++)
		{
			int num = mapCoords.X + dx;
			int halfHeight = ((!useCircle) ? brushSize : ((int)Math.Sqrt(Math.Max(0, brushSizeSq - 1 - dx * dx))));
			Mods.GetMapCoordinatesModV2(num, mapCoords.Y - halfHeight, 1, out var columnTop);
			Mods.GetMapCoordinatesModV2(num + 1, mapCoords.Y + halfHeight + 1, 1, out var columnBottom);
			((Rectangle)(ref rect))._002Ector(columnTop.X, columnTop.Y, columnBottom.X - columnTop.X, columnBottom.Y - columnTop.Y);
			if (((Rectangle)(ref rect)).Intersects(screenRect))
			{
				Rectangle.Intersect(ref screenRect, ref rect, ref result);
				((Rectangle)(ref result)).Offset(-screenRect.X, -screenRect.Y);
				spriteBatch.Draw(texture, result, (Rectangle?)new Rectangle(0, 0, 1, 1), color);
			}
		}
	}

	static Brushes()
	{
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		tilesToBrush = new List<BrushData>();
		Patterns = new Dictionary<int, Func<int, int, bool>>
		{
			{
				0,
				(int i, int j) => true
			},
			{
				1,
				(int i, int j) => int.IsEvenInteger(i + j)
			},
			{
				2,
				(int i, int j) => !int.IsEvenInteger(i + j)
			},
			{
				3,
				(int i, int j) => int.IsEvenInteger(i / 2 + j)
			},
			{
				4,
				(int i, int j) => !int.IsEvenInteger(i / 2 + j)
			},
			{
				5,
				(int i, int j) => int.IsEvenInteger(i / 3 + j)
			},
			{
				6,
				(int i, int j) => !int.IsEvenInteger(i / 3 + j)
			},
			{
				7,
				(int i, int j) => int.IsEvenInteger(j / 2 + i)
			},
			{
				8,
				(int i, int j) => !int.IsEvenInteger(j / 2 + i)
			},
			{
				9,
				(int i, int j) => int.IsEvenInteger(j / 3 + i)
			},
			{
				10,
				(int i, int j) => !int.IsEvenInteger(j / 3 + i)
			},
			{
				11,
				(int i, int j) => (i + j * 2) % 3 == 0
			},
			{
				12,
				(int i, int j) => (i + j * 2) % 3 != 0
			},
			{
				13,
				(int i, int j) => (i - j * 2) % 3 == 0
			},
			{
				14,
				(int i, int j) => (i - j * 2) % 3 != 0
			},
			{
				15,
				(int i, int j) => (i + j * 2) % 4 == 0
			},
			{
				16,
				(int i, int j) => (i + j * 2) % 4 != 0
			},
			{
				17,
				(int i, int j) => (i + j * 3) % 4 == 0
			},
			{
				18,
				(int i, int j) => (i + j * 3) % 4 != 0
			},
			{
				19,
				(int i, int j) => (i - j * 3) % 4 == 0
			},
			{
				20,
				(int i, int j) => (i - j * 3) % 4 != 0
			},
			{
				21,
				(int i, int j) => (i + j * 3) % 5 == 0
			},
			{
				22,
				(int i, int j) => (i + j * 3) % 5 != 0
			},
			{
				23,
				(int i, int j) => (i - j * 3) % 5 == 0
			},
			{
				24,
				(int i, int j) => (i - j * 3) % 5 != 0
			},
			{
				25,
				(int i, int j) => !int.IsEvenInteger(i * 2 + j)
			},
			{
				26,
				(int i, int j) => int.IsEvenInteger(i * 2 + j)
			},
			{
				27,
				(int i, int j) => int.IsEvenInteger(j * 2 + i)
			},
			{
				28,
				(int i, int j) => !int.IsEvenInteger(j * 2 + i)
			},
			{
				29,
				(int i, int j) => int.IsEvenInteger(i * 2 + j) || int.IsEvenInteger(j * 2 + i)
			},
			{
				30,
				(int i, int j) => !int.IsEvenInteger(i * 2 + j) && !int.IsEvenInteger(j * 2 + i)
			},
			{
				31,
				(int i, int j) => (i + j) % 4 == 0 || (i + j * 3) % 4 == 0
			},
			{
				32,
				(int i, int j) => (i + j) % 4 != 0 && (i + j * 3) % 4 != 0
			},
			{
				33,
				(int i, int j) => (i + j * 2) % 4 == 0 || (i + j * 3) % 5 == 0
			},
			{
				34,
				(int i, int j) => (i + j * 2) % 4 != 0 && (i + j * 3) % 5 != 0
			},
			{
				35,
				(int i, int j) => (i - j * 2) % 4 == 0 || (i - j * 3) % 5 == 0
			},
			{
				36,
				(int i, int j) => (i - j * 2) % 4 != 0 && (i - j * 3) % 5 != 0
			},
			{
				37,
				(int i, int j) => (i + j * 2) % 4 == 0 || (i + j * 3) % 5 == 0 || (i - j * 3) % 5 == 0
			},
			{
				38,
				(int i, int j) => (i + j * 2) % 4 != 0 && (i + j * 3) % 5 != 0 && (i - j * 3) % 5 != 0
			},
			{
				39,
				(int i, int j) => (i + j * 3) % 5 == 0 || (i - j * 3) % 5 == 0
			},
			{
				40,
				(int i, int j) => (i + j * 3) % 5 != 0 && (i - j * 3) % 5 != 0
			},
			{
				41,
				(int i, int j) => ((i / 2 + j / 3) % 2 == 0) ^ ((j / 2 + i / 3) % 2 == 0)
			},
			{
				42,
				(int i, int j) => (i * i + j * j + i * j) % 11 < 3
			},
			{
				43,
				(int i, int j) => (i * i - j * j) / 25 % 3 == 0
			},
			{
				44,
				(int i, int j) => (int)((Math.Sin((double)i * 0.2) + Math.Cos((double)j * 0.3)) * 10.0) % 7 == 0
			},
			{
				45,
				delegate(int i, int j)
				{
					int num = i ^ j;
					int num2 = 0;
					while (num != 0)
					{
						num2 += num & 1;
						num >>= 1;
					}
					return num2 % 2 != 0;
				}
			},
			{
				46,
				(int i, int j) => i * j % 17 < 5
			},
			{
				47,
				(int i, int j) => (i * i + j * j + i * j) / 30 % 5 == 0
			},
			{
				48,
				(int i, int j) => (i * j - i - j) % 13 < 4
			},
			{
				49,
				(int i, int j) => (int)(Math.Sin((double)i * 0.3) * Math.Cos((double)j * 0.3) * 10.0) % 6 == 0
			},
			{
				50,
				(int i, int j) => (i * i - j * j + i * j) / 15 % 3 == 0
			},
			{
				51,
				(int i, int j) => (i * j + i - j) % 11 < 3
			},
			{
				52,
				(int i, int j) => (i * i + j * j) / 10 % 4 == 0
			},
			{
				53,
				(int i, int j) => (int)(Math.Sin(Math.Sqrt(i * i + j * j) * 0.15) * Math.Cos((double)(i - j) * 0.2) * 10.0) % 6 == 0
			},
			{
				54,
				(int i, int j) => (int)(Math.Sin((double)(i * i - j * j) * 0.02) + Math.Cos((double)(i * j) * 0.01 + Math.Sqrt(i * i + j * j) * 0.05)) % 7 == 0
			},
			{
				55,
				(int i, int j) => (int)(Math.Cos(Math.Sqrt(i * i + j * j) * 0.1) * Math.Sin((double)(i + j) * 0.2) * 8.0) % 6 == 0
			}
		};
		AmongUsPattern = new int[6, 5]
		{
			{ -1, 1, 1, 1, -1 },
			{ 1, 1, 0, 0, -1 },
			{ 1, 1, 1, 1, -1 },
			{ -1, 1, 1, 1, -1 },
			{ -1, 1, -1, 1, -1 },
			{ -1, -1, -1, -1, -1 }
		};
		BrushSrcRect = new Rectangle(0, 0, 18, 18);
	}
}
