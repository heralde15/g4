using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.ZazaMod.LegacyWorldGen;

internal class WorldGen_PreAlpha
{
	private static bool mergeUp;

	private static bool mergeDown;

	private static bool mergeLeft;

	private static bool mergeRight;

	private static bool destroyObject;

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
		int num = 0;
		int num2 = 0;
		double num3 = (double)Main.maxTilesY * 0.35;
		num3 *= (double)WorldGen._genRand.Next(90, 110) * 0.01;
		double num4 = num3 + (double)Main.maxTilesY * 0.15;
		num4 *= (double)WorldGen._genRand.Next(90, 110) * 0.01;
		double num5 = num3;
		double num6 = num3;
		double num7 = num4;
		double num8 = num4;
		int num1086 = 86400;
		Main.slimeRainTime = -WorldGen.genRand.Next(num1086 * 2, num1086 * 3);
		Main.cloudBGActive = -WorldGen.genRand.Next(8640, 86400);
		WorldGen.SavedOreTiers.Copper = 7;
		WorldGen.SavedOreTiers.Iron = 6;
		WorldGen.SavedOreTiers.Silver = 9;
		WorldGen.SavedOreTiers.Gold = 8;
		WorldGen.crimson = false;
		Main.worldID = WorldGen.genRand.Next(int.MaxValue);
		WorldGen.RandomizeTreeStyle();
		WorldGen.RandomizeCaveBackgrounds();
		WorldGen.RandomizeBackgrounds(WorldGen.genRand);
		WorldGen.RandomizeMoonState(WorldGen.genRand);
		WorldGen.TreeTops.CopyExistingWorldInfoForWorldGeneration();
		WorldGen.noTileActions = true;
		WorldGen.AddGenerationPass("Shaping Classic World", delegate(GenerationProgress progress, GameConfiguration passConfig)
		{
			progress.Message = "Shaping Classic World...";
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
					num = WorldGen._genRand.Next(0, 5);
					num2 = WorldGen._genRand.Next(5, 40);
					if (num == 0)
					{
						num2 *= (int)((double)WorldGen._genRand.Next(15, 30) * 0.1);
					}
				}
				num2--;
				if (num == 0)
				{
					while (WorldGen._genRand.Next(0, 7) == 0)
					{
						num3 += WorldGen._genRand.Next(-1, 2);
					}
				}
				else if (num == 1)
				{
					while (WorldGen._genRand.Next(0, 4) == 0)
					{
						num3--;
					}
					while (WorldGen._genRand.Next(0, 10) == 0)
					{
						num3++;
					}
				}
				else if (num == 2)
				{
					while (WorldGen._genRand.Next(0, 4) == 0)
					{
						num3++;
					}
					while (WorldGen._genRand.Next(0, 10) == 0)
					{
						num3--;
					}
				}
				else if (num == 3)
				{
					while (WorldGen._genRand.Next(0, 2) == 0)
					{
						num3--;
					}
					while (WorldGen._genRand.Next(0, 6) == 0)
					{
						num3++;
					}
				}
				else if (num == 4)
				{
					while (WorldGen._genRand.Next(0, 2) == 0)
					{
						num3++;
					}
					while (WorldGen._genRand.Next(0, 5) == 0)
					{
						num3--;
					}
				}
				if (num3 < 125.05000000000001)
				{
					num3 = 125.05000000000001;
					num2 = 0;
				}
				else if (num3 > 2375.95)
				{
					num3 = 2375.95;
					num2 = 0;
				}
				while (WorldGen._genRand.Next(0, 3) == 0)
				{
					num4 += WorldGen._genRand.Next(-2, 3);
				}
				if (num4 < num3 + 125.05000000000001)
				{
					num4++;
				}
				if (num4 > num3 + 625.25)
				{
					num4--;
				}
				for (int j = 0; (double)j < num3; j++)
				{
					Main.tile[i, j].active(active: false);
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
			Main.worldSurface = (int)num6;
			Main.rockLayer = num8;
			double num1087 = (int)((Main.rockLayer - Main.worldSurface) / 6.0) * 6;
			Main.rockLayer = (int)(Main.worldSurface + num1087);
			for (int l = 0; l < 25015; l++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num5, (int)num8 + 1), WorldGen._genRand.Next(2, 7), WorldGen._genRand.Next(2, 23), 1);
			}
			for (int m = 0; m < 62537; m++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num7, Main.maxTilesY), WorldGen._genRand.Next(2, 6), WorldGen._genRand.Next(2, 40), 0);
			}
			for (int n = 0; n < 12507; n++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num6, Main.maxTilesY), WorldGen._genRand.Next(2, 5), WorldGen._genRand.Next(2, 20), -1);
			}
			for (int num1088 = 0; num1088 < 12507; num1088++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num6, Main.maxTilesY), WorldGen._genRand.Next(8, 15), WorldGen._genRand.Next(7, 30), -1);
			}
			for (int num1089 = 0; num1089 < Main.maxTilesY; num1089++)
			{
				if (num8 <= (double)Main.maxTilesY)
				{
					TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num8, Main.maxTilesY), WorldGen._genRand.Next(10, 20), WorldGen._genRand.Next(50, 300), -1);
				}
			}
			int num1090 = 0;
			for (int num1091 = 0; num1091 < 25; num1091++)
			{
				num1090 = WorldGen._genRand.Next(0, Main.maxTilesX);
				for (int num1092 = 0; (double)num1092 < num6; num1092++)
				{
					if (Main.tile[num1090, num1092].active())
					{
						TileRunner(num1090, num1092, WorldGen._genRand.Next(3, 6), WorldGen._genRand.Next(5, 50), -1, addTile: false, (float)WorldGen._genRand.Next(-10, 11) * 0.1f, 1f);
						break;
					}
				}
			}
			for (int num1093 = 0; num1093 < 40; num1093++)
			{
				num1090 = WorldGen._genRand.Next(0, Main.maxTilesX);
				for (int num1094 = 0; (double)num1094 < num6; num1094++)
				{
					if (Main.tile[num1090, num1094].active())
					{
						TileRunner(num1090, num1094, WorldGen._genRand.Next(10, 15), WorldGen._genRand.Next(50, 130), -1, addTile: false, (float)WorldGen._genRand.Next(-10, 11) * 0.1f, 1f);
						break;
					}
				}
			}
			for (int num1095 = 0; num1095 < 10; num1095++)
			{
				num1090 = WorldGen._genRand.Next(0, Main.maxTilesX);
				for (int num1096 = 0; (double)num1096 < num6; num1096++)
				{
					if (Main.tile[num1090, num1096].active())
					{
						TileRunner(num1090, num1096, WorldGen._genRand.Next(12, 25), WorldGen._genRand.Next(100, 400), -1, addTile: false, (float)WorldGen._genRand.Next(-10, 11) * 0.1f, 3f);
						TileRunner(num1090, num1096, WorldGen._genRand.Next(8, 17), WorldGen._genRand.Next(60, 200), -1, addTile: false, (float)WorldGen._genRand.Next(-10, 11) * 0.1f, 2f);
						TileRunner(num1090, num1096, WorldGen._genRand.Next(5, 13), WorldGen._genRand.Next(40, 170), -1, addTile: false, (float)WorldGen._genRand.Next(-10, 11) * 0.1f, 2f);
						break;
					}
				}
			}
			progress.Set(0.66);
			for (int num1097 = 0; num1097 < 12507; num1097++)
			{
				int num1098 = WorldGen._genRand.Next(1, Main.maxTilesX - 1);
				int num1099 = WorldGen._genRand.Next((int)num5, (int)num6);
				if (num1099 >= Main.maxTilesY)
				{
					num1099 = 2499;
				}
				if (Main.tile[num1098 - 1, num1099].active() && Main.tile[num1098 - 1, num1099].type == 0 && Main.tile[num1098 + 1, num1099].active() && Main.tile[num1098 + 1, num1099].type == 0 && Main.tile[num1098, num1099 - 1].active() && Main.tile[num1098, num1099 - 1].type == 0 && Main.tile[num1098, num1099 + 1].active() && Main.tile[num1098, num1099 + 1].type == 0)
				{
					Main.tile[num1098, num1099].active(active: true);
					Main.tile[num1098, num1099].type = 2;
				}
			}
			for (int num1100 = 0; num1100 < 625; num1100++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num5, (int)num6), WorldGen._genRand.Next(3, 4), WorldGen._genRand.Next(2, 5), 7);
			}
			for (int num1101 = 0; num1101 < 750; num1101++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num6, (int)num8), WorldGen._genRand.Next(3, 6), WorldGen._genRand.Next(3, 6), 7);
			}
			for (int num1102 = 0; num1102 < 3752; num1102++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num7, Main.maxTilesY), WorldGen._genRand.Next(4, 9), WorldGen._genRand.Next(4, 8), 7);
			}
			for (int num1103 = 0; num1103 < 500; num1103++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num5, (int)num6), WorldGen._genRand.Next(3, 4), WorldGen._genRand.Next(2, 5), 6);
			}
			for (int num1104 = 0; num1104 < 625; num1104++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num6, (int)num8), WorldGen._genRand.Next(3, 6), WorldGen._genRand.Next(3, 6), 6);
			}
			for (int num1105 = 0; num1105 < Main.maxTilesY; num1105++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num7, Main.maxTilesY), WorldGen._genRand.Next(4, 9), WorldGen._genRand.Next(4, 8), 6);
			}
			for (int num1106 = 0; num1106 < 125; num1106++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num5, (int)num6), WorldGen._genRand.Next(3, 4), WorldGen._genRand.Next(2, 5), 9);
			}
			for (int num1107 = 0; num1107 < 250; num1107++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num6, (int)num8), WorldGen._genRand.Next(3, 6), WorldGen._genRand.Next(3, 6), 9);
			}
			for (int num1108 = 0; num1108 < 1250; num1108++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num7, Main.maxTilesY), WorldGen._genRand.Next(4, 9), WorldGen._genRand.Next(4, 8), 9);
			}
			for (int num1109 = 0; num1109 < 62; num1109++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num5, (int)num6), WorldGen._genRand.Next(3, 4), WorldGen._genRand.Next(2, 5), 8);
			}
			for (int num1110 = 0; num1110 < 125; num1110++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num6, (int)num8), WorldGen._genRand.Next(3, 6), WorldGen._genRand.Next(3, 6), 8);
			}
			for (int num1111 = 0; num1111 < 625; num1111++)
			{
				TileRunner(WorldGen._genRand.Next(0, Main.maxTilesX), WorldGen._genRand.Next((int)num7, Main.maxTilesY), WorldGen._genRand.Next(4, 9), WorldGen._genRand.Next(4, 8), 8);
			}
			progress.Set(0.99);
			for (int num1112 = 0; num1112 < Main.maxTilesX; num1112++)
			{
				num1090 = num1112;
				for (int num1113 = 0; (double)num1113 < Main.worldSurface - 1.0; num1113++)
				{
					if (Main.tile[num1090, num1113].active())
					{
						if (Main.tile[num1090, num1113].type == 0)
						{
							SpreadGrass(num1090, num1113);
						}
						break;
					}
				}
			}
		});
		WorldGen.AddGenerationPass("Planting Trees", delegate(GenerationProgress progress, GameConfiguration passConfig)
		{
			progress.Set(1.0);
			progress.Message = "Planting Trees...";
			AddTrees();
		});
		WorldGen.AddGenerationPass("Tile Framing", delegate(GenerationProgress progress, GameConfiguration passConfig)
		{
			progress.Set(0.5);
			progress.Message = "Tile Framing...";
			EveryTileFrame();
		});
		WorldGen.AddGenerationPass("Adding Plants", delegate(GenerationProgress progress, GameConfiguration passConfig)
		{
			progress.Set(1.0);
			progress.Message = "Adding Plants...";
			AddPlants();
		});
		WorldGen.AddGenerationPass("Spawn Point", delegate(GenerationProgress progress, GameConfiguration passConfig)
		{
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			progress.Set(1.0);
			int num1087 = 5;
			bool flag = true;
			int num1088 = Main.maxTilesX / 2;
			while (flag)
			{
				int value = num1088 + WorldGen.genRand.Next(-num1087, num1087 + 1);
				value = Math.Clamp(value, 40, Main.maxTilesX - 40);
				for (int i = 0; i < Main.maxTilesY; i++)
				{
					if (WorldGen.InWorld(value, i, 40) && Main.tile[value, i].active())
					{
						Main.spawnTileX = value;
						Main.spawnTileY = i;
						break;
					}
				}
				flag = false;
				num1087++;
				if (num1087 > 10000)
				{
					Main.spawnTileX = Main.maxTilesX / 2;
					for (int j = 40; (double)j < Main.worldSurface; j++)
					{
						if (Main.tile[Main.spawnTileX, j].active() && Collision.IsWorldPointSolid(new Vector2((float)(Main.spawnTileX * 16), (float)(j * 16))))
						{
							Main.spawnTileY = j;
							break;
						}
						if ((double)j == Main.worldSurface - 1.0)
						{
							Main.spawnTileY = j;
						}
					}
					break;
				}
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
				int num1090 = WorldGen.genRand.Next(num1088 - num1089, num1088 + num1089);
				for (int k = 0; k < Main.maxTilesY; k++)
				{
					if (Main.tile[num1090, k].active())
					{
						Main.spawnTileX = num1090;
						Main.spawnTileY = k;
						break;
					}
				}
				num1089++;
			}
		});
		WorldGen.AddGenerationPass("Finalizing", delegate(GenerationProgress progress, GameConfiguration passConfig)
		{
			progress.Message = "Finalizing";
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

	public static bool EmptyTileCheck(int startX, int endX, int startY, int endY)
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
				if (Main.tile[i, j].active())
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

	public static void PlaceTile(int i, int j, int type, bool mute = false)
	{
		if (i < 0 || j < 0 || i >= Main.maxTilesX || j >= Main.maxTilesY || !Collision.EmptyTile(i, j))
		{
			return;
		}
		Main.tile[i, j].frameY = 0;
		Main.tile[i, j].frameX = 0;
		switch (type)
		{
		case 3:
			if (j + 1 < Main.maxTilesY && Main.tile[i, j + 1].type == 2 && Main.tile[i, j + 1].active())
			{
				if (WorldGen._genRand.Next(50) == 0)
				{
					Main.tile[i, j].active(active: true);
					Main.tile[i, j].type = 3;
					Main.tile[i, j].frameX = 144;
				}
				else if (WorldGen._genRand.Next(35) == 0)
				{
					Main.tile[i, j].active(active: true);
					Main.tile[i, j].type = 3;
					Main.tile[i, j].frameX = (short)(WorldGen._genRand.Next(2) * 18 + 108);
				}
				else
				{
					Main.tile[i, j].active(active: true);
					Main.tile[i, j].type = 3;
					Main.tile[i, j].frameX = (short)(WorldGen._genRand.Next(6) * 18);
				}
			}
			break;
		case 4:
			if ((Main.tile[i - 1, j].active() && (Main.tileSolid[Main.tile[i - 1, j].type] || (Main.tile[i - 1, j].type == 5 && Main.tile[i - 1, j - 1].type == 5 && Main.tile[i - 1, j + 1].type == 5))) || (Main.tile[i + 1, j].active() && (Main.tileSolid[Main.tile[i + 1, j].type] || (Main.tile[i + 1, j].type == 5 && Main.tile[i + 1, j - 1].type == 5 && Main.tile[i + 1, j + 1].type == 5))) || (Main.tile[i, j + 1].active() && Main.tileSolid[Main.tile[i, j + 1].type]))
			{
				Main.tile[i, j].active(active: true);
				Main.tile[i, j].type = (byte)type;
				SquareTileFrame(i, j);
			}
			break;
		case 10:
			if (!Main.tile[i, j - 1].active() && !Main.tile[i, j - 2].active() && Main.tile[i, j - 3].active() && Main.tileSolid[Main.tile[i, j - 3].type])
			{
				PlaceDoor(i, j - 1, type);
				SquareTileFrame(i, j);
				break;
			}
			if (!Main.tile[i, j + 1].active() && !Main.tile[i, j + 2].active() && Main.tile[i, j + 3].active() && Main.tileSolid[Main.tile[i, j + 3].type])
			{
				PlaceDoor(i, j + 1, type);
				SquareTileFrame(i, j);
				break;
			}
			return;
		default:
			Main.tile[i, j].active(active: true);
			Main.tile[i, j].type = (byte)type;
			SquareTileFrame(i, j);
			break;
		}
		if (Main.tile[i, j].active() && !mute)
		{
			WorldGen._genRand.Next(3);
		}
	}

	public static void KillTile(int i, int j, bool fail = false)
	{
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		if (i < 0 || j < 0 || i >= Main.maxTilesX || j >= Main.maxTilesY || !Main.tile[i, j].active() || (j >= 1 && Main.tile[i, j - 1].active() && Main.tile[i, j - 1].type == 5 && Main.tile[i, j].type != 5 && (Main.tile[i, j - 1].frameX != 66 || Main.tile[i, j - 1].frameY < 0 || Main.tile[i, j - 1].frameY > 44) && (Main.tile[i, j - 1].frameX != 88 || Main.tile[i, j - 1].frameY < 66 || Main.tile[i, j - 1].frameY > 110) && Main.tile[i, j - 1].frameY < 198))
		{
			return;
		}
		int num2 = 10;
		if (fail)
		{
			num2 = 3;
		}
		for (int k = 0; k < num2; k++)
		{
			int type = 0;
			if (Main.tile[i, j].type == 0)
			{
				type = 0;
			}
			if (Main.tile[i, j].type == 1)
			{
				type = 1;
			}
			if (Main.tile[i, j].type == 4)
			{
				type = 6;
			}
			if (Main.tile[i, j].type == 5 || Main.tile[i, j].type == 10 || Main.tile[i, j].type == 11)
			{
				type = 7;
			}
			if (Main.tile[i, j].type == 2)
			{
				type = ((WorldGen._genRand.Next(2) != 0) ? 2 : 0);
			}
			if (Main.tile[i, j].type == 6)
			{
				type = 8;
			}
			if (Main.tile[i, j].type == 7)
			{
				type = 9;
			}
			if (Main.tile[i, j].type == 8)
			{
				type = 10;
			}
			if (Main.tile[i, j].type == 9)
			{
				type = 11;
			}
			if (Main.tile[i, j].type == 3)
			{
				type = 3;
			}
			Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, type);
		}
		if (fail)
		{
			if (Main.tile[i, j].type == 2)
			{
				Main.tile[i, j].type = 0;
			}
			SquareTileFrame(i, j);
		}
		else
		{
			Main.tile[i, j].active(active: false);
			Main.tile[i, j].frameX = -1;
			Main.tile[i, j].frameY = -1;
			Main.tile[i, j].frameNumber(0);
			Main.tile[i, j].type = 0;
			SquareTileFrame(i, j);
		}
	}

	public static void AddPlants()
	{
		try
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 1; j < Main.maxTilesY; j++)
				{
					if (Main.tile[i, j].type == 2 && Main.tile[i, j].active() && !Main.tile[i, j - 1].active())
					{
						PlaceTile(i, j - 1, 3, mute: true);
					}
				}
			}
		}
		catch
		{
		}
	}

	public static void SpreadGrass(int i, int j, int dirt = 0, int grass = 2, bool repeat = true)
	{
		if (Main.tile[i, j].type != dirt || !Main.tile[i, j].active() || (double)j >= Main.worldSurface)
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
				if (!Main.tile[k, l].active())
				{
					flag = false;
					break;
				}
			}
		}
		if (flag)
		{
			return;
		}
		Main.tile[i, j].type = (byte)grass;
		for (int m = num; m < num2; m++)
		{
			for (int n = num3; n < num4; n++)
			{
				if ((Main.tile[m, n].active() && Main.tile[m, n].type == dirt) & repeat)
				{
					SpreadGrass(m, n, dirt, grass);
				}
			}
		}
	}

	public static void TileRunner(int i, int j, double strength, int steps, int type, bool addTile = false, float speedX = 0f, float speedY = 0f)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		double num = strength;
		float num2 = steps;
		Vector2 vector = new Vector2
		{
			X = i,
			Y = j
		};
		Vector2 vector2 = new Vector2
		{
			X = (float)WorldGen._genRand.Next(-10, 11) * 0.1f,
			Y = (float)WorldGen._genRand.Next(-10, 11) * 0.1f
		};
		if (speedX != 0f || speedY != 0f)
		{
			vector2.X = speedX;
			vector2.Y = speedY;
		}
		while (num > 0.0 && num2 > 0f)
		{
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
					if (!((double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < strength * 0.5 * (1.0 + (double)WorldGen._genRand.Next(-10, 11) * 0.015)))
					{
						continue;
					}
					if (type == -1)
					{
						Main.tile[k, l].active(active: false);
						continue;
					}
					if (addTile)
					{
						Main.tile[k, l].active(active: true);
					}
					Main.tile[k, l].type = (byte)type;
				}
			}
			vector += vector2;
			vector2.X += (float)WorldGen._genRand.Next(-10, 11) * 0.05f;
			vector2.Y += (float)WorldGen._genRand.Next(-10, 11) * 0.05f;
			if (vector2.X > 1f)
			{
				vector2.X = 1f;
			}
			if (vector2.X < -1f)
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
		//IL_38dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4956: Unknown result type (might be due to invalid IL or missing references)
		//IL_38e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4960: Unknown result type (might be due to invalid IL or missing references)
		//IL_515e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5168: Unknown result type (might be due to invalid IL or missing references)
		//IL_51cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_51ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_51f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_51fe: Unknown result type (might be due to invalid IL or missing references)
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
}
