using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.DataStructures;

namespace Terraria.WorldBuilding;

public static class GenVars
{
	public static WorldGenConfiguration configuration;

	public static StructureMap structures;

	public static int copper;

	public static int iron;

	public static int silver;

	public static int gold;

	public static int copperBar;

	public static int ironBar;

	public static int silverBar;

	public static int goldBar;

	public static ushort mossTile;

	public static ushort mossWall;

	public static int lavaLine;

	public static int waterLine;

	public static double worldSurfaceLow;

	public static double worldSurface;

	public static double worldSurfaceHigh;

	public static double rockLayerLow;

	public static double rockLayer;

	public static double rockLayerHigh;

	public static int snowTop;

	public static int snowBottom;

	public static int snowOriginLeft;

	public static int snowOriginRight;

	public static int[] snowMinX;

	public static int[] snowMaxX;

	public static int leftBeachEnd;

	public static int rightBeachStart;

	public static int beachBordersWidth;

	public static int beachSandRandomCenter;

	public static int beachSandRandomWidthRange;

	public static int beachSandDungeonExtraWidth;

	public static int beachSandJungleExtraWidth;

	public static int shellStartXLeft;

	public static int shellStartYLeft;

	public static int shellStartXRight;

	public static int shellStartYRight;

	public static int oceanWaterStartRandomMin;

	public static int oceanWaterStartRandomMax;

	public static int oceanWaterForcedJungleLength;

	public static int evilBiomeBeachAvoidance;

	public static int evilBiomeAvoidanceMidFixer;

	public static int lakesBeachAvoidance;

	public static int smallHolesBeachAvoidance;

	public static int surfaceCavesBeachAvoidance;

	public static int surfaceCavesBeachAvoidance2;

	public static readonly int maxOceanCaveTreasure;

	public static int numOceanCaveTreasure;

	public static Point[] oceanCaveTreasure;

	public static bool skipDesertTileCheck;

	public static Rectangle UndergroundDesertLocation;

	public static Rectangle UndergroundDesertHiveLocation;

	public static int desertHiveHigh;

	public static int desertHiveLow;

	public static int desertHiveLeft;

	public static int desertHiveRight;

	public static int numLarva;

	public static int[] larvaY;

	public static int[] larvaX;

	public static int numPyr;

	public static int[] PyrX;

	public static int[] PyrY;

	public static int extraBastStatueCount;

	public static int extraBastStatueCountMax;

	public static int jungleOriginX;

	public static int jungleMinX;

	public static int jungleMaxX;

	public static int JungleX;

	public static ushort jungleHut;

	public static bool mudWall;

	public static int JungleItemCount;

	public static int[] JChestX;

	public static int[] JChestY;

	public static int numJChests;

	public static int tLeft;

	public static int tRight;

	public static int tTop;

	public static int tBottom;

	public static int tRooms;

	public static int lAltarX;

	public static int lAltarY;

	public static int dungeonSide;

	public static int dungeonLocation;

	public static bool dungeonLake;

	public static ushort crackedType;

	public static int dungeonX;

	public static int dungeonY;

	public static Vector2D lastDungeonHall;

	public static readonly int maxDRooms;

	public static int numDRooms;

	public static int[] dRoomX;

	public static int[] dRoomY;

	public static int[] dRoomSize;

	public static bool[] dRoomTreasure;

	public static int[] dRoomL;

	public static int[] dRoomR;

	public static int[] dRoomT;

	public static int[] dRoomB;

	public static int numDDoors;

	public static int[] DDoorX;

	public static int[] DDoorY;

	public static int[] DDoorPos;

	public static int numDungeonPlatforms;

	public static int[] dungeonPlatformX;

	public static int[] dungeonPlatformY;

	public static int dEnteranceX;

	public static bool dSurface;

	public static double dxStrength1;

	public static double dyStrength1;

	public static double dxStrength2;

	public static double dyStrength2;

	public static int dMinX;

	public static int dMaxX;

	public static int dMinY;

	public static int dMaxY;

	public static int skyLakes;

	public static bool generatedShadowKey;

	public static int numIslandHouses;

	public static int skyIslandHouseCount;

	public static bool[] skyLake;

	public static int[] floatingIslandHouseX;

	public static int[] floatingIslandHouseY;

	public static int[] floatingIslandStyle;

	public static int numMCaves;

	public static int[] mCaveX;

	public static int[] mCaveY;

	public static readonly int maxTunnels;

	public static int numTunnels;

	public static int[] tunnelX;

	public static readonly int maxOrePatch;

	public static int numOrePatch;

	public static int[] orePatchX;

	public static readonly int maxMushroomBiomes;

	public static int numMushroomBiomes;

	public static Point[] mushroomBiomesPosition;

	public static int logX;

	public static int logY;

	public static readonly int maxLakes;

	public static int numLakes;

	public static int[] LakeX;

	public static readonly int maxOasis;

	public static int numOasis;

	public static Point[] oasisPosition;

	public static int[] oasisWidth;

	public static readonly int oasisHeight;

	public static int hellChest;

	public static int[] hellChestItem;

	public static Point16[] statueList;

	public static List<int> StatuesWithTraps;

	public static bool crimsonLeft;

	public static Vector2D shimmerPosition;

	static GenVars()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		copperBar = 20;
		ironBar = 22;
		silverBar = 21;
		goldBar = 19;
		mossTile = 179;
		mossWall = 54;
		maxOceanCaveTreasure = 2;
		numOceanCaveTreasure = 0;
		oceanCaveTreasure = (Point[])(object)new Point[maxOceanCaveTreasure];
		skipDesertTileCheck = false;
		UndergroundDesertLocation = Rectangle.Empty;
		UndergroundDesertHiveLocation = Rectangle.Empty;
		larvaY = new int[100];
		larvaX = new int[100];
		JChestX = new int[100];
		JChestY = new int[100];
		crackedType = 481;
		lastDungeonHall = Vector2D.Zero;
		maxDRooms = 100;
		dRoomX = new int[maxDRooms];
		dRoomY = new int[maxDRooms];
		dRoomSize = new int[maxDRooms];
		dRoomTreasure = new bool[maxDRooms];
		dRoomL = new int[maxDRooms];
		dRoomR = new int[maxDRooms];
		dRoomT = new int[maxDRooms];
		dRoomB = new int[maxDRooms];
		DDoorX = new int[500];
		DDoorY = new int[500];
		DDoorPos = new int[500];
		dungeonPlatformX = new int[500];
		dungeonPlatformY = new int[500];
		skyLake = new bool[30];
		floatingIslandHouseX = new int[30];
		floatingIslandHouseY = new int[30];
		floatingIslandStyle = new int[30];
		mCaveX = new int[30];
		mCaveY = new int[30];
		maxTunnels = 50;
		tunnelX = new int[maxTunnels];
		maxOrePatch = 50;
		orePatchX = new int[maxOrePatch];
		maxMushroomBiomes = 50;
		numMushroomBiomes = 0;
		mushroomBiomesPosition = (Point[])(object)new Point[maxMushroomBiomes];
		maxLakes = 50;
		numLakes = 0;
		LakeX = new int[maxLakes];
		maxOasis = 20;
		numOasis = 0;
		oasisPosition = (Point[])(object)new Point[maxOasis];
		oasisWidth = new int[maxOasis];
		oasisHeight = 20;
		StatuesWithTraps = new List<int>(new int[4] { 4, 7, 10, 18 });
		crimsonLeft = true;
	}
}
