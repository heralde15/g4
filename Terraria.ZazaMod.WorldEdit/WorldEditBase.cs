using Terraria.ID;
using Terraria.ObjectData;

namespace Terraria.ZazaMod.WorldEdit;

public class WorldEditBase
{
	public static TileObjectData PlaceDirectObject(int i, int j, int tileType, out int xCoord, out int yCoord, out TileObject objectData, int placeStyle = 0, int direction = 1, int alternate = -1, int forcedRandom = -1)
	{
		xCoord = -1;
		yCoord = -1;
		objectData = default(TileObject);
		if (!TileObjectData.CustomPlace(tileType, placeStyle))
		{
			Main.NewText($"PlaceDirectObject was called for a non-tile object. ({TileID.Search.GetName(tileType)}, {tileType})");
			return null;
		}
		if (WorldGen.InWorld(i, j, 10))
		{
			TileObjectData tileObjectData = TileObjectData.GetTileData(tileType, placeStyle);
			if (!TileObject.CanPlace(i, j, tileType, placeStyle, direction, out objectData, onlyCheck: false, (forcedRandom != -1) ? new int?(forcedRandom) : ((int?)null), Main.netMode == 0 && Mods.placeAnywhere))
			{
				return null;
			}
			if (forcedRandom != -1)
			{
				objectData.random = forcedRandom;
			}
			ushort num = (ushort)objectData.type;
			int num2 = 0;
			int num3 = 0;
			if (tileObjectData.AlternatesCount != 0)
			{
				num3 = tileObjectData.AlternatesCount;
				tileObjectData = TileObjectData.GetTileData(tileType, placeStyle, (alternate != -1) ? alternate : ((direction != -1) ? 1 : 0));
				objectData.alternate = ((alternate != -1) ? alternate : ((direction != -1) ? 1 : 0));
			}
			else
			{
				tileObjectData = TileObjectData.GetTileData(tileType, placeStyle);
			}
			int num4 = tileObjectData.CalculatePlacementStyle(objectData.style, objectData.alternate, objectData.random);
			int num5 = 0;
			xCoord = objectData.xCoord;
			yCoord = objectData.yCoord;
			for (int k = 0; k < tileObjectData.Width; k++)
			{
				_ = tileObjectData.CoordinateWidth;
				_ = tileObjectData.CoordinatePadding;
				for (int l = 0; l < tileObjectData.Height; l++)
				{
					if (Main.tile[xCoord + k, yCoord + l].active() && Main.tile[xCoord + k, yCoord + l].type == num)
					{
						return null;
					}
				}
			}
			if (tileObjectData.StyleWrapLimit > 0)
			{
				num5 = num4 / tileObjectData.StyleWrapLimit * tileObjectData.StyleLineSkip;
				num4 %= tileObjectData.StyleWrapLimit;
			}
			if (tileObjectData.StyleHorizontal)
			{
				num2 = tileObjectData.CoordinateFullWidth * num4;
				num3 = tileObjectData.CoordinateFullHeight * num5;
			}
			else
			{
				num2 = tileObjectData.CoordinateFullWidth * num5;
				num3 = tileObjectData.CoordinateFullHeight * num4;
			}
			for (int m = 0; m < tileObjectData.Width; m++)
			{
				int num8 = num2 + m * (tileObjectData.CoordinateWidth + tileObjectData.CoordinatePadding);
				int num9 = num3;
				for (int n = 0; n < tileObjectData.Height; n++)
				{
					Tile tileSafely2 = Framing.GetTileSafely(xCoord + m, yCoord + n);
					if (tileSafely2.active() && tileSafely2.type == num)
					{
						return null;
					}
					tileSafely2.active(active: true);
					tileSafely2.frameX = (short)num8;
					tileSafely2.frameY = (short)num9;
					tileSafely2.type = num;
					num9 += tileObjectData.CoordinateHeights[n] + tileObjectData.CoordinatePadding;
				}
			}
			if (Main.netMode == 0)
			{
				TileObjectData.CallPostPlacementPlayerHook(i, j, objectData.type, objectData.style, direction, objectData.alternate, objectData);
			}
			return tileObjectData;
		}
		return null;
	}
}
