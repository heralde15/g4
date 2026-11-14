using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent;
using Terraria.GameContent.Biomes;
using Terraria.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.ZazaMod.LegacyWorldGen;

internal class WorldGen_1
{
	private static bool mergeUp;

	private static bool mergeDown;

	private static bool mergeLeft;

	private static bool mergeRight;

	private static bool destroyObject;

	public static int lavaLine;

	public static int waterLine;

	public static int bestX;

	public static int bestY;

	public static int hiScore;

	public static int dungeonX;

	public static int dungeonY;

	public static Vector2 lastDungeonHall;

	public static int maxDRooms;

	public static int numDRooms;

	public static int[] dRoomX;

	public static int[] dRoomY;

	public static int[] dRoomSize;

	private static bool[] dRoomTreasure;

	private static int[] dRoomL;

	private static int[] dRoomR;

	private static int[] dRoomT;

	private static int[] dRoomB;

	private static int numDDoors;

	private static int[] DDoorX;

	private static int[] DDoorY;

	private static int[] DDoorPos;

	private static int numDPlats;

	private static int[] DPlatX;

	private static int[] DPlatY;

	private static int[] JChestX;

	private static int[] JChestY;

	private static int numJChests;

	public static int dEnteranceX;

	public static bool dSurface;

	private static double dxStrength1;

	private static double dyStrength1;

	private static double dxStrength2;

	private static double dyStrength2;

	private static int dMinX;

	private static int dMaxX;

	private static int dMinY;

	private static int dMaxY;

	private static int numIslandHouses;

	private static int houseCount;

	private static int[] fihX;

	private static int[] fihY;

	private static int numMCaves;

	private static int[] mCaveX;

	private static int[] mCaveY;

	private static int JungleX;

	private static int hellChest;

	private static bool mudWall;

	private static int lastMaxTilesX;

	private static int lastMaxTilesY;

	public static void generateWorld(int seed, GenerationProgress customProgressObject = null)
	{
		WorldGen.remixWorldGen = false;
		WorldGen.tenthAnniversaryWorldGen = false;
		WorldGen.drunkWorldGen = false;
		WorldGen.drunkWorldGenText = false;
		Main.afterPartyOfDoom = false;
		Main.zenithWorld = false;
		GenVars.configuration = WorldGenConfiguration.FromEmbeddedPath("Terraria.GameContent.WorldBuilding.Configuration.json");
		WorldGen.Hooks.ProcessWorldGenConfig(ref GenVars.configuration);
		WorldGen._lastSeed = seed;
		WorldGen._generator = new WorldGenerator(seed, GenVars.configuration);
		WorldGen._genRand = new UnifiedRandom(seed);
		lastMaxTilesX = Main.maxTilesX;
		lastMaxTilesY = Main.maxTilesY;
		int num = 0;
		int num2 = 0;
		double num3 = (double)Main.maxTilesY * 0.3;
		num3 *= (double)WorldGen.genRand.Next(90, 110) * 0.005;
		double num4 = num3 + (double)Main.maxTilesY * 0.2;
		num4 *= (double)WorldGen.genRand.Next(90, 110) * 0.01;
		double num5 = num3;
		double num6 = num3;
		double num7 = num4;
		double num8 = num4;
		int num9 = 0;
		int num1086 = 86400;
		Main.slimeRainTime = -WorldGen.genRand.Next(num1086 * 2, num1086 * 3);
		Main.cloudBGActive = -WorldGen.genRand.Next(8640, 86400);
		WorldGen.SavedOreTiers.Copper = 7;
		WorldGen.SavedOreTiers.Iron = 6;
		WorldGen.SavedOreTiers.Silver = 9;
		WorldGen.SavedOreTiers.Gold = 8;
		GenVars.structures = new StructureMap();
		WorldGen.crimson = false;
		Main.worldID = WorldGen.genRand.Next(int.MaxValue);
		WorldGen.RandomizeTreeStyle();
		WorldGen.RandomizeCaveBackgrounds();
		WorldGen.RandomizeBackgrounds(WorldGen.genRand);
		WorldGen.RandomizeMoonState(WorldGen.genRand);
		WorldGen.TreeTops.CopyExistingWorldInfoForWorldGeneration();
		WorldGen.noTileActions = true;
		DDoorX = new int[320];
		DDoorY = new int[320];
		DDoorPos = new int[320];
		DPlatX = new int[320];
		DPlatY = new int[320];
		JChestX = new int[300];
		JChestY = new int[300];
		WorldGen.AddGenerationPass("Shaping Classic World", delegate(GenerationProgress progress, GameConfiguration passConfig)
		{
			progress.Message = "Shaping Classic World...";
			num9 = ((WorldGen.genRand.Next(2) != 0) ? 1 : (-1));
			GenVars.dungeonSide = num9;
			WorldGen.SetupStatueList();
			WorldGen.RandomizeWeather();
			Main.cloudAlpha = 0f;
			Main.maxRaining = 0f;
			Main.raining = false;
			GenVars.extraBastStatueCount = 0;
			GenVars.extraBastStatueCountMax = 2;
			Main.checkXMas();
			Main.checkHalloween();
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				float num1087 = (float)i / (float)Main.maxTilesX;
				Main.statusText = Lang.gen[0]?.ToString() + " " + (int)(num1087 * 100f + 1f) + "%";
				if (num3 < num5)
				{
					num5 = num3;
				}
				if (num3 > num6)
				{
					num6 = num3;
				}
				if (num4 < num7)
				{
					num7 = num4;
				}
				if (num4 > num8)
				{
					num8 = num4;
				}
				if (num2 <= 0)
				{
					num = WorldGen.genRand.Next(0, 5);
					num2 = WorldGen.genRand.Next(5, 40);
					if (num == 0)
					{
						num2 *= (int)((double)WorldGen.genRand.Next(5, 30) * 0.2);
					}
				}
				num2--;
				if (num == 0)
				{
					while (WorldGen.genRand.Next(0, 7) == 0)
					{
						num3 += WorldGen.genRand.Next(-1, 2);
					}
				}
				else if (num == 1)
				{
					while (WorldGen.genRand.Next(0, 4) == 0)
					{
						num3--;
					}
					while (WorldGen.genRand.Next(0, 10) == 0)
					{
						num3++;
					}
				}
				else if (num == 2)
				{
					while (WorldGen.genRand.Next(0, 4) == 0)
					{
						num3++;
					}
					while (WorldGen.genRand.Next(0, 10) == 0)
					{
						num3--;
					}
				}
				else if (num == 3)
				{
					while (WorldGen.genRand.Next(0, 2) == 0)
					{
						num3--;
					}
					while (WorldGen.genRand.Next(0, 6) == 0)
					{
						num3++;
					}
				}
				else if (num == 4)
				{
					while (WorldGen.genRand.Next(0, 2) == 0)
					{
						num3++;
					}
					while (WorldGen.genRand.Next(0, 5) == 0)
					{
						num3--;
					}
				}
				if (num3 < (double)Main.maxTilesY * 0.17)
				{
					num3 = (double)Main.maxTilesY * 0.17;
					num2 = 0;
				}
				else if (num3 > (double)Main.maxTilesY * 0.3)
				{
					num3 = (double)Main.maxTilesY * 0.3;
					num2 = 0;
				}
				if ((i < 275 || i > Main.maxTilesX - 275) && num3 > (double)Main.maxTilesY * 0.25)
				{
					num3 = (double)Main.maxTilesY * 0.25;
					num2 = 1;
				}
				while (WorldGen.genRand.Next(0, 3) == 0)
				{
					num4 += WorldGen.genRand.Next(-2, 3);
				}
				if (num4 < num3 + (double)Main.maxTilesY * 0.05)
				{
					num4++;
				}
				if (num4 > num3 + (double)Main.maxTilesY * 0.35)
				{
					num4--;
				}
				for (int j = 0; (double)j < num3; j++)
				{
					Main.tile[i, j].active(active: false);
					Main.tile[i, j].frameX = -1;
					Main.tile[i, j].frameY = -1;
				}
				for (int k = (int)num3; k < Main.maxTilesY; k++)
				{
					if ((double)k < num4)
					{
						Main.tile[i, k].active(active: true);
						Main.tile[i, k].type = 0;
						Main.tile[i, k].frameX = -1;
						Main.tile[i, k].frameY = -1;
					}
					else
					{
						Main.tile[i, k].active(active: true);
						Main.tile[i, k].type = 1;
						Main.tile[i, k].frameX = -1;
						Main.tile[i, k].frameY = -1;
					}
				}
			}
			progress.Set(0.33);
			Main.worldSurface = num6 + 25.0;
			Main.rockLayer = num8;
			double num1088 = (int)((Main.rockLayer - Main.worldSurface) / 6.0) * 6;
			Main.rockLayer = Main.worldSurface + num1088;
			waterLine = (int)(Main.rockLayer + (double)Main.maxTilesY) / 2;
			waterLine += WorldGen.genRand.Next(-100, 20);
			lavaLine = waterLine + WorldGen.genRand.Next(50, 80);
			int num1089 = 0;
			for (int l = 0; l < (int)((double)Main.maxTilesX * 0.0015); l++)
			{
				int[] array = new int[10];
				int[] array2 = new int[10];
				int num1090 = WorldGen.genRand.Next(450, Main.maxTilesX - 450);
				int m = 0;
				for (int n = 0; n < 10; n++)
				{
					for (; !Main.tile[num1090, m].HasTile; m++)
					{
					}
					array[n] = num1090;
					array2[n] = m - WorldGen.genRand.Next(11, 16);
					num1090 += WorldGen.genRand.Next(5, 11);
				}
				for (int num1091 = 0; num1091 < 10; num1091++)
				{
					TileRunner(array[num1091], array2[num1091], WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(6, 9), 0, addTile: true, -2f, -0.3f);
					TileRunner(array[num1091], array2[num1091], WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(6, 9), 0, addTile: true, 2f, -0.3f);
				}
			}
			Main.statusText = Lang.gen[1].Value;
			int num1092 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.0008), (int)((double)Main.maxTilesX * 0.0025));
			num1092 += 2;
			for (int num1093 = 0; num1093 < num1092; num1093++)
			{
				int num1094 = WorldGen.genRand.Next(Main.maxTilesX);
				while ((float)num1094 > (float)Main.maxTilesX * 0.4f && (float)num1094 < (float)Main.maxTilesX * 0.6f)
				{
					num1094 = WorldGen.genRand.Next(Main.maxTilesX);
				}
				int num1095 = WorldGen.genRand.Next(35, 90);
				if (num1093 == 1)
				{
					float num1096 = Main.maxTilesX / 4200;
					num1095 += (int)((float)WorldGen.genRand.Next(20, 40) * num1096);
				}
				if (WorldGen.genRand.Next(3) == 0)
				{
					num1095 *= 2;
				}
				if (num1093 == 1)
				{
					num1095 *= 2;
				}
				int num1097 = num1094 - num1095;
				num1095 = WorldGen.genRand.Next(35, 90);
				if (WorldGen.genRand.Next(3) == 0)
				{
					num1095 *= 2;
				}
				if (num1093 == 1)
				{
					num1095 *= 2;
				}
				int num1098 = num1094 + num1095;
				if (num1097 < 0)
				{
					num1097 = 0;
				}
				if (num1098 > Main.maxTilesX)
				{
					num1098 = Main.maxTilesX;
				}
				switch (num1093)
				{
				case 0:
					num1097 = 0;
					num1098 = WorldGen.genRand.Next(260, 300);
					if (num9 == 1)
					{
						num1098 += 40;
					}
					break;
				case 2:
					num1097 = Main.maxTilesX - WorldGen.genRand.Next(260, 300);
					num1098 = Main.maxTilesX;
					if (num9 == -1)
					{
						num1097 -= 40;
					}
					break;
				}
				int num1099 = WorldGen.genRand.Next(50, 100);
				for (int num1100 = num1097; num1100 < num1098; num1100++)
				{
					if (WorldGen.genRand.Next(2) == 0)
					{
						num1099 += WorldGen.genRand.Next(-1, 2);
						if (num1099 < 50)
						{
							num1099 = 50;
						}
						if (num1099 > 100)
						{
							num1099 = 100;
						}
					}
					for (int num1101 = 0; (double)num1101 < Main.worldSurface; num1101++)
					{
						if (Main.tile[num1100, num1101].HasTile)
						{
							int num1102 = num1099;
							if (num1100 - num1097 < num1102)
							{
								num1102 = num1100 - num1097;
							}
							if (num1098 - num1100 < num1102)
							{
								num1102 = num1098 - num1100;
							}
							num1102 += WorldGen.genRand.Next(5);
							for (int num1103 = num1101; num1103 < num1101 + num1102; num1103++)
							{
								if (num1100 > num1097 + WorldGen.genRand.Next(5) && num1100 < num1098 - WorldGen.genRand.Next(5))
								{
									Main.tile[num1100, num1103].type = 53;
								}
							}
							break;
						}
					}
				}
			}
			for (int num1104 = 0; num1104 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 8E-06); num1104++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)Main.worldSurface, (int)Main.rockLayer), WorldGen.genRand.Next(15, 70), WorldGen.genRand.Next(20, 130), 53);
			}
			numMCaves = 0;
			Main.statusText = Lang.gen[2].Value;
			for (int num1105 = 0; num1105 < (int)((double)Main.maxTilesX * 0.0008); num1105++)
			{
				int num1106 = 0;
				bool flag = false;
				bool flag2 = false;
				int num1107 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.25), (int)((double)Main.maxTilesX * 0.75));
				while (!flag2)
				{
					flag2 = true;
					while (num1107 > Main.maxTilesX / 2 - 100 && num1107 < Main.maxTilesX / 2 + 100)
					{
						num1107 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.25), (int)((double)Main.maxTilesX * 0.75));
					}
					for (int num1108 = 0; num1108 < numMCaves; num1108++)
					{
						if (num1107 > mCaveX[num1108] - 50 && num1107 < mCaveX[num1108] + 50)
						{
							num1106++;
							flag2 = false;
							break;
						}
					}
					if (num1106 >= 200)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					for (int num1109 = 0; (double)num1109 < Main.worldSurface; num1109++)
					{
						if (Main.tile[num1107, num1109].HasTile)
						{
							Mountinater(num1107, num1109);
							mCaveX[numMCaves] = num1107;
							mCaveY[numMCaves] = num1109;
							numMCaves++;
							break;
						}
					}
				}
			}
			if (true)
			{
				Main.statusText = Lang.gen[56].Value;
				int num1110 = WorldGen.genRand.Next(Main.maxTilesX);
				while ((float)num1110 < (float)Main.maxTilesX * 0.35f || (float)num1110 > (float)Main.maxTilesX * 0.65f)
				{
					num1110 = WorldGen.genRand.Next(Main.maxTilesX);
				}
				int num1111 = WorldGen.genRand.Next(35, 90);
				float num1112 = Main.maxTilesX / 4200;
				num1111 += (int)((float)WorldGen.genRand.Next(20, 40) * num1112);
				num1111 += (int)((float)WorldGen.genRand.Next(20, 40) * num1112);
				int num1113 = num1110 - num1111;
				num1111 = WorldGen.genRand.Next(35, 90);
				num1111 += (int)((float)WorldGen.genRand.Next(20, 40) * num1112);
				num1111 += (int)((float)WorldGen.genRand.Next(20, 40) * num1112);
				int num1114 = num1110 + num1111;
				if (num1113 < 0)
				{
					num1113 = 0;
				}
				if (num1114 > Main.maxTilesX)
				{
					num1114 = Main.maxTilesX;
				}
				int num1115 = WorldGen.genRand.Next(50, 100);
				for (int num1116 = num1113; num1116 < num1114; num1116++)
				{
					if (WorldGen.genRand.Next(2) == 0)
					{
						num1115 += WorldGen.genRand.Next(-1, 2);
						if (num1115 < 50)
						{
							num1115 = 50;
						}
						if (num1115 > 100)
						{
							num1115 = 100;
						}
					}
					for (int num1117 = 0; (double)num1117 < Main.worldSurface; num1117++)
					{
						if (Main.tile[num1116, num1117].HasTile)
						{
							int num1118 = num1115;
							if (num1116 - num1113 < num1118)
							{
								num1118 = num1116 - num1113;
							}
							if (num1114 - num1116 < num1118)
							{
								num1118 = num1114 - num1116;
							}
							num1118 += WorldGen.genRand.Next(5);
							for (int num1119 = num1117; num1119 < num1117 + num1118; num1119++)
							{
								if (num1116 > num1113 + WorldGen.genRand.Next(5) && num1116 < num1114 - WorldGen.genRand.Next(5))
								{
									Main.tile[num1116, num1119].type = 147;
								}
							}
							break;
						}
					}
				}
			}
			for (int num1120 = 1; num1120 < Main.maxTilesX - 1; num1120++)
			{
				float num1121 = (float)num1120 / (float)Main.maxTilesX;
				Main.statusText = Lang.gen[3]?.ToString() + " " + (int)(num1121 * 100f + 1f) + "%";
				bool flag3 = false;
				num1089 += WorldGen.genRand.Next(-1, 2);
				if (num1089 < 0)
				{
					num1089 = 0;
				}
				if (num1089 > 10)
				{
					num1089 = 10;
				}
				for (int num1122 = 0; (double)num1122 < Main.worldSurface + 10.0 && !((double)num1122 > Main.worldSurface + (double)num1089); num1122++)
				{
					if (flag3)
					{
						Main.tile[num1120, num1122].wall = 2;
					}
					if (Main.tile[num1120, num1122].HasTile && Main.tile[num1120 - 1, num1122].HasTile && Main.tile[num1120 + 1, num1122].HasTile && Main.tile[num1120, num1122 + 1].HasTile && Main.tile[num1120 - 1, num1122 + 1].HasTile && Main.tile[num1120 + 1, num1122 + 1].HasTile)
					{
						flag3 = true;
					}
				}
			}
			Main.statusText = Lang.gen[4].Value;
			for (int num1123 = 0; num1123 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0002); num1123++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, (int)num5 + 1), WorldGen.genRand.Next(4, 15), WorldGen.genRand.Next(5, 40), 1);
			}
			for (int num1124 = 0; num1124 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0002); num1124++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num5, (int)num6 + 1), WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 30), 1);
			}
			for (int num1125 = 0; num1125 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0045); num1125++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num6, (int)num8 + 1), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(2, 23), 1);
			}
			Main.statusText = Lang.gen[5].Value;
			for (int num1126 = 0; num1126 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.005); num1126++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num7, Main.maxTilesY), WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(2, 40), 0);
			}
			Main.statusText = Lang.gen[6].Value;
			for (int num1127 = 0; num1127 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05); num1127++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, (int)num5), WorldGen.genRand.Next(4, 14), WorldGen.genRand.Next(10, 50), 40);
			}
			for (int num1128 = 0; num1128 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 5E-05); num1128++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num5, (int)num6 + 1), WorldGen.genRand.Next(8, 14), WorldGen.genRand.Next(15, 45), 40);
			}
			for (int num1129 = 0; num1129 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05); num1129++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num6, (int)num8 + 1), WorldGen.genRand.Next(8, 15), WorldGen.genRand.Next(5, 50), 40);
			}
			for (int num1130 = 5; num1130 < Main.maxTilesX - 5; num1130++)
			{
				for (int num1131 = 1; (double)num1131 < Main.worldSurface - 1.0; num1131++)
				{
					if (Main.tile[num1130, num1131].HasTile)
					{
						for (int num1132 = num1131; num1132 < num1131 + 5; num1132++)
						{
							if (Main.tile[num1130, num1132].type == 40)
							{
								Main.tile[num1130, num1132].type = 0;
							}
						}
						break;
					}
				}
			}
			int num1133 = 0;
			for (int num1134 = 0; num1134 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0015); num1134++)
			{
				float num1135 = (float)((double)num1134 / ((double)(Main.maxTilesX * Main.maxTilesY) * 0.0015));
				Main.statusText = Lang.gen[7].Value + " " + (int)(num1135 * 100f + 1f) + "%";
				int type = -1;
				if (WorldGen.genRand.Next(5) == 0)
				{
					type = -2;
				}
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num6, Main.maxTilesY), WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(2, 20), type);
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num6, Main.maxTilesY), WorldGen.genRand.Next(8, 15), WorldGen.genRand.Next(7, 30), type);
			}
			for (int num1136 = 0; num1136 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 3E-05); num1136++)
			{
				float num1137 = (float)((double)num1136 / ((double)(Main.maxTilesX * Main.maxTilesY) * 3E-05));
				Main.statusText = Lang.gen[8].Value + " " + (int)(num1137 * 100f + 1f) + "%";
				if (num8 <= (double)Main.maxTilesY)
				{
					int type2 = -1;
					if (WorldGen.genRand.Next(6) == 0)
					{
						type2 = -2;
					}
					TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num5, (int)num8 + 1), WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(30, 200), type2);
				}
			}
			for (int num1138 = 0; num1138 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.00013); num1138++)
			{
				float num1139 = (float)((double)num1138 / ((double)(Main.maxTilesX * Main.maxTilesY) * 0.00013));
				Main.statusText = Lang.gen[9].Value + " " + (int)(num1139 * 100f + 1f) + "%";
				if (num8 <= (double)Main.maxTilesY)
				{
					int type3 = -1;
					if (WorldGen.genRand.Next(10) == 0)
					{
						type3 = -2;
					}
					TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num8, Main.maxTilesY), WorldGen.genRand.Next(6, 20), WorldGen.genRand.Next(50, 300), type3);
				}
			}
			Main.statusText = Lang.gen[10].Value;
			for (int num1140 = 0; num1140 < (int)((double)Main.maxTilesX * 0.0025); num1140++)
			{
				num1133 = WorldGen.genRand.Next(0, Main.maxTilesX);
				for (int num1141 = 0; (double)num1141 < num6; num1141++)
				{
					if (Main.tile[num1133, num1141].HasTile)
					{
						TileRunner(num1133, num1141, WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(5, 50), -1, addTile: false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 1f);
						break;
					}
				}
			}
			for (int num1142 = 0; num1142 < (int)((double)Main.maxTilesX * 0.0007); num1142++)
			{
				num1133 = WorldGen.genRand.Next(0, Main.maxTilesX);
				for (int num1143 = 0; (double)num1143 < num6; num1143++)
				{
					if (Main.tile[num1133, num1143].HasTile)
					{
						TileRunner(num1133, num1143, WorldGen.genRand.Next(10, 15), WorldGen.genRand.Next(50, 130), -1, addTile: false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 2f);
						break;
					}
				}
			}
			for (int num1144 = 0; num1144 < (int)((double)Main.maxTilesX * 0.0003); num1144++)
			{
				num1133 = WorldGen.genRand.Next(0, Main.maxTilesX);
				for (int num1145 = 0; (double)num1145 < num6; num1145++)
				{
					if (Main.tile[num1133, num1145].HasTile)
					{
						TileRunner(num1133, num1145, WorldGen.genRand.Next(12, 25), WorldGen.genRand.Next(150, 500), -1, addTile: false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 4f);
						TileRunner(num1133, num1145, WorldGen.genRand.Next(8, 17), WorldGen.genRand.Next(60, 200), -1, addTile: false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 2f);
						TileRunner(num1133, num1145, WorldGen.genRand.Next(5, 13), WorldGen.genRand.Next(40, 170), -1, addTile: false, (float)WorldGen.genRand.Next(-10, 11) * 0.1f, 2f);
						break;
					}
				}
			}
			for (int num1146 = 0; num1146 < (int)((double)Main.maxTilesX * 0.0004); num1146++)
			{
				num1133 = WorldGen.genRand.Next(0, Main.maxTilesX);
				for (int num1147 = 0; (double)num1147 < num6; num1147++)
				{
					if (Main.tile[num1133, num1147].HasTile)
					{
						TileRunner(num1133, num1147, WorldGen.genRand.Next(7, 12), WorldGen.genRand.Next(150, 250), -1, addTile: false, 0f, 1f, noYChange: true);
						break;
					}
				}
			}
			float num1148 = Main.maxTilesX / 4200;
			for (int num1149 = 0; (float)num1149 < 5f * num1148; num1149++)
			{
				try
				{
					Caverer(WorldGen.genRand.Next(100, Main.maxTilesX - 100), WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 400));
				}
				catch
				{
				}
			}
			for (int num1150 = 0; num1150 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.002); num1150++)
			{
				int num1151 = WorldGen.genRand.Next(1, Main.maxTilesX - 1);
				int num1152 = WorldGen.genRand.Next((int)num5, (int)num6);
				if (num1152 >= Main.maxTilesY)
				{
					num1152 = Main.maxTilesY - 2;
				}
				if (Main.tile[num1151 - 1, num1152].HasTile && Main.tile[num1151 - 1, num1152].type == 0 && Main.tile[num1151 + 1, num1152].HasTile && Main.tile[num1151 + 1, num1152].type == 0 && Main.tile[num1151, num1152 - 1].HasTile && Main.tile[num1151, num1152 - 1].type == 0 && Main.tile[num1151, num1152 + 1].HasTile && Main.tile[num1151, num1152 + 1].type == 0)
				{
					Main.tile[num1151, num1152].active(active: true);
					Main.tile[num1151, num1152].type = 2;
				}
				num1151 = WorldGen.genRand.Next(1, Main.maxTilesX - 1);
				num1152 = WorldGen.genRand.Next(0, (int)num5);
				if (num1152 >= Main.maxTilesY)
				{
					num1152 = Main.maxTilesY - 2;
				}
				if (Main.tile[num1151 - 1, num1152].HasTile && Main.tile[num1151 - 1, num1152].type == 0 && Main.tile[num1151 + 1, num1152].HasTile && Main.tile[num1151 + 1, num1152].type == 0 && Main.tile[num1151, num1152 - 1].HasTile && Main.tile[num1151, num1152 - 1].type == 0 && Main.tile[num1151, num1152 + 1].HasTile && Main.tile[num1151, num1152 + 1].type == 0)
				{
					Main.tile[num1151, num1152].active(active: true);
					Main.tile[num1151, num1152].type = 2;
				}
			}
			Main.statusText = Lang.gen[11]?.ToString() + " 0%";
			float num1153 = Main.maxTilesX / 4200;
			num1153 *= 1.5f;
			int num1154 = 0;
			float num1155 = (float)WorldGen.genRand.Next(15, 30) * 0.01f;
			if (num9 == -1)
			{
				num1155 = 1f - num1155;
				num1154 = (int)((float)Main.maxTilesX * num1155);
			}
			else
			{
				num1154 = (int)((float)Main.maxTilesX * num1155);
			}
			int num1156 = (int)((double)Main.maxTilesY + Main.rockLayer) / 2;
			num1154 += WorldGen.genRand.Next((int)(-100f * num1153), (int)(101f * num1153));
			num1156 += WorldGen.genRand.Next((int)(-100f * num1153), (int)(101f * num1153));
			int num1157 = num1154;
			int num1158 = num1156;
			TileRunner(num1154, num1156, WorldGen.genRand.Next((int)(250f * num1153), (int)(500f * num1153)), WorldGen.genRand.Next(50, 150), 59, addTile: false, num9 * 3);
			for (int num1159 = 0; (float)num1159 < 6f * num1153; num1159++)
			{
				TileRunner(num1154 + WorldGen.genRand.Next(-(int)(125f * num1153), (int)(125f * num1153)), num1156 + WorldGen.genRand.Next(-(int)(125f * num1153), (int)(125f * num1153)), WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(63, 65));
			}
			mudWall = true;
			Main.statusText = Lang.gen[11]?.ToString() + " 15%";
			num1154 += WorldGen.genRand.Next((int)(-250f * num1153), (int)(251f * num1153));
			num1156 += WorldGen.genRand.Next((int)(-150f * num1153), (int)(151f * num1153));
			int num1160 = num1154;
			int num1161 = num1156;
			int num1162 = num1154;
			int num1163 = num1156;
			TileRunner(num1154, num1156, WorldGen.genRand.Next((int)(250f * num1153), (int)(500f * num1153)), WorldGen.genRand.Next(50, 150), 59);
			mudWall = false;
			for (int num1164 = 0; (float)num1164 < 6f * num1153; num1164++)
			{
				TileRunner(num1154 + WorldGen.genRand.Next(-(int)(125f * num1153), (int)(125f * num1153)), num1156 + WorldGen.genRand.Next(-(int)(125f * num1153), (int)(125f * num1153)), WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(65, 67));
			}
			mudWall = true;
			Main.statusText = Lang.gen[11]?.ToString() + " 30%";
			num1154 += WorldGen.genRand.Next((int)(-400f * num1153), (int)(401f * num1153));
			num1156 += WorldGen.genRand.Next((int)(-150f * num1153), (int)(151f * num1153));
			int num1165 = num1154;
			int num1166 = num1156;
			TileRunner(num1154, num1156, WorldGen.genRand.Next((int)(250f * num1153), (int)(500f * num1153)), WorldGen.genRand.Next(50, 150), 59, addTile: false, num9 * -3);
			mudWall = false;
			for (int num1167 = 0; (float)num1167 < 6f * num1153; num1167++)
			{
				TileRunner(num1154 + WorldGen.genRand.Next(-(int)(125f * num1153), (int)(125f * num1153)), num1156 + WorldGen.genRand.Next(-(int)(125f * num1153), (int)(125f * num1153)), WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(67, 69));
			}
			mudWall = true;
			Main.statusText = Lang.gen[11]?.ToString() + " 45%";
			num1154 = (num1157 + num1160 + num1165) / 3;
			num1156 = (num1158 + num1161 + num1166) / 3;
			TileRunner(num1154, num1156, WorldGen.genRand.Next((int)(400f * num1153), (int)(600f * num1153)), 10000, 59, addTile: false, 0f, -20f, noYChange: true);
			JungleRunner(num1154, num1156);
			Main.statusText = Lang.gen[11]?.ToString() + " 60%";
			mudWall = false;
			for (int num1168 = 0; num1168 < Main.maxTilesX / 10; num1168++)
			{
				num1154 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
				num1156 = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 200);
				while (Main.tile[num1154, num1156].wall != 15)
				{
					num1154 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
					num1156 = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 200);
				}
				MudWallRunner(num1154, num1156);
			}
			num1154 = num1162;
			num1156 = num1163;
			for (int num1169 = 0; (float)num1169 <= 20f * num1153; num1169++)
			{
				Main.statusText = Lang.gen[11]?.ToString() + " " + (int)(60f + (float)num1169 / num1153) + "%";
				num1154 += WorldGen.genRand.Next((int)(-5f * num1153), (int)(6f * num1153));
				num1156 += WorldGen.genRand.Next((int)(-5f * num1153), (int)(6f * num1153));
				TileRunner(num1154, num1156, WorldGen.genRand.Next(40, 100), WorldGen.genRand.Next(300, 500), 59);
			}
			for (int num1170 = 0; (float)num1170 <= 10f * num1153; num1170++)
			{
				Main.statusText = Lang.gen[11]?.ToString() + " " + (int)(80f + (float)num1170 / num1153 * 2f) + "%";
				num1154 = num1162 + WorldGen.genRand.Next((int)(-600f * num1153), (int)(600f * num1153));
				num1156 = num1163 + WorldGen.genRand.Next((int)(-200f * num1153), (int)(200f * num1153));
				while (num1154 < 1 || num1154 >= Main.maxTilesX - 1 || num1156 < 1 || num1156 >= Main.maxTilesY - 1 || Main.tile[num1154, num1156].type != 59)
				{
					num1154 = num1162 + WorldGen.genRand.Next((int)(-600f * num1153), (int)(600f * num1153));
					num1156 = num1163 + WorldGen.genRand.Next((int)(-200f * num1153), (int)(200f * num1153));
				}
				for (int num1171 = 0; (float)num1171 < 8f * num1153; num1171++)
				{
					num1154 += WorldGen.genRand.Next(-30, 31);
					num1156 += WorldGen.genRand.Next(-30, 31);
					int type4 = -1;
					if (WorldGen.genRand.Next(7) == 0)
					{
						type4 = -2;
					}
					TileRunner(num1154, num1156, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(30, 70), type4);
				}
			}
			for (int num1172 = 0; (float)num1172 <= 300f * num1153; num1172++)
			{
				num1154 = num1162 + WorldGen.genRand.Next((int)(-600f * num1153), (int)(600f * num1153));
				num1156 = num1163 + WorldGen.genRand.Next((int)(-200f * num1153), (int)(200f * num1153));
				while (num1154 < 1 || num1154 >= Main.maxTilesX - 1 || num1156 < 1 || num1156 >= Main.maxTilesY - 1 || Main.tile[num1154, num1156].type != 59)
				{
					num1154 = num1162 + WorldGen.genRand.Next((int)(-600f * num1153), (int)(600f * num1153));
					num1156 = num1163 + WorldGen.genRand.Next((int)(-200f * num1153), (int)(200f * num1153));
				}
				TileRunner(num1154, num1156, WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 30), 1);
				if (WorldGen.genRand.Next(4) == 0)
				{
					int type5 = WorldGen.genRand.Next(63, 69);
					TileRunner(num1154 + WorldGen.genRand.Next(-1, 2), num1156 + WorldGen.genRand.Next(-1, 2), WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(4, 8), type5);
				}
			}
			num1154 = num1162;
			num1156 = num1163;
			float num1173 = WorldGen.genRand.Next(6, 10);
			float num1174 = Main.maxTilesX / 4200;
			num1173 *= num1174;
			for (int num1175 = 0; (float)num1175 < num1173; num1175++)
			{
				bool flag4 = true;
				while (flag4)
				{
					num1154 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
					num1156 = WorldGen.genRand.Next((int)(Main.worldSurface + Main.rockLayer) / 2, Main.maxTilesY - 300);
					if (Main.tile[num1154, num1156].type == 59)
					{
						flag4 = false;
						int num1176 = WorldGen.genRand.Next(2, 4);
						int num1177 = WorldGen.genRand.Next(2, 4);
						for (int num1178 = num1154 - num1176 - 1; num1178 <= num1154 + num1176 + 1; num1178++)
						{
							for (int num1179 = num1156 - num1177 - 1; num1179 <= num1156 + num1177 + 1; num1179++)
							{
								Main.tile[num1178, num1179].active(active: true);
								Main.tile[num1178, num1179].type = 45;
								Main.tile[num1178, num1179].liquid = 0;
								Main.tile[num1178, num1179].lava(lava: false);
							}
						}
						for (int num1180 = num1154 - num1176; num1180 <= num1154 + num1176; num1180++)
						{
							for (int num1181 = num1156 - num1177; num1181 <= num1156 + num1177; num1181++)
							{
								Main.tile[num1180, num1181].active(active: false);
							}
						}
						bool flag5 = false;
						int num1182 = 0;
						while (!flag5 && num1182 < 100)
						{
							num1182++;
							int num1183 = WorldGen.genRand.Next(num1154 - num1176, num1154 + num1176 + 1);
							int num1184 = WorldGen.genRand.Next(num1156 - num1177, num1156 + num1177 - 2);
							PlaceTile(num1183, num1184, 4, mute: true);
							if (Main.tile[num1183, num1184].type == 4)
							{
								flag5 = true;
							}
						}
						for (int num1185 = num1154 - num1176 - 1; num1185 <= num1154 + num1176 + 1; num1185++)
						{
							for (int num1186 = num1156 + num1177 - 2; num1186 <= num1156 + num1177; num1186++)
							{
								Main.tile[num1185, num1186].active(active: false);
							}
						}
						for (int num1187 = num1154 - num1176 - 1; num1187 <= num1154 + num1176 + 1; num1187++)
						{
							for (int num1188 = num1156 + num1177 - 2; num1188 <= num1156 + num1177 - 1; num1188++)
							{
								Main.tile[num1187, num1188].active(active: false);
							}
						}
						for (int num1189 = num1154 - num1176 - 1; num1189 <= num1154 + num1176 + 1; num1189++)
						{
							int num1190 = 4;
							int num1191 = num1156 + num1177 + 2;
							while (!Main.tile[num1189, num1191].HasTile && num1191 < Main.maxTilesY && num1190 > 0)
							{
								Main.tile[num1189, num1191].active(active: true);
								Main.tile[num1189, num1191].type = 59;
								num1191++;
								num1190--;
							}
						}
						num1176 -= WorldGen.genRand.Next(1, 3);
						int num1192 = num1156 - num1177 - 2;
						while (num1176 > -1)
						{
							for (int num1193 = num1154 - num1176 - 1; num1193 <= num1154 + num1176 + 1; num1193++)
							{
								Main.tile[num1193, num1192].active(active: true);
								Main.tile[num1193, num1192].type = 45;
							}
							num1176 -= WorldGen.genRand.Next(1, 3);
							num1192--;
						}
						JChestX[numJChests] = num1154;
						JChestY[numJChests] = num1156;
						numJChests++;
					}
				}
			}
			for (int num1194 = 0; num1194 < Main.maxTilesX; num1194++)
			{
				for (int num1195 = 0; num1195 < Main.maxTilesY; num1195++)
				{
					if (Main.tile[num1194, num1195].HasTile)
					{
						try
						{
							WorldGen.grassSpread = 0;
							SpreadGrass(num1194, num1195, 59, 60);
						}
						catch
						{
							WorldGen.grassSpread = 0;
							SpreadGrass(num1194, num1195, 59, 60, repeat: false);
						}
					}
				}
			}
			numIslandHouses = 0;
			houseCount = 0;
			Main.statusText = Lang.gen[12].Value;
			for (int num1196 = 0; num1196 < (int)((double)Main.maxTilesX * 0.0008); num1196++)
			{
				int num1197 = 0;
				bool flag6 = false;
				int num1198 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.1), (int)((double)Main.maxTilesX * 0.9));
				bool flag7 = false;
				while (!flag7)
				{
					flag7 = true;
					while (num1198 > Main.maxTilesX / 2 - 80 && num1198 < Main.maxTilesX / 2 + 80)
					{
						num1198 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.1), (int)((double)Main.maxTilesX * 0.9));
					}
					for (int num1199 = 0; num1199 < numIslandHouses; num1199++)
					{
						if (num1198 > fihX[num1199] - 80 && num1198 < fihX[num1199] + 80)
						{
							num1197++;
							flag7 = false;
							break;
						}
					}
					if (num1197 >= 200)
					{
						flag6 = true;
						break;
					}
				}
				if (!flag6)
				{
					for (int num1200 = 200; (double)num1200 < Main.worldSurface; num1200++)
					{
						if (Main.tile[num1198, num1200].HasTile)
						{
							int num1201 = num1198;
							int num1202 = WorldGen.genRand.Next(90, num1200 - 100);
							while ((double)num1202 > num5 - 50.0)
							{
								num1202--;
							}
							FloatingIsland(num1201, num1202);
							fihX[numIslandHouses] = num1201;
							fihY[numIslandHouses] = num1202;
							numIslandHouses++;
							break;
						}
					}
				}
			}
			Main.statusText = Lang.gen[13].Value;
			for (int num1203 = 0; num1203 < Main.maxTilesX / 500; num1203++)
			{
				int i2 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.3), (int)((double)Main.maxTilesX * 0.7));
				int j2 = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 350);
				ShroomPatch(i2, j2);
			}
			for (int num1204 = 0; num1204 < Main.maxTilesX; num1204++)
			{
				for (int num1205 = (int)Main.worldSurface; num1205 < Main.maxTilesY; num1205++)
				{
					if (Main.tile[num1204, num1205].HasTile)
					{
						WorldGen.grassSpread = 0;
						SpreadGrass(num1204, num1205, 59, 70, repeat: false);
					}
				}
			}
			Main.statusText = Lang.gen[14].Value;
			for (int num1206 = 0; num1206 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.001); num1206++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num7, Main.maxTilesY), WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(2, 40), 59);
			}
			Main.statusText = Lang.gen[15].Value;
			for (int num1207 = 0; num1207 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0001); num1207++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num8, Main.maxTilesY), WorldGen.genRand.Next(5, 12), WorldGen.genRand.Next(15, 50), 123);
			}
			for (int num1208 = 0; num1208 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0005); num1208++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num8, Main.maxTilesY), WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(2, 5), 123);
			}
			Main.statusText = Lang.gen[16].Value;
			for (int num1209 = 0; num1209 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05); num1209++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num5, (int)num6), WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(2, 6), 7);
			}
			for (int num1210 = 0; num1210 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 8E-05); num1210++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num6, (int)num8), WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(3, 7), 7);
			}
			for (int num1211 = 0; num1211 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0002); num1211++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num7, Main.maxTilesY), WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(4, 8), 7);
			}
			for (int num1212 = 0; num1212 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 3E-05); num1212++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num5, (int)num6), WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(2, 5), 6);
			}
			for (int num1213 = 0; num1213 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 8E-05); num1213++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num6, (int)num8), WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(3, 6), 6);
			}
			for (int num1214 = 0; num1214 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0002); num1214++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num7, Main.maxTilesY), WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(4, 8), 6);
			}
			for (int num1215 = 0; num1215 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 2.6E-05); num1215++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num6, (int)num8), WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(3, 6), 9);
			}
			for (int num1216 = 0; num1216 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.00015); num1216++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num7, Main.maxTilesY), WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(4, 8), 9);
			}
			for (int num1217 = 0; num1217 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.00017); num1217++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, (int)num5), WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(4, 8), 9);
			}
			for (int num1218 = 0; num1218 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.00012); num1218++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num7, Main.maxTilesY), WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), 8);
			}
			for (int num1219 = 0; num1219 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.00012); num1219++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(0, (int)num5 - 20), WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), 8);
			}
			for (int num1220 = 0; num1220 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05); num1220++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next((int)num7, Main.maxTilesY), WorldGen.genRand.Next(2, 4), WorldGen.genRand.Next(3, 6), 22);
			}
			Main.statusText = Lang.gen[17].Value;
			for (int num1221 = 0; num1221 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0006); num1221++)
			{
				int num1222 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
				int num1223 = WorldGen.genRand.Next((int)num5, Main.maxTilesY - 20);
				if (num1221 < numMCaves)
				{
					num1222 = mCaveX[num1221];
					num1223 = mCaveY[num1221];
				}
				if (!Main.tile[num1222, num1223].HasTile && ((double)num1223 > Main.worldSurface || Main.tile[num1222, num1223].wall > 0))
				{
					while (!Main.tile[num1222, num1223].HasTile && num1223 > (int)num5)
					{
						num1223--;
					}
					num1223++;
					int num1224 = 1;
					if (WorldGen.genRand.Next(2) == 0)
					{
						num1224 = -1;
					}
					for (; !Main.tile[num1222, num1223].HasTile && num1222 > 10 && num1222 < Main.maxTilesX - 10; num1222 += num1224)
					{
					}
					num1222 -= num1224;
					if ((double)num1223 > Main.worldSurface || Main.tile[num1222, num1223].wall > 0)
					{
						TileRunner(num1222, num1223, WorldGen.genRand.Next(4, 11), WorldGen.genRand.Next(2, 4), 51, addTile: true, num1224, -1f, noYChange: false, overRide: false);
					}
				}
			}
			Main.statusText = Lang.gen[18]?.ToString() + " 0%";
			int num1225 = Main.maxTilesY - WorldGen.genRand.Next(150, 190);
			for (int num1226 = 0; num1226 < Main.maxTilesX; num1226++)
			{
				num1225 += WorldGen.genRand.Next(-3, 4);
				if (num1225 < Main.maxTilesY - 190)
				{
					num1225 = Main.maxTilesY - 190;
				}
				if (num1225 > Main.maxTilesY - 160)
				{
					num1225 = Main.maxTilesY - 160;
				}
				for (int num1227 = num1225 - 20 - WorldGen.genRand.Next(3); num1227 < Main.maxTilesY; num1227++)
				{
					if (num1227 >= num1225)
					{
						Main.tile[num1226, num1227].active(active: false);
						Main.tile[num1226, num1227].lava(lava: false);
						Main.tile[num1226, num1227].liquid = 0;
					}
					else
					{
						Main.tile[num1226, num1227].type = 57;
					}
				}
			}
			int num1228 = Main.maxTilesY - WorldGen.genRand.Next(40, 70);
			for (int num1229 = 10; num1229 < Main.maxTilesX - 10; num1229++)
			{
				num1228 += WorldGen.genRand.Next(-10, 11);
				if (num1228 > Main.maxTilesY - 60)
				{
					num1228 = Main.maxTilesY - 60;
				}
				if (num1228 < Main.maxTilesY - 100)
				{
					num1228 = Main.maxTilesY - 120;
				}
				for (int num1230 = num1228; num1230 < Main.maxTilesY - 10; num1230++)
				{
					if (!Main.tile[num1229, num1230].HasTile)
					{
						Main.tile[num1229, num1230].lava(lava: true);
						Main.tile[num1229, num1230].liquid = byte.MaxValue;
					}
				}
			}
			for (int num1231 = 0; num1231 < Main.maxTilesX; num1231++)
			{
				if (WorldGen.genRand.Next(50) == 0)
				{
					int num1232 = Main.maxTilesY - 65;
					while (!Main.tile[num1231, num1232].HasTile && num1232 > Main.maxTilesY - 135)
					{
						num1232--;
					}
					TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), num1232 + WorldGen.genRand.Next(20, 50), WorldGen.genRand.Next(15, 20), 1000, 57, addTile: true, 0f, WorldGen.genRand.Next(1, 3), noYChange: true);
				}
			}
			Liquid.QuickWater(-2);
			for (int num1233 = 0; num1233 < Main.maxTilesX; num1233++)
			{
				float num1234 = (float)num1233 / (float)(Main.maxTilesX - 1);
				Main.statusText = Lang.gen[18]?.ToString() + " " + (int)(num1234 * 100f / 2f + 50f) + "%";
				if (WorldGen.genRand.Next(13) == 0)
				{
					int num1235 = Main.maxTilesY - 65;
					while ((Main.tile[num1233, num1235].liquid > 0 || Main.tile[num1233, num1235].HasTile) && num1235 > Main.maxTilesY - 140)
					{
						num1235--;
					}
					TileRunner(num1233, num1235 - WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(5, 30), 1000, 57, addTile: true, 0f, WorldGen.genRand.Next(1, 3), noYChange: true);
					float num1236 = WorldGen.genRand.Next(1, 3);
					if (WorldGen.genRand.Next(3) == 0)
					{
						num1236 *= 0.5f;
					}
					if (WorldGen.genRand.Next(2) == 0)
					{
						TileRunner(num1233, num1235 - WorldGen.genRand.Next(2, 5), (int)((float)WorldGen.genRand.Next(5, 15) * num1236), (int)((float)WorldGen.genRand.Next(10, 15) * num1236), 57, addTile: true, 1f, 0.3f);
					}
					if (WorldGen.genRand.Next(2) == 0)
					{
						num1236 = WorldGen.genRand.Next(1, 3);
						TileRunner(num1233, num1235 - WorldGen.genRand.Next(2, 5), (int)((float)WorldGen.genRand.Next(5, 15) * num1236), (int)((float)WorldGen.genRand.Next(10, 15) * num1236), 57, addTile: true, -1f, 0.3f);
					}
					TileRunner(num1233 + WorldGen.genRand.Next(-10, 10), num1235 + WorldGen.genRand.Next(-10, 10), WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(5, 10), -2, addTile: false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
					if (WorldGen.genRand.Next(3) == 0)
					{
						TileRunner(num1233 + WorldGen.genRand.Next(-10, 10), num1235 + WorldGen.genRand.Next(-10, 10), WorldGen.genRand.Next(10, 30), WorldGen.genRand.Next(10, 20), -2, addTile: false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
					}
					if (WorldGen.genRand.Next(5) == 0)
					{
						TileRunner(num1233 + WorldGen.genRand.Next(-15, 15), num1235 + WorldGen.genRand.Next(-15, 10), WorldGen.genRand.Next(15, 30), WorldGen.genRand.Next(5, 20), -2, addTile: false, WorldGen.genRand.Next(-1, 3), WorldGen.genRand.Next(-1, 3));
					}
				}
			}
			for (int num1237 = 0; num1237 < Main.maxTilesX; num1237++)
			{
				TileRunner(WorldGen.genRand.Next(20, Main.maxTilesX - 20), WorldGen.genRand.Next(Main.maxTilesY - 180, Main.maxTilesY - 10), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(2, 7), -2);
			}
			for (int num1238 = 0; num1238 < Main.maxTilesX; num1238++)
			{
				if (!Main.tile[num1238, Main.maxTilesY - 145].HasTile)
				{
					Main.tile[num1238, Main.maxTilesY - 145].liquid = byte.MaxValue;
					Main.tile[num1238, Main.maxTilesY - 145].lava(lava: true);
				}
				if (!Main.tile[num1238, Main.maxTilesY - 144].HasTile)
				{
					Main.tile[num1238, Main.maxTilesY - 144].liquid = byte.MaxValue;
					Main.tile[num1238, Main.maxTilesY - 144].lava(lava: true);
				}
			}
			for (int num1239 = 0; num1239 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008); num1239++)
			{
				TileRunner(WorldGen.genRand.Next(0, Main.maxTilesX), WorldGen.genRand.Next(Main.maxTilesY - 140, Main.maxTilesY), WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 7), 58);
			}
			AddHellHouses();
			int num1240 = WorldGen.genRand.Next(2, (int)((double)Main.maxTilesX * 0.005));
			for (int num1241 = 0; num1241 < num1240; num1241++)
			{
				float num1242 = (float)num1241 / (float)num1240;
				Main.statusText = Lang.gen[19]?.ToString() + " " + (int)(num1242 * 100f) + "%";
				int num1243 = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
				while (num1243 > Main.maxTilesX / 2 - 50 && num1243 < Main.maxTilesX / 2 + 50)
				{
					num1243 = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
				}
				int num1244;
				for (num1244 = (int)num5 - 20; !Main.tile[num1243, num1244].HasTile; num1244++)
				{
				}
				Lakinater(num1243, num1244);
			}
			int num1245 = 0;
			if (num9 == -1)
			{
				num1245 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.05), (int)((double)Main.maxTilesX * 0.2));
				num9 = -1;
			}
			else
			{
				num1245 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.8), (int)((double)Main.maxTilesX * 0.95));
				num9 = 1;
			}
			int y = (int)((Main.rockLayer + (double)Main.maxTilesY) / 2.0) + WorldGen.genRand.Next(-200, 200);
			MakeDungeon(num1245, y);
			for (int num1246 = 0; (double)num1246 < (double)Main.maxTilesX * 0.00045; num1246++)
			{
				float num1247 = (float)((double)num1246 / ((double)Main.maxTilesX * 0.00045));
				Main.statusText = Lang.gen[20]?.ToString() + " " + (int)(num1247 * 100f) + "%";
				bool flag8 = false;
				int num1248 = 0;
				int num1249 = 0;
				int num1250 = 0;
				while (!flag8)
				{
					int num1251 = 0;
					flag8 = true;
					int num1252 = Main.maxTilesX / 2;
					int num1253 = 200;
					num1248 = WorldGen.genRand.Next(320, Main.maxTilesX - 320);
					num1249 = num1248 - WorldGen.genRand.Next(200) - 100;
					num1250 = num1248 + WorldGen.genRand.Next(200) + 100;
					if (num1249 < 285)
					{
						num1249 = 285;
					}
					if (num1250 > Main.maxTilesX - 285)
					{
						num1250 = Main.maxTilesX - 285;
					}
					if (num1248 > num1252 - num1253 && num1248 < num1252 + num1253)
					{
						flag8 = false;
					}
					if (num1249 > num1252 - num1253 && num1249 < num1252 + num1253)
					{
						flag8 = false;
					}
					if (num1250 > num1252 - num1253 && num1250 < num1252 + num1253)
					{
						flag8 = false;
					}
					for (int num1254 = num1249; num1254 < num1250; num1254++)
					{
						for (int num1255 = 0; num1255 < (int)Main.worldSurface; num1255 += 5)
						{
							if (Main.tile[num1254, num1255].HasTile && Main.tileDungeon[Main.tile[num1254, num1255].type])
							{
								flag8 = false;
								break;
							}
							if (!flag8)
							{
								break;
							}
						}
					}
					if (num1251 < 200 && JungleX > num1249 && JungleX < num1250)
					{
						num1251++;
						flag8 = false;
					}
				}
				int num1256 = 0;
				for (int num1257 = num1249; num1257 < num1250; num1257++)
				{
					if (num1256 > 0)
					{
						num1256--;
					}
					if (num1257 == num1248 || num1256 == 0)
					{
						for (int num1258 = (int)num5; (double)num1258 < Main.worldSurface - 1.0; num1258++)
						{
							if (Main.tile[num1257, num1258].HasTile || Main.tile[num1257, num1258].wall > 0)
							{
								if (num1257 == num1248)
								{
									num1256 = 20;
									ChasmRunner(num1257, num1258, WorldGen.genRand.Next(150) + 150, makeOrb: true);
								}
								else if (WorldGen.genRand.Next(35) == 0 && num1256 == 0)
								{
									num1256 = 30;
									bool makeOrb = true;
									ChasmRunner(num1257, num1258, WorldGen.genRand.Next(50) + 50, makeOrb);
								}
								break;
							}
						}
					}
					for (int num1259 = (int)num5; (double)num1259 < Main.worldSurface - 1.0; num1259++)
					{
						if (Main.tile[num1257, num1259].HasTile)
						{
							int num1260 = num1259 + WorldGen.genRand.Next(10, 14);
							for (int num1261 = num1259; num1261 < num1260; num1261++)
							{
								if ((Main.tile[num1257, num1261].type == 59 || Main.tile[num1257, num1261].type == 60) && num1257 >= num1249 + WorldGen.genRand.Next(5) && num1257 < num1250 - WorldGen.genRand.Next(5))
								{
									Main.tile[num1257, num1261].type = 0;
								}
							}
							break;
						}
					}
				}
				double num1262 = Main.worldSurface + 40.0;
				for (int num1263 = num1249; num1263 < num1250; num1263++)
				{
					num1262 += (double)WorldGen.genRand.Next(-2, 3);
					if (num1262 < Main.worldSurface + 30.0)
					{
						num1262 = Main.worldSurface + 30.0;
					}
					if (num1262 > Main.worldSurface + 50.0)
					{
						num1262 = Main.worldSurface + 50.0;
					}
					num1133 = num1263;
					bool flag9 = false;
					for (int num1264 = (int)num5; (double)num1264 < num1262; num1264++)
					{
						if (Main.tile[num1133, num1264].HasTile)
						{
							if (Main.tile[num1133, num1264].type == 53 && num1133 >= num1249 + WorldGen.genRand.Next(5) && num1133 <= num1250 - WorldGen.genRand.Next(5))
							{
								Main.tile[num1133, num1264].type = 0;
							}
							if (Main.tile[num1133, num1264].type == 0 && (double)num1264 < Main.worldSurface - 1.0 && !flag9)
							{
								WorldGen.grassSpread = 0;
								SpreadGrass(num1133, num1264, 0, 23);
							}
							flag9 = true;
							if (Main.tile[num1133, num1264].type == 1 && num1133 >= num1249 + WorldGen.genRand.Next(5) && num1133 <= num1250 - WorldGen.genRand.Next(5))
							{
								Main.tile[num1133, num1264].type = 25;
							}
							if (Main.tile[num1133, num1264].type == 2)
							{
								Main.tile[num1133, num1264].type = 23;
							}
						}
					}
				}
				for (int num1265 = num1249; num1265 < num1250; num1265++)
				{
					for (int num1266 = 0; num1266 < Main.maxTilesY - 50; num1266++)
					{
						if (Main.tile[num1265, num1266].HasTile && Main.tile[num1265, num1266].type == 31)
						{
							int num1267 = num1265 - 13;
							int num1268 = num1265 + 13;
							int num1269 = num1266 - 13;
							int num1270 = num1266 + 13;
							for (int num1271 = num1267; num1271 < num1268; num1271++)
							{
								if (num1271 > 10 && num1271 < Main.maxTilesX - 10)
								{
									for (int num1272 = num1269; num1272 < num1270; num1272++)
									{
										if (Math.Abs(num1271 - num1265) + Math.Abs(num1272 - num1266) < 9 + WorldGen.genRand.Next(11) && WorldGen.genRand.Next(3) != 0 && Main.tile[num1271, num1272].type != 31)
										{
											Main.tile[num1271, num1272].active(active: true);
											Main.tile[num1271, num1272].type = 25;
											if (Math.Abs(num1271 - num1265) <= 1 && Math.Abs(num1272 - num1266) <= 1)
											{
												Main.tile[num1271, num1272].active(active: false);
											}
										}
										if (Main.tile[num1271, num1272].type != 31 && Math.Abs(num1271 - num1265) <= 2 + WorldGen.genRand.Next(3) && Math.Abs(num1272 - num1266) <= 2 + WorldGen.genRand.Next(3))
										{
											Main.tile[num1271, num1272].active(active: false);
										}
									}
								}
							}
						}
					}
				}
			}
			Main.statusText = Lang.gen[21].Value;
			for (int num1273 = 0; num1273 < numMCaves; num1273++)
			{
				int i3 = mCaveX[num1273];
				int j3 = mCaveY[num1273];
				CaveOpenater(i3, j3);
				Cavinator(i3, j3, WorldGen.genRand.Next(40, 50));
			}
			int num1274 = 0;
			int num1275 = 0;
			int x = 20;
			int x2 = Main.maxTilesX - 20;
			Main.statusText = Lang.gen[22].Value;
			for (int num1276 = 0; num1276 < 2; num1276++)
			{
				int num1277 = 0;
				int num1278 = 0;
				if (num1276 == 0)
				{
					num1277 = 0;
					num1278 = WorldGen.genRand.Next(125, 200) + 50;
					if (num9 == 1)
					{
						num1278 = 275;
					}
					int num1279 = 0;
					float num1280 = 1f;
					int num1281;
					for (num1281 = 0; !Main.tile[num1278 - 1, num1281].HasTile; num1281++)
					{
					}
					num1274 = num1281;
					num1281 += WorldGen.genRand.Next(1, 5);
					for (int num1282 = num1278 - 1; num1282 >= num1277; num1282--)
					{
						num1279++;
						if (num1279 < 3)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.2f;
						}
						else if (num1279 < 6)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.15f;
						}
						else if (num1279 < 9)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.1f;
						}
						else if (num1279 < 15)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.07f;
						}
						else if (num1279 < 50)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.05f;
						}
						else if (num1279 < 75)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.04f;
						}
						else if (num1279 < 100)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.03f;
						}
						else if (num1279 < 125)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.02f;
						}
						else if (num1279 < 150)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.01f;
						}
						else if (num1279 < 175)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.005f;
						}
						else if (num1279 < 200)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.001f;
						}
						else if (num1279 < 230)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.01f;
						}
						else if (num1279 < 235)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.05f;
						}
						else if (num1279 < 240)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.1f;
						}
						else if (num1279 < 245)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.05f;
						}
						else if (num1279 < 255)
						{
							num1280 += (float)WorldGen.genRand.Next(10, 20) * 0.01f;
						}
						if (num1279 == 235)
						{
							x2 = num1282;
						}
						if (num1279 == 235)
						{
							x = num1282;
						}
						int num1283 = WorldGen.genRand.Next(15, 20);
						for (int num1284 = 0; (float)num1284 < (float)num1281 + num1280 + (float)num1283; num1284++)
						{
							if ((float)num1284 < (float)num1281 + num1280 * 0.75f - 3f)
							{
								Main.tile[num1282, num1284].active(active: false);
								if (num1284 > num1281)
								{
									Main.tile[num1282, num1284].liquid = byte.MaxValue;
								}
								else if (num1284 == num1281)
								{
									Main.tile[num1282, num1284].liquid = 127;
								}
							}
							else if (num1284 > num1281)
							{
								Main.tile[num1282, num1284].type = 53;
								Main.tile[num1282, num1284].active(active: true);
							}
							Main.tile[num1282, num1284].wall = 0;
						}
					}
				}
				else
				{
					num1277 = Main.maxTilesX - WorldGen.genRand.Next(125, 200) - 50;
					num1278 = Main.maxTilesX;
					if (num9 == -1)
					{
						num1277 = Main.maxTilesX - 275;
					}
					float num1285 = 1f;
					int num1286 = 0;
					int num1287;
					for (num1287 = 0; !Main.tile[num1277, num1287].HasTile; num1287++)
					{
					}
					num1275 = num1287;
					num1287 += WorldGen.genRand.Next(1, 5);
					for (int num1288 = num1277; num1288 < num1278; num1288++)
					{
						num1286++;
						if (num1286 < 3)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.2f;
						}
						else if (num1286 < 6)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.15f;
						}
						else if (num1286 < 9)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.1f;
						}
						else if (num1286 < 15)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.07f;
						}
						else if (num1286 < 50)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.05f;
						}
						else if (num1286 < 75)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.04f;
						}
						else if (num1286 < 100)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.03f;
						}
						else if (num1286 < 125)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.02f;
						}
						else if (num1286 < 150)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.01f;
						}
						else if (num1286 < 175)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.005f;
						}
						else if (num1286 < 200)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.001f;
						}
						else if (num1286 < 230)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.01f;
						}
						else if (num1286 < 235)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.05f;
						}
						else if (num1286 < 240)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.1f;
						}
						else if (num1286 < 245)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.05f;
						}
						else if (num1286 < 255)
						{
							num1285 += (float)WorldGen.genRand.Next(10, 20) * 0.01f;
						}
						if (num1286 == 235)
						{
							x2 = num1288;
						}
						int num1289 = WorldGen.genRand.Next(15, 20);
						for (int num1290 = 0; (float)num1290 < (float)num1287 + num1285 + (float)num1289; num1290++)
						{
							if ((float)num1290 < (float)num1287 + num1285 * 0.75f - 3f && (double)num1290 < Main.worldSurface - 2.0)
							{
								Main.tile[num1288, num1290].active(active: false);
								if (num1290 > num1287)
								{
									Main.tile[num1288, num1290].liquid = byte.MaxValue;
								}
								else if (num1290 == num1287)
								{
									Main.tile[num1288, num1290].liquid = 127;
								}
							}
							else if (num1290 > num1287)
							{
								Main.tile[num1288, num1290].type = 53;
								Main.tile[num1288, num1290].active(active: true);
							}
							Main.tile[num1288, num1290].wall = 0;
						}
					}
				}
			}
			for (; !Main.tile[x, num1274].HasTile; num1274++)
			{
			}
			num1274++;
			for (; !Main.tile[x2, num1275].HasTile; num1275++)
			{
			}
			num1275++;
			Main.statusText = Lang.gen[23].Value;
			for (int num1291 = 63; num1291 <= 68; num1291++)
			{
				float num1292 = 0f;
				switch (num1291)
				{
				case 67:
					num1292 = (float)Main.maxTilesX * 0.5f;
					break;
				case 66:
					num1292 = (float)Main.maxTilesX * 0.45f;
					break;
				case 63:
					num1292 = (float)Main.maxTilesX * 0.3f;
					break;
				case 65:
					num1292 = (float)Main.maxTilesX * 0.25f;
					break;
				case 64:
					num1292 = (float)Main.maxTilesX * 0.1f;
					break;
				case 68:
					num1292 = (float)Main.maxTilesX * 0.05f;
					break;
				}
				num1292 *= 0.2f;
				for (int num1293 = 0; (float)num1293 < num1292; num1293++)
				{
					int num1294 = WorldGen.genRand.Next(0, Main.maxTilesX);
					int num1295 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY);
					while (Main.tile[num1294, num1295].type != 1)
					{
						num1294 = WorldGen.genRand.Next(0, Main.maxTilesX);
						num1295 = WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY);
					}
					TileRunner(num1294, num1295, WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(3, 7), num1291);
				}
			}
			for (int num1296 = 0; num1296 < 2; num1296++)
			{
				int num1297 = 1;
				int num1298 = 5;
				int num1299 = Main.maxTilesX - 5;
				if (num1296 == 1)
				{
					num1297 = -1;
					num1298 = Main.maxTilesX - 5;
					num1299 = 5;
				}
				for (int num1300 = num1298; num1300 != num1299; num1300 += num1297)
				{
					for (int num1301 = 10; num1301 < Main.maxTilesY - 10; num1301++)
					{
						if (Main.tile[num1300, num1301].HasTile && Main.tile[num1300, num1301].type == 53 && Main.tile[num1300, num1301 + 1].HasTile && Main.tile[num1300, num1301 + 1].type == 53)
						{
							int x3 = num1300 + num1297;
							int num1302 = num1301 + 1;
							if (!Main.tile[x3, num1301].HasTile && !Main.tile[x3, num1301 + 1].HasTile)
							{
								for (; !Main.tile[x3, num1302].HasTile; num1302++)
								{
								}
								num1302--;
								Main.tile[num1300, num1301].active(active: false);
								Main.tile[x3, num1302].active(active: true);
								Main.tile[x3, num1302].type = 53;
							}
						}
					}
				}
			}
			for (int num1303 = 0; num1303 < Main.maxTilesX; num1303++)
			{
				float num1304 = (float)num1303 / (float)(Main.maxTilesX - 1);
				Main.statusText = Lang.gen[24]?.ToString() + " " + (int)(num1304 * 100f) + "%";
				for (int num1305 = Main.maxTilesY - 5; num1305 > 0; num1305--)
				{
					if (Main.tile[num1303, num1305].HasTile)
					{
						if (Main.tile[num1303, num1305].type == 53)
						{
							for (int num1306 = num1305; !Main.tile[num1303, num1306 + 1].HasTile && num1306 < Main.maxTilesY - 5; num1306++)
							{
								Main.tile[num1303, num1306 + 1].active(active: true);
								Main.tile[num1303, num1306 + 1].type = 53;
							}
						}
						else if (Main.tile[num1303, num1305].type == 123)
						{
							for (int num1307 = num1305; !Main.tile[num1303, num1307 + 1].HasTile && num1307 < Main.maxTilesY - 5; num1307++)
							{
								Main.tile[num1303, num1307 + 1].active(active: true);
								Main.tile[num1303, num1307 + 1].type = 123;
								Main.tile[num1303, num1307].active(active: false);
							}
						}
					}
				}
			}
			for (int num1308 = 3; num1308 < Main.maxTilesX - 3; num1308++)
			{
				float num1309 = (float)num1308 / (float)Main.maxTilesX;
				Main.statusText = Lang.gen[25]?.ToString() + " " + (int)(num1309 * 100f + 1f) + "%";
				bool flag10 = true;
				for (int num1310 = 0; (double)num1310 < Main.worldSurface; num1310++)
				{
					if (flag10)
					{
						if (Main.tile[num1308, num1310].wall == 2)
						{
							Main.tile[num1308, num1310].wall = 0;
						}
						if (Main.tile[num1308, num1310].type != 53)
						{
							if (Main.tile[num1308 - 1, num1310].wall == 2)
							{
								Main.tile[num1308 - 1, num1310].wall = 0;
							}
							if (Main.tile[num1308 - 2, num1310].wall == 2 && WorldGen.genRand.Next(2) == 0)
							{
								Main.tile[num1308 - 2, num1310].wall = 0;
							}
							if (Main.tile[num1308 - 3, num1310].wall == 2 && WorldGen.genRand.Next(2) == 0)
							{
								Main.tile[num1308 - 3, num1310].wall = 0;
							}
							if (Main.tile[num1308 + 1, num1310].wall == 2)
							{
								Main.tile[num1308 + 1, num1310].wall = 0;
							}
							if (Main.tile[num1308 + 2, num1310].wall == 2 && WorldGen.genRand.Next(2) == 0)
							{
								Main.tile[num1308 + 2, num1310].wall = 0;
							}
							if (Main.tile[num1308 + 3, num1310].wall == 2 && WorldGen.genRand.Next(2) == 0)
							{
								Main.tile[num1308 + 3, num1310].wall = 0;
							}
							if (Main.tile[num1308, num1310].HasTile)
							{
								flag10 = false;
							}
						}
					}
					else if (Main.tile[num1308, num1310].wall == 0 && Main.tile[num1308, num1310 + 1].wall == 0 && Main.tile[num1308, num1310 + 2].wall == 0 && Main.tile[num1308, num1310 + 3].wall == 0 && Main.tile[num1308, num1310 + 4].wall == 0 && Main.tile[num1308 - 1, num1310].wall == 0 && Main.tile[num1308 + 1, num1310].wall == 0 && Main.tile[num1308 - 2, num1310].wall == 0 && Main.tile[num1308 + 2, num1310].wall == 0 && !Main.tile[num1308, num1310].HasTile && !Main.tile[num1308, num1310 + 1].HasTile && !Main.tile[num1308, num1310 + 2].HasTile && !Main.tile[num1308, num1310 + 3].HasTile)
					{
						flag10 = true;
					}
				}
			}
			for (int num1311 = 0; num1311 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05); num1311++)
			{
				float num1312 = (float)((double)num1311 / ((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05));
				Main.statusText = Lang.gen[26]?.ToString() + " " + (int)(num1312 * 100f + 1f) + "%";
				bool flag11 = false;
				int num1313 = 0;
				while (!flag11)
				{
					int x4 = WorldGen.genRand.Next(1, Main.maxTilesX);
					int y2 = (int)(num6 + 20.0);
					Place3x2(x4, y2, 26);
					if (Main.tile[x4, y2].type == 26)
					{
						flag11 = true;
					}
					else
					{
						num1313++;
						if (num1313 >= 10000)
						{
							flag11 = true;
						}
					}
				}
			}
			for (int num1314 = 0; num1314 < Main.maxTilesX; num1314++)
			{
				num1133 = num1314;
				for (int num1315 = (int)num5; (double)num1315 < Main.worldSurface - 1.0; num1315++)
				{
					if (Main.tile[num1133, num1315].HasTile)
					{
						if (Main.tile[num1133, num1315].type == 60)
						{
							Main.tile[num1133, num1315 - 1].liquid = byte.MaxValue;
							Main.tile[num1133, num1315 - 2].liquid = byte.MaxValue;
						}
						break;
					}
				}
			}
			for (int num1316 = 400; num1316 < Main.maxTilesX - 400; num1316++)
			{
				num1133 = num1316;
				for (int num1317 = (int)num5; (double)num1317 < Main.worldSurface - 1.0; num1317++)
				{
					if (Main.tile[num1133, num1317].HasTile)
					{
						if (Main.tile[num1133, num1317].type == 53)
						{
							int num1318 = num1317;
							while ((double)num1318 > num5)
							{
								num1318--;
								Main.tile[num1133, num1318].liquid = 0;
							}
						}
						break;
					}
				}
			}
			Liquid.QuickWater(3);
			WorldGen.WaterCheck();
			int num1319 = 0;
			Liquid.quickSettle = true;
			while (num1319 < 10)
			{
				int num1320 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
				num1319++;
				float num1321 = 0f;
				while (Liquid.numLiquid > 0)
				{
					float num1322 = (float)(num1320 - (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer)) / (float)num1320;
					if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > num1320)
					{
						num1320 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
					}
					if (num1322 > num1321)
					{
						num1321 = num1322;
					}
					else
					{
						num1322 = num1321;
					}
					if (num1319 == 1)
					{
						Main.statusText = Lang.gen[27]?.ToString() + " " + (int)(num1322 * 100f / 3f + 33f) + "%";
					}
					int num1323 = 10;
					if (num1319 > num1323)
					{
						num1323 = num1319;
					}
					Liquid.UpdateLiquid();
				}
				WorldGen.WaterCheck();
				Main.statusText = Lang.gen[27]?.ToString() + " " + (int)((float)num1319 * 10f / 3f + 66f) + "%";
			}
			Liquid.quickSettle = false;
			float num1324 = Main.maxTilesX / 4200;
			for (int num1325 = 0; num1325 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05); num1325++)
			{
				float num1326 = (float)((double)num1325 / ((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05));
				Main.statusText = Lang.gen[28]?.ToString() + " " + (int)(num1326 * 100f + 1f) + "%";
				bool flag12 = false;
				int num1327 = 0;
				while (!flag12)
				{
					if (AddLifeCrystal(WorldGen.genRand.Next(1, Main.maxTilesX), WorldGen.genRand.Next((int)(num6 + 20.0), Main.maxTilesY)))
					{
						flag12 = true;
					}
					else
					{
						num1327++;
						if (num1327 >= 10000)
						{
							flag12 = true;
						}
					}
				}
			}
			int num1328 = 0;
			for (int num1329 = 0; (float)num1329 < 82f * num1324; num1329++)
			{
				if (num1328 > 41)
				{
					num1328 = 0;
				}
				float num1330 = (float)num1329 / (200f * num1324);
				Main.statusText = Lang.gen[29]?.ToString() + " " + (int)(num1330 * 100f + 1f) + "%";
				bool flag13 = false;
				int num1331 = 0;
				while (!flag13)
				{
					int num1332 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
					int num1333;
					for (num1333 = WorldGen.genRand.Next((int)(num6 + 20.0), Main.maxTilesY - 300); !Main.tile[num1332, num1333].HasTile; num1333++)
					{
					}
					num1333--;
					PlaceTile(num1332, num1333, 105, mute: true, forced: true, -1, num1328);
					if (Main.tile[num1332, num1333].HasTile && Main.tile[num1332, num1333].type == 105)
					{
						flag13 = true;
						num1328++;
					}
					else
					{
						num1331++;
						if (num1331 >= 10000)
						{
							flag13 = true;
						}
					}
				}
			}
			for (int num1334 = 0; num1334 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 1.6E-05); num1334++)
			{
				float num1335 = (float)((double)num1334 / ((double)(Main.maxTilesX * Main.maxTilesY) * 1.6E-05));
				Main.statusText = Lang.gen[30]?.ToString() + " " + (int)(num1335 * 100f + 1f) + "%";
				bool flag14 = false;
				int num1336 = 0;
				while (!flag14)
				{
					int num1337 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
					int num1338 = WorldGen.genRand.Next((int)(num6 + 20.0), Main.maxTilesY - 230);
					if ((float)num1334 <= 3f * num1324)
					{
						num1338 = WorldGen.genRand.Next(Main.maxTilesY - 200, Main.maxTilesY - 50);
					}
					while (Main.tile[num1337, num1338].wall == 7 || Main.tile[num1337, num1338].wall == 8 || Main.tile[num1337, num1338].wall == 9)
					{
						num1337 = WorldGen.genRand.Next(1, Main.maxTilesX);
						num1338 = WorldGen.genRand.Next((int)(num6 + 20.0), Main.maxTilesY - 230);
						if (num1334 <= 3)
						{
							num1338 = WorldGen.genRand.Next(Main.maxTilesY - 200, Main.maxTilesY - 50);
						}
					}
					if (AddBuriedChest(num1337, num1338))
					{
						flag14 = true;
						if (WorldGen.genRand.Next(2) == 0)
						{
							int num1339;
							for (num1339 = num1338; Main.tile[num1337, num1339].type != 21 && num1339 < Main.maxTilesY - 300; num1339++)
							{
							}
							if (num1338 < Main.maxTilesY - 300)
							{
								MineHouse(num1337, num1339);
							}
						}
					}
					else
					{
						num1336++;
						if (num1336 >= 5000)
						{
							flag14 = true;
						}
					}
				}
			}
			for (int num1340 = 0; num1340 < (int)((double)Main.maxTilesX * 0.005); num1340++)
			{
				float num1341 = (float)((double)num1340 / ((double)Main.maxTilesX * 0.005));
				Main.statusText = Lang.gen[31]?.ToString() + " " + (int)(num1341 * 100f + 1f) + "%";
				bool flag15 = false;
				int num1342 = 0;
				while (!flag15)
				{
					int num1343 = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
					int num1344 = WorldGen.genRand.Next((int)num5, (int)Main.worldSurface);
					bool flag16 = false;
					if (Main.tile[num1343, num1344].wall == 2 && !Main.tile[num1343, num1344].HasTile)
					{
						flag16 = true;
					}
					if (flag16 && AddBuriedChest(num1343, num1344, 0, notNearOtherChests: true))
					{
						flag15 = true;
					}
					else
					{
						num1342++;
						if (num1342 >= 2000)
						{
							flag15 = true;
						}
					}
				}
			}
			int num1345 = 0;
			for (int num1346 = 0; num1346 < numJChests; num1346++)
			{
				float num1347 = num1346 / numJChests;
				Main.statusText = Lang.gen[32]?.ToString() + " " + (int)(num1347 * 100f + 1f) + "%";
				num1345++;
				int contain = 211;
				switch (num1345)
				{
				case 1:
					contain = 211;
					break;
				case 2:
					contain = 212;
					break;
				case 3:
					contain = 213;
					break;
				}
				if (num1345 > 3)
				{
					num1345 = 0;
				}
				if (!WorldGen.InWorld(JChestX[num1346], JChestY[num1346], 20) || !AddBuriedChest(JChestX[num1346] + WorldGen.genRand.Next(2), JChestY[num1346], contain))
				{
					for (int num1348 = JChestX[num1346]; num1348 <= JChestX[num1346] + 1; num1348++)
					{
						for (int num1349 = JChestY[num1346]; num1349 <= JChestY[num1346] + 1; num1349++)
						{
							KillTile(num1348, num1349);
						}
					}
					if (WorldGen.InWorld(JChestX[num1346], JChestY[num1346], 20))
					{
						AddBuriedChest(JChestX[num1346], JChestY[num1346], contain);
					}
				}
			}
			int num1350 = 0;
			for (int num1351 = 0; (float)num1351 < 9f * num1324; num1351++)
			{
				float num1352 = (float)num1351 / (9f * num1324);
				Main.statusText = Lang.gen[33]?.ToString() + " " + (int)(num1352 * 100f + 1f) + "%";
				int num1353 = 0;
				num1350++;
				switch (num1350)
				{
				case 1:
					num1353 = 186;
					break;
				case 2:
					num1353 = 277;
					break;
				default:
					num1353 = 187;
					num1350 = 0;
					break;
				}
				bool flag17 = false;
				while (!flag17)
				{
					int num1354 = WorldGen.genRand.Next(1, Main.maxTilesX);
					int num1355 = WorldGen.genRand.Next(1, Main.maxTilesY - 200);
					while (Main.tile[num1354, num1355].liquid < 200 || Main.tile[num1354, num1355].lava())
					{
						num1354 = WorldGen.genRand.Next(1, Main.maxTilesX);
						num1355 = WorldGen.genRand.Next(1, Main.maxTilesY - 200);
					}
					flag17 = AddBuriedChest(num1354, num1355, num1353);
				}
			}
			for (int num1356 = 0; num1356 < numIslandHouses; num1356++)
			{
				IslandHouse(fihX[num1356], fihY[num1356]);
			}
			for (int num1357 = 0; num1357 < (int)((double)Main.maxTilesX * 0.05); num1357++)
			{
				float num1358 = (float)((double)num1357 / ((double)Main.maxTilesX * 0.05));
				Main.statusText = Lang.gen[34]?.ToString() + " " + (int)(num1358 * 100f + 1f) + "%";
				for (int num1359 = 0; num1359 < 1000; num1359++)
				{
					int num1360 = Main.rand.Next(200, Main.maxTilesX - 200);
					int num1361 = Main.rand.Next((int)Main.worldSurface, Main.maxTilesY - 300);
					if (Main.tile[num1360, num1361].wall == 0 && WorldGen.placeTrap(num1360, num1361))
					{
						break;
					}
				}
			}
			for (int num1362 = 0; num1362 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008); num1362++)
			{
				float num1363 = (float)((double)num1362 / ((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008));
				Main.statusText = Lang.gen[35]?.ToString() + " " + (int)(num1363 * 100f + 1f) + "%";
				bool flag18 = false;
				int num1364 = 0;
				while (!flag18)
				{
					int num1365 = WorldGen.genRand.Next((int)num6, Main.maxTilesY - 10);
					if ((double)num1363 > 0.93)
					{
						num1365 = Main.maxTilesY - 150;
					}
					else if ((double)num1363 > 0.75)
					{
						num1365 = (int)num5;
					}
					int x5 = WorldGen.genRand.Next(1, Main.maxTilesX);
					bool flag19 = false;
					for (int num1366 = num1365; num1366 < Main.maxTilesY; num1366++)
					{
						if (!flag19)
						{
							if (Main.tile[x5, num1366].HasTile && Main.tileSolid[Main.tile[x5, num1366].type] && !Main.tile[x5, num1366 - 1].lava())
							{
								flag19 = true;
							}
						}
						else
						{
							if (PlacePot(x5, num1366))
							{
								flag18 = true;
								break;
							}
							num1364++;
							if (num1364 >= 10000)
							{
								flag18 = true;
								break;
							}
						}
					}
				}
			}
			for (int num1367 = 0; num1367 < Main.maxTilesX / 200; num1367++)
			{
				float num1368 = num1367 / (Main.maxTilesX / 200);
				Main.statusText = Lang.gen[36]?.ToString() + " " + (int)(num1368 * 100f + 1f) + "%";
				bool flag20 = false;
				int num1369 = 0;
				while (!flag20)
				{
					int num1370 = WorldGen.genRand.Next(1, Main.maxTilesX);
					int num1371 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 5);
					try
					{
						if (Main.tile[num1370, num1371].wall == 13 || Main.tile[num1370, num1371].wall == 14)
						{
							for (; !Main.tile[num1370, num1371].HasTile; num1371++)
							{
							}
							num1371--;
							PlaceTile(num1370, num1371, 77);
							if (Main.tile[num1370, num1371].type == 77)
							{
								flag20 = true;
							}
							else
							{
								num1369++;
								if (num1369 >= 10000)
								{
									flag20 = true;
								}
							}
						}
					}
					catch
					{
					}
				}
			}
			Main.statusText = Lang.gen[37].Value;
			for (int num1372 = 0; num1372 < Main.maxTilesX; num1372++)
			{
				num1133 = num1372;
				bool flag21 = true;
				for (int num1373 = 0; (double)num1373 < Main.worldSurface - 1.0; num1373++)
				{
					if (Main.tile[num1133, num1373].HasTile)
					{
						if (flag21 && Main.tile[num1133, num1373].type == 0)
						{
							try
							{
								WorldGen.grassSpread = 0;
								SpreadGrass(num1133, num1373);
							}
							catch
							{
								WorldGen.grassSpread = 0;
								SpreadGrass(num1133, num1373, 0, 2, repeat: false);
							}
						}
						if ((double)num1373 > num6)
						{
							break;
						}
						flag21 = false;
					}
					else if (Main.tile[num1133, num1373].wall == 0)
					{
						flag21 = true;
					}
				}
			}
			Main.statusText = Lang.gen[38].Value;
			for (int num1374 = 5; num1374 < Main.maxTilesX - 5; num1374++)
			{
				if (WorldGen.genRand.Next(8) == 0)
				{
					for (int num1375 = 0; (double)num1375 < Main.worldSurface - 1.0; num1375++)
					{
						if (Main.tile[num1374, num1375].HasTile && Main.tile[num1374, num1375].type == 53 && !Main.tile[num1374, num1375 - 1].HasTile && Main.tile[num1374, num1375 - 1].wall == 0)
						{
							if (num1374 < 250 || num1374 > Main.maxTilesX - 250)
							{
								if (Main.tile[num1374, num1375 - 2].liquid == byte.MaxValue && Main.tile[num1374, num1375 - 3].liquid == byte.MaxValue && Main.tile[num1374, num1375 - 4].liquid == byte.MaxValue)
								{
									PlaceTile(num1374, num1375 - 1, 81, mute: true);
								}
							}
							else if (num1374 > 400 && num1374 < Main.maxTilesX - 400)
							{
								PlantCactus(num1374, num1375);
							}
						}
					}
				}
			}
			Main.statusText = Lang.gen[39].Value;
			for (int num1376 = 0; (double)num1376 < (double)Main.maxTilesX * 0.002; num1376++)
			{
				int num1377 = 0;
				int num1378 = 0;
				_ = Main.maxTilesX / 2;
				int num1379 = WorldGen.genRand.Next(Main.maxTilesX);
				num1377 = num1379 - WorldGen.genRand.Next(10) - 7;
				num1378 = num1379 + WorldGen.genRand.Next(10) + 7;
				if (num1377 < 0)
				{
					num1377 = 0;
				}
				if (num1378 > Main.maxTilesX - 1)
				{
					num1378 = Main.maxTilesX - 1;
				}
				for (int num1380 = num1377; num1380 < num1378; num1380++)
				{
					for (int num1381 = 1; (double)num1381 < Main.worldSurface - 1.0; num1381++)
					{
						if (Main.tile[num1380, num1381].type == 2 && Main.tile[num1380, num1381].HasTile && !Main.tile[num1380, num1381 - 1].HasTile)
						{
							PlaceTile(num1380, num1381 - 1, 27, mute: true);
						}
						if (Main.tile[num1380, num1381].HasTile)
						{
							break;
						}
					}
				}
			}
		});
		WorldGen.AddGenerationPass("Shimmer", delegate
		{
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			int num1087 = 50;
			int num1088 = (int)(Main.worldSurface + Main.rockLayer) / 2 + num1087;
			int num1089 = (int)((double)((Main.maxTilesY - 250) * 2) + Main.rockLayer) / 3;
			if (num1089 > Main.maxTilesY - 330 - 100 - 30)
			{
				num1089 = Main.maxTilesY - 330 - 100 - 30;
			}
			if (num1089 <= num1088)
			{
				num1089 = num1088 + 50;
			}
			int num1090 = WorldGen.genRand.Next(num1088, num1089);
			int num1091 = ((GenVars.dungeonSide < 0) ? WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.89), Main.maxTilesX - 200) : WorldGen.genRand.Next(200, (int)((double)Main.maxTilesX * 0.11)));
			int num1092 = (int)Main.worldSurface + 150;
			int num1093 = (int)(Main.rockLayer + Main.worldSurface + 200.0) / 2;
			if (num1093 <= num1092)
			{
				num1093 = num1092 + 50;
			}
			if (WorldGen.tenthAnniversaryWorldGen)
			{
				num1090 = WorldGen.genRand.Next(num1092, num1093);
			}
			int num1094 = 0;
			while (!WorldGen.ShimmerMakeBiome(num1091, num1090))
			{
				num1094++;
				if (WorldGen.tenthAnniversaryWorldGen && num1094 < 10000)
				{
					num1090 = WorldGen.genRand.Next(num1092, num1093);
					num1091 = ((GenVars.dungeonSide < 0) ? WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.89), Main.maxTilesX - 200) : WorldGen.genRand.Next(200, (int)((double)Main.maxTilesX * 0.11)));
				}
				else if (num1094 > 20000)
				{
					num1090 = WorldGen.genRand.Next((int)Main.worldSurface + 100 + 20, num1089);
					num1091 = ((GenVars.dungeonSide < 0) ? WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.8), Main.maxTilesX - 200) : WorldGen.genRand.Next(200, (int)((double)Main.maxTilesX * 0.2)));
				}
				else
				{
					num1090 = WorldGen.genRand.Next((int)(Main.worldSurface + Main.rockLayer) / 2 + 20, num1089);
					num1091 = ((GenVars.dungeonSide < 0) ? WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.89), Main.maxTilesX - 200) : WorldGen.genRand.Next(200, (int)((double)Main.maxTilesX * 0.11)));
				}
			}
			GenVars.shimmerPosition = new Vector2D((double)num1091, (double)num1090);
			int num1095 = 200;
			GenVars.structures.AddProtectedStructure(new Rectangle(num1091 - num1095 / 2, num1090 - num1095 / 2, num1095, num1095));
		});
		WorldGen.AddGenerationPass("Jungle Temple", delegate(GenerationProgress progress, GameConfiguration passConfig)
		{
			int num1087 = 0;
			progress.Message = Lang.gen[70].Value;
			long num1088 = 0L;
			double num1089 = 0.25;
			bool flag = false;
			while (true)
			{
				int num1090 = (int)Main.rockLayer;
				int num1091 = Main.maxTilesY - 500;
				if (num1090 > num1091 - 1)
				{
					num1090 = num1091 - 1;
				}
				int num1092 = WorldGen.genRand.Next(num1090, num1091);
				int x = (int)(((WorldGen.genRand.NextDouble() * num1089 + 0.1) * (double)(-GenVars.dungeonSide) + 0.5) * (double)Main.maxTilesX);
				if (WorldGen.remixWorldGen)
				{
					if (WorldGen.notTheBees)
					{
						x = ((GenVars.dungeonSide <= 0) ? WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.6), (int)((double)Main.maxTilesX * 0.8)) : WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.2), (int)((double)Main.maxTilesX * 0.4)));
					}
					else
					{
						x = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.2), (int)((double)Main.maxTilesX * 0.8));
						while ((double)x > (double)Main.maxTilesX * 0.4 && (double)x < (double)Main.maxTilesX * 0.6)
						{
							x = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.2), (int)((double)Main.maxTilesX * 0.8));
						}
					}
					while (Main.tile[x, num1092].active() || Main.tile[x, num1092].wall > 0 || (double)num1092 > Main.worldSurface - 5.0)
					{
						num1092--;
					}
					num1092++;
					if (Main.tile[x, num1092].active() && (Main.tile[x, num1092].type == 60 || Main.tile[x, num1092].type == 59))
					{
						int num1093 = 10;
						bool flag2 = false;
						for (int i = x - num1093; i <= i + num1093; i++)
						{
							for (int j = num1092 - num1093; j < num1093; j++)
							{
								if (Main.tile[i, j].type == 191 || Main.tileDungeon[Main.tile[i, j].type])
								{
									flag2 = true;
								}
							}
						}
						if (!flag2)
						{
							flag = true;
							num1092 -= 10 + WorldGen.genRand.Next(10);
							WorldGen.makeTemple(x, num1092);
							break;
						}
					}
				}
				else if (Main.tile[x, num1092].active() && Main.tile[x, num1092].type == 60)
				{
					flag = true;
					WorldGen.makeTemple(x, num1092);
					break;
				}
				if (num1088++ > 2000000)
				{
					if (num1089 == 0.35)
					{
						num1087++;
						if (num1087 > 10)
						{
							break;
						}
					}
					num1089 = Math.Min(0.35, num1089 + 0.05);
					num1088 = 0L;
				}
			}
			if (!flag)
			{
				int x2 = Main.maxTilesX - GenVars.dungeonX;
				int y = (int)Main.rockLayer + 100;
				if (WorldGen.remixWorldGen)
				{
					x2 = (WorldGen.notTheBees ? ((GenVars.dungeonSide > 0) ? ((int)((double)Main.maxTilesX * 0.3)) : ((int)((double)Main.maxTilesX * 0.7))) : ((GenVars.dungeonSide > 0) ? ((int)((double)Main.maxTilesX * 0.4)) : ((int)((double)Main.maxTilesX * 0.6))));
				}
				WorldGen.makeTemple(x2, y);
			}
		});
		WorldGen.AddGenerationPass("Hives", delegate(GenerationProgress progress, GameConfiguration passConfig)
		{
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			progress.Message = Lang.gen[71].Value;
			double num1087 = (double)Main.maxTilesX / 4200.0;
			double num1088 = 1 + WorldGen.genRand.Next((int)(5.0 * num1087), (int)(8.0 * num1087));
			if (WorldGen.drunkWorldGen)
			{
				num1088 *= 0.667;
			}
			int num1089 = 10000;
			HiveBiome hiveBiome = GenVars.configuration.CreateBiome<HiveBiome>();
			HoneyPatchBiome honeyPatchBiome = GenVars.configuration.CreateBiome<HoneyPatchBiome>();
			while (num1088 > 0.0 && num1089 > 0)
			{
				num1089--;
				Point val = WorldGen.RandomWorldPoint((int)(Main.worldSurface + Main.rockLayer) >> 1, 20, 300, 20);
				if (WorldGen.drunkWorldGen)
				{
					WorldGen.RandomWorldPoint((int)Main.worldSurface, 20, 300, 20);
				}
				if (hiveBiome.Place(val, GenVars.structures))
				{
					num1088--;
					int num1090 = WorldGen.genRand.Next(5);
					int num1091 = 0;
					int num1092 = 10000;
					while (num1091 < num1090 && num1092 > 0)
					{
						double num1093 = WorldGen.genRand.NextDouble() * 60.0 + 30.0;
						double num1094 = WorldGen.genRand.NextDouble() * 6.2831854820251465;
						int num1095 = (int)(Math.Cos(num1094) * num1093) + val.X;
						int num1096 = (int)(Math.Sin(num1094) * num1093) + val.Y;
						num1092--;
						if (num1095 > 50 && num1095 < Main.maxTilesX - 50 && honeyPatchBiome.Place(new Point(num1095, num1096), GenVars.structures))
						{
							num1091++;
						}
					}
				}
			}
		});
		WorldGen.AddGenerationPass("Temple", delegate
		{
			Main.tileSolid[162] = false;
			Main.tileSolid[226] = true;
			WorldGen.templePart2();
			Main.tileSolid[232] = false;
		});
		WorldGen.AddGenerationPass("Planting Trees And Stuff", delegate(GenerationProgress progress, GameConfiguration passConfig)
		{
			progress.Set(1.0);
			progress.Message = "Planting Trees And Stuff...";
			Main.statusText = Lang.gen[40].Value;
			for (int i = 0; (double)i < (double)Main.maxTilesX * 0.003; i++)
			{
				int num1087 = WorldGen.genRand.Next(50, Main.maxTilesX - 50);
				int num1088 = WorldGen.genRand.Next(25, 50);
				for (int j = num1087 - num1088; j < num1087 + num1088; j++)
				{
					for (int k = 20; (double)k < Main.worldSurface; k++)
					{
						GrowEpicTree(j, k);
					}
				}
			}
			AddTrees();
			Main.statusText = Lang.gen[41].Value;
			for (int l = 0; (double)l < (double)Main.maxTilesX * 1.7; l++)
			{
				PlantAlch();
			}
			Main.statusText = Lang.gen[42].Value;
			AddPlants();
			for (int m = 0; m < Main.maxTilesX; m++)
			{
				for (int n = 0; n < Main.maxTilesY; n++)
				{
					if (Main.tile[m, n].HasTile)
					{
						if (n >= (int)Main.worldSurface && Main.tile[m, n].type == 70 && !Main.tile[m, n - 1].HasTile)
						{
							GrowShroom(m, n);
							if (!Main.tile[m, n - 1].HasTile)
							{
								PlaceTile(m, n - 1, 71, mute: true);
							}
						}
						if (Main.tile[m, n].type == 60 && !Main.tile[m, n - 1].HasTile)
						{
							PlaceTile(m, n - 1, 61, mute: true);
						}
					}
				}
			}
		});
		WorldGen.AddGenerationPass("Adding Things", delegate(GenerationProgress progress, GameConfiguration passConfig)
		{
			progress.Set(1.0);
			progress.Message = "Adding Things...";
			for (int i = 0; i < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05); i++)
			{
				float num1087 = (float)((double)i / ((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05));
				Main.statusText = Lang.gen[28]?.ToString() + " " + (int)(num1087 * 100f + 1f) + "%";
				bool flag = false;
				int num1088 = 0;
				while (!flag)
				{
					if (AddLifeCrystal(WorldGen.genRand.Next(1, Main.maxTilesX), WorldGen.genRand.Next((int)(num6 + 20.0), Main.maxTilesY)))
					{
						flag = true;
					}
					else
					{
						num1088++;
						if (num1088 >= 10000)
						{
							flag = true;
						}
					}
				}
			}
			int num1089 = 0;
			float num1090 = Main.maxTilesX / 4200;
			for (int j = 0; (float)j < 82f * num1090; j++)
			{
				if (num1089 > 41)
				{
					num1089 = 0;
				}
				float num1091 = (float)j / (200f * num1090);
				Main.statusText = Lang.gen[29]?.ToString() + " " + (int)(num1091 * 100f + 1f) + "%";
				bool flag2 = false;
				int num1092 = 0;
				while (!flag2)
				{
					int num1093 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
					int k;
					for (k = WorldGen.genRand.Next((int)(num6 + 20.0), Main.maxTilesY - 300); !Main.tile[num1093, k].HasTile; k++)
					{
					}
					k--;
					PlaceTile(num1093, k, 105, mute: true, forced: true, -1, num1089);
					if (Main.tile[num1093, k].HasTile && Main.tile[num1093, k].type == 105)
					{
						flag2 = true;
						num1089++;
					}
					else
					{
						num1092++;
						if (num1092 >= 10000)
						{
							flag2 = true;
						}
					}
				}
			}
			for (int l = 0; l < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 1.6E-05); l++)
			{
				float num1094 = (float)((double)l / ((double)(Main.maxTilesX * Main.maxTilesY) * 1.6E-05));
				Main.statusText = Lang.gen[30]?.ToString() + " " + (int)(num1094 * 100f + 1f) + "%";
				bool flag3 = false;
				int num1095 = 0;
				while (!flag3)
				{
					int num1096 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
					int num1097 = WorldGen.genRand.Next((int)(num6 + 20.0), Main.maxTilesY - 230);
					if ((float)l <= 3f * num1090)
					{
						num1097 = WorldGen.genRand.Next(Main.maxTilesY - 200, Main.maxTilesY - 50);
					}
					while (Main.tile[num1096, num1097].wall == 7 || Main.tile[num1096, num1097].wall == 8 || Main.tile[num1096, num1097].wall == 9)
					{
						num1096 = WorldGen.genRand.Next(1, Main.maxTilesX);
						num1097 = WorldGen.genRand.Next((int)(num6 + 20.0), Main.maxTilesY - 230);
						if (l <= 3)
						{
							num1097 = WorldGen.genRand.Next(Main.maxTilesY - 200, Main.maxTilesY - 50);
						}
					}
					if (AddBuriedChest(num1096, num1097))
					{
						flag3 = true;
						if (WorldGen.genRand.Next(2) == 0)
						{
							int m;
							for (m = num1097; Main.tile[num1096, m].type != 21 && m < Main.maxTilesY - 300; m++)
							{
							}
							if (num1097 < Main.maxTilesY - 300)
							{
								MineHouse(num1096, m);
							}
						}
					}
					else
					{
						num1095++;
						if (num1095 >= 5000)
						{
							flag3 = true;
						}
					}
				}
			}
			for (int n = 0; n < (int)((double)Main.maxTilesX * 0.005); n++)
			{
				float num1098 = (float)((double)n / ((double)Main.maxTilesX * 0.005));
				Main.statusText = Lang.gen[31]?.ToString() + " " + (int)(num1098 * 100f + 1f) + "%";
				bool flag4 = false;
				int num1099 = 0;
				while (!flag4)
				{
					int num1100 = WorldGen.genRand.Next(300, Main.maxTilesX - 300);
					int num1101 = WorldGen.genRand.Next((int)num5, (int)Main.worldSurface);
					bool flag5 = false;
					if (Main.tile[num1100, num1101].wall == 2 && !Main.tile[num1100, num1101].HasTile)
					{
						flag5 = true;
					}
					if (flag5 && AddBuriedChest(num1100, num1101, 0, notNearOtherChests: true))
					{
						flag4 = true;
					}
					else
					{
						num1099++;
						if (num1099 >= 2000)
						{
							flag4 = true;
						}
					}
				}
			}
			int num1102 = 0;
			for (int num1103 = 0; num1103 < numJChests; num1103++)
			{
				float num1104 = num1103 / numJChests;
				Main.statusText = Lang.gen[32]?.ToString() + " " + (int)(num1104 * 100f + 1f) + "%";
				num1102++;
				int contain = 211;
				switch (num1102)
				{
				case 1:
					contain = 211;
					break;
				case 2:
					contain = 212;
					break;
				case 3:
					contain = 213;
					break;
				}
				if (num1102 > 3)
				{
					num1102 = 0;
				}
				if (!AddBuriedChest(JChestX[num1103] + WorldGen.genRand.Next(2), JChestY[num1103], contain))
				{
					for (int num1105 = JChestX[num1103]; num1105 <= JChestX[num1103] + 1; num1105++)
					{
						for (int num1106 = JChestY[num1103]; num1106 <= JChestY[num1103] + 1; num1106++)
						{
							KillTile(num1105, num1106);
						}
					}
					AddBuriedChest(JChestX[num1103], JChestY[num1103], contain);
				}
			}
			int num1107 = 0;
			for (int num1108 = 0; (float)num1108 < 9f * num1090; num1108++)
			{
				float num1109 = (float)num1108 / (9f * num1090);
				Main.statusText = Lang.gen[33]?.ToString() + " " + (int)(num1109 * 100f + 1f) + "%";
				int num1110 = 0;
				num1107++;
				switch (num1107)
				{
				case 1:
					num1110 = 186;
					break;
				case 2:
					num1110 = 277;
					break;
				default:
					num1110 = 187;
					num1107 = 0;
					break;
				}
				bool flag6 = false;
				while (!flag6)
				{
					int num1111 = WorldGen.genRand.Next(1, Main.maxTilesX);
					int num1112 = WorldGen.genRand.Next(1, Main.maxTilesY - 200);
					while (Main.tile[num1111, num1112].liquid < 200 || Main.tile[num1111, num1112].lava())
					{
						num1111 = WorldGen.genRand.Next(1, Main.maxTilesX);
						num1112 = WorldGen.genRand.Next(1, Main.maxTilesY - 200);
					}
					flag6 = AddBuriedChest(num1111, num1112, num1110);
				}
			}
			for (int num1113 = 0; num1113 < numIslandHouses; num1113++)
			{
				IslandHouse(fihX[num1113], fihY[num1113]);
			}
			for (int num1114 = 0; num1114 < (int)((double)Main.maxTilesX * 0.05); num1114++)
			{
				float num1115 = (float)((double)num1114 / ((double)Main.maxTilesX * 0.05));
				Main.statusText = Lang.gen[34]?.ToString() + " " + (int)(num1115 * 100f + 1f) + "%";
				for (int num1116 = 0; num1116 < 1000; num1116++)
				{
					int num1117 = Main.rand.Next(200, Main.maxTilesX - 200);
					int num1118 = Main.rand.Next((int)Main.worldSurface, Main.maxTilesY - 300);
					if (Main.tile[num1117, num1118].wall == 0 && WorldGen.placeTrap(num1117, num1118))
					{
						break;
					}
				}
			}
			for (int num1119 = 0; num1119 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008); num1119++)
			{
				float num1120 = (float)((double)num1119 / ((double)(Main.maxTilesX * Main.maxTilesY) * 0.0008));
				Main.statusText = Lang.gen[35]?.ToString() + " " + (int)(num1120 * 100f + 1f) + "%";
				bool flag7 = false;
				int num1121 = 0;
				while (!flag7)
				{
					int num1122 = WorldGen.genRand.Next((int)num6, Main.maxTilesY - 10);
					if ((double)num1120 > 0.93)
					{
						num1122 = Main.maxTilesY - 150;
					}
					else if ((double)num1120 > 0.75)
					{
						num1122 = (int)num5;
					}
					int x = WorldGen.genRand.Next(1, Main.maxTilesX);
					bool flag8 = false;
					for (int num1123 = num1122; num1123 < Main.maxTilesY; num1123++)
					{
						if (!flag8)
						{
							if (Main.tile[x, num1123].HasTile && Main.tileSolid[Main.tile[x, num1123].type] && !Main.tile[x, num1123 - 1].lava())
							{
								flag8 = true;
							}
						}
						else
						{
							if (PlacePot(x, num1123))
							{
								flag7 = true;
								break;
							}
							num1121++;
							if (num1121 >= 10000)
							{
								flag7 = true;
								break;
							}
						}
					}
				}
			}
			for (int num1124 = 0; num1124 < Main.maxTilesX / 200; num1124++)
			{
				float num1125 = num1124 / (Main.maxTilesX / 200);
				Main.statusText = Lang.gen[36]?.ToString() + " " + (int)(num1125 * 100f + 1f) + "%";
				bool flag9 = false;
				int num1126 = 0;
				while (!flag9)
				{
					int num1127 = WorldGen.genRand.Next(1, Main.maxTilesX);
					int num1128 = WorldGen.genRand.Next(Main.maxTilesY - 250, Main.maxTilesY - 5);
					try
					{
						if (Main.tile[num1127, num1128].wall == 13 || Main.tile[num1127, num1128].wall == 14)
						{
							for (; !Main.tile[num1127, num1128].HasTile; num1128++)
							{
							}
							num1128--;
							PlaceTile(num1127, num1128, 77);
							if (Main.tile[num1127, num1128].type == 77)
							{
								flag9 = true;
							}
							else
							{
								num1126++;
								if (num1126 >= 10000)
								{
									flag9 = true;
								}
							}
						}
					}
					catch
					{
					}
				}
			}
			Main.statusText = Lang.gen[37].Value;
			int num1129 = 0;
			for (int num1130 = 0; num1130 < Main.maxTilesX; num1130++)
			{
				num1129 = num1130;
				bool flag10 = true;
				for (int num1131 = 0; (double)num1131 < Main.worldSurface - 1.0; num1131++)
				{
					if (Main.tile[num1129, num1131].HasTile)
					{
						if (flag10 && Main.tile[num1129, num1131].type == 0)
						{
							try
							{
								WorldGen.grassSpread = 0;
								SpreadGrass(num1129, num1131);
							}
							catch
							{
								WorldGen.grassSpread = 0;
								SpreadGrass(num1129, num1131, 0, 2, repeat: false);
							}
						}
						if ((double)num1131 > num6)
						{
							break;
						}
						flag10 = false;
					}
					else if (Main.tile[num1129, num1131].wall == 0)
					{
						flag10 = true;
					}
				}
			}
			Main.statusText = Lang.gen[38].Value;
			for (int num1132 = 5; num1132 < Main.maxTilesX - 5; num1132++)
			{
				if (WorldGen.genRand.Next(8) == 0)
				{
					for (int num1133 = 0; (double)num1133 < Main.worldSurface - 1.0; num1133++)
					{
						if (Main.tile[num1132, num1133].HasTile && Main.tile[num1132, num1133].type == 53 && !Main.tile[num1132, num1133 - 1].HasTile && Main.tile[num1132, num1133 - 1].wall == 0)
						{
							if (num1132 < 250 || num1132 > Main.maxTilesX - 250)
							{
								if (Main.tile[num1132, num1133 - 2].liquid == byte.MaxValue && Main.tile[num1132, num1133 - 3].liquid == byte.MaxValue && Main.tile[num1132, num1133 - 4].liquid == byte.MaxValue)
								{
									PlaceTile(num1132, num1133 - 1, 81, mute: true);
								}
							}
							else if (num1132 > 400 && num1132 < Main.maxTilesX - 400)
							{
								PlantCactus(num1132, num1133);
							}
						}
					}
				}
			}
		});
		WorldGen.AddGenerationPass("Spawn Point", delegate(GenerationProgress progress, GameConfiguration passConfig)
		{
			progress.Set(1.0);
			int num1087 = 5;
			bool flag = true;
			while (flag)
			{
				int num1088 = Main.maxTilesX / 2 + WorldGen.genRand.Next(-num1087, num1087 + 1);
				for (int i = 0; i < Main.maxTilesY; i++)
				{
					if (Main.tile[num1088, i].HasTile)
					{
						Main.spawnTileX = num1088;
						Main.spawnTileY = i;
						break;
					}
				}
				flag = false;
				num1087++;
				if ((double)Main.spawnTileY > Main.worldSurface)
				{
					flag = true;
				}
				if (Main.tile[Main.spawnTileX, Main.spawnTileY - 1].liquid > 0)
				{
					flag = true;
				}
			}
			int num1089 = 10;
			while ((double)Main.spawnTileY > Main.worldSurface)
			{
				int num1090 = WorldGen.genRand.Next(Main.maxTilesX / 2 - num1089, Main.maxTilesX / 2 + num1089);
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					if (Main.tile[num1090, j].HasTile)
					{
						Main.spawnTileX = num1090;
						Main.spawnTileY = j;
						break;
					}
				}
				num1089++;
			}
			int num1091 = NPC.NewNPC(null, Main.spawnTileX * 16, Main.spawnTileY * 16, 22);
			Main.npc[num1091].homeTileX = Main.spawnTileX;
			Main.npc[num1091].homeTileY = Main.spawnTileY;
			Main.npc[num1091].direction = 1;
			Main.npc[num1091].homeless = true;
		});
		WorldGen.AddGenerationPass("Finalizing", delegate(GenerationProgress progress, GameConfiguration passConfig)
		{
			progress.Message = "Finalizing";
			Main.statusText = Lang.gen[43].Value;
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				int num1087 = 0;
				for (int j = 0; (double)j < Main.worldSurface; j++)
				{
					if (num1087 > 0 && !Main.tile[i, j].HasTile)
					{
						Main.tile[i, j].active(active: true);
						Main.tile[i, j].type = 52;
						num1087--;
					}
					else
					{
						num1087 = 0;
					}
					if (Main.tile[i, j].HasTile && Main.tile[i, j].type == 2 && WorldGen.genRand.Next(5) < 3)
					{
						num1087 = WorldGen.genRand.Next(1, 10);
					}
				}
				num1087 = 0;
				for (int k = 0; k < Main.maxTilesY; k++)
				{
					if (num1087 > 0 && !Main.tile[i, k].HasTile)
					{
						Main.tile[i, k].active(active: true);
						Main.tile[i, k].type = 62;
						num1087--;
					}
					else
					{
						num1087 = 0;
					}
					if (Main.tile[i, k].HasTile && Main.tile[i, k].type == 60 && WorldGen.genRand.Next(5) < 3)
					{
						num1087 = WorldGen.genRand.Next(1, 10);
					}
				}
			}
			Main.statusText = Lang.gen[44].Value;
			for (int l = 0; (double)l < (double)Main.maxTilesX * 0.005; l++)
			{
				int num1088 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
				int num1089 = WorldGen.genRand.Next(5, 15);
				int num1090 = WorldGen.genRand.Next(15, 30);
				for (int m = 1; (double)m < Main.worldSurface - 1.0; m++)
				{
					if (Main.tile[num1088, m].HasTile)
					{
						for (int n = num1088 - num1089; n < num1088 + num1089; n++)
						{
							for (int num1091 = m - num1090; num1091 < m + num1090; num1091++)
							{
								if (Main.tile[n, num1091].type == 3 || Main.tile[n, num1091].type == 24)
								{
									Main.tile[n, num1091].frameX = (short)(WorldGen.genRand.Next(6, 8) * 18);
								}
							}
						}
						break;
					}
				}
			}
			Main.statusText = Lang.gen[45].Value;
			for (int num1092 = 0; (double)num1092 < (double)Main.maxTilesX * 0.002; num1092++)
			{
				int num1093 = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
				int num1094 = WorldGen.genRand.Next(4, 10);
				int num1095 = WorldGen.genRand.Next(15, 30);
				for (int num1096 = 1; (double)num1096 < Main.worldSurface - 1.0; num1096++)
				{
					if (Main.tile[num1093, num1096].HasTile)
					{
						for (int num1097 = num1093 - num1094; num1097 < num1093 + num1094; num1097++)
						{
							for (int num1098 = num1096 - num1095; num1098 < num1096 + num1095; num1098++)
							{
								if (Main.tile[num1097, num1098].type == 3 || Main.tile[num1097, num1098].type == 24)
								{
									Main.tile[num1097, num1098].frameX = 144;
								}
							}
						}
						break;
					}
				}
			}
			Main.AnglerQuestSwap();
			WorldGen.skipFramingDuringGen = false;
			WorldGen.noTileActions = false;
			progress.Message = Lang.gen[87].Value;
			WorldGen.gen = false;
		});
		WorldGen._generator.GenerateWorld(customProgressObject);
		Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
		Main.NotifyOfEvent(GameNotificationType.WorldGen);
		WorldGen.drunkWorldGenText = false;
	}

	public static bool GrowEpicTree(int i, int y)
	{
		int j;
		for (j = y; Main.tile[i, j].type == 20; j++)
		{
		}
		if (Main.tile[i, j].HasTile && Main.tile[i, j].type == 2 && Main.tile[i, j - 1].wall == 0 && Main.tile[i, j - 1].liquid == 0 && ((Main.tile[i - 1, j].HasTile && (Main.tile[i - 1, j].type == 2 || Main.tile[i - 1, j].type == 23 || Main.tile[i - 1, j].type == 60 || Main.tile[i - 1, j].type == 109)) || (Main.tile[i + 1, j].HasTile && (Main.tile[i + 1, j].type == 2 || Main.tile[i + 1, j].type == 23 || Main.tile[i + 1, j].type == 60 || Main.tile[i + 1, j].type == 109))))
		{
			int num = 1;
			if (EmptyTileCheck(i - num, i + num, j - 55, j - 1, 20))
			{
				bool flag = false;
				bool flag2 = false;
				int num2 = WorldGen.genRand.Next(20, 30);
				int num3;
				for (int k = j - num2; k < j; k++)
				{
					Main.tile[i, k].frameNumber((byte)WorldGen.genRand.Next(3));
					Main.tile[i, k].active(active: true);
					Main.tile[i, k].type = 5;
					num3 = WorldGen.genRand.Next(3);
					int num4 = WorldGen.genRand.Next(10);
					if (k == j - 1 || k == j - num2)
					{
						num4 = 0;
					}
					while (((num4 == 5 || num4 == 7) & flag) || ((num4 == 6 || num4 == 7) & flag2))
					{
						num4 = WorldGen.genRand.Next(10);
					}
					flag = false;
					flag2 = false;
					if (num4 == 5 || num4 == 7)
					{
						flag = true;
					}
					if (num4 == 6 || num4 == 7)
					{
						flag2 = true;
					}
					switch (num4)
					{
					case 1:
						if (num3 == 0)
						{
							Main.tile[i, k].frameX = 0;
							Main.tile[i, k].frameY = 66;
						}
						if (num3 == 1)
						{
							Main.tile[i, k].frameX = 0;
							Main.tile[i, k].frameY = 88;
						}
						if (num3 == 2)
						{
							Main.tile[i, k].frameX = 0;
							Main.tile[i, k].frameY = 110;
						}
						break;
					case 2:
						if (num3 == 0)
						{
							Main.tile[i, k].frameX = 22;
							Main.tile[i, k].frameY = 0;
						}
						if (num3 == 1)
						{
							Main.tile[i, k].frameX = 22;
							Main.tile[i, k].frameY = 22;
						}
						if (num3 == 2)
						{
							Main.tile[i, k].frameX = 22;
							Main.tile[i, k].frameY = 44;
						}
						break;
					case 3:
						if (num3 == 0)
						{
							Main.tile[i, k].frameX = 44;
							Main.tile[i, k].frameY = 66;
						}
						if (num3 == 1)
						{
							Main.tile[i, k].frameX = 44;
							Main.tile[i, k].frameY = 88;
						}
						if (num3 == 2)
						{
							Main.tile[i, k].frameX = 44;
							Main.tile[i, k].frameY = 110;
						}
						break;
					case 4:
						if (num3 == 0)
						{
							Main.tile[i, k].frameX = 22;
							Main.tile[i, k].frameY = 66;
						}
						if (num3 == 1)
						{
							Main.tile[i, k].frameX = 22;
							Main.tile[i, k].frameY = 88;
						}
						if (num3 == 2)
						{
							Main.tile[i, k].frameX = 22;
							Main.tile[i, k].frameY = 110;
						}
						break;
					case 5:
						if (num3 == 0)
						{
							Main.tile[i, k].frameX = 88;
							Main.tile[i, k].frameY = 0;
						}
						if (num3 == 1)
						{
							Main.tile[i, k].frameX = 88;
							Main.tile[i, k].frameY = 22;
						}
						if (num3 == 2)
						{
							Main.tile[i, k].frameX = 88;
							Main.tile[i, k].frameY = 44;
						}
						break;
					case 6:
						if (num3 == 0)
						{
							Main.tile[i, k].frameX = 66;
							Main.tile[i, k].frameY = 66;
						}
						if (num3 == 1)
						{
							Main.tile[i, k].frameX = 66;
							Main.tile[i, k].frameY = 88;
						}
						if (num3 == 2)
						{
							Main.tile[i, k].frameX = 66;
							Main.tile[i, k].frameY = 110;
						}
						break;
					case 7:
						if (num3 == 0)
						{
							Main.tile[i, k].frameX = 110;
							Main.tile[i, k].frameY = 66;
						}
						if (num3 == 1)
						{
							Main.tile[i, k].frameX = 110;
							Main.tile[i, k].frameY = 88;
						}
						if (num3 == 2)
						{
							Main.tile[i, k].frameX = 110;
							Main.tile[i, k].frameY = 110;
						}
						break;
					default:
						if (num3 == 0)
						{
							Main.tile[i, k].frameX = 0;
							Main.tile[i, k].frameY = 0;
						}
						if (num3 == 1)
						{
							Main.tile[i, k].frameX = 0;
							Main.tile[i, k].frameY = 22;
						}
						if (num3 == 2)
						{
							Main.tile[i, k].frameX = 0;
							Main.tile[i, k].frameY = 44;
						}
						break;
					}
					if (num4 == 5 || num4 == 7)
					{
						Main.tile[i - 1, k].active(active: true);
						Main.tile[i - 1, k].type = 5;
						num3 = WorldGen.genRand.Next(3);
						if (WorldGen.genRand.Next(3) < 2)
						{
							if (num3 == 0)
							{
								Main.tile[i - 1, k].frameX = 44;
								Main.tile[i - 1, k].frameY = 198;
							}
							if (num3 == 1)
							{
								Main.tile[i - 1, k].frameX = 44;
								Main.tile[i - 1, k].frameY = 220;
							}
							if (num3 == 2)
							{
								Main.tile[i - 1, k].frameX = 44;
								Main.tile[i - 1, k].frameY = 242;
							}
						}
						else
						{
							if (num3 == 0)
							{
								Main.tile[i - 1, k].frameX = 66;
								Main.tile[i - 1, k].frameY = 0;
							}
							if (num3 == 1)
							{
								Main.tile[i - 1, k].frameX = 66;
								Main.tile[i - 1, k].frameY = 22;
							}
							if (num3 == 2)
							{
								Main.tile[i - 1, k].frameX = 66;
								Main.tile[i - 1, k].frameY = 44;
							}
						}
					}
					if (num4 != 6 && num4 != 7)
					{
						continue;
					}
					Main.tile[i + 1, k].active(active: true);
					Main.tile[i + 1, k].type = 5;
					num3 = WorldGen.genRand.Next(3);
					if (WorldGen.genRand.Next(3) < 2)
					{
						if (num3 == 0)
						{
							Main.tile[i + 1, k].frameX = 66;
							Main.tile[i + 1, k].frameY = 198;
						}
						if (num3 == 1)
						{
							Main.tile[i + 1, k].frameX = 66;
							Main.tile[i + 1, k].frameY = 220;
						}
						if (num3 == 2)
						{
							Main.tile[i + 1, k].frameX = 66;
							Main.tile[i + 1, k].frameY = 242;
						}
					}
					else
					{
						if (num3 == 0)
						{
							Main.tile[i + 1, k].frameX = 88;
							Main.tile[i + 1, k].frameY = 66;
						}
						if (num3 == 1)
						{
							Main.tile[i + 1, k].frameX = 88;
							Main.tile[i + 1, k].frameY = 88;
						}
						if (num3 == 2)
						{
							Main.tile[i + 1, k].frameX = 88;
							Main.tile[i + 1, k].frameY = 110;
						}
					}
				}
				int num5 = WorldGen.genRand.Next(3);
				bool flag3 = false;
				bool flag4 = false;
				if (Main.tile[i - 1, j].HasTile && (Main.tile[i - 1, j].type == 2 || Main.tile[i - 1, j].type == 23 || Main.tile[i - 1, j].type == 60 || Main.tile[i - 1, j].type == 109))
				{
					flag3 = true;
				}
				if (Main.tile[i + 1, j].HasTile && (Main.tile[i + 1, j].type == 2 || Main.tile[i + 1, j].type == 23 || Main.tile[i + 1, j].type == 60 || Main.tile[i + 1, j].type == 109))
				{
					flag4 = true;
				}
				if (!flag3)
				{
					if (num5 == 0)
					{
						num5 = 2;
					}
					if (num5 == 1)
					{
						num5 = 3;
					}
				}
				if (!flag4)
				{
					if (num5 == 0)
					{
						num5 = 1;
					}
					if (num5 == 2)
					{
						num5 = 3;
					}
				}
				if (flag3 && !flag4)
				{
					num5 = 1;
				}
				if (flag4 && !flag3)
				{
					num5 = 2;
				}
				if (num5 == 0 || num5 == 1)
				{
					Main.tile[i + 1, j - 1].active(active: true);
					Main.tile[i + 1, j - 1].type = 5;
					num3 = WorldGen.genRand.Next(3);
					if (num3 == 0)
					{
						Main.tile[i + 1, j - 1].frameX = 22;
						Main.tile[i + 1, j - 1].frameY = 132;
					}
					if (num3 == 1)
					{
						Main.tile[i + 1, j - 1].frameX = 22;
						Main.tile[i + 1, j - 1].frameY = 154;
					}
					if (num3 == 2)
					{
						Main.tile[i + 1, j - 1].frameX = 22;
						Main.tile[i + 1, j - 1].frameY = 176;
					}
				}
				if (num5 == 0 || num5 == 2)
				{
					Main.tile[i - 1, j - 1].active(active: true);
					Main.tile[i - 1, j - 1].type = 5;
					num3 = WorldGen.genRand.Next(3);
					if (num3 == 0)
					{
						Main.tile[i - 1, j - 1].frameX = 44;
						Main.tile[i - 1, j - 1].frameY = 132;
					}
					if (num3 == 1)
					{
						Main.tile[i - 1, j - 1].frameX = 44;
						Main.tile[i - 1, j - 1].frameY = 154;
					}
					if (num3 == 2)
					{
						Main.tile[i - 1, j - 1].frameX = 44;
						Main.tile[i - 1, j - 1].frameY = 176;
					}
				}
				num3 = WorldGen.genRand.Next(3);
				switch (num5)
				{
				case 0:
					if (num3 == 0)
					{
						Main.tile[i, j - 1].frameX = 88;
						Main.tile[i, j - 1].frameY = 132;
					}
					if (num3 == 1)
					{
						Main.tile[i, j - 1].frameX = 88;
						Main.tile[i, j - 1].frameY = 154;
					}
					if (num3 == 2)
					{
						Main.tile[i, j - 1].frameX = 88;
						Main.tile[i, j - 1].frameY = 176;
					}
					break;
				case 1:
					if (num3 == 0)
					{
						Main.tile[i, j - 1].frameX = 0;
						Main.tile[i, j - 1].frameY = 132;
					}
					if (num3 == 1)
					{
						Main.tile[i, j - 1].frameX = 0;
						Main.tile[i, j - 1].frameY = 154;
					}
					if (num3 == 2)
					{
						Main.tile[i, j - 1].frameX = 0;
						Main.tile[i, j - 1].frameY = 176;
					}
					break;
				case 2:
					if (num3 == 0)
					{
						Main.tile[i, j - 1].frameX = 66;
						Main.tile[i, j - 1].frameY = 132;
					}
					if (num3 == 1)
					{
						Main.tile[i, j - 1].frameX = 66;
						Main.tile[i, j - 1].frameY = 154;
					}
					if (num3 == 2)
					{
						Main.tile[i, j - 1].frameX = 66;
						Main.tile[i, j - 1].frameY = 176;
					}
					break;
				}
				if (WorldGen.genRand.Next(3) < 2)
				{
					num3 = WorldGen.genRand.Next(3);
					if (num3 == 0)
					{
						Main.tile[i, j - num2].frameX = 22;
						Main.tile[i, j - num2].frameY = 198;
					}
					if (num3 == 1)
					{
						Main.tile[i, j - num2].frameX = 22;
						Main.tile[i, j - num2].frameY = 220;
					}
					if (num3 == 2)
					{
						Main.tile[i, j - num2].frameX = 22;
						Main.tile[i, j - num2].frameY = 242;
					}
				}
				else
				{
					num3 = WorldGen.genRand.Next(3);
					if (num3 == 0)
					{
						Main.tile[i, j - num2].frameX = 0;
						Main.tile[i, j - num2].frameY = 198;
					}
					if (num3 == 1)
					{
						Main.tile[i, j - num2].frameX = 0;
						Main.tile[i, j - num2].frameY = 220;
					}
					if (num3 == 2)
					{
						Main.tile[i, j - num2].frameX = 0;
						Main.tile[i, j - num2].frameY = 242;
					}
				}
				WorldGen.RangeFrame(i - 2, j - num2 - 1, i + 2, j + 1);
				if (Main.netMode == 2)
				{
					NetMessage.SendTileSquare(-1, i, (int)((double)j - (double)num2 * 0.5), num2 + 1);
				}
				return true;
			}
		}
		return false;
	}

	public static void GrowTree(int i, int y)
	{
		int j;
		for (j = y; Main.tile[i, j].type == 20; j++)
		{
		}
		if (((Main.tile[i - 1, j - 1].liquid != 0 || Main.tile[i - 1, j - 1].liquid != 0 || Main.tile[i + 1, j - 1].liquid != 0) && Main.tile[i, j].type != 60) || !Main.tile[i, j].HasTile || (Main.tile[i, j].type != 2 && Main.tile[i, j].type != 23 && Main.tile[i, j].type != 60 && Main.tile[i, j].type != 109 && Main.tile[i, j].type != 147) || Main.tile[i, j - 1].wall != 0 || ((!Main.tile[i - 1, j].HasTile || (Main.tile[i - 1, j].type != 2 && Main.tile[i - 1, j].type != 23 && Main.tile[i - 1, j].type != 60 && Main.tile[i - 1, j].type != 109 && Main.tile[i - 1, j].type != 147)) && (!Main.tile[i + 1, j].HasTile || (Main.tile[i + 1, j].type != 2 && Main.tile[i + 1, j].type != 23 && Main.tile[i + 1, j].type != 60 && Main.tile[i + 1, j].type != 109 && Main.tile[i + 1, j].type != 147))))
		{
			return;
		}
		int num = 1;
		int num2 = 16;
		if (Main.tile[i, j].type == 60)
		{
			num2 += 5;
		}
		if (!EmptyTileCheck(i - num, i + num, j - num2, j - 1, 20))
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		int num3 = WorldGen.genRand.Next(5, num2 + 1);
		int num4;
		for (int k = j - num3; k < j; k++)
		{
			Main.tile[i, k].frameNumber((byte)WorldGen.genRand.Next(3));
			Main.tile[i, k].active(active: true);
			Main.tile[i, k].type = 5;
			num4 = WorldGen.genRand.Next(3);
			int num5 = WorldGen.genRand.Next(10);
			if (k == j - 1 || k == j - num3)
			{
				num5 = 0;
			}
			while (((num5 == 5 || num5 == 7) & flag) || ((num5 == 6 || num5 == 7) & flag2))
			{
				num5 = WorldGen.genRand.Next(10);
			}
			flag = false;
			flag2 = false;
			if (num5 == 5 || num5 == 7)
			{
				flag = true;
			}
			if (num5 == 6 || num5 == 7)
			{
				flag2 = true;
			}
			switch (num5)
			{
			case 1:
				if (num4 == 0)
				{
					Main.tile[i, k].frameX = 0;
					Main.tile[i, k].frameY = 66;
				}
				if (num4 == 1)
				{
					Main.tile[i, k].frameX = 0;
					Main.tile[i, k].frameY = 88;
				}
				if (num4 == 2)
				{
					Main.tile[i, k].frameX = 0;
					Main.tile[i, k].frameY = 110;
				}
				break;
			case 2:
				if (num4 == 0)
				{
					Main.tile[i, k].frameX = 22;
					Main.tile[i, k].frameY = 0;
				}
				if (num4 == 1)
				{
					Main.tile[i, k].frameX = 22;
					Main.tile[i, k].frameY = 22;
				}
				if (num4 == 2)
				{
					Main.tile[i, k].frameX = 22;
					Main.tile[i, k].frameY = 44;
				}
				break;
			case 3:
				if (num4 == 0)
				{
					Main.tile[i, k].frameX = 44;
					Main.tile[i, k].frameY = 66;
				}
				if (num4 == 1)
				{
					Main.tile[i, k].frameX = 44;
					Main.tile[i, k].frameY = 88;
				}
				if (num4 == 2)
				{
					Main.tile[i, k].frameX = 44;
					Main.tile[i, k].frameY = 110;
				}
				break;
			case 4:
				if (num4 == 0)
				{
					Main.tile[i, k].frameX = 22;
					Main.tile[i, k].frameY = 66;
				}
				if (num4 == 1)
				{
					Main.tile[i, k].frameX = 22;
					Main.tile[i, k].frameY = 88;
				}
				if (num4 == 2)
				{
					Main.tile[i, k].frameX = 22;
					Main.tile[i, k].frameY = 110;
				}
				break;
			case 5:
				if (num4 == 0)
				{
					Main.tile[i, k].frameX = 88;
					Main.tile[i, k].frameY = 0;
				}
				if (num4 == 1)
				{
					Main.tile[i, k].frameX = 88;
					Main.tile[i, k].frameY = 22;
				}
				if (num4 == 2)
				{
					Main.tile[i, k].frameX = 88;
					Main.tile[i, k].frameY = 44;
				}
				break;
			case 6:
				if (num4 == 0)
				{
					Main.tile[i, k].frameX = 66;
					Main.tile[i, k].frameY = 66;
				}
				if (num4 == 1)
				{
					Main.tile[i, k].frameX = 66;
					Main.tile[i, k].frameY = 88;
				}
				if (num4 == 2)
				{
					Main.tile[i, k].frameX = 66;
					Main.tile[i, k].frameY = 110;
				}
				break;
			case 7:
				if (num4 == 0)
				{
					Main.tile[i, k].frameX = 110;
					Main.tile[i, k].frameY = 66;
				}
				if (num4 == 1)
				{
					Main.tile[i, k].frameX = 110;
					Main.tile[i, k].frameY = 88;
				}
				if (num4 == 2)
				{
					Main.tile[i, k].frameX = 110;
					Main.tile[i, k].frameY = 110;
				}
				break;
			default:
				if (num4 == 0)
				{
					Main.tile[i, k].frameX = 0;
					Main.tile[i, k].frameY = 0;
				}
				if (num4 == 1)
				{
					Main.tile[i, k].frameX = 0;
					Main.tile[i, k].frameY = 22;
				}
				if (num4 == 2)
				{
					Main.tile[i, k].frameX = 0;
					Main.tile[i, k].frameY = 44;
				}
				break;
			}
			if (num5 == 5 || num5 == 7)
			{
				Main.tile[i - 1, k].active(active: true);
				Main.tile[i - 1, k].type = 5;
				num4 = WorldGen.genRand.Next(3);
				if (WorldGen.genRand.Next(3) < 2)
				{
					if (num4 == 0)
					{
						Main.tile[i - 1, k].frameX = 44;
						Main.tile[i - 1, k].frameY = 198;
					}
					if (num4 == 1)
					{
						Main.tile[i - 1, k].frameX = 44;
						Main.tile[i - 1, k].frameY = 220;
					}
					if (num4 == 2)
					{
						Main.tile[i - 1, k].frameX = 44;
						Main.tile[i - 1, k].frameY = 242;
					}
				}
				else
				{
					if (num4 == 0)
					{
						Main.tile[i - 1, k].frameX = 66;
						Main.tile[i - 1, k].frameY = 0;
					}
					if (num4 == 1)
					{
						Main.tile[i - 1, k].frameX = 66;
						Main.tile[i - 1, k].frameY = 22;
					}
					if (num4 == 2)
					{
						Main.tile[i - 1, k].frameX = 66;
						Main.tile[i - 1, k].frameY = 44;
					}
				}
			}
			if (num5 != 6 && num5 != 7)
			{
				continue;
			}
			Main.tile[i + 1, k].active(active: true);
			Main.tile[i + 1, k].type = 5;
			num4 = WorldGen.genRand.Next(3);
			if (WorldGen.genRand.Next(3) < 2)
			{
				if (num4 == 0)
				{
					Main.tile[i + 1, k].frameX = 66;
					Main.tile[i + 1, k].frameY = 198;
				}
				if (num4 == 1)
				{
					Main.tile[i + 1, k].frameX = 66;
					Main.tile[i + 1, k].frameY = 220;
				}
				if (num4 == 2)
				{
					Main.tile[i + 1, k].frameX = 66;
					Main.tile[i + 1, k].frameY = 242;
				}
			}
			else
			{
				if (num4 == 0)
				{
					Main.tile[i + 1, k].frameX = 88;
					Main.tile[i + 1, k].frameY = 66;
				}
				if (num4 == 1)
				{
					Main.tile[i + 1, k].frameX = 88;
					Main.tile[i + 1, k].frameY = 88;
				}
				if (num4 == 2)
				{
					Main.tile[i + 1, k].frameX = 88;
					Main.tile[i + 1, k].frameY = 110;
				}
			}
		}
		int num6 = WorldGen.genRand.Next(3);
		bool flag3 = false;
		bool flag4 = false;
		if (Main.tile[i - 1, j].HasTile && (Main.tile[i - 1, j].type == 2 || Main.tile[i - 1, j].type == 23 || Main.tile[i - 1, j].type == 60 || Main.tile[i - 1, j].type == 109 || Main.tile[i - 1, j].type == 147))
		{
			flag3 = true;
		}
		if (Main.tile[i + 1, j].HasTile && (Main.tile[i + 1, j].type == 2 || Main.tile[i + 1, j].type == 23 || Main.tile[i + 1, j].type == 60 || Main.tile[i + 1, j].type == 109 || Main.tile[i + 1, j].type == 147))
		{
			flag4 = true;
		}
		if (!flag3)
		{
			if (num6 == 0)
			{
				num6 = 2;
			}
			if (num6 == 1)
			{
				num6 = 3;
			}
		}
		if (!flag4)
		{
			if (num6 == 0)
			{
				num6 = 1;
			}
			if (num6 == 2)
			{
				num6 = 3;
			}
		}
		if (flag3 && !flag4)
		{
			num6 = 1;
		}
		if (flag4 && !flag3)
		{
			num6 = 2;
		}
		if (num6 == 0 || num6 == 1)
		{
			Main.tile[i + 1, j - 1].active(active: true);
			Main.tile[i + 1, j - 1].type = 5;
			num4 = WorldGen.genRand.Next(3);
			if (num4 == 0)
			{
				Main.tile[i + 1, j - 1].frameX = 22;
				Main.tile[i + 1, j - 1].frameY = 132;
			}
			if (num4 == 1)
			{
				Main.tile[i + 1, j - 1].frameX = 22;
				Main.tile[i + 1, j - 1].frameY = 154;
			}
			if (num4 == 2)
			{
				Main.tile[i + 1, j - 1].frameX = 22;
				Main.tile[i + 1, j - 1].frameY = 176;
			}
		}
		if (num6 == 0 || num6 == 2)
		{
			Main.tile[i - 1, j - 1].active(active: true);
			Main.tile[i - 1, j - 1].type = 5;
			num4 = WorldGen.genRand.Next(3);
			if (num4 == 0)
			{
				Main.tile[i - 1, j - 1].frameX = 44;
				Main.tile[i - 1, j - 1].frameY = 132;
			}
			if (num4 == 1)
			{
				Main.tile[i - 1, j - 1].frameX = 44;
				Main.tile[i - 1, j - 1].frameY = 154;
			}
			if (num4 == 2)
			{
				Main.tile[i - 1, j - 1].frameX = 44;
				Main.tile[i - 1, j - 1].frameY = 176;
			}
		}
		num4 = WorldGen.genRand.Next(3);
		switch (num6)
		{
		case 0:
			if (num4 == 0)
			{
				Main.tile[i, j - 1].frameX = 88;
				Main.tile[i, j - 1].frameY = 132;
			}
			if (num4 == 1)
			{
				Main.tile[i, j - 1].frameX = 88;
				Main.tile[i, j - 1].frameY = 154;
			}
			if (num4 == 2)
			{
				Main.tile[i, j - 1].frameX = 88;
				Main.tile[i, j - 1].frameY = 176;
			}
			break;
		case 1:
			if (num4 == 0)
			{
				Main.tile[i, j - 1].frameX = 0;
				Main.tile[i, j - 1].frameY = 132;
			}
			if (num4 == 1)
			{
				Main.tile[i, j - 1].frameX = 0;
				Main.tile[i, j - 1].frameY = 154;
			}
			if (num4 == 2)
			{
				Main.tile[i, j - 1].frameX = 0;
				Main.tile[i, j - 1].frameY = 176;
			}
			break;
		case 2:
			if (num4 == 0)
			{
				Main.tile[i, j - 1].frameX = 66;
				Main.tile[i, j - 1].frameY = 132;
			}
			if (num4 == 1)
			{
				Main.tile[i, j - 1].frameX = 66;
				Main.tile[i, j - 1].frameY = 154;
			}
			if (num4 == 2)
			{
				Main.tile[i, j - 1].frameX = 66;
				Main.tile[i, j - 1].frameY = 176;
			}
			break;
		}
		if (WorldGen.genRand.Next(4) < 3)
		{
			num4 = WorldGen.genRand.Next(3);
			if (num4 == 0)
			{
				Main.tile[i, j - num3].frameX = 22;
				Main.tile[i, j - num3].frameY = 198;
			}
			if (num4 == 1)
			{
				Main.tile[i, j - num3].frameX = 22;
				Main.tile[i, j - num3].frameY = 220;
			}
			if (num4 == 2)
			{
				Main.tile[i, j - num3].frameX = 22;
				Main.tile[i, j - num3].frameY = 242;
			}
		}
		else
		{
			num4 = WorldGen.genRand.Next(3);
			if (num4 == 0)
			{
				Main.tile[i, j - num3].frameX = 0;
				Main.tile[i, j - num3].frameY = 198;
			}
			if (num4 == 1)
			{
				Main.tile[i, j - num3].frameX = 0;
				Main.tile[i, j - num3].frameY = 220;
			}
			if (num4 == 2)
			{
				Main.tile[i, j - num3].frameX = 0;
				Main.tile[i, j - num3].frameY = 242;
			}
		}
		WorldGen.RangeFrame(i - 2, j - num3 - 1, i + 2, j + 1);
		if (Main.netMode == 2)
		{
			NetMessage.SendTileSquare(-1, i, (int)((double)j - (double)num3 * 0.5), num3 + 1);
		}
	}

	public static void GrowShroom(int i, int y)
	{
		if (Main.tile[i - 1, y - 1].lava() || Main.tile[i - 1, y - 1].lava() || Main.tile[i + 1, y - 1].lava() || !Main.tile[i, y].HasTile || Main.tile[i, y].type != 70 || Main.tile[i, y - 1].wall != 0 || !Main.tile[i - 1, y].HasTile || Main.tile[i - 1, y].type != 70 || !Main.tile[i + 1, y].HasTile || Main.tile[i + 1, y].type != 70 || !EmptyTileCheck(i - 2, i + 2, y - 13, y - 1, 71))
		{
			return;
		}
		int num = WorldGen.genRand.Next(4, 11);
		for (int j = y - num; j < y; j++)
		{
			Main.tile[i, j].frameNumber((byte)WorldGen.genRand.Next(3));
			Main.tile[i, j].active(active: true);
			Main.tile[i, j].type = 72;
			int num2 = WorldGen.genRand.Next(3);
			if (num2 == 0)
			{
				Main.tile[i, j].frameX = 0;
				Main.tile[i, j].frameY = 0;
			}
			if (num2 == 1)
			{
				Main.tile[i, j].frameX = 0;
				Main.tile[i, j].frameY = 18;
			}
			if (num2 == 2)
			{
				Main.tile[i, j].frameX = 0;
				Main.tile[i, j].frameY = 36;
			}
		}
		int num3 = WorldGen.genRand.Next(3);
		if (num3 == 0)
		{
			Main.tile[i, y - num].frameX = 36;
			Main.tile[i, y - num].frameY = 0;
		}
		if (num3 == 1)
		{
			Main.tile[i, y - num].frameX = 36;
			Main.tile[i, y - num].frameY = 18;
		}
		if (num3 == 2)
		{
			Main.tile[i, y - num].frameX = 36;
			Main.tile[i, y - num].frameY = 36;
		}
		WorldGen.RangeFrame(i - 2, y - num - 1, i + 2, y + 1);
		if (Main.netMode == 2)
		{
			NetMessage.SendTileSquare(-1, i, (int)((double)y - (double)num * 0.5), num + 1);
		}
	}

	public static void AddTrees()
	{
		for (int i = 1; i < Main.maxTilesX - 1; i++)
		{
			for (int j = 20; (double)j < Main.worldSurface; j++)
			{
				if (!Main.tile[i, j].active() || Main.tile[i, j].type != 2 || !Main.tile[i - 1, j].active() || Main.tile[i - 1, j].type != 2 || !Main.tile[i + 1, j].active() || Main.tile[i + 1, j].type != 2 || !EmptyTileCheck(i - 2, i + 2, j - 14, j - 1))
				{
					continue;
				}
				bool flag = false;
				bool flag2 = false;
				int num = WorldGen._genRand.Next(5, 15);
				int num2;
				for (int k = j - num; k < j; k++)
				{
					Main.tile[i, k].frameNumber((byte)WorldGen._genRand.Next(3));
					Main.tile[i, k].active(active: true);
					Main.tile[i, k].type = 5;
					num2 = WorldGen._genRand.Next(3);
					int num3 = WorldGen._genRand.Next(10);
					if (k == j - 1 || k == j - num)
					{
						num3 = 0;
					}
					while (((num3 == 5 || num3 == 7) & flag) || ((num3 == 6 || num3 == 7) & flag2))
					{
						num3 = WorldGen._genRand.Next(10);
					}
					flag = false;
					flag2 = false;
					if (num3 == 5 || num3 == 7)
					{
						flag = true;
					}
					if (num3 == 6 || num3 == 7)
					{
						flag2 = true;
					}
					switch (num3)
					{
					case 1:
						if (num2 == 0)
						{
							Main.tile[i, k].frameX = 0;
							Main.tile[i, k].frameY = 66;
						}
						if (num2 == 1)
						{
							Main.tile[i, k].frameX = 0;
							Main.tile[i, k].frameY = 88;
						}
						if (num2 == 2)
						{
							Main.tile[i, k].frameX = 0;
							Main.tile[i, k].frameY = 110;
						}
						break;
					case 2:
						if (num2 == 0)
						{
							Main.tile[i, k].frameX = 22;
							Main.tile[i, k].frameY = 0;
						}
						if (num2 == 1)
						{
							Main.tile[i, k].frameX = 22;
							Main.tile[i, k].frameY = 22;
						}
						if (num2 == 2)
						{
							Main.tile[i, k].frameX = 22;
							Main.tile[i, k].frameY = 44;
						}
						break;
					case 3:
						if (num2 == 0)
						{
							Main.tile[i, k].frameX = 44;
							Main.tile[i, k].frameY = 66;
						}
						if (num2 == 1)
						{
							Main.tile[i, k].frameX = 44;
							Main.tile[i, k].frameY = 88;
						}
						if (num2 == 2)
						{
							Main.tile[i, k].frameX = 44;
							Main.tile[i, k].frameY = 110;
						}
						break;
					case 4:
						if (num2 == 0)
						{
							Main.tile[i, k].frameX = 22;
							Main.tile[i, k].frameY = 66;
						}
						if (num2 == 1)
						{
							Main.tile[i, k].frameX = 22;
							Main.tile[i, k].frameY = 88;
						}
						if (num2 == 2)
						{
							Main.tile[i, k].frameX = 22;
							Main.tile[i, k].frameY = 110;
						}
						break;
					case 5:
						if (num2 == 0)
						{
							Main.tile[i, k].frameX = 88;
							Main.tile[i, k].frameY = 0;
						}
						if (num2 == 1)
						{
							Main.tile[i, k].frameX = 88;
							Main.tile[i, k].frameY = 22;
						}
						if (num2 == 2)
						{
							Main.tile[i, k].frameX = 88;
							Main.tile[i, k].frameY = 44;
						}
						break;
					case 6:
						if (num2 == 0)
						{
							Main.tile[i, k].frameX = 66;
							Main.tile[i, k].frameY = 66;
						}
						if (num2 == 1)
						{
							Main.tile[i, k].frameX = 66;
							Main.tile[i, k].frameY = 88;
						}
						if (num2 == 2)
						{
							Main.tile[i, k].frameX = 66;
							Main.tile[i, k].frameY = 110;
						}
						break;
					case 7:
						if (num2 == 0)
						{
							Main.tile[i, k].frameX = 110;
							Main.tile[i, k].frameY = 66;
						}
						if (num2 == 1)
						{
							Main.tile[i, k].frameX = 110;
							Main.tile[i, k].frameY = 88;
						}
						if (num2 == 2)
						{
							Main.tile[i, k].frameX = 110;
							Main.tile[i, k].frameY = 110;
						}
						break;
					default:
						if (num2 == 0)
						{
							Main.tile[i, k].frameX = 0;
							Main.tile[i, k].frameY = 0;
						}
						if (num2 == 1)
						{
							Main.tile[i, k].frameX = 0;
							Main.tile[i, k].frameY = 22;
						}
						if (num2 == 2)
						{
							Main.tile[i, k].frameX = 0;
							Main.tile[i, k].frameY = 44;
						}
						break;
					}
					if (num3 == 5 || num3 == 7)
					{
						Main.tile[i - 1, k].active(active: true);
						Main.tile[i - 1, k].type = 5;
						num2 = WorldGen._genRand.Next(3);
						if (WorldGen._genRand.Next(3) < 2)
						{
							if (num2 == 0)
							{
								Main.tile[i - 1, k].frameX = 44;
								Main.tile[i - 1, k].frameY = 198;
							}
							if (num2 == 1)
							{
								Main.tile[i - 1, k].frameX = 44;
								Main.tile[i - 1, k].frameY = 220;
							}
							if (num2 == 2)
							{
								Main.tile[i - 1, k].frameX = 44;
								Main.tile[i - 1, k].frameY = 242;
							}
						}
						else
						{
							if (num2 == 0)
							{
								Main.tile[i - 1, k].frameX = 66;
								Main.tile[i - 1, k].frameY = 0;
							}
							if (num2 == 1)
							{
								Main.tile[i - 1, k].frameX = 66;
								Main.tile[i - 1, k].frameY = 22;
							}
							if (num2 == 2)
							{
								Main.tile[i - 1, k].frameX = 66;
								Main.tile[i - 1, k].frameY = 44;
							}
						}
					}
					if (num3 != 6 && num3 != 7)
					{
						continue;
					}
					Main.tile[i + 1, k].active(active: true);
					Main.tile[i + 1, k].type = 5;
					num2 = WorldGen._genRand.Next(3);
					if (WorldGen._genRand.Next(3) < 2)
					{
						if (num2 == 0)
						{
							Main.tile[i + 1, k].frameX = 66;
							Main.tile[i + 1, k].frameY = 198;
						}
						if (num2 == 1)
						{
							Main.tile[i + 1, k].frameX = 66;
							Main.tile[i + 1, k].frameY = 220;
						}
						if (num2 == 2)
						{
							Main.tile[i + 1, k].frameX = 66;
							Main.tile[i + 1, k].frameY = 242;
						}
					}
					else
					{
						if (num2 == 0)
						{
							Main.tile[i + 1, k].frameX = 88;
							Main.tile[i + 1, k].frameY = 66;
						}
						if (num2 == 1)
						{
							Main.tile[i + 1, k].frameX = 88;
							Main.tile[i + 1, k].frameY = 88;
						}
						if (num2 == 2)
						{
							Main.tile[i + 1, k].frameX = 88;
							Main.tile[i + 1, k].frameY = 110;
						}
					}
				}
				int num4 = WorldGen._genRand.Next(3);
				if (num4 == 0 || num4 == 1)
				{
					Main.tile[i + 1, j - 1].active(active: true);
					Main.tile[i + 1, j - 1].type = 5;
					num2 = WorldGen._genRand.Next(3);
					if (num2 == 0)
					{
						Main.tile[i + 1, j - 1].frameX = 22;
						Main.tile[i + 1, j - 1].frameY = 132;
					}
					if (num2 == 1)
					{
						Main.tile[i + 1, j - 1].frameX = 22;
						Main.tile[i + 1, j - 1].frameY = 154;
					}
					if (num2 == 2)
					{
						Main.tile[i + 1, j - 1].frameX = 22;
						Main.tile[i + 1, j - 1].frameY = 176;
					}
				}
				if (num4 == 0 || num4 == 2)
				{
					Main.tile[i - 1, j - 1].active(active: true);
					Main.tile[i - 1, j - 1].type = 5;
					num2 = WorldGen._genRand.Next(3);
					if (num2 == 0)
					{
						Main.tile[i - 1, j - 1].frameX = 44;
						Main.tile[i - 1, j - 1].frameY = 132;
					}
					if (num2 == 1)
					{
						Main.tile[i - 1, j - 1].frameX = 44;
						Main.tile[i - 1, j - 1].frameY = 154;
					}
					if (num2 == 2)
					{
						Main.tile[i - 1, j - 1].frameX = 44;
						Main.tile[i - 1, j - 1].frameY = 176;
					}
				}
				num2 = WorldGen._genRand.Next(3);
				switch (num4)
				{
				case 0:
					if (num2 == 0)
					{
						Main.tile[i, j - 1].frameX = 88;
						Main.tile[i, j - 1].frameY = 132;
					}
					if (num2 == 1)
					{
						Main.tile[i, j - 1].frameX = 88;
						Main.tile[i, j - 1].frameY = 154;
					}
					if (num2 == 2)
					{
						Main.tile[i, j - 1].frameX = 88;
						Main.tile[i, j - 1].frameY = 176;
					}
					break;
				case 1:
					if (num2 == 0)
					{
						Main.tile[i, j - 1].frameX = 0;
						Main.tile[i, j - 1].frameY = 132;
					}
					if (num2 == 1)
					{
						Main.tile[i, j - 1].frameX = 0;
						Main.tile[i, j - 1].frameY = 154;
					}
					if (num2 == 2)
					{
						Main.tile[i, j - 1].frameX = 0;
						Main.tile[i, j - 1].frameY = 176;
					}
					break;
				case 2:
					if (num2 == 0)
					{
						Main.tile[i, j - 1].frameX = 66;
						Main.tile[i, j - 1].frameY = 132;
					}
					if (num2 == 1)
					{
						Main.tile[i, j - 1].frameX = 66;
						Main.tile[i, j - 1].frameY = 154;
					}
					if (num2 == 2)
					{
						Main.tile[i, j - 1].frameX = 66;
						Main.tile[i, j - 1].frameY = 176;
					}
					break;
				}
				if (WorldGen._genRand.Next(3) < 2)
				{
					num2 = WorldGen._genRand.Next(3);
					if (num2 == 0)
					{
						Main.tile[i, j - num].frameX = 22;
						Main.tile[i, j - num].frameY = 198;
					}
					if (num2 == 1)
					{
						Main.tile[i, j - num].frameX = 22;
						Main.tile[i, j - num].frameY = 220;
					}
					if (num2 == 2)
					{
						Main.tile[i, j - num].frameX = 22;
						Main.tile[i, j - num].frameY = 242;
					}
				}
				else
				{
					num2 = WorldGen._genRand.Next(3);
					if (num2 == 0)
					{
						Main.tile[i, j - num].frameX = 0;
						Main.tile[i, j - num].frameY = 198;
					}
					if (num2 == 1)
					{
						Main.tile[i, j - num].frameX = 0;
						Main.tile[i, j - num].frameY = 220;
					}
					if (num2 == 2)
					{
						Main.tile[i, j - num].frameX = 0;
						Main.tile[i, j - num].frameY = 242;
					}
				}
			}
		}
	}

	public static bool EmptyTileCheck(int startX, int endX, int startY, int endY, int ignoreStyle = -1)
	{
		if (startX < 0)
		{
			return false;
		}
		if (endX >= Main.maxTilesX)
		{
			return false;
		}
		if (startY < 0)
		{
			return false;
		}
		if (endY >= Main.maxTilesY)
		{
			return false;
		}
		for (int i = startX; i < endX + 1; i++)
		{
			for (int j = startY; j < endY + 1; j++)
			{
				if (!Main.tile[i, j].HasTile)
				{
					continue;
				}
				switch (ignoreStyle)
				{
				case -1:
					return false;
				case 11:
					if (Main.tile[i, j].type != 11)
					{
						return false;
					}
					break;
				}
				if (ignoreStyle == 20 && Main.tile[i, j].type != 20 && Main.tile[i, j].type != 3 && Main.tile[i, j].type != 24 && Main.tile[i, j].type != 61 && Main.tile[i, j].type != 32 && Main.tile[i, j].type != 69 && Main.tile[i, j].type != 73 && Main.tile[i, j].type != 74 && Main.tile[i, j].type != 110 && Main.tile[i, j].type != 113)
				{
					return false;
				}
				if (ignoreStyle == 71 && Main.tile[i, j].type != 71)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static bool PlaceDoor(int i, int j, int type)
	{
		try
		{
			if (Main.tile[i, j - 2].active() && Main.tileSolid[Main.tile[i, j - 2].type] && Main.tile[i, j + 2].active() && Main.tileSolid[Main.tile[i, j + 2].type])
			{
				Main.tile[i, j - 1].active(active: true);
				Main.tile[i, j - 1].type = 10;
				Main.tile[i, j - 1].frameY = 0;
				Main.tile[i, j - 1].frameX = (short)(WorldGen._genRand.Next(3) * 18);
				Main.tile[i, j].active(active: true);
				Main.tile[i, j].type = 10;
				Main.tile[i, j].frameY = 18;
				Main.tile[i, j].frameX = (short)(WorldGen._genRand.Next(3) * 18);
				Main.tile[i, j + 1].active(active: true);
				Main.tile[i, j + 1].type = 10;
				Main.tile[i, j + 1].frameY = 36;
				Main.tile[i, j + 1].frameX = (short)(WorldGen._genRand.Next(3) * 18);
				return true;
			}
			return false;
		}
		catch
		{
			return false;
		}
	}

	public static bool AddLifeCrystal(int i, int j)
	{
		for (int k = j; k < Main.maxTilesY; k++)
		{
			if (Main.tile[i, k].HasTile && Main.tileSolid[Main.tile[i, k].type])
			{
				int num = k - 1;
				if (Main.tile[i, num - 1].lava() || Main.tile[i - 1, num - 1].lava())
				{
					return false;
				}
				if (!EmptyTileCheck(i - 1, i, num - 1, num))
				{
					return false;
				}
				Main.tile[i - 1, num - 1].active(active: true);
				Main.tile[i - 1, num - 1].type = 12;
				Main.tile[i - 1, num - 1].frameX = 0;
				Main.tile[i - 1, num - 1].frameY = 0;
				Main.tile[i, num - 1].active(active: true);
				Main.tile[i, num - 1].type = 12;
				Main.tile[i, num - 1].frameX = 18;
				Main.tile[i, num - 1].frameY = 0;
				Main.tile[i - 1, num].active(active: true);
				Main.tile[i - 1, num].type = 12;
				Main.tile[i - 1, num].frameX = 0;
				Main.tile[i - 1, num].frameY = 18;
				Main.tile[i, num].active(active: true);
				Main.tile[i, num].type = 12;
				Main.tile[i, num].frameX = 18;
				Main.tile[i, num].frameY = 18;
				return true;
			}
		}
		return false;
	}

	public static void AddShadowOrb(int x, int y)
	{
		if (x < 10 || x > Main.maxTilesX - 10 || y < 10 || y > Main.maxTilesY - 10)
		{
			return;
		}
		for (int i = x - 1; i < x + 1; i++)
		{
			for (int j = y - 1; j < y + 1; j++)
			{
				if (Main.tile[i, j].HasTile && Main.tile[i, j].type == 31)
				{
					return;
				}
			}
		}
		Main.tile[x - 1, y - 1].active(active: true);
		Main.tile[x - 1, y - 1].type = 31;
		Main.tile[x - 1, y - 1].frameX = 0;
		Main.tile[x - 1, y - 1].frameY = 0;
		Main.tile[x, y - 1].active(active: true);
		Main.tile[x, y - 1].type = 31;
		Main.tile[x, y - 1].frameX = 18;
		Main.tile[x, y - 1].frameY = 0;
		Main.tile[x - 1, y].active(active: true);
		Main.tile[x - 1, y].type = 31;
		Main.tile[x - 1, y].frameX = 0;
		Main.tile[x - 1, y].frameY = 18;
		Main.tile[x, y].active(active: true);
		Main.tile[x, y].type = 31;
		Main.tile[x, y].frameX = 18;
		Main.tile[x, y].frameY = 18;
	}

	public static void AddHellHouses()
	{
		int num = (int)((double)Main.maxTilesX * 0.25);
		for (int i = num; i < Main.maxTilesX - num; i++)
		{
			int num2 = Main.maxTilesY - 40;
			while (Main.tile[i, num2].HasTile || Main.tile[i, num2].liquid > 0)
			{
				num2--;
			}
			if (Main.tile[i, num2 + 1].HasTile)
			{
				byte b = (byte)WorldGen.genRand.Next(75, 77);
				byte wall = 13;
				if (WorldGen.genRand.Next(5) > 0)
				{
					b = 75;
				}
				if (b == 75)
				{
					wall = 14;
				}
				HellHouse(i, num2, b, wall);
				i += WorldGen.genRand.Next(15, 80);
			}
		}
		float num3 = Main.maxTilesX / 4200;
		for (int j = 0; (float)j < 200f * num3; j++)
		{
			int num4 = 0;
			bool flag = false;
			while (!flag)
			{
				num4++;
				int num5 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.2), (int)((double)Main.maxTilesX * 0.8));
				int num6 = WorldGen.genRand.Next(Main.maxTilesY - 300, Main.maxTilesY - 20);
				if (Main.tile[num5, num6].HasTile && (Main.tile[num5, num6].type == 75 || Main.tile[num5, num6].type == 76))
				{
					int num7 = 0;
					if (Main.tile[num5 - 1, num6].wall > 0)
					{
						num7 = -1;
					}
					else if (Main.tile[num5 + 1, num6].wall > 0)
					{
						num7 = 1;
					}
					if (!Main.tile[num5 + num7, num6].HasTile && !Main.tile[num5 + num7, num6 + 1].HasTile)
					{
						bool flag2 = false;
						for (int k = num5 - 8; k < num5 + 8; k++)
						{
							for (int l = num6 - 8; l < num6 + 8; l++)
							{
								if (Main.tile[k, l].HasTile && Main.tile[k, l].type == 4)
								{
									flag2 = true;
									break;
								}
							}
						}
						if (!flag2)
						{
							PlaceTile(num5 + num7, num6, 4, mute: true, forced: true, -1, 7);
							flag = true;
						}
					}
				}
				if (num4 > 1000)
				{
					flag = true;
				}
			}
		}
	}

	public static void HellHouse(int i, int j, byte type = 76, byte wall = 13)
	{
		int num = WorldGen.genRand.Next(8, 20);
		int num2 = WorldGen.genRand.Next(1, 3);
		int num3 = WorldGen.genRand.Next(4, 13);
		int num4 = j;
		for (int k = 0; k < num2; k++)
		{
			int num5 = WorldGen.genRand.Next(5, 9);
			HellRoom(i, num4, num, num5, type, wall);
			num4 -= num5;
		}
		num4 = j;
		for (int l = 0; l < num3; l++)
		{
			int num6 = WorldGen.genRand.Next(5, 9);
			num4 += num6;
			HellRoom(i, num4, num, num6, type, wall);
		}
		for (int m = i - num / 2; m <= i + num / 2; m++)
		{
			for (num4 = j; num4 < Main.maxTilesY && ((Main.tile[m, num4].HasTile && (Main.tile[m, num4].type == 76 || Main.tile[m, num4].type == 75)) || Main.tile[i, num4].wall == 13 || Main.tile[i, num4].wall == 14); num4++)
			{
			}
			int num7 = 6 + WorldGen.genRand.Next(3);
			while (num4 < Main.maxTilesY && !Main.tile[m, num4].HasTile)
			{
				num7--;
				Main.tile[m, num4].active(active: true);
				Main.tile[m, num4].type = 57;
				num4++;
				if (num7 <= 0)
				{
					break;
				}
			}
		}
		int num8 = 0;
		int num9 = 0;
		for (num4 = j; num4 < Main.maxTilesY && ((Main.tile[i, num4].HasTile && (Main.tile[i, num4].type == 76 || Main.tile[i, num4].type == 75)) || Main.tile[i, num4].wall == 13 || Main.tile[i, num4].wall == 14); num4++)
		{
		}
		num4--;
		num9 = num4;
		while ((Main.tile[i, num4].HasTile && (Main.tile[i, num4].type == 76 || Main.tile[i, num4].type == 75)) || Main.tile[i, num4].wall == 13 || Main.tile[i, num4].wall == 14)
		{
			num4--;
			if (!Main.tile[i, num4].HasTile || (Main.tile[i, num4].type != 76 && Main.tile[i, num4].type != 75))
			{
				continue;
			}
			int num10 = WorldGen.genRand.Next(i - num / 2 + 1, i + num / 2 - 1);
			int num11 = WorldGen.genRand.Next(i - num / 2 + 1, i + num / 2 - 1);
			if (num10 > num11)
			{
				int num12 = num10;
				num10 = num11;
				num11 = num12;
			}
			if (num10 == num11)
			{
				if (num10 < i)
				{
					num11++;
				}
				else
				{
					num10--;
				}
			}
			for (int n = num10; n <= num11; n++)
			{
				if (Main.tile[n, num4 - 1].wall == 13)
				{
					Main.tile[n, num4].wall = 13;
				}
				if (Main.tile[n, num4 - 1].wall == 14)
				{
					Main.tile[n, num4].wall = 14;
				}
				Main.tile[n, num4].type = 19;
				Main.tile[n, num4].active(active: true);
			}
			num4--;
		}
		num8 = num4;
		float num13 = (float)((num9 - num8) * num) * 0.02f;
		for (int num14 = 0; (float)num14 < num13; num14++)
		{
			int num15 = WorldGen.genRand.Next(i - num / 2, i + num / 2 + 1);
			int num16 = WorldGen.genRand.Next(num8, num9);
			int num17 = WorldGen.genRand.Next(3, 8);
			for (int num18 = num15 - num17; num18 <= num15 + num17; num18++)
			{
				for (int num19 = num16 - num17; num19 <= num16 + num17; num19++)
				{
					float num20 = Math.Abs(num18 - num15);
					float num21 = Math.Abs(num19 - num16);
					if (!(Math.Sqrt(num20 * num20 + num21 * num21) < (double)num17 * 0.4))
					{
						continue;
					}
					try
					{
						if (Main.tile[num18, num19].type == 76 || Main.tile[num18, num19].type == 19)
						{
							Main.tile[num18, num19].active(active: false);
						}
						Main.tile[num18, num19].wall = 0;
					}
					catch
					{
					}
				}
			}
		}
	}

	public static void HellRoom(int i, int j, int width, int height, byte type = 76, byte wall = 13)
	{
		if (j > Main.maxTilesY - 40)
		{
			return;
		}
		for (int k = i - width / 2; k <= i + width / 2; k++)
		{
			for (int l = j - height; l <= j; l++)
			{
				try
				{
					Main.tile[k, l].active(active: true);
					Main.tile[k, l].type = type;
					Main.tile[k, l].liquid = 0;
					Main.tile[k, l].lava(lava: false);
				}
				catch
				{
				}
			}
		}
		for (int m = i - width / 2 + 1; m <= i + width / 2 - 1; m++)
		{
			for (int n = j - height + 1; n <= j - 1; n++)
			{
				try
				{
					Main.tile[m, n].active(active: false);
					Main.tile[m, n].wall = wall;
					Main.tile[m, n].liquid = 0;
					Main.tile[m, n].lava(lava: false);
				}
				catch
				{
				}
			}
		}
	}

	public static void MakeDungeon(int x, int y, int tileType = 41, int wallType = 7)
	{
		//IL_1ed4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eeb: Unknown result type (might be due to invalid IL or missing references)
		int num = WorldGen.genRand.Next(3);
		int num2 = WorldGen.genRand.Next(3);
		switch (num)
		{
		case 1:
			tileType = 43;
			break;
		case 2:
			tileType = 44;
			break;
		}
		switch (num2)
		{
		case 1:
			wallType = 8;
			break;
		case 2:
			wallType = 9;
			break;
		}
		numDDoors = 0;
		numDPlats = 0;
		numDRooms = 0;
		dungeonX = x;
		dungeonY = y;
		dMinX = x;
		dMaxX = x;
		dMinY = y;
		dMaxY = y;
		dxStrength1 = WorldGen.genRand.Next(25, 30);
		dyStrength1 = WorldGen.genRand.Next(20, 25);
		dxStrength2 = WorldGen.genRand.Next(35, 50);
		dyStrength2 = WorldGen.genRand.Next(10, 15);
		float num3 = Main.maxTilesX / 60;
		num3 += (float)WorldGen.genRand.Next(0, (int)(num3 / 3f));
		float num4 = num3;
		int num5 = 5;
		DungeonRoom(dungeonX, dungeonY, tileType, wallType);
		while (num3 > 0f)
		{
			if (dungeonX < dMinX)
			{
				dMinX = dungeonX;
			}
			if (dungeonX > dMaxX)
			{
				dMaxX = dungeonX;
			}
			if (dungeonY > dMaxY)
			{
				dMaxY = dungeonY;
			}
			num3--;
			Main.statusText = Lang.gen[58]?.ToString() + " " + (int)((num4 - num3) / num4 * 60f) + "%";
			if (num5 > 0)
			{
				num5--;
			}
			if ((num5 == 0) & (WorldGen.genRand.Next(3) == 0))
			{
				num5 = 5;
				if (WorldGen.genRand.Next(2) == 0)
				{
					int num6 = dungeonX;
					int num7 = dungeonY;
					DungeonHalls(dungeonX, dungeonY, tileType, wallType);
					if (WorldGen.genRand.Next(2) == 0)
					{
						DungeonHalls(dungeonX, dungeonY, tileType, wallType);
					}
					DungeonRoom(dungeonX, dungeonY, tileType, wallType);
					dungeonX = num6;
					dungeonY = num7;
				}
				else
				{
					DungeonRoom(dungeonX, dungeonY, tileType, wallType);
				}
			}
			else
			{
				DungeonHalls(dungeonX, dungeonY, tileType, wallType);
			}
		}
		DungeonRoom(dungeonX, dungeonY, tileType, wallType);
		int num8 = dRoomX[0];
		int num9 = dRoomY[0];
		for (int i = 0; i < numDRooms; i++)
		{
			if (dRoomY[i] < num9)
			{
				num8 = dRoomX[i];
				num9 = dRoomY[i];
			}
		}
		dungeonX = num8;
		dungeonY = num9;
		dEnteranceX = num8;
		dSurface = false;
		num5 = 5;
		while (!dSurface)
		{
			if (num5 > 0)
			{
				num5--;
			}
			if (((num5 == 0) & (WorldGen.genRand.Next(5) == 0)) && (double)dungeonY > Main.worldSurface + 50.0)
			{
				num5 = 10;
				int num10 = dungeonX;
				int num11 = dungeonY;
				DungeonHalls(dungeonX, dungeonY, tileType, wallType, forceX: true);
				DungeonRoom(dungeonX, dungeonY, tileType, wallType);
				dungeonX = num10;
				dungeonY = num11;
			}
			DungeonStairs(dungeonX, dungeonY, tileType, wallType);
		}
		DungeonEnt(dungeonX, dungeonY, tileType, wallType);
		Main.statusText = Lang.gen[58]?.ToString() + " 65%";
		for (int j = 0; j < numDRooms; j++)
		{
			for (int k = dRoomL[j]; k <= dRoomR[j]; k++)
			{
				if (numDPlats < DPlatX.Length && j < dRoomT.Length && !Main.tile[k, dRoomT[j] - 1].HasTile)
				{
					DPlatX[numDPlats] = k;
					DPlatY[numDPlats] = dRoomT[j] - 1;
					numDPlats++;
					break;
				}
			}
			for (int l = dRoomL[j]; l <= dRoomR[j]; l++)
			{
				if (numDPlats < DPlatX.Length && j < dRoomB.Length && !Main.tile[l, dRoomB[j] + 1].HasTile)
				{
					DPlatX[numDPlats] = l;
					DPlatY[numDPlats] = dRoomB[j] + 1;
					numDPlats++;
					break;
				}
			}
			for (int m = dRoomT[j]; m <= dRoomB[j]; m++)
			{
				if (numDPlats < DDoorX.Length && j < dRoomL.Length && !Main.tile[dRoomL[j] - 1, m].HasTile)
				{
					DDoorX[numDDoors] = dRoomL[j] - 1;
					DDoorY[numDDoors] = m;
					DDoorPos[numDDoors] = -1;
					numDDoors++;
					break;
				}
			}
			for (int n = dRoomT[j]; n <= dRoomB[j]; n++)
			{
				if (numDPlats < DDoorX.Length && j < dRoomR.Length && !Main.tile[dRoomR[j] + 1, n].HasTile)
				{
					DDoorX[numDDoors] = dRoomR[j] + 1;
					DDoorY[numDDoors] = n;
					DDoorPos[numDDoors] = 1;
					numDDoors++;
					break;
				}
			}
		}
		Main.statusText = Lang.gen[58]?.ToString() + " 70%";
		int num12 = 0;
		int num13 = 1000;
		int num14 = 0;
		while (num14 < Main.maxTilesX / 100)
		{
			num12++;
			int num15 = WorldGen.genRand.Next(dMinX, dMaxX);
			int num16 = WorldGen.genRand.Next((int)Main.worldSurface + 25, dMaxY);
			int num17 = num15;
			if (Main.tile[num15, num16].wall == wallType && !Main.tile[num15, num16].HasTile)
			{
				int num18 = 1;
				if (WorldGen.genRand.Next(2) == 0)
				{
					num18 = -1;
				}
				for (; !Main.tile[num15, num16].HasTile; num16 += num18)
				{
				}
				if (Main.tile[num15 - 1, num16].HasTile && Main.tile[num15 + 1, num16].HasTile && !Main.tile[num15 - 1, num16 - num18].HasTile && !Main.tile[num15 + 1, num16 - num18].HasTile)
				{
					num14++;
					int num19 = WorldGen.genRand.Next(5, 13);
					while (Main.tile[num15 - 1, num16].HasTile && Main.tile[num15, num16 + num18].HasTile && Main.tile[num15, num16].HasTile && !Main.tile[num15, num16 - num18].HasTile && num19 > 0)
					{
						Main.tile[num15, num16].type = 48;
						if (!Main.tile[num15 - 1, num16 - num18].HasTile && !Main.tile[num15 + 1, num16 - num18].HasTile)
						{
							Main.tile[num15, num16 - num18].type = 48;
							Main.tile[num15, num16 - num18].active(active: true);
						}
						num15--;
						num19--;
					}
					num19 = WorldGen.genRand.Next(5, 13);
					num15 = num17 + 1;
					while (Main.tile[num15 + 1, num16].HasTile && Main.tile[num15, num16 + num18].HasTile && Main.tile[num15, num16].HasTile && !Main.tile[num15, num16 - num18].HasTile && num19 > 0)
					{
						Main.tile[num15, num16].type = 48;
						if (!Main.tile[num15 - 1, num16 - num18].HasTile && !Main.tile[num15 + 1, num16 - num18].HasTile)
						{
							Main.tile[num15, num16 - num18].type = 48;
							Main.tile[num15, num16 - num18].active(active: true);
						}
						num15++;
						num19--;
					}
				}
			}
			if (num12 > num13)
			{
				num12 = 0;
				num14++;
			}
		}
		num12 = 0;
		num13 = 1000;
		num14 = 0;
		Main.statusText = Lang.gen[58]?.ToString() + " 75%";
		while (num14 < Main.maxTilesX / 100)
		{
			num12++;
			int num20 = WorldGen.genRand.Next(dMinX, dMaxX);
			int num21 = WorldGen.genRand.Next((int)Main.worldSurface + 25, dMaxY);
			int num22 = num21;
			if (Main.tile[num20, num21].wall == wallType && !Main.tile[num20, num21].HasTile)
			{
				int num23 = 1;
				if (WorldGen.genRand.Next(2) == 0)
				{
					num23 = -1;
				}
				for (; num20 > 5 && num20 < Main.maxTilesX - 5 && !Main.tile[num20, num21].HasTile; num20 += num23)
				{
				}
				if (Main.tile[num20, num21 - 1].HasTile && Main.tile[num20, num21 + 1].HasTile && !Main.tile[num20 - num23, num21 - 1].HasTile && !Main.tile[num20 - num23, num21 + 1].HasTile)
				{
					num14++;
					int num24 = WorldGen.genRand.Next(5, 13);
					while (Main.tile[num20, num21 - 1].HasTile && Main.tile[num20 + num23, num21].HasTile && Main.tile[num20, num21].HasTile && !Main.tile[num20 - num23, num21].HasTile && num24 > 0)
					{
						Main.tile[num20, num21].type = 48;
						if (!Main.tile[num20 - num23, num21 - 1].HasTile && !Main.tile[num20 - num23, num21 + 1].HasTile)
						{
							Main.tile[num20 - num23, num21].type = 48;
							Main.tile[num20 - num23, num21].active(active: true);
						}
						num21--;
						num24--;
					}
					num24 = WorldGen.genRand.Next(5, 13);
					num21 = num22 + 1;
					while (Main.tile[num20, num21 + 1].HasTile && Main.tile[num20 + num23, num21].HasTile && Main.tile[num20, num21].HasTile && !Main.tile[num20 - num23, num21].HasTile && num24 > 0)
					{
						Main.tile[num20, num21].type = 48;
						if (!Main.tile[num20 - num23, num21 - 1].HasTile && !Main.tile[num20 - num23, num21 + 1].HasTile)
						{
							Main.tile[num20 - num23, num21].type = 48;
							Main.tile[num20 - num23, num21].active(active: true);
						}
						num21++;
						num24--;
					}
				}
			}
			if (num12 > num13)
			{
				num12 = 0;
				num14++;
			}
		}
		Main.statusText = Lang.gen[58]?.ToString() + " 80%";
		for (int num25 = 0; num25 < numDDoors; num25++)
		{
			int num26 = DDoorX[num25] - 10;
			int num27 = DDoorX[num25] + 10;
			int num28 = 100;
			int num29 = 0;
			int num30 = 0;
			int num31 = 0;
			for (int num32 = num26; num32 < num27; num32++)
			{
				bool flag = true;
				int num33 = DDoorY[num25];
				while (!Main.tile[num32, num33].HasTile)
				{
					num33--;
				}
				if (!Main.tileDungeon[Main.tile[num32, num33].type])
				{
					flag = false;
				}
				num30 = num33;
				for (num33 = DDoorY[num25]; !Main.tile[num32, num33].HasTile; num33++)
				{
				}
				if (!Main.tileDungeon[Main.tile[num32, num33].type])
				{
					flag = false;
				}
				num31 = num33;
				if (num31 - num30 < 3)
				{
					continue;
				}
				int num34 = num32 - 20;
				int num35 = num32 + 20;
				int num36 = num31 - 10;
				int num37 = num31 + 10;
				for (int num38 = num34; num38 < num35; num38++)
				{
					for (int num39 = num36; num39 < num37; num39++)
					{
						if (Main.tile[num38, num39].HasTile && Main.tile[num38, num39].type == 10)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					for (int num40 = num31 - 3; num40 < num31; num40++)
					{
						for (int num41 = num32 - 3; num41 <= num32 + 3; num41++)
						{
							if (Main.tile[num41, num40].HasTile)
							{
								flag = false;
								break;
							}
						}
					}
				}
				if (flag && num31 - num30 < 20)
				{
					bool flag2 = false;
					if (DDoorPos[num25] == 0 && num31 - num30 < num28)
					{
						flag2 = true;
					}
					if (DDoorPos[num25] == -1 && num32 > num29)
					{
						flag2 = true;
					}
					if (DDoorPos[num25] == 1 && (num32 < num29 || num29 == 0))
					{
						flag2 = true;
					}
					if (flag2)
					{
						num29 = num32;
						num28 = num31 - num30;
					}
				}
			}
			if (num28 >= 20)
			{
				continue;
			}
			int num42 = num29;
			int num43 = DDoorY[num25];
			int num44 = num43;
			for (; !Main.tile[num42, num43].HasTile; num43++)
			{
				Main.tile[num42, num43].active(active: false);
			}
			while (!Main.tile[num42, num44].HasTile)
			{
				num44--;
			}
			num43--;
			num44++;
			for (int num45 = num44; num45 < num43 - 2; num45++)
			{
				Main.tile[num42, num45].active(active: true);
				Main.tile[num42, num45].type = (byte)tileType;
			}
			PlaceTile(num42, num43, 10, mute: true);
			num42--;
			int num46 = num43 - 3;
			while (!Main.tile[num42, num46].HasTile)
			{
				num46--;
			}
			if (num43 - num46 < num43 - num44 + 5 && Main.tileDungeon[Main.tile[num42, num46].type])
			{
				for (int num47 = num43 - 4 - WorldGen.genRand.Next(3); num47 > num46; num47--)
				{
					Main.tile[num42, num47].active(active: true);
					Main.tile[num42, num47].type = (byte)tileType;
				}
			}
			num42 += 2;
			num46 = num43 - 3;
			while (!Main.tile[num42, num46].HasTile)
			{
				num46--;
			}
			if (num43 - num46 < num43 - num44 + 5 && Main.tileDungeon[Main.tile[num42, num46].type])
			{
				for (int num48 = num43 - 4 - WorldGen.genRand.Next(3); num48 > num46; num48--)
				{
					Main.tile[num42, num48].active(active: true);
					Main.tile[num42, num48].type = (byte)tileType;
				}
			}
			num43++;
			num42--;
			Main.tile[num42 - 1, num43].active(active: true);
			Main.tile[num42 - 1, num43].type = (byte)tileType;
			Main.tile[num42 + 1, num43].active(active: true);
			Main.tile[num42 + 1, num43].type = (byte)tileType;
		}
		Main.statusText = Lang.gen[58]?.ToString() + " 85%";
		for (int num49 = 0; num49 < numDPlats; num49++)
		{
			int num50 = DPlatX[num49];
			int num51 = DPlatY[num49];
			int num52 = Main.maxTilesX;
			int num53 = 10;
			for (int num54 = num51 - 5; num54 <= num51 + 5; num54++)
			{
				int num55 = num50;
				int num56 = num50;
				bool flag3 = false;
				if (Main.tile[num55, num54].HasTile)
				{
					flag3 = true;
				}
				else
				{
					while (!Main.tile[num55, num54].HasTile)
					{
						num55--;
						if (!Main.tileDungeon[Main.tile[num55, num54].type])
						{
							flag3 = true;
						}
					}
					while (!Main.tile[num56, num54].HasTile)
					{
						num56++;
						if (!Main.tileDungeon[Main.tile[num56, num54].type])
						{
							flag3 = true;
						}
					}
				}
				if (flag3 || num56 - num55 > num53)
				{
					continue;
				}
				bool flag4 = true;
				int num57 = num50 - num53 / 2 - 2;
				int num58 = num50 + num53 / 2 + 2;
				int num59 = num54 - 5;
				int num60 = num54 + 5;
				for (int num61 = num57; num61 <= num58; num61++)
				{
					for (int num62 = num59; num62 <= num60; num62++)
					{
						if (Main.tile[num61, num62].HasTile && Main.tile[num61, num62].type == 19)
						{
							flag4 = false;
							break;
						}
					}
				}
				for (int num63 = num54 + 3; num63 >= num54 - 5; num63--)
				{
					if (Main.tile[num50, num63].HasTile)
					{
						flag4 = false;
						break;
					}
				}
				if (flag4)
				{
					num52 = num54;
					break;
				}
			}
			if (num52 > num51 - 10 && num52 < num51 + 10)
			{
				int num64 = num50;
				int num65 = num52;
				int num66 = num50 + 1;
				while (!Main.tile[num64, num65].HasTile)
				{
					Main.tile[num64, num65].active(active: true);
					Main.tile[num64, num65].type = 19;
					num64--;
				}
				for (; !Main.tile[num66, num65].HasTile; num66++)
				{
					Main.tile[num66, num65].active(active: true);
					Main.tile[num66, num65].type = 19;
				}
			}
			for (int num67 = 0; num67 < 5; num67++)
			{
				bool flag5 = false;
			}
		}
		Main.statusText = Lang.gen[58]?.ToString() + " 90%";
		num12 = 0;
		num13 = 1000;
		num14 = 0;
		while (num14 < Main.maxTilesX / 20)
		{
			num12++;
			int num68 = WorldGen.genRand.Next(dMinX, dMaxX);
			int num69 = WorldGen.genRand.Next(dMinY, dMaxY);
			bool flag6 = true;
			if (Main.tile[num68, num69].wall == wallType && !Main.tile[num68, num69].HasTile)
			{
				int num70 = 1;
				if (WorldGen.genRand.Next(2) == 0)
				{
					num70 = -1;
				}
				while (flag6 && !Main.tile[num68, num69].HasTile)
				{
					num68 -= num70;
					if (num68 < 5 || num68 > Main.maxTilesX - 5)
					{
						flag6 = false;
					}
					else if (Main.tile[num68, num69].HasTile && !Main.tileDungeon[Main.tile[num68, num69].type])
					{
						flag6 = false;
					}
				}
				if (flag6 && Main.tile[num68, num69].HasTile && Main.tileDungeon[Main.tile[num68, num69].type] && Main.tile[num68, num69 - 1].HasTile && Main.tileDungeon[Main.tile[num68, num69 - 1].type] && Main.tile[num68, num69 + 1].HasTile && Main.tileDungeon[Main.tile[num68, num69 + 1].type])
				{
					num68 += num70;
					for (int num71 = num68 - 3; num71 <= num68 + 3; num71++)
					{
						for (int num72 = num69 - 3; num72 <= num69 + 3; num72++)
						{
							if (Main.tile[num71, num72].HasTile && Main.tile[num71, num72].type == 19)
							{
								flag6 = false;
								break;
							}
						}
					}
					if (flag6 && (!Main.tile[num68, num69 - 1].HasTile & !Main.tile[num68, num69 - 2].HasTile & !Main.tile[num68, num69 - 3].HasTile))
					{
						int num73 = num68;
						int num74 = num68;
						for (; num73 > dMinX && num73 < dMaxX && !Main.tile[num73, num69].HasTile && !Main.tile[num73, num69 - 1].HasTile && !Main.tile[num73, num69 + 1].HasTile; num73 += num70)
						{
						}
						num73 = Math.Abs(num68 - num73);
						bool flag7 = false;
						if (WorldGen.genRand.Next(2) == 0)
						{
							flag7 = true;
						}
						if (num73 > 5)
						{
							for (int num75 = WorldGen.genRand.Next(1, 4); num75 > 0; num75--)
							{
								Main.tile[num68, num69].active(active: true);
								Main.tile[num68, num69].type = 19;
								if (flag7)
								{
									PlaceTile(num68, num69 - 1, 50, mute: true);
									if (WorldGen.genRand.Next(50) == 0 && Main.tile[num68, num69 - 1].type == 50)
									{
										Main.tile[num68, num69 - 1].frameX = 90;
									}
								}
								num68 += num70;
							}
							num12 = 0;
							num14++;
							if (!flag7 && WorldGen.genRand.Next(2) == 0)
							{
								num68 = num74;
								num69--;
								int num76 = 0;
								if (WorldGen.genRand.Next(4) == 0)
								{
									num76 = 1;
								}
								switch (num76)
								{
								case 0:
									num76 = 13;
									break;
								case 1:
									num76 = 49;
									break;
								}
								PlaceTile(num68, num69, num76, mute: true);
								if (Main.tile[num68, num69].type == 13)
								{
									if (WorldGen.genRand.Next(2) == 0)
									{
										Main.tile[num68, num69].frameX = 18;
									}
									else
									{
										Main.tile[num68, num69].frameX = 36;
									}
								}
							}
						}
					}
				}
			}
			if (num12 > num13)
			{
				num12 = 0;
				num14++;
			}
		}
		Main.statusText = Lang.gen[58]?.ToString() + " 95%";
		int num77 = 0;
		for (int num78 = 0; num78 < numDRooms; num78++)
		{
			int num79 = 0;
			while (num79 < 1000)
			{
				int num80 = (int)((double)dRoomSize[num78] * 0.4);
				int i2 = dRoomX[num78] + WorldGen.genRand.Next(-num80, num80 + 1);
				int num81 = dRoomY[num78] + WorldGen.genRand.Next(-num80, num80 + 1);
				int num82 = 0;
				num77++;
				int style = 2;
				switch (num77)
				{
				case 1:
					num82 = 329;
					break;
				case 2:
					num82 = 155;
					break;
				case 3:
					num82 = 156;
					break;
				case 4:
					num82 = 157;
					break;
				case 5:
					num82 = 163;
					break;
				case 6:
					num82 = 113;
					break;
				case 7:
					num82 = 327;
					style = 0;
					break;
				default:
					num82 = 164;
					num77 = 0;
					break;
				}
				if ((double)num81 < Main.worldSurface + 50.0)
				{
					num82 = 327;
					style = 0;
				}
				if (num82 == 0 && WorldGen.genRand.Next(2) == 0)
				{
					num79 = 1000;
					continue;
				}
				if (AddBuriedChest(i2, num81, num82, notNearOtherChests: false, style))
				{
					num79 += 1000;
				}
				num79++;
			}
		}
		dMinX -= 25;
		dMaxX += 25;
		dMinY -= 25;
		dMaxY += 25;
		if (dMinX < 0)
		{
			dMinX = 0;
		}
		if (dMaxX > Main.maxTilesX)
		{
			dMaxX = Main.maxTilesX;
		}
		if (dMinY < 0)
		{
			dMinY = 0;
		}
		if (dMaxY > Main.maxTilesY)
		{
			dMaxY = Main.maxTilesY;
		}
		num12 = 0;
		num13 = 1000;
		num14 = 0;
		while (num14 < Main.maxTilesX / 150)
		{
			num12++;
			int num83 = WorldGen.genRand.Next(dMinX, dMaxX);
			int num84 = WorldGen.genRand.Next(dMinY, dMaxY);
			if (Main.tile[num83, num84].wall == wallType)
			{
				for (int num85 = num84; num85 > dMinY; num85--)
				{
					if (Main.tile[num83, num85 - 1].HasTile && Main.tile[num83, num85 - 1].type == tileType)
					{
						bool flag8 = false;
						for (int num86 = num83 - 15; num86 < num83 + 15; num86++)
						{
							for (int num87 = num85 - 15; num87 < num85 + 15; num87++)
							{
								if (num86 > 0 && num86 < Main.maxTilesX && num87 > 0 && num87 < Main.maxTilesY && Main.tile[num86, num87].type == 42)
								{
									flag8 = true;
									break;
								}
							}
						}
						if (Main.tile[num83 - 1, num85].HasTile || Main.tile[num83 + 1, num85].HasTile || Main.tile[num83 - 1, num85 + 1].HasTile || Main.tile[num83 + 1, num85 + 1].HasTile || Main.tile[num83, num85 + 2].HasTile)
						{
							flag8 = true;
						}
						if (flag8)
						{
							break;
						}
						Place1x2Top(num83, num85, 42);
						if (Main.tile[num83, num85].type != 42)
						{
							break;
						}
						num12 = 0;
						num14++;
						for (int num88 = 0; num88 < 1000; num88++)
						{
							int num89 = num83 + WorldGen.genRand.Next(-12, 13);
							int num90 = num85 + WorldGen.genRand.Next(3, 21);
							if (Main.tile[num89, num90].HasTile || Main.tile[num89, num90 + 1].HasTile || Main.tile[num89 - 1, num90].type == 48 || Main.tile[num89 + 1, num90].type == 48 || !Collision.CanHit(new Vector2((float)(num89 * 16), (float)(num90 * 16)), 16, 16, new Vector2((float)(num83 * 16), (float)(num85 * 16 + 1)), 16, 16))
							{
								continue;
							}
							PlaceTile(num89, num90, 136, mute: true);
							if (!Main.tile[num89, num90].HasTile)
							{
								continue;
							}
							while (num89 != num83 || num90 != num85)
							{
								Main.tile[num89, num90].wire(wire: true);
								if (num89 > num83)
								{
									num89--;
								}
								if (num89 < num83)
								{
									num89++;
								}
								Main.tile[num89, num90].wire(wire: true);
								if (num90 > num85)
								{
									num90--;
								}
								if (num90 < num85)
								{
									num90++;
								}
								Main.tile[num89, num90].wire(wire: true);
							}
							if (Main.rand.Next(3) > 0)
							{
								Main.tile[num83, num85].frameX = 18;
								Main.tile[num83, num85 + 1].frameX = 18;
							}
							break;
						}
						break;
					}
				}
			}
			if (num12 > num13)
			{
				num14++;
				num12 = 0;
			}
		}
		num12 = 0;
		num13 = 1000;
		num14 = 0;
		while (num14 < Main.maxTilesX / 500)
		{
			num12++;
			int num91 = WorldGen.genRand.Next(dMinX, dMaxX);
			int num92 = WorldGen.genRand.Next(dMinY, dMaxY);
			if (Main.tile[num91, num92].wall == wallType && WorldGen.placeTrap(num91, num92, 0))
			{
				num12 = num13;
			}
			if (num12 > num13)
			{
				num14++;
				num12 = 0;
			}
		}
	}

	public static void DungeonStairs(int i, int j, int tileType, int wallType)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = default(Vector2);
		double num = WorldGen.genRand.Next(5, 9);
		int num2 = 1;
		Vector2 vector2 = new Vector2
		{
			X = i,
			Y = j
		};
		int num3 = WorldGen.genRand.Next(10, 30);
		num2 = ((i <= dEnteranceX) ? 1 : (-1));
		if (i > Main.maxTilesX - 400)
		{
			num2 = -1;
		}
		else if (i < 400)
		{
			num2 = 1;
		}
		vector.Y = -1f;
		vector.X = num2;
		if (WorldGen.genRand.Next(3) == 0)
		{
			vector.X *= 0.5f;
		}
		else if (WorldGen.genRand.Next(3) == 0)
		{
			vector.Y *= 2f;
		}
		while (num3 > 0)
		{
			num3--;
			int num4 = (int)((double)vector2.X - num - 4.0 - (double)WorldGen.genRand.Next(6));
			int num5 = (int)((double)vector2.X + num + 4.0 + (double)WorldGen.genRand.Next(6));
			int num6 = (int)((double)vector2.Y - num - 4.0);
			int num7 = (int)((double)vector2.Y + num + 4.0 + (double)WorldGen.genRand.Next(6));
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 > Main.maxTilesX)
			{
				num5 = Main.maxTilesX;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.maxTilesY)
			{
				num7 = Main.maxTilesY;
			}
			int num8 = 1;
			if (vector2.X > (float)(Main.maxTilesX / 2))
			{
				num8 = -1;
			}
			int num9 = (int)(vector2.X + (float)dxStrength1 * 0.6f * (float)num8 + (float)dxStrength2 * (float)num8);
			int num10 = (int)(dyStrength2 * 0.5);
			if ((double)vector2.Y < Main.worldSurface - 5.0 && Main.tile[num9, (int)((double)vector2.Y - num - 6.0 + (double)num10)].wall == 0 && Main.tile[num9, (int)((double)vector2.Y - num - 7.0 + (double)num10)].wall == 0 && Main.tile[num9, (int)((double)vector2.Y - num - 8.0 + (double)num10)].wall == 0)
			{
				dSurface = true;
				TileRunner(num9, (int)((double)vector2.Y - num - 6.0 + (double)num10), WorldGen.genRand.Next(25, 35), WorldGen.genRand.Next(10, 20), -1, addTile: false, 0f, -1f);
			}
			for (int k = num4; k < num5; k++)
			{
				for (int l = num6; l < num7; l++)
				{
					Main.tile[k, l].liquid = 0;
					if (Main.tile[k, l].wall != wallType)
					{
						Main.tile[k, l].wall = 0;
						Main.tile[k, l].active(active: true);
						Main.tile[k, l].type = (byte)tileType;
					}
				}
			}
			for (int m = num4 + 1; m < num5 - 1; m++)
			{
				for (int n = num6 + 1; n < num7 - 1; n++)
				{
					PlaceWall(m, n, wallType, mute: true);
				}
			}
			int num11 = 0;
			if (WorldGen.genRand.Next((int)num) == 0)
			{
				num11 = WorldGen.genRand.Next(1, 3);
			}
			num4 = (int)((double)vector2.X - num * 0.5 - (double)num11);
			num5 = (int)((double)vector2.X + num * 0.5 + (double)num11);
			num6 = (int)((double)vector2.Y - num * 0.5 - (double)num11);
			num7 = (int)((double)vector2.Y + num * 0.5 + (double)num11);
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 > Main.maxTilesX)
			{
				num5 = Main.maxTilesX;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.maxTilesY)
			{
				num7 = Main.maxTilesY;
			}
			for (int num12 = num4; num12 < num5; num12++)
			{
				for (int num13 = num6; num13 < num7; num13++)
				{
					Main.tile[num12, num13].active(active: false);
					PlaceWall(num12, num13, wallType, mute: true);
				}
			}
			if (dSurface)
			{
				num3 = 0;
			}
			vector2 += vector;
		}
		dungeonX = (int)vector2.X;
		dungeonY = (int)vector2.Y;
	}

	public static void DungeonHalls(int i, int j, int tileType, int wallType, bool forceX = false)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0955: Unknown result type (might be due to invalid IL or missing references)
		//IL_0962: Unknown result type (might be due to invalid IL or missing references)
		//IL_096f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Unknown result type (might be due to invalid IL or missing references)
		//IL_0851: Unknown result type (might be due to invalid IL or missing references)
		//IL_086c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0887: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0943: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_0946: Unknown result type (might be due to invalid IL or missing references)
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = default(Vector2);
		double num = WorldGen.genRand.Next(4, 6);
		Vector2 vector2 = default(Vector2);
		Vector2 vector3 = default(Vector2);
		int num2 = 1;
		Vector2 vector4 = new Vector2
		{
			X = i,
			Y = j
		};
		int num3 = WorldGen.genRand.Next(35, 80);
		if (forceX)
		{
			num3 += 20;
			lastDungeonHall = default(Vector2);
		}
		else if (WorldGen.genRand.Next(5) == 0)
		{
			num *= 2.0;
			num3 /= 2;
		}
		bool flag = false;
		while (!flag)
		{
			num2 = ((WorldGen.genRand.Next(2) != 0) ? 1 : (-1));
			bool flag2 = false;
			if (WorldGen.genRand.Next(2) == 0)
			{
				flag2 = true;
			}
			if (forceX)
			{
				flag2 = true;
			}
			if (flag2)
			{
				vector2.Y = 0f;
				vector2.X = num2;
				vector3.Y = 0f;
				vector3.X = -num2;
				vector.Y = 0f;
				vector.X = num2;
				if (WorldGen.genRand.Next(3) == 0)
				{
					if (WorldGen.genRand.Next(2) == 0)
					{
						vector.Y = -0.2f;
					}
					else
					{
						vector.Y = 0.2f;
					}
				}
			}
			else
			{
				num++;
				vector.Y = num2;
				vector.X = 0f;
				vector2.X = 0f;
				vector2.Y = num2;
				vector3.X = 0f;
				vector3.Y = -num2;
				if (WorldGen.genRand.Next(2) == 0)
				{
					if (WorldGen.genRand.Next(2) == 0)
					{
						vector.X = 0.3f;
					}
					else
					{
						vector.X = -0.3f;
					}
				}
				else
				{
					num3 /= 2;
				}
			}
			if (lastDungeonHall != vector3)
			{
				flag = true;
			}
		}
		if (!forceX)
		{
			if (vector4.X > (float)(lastMaxTilesX - 200))
			{
				num2 = -1;
				vector2.Y = 0f;
				vector2.X = num2;
				vector.Y = 0f;
				vector.X = num2;
				if (WorldGen.genRand.Next(3) == 0)
				{
					if (WorldGen.genRand.Next(2) == 0)
					{
						vector.Y = -0.2f;
					}
					else
					{
						vector.Y = 0.2f;
					}
				}
			}
			else if (vector4.X < 200f)
			{
				num2 = 1;
				vector2.Y = 0f;
				vector2.X = num2;
				vector.Y = 0f;
				vector.X = num2;
				if (WorldGen.genRand.Next(3) == 0)
				{
					if (WorldGen.genRand.Next(2) == 0)
					{
						vector.Y = -0.2f;
					}
					else
					{
						vector.Y = 0.2f;
					}
				}
			}
			else if (vector4.Y > (float)(lastMaxTilesY - 300))
			{
				num2 = -1;
				num++;
				vector.Y = num2;
				vector.X = 0f;
				vector2.X = 0f;
				vector2.Y = num2;
				if (WorldGen.genRand.Next(2) == 0)
				{
					if (WorldGen.genRand.Next(2) == 0)
					{
						vector.X = 0.3f;
					}
					else
					{
						vector.X = -0.3f;
					}
				}
			}
			else if ((double)vector4.Y < Main.rockLayer)
			{
				num2 = 1;
				num++;
				vector.Y = num2;
				vector.X = 0f;
				vector2.X = 0f;
				vector2.Y = num2;
				if (WorldGen.genRand.Next(2) == 0)
				{
					if (WorldGen.genRand.Next(2) == 0)
					{
						vector.X = 0.3f;
					}
					else
					{
						vector.X = -0.3f;
					}
				}
			}
			else if (vector4.X < (float)(Main.maxTilesX / 2) && (double)vector4.X > (double)Main.maxTilesX * 0.25)
			{
				num2 = -1;
				vector2.Y = 0f;
				vector2.X = num2;
				vector.Y = 0f;
				vector.X = num2;
				if (WorldGen.genRand.Next(3) == 0)
				{
					if (WorldGen.genRand.Next(2) == 0)
					{
						vector.Y = -0.2f;
					}
					else
					{
						vector.Y = 0.2f;
					}
				}
			}
			else if (vector4.X > (float)(Main.maxTilesX / 2) && (double)vector4.X < (double)Main.maxTilesX * 0.75)
			{
				num2 = 1;
				vector2.Y = 0f;
				vector2.X = num2;
				vector.Y = 0f;
				vector.X = num2;
				if (WorldGen.genRand.Next(3) == 0)
				{
					if (WorldGen.genRand.Next(2) == 0)
					{
						vector.Y = -0.2f;
					}
					else
					{
						vector.Y = 0.2f;
					}
				}
			}
		}
		if (vector2.Y == 0f)
		{
			DDoorX[numDDoors] = (int)vector4.X;
			DDoorY[numDDoors] = (int)vector4.Y;
			DDoorPos[numDDoors] = 0;
			numDDoors++;
		}
		else
		{
			DPlatX[numDPlats] = (int)vector4.X;
			DPlatY[numDPlats] = (int)vector4.Y;
			numDPlats++;
		}
		lastDungeonHall = vector2;
		while (num3 > 0)
		{
			if (vector2.X > 0f && vector4.X > (float)(Main.maxTilesX - 100))
			{
				num3 = 0;
			}
			else if (vector2.X < 0f && vector4.X < 100f)
			{
				num3 = 0;
			}
			else if (vector2.Y > 0f && vector4.Y > (float)(Main.maxTilesY - 100))
			{
				num3 = 0;
			}
			else if (vector2.Y < 0f && (double)vector4.Y < Main.rockLayer + 50.0)
			{
				num3 = 0;
			}
			num3--;
			int num4 = (int)((double)vector4.X - num - 4.0 - (double)WorldGen.genRand.Next(6));
			int num5 = (int)((double)vector4.X + num + 4.0 + (double)WorldGen.genRand.Next(6));
			int num6 = (int)((double)vector4.Y - num - 4.0 - (double)WorldGen.genRand.Next(6));
			int num7 = (int)((double)vector4.Y + num + 4.0 + (double)WorldGen.genRand.Next(6));
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 > Main.maxTilesX)
			{
				num5 = Main.maxTilesX;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.maxTilesY)
			{
				num7 = Main.maxTilesY;
			}
			for (int k = num4; k < num5; k++)
			{
				for (int l = num6; l < num7; l++)
				{
					Main.tile[k, l].liquid = 0;
					if (Main.tile[k, l].wall == 0)
					{
						Main.tile[k, l].active(active: true);
						Main.tile[k, l].type = (byte)tileType;
					}
				}
			}
			for (int m = num4 + 1; m < num5 - 1; m++)
			{
				for (int n = num6 + 1; n < num7 - 1; n++)
				{
					PlaceWall(m, n, wallType, mute: true);
				}
			}
			int num8 = 0;
			if (vector.Y == 0f && WorldGen.genRand.Next((int)num + 1) == 0)
			{
				num8 = WorldGen.genRand.Next(1, 3);
			}
			else if (vector.X == 0f && WorldGen.genRand.Next((int)num - 1) == 0)
			{
				num8 = WorldGen.genRand.Next(1, 3);
			}
			else if (WorldGen.genRand.Next((int)num * 3) == 0)
			{
				num8 = WorldGen.genRand.Next(1, 3);
			}
			num4 = (int)((double)vector4.X - num * 0.5 - (double)num8);
			num5 = (int)((double)vector4.X + num * 0.5 + (double)num8);
			num6 = (int)((double)vector4.Y - num * 0.5 - (double)num8);
			num7 = (int)((double)vector4.Y + num * 0.5 + (double)num8);
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 > Main.maxTilesX)
			{
				num5 = Main.maxTilesX;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.maxTilesY)
			{
				num7 = Main.maxTilesY;
			}
			for (int num9 = num4; num9 < num5; num9++)
			{
				for (int num10 = num6; num10 < num7; num10++)
				{
					Main.tile[num9, num10].active(active: false);
					Main.tile[num9, num10].wall = (byte)wallType;
				}
			}
			vector4 += vector;
		}
		dungeonX = (int)vector4.X;
		dungeonY = (int)vector4.Y;
		if (vector2.Y == 0f)
		{
			DDoorX[numDDoors] = (int)vector4.X;
			DDoorY[numDDoors] = (int)vector4.Y;
			DDoorPos[numDDoors] = 0;
			numDDoors++;
		}
		else
		{
			DPlatX[numDPlats] = (int)vector4.X;
			DPlatY[numDPlats] = (int)vector4.Y;
			numDPlats++;
		}
	}

	public static void DungeonRoom(int i, int j, int tileType, int wallType)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		double num = WorldGen.genRand.Next(15, 30);
		Vector2 vector = new Vector2
		{
			X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f,
			Y = (float)WorldGen.genRand.Next(-10, 11) * 0.1f
		};
		Vector2 vector2 = new Vector2
		{
			X = i,
			Y = (float)j - (float)num / 2f
		};
		int num2 = WorldGen.genRand.Next(10, 20);
		double num3 = vector2.X;
		double num4 = vector2.X;
		double num5 = vector2.Y;
		double num6 = vector2.Y;
		while (num2 > 0)
		{
			num2--;
			int num7 = (int)((double)vector2.X - num * 0.800000011920929 - 5.0);
			int num8 = (int)((double)vector2.X + num * 0.800000011920929 + 5.0);
			int num9 = (int)((double)vector2.Y - num * 0.800000011920929 - 5.0);
			int num10 = (int)((double)vector2.Y + num * 0.800000011920929 + 5.0);
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.maxTilesX)
			{
				num8 = Main.maxTilesX;
			}
			if (num9 < 0)
			{
				num9 = 0;
			}
			if (num10 > Main.maxTilesY)
			{
				num10 = Main.maxTilesY;
			}
			for (int k = num7; k < num8; k++)
			{
				for (int l = num9; l < num10; l++)
				{
					Main.tile[k, l].liquid = 0;
					if (Main.tile[k, l].wall == 0)
					{
						Main.tile[k, l].active(active: true);
						Main.tile[k, l].type = (byte)tileType;
					}
				}
			}
			for (int m = num7 + 1; m < num8 - 1; m++)
			{
				for (int n = num9 + 1; n < num10 - 1; n++)
				{
					PlaceWall(m, n, wallType, mute: true);
				}
			}
			num7 = (int)((double)vector2.X - num * 0.5);
			num8 = (int)((double)vector2.X + num * 0.5);
			num9 = (int)((double)vector2.Y - num * 0.5);
			num10 = (int)((double)vector2.Y + num * 0.5);
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.maxTilesX)
			{
				num8 = Main.maxTilesX;
			}
			if (num9 < 0)
			{
				num9 = 0;
			}
			if (num10 > Main.maxTilesY)
			{
				num10 = Main.maxTilesY;
			}
			if ((double)num7 < num3)
			{
				num3 = num7;
			}
			if ((double)num8 > num4)
			{
				num4 = num8;
			}
			if ((double)num9 < num5)
			{
				num5 = num9;
			}
			if ((double)num10 > num6)
			{
				num6 = num10;
			}
			for (int num11 = num7; num11 < num8; num11++)
			{
				for (int num12 = num9; num12 < num10; num12++)
				{
					Main.tile[num11, num12].active(active: false);
					Main.tile[num11, num12].wall = (byte)wallType;
				}
			}
			vector2 += vector;
			vector.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			vector.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			if (vector.X > 1f)
			{
				vector.X = 1f;
			}
			if (vector.X < -1f)
			{
				vector.X = -1f;
			}
			if (vector.Y > 1f)
			{
				vector.Y = 1f;
			}
			if (vector.Y < -1f)
			{
				vector.Y = -1f;
			}
		}
		dRoomX[numDRooms] = (int)vector2.X;
		dRoomY[numDRooms] = (int)vector2.Y;
		dRoomSize[numDRooms] = (int)num;
		dRoomL[numDRooms] = (int)num3;
		dRoomR[numDRooms] = (int)num4;
		dRoomT[numDRooms] = (int)num5;
		dRoomB[numDRooms] = (int)num6;
		dRoomTreasure[numDRooms] = false;
		numDRooms++;
	}

	public static void DungeonEnt(int i, int j, int tileType, int wallType)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0883: Unknown result type (might be due to invalid IL or missing references)
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff6: Unknown result type (might be due to invalid IL or missing references)
		//IL_108d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1094: Unknown result type (might be due to invalid IL or missing references)
		int num = 60;
		for (int k = i - num; k < i + num; k++)
		{
			for (int l = j - num; l < j + num; l++)
			{
				Main.tile[k, l].liquid = 0;
				Main.tile[k, l].lava(lava: false);
			}
		}
		double num2 = dxStrength1;
		double num3 = dyStrength1;
		Vector2 vector = new Vector2
		{
			X = i,
			Y = (float)j - (float)num3 / 2f
		};
		dMinY = (int)vector.Y;
		int num4 = 1;
		if (i > Main.maxTilesX / 2)
		{
			num4 = -1;
		}
		int num5 = (int)((double)vector.X - num2 * 0.6000000238418579 - (double)WorldGen.genRand.Next(2, 5));
		int num6 = (int)((double)vector.X + num2 * 0.6000000238418579 + (double)WorldGen.genRand.Next(2, 5));
		int num7 = (int)((double)vector.Y - num3 * 0.6000000238418579 - (double)WorldGen.genRand.Next(2, 5));
		int num8 = (int)((double)vector.Y + num3 * 0.6000000238418579 + (double)WorldGen.genRand.Next(8, 16));
		if (num5 < 0)
		{
			num5 = 0;
		}
		if (num6 > Main.maxTilesX)
		{
			num6 = Main.maxTilesX;
		}
		if (num7 < 0)
		{
			num7 = 0;
		}
		if (num8 > Main.maxTilesY)
		{
			num8 = Main.maxTilesY;
		}
		for (int m = num5; m < num6; m++)
		{
			for (int n = num7; n < num8; n++)
			{
				Main.tile[m, n].liquid = 0;
				if (Main.tile[m, n].wall != wallType)
				{
					Main.tile[m, n].wall = 0;
					if (m > num5 + 1 && m < num6 - 2 && n > num7 + 1 && n < num8 - 2)
					{
						PlaceWall(m, n, wallType, mute: true);
					}
					Main.tile[m, n].active(active: true);
					Main.tile[m, n].type = (byte)tileType;
				}
			}
		}
		int num9 = num5;
		int num10 = num5 + 5 + WorldGen.genRand.Next(4);
		int num11 = num7 - 3 - WorldGen.genRand.Next(3);
		int num12 = num7;
		for (int num13 = num9; num13 < num10; num13++)
		{
			for (int num14 = num11; num14 < num12; num14++)
			{
				if (Main.tile[num13, num14].wall != wallType)
				{
					Main.tile[num13, num14].active(active: true);
					Main.tile[num13, num14].type = (byte)tileType;
				}
			}
		}
		num9 = num6 - 5 - WorldGen.genRand.Next(4);
		num10 = num6;
		num11 = num7 - 3 - WorldGen.genRand.Next(3);
		num12 = num7;
		for (int num15 = num9; num15 < num10; num15++)
		{
			for (int num16 = num11; num16 < num12; num16++)
			{
				if (Main.tile[num15, num16].wall != wallType)
				{
					Main.tile[num15, num16].active(active: true);
					Main.tile[num15, num16].type = (byte)tileType;
				}
			}
		}
		int num17 = 1 + WorldGen.genRand.Next(2);
		int num18 = 2 + WorldGen.genRand.Next(4);
		int num19 = 0;
		for (int num20 = num5; num20 < num6; num20++)
		{
			for (int num21 = num7 - num17; num21 < num7; num21++)
			{
				if (Main.tile[num20, num21].wall != wallType)
				{
					Main.tile[num20, num21].active(active: true);
					Main.tile[num20, num21].type = (byte)tileType;
				}
			}
			num19++;
			if (num19 >= num18)
			{
				num20 += num18;
				num19 = 0;
			}
		}
		for (int num22 = num5; num22 < num6; num22++)
		{
			for (int num23 = num8; num23 < num8 + 100; num23++)
			{
				PlaceWall(num22, num23, 2, mute: true);
			}
		}
		num5 = (int)((double)vector.X - num2 * 0.6000000238418579);
		num6 = (int)((double)vector.X + num2 * 0.6000000238418579);
		num7 = (int)((double)vector.Y - num3 * 0.6000000238418579);
		num8 = (int)((double)vector.Y + num3 * 0.6000000238418579);
		if (num5 < 0)
		{
			num5 = 0;
		}
		if (num6 > Main.maxTilesX)
		{
			num6 = Main.maxTilesX;
		}
		if (num7 < 0)
		{
			num7 = 0;
		}
		if (num8 > Main.maxTilesY)
		{
			num8 = Main.maxTilesY;
		}
		for (int num24 = num5; num24 < num6; num24++)
		{
			for (int num25 = num7; num25 < num8; num25++)
			{
				PlaceWall(num24, num25, wallType, mute: true);
			}
		}
		num5 = (int)((double)vector.X - num2 * 0.6 - 1.0);
		num6 = (int)((double)vector.X + num2 * 0.6 + 1.0);
		num7 = (int)((double)vector.Y - num3 * 0.6 - 1.0);
		num8 = (int)((double)vector.Y + num3 * 0.6 + 1.0);
		if (num5 < 0)
		{
			num5 = 0;
		}
		if (num6 > Main.maxTilesX)
		{
			num6 = Main.maxTilesX;
		}
		if (num7 < 0)
		{
			num7 = 0;
		}
		if (num8 > Main.maxTilesY)
		{
			num8 = Main.maxTilesY;
		}
		for (int num26 = num5; num26 < num6; num26++)
		{
			for (int num27 = num7; num27 < num8; num27++)
			{
				Main.tile[num26, num27].wall = (byte)wallType;
			}
		}
		num5 = (int)((double)vector.X - num2 * 0.5);
		num6 = (int)((double)vector.X + num2 * 0.5);
		num7 = (int)((double)vector.Y - num3 * 0.5);
		num8 = (int)((double)vector.Y + num3 * 0.5);
		if (num5 < 0)
		{
			num5 = 0;
		}
		if (num6 > Main.maxTilesX)
		{
			num6 = Main.maxTilesX;
		}
		if (num7 < 0)
		{
			num7 = 0;
		}
		if (num8 > Main.maxTilesY)
		{
			num8 = Main.maxTilesY;
		}
		for (int num28 = num5; num28 < num6; num28++)
		{
			for (int num29 = num7; num29 < num8; num29++)
			{
				Main.tile[num28, num29].active(active: false);
				Main.tile[num28, num29].wall = (byte)wallType;
			}
		}
		DPlatX[numDPlats] = (int)vector.X;
		DPlatY[numDPlats] = num8;
		numDPlats++;
		vector.X += (float)num2 * 0.6f * (float)num4;
		vector.Y += (float)num3 * 0.5f;
		num2 = dxStrength2;
		num3 = dyStrength2;
		vector.X += (float)num2 * 0.55f * (float)num4;
		vector.Y -= (float)num3 * 0.5f;
		num5 = (int)((double)vector.X - num2 * 0.6000000238418579 - (double)WorldGen.genRand.Next(1, 3));
		num6 = (int)((double)vector.X + num2 * 0.6000000238418579 + (double)WorldGen.genRand.Next(1, 3));
		num7 = (int)((double)vector.Y - num3 * 0.6000000238418579 - (double)WorldGen.genRand.Next(1, 3));
		num8 = (int)((double)vector.Y + num3 * 0.6000000238418579 + (double)WorldGen.genRand.Next(6, 16));
		if (num5 < 0)
		{
			num5 = 0;
		}
		if (num6 > Main.maxTilesX)
		{
			num6 = Main.maxTilesX;
		}
		if (num7 < 0)
		{
			num7 = 0;
		}
		if (num8 > Main.maxTilesY)
		{
			num8 = Main.maxTilesY;
		}
		for (int num30 = num5; num30 < num6; num30++)
		{
			for (int num31 = num7; num31 < num8; num31++)
			{
				if (Main.tile[num30, num31].wall == wallType)
				{
					continue;
				}
				bool flag = true;
				if (num4 < 0)
				{
					if ((double)num30 < (double)vector.X - num2 * 0.5)
					{
						flag = false;
					}
				}
				else if ((double)num30 > (double)vector.X + num2 * 0.5 - 1.0)
				{
					flag = false;
				}
				if (flag)
				{
					Main.tile[num30, num31].wall = 0;
					Main.tile[num30, num31].active(active: true);
					Main.tile[num30, num31].type = (byte)tileType;
				}
			}
		}
		for (int num32 = num5; num32 < num6; num32++)
		{
			for (int num33 = num8; num33 < num8 + 100; num33++)
			{
				PlaceWall(num32, num33, 2, mute: true);
			}
		}
		num5 = (int)((double)vector.X - num2 * 0.5);
		num6 = (int)((double)vector.X + num2 * 0.5);
		num9 = num5;
		if (num4 < 0)
		{
			num9++;
		}
		num10 = num9 + 5 + WorldGen.genRand.Next(4);
		num11 = num7 - 3 - WorldGen.genRand.Next(3);
		num12 = num7;
		for (int num34 = num9; num34 < num10; num34++)
		{
			for (int num35 = num11; num35 < num12; num35++)
			{
				if (Main.tile[num34, num35].wall != wallType)
				{
					Main.tile[num34, num35].active(active: true);
					Main.tile[num34, num35].type = (byte)tileType;
				}
			}
		}
		num9 = num6 - 5 - WorldGen.genRand.Next(4);
		num10 = num6;
		num11 = num7 - 3 - WorldGen.genRand.Next(3);
		num12 = num7;
		for (int num36 = num9; num36 < num10; num36++)
		{
			for (int num37 = num11; num37 < num12; num37++)
			{
				if (Main.tile[num36, num37].wall != wallType)
				{
					Main.tile[num36, num37].active(active: true);
					Main.tile[num36, num37].type = (byte)tileType;
				}
			}
		}
		num17 = 1 + WorldGen.genRand.Next(2);
		num18 = 2 + WorldGen.genRand.Next(4);
		num19 = 0;
		if (num4 < 0)
		{
			num6++;
		}
		for (int num38 = num5 + 1; num38 < num6 - 1; num38++)
		{
			for (int num39 = num7 - num17; num39 < num7; num39++)
			{
				if (Main.tile[num38, num39].wall != wallType)
				{
					Main.tile[num38, num39].active(active: true);
					Main.tile[num38, num39].type = (byte)tileType;
				}
			}
			num19++;
			if (num19 >= num18)
			{
				num38 += num18;
				num19 = 0;
			}
		}
		num5 = (int)((double)vector.X - num2 * 0.6);
		num6 = (int)((double)vector.X + num2 * 0.6);
		num7 = (int)((double)vector.Y - num3 * 0.6);
		num8 = (int)((double)vector.Y + num3 * 0.6);
		if (num5 < 0)
		{
			num5 = 0;
		}
		if (num6 > Main.maxTilesX)
		{
			num6 = Main.maxTilesX;
		}
		if (num7 < 0)
		{
			num7 = 0;
		}
		if (num8 > Main.maxTilesY)
		{
			num8 = Main.maxTilesY;
		}
		for (int num40 = num5; num40 < num6; num40++)
		{
			for (int num41 = num7; num41 < num8; num41++)
			{
				Main.tile[num40, num41].wall = 0;
			}
		}
		num5 = (int)((double)vector.X - num2 * 0.5);
		num6 = (int)((double)vector.X + num2 * 0.5);
		num7 = (int)((double)vector.Y - num3 * 0.5);
		num8 = (int)((double)vector.Y + num3 * 0.5);
		if (num5 < 0)
		{
			num5 = 0;
		}
		if (num6 > Main.maxTilesX)
		{
			num6 = Main.maxTilesX;
		}
		if (num7 < 0)
		{
			num7 = 0;
		}
		if (num8 > Main.maxTilesY)
		{
			num8 = Main.maxTilesY;
		}
		for (int num42 = num5; num42 < num6; num42++)
		{
			for (int num43 = num7; num43 < num8; num43++)
			{
				Main.tile[num42, num43].active(active: false);
				Main.tile[num42, num43].wall = 0;
			}
		}
		for (int num44 = num5; num44 < num6; num44++)
		{
			if (!Main.tile[num44, num8].HasTile)
			{
				Main.tile[num44, num8].active(active: true);
				Main.tile[num44, num8].type = 19;
			}
		}
		Main.dungeonX = (int)vector.X;
		Main.dungeonY = num8;
		int num45 = NPC.NewNPC(null, Main.dungeonX * 16 + 8, Main.dungeonY * 16, 37);
		Main.npc[num45].homeless = false;
		Main.npc[num45].homeTileX = Main.dungeonX;
		Main.npc[num45].homeTileY = Main.dungeonY;
		if (num4 == 1)
		{
			int num46 = 0;
			for (int num47 = num6; num47 < num6 + 25; num47++)
			{
				num46++;
				for (int num48 = num8 + num46; num48 < num8 + 25; num48++)
				{
					Main.tile[num47, num48].active(active: true);
					Main.tile[num47, num48].type = (byte)tileType;
				}
			}
		}
		else
		{
			int num49 = 0;
			for (int num50 = num5; num50 > num5 - 25; num50--)
			{
				num49++;
				for (int num51 = num8 + num49; num51 < num8 + 25; num51++)
				{
					Main.tile[num50, num51].active(active: true);
					Main.tile[num50, num51].type = (byte)tileType;
				}
			}
		}
		num17 = 1 + WorldGen.genRand.Next(2);
		num18 = 2 + WorldGen.genRand.Next(4);
		num19 = 0;
		num5 = (int)((double)vector.X - num2 * 0.5);
		num6 = (int)((double)vector.X + num2 * 0.5);
		num5 += 2;
		num6 -= 2;
		for (int num52 = num5; num52 < num6; num52++)
		{
			for (int num53 = num7; num53 < num8; num53++)
			{
				PlaceWall(num52, num53, wallType, mute: true);
			}
			num19++;
			if (num19 >= num18)
			{
				num52 += num18 * 2;
				num19 = 0;
			}
		}
		vector.X -= (float)num2 * 0.6f * (float)num4;
		vector.Y += (float)num3 * 0.5f;
		num2 = 15.0;
		num3 = 3.0;
		vector.Y -= (float)num3 * 0.5f;
		num5 = (int)((double)vector.X - num2 * 0.5);
		num6 = (int)((double)vector.X + num2 * 0.5);
		num7 = (int)((double)vector.Y - num3 * 0.5);
		num8 = (int)((double)vector.Y + num3 * 0.5);
		if (num5 < 0)
		{
			num5 = 0;
		}
		if (num6 > Main.maxTilesX)
		{
			num6 = Main.maxTilesX;
		}
		if (num7 < 0)
		{
			num7 = 0;
		}
		if (num8 > Main.maxTilesY)
		{
			num8 = Main.maxTilesY;
		}
		for (int num54 = num5; num54 < num6; num54++)
		{
			for (int num55 = num7; num55 < num8; num55++)
			{
				Main.tile[num54, num55].active(active: false);
			}
		}
		if (num4 < 0)
		{
			vector.X--;
		}
		PlaceTile((int)vector.X, (int)vector.Y + 1, 10);
	}

	public static bool AddBuriedChest(int i, int j, int contain = 0, bool notNearOtherChests = false, int Style = -1)
	{
		for (int k = j; k < Main.maxTilesY; k++)
		{
			if (!Main.tile[i, k].HasTile || !Main.tileSolid[Main.tile[i, k].type])
			{
				continue;
			}
			bool flag = false;
			int num = k;
			int num2 = -1;
			int style = 0;
			if ((double)num >= Main.worldSurface + 25.0 || contain > 0)
			{
				style = 1;
			}
			if (Style >= 0)
			{
				style = Style;
			}
			if (num > Main.maxTilesY - 205 && contain == 0)
			{
				if (hellChest == 0)
				{
					contain = 274;
					style = 4;
					flag = true;
				}
				else if (hellChest == 1)
				{
					contain = 220;
					style = 4;
					flag = true;
				}
				else if (hellChest == 2)
				{
					contain = 112;
					style = 4;
					flag = true;
				}
				else if (hellChest == 3)
				{
					contain = 218;
					style = 4;
					flag = true;
					hellChest = 0;
				}
			}
			if (WorldGen.InWorld(i - 1, num - 1, 20))
			{
				num2 = PlaceChest(i - 1, num - 1, 21, notNearOtherChests, style);
			}
			if (num2 >= 0)
			{
				if (flag)
				{
					hellChest++;
				}
				int num3 = 0;
				while (num3 == 0)
				{
					if ((double)num < Main.worldSurface + 25.0)
					{
						if (contain > 0)
						{
							Main.chest[num2].item[num3].SetDefaults(contain);
							Main.chest[num2].item[num3].Prefix(-1);
							num3++;
						}
						else
						{
							int num4 = WorldGen.genRand.Next(6);
							if (num4 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(280);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							if (num4 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(281);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							if (num4 == 2)
							{
								Main.chest[num2].item[num3].SetDefaults(284);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							if (num4 == 3)
							{
								Main.chest[num2].item[num3].SetDefaults(282);
								Main.chest[num2].item[num3].stack = WorldGen.genRand.Next(50, 75);
							}
							if (num4 == 4)
							{
								Main.chest[num2].item[num3].SetDefaults(279);
								Main.chest[num2].item[num3].stack = WorldGen.genRand.Next(25, 50);
							}
							if (num4 == 5)
							{
								Main.chest[num2].item[num3].SetDefaults(285);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							num3++;
						}
						if (WorldGen.genRand.Next(3) == 0)
						{
							Main.chest[num2].item[num3].SetDefaults(168);
							Main.chest[num2].item[num3].stack = WorldGen.genRand.Next(3, 6);
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							int num5 = WorldGen.genRand.Next(2);
							int stack = WorldGen.genRand.Next(8) + 3;
							if (num5 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(20);
							}
							if (num5 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(22);
							}
							Main.chest[num2].item[num3].stack = stack;
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							int num6 = WorldGen.genRand.Next(2);
							int stack2 = WorldGen.genRand.Next(26) + 25;
							if (num6 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(40);
							}
							if (num6 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(42);
							}
							Main.chest[num2].item[num3].stack = stack2;
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							int num7 = WorldGen.genRand.Next(1);
							int stack3 = WorldGen.genRand.Next(3) + 3;
							if (num7 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(28);
							}
							Main.chest[num2].item[num3].stack = stack3;
							num3++;
						}
						if (WorldGen.genRand.Next(3) > 0)
						{
							int num8 = WorldGen.genRand.Next(4);
							int stack4 = WorldGen.genRand.Next(1, 3);
							if (num8 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(292);
							}
							if (num8 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(298);
							}
							if (num8 == 2)
							{
								Main.chest[num2].item[num3].SetDefaults(299);
							}
							if (num8 == 3)
							{
								Main.chest[num2].item[num3].SetDefaults(290);
							}
							Main.chest[num2].item[num3].stack = stack4;
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							int num9 = WorldGen.genRand.Next(2);
							int stack5 = WorldGen.genRand.Next(11) + 10;
							if (num9 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(8);
							}
							if (num9 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(31);
							}
							Main.chest[num2].item[num3].stack = stack5;
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							Main.chest[num2].item[num3].SetDefaults(72);
							Main.chest[num2].item[num3].stack = WorldGen.genRand.Next(10, 30);
							num3++;
						}
						continue;
					}
					if ((double)num < Main.rockLayer)
					{
						if (contain > 0)
						{
							Main.chest[num2].item[num3].SetDefaults(contain);
							Main.chest[num2].item[num3].Prefix(-1);
							num3++;
						}
						else
						{
							int num10 = WorldGen.genRand.Next(7);
							if (num10 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(49);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							if (num10 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(50);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							if (num10 == 2)
							{
								Main.chest[num2].item[num3].SetDefaults(52);
							}
							if (num10 == 3)
							{
								Main.chest[num2].item[num3].SetDefaults(53);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							if (num10 == 4)
							{
								Main.chest[num2].item[num3].SetDefaults(54);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							if (num10 == 5)
							{
								Main.chest[num2].item[num3].SetDefaults(55);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							if (num10 == 6)
							{
								Main.chest[num2].item[num3].SetDefaults(51);
								Main.chest[num2].item[num3].stack = WorldGen.genRand.Next(26) + 25;
							}
							num3++;
						}
						if (WorldGen.genRand.Next(3) == 0)
						{
							Main.chest[num2].item[num3].SetDefaults(166);
							Main.chest[num2].item[num3].stack = WorldGen.genRand.Next(10, 20);
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							int num11 = WorldGen.genRand.Next(2);
							int stack6 = WorldGen.genRand.Next(10) + 5;
							if (num11 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(22);
							}
							if (num11 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(21);
							}
							Main.chest[num2].item[num3].stack = stack6;
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							int num12 = WorldGen.genRand.Next(2);
							int stack7 = WorldGen.genRand.Next(25) + 25;
							if (num12 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(40);
							}
							if (num12 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(42);
							}
							Main.chest[num2].item[num3].stack = stack7;
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							int num13 = WorldGen.genRand.Next(1);
							int stack8 = WorldGen.genRand.Next(3) + 3;
							if (num13 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(28);
							}
							Main.chest[num2].item[num3].stack = stack8;
							num3++;
						}
						if (WorldGen.genRand.Next(3) > 0)
						{
							int num14 = WorldGen.genRand.Next(7);
							int stack9 = WorldGen.genRand.Next(1, 3);
							if (num14 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(289);
							}
							if (num14 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(298);
							}
							if (num14 == 2)
							{
								Main.chest[num2].item[num3].SetDefaults(299);
							}
							if (num14 == 3)
							{
								Main.chest[num2].item[num3].SetDefaults(290);
							}
							if (num14 == 4)
							{
								Main.chest[num2].item[num3].SetDefaults(303);
							}
							if (num14 == 5)
							{
								Main.chest[num2].item[num3].SetDefaults(291);
							}
							if (num14 == 6)
							{
								Main.chest[num2].item[num3].SetDefaults(304);
							}
							Main.chest[num2].item[num3].stack = stack9;
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							int stack10 = WorldGen.genRand.Next(11) + 10;
							Main.chest[num2].item[num3].SetDefaults(8);
							Main.chest[num2].item[num3].stack = stack10;
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							Main.chest[num2].item[num3].SetDefaults(72);
							Main.chest[num2].item[num3].stack = WorldGen.genRand.Next(50, 90);
							num3++;
						}
						continue;
					}
					if (num < Main.maxTilesY - 250)
					{
						if (contain > 0)
						{
							Main.chest[num2].item[num3].SetDefaults(contain);
							Main.chest[num2].item[num3].Prefix(-1);
							num3++;
						}
						else
						{
							int num15 = WorldGen.genRand.Next(7);
							if (num15 == 2 && WorldGen.genRand.Next(2) == 0)
							{
								num15 = WorldGen.genRand.Next(7);
							}
							if (num15 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(49);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							if (num15 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(50);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							if (num15 == 2)
							{
								Main.chest[num2].item[num3].SetDefaults(52);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							if (num15 == 3)
							{
								Main.chest[num2].item[num3].SetDefaults(53);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							if (num15 == 4)
							{
								Main.chest[num2].item[num3].SetDefaults(54);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							if (num15 == 5)
							{
								Main.chest[num2].item[num3].SetDefaults(55);
								Main.chest[num2].item[num3].Prefix(-1);
							}
							if (num15 == 6)
							{
								Main.chest[num2].item[num3].SetDefaults(51);
								Main.chest[num2].item[num3].stack = WorldGen.genRand.Next(26) + 25;
							}
							num3++;
						}
						if (WorldGen.genRand.Next(5) == 0)
						{
							Main.chest[num2].item[num3].SetDefaults(43);
							num3++;
						}
						if (WorldGen.genRand.Next(3) == 0)
						{
							Main.chest[num2].item[num3].SetDefaults(167);
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							int num16 = WorldGen.genRand.Next(2);
							int stack11 = WorldGen.genRand.Next(8) + 3;
							if (num16 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(19);
							}
							if (num16 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(21);
							}
							Main.chest[num2].item[num3].stack = stack11;
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							int num17 = WorldGen.genRand.Next(2);
							int stack12 = WorldGen.genRand.Next(26) + 25;
							if (num17 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(41);
							}
							if (num17 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(279);
							}
							Main.chest[num2].item[num3].stack = stack12;
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							int num18 = WorldGen.genRand.Next(1);
							int stack13 = WorldGen.genRand.Next(3) + 3;
							if (num18 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(188);
							}
							Main.chest[num2].item[num3].stack = stack13;
							num3++;
						}
						if (WorldGen.genRand.Next(3) > 0)
						{
							int num19 = WorldGen.genRand.Next(6);
							int stack14 = WorldGen.genRand.Next(1, 3);
							if (num19 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(296);
							}
							if (num19 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(295);
							}
							if (num19 == 2)
							{
								Main.chest[num2].item[num3].SetDefaults(299);
							}
							if (num19 == 3)
							{
								Main.chest[num2].item[num3].SetDefaults(302);
							}
							if (num19 == 4)
							{
								Main.chest[num2].item[num3].SetDefaults(303);
							}
							if (num19 == 5)
							{
								Main.chest[num2].item[num3].SetDefaults(305);
							}
							Main.chest[num2].item[num3].stack = stack14;
							num3++;
						}
						if (WorldGen.genRand.Next(3) > 1)
						{
							int num20 = WorldGen.genRand.Next(4);
							int stack15 = WorldGen.genRand.Next(1, 3);
							if (num20 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(301);
							}
							if (num20 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(302);
							}
							if (num20 == 2)
							{
								Main.chest[num2].item[num3].SetDefaults(297);
							}
							if (num20 == 3)
							{
								Main.chest[num2].item[num3].SetDefaults(304);
							}
							Main.chest[num2].item[num3].stack = stack15;
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							int num21 = WorldGen.genRand.Next(2);
							int stack16 = WorldGen.genRand.Next(15) + 15;
							if (num21 == 0)
							{
								Main.chest[num2].item[num3].SetDefaults(8);
							}
							if (num21 == 1)
							{
								Main.chest[num2].item[num3].SetDefaults(282);
							}
							Main.chest[num2].item[num3].stack = stack16;
							num3++;
						}
						if (WorldGen.genRand.Next(2) == 0)
						{
							Main.chest[num2].item[num3].SetDefaults(73);
							Main.chest[num2].item[num3].stack = WorldGen.genRand.Next(1, 3);
							num3++;
						}
						continue;
					}
					if (contain > 0)
					{
						Main.chest[num2].item[num3].SetDefaults(contain);
						Main.chest[num2].item[num3].Prefix(-1);
						num3++;
					}
					else
					{
						int num22 = WorldGen.genRand.Next(4);
						if (num22 == 0)
						{
							Main.chest[num2].item[num3].SetDefaults(49);
							Main.chest[num2].item[num3].Prefix(-1);
						}
						if (num22 == 1)
						{
							Main.chest[num2].item[num3].SetDefaults(50);
							Main.chest[num2].item[num3].Prefix(-1);
						}
						if (num22 == 2)
						{
							Main.chest[num2].item[num3].SetDefaults(53);
							Main.chest[num2].item[num3].Prefix(-1);
						}
						if (num22 == 3)
						{
							Main.chest[num2].item[num3].SetDefaults(54);
							Main.chest[num2].item[num3].Prefix(-1);
						}
						num3++;
					}
					if (WorldGen.genRand.Next(3) == 0)
					{
						Main.chest[num2].item[num3].SetDefaults(167);
						num3++;
					}
					if (WorldGen.genRand.Next(2) == 0)
					{
						int num23 = WorldGen.genRand.Next(2);
						int stack17 = WorldGen.genRand.Next(15) + 15;
						if (num23 == 0)
						{
							Main.chest[num2].item[num3].SetDefaults(117);
						}
						if (num23 == 1)
						{
							Main.chest[num2].item[num3].SetDefaults(19);
						}
						Main.chest[num2].item[num3].stack = stack17;
						num3++;
					}
					if (WorldGen.genRand.Next(2) == 0)
					{
						int num24 = WorldGen.genRand.Next(2);
						int stack18 = WorldGen.genRand.Next(25) + 50;
						if (num24 == 0)
						{
							Main.chest[num2].item[num3].SetDefaults(265);
						}
						if (num24 == 1)
						{
							Main.chest[num2].item[num3].SetDefaults(278);
						}
						Main.chest[num2].item[num3].stack = stack18;
						num3++;
					}
					if (WorldGen.genRand.Next(2) == 0)
					{
						int num25 = WorldGen.genRand.Next(2);
						int stack19 = WorldGen.genRand.Next(15) + 15;
						if (num25 == 0)
						{
							Main.chest[num2].item[num3].SetDefaults(226);
						}
						if (num25 == 1)
						{
							Main.chest[num2].item[num3].SetDefaults(227);
						}
						Main.chest[num2].item[num3].stack = stack19;
						num3++;
					}
					if (WorldGen.genRand.Next(4) > 0)
					{
						int num26 = WorldGen.genRand.Next(7);
						int stack20 = WorldGen.genRand.Next(1, 3);
						if (num26 == 0)
						{
							Main.chest[num2].item[num3].SetDefaults(296);
						}
						if (num26 == 1)
						{
							Main.chest[num2].item[num3].SetDefaults(295);
						}
						if (num26 == 2)
						{
							Main.chest[num2].item[num3].SetDefaults(293);
						}
						if (num26 == 3)
						{
							Main.chest[num2].item[num3].SetDefaults(288);
						}
						if (num26 == 4)
						{
							Main.chest[num2].item[num3].SetDefaults(294);
						}
						if (num26 == 5)
						{
							Main.chest[num2].item[num3].SetDefaults(297);
						}
						if (num26 == 6)
						{
							Main.chest[num2].item[num3].SetDefaults(304);
						}
						Main.chest[num2].item[num3].stack = stack20;
						num3++;
					}
					if (WorldGen.genRand.Next(3) > 0)
					{
						int num27 = WorldGen.genRand.Next(5);
						int stack21 = WorldGen.genRand.Next(1, 3);
						if (num27 == 0)
						{
							Main.chest[num2].item[num3].SetDefaults(305);
						}
						if (num27 == 1)
						{
							Main.chest[num2].item[num3].SetDefaults(301);
						}
						if (num27 == 2)
						{
							Main.chest[num2].item[num3].SetDefaults(302);
						}
						if (num27 == 3)
						{
							Main.chest[num2].item[num3].SetDefaults(288);
						}
						if (num27 == 4)
						{
							Main.chest[num2].item[num3].SetDefaults(300);
						}
						Main.chest[num2].item[num3].stack = stack21;
						num3++;
					}
					if (WorldGen.genRand.Next(2) == 0)
					{
						int num28 = WorldGen.genRand.Next(2);
						int stack22 = WorldGen.genRand.Next(15) + 15;
						if (num28 == 0)
						{
							Main.chest[num2].item[num3].SetDefaults(8);
						}
						if (num28 == 1)
						{
							Main.chest[num2].item[num3].SetDefaults(282);
						}
						Main.chest[num2].item[num3].stack = stack22;
						num3++;
					}
					if (WorldGen.genRand.Next(2) == 0)
					{
						Main.chest[num2].item[num3].SetDefaults(73);
						Main.chest[num2].item[num3].stack = WorldGen.genRand.Next(2, 5);
						num3++;
					}
				}
				return true;
			}
			return false;
		}
		return false;
	}

	public static void Place1xX(int x, int y, int type, int style = 0)
	{
		int num = style * 18;
		int num2 = 3;
		if (type == 92)
		{
			num2 = 6;
		}
		bool flag = true;
		for (int i = y - num2 + 1; i < y + 1; i++)
		{
			if (Main.tile[x, i] == null)
			{
				Main.tile[x, i] = default(Tile);
			}
			if (Main.tile[x, i].HasTile)
			{
				flag = false;
			}
			if (type == 93 && Main.tile[x, i].liquid > 0)
			{
				flag = false;
			}
		}
		if (flag && Main.tile[x, y + 1].HasTile && Main.tileSolid[Main.tile[x, y + 1].type])
		{
			for (int j = 0; j < num2; j++)
			{
				Main.tile[x, y - num2 + 1 + j].active(active: true);
				Main.tile[x, y - num2 + 1 + j].frameY = (short)(j * 18);
				Main.tile[x, y - num2 + 1 + j].frameX = (short)num;
				Main.tile[x, y - num2 + 1 + j].type = (byte)type;
			}
		}
	}

	public static void Place2xX(int x, int y, int type, int style = 0)
	{
		int num = style * 36;
		int num2 = 3;
		if (type == 104)
		{
			num2 = 5;
		}
		bool flag = true;
		for (int i = y - num2 + 1; i < y + 1; i++)
		{
			if (Main.tile[x, i] == null)
			{
				Main.tile[x, i] = default(Tile);
			}
			if (Main.tile[x, i].HasTile)
			{
				flag = false;
			}
			if (Main.tile[x + 1, i] == null)
			{
				Main.tile[x + 1, i] = default(Tile);
			}
			if (Main.tile[x + 1, i].HasTile)
			{
				flag = false;
			}
		}
		if (flag && Main.tile[x, y + 1].HasTile && Main.tileSolid[Main.tile[x, y + 1].type] && Main.tile[x + 1, y + 1].HasTile && Main.tileSolid[Main.tile[x + 1, y + 1].type])
		{
			for (int j = 0; j < num2; j++)
			{
				Main.tile[x, y - num2 + 1 + j].active(active: true);
				Main.tile[x, y - num2 + 1 + j].frameY = (short)(j * 18);
				Main.tile[x, y - num2 + 1 + j].frameX = (short)num;
				Main.tile[x, y - num2 + 1 + j].type = (byte)type;
				Main.tile[x + 1, y - num2 + 1 + j].active(active: true);
				Main.tile[x + 1, y - num2 + 1 + j].frameY = (short)(j * 18);
				Main.tile[x + 1, y - num2 + 1 + j].frameX = (short)(num + 18);
				Main.tile[x + 1, y - num2 + 1 + j].type = (byte)type;
			}
		}
	}

	public static bool PlaceSign(int x, int y, int type)
	{
		int num = x - 2;
		int num2 = x + 3;
		int num3 = y - 2;
		int num4 = y + 3;
		if (num < 0)
		{
			return false;
		}
		if (num2 > Main.maxTilesX)
		{
			return false;
		}
		if (num3 < 0)
		{
			return false;
		}
		if (num4 > Main.maxTilesY)
		{
			return false;
		}
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				if (Main.tile[i, j] == null)
				{
					Main.tile[i, j] = default(Tile);
				}
			}
		}
		int num5 = x;
		int num6 = y;
		int num7 = 0;
		switch (type)
		{
		case 55:
			if (Main.tile[x, y + 1].HasTile && Main.tileSolid[Main.tile[x, y + 1].type] && Main.tile[x + 1, y + 1].HasTile && Main.tileSolid[Main.tile[x + 1, y + 1].type])
			{
				num6--;
				num7 = 0;
				break;
			}
			if (Main.tile[x, y - 1].HasTile && Main.tileSolid[Main.tile[x, y - 1].type] && !Main.tileSolidTop[Main.tile[x, y - 1].type] && Main.tile[x + 1, y - 1].HasTile && Main.tileSolid[Main.tile[x + 1, y - 1].type] && !Main.tileSolidTop[Main.tile[x + 1, y - 1].type])
			{
				num7 = 1;
				break;
			}
			if (Main.tile[x - 1, y].HasTile && Main.tileSolid[Main.tile[x - 1, y].type] && !Main.tileSolidTop[Main.tile[x - 1, y].type] && !Main.tileNoAttach[Main.tile[x - 1, y].type] && Main.tile[x - 1, y + 1].HasTile && Main.tileSolid[Main.tile[x - 1, y + 1].type] && !Main.tileSolidTop[Main.tile[x - 1, y + 1].type] && !Main.tileNoAttach[Main.tile[x - 1, y + 1].type])
			{
				num7 = 2;
				break;
			}
			if (Main.tile[x + 1, y].HasTile && Main.tileSolid[Main.tile[x + 1, y].type] && !Main.tileSolidTop[Main.tile[x + 1, y].type] && !Main.tileNoAttach[Main.tile[x + 1, y].type] && Main.tile[x + 1, y + 1].HasTile && Main.tileSolid[Main.tile[x + 1, y + 1].type] && !Main.tileSolidTop[Main.tile[x + 1, y + 1].type] && !Main.tileNoAttach[Main.tile[x + 1, y + 1].type])
			{
				num5--;
				num7 = 3;
				break;
			}
			return false;
		case 85:
			if (Main.tile[x, y + 1].HasTile && Main.tileSolid[Main.tile[x, y + 1].type] && Main.tile[x + 1, y + 1].HasTile && Main.tileSolid[Main.tile[x + 1, y + 1].type])
			{
				num6--;
				num7 = 0;
				break;
			}
			return false;
		}
		if (Main.tile[num5, num6].HasTile || Main.tile[num5 + 1, num6].HasTile || Main.tile[num5, num6 + 1].HasTile || Main.tile[num5 + 1, num6 + 1].HasTile)
		{
			return false;
		}
		int num8 = 36 * num7;
		for (int k = 0; k < 2; k++)
		{
			for (int l = 0; l < 2; l++)
			{
				Main.tile[num5 + k, num6 + l].active(active: true);
				Main.tile[num5 + k, num6 + l].type = (byte)type;
				Main.tile[num5 + k, num6 + l].frameX = (short)(num8 + 18 * k);
				Main.tile[num5 + k, num6 + l].frameY = (short)(18 * l);
			}
		}
		return true;
	}

	public static void Place1x1(int x, int y, int type, int style = 0)
	{
		if (Main.tile[x, y] == null)
		{
			Main.tile[x, y] = default(Tile);
		}
		if (Main.tile[x, y + 1] == null)
		{
			Main.tile[x, y + 1] = default(Tile);
		}
		if (SolidTile(x, y + 1) && !Main.tile[x, y].HasTile)
		{
			Main.tile[x, y].active(active: true);
			Main.tile[x, y].type = (byte)type;
			if (type == 144)
			{
				Main.tile[x, y].frameX = (short)(style * 18);
				Main.tile[x, y].frameY = 0;
			}
			else
			{
				Main.tile[x, y].frameY = (short)(style * 18);
			}
		}
	}

	public static void PlaceOnTable1x1(int x, int y, int type, int style = 0)
	{
		bool flag = false;
		if (Main.tile[x, y] == null)
		{
			Main.tile[x, y] = default(Tile);
		}
		if (Main.tile[x, y + 1] == null)
		{
			Main.tile[x, y + 1] = default(Tile);
		}
		if (!Main.tile[x, y].HasTile && Main.tile[x, y + 1].HasTile && Main.tileTable[Main.tile[x, y + 1].type])
		{
			flag = true;
		}
		if (type == 78 && !Main.tile[x, y].HasTile && Main.tile[x, y + 1].HasTile && Main.tileSolid[Main.tile[x, y + 1].type])
		{
			flag = true;
		}
		if (flag)
		{
			Main.tile[x, y].active(active: true);
			Main.tile[x, y].frameX = (short)(style * 18);
			Main.tile[x, y].frameY = 0;
			Main.tile[x, y].type = (byte)type;
			if (type == 50)
			{
				Main.tile[x, y].frameX = (short)(18 * WorldGen.genRand.Next(5));
			}
		}
	}

	public static bool PlaceAlch(int x, int y, int style)
	{
		if (Main.tile[x, y] == null)
		{
			Main.tile[x, y] = default(Tile);
		}
		if (Main.tile[x, y + 1] == null)
		{
			Main.tile[x, y + 1] = default(Tile);
		}
		if (!Main.tile[x, y].HasTile && Main.tile[x, y + 1].HasTile)
		{
			bool flag = false;
			switch (style)
			{
			case 0:
				if (Main.tile[x, y + 1].type != 2 && Main.tile[x, y + 1].type != 78 && Main.tile[x, y + 1].type != 109)
				{
					flag = true;
				}
				if (Main.tile[x, y].liquid > 0)
				{
					flag = true;
				}
				break;
			case 1:
				if (Main.tile[x, y + 1].type != 60 && Main.tile[x, y + 1].type != 78)
				{
					flag = true;
				}
				if (Main.tile[x, y].liquid > 0)
				{
					flag = true;
				}
				break;
			case 2:
				if (Main.tile[x, y + 1].type != 0 && Main.tile[x, y + 1].type != 59 && Main.tile[x, y + 1].type != 78)
				{
					flag = true;
				}
				if (Main.tile[x, y].liquid > 0)
				{
					flag = true;
				}
				break;
			case 3:
				if (Main.tile[x, y + 1].type != 23 && Main.tile[x, y + 1].type != 25 && Main.tile[x, y + 1].type != 78)
				{
					flag = true;
				}
				if (Main.tile[x, y].liquid > 0)
				{
					flag = true;
				}
				break;
			case 4:
				if (Main.tile[x, y + 1].type != 53 && Main.tile[x, y + 1].type != 78 && Main.tile[x, y + 1].type != 116)
				{
					flag = true;
				}
				if (Main.tile[x, y].liquid > 0 && Main.tile[x, y].lava())
				{
					flag = true;
				}
				break;
			case 5:
				if (Main.tile[x, y + 1].type != 57 && Main.tile[x, y + 1].type != 78)
				{
					flag = true;
				}
				if (Main.tile[x, y].liquid > 0 && !Main.tile[x, y].lava())
				{
					flag = true;
				}
				break;
			}
			if (!flag)
			{
				Main.tile[x, y].active(active: true);
				Main.tile[x, y].type = 82;
				Main.tile[x, y].frameX = (short)(18 * style);
				Main.tile[x, y].frameY = 0;
				return true;
			}
		}
		return false;
	}

	public static void PlantAlch()
	{
		int num = WorldGen.genRand.Next(20, Main.maxTilesX - 20);
		int num2 = 0;
		for (num2 = ((WorldGen.genRand.Next(40) == 0) ? WorldGen.genRand.Next((int)(Main.rockLayer + (double)Main.maxTilesY) / 2, Main.maxTilesY - 20) : ((WorldGen.genRand.Next(10) != 0) ? WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 20) : WorldGen.genRand.Next(0, Main.maxTilesY - 20))); num2 < Main.maxTilesY - 20 && !Main.tile[num, num2].HasTile; num2++)
		{
		}
		if (Main.tile[num, num2].HasTile && !Main.tile[num, num2 - 1].HasTile && Main.tile[num, num2 - 1].liquid == 0)
		{
			if (Main.tile[num, num2].type == 2 || Main.tile[num, num2].type == 109)
			{
				PlaceAlch(num, num2 - 1, 0);
			}
			if (Main.tile[num, num2].type == 60)
			{
				PlaceAlch(num, num2 - 1, 1);
			}
			if (Main.tile[num, num2].type == 0 || Main.tile[num, num2].type == 59)
			{
				PlaceAlch(num, num2 - 1, 2);
			}
			if (Main.tile[num, num2].type == 23 || Main.tile[num, num2].type == 25)
			{
				PlaceAlch(num, num2 - 1, 3);
			}
			if (Main.tile[num, num2].type == 53 || Main.tile[num, num2].type == 116)
			{
				PlaceAlch(num, num2 - 1, 4);
			}
			if (Main.tile[num, num2].type == 57)
			{
				PlaceAlch(num, num2 - 1, 5);
			}
			if (Main.tile[num, num2 - 1].HasTile && Main.netMode == 2)
			{
				NetMessage.SendTileSquare(-1, num, num2 - 1, 1);
			}
		}
	}

	public static void PlaceBanner(int x, int y, int type, int style = 0)
	{
		int num = style * 18;
		if (Main.tile[x, y - 1] == null)
		{
			Main.tile[x, y - 1] = default(Tile);
		}
		if (Main.tile[x, y] == null)
		{
			Main.tile[x, y] = default(Tile);
		}
		if (Main.tile[x, y + 1] == null)
		{
			Main.tile[x, y + 1] = default(Tile);
		}
		if (Main.tile[x, y + 2] == null)
		{
			Main.tile[x, y + 2] = default(Tile);
		}
		if (Main.tile[x, y - 1].HasTile && Main.tileSolid[Main.tile[x, y - 1].type] && !Main.tileSolidTop[Main.tile[x, y - 1].type] && !Main.tile[x, y].HasTile && !Main.tile[x, y + 1].HasTile && !Main.tile[x, y + 2].HasTile)
		{
			Main.tile[x, y].active(active: true);
			Main.tile[x, y].frameY = 0;
			Main.tile[x, y].frameX = (short)num;
			Main.tile[x, y].type = (byte)type;
			Main.tile[x, y + 1].active(active: true);
			Main.tile[x, y + 1].frameY = 18;
			Main.tile[x, y + 1].frameX = (short)num;
			Main.tile[x, y + 1].type = (byte)type;
			Main.tile[x, y + 2].active(active: true);
			Main.tile[x, y + 2].frameY = 36;
			Main.tile[x, y + 2].frameX = (short)num;
			Main.tile[x, y + 2].type = (byte)type;
		}
	}

	public static void PlaceMan(int i, int j, int dir)
	{
		for (int k = i; k <= i + 1; k++)
		{
			for (int l = j - 2; l <= j; l++)
			{
				if (Main.tile[k, l].HasTile)
				{
					return;
				}
			}
		}
		if (SolidTile(i, j + 1) && SolidTile(i + 1, j + 1))
		{
			byte b = 0;
			if (dir == 1)
			{
				b = 36;
			}
			Main.tile[i, j - 2].active(active: true);
			Main.tile[i, j - 2].frameY = 0;
			Main.tile[i, j - 2].frameX = b;
			Main.tile[i, j - 2].type = 128;
			Main.tile[i, j - 1].active(active: true);
			Main.tile[i, j - 1].frameY = 18;
			Main.tile[i, j - 1].frameX = b;
			Main.tile[i, j - 1].type = 128;
			Main.tile[i, j].active(active: true);
			Main.tile[i, j].frameY = 36;
			Main.tile[i, j].frameX = b;
			Main.tile[i, j].type = 128;
			Main.tile[i + 1, j - 2].active(active: true);
			Main.tile[i + 1, j - 2].frameY = 0;
			Main.tile[i + 1, j - 2].frameX = (byte)(18 + b);
			Main.tile[i + 1, j - 2].type = 128;
			Main.tile[i + 1, j - 1].active(active: true);
			Main.tile[i + 1, j - 1].frameY = 18;
			Main.tile[i + 1, j - 1].frameX = (byte)(18 + b);
			Main.tile[i + 1, j - 1].type = 128;
			Main.tile[i + 1, j].active(active: true);
			Main.tile[i + 1, j].frameY = 36;
			Main.tile[i + 1, j].frameX = (byte)(18 + b);
			Main.tile[i + 1, j].type = 128;
		}
	}

	public static void Place1x2(int x, int y, int type, int style)
	{
		short frameX = 0;
		if (type == 20)
		{
			frameX = (short)(WorldGen.genRand.Next(3) * 18);
		}
		if (Main.tile[x, y - 1] == null)
		{
			Main.tile[x, y - 1] = default(Tile);
		}
		if (Main.tile[x, y + 1] == null)
		{
			Main.tile[x, y + 1] = default(Tile);
		}
		if (Main.tile[x, y + 1].HasTile && Main.tileSolid[Main.tile[x, y + 1].type] && !Main.tile[x, y - 1].HasTile)
		{
			short num = (short)(style * 40);
			Main.tile[x, y - 1].active(active: true);
			Main.tile[x, y - 1].frameY = num;
			Main.tile[x, y - 1].frameX = frameX;
			Main.tile[x, y - 1].type = (byte)type;
			Main.tile[x, y].active(active: true);
			Main.tile[x, y].frameY = (short)(num + 18);
			Main.tile[x, y].frameX = frameX;
			Main.tile[x, y].type = (byte)type;
		}
	}

	public static void Place1x2Top(int x, int y, int type)
	{
		short frameX = 0;
		if (Main.tile[x, y - 1] == null)
		{
			Main.tile[x, y - 1] = default(Tile);
		}
		if (Main.tile[x, y + 1] == null)
		{
			Main.tile[x, y + 1] = default(Tile);
		}
		if (Main.tile[x, y - 1].HasTile && Main.tileSolid[Main.tile[x, y - 1].type] && !Main.tileSolidTop[Main.tile[x, y - 1].type] && !Main.tile[x, y + 1].HasTile)
		{
			Main.tile[x, y].active(active: true);
			Main.tile[x, y].frameY = 0;
			Main.tile[x, y].frameX = frameX;
			Main.tile[x, y].type = (byte)type;
			Main.tile[x, y + 1].active(active: true);
			Main.tile[x, y + 1].frameY = 18;
			Main.tile[x, y + 1].frameX = frameX;
			Main.tile[x, y + 1].type = (byte)type;
		}
	}

	public static void Place2x1(int x, int y, int type)
	{
		if (Main.tile[x, y] == null)
		{
			Main.tile[x, y] = default(Tile);
		}
		if (Main.tile[x + 1, y] == null)
		{
			Main.tile[x + 1, y] = default(Tile);
		}
		if (Main.tile[x, y + 1] == null)
		{
			Main.tile[x, y + 1] = default(Tile);
		}
		if (Main.tile[x + 1, y + 1] == null)
		{
			Main.tile[x + 1, y + 1] = default(Tile);
		}
		bool flag = false;
		if (type != 29 && type != 103 && Main.tile[x, y + 1].HasTile && Main.tile[x + 1, y + 1].HasTile && Main.tileSolid[Main.tile[x, y + 1].type] && Main.tileSolid[Main.tile[x + 1, y + 1].type] && !Main.tile[x, y].HasTile && !Main.tile[x + 1, y].HasTile)
		{
			flag = true;
		}
		else if ((type == 29 || type == 103) && Main.tile[x, y + 1].HasTile && Main.tile[x + 1, y + 1].HasTile && Main.tileTable[Main.tile[x, y + 1].type] && Main.tileTable[Main.tile[x + 1, y + 1].type] && !Main.tile[x, y].HasTile && !Main.tile[x + 1, y].HasTile)
		{
			flag = true;
		}
		if (flag)
		{
			Main.tile[x, y].active(active: true);
			Main.tile[x, y].frameY = 0;
			Main.tile[x, y].frameX = 0;
			Main.tile[x, y].type = (byte)type;
			Main.tile[x + 1, y].active(active: true);
			Main.tile[x + 1, y].frameY = 0;
			Main.tile[x + 1, y].frameX = 18;
			Main.tile[x + 1, y].type = (byte)type;
		}
	}

	public static void OreRunner(int i, int j, double strength, int steps, int type)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		double num = strength;
		float num2 = steps;
		Vector2 vector = new Vector2
		{
			X = i,
			Y = j
		};
		Vector2 vector2 = new Vector2
		{
			X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f,
			Y = (float)WorldGen.genRand.Next(-10, 11) * 0.1f
		};
		while (num > 0.0 && num2 > 0f)
		{
			if (vector.Y < 0f && num2 > 0f && type == 59)
			{
				num2 = 0f;
			}
			num = strength * (double)(num2 / (float)steps);
			num2--;
			int num3 = (int)((double)vector.X - num * 0.5);
			int num4 = (int)((double)vector.X + num * 0.5);
			int num5 = (int)((double)vector.Y - num * 0.5);
			int num6 = (int)((double)vector.Y + num * 0.5);
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num4 > Main.maxTilesX)
			{
				num4 = Main.maxTilesX;
			}
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.maxTilesY)
			{
				num6 = Main.maxTilesY;
			}
			for (int k = num3; k < num4; k++)
			{
				for (int l = num5; l < num6; l++)
				{
					if ((double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < strength * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015) && Main.tile[k, l].HasTile && (Main.tile[k, l].type == 0 || Main.tile[k, l].type == 1 || Main.tile[k, l].type == 23 || Main.tile[k, l].type == 25 || Main.tile[k, l].type == 40 || Main.tile[k, l].type == 53 || Main.tile[k, l].type == 57 || Main.tile[k, l].type == 59 || Main.tile[k, l].type == 60 || Main.tile[k, l].type == 70 || Main.tile[k, l].type == 109 || Main.tile[k, l].type == 112 || Main.tile[k, l].type == 116 || Main.tile[k, l].type == 117))
					{
						Main.tile[k, l].type = (byte)type;
						SquareTileFrame(k, l);
						if (Main.netMode == 2)
						{
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
					}
				}
			}
			vector += vector2;
			vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			if (vector2.X > 1f)
			{
				vector2.X = 1f;
			}
			if (vector2.X < -1f)
			{
				vector2.X = -1f;
			}
		}
	}

	public static void SmashAltar(int i, int j)
	{
	}

	public static void Place4x2(int x, int y, int type, int direction = -1)
	{
		if (x < 5 || x > Main.maxTilesX - 5 || y < 5 || y > Main.maxTilesY - 5)
		{
			return;
		}
		bool flag = true;
		for (int i = x - 1; i < x + 3; i++)
		{
			for (int j = y - 1; j < y + 1; j++)
			{
				if (Main.tile[i, j] == null)
				{
					Main.tile[i, j] = default(Tile);
				}
				if (Main.tile[i, j].HasTile)
				{
					flag = false;
				}
			}
			if (Main.tile[i, y + 1] == null)
			{
				Main.tile[i, y + 1] = default(Tile);
			}
			if (!Main.tile[i, y + 1].HasTile || !Main.tileSolid[Main.tile[i, y + 1].type])
			{
				flag = false;
			}
		}
		short num = 0;
		if (direction == 1)
		{
			num = 72;
		}
		if (flag)
		{
			Main.tile[x - 1, y - 1].active(active: true);
			Main.tile[x - 1, y - 1].frameY = 0;
			Main.tile[x - 1, y - 1].frameX = num;
			Main.tile[x - 1, y - 1].type = (byte)type;
			Main.tile[x, y - 1].active(active: true);
			Main.tile[x, y - 1].frameY = 0;
			Main.tile[x, y - 1].frameX = (short)(18 + num);
			Main.tile[x, y - 1].type = (byte)type;
			Main.tile[x + 1, y - 1].active(active: true);
			Main.tile[x + 1, y - 1].frameY = 0;
			Main.tile[x + 1, y - 1].frameX = (short)(36 + num);
			Main.tile[x + 1, y - 1].type = (byte)type;
			Main.tile[x + 2, y - 1].active(active: true);
			Main.tile[x + 2, y - 1].frameY = 0;
			Main.tile[x + 2, y - 1].frameX = (short)(54 + num);
			Main.tile[x + 2, y - 1].type = (byte)type;
			Main.tile[x - 1, y].active(active: true);
			Main.tile[x - 1, y].frameY = 18;
			Main.tile[x - 1, y].frameX = num;
			Main.tile[x - 1, y].type = (byte)type;
			Main.tile[x, y].active(active: true);
			Main.tile[x, y].frameY = 18;
			Main.tile[x, y].frameX = (short)(18 + num);
			Main.tile[x, y].type = (byte)type;
			Main.tile[x + 1, y].active(active: true);
			Main.tile[x + 1, y].frameY = 18;
			Main.tile[x + 1, y].frameX = (short)(36 + num);
			Main.tile[x + 1, y].type = (byte)type;
			Main.tile[x + 2, y].active(active: true);
			Main.tile[x + 2, y].frameY = 18;
			Main.tile[x + 2, y].frameX = (short)(54 + num);
			Main.tile[x + 2, y].type = (byte)type;
		}
	}

	public static void PlaceMB(int X, int y, int type, int style)
	{
		int num = X + 1;
		if (num < 5 || num > Main.maxTilesX - 5 || y < 5 || y > Main.maxTilesY - 5)
		{
			return;
		}
		bool flag = true;
		for (int i = num - 1; i < num + 1; i++)
		{
			for (int j = y - 1; j < y + 1; j++)
			{
				if (Main.tile[i, j] == null)
				{
					Main.tile[i, j] = default(Tile);
				}
				if (Main.tile[i, j].HasTile)
				{
					flag = false;
				}
			}
			if (Main.tile[i, y + 1] == null)
			{
				Main.tile[i, y + 1] = default(Tile);
			}
			if (!Main.tile[i, y + 1].HasTile || (!Main.tileSolid[Main.tile[i, y + 1].type] && !Main.tileTable[Main.tile[i, y + 1].type]))
			{
				flag = false;
			}
		}
		if (flag)
		{
			Main.tile[num - 1, y - 1].active(active: true);
			Main.tile[num - 1, y - 1].frameY = (short)(style * 36);
			Main.tile[num - 1, y - 1].frameX = 0;
			Main.tile[num - 1, y - 1].type = (byte)type;
			Main.tile[num, y - 1].active(active: true);
			Main.tile[num, y - 1].frameY = (short)(style * 36);
			Main.tile[num, y - 1].frameX = 18;
			Main.tile[num, y - 1].type = (byte)type;
			Main.tile[num - 1, y].active(active: true);
			Main.tile[num - 1, y].frameY = (short)(style * 36 + 18);
			Main.tile[num - 1, y].frameX = 0;
			Main.tile[num - 1, y].type = (byte)type;
			Main.tile[num, y].active(active: true);
			Main.tile[num, y].frameY = (short)(style * 36 + 18);
			Main.tile[num, y].frameX = 18;
			Main.tile[num, y].type = (byte)type;
		}
	}

	public static void Place2x2(int x, int superY, int type)
	{
		int num = superY;
		if (type == 95 || type == 126)
		{
			num++;
		}
		if (x < 5 || x > Main.maxTilesX - 5 || num < 5 || num > Main.maxTilesY - 5)
		{
			return;
		}
		bool flag = true;
		for (int i = x - 1; i < x + 1; i++)
		{
			for (int j = num - 1; j < num + 1; j++)
			{
				if (Main.tile[i, j] == null)
				{
					Main.tile[i, j] = default(Tile);
				}
				if (Main.tile[i, j].HasTile)
				{
					flag = false;
				}
				if (type == 98 && Main.tile[i, j].liquid > 0)
				{
					flag = false;
				}
			}
			if (type == 95 || type == 126)
			{
				if (Main.tile[i, num - 2] == null)
				{
					Main.tile[i, num - 2] = default(Tile);
				}
				if (!Main.tile[i, num - 2].HasTile || !Main.tileSolid[Main.tile[i, num - 2].type] || Main.tileSolidTop[Main.tile[i, num - 2].type])
				{
					flag = false;
				}
			}
			else
			{
				if (Main.tile[i, num + 1] == null)
				{
					Main.tile[i, num + 1] = default(Tile);
				}
				if (!Main.tile[i, num + 1].HasTile || (!Main.tileSolid[Main.tile[i, num + 1].type] && !Main.tileTable[Main.tile[i, num + 1].type]))
				{
					flag = false;
				}
			}
		}
		if (flag)
		{
			Main.tile[x - 1, num - 1].active(active: true);
			Main.tile[x - 1, num - 1].frameY = 0;
			Main.tile[x - 1, num - 1].frameX = 0;
			Main.tile[x - 1, num - 1].type = (byte)type;
			Main.tile[x, num - 1].active(active: true);
			Main.tile[x, num - 1].frameY = 0;
			Main.tile[x, num - 1].frameX = 18;
			Main.tile[x, num - 1].type = (byte)type;
			Main.tile[x - 1, num].active(active: true);
			Main.tile[x - 1, num].frameY = 18;
			Main.tile[x - 1, num].frameX = 0;
			Main.tile[x - 1, num].type = (byte)type;
			Main.tile[x, num].active(active: true);
			Main.tile[x, num].frameY = 18;
			Main.tile[x, num].frameX = 18;
			Main.tile[x, num].type = (byte)type;
		}
	}

	public static void Place3x4(int x, int y, int type)
	{
		if (x < 5 || x > Main.maxTilesX - 5 || y < 5 || y > Main.maxTilesY - 5)
		{
			return;
		}
		bool flag = true;
		for (int i = x - 1; i < x + 2; i++)
		{
			for (int j = y - 3; j < y + 1; j++)
			{
				if (Main.tile[i, j] == null)
				{
					Main.tile[i, j] = default(Tile);
				}
				if (Main.tile[i, j].HasTile)
				{
					flag = false;
				}
			}
			if (Main.tile[i, y + 1] == null)
			{
				Main.tile[i, y + 1] = default(Tile);
			}
			if (!Main.tile[i, y + 1].HasTile || !Main.tileSolid[Main.tile[i, y + 1].type])
			{
				flag = false;
			}
		}
		if (flag)
		{
			for (int k = -3; k <= 0; k++)
			{
				short frameY = (short)((3 + k) * 18);
				Main.tile[x - 1, y + k].active(active: true);
				Main.tile[x - 1, y + k].frameY = frameY;
				Main.tile[x - 1, y + k].frameX = 0;
				Main.tile[x - 1, y + k].type = (byte)type;
				Main.tile[x, y + k].active(active: true);
				Main.tile[x, y + k].frameY = frameY;
				Main.tile[x, y + k].frameX = 18;
				Main.tile[x, y + k].type = (byte)type;
				Main.tile[x + 1, y + k].active(active: true);
				Main.tile[x + 1, y + k].frameY = frameY;
				Main.tile[x + 1, y + k].frameX = 36;
				Main.tile[x + 1, y + k].type = (byte)type;
			}
		}
	}

	public static void Place3x2(int x, int y, int type)
	{
		if (x < 5 || x > Main.maxTilesX - 5 || y < 5 || y > Main.maxTilesY - 5)
		{
			return;
		}
		bool flag = true;
		for (int i = x - 1; i < x + 2; i++)
		{
			for (int j = y - 1; j < y + 1; j++)
			{
				if (Main.tile[i, j] == null)
				{
					Main.tile[i, j] = default(Tile);
				}
				if (Main.tile[i, j].HasTile)
				{
					flag = false;
				}
			}
			if (Main.tile[i, y + 1] == null)
			{
				Main.tile[i, y + 1] = default(Tile);
			}
			if (!Main.tile[i, y + 1].HasTile || !Main.tileSolid[Main.tile[i, y + 1].type])
			{
				flag = false;
			}
		}
		if (flag)
		{
			Main.tile[x - 1, y - 1].active(active: true);
			Main.tile[x - 1, y - 1].frameY = 0;
			Main.tile[x - 1, y - 1].frameX = 0;
			Main.tile[x - 1, y - 1].type = (byte)type;
			Main.tile[x, y - 1].active(active: true);
			Main.tile[x, y - 1].frameY = 0;
			Main.tile[x, y - 1].frameX = 18;
			Main.tile[x, y - 1].type = (byte)type;
			Main.tile[x + 1, y - 1].active(active: true);
			Main.tile[x + 1, y - 1].frameY = 0;
			Main.tile[x + 1, y - 1].frameX = 36;
			Main.tile[x + 1, y - 1].type = (byte)type;
			Main.tile[x - 1, y].active(active: true);
			Main.tile[x - 1, y].frameY = 18;
			Main.tile[x - 1, y].frameX = 0;
			Main.tile[x - 1, y].type = (byte)type;
			Main.tile[x, y].active(active: true);
			Main.tile[x, y].frameY = 18;
			Main.tile[x, y].frameX = 18;
			Main.tile[x, y].type = (byte)type;
			Main.tile[x + 1, y].active(active: true);
			Main.tile[x + 1, y].frameY = 18;
			Main.tile[x + 1, y].frameX = 36;
			Main.tile[x + 1, y].type = (byte)type;
		}
	}

	public static void Place3x3(int x, int y, int type)
	{
		bool flag = true;
		int num = 0;
		if (type == 106)
		{
			num = -2;
			for (int i = x - 1; i < x + 2; i++)
			{
				for (int j = y - 2; j < y + 1; j++)
				{
					if (Main.tile[i, j] == null)
					{
						Main.tile[i, j] = default(Tile);
					}
					if (Main.tile[i, j].HasTile)
					{
						flag = false;
					}
				}
			}
			for (int k = x - 1; k < x + 2; k++)
			{
				if (Main.tile[k, y + 1] == null)
				{
					Main.tile[k, y + 1] = default(Tile);
				}
				if (!Main.tile[k, y + 1].HasTile || !Main.tileSolid[Main.tile[k, y + 1].type])
				{
					flag = false;
					break;
				}
			}
		}
		else
		{
			for (int l = x - 1; l < x + 2; l++)
			{
				for (int m = y; m < y + 3; m++)
				{
					if (Main.tile[l, m] == null)
					{
						Main.tile[l, m] = default(Tile);
					}
					if (Main.tile[l, m].HasTile)
					{
						flag = false;
					}
				}
			}
			if (Main.tile[x, y - 1] == null)
			{
				Main.tile[x, y - 1] = default(Tile);
			}
			if (!Main.tile[x, y - 1].HasTile || !Main.tileSolid[Main.tile[x, y - 1].type] || Main.tileSolidTop[Main.tile[x, y - 1].type])
			{
				flag = false;
			}
		}
		if (flag)
		{
			Main.tile[x - 1, y + num].active(active: true);
			Main.tile[x - 1, y + num].frameY = 0;
			Main.tile[x - 1, y + num].frameX = 0;
			Main.tile[x - 1, y + num].type = (byte)type;
			Main.tile[x, y + num].active(active: true);
			Main.tile[x, y + num].frameY = 0;
			Main.tile[x, y + num].frameX = 18;
			Main.tile[x, y + num].type = (byte)type;
			Main.tile[x + 1, y + num].active(active: true);
			Main.tile[x + 1, y + num].frameY = 0;
			Main.tile[x + 1, y + num].frameX = 36;
			Main.tile[x + 1, y + num].type = (byte)type;
			Main.tile[x - 1, y + 1 + num].active(active: true);
			Main.tile[x - 1, y + 1 + num].frameY = 18;
			Main.tile[x - 1, y + 1 + num].frameX = 0;
			Main.tile[x - 1, y + 1 + num].type = (byte)type;
			Main.tile[x, y + 1 + num].active(active: true);
			Main.tile[x, y + 1 + num].frameY = 18;
			Main.tile[x, y + 1 + num].frameX = 18;
			Main.tile[x, y + 1 + num].type = (byte)type;
			Main.tile[x + 1, y + 1 + num].active(active: true);
			Main.tile[x + 1, y + 1 + num].frameY = 18;
			Main.tile[x + 1, y + 1 + num].frameX = 36;
			Main.tile[x + 1, y + 1 + num].type = (byte)type;
			Main.tile[x - 1, y + 2 + num].active(active: true);
			Main.tile[x - 1, y + 2 + num].frameY = 36;
			Main.tile[x - 1, y + 2 + num].frameX = 0;
			Main.tile[x - 1, y + 2 + num].type = (byte)type;
			Main.tile[x, y + 2 + num].active(active: true);
			Main.tile[x, y + 2 + num].frameY = 36;
			Main.tile[x, y + 2 + num].frameX = 18;
			Main.tile[x, y + 2 + num].type = (byte)type;
			Main.tile[x + 1, y + 2 + num].active(active: true);
			Main.tile[x + 1, y + 2 + num].frameY = 36;
			Main.tile[x + 1, y + 2 + num].frameX = 36;
			Main.tile[x + 1, y + 2 + num].type = (byte)type;
		}
	}

	public static void PlaceSunflower(int x, int y, int type = 27)
	{
		if ((double)y > Main.worldSurface - 1.0)
		{
			return;
		}
		bool flag = true;
		for (int i = x; i < x + 2; i++)
		{
			for (int j = y - 3; j < y + 1; j++)
			{
				if (Main.tile[i, j] == null)
				{
					Main.tile[i, j] = default(Tile);
				}
				if (Main.tile[i, j].HasTile || Main.tile[i, j].wall > 0)
				{
					flag = false;
				}
			}
			if (Main.tile[i, y + 1] == null)
			{
				Main.tile[i, y + 1] = default(Tile);
			}
			if (!Main.tile[i, y + 1].HasTile || (Main.tile[i, y + 1].type != 2 && Main.tile[i, y + 1].type != 109))
			{
				flag = false;
			}
		}
		if (!flag)
		{
			return;
		}
		for (int k = 0; k < 2; k++)
		{
			for (int l = -3; l < 1; l++)
			{
				int num = k * 18 + WorldGen.genRand.Next(3) * 36;
				int num2 = (l + 3) * 18;
				Main.tile[x + k, y + l].active(active: true);
				Main.tile[x + k, y + l].frameX = (short)num;
				Main.tile[x + k, y + l].frameY = (short)num2;
				Main.tile[x + k, y + l].type = (byte)type;
			}
		}
	}

	public static bool PlacePot(int x, int y, int type = 28)
	{
		bool flag = true;
		for (int i = x; i < x + 2; i++)
		{
			for (int j = y - 1; j < y + 1; j++)
			{
				if (Main.tile[i, j] == null)
				{
					Main.tile[i, j] = default(Tile);
				}
				if (Main.tile[i, j].HasTile)
				{
					flag = false;
				}
			}
			if (Main.tile[i, y + 1] == null)
			{
				Main.tile[i, y + 1] = default(Tile);
			}
			if (!Main.tile[i, y + 1].HasTile || !Main.tileSolid[Main.tile[i, y + 1].type])
			{
				flag = false;
			}
		}
		if (flag)
		{
			for (int k = 0; k < 2; k++)
			{
				for (int l = -1; l < 1; l++)
				{
					int num = k * 18 + WorldGen.genRand.Next(3) * 36;
					int num2 = (l + 1) * 18;
					Main.tile[x + k, y + l].active(active: true);
					Main.tile[x + k, y + l].frameX = (short)num;
					Main.tile[x + k, y + l].frameY = (short)num2;
					Main.tile[x + k, y + l].type = (byte)type;
				}
			}
			return true;
		}
		return false;
	}

	public static bool CheckCactus(int i, int j)
	{
		int num = j;
		int num2 = i;
		while (Main.tile[num2, num].HasTile && Main.tile[num2, num].type == 80)
		{
			num++;
			if (!Main.tile[num2, num].HasTile || Main.tile[num2, num].type != 80)
			{
				if (Main.tile[num2 - 1, num].HasTile && Main.tile[num2 - 1, num].type == 80 && Main.tile[num2 - 1, num - 1].HasTile && Main.tile[num2 - 1, num - 1].type == 80 && num2 >= i)
				{
					num2--;
				}
				if (Main.tile[num2 + 1, num].HasTile && Main.tile[num2 + 1, num].type == 80 && Main.tile[num2 + 1, num - 1].HasTile && Main.tile[num2 + 1, num - 1].type == 80 && num2 <= i)
				{
					num2++;
				}
			}
		}
		if (!Main.tile[num2, num].HasTile || (Main.tile[num2, num].type != 53 && Main.tile[num2, num].type != 112 && Main.tile[num2, num].type != 116))
		{
			KillTile(i, j);
			return true;
		}
		if (i != num2)
		{
			if ((!Main.tile[i, j + 1].HasTile || Main.tile[i, j + 1].type != 80) && (!Main.tile[i - 1, j].HasTile || Main.tile[i - 1, j].type != 80) && (!Main.tile[i + 1, j].HasTile || Main.tile[i + 1, j].type != 80))
			{
				KillTile(i, j);
				return true;
			}
		}
		else if (i == num2 && (!Main.tile[i, j + 1].HasTile || (Main.tile[i, j + 1].type != 80 && Main.tile[i, j + 1].type != 53 && Main.tile[i, j + 1].type != 112 && Main.tile[i, j + 1].type != 116)))
		{
			KillTile(i, j);
			return true;
		}
		return false;
	}

	public static void PlantCactus(int i, int j)
	{
		GrowCactus(i, j);
		for (int k = 0; k < 150; k++)
		{
			int i2 = WorldGen.genRand.Next(i - 1, i + 2);
			int j2 = WorldGen.genRand.Next(j - 10, j + 2);
			GrowCactus(i2, j2);
		}
	}

	public static void CactusFrame(int i, int j)
	{
		try
		{
			int num = j;
			int num2 = i;
			if (CheckCactus(i, j))
			{
				return;
			}
			while (Main.tile[num2, num].HasTile && Main.tile[num2, num].type == 80)
			{
				num++;
				if (!Main.tile[num2, num].HasTile || Main.tile[num2, num].type != 80)
				{
					if (Main.tile[num2 - 1, num].HasTile && Main.tile[num2 - 1, num].type == 80 && Main.tile[num2 - 1, num - 1].HasTile && Main.tile[num2 - 1, num - 1].type == 80 && num2 >= i)
					{
						num2--;
					}
					if (Main.tile[num2 + 1, num].HasTile && Main.tile[num2 + 1, num].type == 80 && Main.tile[num2 + 1, num - 1].HasTile && Main.tile[num2 + 1, num - 1].type == 80 && num2 <= i)
					{
						num2++;
					}
				}
			}
			num--;
			int num3 = i - num2;
			num2 = i;
			num = j;
			int type = Main.tile[i - 2, j].type;
			int num4 = Main.tile[i - 1, j].type;
			int num5 = Main.tile[i + 1, j].type;
			int num6 = Main.tile[i, j - 1].type;
			int num7 = Main.tile[i, j + 1].type;
			int num8 = Main.tile[i - 1, j + 1].type;
			int num9 = Main.tile[i + 1, j + 1].type;
			if (!Main.tile[i - 1, j].HasTile)
			{
				num4 = -1;
			}
			if (!Main.tile[i + 1, j].HasTile)
			{
				num5 = -1;
			}
			if (!Main.tile[i, j - 1].HasTile)
			{
				num6 = -1;
			}
			if (!Main.tile[i, j + 1].HasTile)
			{
				num7 = -1;
			}
			if (!Main.tile[i - 1, j + 1].HasTile)
			{
				num8 = -1;
			}
			if (!Main.tile[i + 1, j + 1].HasTile)
			{
				num9 = -1;
			}
			short num10 = Main.tile[i, j].frameX;
			short num11 = Main.tile[i, j].frameY;
			switch (num3)
			{
			case 0:
				if (num6 != 80)
				{
					if (num4 == 80 && num5 == 80 && num8 != 80 && num9 != 80 && type != 80)
					{
						num10 = 90;
						num11 = 0;
					}
					else if (num4 == 80 && num8 != 80 && type != 80)
					{
						num10 = 72;
						num11 = 0;
					}
					else if (num5 == 80 && num9 != 80)
					{
						num10 = 18;
						num11 = 0;
					}
					else
					{
						num10 = 0;
						num11 = 0;
					}
				}
				else if (num4 == 80 && num5 == 80 && num8 != 80 && num9 != 80 && type != 80)
				{
					num10 = 90;
					num11 = 36;
				}
				else if (num4 == 80 && num8 != 80 && type != 80)
				{
					num10 = 72;
					num11 = 36;
				}
				else if (num5 == 80 && num9 != 80)
				{
					num10 = 18;
					num11 = 36;
				}
				else if (num7 >= 0 && Main.tileSolid[num7])
				{
					num10 = 0;
					num11 = 36;
				}
				else
				{
					num10 = 0;
					num11 = 18;
				}
				break;
			case -1:
				if (num5 == 80)
				{
					if (num6 != 80 && num7 != 80)
					{
						num10 = 108;
						num11 = 36;
					}
					else if (num7 != 80)
					{
						num10 = 54;
						num11 = 36;
					}
					else if (num6 != 80)
					{
						num10 = 54;
						num11 = 0;
					}
					else
					{
						num10 = 54;
						num11 = 18;
					}
				}
				else if (num6 != 80)
				{
					num10 = 54;
					num11 = 0;
				}
				else
				{
					num10 = 54;
					num11 = 18;
				}
				break;
			case 1:
				if (num4 == 80)
				{
					if (num6 != 80 && num7 != 80)
					{
						num10 = 108;
						num11 = 16;
					}
					else if (num7 != 80)
					{
						num10 = 36;
						num11 = 36;
					}
					else if (num6 != 80)
					{
						num10 = 36;
						num11 = 0;
					}
					else
					{
						num10 = 36;
						num11 = 18;
					}
				}
				else if (num6 != 80)
				{
					num10 = 36;
					num11 = 0;
				}
				else
				{
					num10 = 36;
					num11 = 18;
				}
				break;
			}
			if (num10 != Main.tile[i, j].frameX || num11 != Main.tile[i, j].frameY)
			{
				Main.tile[i, j].frameX = num10;
				Main.tile[i, j].frameY = num11;
				SquareTileFrame(i, j);
			}
		}
		catch
		{
			Main.tile[i, j].frameX = 0;
			Main.tile[i, j].frameY = 0;
		}
	}

	public static void GrowCactus(int i, int j)
	{
		int num = j;
		int num2 = i;
		Tile tile = Main.tile[i, j];
		if (!tile.HasTile)
		{
			return;
		}
		tile = Main.tile[i, j - 1];
		if (tile.liquid > 0)
		{
			return;
		}
		tile = Main.tile[i, j];
		if (tile.type != 53)
		{
			tile = Main.tile[i, j];
			if (tile.type != 80)
			{
				tile = Main.tile[i, j];
				if (tile.type != 112)
				{
					tile = Main.tile[i, j];
					if (tile.type != 116)
					{
						return;
					}
				}
			}
		}
		tile = Main.tile[i, j];
		if (tile.type != 53)
		{
			tile = Main.tile[i, j];
			if (tile.type != 112)
			{
				tile = Main.tile[i, j];
				if (tile.type != 116)
				{
					tile = Main.tile[i, j];
					if (tile.type != 80)
					{
						return;
					}
					while (true)
					{
						tile = Main.tile[num2, num];
						if (!tile.HasTile)
						{
							break;
						}
						tile = Main.tile[num2, num];
						if (tile.type != 80)
						{
							break;
						}
						num++;
						tile = Main.tile[num2, num];
						if (tile.HasTile)
						{
							tile = Main.tile[num2, num];
							if (tile.type == 80)
							{
								continue;
							}
						}
						tile = Main.tile[num2 - 1, num];
						if (tile.HasTile)
						{
							tile = Main.tile[num2 - 1, num];
							if (tile.type == 80)
							{
								tile = Main.tile[num2 - 1, num - 1];
								if (tile.HasTile)
								{
									tile = Main.tile[num2 - 1, num - 1];
									if (tile.type == 80 && num2 >= i)
									{
										num2--;
									}
								}
							}
						}
						tile = Main.tile[num2 + 1, num];
						if (!tile.HasTile)
						{
							continue;
						}
						tile = Main.tile[num2 + 1, num];
						if (tile.type != 80)
						{
							continue;
						}
						tile = Main.tile[num2 + 1, num - 1];
						if (tile.HasTile)
						{
							tile = Main.tile[num2 + 1, num - 1];
							if (tile.type == 80 && num2 <= i)
							{
								num2++;
							}
						}
					}
					num--;
					int num5 = num - j;
					int num6 = i - num2;
					num2 = i - num6;
					num = j;
					int num7 = 11 - num5;
					int num8 = 0;
					for (int m = num2 - 2; m <= num2 + 2; m++)
					{
						for (int n = num - num7; n <= num + num5; n++)
						{
							tile = Main.tile[m, n];
							if (tile.HasTile)
							{
								tile = Main.tile[m, n];
								if (tile.type == 80)
								{
									num8++;
								}
							}
						}
					}
					if (num8 >= WorldGen.genRand.Next(11, 13))
					{
						return;
					}
					num2 = i;
					num = j;
					if (num6 == 0)
					{
						if (num5 == 0)
						{
							tile = Main.tile[num2, num - 1];
							if (!tile.HasTile)
							{
								tile = Main.tile[num2, num - 1];
								tile.active(active: true);
								tile = Main.tile[num2, num - 1];
								tile.type = 80;
								SquareTileFrame(num2, num - 1);
								if (Main.netMode == 2)
								{
									NetMessage.SendTileSquare(-1, num2, num - 1, 1);
								}
							}
							return;
						}
						bool flag = false;
						bool flag2 = false;
						tile = Main.tile[num2, num - 1];
						if (tile.HasTile)
						{
							tile = Main.tile[num2, num - 1];
							if (tile.type == 80)
							{
								tile = Main.tile[num2 - 1, num];
								if (!tile.HasTile)
								{
									tile = Main.tile[num2 - 2, num + 1];
									if (!tile.HasTile)
									{
										tile = Main.tile[num2 - 1, num - 1];
										if (!tile.HasTile)
										{
											tile = Main.tile[num2 - 1, num + 1];
											if (!tile.HasTile)
											{
												tile = Main.tile[num2 - 2, num];
												if (!tile.HasTile)
												{
													flag = true;
												}
											}
										}
									}
								}
								tile = Main.tile[num2 + 1, num];
								if (!tile.HasTile)
								{
									tile = Main.tile[num2 + 2, num + 1];
									if (!tile.HasTile)
									{
										tile = Main.tile[num2 + 1, num - 1];
										if (!tile.HasTile)
										{
											tile = Main.tile[num2 + 1, num + 1];
											if (!tile.HasTile)
											{
												tile = Main.tile[num2 + 2, num];
												if (!tile.HasTile)
												{
													flag2 = true;
												}
											}
										}
									}
								}
							}
						}
						int num9 = WorldGen.genRand.Next(3);
						if ((num9 == 0) & flag)
						{
							tile = Main.tile[num2 - 1, num];
							tile.active(active: true);
							tile = Main.tile[num2 - 1, num];
							tile.type = 80;
							SquareTileFrame(num2 - 1, num);
							if (Main.netMode == 2)
							{
								NetMessage.SendTileSquare(-1, num2 - 1, num, 1);
							}
						}
						else if ((num9 == 1) & flag2)
						{
							tile = Main.tile[num2 + 1, num];
							tile.active(active: true);
							tile = Main.tile[num2 + 1, num];
							tile.type = 80;
							SquareTileFrame(num2 + 1, num);
							if (Main.netMode == 2)
							{
								NetMessage.SendTileSquare(-1, num2 + 1, num, 1);
							}
						}
						else
						{
							if (num5 >= WorldGen.genRand.Next(2, 8))
							{
								return;
							}
							tile = Main.tile[num2 - 1, num - 1];
							if (tile.HasTile)
							{
								tile = Main.tile[num2 - 1, num - 1];
								_ = ref tile.type;
							}
							tile = Main.tile[num2 + 1, num - 1];
							if (tile.HasTile)
							{
								tile = Main.tile[num2 + 1, num - 1];
								if (tile.type == 80)
								{
									return;
								}
							}
							tile = Main.tile[num2, num - 1];
							if (!tile.HasTile)
							{
								tile = Main.tile[num2, num - 1];
								tile.active(active: true);
								tile = Main.tile[num2, num - 1];
								tile.type = 80;
								SquareTileFrame(num2, num - 1);
								if (Main.netMode == 2)
								{
									NetMessage.SendTileSquare(-1, num2, num - 1, 1);
								}
							}
						}
						return;
					}
					tile = Main.tile[num2, num - 1];
					if (tile.HasTile)
					{
						return;
					}
					tile = Main.tile[num2, num - 2];
					if (tile.HasTile)
					{
						return;
					}
					tile = Main.tile[num2 + num6, num - 1];
					if (tile.HasTile)
					{
						return;
					}
					tile = Main.tile[num2 - num6, num - 1];
					if (!tile.HasTile)
					{
						return;
					}
					tile = Main.tile[num2 - num6, num - 1];
					if (tile.type == 80)
					{
						tile = Main.tile[num2, num - 1];
						tile.active(active: true);
						tile = Main.tile[num2, num - 1];
						tile.type = 80;
						SquareTileFrame(num2, num - 1);
						if (Main.netMode == 2)
						{
							NetMessage.SendTileSquare(-1, num2, num - 1, 1);
						}
					}
					return;
				}
			}
		}
		tile = Main.tile[i, j - 1];
		if (tile.HasTile)
		{
			return;
		}
		tile = Main.tile[i - 1, j - 1];
		if (tile.HasTile)
		{
			return;
		}
		tile = Main.tile[i + 1, j - 1];
		if (tile.HasTile)
		{
			return;
		}
		int num10 = 0;
		int num11 = 0;
		for (int k = i - 6; k <= i + 6; k++)
		{
			for (int l = j - 3; l <= j + 1; l++)
			{
				try
				{
					tile = Main.tile[k, l];
					if (!tile.HasTile)
					{
						continue;
					}
					tile = Main.tile[k, l];
					if (tile.type == 80)
					{
						num10++;
						if (num10 >= 4)
						{
							return;
						}
					}
					tile = Main.tile[k, l];
					if (tile.type == 53)
					{
						goto IL_01e4;
					}
					tile = Main.tile[k, l];
					if (tile.type == 112)
					{
						goto IL_01e4;
					}
					tile = Main.tile[k, l];
					if (tile.type == 116)
					{
						goto IL_01e4;
					}
					goto end_IL_014e;
					IL_01e4:
					num11++;
					end_IL_014e:;
				}
				catch
				{
				}
			}
		}
		if (num11 > 10)
		{
			tile = Main.tile[i, j - 1];
			tile.active(active: true);
			tile = Main.tile[i, j - 1];
			tile.type = 80;
			if (Main.netMode == 2)
			{
				NetMessage.SendTileSquare(-1, i, j - 1, 1);
			}
			SquareTileFrame(num2, num - 1);
		}
	}

	public static int PlaceChest(int x, int y, int type = 21, bool notNearOtherChests = false, int style = 0)
	{
		bool flag = true;
		int num = -1;
		for (int i = x; i < x + 2; i++)
		{
			for (int j = y - 1; j < y + 1; j++)
			{
				if (WorldGen.InWorld(i, j, 20))
				{
					if (Main.tile[i, j] == null)
					{
						Main.tile[i, j] = default(Tile);
					}
					if (Main.tile[i, j].HasTile)
					{
						flag = false;
					}
					if (Main.tile[i, j].lava())
					{
						flag = false;
					}
				}
			}
			if (WorldGen.InWorld(i, y, 20))
			{
				if (Main.tile[i, y + 1] == null)
				{
					Main.tile[i, y + 1] = default(Tile);
				}
				if (!Main.tile[i, y + 1].HasTile || !Main.tileSolid[Main.tile[i, y + 1].type])
				{
					flag = false;
				}
			}
		}
		if (flag & notNearOtherChests)
		{
			for (int k = x - 25; k < x + 25; k++)
			{
				for (int l = y - 8; l < y + 8; l++)
				{
					try
					{
						if (Main.tile[k, l].HasTile && Main.tile[k, l].type == 21)
						{
							flag = false;
							return -1;
						}
					}
					catch
					{
					}
				}
			}
		}
		if (flag)
		{
			num = Chest.CreateChest(x, y - 1);
			if (num == -1)
			{
				flag = false;
			}
		}
		if (flag)
		{
			Main.tile[x, y - 1].active(active: true);
			Main.tile[x, y - 1].frameY = 0;
			Main.tile[x, y - 1].frameX = (short)(36 * style);
			Main.tile[x, y - 1].type = (byte)type;
			Main.tile[x + 1, y - 1].active(active: true);
			Main.tile[x + 1, y - 1].frameY = 0;
			Main.tile[x + 1, y - 1].frameX = (short)(18 + 36 * style);
			Main.tile[x + 1, y - 1].type = (byte)type;
			Main.tile[x, y].active(active: true);
			Main.tile[x, y].frameY = 18;
			Main.tile[x, y].frameX = (short)(36 * style);
			Main.tile[x, y].type = (byte)type;
			Main.tile[x + 1, y].active(active: true);
			Main.tile[x + 1, y].frameY = 18;
			Main.tile[x + 1, y].frameX = (short)(18 + 36 * style);
			Main.tile[x + 1, y].type = (byte)type;
		}
		return num;
	}

	public static bool PlaceTile(int i, int j, int type, bool mute = false, bool forced = false, int plr = -1, int style = 0)
	{
		//IL_1777: Unknown result type (might be due to invalid IL or missing references)
		//IL_178f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1795: Unknown result type (might be due to invalid IL or missing references)
		if (type >= 150)
		{
			return false;
		}
		bool result = false;
		if (i >= 0 && j >= 0 && i < Main.maxTilesX && j < Main.maxTilesY)
		{
			if (Main.tile[i, j] == null)
			{
				Main.tile[i, j] = default(Tile);
			}
			if (forced || Collision.EmptyTile(i, j) || !Main.tileSolid[type] || (type == 23 && Main.tile[i, j].type == 0 && Main.tile[i, j].HasTile) || (type == 2 && Main.tile[i, j].type == 0 && Main.tile[i, j].HasTile) || (type == 109 && Main.tile[i, j].type == 0 && Main.tile[i, j].HasTile) || (type == 60 && Main.tile[i, j].type == 59 && Main.tile[i, j].HasTile) || (type == 70 && Main.tile[i, j].type == 59 && Main.tile[i, j].HasTile))
			{
				if (type == 23 && (Main.tile[i, j].type != 0 || !Main.tile[i, j].HasTile))
				{
					return false;
				}
				if (type == 2 && (Main.tile[i, j].type != 0 || !Main.tile[i, j].HasTile))
				{
					return false;
				}
				if (type == 109 && (Main.tile[i, j].type != 0 || !Main.tile[i, j].HasTile))
				{
					return false;
				}
				if (type == 60 && (Main.tile[i, j].type != 59 || !Main.tile[i, j].HasTile))
				{
					return false;
				}
				if (type == 81)
				{
					if (Main.tile[i - 1, j] == null)
					{
						Main.tile[i - 1, j] = default(Tile);
					}
					if (Main.tile[i + 1, j] == null)
					{
						Main.tile[i + 1, j] = default(Tile);
					}
					if (Main.tile[i, j - 1] == null)
					{
						Main.tile[i, j - 1] = default(Tile);
					}
					if (Main.tile[i, j + 1] == null)
					{
						Main.tile[i, j + 1] = default(Tile);
					}
					if (Main.tile[i - 1, j].HasTile || Main.tile[i + 1, j].HasTile || Main.tile[i, j - 1].HasTile)
					{
						return false;
					}
					if (!Main.tile[i, j + 1].HasTile || !Main.tileSolid[Main.tile[i, j + 1].type])
					{
						return false;
					}
				}
				if (Main.tile[i, j].liquid > 0)
				{
					if (type == 4)
					{
						if (style != 8)
						{
							return false;
						}
					}
					else if (type == 3 || type == 4 || type == 20 || type == 24 || type == 27 || type == 32 || type == 51 || type == 69 || type == 72)
					{
						return false;
					}
				}
				Main.tile[i, j].frameY = 0;
				Main.tile[i, j].frameX = 0;
				switch (type)
				{
				case 3:
				case 24:
				case 110:
					if (j + 1 >= Main.maxTilesY || !Main.tile[i, j + 1].HasTile || ((Main.tile[i, j + 1].type != 2 || type != 3) && (Main.tile[i, j + 1].type != 23 || type != 24) && (Main.tile[i, j + 1].type != 78 || type != 3) && (Main.tile[i, j + 1].type != 109 || type != 110)))
					{
						break;
					}
					if (type == 24 && WorldGen.genRand.Next(13) == 0)
					{
						Main.tile[i, j].active(active: true);
						Main.tile[i, j].type = 32;
						SquareTileFrame(i, j);
					}
					else if (Main.tile[i, j + 1].type == 78)
					{
						Main.tile[i, j].active(active: true);
						Main.tile[i, j].type = (byte)type;
						Main.tile[i, j].frameX = (short)(WorldGen.genRand.Next(2) * 18 + 108);
					}
					else if (Main.tile[i, j].wall == 0 && Main.tile[i, j + 1].wall == 0)
					{
						if (WorldGen.genRand.Next(50) == 0 || (type == 24 && WorldGen.genRand.Next(40) == 0))
						{
							Main.tile[i, j].active(active: true);
							Main.tile[i, j].type = (byte)type;
							Main.tile[i, j].frameX = 144;
						}
						else if (WorldGen.genRand.Next(35) == 0)
						{
							Main.tile[i, j].active(active: true);
							Main.tile[i, j].type = (byte)type;
							Main.tile[i, j].frameX = (short)(WorldGen.genRand.Next(2) * 18 + 108);
						}
						else
						{
							Main.tile[i, j].active(active: true);
							Main.tile[i, j].type = (byte)type;
							Main.tile[i, j].frameX = (short)(WorldGen.genRand.Next(6) * 18);
						}
					}
					break;
				case 61:
					if (j + 1 < Main.maxTilesY && Main.tile[i, j + 1].HasTile && Main.tile[i, j + 1].type == 60)
					{
						if (WorldGen.genRand.Next(16) == 0 && (double)j > Main.worldSurface)
						{
							Main.tile[i, j].active(active: true);
							Main.tile[i, j].type = 69;
							SquareTileFrame(i, j);
						}
						else if (WorldGen.genRand.Next(60) == 0 && (double)j > Main.rockLayer)
						{
							Main.tile[i, j].active(active: true);
							Main.tile[i, j].type = (byte)type;
							Main.tile[i, j].frameX = 144;
						}
						else if (WorldGen.genRand.Next(1000) == 0 && (double)j > Main.rockLayer)
						{
							Main.tile[i, j].active(active: true);
							Main.tile[i, j].type = (byte)type;
							Main.tile[i, j].frameX = 162;
						}
						else if (WorldGen.genRand.Next(15) == 0)
						{
							Main.tile[i, j].active(active: true);
							Main.tile[i, j].type = (byte)type;
							Main.tile[i, j].frameX = (short)(WorldGen.genRand.Next(2) * 18 + 108);
						}
						else
						{
							Main.tile[i, j].active(active: true);
							Main.tile[i, j].type = (byte)type;
							Main.tile[i, j].frameX = (short)(WorldGen.genRand.Next(6) * 18);
						}
					}
					break;
				case 71:
					if (j + 1 < Main.maxTilesY && Main.tile[i, j + 1].HasTile && Main.tile[i, j + 1].type == 70)
					{
						Main.tile[i, j].active(active: true);
						Main.tile[i, j].type = (byte)type;
						Main.tile[i, j].frameX = (short)(WorldGen.genRand.Next(5) * 18);
					}
					break;
				case 129:
					if (SolidTile(i - 1, j) || SolidTile(i + 1, j) || SolidTile(i, j - 1) || SolidTile(i, j + 1))
					{
						Main.tile[i, j].active(active: true);
						Main.tile[i, j].type = (byte)type;
						Main.tile[i, j].frameX = (short)(WorldGen.genRand.Next(8) * 18);
						SquareTileFrame(i, j);
					}
					break;
				case 132:
				case 138:
				case 142:
				case 143:
					Place2x2(i, j, type);
					break;
				case 137:
					Main.tile[i, j].active(active: true);
					Main.tile[i, j].type = (byte)type;
					if (style == 1)
					{
						Main.tile[i, j].frameX = 18;
					}
					break;
				case 136:
					if (Main.tile[i - 1, j] == null)
					{
						Main.tile[i - 1, j] = default(Tile);
					}
					if (Main.tile[i + 1, j] == null)
					{
						Main.tile[i + 1, j] = default(Tile);
					}
					if (Main.tile[i, j + 1] == null)
					{
						Main.tile[i, j + 1] = default(Tile);
					}
					if ((Main.tile[i - 1, j].HasTile && (Main.tileSolid[Main.tile[i - 1, j].type] || Main.tile[i - 1, j].type == 124 || (Main.tile[i - 1, j].type == 5 && Main.tile[i - 1, j - 1].type == 5 && Main.tile[i - 1, j + 1].type == 5))) || (Main.tile[i + 1, j].HasTile && (Main.tileSolid[Main.tile[i + 1, j].type] || Main.tile[i + 1, j].type == 124 || (Main.tile[i + 1, j].type == 5 && Main.tile[i + 1, j - 1].type == 5 && Main.tile[i + 1, j + 1].type == 5))) || (Main.tile[i, j + 1].HasTile && Main.tileSolid[Main.tile[i, j + 1].type]))
					{
						Main.tile[i, j].active(active: true);
						Main.tile[i, j].type = (byte)type;
						SquareTileFrame(i, j);
					}
					break;
				case 4:
					if (Main.tile[i - 1, j] == null)
					{
						Main.tile[i - 1, j] = default(Tile);
					}
					if (Main.tile[i + 1, j] == null)
					{
						Main.tile[i + 1, j] = default(Tile);
					}
					if (Main.tile[i, j + 1] == null)
					{
						Main.tile[i, j + 1] = default(Tile);
					}
					if ((Main.tile[i - 1, j].HasTile && (Main.tileSolid[Main.tile[i - 1, j].type] || Main.tile[i - 1, j].type == 124 || (Main.tile[i - 1, j].type == 5 && Main.tile[i - 1, j - 1].type == 5 && Main.tile[i - 1, j + 1].type == 5))) || (Main.tile[i + 1, j].HasTile && (Main.tileSolid[Main.tile[i + 1, j].type] || Main.tile[i + 1, j].type == 124 || (Main.tile[i + 1, j].type == 5 && Main.tile[i + 1, j - 1].type == 5 && Main.tile[i + 1, j + 1].type == 5))) || (Main.tile[i, j + 1].HasTile && Main.tileSolid[Main.tile[i, j + 1].type]))
					{
						Main.tile[i, j].active(active: true);
						Main.tile[i, j].type = (byte)type;
						Main.tile[i, j].frameY = (short)(22 * style);
						SquareTileFrame(i, j);
					}
					break;
				case 10:
					if (Main.tile[i, j - 1] == null)
					{
						Main.tile[i, j - 1] = default(Tile);
					}
					if (Main.tile[i, j - 2] == null)
					{
						Main.tile[i, j - 2] = default(Tile);
					}
					if (Main.tile[i, j - 3] == null)
					{
						Main.tile[i, j - 3] = default(Tile);
					}
					if (Main.tile[i, j + 1] == null)
					{
						Main.tile[i, j + 1] = default(Tile);
					}
					if (Main.tile[i, j + 2] == null)
					{
						Main.tile[i, j + 2] = default(Tile);
					}
					if (Main.tile[i, j + 3] == null)
					{
						Main.tile[i, j + 3] = default(Tile);
					}
					if (!Main.tile[i, j - 1].HasTile && !Main.tile[i, j - 2].HasTile && Main.tile[i, j - 3].HasTile && Main.tileSolid[Main.tile[i, j - 3].type])
					{
						PlaceDoor(i, j - 1, type);
						SquareTileFrame(i, j);
						break;
					}
					if (!Main.tile[i, j + 1].HasTile && !Main.tile[i, j + 2].HasTile && Main.tile[i, j + 3].HasTile && Main.tileSolid[Main.tile[i, j + 3].type])
					{
						PlaceDoor(i, j + 1, type);
						SquareTileFrame(i, j);
						break;
					}
					return false;
				case 128:
					PlaceMan(i, j, style);
					SquareTileFrame(i, j);
					break;
				case 149:
					if (SolidTile(i - 1, j) || SolidTile(i + 1, j) || SolidTile(i, j - 1) || SolidTile(i, j + 1))
					{
						Main.tile[i, j].frameX = (short)(18 * style);
						Main.tile[i, j].active(active: true);
						Main.tile[i, j].type = (byte)type;
						SquareTileFrame(i, j);
					}
					break;
				case 139:
					PlaceMB(i, j, type, style);
					SquareTileFrame(i, j);
					break;
				case 34:
				case 35:
				case 36:
				case 106:
					Place3x3(i, j, type);
					SquareTileFrame(i, j);
					break;
				case 13:
				case 33:
				case 49:
				case 50:
				case 78:
					PlaceOnTable1x1(i, j, type, style);
					SquareTileFrame(i, j);
					break;
				case 14:
				case 26:
				case 86:
				case 87:
				case 88:
				case 89:
				case 114:
					Place3x2(i, j, type);
					SquareTileFrame(i, j);
					break;
				case 20:
					if (Main.tile[i, j + 1] == null)
					{
						Main.tile[i, j + 1] = default(Tile);
					}
					if (Main.tile[i, j + 1].HasTile && (Main.tile[i, j + 1].type == 2 || Main.tile[i, j + 1].type == 109 || Main.tile[i, j + 1].type == 147))
					{
						Place1x2(i, j, type, style);
						SquareTileFrame(i, j);
					}
					break;
				case 15:
					if (Main.tile[i, j - 1] == null)
					{
						Main.tile[i, j - 1] = default(Tile);
					}
					if (Main.tile[i, j] == null)
					{
						Main.tile[i, j] = default(Tile);
					}
					Place1x2(i, j, type, style);
					SquareTileFrame(i, j);
					break;
				case 16:
				case 18:
				case 29:
				case 103:
				case 134:
					Place2x1(i, j, type);
					SquareTileFrame(i, j);
					break;
				case 92:
				case 93:
					Place1xX(i, j, type);
					SquareTileFrame(i, j);
					break;
				case 104:
				case 105:
					Place2xX(i, j, type, style);
					SquareTileFrame(i, j);
					break;
				case 17:
				case 77:
				case 133:
					Place3x2(i, j, type);
					SquareTileFrame(i, j);
					break;
				case 21:
					PlaceChest(i, j, type, notNearOtherChests: false, style);
					SquareTileFrame(i, j);
					break;
				case 91:
					PlaceBanner(i, j, type, style);
					SquareTileFrame(i, j);
					break;
				case 135:
				case 141:
				case 144:
					Place1x1(i, j, type, style);
					SquareTileFrame(i, j);
					break;
				case 101:
				case 102:
					Place3x4(i, j, type);
					SquareTileFrame(i, j);
					break;
				case 27:
					PlaceSunflower(i, j);
					SquareTileFrame(i, j);
					break;
				case 28:
					PlacePot(i, j);
					SquareTileFrame(i, j);
					break;
				case 42:
					Place1x2Top(i, j, type);
					SquareTileFrame(i, j);
					break;
				case 55:
				case 85:
					PlaceSign(i, j, type);
					break;
				default:
					if (Main.tileAlch[type])
					{
						PlaceAlch(i, j, style);
						break;
					}
					switch (type)
					{
					case 94:
					case 95:
					case 96:
					case 97:
					case 98:
					case 99:
					case 100:
					case 125:
					case 126:
						Place2x2(i, j, type);
						break;
					case 79:
					case 90:
					{
						int direction = 1;
						if (plr > -1)
						{
							direction = Main.player[plr].direction;
						}
						Place4x2(i, j, type, direction);
						break;
					}
					case 81:
						Main.tile[i, j].frameX = (short)(26 * WorldGen.genRand.Next(6));
						Main.tile[i, j].active(active: true);
						Main.tile[i, j].type = (byte)type;
						break;
					default:
						Main.tile[i, j].active(active: true);
						Main.tile[i, j].type = (byte)type;
						break;
					}
					break;
				}
				if (Main.tile[i, j].HasTile && !mute)
				{
					SquareTileFrame(i, j);
					result = true;
					if (type == 22 || type == 140)
					{
						for (int k = 0; k < 3; k++)
						{
							Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 14);
						}
					}
				}
			}
		}
		return result;
	}

	public static void KillTile(int i, int j, bool fail = false, bool effectOnly = false, bool noItem = false)
	{
		//IL_151e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1536: Unknown result type (might be due to invalid IL or missing references)
		//IL_153c: Unknown result type (might be due to invalid IL or missing references)
		if (i < 0 || j < 0 || i >= Main.maxTilesX || j >= Main.maxTilesY)
		{
			return;
		}
		if (Main.tile[i, j] == null)
		{
			Main.tile[i, j] = default(Tile);
		}
		if (!Main.tile[i, j].HasTile)
		{
			return;
		}
		if (j >= 1 && Main.tile[i, j - 1] == null)
		{
			Main.tile[i, j - 1] = default(Tile);
		}
		if (j >= 1 && Main.tile[i, j - 1].HasTile && ((Main.tile[i, j - 1].type == 5 && Main.tile[i, j].type != 5) || (Main.tile[i, j - 1].type == 21 && Main.tile[i, j].type != 21) || (Main.tile[i, j - 1].type == 26 && Main.tile[i, j].type != 26) || (Main.tile[i, j - 1].type == 72 && Main.tile[i, j].type != 72) || (Main.tile[i, j - 1].type == 12 && Main.tile[i, j].type != 12)) && (Main.tile[i, j - 1].type != 5 || ((Main.tile[i, j - 1].frameX != 66 || Main.tile[i, j - 1].frameY < 0 || Main.tile[i, j - 1].frameY > 44) && (Main.tile[i, j - 1].frameX != 88 || Main.tile[i, j - 1].frameY < 66 || Main.tile[i, j - 1].frameY > 110) && Main.tile[i, j - 1].frameY < 198)))
		{
			return;
		}
		int num = 10;
		if (Main.tile[i, j].type == 128)
		{
			int num2 = i;
			int num3 = Main.tile[i, j].frameX;
			int num4;
			for (num4 = Main.tile[i, j].frameX; num4 >= 100; num4 -= 100)
			{
			}
			while (num4 >= 36)
			{
				num4 -= 36;
			}
			if (num4 == 18)
			{
				num3 = Main.tile[i - 1, j].frameX;
				num2--;
			}
			if (num3 >= 100)
			{
				int num5 = 0;
				while (num3 >= 100)
				{
					num3 -= 100;
					num5++;
				}
				_ = Main.tile[num2, j].frameY / 18;
				for (num3 = Main.tile[num2, j].frameX; num3 >= 100; num3 -= 100)
				{
				}
				Main.tile[num2, j].frameX = (short)num3;
			}
		}
		if (fail)
		{
			num = 3;
		}
		if (Main.tile[i, j].type == 138)
		{
			num = 0;
		}
		for (int k = 0; k < num; k++)
		{
			int num7 = 0;
			if (Main.tile[i, j].type == 0)
			{
				num7 = 0;
			}
			if (Main.tile[i, j].type == 1 || Main.tile[i, j].type == 16 || Main.tile[i, j].type == 17 || Main.tile[i, j].type == 38 || Main.tile[i, j].type == 39 || Main.tile[i, j].type == 41 || Main.tile[i, j].type == 43 || Main.tile[i, j].type == 44 || Main.tile[i, j].type == 48 || Main.tileStone[Main.tile[i, j].type] || Main.tile[i, j].type == 85 || Main.tile[i, j].type == 90 || Main.tile[i, j].type == 92 || Main.tile[i, j].type == 96 || Main.tile[i, j].type == 97 || Main.tile[i, j].type == 99 || Main.tile[i, j].type == 105 || Main.tile[i, j].type == 117 || Main.tile[i, j].type == 130 || Main.tile[i, j].type == 131 || Main.tile[i, j].type == 132 || Main.tile[i, j].type == 135 || Main.tile[i, j].type == 135 || Main.tile[i, j].type == 137 || Main.tile[i, j].type == 142 || Main.tile[i, j].type == 143 || Main.tile[i, j].type == 144)
			{
				num7 = 1;
			}
			if (Main.tile[i, j].type == 33 || Main.tile[i, j].type == 95 || Main.tile[i, j].type == 98 || Main.tile[i, j].type == 100)
			{
				num7 = 6;
			}
			if (Main.tile[i, j].type == 5 || Main.tile[i, j].type == 10 || Main.tile[i, j].type == 11 || Main.tile[i, j].type == 14 || Main.tile[i, j].type == 15 || Main.tile[i, j].type == 19 || Main.tile[i, j].type == 30 || Main.tile[i, j].type == 86 || Main.tile[i, j].type == 87 || Main.tile[i, j].type == 88 || Main.tile[i, j].type == 89 || Main.tile[i, j].type == 93 || Main.tile[i, j].type == 94 || Main.tile[i, j].type == 104 || Main.tile[i, j].type == 106 || Main.tile[i, j].type == 114 || Main.tile[i, j].type == 124 || Main.tile[i, j].type == 128 || Main.tile[i, j].type == 139)
			{
				num7 = 7;
			}
			if (Main.tile[i, j].type == 21)
			{
				num7 = ((Main.tile[i, j].frameX >= 108) ? 37 : ((Main.tile[i, j].frameX < 36) ? 7 : 10));
			}
			if (Main.tile[i, j].type == 2)
			{
				num7 = ((WorldGen.genRand.Next(2) != 0) ? 2 : 0);
			}
			if (Main.tile[i, j].type == 127)
			{
				num7 = 67;
			}
			if (Main.tile[i, j].type == 91)
			{
				num7 = -1;
			}
			if (Main.tile[i, j].type == 6 || Main.tile[i, j].type == 26)
			{
				num7 = 8;
			}
			if (Main.tile[i, j].type == 7 || Main.tile[i, j].type == 34 || Main.tile[i, j].type == 47)
			{
				num7 = 9;
			}
			if (Main.tile[i, j].type == 8 || Main.tile[i, j].type == 36 || Main.tile[i, j].type == 45 || Main.tile[i, j].type == 102)
			{
				num7 = 10;
			}
			if (Main.tile[i, j].type == 9 || Main.tile[i, j].type == 35 || Main.tile[i, j].type == 42 || Main.tile[i, j].type == 46 || Main.tile[i, j].type == 126 || Main.tile[i, j].type == 136)
			{
				num7 = 11;
			}
			if (Main.tile[i, j].type == 12)
			{
				num7 = 12;
			}
			if (Main.tile[i, j].type == 3 || Main.tile[i, j].type == 73)
			{
				num7 = 3;
			}
			if (Main.tile[i, j].type == 13 || Main.tile[i, j].type == 54)
			{
				num7 = 13;
			}
			if (Main.tile[i, j].type == 22 || Main.tile[i, j].type == 140)
			{
				num7 = 14;
			}
			if (Main.tile[i, j].type == 28 || Main.tile[i, j].type == 78)
			{
				num7 = 22;
			}
			if (Main.tile[i, j].type == 29)
			{
				num7 = 23;
			}
			if (Main.tile[i, j].type == 40 || Main.tile[i, j].type == 103)
			{
				num7 = 28;
			}
			if (Main.tile[i, j].type == 49)
			{
				num7 = 29;
			}
			if (Main.tile[i, j].type == 50)
			{
				num7 = 22;
			}
			if (Main.tile[i, j].type == 51)
			{
				num7 = 30;
			}
			if (Main.tile[i, j].type == 52)
			{
				num7 = 3;
			}
			if (Main.tile[i, j].type == 53 || Main.tile[i, j].type == 81)
			{
				num7 = 32;
			}
			if (Main.tile[i, j].type == 56 || Main.tile[i, j].type == 75)
			{
				num7 = 37;
			}
			if (Main.tile[i, j].type == 57 || Main.tile[i, j].type == 119 || Main.tile[i, j].type == 141)
			{
				num7 = 36;
			}
			if (Main.tile[i, j].type == 59 || Main.tile[i, j].type == 120)
			{
				num7 = 38;
			}
			if (Main.tile[i, j].type == 61 || Main.tile[i, j].type == 62 || Main.tile[i, j].type == 74 || Main.tile[i, j].type == 80)
			{
				num7 = 40;
			}
			if (Main.tile[i, j].type == 69)
			{
				num7 = 7;
			}
			if (Main.tile[i, j].type == 71 || Main.tile[i, j].type == 72)
			{
				num7 = 26;
			}
			if (Main.tile[i, j].type == 70)
			{
				num7 = 17;
			}
			if (Main.tile[i, j].type == 112)
			{
				num7 = 14;
			}
			if (Main.tile[i, j].type == 123)
			{
				num7 = 53;
			}
			if (Main.tile[i, j].type == 116 || Main.tile[i, j].type == 118 || Main.tile[i, j].type == 147 || Main.tile[i, j].type == 148)
			{
				num7 = 51;
			}
			if (Main.tile[i, j].type == 109)
			{
				num7 = ((WorldGen.genRand.Next(2) != 0) ? 47 : 0);
			}
			if (Main.tile[i, j].type == 110 || Main.tile[i, j].type == 113 || Main.tile[i, j].type == 115)
			{
				num7 = 47;
			}
			if (Main.tile[i, j].type == 107 || Main.tile[i, j].type == 121)
			{
				num7 = 48;
			}
			if (Main.tile[i, j].type == 108 || Main.tile[i, j].type == 122 || Main.tile[i, j].type == 134 || Main.tile[i, j].type == 146)
			{
				num7 = 49;
			}
			if (Main.tile[i, j].type == 111 || Main.tile[i, j].type == 133 || Main.tile[i, j].type == 145)
			{
				num7 = 50;
			}
			if (Main.tile[i, j].type == 149)
			{
				num7 = 49;
			}
			if (Main.tileAlch[Main.tile[i, j].type])
			{
				int num8 = Main.tile[i, j].frameX / 18;
				if (num8 == 0)
				{
					num7 = 3;
				}
				if (num8 == 1)
				{
					num7 = 3;
				}
				if (num8 == 2)
				{
					num7 = 7;
				}
				if (num8 == 3)
				{
					num7 = 17;
				}
				if (num8 == 4)
				{
					num7 = 3;
				}
				if (num8 == 5)
				{
					num7 = 6;
				}
			}
			if (Main.tile[i, j].type == 61)
			{
				num7 = ((WorldGen.genRand.Next(2) != 0) ? 39 : 38);
			}
			if (Main.tile[i, j].type == 58 || Main.tile[i, j].type == 76 || Main.tile[i, j].type == 77)
			{
				num7 = ((WorldGen.genRand.Next(2) != 0) ? 25 : 6);
			}
			if (Main.tile[i, j].type == 37)
			{
				num7 = ((WorldGen.genRand.Next(2) != 0) ? 23 : 6);
			}
			if (Main.tile[i, j].type == 32)
			{
				num7 = ((WorldGen.genRand.Next(2) != 0) ? 24 : 14);
			}
			if (Main.tile[i, j].type == 23 || Main.tile[i, j].type == 24)
			{
				num7 = ((WorldGen.genRand.Next(2) != 0) ? 17 : 14);
			}
			if (Main.tile[i, j].type == 25 || Main.tile[i, j].type == 31)
			{
				num7 = ((WorldGen.genRand.Next(2) != 0) ? 1 : 14);
			}
			if (Main.tile[i, j].type == 20)
			{
				num7 = ((WorldGen.genRand.Next(2) != 0) ? 2 : 7);
			}
			if (Main.tile[i, j].type == 27)
			{
				num7 = ((WorldGen.genRand.Next(2) != 0) ? 19 : 3);
			}
			if (Main.tile[i, j].type == 129)
			{
				num7 = ((Main.tile[i, j].frameX != 0 && Main.tile[i, j].frameX != 54 && Main.tile[i, j].frameX != 108) ? ((Main.tile[i, j].frameX != 18 && Main.tile[i, j].frameX != 72 && Main.tile[i, j].frameX != 126) ? 70 : 69) : 68);
			}
			if (Main.tile[i, j].type == 4)
			{
				int num9 = Main.tile[i, j].frameY / 22;
				num7 = num9 switch
				{
					0 => 6, 
					8 => 75, 
					_ => 58 + num9, 
				};
			}
			if ((Main.tile[i, j].type == 34 || Main.tile[i, j].type == 35 || Main.tile[i, j].type == 36 || Main.tile[i, j].type == 42) && Main.rand.Next(2) == 0)
			{
				num7 = 6;
			}
			if (num7 >= 0)
			{
				Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, num7);
			}
		}
		if (effectOnly)
		{
			return;
		}
		if (fail)
		{
			if (Main.tile[i, j].type == 2 || Main.tile[i, j].type == 23 || Main.tile[i, j].type == 109)
			{
				Main.tile[i, j].type = 0;
			}
			if (Main.tile[i, j].type == 60 || Main.tile[i, j].type == 70)
			{
				Main.tile[i, j].type = 59;
			}
			SquareTileFrame(i, j);
			return;
		}
		if (Main.tile[i, j].type == 21 && Main.netMode != 1)
		{
			int num10 = Main.tile[i, j].frameX / 18;
			int y = j - Main.tile[i, j].frameY / 18;
			while (num10 > 1)
			{
				num10 -= 2;
			}
			num10 = i - num10;
			if (!Chest.DestroyChest(num10, y))
			{
				return;
			}
		}
		Main.tile[i, j].active(active: false);
		Main.tile[i, j].frameX = -1;
		Main.tile[i, j].frameY = -1;
		Main.tile[i, j].frameNumber(0);
		if (Main.tile[i, j].type == 58 && j > Main.maxTilesY - 200)
		{
			Main.tile[i, j].lava(lava: true);
			Main.tile[i, j].liquid = 128;
		}
		Main.tile[i, j].type = 0;
		SquareTileFrame(i, j);
	}

	public static bool SolidTile(int i, int j)
	{
		try
		{
			if (Main.tile[i, j] == null)
			{
				return true;
			}
			if (Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].type] && !Main.tileSolidTop[Main.tile[i, j].type])
			{
				return true;
			}
		}
		catch
		{
		}
		return false;
	}

	public static void MineHouse(int i, int j)
	{
		if (i < 50 || i > Main.maxTilesX - 50 || j < 50 || j > Main.maxTilesY - 50)
		{
			return;
		}
		int num = WorldGen.genRand.Next(6, 12);
		int num2 = WorldGen.genRand.Next(3, 6);
		int num3 = WorldGen.genRand.Next(15, 30);
		int num4 = WorldGen.genRand.Next(15, 30);
		if (SolidTile(i, j) || Main.tile[i, j].wall > 0)
		{
			return;
		}
		int num5 = j - num;
		int num6 = j + num2;
		for (int k = 0; k < 2; k++)
		{
			bool flag = true;
			int num7 = i;
			int num8 = j;
			int num9 = -1;
			int num10 = num3;
			if (k == 1)
			{
				num9 = 1;
				num10 = num4;
				num7++;
			}
			while (flag)
			{
				if (num8 - num < num5)
				{
					num5 = num8 - num;
				}
				if (num8 + num2 > num6)
				{
					num6 = num8 + num2;
				}
				for (int l = 0; l < 2; l++)
				{
					int num11 = num8;
					bool flag2 = true;
					int num12 = num;
					int num13 = -1;
					if (l == 1)
					{
						num11++;
						num12 = num2;
						num13 = 1;
					}
					while (flag2)
					{
						if (num7 != i && Main.tile[num7 - num9, num11].wall != 27 && (SolidTile(num7 - num9, num11) || !Main.tile[num7 - num9, num11].HasTile))
						{
							Main.tile[num7 - num9, num11].active(active: true);
							Main.tile[num7 - num9, num11].type = 30;
						}
						if (SolidTile(num7 - 1, num11))
						{
							Main.tile[num7 - 1, num11].type = 30;
						}
						if (SolidTile(num7 + 1, num11))
						{
							Main.tile[num7 + 1, num11].type = 30;
						}
						if (SolidTile(num7, num11))
						{
							int num14 = 0;
							if (SolidTile(num7 - 1, num11))
							{
								num14++;
							}
							if (SolidTile(num7 + 1, num11))
							{
								num14++;
							}
							if (SolidTile(num7, num11 - 1))
							{
								num14++;
							}
							if (SolidTile(num7, num11 + 1))
							{
								num14++;
							}
							if (num14 < 2)
							{
								Main.tile[num7, num11].active(active: false);
							}
							else
							{
								flag2 = false;
								Main.tile[num7, num11].type = 30;
							}
						}
						else
						{
							Main.tile[num7, num11].wall = 27;
							Main.tile[num7, num11].liquid = 0;
							Main.tile[num7, num11].lava(lava: false);
						}
						num11 += num13;
						num12--;
						if (num12 <= 0)
						{
							if (!Main.tile[num7, num11].HasTile)
							{
								Main.tile[num7, num11].active(active: true);
								Main.tile[num7, num11].type = 30;
							}
							flag2 = false;
						}
					}
				}
				num10--;
				num7 += num9;
				if (SolidTile(num7, num8))
				{
					int num15 = 0;
					int num16 = 0;
					int num17 = num8;
					bool flag3 = true;
					while (flag3)
					{
						num17--;
						num15++;
						if (SolidTile(num7 - num9, num17))
						{
							num15 = 999;
							flag3 = false;
						}
						else if (!SolidTile(num7, num17))
						{
							flag3 = false;
						}
					}
					num17 = num8;
					flag3 = true;
					while (flag3)
					{
						num17++;
						num16++;
						if (SolidTile(num7 - num9, num17))
						{
							num16 = 999;
							flag3 = false;
						}
						else if (!SolidTile(num7, num17))
						{
							flag3 = false;
						}
					}
					if (num16 <= num15)
					{
						if (num16 > num2)
						{
							num10 = 0;
						}
						else
						{
							num8 += num16 + 1;
						}
					}
					else if (num15 > num)
					{
						num10 = 0;
					}
					else
					{
						num8 -= num15 + 1;
					}
				}
				if (num10 <= 0)
				{
					flag = false;
				}
			}
		}
		int num18 = i - num3 - 1;
		int num19 = i + num4 + 2;
		int num20 = num5 - 1;
		int num21 = num6 + 2;
		for (int m = num18; m < num19; m++)
		{
			for (int n = num20; n < num21; n++)
			{
				if (Main.tile[m, n].wall == 27 && !Main.tile[m, n].HasTile)
				{
					if (Main.tile[m - 1, n].wall != 27 && m < i && !SolidTile(m - 1, n))
					{
						PlaceTile(m, n, 30, mute: true);
						Main.tile[m, n].wall = 0;
					}
					if (Main.tile[m + 1, n].wall != 27 && m > i && !SolidTile(m + 1, n))
					{
						PlaceTile(m, n, 30, mute: true);
						Main.tile[m, n].wall = 0;
					}
					for (int num22 = m - 1; num22 <= m + 1; num22++)
					{
						for (int num23 = n - 1; num23 <= n + 1; num23++)
						{
							if (SolidTile(num22, num23))
							{
								Main.tile[num22, num23].type = 30;
							}
						}
					}
				}
				if (Main.tile[m, n].type == 30 && Main.tile[m - 1, n].wall == 27 && Main.tile[m + 1, n].wall == 27 && (Main.tile[m, n - 1].wall == 27 || Main.tile[m, n - 1].HasTile) && (Main.tile[m, n + 1].wall == 27 || Main.tile[m, n + 1].HasTile))
				{
					Main.tile[m, n].active(active: false);
					Main.tile[m, n].wall = 27;
				}
			}
		}
		for (int num24 = num18; num24 < num19; num24++)
		{
			for (int num25 = num20; num25 < num21; num25++)
			{
				if (Main.tile[num24, num25].type == 30)
				{
					if (Main.tile[num24 - 1, num25].wall == 27 && Main.tile[num24 + 1, num25].wall == 27 && !Main.tile[num24 - 1, num25].HasTile && !Main.tile[num24 + 1, num25].HasTile)
					{
						Main.tile[num24, num25].active(active: false);
						Main.tile[num24, num25].wall = 27;
					}
					if (Main.tile[num24, num25 - 1].type != 21 && Main.tile[num24 - 1, num25].wall == 27 && Main.tile[num24 + 1, num25].type == 30 && Main.tile[num24 + 2, num25].wall == 27 && !Main.tile[num24 - 1, num25].HasTile && !Main.tile[num24 + 2, num25].HasTile)
					{
						Main.tile[num24, num25].active(active: false);
						Main.tile[num24, num25].wall = 27;
						Main.tile[num24 + 1, num25].active(active: false);
						Main.tile[num24 + 1, num25].wall = 27;
					}
					if (Main.tile[num24, num25 - 1].wall == 27 && Main.tile[num24, num25 + 1].wall == 27 && !Main.tile[num24, num25 - 1].HasTile && !Main.tile[num24, num25 + 1].HasTile)
					{
						Main.tile[num24, num25].active(active: false);
						Main.tile[num24, num25].wall = 27;
					}
				}
			}
		}
		for (int num26 = num18; num26 < num19; num26++)
		{
			for (int num27 = num21; num27 > num20; num27--)
			{
				bool flag4 = false;
				if (Main.tile[num26, num27].HasTile && Main.tile[num26, num27].type == 30)
				{
					int num28 = -1;
					for (int num29 = 0; num29 < 2; num29++)
					{
						if (!SolidTile(num26 + num28, num27) && Main.tile[num26 + num28, num27].wall == 0)
						{
							int num30 = 0;
							int num31 = num27;
							int num32 = num27;
							while (Main.tile[num26, num31].HasTile && Main.tile[num26, num31].type == 30 && !SolidTile(num26 + num28, num31) && Main.tile[num26 + num28, num31].wall == 0)
							{
								num31--;
								num30++;
							}
							num31++;
							int num33 = num31 + 1;
							if (num30 > 4)
							{
								if (WorldGen.genRand.Next(2) == 0)
								{
									num31 = num32 - 1;
									bool flag5 = true;
									for (int num34 = num26 - 2; num34 <= num26 + 2; num34++)
									{
										for (int num35 = num31 - 2; num35 <= num31; num35++)
										{
											if (num34 != num26 && Main.tile[num34, num35].HasTile)
											{
												flag5 = false;
											}
										}
									}
									if (flag5)
									{
										Main.tile[num26, num31].active(active: false);
										Main.tile[num26, num31 - 1].active(active: false);
										Main.tile[num26, num31 - 2].active(active: false);
										PlaceTile(num26, num31, 10, mute: true);
										flag4 = true;
									}
								}
								if (!flag4)
								{
									for (int num36 = num33; num36 < num32; num36++)
									{
										Main.tile[num26, num36].type = 124;
									}
								}
							}
						}
						num28 = 1;
					}
				}
				if (flag4)
				{
					break;
				}
			}
		}
		int num37;
		for (num37 = num18; num37 < num19; num37++)
		{
			bool flag6 = true;
			for (int num38 = num20; num38 < num21; num38++)
			{
				for (int num39 = num37 - 2; num39 <= num37 + 2; num39++)
				{
					if (Main.tile[num39, num38].HasTile && (!SolidTile(num39, num38) || Main.tile[num39, num38].type == 10))
					{
						flag6 = false;
					}
				}
			}
			if (flag6)
			{
				for (int num40 = num20; num40 < num21; num40++)
				{
					if (Main.tile[num37, num40].wall == 27 && !Main.tile[num37, num40].HasTile)
					{
						PlaceTile(num37, num40, 124, mute: true);
					}
				}
			}
			num37 += WorldGen.genRand.Next(3);
		}
		for (int num41 = 0; num41 < 4; num41++)
		{
			int num42 = WorldGen.genRand.Next(num18 + 2, num19 - 1);
			int num43 = WorldGen.genRand.Next(num20 + 2, num21 - 1);
			while (Main.tile[num42, num43].wall != 27)
			{
				num42 = WorldGen.genRand.Next(num18 + 2, num19 - 1);
				num43 = WorldGen.genRand.Next(num20 + 2, num21 - 1);
			}
			while (Main.tile[num42, num43].HasTile)
			{
				num43--;
			}
			for (; !Main.tile[num42, num43].HasTile; num43++)
			{
			}
			num43--;
			if (Main.tile[num42, num43].wall != 27)
			{
				continue;
			}
			if (WorldGen.genRand.Next(3) == 0)
			{
				int num44 = WorldGen.genRand.Next(9);
				if (num44 == 0)
				{
					num44 = 14;
				}
				if (num44 == 1)
				{
					num44 = 16;
				}
				if (num44 == 2)
				{
					num44 = 18;
				}
				if (num44 == 3)
				{
					num44 = 86;
				}
				if (num44 == 4)
				{
					num44 = 87;
				}
				if (num44 == 5)
				{
					num44 = 94;
				}
				if (num44 == 6)
				{
					num44 = 101;
				}
				if (num44 == 7)
				{
					num44 = 104;
				}
				if (num44 == 8)
				{
					num44 = 106;
				}
				PlaceTile(num42, num43, num44, mute: true);
			}
			else
			{
				int style = WorldGen.genRand.Next(2, 43);
				PlaceTile(num42, num43, 105, mute: true, forced: true, -1, style);
			}
		}
	}

	public static void PlaceWall(int i, int j, int type, bool mute = false)
	{
		if (i > 1 && j > 1 && i < Main.maxTilesX - 2 && j < Main.maxTilesY - 2)
		{
			if (Main.tile[i, j] == null)
			{
				Main.tile[i, j] = default(Tile);
			}
			if (Main.tile[i, j].wall == 0)
			{
				Main.tile[i, j].wall = (byte)type;
				SquareWallFrame(i, j);
			}
		}
	}

	public static void AddPlants()
	{
		for (int i = 0; i < Main.maxTilesX; i++)
		{
			for (int j = 1; j < Main.maxTilesY; j++)
			{
				if (Main.tile[i, j].type == 2 && Main.tile[i, j].HasTile)
				{
					if (!Main.tile[i, j - 1].HasTile)
					{
						PlaceTile(i, j - 1, 3, mute: true);
					}
				}
				else if (Main.tile[i, j].type == 23 && Main.tile[i, j].HasTile && !Main.tile[i, j - 1].HasTile)
				{
					PlaceTile(i, j - 1, 24, mute: true);
				}
			}
		}
	}

	public static void SpreadGrass(int i, int j, int dirt = 0, int grass = 2, bool repeat = true)
	{
		try
		{
			if (Main.tile[i, j].type != dirt || !Main.tile[i, j].HasTile || ((double)j < Main.worldSurface && grass == 70) || ((double)j >= Main.worldSurface && dirt == 0))
			{
				return;
			}
			int num = i - 1;
			int num2 = i + 2;
			int num3 = j - 1;
			int num4 = j + 2;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 > Main.maxTilesX)
			{
				num2 = Main.maxTilesX;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num4 > Main.maxTilesY)
			{
				num4 = Main.maxTilesY;
			}
			bool flag = true;
			for (int k = num; k < num2; k++)
			{
				for (int l = num3; l < num4; l++)
				{
					if (!Main.tile[k, l].HasTile || !Main.tileSolid[Main.tile[k, l].type])
					{
						flag = false;
					}
					if (Main.tile[k, l].lava() && Main.tile[k, l].liquid > 0)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag || (grass == 23 && Main.tile[i, j - 1].type == 27))
			{
				return;
			}
			Main.tile[i, j].type = (byte)grass;
			for (int m = num; m < num2; m++)
			{
				for (int n = num3; n < num4; n++)
				{
					if (!Main.tile[m, n].HasTile || Main.tile[m, n].type != dirt)
					{
						continue;
					}
					try
					{
						if (repeat && WorldGen.grassSpread < 1000)
						{
							WorldGen.grassSpread++;
							SpreadGrass(m, n, dirt, grass);
							WorldGen.grassSpread--;
						}
					}
					catch
					{
					}
				}
			}
		}
		catch
		{
		}
	}

	public static void ChasmRunnerSideways(int i, int j, int direction, int steps)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_070d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		float num = steps;
		Vector2 vector = new Vector2
		{
			X = i,
			Y = j
		};
		Vector2 vector2 = new Vector2
		{
			X = (float)WorldGen.genRand.Next(10, 21) * 0.1f * (float)direction,
			Y = (float)WorldGen.genRand.Next(-10, 10) * 0.01f
		};
		double num2 = WorldGen.genRand.Next(5) + 7;
		while (num2 > 0.0)
		{
			if (num > 0f)
			{
				num2 += (double)WorldGen.genRand.Next(3);
				num2 -= (double)WorldGen.genRand.Next(3);
				if (num2 < 7.0)
				{
					num2 = 7.0;
				}
				if (num2 > 20.0)
				{
					num2 = 20.0;
				}
				if (num == 1f && num2 < 10.0)
				{
					num2 = 10.0;
				}
			}
			else
			{
				num2 -= (double)WorldGen.genRand.Next(4);
			}
			if ((double)vector.Y > Main.rockLayer && num > 0f)
			{
				num = 0f;
			}
			num--;
			int num3 = (int)((double)vector.X - num2 * 0.5);
			int num4 = (int)((double)vector.X + num2 * 0.5);
			int num5 = (int)((double)vector.Y - num2 * 0.5);
			int num6 = (int)((double)vector.Y + num2 * 0.5);
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num4 > Main.maxTilesX - 1)
			{
				num4 = Main.maxTilesX - 1;
			}
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.maxTilesY)
			{
				num6 = Main.maxTilesY;
			}
			for (int k = num3; k < num4; k++)
			{
				for (int l = num5; l < num6; l++)
				{
					if ((double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < num2 * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015) && Main.tile[k, l].type != 31 && Main.tile[k, l].type != 22)
					{
						Main.tile[k, l].active(active: false);
					}
				}
			}
			vector += vector2;
			vector2.Y += (float)WorldGen.genRand.Next(-10, 10) * 0.1f;
			if (vector.Y < (float)(j - 20))
			{
				vector2.Y += (float)WorldGen.genRand.Next(20) * 0.01f;
			}
			if (vector.Y > (float)(j + 20))
			{
				vector2.Y -= (float)WorldGen.genRand.Next(20) * 0.01f;
			}
			if ((double)vector2.Y < -0.5)
			{
				vector2.Y = -0.5f;
			}
			if ((double)vector2.Y > 0.5)
			{
				vector2.Y = 0.5f;
			}
			vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.01f;
			switch (direction)
			{
			case -1:
				if ((double)vector2.X > -0.5)
				{
					vector2.X = -0.5f;
				}
				if (vector2.X < -2f)
				{
					vector2.X = -2f;
				}
				break;
			case 1:
				if ((double)vector2.X < 0.5)
				{
					vector2.X = 0.5f;
				}
				if (vector2.X > 2f)
				{
					vector2.X = 2f;
				}
				break;
			}
			num3 = (int)((double)vector.X - num2 * 1.1);
			num4 = (int)((double)vector.X + num2 * 1.1);
			num5 = (int)((double)vector.Y - num2 * 1.1);
			num6 = (int)((double)vector.Y + num2 * 1.1);
			if (num3 < 1)
			{
				num3 = 1;
			}
			if (num4 > Main.maxTilesX - 1)
			{
				num4 = Main.maxTilesX - 1;
			}
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.maxTilesY)
			{
				num6 = Main.maxTilesY;
			}
			for (int m = num3; m < num4; m++)
			{
				for (int n = num5; n < num6; n++)
				{
					if ((double)(Math.Abs((float)m - vector.X) + Math.Abs((float)n - vector.Y)) < num2 * 1.1 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015) && Main.tile[m, n].wall != 3)
					{
						if (Main.tile[m, n].type != 25 && n > j + WorldGen.genRand.Next(3, 20))
						{
							Main.tile[m, n].active(active: true);
						}
						Main.tile[m, n].active(active: true);
						if (Main.tile[m, n].type != 31 && Main.tile[m, n].type != 22)
						{
							Main.tile[m, n].type = 25;
						}
						if (Main.tile[m, n].wall == 2)
						{
							Main.tile[m, n].wall = 0;
						}
					}
				}
			}
			for (int num7 = num3; num7 < num4; num7++)
			{
				for (int num8 = num5; num8 < num6; num8++)
				{
					if ((double)(Math.Abs((float)num7 - vector.X) + Math.Abs((float)num8 - vector.Y)) < num2 * 1.1 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015) && Main.tile[num7, num8].wall != 3)
					{
						if (Main.tile[num7, num8].type != 31 && Main.tile[num7, num8].type != 22)
						{
							Main.tile[num7, num8].type = 25;
						}
						Main.tile[num7, num8].active(active: true);
						PlaceWall(num7, num8, 3, mute: true);
					}
				}
			}
		}
		if (WorldGen.genRand.Next(3) == 0)
		{
			int num9 = (int)vector.X;
			int num10;
			for (num10 = (int)vector.Y; !Main.tile[num9, num10].HasTile; num10++)
			{
			}
			TileRunner(num9, num10, WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(3, 7), 22);
		}
	}

	public static void ChasmRunner(int i, int j, int steps, bool makeOrb = false)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (!makeOrb)
		{
			flag2 = true;
		}
		float num = steps;
		Vector2 vector = new Vector2
		{
			X = i,
			Y = j
		};
		Vector2 vector2 = new Vector2
		{
			X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f,
			Y = (float)WorldGen.genRand.Next(11) * 0.2f + 0.5f
		};
		int num2 = 5;
		double num3 = WorldGen.genRand.Next(5) + 7;
		while (num3 > 0.0)
		{
			if (num > 0f)
			{
				num3 += (double)WorldGen.genRand.Next(3);
				num3 -= (double)WorldGen.genRand.Next(3);
				if (num3 < 7.0)
				{
					num3 = 7.0;
				}
				if (num3 > 20.0)
				{
					num3 = 20.0;
				}
				if (num == 1f && num3 < 10.0)
				{
					num3 = 10.0;
				}
			}
			else if ((double)vector.Y > Main.worldSurface + 45.0)
			{
				num3 -= (double)WorldGen.genRand.Next(4);
			}
			if ((double)vector.Y > Main.rockLayer && num > 0f)
			{
				num = 0f;
			}
			num--;
			if (!flag && (double)vector.Y > Main.worldSurface + 20.0)
			{
				flag = true;
				ChasmRunnerSideways((int)vector.X, (int)vector.Y, -1, WorldGen.genRand.Next(20, 40));
				ChasmRunnerSideways((int)vector.X, (int)vector.Y, 1, WorldGen.genRand.Next(20, 40));
			}
			int num4;
			int num5;
			int num6;
			int num7;
			if (num > (float)num2)
			{
				num4 = (int)((double)vector.X - num3 * 0.5);
				num5 = (int)((double)vector.X + num3 * 0.5);
				num6 = (int)((double)vector.Y - num3 * 0.5);
				num7 = (int)((double)vector.Y + num3 * 0.5);
				if (num4 < 0)
				{
					num4 = 0;
				}
				if (num5 > Main.maxTilesX - 1)
				{
					num5 = Main.maxTilesX - 1;
				}
				if (num6 < 0)
				{
					num6 = 0;
				}
				if (num7 > Main.maxTilesY)
				{
					num7 = Main.maxTilesY;
				}
				for (int k = num4; k < num5; k++)
				{
					for (int l = num6; l < num7; l++)
					{
						if ((double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < num3 * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015) && Main.tile[k, l].type != 31 && Main.tile[k, l].type != 22)
						{
							Main.tile[k, l].active(active: false);
						}
					}
				}
			}
			if (num <= 2f && (double)vector.Y < Main.worldSurface + 45.0)
			{
				num = 2f;
			}
			if (num <= 0f)
			{
				if (!flag2)
				{
					flag2 = true;
					AddShadowOrb((int)vector.X, (int)vector.Y);
				}
				else if (!flag3)
				{
					flag3 = false;
					bool flag4 = false;
					int num8 = 0;
					while (!flag4)
					{
						int num9 = WorldGen.genRand.Next((int)vector.X - 25, (int)vector.X + 25);
						int num10 = WorldGen.genRand.Next((int)vector.Y - 50, (int)vector.Y);
						if (num9 < 5)
						{
							num9 = 5;
						}
						if (num9 > Main.maxTilesX - 5)
						{
							num9 = Main.maxTilesX - 5;
						}
						if (num10 < 5)
						{
							num10 = 5;
						}
						if (num10 > Main.maxTilesY - 5)
						{
							num10 = Main.maxTilesY - 5;
						}
						if ((double)num10 > Main.worldSurface)
						{
							Place3x2(num9, num10, 26);
							if (Main.tile[num9, num10].type == 26)
							{
								flag4 = true;
								continue;
							}
							num8++;
							if (num8 >= 10000)
							{
								flag4 = true;
							}
						}
						else
						{
							flag4 = true;
						}
					}
				}
			}
			vector += vector2;
			vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.01f;
			if ((double)vector2.X > 0.3)
			{
				vector2.X = 0.3f;
			}
			if ((double)vector2.X < -0.3)
			{
				vector2.X = -0.3f;
			}
			num4 = (int)((double)vector.X - num3 * 1.1);
			num5 = (int)((double)vector.X + num3 * 1.1);
			num6 = (int)((double)vector.Y - num3 * 1.1);
			num7 = (int)((double)vector.Y + num3 * 1.1);
			if (num4 < 1)
			{
				num4 = 1;
			}
			if (num5 > Main.maxTilesX - 1)
			{
				num5 = Main.maxTilesX - 1;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.maxTilesY)
			{
				num7 = Main.maxTilesY;
			}
			for (int m = num4; m < num5; m++)
			{
				for (int n = num6; n < num7; n++)
				{
					if ((double)(Math.Abs((float)m - vector.X) + Math.Abs((float)n - vector.Y)) < num3 * 1.1 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
					{
						if (Main.tile[m, n].type != 25 && n > j + WorldGen.genRand.Next(3, 20))
						{
							Main.tile[m, n].active(active: true);
						}
						if (steps <= num2)
						{
							Main.tile[m, n].active(active: true);
						}
						if (Main.tile[m, n].type != 31)
						{
							Main.tile[m, n].type = 25;
						}
						if (Main.tile[m, n].wall == 2)
						{
							Main.tile[m, n].wall = 0;
						}
					}
				}
			}
			for (int num11 = num4; num11 < num5; num11++)
			{
				for (int num12 = num6; num12 < num7; num12++)
				{
					if ((double)(Math.Abs((float)num11 - vector.X) + Math.Abs((float)num12 - vector.Y)) < num3 * 1.1 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
					{
						if (Main.tile[num11, num12].type != 31)
						{
							Main.tile[num11, num12].type = 25;
						}
						if (steps <= num2)
						{
							Main.tile[num11, num12].active(active: true);
						}
						if (num12 > j + WorldGen.genRand.Next(3, 20))
						{
							PlaceWall(num11, num12, 3, mute: true);
						}
					}
				}
			}
		}
	}

	public static void JungleRunner(int i, int j)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		double num = WorldGen.genRand.Next(5, 11);
		Vector2 vector = new Vector2
		{
			X = i,
			Y = j
		};
		Vector2 vector2 = new Vector2
		{
			X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f,
			Y = (float)WorldGen.genRand.Next(10, 20) * 0.1f
		};
		int num2 = 0;
		bool flag = true;
		while (flag)
		{
			if ((double)vector.Y < Main.worldSurface)
			{
				int num3 = (int)vector.X;
				int num4 = (int)vector.Y;
				if (Main.tile[num3, num4].wall == 0 && !Main.tile[num3, num4].HasTile && Main.tile[num3, num4 - 3].wall == 0 && !Main.tile[num3, num4 - 3].HasTile && Main.tile[num3, num4 - 1].wall == 0 && !Main.tile[num3, num4 - 1].HasTile && Main.tile[num3, num4 - 4].wall == 0 && !Main.tile[num3, num4 - 4].HasTile && Main.tile[num3, num4 - 2].wall == 0 && !Main.tile[num3, num4 - 2].HasTile && Main.tile[num3, num4 - 5].wall == 0 && !Main.tile[num3, num4 - 5].HasTile)
				{
					flag = false;
				}
			}
			JungleX = (int)vector.X;
			num += (double)((float)WorldGen.genRand.Next(-20, 21) * 0.1f);
			if (num < 5.0)
			{
				num = 5.0;
			}
			if (num > 10.0)
			{
				num = 10.0;
			}
			int num5 = (int)((double)vector.X - num * 0.5);
			int num6 = (int)((double)vector.X + num * 0.5);
			int num7 = (int)((double)vector.Y - num * 0.5);
			int num8 = (int)((double)vector.Y + num * 0.5);
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.maxTilesX)
			{
				num6 = Main.maxTilesX;
			}
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.maxTilesY)
			{
				num8 = Main.maxTilesY;
			}
			for (int k = num5; k < num6; k++)
			{
				for (int l = num7; l < num8; l++)
				{
					if ((double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < num * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
					{
						KillTile(k, l);
					}
				}
			}
			num2++;
			if (num2 > 10 && WorldGen.genRand.Next(50) < num2)
			{
				num2 = 0;
				int num9 = -2;
				if (WorldGen.genRand.Next(2) == 0)
				{
					num9 = 2;
				}
				TileRunner((int)vector.X, (int)vector.Y, WorldGen.genRand.Next(3, 20), WorldGen.genRand.Next(10, 100), -1, addTile: false, num9);
			}
			vector += vector2;
			vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.01f;
			if (vector2.Y > 0f)
			{
				vector2.Y = 0f;
			}
			if (vector2.Y < -2f)
			{
				vector2.Y = -2f;
			}
			vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
			if (vector.X < (float)(i - 200))
			{
				vector2.X += (float)WorldGen.genRand.Next(5, 21) * 0.1f;
			}
			if (vector.X > (float)(i + 200))
			{
				vector2.X -= (float)WorldGen.genRand.Next(5, 21) * 0.1f;
			}
			if ((double)vector2.X > 1.5)
			{
				vector2.X = 1.5f;
			}
			if ((double)vector2.X < -1.5)
			{
				vector2.X = -1.5f;
			}
		}
	}

	public static void GERunner(int i, int j, float speedX = 0f, float speedY = 0f, bool good = true)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		int num = WorldGen.genRand.Next(200, 250);
		float num2 = Main.maxTilesX / 4200;
		num = (int)((float)num * num2);
		double num3 = num;
		Vector2 vector = new Vector2
		{
			X = i,
			Y = j
		};
		Vector2 vector2 = new Vector2
		{
			X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f,
			Y = (float)WorldGen.genRand.Next(-10, 11) * 0.1f
		};
		if (speedX != 0f || speedY != 0f)
		{
			vector2.X = speedX;
			vector2.Y = speedY;
		}
		bool flag = true;
		while (flag)
		{
			int num4 = (int)((double)vector.X - num3 * 0.5);
			int num5 = (int)((double)vector.X + num3 * 0.5);
			int num6 = (int)((double)vector.Y - num3 * 0.5);
			int num7 = (int)((double)vector.Y + num3 * 0.5);
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 > Main.maxTilesX)
			{
				num5 = Main.maxTilesX;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.maxTilesY)
			{
				num7 = Main.maxTilesY;
			}
			for (int k = num4; k < num5; k++)
			{
				for (int l = num6; l < num7; l++)
				{
					if (!((double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < (double)num * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015)))
					{
						continue;
					}
					if (good)
					{
						if (Main.tile[k, l].wall == 3)
						{
							Main.tile[k, l].wall = 28;
						}
						if (Main.tile[k, l].type == 2)
						{
							Main.tile[k, l].type = 109;
							SquareTileFrame(k, l);
						}
						else if (Main.tile[k, l].type == 1)
						{
							Main.tile[k, l].type = 117;
							SquareTileFrame(k, l);
						}
						else if (Main.tile[k, l].type == 53 || Main.tile[k, l].type == 123)
						{
							Main.tile[k, l].type = 116;
							SquareTileFrame(k, l);
						}
						else if (Main.tile[k, l].type == 23)
						{
							Main.tile[k, l].type = 109;
							SquareTileFrame(k, l);
						}
						else if (Main.tile[k, l].type == 25)
						{
							Main.tile[k, l].type = 117;
							SquareTileFrame(k, l);
						}
						else if (Main.tile[k, l].type == 112)
						{
							Main.tile[k, l].type = 116;
							SquareTileFrame(k, l);
						}
					}
					else if (Main.tile[k, l].type == 2)
					{
						Main.tile[k, l].type = 23;
						SquareTileFrame(k, l);
					}
					else if (Main.tile[k, l].type == 1)
					{
						Main.tile[k, l].type = 25;
						SquareTileFrame(k, l);
					}
					else if (Main.tile[k, l].type == 53 || Main.tile[k, l].type == 123)
					{
						Main.tile[k, l].type = 112;
						SquareTileFrame(k, l);
					}
					else if (Main.tile[k, l].type == 109)
					{
						Main.tile[k, l].type = 23;
						SquareTileFrame(k, l);
					}
					else if (Main.tile[k, l].type == 117)
					{
						Main.tile[k, l].type = 25;
						SquareTileFrame(k, l);
					}
					else if (Main.tile[k, l].type == 116)
					{
						Main.tile[k, l].type = 112;
						SquareTileFrame(k, l);
					}
				}
			}
			vector += vector2;
			vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			if (vector2.X > speedX + 1f)
			{
				vector2.X = speedX + 1f;
			}
			if (vector2.X < speedX - 1f)
			{
				vector2.X = speedX - 1f;
			}
			if (vector.X < (float)(-num) || vector.Y < (float)(-num) || vector.X > (float)(Main.maxTilesX + num) || vector.Y > (float)(Main.maxTilesX + num))
			{
				flag = false;
			}
		}
	}

	public static void TileRunner(int i, int j, double strength, int steps, int type, bool addTile = false, float speedX = 0f, float speedY = 0f, bool noYChange = false, bool overRide = true)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_0767: Unknown result type (might be due to invalid IL or missing references)
		//IL_0768: Unknown result type (might be due to invalid IL or missing references)
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0883: Unknown result type (might be due to invalid IL or missing references)
		//IL_0884: Unknown result type (might be due to invalid IL or missing references)
		//IL_0885: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08df: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
		double num = strength;
		float num2 = steps;
		Vector2 vector = new Vector2
		{
			X = i,
			Y = j
		};
		Vector2 vector2 = new Vector2
		{
			X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f,
			Y = (float)WorldGen.genRand.Next(-10, 11) * 0.1f
		};
		if (speedX != 0f || speedY != 0f)
		{
			vector2.X = speedX;
			vector2.Y = speedY;
		}
		while (num > 0.0 && num2 > 0f)
		{
			if (vector.Y < 0f && num2 > 0f && type == 59)
			{
				num2 = 0f;
			}
			num = strength * (double)(num2 / (float)steps);
			num2--;
			int num3 = (int)((double)vector.X - num * 0.5);
			int num4 = (int)((double)vector.X + num * 0.5);
			int num5 = (int)((double)vector.Y - num * 0.5);
			int num6 = (int)((double)vector.Y + num * 0.5);
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num4 > Main.maxTilesX)
			{
				num4 = Main.maxTilesX;
			}
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.maxTilesY)
			{
				num6 = Main.maxTilesY;
			}
			for (int k = num3; k < num4; k++)
			{
				for (int l = num5; l < num6; l++)
				{
					if (!((double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < strength * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015)))
					{
						continue;
					}
					if (mudWall && (double)l > Main.worldSurface && l < Main.maxTilesY - 210 - WorldGen.genRand.Next(3))
					{
						PlaceWall(k, l, 15, mute: true);
					}
					if (type < 0)
					{
						if (type == -2 && Main.tile[k, l].HasTile && (l < waterLine || l > lavaLine))
						{
							Main.tile[k, l].liquid = byte.MaxValue;
							if (l > lavaLine)
							{
								Main.tile[k, l].lava(lava: true);
							}
						}
						Main.tile[k, l].active(active: false);
						continue;
					}
					if ((overRide || !Main.tile[k, l].HasTile) && (type != 40 || Main.tile[k, l].type != 53) && (!Main.tileStone[type] || Main.tile[k, l].type == 1) && Main.tile[k, l].type != 45 && Main.tile[k, l].type != 147 && (Main.tile[k, l].type != 1 || type != 59 || !((double)l < Main.worldSurface + (double)WorldGen.genRand.Next(-50, 50))))
					{
						if (Main.tile[k, l].type != 53 || !((double)l < Main.worldSurface))
						{
							Main.tile[k, l].type = (byte)type;
						}
						else if (type == 59)
						{
							Main.tile[k, l].type = (byte)type;
						}
					}
					if (addTile)
					{
						Main.tile[k, l].active(active: true);
						Main.tile[k, l].liquid = 0;
						Main.tile[k, l].lava(lava: false);
					}
					if (noYChange && (double)l < Main.worldSurface && type != 59)
					{
						Main.tile[k, l].wall = 2;
					}
					if (type == 59 && l > waterLine && Main.tile[k, l].liquid > 0)
					{
						Main.tile[k, l].lava(lava: false);
						Main.tile[k, l].liquid = 0;
					}
				}
			}
			vector += vector2;
			if (num > 50.0)
			{
				vector += vector2;
				num2--;
				vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
				vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
				if (num > 100.0)
				{
					vector += vector2;
					num2--;
					vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
					vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
					if (num > 150.0)
					{
						vector += vector2;
						num2--;
						vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
						vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
						if (num > 200.0)
						{
							vector += vector2;
							num2--;
							vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
							vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
							if (num > 250.0)
							{
								vector += vector2;
								num2--;
								vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
								vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
								if (num > 300.0)
								{
									vector += vector2;
									num2--;
									vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
									vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
									if (num > 400.0)
									{
										vector += vector2;
										num2--;
										vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
										vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
										if (num > 500.0)
										{
											vector += vector2;
											num2--;
											vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
											vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
											if (num > 600.0)
											{
												vector += vector2;
												num2--;
												vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
												vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
												if (num > 700.0)
												{
													vector += vector2;
													num2--;
													vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
													vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
													if (num > 800.0)
													{
														vector += vector2;
														num2--;
														vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
														vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
														if (num > 900.0)
														{
															vector += vector2;
															num2--;
															vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
															vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			if (vector2.X > 1f)
			{
				vector2.X = 1f;
			}
			if (vector2.X < -1f)
			{
				vector2.X = -1f;
			}
			if (!noYChange)
			{
				vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
				if (vector2.Y > 1f)
				{
					vector2.Y = 1f;
				}
				if (vector2.Y < -1f)
				{
					vector2.Y = -1f;
				}
			}
			else if (type != 59 && num < 3.0)
			{
				if (vector2.Y > 1f)
				{
					vector2.Y = 1f;
				}
				if (vector2.Y < -1f)
				{
					vector2.Y = -1f;
				}
			}
			if (type == 59 && !noYChange)
			{
				if ((double)vector2.Y > 0.5)
				{
					vector2.Y = 0.5f;
				}
				if ((double)vector2.Y < -0.5)
				{
					vector2.Y = -0.5f;
				}
				if ((double)vector.Y < Main.rockLayer + 100.0)
				{
					vector2.Y = 1f;
				}
				if (vector.Y > (float)(Main.maxTilesY - 300))
				{
					vector2.Y = -1f;
				}
			}
		}
	}

	public static void MudWallRunner(int i, int j)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		double num = WorldGen.genRand.Next(5, 15);
		float num2 = WorldGen.genRand.Next(5, 20);
		float num3 = num2;
		Vector2 vector = new Vector2
		{
			X = i,
			Y = j
		};
		Vector2 vector2 = new Vector2
		{
			X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f,
			Y = (float)WorldGen.genRand.Next(-10, 11) * 0.1f
		};
		while (num > 0.0 && num3 > 0f)
		{
			double num4 = num * (double)(num3 / num2);
			num3--;
			int num5 = (int)((double)vector.X - num4 * 0.5);
			int num6 = (int)((double)vector.X + num4 * 0.5);
			int num7 = (int)((double)vector.Y - num4 * 0.5);
			int num8 = (int)((double)vector.Y + num4 * 0.5);
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.maxTilesX)
			{
				num6 = Main.maxTilesX;
			}
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.maxTilesY)
			{
				num8 = Main.maxTilesY;
			}
			for (int k = num5; k < num6; k++)
			{
				for (int l = num7; l < num8; l++)
				{
					if ((double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < num * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
					{
						Main.tile[k, l].wall = 0;
					}
				}
			}
			vector += vector2;
			vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			if (vector2.X > 1f)
			{
				vector2.X = 1f;
			}
			if (vector2.X < -1f)
			{
				vector2.X = -1f;
			}
			vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			if (vector2.Y > 1f)
			{
				vector2.Y = 1f;
			}
			if (vector2.Y < -1f)
			{
				vector2.Y = -1f;
			}
		}
	}

	public static void FloatingIsland(int i, int j)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		double num = WorldGen.genRand.Next(80, 120);
		double num2 = num;
		float num3 = WorldGen.genRand.Next(20, 25);
		Vector2 vector = new Vector2
		{
			X = i,
			Y = j
		};
		Vector2 vector2 = new Vector2
		{
			X = (float)WorldGen.genRand.Next(-20, 21) * 0.2f
		};
		while (vector2.X > -2f && vector2.X < 2f)
		{
			vector2.X = (float)WorldGen.genRand.Next(-20, 21) * 0.2f;
		}
		vector2.Y = (float)WorldGen.genRand.Next(-20, -10) * 0.02f;
		while (num > 0.0 && num3 > 0f)
		{
			num -= (double)WorldGen.genRand.Next(4);
			num3--;
			int num4 = (int)((double)vector.X - num * 0.5);
			int num5 = (int)((double)vector.X + num * 0.5);
			int num6 = (int)((double)vector.Y - num * 0.5);
			int num7 = (int)((double)vector.Y + num * 0.5);
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 > Main.maxTilesX)
			{
				num5 = Main.maxTilesX;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.maxTilesY)
			{
				num7 = Main.maxTilesY;
			}
			num2 = num * (double)WorldGen.genRand.Next(80, 120) * 0.01;
			float num8 = vector.Y + 1f;
			for (int k = num4; k < num5; k++)
			{
				if (WorldGen.genRand.Next(2) == 0)
				{
					num8 += (float)WorldGen.genRand.Next(-1, 2);
				}
				if (num8 < vector.Y)
				{
					num8 = vector.Y;
				}
				if (num8 > vector.Y + 2f)
				{
					num8 = vector.Y + 2f;
				}
				for (int l = num6; l < num7; l++)
				{
					if (!((float)l > num8))
					{
						continue;
					}
					float num9 = Math.Abs((float)k - vector.X);
					float num10 = Math.Abs((float)l - vector.Y) * 2f;
					if (Math.Sqrt(num9 * num9 + num10 * num10) < num2 * 0.4)
					{
						Main.tile[k, l].active(active: true);
						if (Main.tile[k, l].type == 59)
						{
							Main.tile[k, l].type = 0;
						}
					}
				}
			}
			TileRunner(WorldGen.genRand.Next(num4 + 10, num5 - 10), (int)((double)vector.Y + num2 * 0.1 + 5.0), WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(10, 15), 0, addTile: true, 0f, 2f, noYChange: true);
			num4 = (int)((double)vector.X - num * 0.4);
			num5 = (int)((double)vector.X + num * 0.4);
			num6 = (int)((double)vector.Y - num * 0.4);
			num7 = (int)((double)vector.Y + num * 0.4);
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 > Main.maxTilesX)
			{
				num5 = Main.maxTilesX;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.maxTilesY)
			{
				num7 = Main.maxTilesY;
			}
			num2 = num * (double)WorldGen.genRand.Next(80, 120) * 0.01;
			for (int m = num4; m < num5; m++)
			{
				for (int n = num6; n < num7; n++)
				{
					if ((float)n > vector.Y + 2f)
					{
						float num11 = Math.Abs((float)m - vector.X);
						float num12 = Math.Abs((float)n - vector.Y) * 2f;
						if (Math.Sqrt(num11 * num11 + num12 * num12) < num2 * 0.4)
						{
							Main.tile[m, n].wall = 2;
						}
					}
				}
			}
			vector += vector2;
			vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			if (vector2.X > 1f)
			{
				vector2.X = 1f;
			}
			if (vector2.X < -1f)
			{
				vector2.X = -1f;
			}
			if ((double)vector2.Y > 0.2)
			{
				vector2.Y = -0.2f;
			}
			if ((double)vector2.Y < -0.2)
			{
				vector2.Y = -0.2f;
			}
		}
	}

	public static void Caverer(int X, int Y)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		switch (WorldGen.genRand.Next(2))
		{
		case 0:
		{
			int num4 = WorldGen.genRand.Next(7, 9);
			float num5 = (float)WorldGen.genRand.Next(100) * 0.01f;
			float num6 = 1f - num5;
			if (WorldGen.genRand.Next(2) == 0)
			{
				num5 = 0f - num5;
			}
			if (WorldGen.genRand.Next(2) == 0)
			{
				num6 = 0f - num6;
			}
			Vector2 vector2 = default(Vector2);
			((Vector2)(ref vector2))._002Ector((float)X, (float)Y);
			for (int j = 0; j < num4; j++)
			{
				vector2 = digTunnel(vector2.X, vector2.Y, num5, num6, WorldGen.genRand.Next(6, 20), WorldGen.genRand.Next(4, 9));
				num5 += (float)WorldGen.genRand.Next(-20, 21) * 0.1f;
				num6 += (float)WorldGen.genRand.Next(-20, 21) * 0.1f;
				if ((double)num5 < -1.5)
				{
					num5 = -1.5f;
				}
				if ((double)num5 > 1.5)
				{
					num5 = 1.5f;
				}
				if ((double)num6 < -1.5)
				{
					num6 = -1.5f;
				}
				if ((double)num6 > 1.5)
				{
					num6 = 1.5f;
				}
				float num7 = (float)WorldGen.genRand.Next(100) * 0.01f;
				float num8 = 1f - num7;
				if (WorldGen.genRand.Next(2) == 0)
				{
					num7 = 0f - num7;
				}
				if (WorldGen.genRand.Next(2) == 0)
				{
					num8 = 0f - num8;
				}
				Vector2 vector3 = digTunnel(vector2.X, vector2.Y, num7, num8, WorldGen.genRand.Next(30, 50), WorldGen.genRand.Next(3, 6));
				TileRunner((int)vector3.X, (int)vector3.Y, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), -1);
			}
			break;
		}
		case 1:
		{
			int num = WorldGen.genRand.Next(15, 30);
			float num2 = (float)WorldGen.genRand.Next(100) * 0.01f;
			float num3 = 1f - num2;
			if (WorldGen.genRand.Next(2) == 0)
			{
				num2 = 0f - num2;
			}
			if (WorldGen.genRand.Next(2) == 0)
			{
				num3 = 0f - num3;
			}
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector((float)X, (float)Y);
			for (int i = 0; i < num; i++)
			{
				vector = digTunnel(vector.X, vector.Y, num2, num3, WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(2, 6), Wet: true);
				num2 += (float)WorldGen.genRand.Next(-20, 21) * 0.1f;
				num3 += (float)WorldGen.genRand.Next(-20, 21) * 0.1f;
				if ((double)num2 < -1.5)
				{
					num2 = -1.5f;
				}
				if ((double)num2 > 1.5)
				{
					num2 = 1.5f;
				}
				if ((double)num3 < -1.5)
				{
					num3 = -1.5f;
				}
				if ((double)num3 > 1.5)
				{
					num3 = 1.5f;
				}
			}
			break;
		}
		}
	}

	public static Vector2 digTunnel(float X, float Y, float xDir, float yDir, int Steps, int Size, bool Wet = false)
	{
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		float num = X;
		float num2 = Y;
		try
		{
			float num3 = 0f;
			float num4 = 0f;
			float num5 = Size;
			for (int i = 0; i < Steps; i++)
			{
				for (int j = (int)(num - num5); (float)j <= num + num5; j++)
				{
					for (int k = (int)(num2 - num5); (float)k <= num2 + num5; k++)
					{
						if ((double)(Math.Abs((float)j - num) + Math.Abs((float)k - num2)) < (double)num5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.005))
						{
							Main.tile[j, k].active(active: false);
							if (Wet)
							{
								Main.tile[j, k].liquid = byte.MaxValue;
							}
						}
					}
				}
				num5 += (float)WorldGen.genRand.Next(-50, 51) * 0.03f;
				if ((double)num5 < (double)Size * 0.6)
				{
					num5 = (float)Size * 0.6f;
				}
				if (num5 > (float)(Size * 2))
				{
					num5 = (float)Size * 2f;
				}
				num3 += (float)WorldGen.genRand.Next(-20, 21) * 0.01f;
				num4 += (float)WorldGen.genRand.Next(-20, 21) * 0.01f;
				if (num3 < -1f)
				{
					num3 = -1f;
				}
				if (num3 > 1f)
				{
					num3 = 1f;
				}
				if (num4 < -1f)
				{
					num4 = -1f;
				}
				if (num4 > 1f)
				{
					num4 = 1f;
				}
				num += (xDir + num3) * 0.6f;
				num2 += (yDir + num4) * 0.6f;
			}
		}
		catch
		{
		}
		return new Vector2(num, num2);
	}

	public static void IslandHouse(int i, int j)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		byte type = (byte)WorldGen.genRand.Next(45, 48);
		byte wall = (byte)WorldGen.genRand.Next(10, 13);
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)i, (float)j);
		int num = 1;
		if (WorldGen.genRand.Next(2) == 0)
		{
			num = -1;
		}
		int num2 = WorldGen.genRand.Next(7, 12);
		int num3 = WorldGen.genRand.Next(5, 7);
		vector.X = i + (num2 + 2) * num;
		for (int k = j - 15; k < j + 30; k++)
		{
			if (Main.tile[(int)vector.X, k].HasTile)
			{
				vector.Y = k - 1;
				break;
			}
		}
		vector.X = i;
		int num4 = (int)(vector.X - (float)num2 - 2f);
		int num5 = (int)(vector.X + (float)num2 + 2f);
		int num6 = (int)(vector.Y - (float)num3 - 2f);
		int num7 = (int)(vector.Y + 2f + (float)WorldGen.genRand.Next(3, 5));
		if (num4 < 0)
		{
			num4 = 0;
		}
		if (num5 > Main.maxTilesX)
		{
			num5 = Main.maxTilesX;
		}
		if (num6 < 0)
		{
			num6 = 0;
		}
		if (num7 > Main.maxTilesY)
		{
			num7 = Main.maxTilesY;
		}
		for (int l = num4; l <= num5; l++)
		{
			for (int m = num6; m < num7; m++)
			{
				Main.tile[l, m].active(active: true);
				Main.tile[l, m].type = type;
				Main.tile[l, m].wall = 0;
			}
		}
		num4 = (int)(vector.X - (float)num2);
		num5 = (int)(vector.X + (float)num2);
		num6 = (int)(vector.Y - (float)num3);
		num7 = (int)(vector.Y + 1f);
		if (num4 < 0)
		{
			num4 = 0;
		}
		if (num5 > Main.maxTilesX)
		{
			num5 = Main.maxTilesX;
		}
		if (num6 < 0)
		{
			num6 = 0;
		}
		if (num7 > Main.maxTilesY)
		{
			num7 = Main.maxTilesY;
		}
		for (int n = num4; n <= num5; n++)
		{
			for (int num8 = num6; num8 < num7; num8++)
			{
				if (Main.tile[n, num8].wall == 0)
				{
					Main.tile[n, num8].active(active: false);
					Main.tile[n, num8].wall = wall;
				}
			}
		}
		int num9 = i + (num2 + 1) * num;
		int num10 = (int)vector.Y;
		for (int num11 = num9 - 2; num11 <= num9 + 2; num11++)
		{
			Main.tile[num11, num10].active(active: false);
			Main.tile[num11, num10 - 1].active(active: false);
			Main.tile[num11, num10 - 2].active(active: false);
		}
		PlaceTile(num9, num10, 10, mute: true);
		int contain = 0;
		int num12 = houseCount;
		if (num12 > 2)
		{
			num12 = WorldGen.genRand.Next(3);
		}
		switch (num12)
		{
		case 0:
			contain = 159;
			break;
		case 1:
			contain = 65;
			break;
		case 2:
			contain = 158;
			break;
		}
		AddBuriedChest(i, num10 - 3, contain, notNearOtherChests: false, 2);
		houseCount++;
	}

	public static void Mountinater(int i, int j)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		double num = WorldGen.genRand.Next(80, 120);
		double num2 = num;
		float num3 = WorldGen.genRand.Next(40, 55);
		Vector2 vector = new Vector2
		{
			X = i,
			Y = (float)j + num3 / 2f
		};
		Vector2 vector2 = new Vector2
		{
			X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f,
			Y = (float)WorldGen.genRand.Next(-20, -10) * 0.1f
		};
		while (num > 0.0 && num3 > 0f)
		{
			num -= (double)WorldGen.genRand.Next(4);
			num3--;
			int num4 = (int)((double)vector.X - num * 0.5);
			int num5 = (int)((double)vector.X + num * 0.5);
			int num6 = (int)((double)vector.Y - num * 0.5);
			int num7 = (int)((double)vector.Y + num * 0.5);
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 > Main.maxTilesX)
			{
				num5 = Main.maxTilesX;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.maxTilesY)
			{
				num7 = Main.maxTilesY;
			}
			num2 = num * (double)WorldGen.genRand.Next(80, 120) * 0.01;
			for (int k = num4; k < num5; k++)
			{
				for (int l = num6; l < num7; l++)
				{
					float num8 = Math.Abs((float)k - vector.X);
					float num9 = Math.Abs((float)l - vector.Y);
					if (Math.Sqrt(num8 * num8 + num9 * num9) < num2 * 0.4 && !Main.tile[k, l].HasTile)
					{
						Main.tile[k, l].active(active: true);
						Main.tile[k, l].type = 0;
					}
				}
			}
			vector += vector2;
			vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			if ((double)vector2.X > 0.5)
			{
				vector2.X = 0.5f;
			}
			if ((double)vector2.X < -0.5)
			{
				vector2.X = -0.5f;
			}
			if ((double)vector2.Y > -0.5)
			{
				vector2.Y = -0.5f;
			}
			if ((double)vector2.Y < -1.5)
			{
				vector2.Y = -1.5f;
			}
		}
	}

	public static void Lakinater(int i, int j)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		double num = WorldGen.genRand.Next(25, 50);
		double num2 = num;
		float num3 = WorldGen.genRand.Next(30, 80);
		if (WorldGen.genRand.Next(5) == 0)
		{
			num *= 1.5;
			num2 *= 1.5;
			num3 *= 1.2f;
		}
		Vector2 vector = new Vector2
		{
			X = i,
			Y = (float)j - num3 * 0.3f
		};
		Vector2 vector2 = new Vector2
		{
			X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f,
			Y = (float)WorldGen.genRand.Next(-20, -10) * 0.1f
		};
		while (num > 0.0 && num3 > 0f)
		{
			if ((double)vector.Y + num2 * 0.5 > Main.worldSurface)
			{
				num3 = 0f;
			}
			num -= (double)WorldGen.genRand.Next(3);
			num3--;
			int num4 = (int)((double)vector.X - num * 0.5);
			int num5 = (int)((double)vector.X + num * 0.5);
			int num6 = (int)((double)vector.Y - num * 0.5);
			int num7 = (int)((double)vector.Y + num * 0.5);
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 > Main.maxTilesX)
			{
				num5 = Main.maxTilesX;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.maxTilesY)
			{
				num7 = Main.maxTilesY;
			}
			num2 = num * (double)WorldGen.genRand.Next(80, 120) * 0.01;
			for (int k = num4; k < num5; k++)
			{
				for (int l = num6; l < num7; l++)
				{
					float num8 = Math.Abs((float)k - vector.X);
					float num9 = Math.Abs((float)l - vector.Y);
					if (Math.Sqrt(num8 * num8 + num9 * num9) < num2 * 0.4)
					{
						if (Main.tile[k, l].HasTile)
						{
							Main.tile[k, l].liquid = byte.MaxValue;
						}
						Main.tile[k, l].active(active: false);
					}
				}
			}
			vector += vector2;
			vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			if ((double)vector2.X > 0.5)
			{
				vector2.X = 0.5f;
			}
			if ((double)vector2.X < -0.5)
			{
				vector2.X = -0.5f;
			}
			if ((double)vector2.Y > 1.5)
			{
				vector2.Y = 1.5f;
			}
			if ((double)vector2.Y < 0.5)
			{
				vector2.Y = 0.5f;
			}
		}
	}

	public static void ShroomPatch(int i, int j)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		double num = WorldGen.genRand.Next(40, 70);
		double num2 = num;
		float num3 = WorldGen.genRand.Next(20, 30);
		if (WorldGen.genRand.Next(5) == 0)
		{
			num *= 1.5;
			num2 *= 1.5;
			num3 *= 1.2f;
		}
		Vector2 vector = new Vector2
		{
			X = i,
			Y = (float)j - num3 * 0.3f
		};
		Vector2 vector2 = new Vector2
		{
			X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f,
			Y = (float)WorldGen.genRand.Next(-20, -10) * 0.1f
		};
		while (num > 0.0 && num3 > 0f)
		{
			num -= (double)WorldGen.genRand.Next(3);
			num3--;
			int num4 = (int)((double)vector.X - num * 0.5);
			int num5 = (int)((double)vector.X + num * 0.5);
			int num6 = (int)((double)vector.Y - num * 0.5);
			int num7 = (int)((double)vector.Y + num * 0.5);
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num5 > Main.maxTilesX)
			{
				num5 = Main.maxTilesX;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 > Main.maxTilesY)
			{
				num7 = Main.maxTilesY;
			}
			num2 = num * (double)WorldGen.genRand.Next(80, 120) * 0.01;
			for (int k = num4; k < num5; k++)
			{
				for (int l = num6; l < num7; l++)
				{
					float num8 = Math.Abs((float)k - vector.X);
					float num9 = Math.Abs(((float)l - vector.Y) * 2.3f);
					if (!(Math.Sqrt(num8 * num8 + num9 * num9) < num2 * 0.4))
					{
						continue;
					}
					if ((double)l < (double)vector.Y + num2 * 0.02)
					{
						if (Main.tile[k, l].type != 59)
						{
							Main.tile[k, l].active(active: false);
						}
					}
					else
					{
						Main.tile[k, l].type = 59;
					}
					Main.tile[k, l].liquid = 0;
					Main.tile[k, l].lava(lava: false);
				}
			}
			vector += vector2;
			vector.X += vector2.X;
			vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			vector2.Y -= (float)WorldGen.genRand.Next(11) * 0.05f;
			if ((double)vector2.X > -0.5 && (double)vector2.X < 0.5)
			{
				if (vector2.X < 0f)
				{
					vector2.X = -0.5f;
				}
				else
				{
					vector2.X = 0.5f;
				}
			}
			if (vector2.X > 2f)
			{
				vector2.X = 1f;
			}
			if (vector2.X < -2f)
			{
				vector2.X = -1f;
			}
			if (vector2.Y > 1f)
			{
				vector2.Y = 1f;
			}
			if (vector2.Y < -1f)
			{
				vector2.Y = -1f;
			}
			for (int m = 0; m < 2; m++)
			{
				int num10 = (int)vector.X + WorldGen.genRand.Next(-20, 20);
				int num11 = (int)vector.Y + WorldGen.genRand.Next(0, 20);
				while (!Main.tile[num10, num11].HasTile && Main.tile[num10, num11].type != 59)
				{
					num10 = (int)vector.X + WorldGen.genRand.Next(-20, 20);
					num11 = (int)vector.Y + WorldGen.genRand.Next(0, 20);
				}
				int num12 = WorldGen.genRand.Next(7, 10);
				int num13 = WorldGen.genRand.Next(7, 10);
				TileRunner(num10, num11, num12, num13, 59, addTile: false, 0f, 2f, noYChange: true);
				if (WorldGen.genRand.Next(3) == 0)
				{
					TileRunner(num10, num11, num12 - 3, num13 - 3, -1, addTile: false, 0f, 2f, noYChange: true);
				}
			}
		}
	}

	public static void Cavinator(int i, int j, int steps)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		double num = WorldGen.genRand.Next(7, 15);
		double num2 = num;
		int num3 = 1;
		if (WorldGen.genRand.Next(2) == 0)
		{
			num3 = -1;
		}
		Vector2 vector = new Vector2
		{
			X = i,
			Y = j
		};
		int num4 = WorldGen.genRand.Next(20, 40);
		Vector2 vector2 = new Vector2
		{
			Y = (float)WorldGen.genRand.Next(10, 20) * 0.01f,
			X = num3
		};
		while (num4 > 0)
		{
			num4--;
			int num5 = (int)((double)vector.X - num * 0.5);
			int num6 = (int)((double)vector.X + num * 0.5);
			int num7 = (int)((double)vector.Y - num * 0.5);
			int num8 = (int)((double)vector.Y + num * 0.5);
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.maxTilesX)
			{
				num6 = Main.maxTilesX;
			}
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.maxTilesY)
			{
				num8 = Main.maxTilesY;
			}
			num2 = num * (double)WorldGen.genRand.Next(80, 120) * 0.01;
			for (int k = num5; k < num6; k++)
			{
				for (int l = num7; l < num8; l++)
				{
					float num9 = Math.Abs((float)k - vector.X);
					float num10 = Math.Abs((float)l - vector.Y);
					if (Math.Sqrt(num9 * num9 + num10 * num10) < num2 * 0.4)
					{
						Main.tile[k, l].active(active: false);
					}
				}
			}
			vector += vector2;
			vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			if (vector2.X > (float)num3 + 0.5f)
			{
				vector2.X = (float)num3 + 0.5f;
			}
			if (vector2.X < (float)num3 - 0.5f)
			{
				vector2.X = (float)num3 - 0.5f;
			}
			if (vector2.Y > 2f)
			{
				vector2.Y = 2f;
			}
			if (vector2.Y < 0f)
			{
				vector2.Y = 0f;
			}
		}
		if (steps > 0 && (double)(int)vector.Y < Main.rockLayer + 50.0)
		{
			Cavinator((int)vector.X, (int)vector.Y, steps - 1);
		}
	}

	public static void CaveOpenater(int i, int j)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		double num = WorldGen.genRand.Next(7, 12);
		double num2 = num;
		int num3 = 1;
		if (WorldGen.genRand.Next(2) == 0)
		{
			num3 = -1;
		}
		Vector2 vector = new Vector2
		{
			X = i,
			Y = j
		};
		int num4 = 100;
		Vector2 vector2 = new Vector2
		{
			Y = 0f,
			X = num3
		};
		while (num4 > 0)
		{
			if (Main.tile[(int)vector.X, (int)vector.Y].wall == 0)
			{
				num4 = 0;
			}
			num4--;
			int num5 = (int)((double)vector.X - num * 0.5);
			int num6 = (int)((double)vector.X + num * 0.5);
			int num7 = (int)((double)vector.Y - num * 0.5);
			int num8 = (int)((double)vector.Y + num * 0.5);
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 > Main.maxTilesX)
			{
				num6 = Main.maxTilesX;
			}
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 > Main.maxTilesY)
			{
				num8 = Main.maxTilesY;
			}
			num2 = num * (double)WorldGen.genRand.Next(80, 120) * 0.01;
			for (int k = num5; k < num6; k++)
			{
				for (int l = num7; l < num8; l++)
				{
					float num9 = Math.Abs((float)k - vector.X);
					float num10 = Math.Abs((float)l - vector.Y);
					if (Math.Sqrt(num9 * num9 + num10 * num10) < num2 * 0.4)
					{
						Main.tile[k, l].active(active: false);
					}
				}
			}
			vector += vector2;
			vector2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			vector2.Y += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
			if (vector2.X > (float)num3 + 0.5f)
			{
				vector2.X = (float)num3 + 0.5f;
			}
			if (vector2.X < (float)num3 - 0.5f)
			{
				vector2.X = (float)num3 - 0.5f;
			}
			if (vector2.Y > 0f)
			{
				vector2.Y = 0f;
			}
			if ((double)vector2.Y < -0.5)
			{
				vector2.Y = -0.5f;
			}
		}
	}

	public static void SquareTileFrame(int i, int j, bool resetFrame = true)
	{
		TileFrame(i - 1, j - 1);
		TileFrame(i - 1, j);
		TileFrame(i - 1, j + 1);
		TileFrame(i, j - 1);
		TileFrame(i, j, resetFrame);
		TileFrame(i, j + 1);
		TileFrame(i + 1, j - 1);
		TileFrame(i + 1, j);
		TileFrame(i + 1, j + 1);
	}

	public static void SquareWallFrame(int i, int j, bool resetFrame = true)
	{
		WallFrame(i - 1, j - 1);
		WallFrame(i - 1, j);
		WallFrame(i - 1, j + 1);
		WallFrame(i, j - 1);
		WallFrame(i, j, resetFrame);
		WallFrame(i, j + 1);
		WallFrame(i + 1, j - 1);
		WallFrame(i + 1, j);
		WallFrame(i + 1, j + 1);
	}

	public static void EveryTileFrame()
	{
		for (int i = 0; i < Main.maxTilesX; i++)
		{
			for (int j = 0; j < Main.maxTilesY; j++)
			{
				TileFrame(i, j, resetFrame: true);
			}
		}
	}

	public static void PlantCheck(int i, int j)
	{
		int num7 = -1;
		Tile tile = Main.tile[i, j];
		int type = tile.type;
		_ = i - 1;
		_ = 0;
		_ = i + 1;
		_ = Main.maxTilesX;
		_ = j - 1;
		_ = 0;
		if (j + 1 >= Main.maxTilesY)
		{
			num7 = type;
		}
		if (i - 1 >= 0)
		{
			tile = Main.tile[i - 1, j];
			if (tile.active())
			{
				tile = Main.tile[i - 1, j];
				_ = ref tile.type;
			}
		}
		if (i + 1 < Main.maxTilesX)
		{
			tile = Main.tile[i + 1, j];
			if (tile.active())
			{
				tile = Main.tile[i + 1, j];
				_ = ref tile.type;
			}
		}
		if (j - 1 >= 0)
		{
			tile = Main.tile[i, j - 1];
			if (tile.active())
			{
				tile = Main.tile[i, j - 1];
				_ = ref tile.type;
			}
		}
		if (j + 1 < Main.maxTilesY)
		{
			tile = Main.tile[i, j + 1];
			if (tile.active())
			{
				tile = Main.tile[i, j + 1];
				num7 = tile.type;
			}
		}
		if (i - 1 >= 0 && j - 1 >= 0)
		{
			tile = Main.tile[i - 1, j - 1];
			if (tile.active())
			{
				tile = Main.tile[i - 1, j - 1];
				_ = ref tile.type;
			}
		}
		if (i + 1 < Main.maxTilesX && j - 1 >= 0)
		{
			tile = Main.tile[i + 1, j - 1];
			if (tile.active())
			{
				tile = Main.tile[i + 1, j - 1];
				_ = ref tile.type;
			}
		}
		if (i - 1 >= 0 && j + 1 < Main.maxTilesY)
		{
			tile = Main.tile[i - 1, j + 1];
			if (tile.active())
			{
				tile = Main.tile[i - 1, j + 1];
				_ = ref tile.type;
			}
		}
		if (i + 1 < Main.maxTilesX && j + 1 < Main.maxTilesY)
		{
			tile = Main.tile[i + 1, j + 1];
			if (tile.active())
			{
				tile = Main.tile[i + 1, j + 1];
				_ = ref tile.type;
			}
		}
		if (num7 != 2)
		{
			KillTile(i, j);
		}
	}

	public static void WallFrame(int i, int j, bool resetFrame = false)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a93: Unknown result type (might be due to invalid IL or missing references)
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		int num5 = -1;
		int num6 = -1;
		int num7 = -1;
		int num8 = -1;
		int wall = Main.tile[i, j].wall;
		if (wall == 0)
		{
			return;
		}
		int wallFrameX = Main.tile[i, j].wallFrameX();
		int wallFrameY = Main.tile[i, j].wallFrameY();
		Rectangle rectangle = new Rectangle
		{
			X = -1,
			Y = -1
		};
		if (i - 1 < 0)
		{
			num = wall;
			num4 = wall;
			num6 = wall;
		}
		if (i + 1 >= Main.maxTilesX)
		{
			num3 = wall;
			num5 = wall;
			num8 = wall;
		}
		if (j - 1 < 0)
		{
			num = wall;
			num2 = wall;
			num3 = wall;
		}
		if (j + 1 >= Main.maxTilesY)
		{
			num6 = wall;
			num7 = wall;
			num8 = wall;
		}
		if (i - 1 >= 0)
		{
			num4 = Main.tile[i - 1, j].wall;
		}
		if (i + 1 < Main.maxTilesX)
		{
			num5 = Main.tile[i + 1, j].wall;
		}
		if (j - 1 >= 0)
		{
			num2 = Main.tile[i, j - 1].wall;
		}
		if (j + 1 < Main.maxTilesY)
		{
			num7 = Main.tile[i, j + 1].wall;
		}
		if (i - 1 >= 0 && j - 1 >= 0)
		{
			num = Main.tile[i - 1, j - 1].wall;
		}
		if (i + 1 < Main.maxTilesX && j - 1 >= 0)
		{
			num3 = Main.tile[i + 1, j - 1].wall;
		}
		if (i - 1 >= 0 && j + 1 < Main.maxTilesY)
		{
			num6 = Main.tile[i - 1, j + 1].wall;
		}
		if (i + 1 < Main.maxTilesX && j + 1 < Main.maxTilesY)
		{
			num8 = Main.tile[i + 1, j + 1].wall;
		}
		int num9 = 0;
		if (resetFrame)
		{
			num9 = WorldGen._genRand.Next(0, 3);
			Main.tile[i, j].wallFrameNumber((byte)num9);
		}
		else
		{
			num9 = Main.tile[i, j].wallFrameNumber();
		}
		if (rectangle.X < 0 || rectangle.Y < 0)
		{
			if (num2 == wall && num7 == wall && num4 == wall && num5 == wall)
			{
				if (num != wall && num3 != wall)
				{
					if (num9 == 0)
					{
						rectangle.X = 108;
						rectangle.Y = 18;
					}
					if (num9 == 1)
					{
						rectangle.X = 126;
						rectangle.Y = 18;
					}
					if (num9 == 2)
					{
						rectangle.X = 144;
						rectangle.Y = 18;
					}
				}
				else if (num6 != wall && num8 != wall)
				{
					if (num9 == 0)
					{
						rectangle.X = 108;
						rectangle.Y = 36;
					}
					if (num9 == 1)
					{
						rectangle.X = 126;
						rectangle.Y = 36;
					}
					if (num9 == 2)
					{
						rectangle.X = 144;
						rectangle.Y = 36;
					}
				}
				else if (num != wall && num6 != wall)
				{
					if (num9 == 0)
					{
						rectangle.X = 180;
						rectangle.Y = 0;
					}
					if (num9 == 1)
					{
						rectangle.X = 180;
						rectangle.Y = 18;
					}
					if (num9 == 2)
					{
						rectangle.X = 180;
						rectangle.Y = 36;
					}
				}
				else if (num3 != wall && num8 != wall)
				{
					if (num9 == 0)
					{
						rectangle.X = 198;
						rectangle.Y = 0;
					}
					if (num9 == 1)
					{
						rectangle.X = 198;
						rectangle.Y = 18;
					}
					if (num9 == 2)
					{
						rectangle.X = 198;
						rectangle.Y = 36;
					}
				}
				else
				{
					if (num9 == 0)
					{
						rectangle.X = 18;
						rectangle.Y = 18;
					}
					if (num9 == 1)
					{
						rectangle.X = 36;
						rectangle.Y = 18;
					}
					if (num9 == 2)
					{
						rectangle.X = 54;
						rectangle.Y = 18;
					}
				}
			}
			else if (num2 != wall && num7 == wall && num4 == wall && num5 == wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 18;
					rectangle.Y = 0;
				}
				if (num9 == 1)
				{
					rectangle.X = 36;
					rectangle.Y = 0;
				}
				if (num9 == 2)
				{
					rectangle.X = 54;
					rectangle.Y = 0;
				}
			}
			else if (num2 == wall && num7 != wall && num4 == wall && num5 == wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 18;
					rectangle.Y = 36;
				}
				if (num9 == 1)
				{
					rectangle.X = 36;
					rectangle.Y = 36;
				}
				if (num9 == 2)
				{
					rectangle.X = 54;
					rectangle.Y = 36;
				}
			}
			else if (num2 == wall && num7 == wall && num4 != wall && num5 == wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 0;
					rectangle.Y = 0;
				}
				if (num9 == 1)
				{
					rectangle.X = 0;
					rectangle.Y = 18;
				}
				if (num9 == 2)
				{
					rectangle.X = 0;
					rectangle.Y = 36;
				}
			}
			else if (num2 == wall && num7 == wall && num4 == wall && num5 != wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 72;
					rectangle.Y = 0;
				}
				if (num9 == 1)
				{
					rectangle.X = 72;
					rectangle.Y = 18;
				}
				if (num9 == 2)
				{
					rectangle.X = 72;
					rectangle.Y = 36;
				}
			}
			else if (num2 != wall && num7 == wall && num4 != wall && num5 == wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 0;
					rectangle.Y = 54;
				}
				if (num9 == 1)
				{
					rectangle.X = 36;
					rectangle.Y = 54;
				}
				if (num9 == 2)
				{
					rectangle.X = 72;
					rectangle.Y = 54;
				}
			}
			else if (num2 != wall && num7 == wall && num4 == wall && num5 != wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 18;
					rectangle.Y = 54;
				}
				if (num9 == 1)
				{
					rectangle.X = 54;
					rectangle.Y = 54;
				}
				if (num9 == 2)
				{
					rectangle.X = 90;
					rectangle.Y = 54;
				}
			}
			else if (num2 == wall && num7 != wall && num4 != wall && num5 == wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 0;
					rectangle.Y = 72;
				}
				if (num9 == 1)
				{
					rectangle.X = 36;
					rectangle.Y = 72;
				}
				if (num9 == 2)
				{
					rectangle.X = 72;
					rectangle.Y = 72;
				}
			}
			else if (num2 == wall && num7 != wall && num4 == wall && num5 != wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 18;
					rectangle.Y = 72;
				}
				if (num9 == 1)
				{
					rectangle.X = 54;
					rectangle.Y = 72;
				}
				if (num9 == 2)
				{
					rectangle.X = 90;
					rectangle.Y = 72;
				}
			}
			else if (num2 == wall && num7 == wall && num4 != wall && num5 != wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 90;
					rectangle.Y = 0;
				}
				if (num9 == 1)
				{
					rectangle.X = 90;
					rectangle.Y = 18;
				}
				if (num9 == 2)
				{
					rectangle.X = 90;
					rectangle.Y = 36;
				}
			}
			else if (num2 != wall && num7 != wall && num4 == wall && num5 == wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 108;
					rectangle.Y = 72;
				}
				if (num9 == 1)
				{
					rectangle.X = 126;
					rectangle.Y = 72;
				}
				if (num9 == 2)
				{
					rectangle.X = 144;
					rectangle.Y = 72;
				}
			}
			else if (num2 != wall && num7 == wall && num4 != wall && num5 != wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 108;
					rectangle.Y = 0;
				}
				if (num9 == 1)
				{
					rectangle.X = 126;
					rectangle.Y = 0;
				}
				if (num9 == 2)
				{
					rectangle.X = 144;
					rectangle.Y = 0;
				}
			}
			else if (num2 == wall && num7 != wall && num4 != wall && num5 != wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 108;
					rectangle.Y = 54;
				}
				if (num9 == 1)
				{
					rectangle.X = 126;
					rectangle.Y = 54;
				}
				if (num9 == 2)
				{
					rectangle.X = 144;
					rectangle.Y = 54;
				}
			}
			else if (num2 != wall && num7 != wall && num4 != wall && num5 == wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 162;
					rectangle.Y = 0;
				}
				if (num9 == 1)
				{
					rectangle.X = 162;
					rectangle.Y = 18;
				}
				if (num9 == 2)
				{
					rectangle.X = 162;
					rectangle.Y = 36;
				}
			}
			else if (num2 != wall && num7 != wall && num4 == wall && num5 != wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 216;
					rectangle.Y = 0;
				}
				if (num9 == 1)
				{
					rectangle.X = 216;
					rectangle.Y = 18;
				}
				if (num9 == 2)
				{
					rectangle.X = 216;
					rectangle.Y = 36;
				}
			}
			else if (num2 != wall && num7 != wall && num4 != wall && num5 != wall)
			{
				if (num9 == 0)
				{
					rectangle.X = 162;
					rectangle.Y = 54;
				}
				if (num9 == 1)
				{
					rectangle.X = 180;
					rectangle.Y = 54;
				}
				if (num9 == 2)
				{
					rectangle.X = 198;
					rectangle.Y = 54;
				}
			}
		}
		if (rectangle.X <= -1 || rectangle.Y <= -1)
		{
			if (num9 <= 0)
			{
				rectangle.X = 18;
				rectangle.Y = 18;
			}
			if (num9 == 1)
			{
				rectangle.X = 36;
				rectangle.Y = 18;
			}
			if (num9 >= 2)
			{
				rectangle.X = 54;
				rectangle.Y = 18;
			}
		}
		Main.tile[i, j].wallFrameX((byte)rectangle.X);
		Main.tile[i, j].wallFrameY((byte)rectangle.Y);
		if (rectangle.X != wallFrameX && rectangle.Y != wallFrameY && wallFrameX >= 0)
		{
			_ = 0;
		}
	}

	public static void TileFrame(int i, int j, bool resetFrame = false)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_38ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_4966: Unknown result type (might be due to invalid IL or missing references)
		//IL_38f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4970: Unknown result type (might be due to invalid IL or missing references)
		//IL_516e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5178: Unknown result type (might be due to invalid IL or missing references)
		//IL_51dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_51fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_5203: Unknown result type (might be due to invalid IL or missing references)
		//IL_520e: Unknown result type (might be due to invalid IL or missing references)
		if (i < 0 || j < 0 || i >= Main.maxTilesX || j >= Main.maxTilesY || !Main.tile[i, j].active())
		{
			return;
		}
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		int num5 = -1;
		int num6 = -1;
		int num7 = -1;
		int num8 = -1;
		int type = Main.tile[i, j].type;
		int frameX = Main.tile[i, j].frameX;
		int frameY = Main.tile[i, j].frameY;
		Rectangle rectangle = new Rectangle
		{
			X = -1,
			Y = -1
		};
		if (type == 3)
		{
			PlantCheck(i, j);
			return;
		}
		mergeUp = false;
		mergeDown = false;
		mergeLeft = false;
		mergeRight = false;
		if (i - 1 < 0)
		{
			num = type;
			num4 = type;
			num6 = type;
		}
		if (i + 1 >= Main.maxTilesX)
		{
			num3 = type;
			num5 = type;
			num8 = type;
		}
		if (j - 1 < 0)
		{
			num = type;
			num2 = type;
			num3 = type;
		}
		if (j + 1 >= Main.maxTilesY)
		{
			num6 = type;
			num7 = type;
			num8 = type;
		}
		if (i - 1 >= 0 && Main.tile[i - 1, j].active())
		{
			num4 = Main.tile[i - 1, j].type;
		}
		if (i + 1 < Main.maxTilesX && Main.tile[i + 1, j].active())
		{
			num5 = Main.tile[i + 1, j].type;
		}
		if (j - 1 >= 0 && Main.tile[i, j - 1].active())
		{
			num2 = Main.tile[i, j - 1].type;
		}
		if (j + 1 < Main.maxTilesY && Main.tile[i, j + 1].active())
		{
			num7 = Main.tile[i, j + 1].type;
		}
		if (i - 1 >= 0 && j - 1 >= 0 && Main.tile[i - 1, j - 1].active())
		{
			num = Main.tile[i - 1, j - 1].type;
		}
		if (i + 1 < Main.maxTilesX && j - 1 >= 0 && Main.tile[i + 1, j - 1].active())
		{
			num3 = Main.tile[i + 1, j - 1].type;
		}
		if (i - 1 >= 0 && j + 1 < Main.maxTilesY && Main.tile[i - 1, j + 1].active())
		{
			num6 = Main.tile[i - 1, j + 1].type;
		}
		if (i + 1 < Main.maxTilesX && j + 1 < Main.maxTilesY && Main.tile[i + 1, j + 1].active())
		{
			num8 = Main.tile[i + 1, j + 1].type;
		}
		switch (type)
		{
		case 4:
			if (num7 >= 0 && Main.tileSolid[num7])
			{
				Main.tile[i, j].frameX = 0;
			}
			else if ((num4 >= 0 && Main.tileSolid[num4]) || (num4 == 5 && num == 5 && num6 == 5))
			{
				Main.tile[i, j].frameX = 22;
			}
			else if ((num5 >= 0 && Main.tileSolid[num5]) || (num5 == 5 && num3 == 5 && num8 == 5))
			{
				Main.tile[i, j].frameX = 44;
			}
			else
			{
				KillTile(i, j);
			}
			return;
		case 10:
			if (!destroyObject)
			{
				short frameY2 = Main.tile[i, j].frameY;
				int num9 = j;
				bool flag = false;
				if (frameY2 == 0)
				{
					num9 = j;
				}
				if (frameY2 == 18)
				{
					num9 = j - 1;
				}
				if (frameY2 == 36)
				{
					num9 = j - 2;
				}
				if (!Main.tile[i, num9 - 1].active() || !Main.tileSolid[Main.tile[i, num9 - 1].type])
				{
					flag = true;
				}
				if (!Main.tile[i, num9 + 3].active() || !Main.tileSolid[Main.tile[i, num9 + 3].type])
				{
					flag = true;
				}
				if (!Main.tile[i, num9].active() || Main.tile[i, num9].type != type)
				{
					flag = true;
				}
				if (!Main.tile[i, num9 + 1].active() || Main.tile[i, num9 + 1].type != type)
				{
					flag = true;
				}
				if (!Main.tile[i, num9 + 2].active() || Main.tile[i, num9 + 2].type != type)
				{
					flag = true;
				}
				if (flag)
				{
					destroyObject = true;
					KillTile(i, num9);
					KillTile(i, num9 + 1);
					KillTile(i, num9 + 2);
				}
				destroyObject = false;
			}
			return;
		case 11:
		{
			if (destroyObject)
			{
				return;
			}
			int num10 = 0;
			int num11 = i;
			int num12 = j;
			int frameX2 = Main.tile[i, j].frameX;
			int frameY3 = Main.tile[i, j].frameY;
			bool flag2 = false;
			switch (frameX2)
			{
			case 0:
				num11 = i;
				num10 = 1;
				break;
			case 18:
				num11 = i - 1;
				num10 = 1;
				break;
			case 36:
				num11 = i + 1;
				num10 = -1;
				break;
			case 54:
				num11 = i;
				num10 = -1;
				break;
			}
			switch (frameY3)
			{
			case 0:
				num12 = j;
				break;
			case 18:
				num12 = j - 1;
				break;
			case 36:
				num12 = j - 2;
				break;
			}
			if (!Main.tile[num11, num12 - 1].active() || !Main.tileSolid[Main.tile[num11, num12 - 1].type] || !Main.tile[num11, num12 + 3].active() || !Main.tileSolid[Main.tile[num11, num12 + 3].type])
			{
				flag2 = true;
				destroyObject = true;
			}
			int num13 = num11;
			if (num10 == -1)
			{
				num13 = num11 - 1;
			}
			for (int k = num13; k < num13 + 2; k++)
			{
				for (int l = num12; l < num12 + 3; l++)
				{
					if (!flag2 && (Main.tile[k, l].type != 11 || !Main.tile[k, l].active()))
					{
						destroyObject = true;
						flag2 = true;
						k = num13;
						l = num12;
					}
					if (flag2)
					{
						KillTile(k, l);
					}
				}
			}
			destroyObject = false;
			return;
		}
		case 5:
			if (num7 != type && num7 != -1)
			{
				if (Main.tile[i, j].frameX >= 22 && Main.tile[i, j].frameX <= 44 && Main.tile[i, j].frameY >= 132 && Main.tile[i, j].frameY <= 176 && num4 != type && num5 != type)
				{
					KillTile(i, j);
				}
			}
			else if ((Main.tile[i, j].frameX == 88 && Main.tile[i, j].frameY >= 0 && Main.tile[i, j].frameY <= 44) || (Main.tile[i, j].frameX == 66 && Main.tile[i, j].frameY >= 66 && Main.tile[i, j].frameY <= 130) || (Main.tile[i, j].frameX == 110 && Main.tile[i, j].frameY >= 66 && Main.tile[i, j].frameY <= 110) || (Main.tile[i, j].frameX == 132 && Main.tile[i, j].frameY >= 0 && Main.tile[i, j].frameY <= 176))
			{
				if (num4 == type && num5 == type)
				{
					if (Main.tile[i, j].frameNumber() == 0)
					{
						Main.tile[i, j].frameX = 110;
						Main.tile[i, j].frameY = 66;
					}
					if (Main.tile[i, j].frameNumber() == 1)
					{
						Main.tile[i, j].frameX = 110;
						Main.tile[i, j].frameY = 88;
					}
					if (Main.tile[i, j].frameNumber() == 2)
					{
						Main.tile[i, j].frameX = 110;
						Main.tile[i, j].frameY = 110;
					}
				}
				else if (num4 == type)
				{
					if (Main.tile[i, j].frameNumber() == 0)
					{
						Main.tile[i, j].frameX = 88;
						Main.tile[i, j].frameY = 0;
					}
					if (Main.tile[i, j].frameNumber() == 1)
					{
						Main.tile[i, j].frameX = 88;
						Main.tile[i, j].frameY = 22;
					}
					if (Main.tile[i, j].frameNumber() == 2)
					{
						Main.tile[i, j].frameX = 88;
						Main.tile[i, j].frameY = 44;
					}
				}
				else if (num5 == type)
				{
					if (Main.tile[i, j].frameNumber() == 0)
					{
						Main.tile[i, j].frameX = 66;
						Main.tile[i, j].frameY = 66;
					}
					if (Main.tile[i, j].frameNumber() == 1)
					{
						Main.tile[i, j].frameX = 66;
						Main.tile[i, j].frameY = 88;
					}
					if (Main.tile[i, j].frameNumber() == 2)
					{
						Main.tile[i, j].frameX = 66;
						Main.tile[i, j].frameY = 110;
					}
				}
				else
				{
					if (Main.tile[i, j].frameNumber() == 0)
					{
						Main.tile[i, j].frameX = 0;
						Main.tile[i, j].frameY = 22;
					}
					if (Main.tile[i, j].frameNumber() == 1)
					{
						Main.tile[i, j].frameX = 0;
						Main.tile[i, j].frameY = 22;
					}
					if (Main.tile[i, j].frameNumber() == 2)
					{
						Main.tile[i, j].frameX = 0;
						Main.tile[i, j].frameY = 22;
					}
				}
			}
			if (Main.tile[i, j].frameY >= 132 && Main.tile[i, j].frameY <= 176 && (Main.tile[i, j].frameX == 0 || Main.tile[i, j].frameX == 66 || Main.tile[i, j].frameX == 88))
			{
				if (num4 != type && num5 != type)
				{
					if (Main.tile[i, j].frameNumber() == 0)
					{
						Main.tile[i, j].frameX = 0;
						Main.tile[i, j].frameY = 0;
					}
					if (Main.tile[i, j].frameNumber() == 1)
					{
						Main.tile[i, j].frameX = 0;
						Main.tile[i, j].frameY = 22;
					}
					if (Main.tile[i, j].frameNumber() == 2)
					{
						Main.tile[i, j].frameX = 0;
						Main.tile[i, j].frameY = 44;
					}
				}
				else if (num4 != type)
				{
					if (Main.tile[i, j].frameNumber() == 0)
					{
						Main.tile[i, j].frameX = 0;
						Main.tile[i, j].frameY = 132;
					}
					if (Main.tile[i, j].frameNumber() == 1)
					{
						Main.tile[i, j].frameX = 0;
						Main.tile[i, j].frameY = 154;
					}
					if (Main.tile[i, j].frameNumber() == 2)
					{
						Main.tile[i, j].frameX = 0;
						Main.tile[i, j].frameY = 176;
					}
				}
				else if (num5 != type)
				{
					if (Main.tile[i, j].frameNumber() == 0)
					{
						Main.tile[i, j].frameX = 66;
						Main.tile[i, j].frameY = 132;
					}
					if (Main.tile[i, j].frameNumber() == 1)
					{
						Main.tile[i, j].frameX = 66;
						Main.tile[i, j].frameY = 154;
					}
					if (Main.tile[i, j].frameNumber() == 2)
					{
						Main.tile[i, j].frameX = 66;
						Main.tile[i, j].frameY = 176;
					}
				}
				else
				{
					if (Main.tile[i, j].frameNumber() == 0)
					{
						Main.tile[i, j].frameX = 88;
						Main.tile[i, j].frameY = 132;
					}
					if (Main.tile[i, j].frameNumber() == 1)
					{
						Main.tile[i, j].frameX = 88;
						Main.tile[i, j].frameY = 154;
					}
					if (Main.tile[i, j].frameNumber() == 2)
					{
						Main.tile[i, j].frameX = 88;
						Main.tile[i, j].frameY = 176;
					}
				}
			}
			if ((Main.tile[i, j].frameX == 66 && (Main.tile[i, j].frameY == 0 || Main.tile[i, j].frameY == 22 || Main.tile[i, j].frameY == 44)) || (Main.tile[i, j].frameX == 88 && (Main.tile[i, j].frameY == 66 || Main.tile[i, j].frameY == 88 || Main.tile[i, j].frameY == 110)) || (Main.tile[i, j].frameX == 44 && (Main.tile[i, j].frameY == 198 || Main.tile[i, j].frameY == 220 || Main.tile[i, j].frameY == 242)) || (Main.tile[i, j].frameX == 66 && (Main.tile[i, j].frameY == 198 || Main.tile[i, j].frameY == 220 || Main.tile[i, j].frameY == 242)))
			{
				if (num4 != type && num5 != type)
				{
					KillTile(i, j);
				}
			}
			else if (num7 == -1)
			{
				KillTile(i, j);
			}
			else if (num2 != type && Main.tile[i, j].frameY < 198 && ((Main.tile[i, j].frameX != 22 && Main.tile[i, j].frameX != 44) || Main.tile[i, j].frameY < 132))
			{
				if (num4 == type || num5 == type)
				{
					if (num7 == type)
					{
						if (num4 == type && num5 == type)
						{
							if (Main.tile[i, j].frameNumber() == 0)
							{
								Main.tile[i, j].frameX = 132;
								Main.tile[i, j].frameY = 132;
							}
							if (Main.tile[i, j].frameNumber() == 1)
							{
								Main.tile[i, j].frameX = 132;
								Main.tile[i, j].frameY = 154;
							}
							if (Main.tile[i, j].frameNumber() == 2)
							{
								Main.tile[i, j].frameX = 132;
								Main.tile[i, j].frameY = 176;
							}
						}
						else if (num4 == type)
						{
							if (Main.tile[i, j].frameNumber() == 0)
							{
								Main.tile[i, j].frameX = 132;
								Main.tile[i, j].frameY = 0;
							}
							if (Main.tile[i, j].frameNumber() == 1)
							{
								Main.tile[i, j].frameX = 132;
								Main.tile[i, j].frameY = 22;
							}
							if (Main.tile[i, j].frameNumber() == 2)
							{
								Main.tile[i, j].frameX = 132;
								Main.tile[i, j].frameY = 44;
							}
						}
						else if (num5 == type)
						{
							if (Main.tile[i, j].frameNumber() == 0)
							{
								Main.tile[i, j].frameX = 132;
								Main.tile[i, j].frameY = 66;
							}
							if (Main.tile[i, j].frameNumber() == 1)
							{
								Main.tile[i, j].frameX = 132;
								Main.tile[i, j].frameY = 88;
							}
							if (Main.tile[i, j].frameNumber() == 2)
							{
								Main.tile[i, j].frameX = 132;
								Main.tile[i, j].frameY = 110;
							}
						}
					}
					else if (num4 == type && num5 == type)
					{
						if (Main.tile[i, j].frameNumber() == 0)
						{
							Main.tile[i, j].frameX = 154;
							Main.tile[i, j].frameY = 132;
						}
						if (Main.tile[i, j].frameNumber() == 1)
						{
							Main.tile[i, j].frameX = 154;
							Main.tile[i, j].frameY = 154;
						}
						if (Main.tile[i, j].frameNumber() == 2)
						{
							Main.tile[i, j].frameX = 154;
							Main.tile[i, j].frameY = 176;
						}
					}
					else if (num4 == type)
					{
						if (Main.tile[i, j].frameNumber() == 0)
						{
							Main.tile[i, j].frameX = 154;
							Main.tile[i, j].frameY = 0;
						}
						if (Main.tile[i, j].frameNumber() == 1)
						{
							Main.tile[i, j].frameX = 154;
							Main.tile[i, j].frameY = 22;
						}
						if (Main.tile[i, j].frameNumber() == 2)
						{
							Main.tile[i, j].frameX = 154;
							Main.tile[i, j].frameY = 44;
						}
					}
					else if (num5 == type)
					{
						if (Main.tile[i, j].frameNumber() == 0)
						{
							Main.tile[i, j].frameX = 154;
							Main.tile[i, j].frameY = 66;
						}
						if (Main.tile[i, j].frameNumber() == 1)
						{
							Main.tile[i, j].frameX = 154;
							Main.tile[i, j].frameY = 88;
						}
						if (Main.tile[i, j].frameNumber() == 2)
						{
							Main.tile[i, j].frameX = 154;
							Main.tile[i, j].frameY = 110;
						}
					}
				}
				else
				{
					if (Main.tile[i, j].frameNumber() == 0)
					{
						Main.tile[i, j].frameX = 110;
						Main.tile[i, j].frameY = 0;
					}
					if (Main.tile[i, j].frameNumber() == 1)
					{
						Main.tile[i, j].frameX = 110;
						Main.tile[i, j].frameY = 22;
					}
					if (Main.tile[i, j].frameNumber() == 2)
					{
						Main.tile[i, j].frameX = 110;
						Main.tile[i, j].frameY = 44;
					}
				}
			}
			rectangle.X = Main.tile[i, j].frameX;
			rectangle.Y = Main.tile[i, j].frameY;
			break;
		}
		int num14 = 0;
		if (resetFrame)
		{
			num14 = WorldGen._genRand.Next(0, 3);
			Main.tile[i, j].frameNumber((byte)num14);
		}
		else
		{
			num14 = Main.tile[i, j].frameNumber();
		}
		if (type == 0)
		{
			for (int m = 0; m < 12; m++)
			{
				if (m != 1 && m != 6 && m != 7 && m != 8 && m != 9)
				{
					continue;
				}
				if (num2 == m)
				{
					TileFrame(i, j - 1);
					if (mergeDown)
					{
						num2 = type;
					}
				}
				if (num7 == m)
				{
					TileFrame(i, j + 1);
					if (mergeUp)
					{
						num7 = type;
					}
				}
				if (num4 == m)
				{
					TileFrame(i - 1, j);
					if (mergeRight)
					{
						num4 = type;
					}
				}
				if (num5 == m)
				{
					TileFrame(i + 1, j);
					if (mergeLeft)
					{
						num5 = type;
					}
				}
				if (num == m)
				{
					num = type;
				}
				if (num3 == m)
				{
					num3 = type;
				}
				if (num6 == m)
				{
					num6 = type;
				}
				if (num8 == m)
				{
					num8 = type;
				}
			}
			if (num2 == 2)
			{
				num2 = type;
			}
			if (num7 == 2)
			{
				num7 = type;
			}
			if (num4 == 2)
			{
				num4 = type;
			}
			if (num5 == 2)
			{
				num5 = type;
			}
			if (num == 2)
			{
				num = type;
			}
			if (num3 == 2)
			{
				num3 = type;
			}
			if (num6 == 2)
			{
				num6 = type;
			}
			if (num8 == 2)
			{
				num8 = type;
			}
		}
		if (type == 1 || type == 6 || type == 7 || type == 8 || type == 9)
		{
			for (int n = 0; n < 12; n++)
			{
				if (n == 1 || n == 6 || n == 7 || n == 8 || n == 9)
				{
					if (num2 == 0)
					{
						num2 = -2;
					}
					if (num7 == 0)
					{
						num7 = -2;
					}
					if (num4 == 0)
					{
						num4 = -2;
					}
					if (num5 == 0)
					{
						num5 = -2;
					}
					if (num == 0)
					{
						num = -2;
					}
					if (num3 == 0)
					{
						num3 = -2;
					}
					if (num6 == 0)
					{
						num6 = -2;
					}
					if (num8 == 0)
					{
						num8 = -2;
					}
				}
			}
		}
		if (type == 2)
		{
			int num15 = 0;
			if (num2 != type && num2 != num15 && (num7 == type || num7 == num15))
			{
				if (num4 == num15 && num5 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 0;
						rectangle.Y = 198;
					}
					if (num14 == 1)
					{
						rectangle.X = 18;
						rectangle.Y = 198;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 198;
					}
				}
				else if (num4 == type && num5 == num15)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 198;
					}
					if (num14 == 1)
					{
						rectangle.X = 72;
						rectangle.Y = 198;
					}
					if (num14 == 2)
					{
						rectangle.X = 90;
						rectangle.Y = 198;
					}
				}
			}
			else if (num7 != type && num7 != num15 && (num2 == type || num2 == num15))
			{
				if (num4 == num15 && num5 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 0;
						rectangle.Y = 216;
					}
					if (num14 == 1)
					{
						rectangle.X = 18;
						rectangle.Y = 216;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 216;
					}
				}
				else if (num4 == type && num5 == num15)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 216;
					}
					if (num14 == 1)
					{
						rectangle.X = 72;
						rectangle.Y = 216;
					}
					if (num14 == 2)
					{
						rectangle.X = 90;
						rectangle.Y = 216;
					}
				}
			}
			else if (num4 != type && num4 != num15 && (num5 == type || num5 == num15))
			{
				if (num2 == num15 && num7 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 72;
						rectangle.Y = 144;
					}
					if (num14 == 1)
					{
						rectangle.X = 72;
						rectangle.Y = 162;
					}
					if (num14 == 2)
					{
						rectangle.X = 72;
						rectangle.Y = 180;
					}
				}
				else if (num7 == type && num5 == num2)
				{
					if (num14 == 0)
					{
						rectangle.X = 72;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 72;
						rectangle.Y = 108;
					}
					if (num14 == 2)
					{
						rectangle.X = 72;
						rectangle.Y = 126;
					}
				}
			}
			else if (num5 != type && num5 != num15 && (num4 == type || num4 == num15))
			{
				if (num2 == num15 && num7 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 90;
						rectangle.Y = 144;
					}
					if (num14 == 1)
					{
						rectangle.X = 90;
						rectangle.Y = 162;
					}
					if (num14 == 2)
					{
						rectangle.X = 90;
						rectangle.Y = 180;
					}
				}
				else if (num7 == type && num5 == num2)
				{
					if (num14 == 0)
					{
						rectangle.X = 90;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 90;
						rectangle.Y = 108;
					}
					if (num14 == 2)
					{
						rectangle.X = 90;
						rectangle.Y = 126;
					}
				}
			}
			else if (num2 == type && num7 == type && num4 == type && num5 == type)
			{
				if (num != type && num3 != type && num6 != type && num8 != type)
				{
					if (num8 == num15)
					{
						if (num14 == 0)
						{
							rectangle.X = 108;
							rectangle.Y = 324;
						}
						if (num14 == 1)
						{
							rectangle.X = 126;
							rectangle.Y = 324;
						}
						if (num14 == 2)
						{
							rectangle.X = 144;
							rectangle.Y = 324;
						}
					}
					else if (num3 == num15)
					{
						if (num14 == 0)
						{
							rectangle.X = 108;
							rectangle.Y = 342;
						}
						if (num14 == 1)
						{
							rectangle.X = 126;
							rectangle.Y = 342;
						}
						if (num14 == 2)
						{
							rectangle.X = 144;
							rectangle.Y = 342;
						}
					}
					else if (num6 == num15)
					{
						if (num14 == 0)
						{
							rectangle.X = 108;
							rectangle.Y = 360;
						}
						if (num14 == 1)
						{
							rectangle.X = 126;
							rectangle.Y = 360;
						}
						if (num14 == 2)
						{
							rectangle.X = 144;
							rectangle.Y = 360;
						}
					}
					else if (num == num15)
					{
						if (num14 == 0)
						{
							rectangle.X = 108;
							rectangle.Y = 378;
						}
						if (num14 == 1)
						{
							rectangle.X = 126;
							rectangle.Y = 378;
						}
						if (num14 == 2)
						{
							rectangle.X = 144;
							rectangle.Y = 378;
						}
					}
					else
					{
						if (num14 == 0)
						{
							rectangle.X = 144;
							rectangle.Y = 234;
						}
						if (num14 == 1)
						{
							rectangle.X = 198;
							rectangle.Y = 234;
						}
						if (num14 == 2)
						{
							rectangle.X = 252;
							rectangle.Y = 234;
						}
					}
				}
				else if (num != type && num8 != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 36;
						rectangle.Y = 306;
					}
					if (num14 == 1)
					{
						rectangle.X = 54;
						rectangle.Y = 306;
					}
					if (num14 == 2)
					{
						rectangle.X = 72;
						rectangle.Y = 306;
					}
				}
				else if (num3 != type && num6 != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 90;
						rectangle.Y = 306;
					}
					if (num14 == 1)
					{
						rectangle.X = 108;
						rectangle.Y = 306;
					}
					if (num14 == 2)
					{
						rectangle.X = 126;
						rectangle.Y = 306;
					}
				}
				else if (num != type && num3 == type && num6 == type && num8 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 108;
					}
					if (num14 == 1)
					{
						rectangle.X = 54;
						rectangle.Y = 144;
					}
					if (num14 == 2)
					{
						rectangle.X = 54;
						rectangle.Y = 180;
					}
				}
				else if (num == type && num3 != type && num6 == type && num8 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 36;
						rectangle.Y = 108;
					}
					if (num14 == 1)
					{
						rectangle.X = 36;
						rectangle.Y = 144;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 180;
					}
				}
				else if (num == type && num3 == type && num6 != type && num8 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 54;
						rectangle.Y = 126;
					}
					if (num14 == 2)
					{
						rectangle.X = 54;
						rectangle.Y = 162;
					}
				}
				else if (num == type && num3 == type && num6 == type && num8 != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 36;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 36;
						rectangle.Y = 126;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 162;
					}
				}
			}
			else if (num2 == type && num7 == num15 && num4 == type && num5 == type && num == -1 && num3 == -1)
			{
				if (num14 == 0)
				{
					rectangle.X = 108;
					rectangle.Y = 18;
				}
				if (num14 == 1)
				{
					rectangle.X = 126;
					rectangle.Y = 18;
				}
				if (num14 == 2)
				{
					rectangle.X = 144;
					rectangle.Y = 18;
				}
			}
			else if (num2 == num15 && num7 == type && num4 == type && num5 == type && num6 == -1 && num8 == -1)
			{
				if (num14 == 0)
				{
					rectangle.X = 108;
					rectangle.Y = 36;
				}
				if (num14 == 1)
				{
					rectangle.X = 126;
					rectangle.Y = 36;
				}
				if (num14 == 2)
				{
					rectangle.X = 144;
					rectangle.Y = 36;
				}
			}
			else if (num2 == type && num7 == type && num4 == num15 && num5 == type && num3 == -1 && num8 == -1)
			{
				if (num14 == 0)
				{
					rectangle.X = 198;
					rectangle.Y = 0;
				}
				if (num14 == 1)
				{
					rectangle.X = 198;
					rectangle.Y = 18;
				}
				if (num14 == 2)
				{
					rectangle.X = 198;
					rectangle.Y = 36;
				}
			}
			else if (num2 == type && num7 == type && num4 == type && num5 == num15 && num == -1 && num6 == -1)
			{
				if (num14 == 0)
				{
					rectangle.X = 180;
					rectangle.Y = 0;
				}
				if (num14 == 1)
				{
					rectangle.X = 180;
					rectangle.Y = 18;
				}
				if (num14 == 2)
				{
					rectangle.X = 180;
					rectangle.Y = 36;
				}
			}
			else if (num2 == type && num7 == num15 && num4 == type && num5 == type)
			{
				if (num3 != -1)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 108;
					}
					if (num14 == 1)
					{
						rectangle.X = 54;
						rectangle.Y = 144;
					}
					if (num14 == 2)
					{
						rectangle.X = 54;
						rectangle.Y = 180;
					}
				}
				else if (num != -1)
				{
					if (num14 == 0)
					{
						rectangle.X = 36;
						rectangle.Y = 108;
					}
					if (num14 == 1)
					{
						rectangle.X = 36;
						rectangle.Y = 144;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 180;
					}
				}
			}
			else if (num2 == num15 && num7 == type && num4 == type && num5 == type)
			{
				if (num8 != -1)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 54;
						rectangle.Y = 126;
					}
					if (num14 == 2)
					{
						rectangle.X = 54;
						rectangle.Y = 162;
					}
				}
				else if (num6 != -1)
				{
					if (num14 == 0)
					{
						rectangle.X = 36;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 36;
						rectangle.Y = 126;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 162;
					}
				}
			}
			else if (num2 == type && num7 == type && num4 == type && num5 == num15)
			{
				if (num != -1)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 54;
						rectangle.Y = 126;
					}
					if (num14 == 2)
					{
						rectangle.X = 54;
						rectangle.Y = 162;
					}
				}
				else if (num6 != -1)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 108;
					}
					if (num14 == 1)
					{
						rectangle.X = 54;
						rectangle.Y = 144;
					}
					if (num14 == 2)
					{
						rectangle.X = 54;
						rectangle.Y = 180;
					}
				}
			}
			else if (num2 == type && num7 == type && num4 == num15 && num5 == type)
			{
				if (num3 != -1)
				{
					if (num14 == 0)
					{
						rectangle.X = 36;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 36;
						rectangle.Y = 126;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 162;
					}
				}
				else if (num8 != -1)
				{
					if (num14 == 0)
					{
						rectangle.X = 36;
						rectangle.Y = 108;
					}
					if (num14 == 1)
					{
						rectangle.X = 36;
						rectangle.Y = 144;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 180;
					}
				}
			}
			else if ((num2 == num15 && num7 == type && num4 == type && num5 == type) || (num2 == type && num7 == num15 && num4 == type && num5 == type) || (num2 == type && num7 == type && num4 == num15 && num5 == type) || (num2 == type && num7 == type && num4 == type && num5 == num15))
			{
				if (num14 == 0)
				{
					rectangle.X = 18;
					rectangle.Y = 18;
				}
				if (num14 == 1)
				{
					rectangle.X = 36;
					rectangle.Y = 18;
				}
				if (num14 == 2)
				{
					rectangle.X = 54;
					rectangle.Y = 18;
				}
			}
			if ((num2 == type || num2 == num15) && (num7 == type || num7 == num15) && (num4 == type || num4 == num15) && (num5 == type || num5 == num15))
			{
				if (num != type && num != num15 && (num3 == type || num3 == num15) && (num6 == type || num6 == num15) && (num8 == type || num8 == num15))
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 108;
					}
					if (num14 == 1)
					{
						rectangle.X = 54;
						rectangle.Y = 144;
					}
					if (num14 == 2)
					{
						rectangle.X = 54;
						rectangle.Y = 180;
					}
				}
				else if (num3 != type && num3 != num15 && (num == type || num == num15) && (num6 == type || num6 == num15) && (num8 == type || num8 == num15))
				{
					if (num14 == 0)
					{
						rectangle.X = 36;
						rectangle.Y = 108;
					}
					if (num14 == 1)
					{
						rectangle.X = 36;
						rectangle.Y = 144;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 180;
					}
				}
				else if (num6 != type && num6 != num15 && (num == type || num == num15) && (num3 == type || num3 == num15) && (num8 == type || num8 == num15))
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 54;
						rectangle.Y = 126;
					}
					if (num14 == 2)
					{
						rectangle.X = 54;
						rectangle.Y = 162;
					}
				}
				else if (num8 != type && num8 != num15 && (num == type || num == num15) && (num6 == type || num6 == num15) && (num3 == type || num3 == num15))
				{
					if (num14 == 0)
					{
						rectangle.X = 36;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 36;
						rectangle.Y = 126;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 162;
					}
				}
			}
			if (num2 != num15 && num2 != type && num7 == type && num4 != num15 && num4 != type && num5 == type && num8 != num15 && num8 != type)
			{
				if (num14 == 0)
				{
					rectangle.X = 90;
					rectangle.Y = 270;
				}
				if (num14 == 1)
				{
					rectangle.X = 108;
					rectangle.Y = 270;
				}
				if (num14 == 2)
				{
					rectangle.X = 126;
					rectangle.Y = 270;
				}
			}
			else if (num2 != num15 && num2 != type && num7 == type && num4 == type && num5 != num15 && num5 != type && num6 != num15 && num6 != type)
			{
				if (num14 == 0)
				{
					rectangle.X = 144;
					rectangle.Y = 270;
				}
				if (num14 == 1)
				{
					rectangle.X = 162;
					rectangle.Y = 270;
				}
				if (num14 == 2)
				{
					rectangle.X = 180;
					rectangle.Y = 270;
				}
			}
			else if (num7 != num15 && num7 != type && num2 == type && num4 != num15 && num4 != type && num5 == type && num3 != num15 && num3 != type)
			{
				if (num14 == 0)
				{
					rectangle.X = 90;
					rectangle.Y = 288;
				}
				if (num14 == 1)
				{
					rectangle.X = 108;
					rectangle.Y = 288;
				}
				if (num14 == 2)
				{
					rectangle.X = 126;
					rectangle.Y = 288;
				}
			}
			else if (num7 != num15 && num7 != type && num2 == type && num4 == type && num5 != num15 && num5 != type && num != num15 && num != type)
			{
				if (num14 == 0)
				{
					rectangle.X = 144;
					rectangle.Y = 288;
				}
				if (num14 == 1)
				{
					rectangle.X = 162;
					rectangle.Y = 288;
				}
				if (num14 == 2)
				{
					rectangle.X = 180;
					rectangle.Y = 288;
				}
			}
			else if (num2 != type && num2 != num15 && num7 == type && num4 == type && num5 == type && num6 != type && num6 != num15 && num8 != type && num8 != num15)
			{
				if (num14 == 0)
				{
					rectangle.X = 144;
					rectangle.Y = 216;
				}
				if (num14 == 1)
				{
					rectangle.X = 198;
					rectangle.Y = 216;
				}
				if (num14 == 2)
				{
					rectangle.X = 252;
					rectangle.Y = 216;
				}
			}
			else if (num7 != type && num7 != num15 && num2 == type && num4 == type && num5 == type && num != type && num != num15 && num3 != type && num3 != num15)
			{
				if (num14 == 0)
				{
					rectangle.X = 144;
					rectangle.Y = 252;
				}
				if (num14 == 1)
				{
					rectangle.X = 198;
					rectangle.Y = 252;
				}
				if (num14 == 2)
				{
					rectangle.X = 252;
					rectangle.Y = 252;
				}
			}
			else if (num4 != type && num4 != num15 && num7 == type && num2 == type && num5 == type && num3 != type && num3 != num15 && num8 != type && num8 != num15)
			{
				if (num14 == 0)
				{
					rectangle.X = 126;
					rectangle.Y = 234;
				}
				if (num14 == 1)
				{
					rectangle.X = 180;
					rectangle.Y = 234;
				}
				if (num14 == 2)
				{
					rectangle.X = 234;
					rectangle.Y = 234;
				}
			}
			else if (num5 != type && num5 != num15 && num7 == type && num2 == type && num4 == type && num != type && num != num15 && num6 != type && num6 != num15)
			{
				if (num14 == 0)
				{
					rectangle.X = 162;
					rectangle.Y = 234;
				}
				if (num14 == 1)
				{
					rectangle.X = 216;
					rectangle.Y = 234;
				}
				if (num14 == 2)
				{
					rectangle.X = 270;
					rectangle.Y = 234;
				}
			}
			else if (num2 != num15 && num2 != type && (num7 == num15 || num7 == type) && num4 == num15 && num5 == num15)
			{
				if (num14 == 0)
				{
					rectangle.X = 36;
					rectangle.Y = 270;
				}
				if (num14 == 1)
				{
					rectangle.X = 54;
					rectangle.Y = 270;
				}
				if (num14 == 2)
				{
					rectangle.X = 72;
					rectangle.Y = 270;
				}
			}
			else if (num7 != num15 && num7 != type && (num2 == num15 || num2 == type) && num4 == num15 && num5 == num15)
			{
				if (num14 == 0)
				{
					rectangle.X = 36;
					rectangle.Y = 288;
				}
				if (num14 == 1)
				{
					rectangle.X = 54;
					rectangle.Y = 288;
				}
				if (num14 == 2)
				{
					rectangle.X = 72;
					rectangle.Y = 288;
				}
			}
			else if (num4 != num15 && num4 != type && (num5 == num15 || num5 == type) && num2 == num15 && num7 == num15)
			{
				if (num14 == 0)
				{
					rectangle.X = 0;
					rectangle.Y = 270;
				}
				if (num14 == 1)
				{
					rectangle.X = 0;
					rectangle.Y = 288;
				}
				if (num14 == 2)
				{
					rectangle.X = 0;
					rectangle.Y = 306;
				}
			}
			else if (num5 != num15 && num5 != type && (num4 == num15 || num4 == type) && num2 == num15 && num7 == num15)
			{
				if (num14 == 0)
				{
					rectangle.X = 18;
					rectangle.Y = 270;
				}
				if (num14 == 1)
				{
					rectangle.X = 18;
					rectangle.Y = 288;
				}
				if (num14 == 2)
				{
					rectangle.X = 18;
					rectangle.Y = 306;
				}
			}
			else if (num2 == type && num7 == num15 && num4 == num15 && num5 == num15)
			{
				if (num14 == 0)
				{
					rectangle.X = 198;
					rectangle.Y = 288;
				}
				if (num14 == 1)
				{
					rectangle.X = 216;
					rectangle.Y = 288;
				}
				if (num14 == 2)
				{
					rectangle.X = 234;
					rectangle.Y = 288;
				}
			}
			else if (num2 == num15 && num7 == type && num4 == num15 && num5 == num15)
			{
				if (num14 == 0)
				{
					rectangle.X = 198;
					rectangle.Y = 270;
				}
				if (num14 == 1)
				{
					rectangle.X = 216;
					rectangle.Y = 270;
				}
				if (num14 == 2)
				{
					rectangle.X = 234;
					rectangle.Y = 270;
				}
			}
			else if (num2 == num15 && num7 == num15 && num4 == type && num5 == num15)
			{
				if (num14 == 0)
				{
					rectangle.X = 198;
					rectangle.Y = 306;
				}
				if (num14 == 1)
				{
					rectangle.X = 216;
					rectangle.Y = 306;
				}
				if (num14 == 2)
				{
					rectangle.X = 234;
					rectangle.Y = 306;
				}
			}
			else if (num2 == num15 && num7 == num15 && num4 == num15 && num5 == type)
			{
				if (num14 == 0)
				{
					rectangle.X = 144;
					rectangle.Y = 306;
				}
				if (num14 == 1)
				{
					rectangle.X = 162;
					rectangle.Y = 306;
				}
				if (num14 == 2)
				{
					rectangle.X = 180;
					rectangle.Y = 306;
				}
			}
			if (num2 != type && num2 != num15 && num7 == type && num4 == type && num5 == type)
			{
				if ((num6 == num15 || num6 == type) && num8 != num15 && num8 != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 0;
						rectangle.Y = 324;
					}
					if (num14 == 1)
					{
						rectangle.X = 18;
						rectangle.Y = 324;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 324;
					}
				}
				else if ((num8 == num15 || num8 == type) && num6 != num15 && num6 != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 324;
					}
					if (num14 == 1)
					{
						rectangle.X = 72;
						rectangle.Y = 324;
					}
					if (num14 == 2)
					{
						rectangle.X = 90;
						rectangle.Y = 324;
					}
				}
			}
			else if (num7 != type && num7 != num15 && num2 == type && num4 == type && num5 == type)
			{
				if ((num == num15 || num == type) && num3 != num15 && num3 != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 0;
						rectangle.Y = 342;
					}
					if (num14 == 1)
					{
						rectangle.X = 18;
						rectangle.Y = 342;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 342;
					}
				}
				else if ((num3 == num15 || num3 == type) && num != num15 && num != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 342;
					}
					if (num14 == 1)
					{
						rectangle.X = 72;
						rectangle.Y = 342;
					}
					if (num14 == 2)
					{
						rectangle.X = 90;
						rectangle.Y = 342;
					}
				}
			}
			else if (num4 != type && num4 != num15 && num2 == type && num7 == type && num5 == type)
			{
				if ((num3 == num15 || num3 == type) && num8 != num15 && num8 != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 360;
					}
					if (num14 == 1)
					{
						rectangle.X = 72;
						rectangle.Y = 360;
					}
					if (num14 == 2)
					{
						rectangle.X = 90;
						rectangle.Y = 360;
					}
				}
				else if ((num8 == num15 || num8 == type) && num3 != num15 && num3 != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 0;
						rectangle.Y = 360;
					}
					if (num14 == 1)
					{
						rectangle.X = 18;
						rectangle.Y = 360;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 360;
					}
				}
			}
			else if (num5 != type && num5 != num15 && num2 == type && num7 == type && num4 == type)
			{
				if ((num == num15 || num == type) && num6 != num15 && num6 != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 0;
						rectangle.Y = 378;
					}
					if (num14 == 1)
					{
						rectangle.X = 18;
						rectangle.Y = 378;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 378;
					}
				}
				else if ((num6 == num15 || num6 == type) && num != num15 && num != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 378;
					}
					if (num14 == 1)
					{
						rectangle.X = 72;
						rectangle.Y = 378;
					}
					if (num14 == 2)
					{
						rectangle.X = 90;
						rectangle.Y = 378;
					}
				}
			}
			if ((num2 == type || num2 == num15) && (num7 == type || num7 == num15) && (num4 == type || num4 == num15) && (num5 == type || num5 == num15) && num != -1 && num3 != -1 && num6 != -1 && num8 != -1)
			{
				if (num14 == 0)
				{
					rectangle.X = 18;
					rectangle.Y = 18;
				}
				if (num14 == 1)
				{
					rectangle.X = 36;
					rectangle.Y = 18;
				}
				if (num14 == 2)
				{
					rectangle.X = 54;
					rectangle.Y = 18;
				}
			}
			if (num2 == num15)
			{
				num2 = -2;
			}
			if (num7 == num15)
			{
				num7 = -2;
			}
			if (num4 == num15)
			{
				num4 = -2;
			}
			if (num5 == num15)
			{
				num5 = -2;
			}
			if (num == num15)
			{
				num = -2;
			}
			if (num3 == num15)
			{
				num3 = -2;
			}
			if (num6 == num15)
			{
				num6 = -2;
			}
			if (num8 == num15)
			{
				num8 = -2;
			}
		}
		if ((type == 1 || type == 2 || type == 6 || type == 7) && rectangle.X == -1 && rectangle.Y == -1)
		{
			if (num2 >= 0 && num2 != type)
			{
				num2 = -1;
			}
			if (num7 >= 0 && num7 != type)
			{
				num7 = -1;
			}
			if (num4 >= 0 && num4 != type)
			{
				num4 = -1;
			}
			if (num5 >= 0 && num5 != type)
			{
				num5 = -1;
			}
			if (num2 != -1 && num7 != -1 && num4 != -1 && num5 != -1)
			{
				if (num2 == -2 && num7 == type && num4 == type && num5 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 144;
						rectangle.Y = 108;
					}
					if (num14 == 1)
					{
						rectangle.X = 162;
						rectangle.Y = 108;
					}
					if (num14 == 2)
					{
						rectangle.X = 180;
						rectangle.Y = 108;
					}
					mergeUp = true;
				}
				else if (num2 == type && num7 == -2 && num4 == type && num5 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 144;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 162;
						rectangle.Y = 90;
					}
					if (num14 == 2)
					{
						rectangle.X = 180;
						rectangle.Y = 90;
					}
					mergeDown = true;
				}
				else if (num2 == type && num7 == type && num4 == -2 && num5 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 162;
						rectangle.Y = 126;
					}
					if (num14 == 1)
					{
						rectangle.X = 162;
						rectangle.Y = 144;
					}
					if (num14 == 2)
					{
						rectangle.X = 162;
						rectangle.Y = 162;
					}
					mergeLeft = true;
				}
				else if (num2 == type && num7 == type && num4 == type && num5 == -2)
				{
					if (num14 == 0)
					{
						rectangle.X = 144;
						rectangle.Y = 126;
					}
					if (num14 == 1)
					{
						rectangle.X = 144;
						rectangle.Y = 144;
					}
					if (num14 == 2)
					{
						rectangle.X = 144;
						rectangle.Y = 162;
					}
					mergeRight = true;
				}
				else if (num2 == -2 && num7 == type && num4 == -2 && num5 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 36;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 36;
						rectangle.Y = 126;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 162;
					}
					mergeUp = true;
					mergeLeft = true;
				}
				else if (num2 == -2 && num7 == type && num4 == type && num5 == -2)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 54;
						rectangle.Y = 126;
					}
					if (num14 == 2)
					{
						rectangle.X = 54;
						rectangle.Y = 162;
					}
					mergeUp = true;
					mergeRight = true;
				}
				else if (num2 == type && num7 == -2 && num4 == -2 && num5 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 36;
						rectangle.Y = 108;
					}
					if (num14 == 1)
					{
						rectangle.X = 36;
						rectangle.Y = 144;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 180;
					}
					mergeDown = true;
					mergeLeft = true;
				}
				else if (num2 == type && num7 == -2 && num4 == type && num5 == -2)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 108;
					}
					if (num14 == 1)
					{
						rectangle.X = 54;
						rectangle.Y = 144;
					}
					if (num14 == 2)
					{
						rectangle.X = 54;
						rectangle.Y = 180;
					}
					mergeDown = true;
					mergeRight = true;
				}
				else if (num2 == type && num7 == type && num4 == -2 && num5 == -2)
				{
					if (num14 == 0)
					{
						rectangle.X = 180;
						rectangle.Y = 126;
					}
					if (num14 == 1)
					{
						rectangle.X = 180;
						rectangle.Y = 144;
					}
					if (num14 == 2)
					{
						rectangle.X = 180;
						rectangle.Y = 162;
					}
					mergeLeft = true;
					mergeRight = true;
				}
				else if (num2 == -2 && num7 == -2 && num4 == type && num5 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 144;
						rectangle.Y = 180;
					}
					if (num14 == 1)
					{
						rectangle.X = 162;
						rectangle.Y = 180;
					}
					if (num14 == 2)
					{
						rectangle.X = 180;
						rectangle.Y = 180;
					}
					mergeUp = true;
					mergeDown = true;
				}
				else if (num2 == -2 && num7 == type && num4 == -2 && num5 == -2)
				{
					if (num14 == 0)
					{
						rectangle.X = 198;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 198;
						rectangle.Y = 108;
					}
					if (num14 == 2)
					{
						rectangle.X = 198;
						rectangle.Y = 126;
					}
					mergeUp = true;
					mergeLeft = true;
					mergeRight = true;
				}
				else if (num2 == type && num7 == -2 && num4 == -2 && num5 == -2)
				{
					if (num14 == 0)
					{
						rectangle.X = 198;
						rectangle.Y = 144;
					}
					if (num14 == 1)
					{
						rectangle.X = 198;
						rectangle.Y = 162;
					}
					if (num14 == 2)
					{
						rectangle.X = 198;
						rectangle.Y = 180;
					}
					mergeDown = true;
					mergeLeft = true;
					mergeRight = true;
				}
				else if (num2 == -2 && num7 == -2 && num4 == type && num5 == -2)
				{
					if (num14 == 0)
					{
						rectangle.X = 216;
						rectangle.Y = 144;
					}
					if (num14 == 1)
					{
						rectangle.X = 216;
						rectangle.Y = 162;
					}
					if (num14 == 2)
					{
						rectangle.X = 216;
						rectangle.Y = 180;
					}
					mergeUp = true;
					mergeDown = true;
					mergeRight = true;
				}
				else if (num2 == -2 && num7 == -2 && num4 == -2 && num5 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 216;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 216;
						rectangle.Y = 108;
					}
					if (num14 == 2)
					{
						rectangle.X = 216;
						rectangle.Y = 126;
					}
					mergeUp = true;
					mergeDown = true;
					mergeLeft = true;
				}
				else if (num2 == -2 && num7 == -2 && num4 == -2 && num5 == -2)
				{
					if (num14 == 0)
					{
						rectangle.X = 108;
						rectangle.Y = 198;
					}
					if (num14 == 1)
					{
						rectangle.X = 126;
						rectangle.Y = 198;
					}
					if (num14 == 2)
					{
						rectangle.X = 144;
						rectangle.Y = 198;
					}
					mergeUp = true;
					mergeDown = true;
					mergeLeft = true;
					mergeRight = true;
				}
				else if (num2 == type && num7 == type && num4 == type && num5 == type)
				{
					if (num == -2)
					{
						if (num14 == 0)
						{
							rectangle.X = 18;
							rectangle.Y = 108;
						}
						if (num14 == 1)
						{
							rectangle.X = 18;
							rectangle.Y = 144;
						}
						if (num14 == 2)
						{
							rectangle.X = 18;
							rectangle.Y = 180;
						}
					}
					if (num3 == -2)
					{
						if (num14 == 0)
						{
							rectangle.X = 0;
							rectangle.Y = 108;
						}
						if (num14 == 1)
						{
							rectangle.X = 0;
							rectangle.Y = 144;
						}
						if (num14 == 2)
						{
							rectangle.X = 0;
							rectangle.Y = 180;
						}
					}
					if (num6 == -2)
					{
						if (num14 == 0)
						{
							rectangle.X = 18;
							rectangle.Y = 90;
						}
						if (num14 == 1)
						{
							rectangle.X = 18;
							rectangle.Y = 126;
						}
						if (num14 == 2)
						{
							rectangle.X = 18;
							rectangle.Y = 162;
						}
					}
					if (num8 == -2)
					{
						if (num14 == 0)
						{
							rectangle.X = 0;
							rectangle.Y = 90;
						}
						if (num14 == 1)
						{
							rectangle.X = 0;
							rectangle.Y = 126;
						}
						if (num14 == 2)
						{
							rectangle.X = 0;
							rectangle.Y = 162;
						}
					}
				}
			}
			else if (num2 != -1 && num7 != -1 && num4 == -1 && num5 == type)
			{
				if (num2 == -2 && num7 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 72;
						rectangle.Y = 144;
					}
					if (num14 == 1)
					{
						rectangle.X = 72;
						rectangle.Y = 162;
					}
					if (num14 == 2)
					{
						rectangle.X = 72;
						rectangle.Y = 180;
					}
					mergeUp = true;
				}
				else if (num7 == -2 && num2 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 72;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 72;
						rectangle.Y = 108;
					}
					if (num14 == 2)
					{
						rectangle.X = 72;
						rectangle.Y = 126;
					}
					mergeDown = true;
				}
			}
			else if (num2 != -1 && num7 != -1 && num4 == type && num5 == -1)
			{
				if (num2 == -2 && num7 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 90;
						rectangle.Y = 144;
					}
					if (num14 == 1)
					{
						rectangle.X = 90;
						rectangle.Y = 162;
					}
					if (num14 == 2)
					{
						rectangle.X = 90;
						rectangle.Y = 180;
					}
					mergeUp = true;
				}
				else if (num7 == -2 && num2 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 90;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 90;
						rectangle.Y = 108;
					}
					if (num14 == 2)
					{
						rectangle.X = 90;
						rectangle.Y = 126;
					}
					mergeDown = true;
				}
			}
			else if (num2 == -1 && num7 == type && num4 != -1 && num5 != -1)
			{
				if (num4 == -2 && num5 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 0;
						rectangle.Y = 198;
					}
					if (num14 == 1)
					{
						rectangle.X = 18;
						rectangle.Y = 198;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 198;
					}
					mergeLeft = true;
				}
				else if (num5 == -2 && num4 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 198;
					}
					if (num14 == 1)
					{
						rectangle.X = 72;
						rectangle.Y = 198;
					}
					if (num14 == 2)
					{
						rectangle.X = 90;
						rectangle.Y = 198;
					}
					mergeRight = true;
				}
			}
			else if (num2 == type && num7 == -1 && num4 != -1 && num5 != -1)
			{
				if (num4 == -2 && num5 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 0;
						rectangle.Y = 216;
					}
					if (num14 == 1)
					{
						rectangle.X = 18;
						rectangle.Y = 216;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 216;
					}
					mergeLeft = true;
				}
				else if (num5 == -2 && num4 == type)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 216;
					}
					if (num14 == 1)
					{
						rectangle.X = 72;
						rectangle.Y = 216;
					}
					if (num14 == 2)
					{
						rectangle.X = 90;
						rectangle.Y = 216;
					}
					mergeRight = true;
				}
			}
			else if (num2 != -1 && num7 != -1 && num4 == -1 && num5 == -1)
			{
				if (num2 == -2 && num7 == -2)
				{
					if (num14 == 0)
					{
						rectangle.X = 108;
						rectangle.Y = 216;
					}
					if (num14 == 1)
					{
						rectangle.X = 108;
						rectangle.Y = 234;
					}
					if (num14 == 2)
					{
						rectangle.X = 108;
						rectangle.Y = 252;
					}
					mergeUp = true;
					mergeDown = true;
				}
				else if (num2 == -2)
				{
					if (num14 == 0)
					{
						rectangle.X = 126;
						rectangle.Y = 144;
					}
					if (num14 == 1)
					{
						rectangle.X = 126;
						rectangle.Y = 162;
					}
					if (num14 == 2)
					{
						rectangle.X = 126;
						rectangle.Y = 180;
					}
					mergeUp = true;
				}
				else if (num7 == -2)
				{
					if (num14 == 0)
					{
						rectangle.X = 126;
						rectangle.Y = 90;
					}
					if (num14 == 1)
					{
						rectangle.X = 126;
						rectangle.Y = 108;
					}
					if (num14 == 2)
					{
						rectangle.X = 126;
						rectangle.Y = 126;
					}
					mergeDown = true;
				}
			}
			else if (num2 == -1 && num7 == -1 && num4 != -1 && num5 != -1)
			{
				if (num4 == -2 && num5 == -2)
				{
					if (num14 == 0)
					{
						rectangle.X = 162;
						rectangle.Y = 198;
					}
					if (num14 == 1)
					{
						rectangle.X = 180;
						rectangle.Y = 198;
					}
					if (num14 == 2)
					{
						rectangle.X = 198;
						rectangle.Y = 198;
					}
					mergeLeft = true;
					mergeRight = true;
				}
				else if (num4 == -2)
				{
					if (num14 == 0)
					{
						rectangle.X = 0;
						rectangle.Y = 252;
					}
					if (num14 == 1)
					{
						rectangle.X = 18;
						rectangle.Y = 252;
					}
					if (num14 == 2)
					{
						rectangle.X = 36;
						rectangle.Y = 252;
					}
					mergeLeft = true;
				}
				else if (num5 == -2)
				{
					if (num14 == 0)
					{
						rectangle.X = 54;
						rectangle.Y = 252;
					}
					if (num14 == 1)
					{
						rectangle.X = 72;
						rectangle.Y = 252;
					}
					if (num14 == 2)
					{
						rectangle.X = 90;
						rectangle.Y = 252;
					}
					mergeRight = true;
				}
			}
			else if (num2 == -2 && num7 == -1 && num4 == -1 && num5 == -1)
			{
				if (num14 == 0)
				{
					rectangle.X = 108;
					rectangle.Y = 144;
				}
				if (num14 == 1)
				{
					rectangle.X = 108;
					rectangle.Y = 162;
				}
				if (num14 == 2)
				{
					rectangle.X = 108;
					rectangle.Y = 180;
				}
				mergeUp = true;
			}
			else if (num2 == -1 && num7 == -2 && num4 == -1 && num5 == -1)
			{
				if (num14 == 0)
				{
					rectangle.X = 108;
					rectangle.Y = 90;
				}
				if (num14 == 1)
				{
					rectangle.X = 108;
					rectangle.Y = 108;
				}
				if (num14 == 2)
				{
					rectangle.X = 108;
					rectangle.Y = 126;
				}
				mergeDown = true;
			}
			else if (num2 == -1 && num7 == -1 && num4 == -2 && num5 == -1)
			{
				if (num14 == 0)
				{
					rectangle.X = 0;
					rectangle.Y = 234;
				}
				if (num14 == 1)
				{
					rectangle.X = 18;
					rectangle.Y = 234;
				}
				if (num14 == 2)
				{
					rectangle.X = 36;
					rectangle.Y = 234;
				}
				mergeLeft = true;
			}
			else if (num2 == -1 && num7 == -1 && num4 == -1 && num5 == -2)
			{
				if (num14 == 0)
				{
					rectangle.X = 54;
					rectangle.Y = 234;
				}
				if (num14 == 1)
				{
					rectangle.X = 72;
					rectangle.Y = 234;
				}
				if (num14 == 2)
				{
					rectangle.X = 90;
					rectangle.Y = 234;
				}
				mergeRight = true;
			}
		}
		if (rectangle.X < 0 || rectangle.Y < 0)
		{
			if (type == 2)
			{
				if (num2 == -2)
				{
					num2 = type;
				}
				if (num7 == -2)
				{
					num7 = type;
				}
				if (num4 == -2)
				{
					num4 = type;
				}
				if (num5 == -2)
				{
					num5 = type;
				}
				if (num == -2)
				{
					num = type;
				}
				if (num3 == -2)
				{
					num3 = type;
				}
				if (num6 == -2)
				{
					num6 = type;
				}
				if (num8 == -2)
				{
					num8 = type;
				}
			}
			if (num2 == type && num7 == type && num4 == type && num5 == type)
			{
				if (num != type && num3 != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 108;
						rectangle.Y = 18;
					}
					if (num14 == 1)
					{
						rectangle.X = 126;
						rectangle.Y = 18;
					}
					if (num14 == 2)
					{
						rectangle.X = 144;
						rectangle.Y = 18;
					}
				}
				else if (num6 != type && num8 != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 108;
						rectangle.Y = 36;
					}
					if (num14 == 1)
					{
						rectangle.X = 126;
						rectangle.Y = 36;
					}
					if (num14 == 2)
					{
						rectangle.X = 144;
						rectangle.Y = 36;
					}
				}
				else if (num != type && num6 != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 180;
						rectangle.Y = 0;
					}
					if (num14 == 1)
					{
						rectangle.X = 180;
						rectangle.Y = 18;
					}
					if (num14 == 2)
					{
						rectangle.X = 180;
						rectangle.Y = 36;
					}
				}
				else if (num3 != type && num8 != type)
				{
					if (num14 == 0)
					{
						rectangle.X = 198;
						rectangle.Y = 0;
					}
					if (num14 == 1)
					{
						rectangle.X = 198;
						rectangle.Y = 18;
					}
					if (num14 == 2)
					{
						rectangle.X = 198;
						rectangle.Y = 36;
					}
				}
				else
				{
					if (num14 == 0)
					{
						rectangle.X = 18;
						rectangle.Y = 18;
					}
					if (num14 == 1)
					{
						rectangle.X = 36;
						rectangle.Y = 18;
					}
					if (num14 == 2)
					{
						rectangle.X = 54;
						rectangle.Y = 18;
					}
				}
			}
			else if (num2 != type && num7 == type && num4 == type && num5 == type)
			{
				if (num14 == 0)
				{
					rectangle.X = 18;
					rectangle.Y = 0;
				}
				if (num14 == 1)
				{
					rectangle.X = 36;
					rectangle.Y = 0;
				}
				if (num14 == 2)
				{
					rectangle.X = 54;
					rectangle.Y = 0;
				}
			}
			else if (num2 == type && num7 != type && num4 == type && num5 == type)
			{
				if (num14 == 0)
				{
					rectangle.X = 18;
					rectangle.Y = 36;
				}
				if (num14 == 1)
				{
					rectangle.X = 36;
					rectangle.Y = 36;
				}
				if (num14 == 2)
				{
					rectangle.X = 54;
					rectangle.Y = 36;
				}
			}
			else if (num2 == type && num7 == type && num4 != type && num5 == type)
			{
				if (num14 == 0)
				{
					rectangle.X = 0;
					rectangle.Y = 0;
				}
				if (num14 == 1)
				{
					rectangle.X = 0;
					rectangle.Y = 18;
				}
				if (num14 == 2)
				{
					rectangle.X = 0;
					rectangle.Y = 36;
				}
			}
			else if (num2 == type && num7 == type && num4 == type && num5 != type)
			{
				if (num14 == 0)
				{
					rectangle.X = 72;
					rectangle.Y = 0;
				}
				if (num14 == 1)
				{
					rectangle.X = 72;
					rectangle.Y = 18;
				}
				if (num14 == 2)
				{
					rectangle.X = 72;
					rectangle.Y = 36;
				}
			}
			else if (num2 != type && num7 == type && num4 != type && num5 == type)
			{
				if (num14 == 0)
				{
					rectangle.X = 0;
					rectangle.Y = 54;
				}
				if (num14 == 1)
				{
					rectangle.X = 36;
					rectangle.Y = 54;
				}
				if (num14 == 2)
				{
					rectangle.X = 72;
					rectangle.Y = 54;
				}
			}
			else if (num2 != type && num7 == type && num4 == type && num5 != type)
			{
				if (num14 == 0)
				{
					rectangle.X = 18;
					rectangle.Y = 54;
				}
				if (num14 == 1)
				{
					rectangle.X = 54;
					rectangle.Y = 54;
				}
				if (num14 == 2)
				{
					rectangle.X = 90;
					rectangle.Y = 54;
				}
			}
			else if (num2 == type && num7 != type && num4 != type && num5 == type)
			{
				if (num14 == 0)
				{
					rectangle.X = 0;
					rectangle.Y = 72;
				}
				if (num14 == 1)
				{
					rectangle.X = 36;
					rectangle.Y = 72;
				}
				if (num14 == 2)
				{
					rectangle.X = 72;
					rectangle.Y = 72;
				}
			}
			else if (num2 == type && num7 != type && num4 == type && num5 != type)
			{
				if (num14 == 0)
				{
					rectangle.X = 18;
					rectangle.Y = 72;
				}
				if (num14 == 1)
				{
					rectangle.X = 54;
					rectangle.Y = 72;
				}
				if (num14 == 2)
				{
					rectangle.X = 90;
					rectangle.Y = 72;
				}
			}
			else if (num2 == type && num7 == type && num4 != type && num5 != type)
			{
				if (num14 == 0)
				{
					rectangle.X = 90;
					rectangle.Y = 0;
				}
				if (num14 == 1)
				{
					rectangle.X = 90;
					rectangle.Y = 18;
				}
				if (num14 == 2)
				{
					rectangle.X = 90;
					rectangle.Y = 36;
				}
			}
			else if (num2 != type && num7 != type && num4 == type && num5 == type)
			{
				if (num14 == 0)
				{
					rectangle.X = 108;
					rectangle.Y = 72;
				}
				if (num14 == 1)
				{
					rectangle.X = 126;
					rectangle.Y = 72;
				}
				if (num14 == 2)
				{
					rectangle.X = 144;
					rectangle.Y = 72;
				}
			}
			else if (num2 != type && num7 == type && num4 != type && num5 != type)
			{
				if (num14 == 0)
				{
					rectangle.X = 108;
					rectangle.Y = 0;
				}
				if (num14 == 1)
				{
					rectangle.X = 126;
					rectangle.Y = 0;
				}
				if (num14 == 2)
				{
					rectangle.X = 144;
					rectangle.Y = 0;
				}
			}
			else if (num2 == type && num7 != type && num4 != type && num5 != type)
			{
				if (num14 == 0)
				{
					rectangle.X = 108;
					rectangle.Y = 54;
				}
				if (num14 == 1)
				{
					rectangle.X = 126;
					rectangle.Y = 54;
				}
				if (num14 == 2)
				{
					rectangle.X = 144;
					rectangle.Y = 54;
				}
			}
			else if (num2 != type && num7 != type && num4 != type && num5 == type)
			{
				if (num14 == 0)
				{
					rectangle.X = 162;
					rectangle.Y = 0;
				}
				if (num14 == 1)
				{
					rectangle.X = 162;
					rectangle.Y = 18;
				}
				if (num14 == 2)
				{
					rectangle.X = 162;
					rectangle.Y = 36;
				}
			}
			else if (num2 != type && num7 != type && num4 == type && num5 != type)
			{
				if (num14 == 0)
				{
					rectangle.X = 216;
					rectangle.Y = 0;
				}
				if (num14 == 1)
				{
					rectangle.X = 216;
					rectangle.Y = 18;
				}
				if (num14 == 2)
				{
					rectangle.X = 216;
					rectangle.Y = 36;
				}
			}
			else if (num2 != type && num7 != type && num4 != type && num5 != type)
			{
				if (num14 == 0)
				{
					rectangle.X = 162;
					rectangle.Y = 54;
				}
				if (num14 == 1)
				{
					rectangle.X = 180;
					rectangle.Y = 54;
				}
				if (num14 == 2)
				{
					rectangle.X = 198;
					rectangle.Y = 54;
				}
			}
		}
		if (rectangle.X <= -1 || rectangle.Y <= -1)
		{
			if (num14 <= 0)
			{
				rectangle.X = 18;
				rectangle.Y = 18;
			}
			if (num14 == 1)
			{
				rectangle.X = 36;
				rectangle.Y = 18;
			}
			if (num14 >= 2)
			{
				rectangle.X = 54;
				rectangle.Y = 18;
			}
		}
		Main.tile[i, j].frameX = (short)rectangle.X;
		Main.tile[i, j].frameY = (short)rectangle.Y;
		if (rectangle.X != frameX && rectangle.Y != frameY && frameX >= 0 && frameY >= 0)
		{
			bool num16 = mergeUp;
			bool flag3 = mergeDown;
			bool flag4 = mergeLeft;
			bool flag5 = mergeRight;
			TileFrame(i - 1, j);
			TileFrame(i + 1, j);
			TileFrame(i, j - 1);
			TileFrame(i, j + 1);
			mergeUp = num16;
			mergeDown = flag3;
			mergeLeft = flag4;
			mergeRight = flag5;
		}
	}

	static WorldGen_1()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		mergeUp = false;
		mergeDown = false;
		mergeLeft = false;
		mergeRight = false;
		destroyObject = false;
		bestX = 0;
		bestY = 0;
		hiScore = 0;
		lastDungeonHall = default(Vector2);
		maxDRooms = 100;
		numDRooms = 0;
		dRoomX = new int[maxDRooms];
		dRoomY = new int[maxDRooms];
		dRoomSize = new int[maxDRooms];
		dRoomTreasure = new bool[maxDRooms];
		dRoomL = new int[maxDRooms];
		dRoomR = new int[maxDRooms];
		dRoomT = new int[maxDRooms];
		dRoomB = new int[maxDRooms];
		DDoorX = new int[320];
		DDoorY = new int[320];
		DDoorPos = new int[320];
		DPlatX = new int[320];
		DPlatY = new int[320];
		JChestX = new int[320];
		JChestY = new int[320];
		numJChests = 0;
		dEnteranceX = 0;
		dSurface = false;
		numIslandHouses = 0;
		houseCount = 0;
		fihX = new int[300];
		fihY = new int[300];
		numMCaves = 0;
		mCaveX = new int[300];
		mCaveY = new int[300];
		JungleX = 0;
		hellChest = 0;
		mudWall = false;
		lastMaxTilesX = 0;
		lastMaxTilesY = 0;
	}
}
