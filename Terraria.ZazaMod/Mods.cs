using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ReLogic.OS;
using ReLogic.Utilities;
using SkiaSharp;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.Cinematics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.GameContent.Items;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Chat;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ObjectData;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using Terraria.ZazaMod.CheatSystem;
using Terraria.ZazaMod.ItemModSystem;
using Terraria.ZazaMod.UI;
using Terraria.ZazaMod.UI.Elements;
using Terraria.ZazaMod.UI.Legacy;
using Terraria.ZazaMod.WorldEdit;

namespace Terraria.ZazaMod;

public class Mods
{
	public static Mods Instance;

	public static Preferences ZazaConfiguration;

	public static string zazaErrorTxt;

	public static bool rememberEnabled;

	public static char commandSpecifier;

	public static bool hideHackedMOTD;

	public const string zazaVersion = "ZazaMod 1.3.7.1.4";

	public static Color zazasFavoriteColor;

	public static Color errorColor;

	public static Color infoColor;

	public const string godModeKeyName = "God Mode";

	public const string mouseTpKeyName = "Mouse TP";

	public const string mapTpKeyName = "Map TP";

	public const string openMenuKeyName = "Open Menu";

	public const string noclipKeyName = "Noclip";

	public const string noclipSpeedKeyName = "Noclip Speed Modifier";

	public const string ghostKeyName = "Ghost";

	public const string holdActivatorKeyName = "Hold for Activator";

	public const string holdActivatorMapKeyName = "Hold for Activator (Map)";

	public const string holdRapidfireKeyName = "Hold for Rapidfire";

	public const string useBrushKeyName = "Use Brush";

	public const string useBrushMapKeyName = "Use Brush (Map)";

	public const string loadConfigKeyName = "Load Config";

	public const string openChatKeyName = "Open Chat";

	public const string loadItemModFromPresetName = "Apply Item Mod";

	public const string repeatLastCommandName = "Repeat Last Command";

	public const string bindableCommand1Name = "Bindable Command 1";

	public const string bindableCommand2Name = "Bindable Command 2";

	public const string bindableCommand3Name = "Bindable Command 3";

	public const string bindableCommand4Name = "Bindable Command 4";

	public const string bindableCommand5Name = "Bindable Command 5";

	public const string bindableCommand6Name = "Bindable Command 6";

	public const string bindableCommand7Name = "Bindable Command 7";

	public const string bindableCommand8Name = "Bindable Command 8";

	public static string cBack;

	public static string cEnter;

	public static string cDelete;

	public static string cOpenChat;

	public static Keys BackspaceKey;

	public static Keys SubmitTextKey;

	public static Keys AltBackspaceKey;

	public static Keys OpenChatKey;

	public static bool serverInSameProcess;

	public static bool superFastSplash;

	public static bool backgroundMusic;

	public static bool noclip;

	public static int noclipSpeed;

	public static bool gotoMouse;

	public static bool journeyMenu;

	public static bool hackedMOTD;

	public static bool playerTracker;

	public static bool fullBright;

	public static Vector3 fullBrightBrightness;

	public static bool mapTeleportation;

	public static bool autoButcher;

	public static short autoButcherDamage;

	public static bool revealMapCooldown;

	public static bool chatSpammer;

	public static string chatSpammerString;

	public static bool showUI;

	public static bool hideCustomUI;

	public static bool builderMenu;

	public static List<LegacyTile> copiedTiles;

	public static int tileLoopCount;

	public static Point copyStartA;

	public static bool edgeA;

	public static bool edgeB;

	public static bool copyMode;

	public static bool pasteMode;

	public static bool brushMode;

	public static Point selectionA;

	public static Point selectionB;

	public static bool selectionMode;

	public static bool selectionModeInfo;

	public static Point copyStartB;

	public static Item[] inventoryBackUp;

	public static Item[] armorAndAccessoriesBackUp;

	public static Item[] dyeBackUp;

	public static Item[] miscsBackUp;

	public static Item[] miscsDyesBackUp;

	public static int helthBackUp;

	public static int manaBackUp;

	public static Color hairColorBackUp;

	public static Color skinColorBackUp;

	public static Color eyeColorBackUp;

	public static Color shirtColorBackUp;

	public static Color underShirtColorBackUp;

	public static Color pantsrColorBackUp;

	public static Color shoeColorBackUp;

	public static int hairBackUp;

	public static bool genderBackUp;

	public static Rectangle[] plrInListHitBoxes;

	public static Rectangle[] plrItemSlotsHitBox;

	public static Rectangle[] plrArmorSlotsHitBox;

	public static bool tileBrush;

	public static bool liquidBrush;

	public static bool eraseBrush;

	public static Vector2 tileBrushFalseCoords;

	public static int tileBrushSize;

	public static int eraseBrushSize;

	public static int liquidBrushSize;

	public static bool wallBrush;

	public static int wallBrushSize;

	public static bool infiniteMana;

	public static bool showListOrPlayerInfo;

	public static bool bossAndWorldProgression;

	public static int storePlrWhoAmI;

	public static bool isAppearanceBackedUp;

	public static bool isInventoryBackedUp;

	public static bool spectatingPlayer;

	public static int spectatePlayerTarget;

	public static bool showPlayerInfo;

	public static bool showPlayerList;

	public static bool playerInspection;

	public static Point pasteImagePreviewPos;

	public static bool motionlessPaste;

	public static Point pasteImagePreviewPos2;

	public static bool commandPannel;

	public static bool commandPanelPlayerButton;

	public static bool commandPanelTravelButton;

	public static bool commandPanelBuildMenuButton;

	public static bool instantRespawn;

	public static bool infiniteReach;

	public static bool commandPanelMiscsButton;

	public static bool crystalKillAura;

	public static int projectileWhoAmISlot;

	public static bool tildeTerminal;

	public static bool autoKillProjectiles;

	public static float panelWidth;

	public static float panelHeight;

	public static int panelPageNumber;

	public static bool eraseBrushMap;

	public static int eraseBrushMapSize;

	public static bool selectionUseTileSync;

	public static bool tilePacketMaxMode;

	public static int tileSafetyIterations;

	public static int tilePacketRate;

	public static int erasePacketRate;

	public static int liquidPacketRate;

	public static int paintPacketRate;

	public static int tilePaintBrushSize;

	public static byte tilePaintBrushPaintID;

	public static bool tilePaintBrush;

	public static bool rainbowPaintDeep;

	public static int rainbowPaintType;

	public static short paintOnlyTile;

	public static short paintOnlyWall;

	[Old("Deprecated. Use tilesToBrush.")]
	public static ConcurrentDictionary<Point, int> markedTilesForTilePaintBrush;

	public static List<Color> pixelColors;

	public static bool drawPhotoMode;

	public static int drawPhotoWidth;

	public static int drawPhotoHeight;

	public static int drawPhotoType;

	public static List<string> pixelColorsInHex;

	private static Dictionary<int, short> tileIDToItemID;

	private static Dictionary<int, int> indexToTileID;

	public static Color[] hexValues;

	public static List<Schematics.ColorTable> wallColors;

	public static bool handlingPackets;

	public static bool runningBrushThread;

	public static bool runningBrushMapThread;

	[Old("Deprecated. Use tilesToBrush.")]
	public static ConcurrentDictionary<Point, Point> markedTilesForTileBrush;

	[Old("Deprecated. Use tilesToBrush.")]
	public static ConcurrentDictionary<Point, int> markedTilesForEraseBrush;

	[Old("Deprecated. Use tilesToBrush.")]
	public static int savedSelectedItemForTileBrush;

	[Old("Deprecated. Use tilesToBrush.")]
	public static int savedCreateTileForTileBrush;

	[Old("Deprecated. Use tilesToBrush.")]
	public static int savedPlaceStyleForTileBrush;

	public static bool tileBrushMap;

	public static int tileBrushMapSize;

	public static bool spamLucyMessage;

	public static bool[] annoyPlayerV2;

	public static bool[] annoyPlayerShimmer;

	public static bool[] annoyPlayer3;

	public static Vector2 storedPlayerListPos;

	public static UserInterface customUI;

	public static int quedPackets;

	public static bool commandPanelSelectedItemButton;

	public static bool blockTownNPCs;

	public static bool killAura;

	public static int killAuraSize;

	public static bool infiniteMinions;

	public static bool infinitePickUpRange;

	public static bool extendedPickUpRange;

	public static bool extendedPlacementRange;

	[Old("Deprecated. Use tilesToBrush.")]
	public static ConcurrentDictionary<Point, int> markedTilesForWallBrush;

	private static Dictionary<int, short> nonSolidTileIDToItemID;

	public static Dictionary<int, int> indexToNonSolidTileID;

	private static Dictionary<int, short> wallIDToItemID;

	public static bool freeCam;

	public static Vector2 savedFreeCam;

	public static List<Keys> theKeyBinds;

	public static List<string> theMouseBinds;

	public static int[] bindSetMode;

	[Old("Deprecated. Replaced with native keybindings.")]
	public static Dictionary<int, Tuple<Keys, object>> keyBindMaster;

	[Old("Deprecated. Use tilesToBrush.")]
	public static ConcurrentDictionary<Point, int> markedTilesForLiquidAndDryBrush;

	public static bool brushCircle;

	public static int brushPattern;

	public static bool customMouseOver;

	public static float savedMapScale;

	public static bool playerVitalStatsUI;

	public static int prevTileBrushSize;

	public static int prevEraseBrushSize;

	public static int prevLiquidBrushSize;

	public static int prevWallBrushSize;

	public static Dictionary<int, int> tileToItemDictionarySearch;

	public static Dictionary<int, int> wallToItemDictionarySearch;

	public static bool pasteInTShock;

	public static bool lag;

	public static bool cancelPaste;

	public static bool cancelAction;

	public static bool showNetplayText;

	public static int prevPlrCount;

	public static bool notifyPacket4;

	public static int schematicSizeX;

	public static int schematicSizeY;

	public static byte[] TEditImportantFrames;

	public static bool blendAirTileMode;

	public static RemoteServer remoteServerTestingThing;

	public static bool dontDropItemsOnDeath;

	public static bool unlimitedRopeSnakes;

	public static bool unlimitedIce;

	public static bool dontDeleteCharacterOnHardcoreDeath;

	public static bool dontDeleteMyItemsOnDeath;

	public static bool reflectAllMyProjectiles;

	public static int customRopeSnakeHeight;

	public static bool ropeSnakeHeightOverride;

	public static bool spammingBeeMovieScript;

	public static bool forcedDropInventory;

	public static bool shimmerGodMode;

	public static bool rgbPlayerOutline;

	public static bool armorAfterimageCosmetic;

	public static bool armorPulseCosmetic;

	public static bool infiniteAmmo;

	public static bool rgbNpcIcons;

	public static bool rgbBossIcons;

	public static bool shimmerWalkParticles;

	public static int walkParticleType;

	public static bool ignoreLiquids;

	public static bool ignoreDamageBlocks;

	public static bool bypassTeleportRequests;

	public static bool deathParticleCosmeticEnabled;

	public static int deathParticleCosmeticId;

	public static bool deathGoreCosmeticEnabled;

	public static int deathGoreCosmeticId;

	public static bool deathSoundCosmeticEnabled;

	public static int deathSoundCosmeticId;

	public static bool teleportBypassInfoShown;

	public static bool shownLowFpsBlockedProjMsg;

	public static bool antiDeathGod;

	public static bool deathParticleCosmeticForAll;

	public static bool deathGoreCosmeticForAll;

	public static bool deathCosmeticsForAll;

	public static bool invisibility;

	public static bool jitterPos;

	public static int jitterAmount;

	public static bool jitteringMyPosRn;

	public static bool autoQuickCheats;

	public static bool hideGameMenuUi;

	public static bool isCoolClient;

	public static bool freeCraft;

	public static bool craftAnywhere;

	public static int fishItem;

	public static int lucyMessagesMode;

	public static int lucyDeathMessageMode;

	public static bool worldDownload;

	public static int forceSoundtrackId;

	public static bool forcedClothesWindow;

	public static bool forcedHairWindow;

	public static bool forcedGuideCraftingMenu;

	public static bool forcedReforgeMenu;

	public static bool freeShops;

	public static bool controlTildeTerminal;

	public static bool releaseTildeTerminal;

	public static bool hasLoadedIntoWorld;

	public static bool didTShockDisableYou;

	public static int tShockDisableTime;

	public static bool tileSyncExploit;

	public static int concurrentPastes;

	public static bool placeAnywhere;

	public static bool forcedContainer;

	public static bool useCustomUUID;

	public static string customUUID;

	public static int hideUUID;

	public static bool randomUUID;

	public static bool seeInvisPlayers;

	public static bool removeForcedMinimumZoom;

	public static bool blockServerControl;

	public static bool notifyServerModifiedPlayer;

	public static bool recievedServerDamage;

	public static int globalBlockedProjTime;

	public static int updatesAtHighMsThreshold;

	public static bool sendingBlockedSSC;

	public static bool rapidPosUpdate;

	public static bool tileExploitBrush;

	public static short tileExploitBrushFX;

	public static short tileExploitBrushFY;

	public static bool tileExploitBrushActive;

	public static ushort tileExploitBrushType;

	public static int tileExploitBrushSize;

	public static int noKillSoundTime;

	public static bool blockWirePayment;

	public static bool debugMessages;

	public static bool freeCam2;

	public static byte outsideWorldMapLight;

	[Old("Deprecated.")]
	public static List<Point> framingBlockedTiles;

	public static bool classicMusic;

	public static int lightsAddedPerFrame;

	public static bool logLightsAdded;

	public static ushort randomProjsToShoot;

	public static int dustLevel;

	public static bool uiRework;

	public static UserInterface customUI2;

	public static int fullBrightLevel;

	public static float fullBrightPercent;

	public static int deathItemDropMode;

	public static bool instantRegen;

	public static bool dodgeEverything;

	public static bool antiDebuff;

	public static bool playerHitboxes;

	public static bool npcHitboxes;

	public static bool projectileHitboxes;

	public static bool useExtraProjHitboxes;

	public static bool itemHitboxes;

	public static bool stopMapReveal;

	public static List<Rectangle> extraProjHitboxes;

	public static bool copyingFromFillOrDelMode;

	public static bool savingTEDITSchematic;

	public static bool openingTEDITSchematic;

	public static int awaitTshockResponseTime;

	public static bool onTshockServer;

	public static int worldBorderSize;

	public static bool noScreenClamp;

	public static bool keepThinVanillaBorder;

	public static int awaitingTshockPvpResponseTime;

	public static int detectedVanishedPlayers;

	public static bool earRape;

	public static bool[] buffForceImmune;

	public static bool suppressHorrified;

	public static bool purifyingWorld;

	public static bool localChests;

	public static bool extraAccSlots;

	public static bool suppressCreativeShock;

	public static bool bugFixFakeNewChest;

	public static int trollItemId;

	public static bool seedMixer;

	public static int concurrentFillOrDels;

	public static int concurrentConversions;

	public static bool grappleMounted;

	public static bool showSpectatedInventory;

	public static bool loadingChunks;

	public static bool freeCamDebug;

	public static int persistBuffsMode;

	public static bool unrestrictTeleportRod;

	public static bool cancelAnyBuff;

	public static bool ignoreCC;

	public static bool showIDs;

	public static bool extraItemInfo;

	public static bool serverSideCursor;

	public static int dreamLuckMode;

	public static int hitBoxesTexture;

	public static bool noSwapRestriction;

	public static bool openLockedChests;

	public static bool hammerAnything;

	public static bool equipDupeAccessory;

	public static bool anythingOnPlates;

	public static bool actuatorRodPlus;

	public static bool golfBallSpammer;

	public static bool sittingTest;

	public static bool testShield;

	public static int forcedTileObjectRandom;

	public static int forcedTileObjectAlternate;

	public static bool extendChat;

	public static bool suppressZones;

	public static bool shareMyInfo;

	public static bool fastJoin;

	public static bool rejoining;

	public static bool tempRejoin;

	public static bool autoRejoin;

	public static bool rejoinRememberPos;

	public static bool loadingMap;

	public static bool originalGetGoodWorld;

	public static bool originalRemixWorld;

	public static bool autoDeleteItems;

	public static int seedlerAmount;

	public static int seedlerProj;

	public static bool cloak;

	public static int cloakMode;

	public static int bombAmount;

	public static bool bombFiller;

	public static int bombFillerType;

	public static int targetPlayerIndex;

	public static bool placeAnywhereTileSync;

	public static int zenithItem;

	public static int noSoundsTime;

	public static bool antiLagProjectiles;

	public static bool antiLagSounds;

	public static bool antiLagParticles;

	public static bool antiLagFps;

	public static int disableProjLightTime;

	public static bool disableProjLight;

	public static bool forceSpelunker;

	public static bool forceBiomeSight;

	public static bool forceHunter;

	public static bool forceDangersense;

	public static bool alwaysPvpDeath;

	public static int surfaceFrictionMode;

	public static bool bouncySurfaces;

	public static bool asphaltSurfaces;

	public static int conveyerBeltMode;

	public static bool ignorePlayerHostileForLockOn;

	public static bool fixHitParticles;

	public static int fixUpperTime;

	public static bool oldMus_JungleNight;

	public static bool oldMus_UndergroundJungle;

	public static bool oldMus_Dungeon;

	public static bool oldMus_Underworld;

	public static bool oldMus_UndergroundDesert;

	public static bool oldMus_Fishron;

	public static bool oldMus_Goblins;

	public static bool oldMus_Pirates;

	public static bool oldMus_MorningRain;

	public static bool oldMus_Storm;

	public static bool oldMus_WindyDay;

	public static bool oldMus_SlimeRain;

	public static bool oldMus_Graveyard;

	public static bool oldMus_NoTown;

	public static int quedMapRefreshTime;

	public static bool automaticMartians;

	public static int automaticMartiansWaitTime;

	public static bool fuckChestName;

	public static bool alwaysStardustGuardian;

	public static bool alwaysSolarExplosion;

	public static bool alwaysGhostHeal;

	public static bool alwaysGhostHurt;

	public static bool alwaysNebulaSet;

	public static int forceDashType;

	public static bool renderInvisibleTiles;

	public static bool enableTinyWorld;

	public static bool renderOtherWireKites;

	public static bool unlimitedWinds;

	public static bool superExplosives;

	public static bool seeTileData;

	public static bool seeNpcData;

	public static short controlledNpcType;

	public static NPC controlledNpc;

	public static bool npcCycleRequested;

	public static int lastControlledNpcIndex;

	public static bool shouldStrikeControlledNpc;

	public static bool controlNpcAtMouse;

	public static Vector2 OldMouseWorld;

	public static Vector2 mouseVelocity;

	public static Player backupPlayer;

	public static string customTextOverride;

	public static byte paintOverride;

	public static byte paintCoatOverride;

	public static bool noBloodMoonShader;

	public static bool noMoonLordShader;

	public static bool useCrt;

	public static bool useGameBoy;

	public static bool disableAltEnterFullscreen;

	public static bool infiniteBreath;

	public static bool noFallDamage;

	public static bool portalPhysics;

	public static bool infiniteFlight;

	public static bool alwaysHoverable;

	public static bool responsiveControls;

	public static bool maxWings;

	public static Dictionary<short, short> itemTranslation;

	public static bool antiChatSpam;

	public static bool autoExitPaste;

	public static int pasteDelay;

	public static int pasteDelayCounter;

	public const int DefaultCrashDelay = 900;

	public static bool autoMobileCrash_Explosives;

	public static int[] autoCrashDelays_Explosives;

	public static bool autoMobileCrash_Dressers;

	public static int[] autoCrashDelays_Dressers;

	public const float MinPlayerTransparency = 0.6f;

	public static bool convertingToSchematic;

	public static bool alwaysDrawModifiedIndicator;

	public static bool infiniteBuffs;

	public static bool lightEveryFrame;

	public static bool _isLightingEveryFrame;

	public static float nPCInterpolationRate;

	public static string lastCommand;

	public static string boundCommand1;

	public static string boundCommand2;

	public static string boundCommand3;

	public static string boundCommand4;

	public static string boundCommand5;

	public static string boundCommand6;

	public static string boundCommand7;

	public static string boundCommand8;

	public const string DefaultEncryptionKey = "ZyUg_y3h";

	public static string EncChatKeyString;

	public static bool encryptChat;

	public static bool encryptChatWarning;

	public static int encryptChatWarnTime;

	public static bool encryptChatInvalidKeyWarning;

	public static PopupText encryptedChatWarningPopup;

	public static bool useAsyncSocket;

	public static bool useNewServerList;

	public static Color pasteBorderColor;

	public static int pasteBorderColorChange;

	public static bool worldPreviewOnMap;

	public static bool inWorldMapPreview;

	public static bool erasePickOnly;

	public static bool pixelBg;

	public static bool godMode
	{
		get
		{
			return CheatManager.GetCheat<Cheats.GodmodeCheat>().Enabled;
		}
		set
		{
			CheatManager.GetCheat<Cheats.GodmodeCheat>().Toggle();
		}
	}

	public static bool selectionTileSyncingMode
	{
		get
		{
			if (!selectionUseTileSync)
			{
				return Main.netMode == 2;
			}
			return true;
		}
		set
		{
			selectionUseTileSync = value;
		}
	}

	public static byte[] EncryptedChatKey => new UnicodeEncoding().GetBytes(EncChatKeyString);

	public static bool customMouseText
	{
		get
		{
			if (seeTileData)
			{
				return customTextOverride != "";
			}
			return false;
		}
	}

	public static bool CustomUIMousedOver
	{
		get
		{
			if (!tildeTerminal || customUI2.CurrentState != ModsNewUICheatPanel.uiStateNewCheatPanel || !ModsNewUICheatPanel.mousedOver)
			{
				if (customUI2.CurrentState == ModsNewUIInventoryViewer.instance)
				{
					return ModsNewUIInventoryViewer.mousedOver;
				}
				return false;
			}
			return true;
		}
	}

	public static bool DraggingCustomUISlider
	{
		get
		{
			if (tildeTerminal && customUI2.CurrentState == ModsNewUICheatPanel.uiStateNewCheatPanel)
			{
				if (!ModsNewUICheatPanel.draggingSlider)
				{
					return ModsNewUICheatPanel.draggingScrollbar;
				}
				return true;
			}
			return false;
		}
	}

	public Vector2 OldCenterMod
	{
		get
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(Main.LocalPlayer.oldPosition.X + (float)(Main.LocalPlayer.width / 2), Main.LocalPlayer.oldPosition.Y + (float)(Main.LocalPlayer.height / 2));
		}
		set
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			Main.LocalPlayer.oldPosition = new Vector2(value.X - (float)(Main.LocalPlayer.width / 2), value.Y - (float)(Main.LocalPlayer.height / 2));
		}
	}

	public static bool PasteCancelEnabled => concurrentPastes > 0;

	public static bool FillOrDelCancelEnabled
	{
		get
		{
			if (concurrentFillOrDels <= 0)
			{
				return concurrentConversions > 0;
			}
			return true;
		}
	}

	public static bool SaveSehcmaticEnabled
	{
		get
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (copiedTiles.Count <= 0)
			{
				if (selectionMode && selectionA != Point.Zero)
				{
					return selectionB != Point.Zero;
				}
				return false;
			}
			return true;
		}
	}

	public static bool SunDialEnabled
	{
		get
		{
			if (Main.netMode == 1)
			{
				return Main.sundialCooldown == 0;
			}
			return true;
		}
	}

	public static bool MoonDialEnabled
	{
		get
		{
			if (Main.netMode == 1)
			{
				return Main.moondialCooldown == 0;
			}
			return true;
		}
	}

	public static bool GolemSpawnEnabled
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (Main.netMode != 1)
			{
				return true;
			}
			int num2 = (int)(Main.LocalPlayer.Center.X / 16f);
			int num3 = (int)(Main.LocalPlayer.Center.Y / 16f);
			for (int j = num2 - 20; j < num2 + 20; j++)
			{
				for (int k = num3 - 20; k < num3 + 20; k++)
				{
					if (WorldGen.InWorld(j, k) && Main.tile[j, k] != null && Main.tile[j, k].active() && Main.tile[j, k].type == 237 && Main.tile[j, k].frameX == 18 && Main.tile[j, k].frameY == 0)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public static bool MechdusaEnabled
	{
		get
		{
			if (Main.netMode == 1)
			{
				if (Main.netMode == 1)
				{
					if (!originalGetGoodWorld)
					{
						return originalRemixWorld;
					}
					return true;
				}
				return false;
			}
			return true;
		}
	}

	public static bool DoWeHaveDungeonPosition
	{
		get
		{
			if (Main.dungeonX > 0)
			{
				return Main.dungeonY > 0;
			}
			return false;
		}
	}

	public static bool HoldingAnyPaintTool
	{
		get
		{
			if (!HoldingPaintBrush && !HoldingPaintRoller)
			{
				return HoldingPaintScraper;
			}
			return true;
		}
	}

	public static bool HoldingAnyLiquidBrush
	{
		get
		{
			if (!ItemID.Sets.LiquidBuckets[Main.LocalPlayer.HeldItem.type])
			{
				return ItemID.Sets.LiquidRemoval[Main.LocalPlayer.HeldItem.type];
			}
			return true;
		}
	}

	public static bool HoldingPaintBrush
	{
		get
		{
			if (Main.LocalPlayer.HeldItem.type != 1071)
			{
				return Main.LocalPlayer.HeldItem.type == 1543;
			}
			return true;
		}
	}

	public static bool HoldingPaintRoller
	{
		get
		{
			if (Main.LocalPlayer.HeldItem.type != 1072)
			{
				return Main.LocalPlayer.HeldItem.type == 1544;
			}
			return true;
		}
	}

	public static bool HoldingPaintScraper => ItemID.Sets.IsPaintScraper[Main.LocalPlayer.HeldItem.type];

	public static bool IsInTileBrush
	{
		get
		{
			if (!Main.gameMenu && tileBrush && (Main.LocalPlayer.HeldItem.createTile >= 0 || Main.LocalPlayer.HeldItem.type == 4262 || Main.LocalPlayer.HeldItem.type == 496))
			{
				return !selectionMode;
			}
			return false;
		}
	}

	public static bool IsInWallBrush
	{
		get
		{
			if (!Main.gameMenu && wallBrush && Main.LocalPlayer.HeldItem.createWall > 0 && !IsInTileBrush)
			{
				return !selectionMode;
			}
			return false;
		}
	}

	public static bool IsInLiquidBrush
	{
		get
		{
			if (!Main.gameMenu && liquidBrush && HoldingAnyLiquidBrush)
			{
				return !selectionMode;
			}
			return false;
		}
	}

	public static bool IsInTilePaintBrush
	{
		get
		{
			if (!Main.gameMenu && tilePaintBrush && HoldingAnyPaintTool)
			{
				return !selectionMode;
			}
			return false;
		}
	}

	public static bool IsInEraseBrush
	{
		get
		{
			if (!Main.gameMenu && eraseBrush && !IsInTileBrush && !IsInWallBrush && !IsInLiquidBrush && !HoldingAnyLiquidBrush && !IsInTilePaintBrush && !HoldingAnyPaintTool && CanEraseWith)
			{
				return !selectionMode;
			}
			return false;
		}
	}

	public static bool IsInAnyBrush
	{
		get
		{
			if (IsInTileBrush || IsInWallBrush || IsInLiquidBrush || IsInTilePaintBrush || IsInEraseBrush || tileExploitBrush)
			{
				return !selectionMode;
			}
			return false;
		}
	}

	public static bool IsInAnyWorldEdit
	{
		get
		{
			if (!IsInAnyBrush && !selectionMode && !drawPhotoMode && !pasteMode)
			{
				return copyMode;
			}
			return true;
		}
	}

	public static bool CanEraseWith
	{
		get
		{
			if (erasePickOnly)
			{
				if (erasePickOnly)
				{
					if (Main.LocalPlayer.HeldItem.pick <= 0 && Main.LocalPlayer.HeldItem.createTile <= -1)
					{
						return Main.LocalPlayer.HeldItem.createWall > 0;
					}
					return true;
				}
				return false;
			}
			return true;
		}
	}

	public static bool IsLowFPS
	{
		get
		{
			if (Main.frameRate >= 15)
			{
				return updatesAtHighMsThreshold > 20;
			}
			return true;
		}
	}

	public static bool IsLowFPS_Alt
	{
		get
		{
			if (Main.frameRate >= 15)
			{
				return updatesAtHighMsThreshold > (Main.hasFocus ? 20 : 60);
			}
			return true;
		}
	}

	public static bool IsVeryLowFPS
	{
		get
		{
			if (Main.frameRate >= 10)
			{
				return updatesAtHighMsThreshold > 60;
			}
			return true;
		}
	}

	public int DetermineUnloadedSections
	{
		get
		{
			int num = 0;
			for (int i = 0; i < Main.maxSectionsX; i++)
			{
				for (int j = 0; j < Main.maxSectionsY; j++)
				{
					if (Main.sectionManager != null && !Main.sectionManager.SectionLoaded(i, j))
					{
						num++;
					}
				}
			}
			return num;
		}
	}

	public static string BrushOrRollerOrSscraper()
	{
		if (HoldingPaintRoller)
		{
			return "Roller";
		}
		if (HoldingPaintScraper)
		{
			return "Scraper";
		}
		return "Brush";
	}

	public static string EraseBrushString()
	{
		Item i = Main.LocalPlayer.HeldItem;
		if (i.createTile > -1)
		{
			return "Tile: " + ((i.createTile < TileID.Count) ? TileID.Search.GetName(i.createTile) : "Invalid");
		}
		if (i.createWall > 0)
		{
			return "Wall: " + ((i.createWall < WallID.Count) ? WallID.Search.GetName(i.createWall) : "Invalid");
		}
		return "Brush";
	}

	public static void TryDetermineDungeonPos()
	{
		if (DoWeHaveDungeonPosition || (Main.netMode == 1 && Netplay.Connection.State < 4))
		{
			return;
		}
		for (int i = 0; i < Main.npc.Length; i++)
		{
			if (Main.npc[i].type == 37)
			{
				Main.dungeonX = Main.npc[i].homeTileX;
				Main.dungeonY = Main.npc[i].homeTileY;
				break;
			}
		}
	}

	public static bool IsTile(Tile t)
	{
		if (t == null)
		{
			return false;
		}
		if (!t.active() || t.type == 52 || t.type == 71 || t.type == 636 || t.type == 205 || t.type == 352 || t.type == 201 || t.type == 24 || t.type == 69 || t.type == 528 || t.type == 115 || t.type == 113 || t.type == 110 || t.type == 3 || t.type == 73 || t.type == 637 || t.type == 530 || t.type == 61 || t.type == 74 || t.type == 382 || t.type == 638 || t.type == 528 || t.type == 549)
		{
			return true;
		}
		return false;
	}

	public static void MainHacked()
	{
		Instance = new Mods();
		ModsNewUICheatPanel.useOpinion = Main.rand.Next(0, 100) == 0;
		ModsNewUICheatPanel.useJoke = Main.rand.Next(0, 20) == 0;
		Main.OnTickForRunningModUpdates += Instance.DoModUpdate;
		Main.OnTickForThirdPartySoftwareOnly += Instance.DoEarlyModUpdate;
		Instance.ActivateTileAndWallSearchDict();
		int def = 0;
		ushort udef = 0;
		FlexibleTileWand.ForModders_AddPotsToWand(FlexibleTileWand.Custom_RubblePlacementPots, ref def, ref udef);
		ItemMods.RefreshAllPresets();
		TEditImageToSchematic.BuildColorFilter();
	}

	public static void LogException(Exception ex)
	{
		string text = ex.ToString();
		try
		{
			using StreamWriter streamWriter = new StreamWriter(zazaErrorTxt, append: true);
			streamWriter.WriteLine(DateTime.Now);
			streamWriter.WriteLine(text);
			streamWriter.WriteLine("");
		}
		catch
		{
		}
	}

	public static void SendMobileCrashingWhisper(int playerIndex)
	{
		ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("/w " + (Main.player[playerIndex].spawned ? $"tsi:{playerIndex}" : ("\"" + Main.player[playerIndex].name + "\"")) + " [ct:7,5456] [i:6414] ====== =======  ================================================================================================================================== === :)))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))) [ct:7,"));
	}

	public static void WhisperAndMobileCrash(int playerIndex)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		SendMobileCrashingWhisper(playerIndex);
		bool prevChannelState = Main.LocalPlayer.channel;
		for (int i = 0; i < 10; i++)
		{
			SpawnMobileCrashingThing(Main.player[playerIndex].Center);
		}
		NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
		Main.LocalPlayer.channel = prevChannelState;
	}

	public static bool PlayerHasExplosiveRocket(Player player)
	{
		for (int i = 0; i < player.inventory.Length; i++)
		{
			Item item = player.inventory[i];
			if (item.useStyle == 0 && item.ammo == AmmoID.Rocket)
			{
				return true;
			}
		}
		return false;
	}

	public static bool PlayerHoldingExplosive(Player player)
	{
		Item item = player.HeldItem;
		if (item.createTile == 141 || item.shoot == 28 || item.shoot == 37 || item.shoot == 516 || item.shoot == 519 || item.shoot == 29 || item.shoot == 470 || item.shoot == 637 || item.shoot == 903 || item.shoot == 904 || item.shoot == 905 || (item.useAmmo == AmmoID.Rocket && item.itemShootBehavior != 48 && PlayerHasExplosiveRocket(player)))
		{
			return true;
		}
		return false;
	}

	public static bool IsNearLiquidDresser(Player player, int range = 15)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		int playerTileX = (int)(player.Center.X / 16f);
		int playerTileY = (int)(player.Center.Y / 16f);
		for (int x = playerTileX - range; x <= playerTileX + range; x++)
		{
			for (int y = playerTileY - range; y <= playerTileY + range; y++)
			{
				if (!WorldGen.InWorld(x, y, 1))
				{
					continue;
				}
				Tile tile = Main.tile[x, y];
				if (tile.HasTile && tile.TileType == 88)
				{
					if (tile.LiquidAmount > 0)
					{
						return true;
					}
					if (WorldGen.InWorld(x, y - 1, 1) && Main.tile[x, y - 1].LiquidAmount > 0)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool PlayerHoldingLiquidBucket(Player p)
	{
		Item item = p.HeldItem;
		if (item == null || item.IsAir)
		{
			return false;
		}
		return ItemID.Sets.LiquidBuckets[item.type];
	}

	public static bool IsAnyCoin(Item item)
	{
		if (item == null || item.IsAir)
		{
			return false;
		}
		if (item.type != 71 && item.type != 72 && item.type != 73)
		{
			return item.type == 74;
		}
		return true;
	}

	public static bool IsAnyCreateTile(Item item)
	{
		if (item == null || item.IsAir)
		{
			return false;
		}
		if (item.type != 496 && item.type != 4262 && item.type != 114 && item.type != 266 && item.createTile != 470 && item.createTile != 475 && item.createTile != 471 && item.createTile != 378 && item.createTile != 395 && item.createTile != 520 && item.createTile != 423 && item.createTile != 597 && (item.createTile <= -1 || Main.tileSolid[item.createTile] || TileObjectData.CustomPlace(item.createTile, item.placeStyle)) && item.shoot != 171 && item.shoot != 475 && item.shoot != 505)
		{
			return item.shoot == 506;
		}
		return true;
	}

	public static bool HandleChat(string chatText)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		if (chatText.StartsWith(commandSpecifier))
		{
			try
			{
				DoCommands(chatText);
			}
			catch (Exception ex)
			{
				Main.NewText($"DoCommands error!\n{ex}", errorColor);
				LogException(ex);
			}
			return true;
		}
		return false;
	}

	public static void LoadSettings(bool NoHacks = false)
	{
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		if (!CheatManager.Initialized)
		{
			CheatManager.Initialize();
		}
		Preferences cfg = ZazaConfiguration;
		if (!cfg.Load())
		{
			SaveSettings();
		}
		cfg.Get("RememberEnabledCheats", ref rememberEnabled);
		cfg.Get("CommandSpecifierCharacter", ref commandSpecifier);
		int maxZoom = 2;
		cfg.Get("MaxZoom", ref maxZoom);
		Main.MaxZoom = Math.Clamp(maxZoom, 2, 10);
		Dictionary<string, byte> msgColor = new Dictionary<string, byte>();
		cfg.Get("ZazaNormalMessageColor", ref msgColor);
		if (msgColor.TryGetValue("R", out var value))
		{
			((Color)(ref zazasFavoriteColor)).R = value;
		}
		if (msgColor.TryGetValue("G", out value))
		{
			((Color)(ref zazasFavoriteColor)).G = value;
		}
		if (msgColor.TryGetValue("B", out value))
		{
			((Color)(ref zazasFavoriteColor)).B = value;
		}
		msgColor.Clear();
		Dictionary<string, byte> dictionary = new Dictionary<string, byte>();
		cfg.Get("ZazaErrorMessageColor", ref errorColor);
		if (dictionary.TryGetValue("R", out var value2))
		{
			((Color)(ref errorColor)).R = value2;
		}
		if (dictionary.TryGetValue("G", out value2))
		{
			((Color)(ref errorColor)).G = value2;
		}
		if (dictionary.TryGetValue("B", out value2))
		{
			((Color)(ref errorColor)).B = value2;
		}
		dictionary.Clear();
		Dictionary<string, byte> dictionary2 = new Dictionary<string, byte>();
		cfg.Get("ZazaInfoMessageColor", ref infoColor);
		if (dictionary2.TryGetValue("R", out var value3))
		{
			((Color)(ref infoColor)).R = value3;
		}
		if (dictionary2.TryGetValue("G", out value3))
		{
			((Color)(ref infoColor)).G = value3;
		}
		if (dictionary2.TryGetValue("B", out value3))
		{
			((Color)(ref infoColor)).B = value3;
		}
		dictionary2.Clear();
		cfg.Get("KeyOpenChat", ref cOpenChat);
		if (Enum.TryParse<Keys>(cOpenChat, out Keys result))
		{
			OpenChatKey = result;
		}
		cfg.Get("KeySubmitTextInput", ref cEnter);
		if (Enum.TryParse<Keys>(cEnter, out Keys result2))
		{
			SubmitTextKey = result2;
		}
		cfg.Get("KeyBackspace", ref cBack);
		if (Enum.TryParse<Keys>(cBack, out Keys result3))
		{
			BackspaceKey = result3;
		}
		cfg.Get("KeyBackspaceAlt", ref cDelete);
		if (Enum.TryParse<Keys>(cDelete, out Keys result4))
		{
			AltBackspaceKey = result4;
		}
		if (commandSpecifier == '/' || commandSpecifier == '\u007f' || char.IsWhiteSpace(commandSpecifier) || !char.IsAscii(commandSpecifier) || char.GetNumericValue(commandSpecifier) < 33.0)
		{
			commandSpecifier = '.';
		}
		cfg.Get("UseNewServerList", ref useNewServerList);
		int messageCount = 10;
		cfg.Get("ChatMessageDisplayCount", ref messageCount);
		messageCount = Math.Clamp(messageCount, 5, 30);
		Main.chatMonitor.ShowCount = messageCount;
		bool prevInWorldState = inWorldMapPreview;
		bool prevMapPreview = worldPreviewOnMap;
		cfg.Get("SchematicPreview_UseMapPreviewInWorld", ref inWorldMapPreview);
		cfg.Get("SchematicPreview_UseInWorldPreviewOnMap_WarningThisUsesALotOfRAM", ref worldPreviewOnMap);
		if (!Main.gameMenu && (prevInWorldState != inWorldMapPreview || prevMapPreview != worldPreviewOnMap))
		{
			Schematics.ResetPreview();
		}
		cfg.Get("FixPvpHitParticles", ref fixHitParticles);
		cfg.Get("ZazaInformOtherZazaUsers", ref shareMyInfo);
		cfg.Get("UseAsyncTcpSocket", ref useAsyncSocket);
		cfg.Get("SuperFastSplash", ref superFastSplash);
		cfg.Get("FastJoin", ref fastJoin);
		cfg.Get("RejoinRememberPosition", ref rejoinRememberPos);
		cfg.Get("HideCustomMOTD", ref hideHackedMOTD);
		hackedMOTD = !hideHackedMOTD;
		cfg.Get("EnableTinyWorlds", ref enableTinyWorld);
		cfg.Get("DontDeleteHardcoreCharacters", ref dontDeleteCharacterOnHardcoreDeath);
		cfg.Get("HideUuidMode", ref hideUUID);
		hideUUID = Math.Clamp(hideUUID, 0, 2);
		cfg.Get("BackgroundMusic", ref backgroundMusic);
		cfg.Get("DisableAltEnterFullscreen", ref disableAltEnterFullscreen);
		cfg.Get("UseIntegratedServerHosting", ref serverInSameProcess);
		cfg.Get("PasteDelay", ref pasteDelay);
		pasteDelay = Math.Clamp(pasteDelay, 0, 120);
		cfg.Get("DrawPhotoType", ref drawPhotoType);
		drawPhotoType = Math.Clamp(drawPhotoType, 0, 2);
		cfg.Get("DisableForcedMinimumZoom", ref removeForcedMinimumZoom);
		cfg.Get("PixelatedBackgrounds", ref pixelBg);
		cfg.Get("MultiplayerNPCInterpolationRate", ref nPCInterpolationRate);
		cfg.Get("InfiniteBuffs", ref infiniteBuffs);
		cfg.Get("LightingEveryFrame", ref lightEveryFrame);
		cfg.Get("LockOnInPvP", ref LockOnHelper.targetPlayers);
		cfg.Get("LockOnToFriendlyPlayers", ref ignorePlayerHostileForLockOn);
		cfg.Get("AlwaysShowModifiedItemIndicator", ref alwaysDrawModifiedIndicator);
		cfg.Get("EraseBrushEraseWithPickaxeOnly", ref erasePickOnly);
		cfg.Get("BindableCommand_1", ref boundCommand1);
		cfg.Get("BindableCommand_2", ref boundCommand2);
		cfg.Get("BindableCommand_3", ref boundCommand3);
		cfg.Get("BindableCommand_4", ref boundCommand4);
		cfg.Get("BindableCommand_5", ref boundCommand5);
		cfg.Get("BindableCommand_6", ref boundCommand6);
		cfg.Get("BindableCommand_7", ref boundCommand7);
		cfg.Get("BindableCommand_8", ref boundCommand8);
		cfg.Get("EncryptedChatKey", ref EncChatKeyString);
		if (EncChatKeyString.Length != 8)
		{
			encryptChatInvalidKeyWarning = true;
		}
		if (string.IsNullOrEmpty(EncChatKeyString))
		{
			EncChatKeyString = "ZyUg_y3h";
		}
		while (EncChatKeyString.Length < 8)
		{
			EncChatKeyString += "ZyUg_y3h"[EncChatKeyString.Length];
		}
		if (EncChatKeyString.Length > 8)
		{
			EncChatKeyString = EncChatKeyString.Substring(0, 8);
		}
		if (!Main.gameMenu && encryptChatInvalidKeyWarning)
		{
			Main.NewText("Warning: The encryption key for Encrypted Chat was invalid. It has been changed to: " + EncChatKeyString, infoColor);
			encryptChatInvalidKeyWarning = false;
		}
		if (rememberEnabled && !NoHacks)
		{
			bool fakeBool = false;
			cfg.Get("Godmode", ref fakeBool);
			godMode = fakeBool;
			cfg.Get("AntiDeath", ref antiDeathGod);
			cfg.Get("DodgeEverything", ref dodgeEverything);
			cfg.Get("InstantRegen", ref instantRegen);
			cfg.Get("InfiniteMana", ref infiniteMana);
			cfg.Get("AntiDebuff", ref antiDebuff);
			cfg.Get("JourneyMode", ref journeyMenu);
			cfg.Get("InstantRespawn", ref instantRespawn);
			cfg.Get("ExtendPlaceRange", ref extendedPlacementRange);
			cfg.Get("ExtendItemReach", ref extendedPickUpRange);
			cfg.Get("InfiniteMinions", ref infiniteMinions);
			cfg.Get("InfiniteAmmo", ref infiniteAmmo);
			cfg.Get("InfiniteBreath", ref infiniteBreath);
			cfg.Get("NoFallDamage", ref noFallDamage);
			cfg.Get("IgnoreLiquids", ref ignoreLiquids);
			cfg.Get("IgnoreDamageBlocks", ref ignoreDamageBlocks);
			cfg.Get("ItemLossType", ref deathItemDropMode);
			deathItemDropMode = Math.Clamp(deathItemDropMode, 0, 2);
			cfg.Get("BuffPersistenceType", ref persistBuffsMode);
			persistBuffsMode = Math.Clamp(persistBuffsMode, 0, 5);
			cfg.Get("UnlimitTeleportRod", ref unrestrictTeleportRod);
			cfg.Get("ForceLockOn", ref LockOnHelper.ForceUsability);
			cfg.Get("SupressHorrified", ref suppressHorrified);
			cfg.Get("SuppressNoBuilding", ref suppressCreativeShock);
			cfg.Get("IgnoreCrowdControl", ref ignoreCC);
			cfg.Get("CancelDebuffs", ref cancelAnyBuff);
			cfg.Get("ForceExtraAccessories", ref extraAccSlots);
			cfg.Get("EquipDuplicateAccessories", ref equipDupeAccessory);
			cfg.Get("GrappleWithMounts", ref grappleMounted);
			cfg.Get("FullHpRespawn", ref alwaysPvpDeath);
			cfg.Get("FreeCrafting", ref freeCraft);
			cfg.Get("CraftAnywhere", ref craftAnywhere);
			cfg.Get("SwapItemsAnytime", ref noSwapRestriction);
			cfg.Get("HammerAnything", ref hammerAnything);
			cfg.Get("PlaceAnywhere", ref placeAnywhere);
			cfg.Get("ExplodeOnHit", ref alwaysSolarExplosion);
			cfg.Get("AlwaysGuardian", ref alwaysStardustGuardian);
			cfg.Get("AlwaysSpiritHeal", ref alwaysGhostHeal);
			cfg.Get("AlwaysSpiritHurt", ref alwaysGhostHurt);
			cfg.Get("AlwaysNebulaBonus", ref alwaysNebulaSet);
			cfg.Get("DashOverrideType", ref forceDashType);
			forceDashType = Math.Clamp(forceDashType, 0, 5);
			if (forceDashType == 4)
			{
				forceDashType = 0;
			}
			cfg.Get("LuckOverrideType", ref dreamLuckMode);
			dreamLuckMode = Math.Clamp(dreamLuckMode, 0, 2);
			cfg.Get("SurfacesAsphalt", ref asphaltSurfaces);
			cfg.Get("SurfacesBouncy", ref bouncySurfaces);
			cfg.Get("SurfacesFrictionType", ref surfaceFrictionMode);
			surfaceFrictionMode = Math.Clamp(surfaceFrictionMode, 0, 3);
			cfg.Get("SurfacesConveyerType", ref conveyerBeltMode);
			conveyerBeltMode = Math.Clamp(conveyerBeltMode, 0, 3);
			cfg.Get("InfiniteFlight", ref infiniteFlight);
			cfg.Get("AlwaysHoverAndBoost", ref alwaysHoverable);
			cfg.Get("MaxWingStats", ref maxWings);
			cfg.Get("PortalPhysics", ref portalPhysics);
			cfg.Get("MouseTeleport", ref gotoMouse);
			cfg.Get("MapTeleport", ref mapTeleportation);
			cfg.Get("ClientSideTeleport", ref bypassTeleportRequests);
			cfg.Get("HighDataRate", ref rapidPosUpdate);
			cfg.Get("DisplayNetText", ref showNetplayText);
			cfg.Get("ShowPlayerVitals", ref playerVitalStatsUI);
			cfg.Get("ShowDroppedItemOwner", ref Main.showItemOwner);
			cfg.Get("NotifyServerUpdatedPlayer", ref notifyServerModifiedPlayer);
			cfg.Get("NotifyConnectingPlayer", ref notifyPacket4);
			cfg.Get("ShowDataIds", ref showIDs);
			cfg.Get("ShowExtraItemData", ref extraItemInfo);
			cfg.Get("ShowExtraNpcInfo", ref seeNpcData);
			cfg.Get("ShowExtraPasteSelectionInfo", ref selectionModeInfo);
			cfg.Get("SeeTileData", ref seeTileData);
			cfg.Get("RenderPlayerHitboxes", ref playerHitboxes);
			cfg.Get("RenderNpcHitboxes", ref npcHitboxes);
			cfg.Get("RenderProjectileHitboxes", ref projectileHitboxes);
			cfg.Get("RenderDroppedItemHitboxes", ref itemHitboxes);
			cfg.Get("HitboxRenderType", ref hitBoxesTexture);
			hitBoxesTexture = Math.Clamp(hitBoxesTexture, 0, 2);
			cfg.Get("AlwaysRenderSpelunker", ref forceSpelunker);
			cfg.Get("AlwaysRenderHunter", ref forceHunter);
			cfg.Get("AlwaysRenderDangersense", ref forceDangersense);
			cfg.Get("AlwaysRenderBiomeSight", ref forceBiomeSight);
			cfg.Get("RenderOtherWireKites", ref renderOtherWireKites);
			cfg.Get("AlwaysRenderInvisibleBlocks", ref renderInvisibleTiles);
			cfg.Get("DisablePlayerInvisibility", ref seeInvisPlayers);
			cfg.Get("UseCRTShader", ref useCrt);
			cfg.Get("UseGameBoyShader", ref useGameBoy);
			cfg.Get("DisableBloodMoonShader", ref noBloodMoonShader);
			bool disabledGraveyardShader = false;
			cfg.Get("DisableGraveyardShader", ref disabledGraveyardShader);
			CheatManager.GetCheat<Cheats.DisableGraveyardShaderCheat>().Enabled = disabledGraveyardShader;
			bool disabledPillarShaders = false;
			cfg.Get("DisablePillarShaders", ref disabledPillarShaders);
			CheatManager.GetCheat<Cheats.DisablePillarShadersCheat>().Enabled = disabledPillarShaders;
			cfg.Get("DisableMoonLordShader", ref noMoonLordShader);
			cfg.Get("GlobalBrightness", ref fullBrightPercent);
			fullBrightPercent = Math.Clamp(fullBrightPercent, 0f, 1f);
			cfg.Get("DustDensity", ref dustLevel);
			dustLevel = Math.Clamp(dustLevel, 0, 100);
			cfg.Get("WorldBorderLighting", ref outsideWorldMapLight);
			cfg.Get("WalkParticleType", ref walkParticleType);
			walkParticleType = Math.Clamp(walkParticleType, 0, 3);
			cfg.Get("ParticleCursor", ref serverSideCursor);
			cfg.Get("UseDeathParticleCosmetic", ref deathParticleCosmeticEnabled);
			cfg.Get("DeathParticleId", ref deathParticleCosmeticId);
			deathParticleCosmeticId = Math.Clamp(deathParticleCosmeticId, 0, 32);
			cfg.Get("UseDeathEffectCosmetic", ref deathGoreCosmeticEnabled);
			cfg.Get("DeathEffectId", ref deathGoreCosmeticId);
			deathGoreCosmeticId = Math.Clamp(deathGoreCosmeticId, -3, GoreID.Count);
			cfg.Get("UseDeathSoundCosmetic", ref deathSoundCosmeticEnabled);
			cfg.Get("DeathSoundId", ref deathSoundCosmeticId);
			deathSoundCosmeticId = Math.Clamp(deathSoundCosmeticId, 0, 32);
			cfg.Get("LucyMessageType", ref lucyMessagesMode);
			lucyMessagesMode = Math.Clamp(lucyMessagesMode, 0, 2);
			cfg.Get("LucyDeathMessageType", ref lucyDeathMessageMode);
			lucyDeathMessageMode = Math.Clamp(lucyDeathMessageMode, 0, 3);
			cfg.Get("UseDeathComseticsOnAllPlayers", ref deathCosmeticsForAll);
			cfg.Get("ClientOnlyRgbNpcIcons", ref rgbNpcIcons);
			cfg.Get("ClientOnlyBossIcons", ref rgbBossIcons);
			cfg.Get("TrackPlayers", ref playerTracker);
			cfg.Get("FreeShops", ref freeShops);
			cfg.Get("OpenLockedChests", ref openLockedChests);
			cfg.Get("FixFakeNewChest", ref bugFixFakeNewChest);
			cfg.Get("UnrestrictPlates", ref anythingOnPlates);
			cfg.Get("SuperActuationRod", ref actuatorRodPlus);
			cfg.Get("AntiLagProjectiles", ref antiLagProjectiles);
			cfg.Get("AntiLagSounds", ref antiLagSounds);
			cfg.Get("AntiLagParticles", ref antiLagProjectiles);
			cfg.Get("AntiLagFps", ref antiLagFps);
			cfg.Get("RandomUuid", ref randomUUID);
			cfg.Get("Packets500msMode", ref tilePacketMaxMode);
			cfg.Get("TilePacketRate", ref tilePacketRate);
			tilePacketRate = Math.Clamp(tilePacketRate, 0, 1000);
			cfg.Get("ErasePacketRate", ref erasePacketRate);
			erasePacketRate = Math.Clamp(erasePacketRate, 0, 1000);
			cfg.Get("LiquidPacketRate", ref liquidPacketRate);
			liquidPacketRate = Math.Clamp(liquidPacketRate, 0, 1000);
			cfg.Get("PaintPacketRate", ref paintPacketRate);
			paintPacketRate = Math.Clamp(paintPacketRate, 0, 1000);
			cfg.Get("SelectionUseTileSyncing", ref selectionUseTileSync);
			cfg.Get("PasteModeAirBlending", ref blendAirTileMode);
			cfg.Get("PasteInTshock", ref pasteInTShock);
			cfg.Get("AutoExitPaste", ref autoExitPaste);
			cfg.Get("BrushCircle", ref brushCircle);
			cfg.Get("WorldBorderSize", ref worldBorderSize);
			worldBorderSize = Math.Clamp(worldBorderSize, -2, 40);
			cfg.Get("KeepVanillaBorder", ref keepThinVanillaBorder);
			cfg.Get("DontClampScreen", ref noScreenClamp);
			cfg.Get("ForcedSoundtrackType", ref forceSoundtrackId);
			forceSoundtrackId = Math.Clamp(forceSoundtrackId, 0, 2);
			cfg.Get("RevertJungleNightTheme", ref oldMus_JungleNight);
			cfg.Get("RevertUndergroundJungleTheme", ref oldMus_UndergroundJungle);
			cfg.Get("ClassicDungeonTheme", ref oldMus_Dungeon);
			cfg.Get("ClassicUnderworldTheme", ref oldMus_Underworld);
			cfg.Get("RevertUndergroundDesertTheme", ref oldMus_UndergroundDesert);
			cfg.Get("RevertDukeFishronTheme", ref oldMus_Fishron);
			cfg.Get("ClassicGoblinArmyTheme", ref oldMus_Goblins);
			cfg.Get("ClassicPiratesTheme", ref oldMus_Pirates);
			cfg.Get("DisableMorningRainTheme", ref oldMus_MorningRain);
			cfg.Get("DisableStormTheme", ref oldMus_Storm);
			cfg.Get("DisableWindyDayTheme", ref oldMus_WindyDay);
			cfg.Get("DisableSlimeRainTheme", ref oldMus_SlimeRain);
			cfg.Get("DisableGraveyardTheme", ref oldMus_Graveyard);
			cfg.Get("DisableTownThemes", ref oldMus_NoTown);
		}
	}

	public static void SaveSettings()
	{
		Preferences zazaConfiguration = ZazaConfiguration;
		zazaConfiguration.Clear();
		zazaConfiguration.Put("RememberEnabledCheats", rememberEnabled);
		zazaConfiguration.Put("CommandSpecifierCharacter", commandSpecifier);
		zazaConfiguration.Put("UseNewServerList", useNewServerList);
		zazaConfiguration.Put("ChatMessageDisplayCount", Main.chatMonitor.ShowCount);
		int maxZoom = (int)Main.MaxZoom;
		zazaConfiguration.Put("MaxZoom", maxZoom);
		zazaConfiguration.Put("ZazaNormalMessageColor", new Dictionary<string, byte>
		{
			{
				"R",
				((Color)(ref zazasFavoriteColor)).R
			},
			{
				"G",
				((Color)(ref zazasFavoriteColor)).G
			},
			{
				"B",
				((Color)(ref zazasFavoriteColor)).B
			}
		});
		zazaConfiguration.Put("ZazaErrorMessageColor", new Dictionary<string, byte>
		{
			{
				"R",
				((Color)(ref errorColor)).R
			},
			{
				"G",
				((Color)(ref errorColor)).G
			},
			{
				"B",
				((Color)(ref errorColor)).B
			}
		});
		zazaConfiguration.Put("ZazaInfoMessageColor", new Dictionary<string, byte>
		{
			{
				"R",
				((Color)(ref infoColor)).R
			},
			{
				"G",
				((Color)(ref infoColor)).G
			},
			{
				"B",
				((Color)(ref infoColor)).B
			}
		});
		zazaConfiguration.Put("KeyOpenChat", cOpenChat);
		zazaConfiguration.Put("KeySubmitTextInput", cEnter);
		zazaConfiguration.Put("KeyBackspace", cBack);
		zazaConfiguration.Put("KeyBackspaceAlt", cDelete);
		zazaConfiguration.Put("Godmode", godMode);
		zazaConfiguration.Put("AntiDeath", antiDeathGod);
		zazaConfiguration.Put("DodgeEverything", dodgeEverything);
		zazaConfiguration.Put("InstantRegen", instantRegen);
		zazaConfiguration.Put("InfiniteMana", infiniteMana);
		zazaConfiguration.Put("AntiDebuff", antiDebuff);
		zazaConfiguration.Put("JourneyMode", journeyMenu);
		zazaConfiguration.Put("InstantRespawn", instantRespawn);
		zazaConfiguration.Put("ExtendPlaceRange", extendedPlacementRange);
		zazaConfiguration.Put("ExtendItemReach", extendedPickUpRange);
		zazaConfiguration.Put("InfiniteMinions", infiniteMinions);
		zazaConfiguration.Put("InfiniteAmmo", infiniteAmmo);
		zazaConfiguration.Put("InfiniteBreath", infiniteBreath);
		zazaConfiguration.Put("NoFallDamage", noFallDamage);
		zazaConfiguration.Put("IgnoreLiquids", ignoreLiquids);
		zazaConfiguration.Put("IgnoreDamageBlocks", ignoreDamageBlocks);
		zazaConfiguration.Put("ItemLossType", deathItemDropMode);
		zazaConfiguration.Put("BuffPersistenceType", persistBuffsMode);
		zazaConfiguration.Put("UnlimitTeleportRod", unrestrictTeleportRod);
		zazaConfiguration.Put("ForceLockOn", LockOnHelper.ForceUsability);
		zazaConfiguration.Put("SupressHorrified", suppressHorrified);
		zazaConfiguration.Put("SuppressNoBuilding", suppressCreativeShock);
		zazaConfiguration.Put("IgnoreCrowdControl", ignoreCC);
		zazaConfiguration.Put("CancelDebuffs", cancelAnyBuff);
		zazaConfiguration.Put("ForceExtraAccessories", extraAccSlots);
		zazaConfiguration.Put("EquipDuplicateAccessories", equipDupeAccessory);
		zazaConfiguration.Put("GrappleWithMounts", grappleMounted);
		zazaConfiguration.Put("FullHpRespawn", alwaysPvpDeath);
		zazaConfiguration.Put("FreeCrafting", freeCraft);
		zazaConfiguration.Put("CraftAnywhere", craftAnywhere);
		zazaConfiguration.Put("SwapItemsAnytime", noSwapRestriction);
		zazaConfiguration.Put("HammerAnything", hammerAnything);
		zazaConfiguration.Put("PlaceAnywhere", placeAnywhere);
		zazaConfiguration.Put("ExplodeOnHit", alwaysSolarExplosion);
		zazaConfiguration.Put("AlwaysGuardian", alwaysStardustGuardian);
		zazaConfiguration.Put("AlwaysSpiritHeal", alwaysGhostHeal);
		zazaConfiguration.Put("AlwaysSpiritHurt", alwaysGhostHurt);
		zazaConfiguration.Put("AlwaysNebulaBonus", alwaysNebulaSet);
		zazaConfiguration.Put("DashOverrideType", forceDashType);
		zazaConfiguration.Put("LuckOverrideType", dreamLuckMode);
		zazaConfiguration.Put("SurfacesAsphalt", asphaltSurfaces);
		zazaConfiguration.Put("SurfacesBouncy", bouncySurfaces);
		zazaConfiguration.Put("SurfacesFrictionType", surfaceFrictionMode);
		zazaConfiguration.Put("SurfacesConveyerType", conveyerBeltMode);
		zazaConfiguration.Put("InfiniteFlight", infiniteFlight);
		zazaConfiguration.Put("AlwaysHoverAndBoost", alwaysHoverable);
		zazaConfiguration.Put("MaxWingStats", maxWings);
		zazaConfiguration.Put("PortalPhysics", portalPhysics);
		zazaConfiguration.Put("MouseTeleport", gotoMouse);
		zazaConfiguration.Put("MapTeleport", mapTeleportation);
		zazaConfiguration.Put("ClientSideTeleport", bypassTeleportRequests);
		zazaConfiguration.Put("HighDataRate", rapidPosUpdate);
		zazaConfiguration.Put("DisplayNetText", showNetplayText);
		zazaConfiguration.Put("ShowPlayerVitals", playerVitalStatsUI);
		zazaConfiguration.Put("ShowDroppedItemOwner", Main.showItemOwner);
		zazaConfiguration.Put("NotifyServerUpdatedPlayer", notifyServerModifiedPlayer);
		zazaConfiguration.Put("NotifyConnectingPlayer", notifyPacket4);
		zazaConfiguration.Put("ShowDataIds", showIDs);
		zazaConfiguration.Put("ShowExtraItemData", extraItemInfo);
		zazaConfiguration.Put("ShowExtraNpcInfo", seeNpcData);
		zazaConfiguration.Put("ShowExtraPasteSelectionInfo", selectionModeInfo);
		zazaConfiguration.Put("SeeTileData", seeTileData);
		zazaConfiguration.Put("RenderPlayerHitboxes", playerHitboxes);
		zazaConfiguration.Put("RenderNpcHitboxes", npcHitboxes);
		zazaConfiguration.Put("RenderProjectileHitboxes", projectileHitboxes);
		zazaConfiguration.Put("RenderDroppedItemHitboxes", itemHitboxes);
		zazaConfiguration.Put("HitboxRenderType", hitBoxesTexture);
		zazaConfiguration.Put("AlwaysRenderSpelunker", forceSpelunker);
		zazaConfiguration.Put("AlwaysRenderHunter", forceHunter);
		zazaConfiguration.Put("AlwaysRenderDangersense", forceDangersense);
		zazaConfiguration.Put("AlwaysRenderBiomeSight", forceBiomeSight);
		zazaConfiguration.Put("RenderOtherWireKites", renderOtherWireKites);
		zazaConfiguration.Put("AlwaysRenderInvisibleBlocks", renderInvisibleTiles);
		zazaConfiguration.Put("DisablePlayerInvisibility", seeInvisPlayers);
		zazaConfiguration.Put("UseCRTShader", useCrt);
		zazaConfiguration.Put("UseGameBoyShader", useGameBoy);
		zazaConfiguration.Put("DisableBloodMoonShader", noBloodMoonShader);
		zazaConfiguration.Put("DisableGraveyardShader", CheatManager.IsCheatActive("Disable Graveyard Shader"));
		zazaConfiguration.Put("DisablePillarShaders", CheatManager.IsCheatActive("Disable Pillar Shaders"));
		zazaConfiguration.Put("DisableMoonLordShader", noMoonLordShader);
		zazaConfiguration.Put("GlobalBrightness", fullBrightPercent);
		zazaConfiguration.Put("DustDensity", dustLevel);
		zazaConfiguration.Put("WorldBorderLighting", outsideWorldMapLight);
		zazaConfiguration.Put("AlwaysShowModifiedItemIndicator", alwaysDrawModifiedIndicator);
		zazaConfiguration.Put("EraseBrushEraseWithPickaxeOnly", erasePickOnly);
		zazaConfiguration.Put("DisableForcedMinimumZoom", removeForcedMinimumZoom);
		zazaConfiguration.Put("PixelatedBackgrounds", pixelBg);
		zazaConfiguration.Put("SchematicPreview_UseMapPreviewInWorld", inWorldMapPreview);
		zazaConfiguration.Put("SchematicPreview_UseInWorldPreviewOnMap_WarningThisUsesALotOfRAM", worldPreviewOnMap);
		zazaConfiguration.Put("WalkParticleType", walkParticleType);
		zazaConfiguration.Put("ParticleCursor", serverSideCursor);
		zazaConfiguration.Put("UseDeathParticleCosmetic", deathParticleCosmeticEnabled);
		zazaConfiguration.Put("DeathParticleId", deathParticleCosmeticId);
		zazaConfiguration.Put("UseDeathEffectCosmetic", deathGoreCosmeticEnabled);
		zazaConfiguration.Put("DeathEffectId", deathGoreCosmeticId);
		zazaConfiguration.Put("UseDeathSoundCosmetic", deathSoundCosmeticEnabled);
		zazaConfiguration.Put("DeathSoundId", deathSoundCosmeticId);
		zazaConfiguration.Put("LucyMessageType", lucyMessagesMode);
		zazaConfiguration.Put("LucyDeathMessageType", lucyDeathMessageMode);
		zazaConfiguration.Put("UseDeathComseticsOnAllPlayers", deathCosmeticsForAll);
		zazaConfiguration.Put("ClientOnlyRgbNpcIcons", rgbNpcIcons);
		zazaConfiguration.Put("ClientOnlyBossIcons", rgbBossIcons);
		zazaConfiguration.Put("FixPvpHitParticles", fixHitParticles);
		zazaConfiguration.Put("ZazaInformOtherZazaUsers", shareMyInfo);
		zazaConfiguration.Put("TrackPlayers", playerTracker);
		zazaConfiguration.Put("FreeShops", freeShops);
		zazaConfiguration.Put("OpenLockedChests", openLockedChests);
		zazaConfiguration.Put("FixFakeNewChest", bugFixFakeNewChest);
		zazaConfiguration.Put("UnrestrictPlates", anythingOnPlates);
		zazaConfiguration.Put("SuperActuationRod", actuatorRodPlus);
		zazaConfiguration.Put("RandomUuid", randomUUID);
		zazaConfiguration.Put("HideUuidMode", hideUUID);
		zazaConfiguration.Put("FastJoin", fastJoin);
		zazaConfiguration.Put("RejoinRememberPosition", rejoinRememberPos);
		zazaConfiguration.Put("HideCustomMOTD", hideHackedMOTD);
		zazaConfiguration.Put("EnableTinyWorlds", enableTinyWorld);
		zazaConfiguration.Put("DontDeleteHardcoreCharacters", dontDeleteCharacterOnHardcoreDeath);
		zazaConfiguration.Put("SuperFastSplash", superFastSplash);
		zazaConfiguration.Put("BackgroundMusic", backgroundMusic);
		zazaConfiguration.Put("DisableAltEnterFullscreen", disableAltEnterFullscreen);
		zazaConfiguration.Put("UseIntegratedServerHosting", serverInSameProcess);
		zazaConfiguration.Put("MultiplayerNPCInterpolationRate", nPCInterpolationRate);
		zazaConfiguration.Put("InfiniteBuffs", infiniteBuffs);
		zazaConfiguration.Put("LightingEveryFrame", lightEveryFrame);
		zazaConfiguration.Put("UseAsyncTcpSocket", useAsyncSocket);
		zazaConfiguration.Put("AntiLagProjectiles", antiLagProjectiles);
		zazaConfiguration.Put("AntiLagSounds", antiLagSounds);
		zazaConfiguration.Put("AntiLagParticles", antiLagProjectiles);
		zazaConfiguration.Put("AntiLagFps", antiLagFps);
		zazaConfiguration.Put("LockOnInPvP", LockOnHelper.targetPlayers);
		zazaConfiguration.Put("LockOnToFriendlyPlayers", ignorePlayerHostileForLockOn);
		zazaConfiguration.Put("Packets500msMode", tilePacketMaxMode);
		zazaConfiguration.Put("TilePacketRate", tilePacketRate);
		zazaConfiguration.Put("ErasePacketRate", erasePacketRate);
		zazaConfiguration.Put("LiquidPacketRate", liquidPacketRate);
		zazaConfiguration.Put("PaintPacketRate", paintPacketRate);
		zazaConfiguration.Put("SelectionUseTileSyncing", selectionUseTileSync);
		zazaConfiguration.Put("PasteModeAirBlending", blendAirTileMode);
		zazaConfiguration.Put("PasteInTshock", pasteInTShock);
		zazaConfiguration.Put("AutoExitPaste", autoExitPaste);
		zazaConfiguration.Put("PasteDelay", pasteDelay);
		zazaConfiguration.Put("DrawPhotoType", drawPhotoType);
		zazaConfiguration.Put("BrushCircle", brushCircle);
		zazaConfiguration.Put("WorldBorderSize", worldBorderSize);
		zazaConfiguration.Put("KeepVanillaBorder", keepThinVanillaBorder);
		zazaConfiguration.Put("DontClampScreen", noScreenClamp);
		zazaConfiguration.Put("ForcedSoundtrackType", forceSoundtrackId);
		zazaConfiguration.Put("RevertJungleNightTheme", oldMus_JungleNight);
		zazaConfiguration.Put("RevertUndergroundJungleTheme", oldMus_UndergroundJungle);
		zazaConfiguration.Put("ClassicDungeonTheme", oldMus_Dungeon);
		zazaConfiguration.Put("ClassicUnderworldTheme", oldMus_Underworld);
		zazaConfiguration.Put("RevertUndergroundDesertTheme", oldMus_UndergroundDesert);
		zazaConfiguration.Put("RevertDukeFishronTheme", oldMus_Fishron);
		zazaConfiguration.Put("ClassicGoblinArmyTheme", oldMus_Goblins);
		zazaConfiguration.Put("ClassicPiratesTheme", oldMus_Pirates);
		zazaConfiguration.Put("DisableMorningRainTheme", oldMus_MorningRain);
		zazaConfiguration.Put("DisableStormTheme", oldMus_Storm);
		zazaConfiguration.Put("DisableWindyDayTheme", oldMus_WindyDay);
		zazaConfiguration.Put("DisableSlimeRainTheme", oldMus_SlimeRain);
		zazaConfiguration.Put("DisableGraveyardTheme", oldMus_Graveyard);
		zazaConfiguration.Put("DisableTownThemes", oldMus_NoTown);
		zazaConfiguration.Put("BindableCommand_1", boundCommand1);
		zazaConfiguration.Put("BindableCommand_2", boundCommand2);
		zazaConfiguration.Put("BindableCommand_3", boundCommand3);
		zazaConfiguration.Put("BindableCommand_4", boundCommand4);
		zazaConfiguration.Put("BindableCommand_5", boundCommand5);
		zazaConfiguration.Put("BindableCommand_6", boundCommand6);
		zazaConfiguration.Put("BindableCommand_7", boundCommand7);
		zazaConfiguration.Put("BindableCommand_8", boundCommand8);
		zazaConfiguration.Put("EncryptedChatKey", EncChatKeyString);
		ZazaConfiguration.Save();
	}

	public void DoEarlyModUpdate()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		if (!Main._isAsyncLoadComplete)
		{
			return;
		}
		try
		{
			if (mapTeleportation && PlayerInput.Triggers.Current.KeyStatusHandler("Map TP") && Main.mapFullscreen && GetMapCoordinatesMod(Main.mouseX, Main.mouseY, out var result) && Main.hasFocus && !CustomUIMousedOver)
			{
				Vector2 vector4 = default(Vector2);
				((Vector2)(ref vector4))._002Ector((float)result.X * 16f, (float)result.Y * 16f);
				Main.LocalPlayer.position = vector4 + (Main.LocalPlayer.velocity = Vector2.Zero);
				Main.LocalPlayer.oldPosition = Main.LocalPlayer.position;
				Main.LocalPlayer.fallStart = (int)vector4.X / 16;
				Main.LocalPlayer.noFallDmg = true;
				if (Main.netMode != 0)
				{
					NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
				}
			}
			if (pasteMode && pasteDelayCounter <= 0 && !Main.gameMenu && Main.hasFocus && Main.mapFullscreen && PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Activator (Map)") && PlayerInput.Triggers.JustPressed.MouseRight && !CustomUIMousedOver)
			{
				Point result6 = default(Point);
				if (motionlessPaste)
				{
					result6 = pasteImagePreviewPos;
				}
				if (motionlessPaste || (GetMapCoordinatesModV2(Main.mouseX, Main.mouseY, 0, out result6) && result6.X > -1 && result6.Y > -1))
				{
					Schematics.PasteTiles(result6);
				}
			}
			if (IsInTileBrush && !pasteMode && Main.mapFullscreen && PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Activator (Map)") && PlayerInput.Triggers.Current.KeyStatusHandler("Use Brush (Map)") && !CustomUIMousedOver)
			{
				Point mouseToTile = default(Point);
				if (GetMapCoordinatesModV2(Main.mouseX, Main.mouseY, 0, out mouseToTile) && mouseToTile.X > -1 && mouseToTile.Y > -1)
				{
					Brushes.TileBrush(mouseToTile, tileBrushSize);
				}
			}
			if (tileExploitBrush && !pasteMode && !Main.gameMenu && Main.mapFullscreen && PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Activator (Map)") && PlayerInput.Triggers.Current.KeyStatusHandler("Use Brush (Map)") && !CustomUIMousedOver)
			{
				Point mouseToTile2 = default(Point);
				if (GetMapCoordinatesModV2(Main.mouseX, Main.mouseY, 0, out mouseToTile2) && mouseToTile2.X > -1 && mouseToTile2.Y > -1)
				{
					Brushes.TileExploitBrushV2(mouseToTile2, tileExploitBrushSize, tileExploitBrushType, tileExploitBrushFX, tileExploitBrushFY, tileExploitBrushActive);
				}
			}
			if (IsInWallBrush && !pasteMode && Main.mapFullscreen && PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Activator (Map)") && PlayerInput.Triggers.Current.KeyStatusHandler("Use Brush (Map)") && !CustomUIMousedOver)
			{
				Point result7 = default(Point);
				if (GetMapCoordinatesModV2(Main.mouseX, Main.mouseY, 0, out result7) && result7.X > -1 && result7.Y > -1)
				{
					Brushes.WallBrush(result7, wallBrushSize);
				}
			}
			if (IsInEraseBrush && !pasteMode && Main.mapFullscreen && PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Activator (Map)") && PlayerInput.Triggers.Current.KeyStatusHandler("Use Brush (Map)") && !CustomUIMousedOver)
			{
				Point result8 = default(Point);
				if (GetMapCoordinatesModV2(Main.mouseX, Main.mouseY, 0, out result8) && result8.X > -1 && result8.Y > -1)
				{
					Brushes.EraseBrush(result8, eraseBrushSize);
				}
			}
			if (IsInLiquidBrush && !pasteMode && Main.mapFullscreen && PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Activator (Map)") && PlayerInput.Triggers.Current.KeyStatusHandler("Use Brush (Map)") && !CustomUIMousedOver)
			{
				Point result9 = default(Point);
				if (GetMapCoordinatesModV2(Main.mouseX, Main.mouseY, 0, out result9) && result9.X > -1 && result9.Y > -1)
				{
					Brushes.LiquidAndDryBrush(result9, liquidBrushSize);
				}
			}
			if (IsInTilePaintBrush && !pasteMode && Main.mapFullscreen && PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Activator (Map)") && PlayerInput.Triggers.Current.KeyStatusHandler("Use Brush (Map)") && !CustomUIMousedOver)
			{
				Point result10 = default(Point);
				if (GetMapCoordinatesModV2(Main.mouseX, Main.mouseY, 0, out result10) && result10.X > -1 && result10.Y > -1)
				{
					Brushes.TilePaintBrush(result10, tilePaintBrushSize);
				}
			}
		}
		catch
		{
		}
	}

	public void DoModUpdate()
	{
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_095b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_103b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1040: Unknown result type (might be due to invalid IL or missing references)
		//IL_1096: Unknown result type (might be due to invalid IL or missing references)
		//IL_109b: Unknown result type (might be due to invalid IL or missing references)
		//IL_110a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1217: Unknown result type (might be due to invalid IL or missing references)
		//IL_1226: Unknown result type (might be due to invalid IL or missing references)
		//IL_122b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1230: Unknown result type (might be due to invalid IL or missing references)
		//IL_1238: Unknown result type (might be due to invalid IL or missing references)
		//IL_1244: Unknown result type (might be due to invalid IL or missing references)
		//IL_127d: Unknown result type (might be due to invalid IL or missing references)
		//IL_127f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1286: Unknown result type (might be due to invalid IL or missing references)
		//IL_1288: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_1319: Unknown result type (might be due to invalid IL or missing references)
		//IL_1328: Unknown result type (might be due to invalid IL or missing references)
		//IL_1338: Unknown result type (might be due to invalid IL or missing references)
		//IL_133d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1344: Unknown result type (might be due to invalid IL or missing references)
		//IL_1349: Unknown result type (might be due to invalid IL or missing references)
		if (!Main._isAsyncLoadComplete)
		{
			return;
		}
		try
		{
			if (ItemMods.Presets.Count < 1)
			{
				try
				{
					ItemMods.Presets.Add(new ItemModPreset("None", isLocked: true, "Default Item Mods Preset"));
				}
				catch (Exception ex)
				{
					LogException(ex);
				}
			}
			((Color)(ref pasteBorderColor)).R = (byte)(((Color)(ref pasteBorderColor)).R + (byte)pasteBorderColorChange);
			((Color)(ref pasteBorderColor)).G = (byte)(((Color)(ref pasteBorderColor)).G + (byte)pasteBorderColorChange);
			((Color)(ref pasteBorderColor)).B = (byte)(((Color)(ref pasteBorderColor)).B + (byte)pasteBorderColorChange);
			if (((Color)(ref pasteBorderColor)).R >= 165)
			{
				pasteBorderColorChange = -1;
			}
			if (((Color)(ref pasteBorderColor)).R <= 65)
			{
				pasteBorderColorChange = 1;
			}
			if (Main.ServerSideCharacter)
			{
				onTshockServer = true;
			}
			controlTildeTerminal = false;
			if (Main.gameMenu)
			{
				autoDeleteItems = false;
				hasLoadedIntoWorld = false;
				didTShockDisableYou = false;
				if (!Main.ServerSideCharacter)
				{
					onTshockServer = false;
				}
				tildeTerminal = false;
				customUI.SetState(null);
				markedTilesForEraseBrush.Clear();
				markedTilesForLiquidAndDryBrush.Clear();
				markedTilesForWallBrush.Clear();
				markedTilesForTileBrush.Clear();
				markedTilesForTilePaintBrush.Clear();
				handlingPackets = false;
				spectatingPlayer = false;
				spectatePlayerTarget = -1;
				showSpectatedInventory = false;
				freeCam = false;
				DisableBrushes();
				copyMode = false;
				pasteMode = false;
				loadingChunks = false;
				prevPlrCount = 0;
				ModsUIPlayerListScrollBar.previousPlayerCount = 99999;
				if (Main.menuMode == 0)
				{
					PlayerVariantID.Sets.spyingOnServer = false;
					rejoining = false;
				}
				if (Schematics.HasTargets)
				{
					Schematics.ResetPreview();
				}
			}
			if (PlayerInput.Triggers.Current.KeyStatusHandler("Open Menu"))
			{
				controlTildeTerminal = true;
			}
			if (controlTildeTerminal)
			{
				if (releaseTildeTerminal)
				{
					tildeTerminal = !tildeTerminal;
					releaseTildeTerminal = false;
				}
				releaseTildeTerminal = false;
			}
			else
			{
				releaseTildeTerminal = true;
			}
			if (!Main.drawingPlayerChat && !Main.gameMenu && !PlayerInput.WritingText)
			{
				if (PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Activator"))
				{
					if (PlayerInput.Triggers.JustPressed.KeyStatusHandler("God Mode"))
					{
						ModTogglerV2(CheatManager.GetCheat<Cheats.GodmodeCheat>());
					}
					if (PlayerInput.Triggers.JustPressed.KeyStatusHandler("Noclip"))
					{
						ModToggler("Noclip", ref noclip);
					}
					if (PlayerInput.Triggers.JustPressed.KeyStatusHandler("Ghost"))
					{
						ModToggler("Ghost mode", ref Main.LocalPlayer.ghost);
						if (Main.LocalPlayer.dead)
						{
							Main.NewText("You were dead when unghosting, so you have been respawned.", infoColor);
							Main.NewText("This may cause issues or desyncs on servers, and could be picked up on. Use with caution.", infoColor);
							Main.LocalPlayer.dead = false;
							Main.LocalPlayer.ghost = false;
							Main.LocalPlayer.deadForGood = false;
							Main.LocalPlayer.Spawn(PlayerSpawnContext.ReviveFromDeath);
						}
						NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
					}
					if (PlayerInput.Triggers.JustPressed.KeyStatusHandler("Load Config"))
					{
						LoadSettings(NoHacks: true);
						Main.NewText("Loaded configuration!", zazasFavoriteColor);
					}
				}
				if (PlayerInput.Triggers.JustPressed.KeyStatusHandler("Apply Item Mod") && ItemMods.ActivePreset.HasItem(Main.LocalPlayer.HeldItem.netID))
				{
					bool modifiedByServer = Main.LocalPlayer.HeldItem.modifiedByServer;
					ItemMods.ActivePreset.ApplyToItem(Main.LocalPlayer.HeldItem);
					if (!modifiedByServer)
					{
						Main.NewText("Succesfully applied item mod!", zazasFavoriteColor);
					}
				}
				if (PlayerInput.Triggers.JustPressed.KeyStatusHandler("Repeat Last Command") && !string.IsNullOrEmpty(lastCommand))
				{
					DoCommands(lastCommand);
				}
				if (PlayerInput.Triggers.JustPressed.KeyStatusHandler("Bindable Command 1") && !string.IsNullOrEmpty(boundCommand1))
				{
					string commandToExecute = boundCommand1;
					if (!commandToExecute.StartsWith(commandSpecifier))
					{
						commandToExecute = commandToExecute.Insert(0, $"{commandSpecifier}");
					}
					DoCommands(commandToExecute, isBoundCommand: true);
				}
				if (PlayerInput.Triggers.JustPressed.KeyStatusHandler("Bindable Command 2") && !string.IsNullOrEmpty(boundCommand2))
				{
					string commandToExecute2 = boundCommand2;
					if (!commandToExecute2.StartsWith(commandSpecifier))
					{
						commandToExecute2 = commandToExecute2.Insert(0, $"{commandSpecifier}");
					}
					DoCommands(commandToExecute2, isBoundCommand: true);
				}
				if (PlayerInput.Triggers.JustPressed.KeyStatusHandler("Bindable Command 3") && !string.IsNullOrEmpty(boundCommand3))
				{
					string commandToExecute3 = boundCommand3;
					if (!commandToExecute3.StartsWith(commandSpecifier))
					{
						commandToExecute3 = commandToExecute3.Insert(0, $"{commandSpecifier}");
					}
					DoCommands(commandToExecute3, isBoundCommand: true);
				}
				if (PlayerInput.Triggers.JustPressed.KeyStatusHandler("Bindable Command 4") && !string.IsNullOrEmpty(boundCommand4))
				{
					string commandToExecute4 = boundCommand4;
					if (!commandToExecute4.StartsWith(commandSpecifier))
					{
						commandToExecute4 = commandToExecute4.Insert(0, $"{commandSpecifier}");
					}
					DoCommands(commandToExecute4, isBoundCommand: true);
				}
				if (PlayerInput.Triggers.JustPressed.KeyStatusHandler("Bindable Command 5") && !string.IsNullOrEmpty(boundCommand5))
				{
					string commandToExecute5 = boundCommand5;
					if (!commandToExecute5.StartsWith(commandSpecifier))
					{
						commandToExecute5 = commandToExecute5.Insert(0, $"{commandSpecifier}");
					}
					DoCommands(commandToExecute5, isBoundCommand: true);
				}
				if (PlayerInput.Triggers.JustPressed.KeyStatusHandler("Bindable Command 6") && !string.IsNullOrEmpty(boundCommand6))
				{
					string commandToExecute6 = boundCommand6;
					if (!commandToExecute6.StartsWith(commandSpecifier))
					{
						commandToExecute6 = commandToExecute6.Insert(0, $"{commandSpecifier}");
					}
					DoCommands(commandToExecute6, isBoundCommand: true);
				}
				if (PlayerInput.Triggers.JustPressed.KeyStatusHandler("Bindable Command 7") && !string.IsNullOrEmpty(boundCommand7))
				{
					string commandToExecute7 = boundCommand7;
					if (!commandToExecute7.StartsWith(commandSpecifier))
					{
						commandToExecute7 = commandToExecute7.Insert(0, $"{commandSpecifier}");
					}
					DoCommands(commandToExecute7, isBoundCommand: true);
				}
				if (PlayerInput.Triggers.JustPressed.KeyStatusHandler("Bindable Command 8") && !string.IsNullOrEmpty(boundCommand8))
				{
					string commandToExecute8 = boundCommand8;
					if (!commandToExecute8.StartsWith(commandSpecifier))
					{
						commandToExecute8 = commandToExecute8.Insert(0, $"{commandSpecifier}");
					}
					DoCommands(commandToExecute8, isBoundCommand: true);
				}
			}
			CopyTilesSelcectionMode();
			Player player = Main.player[Main.myPlayer];
			if (!Main.LocalPlayer.mouseInterface)
			{
				ModsUICommandPanelButtons.sharedPrevSelectedItem = Main.LocalPlayer.selectedItem;
			}
			if (player.chest == -1)
			{
				forcedContainer = false;
			}
			if (Main.player[255].spammedProjectileCount > 0)
			{
				Main.player[255].spammedProjectileCount -= 4;
			}
			if (Main.player[255].spammedSoundCount > 0)
			{
				Main.player[255].spammedSoundCount -= 4;
			}
			if (Main.player[255].blockProjectileRecieveTime > 0)
			{
				Main.player[255].blockProjectileRecieveTime--;
			}
			if (Main.player[255].blockSoundRecieveTime > 0)
			{
				Main.player[255].blockSoundRecieveTime--;
			}
			if (!Main.gameMenu)
			{
				DoModUpdate_InWorld();
			}
			else
			{
				controlledNpc = null;
				controlledNpcType = 0;
				targetPlayerIndex = -1;
				shouldStrikeControlledNpc = true;
			}
			if (Main.netMode != 0)
			{
				controlledNpc = null;
				controlledNpcType = 0;
			}
			if (!jitterPos)
			{
				jitteringMyPosRn = false;
			}
			if (reflectAllMyProjectiles)
			{
				for (int num7 = 0; num7 < 1000; num7++)
				{
					if (Main.projectile[num7].active && Main.projectile[num7].owner == Main.myPlayer)
					{
						Main.projectile[num7].reflected = true;
						Main.projectile[num7].hostile = true;
						Main.projectile[num7].friendly = false;
					}
				}
			}
			if (gotoMouse && !Main.gameMenu && !Main.drawingPlayerChat && !Main.editSign && !Main.editChest && !PlayerInput.WritingText && !Main.LocalPlayer.mouseInterface && Main.hasFocus)
			{
				Vector2 vector3 = default(Vector2);
				((Vector2)(ref vector3))._002Ector(Main.MouseWorld.X, Main.MouseWorld.Y);
				if ((!PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Rapidfire") && PlayerInput.Triggers.JustPressed.KeyStatusHandler("Mouse TP")) || (PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Rapidfire") && PlayerInput.Triggers.Current.KeyStatusHandler("Mouse TP")))
				{
					MouseTP(vector3);
				}
			}
			if (noclip && !player.dead && !Main.gameMenu)
			{
				if (PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Activator") && PlayerInput.Triggers.JustPressed.KeyStatusHandler("Noclip Speed Modifier"))
				{
					string speedString = "";
					switch (noclipSpeed)
					{
					case 9:
						noclipSpeed = 25;
						speedString = "default";
						break;
					case 64:
						noclipSpeed = 9;
						speedString = "slow";
						break;
					default:
						noclipSpeed = 64;
						speedString = "fast";
						break;
					}
					Main.NewText("Noclip speed set to " + speedString + ".", zazasFavoriteColor);
				}
				if ((player.controlUp || player.controlJump) && !spectatingPlayer)
				{
					player.oldPosition = new Vector2(player.oldPosition.X, player.oldPosition.Y - (float)noclipSpeed * player.gravDir);
				}
				if (player.controlLeft && !spectatingPlayer)
				{
					player.oldPosition = new Vector2(player.oldPosition.X - (float)noclipSpeed, player.oldPosition.Y);
				}
				if (player.controlRight && !spectatingPlayer)
				{
					player.oldPosition = new Vector2(player.oldPosition.X + (float)noclipSpeed, player.oldPosition.Y);
				}
				if (player.controlDown && !spectatingPlayer)
				{
					player.oldPosition = new Vector2(player.oldPosition.X, player.oldPosition.Y + (float)noclipSpeed * player.gravDir);
				}
				if (player.oldPosition.X < Main.leftWorld + (float)worldBorderSize * 16f + 16f && worldBorderSize > -2)
				{
					player.oldPosition.X = Main.leftWorld + (float)worldBorderSize * 16f + 16f;
				}
				if (player.oldPosition.X + (float)player.width > Main.rightWorld - (float)worldBorderSize * 16f - 32f && worldBorderSize > -2)
				{
					_ = Main.rightWorld;
					_ = worldBorderSize;
					_ = player.width;
					player.oldPosition.X = Main.rightWorld - (float)worldBorderSize * 16f - 32f - (float)player.width;
				}
				if (player.oldPosition.Y < Main.topWorld + (float)worldBorderSize * 16f + 16f && worldBorderSize > -2)
				{
					player.oldPosition.Y = Main.topWorld + (float)worldBorderSize * 16f + 16f;
				}
				if (player.oldPosition.Y > Main.bottomWorld - (float)worldBorderSize * 16f - 32f + (float)player.height && worldBorderSize > -2)
				{
					player.oldPosition.Y = Main.bottomWorld - (float)worldBorderSize * 16f - 32f + (float)player.height;
				}
				player.position = (player.oldPosition += (player.velocity = Vector2.Zero));
				player.fallStart = (int)(player.position.Y / 16f);
				if ((player.shimmering || player.shimmerWet) && !player.IsShimmerGodMode)
				{
					player.ClearBuff(353);
				}
				player.noFallDmg = true;
			}
			if (autoButcher && !player.dead)
			{
				ButcherMobsAndNpcs(autoButcherDamage, 1);
			}
			if (invisibility)
			{
				NetMessage.SendData(16, -1, -1, null, Main.myPlayer);
			}
			if (revealMapCooldown)
			{
				Main.LocalPlayer.isOperatingAnotherEntity = false;
				if (Main.gameMenu)
				{
					revealMapCooldown = false;
				}
			}
			if (chatSpammer && !Main.gameMenu)
			{
				if (chatSpammerString == ".nochat")
				{
					spammingBeeMovieScript = false;
					ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("\n\n\n\n\n\n\n\n\n\n"));
				}
				else if (chatSpammerString == ".fakechat")
				{
					spammingBeeMovieScript = false;
					ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("\n\n\n\n\n\n\n\n\n<CFH[i/s999:5013]^%!^DF&&F!F!F%%&*GG@?GG*H*^!> Stop spamming!"));
				}
				else
				{
					spammingBeeMovieScript = false;
					ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage(chatSpammerString));
				}
			}
			if (IsInTileBrush && Main.hasFocus && !pasteMode && !Main.mapFullscreen && PlayerInput.Triggers.Current.KeyStatusHandler("Use Brush") && !Main.LocalPlayer.mouseInterface)
			{
				Brushes.TileBrush(Main.MouseWorld.ToTileCoordinates(), tileBrushSize);
			}
			if (tileExploitBrush && Main.hasFocus && !pasteMode && !Main.mapFullscreen && PlayerInput.Triggers.Current.KeyStatusHandler("Use Brush") && !Main.LocalPlayer.mouseInterface)
			{
				Brushes.TileExploitBrushV2(Main.MouseWorld.ToTileCoordinates(), tileExploitBrushSize, tileExploitBrushType, tileExploitBrushFX, tileExploitBrushFY, tileExploitBrushActive);
			}
			if (IsInWallBrush && Main.hasFocus && !pasteMode && !Main.mapFullscreen && PlayerInput.Triggers.Current.KeyStatusHandler("Use Brush") && !Main.LocalPlayer.mouseInterface)
			{
				Brushes.WallBrush(Main.MouseWorld.ToTileCoordinates(), wallBrushSize);
			}
			if (IsInEraseBrush && Main.hasFocus && !pasteMode && !Main.mapFullscreen && PlayerInput.Triggers.Current.KeyStatusHandler("Use Brush") && !Main.LocalPlayer.mouseInterface)
			{
				Brushes.EraseBrush(Main.MouseWorld.ToTileCoordinates(), eraseBrushSize);
			}
			if (IsInLiquidBrush && Main.hasFocus && !pasteMode && !Main.mapFullscreen && PlayerInput.Triggers.Current.KeyStatusHandler("Use Brush") && !Main.LocalPlayer.mouseInterface)
			{
				Brushes.LiquidAndDryBrush(Main.MouseWorld.ToTileCoordinates(), liquidBrushSize);
			}
			if (IsInTilePaintBrush && Main.hasFocus && !pasteMode && !Main.mapFullscreen && PlayerInput.Triggers.Current.KeyStatusHandler("Use Brush") && !Main.LocalPlayer.mouseInterface)
			{
				Brushes.TilePaintBrush(Main.MouseWorld.ToTileCoordinates(), tilePaintBrushSize);
			}
			if (pasteMode && pasteDelayCounter <= 0 && !Main.gameMenu && Main.hasFocus && PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Activator") && PlayerInput.Triggers.JustPressed.MouseLeft && !Main.LocalPlayer.mouseInterface && !Main.mapFullscreen)
			{
				Schematics.PasteTiles(pasteImagePreviewPos);
			}
			if (selectionMode && Main.hasFocus)
			{
				FillOrEraseSelectionPoints();
			}
			if (infiniteMana && !Main.gameMenu)
			{
				player.statMana = player.statManaMax;
			}
			if (instantRespawn && !Main.gameMenu && player.respawnTimer > 0 && player.dead)
			{
				if (Main.mouseItem.type > 0)
				{
					Main.playerInventory = true;
				}
				player.respawnTimer = 0;
				player.Spawn(PlayerSpawnContext.ReviveFromDeath);
			}
			if (autoKillProjectiles && !Main.gameMenu)
			{
				KillAllProjectiles();
			}
			if (!handlingPackets && !Main.gameMenu && Main.netMode != 0 && Brushes.tilesToBrush.Count > 0)
			{
				Brushes.HandleBrushNetMessagesV3();
			}
			if (killAura && !Main.gameMenu && !player.dead)
			{
				KillBubble(killAuraSize);
			}
			if (lag && !Main.gameMenu && Netplay.Connection.State == 10 && quedPackets < 60000)
			{
				Vector2 vector5 = Main.LocalPlayer.MountedCenter + new Vector2(0f, 3200f);
				ModsNetMessages.SendModifiedDataProj(27, -1, -1, null, 0, vector5, new Vector2(0f, 100000000f), Main.myPlayer, 611);
				Vector2 vector6 = default(Vector2);
				((Vector2)(ref vector6))._002Ector(-1600f, -1600f);
				for (int k = 0; k < 40; k++)
				{
					ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.ShimmerTownNPC, new ParticleOrchestraSettings
					{
						PositionInWorld = vector6,
						MovementVector = vector6
					});
				}
				for (int i = 0; i < 40; i++)
				{
					ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.GasTrap, new ParticleOrchestraSettings
					{
						PositionInWorld = vector6,
						MovementVector = vector6
					});
				}
				for (int j = 0; j < 40; j++)
				{
					ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.FlameWaders, new ParticleOrchestraSettings
					{
						PositionInWorld = vector6,
						MovementVector = vector6
					});
				}
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ShimmerTownNPCSend, new ParticleOrchestraSettings
				{
					PositionInWorld = new Vector2(Main.LocalPlayer.Center.X, Main.LocalPlayer.Center.Y - 128f),
					MovementVector = Vector2.Zero
				});
			}
			DoModUpdate_Timers();
		}
		catch
		{
		}
	}

	public void DoModUpdate_InWorld()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		if (OldMouseWorld != Vector2.Zero)
		{
			mouseVelocity = Main.MouseWorld - OldMouseWorld;
		}
		OldMouseWorld = Main.MouseWorld;
		if (autoDeleteItems)
		{
			DeleteGroundItemsCall(1);
		}
		if (bombFiller && selectionMode && tShockDisableTime <= 0)
		{
			BombFillSelection();
		}
		if (cloak)
		{
			Cloak();
		}
		TryDetermineDungeonPos();
		Player player = Main.player[Main.myPlayer];
		if (!IsLowFPS_Alt)
		{
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active && annoyPlayerV2[i])
				{
					SoundEngine.PlaySound(in SoundID.Item1, Main.player[i].Center);
					if (Main.netMode != 0)
					{
						NetMessage.TrySendData(51, -1, -1, null, Main.player[i].whoAmI, 2f);
					}
				}
				if (Main.player[i].active && annoyPlayerShimmer[i])
				{
					ParticleOrchestrator.BroadcastOrRequestParticleSpawn(ParticleOrchestraType.ShimmerTownNPC, new ParticleOrchestraSettings
					{
						PositionInWorld = Main.player[i].Bottom
					});
				}
				if (Main.player[i].active && annoyPlayer3[i])
				{
					ParticleOrchestrator.BroadcastOrRequestParticleSpawn(ParticleOrchestraType.GasTrap, new ParticleOrchestraSettings
					{
						PositionInWorld = Main.player[i].Center
					});
				}
				if (!Main.player[i].active)
				{
					annoyPlayerV2[i] = false;
					annoyPlayerShimmer[i] = false;
					annoyPlayer3[i] = false;
				}
			}
		}
		if (earRape)
		{
			for (int k = 0; k < 100; k++)
			{
				NetMessage.TrySendData(58, -1, -1, null, Main.myPlayer, Main.rand.Next(0, 101));
			}
		}
		if (golfBallSpammer)
		{
			bool foundTile = false;
			int foundTiles = 0;
			for (int j = (int)(Main.LocalPlayer.position.X / 16f) - 60; j < (int)(Main.LocalPlayer.position.X / 16f) + 60; j++)
			{
				if (foundTiles > 50)
				{
					break;
				}
				for (int l = (int)(Main.LocalPlayer.position.Y / 16f) - 60; l < (int)(Main.LocalPlayer.position.Y / 16f) + 60; l++)
				{
					if (foundTiles > 50)
					{
						break;
					}
					if (WorldGen.InWorld(j, l, 3) && Main.tile[j, l] != null && Main.tile[j, l].active() && Main.tile[j, l].type == 476)
					{
						foundTiles++;
						if (!foundTile)
						{
							FakeNetMessage13(Main.LocalPlayer.position, 58);
							ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, 4590f);
							int proj = Projectile.NewProjectile(null, Main.LocalPlayer.Center, Vector2.Zero, 722, 0, 0f);
							Main.projectile[proj].Kill();
						}
						for (int m = 0; m < 20; m++)
						{
							NetMessage.SendData(128, -1, -1, null, Main.myPlayer, 65535f, 721f, 0f, j, l);
						}
						if (!foundTile)
						{
							NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
							NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
							foundTile = true;
						}
					}
				}
			}
		}
		if (Main.netMode != 1)
		{
			originalGetGoodWorld = Main.getGoodWorld;
			originalRemixWorld = Main.remixWorld;
		}
		if (trollItemId != 0)
		{
			Vector2 spawnPos = default(Vector2);
			for (int num27 = 0; num27 < 255; num27++)
			{
				Player player3 = Main.player[num27];
				if (player3.active && player3.whoAmI != Main.myPlayer)
				{
					((Vector2)(ref spawnPos))._002Ector((float)Main.rand.Next((int)Main.player[num27].Center.X - 960, (int)Main.player[num27].Center.X + 961), (float)Main.rand.Next((int)Main.player[num27].Center.Y - 540, (int)Main.player[num27].Center.Y + 541));
					ParticleOrchestrator.BroadcastOrRequestParticleSpawn(ParticleOrchestraType.ItemTransfer, new ParticleOrchestraSettings
					{
						PositionInWorld = spawnPos,
						MovementVector = Main.player[num27].Center - spawnPos,
						UniqueInfoPiece = ((trollItemId == -1) ? Main.rand.Next(1, 5456) : trollItemId)
					});
				}
			}
		}
		try
		{
			if (NPCID.Sets.ShouldUpdateEarlierForPossession[controlledNpcType] && Main.netMode == 0)
			{
				PossessNPC(player);
			}
		}
		catch
		{
			controlledNpc = null;
			controlledNpcType = 0;
			shouldStrikeControlledNpc = true;
		}
		if (serverSideCursor && !spectatingPlayer && spectatePlayerTarget == -1 && !freeCam)
		{
			ParticleOrchestrator.BroadcastOrRequestParticleSpawn(ParticleOrchestraType.ShimmerBlock, new ParticleOrchestraSettings
			{
				PositionInWorld = Main.MouseWorld
			});
		}
		if (jitterPos && !jitteringMyPosRn && Main.netMode != 0)
		{
			JitterPlayerPosition();
		}
		if (crystalKillAura)
		{
			for (int n = 0; n < Main.npc.Length; n++)
			{
				NPC nPC2 = Main.npc[n];
				if (!nPC2.active || !DD2Event.Ongoing)
				{
					continue;
				}
				for (int num28 = 0; num28 < Main.npc.Length; num28++)
				{
					NPC nPC3 = Main.npc[num28];
					if (!nPC3.active || nPC3.type != 548 || ((nPC2.type != 636 || !(nPC2.Distance(Main.npc[num28].Center) <= 2048f)) && (nPC2.type != 370 || !(nPC2.Distance(Main.npc[num28].Center) <= 512f)) && (nPC2.friendly || nPC2.townNPC || NPCID.Sets.BelongsToInvasionOldOnesArmy[nPC2.type] || NPCID.Sets.CountsAsCritter[nPC2.type] || nPC2.type == 396 || nPC2.type == 397 || nPC2.type == 398 || !(nPC2.Distance(Main.npc[num28].Center) <= 256f))))
					{
						continue;
					}
					for (int num29 = 0; num29 < 10; num29++)
					{
						nPC2.StrikeNPCNoInteraction(32767, 10f, -nPC2.direction, crit: true);
						if (Main.netMode != 0)
						{
							FalsePlayerPacket(new Vector2(nPC2.position.X, nPC2.position.Y));
							NetMessage.SendData(28, -1, -1, null, n, 32767f, 10f, 0f - (float)nPC2.direction, 1);
							NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
						}
					}
				}
			}
		}
		CheatManager.TickAllCheats();
	}

	public void DoModUpdate_Timers()
	{
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		if (didTShockDisableYou && tShockDisableTime <= 0)
		{
			didTShockDisableYou = false;
		}
		if (tShockDisableTime > 0)
		{
			tShockDisableTime--;
		}
		if (concurrentPastes < 0)
		{
			concurrentPastes = 0;
		}
		if (globalBlockedProjTime > 0)
		{
			globalBlockedProjTime--;
		}
		if (noSoundsTime > 0)
		{
			noSoundsTime--;
		}
		if (noKillSoundTime > 0)
		{
			noKillSoundTime--;
		}
		if (disableProjLightTime > 0)
		{
			disableProjLightTime--;
		}
		if (quedMapRefreshTime > 0)
		{
			quedMapRefreshTime--;
			if (quedMapRefreshTime == 0)
			{
				Main.refreshMap = true;
			}
		}
		int totalCount = 0;
		for (int i = 0; i < Main.player.Length - 1; i++)
		{
			Player p = Main.player[i];
			if (autoCrashDelays_Explosives[i] > 0)
			{
				autoCrashDelays_Explosives[i]--;
				totalCount++;
			}
			else if (i != Main.myPlayer && p.active && p.spawned && !p.IsZazaUser && autoMobileCrash_Explosives && autoCrashDelays_Explosives[i] <= 0 && (p.mount.Type == 8 || PlayerHoldingExplosive(p)))
			{
				WhisperAndMobileCrash(i);
				autoCrashDelays_Explosives[i] = 900;
				ParticleOrchestrator.BroadcastOrRequestParticleSpawn(ParticleOrchestraType.GasTrap, new ParticleOrchestraSettings
				{
					PositionInWorld = p.Center
				});
			}
			if (autoCrashDelays_Dressers[i] > 0)
			{
				autoCrashDelays_Dressers[i]--;
				totalCount++;
			}
			else if (i != Main.myPlayer && p.active && p.spawned && !p.IsZazaUser && autoMobileCrash_Dressers && autoCrashDelays_Dressers[i] <= 0 && IsNearLiquidDresser(p) && (PlayerHoldingLiquidBucket(p) || IsAnyCoin(p.HeldItem) || IsAnyCreateTile(p.HeldItem)))
			{
				WhisperAndMobileCrash(i);
				autoCrashDelays_Dressers[i] = 900;
				ParticleOrchestrator.BroadcastOrRequestParticleSpawn(ParticleOrchestraType.GasTrap, new ParticleOrchestraSettings
				{
					PositionInWorld = p.Center
				});
			}
		}
		if (fixUpperTime > 0)
		{
			cancelPaste = true;
			cancelAction = true;
			concurrentPastes = 0;
			concurrentFillOrDels = 0;
			concurrentConversions = 0;
			openingTEDITSchematic = false;
			savingTEDITSchematic = false;
			fixUpperTime--;
		}
		if (automaticMartians && automaticMartiansWaitTime > 0)
		{
			automaticMartiansWaitTime--;
			if (automaticMartiansWaitTime == 0 && (Main.invasionType == 0 || (Main.invasionProgressIcon == 7 && Main.invasionProgress == Main.invasionProgressMax)))
			{
				NetMessage.SendData(61, -1, -1, null, Main.myPlayer, -7f);
			}
			automaticMartiansWaitTime = 180;
		}
		if (awaitTshockResponseTime > 0 && Netplay.Connection.TimeOutTimer < 40)
		{
			awaitTshockResponseTime--;
		}
		if (awaitingTshockPvpResponseTime > 0 && Netplay.Connection.TimeOutTimer < 40)
		{
			awaitingTshockPvpResponseTime--;
			if (awaitingTshockPvpResponseTime == 0)
			{
				Main.NewText((detectedVanishedPlayers > 0) ? ("Detected " + detectedVanishedPlayers + " players in vanish!") : "Did not detect any vanished players.", zazasFavoriteColor);
				detectedVanishedPlayers = 0;
			}
		}
		if (globalBlockedProjTime == 0)
		{
			shownLowFpsBlockedProjMsg = false;
			for (int j = 0; j < Main.projectile.Length; j++)
			{
				Projectile proj = Main.projectile[j];
				if (proj.active && proj.type != proj.originalType)
				{
					proj.type = proj.originalType;
					proj.scale = proj.originalScale;
				}
			}
		}
		if (pasteDelayCounter > 0)
		{
			pasteDelayCounter--;
		}
		if (encryptChatWarnTime > 0 && encryptChat && !encryptChatWarning && !Main.gameMenu)
		{
			encryptChatWarnTime--;
			if (encryptChatWarnTime <= 0)
			{
				EncryptedChatWarningPopup();
				encryptChatWarnTime = 18000;
			}
		}
		else
		{
			encryptChatWarnTime = 18000;
		}
	}

	public static void EncryptedChatWarningPopup()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		encryptedChatWarningPopup = PopupText.SpecialNewText(new AdvancedPopupRequest
		{
			Text = $"Reminder... you're using encrypted chat! Run the {commandSpecifier}echat command to return to normal chatting.",
			DurationInFrames = 600,
			Velocity = Vector2.Zero,
			Color = infoColor * 1.15f
		}, Main.LocalPlayer.Top);
		SoundEngine.PlaySound(in SoundID.LucyTheAxeTalk, Main.LocalPlayer.Top);
	}

	public static void Cloak()
	{
		for (int i = 0; i < 59; i++)
		{
			if (cloak && cloakMode != -555 && i != Main.LocalPlayer.selectedItem)
			{
				NetMessage.SendData(5, -1, -1, null, Main.myPlayer, i, 0f, 0f, 0, 6);
			}
			else
			{
				NetMessage.SendData(5, -1, -1, null, Main.myPlayer, i);
			}
		}
	}

	public static void DoCommands(string chatText, bool isBoundCommand = false)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0645: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_0609: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09df: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0978: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ded: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d69: Unknown result type (might be due to invalid IL or missing references)
		//IL_109a: Unknown result type (might be due to invalid IL or missing references)
		//IL_100b: Unknown result type (might be due to invalid IL or missing references)
		//IL_107b: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_106b: Unknown result type (might be due to invalid IL or missing references)
		//IL_118d: Unknown result type (might be due to invalid IL or missing references)
		//IL_125d: Unknown result type (might be due to invalid IL or missing references)
		//IL_117d: Unknown result type (might be due to invalid IL or missing references)
		//IL_114d: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_116d: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1275: Unknown result type (might be due to invalid IL or missing references)
		//IL_130c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1320: Unknown result type (might be due to invalid IL or missing references)
		//IL_1334: Unknown result type (might be due to invalid IL or missing references)
		//IL_1348: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_162b: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_168d: Unknown result type (might be due to invalid IL or missing references)
		//IL_161b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1504: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1720: Unknown result type (might be due to invalid IL or missing references)
		//IL_1725: Unknown result type (might be due to invalid IL or missing references)
		//IL_172a: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1951: Unknown result type (might be due to invalid IL or missing references)
		//IL_1956: Unknown result type (might be due to invalid IL or missing references)
		//IL_195b: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_196e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1978: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b88: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1809: Unknown result type (might be due to invalid IL or missing references)
		//IL_181e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e86: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2143: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2133: Unknown result type (might be due to invalid IL or missing references)
		//IL_2308: Unknown result type (might be due to invalid IL or missing references)
		//IL_228a: Unknown result type (might be due to invalid IL or missing references)
		//IL_21db: Unknown result type (might be due to invalid IL or missing references)
		//IL_22a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_22cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_22df: Unknown result type (might be due to invalid IL or missing references)
		//IL_2092: Unknown result type (might be due to invalid IL or missing references)
		//IL_207b: Unknown result type (might be due to invalid IL or missing references)
		//IL_233e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2269: Unknown result type (might be due to invalid IL or missing references)
		//IL_2355: Unknown result type (might be due to invalid IL or missing references)
		//IL_23a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_236b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2486: Unknown result type (might be due to invalid IL or missing references)
		//IL_2461: Unknown result type (might be due to invalid IL or missing references)
		//IL_2451: Unknown result type (might be due to invalid IL or missing references)
		//IL_24cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2796: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2850: Unknown result type (might be due to invalid IL or missing references)
		//IL_2872: Unknown result type (might be due to invalid IL or missing references)
		//IL_2924: Unknown result type (might be due to invalid IL or missing references)
		//IL_2946: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cac: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dda: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dba: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e78: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e68: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e58: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f16: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f06: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ef6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_3040: Unknown result type (might be due to invalid IL or missing references)
		//IL_302a: Unknown result type (might be due to invalid IL or missing references)
		//IL_301a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3004: Unknown result type (might be due to invalid IL or missing references)
		//IL_30da: Unknown result type (might be due to invalid IL or missing references)
		//IL_3141: Unknown result type (might be due to invalid IL or missing references)
		//IL_31d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_315e: Unknown result type (might be due to invalid IL or missing references)
		//IL_33b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_321a: Unknown result type (might be due to invalid IL or missing references)
		//IL_33a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3394: Unknown result type (might be due to invalid IL or missing references)
		//IL_33fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_34d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3384: Unknown result type (might be due to invalid IL or missing references)
		//IL_373c: Unknown result type (might be due to invalid IL or missing references)
		//IL_384e: Unknown result type (might be due to invalid IL or missing references)
		//IL_37c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b54: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bca: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e25: Unknown result type (might be due to invalid IL or missing references)
		//IL_3db7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4015: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ed5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4067: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_40c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4005: Unknown result type (might be due to invalid IL or missing references)
		//IL_4144: Unknown result type (might be due to invalid IL or missing references)
		//IL_4126: Unknown result type (might be due to invalid IL or missing references)
		//IL_41a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_41b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_41c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4255: Unknown result type (might be due to invalid IL or missing references)
		//IL_42b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_42d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4327: Unknown result type (might be due to invalid IL or missing references)
		//IL_437b: Unknown result type (might be due to invalid IL or missing references)
		//IL_43e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_443d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4491: Unknown result type (might be due to invalid IL or missing references)
		//IL_44e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4539: Unknown result type (might be due to invalid IL or missing references)
		//IL_458d: Unknown result type (might be due to invalid IL or missing references)
		//IL_45e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4691: Unknown result type (might be due to invalid IL or missing references)
		//IL_46e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4825: Unknown result type (might be due to invalid IL or missing references)
		//IL_482a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4835: Unknown result type (might be due to invalid IL or missing references)
		//IL_483a: Unknown result type (might be due to invalid IL or missing references)
		//IL_486a: Unknown result type (might be due to invalid IL or missing references)
		//IL_486f: Unknown result type (might be due to invalid IL or missing references)
		//IL_487f: Unknown result type (might be due to invalid IL or missing references)
		//IL_48c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_47cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4794: Unknown result type (might be due to invalid IL or missing references)
		//IL_4751: Unknown result type (might be due to invalid IL or missing references)
		//IL_492e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4933: Unknown result type (might be due to invalid IL or missing references)
		//IL_493e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4943: Unknown result type (might be due to invalid IL or missing references)
		//IL_4973: Unknown result type (might be due to invalid IL or missing references)
		//IL_4978: Unknown result type (might be due to invalid IL or missing references)
		//IL_4988: Unknown result type (might be due to invalid IL or missing references)
		//IL_49cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_49db: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a49: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b07: Unknown result type (might be due to invalid IL or missing references)
		//IL_4af7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bca: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c93: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ea5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e95: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e85: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eff: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_506d: Unknown result type (might be due to invalid IL or missing references)
		//IL_505d: Unknown result type (might be due to invalid IL or missing references)
		//IL_504d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5157: Unknown result type (might be due to invalid IL or missing references)
		//IL_5147: Unknown result type (might be due to invalid IL or missing references)
		//IL_51de: Unknown result type (might be due to invalid IL or missing references)
		//IL_51ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_51be: Unknown result type (might be due to invalid IL or missing references)
		//IL_5137: Unknown result type (might be due to invalid IL or missing references)
		//IL_5258: Unknown result type (might be due to invalid IL or missing references)
		//IL_5248: Unknown result type (might be due to invalid IL or missing references)
		//IL_5238: Unknown result type (might be due to invalid IL or missing references)
		//IL_52a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_53cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_53bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_534a: Unknown result type (might be due to invalid IL or missing references)
		//IL_535f: Unknown result type (might be due to invalid IL or missing references)
		//IL_53ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_54f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_54e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5486: Unknown result type (might be due to invalid IL or missing references)
		//IL_54d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_56d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_56c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_56b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_574a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5646: Unknown result type (might be due to invalid IL or missing references)
		//IL_573a: Unknown result type (might be due to invalid IL or missing references)
		//IL_572a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5656: Unknown result type (might be due to invalid IL or missing references)
		//IL_57cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_57bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_57ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_584c: Unknown result type (might be due to invalid IL or missing references)
		//IL_583c: Unknown result type (might be due to invalid IL or missing references)
		//IL_582c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5893: Unknown result type (might be due to invalid IL or missing references)
		//IL_58da: Unknown result type (might be due to invalid IL or missing references)
		//IL_5921: Unknown result type (might be due to invalid IL or missing references)
		//IL_5996: Unknown result type (might be due to invalid IL or missing references)
		//IL_59ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c68: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bea: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d39: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e68: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e58: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f55: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ec4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f45: Unknown result type (might be due to invalid IL or missing references)
		//IL_6103: Unknown result type (might be due to invalid IL or missing references)
		//IL_60a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_6089: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_6079: Unknown result type (might be due to invalid IL or missing references)
		//IL_601d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6253: Unknown result type (might be due to invalid IL or missing references)
		//IL_61f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_61da: Unknown result type (might be due to invalid IL or missing references)
		//IL_61ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_616e: Unknown result type (might be due to invalid IL or missing references)
		//IL_62e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_62d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_63d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_6340: Unknown result type (might be due to invalid IL or missing references)
		//IL_63c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_6512: Unknown result type (might be due to invalid IL or missing references)
		//IL_6474: Unknown result type (might be due to invalid IL or missing references)
		//IL_642d: Unknown result type (might be due to invalid IL or missing references)
		//IL_652f: Unknown result type (might be due to invalid IL or missing references)
		//IL_64d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_65ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_666e: Unknown result type (might be due to invalid IL or missing references)
		//IL_66c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_673b: Unknown result type (might be due to invalid IL or missing references)
		//IL_67f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_681c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6843: Unknown result type (might be due to invalid IL or missing references)
		//IL_6897: Unknown result type (might be due to invalid IL or missing references)
		//IL_68eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_690b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6919: Unknown result type (might be due to invalid IL or missing references)
		//IL_6933: Unknown result type (might be due to invalid IL or missing references)
		//IL_69bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_6adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aea: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ce5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_6dc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f73: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_7049: Unknown result type (might be due to invalid IL or missing references)
		//IL_6eb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_6eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ece: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ed3: Unknown result type (might be due to invalid IL or missing references)
		//IL_6edb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ee7: Unknown result type (might be due to invalid IL or missing references)
		//IL_70a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_714c: Unknown result type (might be due to invalid IL or missing references)
		//IL_70d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_70e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_7162: Unknown result type (might be due to invalid IL or missing references)
		//IL_7171: Unknown result type (might be due to invalid IL or missing references)
		//IL_7180: Unknown result type (might be due to invalid IL or missing references)
		//IL_718f: Unknown result type (might be due to invalid IL or missing references)
		//IL_71af: Unknown result type (might be due to invalid IL or missing references)
		//IL_7561: Unknown result type (might be due to invalid IL or missing references)
		//IL_7613: Unknown result type (might be due to invalid IL or missing references)
		//IL_75fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_75e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_7842: Unknown result type (might be due to invalid IL or missing references)
		//IL_77a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_78cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_7874: Unknown result type (might be due to invalid IL or missing references)
		//IL_76a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_76be: Unknown result type (might be due to invalid IL or missing references)
		//IL_76e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_76fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_7723: Unknown result type (might be due to invalid IL or missing references)
		//IL_7725: Unknown result type (might be due to invalid IL or missing references)
		//IL_7734: Unknown result type (might be due to invalid IL or missing references)
		//IL_7739: Unknown result type (might be due to invalid IL or missing references)
		//IL_773b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7740: Unknown result type (might be due to invalid IL or missing references)
		//IL_7979: Unknown result type (might be due to invalid IL or missing references)
		//IL_79f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_79e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_7969: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7928: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_7bc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c42: Unknown result type (might be due to invalid IL or missing references)
		//IL_7be1: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7cfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_7df2: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ebf: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ee6: Unknown result type (might be due to invalid IL or missing references)
		if (!isBoundCommand)
		{
			lastCommand = chatText;
		}
		Player localPlayer = Main.LocalPlayer;
		string[] array = chatText.Substring(1).Split(new char[1] { ' ' });
		string text = array[0].ToLower();
		if (text == "config")
		{
			LoadSettings(NoHacks: true);
			Main.NewText("Loaded configuration!", zazasFavoriteColor);
		}
		if (text == "help")
		{
			Main.NewText("----------------------------------------------", zazasFavoriteColor);
			Main.NewText("Some hacks are not accessible through the user interface.", zazasFavoriteColor);
			Main.NewText("View 'Commands.txt' for a list of every possible command.", zazasFavoriteColor);
			Main.NewText("----------------------------------------------", zazasFavoriteColor);
			return;
		}
		if (text == "e" || text == "enc")
		{
			if (array.Length > 1 && chatText.Length > array[0].Length + 2)
			{
				ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage(EncryptedMessageTagHandler.GenerateTag(chatText.Substring(array[0].Length + 2))));
			}
			return;
		}
		if (text == "echat" || text == "encchat" || text == "encryptchat")
		{
			encryptChat = !encryptChat;
			Main.NewText("Switched to " + (encryptChat ? "encrypted" : "normal") + " chat mode.", zazasFavoriteColor);
			return;
		}
		if (text == "tilebrushexploit" || text == "tbe")
		{
			if (array.Length > 5)
			{
				if (short.TryParse(array[5], out tileExploitBrushFY))
				{
					tileExploitBrush = true;
				}
				else
				{
					Main.NewText("Invalid Short ARRAY[5] ", errorColor);
				}
			}
			if (array.Length > 4)
			{
				if (short.TryParse(array[4], out tileExploitBrushFX))
				{
					tileExploitBrush = true;
				}
				else
				{
					Main.NewText("Invalid Short ARRAY[4] ", errorColor);
				}
			}
			if (array.Length > 3)
			{
				if (bool.TryParse(array[3], out tileExploitBrushActive))
				{
					tileExploitBrush = true;
				}
				else
				{
					Main.NewText("Invalid Boolean ARRAY[3] ", errorColor);
				}
			}
			if (array.Length > 2)
			{
				if (ushort.TryParse(array[2], out tileExploitBrushType))
				{
					tileExploitBrush = true;
				}
				else
				{
					Main.NewText("Invalid UShort ARRAY[2] ", errorColor);
				}
			}
			if (array.Length > 1)
			{
				if (int.TryParse(array[1], out tileExploitBrushSize))
				{
					tileExploitBrush = true;
					Main.NewText(string.Format("Tile Exploit Brush " + (tileExploitBrush ? "En" : "Dis") + "abled  Size: {0}  Type: {1}  Active: {2}  FX: {3}  FY: {4}", tileExploitBrushSize, tileExploitBrushType, tileExploitBrushActive, tileExploitBrushFX, tileExploitBrushFY), zazasFavoriteColor);
				}
				else
				{
					Main.NewText("Invalid Int ARRAY[1]  ", errorColor);
				}
			}
			else
			{
				tileExploitBrush = !tileExploitBrush;
				if (tileExploitBrush)
				{
					Main.NewText(string.Format("Tile Exploit Brush " + (tileExploitBrush ? "En" : "Dis") + "abled  Size: {0}  Type: {1}  Active: {2}  FX: {3}  FY: {4}", tileExploitBrushSize, tileExploitBrushType, tileExploitBrushActive, tileExploitBrushFX, tileExploitBrushFY), zazasFavoriteColor);
				}
				else
				{
					Main.NewText("Tile Exploit Brush " + (tileExploitBrush ? "En" : "Dis") + "abled", zazasFavoriteColor);
				}
			}
			return;
		}
		if (text == "notify")
		{
			notifyServerModifiedPlayer = !notifyServerModifiedPlayer;
			Main.NewText("Messages notifying when server modifies your player " + (notifyServerModifiedPlayer ? "en" : "dis") + "abled.", zazasFavoriteColor);
			return;
		}
		if (text == "detsp")
		{
			notifyPacket4 = !notifyPacket4;
			Main.NewText("Detecting spying players " + (notifyPacket4 ? "en" : "dis") + "abled.", zazasFavoriteColor);
			return;
		}
		if (text == "rapidpos")
		{
			rapidPosUpdate = !rapidPosUpdate;
			Main.NewText("Rapid position updates " + (rapidPosUpdate ? "en" : "dis") + "abled.", zazasFavoriteColor);
			return;
		}
		if (text == "blockssc")
		{
			blockServerControl = !blockServerControl;
			Main.NewText("Server side character handling will " + (blockServerControl ? "now be " : "no longer be ") + "blocked.", zazasFavoriteColor);
			return;
		}
		if (text == "moon")
		{
			if (array.Length > 1)
			{
				int moonPhase = 0;
				if (int.TryParse(array[1], out moonPhase))
				{
					Main.moonPhase = moonPhase;
					Main.NewText($"Moon phase set to {moonPhase}", zazasFavoriteColor);
				}
				else
				{
					Main.NewText("Not a valid int", errorColor);
				}
			}
			else
			{
				Main.NewText("Value Required.", errorColor);
			}
			return;
		}
		if (text == "time")
		{
			if (array.Length > 1)
			{
				double time = 0.0;
				if (double.TryParse(array[1], out time))
				{
					Main.time = time;
					Main.NewText($"Time set to {time}", zazasFavoriteColor);
				}
				else
				{
					Main.NewText("Not a valid double", errorColor);
				}
			}
			return;
		}
		if (text == "forcepaint" || text == "fpaint")
		{
			if (array.Length > 1)
			{
				byte paintId = 0;
				if (!byte.TryParse(array[1], out paintId) || paintId >= PaintID.Count)
				{
					Main.NewText("Invalid Paint ID.", errorColor);
					return;
				}
				paintOverride = paintId;
				if (paintCoatOverride != 0 && paintOverride != 0)
				{
					paintCoatOverride = 0;
					Main.NewText("Paint coating override disabled.", zazasFavoriteColor);
				}
				if (paintOverride != 0)
				{
					Main.NewText("Paint override set to " + PaintID.Search.GetName((int)paintId) + ".", zazasFavoriteColor);
				}
				else
				{
					Main.NewText("Paint override disabled.", zazasFavoriteColor);
				}
			}
			else
			{
				Main.NewText("Value Required.", errorColor);
			}
			return;
		}
		if (text == "forcepaintcoat" || text == "fpaintcoat")
		{
			if (array.Length > 1)
			{
				byte paintCoatId = 0;
				if (!byte.TryParse(array[1], out paintCoatId) || paintCoatId >= PaintCoatingID.Count)
				{
					Main.NewText("Invalid Paint Coating ID.", errorColor);
					return;
				}
				paintCoatOverride = paintCoatId;
				if (paintOverride != 0 && paintCoatOverride != 0)
				{
					paintOverride = 0;
					Main.NewText("Paint override disabled.", zazasFavoriteColor);
				}
				if (paintCoatOverride != 0)
				{
					Main.NewText("Paint coat override set to " + PaintCoatingID.Search.GetName((int)paintCoatId) + ".", zazasFavoriteColor);
				}
				else
				{
					Main.NewText("Paint coating override disabled.", zazasFavoriteColor);
				}
			}
			else
			{
				Main.NewText("Value Required.", errorColor);
			}
			return;
		}
		if (text == "plb")
		{
			placeAnywhere = !placeAnywhere;
			Main.NewText("Placement bypass " + (placeAnywhere ? "en" : "dis") + "abled.", zazasFavoriteColor);
			return;
		}
		if (text == "noinvis")
		{
			seeInvisPlayers = !seeInvisPlayers;
			Main.NewText("Invisibility for others will " + (seeInvisPlayers ? "now be " : "no longer be ") + "hidden.", zazasFavoriteColor);
			return;
		}
		if (text == "crt" || text == "crtshader")
		{
			useCrt = !useCrt;
			Main.NewText("Using CRT Shader " + (useCrt ? "en" : "dis") + "abled.", zazasFavoriteColor);
			if (useCrt && !Filters.Scene["CRT"].IsActive())
			{
				Filters.Scene.Activate("CRT", default(Vector2));
			}
			else if (!useCrt && Filters.Scene["CRT"].IsActive())
			{
				Filters.Scene.Deactivate("CRT");
			}
			return;
		}
		if (text == "gameboy")
		{
			useGameBoy = !useGameBoy;
			Main.NewText("GameBoy Shader " + (useGameBoy ? "en" : "dis") + "abled.", zazasFavoriteColor);
			if (useGameBoy && !Filters.Scene["Test2"].IsActive())
			{
				Filters.Scene.Activate("Test2", default(Vector2));
			}
			else if (!useGameBoy && Filters.Scene["Test2"].IsActive())
			{
				Filters.Scene.Deactivate("Test2");
			}
			return;
		}
		if (text == "seeinvis")
		{
			renderInvisibleTiles = !renderInvisibleTiles;
			Main.NewText("Always visible echo tiles " + (renderInvisibleTiles ? "en" : "dis") + "abled.", zazasFavoriteColor);
			return;
		}
		if (text == "specialthanks" || text == "spth" || text == "thanks")
		{
			Main.NewText("I would like to give a special thanks to those who helped me along the way with making this client. It wouldn't have been possible without them.", zazasFavoriteColor);
			Main.NewText("Bun/Cooki - Helped me get started, taught me how to properly edit using dnSpy, and made some features present in the client like bomb filler.", zazasFavoriteColor);
			Main.NewText("Will - Good friend of mine. Helped me understand a lot about C# and made my life a lot easier when working on this.", zazasFavoriteColor);
			Main.NewText("Bryld - Helped me make a few things. Made the biome spoofers, came up with several ideas & more. It's a lot to list.", zazasFavoriteColor);
			Main.NewText(".3 - Created the original client. Sparked my interest in making my own shit for this game.", zazasFavoriteColor);
			Main.NewText("yay - Code for rejoin command that they posted in the Terraform discord. It's not mine, after all.", zazasFavoriteColor);
			Main.NewText("Async - Linux clipboard stuff in the ReLogic library.", zazasFavoriteColor);
			Main.NewText("Arc - Testing very early builds and giving some ideas. Gave me the idea to make Super Paint Brush!", zazasFavoriteColor);
			Main.NewText("JacquesTheThird - New keybinds system and other things I forgot. I'm forgetful as hell.", zazasFavoriteColor);
			Main.NewText("Veik - For creating the Better Chat mod, which is used in the client. Links to the GitHub & Steam Workshop page in the README.", zazasFavoriteColor);
			Main.NewText("moorowl - Pixelated Backgrounds mod. I didn't realize this was already made by someone else for tModLoader when implementing it, so I felt credit was necessary.", zazasFavoriteColor);
			Main.NewText("NotLe0n - Better Zoom mod for tModLoader. I'm really bad at math, so I borrowed their formula for the zoom slider in the settings. Credit where credit is due.", zazasFavoriteColor);
			Main.NewText("tomat - Not Quite Nitrate mod for tModLoader. Some of their optimizations are in ZazaMod.", zazasFavoriteColor);
			Main.NewText("tML Team - For creating tModLoader & creating a lot of code that I borrowed to get this running on .NET 8 for all platforms. Including their Tile & SoundEngine rewrites. I wanted to reduce memory usage & reduce/stop access violations from FAudio.", zazasFavoriteColor);
			return;
		}
		if (text == "scanworld")
		{
			if (array.Length > 1)
			{
				int tileId = -1;
				bool teleport = false;
				if (!int.TryParse(array[1], out tileId) || tileId < 0 || tileId > 692)
				{
					Main.NewText("Invalid Tile ID.", errorColor);
					return;
				}
				if (array.Length > 2)
				{
					bool.TryParse(array[2], out teleport);
				}
				ScanWorldForSpecificTile(tileId, teleport);
			}
			else
			{
				Main.NewText("Value Required.", errorColor);
			}
			return;
		}
		if (text == "replaceliquid" || text == "rpl")
		{
			if (Main.netMode != 0)
			{
				Main.NewText("You can only do this offline.", errorColor);
			}
			else if (array.Length > 1)
			{
				int replacedLiquid = -1;
				if (int.TryParse(array[1], out var toReplace) && replacedLiquid >= 0 && replacedLiquid < LiquidID.Count)
				{
					replacedLiquid = toReplace;
				}
				if (array.Length <= 2)
				{
					return;
				}
				if (!byte.TryParse(array[2], out var newId) || newId > LiquidID.Count)
				{
					Main.NewText("Invalid Liquid ID.", errorColor);
					return;
				}
				byte newLiquidId = newId;
				if (replacedLiquid == newLiquidId)
				{
					Main.NewText("These are the same liquid IDs.", errorColor);
				}
				else
				{
					ReplaceAllLiquids(newLiquidId, replacedLiquid);
				}
			}
			else
			{
				Main.NewText("Value Required.", errorColor);
			}
			return;
		}
		if (text == "remalltile" || text == "remtile")
		{
			if (array.Length > 1)
			{
				int tileId2 = -1;
				if (!int.TryParse(array[1], out tileId2) || tileId2 < 0 || tileId2 > 692)
				{
					Main.NewText("Invalid Tile ID.", errorColor);
				}
				else
				{
					RemoveTilesOfType(tileId2);
				}
			}
			else
			{
				Main.NewText("Value Required.", errorColor);
			}
			return;
		}
		if (text == "replacealltiles" || text == "repltile")
		{
			int tileType = Main.LocalPlayer.HeldItem.createTile;
			if (tileType != -1 && tileType != 137 && Main.tileSolid[tileType] && !TileID.Sets.Platforms[tileType] && !TileObjectData.CustomPlace(tileType, localPlayer.HeldItem.placeStyle))
			{
				ReplaceAllTilesWithType(tileType);
			}
			else
			{
				Main.NewText("You need to hold the item that creates the tile you want to replace all tiles in the " + (selectionMode ? "selecttion" : "world") + " with.\nThe tile must be solid and must not be a platform or trap tile.", errorColor);
			}
			return;
		}
		if (text == "tp")
		{
			if (array.Length > 1)
			{
				int X = (int)localPlayer.Bottom.X / 16;
				int Y = (int)localPlayer.Bottom.Y / 16;
				if (array[1] != "*" && !int.TryParse(array[1], out X))
				{
					Main.NewText("Invalid X position.", errorColor);
					X = (int)localPlayer.Bottom.X / 16;
				}
				if (array.Length > 2 && array[2] != "*" && !int.TryParse(array[2], out Y))
				{
					Main.NewText("Invalid Y position.", errorColor);
					Y = (int)localPlayer.Bottom.Y / 16;
				}
				localPlayer.BottomLeft = new Vector2((float)X * 16f - (float)(localPlayer.width / 2) + 8f, (float)Y * 16f);
				localPlayer.oldPosition = localPlayer.position;
				localPlayer.fallStart = (int)localPlayer.position.Y;
				Instance.OldCenterMod = localPlayer.Center;
				Main.LocalPlayer.noFallDmg = true;
				NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
				Main.NewText($"Teleported to {X}, {Y}", zazasFavoriteColor);
			}
			else
			{
				Main.NewText("Coordinates Required.", errorColor);
			}
			return;
		}
		if (text == "tptoinf")
		{
			Main.LocalPlayer.position = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
			return;
		}
		if (text == "spawnpos" || text == "setworldspawn")
		{
			if (Main.netMode == 0)
			{
				Main.spawnTileX = (int)localPlayer.Bottom.X / 16;
				Main.spawnTileY = (int)localPlayer.Bottom.Y / 16;
				Main.NewText("Set world spawn to " + Main.spawnTileX + ", " + Main.spawnTileY, zazasFavoriteColor);
			}
			else
			{
				Main.NewText("You can't change the world spawn in an online game.", errorColor);
			}
			return;
		}
		if (text == "dungeonpos" || text == "setdungeonpos")
		{
			if (Main.netMode == 0)
			{
				Main.dungeonX = (int)localPlayer.Bottom.X / 16;
				Main.dungeonY = (int)localPlayer.Bottom.Y / 16;
				Main.NewText("Set dungeon position to " + Main.dungeonX + ", " + Main.dungeonX);
			}
			else
			{
				Main.NewText("You can't change the dungeon position in an online game.", errorColor);
			}
			return;
		}
		if (text == "freewire")
		{
			blockWirePayment = !blockWirePayment;
			Main.NewText("Free mass wire operations " + (blockWirePayment ? "en" : "dis") + "abled.", zazasFavoriteColor);
			return;
		}
		if (text == "mapoutsidelight" || text == "mpol")
		{
			if (array.Length > 1)
			{
				if (byte.TryParse(array[1], out var num4) && num4 >= 0 && num4 <= byte.MaxValue)
				{
					outsideWorldMapLight = num4;
					Main.NewText("Map lighting for tiles outside the normal world border set to " + outsideWorldMapLight, zazasFavoriteColor);
				}
				else
				{
					Main.NewText("Invalid light amount.", errorColor);
				}
			}
			else
			{
				Main.NewText("Current light amount: " + outsideWorldMapLight, zazasFavoriteColor);
			}
			return;
		}
		if (text == "zitem" || text == "zenithitem")
		{
			if (array.Length > 1)
			{
				if (int.TryParse(array[1], out var num5) && num5 >= -1)
				{
					zenithItem = num5;
					if (num5 != -1)
					{
						Item item = new Item();
						item.SetDefaults(num5);
						Main.NewText($"Zenith projectile's displayed item set to {item.Name} (NetID: {num5}).", zazasFavoriteColor);
						if (num5 < 0 || num5 >= 5456)
						{
							Main.NewText("The specified item ID is not a valid item. A fallback texture will instead be used by the game for the zenith projectile.", errorColor);
						}
					}
					else
					{
						Main.NewText("Zenith projectile item modifier disabled.", zazasFavoriteColor);
					}
				}
				else
				{
					Main.NewText("Invalid item ID.", errorColor);
				}
			}
			else
			{
				Item item2 = new Item();
				item2.netDefaults(zenithItem);
				Main.NewText($"Current Zenith projectile item override: {item2.Name} (NetID: {zenithItem}).", zazasFavoriteColor);
			}
			return;
		}
		if (text == "test")
		{
			InGameNotificationsTracker.AddJoinRequest(new FuckingJoinRequest("Redigit", $"{Main.rand.Next(10000000, int.MaxValue)}"));
			return;
		}
		if (text == "test2")
		{
			for (int i = (int)(Main.MouseWorld.X / 16f) - 30; i < (int)(Main.MouseWorld.X / 16f) + 30; i++)
			{
				for (int j = (int)(Main.MouseWorld.Y / 16f) - 30; j < (int)(Main.MouseWorld.Y / 16f) + 30; j++)
				{
					byte slope = (byte)Main.rand.Next(1, 5);
					Main.tile[i, j].slope(slope);
					NetMessage.SendData(17, -1, -1, null, 14, i, j, (int)slope);
				}
			}
			WorldGen.RangeFrame((int)(Main.MouseWorld.X / 16f) - 30, (int)(Main.MouseWorld.Y / 16f) - 30, (int)(Main.MouseWorld.X / 16f) + 30, (int)(Main.MouseWorld.Y / 16f) + 30);
			return;
		}
		if (text == "pnotes")
		{
			if (array.Length > 1)
			{
				if (int.TryParse(array[1], out var num6) && num6 > 0)
				{
					Player.musicNotes = num6;
					Main.NewText($"Max player music notes set to {num6}", zazasFavoriteColor);
				}
				else
				{
					Main.NewText("Invalid notes (must be greater than 0).", errorColor);
				}
			}
			else
			{
				Main.NewText($"Current player music notes: {Player.musicNotes}", zazasFavoriteColor);
			}
			return;
		}
		if (text == "test2")
		{
			if (array.Length > 1)
			{
				if (int.TryParse(array[1], out var tempInt) && tempInt >= 0)
				{
					_ = 5;
				}
				float tempFloat = 0f;
				if (array.Length > 2 && float.TryParse(array[2], out tempFloat) && tempFloat >= -1f)
				{
					_ = 1f;
				}
			}
			return;
		}
		if (text == "ibuffs")
		{
			for (int k = 0; k < Player.maxBuffs; k++)
			{
				if (!Main.debuff[localPlayer.buffType[k]])
				{
					localPlayer.buffTime[k] = int.MaxValue;
				}
			}
			Main.NewText("Set all buff durations to max (not debuffs).", zazasFavoriteColor);
			return;
		}
		if (text == "buff")
		{
			if (array.Length > 1)
			{
				int buffType = 0;
				int buffTime = 1800;
				if (!int.TryParse(array[1], out buffType) && buffType > 0 && buffType < BuffID.Count)
				{
					Main.NewText("Invalid buff ID.", errorColor);
					return;
				}
				if (array.Length > 2)
				{
					if (!int.TryParse(array[2], out buffTime) || buffTime < -1)
					{
						buffTime = 1800;
					}
					else if (buffTime == -1)
					{
						buffTime = int.MaxValue;
					}
				}
				if (localPlayer.AddBuff_ActuallyTryToAddTheBuff(buffType, buffTime))
				{
					Main.NewText($"Added buff {Lang.GetBuffName(buffType)} for {(float)buffTime / 60f} second{(((float)(buffTime / 60) == 1f) ? "" : "s")}.", zazasFavoriteColor);
					if (Main.netMode != 0)
					{
						NetMessage.SendData(50, -1, -1, null, Main.myPlayer);
					}
				}
				else
				{
					Main.NewText("Failed to add buff " + Lang.GetBuffName(buffType) + ".", zazasFavoriteColor);
				}
			}
			else
			{
				Main.NewText("Buff ID Required.", errorColor);
			}
			return;
		}
		if (!(text == "spawnmob") && !(text == "sm"))
		{
			switch (text)
			{
			case "summon":
			case "summonmob":
			case "npc":
				break;
			case "coins":
			{
				for (int i3 = 0; i3 < 5; i3++)
				{
					for (int j2 = 0; j2 < Main.npc.Length; j2++)
					{
						if (Main.npc[j2].active)
						{
							NetMessage.SendData(92, -1, -1, null, j2, 100000000f, Main.npc[j2].position.X, Main.npc[j2].position.Y);
						}
					}
				}
				Main.NewText("Gave every NPC 500 platinum coins.", zazasFavoriteColor);
				return;
			}
			case "rj":
				RejoinServer();
				return;
			case "fastjoin":
			case "speedjoin":
				fastJoin = !fastJoin;
				Main.NewText("Fast Server joining " + (fastJoin ? "en" : "dis") + "abled.", zazasFavoriteColor);
				return;
			case "bamount":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var amt))
					{
						bombAmount = amt;
						Main.NewText($"Bomb amount set to {bombAmount}.", zazasFavoriteColor);
					}
				}
				else
				{
					Main.NewText($"Current bomb amount: {bombAmount}", zazasFavoriteColor);
				}
				return;
			case "btype":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var type) && type >= 0 && type <= 8)
					{
						bombFillerType = type;
						string bombName = "";
						switch (bombFillerType)
						{
						case 0:
							bombName = "Dirt Bombs.";
							break;
						case 1:
							bombName = "Water Bombs.";
							break;
						case 2:
							bombName = "Lava Bombs.";
							break;
						case 3:
							bombName = "Honey Bombs.";
							break;
						case 4:
							bombName = "Dry Bombs.";
							break;
						case 5:
							bombName = "Random Liquid Bombs.";
							break;
						case 6:
							bombName = "Water & Lava Bombs (Creates Obsidian).";
							break;
						case 7:
							bombName = "Water & Honey Bombs (Creates Honey Blocks).";
							break;
						case 8:
							bombName = "Lava & Honey Bombs (Creates Crispy Honey Blocks).";
							break;
						}
						Main.NewText("Bomb type set to " + bombName, zazasFavoriteColor);
					}
				}
				else
				{
					string bombName2 = "";
					switch (bombFillerType)
					{
					case 0:
						bombName2 = "Dirt Bombs.";
						break;
					case 1:
						bombName2 = "Water Bombs.";
						break;
					case 2:
						bombName2 = "Lava Bombs.";
						break;
					case 3:
						bombName2 = "Honey Bombs.";
						break;
					case 4:
						bombName2 = "Dry Bombs.";
						break;
					case 5:
						bombName2 = "Random Liquid Bombs.";
						break;
					case 6:
						bombName2 = "Water & Lava Bombs (Creates Obsidian).";
						break;
					case 7:
						bombName2 = "Water & Honey Bombs (Creates Honey Blocks).";
						break;
					case 8:
						bombName2 = "Lava & Honey Bombs (Creates Crispy Honey Blocks).";
						break;
					}
					Main.NewText("Current bomb type: " + bombName2, zazasFavoriteColor);
				}
				return;
			case "bombs":
			case "bombfiller":
				bombFiller = !bombFiller;
				Main.NewText("Auto bomb filler " + (bombFiller ? "en" : "dis") + "abled.", zazasFavoriteColor);
				return;
			case "autowipe":
			case "aw":
				autoDeleteItems = !autoDeleteItems;
				Main.NewText("Automatic item deleter " + (autoDeleteItems ? "en" : "dis") + "abled.", zazasFavoriteColor);
				return;
			case "fillall":
			case "fa":
			{
				foreach (Player player4 in Main.player.Where((Player player5) => player5.active && player5.whoAmI != Main.myPlayer))
				{
					List<Item> list = player4.inventory.Where((Item item4) => item4.stack > 100 && item4.stack < item4.maxStack).ToList();
					IEntitySource itemSource = WorldGen.GetItemSource_FromTileBreak((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y);
					foreach (Item item3 in list)
					{
						player4.QuickSpawnItem(itemSource, item3.netID, item3.maxStack - item3.stack);
					}
					Main.NewText("Filled " + player4.name, zazasFavoriteColor);
				}
				return;
			}
			case "tstest":
				placeAnywhereTileSync = !placeAnywhereTileSync;
				Main.NewText("Test tile syncing " + (placeAnywhereTileSync ? "enabled." : "disabled."));
				return;
			case "controlnpc":
			case "cnpc":
			case "cn":
			case "cnf":
			case "cnm":
			case "cnmf":
				if (Main.netMode != 0)
				{
					Main.NewText("This only works offline.", errorColor);
				}
				else if (array.Length > 1)
				{
					if (short.TryParse(array[1], out var npcType) && npcType < NPCID.Count && npcType > -66)
					{
						if (controlledNpc != null)
						{
							controlledNpc.aiStyle = controlledNpc.oldAiStyle;
							if (!controlledNpc.noTileCollide)
							{
								controlledNpc.width = controlledNpc.oldWidth;
								controlledNpc.height = controlledNpc.oldHeight;
							}
							if (Main.netMode != 1)
							{
								controlledNpc.immune[Main.myPlayer] = 0;
								if (!controlledNpc.noTileCollide)
								{
									controlledNpc.Bottom = localPlayer.Bottom;
								}
								else
								{
									controlledNpc.Center = localPlayer.Center;
								}
							}
						}
						if (controlledNpcType == npcType && npcType != 0)
						{
							npcCycleRequested = true;
							if (controlledNpc != null)
							{
								lastControlledNpcIndex = controlledNpc.whoAmI;
							}
							targetPlayerIndex = Main.myPlayer;
						}
						controlledNpcType = npcType;
						controlledNpc = null;
						Main.NewText($"Controlled NPC tpe set to {NPCID.Search.GetName((int)controlledNpcType)} (ID: {controlledNpcType}).", zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid NPC type.", errorColor);
					}
				}
				else
				{
					Main.NewText($"Current controlled NPC tpe: {NPCID.Search.GetName((int)controlledNpcType)} (ID: {controlledNpcType}).", zazasFavoriteColor);
				}
				return;
			case "17":
			{
				for (int l = (int)(Main.MouseWorld.X / 16f) - 30; l < (int)(Main.MouseWorld.X / 16f) + 31; l++)
				{
					for (int m = (int)(Main.MouseWorld.Y / 16f) - 30; m < (int)(Main.MouseWorld.Y / 16f) + 31; m++)
					{
						if (WorldGen.InWorld(l, m))
						{
							Main.tile[l, m].type = 504;
							Main.tile[l, m].active(active: true);
							NetMessage.SendData(17, -1, -1, null, 1, l, m, (int)Main.tile[l, m].type);
						}
					}
				}
				WorldGen.RangeFrame((int)(Main.MouseWorld.X / 16f) - 30, (int)(Main.MouseWorld.Y / 16f) - 30, (int)(Main.MouseWorld.X / 16f) + 31, (int)(Main.MouseWorld.Y / 16f) + 31, resetFrame: true);
				return;
			}
			case "trollplrs":
				TeleportPlayers(Main.MouseWorld);
				return;
			case "regen":
			case "regenworld":
				if (WorldGen.gen)
				{
					Main.NewText("The world is already regenerating!", errorColor);
				}
				else if (Main.netMode != 0)
				{
					Main.NewText("You cannot regenerate the world while in an online game.", errorColor);
				}
				else if (array.Length <= 1)
				{
					Main.NewText("Regenerating world with same seed...", zazasFavoriteColor);
					Instance.RegenerateTheWorld("");
				}
				else
				{
					string text2 = chatText.Substring(array[0].Length + 2);
					Main.NewText("Regenerating world using seed '" + text2 + "'...", zazasFavoriteColor);
					Instance.RegenerateTheWorld(text2);
				}
				return;
			case "clothingstyle":
			case "clothing":
			case "clothstyle":
			case "cltst":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num54) && num54 < 12 && num54 >= 0)
					{
						localPlayer.skinVariant = num54;
						NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
						Main.NewText("Skin variant (Clothes and Gender) set to " + num54, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid skin variant.", errorColor);
					}
				}
				else
				{
					Main.NewText("Current skin variant: " + localPlayer.skinVariant, zazasFavoriteColor);
				}
				return;
			case "reflectall":
				reflectAllMyProjectiles = !reflectAllMyProjectiles;
				Main.NewText("All of your projectiles will " + (reflectAllMyProjectiles ? "now " : "no longer ") + "be reflected.", zazasFavoriteColor);
				return;
			case "dresser":
			case "clothesmenu":
			case "cltmen":
				forcedClothesWindow = true;
				forcedGuideCraftingMenu = false;
				forcedReforgeMenu = false;
				Main.playerInventory = false;
				localPlayer.chest = -1;
				Recipe.FindRecipes();
				localPlayer.SetTalkNPC(-1);
				Main.npcChatCornerItem = 0;
				Main.npcChatText = "";
				Main.interactedDresserTopLeftX = (int)(localPlayer.position.X / 16f);
				Main.interactedDresserTopLeftY = (int)(localPlayer.position.Y / 16f);
				if (Main.hairWindow)
				{
					Main.CancelHairWindow();
				}
				Main.OpenClothesWindow();
				return;
			case "stylist":
			case "hairmenu":
			case "hmen":
				forcedHairWindow = true;
				forcedGuideCraftingMenu = false;
				forcedReforgeMenu = false;
				Main.playerInventory = false;
				localPlayer.chest = -1;
				Recipe.FindRecipes();
				localPlayer.SetTalkNPC(-1);
				Main.npcChatCornerItem = 0;
				Main.npcChatText = "";
				if (Main.clothesWindow)
				{
					Main.CancelClothesWindow();
				}
				Main.OpenHairWindow();
				return;
			case "guide":
			case "guidemenu":
			case "craftingguide":
			case "recipeguide":
				forcedGuideCraftingMenu = true;
				forcedReforgeMenu = false;
				Main.playerInventory = true;
				Main.npcChatText = "";
				SoundEngine.PlaySound(12);
				Recipe.FindRecipes();
				Main.InGuideCraftMenu = true;
				Main.InReforgeMenu = false;
				if (Main.clothesWindow)
				{
					Main.CancelClothesWindow();
				}
				if (Main.hairWindow)
				{
					Main.CancelHairWindow();
				}
				UILinkPointNavigator.GoToDefaultPage();
				return;
			case "reforge":
			case "reforgemenu":
			case "goblin":
			case "tinkerer":
				forcedGuideCraftingMenu = false;
				forcedReforgeMenu = true;
				Main.playerInventory = true;
				Main.npcChatText = "";
				SoundEngine.PlaySound(12);
				Main.InGuideCraftMenu = false;
				Main.InReforgeMenu = true;
				if (Main.clothesWindow)
				{
					Main.CancelClothesWindow();
				}
				if (Main.hairWindow)
				{
					Main.CancelHairWindow();
				}
				UILinkPointNavigator.GoToDefaultPage();
				return;
			case "bank":
			case "piggybank":
			{
				forcedContainer = true;
				localPlayer.chest = -2;
				for (int m2 = 0; m2 < 40; m2++)
				{
					ItemSlot.SetGlow(m2, -1f, chest: true);
				}
				localPlayer.chestX = (int)(localPlayer.Center.X / 16f);
				localPlayer.chestY = (int)(localPlayer.Center.Y / 16f);
				localPlayer.SetTalkNPC(-1);
				Main.SetNPCShopIndex(0);
				Main.playerInventory = true;
				SoundEngine.PlaySound(10);
				Recipe.FindRecipes();
				return;
			}
			case "safe":
			{
				forcedContainer = true;
				localPlayer.chest = -3;
				for (int n = 0; n < 40; n++)
				{
					ItemSlot.SetGlow(n, -1f, chest: true);
				}
				localPlayer.chestX = (int)(localPlayer.Center.X / 16f);
				localPlayer.chestY = (int)(localPlayer.Center.Y / 16f);
				localPlayer.SetTalkNPC(-1);
				Main.SetNPCShopIndex(0);
				Main.playerInventory = true;
				SoundEngine.PlaySound(10);
				Recipe.FindRecipes();
				return;
			}
			case "forge":
			case "defenderforge":
			case "defforge":
			{
				forcedContainer = true;
				localPlayer.chest = -4;
				for (int num38 = 0; num38 < 40; num38++)
				{
					ItemSlot.SetGlow(num38, -1f, chest: true);
				}
				localPlayer.chestX = (int)(localPlayer.Center.X / 16f);
				localPlayer.chestY = (int)(localPlayer.Center.Y / 16f);
				localPlayer.SetTalkNPC(-1);
				Main.SetNPCShopIndex(0);
				Main.playerInventory = true;
				SoundEngine.PlaySound(10);
				Recipe.FindRecipes();
				return;
			}
			case "void":
			case "voidvault":
			case "voidbag":
			{
				forcedContainer = true;
				localPlayer.chest = -5;
				for (int num126 = 0; num126 < 40; num126++)
				{
					ItemSlot.SetGlow(num126, -1f, chest: true);
				}
				localPlayer.chestX = (int)(localPlayer.Center.X / 16f);
				localPlayer.chestY = (int)(localPlayer.Center.Y / 16f);
				localPlayer.SetTalkNPC(-1);
				Main.SetNPCShopIndex(0);
				Main.playerInventory = true;
				SoundEngine.PlaySound(10);
				Recipe.FindRecipes();
				return;
			}
			case "infsn":
				unlimitedRopeSnakes = !unlimitedRopeSnakes;
				Main.NewText("Unlimited Snake Ropes " + (unlimitedRopeSnakes ? "en" : "dis") + "abled.", zazasFavoriteColor);
				return;
			case "infice":
				unlimitedIce = !unlimitedIce;
				Main.NewText("Unlimited Magical Ice " + (unlimitedIce ? "en" : "dis") + "abled.", zazasFavoriteColor);
				return;
			case "icerod":
				if (localPlayer.HeldItem.type == 496)
				{
					bool reverse = localPlayer.HeldItem.createTile == 127;
					localPlayer.HeldItem.createTile = (reverse ? (-1) : 127);
					localPlayer.HeldItem.shoot = (reverse ? 80 : 0);
					localPlayer.HeldItem.mana = (reverse ? 6 : 0);
					if (localPlayer.selectedItem == 58)
					{
						Main.mouseItem = localPlayer.HeldItem.Clone();
					}
					Main.NewText("Your Ice Rod " + ((!reverse) ? "now " : "no longer ") + "directly places Magical Ice.", zazasFavoriteColor);
				}
				else
				{
					Main.NewText("You need to hold an Ice Rod to use this.", errorColor);
				}
				return;
			case "fc":
			case "freecraft":
				freeCraft = !freeCraft;
				Main.NewText("Free Crafting " + (freeCraft ? "en" : "dis") + "abled.", zazasFavoriteColor);
				return;
			case "snh":
			case "snakeh":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num60) && num60 < Main.maxTilesY && num60 >= 0)
					{
						if (num60 > 0)
						{
							customRopeSnakeHeight = num60;
							ropeSnakeHeightOverride = true;
							Main.NewText("Custom Snake Rope height set to " + num60, zazasFavoriteColor);
						}
						else
						{
							customRopeSnakeHeight = num60;
							ropeSnakeHeightOverride = false;
							Main.NewText("Custom Snake Rope height disabled.", zazasFavoriteColor);
						}
					}
					else
					{
						Main.NewText("Invalid Snake Rope height.", errorColor);
					}
				}
				else
				{
					Main.NewText("Current Snake Rope height: " + customRopeSnakeHeight, zazasFavoriteColor);
				}
				return;
			case "dd2film":
			{
				Film film = new DD2Film();
				CinematicManager.Instance.PlayFilm(film);
				return;
			}
			case "dstfilm":
			{
				Film film2 = new DSTFilm();
				CinematicManager.Instance.PlayFilm(film2);
				return;
			}
			case "packetmax":
			case "packetlimit":
			case "packm":
			case "pl":
			case "tilepacketmax":
			case "tilepacketlimit":
			case "tpackm":
			case "tpl":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num40))
					{
						tilePacketRate = num40;
						Main.NewText("Tile & Wall Packet Rate set to " + num40, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid packet rate.", errorColor);
					}
				}
				else
				{
					Main.NewText("No packet rate specified.", errorColor);
				}
				return;
			case "erasepacketmax":
			case "erasepacketlimit":
			case "epackm":
			case "epl":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num37))
					{
						erasePacketRate = num37;
						Main.NewText($"Erase Packet Rate set to {num37}", zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid packet rate.", errorColor);
					}
				}
				else
				{
					Main.NewText("No packet rate specified.", errorColor);
				}
				return;
			case "liquidpacketmax":
			case "liquidpacketlimit":
			case "lpackm":
			case "lpl":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num52))
					{
						liquidPacketRate = num52;
						Main.NewText($"Liquid Packet Rate set to {num52}", zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid packet rate.", errorColor);
					}
				}
				else
				{
					Main.NewText("No packet rate specified.", errorColor);
				}
				return;
			case "paintpacketmax":
			case "paintpacketlimit":
			case "ppackm":
			case "ppl":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num24))
					{
						paintPacketRate = num24;
						Main.NewText($"Paint Packet Rate set to {num24}", zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid packet rate.", errorColor);
					}
				}
				else
				{
					Main.NewText("No packet rate specified.", errorColor);
				}
				return;
			case "jit":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num122))
					{
						jitterAmount = num122;
						if (jitterAmount > 0)
						{
							jitterPos = true;
							Main.NewText("Jitter position enabled & set to " + num122, zazasFavoriteColor);
						}
						else
						{
							jitterPos = false;
							Main.NewText("Jitter position disabled.", zazasFavoriteColor);
						}
					}
					else
					{
						Main.NewText("Invalid jitter amount.", errorColor);
					}
				}
				else
				{
					jitterPos = false;
					Main.NewText("Jitter position disabled.", zazasFavoriteColor);
				}
				return;
			case "arrowgrav":
				ModTogglerV2(CheatManager.GetCheat<Cheats.NoArrowGravityCheat>());
				return;
			case "terradisc":
				ModTogglerV2(CheatManager.GetCheat<Cheats.TerraDiscsCheat>());
				return;
			case "relockchests":
				Instance.LockOrUnlockAllChests();
				return;
			case "unlockchests":
				Instance.LockOrUnlockAllChests();
				return;
			case "worlddownload":
			case "wlddown":
				if (Main.netMode == 1)
				{
					WorldDownload();
				}
				else
				{
					Main.NewText("You can only do this in an online game.", errorColor);
				}
				return;
			case "savemap":
			case "exportmap":
				SaveMapImage();
				return;
			case "tinvis":
				invisibility = !invisibility;
				Main.NewText("You are " + (invisibility ? "now " : "no longer ") + "invisible.", zazasFavoriteColor);
				if (invisibility && onTshockServer)
				{
					Main.NewText("Doesn't work on TShock servers.", infoColor);
				}
				NetMessage.SendData(16, -1, -1, null, Main.myPlayer);
				return;
			case "fuckchest":
			case "extendchestname":
				fuckChestName = !fuckChestName;
				Main.NewText("Extended chest names " + (fuckChestName ? "en" : "dis") + "abled.", zazasFavoriteColor);
				return;
			case "lchest":
				localChests = !localChests;
				Main.NewText("Chest contents are " + ((!localChests) ? "now " : "no longer ") + "requested from the server.", zazasFavoriteColor);
				return;
			case "fillchest":
			case "fillc":
				if (localPlayer.chest >= 0)
				{
					if (array.Length > 1)
					{
						if (int.TryParse(array[1], out var itemId) && itemId < 5456 && itemId >= -1)
						{
							for (int i2 = 0; i2 < Main.chest[localPlayer.chest].item.Length; i2++)
							{
								Main.chest[localPlayer.chest].item[i2].SetDefaults((itemId == -1) ? Main.rand.Next(1, 5456) : itemId);
								Main.chest[localPlayer.chest].item[i2].stack = Main.chest[localPlayer.chest].item[i2].maxStack;
								if (Main.netMode == 1)
								{
									NetMessage.SendData(32, -1, -1, null, localPlayer.chest, i2);
								}
							}
							Main.NewText("Filled chest with " + ((itemId == -1) ? "random items" : Main.chest[localPlayer.chest].item[0].Name) + ".", zazasFavoriteColor);
						}
						else
						{
							Main.NewText("Invalid Item ID.", errorColor);
						}
					}
					else
					{
						Main.NewText("No Item ID provided.", zazasFavoriteColor);
					}
				}
				else
				{
					Main.NewText("You need to be in a chest for this.", zazasFavoriteColor);
				}
				return;
			case "nohorror":
				suppressHorrified = !suppressHorrified;
				Main.NewText("You are " + ((!suppressHorrified) ? "now " : "no longer ") + "affected by the Horrified debuff's effects.", zazasFavoriteColor);
				return;
			case "blockbuff":
			case "bbuff":
			{
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num68) && num68 > -1 && num68 < BuffID.Count)
					{
						buffForceImmune[num68] = !buffForceImmune[num68];
						Main.NewText("You are " + (buffForceImmune[num68] ? "now " : "no longer ") + "immune to " + Lang.GetBuffName(num68) + " (ID " + num68 + ")");
					}
					else
					{
						Main.NewText("Invalid Buff ID specified.", errorColor);
					}
					return;
				}
				Main.NewText("Current buffs you are forced immune to:");
				for (int i4 = 0; i4 < BuffID.Count; i4++)
				{
					if (buffForceImmune[i4])
					{
						Main.NewText(Lang.GetBuffName(i4) + " (ID " + i4 + ")");
					}
				}
				return;
			}
			case "plrdif":
			case "playerdifficulty":
			case "chrdif":
			case "characterdifficulty":
			{
				if (array.Length <= 1 || string.IsNullOrWhiteSpace(array[1]))
				{
					PrintCurrentPlayerDifficulty();
					return;
				}
				string input = array[1].ToLower();
				bool invalidDiff = false;
				string difficultyDisplayName = "";
				if (new Dictionary<string, (int, string)>
				{
					{
						"0",
						(0, "Softcore")
					},
					{
						"1",
						(1, "Mediumcore")
					},
					{
						"2",
						(2, "Hardcore")
					},
					{
						"3",
						(3, "Creative")
					},
					{
						"sc",
						(0, "Softcore")
					},
					{
						"mc",
						(1, "Mediumcore")
					},
					{
						"hc",
						(2, "Hardcore")
					},
					{
						"cr",
						(3, "Creative")
					},
					{
						"classic",
						(0, "Softcore")
					},
					{
						"medium",
						(1, "Mediumcore")
					},
					{
						"hard",
						(2, "Hardcore")
					},
					{
						"journey",
						(3, "Creative")
					},
					{
						"mediumcore",
						(1, "Mediumcore")
					},
					{
						"hardcore",
						(2, "Hardcore")
					},
					{
						"creative",
						(3, "Creative")
					}
				}.TryGetValue(input, out var diff))
				{
					localPlayer.difficulty = (byte)diff.Item1;
					difficultyDisplayName = Language.GetTextValue("UI." + diff.Item2);
				}
				else
				{
					invalidDiff = true;
				}
				if (invalidDiff)
				{
					Main.NewText("Invalid player difficulty.", errorColor);
					return;
				}
				if (Main.netMode == 1)
				{
					NetMessage.TrySendData(4, -1, -1, null, Main.myPlayer);
				}
				Main.NewText($"Player difficulty changed to: {difficultyDisplayName} (ID: {localPlayer.difficulty})", zazasFavoriteColor);
				return;
			}
			case "wlddif":
			case "worlddifficulty":
			{
				if (array.Length <= 1 || string.IsNullOrWhiteSpace(array[1]))
				{
					string currentWorldName = ModsNewUIListItem.GetWorldDifficultyName(ignoreLegendary: true);
					int currentWorldId = Main.GameMode;
					Main.NewText($"Current world difficulty: {currentWorldName} (ID: {currentWorldId})", zazasFavoriteColor);
					return;
				}
				string worldInput = array[1].ToLowerInvariant();
				bool invalidWorldDiff = false;
				string WorldDiffDisplayName = "";
				if (new Dictionary<string, (int, string)>
				{
					{
						"0",
						(0, "Classic")
					},
					{
						"c",
						(0, "Classic")
					},
					{
						"classic",
						(0, "Classic")
					},
					{
						"normal",
						(0, "Classic")
					},
					{
						"1",
						(1, "Expert")
					},
					{
						"e",
						(1, "Expert")
					},
					{
						"expert",
						(1, "Expert")
					},
					{
						"2",
						(2, "Master")
					},
					{
						"m",
						(2, "Master")
					},
					{
						"master",
						(2, "Master")
					},
					{
						"legendary",
						(2, "Master")
					},
					{
						"legend",
						(2, "Master")
					},
					{
						"3",
						(3, "Journey")
					},
					{
						"j",
						(3, "Journey")
					},
					{
						"cr",
						(3, "Journey")
					},
					{
						"journey",
						(3, "Journey")
					},
					{
						"creative",
						(3, "Journey")
					}
				}.TryGetValue(worldInput, out var worldDiff))
				{
					(Main.GameMode, WorldDiffDisplayName) = worldDiff;
				}
				else
				{
					invalidWorldDiff = true;
				}
				if (invalidWorldDiff)
				{
					Main.NewText("Invalid world difficulty.", errorColor);
					return;
				}
				Main.NewText($"World difficulty changed to: {WorldDiffDisplayName} (ID: {Main.GameMode})", zazasFavoriteColor);
				return;
			}
			case "luck":
				Main.NewText("Overall Luck: " + localPlayer.luck, zazasFavoriteColor);
				Main.NewText("Coin Luck: " + localPlayer.coinLuck, zazasFavoriteColor);
				Main.NewText("Torch Luck: " + localPlayer.torchLuck, zazasFavoriteColor);
				Main.NewText("Luck Potion: " + localPlayer.luckPotion, zazasFavoriteColor);
				Main.NewText("Ladybug Luck Time: " + localPlayer.ladyBugLuckTimeLeft, zazasFavoriteColor);
				Main.NewText("Garden Gnome Nearby: " + localPlayer.HasGardenGnomeNearby, zazasFavoriteColor);
				return;
			case "suicide":
			case "killself":
			case "killme":
			case "kms":
			case "die":
			{
				if (localPlayer.dead)
				{
					Main.NewText("You are already dead.", errorColor);
					return;
				}
				string reasonType = ((array.Length > 1) ? array[1].ToLower() : "");
				int param = ((array.Length > 2 && int.TryParse(array[2], out var p)) ? p : 0);
				int param2 = ((array.Length > 3 && int.TryParse(array[3], out var p2)) ? p2 : (-1));
				if (param2 >= Main.player.Length)
				{
					param2 = Main.player.Length - 1;
				}
				if (new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
				{
					{
						"p",
						delegate
						{
							localPlayer.KillMe(PlayerDeathReason.ByPlayer(param), 10.0, 0, pvp: true, forced: true);
						}
					},
					{
						"plr",
						delegate
						{
							localPlayer.KillMe(PlayerDeathReason.ByPlayer(param), 10.0, 0, pvp: true, forced: true);
						}
					},
					{
						"player",
						delegate
						{
							localPlayer.KillMe(PlayerDeathReason.ByPlayer(param), 10.0, 0, pvp: true, forced: true);
						}
					},
					{
						"n",
						delegate
						{
							localPlayer.KillMe(PlayerDeathReason.ByNPC(param), 10.0, 0, pvp: true, forced: true);
						}
					},
					{
						"npc",
						delegate
						{
							localPlayer.KillMe(PlayerDeathReason.ByNPC(param), 10.0, 0, pvp: true, forced: true);
						}
					},
					{
						"pr",
						delegate
						{
							localPlayer.KillMe(PlayerDeathReason.ByProjectile(param2, 9999, param), 10.0, 0, pvp: true, forced: true);
						}
					},
					{
						"proj",
						delegate
						{
							localPlayer.KillMe(PlayerDeathReason.ByProjectile(param2, 9999, param), 10.0, 0, pvp: true, forced: true);
						}
					},
					{
						"projectile",
						delegate
						{
							localPlayer.KillMe(PlayerDeathReason.ByProjectile(param2, 9999, param), 10.0, 0, pvp: true, forced: true);
						}
					},
					{
						"o",
						delegate
						{
							localPlayer.KillMe(PlayerDeathReason.ByOther(param), 10.0, 0, pvp: true, forced: true);
						}
					},
					{
						"other",
						delegate
						{
							localPlayer.KillMe(PlayerDeathReason.ByOther(param), 10.0, 0, pvp: true, forced: true);
						}
					},
					{
						"e",
						delegate
						{
							localPlayer.KillMe(PlayerDeathReason.LegacyEmpty(), 10.0, 0, pvp: true, forced: true);
						}
					},
					{
						"empty",
						delegate
						{
							localPlayer.KillMe(PlayerDeathReason.LegacyEmpty(), 10.0, 0, pvp: true, forced: true);
						}
					}
				}.TryGetValue(reasonType, out var killAction))
				{
					killAction();
				}
				else
				{
					localPlayer.KillMe(PlayerDeathReason.LegacyDefault(), 10.0, 0, pvp: true, forced: true);
				}
				Main.NewText("Committed suicide.", zazasFavoriteColor);
				return;
			}
			case "crystalkillaura":
			case "crystalaura":
			case "cka":
			case "ca":
				crystalKillAura = !crystalKillAura;
				Main.NewText("Enemies will " + (crystalKillAura ? "now be " : "no longer be ") + "automatically nuked when near the Eternia Crystal.", zazasFavoriteColor);
				return;
			case "shimmerall":
			case "shma":
				if (array.Length > 1)
				{
					if (array[1] == "town" || array[1] == "townnpc" || array[1] == "tn")
					{
						for (int num57 = 0; num57 < Main.npc.Length; num57++)
						{
							NPC npc = Main.npc[num57];
							if (NPCID.Sets.ShimmerTownTransform[npc.type] && npc.townNpcVariationIndex == 0)
							{
								Main.npc[num57].AddBuff(353, 100);
							}
						}
						Main.NewText("Shimmered all Town NPCs into their shimmer variants!", zazasFavoriteColor);
					}
					else if (array[1] == "townalt" || array[1] == "townnpcalt" || array[1] == "tnalt")
					{
						for (int num58 = 0; num58 < Main.npc.Length; num58++)
						{
							NPC npc2 = Main.npc[num58];
							if (NPCID.Sets.ShimmerTownTransform[npc2.type] && npc2.townNpcVariationIndex == 1)
							{
								Main.npc[num58].AddBuff(353, 100);
							}
						}
						Main.NewText("Shimmered all Town NPCs to normal!", zazasFavoriteColor);
					}
					else if (array[1] == "enemy" || array[1] == "unfriendly" || array[1] == "en" || array[1] == "uf")
					{
						for (int num59 = 0; num59 < Main.npc.Length; num59++)
						{
							if (!Main.npc[num59].friendly && !Main.npc[num59].CountsAsACritter)
							{
								Main.npc[num59].AddBuff(353, 100);
							}
						}
						Main.NewText("Shimmered all enemies!", zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid NPC type specified.", errorColor);
					}
				}
				else
				{
					Main.NewText("No NPC type specified.", errorColor);
				}
				return;
			case "infammo":
			case "infiniteammo":
			case "infam":
				infiniteAmmo = !infiniteAmmo;
				Main.NewText(infiniteAmmo ? "Infinite ammo enabled." : "Infinite ammo disabled.", zazasFavoriteColor);
				return;
			case "blocknpcs":
			case "disablenpcs":
			case "bannpcs":
			case "bnpc":
				blockTownNPCs = !blockTownNPCs;
				Main.NewText(blockTownNPCs ? "All town NPCs are now blocked." : "Town NPCs are no longer blocked.", zazasFavoriteColor);
				return;
			case "changemusic":
			case "swapmusic":
			case "sm":
				Main.swapMusic = !Main.swapMusic;
				if (Main.drunkWorld && !Main.remixWorld)
				{
					Main.NewText(Main.swapMusic ? "Music swapped to normal!" : "Music returned to Otherworldly.", zazasFavoriteColor);
				}
				else
				{
					Main.NewText(Main.swapMusic ? "Music swapped to Otherworldy music!" : "Music returned to normal.", zazasFavoriteColor);
				}
				return;
			case "ghost":
			case "ghostmode":
			case "gh":
			case "g":
				if (localPlayer.ghost && localPlayer.dead)
				{
					Main.NewText("Respawned and returned to normal!", zazasFavoriteColor);
					Main.NewText("You were dead when unghosting, so you have been respawned.", infoColor);
					Main.NewText("This may cause issues or desyncs on servers, and could be picked up on. Use with caution.", infoColor);
					localPlayer.dead = false;
					localPlayer.ghost = false;
					localPlayer.deadForGood = false;
					localPlayer.Spawn(PlayerSpawnContext.ReviveFromDeath);
				}
				else
				{
					if (localPlayer.dead)
					{
						localPlayer.dead = false;
						localPlayer.Spawn(PlayerSpawnContext.ReviveFromDeath);
					}
					localPlayer.ghost = !localPlayer.ghost;
					Main.NewText(localPlayer.ghost ? "Turned into ghost." : "Reverted to normal", zazasFavoriteColor);
				}
				return;
			case "lagothers":
			case "lagotherplayers":
			case "lo":
				LagOutServerPlayersUnlessTheyHaveCoolClient();
				return;
			case "rotw":
			case "rotateworld":
				if (WorldGen.gen)
				{
					Main.NewText("You can't do that during world gen!", errorColor);
				}
				else if (Main.netMode == 0)
				{
					RotateTheWorld();
				}
				else
				{
					Main.NewText("You cannot rotate the world while in an online game.", errorColor);
				}
				return;
			case "god":
			case "godmode":
				godMode = !godMode;
				Main.NewText("GodMode " + (godMode ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "shgod":
			case "shimmergodmode":
				shimmerGodMode = !shimmerGodMode;
				Main.NewText("Shimmer Godmode " + (shimmerGodMode ? "En" : "Dis") + "abled.", zazasFavoriteColor);
				if (!shimmerGodMode)
				{
					localPlayer.ClearBuff(353);
				}
				return;
			case "shp":
			case "shimmerparticles":
				shimmerWalkParticles = !shimmerWalkParticles;
				Main.NewText("Shimmer Walking Particles " + (shimmerWalkParticles ? "En" : "Dis") + "abled.", zazasFavoriteColor);
				return;
			case "ignoreliquids":
			case "igl":
				ignoreLiquids = !ignoreLiquids;
				Main.NewText("Ignore Liquids " + (ignoreLiquids ? "En" : "Dis") + "abled.", zazasFavoriteColor);
				return;
			case "ignoredmgblocks":
			case "igd":
				ignoreDamageBlocks = !ignoreDamageBlocks;
				Main.NewText("Ignore Damage Blocks " + (ignoreDamageBlocks ? "En" : "Dis") + "abled.", zazasFavoriteColor);
				return;
			case "rgboutline":
			case "rgbout":
				rgbPlayerOutline = !rgbPlayerOutline;
				Main.NewText("Rainbow outline for self " + (rgbPlayerOutline ? "En" : "Dis") + "abled.", zazasFavoriteColor);
				return;
			case "rgbnpcicon":
			case "rgbnpc":
				rgbNpcIcons = !rgbNpcIcons;
				Main.NewText("Rainbow NPC icons " + (rgbNpcIcons ? "En" : "Dis") + "abled.", zazasFavoriteColor);
				return;
			case "rgbbossicon":
			case "rgbboss":
				rgbBossIcons = !rgbBossIcons;
				Main.NewText("Rainbow boss icons " + (rgbBossIcons ? "En" : "Dis") + "abled.", zazasFavoriteColor);
				return;
			case "noclip":
			case "nc":
				noclip = !noclip;
				Main.NewText("Noclip " + (noclip ? "En" : "Dis") + "abled", zazasFavoriteColor);
				if (Main.netMode != 0)
				{
					NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
					NetMessage.SendData(50, -1, -1, null, Main.myPlayer);
				}
				return;
			case "tpm":
			case "mousetp":
			case "gotomouse":
				gotoMouse = !gotoMouse;
				Main.NewText("Mouse Teleportation " + (gotoMouse ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "journeymenu":
			case "jm":
				journeyMenu = !journeyMenu;
				Main.NewText("Journey Menu " + (journeyMenu ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "fb":
			case "fullbright":
				if (array.Length > 1)
				{
					if (array[1] == "full")
					{
						fullBrightPercent = 1f;
						Main.NewText("Fullbright, Full Brightness Mode " + (fullBright ? "En" : "Dis") + "abled", zazasFavoriteColor);
					}
				}
				else if (fullBrightPercent == 1f)
				{
					fullBrightPercent = 0.5f;
					Main.NewText("Fullbright, Full Brightness Mode " + (fullBright ? "En" : "Dis") + "abled", zazasFavoriteColor);
				}
				else
				{
					fullBrightPercent = 0.5f;
					Main.NewText("Fullbright, Half Brightness Mode " + (fullBright ? "En" : "Dis") + "abled", zazasFavoriteColor);
				}
				return;
			case "spawn":
				localPlayer.position = new Vector2((float)Main.spawnTileX * 16f - (float)(Main.LocalPlayer.width / 2) + 8f, (float)Main.spawnTileY * 16f - (float)localPlayer.height);
				localPlayer.velocity = Vector2.Zero;
				localPlayer.fallStart = (int)localPlayer.position.Y / 16;
				localPlayer.oldPosition = localPlayer.position;
				Instance.OldCenterMod = localPlayer.Center;
				Main.LocalPlayer.noFallDmg = true;
				if (Main.netMode != 0)
				{
					NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
				}
				Main.NewText("Teleported to spawn.", zazasFavoriteColor);
				return;
			case "dungeon":
				if (Main.netMode != 1 || DoWeHaveDungeonPosition)
				{
					localPlayer.position = new Vector2((float)Main.dungeonX * 16f - (float)(localPlayer.width / 2) + 8f, (float)Main.dungeonY * 16f - (float)localPlayer.height);
					localPlayer.velocity = Vector2.Zero;
					localPlayer.fallStart = (int)localPlayer.position.Y / 16;
					localPlayer.oldPosition = localPlayer.position;
					Instance.OldCenterMod = localPlayer.Center;
					localPlayer.noFallDmg = true;
					if (Main.netMode != 0)
					{
						NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
					}
					Main.NewText("Teleported to the dungeon.", zazasFavoriteColor);
				}
				else
				{
					Main.NewText("The dungeon position is unavailable.", errorColor);
				}
				return;
			case "track":
			case "trackplayers":
			case "plrtracker":
			case "playertracker":
				playerTracker = !playerTracker;
				Main.NewText("Player Tracker " + (playerTracker ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "maptp":
			case "mapteleport":
				mapTeleportation = !mapTeleportation;
				Main.NewText("Map Teleportation " + (mapTeleportation ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "ut":
			case "usetime":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num125))
					{
						localPlayer.HeldItem.useTime = num125;
						Main.NewText("Usetime set to " + num125, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Ammount ", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required ", errorColor);
				}
				return;
			case "shtst":
			case "shootstyle":
			{
				ItemShootBehavior shootStyle = (ItemShootBehavior)localPlayer.HeldItem.itemShootBehavior;
				if (array.Length > 1)
				{
					if (byte.TryParse(array[1], out var num121) && num121 <= 106)
					{
						localPlayer.HeldItem.itemShootBehavior = num121;
						shootStyle = (ItemShootBehavior)num121;
						Main.NewText("Shoot Style set to " + shootStyle.GetType().GetEnumName(shootStyle) + " (ID: " + num121 + ")", zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Shoot Style", errorColor);
					}
				}
				else
				{
					Main.NewText("Current Shoot Style: " + shootStyle.GetType().GetEnumName(shootStyle) + " (ID: " + localPlayer.HeldItem.itemShootBehavior + ")", zazasFavoriteColor);
				}
				return;
			}
			case "rngam":
			case "rngamount":
				if (array.Length > 1)
				{
					if (ushort.TryParse(array[1], out var rngAm) && rngAm <= 1000)
					{
						randomProjsToShoot = rngAm;
						Main.NewText("Random projectile amount for RNG shoot styles set to " + rngAm, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid amount.", errorColor);
					}
				}
				else
				{
					Main.NewText("Current random projectile amount: " + randomProjsToShoot, zazasFavoriteColor);
				}
				return;
			case "at":
			case "animationtime":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num66))
					{
						localPlayer.HeldItem.useAnimation = num66;
						Main.NewText("Animation Time set to " + num66, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Ammount ", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required ", errorColor);
				}
				return;
			case "crtl":
			case "createtile":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num61) && num61 > -2 && num61 < TileID.Count)
					{
						localPlayer.HeldItem.createTile = num61;
						Main.NewText($"CreateTile set to {((num61 > -1) ? TileID.Search.GetName(num61) : "None")} (ID: {num61})", zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Tile ID.", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required.", errorColor);
				}
				return;
			case "crwl":
			case "createwall":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num55) && num55 > -1 && num55 < WallID.Count)
					{
						localPlayer.HeldItem.createWall = num55;
						Main.NewText("CreateWall set to " + num55, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Wall ID.", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required.", errorColor);
				}
				return;
			case "plst":
			case "placeStyle":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num53))
					{
						localPlayer.HeldItem.placeStyle = num53;
						Main.NewText("PlaceStyle set to " + num53, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Ammount ", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required ", errorColor);
				}
				return;
			case "shootperuse":
			case "shootamount":
			case "spu":
			case "sha":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num41) && num41 > 0 && num41 <= 1000)
					{
						localPlayer.HeldItem.timesToShoot = (ushort)num41;
						Main.NewText("Held item will now shoot " + num41 + " times.", zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Ammount.", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required.", errorColor);
				}
				return;
			case "scale":
			case "scl":
			case "itemsize":
			case "size":
				if (array.Length > 1)
				{
					if (float.TryParse(array[1], out var num39) && num39 > 0f && num39 < float.MaxValue)
					{
						localPlayer.HeldItem.scale = num39;
						Main.NewText("Scale of held item set to " + num39, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Ammount ", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required ", errorColor);
				}
				return;
			case "proj":
			case "projectile":
			case "shoot":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num33) && num33 > -1 && num33 < ProjectileID.Count)
					{
						localPlayer.HeldItem.shoot = num33;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Projectile set to ");
						defaultInterpolatedStringHandler.AppendFormatted((num33 > 0) ? ((object)Lang.GetProjectileName(num33)) : ((object)"None"));
						defaultInterpolatedStringHandler.AppendLiteral(" (ID: ");
						defaultInterpolatedStringHandler.AppendFormatted(num33);
						defaultInterpolatedStringHandler.AppendLiteral(")");
						Main.NewText(defaultInterpolatedStringHandler.ToStringAndClear(), zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Projectile ID.", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required.", errorColor);
				}
				return;
			case "projspeed":
			case "projectilespeed":
			case "shootspeed":
				if (array.Length > 1)
				{
					if (float.TryParse(array[1], out var num25))
					{
						localPlayer.HeldItem.shootSpeed = num25;
						Main.NewText("Projectile speed set to " + num25, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Projectile Speed.", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required.", errorColor);
				}
				return;
			case "damage":
			case "dmg":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num23))
					{
						localPlayer.HeldItem.damage = num23;
						Main.NewText("Damage set to " + num23, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Damage.", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required.", errorColor);
				}
				return;
			case "butcher":
			case "killall":
			{
				int num22 = 0;
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out num22))
					{
						ButcherMobsAndNpcs(num22, 1);
					}
					else
					{
						Main.NewText("Invalid Ammount ", errorColor);
					}
				}
				else
				{
					ButcherMobsAndNpcs(num22, 1);
				}
				return;
			}
			case "setmaxhp":
			case "sethp":
			case "maxhp":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num124))
					{
						localPlayer.statLifeMax = num124;
						NetMessage.SendData(16, -1, -1, null, Main.myPlayer);
						Main.NewText($"Set your max HP to {num124}", zazasFavoriteColor);
						if (num124 < 100)
						{
							Main.NewText("Your HP is too low. The minimum max HP that can be synced on servers is 100, so other players will only see 100 HP.", infoColor);
						}
						else if (num124 > 32767)
						{
							Main.NewText($"Your HP is too high. The maximum HP that can be synced on servers is {short.MaxValue}. An incorrect value will end up being synced to others.", infoColor);
						}
					}
					else
					{
						Main.NewText("Invalid Ammount.", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required.", errorColor);
				}
				return;
			case "setmaxmp":
			case "setmp":
			case "setmaxmana":
			case "setmana":
			case "maxmp":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num123))
					{
						localPlayer.statManaMax = num123;
						NetMessage.SendData(42, -1, -1, null, Main.myPlayer);
						Main.NewText($"Set your max mana to {num123}", zazasFavoriteColor);
						if (num123 > 32767)
						{
							Main.NewText($"Your mana is too high. The maximum mana that can be synced on servers is {short.MaxValue}. An incorrect value will end up being synced to others.", infoColor);
						}
					}
					else
					{
						Main.NewText("Invalid Ammount.", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required.", errorColor);
				}
				return;
			case "heal":
			{
				bool hideEffects = false;
				if (array.Length > 1 && bool.TryParse(array[1], out var shouldHide))
				{
					hideEffects = shouldHide;
				}
				int hpAm = localPlayer.statLifeMax2 - localPlayer.statLife;
				int mpAm = localPlayer.statManaMax2 - localPlayer.statMana;
				if (hpAm > 0)
				{
					localPlayer.statLife = localPlayer.statLifeMax2;
					NetMessage.SendData(16, -1, -1, null, Main.myPlayer);
					if (!hideEffects)
					{
						localPlayer.HealEffect(hpAm);
					}
				}
				if (mpAm > 0)
				{
					localPlayer.statMana = localPlayer.statManaMax2;
					NetMessage.SendData(42, -1, -1, null, Main.myPlayer);
					if (!hideEffects)
					{
						localPlayer.ManaEffect(mpAm);
					}
				}
				if (hpAm > 0 || mpAm > 0)
				{
					Main.NewText($"You've healed yourself for {hpAm} HP and {mpAm} mana.", zazasFavoriteColor);
				}
				else
				{
					Main.NewText(FunnyQuirkyIndieGameLevelWriting(), infoColor);
				}
				return;
			}
			case "hlf":
			case "heallife":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num65))
					{
						localPlayer.HeldItem.healLife = num65;
						Main.NewText("HealLife set to " + num65, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Ammount.", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required.", errorColor);
				}
				return;
			case "hlm":
			case "healmana":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num63))
					{
						localPlayer.HeldItem.healMana = num63;
						Main.NewText("HealMana set to " + num63, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Ammount.", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required.", errorColor);
				}
				return;
			case "kb":
			case "knockback":
				if (array.Length > 1)
				{
					if (float.TryParse(array[1], out var num62) && num62 >= 0f)
					{
						localPlayer.HeldItem.knockBack = num62;
						Main.NewText("Knockback set to " + num62, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Knockback.", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required.", errorColor);
				}
				return;
			case "reud":
			case "reusedelay":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num56) && num56 >= 0)
					{
						localPlayer.HeldItem.reuseDelay = num56;
						Main.NewText("Reuse delay set to " + num56, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Ammount ", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Required ", errorColor);
				}
				return;
			case "extraproj":
				useExtraProjHitboxes = !useExtraProjHitboxes;
				Main.NewText("Rendering extra projectile hitboxes " + (useExtraProjHitboxes ? "en" : "dis") + "abled", zazasFavoriteColor);
				return;
			case "wirekites":
				renderOtherWireKites = !renderOtherWireKites;
				Main.NewText("Rendering other's wire kite projectiles " + (renderOtherWireKites ? "en" : "dis") + "abled", zazasFavoriteColor);
				return;
			case "winds":
				unlimitedWinds = !unlimitedWinds;
				Main.NewText("Unlimited Weather Pain projectiles " + (unlimitedWinds ? "en" : "dis") + "abled", zazasFavoriteColor);
				return;
			case "ar":
			case "autoreuse":
				localPlayer.HeldItem.autoReuse = !localPlayer.HeldItem.autoReuse;
				Main.NewText("Auto Reuse for current item " + (localPlayer.HeldItem.autoReuse ? "en" : "dis") + "abled", zazasFavoriteColor);
				return;
			case "arc":
			case "autorejoin":
				autoRejoin = !autoRejoin;
				Main.NewText("Auto-reconnect on kick/connection lost " + (autoRejoin ? "en" : "dis") + "abled", zazasFavoriteColor);
				return;
			case "seu":
			case "shootseveryuse":
				localPlayer.HeldItem.shootsEveryUse = !localPlayer.HeldItem.shootsEveryUse;
				Main.NewText("Shoots Every Use for current item " + (localPlayer.HeldItem.shootsEveryUse ? "en" : "dis") + "abled", zazasFavoriteColor);
				return;
			case "ab":
			case "autobutcher":
			case "autokill":
				autoButcherDamage = 9999;
				autoButcher = !autoButcher;
				if (array.Length > 1)
				{
					if (short.TryParse(array[1], out autoButcherDamage))
					{
						autoButcher = true;
					}
					else
					{
						Main.NewText("Invalid Value ", errorColor);
						autoButcherDamage = 9999;
					}
				}
				Main.NewText("Auto Butcher " + (autoButcher ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "revmap":
			case "revealmap":
				if (!revealMapCooldown)
				{
					RevealTheMap();
				}
				else
				{
					stopMapReveal = true;
				}
				return;
			case "revmapbed":
			case "revealmapbed":
				if (!revealMapCooldown)
				{
					RevealTheMapWithPacket8();
				}
				else
				{
					stopMapReveal = true;
				}
				return;
			case "acmbg":
				autoMobileCrash_Explosives = !autoMobileCrash_Explosives;
				Main.NewText("Auto-mobile crashing for griefers " + (autoMobileCrash_Explosives ? "en" : "dis") + "abled.", zazasFavoriteColor);
				return;
			case "acmbd":
				autoMobileCrash_Dressers = !autoMobileCrash_Dressers;
				Main.NewText("Auto-mobile crashing for dresser crashers " + (autoMobileCrash_Dressers ? "en" : "dis") + "abled.", zazasFavoriteColor);
				return;
			case "fix":
			case "fixup":
				Main.NewText($"Current pastes: {concurrentPastes}, Current Fill/Delete: {concurrentFillOrDels}, Current Conversion: {concurrentConversions}", zazasFavoriteColor);
				fixUpperTime = 300;
				return;
			case "spamchat":
				if (array.Length > 1)
				{
					if (array[1] == ".beemovie")
					{
						spammingBeeMovieScript = true;
						SpamTheEntireFuckingBeeMovieScript();
						return;
					}
					chatSpammerString = chatText.Substring(array[0].Length + 2);
					chatSpammer = true;
					spammingBeeMovieScript = false;
					Main.NewText("Chat Spammer " + (chatSpammer ? "En" : "Dis") + "abled", zazasFavoriteColor);
				}
				else
				{
					chatSpammer = false;
					spammingBeeMovieScript = false;
					Main.NewText("Chat Spammer " + (chatSpammer ? "En" : "Dis") + "abled", zazasFavoriteColor);
					Main.NewText("No Input Detected", errorColor);
				}
				return;
			case "crashmobileplayersold":
			case "crashmobileold":
			case "cmbo":
				ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("[ct:7,5456] [i:6414] [ct:7, ====== =======  ================================================================================================================================== === :)))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))) "));
				return;
			case "crashmobileplayers":
			case "crashmobile":
			case "cmb":
				CrashMobilePlayersV2();
				return;
			case "fuck":
				ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("```\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f``````\n\n\n\n\n\u034f```"));
				return;
			case "beemovie":
				PutTheEntireFuckingBeeMovieScriptIntoServerChat();
				return;
			case "tilebrush":
			case "tb":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out tileBrushSize))
					{
						tileBrush = true;
						Main.NewText("Tile Brush " + (tileBrush ? "En" : "Dis") + "abled  Size: " + tileBrushSize, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Value ", errorColor);
					}
				}
				else
				{
					tileBrush = !tileBrush;
					Main.NewText("Tile Brush " + (tileBrush ? "En" : "Dis") + "abled" + (tileBrush ? ("  Size: " + tileBrushSize) : ""), zazasFavoriteColor);
				}
				return;
			case "paintbrush":
			case "pb":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out tilePaintBrushSize))
					{
						tilePaintBrush = true;
						Main.NewText("Super Paint Brush " + (tilePaintBrush ? "En" : "Dis") + "abled  Size: " + tilePaintBrushSize, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Value ", errorColor);
					}
				}
				else
				{
					tilePaintBrush = !tilePaintBrush;
					Main.NewText("Super Paint Brush " + (tilePaintBrush ? "En" : "Dis") + "abled " + (tilePaintBrush ? ("  Size: " + tilePaintBrushSize) : ""), zazasFavoriteColor);
				}
				return;
			case "paintonlytile":
			case "onlypainttile":
			case "opt":
				if (array.Length > 1)
				{
					if (short.TryParse(array[1], out var tileID) && tileID < TileID.Count && tileID >= -1)
					{
						paintOnlyTile = tileID;
						if (tileID == -1)
						{
							Main.NewText("Super Paint Brush will no longer paint a specific tile.", zazasFavoriteColor);
							return;
						}
						Main.NewText($"Super Paint Brush will only paint the {TileID.Search.GetName((int)tileID)} tile. (ID: {tileID}).", zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Tile ID.", errorColor);
					}
				}
				else if (paintOnlyTile == -1)
				{
					Main.NewText("No tile target to paint.", zazasFavoriteColor);
				}
				else
				{
					Main.NewText($"Current tile to paint: {TileID.Search.GetName((int)paintOnlyTile)} (ID: {paintOnlyTile}).", zazasFavoriteColor);
				}
				return;
			case "paintonlywall":
			case "onlypaintwall":
			case "opw":
				if (array.Length > 1)
				{
					if (short.TryParse(array[1], out var wallId) && wallId < WallID.Count && wallId >= 0)
					{
						paintOnlyWall = wallId;
						if (wallId == 0)
						{
							Main.NewText("Super Paint Brush will no longer paint a specific wall.", zazasFavoriteColor);
							return;
						}
						Main.NewText($"Super Paint Brush will only paint the {WallID.Search.GetName((int)wallId)} wall. (ID: {wallId}).", zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Wall ID.", errorColor);
					}
				}
				else if (paintOnlyWall == 0)
				{
					Main.NewText("No wall target to paint.", zazasFavoriteColor);
				}
				else
				{
					Main.NewText($"Current wall to paint: {WallID.Search.GetName((int)paintOnlyWall)} (ID: {paintOnlyWall}).", zazasFavoriteColor);
				}
				return;
			case "erasebrush":
			case "eb":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out eraseBrushSize))
					{
						eraseBrush = true;
						Main.NewText("Erase Brush " + (eraseBrush ? "En" : "Dis") + "abled  Size: " + eraseBrushSize, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Value ", errorColor);
					}
				}
				else
				{
					eraseBrush = !eraseBrush;
					Main.NewText("Erase Brush " + (eraseBrush ? "En" : "Dis") + "abled" + (eraseBrush ? ("  Size: " + eraseBrushSize) : ""), zazasFavoriteColor);
				}
				return;
			case "liquidbrush":
			case "lb":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out liquidBrushSize))
					{
						liquidBrush = true;
						Main.NewText("Liquid & Dry Brush " + (liquidBrush ? "En" : "Dis") + "abled  Size: " + liquidBrushSize, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Value ", errorColor);
					}
				}
				else
				{
					liquidBrush = !liquidBrush;
					Main.NewText("Liquid Brush " + (liquidBrush ? "En" : "Dis") + "abled" + (liquidBrush ? ("  Size: " + liquidBrushSize) : ""), zazasFavoriteColor);
				}
				return;
			case "copy":
				copyMode = !copyMode;
				Main.NewText("Copy Selection Mode " + (copyMode ? "En" : "Dis") + "abled", zazasFavoriteColor);
				if (pasteMode || brushMode)
				{
					pasteMode = false;
					brushMode = false;
				}
				return;
			case "paste":
				pasteMode = !pasteMode;
				Main.NewText("Paste Mode " + (pasteMode ? "En" : "Dis") + "abled", zazasFavoriteColor);
				if (copyMode || brushMode)
				{
					copyMode = false;
					brushMode = false;
				}
				return;
			case "loadbp":
				Main.NewText("Removed, use .loadsch to use the new TEdit Schematics.", zazasFavoriteColor);
				return;
			case "savebp":
				Main.NewText("Removed, use .savesch to use the new TEdit Schematics.", zazasFavoriteColor);
				return;
			case "loadsch":
			case "opensch":
				Schematics.LoadTEDITSchematic((array.Length > 1) ? chatText.Substring(text.Length + 2) : "");
				return;
			case "savesch":
				Schematics.SaveTEDITSchematic((array.Length > 1) ? chatText.Substring(text.Length + 2) : "");
				return;
			case "infinitemana":
			case "infmana":
				infiniteMana = !infiniteMana;
				Main.NewText("Infinite Mana " + (infiniteMana ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "delgrounditems":
			case "wipeitems":
			case "wi":
				DeleteGroundItemsCall(0);
				return;
			case "instarespawn":
			case "instantrespawn":
				instantRespawn = !instantRespawn;
				Main.NewText("Instant Respawn " + (instantRespawn ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "infinitereach":
			case "infreach":
				infiniteReach = !infiniteReach;
				Main.NewText("Infinite Reach " + (infiniteReach ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "unlockachievements":
				Main.Achievements.DebugCompleteAll();
				return;
			case "autokillprojectiles":
			case "autokillproj":
			case "akpj":
				autoKillProjectiles = !autoKillProjectiles;
				Main.NewText("AutoKill Projectiles " + (autoKillProjectiles ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "drawphoto":
			case "drawpic":
			case "loadpic":
				Schematics.LoadPhotosFromFile((array.Length > 1) ? chatText.Substring(text.Length + 2) : "");
				return;
			case "dmode":
			{
				if (array.Length > 1 && int.TryParse(array[1], out var mode) && mode >= 0 && mode < 3)
				{
					drawPhotoType = mode;
				}
				return;
			}
			case "checksize":
				Main.NewText("MarkedTilesForEraseBrush - Capacity/Size: " + markedTilesForEraseBrush.Count, zazasFavoriteColor);
				Main.NewText("MarkedTilesForTileBrush - Capacity/Size: " + markedTilesForTileBrush.Count, zazasFavoriteColor);
				Main.NewText("MarkedTilesForLiquidBrush - Capacity/Size: " + markedTilesForLiquidAndDryBrush.Count, zazasFavoriteColor);
				return;
			case "seetiledata":
			case "td":
				seeTileData = !seeTileData;
				Main.NewText("Tile information on the mouse cursor " + (seeTileData ? "en" : "dis") + "abled.", zazasFavoriteColor);
				return;
			case "seenpcdata":
			case "nd":
				seeNpcData = !seeNpcData;
				Main.NewText("Extra NPC information on the moused over NPC " + (seeNpcData ? "en" : "dis") + "abled.", zazasFavoriteColor);
				return;
			case "tiledata":
			{
				Tile tile2 = Main.tile[(int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16];
				Main.NewText("-Tile Data-", infoColor);
				Main.NewText($"Tile Active: {tile2.active()} | Tile Type: {tile2.type}, Tile Wall: {tile2.wall} | Tile FrameX: {tile2.frameX} | Tile FrameY: {tile2.frameY} | Tile Liquid Type: {tile2.liquidType()} | Tile Liquid Amount: {tile2.liquid}", infoColor);
				Main.NewText($"Tile Wire: {tile2.wire()} | Tile Wire2: {tile2.wire2()} | Tile Wire3: {tile2.wire3()} | Tile Wire4: {tile2.wire4()} | Tile Actuator: {tile2.actuator()} | Tile Inactive: {tile2.inActive()}", infoColor);
				Main.NewText($"Tile Paint: {tile2.color()} | Tile Wall Paint: {tile2.wallColor()} | Tile Fullbright: {tile2.fullbrightBlock()} | Tile Fullbright Wall: {tile2.fullbrightWall()} | Tile Invisible: {tile2.invisibleBlock()} | Tile Invisible Wall: {tile2.invisibleWall()}", infoColor);
				Main.NewText(string.Format("Tile Wall FrameX: {0} | Tile Wall FrameY: {1}", new object[2]
				{
					tile2.wallFrameX(),
					tile2.wallFrameY()
				}), infoColor);
				Main.NewText("-----------------", infoColor);
				return;
			}
			case "killaura":
			case "killbubble":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out killAuraSize))
					{
						killAura = true;
						Main.NewText("Kill Aura " + (killAura ? "En" : "Dis") + "abled  Size: " + killAuraSize, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Value ", errorColor);
					}
				}
				else
				{
					killAuraSize = 1;
					killAura = !killAura;
					Main.NewText("Kill Aura " + (killAura ? "En" : "Dis") + "abled" + (killAura ? ("  Size: " + killAuraSize) : ""), zazasFavoriteColor);
				}
				return;
			case "extendedpickup":
				extendedPickUpRange = !extendedPickUpRange;
				Main.NewText("Extended Pick-Up Range " + (extendedPickUpRange ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "extendedplacement":
				extendedPlacementRange = !extendedPlacementRange;
				Main.NewText("Extended Placement Range " + (extendedPlacementRange ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "fitem":
			case "fishitem":
			case "fishi":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num67) && num67 < 5456)
					{
						fishItem = num67;
						Main.NewText("Item ID " + num67 + " will now be fished up every time you fish.", zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Item ID.", errorColor);
					}
				}
				else
				{
					fishItem = 0;
					Main.NewText("No longer overriding fished up items.", zazasFavoriteColor);
				}
				return;
			case "wallbrush":
			case "wb":
				EasyCommand1(array, ref wallBrush, ref wallBrushSize, "Wall Brush");
				return;
			case "freecam":
				freeCam = !freeCam;
				Main.NewText("Free Camera " + (freeCam ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "freecam2":
				freeCam = !freeCam;
				freeCam2 = !freeCam2;
				Main.NewText("Free Camera (No chunk load) " + (freeCam2 ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "freecamd":
				freeCamDebug = !freeCamDebug;
				Main.NewText("Freecam Debug " + (freeCamDebug ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "binds":
			case "bind":
				keyBindMaster.Clear();
				return;
			case "lag":
			{
				lag = !lag;
				Main.NewText("Spamming solar velocity " + (lag ? "En" : "Dis") + "abled", zazasFavoriteColor);
				for (int num64 = 0; num64 < 255; num64++)
				{
					if (Main.player[num64].active && num64 != Main.myPlayer)
					{
						Vector2 vector2 = Main.player[num64].RotatedRelativePoint(Main.player[num64].MountedCenter) + new Vector2(0f, 3200f);
						ModsNetMessages.SendModifiedDataProj(27, -1, -1, null, 0, vector2, new Vector2(0f, 100000000f), Main.myPlayer, 611);
					}
				}
				return;
			}
			case "lucy":
			{
				int lucySource = 0;
				byte lucyVar = 0;
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var tempInt2) && tempInt2 >= 0 && tempInt2 < 7)
					{
						lucySource = tempInt2;
					}
					byte tempByte = byte.MaxValue;
					if (array.Length > 2 && byte.TryParse(array[2], out tempByte) && tempByte <= GetMaxVariationForLucy((LucyAxeMessage.MessageSource)lucySource, tempByte))
					{
						lucyVar = tempByte;
					}
				}
				LucyAxeMessage.Create((LucyAxeMessage.MessageSource)lucySource, Main.MouseWorld, new Vector2(Main.rand.NextFloatDirection() * 7f, -2f + Main.rand.NextFloat() * -2f), forced: true, lucyVar);
				return;
			}
			case "golfball":
			case "golf":
				golfBallSpammer = !golfBallSpammer;
				Main.NewText("Golf spam " + (golfBallSpammer ? "en" : "dis") + "abled.", zazasFavoriteColor);
				return;
			case "shio":
			case "showitemowner":
				Main.showItemOwner = !Main.showItemOwner;
				Main.NewText("Who an item is reserved for will " + (Main.showItemOwner ? "now " : "no longer ") + "be shown when moused over.", zazasFavoriteColor);
				return;
			case "tpbypass":
			case "bypasstp":
				bypassTeleportRequests = !bypassTeleportRequests;
				Main.NewText("Teleportation Bypass " + (bypassTeleportRequests ? "en" : "dis") + "abled.", zazasFavoriteColor);
				if (!teleportBypassInfoShown && bypassTeleportRequests && Instance.DetermineUnloadedSections > 0)
				{
					Main.NewText("Teleportation Potions, Demon Conch, and Magic Conch (including Shellphone teleports) will have issues if the chunks aren't loaded.", infoColor);
					Main.NewText("Reveal the map for the best results.", infoColor);
					teleportBypassInfoShown = true;
				}
				return;
			case "serverspy":
			case "serverpeek":
			case "spy":
				PlayerVariantID.Sets.serverPeekEnabled = !PlayerVariantID.Sets.serverPeekEnabled;
				Main.NewText("You will " + (PlayerVariantID.Sets.serverPeekEnabled ? "now " : "no longer ") + "spy on servers when joining them.", zazasFavoriteColor);
				if (PlayerVariantID.Sets.serverPeekEnabled)
				{
					Main.NewText("In server spy mode, you are not fully connected to a server, but can still talk in chat and explore the world.", infoColor);
					Main.NewText("No other players are visible, and their names in chat may appear incorrectly. You also cannot interact with nor modify anything.", infoColor);
					Main.NewText("Use this carefully. If you send a chat message without having fully joined, you may expose yourself. It is still announced when you leave, so also take that into caution.", infoColor);
					Main.NewText("Server spy mode only starts when you join a server. When you exit server spy, you will respawn and finish joining the server.", infoColor);
				}
				else if (PlayerVariantID.Sets.spyingOnServer)
				{
					PlayerVariantID.Sets.spyingOnServer = false;
					Main.NewText("Server spy ended. You have joined the server as normal. Be warned that it may expose you for cheating if you sent messages before joining.", infoColor);
					NetMessage.TrySendData(4, -1, -1, null, Main.myPlayer);
					if (hideUUID != 1)
					{
						NetMessage.TrySendData(68, -1, -1, null, Main.myPlayer);
					}
					NetMessage.TrySendData(16, -1, -1, null, Main.myPlayer);
					NetMessage.TrySendData(42, -1, -1, null, Main.myPlayer);
					NetMessage.TrySendData(50, -1, -1, null, Main.myPlayer);
					NetMessage.TrySendData(147, -1, -1, null, Main.myPlayer, localPlayer.CurrentLoadoutIndex);
					for (int num51 = 0; num51 < 59; num51++)
					{
						NetMessage.TrySendData(5, -1, -1, null, Main.myPlayer, PlayerItemSlotID.Inventory0 + num51, (int)localPlayer.inventory[num51].prefix);
					}
					MessageBuffer.TrySendingItemArray(Main.myPlayer, localPlayer.armor, PlayerItemSlotID.Armor0);
					MessageBuffer.TrySendingItemArray(Main.myPlayer, localPlayer.dye, PlayerItemSlotID.Dye0);
					MessageBuffer.TrySendingItemArray(Main.myPlayer, localPlayer.miscEquips, PlayerItemSlotID.Misc0);
					MessageBuffer.TrySendingItemArray(Main.myPlayer, localPlayer.miscDyes, PlayerItemSlotID.MiscDye0);
					MessageBuffer.TrySendingItemArray(Main.myPlayer, localPlayer.bank.item, PlayerItemSlotID.Bank1_0);
					MessageBuffer.TrySendingItemArray(Main.myPlayer, localPlayer.bank2.item, PlayerItemSlotID.Bank2_0);
					NetMessage.TrySendData(5, -1, -1, null, Main.myPlayer, PlayerItemSlotID.TrashItem, (int)localPlayer.trashItem.prefix);
					MessageBuffer.TrySendingItemArray(Main.myPlayer, localPlayer.bank3.item, PlayerItemSlotID.Bank3_0);
					MessageBuffer.TrySendingItemArray(Main.myPlayer, localPlayer.bank4.item, PlayerItemSlotID.Bank4_0);
					MessageBuffer.TrySendingItemArray(Main.myPlayer, localPlayer.Loadouts[0].Armor, PlayerItemSlotID.Loadout1_Armor_0);
					MessageBuffer.TrySendingItemArray(Main.myPlayer, localPlayer.Loadouts[0].Dye, PlayerItemSlotID.Loadout1_Dye_0);
					MessageBuffer.TrySendingItemArray(Main.myPlayer, localPlayer.Loadouts[1].Armor, PlayerItemSlotID.Loadout2_Armor_0);
					MessageBuffer.TrySendingItemArray(Main.myPlayer, localPlayer.Loadouts[1].Dye, PlayerItemSlotID.Loadout2_Dye_0);
					MessageBuffer.TrySendingItemArray(Main.myPlayer, localPlayer.Loadouts[2].Armor, PlayerItemSlotID.Loadout3_Armor_0);
					MessageBuffer.TrySendingItemArray(Main.myPlayer, localPlayer.Loadouts[2].Dye, PlayerItemSlotID.Loadout3_Dye_0);
					NetMessage.TrySendData(6);
					Main.player[Main.myPlayer].FindSpawn();
					NetMessage.SendData(8, -1, -1, null, Main.player[Main.myPlayer].SpawnX, Main.player[Main.myPlayer].SpawnY);
				}
				return;
			case "infminions":
			case "infiniteminions":
				infiniteMinions = !infiniteMinions;
				Main.NewText("Infinite Minions " + (infiniteMinions ? "En" : "Dis") + "abled", zazasFavoriteColor);
				return;
			case "trollitem":
			case "tritm":
			{
				int trollId = -1;
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out trollId) && trollId > 0 && trollId < 5456)
					{
						trollItemId = trollId;
					}
					else
					{
						trollItemId = -1;
					}
					Main.NewText("Troll item set to " + ((trollItemId == -1) ? "random items" : trollId.ToString()) + ".", zazasFavoriteColor);
				}
				else if (trollItemId != 0)
				{
					trollItemId = 0;
					Main.NewText("Troll item disabled.", zazasFavoriteColor);
				}
				else
				{
					trollItemId = -1;
					Main.NewText("Troll item set to random items.", zazasFavoriteColor);
				}
				return;
			}
			case "fakeitem":
			case "fakeitm":
			{
				if (array.Length <= 1 || !int.TryParse(array[1], out var itemId2) || itemId2 <= 0 || itemId2 >= 5456)
				{
					return;
				}
				Vector2 spawnPos = default(Vector2);
				for (int num36 = 0; num36 < 255; num36++)
				{
					Player player3 = Main.player[num36];
					if (player3.active && player3.whoAmI != Main.myPlayer)
					{
						((Vector2)(ref spawnPos))._002Ector((float)Main.rand.Next((int)Main.player[num36].Center.X - 960, (int)Main.player[num36].Center.X + 961), (float)Main.rand.Next((int)Main.player[num36].Center.Y - 540, (int)Main.player[num36].Center.Y + 541));
						ParticleOrchestrator.BroadcastOrRequestParticleSpawn(ParticleOrchestraType.ItemTransfer, new ParticleOrchestraSettings
						{
							PositionInWorld = spawnPos,
							MovementVector = Main.player[num36].Center - spawnPos,
							UniqueInfoPiece = itemId2
						});
					}
				}
				return;
			}
			case "pvpkill":
			case "killpvp":
			case "killpvpall":
				if (Main.netMode != 0)
				{
					Main.NewText("Killing Players...", zazasFavoriteColor);
					PlayerDeathReason playerDeathReason2 = PlayerDeathReason.LegacyDefault();
					for (int num35 = 0; num35 < 255; num35++)
					{
						if (Main.player[num35].active && Main.player[num35].whoAmI != Main.myPlayer)
						{
							NetMessage.SendPlayerHurt(num35, playerDeathReason2, 1000, -1, critical: true, pvp: true, -1);
						}
						if (text == "killpvpall" && Main.player[num35].whoAmI == Main.myPlayer)
						{
							Main.player[num35].KillMe(playerDeathReason2, 1000.0, -1, pvp: true, forced: true);
						}
					}
				}
				else
				{
					Main.NewText("You can only do this in an online game.", errorColor);
				}
				return;
			case "detvanish":
			case "findvanish":
				if (Main.netMode == 1)
				{
					Main.NewText("Detecting vanished players...", zazasFavoriteColor);
					PlayerDeathReason playerDeathReason = PlayerDeathReason.ByPlayer(255);
					for (int num34 = 0; num34 < 255; num34++)
					{
						if (!Main.player[num34].active)
						{
							NetMessage.SendPlayerHurt(num34, playerDeathReason, 1, -1, critical: true, pvp: true, -1);
						}
					}
					awaitingTshockPvpResponseTime = 300;
				}
				else
				{
					Main.NewText("You can only do this in an online game.", errorColor);
				}
				return;
			case "invis":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num32))
					{
						if (num32 < -1)
						{
							num32 = 1800;
						}
						else if (num32 == -1)
						{
							num32 = int.MaxValue;
						}
						Main.NewText("Invisible for " + num32, zazasFavoriteColor);
						Main.LocalPlayer.AddBuff(10, num32);
						NetMessage.SendData(50, -1, -1, null, Main.myPlayer);
					}
					else
					{
						Main.NewText("Invalid Value ", errorColor);
					}
				}
				else
				{
					Main.NewText("Value Needed ", errorColor);
				}
				return;
			case "changename":
				if (array.Length > 1)
				{
					Main.LocalPlayer.name = chatText.Substring(array[0].Length + 2);
					ModsNetMessages.SendModifiedDataPlrSync(4, -1, -1, null, Main.myPlayer, Main.LocalPlayer);
					Main.NewText("Name Changed: " + Main.LocalPlayer.name, zazasFavoriteColor);
				}
				else
				{
					Main.NewText("You need a name for yourself. ", errorColor);
				}
				return;
			case "iname":
				if (array.Length > 1)
				{
					Main.LocalPlayer.HeldItem.SetNameOverride(chatText.Substring(array[0].Length + 2));
					Main.NewText("Name Changed: " + Main.LocalPlayer.HeldItem.Name, zazasFavoriteColor);
				}
				else
				{
					Main.NewText("You need a name for yourself. ", errorColor);
				}
				return;
			case "rko":
				localPlayer.HasOutlineEffect = !localPlayer.HasOutlineEffect;
				NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
				Main.NewText("Outline effect " + (localPlayer.HasOutlineEffect ? "en." : "dis.") + "abled.", zazasFavoriteColor);
				return;
			case "earrape":
				earRape = !earRape;
				Main.NewText(earRape ? "ON" : "OFF", zazasFavoriteColor);
				return;
			case "wireline":
				WiresUI.WiresRadial.useLine = !WiresUI.WiresRadial.useLine;
				Main.NewText("Changed Wires UI to " + (WiresUI.WiresRadial.useLine ? "Lines (persistent at bottom of screen)." : "Radial (at cursor)."), zazasFavoriteColor);
				return;
			case "debugmsg":
				debugMessages = !debugMessages;
				Main.NewText(debugMessages ? "ON" : "OFF", zazasFavoriteColor);
				return;
			case "oldui":
				uiRework = !uiRework;
				Main.NewText("Switched to " + (uiRework ? "NEW UI" : "OLD UI"), zazasFavoriteColor);
				if (!uiRework)
				{
					Main.NewText("THIS UI IS UNMAINTAINED. WILL BE REMOVED IN TERRARIA 1.4.5", infoColor);
				}
				return;
			case "refui":
				ModsNewUICheatPanel.RefreshUI();
				customUI2.SetState(ModsNewUICheatPanel.uiStateNewCheatPanel);
				Main.NewText("Refreshed New Mods UI", zazasFavoriteColor);
				return;
			case "asm":
				Main.NewText(Assembly.GetExecutingAssembly().GetName().Name ?? "", zazasFavoriteColor);
				return;
			case "anim":
				if (array.Length > 1)
				{
					if (array.Length > 2)
					{
						if (!float.TryParse(array[1], out var rotation))
						{
							Main.NewText("Invalid rotation (float).", errorColor);
							return;
						}
						if (!short.TryParse(array[2], out var time2))
						{
							Main.NewText("Invalid animation time (short).", errorColor);
							return;
						}
						localPlayer.itemRotation = rotation;
						localPlayer.itemAnimation = time2;
						if (Main.netMode != 0)
						{
							NetMessage.SendData(41, -1, -1, null, Main.myPlayer);
						}
						Main.NewText("Played item animation on yourself.", zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Item time needed.", errorColor);
					}
				}
				else
				{
					Main.NewText("Item rotation needed.", errorColor);
				}
				return;
			case "dust":
				if (array.Length > 1)
				{
					if (int.TryParse(array[1], out var num12) && num12 >= 0 && num12 <= 100)
					{
						dustLevel = num12;
						Main.NewText("Dust level changed to " + dustLevel, zazasFavoriteColor);
					}
					else
					{
						Main.NewText("Invalid Dust level (0 - 100)", errorColor);
					}
				}
				else
				{
					Main.NewText("Dust level is " + dustLevel, zazasFavoriteColor);
				}
				return;
			case "cleangores":
			case "cgores":
			case "cleangore":
			case "cgore":
				CleanupGores();
				return;
			case "seed":
			{
				string currentWorldSeed = WorldGen.currentWorldSeed;
				Main.NewText("Seed: " + currentWorldSeed, zazasFavoriteColor);
				return;
			}
			case "crackseed":
			{
				int candidateSeed = (int)((316080314L * (long)Main.worldID + 75192552) % int.MaxValue);
				Platform.Get<IClipboard>().Value = $"{candidateSeed}";
				Main.NewText($"Possible world seed: {candidateSeed} has been copied to your clipboard.\nNOTE: This may be inaccurate, there is a 1/800 chance of this being the incorrect seed.", zazasFavoriteColor);
				return;
			}
			case "on":
			case "quickcheat":
			case "qc":
				EnableMostModHacks();
				Main.NewText("Enabled most cheats.", zazasFavoriteColor);
				return;
			case "off":
				Instance.DisableMostModHacks();
				Main.NewText("Disabled Some Bools", zazasFavoriteColor);
				return;
			default:
				Main.NewText("Unknown command: " + text, byte.MaxValue, 50, 80);
				return;
			}
		}
		string npcName = "";
		if (onTshockServer)
		{
			Main.NewText("Spawnmob doesn't work on TShock Servers.", errorColor);
			return;
		}
		Func<string, Tuple<bool, int, string>> getNpcId = delegate(string text3)
		{
			FieldInfo fieldInfo = typeof(NPCID).GetFields().FirstOrDefault((FieldInfo info) => info.Name.ToLower() == text3.ToLower());
			bool flag = int.TryParse(text3, out var result);
			if (fieldInfo != null)
			{
				result = NPCID.Search.GetId(fieldInfo.Name);
				return Tuple.Create(item1: true, result, fieldInfo.Name);
			}
			return flag ? Tuple.Create(item1: true, result, NPCID.Search.GetName(result)) : Tuple.Create(item1: false, 0, "");
		};
		int amount = 1;
		if (array.Length > 2)
		{
			Tuple<bool, int, string> tuple2 = getNpcId(array[1]);
			bool parse = tuple2.Item1;
			int npcId = tuple2.Item2;
			npcName = tuple2.Item3;
			if (parse)
			{
				Point point = Main.MouseWorld.ToTileCoordinates();
				point.X -= 40;
				int.TryParse(array[2], out amount);
				amount = Math.Clamp(amount, 1, 200);
				Point origin = Main.MouseWorld.ToTileCoordinates();
				int maxTiles = 51;
				int cols = Math.Min((int)Math.Ceiling(Math.Sqrt(amount)), maxTiles);
				int rows = Math.Min((int)Math.Ceiling((double)amount / (double)cols), maxTiles);
				double spacingX = ((cols > 1) ? (50.0 / (double)(cols - 1)) : 0.0);
				double spacingY = ((rows > 1) ? (50.0 / (double)(rows - 1)) : 0.0);
				if (amount > 0 && amount <= 200 && npcId > -66 && npcId < NPCID.Count && npcId != 0)
				{
					for (int num127 = 0; num127 < amount; num127++)
					{
						int cx = num127 % cols;
						int cy = num127 / cols;
						double offsetX = -25.0 + spacingX * (double)cx;
						double offsetY = -25.0 + spacingY * (double)cy;
						int spawnX = (origin.X + (int)Math.Round(offsetX)) * 16;
						int spawnY = (origin.Y + (int)Math.Round(offsetY)) * 16;
						if (Main.netMode != 1)
						{
							NPC.NewNPC(new EntitySource_FishedOut(Main.LocalPlayer), spawnX, spawnY, npcId);
						}
						else
						{
							NetMessage.SendData(130, -1, -1, null, spawnX / 16, spawnY / 16, npcId);
						}
					}
				}
				else if (npcId < -65 || npcId >= NPCID.Count || npcId == 0)
				{
					Main.NewText("Invalid NPC.", errorColor);
				}
				else if (amount <= 0 || amount > 200)
				{
					Main.NewText("Invalid amount.", errorColor);
				}
			}
			else
			{
				Main.NewText("Invalid NPC or amount.", errorColor);
			}
		}
		else if (array.Length > 1)
		{
			Tuple<bool, int, string> tuple3 = getNpcId(array[1]);
			bool parse2 = tuple3.Item1;
			int npcId2 = tuple3.Item2;
			npcName = tuple3.Item3;
			if (npcId2 > -66 && npcId2 < NPCID.Count && npcId2 != 0)
			{
				if (parse2)
				{
					Point point2 = Main.MouseWorld.ToTileCoordinates();
					if (Main.netMode == 0)
					{
						NPC.NewNPC(new EntitySource_FishedOut(Main.LocalPlayer), point2.X * 16, point2.Y * 16, npcId2);
					}
					else
					{
						NetMessage.SendData(130, -1, -1, null, point2.X, point2.Y, npcId2);
					}
				}
			}
			else
			{
				Main.NewText("Invalid NPC.", errorColor);
			}
		}
		else
		{
			Main.NewText("Invalid NPC or amount.", errorColor);
		}
		if (npcName != "")
		{
			Main.NewText("Spawned " + ((array.Length > 2 && amount > 1) ? array[2] : "1") + " " + npcName, zazasFavoriteColor);
		}
	}

	public void UponEnteringWorld()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		if (hackedMOTD)
		{
			Main.NewText("------------------------------------", zazasFavoriteColor);
			Main.NewText("ZazaMod 1.3.7.1.4", zazasFavoriteColor);
			Main.NewText("Please report any issues you encounter.", zazasFavoriteColor);
			Main.NewText("------------------------------------", zazasFavoriteColor);
			hackedMOTD = false;
		}
		if (PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus["Open Menu"].Count <= 0)
		{
			Main.NewText("Warning: Open Menu is not bound! Please open your controls and bind the key!", infoColor);
		}
		if (PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus["Mouse TP"].Count <= 0)
		{
			Main.NewText("Warning: Mouse TP is not bound! Please open your controls and bind the key!", infoColor);
		}
		if (PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus["Map TP"].Count <= 0)
		{
			Main.NewText("Warning: Map TP is not bound! Please open your controls and bind the key!", infoColor);
		}
		if (PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus["Use Brush"].Count <= 0)
		{
			Main.NewText("Warning: Use Brush is not bound! Please open your controls and bind the key!", infoColor);
		}
		if (PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus["Use Brush (Map)"].Count <= 0)
		{
			Main.NewText("Warning: Use Brush (Map) is not bound! Please open your controls and bind the key!", infoColor);
		}
		if (PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus["Hold for Activator"].Count <= 0)
		{
			Main.NewText("Warning: Hold for Activator is not bound! Please open your controls and bind the key!", infoColor);
		}
		if (PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus["Hold for Activator (Map)"].Count <= 0)
		{
			Main.NewText("Warning: Hold for Activator (Map) is not bound! Please open your controls and bind the key!", infoColor);
		}
		if (encryptChatInvalidKeyWarning)
		{
			Main.NewText("Warning: The encryption key for Encrypted Chat was invalid. It has been changed to: " + EncChatKeyString, infoColor);
			encryptChatInvalidKeyWarning = false;
		}
		hasLoadedIntoWorld = true;
		if (!Main.ServerSideCharacter && !onTshockServer)
		{
			ModsNetMessages.SendModifiedData(147);
			NetMessage.SendData(147, -1, -1, null, Main.myPlayer, Main.LocalPlayer.CurrentLoadoutIndex);
			awaitTshockResponseTime = 300;
		}
		detectedVanishedPlayers = 0;
		awaitingTshockPvpResponseTime = 0;
		PlayerVariantID.Sets.serverPeekWarningShown = false;
		teleportBypassInfoShown = false;
		concurrentPastes = 0;
		ItemMods.RefreshAllPresets();
		ItemMods.AutoApplyPreset();
		if (PlayerVariantID.Sets.spyingOnServer && Main.netMode == 1)
		{
			Main.NewText("You are in server spy mode. In server spy mode, you are not fully connected to a server, but can still talk in chat and explore the world.", infoColor);
			Main.NewText("No other players are visible, and their names in chat may appear incorrectly. You also cannot interact with nor modify anything.", infoColor);
			Main.NewText("Use this carefully. If you send a chat message without having fully joined, you may expose yourself. It is still announced when you leave, so also take that into caution.", infoColor);
			Main.NewText("To exit server spy, run .serverspy again or disable it in the menu. You will respawn and finish joining the server.", infoColor);
		}
		encryptChatWarning = encryptChat;
	}

	public static void PrintCurrentPlayerDifficulty()
	{
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Main.LocalPlayer;
		string text3 = "";
		text3 = localPlayer.difficulty switch
		{
			0 => Language.GetTextValue("UI.Softcore"), 
			1 => Language.GetTextValue("UI.Mediumcore"), 
			2 => Language.GetTextValue("UI.Hardcore"), 
			3 => Language.GetTextValue("UI.Creative"), 
			_ => "Unknown", 
		};
		Main.NewText($"Current player difficulty: {text3} (ID: {localPlayer.difficulty})", zazasFavoriteColor);
	}

	static Mods()
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_0768: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0787: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Unknown result type (might be due to invalid IL or missing references)
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_083b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Unknown result type (might be due to invalid IL or missing references)
		//IL_084d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Unknown result type (might be due to invalid IL or missing references)
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_0883: Unknown result type (might be due to invalid IL or missing references)
		//IL_0888: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Unknown result type (might be due to invalid IL or missing references)
		//IL_089a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0901: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_0913: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0937: Unknown result type (might be due to invalid IL or missing references)
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0949: Unknown result type (might be due to invalid IL or missing references)
		//IL_094e: Unknown result type (might be due to invalid IL or missing references)
		//IL_095b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0960: Unknown result type (might be due to invalid IL or missing references)
		//IL_096d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_097f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0984: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Unknown result type (might be due to invalid IL or missing references)
		//IL_0996: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09de: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0daa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ded: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_100b: Unknown result type (might be due to invalid IL or missing references)
		//IL_101b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1020: Unknown result type (might be due to invalid IL or missing references)
		//IL_1030: Unknown result type (might be due to invalid IL or missing references)
		//IL_1035: Unknown result type (might be due to invalid IL or missing references)
		//IL_1045: Unknown result type (might be due to invalid IL or missing references)
		//IL_104a: Unknown result type (might be due to invalid IL or missing references)
		//IL_105a: Unknown result type (might be due to invalid IL or missing references)
		//IL_105f: Unknown result type (might be due to invalid IL or missing references)
		//IL_106f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1074: Unknown result type (might be due to invalid IL or missing references)
		//IL_1084: Unknown result type (might be due to invalid IL or missing references)
		//IL_1089: Unknown result type (might be due to invalid IL or missing references)
		//IL_1099: Unknown result type (might be due to invalid IL or missing references)
		//IL_109e: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_10dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1102: Unknown result type (might be due to invalid IL or missing references)
		//IL_1107: Unknown result type (might be due to invalid IL or missing references)
		//IL_1117: Unknown result type (might be due to invalid IL or missing references)
		//IL_111c: Unknown result type (might be due to invalid IL or missing references)
		//IL_112c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1131: Unknown result type (might be due to invalid IL or missing references)
		//IL_1141: Unknown result type (might be due to invalid IL or missing references)
		//IL_1146: Unknown result type (might be due to invalid IL or missing references)
		//IL_1156: Unknown result type (might be due to invalid IL or missing references)
		//IL_115b: Unknown result type (might be due to invalid IL or missing references)
		//IL_116b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1170: Unknown result type (might be due to invalid IL or missing references)
		//IL_1180: Unknown result type (might be due to invalid IL or missing references)
		//IL_1185: Unknown result type (might be due to invalid IL or missing references)
		//IL_1195: Unknown result type (might be due to invalid IL or missing references)
		//IL_119a: Unknown result type (might be due to invalid IL or missing references)
		//IL_11aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_11af: Unknown result type (might be due to invalid IL or missing references)
		//IL_11bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1203: Unknown result type (might be due to invalid IL or missing references)
		//IL_1213: Unknown result type (might be due to invalid IL or missing references)
		//IL_1218: Unknown result type (might be due to invalid IL or missing references)
		//IL_1228: Unknown result type (might be due to invalid IL or missing references)
		//IL_122d: Unknown result type (might be due to invalid IL or missing references)
		//IL_123d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1242: Unknown result type (might be due to invalid IL or missing references)
		//IL_1252: Unknown result type (might be due to invalid IL or missing references)
		//IL_1257: Unknown result type (might be due to invalid IL or missing references)
		//IL_1267: Unknown result type (might be due to invalid IL or missing references)
		//IL_126c: Unknown result type (might be due to invalid IL or missing references)
		//IL_127c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1281: Unknown result type (might be due to invalid IL or missing references)
		//IL_1291: Unknown result type (might be due to invalid IL or missing references)
		//IL_1296: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_130f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1314: Unknown result type (might be due to invalid IL or missing references)
		//IL_1324: Unknown result type (might be due to invalid IL or missing references)
		//IL_1329: Unknown result type (might be due to invalid IL or missing references)
		//IL_1339: Unknown result type (might be due to invalid IL or missing references)
		//IL_133e: Unknown result type (might be due to invalid IL or missing references)
		//IL_134e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1353: Unknown result type (might be due to invalid IL or missing references)
		//IL_1363: Unknown result type (might be due to invalid IL or missing references)
		//IL_1368: Unknown result type (might be due to invalid IL or missing references)
		//IL_1378: Unknown result type (might be due to invalid IL or missing references)
		//IL_137d: Unknown result type (might be due to invalid IL or missing references)
		//IL_138d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1392: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_140b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1410: Unknown result type (might be due to invalid IL or missing references)
		//IL_1420: Unknown result type (might be due to invalid IL or missing references)
		//IL_1425: Unknown result type (might be due to invalid IL or missing references)
		//IL_1435: Unknown result type (might be due to invalid IL or missing references)
		//IL_143a: Unknown result type (might be due to invalid IL or missing references)
		//IL_144a: Unknown result type (might be due to invalid IL or missing references)
		//IL_144f: Unknown result type (might be due to invalid IL or missing references)
		//IL_145f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1464: Unknown result type (might be due to invalid IL or missing references)
		//IL_1474: Unknown result type (might be due to invalid IL or missing references)
		//IL_1479: Unknown result type (might be due to invalid IL or missing references)
		//IL_1489: Unknown result type (might be due to invalid IL or missing references)
		//IL_148e: Unknown result type (might be due to invalid IL or missing references)
		//IL_149e: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_14cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_14dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1507: Unknown result type (might be due to invalid IL or missing references)
		//IL_150c: Unknown result type (might be due to invalid IL or missing references)
		//IL_151c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1521: Unknown result type (might be due to invalid IL or missing references)
		//IL_1531: Unknown result type (might be due to invalid IL or missing references)
		//IL_1536: Unknown result type (might be due to invalid IL or missing references)
		//IL_1546: Unknown result type (might be due to invalid IL or missing references)
		//IL_154b: Unknown result type (might be due to invalid IL or missing references)
		//IL_155b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1560: Unknown result type (might be due to invalid IL or missing references)
		//IL_1570: Unknown result type (might be due to invalid IL or missing references)
		//IL_1575: Unknown result type (might be due to invalid IL or missing references)
		//IL_1585: Unknown result type (might be due to invalid IL or missing references)
		//IL_158a: Unknown result type (might be due to invalid IL or missing references)
		//IL_159a: Unknown result type (might be due to invalid IL or missing references)
		//IL_159f: Unknown result type (might be due to invalid IL or missing references)
		//IL_15af: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15de: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1603: Unknown result type (might be due to invalid IL or missing references)
		//IL_1608: Unknown result type (might be due to invalid IL or missing references)
		//IL_1618: Unknown result type (might be due to invalid IL or missing references)
		//IL_161d: Unknown result type (might be due to invalid IL or missing references)
		//IL_162d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1632: Unknown result type (might be due to invalid IL or missing references)
		//IL_1642: Unknown result type (might be due to invalid IL or missing references)
		//IL_1647: Unknown result type (might be due to invalid IL or missing references)
		//IL_1657: Unknown result type (might be due to invalid IL or missing references)
		//IL_165c: Unknown result type (might be due to invalid IL or missing references)
		//IL_166c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1671: Unknown result type (might be due to invalid IL or missing references)
		//IL_1681: Unknown result type (might be due to invalid IL or missing references)
		//IL_1686: Unknown result type (might be due to invalid IL or missing references)
		//IL_1696: Unknown result type (might be due to invalid IL or missing references)
		//IL_169b: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16da: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1704: Unknown result type (might be due to invalid IL or missing references)
		//IL_1714: Unknown result type (might be due to invalid IL or missing references)
		//IL_1719: Unknown result type (might be due to invalid IL or missing references)
		//IL_1729: Unknown result type (might be due to invalid IL or missing references)
		//IL_172e: Unknown result type (might be due to invalid IL or missing references)
		//IL_173e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1743: Unknown result type (might be due to invalid IL or missing references)
		//IL_1753: Unknown result type (might be due to invalid IL or missing references)
		//IL_1758: Unknown result type (might be due to invalid IL or missing references)
		//IL_1768: Unknown result type (might be due to invalid IL or missing references)
		//IL_176d: Unknown result type (might be due to invalid IL or missing references)
		//IL_177d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1782: Unknown result type (might be due to invalid IL or missing references)
		//IL_1792: Unknown result type (might be due to invalid IL or missing references)
		//IL_1797: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_17bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_17eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1800: Unknown result type (might be due to invalid IL or missing references)
		//IL_1810: Unknown result type (might be due to invalid IL or missing references)
		//IL_1815: Unknown result type (might be due to invalid IL or missing references)
		//IL_1825: Unknown result type (might be due to invalid IL or missing references)
		//IL_182a: Unknown result type (might be due to invalid IL or missing references)
		//IL_183a: Unknown result type (might be due to invalid IL or missing references)
		//IL_183f: Unknown result type (might be due to invalid IL or missing references)
		//IL_184f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1854: Unknown result type (might be due to invalid IL or missing references)
		//IL_1864: Unknown result type (might be due to invalid IL or missing references)
		//IL_1869: Unknown result type (might be due to invalid IL or missing references)
		//IL_1879: Unknown result type (might be due to invalid IL or missing references)
		//IL_187e: Unknown result type (might be due to invalid IL or missing references)
		//IL_188e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1893: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_18fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_190c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1911: Unknown result type (might be due to invalid IL or missing references)
		//IL_1921: Unknown result type (might be due to invalid IL or missing references)
		//IL_1926: Unknown result type (might be due to invalid IL or missing references)
		//IL_1936: Unknown result type (might be due to invalid IL or missing references)
		//IL_193b: Unknown result type (might be due to invalid IL or missing references)
		//IL_194b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1950: Unknown result type (might be due to invalid IL or missing references)
		//IL_1960: Unknown result type (might be due to invalid IL or missing references)
		//IL_1965: Unknown result type (might be due to invalid IL or missing references)
		//IL_1975: Unknown result type (might be due to invalid IL or missing references)
		//IL_197a: Unknown result type (might be due to invalid IL or missing references)
		//IL_198a: Unknown result type (might be due to invalid IL or missing references)
		//IL_198f: Unknown result type (might be due to invalid IL or missing references)
		//IL_199f: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_19de: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a37: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a76: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_1adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b19: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b33: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b48: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b82: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b97: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bac: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c05: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c15: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c93: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d11: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d40: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d55: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d65: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dce: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e27: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e37: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e51: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e76: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eca: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ecf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1edf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f23: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f33: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f38: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f48: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f62: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f77: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2005: Unknown result type (might be due to invalid IL or missing references)
		//IL_200a: Unknown result type (might be due to invalid IL or missing references)
		//IL_201a: Unknown result type (might be due to invalid IL or missing references)
		//IL_201f: Unknown result type (might be due to invalid IL or missing references)
		//IL_202f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2034: Unknown result type (might be due to invalid IL or missing references)
		//IL_2044: Unknown result type (might be due to invalid IL or missing references)
		//IL_2049: Unknown result type (might be due to invalid IL or missing references)
		//IL_2059: Unknown result type (might be due to invalid IL or missing references)
		//IL_205e: Unknown result type (might be due to invalid IL or missing references)
		//IL_206e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2073: Unknown result type (might be due to invalid IL or missing references)
		ZazaConfiguration = new Preferences(Main.SavePath + Path.DirectorySeparatorChar + "ZazaMod Config.json");
		zazaErrorTxt = "ZazaMod-Errors.txt";
		rememberEnabled = true;
		commandSpecifier = '.';
		hideHackedMOTD = false;
		zazasFavoriteColor = new Color(31, 184, 155);
		errorColor = new Color(255, 25, 25);
		infoColor = new Color(255, 240, 20);
		cBack = "Back";
		cEnter = "Enter";
		cDelete = "None";
		cOpenChat = "Enter";
		BackspaceKey = (Keys)8;
		SubmitTextKey = (Keys)32;
		AltBackspaceKey = (Keys)0;
		OpenChatKey = (Keys)13;
		serverInSameProcess = false;
		noclipSpeed = 25;
		hackedMOTD = true;
		autoButcherDamage = 9999;
		spectatePlayerTarget = -1;
		tilePacketRate = 75;
		erasePacketRate = 75;
		liquidPacketRate = 20;
		paintPacketRate = 50;
		rainbowPaintType = 0;
		paintOnlyTile = -1;
		paintOnlyWall = 0;
		markedTilesForTilePaintBrush = new ConcurrentDictionary<Point, int>();
		drawPhotoType = 1;
		wallColors = new List<Schematics.ColorTable>();
		runningBrushThread = false;
		runningBrushMapThread = false;
		markedTilesForTileBrush = new ConcurrentDictionary<Point, Point>();
		markedTilesForEraseBrush = new ConcurrentDictionary<Point, int>();
		annoyPlayerV2 = new bool[255];
		annoyPlayerShimmer = new bool[255];
		annoyPlayer3 = new bool[255];
		markedTilesForWallBrush = new ConcurrentDictionary<Point, int>();
		brushPattern = 0;
		showNetplayText = true;
		deathParticleCosmeticId = 31;
		deathGoreCosmeticId = 1218;
		deathSoundCosmeticId = 20;
		tShockDisableTime = 0;
		hideUUID = 0;
		removeForcedMinimumZoom = false;
		updatesAtHighMsThreshold = 0;
		outsideWorldMapLight = 100;
		lightsAddedPerFrame = 0;
		logLightsAdded = false;
		randomProjsToShoot = 8;
		dustLevel = 100;
		uiRework = true;
		customUI2 = new UserInterface();
		fullBrightLevel = 0;
		fullBrightPercent = 0f;
		deathItemDropMode = 0;
		extraProjHitboxes = new List<Rectangle>();
		awaitTshockResponseTime = 0;
		worldBorderSize = 40;
		awaitingTshockPvpResponseTime = 0;
		buffForceImmune = new bool[BuffID.Count];
		trollItemId = 0;
		concurrentFillOrDels = 0;
		concurrentConversions = 0;
		persistBuffsMode = 0;
		forcedTileObjectRandom = -1;
		forcedTileObjectAlternate = -1;
		shareMyInfo = true;
		fastJoin = false;
		rejoining = false;
		tempRejoin = false;
		autoRejoin = false;
		rejoinRememberPos = false;
		loadingMap = false;
		cloakMode = -555;
		bombAmount = 25;
		bombFillerType = 0;
		targetPlayerIndex = -1;
		zenithItem = -1;
		noSoundsTime = 0;
		antiLagProjectiles = true;
		antiLagSounds = true;
		antiLagParticles = true;
		antiLagFps = true;
		disableProjLightTime = 0;
		forceBiomeSight = false;
		forceHunter = false;
		forceDangersense = false;
		alwaysPvpDeath = false;
		surfaceFrictionMode = 0;
		bouncySurfaces = false;
		asphaltSurfaces = false;
		conveyerBeltMode = 0;
		ignorePlayerHostileForLockOn = false;
		fixHitParticles = true;
		quedMapRefreshTime = 0;
		automaticMartiansWaitTime = 1;
		forceDashType = -1;
		enableTinyWorld = false;
		controlledNpcType = 0;
		controlledNpc = null;
		npcCycleRequested = false;
		lastControlledNpcIndex = -1;
		shouldStrikeControlledNpc = true;
		controlNpcAtMouse = false;
		OldMouseWorld = Vector2.Zero;
		mouseVelocity = Vector2.Zero;
		backupPlayer = new Player();
		customTextOverride = "";
		paintOverride = 0;
		paintCoatOverride = 0;
		itemTranslation = new Dictionary<short, short>
		{
			{ 5456, 5069 },
			{ 5457, 674 },
			{ 5458, 675 },
			{ 5459, 757 },
			{ 5460, 5329 }
		};
		antiChatSpam = false;
		autoExitPaste = false;
		pasteDelay = 45;
		pasteDelayCounter = 0;
		autoMobileCrash_Explosives = false;
		autoCrashDelays_Explosives = new int[255];
		autoMobileCrash_Dressers = false;
		autoCrashDelays_Dressers = new int[255];
		convertingToSchematic = false;
		alwaysDrawModifiedIndicator = false;
		infiniteBuffs = true;
		lightEveryFrame = false;
		_isLightingEveryFrame = false;
		nPCInterpolationRate = 2f;
		lastCommand = "";
		boundCommand1 = "";
		boundCommand2 = "";
		boundCommand3 = "";
		boundCommand4 = "";
		boundCommand5 = "";
		boundCommand6 = "";
		boundCommand7 = "";
		boundCommand8 = "";
		EncChatKeyString = "ZyUg_y3h";
		encryptChatWarnTime = 18000;
		encryptChatInvalidKeyWarning = false;
		encryptedChatWarningPopup = null;
		useAsyncSocket = false;
		useNewServerList = true;
		pasteBorderColor = new Color(50, 50, 50, 100);
		pasteBorderColorChange = 1;
		worldPreviewOnMap = true;
		inWorldMapPreview = false;
		fullBrightBrightness = new Vector3(0.5f, 0.5f, 0.5f);
		copiedTiles = new List<LegacyTile>();
		pixelColors = new List<Color>();
		pixelColorsInHex = new List<string>();
		tileIDToItemID = new Dictionary<int, short>();
		indexToTileID = new Dictionary<int, int>();
		nonSolidTileIDToItemID = new Dictionary<int, short>();
		wallIDToItemID = new Dictionary<int, short>();
		theKeyBinds = new List<Keys>();
		theMouseBinds = new List<string>();
		bindSetMode = new int[6];
		keyBindMaster = new Dictionary<int, Tuple<Keys, object>>();
		markedTilesForLiquidAndDryBrush = new ConcurrentDictionary<Point, int>();
		tileToItemDictionarySearch = new Dictionary<int, int>();
		wallToItemDictionarySearch = new Dictionary<int, int>();
		deathParticleCosmeticId = -1;
		deathGoreCosmeticId = -1;
		lucyMessagesMode = 0;
		lucyDeathMessageMode = 0;
		jitterAmount = 0;
		forceSoundtrackId = 0;
		autoQuickCheats = true;
		isCoolClient = true;
		fishItem = 0;
		TEditImportantFrames = new byte[87]
		{
			56, 252, 63, 189, 30, 4, 134, 32, 128, 231,
			254, 255, 255, 71, 6, 96, 243, 239, 33, 0,
			32, 120, 4, 15, 0, 130, 150, 31, 152, 250,
			255, 64, 0, 224, 248, 239, 255, 255, 127, 244,
			25, 192, 14, 32, 220, 31, 240, 23, 252, 15,
			96, 124, 152, 59, 248, 63, 240, 227, 255, 24,
			241, 111, 10, 230, 192, 255, 62, 196, 251, 223,
			177, 63, 248, 255, 255, 255, 255, 251, 225, 165,
			253, 127, 151, 3, 0, 0, 0
		};
		indexToNonSolidTileID = new Dictionary<int, int>();
		markedTilesForWallBrush = new ConcurrentDictionary<Point, int>();
		crystalKillAura = false;
		storedPlayerListPos = new Vector2((float)Main.screenWidth / 2f, (float)(Main.screenHeight / 2 - 40));
		customUI = new UserInterface();
		hexValues = (Color[])(object)new Color[339]
		{
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("583D2E"),
			Schematics.HexStringToMicrosoftColor("3D3A4E"),
			Schematics.HexStringToMicrosoftColor("493324"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("5B1E1E"),
			Schematics.HexStringToMicrosoftColor("1B1F2A"),
			Schematics.HexStringToMicrosoftColor("1F271A"),
			Schematics.HexStringToMicrosoftColor("291C24"),
			Schematics.HexStringToMicrosoftColor("4A3E0C"),
			Schematics.HexStringToMicrosoftColor("2E383B"),
			Schematics.HexStringToMicrosoftColor("4B200B"),
			Schematics.HexStringToMicrosoftColor("432525"),
			Schematics.HexStringToMicrosoftColor("0F0F0F"),
			Schematics.HexStringToMicrosoftColor("342B2D"),
			Schematics.HexStringToMicrosoftColor("583D2E"),
			Schematics.HexStringToMicrosoftColor("1B1F2A"),
			Schematics.HexStringToMicrosoftColor("1F271A"),
			Schematics.HexStringToMicrosoftColor("291C24"),
			Schematics.HexStringToMicrosoftColor("0F0F0F"),
			Schematics.HexStringToMicrosoftColor("7792FF"),
			Schematics.HexStringToMicrosoftColor("716363"),
			Schematics.HexStringToMicrosoftColor("26262B"),
			Schematics.HexStringToMicrosoftColor("352729"),
			Schematics.HexStringToMicrosoftColor("0B233E"),
			Schematics.HexStringToMicrosoftColor("153F46"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("515465"),
			Schematics.HexStringToMicrosoftColor("581717"),
			Schematics.HexStringToMicrosoftColor("1C5817"),
			Schematics.HexStringToMicrosoftColor("4E5763"),
			Schematics.HexStringToMicrosoftColor("561128"),
			Schematics.HexStringToMicrosoftColor("312F53"),
			Schematics.HexStringToMicrosoftColor("454329"),
			Schematics.HexStringToMicrosoftColor("333346"),
			Schematics.HexStringToMicrosoftColor("573B37"),
			Schematics.HexStringToMicrosoftColor("454329"),
			Schematics.HexStringToMicrosoftColor("313931"),
			Schematics.HexStringToMicrosoftColor("4E4F49"),
			Schematics.HexStringToMicrosoftColor("556667"),
			Schematics.HexStringToMicrosoftColor("34323E"),
			Schematics.HexStringToMicrosoftColor("472A2C"),
			Schematics.HexStringToMicrosoftColor("494232"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("3C3B33"),
			Schematics.HexStringToMicrosoftColor("30392F"),
			Schematics.HexStringToMicrosoftColor("474D55"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("313024"),
			Schematics.HexStringToMicrosoftColor("2B2120"),
			Schematics.HexStringToMicrosoftColor("1F2831"),
			Schematics.HexStringToMicrosoftColor("302334"),
			Schematics.HexStringToMicrosoftColor("583D2E"),
			Schematics.HexStringToMicrosoftColor("013414"),
			Schematics.HexStringToMicrosoftColor("37271A"),
			Schematics.HexStringToMicrosoftColor("27211A"),
			Schematics.HexStringToMicrosoftColor("1E5030"),
			Schematics.HexStringToMicrosoftColor("35501E"),
			Schematics.HexStringToMicrosoftColor("1E5030"),
			Schematics.HexStringToMicrosoftColor("1E5030"),
			Schematics.HexStringToMicrosoftColor("35501E"),
			Schematics.HexStringToMicrosoftColor("1E5030"),
			Schematics.HexStringToMicrosoftColor("2B2A44"),
			Schematics.HexStringToMicrosoftColor("1E4650"),
			Schematics.HexStringToMicrosoftColor("4E6987"),
			Schematics.HexStringToMicrosoftColor("34540C"),
			Schematics.HexStringToMicrosoftColor("BECCDF"),
			Schematics.HexStringToMicrosoftColor("403E50"),
			Schematics.HexStringToMicrosoftColor("414123"),
			Schematics.HexStringToMicrosoftColor("142E68"),
			Schematics.HexStringToMicrosoftColor("3D0D10"),
			Schematics.HexStringToMicrosoftColor("3F271A"),
			Schematics.HexStringToMicrosoftColor("332F60"),
			Schematics.HexStringToMicrosoftColor("403E50"),
			Schematics.HexStringToMicrosoftColor("653333"),
			Schematics.HexStringToMicrosoftColor("4D4022"),
			Schematics.HexStringToMicrosoftColor("3E2629"),
			Schematics.HexStringToMicrosoftColor("304E5D"),
			Schematics.HexStringToMicrosoftColor("363F45"),
			Schematics.HexStringToMicrosoftColor("8A4926"),
			Schematics.HexStringToMicrosoftColor("320F08"),
			Schematics.HexStringToMicrosoftColor("5F6DFF"),
			Schematics.HexStringToMicrosoftColor("6272FF"),
			Schematics.HexStringToMicrosoftColor("6576FF"),
			Schematics.HexStringToMicrosoftColor("687BFF"),
			Schematics.HexStringToMicrosoftColor("6B80FF"),
			Schematics.HexStringToMicrosoftColor("6E84FF"),
			Schematics.HexStringToMicrosoftColor("20282D"),
			Schematics.HexStringToMicrosoftColor("2C2932"),
			Schematics.HexStringToMicrosoftColor("48324D"),
			Schematics.HexStringToMicrosoftColor("4E3245"),
			Schematics.HexStringToMicrosoftColor("242D2C"),
			Schematics.HexStringToMicrosoftColor("263132"),
			Schematics.HexStringToMicrosoftColor("20282D"),
			Schematics.HexStringToMicrosoftColor("2C2932"),
			Schematics.HexStringToMicrosoftColor("48324D"),
			Schematics.HexStringToMicrosoftColor("4E3245"),
			Schematics.HexStringToMicrosoftColor("242D2C"),
			Schematics.HexStringToMicrosoftColor("263132"),
			Schematics.HexStringToMicrosoftColor("4A4DFF"),
			Schematics.HexStringToMicrosoftColor("4D51FF"),
			Schematics.HexStringToMicrosoftColor("8A4926"),
			Schematics.HexStringToMicrosoftColor("5E1911"),
			Schematics.HexStringToMicrosoftColor("7D247A"),
			Schematics.HexStringToMicrosoftColor("33231B"),
			Schematics.HexStringToMicrosoftColor("320F08"),
			Schematics.HexStringToMicrosoftColor("873A00"),
			Schematics.HexStringToMicrosoftColor("41340F"),
			Schematics.HexStringToMicrosoftColor("272A33"),
			Schematics.HexStringToMicrosoftColor("591A1B"),
			Schematics.HexStringToMicrosoftColor("7E7B73"),
			Schematics.HexStringToMicrosoftColor("083213"),
			Schematics.HexStringToMicrosoftColor("5F1518"),
			Schematics.HexStringToMicrosoftColor("111F41"),
			Schematics.HexStringToMicrosoftColor("C0AD8F"),
			Schematics.HexStringToMicrosoftColor("727283"),
			Schematics.HexStringToMicrosoftColor("887707"),
			Schematics.HexStringToMicrosoftColor("084803"),
			Schematics.HexStringToMicrosoftColor("758452"),
			Schematics.HexStringToMicrosoftColor("646672"),
			Schematics.HexStringToMicrosoftColor("1E76E2"),
			Schematics.HexStringToMicrosoftColor("5D0666"),
			Schematics.HexStringToMicrosoftColor("4028A9"),
			Schematics.HexStringToMicrosoftColor("2722B4"),
			Schematics.HexStringToMicrosoftColor("575E7D"),
			Schematics.HexStringToMicrosoftColor("060606"),
			Schematics.HexStringToMicrosoftColor("4548BA"),
			Schematics.HexStringToMicrosoftColor("823E10"),
			Schematics.HexStringToMicrosoftColor("167BA3"),
			Schematics.HexStringToMicrosoftColor("285697"),
			Schematics.HexStringToMicrosoftColor("B74B0F"),
			Schematics.HexStringToMicrosoftColor("535064"),
			Schematics.HexStringToMicrosoftColor("734144"),
			Schematics.HexStringToMicrosoftColor("776C51"),
			Schematics.HexStringToMicrosoftColor("3B4347"),
			Schematics.HexStringToMicrosoftColor("DED8CA"),
			Schematics.HexStringToMicrosoftColor("5A7069"),
			Schematics.HexStringToMicrosoftColor("3E1C57"),
			Schematics.HexStringToMicrosoftColor("748DFF"),
			Schematics.HexStringToMicrosoftColor("783B13"),
			Schematics.HexStringToMicrosoftColor("3B3B3B"),
			Schematics.HexStringToMicrosoftColor("E5DAA1"),
			Schematics.HexStringToMicrosoftColor("493B32"),
			Schematics.HexStringToMicrosoftColor("84A5FF"),
			Schematics.HexStringToMicrosoftColor("664B22"),
			Schematics.HexStringToMicrosoftColor("3E3AFF"),
			Schematics.HexStringToMicrosoftColor("FF914F"),
			Schematics.HexStringToMicrosoftColor("DD4FFF"),
			Schematics.HexStringToMicrosoftColor("F0F0F7"),
			Schematics.HexStringToMicrosoftColor("4FFF59"),
			Schematics.HexStringToMicrosoftColor("9A5331"),
			Schematics.HexStringToMicrosoftColor("6B319A"),
			Schematics.HexStringToMicrosoftColor("555976"),
			Schematics.HexStringToMicrosoftColor("319A44"),
			Schematics.HexStringToMicrosoftColor("9A314D"),
			Schematics.HexStringToMicrosoftColor("31319A"),
			Schematics.HexStringToMicrosoftColor("9A9431"),
			Schematics.HexStringToMicrosoftColor("FF4F4F"),
			Schematics.HexStringToMicrosoftColor("4F66FF"),
			Schematics.HexStringToMicrosoftColor("FAFF4F"),
			Schematics.HexStringToMicrosoftColor("464433"),
			Schematics.HexStringToMicrosoftColor("6E84FF"),
			Schematics.HexStringToMicrosoftColor("050505"),
			Schematics.HexStringToMicrosoftColor("3B2716"),
			Schematics.HexStringToMicrosoftColor("3B2716"),
			Schematics.HexStringToMicrosoftColor("A36000"),
			Schematics.HexStringToMicrosoftColor("5EA32E"),
			Schematics.HexStringToMicrosoftColor("75203B"),
			Schematics.HexStringToMicrosoftColor("140BCB"),
			Schematics.HexStringToMicrosoftColor("4A4558"),
			Schematics.HexStringToMicrosoftColor("3C1E1E"),
			Schematics.HexStringToMicrosoftColor("6F7587"),
			Schematics.HexStringToMicrosoftColor("6F7587"),
			Schematics.HexStringToMicrosoftColor("191736"),
			Schematics.HexStringToMicrosoftColor("191736"),
			Schematics.HexStringToMicrosoftColor("4A4781"),
			Schematics.HexStringToMicrosoftColor("6F7587"),
			Schematics.HexStringToMicrosoftColor("191736"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("260942"),
			Schematics.HexStringToMicrosoftColor("955033"),
			Schematics.HexStringToMicrosoftColor("523F50"),
			Schematics.HexStringToMicrosoftColor("413D4D"),
			Schematics.HexStringToMicrosoftColor("40415C"),
			Schematics.HexStringToMicrosoftColor("4C3554"),
			Schematics.HexStringToMicrosoftColor("904334"),
			Schematics.HexStringToMicrosoftColor("953030"),
			Schematics.HexStringToMicrosoftColor("6F2024"),
			Schematics.HexStringToMicrosoftColor("933037"),
			Schematics.HexStringToMicrosoftColor("614333"),
			Schematics.HexStringToMicrosoftColor("70503E"),
			Schematics.HexStringToMicrosoftColor("583D2E"),
			Schematics.HexStringToMicrosoftColor("7F5E4C"),
			Schematics.HexStringToMicrosoftColor("8F327B"),
			Schematics.HexStringToMicrosoftColor("887883"),
			Schematics.HexStringToMicrosoftColor("DB5C8F"),
			Schematics.HexStringToMicrosoftColor("714096"),
			Schematics.HexStringToMicrosoftColor("4A433C"),
			Schematics.HexStringToMicrosoftColor("3C4E3B"),
			Schematics.HexStringToMicrosoftColor("003615"),
			Schematics.HexStringToMicrosoftColor("4A6148"),
			Schematics.HexStringToMicrosoftColor("282523"),
			Schematics.HexStringToMicrosoftColor("4D3F42"),
			Schematics.HexStringToMicrosoftColor("6F0606"),
			Schematics.HexStringToMicrosoftColor("58433B"),
			Schematics.HexStringToMicrosoftColor("585750"),
			Schematics.HexStringToMicrosoftColor("474743"),
			Schematics.HexStringToMicrosoftColor("4C343C"),
			Schematics.HexStringToMicrosoftColor("59303B"),
			Schematics.HexStringToMicrosoftColor("9E6440"),
			Schematics.HexStringToMicrosoftColor("3E2D4B"),
			Schematics.HexStringToMicrosoftColor("390E0C"),
			Schematics.HexStringToMicrosoftColor("604885"),
			Schematics.HexStringToMicrosoftColor("433750"),
			Schematics.HexStringToMicrosoftColor("40251D"),
			Schematics.HexStringToMicrosoftColor("46335B"),
			Schematics.HexStringToMicrosoftColor("331204"),
			Schematics.HexStringToMicrosoftColor("393734"),
			Schematics.HexStringToMicrosoftColor("444444"),
			Schematics.HexStringToMicrosoftColor("948A4A"),
			Schematics.HexStringToMicrosoftColor("5F89BF"),
			Schematics.HexStringToMicrosoftColor("A0024B"),
			Schematics.HexStringToMicrosoftColor("6437A4"),
			Schematics.HexStringToMicrosoftColor("007565"),
			Schematics.HexStringToMicrosoftColor("6E5A4E"),
			Schematics.HexStringToMicrosoftColor("2F454B"),
			Schematics.HexStringToMicrosoftColor("5B4346"),
			Schematics.HexStringToMicrosoftColor("3C2427"),
			Schematics.HexStringToMicrosoftColor("8C4B30"),
			Schematics.HexStringToMicrosoftColor("7F312C"),
			Schematics.HexStringToMicrosoftColor("C82C12"),
			Schematics.HexStringToMicrosoftColor("185D42"),
			Schematics.HexStringToMicrosoftColor("A057EA"),
			Schematics.HexStringToMicrosoftColor("066AFF"),
			Schematics.HexStringToMicrosoftColor("687BFF"),
			Schematics.HexStringToMicrosoftColor("050505"),
			Schematics.HexStringToMicrosoftColor("050505"),
			Schematics.HexStringToMicrosoftColor("3F271A"),
			Schematics.HexStringToMicrosoftColor("666666"),
			Schematics.HexStringToMicrosoftColor("3D3A4E"),
			Schematics.HexStringToMicrosoftColor("342B2D"),
			Schematics.HexStringToMicrosoftColor("515465"),
			Schematics.HexStringToMicrosoftColor("556667"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("283832"),
			Schematics.HexStringToMicrosoftColor("313024"),
			Schematics.HexStringToMicrosoftColor("2B2120"),
			Schematics.HexStringToMicrosoftColor("1F2831"),
			Schematics.HexStringToMicrosoftColor("302334"),
			Schematics.HexStringToMicrosoftColor("583D2E"),
			Schematics.HexStringToMicrosoftColor("37271A"),
			Schematics.HexStringToMicrosoftColor("27211A"),
			Schematics.HexStringToMicrosoftColor("2B2A44"),
			Schematics.HexStringToMicrosoftColor("1E4650"),
			Schematics.HexStringToMicrosoftColor("4E6987"),
			Schematics.HexStringToMicrosoftColor("332F60"),
			Schematics.HexStringToMicrosoftColor("653333"),
			Schematics.HexStringToMicrosoftColor("3E2629"),
			Schematics.HexStringToMicrosoftColor("3B2716"),
			Schematics.HexStringToMicrosoftColor("3B2716"),
			Schematics.HexStringToMicrosoftColor("6F7587"),
			Schematics.HexStringToMicrosoftColor("191736"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("955033"),
			Schematics.HexStringToMicrosoftColor("523F50"),
			Schematics.HexStringToMicrosoftColor("413D4D"),
			Schematics.HexStringToMicrosoftColor("40415C"),
			Schematics.HexStringToMicrosoftColor("4C3554"),
			Schematics.HexStringToMicrosoftColor("904334"),
			Schematics.HexStringToMicrosoftColor("953030"),
			Schematics.HexStringToMicrosoftColor("6F2024"),
			Schematics.HexStringToMicrosoftColor("933037"),
			Schematics.HexStringToMicrosoftColor("614333"),
			Schematics.HexStringToMicrosoftColor("70503E"),
			Schematics.HexStringToMicrosoftColor("583D2E"),
			Schematics.HexStringToMicrosoftColor("7F5E4C"),
			Schematics.HexStringToMicrosoftColor("8F327B"),
			Schematics.HexStringToMicrosoftColor("887883"),
			Schematics.HexStringToMicrosoftColor("DB5C8F"),
			Schematics.HexStringToMicrosoftColor("714096"),
			Schematics.HexStringToMicrosoftColor("4A433C"),
			Schematics.HexStringToMicrosoftColor("3C4E3B"),
			Schematics.HexStringToMicrosoftColor("003615"),
			Schematics.HexStringToMicrosoftColor("4A6148"),
			Schematics.HexStringToMicrosoftColor("282523"),
			Schematics.HexStringToMicrosoftColor("4D3F42"),
			Schematics.HexStringToMicrosoftColor("6F0606"),
			Schematics.HexStringToMicrosoftColor("58433B"),
			Schematics.HexStringToMicrosoftColor("585750"),
			Schematics.HexStringToMicrosoftColor("474743"),
			Schematics.HexStringToMicrosoftColor("4C343C"),
			Schematics.HexStringToMicrosoftColor("59303B"),
			Schematics.HexStringToMicrosoftColor("9E6440"),
			Schematics.HexStringToMicrosoftColor("3E2D4B"),
			Schematics.HexStringToMicrosoftColor("390E0C"),
			Schematics.HexStringToMicrosoftColor("604885"),
			Schematics.HexStringToMicrosoftColor("433750"),
			Schematics.HexStringToMicrosoftColor("40251D"),
			Schematics.HexStringToMicrosoftColor("46335B"),
			Schematics.HexStringToMicrosoftColor("331204"),
			Schematics.HexStringToMicrosoftColor("4E6E33"),
			Schematics.HexStringToMicrosoftColor("4E6E33"),
			Schematics.HexStringToMicrosoftColor("343434"),
			Schematics.HexStringToMicrosoftColor("413FFF"),
			Schematics.HexStringToMicrosoftColor("493324"),
			Schematics.HexStringToMicrosoftColor("801A34"),
			Schematics.HexStringToMicrosoftColor("897843"),
			Schematics.HexStringToMicrosoftColor("676767"),
			Schematics.HexStringToMicrosoftColor("DB4726"),
			Schematics.HexStringToMicrosoftColor("FD3E03"),
			Schematics.HexStringToMicrosoftColor("60F802"),
			Schematics.HexStringToMicrosoftColor("694ACA"),
			Schematics.HexStringToMicrosoftColor("1DF0FF"),
			Schematics.HexStringToMicrosoftColor("FECA50"),
			Schematics.HexStringToMicrosoftColor("83FCF5"),
			Schematics.HexStringToMicrosoftColor("1F1F1F"),
			Schematics.HexStringToMicrosoftColor("1CD85E"),
			Schematics.HexStringToMicrosoftColor("9288CD"),
			Schematics.HexStringToMicrosoftColor("DFE8E9"),
			Schematics.HexStringToMicrosoftColor("FBD1F0"),
			Schematics.HexStringToMicrosoftColor("5BBAF0"),
			Schematics.HexStringToMicrosoftColor("5CF05B"),
			Schematics.HexStringToMicrosoftColor("F05B93"),
			Schematics.HexStringToMicrosoftColor("949EB8"),
			Schematics.HexStringToMicrosoftColor("4C392C"),
			Schematics.HexStringToMicrosoftColor("7D3D41"),
			Schematics.HexStringToMicrosoftColor("1E1A54"),
			Schematics.HexStringToMicrosoftColor("B2683A"),
			Schematics.HexStringToMicrosoftColor("AC9B6E")
		};
		inventoryBackUp = new Item[Main.player[Main.myPlayer].inventory.Length];
		armorAndAccessoriesBackUp = new Item[Main.player[Main.myPlayer].armor.Length];
		dyeBackUp = new Item[Main.player[Main.myPlayer].dye.Length];
		miscsBackUp = new Item[Main.player[Main.myPlayer].miscEquips.Length];
		miscsDyesBackUp = new Item[Main.player[Main.myPlayer].miscDyes.Length];
		plrInListHitBoxes = (Rectangle[])(object)new Rectangle[255];
		plrItemSlotsHitBox = (Rectangle[])(object)new Rectangle[Main.player[Main.myPlayer].inventory.Length];
		plrArmorSlotsHitBox = (Rectangle[])(object)new Rectangle[Main.player[Main.myPlayer].armor.Length];
		playerInspection = true;
		panelPageNumber = 1;
		tilePacketMaxMode = false;
		tileSafetyIterations = 450;
		customUUID = "shitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass\nshitass";
		wallIDToItemID.Add(1, 26);
		wallIDToItemID.Add(16, 30);
		wallIDToItemID.Add(4, 93);
		wallIDToItemID.Add(5, 130);
		wallIDToItemID.Add(6, 132);
		wallIDToItemID.Add(17, 135);
		wallIDToItemID.Add(18, 138);
		wallIDToItemID.Add(19, 140);
		wallIDToItemID.Add(10, 142);
		wallIDToItemID.Add(11, 144);
		wallIDToItemID.Add(12, 146);
		wallIDToItemID.Add(20, 330);
		wallIDToItemID.Add(21, 392);
		wallIDToItemID.Add(22, 417);
		wallIDToItemID.Add(23, 418);
		wallIDToItemID.Add(24, 419);
		wallIDToItemID.Add(25, 420);
		wallIDToItemID.Add(26, 421);
		wallIDToItemID.Add(27, 479);
		wallIDToItemID.Add(29, 587);
		wallIDToItemID.Add(30, 592);
		wallIDToItemID.Add(31, 595);
		wallIDToItemID.Add(32, 605);
		wallIDToItemID.Add(33, 606);
		wallIDToItemID.Add(34, 608);
		wallIDToItemID.Add(35, 610);
		wallIDToItemID.Add(36, 615);
		wallIDToItemID.Add(37, 616);
		wallIDToItemID.Add(38, 617);
		wallIDToItemID.Add(39, 618);
		wallIDToItemID.Add(41, 622);
		wallIDToItemID.Add(42, 623);
		wallIDToItemID.Add(43, 624);
		wallIDToItemID.Add(44, 663);
		wallIDToItemID.Add(45, 720);
		wallIDToItemID.Add(46, 721);
		wallIDToItemID.Add(47, 722);
		wallIDToItemID.Add(66, 745);
		wallIDToItemID.Add(67, 746);
		wallIDToItemID.Add(68, 747);
		wallIDToItemID.Add(72, 750);
		wallIDToItemID.Add(73, 752);
		wallIDToItemID.Add(74, 764);
		wallIDToItemID.Add(75, 768);
		wallIDToItemID.Add(76, 769);
		wallIDToItemID.Add(77, 770);
		wallIDToItemID.Add(82, 825);
		wallIDToItemID.Add(84, 884);
		wallIDToItemID.Add(85, 927);
		wallIDToItemID.Add(112, 1102);
		wallIDToItemID.Add(108, 1126);
		wallIDToItemID.Add(88, 1267);
		wallIDToItemID.Add(89, 1268);
		wallIDToItemID.Add(90, 1269);
		wallIDToItemID.Add(91, 1270);
		wallIDToItemID.Add(92, 1271);
		wallIDToItemID.Add(93, 1272);
		wallIDToItemID.Add(100, 1378);
		wallIDToItemID.Add(101, 1379);
		wallIDToItemID.Add(102, 1380);
		wallIDToItemID.Add(103, 1381);
		wallIDToItemID.Add(104, 1382);
		wallIDToItemID.Add(105, 1383);
		wallIDToItemID.Add(106, 1447);
		wallIDToItemID.Add(107, 1448);
		wallIDToItemID.Add(244, 1449);
		wallIDToItemID.Add(109, 1590);
		wallIDToItemID.Add(110, 1592);
		wallIDToItemID.Add(111, 1594);
		wallIDToItemID.Add(78, 1723);
		wallIDToItemID.Add(113, 1726);
		wallIDToItemID.Add(114, 1728);
		wallIDToItemID.Add(115, 1730);
		wallIDToItemID.Add(116, 1948);
		wallIDToItemID.Add(117, 1949);
		wallIDToItemID.Add(118, 1950);
		wallIDToItemID.Add(119, 1951);
		wallIDToItemID.Add(120, 1952);
		wallIDToItemID.Add(121, 1953);
		wallIDToItemID.Add(122, 1954);
		wallIDToItemID.Add(123, 1955);
		wallIDToItemID.Add(124, 1956);
		wallIDToItemID.Add(125, 1957);
		wallIDToItemID.Add(126, 2008);
		wallIDToItemID.Add(127, 2009);
		wallIDToItemID.Add(128, 2010);
		wallIDToItemID.Add(129, 2011);
		wallIDToItemID.Add(130, 2012);
		wallIDToItemID.Add(131, 2013);
		wallIDToItemID.Add(132, 2014);
		wallIDToItemID.Add(133, 2158);
		wallIDToItemID.Add(134, 2159);
		wallIDToItemID.Add(135, 2160);
		wallIDToItemID.Add(136, 2169);
		wallIDToItemID.Add(137, 2170);
		wallIDToItemID.Add(138, 2210);
		wallIDToItemID.Add(139, 2211);
		wallIDToItemID.Add(140, 2212);
		wallIDToItemID.Add(141, 2213);
		wallIDToItemID.Add(142, 2263);
		wallIDToItemID.Add(143, 2264);
		wallIDToItemID.Add(144, 2271);
		wallIDToItemID.Add(145, 2333);
		wallIDToItemID.Add(146, 2432);
		wallIDToItemID.Add(147, 2433);
		wallIDToItemID.Add(148, 2434);
		wallIDToItemID.Add(149, 2505);
		wallIDToItemID.Add(151, 2506);
		wallIDToItemID.Add(150, 2507);
		wallIDToItemID.Add(152, 2508);
		wallIDToItemID.Add(153, 2677);
		wallIDToItemID.Add(157, 2678);
		wallIDToItemID.Add(154, 2679);
		wallIDToItemID.Add(158, 2680);
		wallIDToItemID.Add(155, 2681);
		wallIDToItemID.Add(159, 2682);
		wallIDToItemID.Add(156, 2683);
		wallIDToItemID.Add(160, 2684);
		wallIDToItemID.Add(164, 2685);
		wallIDToItemID.Add(161, 2686);
		wallIDToItemID.Add(165, 2687);
		wallIDToItemID.Add(162, 2688);
		wallIDToItemID.Add(166, 2689);
		wallIDToItemID.Add(163, 2690);
		wallIDToItemID.Add(167, 2691);
		wallIDToItemID.Add(168, 2696);
		wallIDToItemID.Add(169, 2698);
		wallIDToItemID.Add(172, 2788);
		wallIDToItemID.Add(173, 2789);
		wallIDToItemID.Add(174, 2790);
		wallIDToItemID.Add(175, 2791);
		wallIDToItemID.Add(176, 2861);
		wallIDToItemID.Add(177, 3067);
		wallIDToItemID.Add(183, 3082);
		wallIDToItemID.Add(179, 3083);
		wallIDToItemID.Add(184, 3088);
		wallIDToItemID.Add(181, 3089);
		wallIDToItemID.Add(182, 3101);
		wallIDToItemID.Add(186, 3238);
		wallIDToItemID.Add(275, 3273);
		wallIDToItemID.Add(304, 3340);
		wallIDToItemID.Add(305, 3341);
		wallIDToItemID.Add(306, 3342);
		wallIDToItemID.Add(307, 3343);
		wallIDToItemID.Add(308, 3344);
		wallIDToItemID.Add(309, 3345);
		wallIDToItemID.Add(310, 3346);
		wallIDToItemID.Add(311, 3348);
		wallIDToItemID.Add(224, 3472);
		wallIDToItemID.Add(60, 3584);
		wallIDToItemID.Add(225, 3751);
		wallIDToItemID.Add(226, 3752);
		wallIDToItemID.Add(227, 3753);
		wallIDToItemID.Add(228, 3760);
		wallIDToItemID.Add(229, 3761);
		wallIDToItemID.Add(230, 3762);
		wallIDToItemID.Add(231, 3952);
		wallIDToItemID.Add(232, 3954);
		wallIDToItemID.Add(233, 3956);
		wallIDToItemID.Add(234, 4052);
		wallIDToItemID.Add(235, 4053);
		wallIDToItemID.Add(236, 4140);
		wallIDToItemID.Add(237, 4233);
		wallIDToItemID.Add(238, 4234);
		wallIDToItemID.Add(239, 4235);
		wallIDToItemID.Add(240, 4236);
		wallIDToItemID.Add(241, 4260);
		wallIDToItemID.Add(242, 4279);
		wallIDToItemID.Add(243, 4280);
		wallIDToItemID.Add(245, 4424);
		wallIDToItemID.Add(246, 4486);
		wallIDToItemID.Add(247, 4487);
		wallIDToItemID.Add(248, 4488);
		wallIDToItemID.Add(249, 4489);
		wallIDToItemID.Add(250, 4490);
		wallIDToItemID.Add(251, 4491);
		wallIDToItemID.Add(252, 4492);
		wallIDToItemID.Add(253, 4493);
		wallIDToItemID.Add(254, 4494);
		wallIDToItemID.Add(255, 4495);
		wallIDToItemID.Add(256, 4496);
		wallIDToItemID.Add(257, 4497);
		wallIDToItemID.Add(258, 4498);
		wallIDToItemID.Add(259, 4499);
		wallIDToItemID.Add(260, 4500);
		wallIDToItemID.Add(261, 4501);
		wallIDToItemID.Add(262, 4502);
		wallIDToItemID.Add(263, 4503);
		wallIDToItemID.Add(264, 4504);
		wallIDToItemID.Add(265, 4505);
		wallIDToItemID.Add(266, 4506);
		wallIDToItemID.Add(267, 4507);
		wallIDToItemID.Add(268, 4508);
		wallIDToItemID.Add(269, 4509);
		wallIDToItemID.Add(270, 4510);
		wallIDToItemID.Add(271, 4511);
		wallIDToItemID.Add(274, 4512);
		wallIDToItemID.Add(276, 4513);
		wallIDToItemID.Add(277, 4514);
		wallIDToItemID.Add(278, 4515);
		wallIDToItemID.Add(279, 4516);
		wallIDToItemID.Add(280, 4517);
		wallIDToItemID.Add(281, 4518);
		wallIDToItemID.Add(282, 4519);
		wallIDToItemID.Add(283, 4520);
		wallIDToItemID.Add(284, 4521);
		wallIDToItemID.Add(285, 4522);
		wallIDToItemID.Add(286, 4523);
		wallIDToItemID.Add(287, 4524);
		wallIDToItemID.Add(288, 4525);
		wallIDToItemID.Add(289, 4526);
		wallIDToItemID.Add(290, 4527);
		wallIDToItemID.Add(291, 4528);
		wallIDToItemID.Add(292, 4529);
		wallIDToItemID.Add(293, 4530);
		wallIDToItemID.Add(294, 4531);
		wallIDToItemID.Add(295, 4532);
		wallIDToItemID.Add(296, 4533);
		wallIDToItemID.Add(297, 4534);
		wallIDToItemID.Add(298, 4535);
		wallIDToItemID.Add(299, 4536);
		wallIDToItemID.Add(300, 4537);
		wallIDToItemID.Add(301, 4538);
		wallIDToItemID.Add(302, 4539);
		wallIDToItemID.Add(303, 4540);
		wallIDToItemID.Add(313, 4548);
		wallIDToItemID.Add(312, 4565);
		wallIDToItemID.Add(314, 4647);
		wallIDToItemID.Add(315, 4667);
		wallIDToItemID.Add(316, 5216);
		wallIDToItemID.Add(317, 5217);
		wallIDToItemID.Add(318, 5291);
		wallIDToItemID.Add(319, 5307);
		wallIDToItemID.Add(62, 5363);
		wallIDToItemID.Add(7, 5365);
		wallIDToItemID.Add(94, 5366);
		wallIDToItemID.Add(95, 5367);
		wallIDToItemID.Add(9, 5368);
		wallIDToItemID.Add(96, 5369);
		wallIDToItemID.Add(97, 5370);
		wallIDToItemID.Add(8, 5371);
		wallIDToItemID.Add(98, 5372);
		wallIDToItemID.Add(99, 5373);
		wallIDToItemID.Add(187, 5374);
		wallIDToItemID.Add(216, 5375);
		wallIDToItemID.Add(87, 5376);
		wallIDToItemID.Add(320, 5396);
		wallIDToItemID.Add(321, 5397);
		wallIDToItemID.Add(322, 5399);
		wallIDToItemID.Add(323, 5409);
		wallIDToItemID.Add(324, 5410);
		wallIDToItemID.Add(325, 5411);
		wallIDToItemID.Add(326, 5412);
		wallIDToItemID.Add(327, 5413);
		wallIDToItemID.Add(328, 5414);
		wallIDToItemID.Add(329, 5415);
		wallIDToItemID.Add(330, 5416);
		wallIDToItemID.Add(331, 5418);
		wallIDToItemID.Add(332, 5420);
		wallIDToItemID.Add(333, 5422);
		wallIDToItemID.Add(334, 5424);
		wallIDToItemID.Add(335, 5426);
		wallIDToItemID.Add(336, 5428);
		wallIDToItemID.Add(339, 5430);
		wallIDToItemID.Add(340, 5432);
		wallIDToItemID.Add(337, 5434);
		wallIDToItemID.Add(338, 5436);
		wallIDToItemID.Add(341, 5445);
		wallIDToItemID.Add(342, 5446);
		wallIDToItemID.Add(343, 5447);
		wallIDToItemID.Add(344, 5448);
		wallIDToItemID.Add(345, 5449);
		wallIDToItemID.Add(346, 5450);
		wallIDToItemID.Add(196, 4521);
		wallIDToItemID.Add(197, 4522);
		wallIDToItemID.Add(198, 4523);
		wallIDToItemID.Add(199, 4524);
		wallIDToItemID.Add(55, 4497);
		wallIDToItemID.Add(56, 4500);
		wallIDToItemID.Add(206, 4529);
		wallIDToItemID.Add(208, 4508);
		wallIDToItemID.Add(211, 4536);
		wallIDToItemID.Add(14, 330);
		wallIDToItemID.Add(15, 4487);
		wallIDToItemID.Add(2, 4521);
		wallIDToItemID.Add(63, 745);
		wallIDToItemID.Add(64, 746);
		wallIDToItemID.Add(83, 4509);
		nonSolidTileIDToItemID.Add(214, 85);
		nonSolidTileIDToItemID.Add(51, 150);
		nonSolidTileIDToItemID.Add(124, 480);
		nonSolidTileIDToItemID.Add(130, 511);
		nonSolidTileIDToItemID.Add(150, 604);
		nonSolidTileIDToItemID.Add(248, 1589);
		nonSolidTileIDToItemID.Add(249, 1591);
		nonSolidTileIDToItemID.Add(336, 2701);
		nonSolidTileIDToItemID.Add(340, 2751);
		nonSolidTileIDToItemID.Add(341, 2752);
		nonSolidTileIDToItemID.Add(342, 2753);
		nonSolidTileIDToItemID.Add(343, 2754);
		nonSolidTileIDToItemID.Add(344, 2755);
		nonSolidTileIDToItemID.Add(351, 2868);
		nonSolidTileIDToItemID.Add(213, 965);
		nonSolidTileIDToItemID.Add(353, 2996);
		nonSolidTileIDToItemID.Add(365, 3077);
		nonSolidTileIDToItemID.Add(366, 3078);
		nonSolidTileIDToItemID.Add(379, 3214);
		nonSolidTileIDToItemID.Add(449, 3739);
		nonSolidTileIDToItemID.Add(450, 3740);
		nonSolidTileIDToItemID.Add(451, 3741);
		nonSolidTileIDToItemID.Add(561, 4554);
		nonSolidTileIDToItemID.Add(574, 4717);
		nonSolidTileIDToItemID.Add(575, 4718);
		nonSolidTileIDToItemID.Add(576, 4719);
		nonSolidTileIDToItemID.Add(577, 4720);
		nonSolidTileIDToItemID.Add(578, 4721);
		indexToNonSolidTileID.Add(313, 51);
		indexToNonSolidTileID.Add(314, 124);
		indexToNonSolidTileID.Add(315, 150);
		indexToNonSolidTileID.Add(316, 213);
		indexToNonSolidTileID.Add(317, 214);
		indexToNonSolidTileID.Add(318, 248);
		indexToNonSolidTileID.Add(319, 336);
		indexToNonSolidTileID.Add(320, 340);
		indexToNonSolidTileID.Add(321, 341);
		indexToNonSolidTileID.Add(322, 342);
		indexToNonSolidTileID.Add(323, 343);
		indexToNonSolidTileID.Add(324, 344);
		indexToNonSolidTileID.Add(325, 351);
		indexToNonSolidTileID.Add(326, 353);
		indexToNonSolidTileID.Add(327, 365);
		indexToNonSolidTileID.Add(328, 366);
		indexToNonSolidTileID.Add(329, 379);
		indexToNonSolidTileID.Add(330, 449);
		indexToNonSolidTileID.Add(331, 450);
		indexToNonSolidTileID.Add(332, 451);
		indexToNonSolidTileID.Add(333, 561);
		indexToNonSolidTileID.Add(334, 574);
		indexToNonSolidTileID.Add(335, 575);
		indexToNonSolidTileID.Add(336, 576);
		indexToNonSolidTileID.Add(337, 577);
		indexToNonSolidTileID.Add(338, 578);
		indexToTileID.Add(0, 0);
		indexToTileID.Add(1, 1);
		indexToTileID.Add(2, 4);
		indexToTileID.Add(3, 6);
		indexToTileID.Add(4, 7);
		indexToTileID.Add(5, 8);
		indexToTileID.Add(6, 9);
		indexToTileID.Add(7, 22);
		indexToTileID.Add(8, 25);
		indexToTileID.Add(9, 30);
		indexToTileID.Add(10, 37);
		indexToTileID.Add(11, 39);
		indexToTileID.Add(12, 40);
		indexToTileID.Add(13, 41);
		indexToTileID.Add(14, 43);
		indexToTileID.Add(15, 44);
		indexToTileID.Add(16, 56);
		indexToTileID.Add(17, 57);
		indexToTileID.Add(18, 58);
		indexToTileID.Add(19, 59);
		indexToTileID.Add(20, 67);
		indexToTileID.Add(21, 75);
		indexToTileID.Add(22, 81);
		indexToTileID.Add(23, 107);
		indexToTileID.Add(24, 108);
		indexToTileID.Add(25, 111);
		indexToTileID.Add(26, 117);
		indexToTileID.Add(27, 118);
		indexToTileID.Add(28, 119);
		indexToTileID.Add(29, 124);
		indexToTileID.Add(30, 131);
		indexToTileID.Add(31, 140);
		indexToTileID.Add(32, 145);
		indexToTileID.Add(33, 146);
		indexToTileID.Add(34, 147);
		indexToTileID.Add(35, 152);
		indexToTileID.Add(36, 153);
		indexToTileID.Add(37, 154);
		indexToTileID.Add(38, 155);
		indexToTileID.Add(39, 156);
		indexToTileID.Add(40, 157);
		indexToTileID.Add(41, 158);
		indexToTileID.Add(42, 159);
		indexToTileID.Add(43, 160);
		indexToTileID.Add(44, 161);
		indexToTileID.Add(45, 162);
		indexToTileID.Add(46, 163);
		indexToTileID.Add(47, 164);
		indexToTileID.Add(48, 166);
		indexToTileID.Add(49, 167);
		indexToTileID.Add(50, 168);
		indexToTileID.Add(51, 169);
		indexToTileID.Add(52, 174);
		indexToTileID.Add(53, 179);
		indexToTileID.Add(54, 180);
		indexToTileID.Add(55, 181);
		indexToTileID.Add(56, 182);
		indexToTileID.Add(57, 183);
		indexToTileID.Add(58, 188);
		indexToTileID.Add(59, 189);
		indexToTileID.Add(60, 190);
		indexToTileID.Add(61, 191);
		indexToTileID.Add(62, 192);
		indexToTileID.Add(63, 193);
		indexToTileID.Add(64, 194);
		indexToTileID.Add(65, 195);
		indexToTileID.Add(66, 196);
		indexToTileID.Add(67, 197);
		indexToTileID.Add(68, 198);
		indexToTileID.Add(69, 202);
		indexToTileID.Add(70, 203);
		indexToTileID.Add(71, 204);
		indexToTileID.Add(72, 206);
		indexToTileID.Add(73, 208);
		indexToTileID.Add(74, 211);
		indexToTileID.Add(75, 213);
		indexToTileID.Add(76, 214);
		indexToTileID.Add(77, 221);
		indexToTileID.Add(78, 222);
		indexToTileID.Add(79, 223);
		indexToTileID.Add(80, 225);
		indexToTileID.Add(81, 226);
		indexToTileID.Add(82, 229);
		indexToTileID.Add(83, 230);
		indexToTileID.Add(84, 232);
		indexToTileID.Add(85, 248);
		indexToTileID.Add(86, 249);
		indexToTileID.Add(87, 250);
		indexToTileID.Add(88, 252);
		indexToTileID.Add(89, 253);
		indexToTileID.Add(90, 262);
		indexToTileID.Add(91, 263);
		indexToTileID.Add(92, 264);
		indexToTileID.Add(93, 265);
		indexToTileID.Add(94, 266);
		indexToTileID.Add(95, 267);
		indexToTileID.Add(96, 268);
		indexToTileID.Add(97, 272);
		indexToTileID.Add(98, 284);
		indexToTileID.Add(99, 311);
		indexToTileID.Add(100, 314);
		indexToTileID.Add(101, 315);
		indexToTileID.Add(102, 321);
		indexToTileID.Add(103, 322);
		indexToTileID.Add(104, 325);
		indexToTileID.Add(105, 326);
		indexToTileID.Add(106, 327);
		indexToTileID.Add(107, 328);
		indexToTileID.Add(108, 329);
		indexToTileID.Add(109, 336);
		indexToTileID.Add(110, 340);
		indexToTileID.Add(111, 341);
		indexToTileID.Add(112, 342);
		indexToTileID.Add(113, 343);
		indexToTileID.Add(114, 344);
		indexToTileID.Add(115, 345);
		indexToTileID.Add(116, 346);
		indexToTileID.Add(117, 348);
		indexToTileID.Add(118, 357);
		indexToTileID.Add(119, 365);
		indexToTileID.Add(120, 366);
		indexToTileID.Add(121, 367);
		indexToTileID.Add(122, 368);
		indexToTileID.Add(123, 369);
		indexToTileID.Add(124, 370);
		indexToTileID.Add(125, 371);
		indexToTileID.Add(126, 379);
		indexToTileID.Add(127, 381);
		indexToTileID.Add(128, 383);
		indexToTileID.Add(129, 384);
		indexToTileID.Add(130, 385);
		indexToTileID.Add(131, 404);
		indexToTileID.Add(132, 407);
		indexToTileID.Add(133, 408);
		indexToTileID.Add(134, 409);
		indexToTileID.Add(135, 415);
		indexToTileID.Add(136, 416);
		indexToTileID.Add(137, 417);
		indexToTileID.Add(138, 418);
		indexToTileID.Add(139, 420);
		indexToTileID.Add(140, 421);
		indexToTileID.Add(141, 422);
		indexToTileID.Add(142, 423);
		indexToTileID.Add(143, 426);
		indexToTileID.Add(144, 427);
		indexToTileID.Add(145, 429);
		indexToTileID.Add(146, 430);
		indexToTileID.Add(147, 431);
		indexToTileID.Add(148, 432);
		indexToTileID.Add(149, 433);
		indexToTileID.Add(150, 434);
		indexToTileID.Add(151, 435);
		indexToTileID.Add(152, 436);
		indexToTileID.Add(153, 437);
		indexToTileID.Add(154, 438);
		indexToTileID.Add(155, 439);
		indexToTileID.Add(156, 444);
		indexToTileID.Add(157, 445);
		indexToTileID.Add(158, 449);
		indexToTileID.Add(159, 450);
		indexToTileID.Add(160, 451);
		indexToTileID.Add(161, 459);
		indexToTileID.Add(162, 460);
		indexToTileID.Add(163, 472);
		indexToTileID.Add(164, 473);
		indexToTileID.Add(165, 474);
		indexToTileID.Add(166, 478);
		indexToTileID.Add(167, 495);
		indexToTileID.Add(168, 496);
		indexToTileID.Add(169, 500);
		indexToTileID.Add(170, 501);
		indexToTileID.Add(171, 502);
		indexToTileID.Add(172, 503);
		indexToTileID.Add(173, 507);
		indexToTileID.Add(174, 508);
		indexToTileID.Add(175, 512);
		indexToTileID.Add(176, 513);
		indexToTileID.Add(177, 514);
		indexToTileID.Add(178, 515);
		indexToTileID.Add(179, 516);
		indexToTileID.Add(180, 517);
		indexToTileID.Add(181, 534);
		indexToTileID.Add(182, 535);
		indexToTileID.Add(183, 536);
		indexToTileID.Add(184, 537);
		indexToTileID.Add(185, 539);
		indexToTileID.Add(186, 540);
		indexToTileID.Add(187, 546);
		indexToTileID.Add(188, 561);
		indexToTileID.Add(189, 562);
		indexToTileID.Add(190, 563);
		indexToTileID.Add(191, 566);
		indexToTileID.Add(192, 571);
		indexToTileID.Add(193, 574);
		indexToTileID.Add(194, 575);
		indexToTileID.Add(195, 576);
		indexToTileID.Add(196, 577);
		indexToTileID.Add(197, 578);
		indexToTileID.Add(198, 618);
	}

	private static bool GetMapCoordinatesMod(int PinX, int PinY, out Point result)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.mapFullscreen)
		{
			result = new Point(-1, -1);
			return false;
		}
		_ = Main.maxTilesX / Main.textureMaxWidth;
		_ = Main.maxTilesY / Main.textureMaxHeight;
		float num = 10f;
		float num2 = 10f;
		float num3 = Main.maxTilesX - 10;
		float num4 = Main.maxTilesY - 10;
		float mapFullscreenScale = Main.mapFullscreenScale;
		float num5 = (float)Main.screenWidth / (float)Main.maxTilesX * 0.8f;
		if (Main.mapFullscreenScale < num5)
		{
			Main.mapFullscreenScale = num5;
		}
		if (Main.mapFullscreenScale > 16f)
		{
			Main.mapFullscreenScale = 16f;
		}
		mapFullscreenScale = Main.mapFullscreenScale;
		if (Main.mapFullscreenPos.X < num)
		{
			Main.mapFullscreenPos.X = num;
		}
		if (Main.mapFullscreenPos.X > num3)
		{
			Main.mapFullscreenPos.X = num3;
		}
		if (Main.mapFullscreenPos.Y < num2)
		{
			Main.mapFullscreenPos.Y = num2;
		}
		if (Main.mapFullscreenPos.Y > num4)
		{
			Main.mapFullscreenPos.Y = num4;
		}
		float x = Main.mapFullscreenPos.X;
		float y = Main.mapFullscreenPos.Y;
		float num6 = x * mapFullscreenScale;
		y *= mapFullscreenScale;
		float num7 = 0f - num6 + (float)(Main.screenWidth / 2);
		float num8 = 0f - y + (float)(Main.screenHeight / 2);
		num7 += num * mapFullscreenScale;
		num8 += num2 * mapFullscreenScale;
		float num9 = Main.maxTilesX / 840;
		num9 *= Main.mapFullscreenScale;
		float num10 = num7;
		float num11 = num8;
		float num12 = TextureAssets.Map.Width();
		float num13 = TextureAssets.Map.Height();
		if (Main.maxTilesX == 8400)
		{
			num9 *= 0.999f;
			num10 -= 40.6f * num9;
			num11 = num8 - 5f * num9;
			num12 -= 8.045f;
			num12 *= num9;
			num13 += 0.12f;
			num13 *= num9;
			if ((double)num9 < 1.2)
			{
				num13++;
			}
		}
		else if (Main.maxTilesX == 6400)
		{
			num9 *= 1.09f;
			num10 -= 38.8f * num9;
			num11 = num8 - 3.85f * num9;
			num12 -= 13.6f;
			num12 *= num9;
			num13 -= 6.92f;
			num13 *= num9;
			if ((double)num9 < 1.2)
			{
				num13 += 2f;
			}
		}
		else if (Main.maxTilesX == 6300)
		{
			num9 *= 1.09f;
			num10 -= 39.8f * num9;
			num11 = num8 - 4.08f * num9;
			num12 -= 26.69f;
			num12 *= num9;
			num13 -= 6.92f;
			num13 *= num9;
			if ((double)num9 < 1.2)
			{
				num13 += 2f;
			}
		}
		else if (Main.maxTilesX == 4200)
		{
			num9 *= 0.998f;
			num10 -= 37.3f * num9;
			num11 -= 1.7f * num9;
			num12 -= 16f;
			num12 *= num9;
			num13 -= 8.31f;
			num13 *= num9;
		}
		int num14 = (int)((0f - num7 + (float)PinX) / mapFullscreenScale + num);
		int num15 = (int)((0f - num8 + (float)PinY) / mapFullscreenScale + num2);
		bool flag = false;
		if ((float)num14 < num)
		{
			flag = true;
		}
		if ((float)num14 >= num3)
		{
			flag = true;
		}
		if ((float)num15 < num2)
		{
			flag = true;
		}
		if ((float)num15 >= num4)
		{
			flag = true;
		}
		if (!flag)
		{
			result = new Point(num14, num15);
			return true;
		}
		result = new Point(-1, -1);
		return false;
	}

	public static void ButcherMobs(int damage)
	{
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		if (damage <= 0)
		{
			damage = (autoButcher ? autoButcherDamage : 5000);
		}
		if (Main.netMode == 0)
		{
			damage = int.MaxValue;
		}
		for (int i = 0; i < Main.npc.Length; i++)
		{
			NPC nPC = Main.npc[i];
			if (nPC.active && nPC.type != 488 && !nPC.townNPC)
			{
				nPC.StrikeNPCNoInteraction(damage, 10f, -nPC.direction, crit: false, noEffect: false, fromNet: false, exact: true);
				if (Main.netMode != 0)
				{
					Instance.FalsePlayerPacket(new Vector2(nPC.position.X, nPC.position.Y));
					NetMessage.SendData(28, -1, -1, null, i, damage, 10f, 0f - (float)nPC.direction);
					NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
				}
				num = ((!autoButcher) ? (num + 1) : 0);
			}
		}
		if (!autoButcher)
		{
			Main.NewText("You've butchered " + num + " npc's", zazasFavoriteColor);
		}
	}

	public static async void RevealTheMap()
	{
		_ = 3;
		try
		{
			if (Main.gameMenu)
			{
				return;
			}
			if (Main.netMode == 1)
			{
				revealMapCooldown = true;
				if (Instance.DetermineUnloadedSections > 0)
				{
					Main.NewText("Retrieving Tile Data.", zazasFavoriteColor);
					Main.NewText("Speeds vary based off connection speed and on how many tiles have already been loaded.", zazasFavoriteColor);
					Main.NewText("Map chunk data retrieval may require prior login depending on the server protection levels.", zazasFavoriteColor);
					while (true)
					{
						for (int i = 0; i < 2; i++)
						{
							for (int x = 0; x < Main.maxSectionsX; x++)
							{
								for (int y = 0; y < Main.maxSectionsY; y++)
								{
									if (!Main.sectionManager.SectionLoaded(x, y))
									{
										if (stopMapReveal || Main.gameMenu)
										{
											stopMapReveal = false;
											revealMapCooldown = false;
											Main.NewText("Canceled retrieving tile data.", zazasFavoriteColor);
											return;
										}
										LoadChunk(x, y);
										if (freeCamDebug)
										{
											Main.NewText(string.Format("Chunk Load attempted at SectionX: {0}, SectionY: {1}", new object[2] { x, y }), zazasFavoriteColor);
										}
										await Task.Delay(50);
									}
								}
							}
							await Task.Delay(250);
						}
						int waitTime = 0;
						while (Instance.DetermineUnloadedSections > 0)
						{
							if (stopMapReveal || Main.gameMenu)
							{
								stopMapReveal = false;
								revealMapCooldown = false;
								Main.NewText("Canceled retrieving tile data.", zazasFavoriteColor);
								return;
							}
							await Task.Delay(1);
							waitTime++;
							if (waitTime > 3000)
							{
								goto IL_02fe;
							}
						}
						break;
						IL_02fe:
						MapRefresh();
						Main.refreshMap = true;
					}
					UpdateAndRefreshMap();
				}
				else
				{
					Main.NewText("Refreshing Map.", zazasFavoriteColor);
					UpdateAndRefreshMap();
				}
				revealMapCooldown = false;
			}
			else
			{
				await Task.Run(delegate
				{
					revealMapCooldown = true;
					UpdateAndRefreshMap();
					revealMapCooldown = false;
				});
			}
		}
		catch (Exception ex)
		{
			Main.NewText($"Reveal Map error!\n{ex}", errorColor);
			LogException(ex);
		}
	}

	public static async void RevealMapWithPositionPacket()
	{
		_ = 3;
		try
		{
			if (Main.gameMenu)
			{
				return;
			}
			if (Main.netMode == 1)
			{
				revealMapCooldown = true;
				if (Instance.DetermineUnloadedSections > 0)
				{
					Main.NewText("Retrieving Tile Data.", zazasFavoriteColor);
					Main.NewText("Speeds vary based off connection speed and on how many tiles have already been loaded.", zazasFavoriteColor);
					Main.NewText("Map chunk data retrieval may require prior login depending on the server protection levels.", zazasFavoriteColor);
					while (true)
					{
						for (int i = 0; i < 2; i++)
						{
							for (int x = 0; x < Main.maxSectionsX; x++)
							{
								for (int y = 0; y < Main.maxSectionsY; y++)
								{
									if (!Main.sectionManager.SectionLoaded(x, y))
									{
										if (stopMapReveal || Main.gameMenu)
										{
											stopMapReveal = false;
											revealMapCooldown = false;
											Main.NewText("Canceled retrieving tile data.", zazasFavoriteColor);
											return;
										}
										FakeNetMessage13(new Vector2(((float)x * 200f + 100f) * 16f, ((float)y * 150f + 75f) * 16f), 58);
										if (freeCamDebug)
										{
											Main.NewText(string.Format("Chunk Load attempted at SectionX: {0}, SectionY: {1}", new object[2] { x, y }), zazasFavoriteColor);
										}
										await Task.Delay(25);
									}
								}
							}
							await Task.Delay(250);
						}
						int waitTime = 0;
						while (Instance.DetermineUnloadedSections > 0)
						{
							if (stopMapReveal || Main.gameMenu)
							{
								stopMapReveal = false;
								revealMapCooldown = false;
								Main.NewText("Canceled retrieving tile data.", zazasFavoriteColor);
								return;
							}
							await Task.Delay(1);
							waitTime++;
							if (waitTime > 3000)
							{
								goto IL_032b;
							}
						}
						break;
						IL_032b:
						MapRefresh();
						Main.refreshMap = true;
					}
					NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
					UpdateAndRefreshMap();
				}
				else
				{
					Main.NewText("Refreshing Map.", zazasFavoriteColor);
					UpdateAndRefreshMap();
				}
				revealMapCooldown = false;
			}
			await Task.Run(delegate
			{
				revealMapCooldown = true;
				UpdateAndRefreshMap();
				revealMapCooldown = false;
			});
		}
		catch (Exception ex)
		{
			Main.NewText($"Reveal Map error!\n{ex}", errorColor);
			LogException(ex);
		}
	}

	public static async void RevealTheMapWithPacket8()
	{
		await Task.Run(delegate
		{
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				_ = Main.LocalPlayer;
				if (!Main.gameMenu)
				{
					if (Main.netMode == 1)
					{
						revealMapCooldown = true;
						if (Instance.DetermineUnloadedSections > 0)
						{
							Main.NewText("Retrieving Tile Data with Packet 8.", zazasFavoriteColor);
							Main.NewText("Speeds vary based off connection speed and on how many tiles have already been loaded.", zazasFavoriteColor);
							Main.NewText("Map chunk data retrieval with this method only works on vanilla servers.", zazasFavoriteColor);
							for (int i = 40; i < Main.maxTilesX; i += 200)
							{
								for (int j = 40; j < Main.maxTilesY; j += 150)
								{
									Main.player[Main.myPlayer].SpawnX = i;
									Main.player[Main.myPlayer].SpawnY = j;
									NetMessage.SendData(8, -1, -1, null, Main.player[Main.myPlayer].SpawnX, Main.player[Main.myPlayer].SpawnY);
									Thread.Sleep(1);
								}
							}
							while (Instance.DetermineUnloadedSections > 0)
							{
								if (!revealMapCooldown)
								{
									return;
								}
								Thread.Sleep(1);
							}
							UpdateAndRefreshMap();
						}
						else
						{
							Main.NewText("Refreshing Map.", zazasFavoriteColor);
							UpdateAndRefreshMap();
						}
						revealMapCooldown = false;
					}
					else
					{
						revealMapCooldown = true;
						UpdateAndRefreshMap();
						revealMapCooldown = false;
					}
				}
			}
			catch (Exception ex)
			{
				Main.NewText("Error: " + ex, errorColor);
			}
		});
	}

	public async void RevealTheMapWithGolfBall()
	{
		await Task.Run(delegate
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				_ = Main.LocalPlayer;
				if (!Main.gameMenu)
				{
					if (Main.netMode == 1)
					{
						revealMapCooldown = true;
						Projectile projectile = Main.projectile[Projectile.NewProjectile(Main.LocalPlayer.GetProjectileSource_Misc(721), new Vector2(40f, 40f), Vector2.Zero, 1020, 0, 0f, Main.myPlayer)];
						NetMessage.SendData(27, -1, -1, null, projectile.whoAmI);
						projectile.active = true;
						if (Instance.DetermineUnloadedSections > 0)
						{
							Main.NewText("Retrieving Tile Data with a Golf Ball.", zazasFavoriteColor);
							Main.NewText("Speeds vary based off connection speed and on how many tiles have already been loaded.", zazasFavoriteColor);
							Main.NewText("Map chunk data retrieval with this method may vary.", zazasFavoriteColor);
							Vector2 position = default(Vector2);
							while (true)
							{
								if (Main.rand == null)
								{
									Main.rand = new UnifiedRandom();
								}
								for (int i = 40; i < Main.maxTilesX; i += 200)
								{
									for (int j = 40; j < Main.maxTilesY; j += 150)
									{
										if (stopMapReveal || Main.gameMenu)
										{
											stopMapReveal = false;
											revealMapCooldown = false;
											Main.NewText("Canceled retrieving tile data.", zazasFavoriteColor);
											return;
										}
										((Vector2)(ref position))._002Ector((float)i * 16f, (float)j * 16f);
										projectile.position = position;
										projectile.velocity = new Vector2(0f, -10f);
										projectile.timeLeft = 1000000;
										projectile.tileCollide = false;
										projectile.ai[0] = 1f;
										projectile.ai[1] = 1f;
										NetMessage.SendData(27, -1, -1, null, projectile.whoAmI);
										Thread.Sleep(50);
									}
								}
								while (Instance.DetermineUnloadedSections > 0)
								{
									if (!projectile.active)
									{
										goto IL_01dd;
									}
									if (!revealMapCooldown)
									{
										return;
									}
									Thread.Sleep(1);
								}
								break;
								IL_01dd:
								Main.NewText("Golf ball lost!!", errorColor);
								projectile.identity = Projectile.NewProjectile(Main.LocalPlayer.GetProjectileSource_Misc(721), new Vector2(40f, 40f), Vector2.Zero, 1020, 0, 0f, Main.myPlayer);
							}
							UpdateAndRefreshMap();
							projectile.Kill();
						}
						else
						{
							Main.NewText("Refreshing Map.", zazasFavoriteColor);
							UpdateAndRefreshMap();
						}
						revealMapCooldown = false;
					}
					else
					{
						revealMapCooldown = true;
						UpdateAndRefreshMap();
						revealMapCooldown = false;
					}
				}
			}
			catch (Exception ex)
			{
				Main.NewText("Error: " + ex, errorColor);
			}
		});
	}

	public void FillOrEraseSelectionPoints()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		if (!selectionMode || Main.gameMenu)
		{
			return;
		}
		if (!Main.mapFullscreen && !Main.player[Main.myPlayer].mouseInterface)
		{
			if (Main.mouseLeft)
			{
				if (selectionB == Point.Zero)
				{
					selectionB = Main.MouseWorld.ToTileCoordinates();
				}
				selectionA = Main.MouseWorld.ToTileCoordinates();
				edgeA = true;
			}
			if (Main.mouseRight)
			{
				if (selectionA == Point.Zero)
				{
					selectionA = Main.MouseWorld.ToTileCoordinates();
				}
				selectionB = Main.MouseWorld.ToTileCoordinates();
				edgeB = true;
			}
			if ((!edgeA && !edgeB) || selectionA == Point.Zero || selectionB == Point.Zero)
			{
				return;
			}
		}
		else
		{
			if (!GetMapCoordinatesModV2(Main.mouseX, Main.mouseY, 0, out var result) || (!uiRework && tildeTerminal))
			{
				return;
			}
			if (Main.mouseLeft && PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Activator (Map)") && !CustomUIMousedOver && !Main.grabbingMap)
			{
				if (selectionB == Point.Zero)
				{
					selectionB = result;
				}
				selectionA = result;
				edgeA = true;
			}
			if (Main.mouseRight && PlayerInput.Triggers.Current.KeyStatusHandler("Hold for Activator (Map)") && !CustomUIMousedOver && !Main.grabbingMap)
			{
				if (selectionA == Point.Zero)
				{
					selectionA = result;
				}
				selectionB = result;
				edgeB = true;
			}
			if ((!edgeA && !edgeB) || selectionA == Point.Zero || selectionB == Point.Zero)
			{
				return;
			}
		}
		if (!Main.mouseLeft && Main.mouseLeftRelease)
		{
			if (edgeA)
			{
				Main.NewText("Point A selected", zazasFavoriteColor);
			}
			edgeA = false;
		}
		if (!Main.mouseRight && Main.mouseRightRelease)
		{
			if (edgeB)
			{
				Main.NewText("Point B selected", zazasFavoriteColor);
			}
			edgeB = false;
		}
	}

	[Old("Deprecated.")]
	public static async void FillTilesSelectionMode()
	{
		await Task.Run(delegate
		{
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			int num = Math.Min(selectionA.X, selectionB.X);
			int num2 = Math.Min(selectionA.Y, selectionB.Y);
			int num3 = Math.Abs(selectionA.X - selectionB.X);
			int num4 = Math.Abs(selectionA.Y - selectionB.Y);
			int selectedItem = Main.LocalPlayer.selectedItem;
			int createTile = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].createTile;
			int createWall = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].createWall;
			int num5 = 0;
			for (int i = num; i <= num + num3; i++)
			{
				_ = NetMessage.buffer[256].spamCount;
				int j;
				for (j = num2; j <= num2 + num4; j++)
				{
					if (WorldGen.InWorld(i, j))
					{
						bool flag = false;
						Tile tile = Main.tile[i, j];
						if (Main.gameMenu)
						{
							Main.NewText("You can't do this here! (This should never appear)", zazasFavoriteColor);
							return;
						}
						if (tile == null)
						{
							tile = default(Tile);
						}
						if (tile.type != createTile && tile.type >= 0 && createWall < 0)
						{
							tile.ClearTile();
						}
						if (!tile.active())
						{
							if (createTile >= 0)
							{
								tile.active(active: true);
								tile.type = (ushort)createTile;
								Main.QueueMainThreadAction(delegate
								{
									WorldGen.SquareTileFrame(i, j, resetFrame: true, noBreak: true);
								});
								flag = true;
								if (Main.netMode != 0 && !selectionTileSyncingMode)
								{
									FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), selectedItem);
									NetMessage.SendData(17, -1, -1, null, 1, i, j, createTile);
								}
							}
							else if (createWall > 0)
							{
								tile.wall = (ushort)createWall;
								Main.QueueMainThreadAction(delegate
								{
									WorldGen.SquareWallFrame(i, j);
								});
								flag = true;
								if (Main.netMode != 0 && !selectionTileSyncingMode)
								{
									FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), selectedItem);
									NetMessage.SendData(17, -1, -1, null, 3, i, j, createWall);
								}
							}
						}
						if (flag)
						{
							while (quedPackets >= tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS)
							{
								Thread.Sleep(150);
							}
							Main.Map.UpdateLighting(i, j, byte.MaxValue);
							Main.refreshMap = true;
							num5++;
						}
					}
				}
			}
			if (Main.netMode != 0 && selectionTileSyncingMode)
			{
				for (int num6 = num; num6 <= num + num3; num6 += 10)
				{
					for (int num7 = num2; num7 <= num2 + num4; num7 += 10)
					{
						NetMessage.SendTileSquare(-1, num6, num7, 10, 10);
					}
				}
			}
			Main.NewText(num5 + " Tiles Filled.", zazasFavoriteColor);
		});
	}

	[Old("Deprecated.")]
	public void CopyTilesSelcectionMode()
	{
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!copyMode || Main.gameMenu || pasteMode || !Main.hasFocus)
			{
				return;
			}
			if (concurrentPastes > 0)
			{
				Main.NewText("You cannot copy tiles while pasting.", errorColor);
				copyMode = false;
			}
			else if (!Main.mapFullscreen && !Main.player[Main.myPlayer].mouseInterface)
			{
				if (Main.mouseLeft)
				{
					if (copyStartB == Point.Zero)
					{
						copyStartB = Main.MouseWorld.ToTileCoordinates();
					}
					copyStartA = Main.MouseWorld.ToTileCoordinates();
					edgeA = true;
				}
				if (Main.mouseRight)
				{
					if (copyStartA == Point.Zero)
					{
						copyStartA = Main.MouseWorld.ToTileCoordinates();
					}
					copyStartB = Main.MouseWorld.ToTileCoordinates();
					edgeB = true;
				}
				else
				{
					if ((!edgeA && !edgeB) || !(copyStartA != Point.Zero) || !(copyStartB != Point.Zero))
					{
						return;
					}
					int num = Math.Min(copyStartA.X, copyStartB.X);
					int num2 = Math.Min(copyStartA.Y, copyStartB.Y);
					schematicSizeX = Math.Abs(copyStartA.X - copyStartB.X) + 1;
					schematicSizeY = Math.Abs(copyStartA.Y - copyStartB.Y) + 1;
					tileLoopCount = 0;
					if (copiedTiles.Capacity > 0 && copiedTiles.Count > 0)
					{
						copiedTiles.Clear();
						copiedTiles.TrimExcess();
					}
					for (int i = num; i < num + schematicSizeX; i++)
					{
						for (int j = num2; j < num2 + schematicSizeY; j++)
						{
							Tile tile = Main.tile[i, j];
							if (tile == null)
							{
								Main.NewText("You cannot copy tiles that have not been loaded");
								break;
							}
							if (WorldGen.InWorld(i, j))
							{
								LegacyTile newTile = new LegacyTile(tile);
								copiedTiles.Add(newTile);
								tileLoopCount++;
							}
						}
					}
					Schematics.ResetPreview();
					if (Main.mouseLeftRelease)
					{
						if (edgeA)
						{
							Main.NewText(tileLoopCount + " Tiles copied", zazasFavoriteColor);
						}
						edgeA = false;
					}
					if (Main.mouseRightRelease)
					{
						if (edgeB)
						{
							Main.NewText(tileLoopCount + " Tiles copied", zazasFavoriteColor);
						}
						edgeB = false;
					}
				}
			}
			else
			{
				if (!GetMapCoordinatesModV2(Main.mouseX, Main.mouseY, 0, out var result) || tildeTerminal)
				{
					return;
				}
				if (Main.mouseLeft && ((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)160))
				{
					if (copyStartB == Point.Zero)
					{
						copyStartB = result;
					}
					copyStartA = result;
					edgeA = true;
				}
				if (Main.mouseRight && ((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)160))
				{
					if (copyStartA == Point.Zero)
					{
						copyStartA = result;
					}
					copyStartB = result;
					edgeB = true;
				}
				else
				{
					if ((!edgeA && !edgeB) || !(copyStartA != Point.Zero) || !(copyStartB != Point.Zero))
					{
						return;
					}
					int num3 = Math.Min(copyStartA.X, copyStartB.X);
					int num4 = Math.Min(copyStartA.Y, copyStartB.Y);
					schematicSizeX = Math.Abs(copyStartA.X - copyStartB.X) + 1;
					schematicSizeY = Math.Abs(copyStartA.Y - copyStartB.Y) + 1;
					tileLoopCount = 0;
					if (copiedTiles.Capacity > 0 && copiedTiles.Count > 0)
					{
						copiedTiles.Clear();
						copiedTiles.TrimExcess();
					}
					for (int k = num3; k < num3 + schematicSizeX; k++)
					{
						for (int l = num4; l < num4 + schematicSizeY; l++)
						{
							Tile tile2 = Main.tile[k, l];
							if (tile2 == null)
							{
								Main.NewText("You cannot copy tiles that have not been loaded.");
								break;
							}
							if (WorldGen.InWorld(k, l))
							{
								LegacyTile newTile2 = new LegacyTile(tile2);
								copiedTiles.Add(newTile2);
								tileLoopCount++;
							}
						}
					}
					Schematics.ResetPreview();
					if (Main.mouseLeftRelease && edgeA)
					{
						edgeA = false;
						Main.NewText(tileLoopCount + " Tiles copied", zazasFavoriteColor);
					}
					else if (Main.mouseRightRelease && edgeB)
					{
						edgeB = false;
					}
				}
			}
		}
		catch (Exception ex)
		{
			copyMode = false;
			Main.NewText($"Copy Tles Error!\n{ex}", errorColor);
			LogException(ex);
		}
	}

	public void LoadTilesFromFile()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		Main.NewText("This operation is deprecated and may not return in the future.", errorColor);
	}

	public void SaveTilesToFile()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		Main.NewText("This operation is deprecated and will not return.", errorColor);
	}

	public static void DeleteGroundItemsCall(int autoMode)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		for (int i = 0; i < 400; i++)
		{
			Item item = Main.item[i];
			if (item.active && item.type > 0 && !item.beingGrabbed)
			{
				item.active = false;
				item.type = 0;
				item.stack = 0;
				num++;
				if (Main.netMode != 0)
				{
					Instance.FalsePlayerPacket(new Vector2(item.position.X, item.position.Y));
					NetMessage.SendData(21, -1, -1, null, i);
					NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
				}
			}
		}
		if (autoMode != 1)
		{
			Main.NewText("Total items removed: " + num, zazasFavoriteColor);
		}
	}

	public static void CleanupGores()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < Main.gore.Length; i++)
		{
			Main.gore[i].active = false;
		}
		Main.NewText("Cleaned up gores.", zazasFavoriteColor);
	}

	public static void KillAllProjectiles(bool selfOnly = false)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		for (int num12 = 0; num12 < Main.projectile.Length; num12++)
		{
			Projectile projectile = Main.projectile[num12];
			bool owned = projectile.owner == Main.myPlayer;
			if (projectile.active)
			{
				if (selfOnly & owned)
				{
					projectile.Kill();
				}
				else if (!selfOnly)
				{
					projectile.active = false;
				}
				if (owned)
				{
					NetMessage.SendData(29, -1, -1, null, projectile.whoAmI, projectile.owner);
				}
			}
		}
		if (selfOnly)
		{
			Main.NewText("Removed all projectiles owned by you.", zazasFavoriteColor);
		}
	}

	public static bool IsPlayerOnGround(Player plr)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		if (plr == null)
		{
			return false;
		}
		bool onGround = true;
		bool ignorePlats = false;
		bool fallThrough = plr.controlDown;
		if (plr.gravDir == -1f || (plr.mount.Active && (plr.mount.Cart || plr.mount.Type == 12 || plr.mount.Type == 7 || plr.mount.Type == 8 || plr.mount.Type == 23 || plr.mount.Type == 44 || plr.mount.Type == 48)) || plr.GoingDownWithGrapple || plr.pulley)
		{
			ignorePlats = true;
			fallThrough = true;
		}
		Vector2 val = Collision.TileCollision(plr.position, plr.velocity, plr.width, plr.height, fallThrough, ignorePlats, (int)plr.gravDir);
		if (val.Y > plr.gravity)
		{
			onGround = false;
		}
		if (val.Y < 0f - plr.gravity)
		{
			onGround = false;
		}
		return onGround;
	}

	public static bool WithinScreenBounds(int x, int y, int fluff = -1)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		return WithinScreenBounds(new Vector2((float)(x * 16), (float)(y * 16)), (float)(fluff * 16));
	}

	public static bool WithinScreenBounds(float x, float y, float fluff = -16f)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return WithinScreenBounds(new Vector2(x, y), fluff);
	}

	public static bool WithinScreenBounds(Vector2 pos, float fluff = -16f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		if (pos.X >= Main.ViewPosition.X + fluff && pos.Y >= Main.ViewPosition.Y + fluff && pos.X < Main.ViewPosition.X - fluff + (float)Main.screenWidth / Main.GameViewMatrix.Zoom.X)
		{
			return pos.Y < Main.ViewPosition.Y - fluff + (float)Main.screenHeight / Main.GameViewMatrix.Zoom.Y;
		}
		return false;
	}

	public static bool WithinScreenBounds(Rectangle rect, int fluff = -16)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		return ((Rectangle)(ref rect)).Intersects(new Rectangle((int)Main.ViewPosition.X + fluff, (int)Main.ViewPosition.Y + fluff, (int)((float)Main.screenWidth / Main.GameViewMatrix.Zoom.X) - fluff, (int)((float)Main.screenHeight / Main.GameViewMatrix.Zoom.Y) - fluff));
	}

	public static bool WithinScreenBoundsMap(Vector2 pos, float fluff = -16f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (pos.X >= 0f + fluff && pos.Y >= 0f + fluff && pos.X < (float)Main.screenWidth - fluff)
		{
			return pos.Y < (float)Main.screenHeight - fluff;
		}
		return false;
	}

	public static bool WithinScreenBoundsMap(Rectangle rect, int fluff = -16)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return ((Rectangle)(ref rect)).Intersects(new Rectangle(fluff, fluff, Main.screenWidth - fluff, Main.screenHeight - fluff));
	}

	public static async void LoadChunksWithGayAssDrone()
	{
		if (loadingChunks)
		{
			return;
		}
		loadingChunks = true;
		await Task.Run(delegate
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			if (Main.netMode == 1)
			{
				while (true)
				{
					try
					{
						if (Main.gameMenu || (uiRework && spectatePlayerTarget == -1 && !freeCam) || (!uiRework && !spectatingPlayer && !freeCam))
						{
							loadingChunks = false;
							break;
						}
						Vector2 val = savedFreeCam;
						if (freeCam)
						{
							val.X = savedFreeCam.X + (float)Main.screenWidth * 0.5f + Main.cameraX;
							val.Y = savedFreeCam.Y + (float)Main.screenHeight * 0.5f + Main.LocalPlayer.gfxOffY;
						}
						int sectionX = Netplay.GetSectionX((int)val.X / 16);
						int sectionY = Netplay.GetSectionY((int)val.Y / 16);
						bool flag = false;
						for (int i = sectionX - 2; i < sectionX + 2; i++)
						{
							for (int j = sectionY - 2; j < sectionY + 2; j++)
							{
								if (i < Main.maxSectionsX && i >= 0 && j < Main.maxSectionsY && j >= 0 && !Main.sectionManager.SectionLoaded(i, j))
								{
									flag = true;
									LoadChunk(i, j);
									if (freeCamDebug)
									{
										Main.NewText(string.Format("Chunk Load attempted at SectionX: {0}, SectionY: {1}", new object[2] { i, j }), zazasFavoriteColor);
									}
									Thread.Sleep(35);
								}
							}
						}
						if (!flag)
						{
							loadingChunks = false;
							break;
						}
					}
					catch
					{
					}
				}
			}
		});
		loadingChunks = false;
	}

	public static void LoadChunk(int x, int y)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.gameMenu)
		{
			Vector2 projPos = default(Vector2);
			((Vector2)(ref projPos))._002Ector((float)(x * 200) * 16f + 8f, (float)(y * 150) * 16f + 8f);
			new Projectile();
			Projectile obj = Main.projectile[Projectile.NewProjectile(null, projPos, new Vector2(1f, 1f), 1020, 0, 0f, Main.myPlayer)];
			obj.velocity = Vector2.Zero;
			obj.timeLeft = 6;
			obj.miscText = "ZazaMod Map Revealing Projectile";
		}
	}

	public async void LoadChunkWithPacket8(Vector2 desiredPos)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode != 1)
		{
			return;
		}
		for (int x = (int)desiredPos.X / 16 - 400; x <= (int)desiredPos.X / 16 - 200; x += 200)
		{
			for (int y = (int)desiredPos.Y / 16 - 300; y < (int)desiredPos.Y / 16 + 300; y += 150)
			{
				if (!Main.sectionManager.SectionLoaded(Netplay.GetSectionX(x), Netplay.GetSectionY(y)) && WorldGen.InWorld(x, y))
				{
					Main.player[Main.myPlayer].SpawnX = x;
					Main.player[Main.myPlayer].SpawnY = y;
					NetMessage.SendData(8, -1, -1, null, Main.player[Main.myPlayer].SpawnX, Main.player[Main.myPlayer].SpawnY);
					while (!Main.sectionManager.SectionLoaded(Netplay.GetSectionX(x), Netplay.GetSectionY(y)))
					{
						await Task.Delay(5);
					}
				}
			}
		}
		for (int x = (int)desiredPos.X / 16 + 400; x >= (int)desiredPos.X / 16 + 200; x -= 200)
		{
			for (int y = (int)desiredPos.Y / 16 - 300; y < (int)desiredPos.Y / 16 + 300; y += 150)
			{
				if (!Main.sectionManager.SectionLoaded(Netplay.GetSectionX(x), Netplay.GetSectionY(y)) && WorldGen.InWorld(x, y))
				{
					Main.player[Main.myPlayer].SpawnX = x;
					Main.player[Main.myPlayer].SpawnY = y;
					NetMessage.SendData(8, -1, -1, null, Main.player[Main.myPlayer].SpawnX, Main.player[Main.myPlayer].SpawnY);
					while (!Main.sectionManager.SectionLoaded(Netplay.GetSectionX(x), Netplay.GetSectionY(y)))
					{
						await Task.Delay(5);
					}
				}
			}
		}
	}

	public static async void EraseTileSelectionMode(int mode)
	{
		await Task.Run(delegate
		{
			//IL_0582: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			if (concurrentFillOrDels >= 6)
			{
				Main.NewText("Cannot have more than 6 actions running at once!", errorColor);
			}
			else
			{
				concurrentFillOrDels++;
				int num = Math.Min(selectionA.X, selectionB.X);
				int num2 = Math.Min(selectionA.Y, selectionB.Y);
				int num3 = Math.Abs(selectionA.X - selectionB.X);
				int num4 = Math.Abs(selectionA.Y - selectionB.Y);
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				if (cancelAction)
				{
					cancelAction = false;
				}
				try
				{
					for (int i = num; i <= num + num3; i++)
					{
						_ = NetMessage.buffer[256].spamCount;
						int j;
						for (j = num2; j <= num2 + num4; j++)
						{
							if (cancelAction)
							{
								NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
								Main.NewText("Caneled tile erasing.", zazasFavoriteColor);
								cancelAction = false;
								concurrentFillOrDels--;
								return;
							}
							if (WorldGen.InWorld(i, j))
							{
								bool flag = false;
								Tile tile = Main.tile[i, j];
								if (Main.gameMenu)
								{
									Main.NewText("You can't do this here! (This should never appear)", errorColor);
									concurrentFillOrDels--;
									return;
								}
								if (tile == null)
								{
									tile = default(Tile);
								}
								if (tile.active() && (mode == 0 || mode == 1))
								{
									flag = true;
									tile.ClearTile();
									Main.QueueMainThreadAction(delegate
									{
										WorldGen.SquareTileFrame(i, j);
									});
									if (Main.netMode == 1 && !selectionTileSyncingMode)
									{
										NetMessage.SendData(17, -1, -1, null, 4, i, j);
										num5++;
										num7++;
									}
								}
								if (tile.wall > 0 && (mode == 0 || mode == 2))
								{
									flag = true;
									tile.wall = 0;
									Main.QueueMainThreadAction(delegate
									{
										WorldGen.SquareWallFrame(i, j);
									});
									if (Main.netMode == 1 && !selectionTileSyncingMode)
									{
										NetMessage.SendData(17, -1, -1, null, 2, i, j);
										num5++;
										num7++;
									}
								}
								if (tile.liquid > 0 && mode == 3)
								{
									flag = true;
									tile.Clear(TileDataType.Liquid);
									Main.QueueMainThreadAction(delegate
									{
										WorldGen.SquareTileFrame(i, j, resetFrame: false);
									});
									if (Main.netMode == 1 && !selectionTileSyncingMode)
									{
										ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, 5304f);
										FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), 58);
										NetMessage.sendWater(i, j);
										NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
										num6++;
										num7++;
									}
								}
								if (flag)
								{
									if (!selectionTileSyncingMode)
									{
										switch (mode)
										{
										case 0:
										case 1:
										case 2:
											if (!tilePacketMaxMode)
											{
												if ((Main.netMode == 1 && num5 % erasePacketRate == 0) || IsLowFPS)
												{
													num5 = 0;
													Thread.Sleep(100);
												}
											}
											else
											{
												while ((Main.netMode == 1 && (num5 > erasePacketRate || Netplay.Connection.TimeOutTimer > 60)) || IsLowFPS)
												{
													num5 = 0;
													Thread.Sleep(500);
												}
											}
											break;
										case 3:
											if (!tilePacketMaxMode)
											{
												if ((Main.netMode == 1 && num6 % liquidPacketRate == 0) || IsLowFPS)
												{
													num6 = 0;
													Thread.Sleep(100);
												}
											}
											else
											{
												while ((Main.netMode == 1 && (num6 > liquidPacketRate || Netplay.Connection.TimeOutTimer > 60)) || IsLowFPS)
												{
													num6 = 0;
													Thread.Sleep(500);
												}
											}
											break;
										}
									}
									Main.Map.UpdateLighting(i, j, byte.MaxValue);
									Main.refreshMap = true;
								}
							}
						}
					}
					if (Main.netMode == 1 && selectionTileSyncingMode)
					{
						for (int num8 = num; num8 <= num + num3; num8 += 10)
						{
							for (int num9 = num2; num9 <= num2 + num4; num9 += 10)
							{
								NetMessage.SendTileSquare(-1, num8, num9, 10, 10);
								while (num5 > erasePacketRate || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS)
								{
									num5 = 0;
									Thread.Sleep(500);
								}
								num5++;
								num7++;
							}
						}
					}
				}
				catch (Exception ex)
				{
					Main.NewText($"Erase selection error!\n{ex}", errorColor);
					LogException(ex);
				}
				Main.NewText(num7 + " Tiles Removed.", zazasFavoriteColor);
				NetMessage.SendData(13);
				concurrentFillOrDels--;
			}
		});
	}

	public static void ButcherMobsAndNpcs(int damage, int mode)
	{
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		for (int i = 0; i < Main.npc.Length; i++)
		{
			NPC nPC = Main.npc[i];
			if (damage <= 0)
			{
				damage = (autoButcher ? autoButcherDamage : 9999);
			}
			if (Main.netMode == 0)
			{
				damage = 9999;
			}
			if (!nPC.active || nPC.type == 488 || !((mode == 1) ? (!nPC.townNPC) : nPC.townNPC))
			{
				continue;
			}
			nPC.StrikeNPCNoInteraction(damage, 10f, -nPC.direction, damage == 9999, noEffect: false, fromNet: false, exact: true);
			if (Main.netMode != 0)
			{
				Instance.FalsePlayerPacket(new Vector2(nPC.position.X, nPC.position.Y));
				if (damage != 9999)
				{
					damage += nPC.defense / 2;
				}
				NetMessage.SendData(28, -1, -1, null, i, damage, 10f, 0f - (float)nPC.direction, (damage == 9999) ? 1 : 0);
				NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
			}
			num = ((!autoButcher) ? (num + 1) : 0);
		}
		if (!autoButcher)
		{
			Main.NewText("You've butchered " + num + " npc's", zazasFavoriteColor);
		}
	}

	public void DisableBrushes()
	{
		tileBrush = false;
		eraseBrush = false;
		liquidBrush = false;
		liquidBrush = false;
		tileExploitBrush = false;
		tilePaintBrush = false;
	}

	[Old("Deprecated.")]
	public async void DrawPhotoV2(Point mouseToTile)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		await Task.Run(delegate
		{
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			for (int i = mouseToTile.X; i < mouseToTile.X + drawPhotoWidth; i++)
			{
				_ = NetMessage.buffer[256].spamCount;
				int j;
				for (j = mouseToTile.Y; j < mouseToTile.Y + drawPhotoHeight; j++)
				{
					if (WorldGen.InWorld(i, j))
					{
						int num2 = Schematics.ConvertBlockColorToClosestPixelColorV2(num);
						bool flag = false;
						Tile tile = Main.tile[i, j];
						if (!Main.sectionManager.SectionLoaded(Netplay.GetSectionX(i), Netplay.GetSectionY(j)))
						{
							Main.NewText("Tiles are not loaded, reveal map.", zazasFavoriteColor);
							return;
						}
						tile.ClearTile();
						if (!tile.active())
						{
							if (j % 15 == 0)
							{
								Thread.Sleep(20);
							}
							if (Schematics.ATempFix(num2))
							{
								tile.active(active: true);
								tile.type = (ushort)num2;
								Main.QueueMainThreadAction(delegate
								{
									WorldGen.SquareTileFrame(i, j);
								});
							}
							else
							{
								tile.wall = (ushort)num2;
								Main.QueueMainThreadAction(delegate
								{
									WorldGen.SquareWallFrame(i, j);
								});
							}
							flag = true;
							if (Main.netMode == 1)
							{
								if (!Schematics.ATempFix(num2))
								{
									num2 = ConvertUnsafeWall((ushort)num2);
								}
								FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), Main.LocalPlayer.selectedItem);
								try
								{
									ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, Main.LocalPlayer.selectedItem, 0f, Schematics.ATempFix(num2) ? nonSolidTileIDToItemID[num2] : wallIDToItemID[num2]);
								}
								catch
								{
								}
								NetMessage.SendData(17, -1, -1, null, (num2 >= 313) ? 1 : 3, i, j, num2);
							}
						}
						if (!tilePacketMaxMode)
						{
							if ((Main.netMode == 1 && num % tilePacketRate == 0) || IsLowFPS)
							{
								Thread.Sleep(100);
							}
						}
						else
						{
							while ((Main.netMode == 1 && (quedPackets > tilePacketRate || Netplay.Connection.TimeOutTimer > 60)) || IsLowFPS)
							{
								Thread.Sleep(500);
							}
						}
						if (flag)
						{
							Main.Map.UpdateLighting(i, j, byte.MaxValue);
							Main.refreshMap = true;
						}
						num++;
					}
				}
			}
			Main.NewText("Drawing Complete.", zazasFavoriteColor);
		});
	}

	public int ReturnItemIDUsingTileID(int tileID)
	{
		int result = 0;
		Item item = new Item();
		int num = 0;
		if (tileID != 214)
		{
			if (tileID > 300)
			{
				num = tileID * 7;
			}
			else if (tileID > 200)
			{
				num = tileID * 4;
			}
			else if (tileID > 100)
			{
				num = tileID * 3;
			}
		}
		for (int i = num; i < 5455; i++)
		{
			item.SetDefaults(i);
			if (item.createTile == tileID)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public static async void UpdateAndRefreshMap()
	{
		await MapRefresh();
		Main.refreshMap = true;
		Main.NewText("Map Revealed", zazasFavoriteColor);
	}

	public static Task MapRefresh()
	{
		for (int i = 0; i < Main.maxTilesX; i++)
		{
			for (int j = 0; j < Main.maxTilesY; j++)
			{
				if (WorldGen.InWorld(i, j, 5) && Main.sectionManager.SectionLoaded(Netplay.GetSectionX(i), Netplay.GetSectionY(j)))
				{
					Main.Map.Update(i, j, byte.MaxValue);
				}
			}
		}
		return Task.CompletedTask;
	}

	public static bool GetMapCoordinatesModV2(int PinX, int PinY, int Goal, out Point result)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.mapFullscreen)
		{
			result = new Point(-1, -1);
			return false;
		}
		_ = Main.maxTilesX / Main.textureMaxWidth;
		_ = Main.maxTilesY / Main.textureMaxHeight;
		float num = 10f;
		float num2 = 10f;
		float num3 = Main.maxTilesX - 10;
		float num4 = Main.maxTilesY - 10;
		float mapFullscreenScale = Main.mapFullscreenScale;
		float num5 = (float)Main.screenWidth / (float)Main.maxTilesX * 0.8f;
		if (Main.mapFullscreenScale < num5)
		{
			Main.mapFullscreenScale = num5;
		}
		if (Main.mapFullscreenScale > 16f)
		{
			Main.mapFullscreenScale = 16f;
		}
		mapFullscreenScale = Main.mapFullscreenScale;
		if (Main.mapFullscreenPos.X < num)
		{
			Main.mapFullscreenPos.X = num;
		}
		if (Main.mapFullscreenPos.X > num3)
		{
			Main.mapFullscreenPos.X = num3;
		}
		if (Main.mapFullscreenPos.Y < num2)
		{
			Main.mapFullscreenPos.Y = num2;
		}
		if (Main.mapFullscreenPos.Y > num4)
		{
			Main.mapFullscreenPos.Y = num4;
		}
		float x = Main.mapFullscreenPos.X;
		float y = Main.mapFullscreenPos.Y;
		float num6 = x * mapFullscreenScale;
		y *= mapFullscreenScale;
		float num7 = 0f - num6 + (float)(Main.screenWidth / 2);
		float num8 = 0f - y + (float)(Main.screenHeight / 2);
		num7 += num * mapFullscreenScale;
		num8 += num2 * mapFullscreenScale;
		float num9 = Main.maxTilesX / 840;
		num9 *= Main.mapFullscreenScale;
		float num10 = num7;
		float num11 = num8;
		float num12 = TextureAssets.Map.Width();
		float num13 = TextureAssets.Map.Height();
		if (Main.maxTilesX == 8400)
		{
			num9 *= 0.999f;
			num10 -= 40.6f * num9;
			num11 = num8 - 5f * num9;
			num12 -= 8.045f;
			num12 *= num9;
			num13 += 0.12f;
			num13 *= num9;
			if ((double)num9 < 1.2)
			{
				num13++;
			}
		}
		else if (Main.maxTilesX == 6400)
		{
			num9 *= 1.09f;
			num10 -= 38.8f * num9;
			num11 = num8 - 3.85f * num9;
			num12 -= 13.6f;
			num12 *= num9;
			num13 -= 6.92f;
			num13 *= num9;
			if ((double)num9 < 1.2)
			{
				num13 += 2f;
			}
		}
		else if (Main.maxTilesX == 6300)
		{
			num9 *= 1.09f;
			num10 -= 39.8f * num9;
			num11 = num8 - 4.08f * num9;
			num12 -= 26.69f;
			num12 *= num9;
			num13 -= 6.92f;
			num13 *= num9;
			if ((double)num9 < 1.2)
			{
				num13 += 2f;
			}
		}
		else if (Main.maxTilesX == 4200)
		{
			num9 *= 0.998f;
			num10 -= 37.3f * num9;
			num11 -= 1.7f * num9;
			num12 -= 16f;
			num12 *= num9;
			num13 -= 8.31f;
			num13 *= num9;
		}
		switch (Goal)
		{
		case 0:
		{
			int num14 = (int)((0f - num7 + (float)PinX) / mapFullscreenScale + num);
			int num15 = (int)((0f - num8 + (float)PinY) / mapFullscreenScale + num2);
			bool flag = false;
			if ((float)num14 < num)
			{
				flag = true;
			}
			if ((float)num14 >= num3)
			{
				flag = true;
			}
			if ((float)num15 < num2)
			{
				flag = true;
			}
			if ((float)num15 >= num4)
			{
				flag = true;
			}
			if (!flag)
			{
				result = new Point(num14, num15);
				return true;
			}
			result = new Point(-1, -1);
			return false;
		}
		case 1:
		{
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector(num7, num8);
			Vector2 vector2 = new Vector2((float)PinX, (float)PinY) * mapFullscreenScale - new Vector2(10f * mapFullscreenScale);
			result = (vector + vector2).ToPoint();
			return true;
		}
		default:
			result = new Point(-1, -1);
			return false;
		}
	}

	[Old("Deprecated. Use HandleBrushNetMessagesV3 instead.")]
	public static async void HandleBrushNetMessagesV2()
	{
		handlingPackets = true;
		try
		{
			int waitTime = 500;
			int value;
			if (markedTilesForEraseBrush.Count > 0 && ((Main.netMode == 2 && !Main.dedServ) ? 2000 : tilePacketRate) > 0)
			{
				int num3 = 0;
				foreach (KeyValuePair<Point, int> keyValuePair2 in markedTilesForEraseBrush.ToList())
				{
					if (Main.gameMenu)
					{
						Main.NewText("A tile was null when sending data. Or you entered the game menu.", zazasFavoriteColor);
						return;
					}
					if (keyValuePair2.Value == 1)
					{
						NetMessage.SendData(17, -1, -1, null, 4, keyValuePair2.Key.X, keyValuePair2.Key.Y);
					}
					else if (keyValuePair2.Value == 2)
					{
						NetMessage.SendData(17, -1, -1, null, 2, keyValuePair2.Key.X, keyValuePair2.Key.Y);
					}
					else
					{
						NetMessage.SendData(17, -1, -1, null, 4, keyValuePair2.Key.X, keyValuePair2.Key.Y);
						NetMessage.SendData(17, -1, -1, null, 2, keyValuePair2.Key.X, keyValuePair2.Key.Y);
					}
					if (!tilePacketMaxMode)
					{
						if (num3 % ((Main.netMode == 2 && !Main.dedServ) ? 2000 : tilePacketRate) == 0 || IsLowFPS)
						{
							await Task.Delay(100);
						}
					}
					else
					{
						while (quedPackets > ((Main.netMode == 2 && !Main.dedServ) ? 2000 : tilePacketRate) || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS)
						{
							await Task.Delay(waitTime);
						}
					}
					try
					{
						markedTilesForEraseBrush.TryRemove(keyValuePair2.Key, out value);
					}
					catch
					{
					}
					value = num3++;
				}
			}
			if (markedTilesForTileBrush.Count > 0)
			{
				int num3 = 0;
				foreach (KeyValuePair<Point, Point> keyValuePair3 in markedTilesForTileBrush.ToList())
				{
					if (Main.tile[keyValuePair3.Key.X, keyValuePair3.Key.Y] == null || Main.gameMenu)
					{
						Main.NewText("A tile was null when sending data. Or you entered the game menu.", zazasFavoriteColor);
						return;
					}
					if (keyValuePair3.Value.X == 1)
					{
						FakeNetMessage13(new Vector2((float)((keyValuePair3.Key.X + 2) * 16), (float)(keyValuePair3.Key.Y * 16)), savedSelectedItemForTileBrush);
						NetMessage.SendData(17, -1, -1, null, 1, keyValuePair3.Key.X, keyValuePair3.Key.Y, savedCreateTileForTileBrush, savedPlaceStyleForTileBrush);
						NetMessage.SendData(17, -1, -1, null, 14, keyValuePair3.Key.X, keyValuePair3.Key.Y);
					}
					if (keyValuePair3.Value.X == 2)
					{
						FakeNetMessage13(new Vector2((float)((keyValuePair3.Key.X + 2) * 16), (float)(keyValuePair3.Key.Y * 16)), savedSelectedItemForTileBrush);
						NetMessage.SendObjectPlacement(Main.myPlayer, keyValuePair3.Key.X, keyValuePair3.Key.Y, savedCreateTileForTileBrush, keyValuePair3.Value.Y, 0, 0, Main.LocalPlayer.direction);
					}
					if (!tilePacketMaxMode)
					{
						if (num3 % ((Main.netMode == 2 && !Main.dedServ) ? 2000 : tilePacketRate) == 0 || IsLowFPS)
						{
							await Task.Delay(100);
						}
					}
					else
					{
						while (quedPackets > ((Main.netMode == 2 && !Main.dedServ) ? 2000 : tilePacketRate) || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS)
						{
							await Task.Delay(waitTime);
						}
					}
					try
					{
						markedTilesForTileBrush.TryRemove(keyValuePair3.Key, out var _);
					}
					catch
					{
					}
					value = num3++;
				}
			}
			if (markedTilesForWallBrush.Count > 0)
			{
				int num3 = 0;
				foreach (KeyValuePair<Point, int> keyValuePair2 in markedTilesForWallBrush.ToList())
				{
					if (Main.tile[keyValuePair2.Key.X, keyValuePair2.Key.Y] == null || Main.gameMenu)
					{
						Main.NewText("A tile was null when sending data. Or you entered the game menu.", zazasFavoriteColor);
						return;
					}
					if (keyValuePair2.Value == 1)
					{
						FakeNetMessage13(new Vector2((float)((keyValuePair2.Key.X + 2) * 16), (float)(keyValuePair2.Key.Y * 16)), Main.LocalPlayer.selectedItem);
						NetMessage.SendData(17, -1, -1, null, 3, keyValuePair2.Key.X, keyValuePair2.Key.Y, Main.LocalPlayer.HeldItem.createWall, Main.LocalPlayer.HeldItem.placeStyle);
					}
					if (!tilePacketMaxMode)
					{
						if (num3 % ((Main.netMode == 2 && !Main.dedServ) ? 2000 : tilePacketRate) == 0 || IsLowFPS)
						{
							await Task.Delay(100);
						}
					}
					else
					{
						while (quedPackets > ((Main.netMode == 2 && !Main.dedServ) ? 2000 : tilePacketRate) || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS)
						{
							await Task.Delay(waitTime);
						}
					}
					try
					{
						markedTilesForWallBrush.TryRemove(keyValuePair2.Key, out value);
					}
					catch
					{
					}
					value = num3++;
				}
			}
			if (markedTilesForLiquidAndDryBrush.Count > 0)
			{
				int num3 = 0;
				foreach (KeyValuePair<Point, int> keyValuePair2 in markedTilesForLiquidAndDryBrush.ToList())
				{
					if (Main.tile[keyValuePair2.Key.X, keyValuePair2.Key.Y] == null || Main.gameMenu)
					{
						Main.NewText("A tile was null when sending data. Or you entered the game menu.", zazasFavoriteColor);
						return;
					}
					if (keyValuePair2.Value == 0 || keyValuePair2.Value == 1 || keyValuePair2.Value == 2 || keyValuePair2.Value == 3 || keyValuePair2.Value == 4)
					{
						FakeNetMessage13(new Vector2((float)((keyValuePair2.Key.X + 2) * 16), (float)(keyValuePair2.Key.Y * 16)), Main.LocalPlayer.selectedItem);
						NetMessage.sendWater(keyValuePair2.Key.X, keyValuePair2.Key.Y);
					}
					if (!tilePacketMaxMode)
					{
						if (num3 % ((Main.netMode == 2 && !Main.dedServ) ? 2000 : tilePacketRate) == 0 || IsLowFPS)
						{
							await Task.Delay(100);
						}
					}
					else
					{
						while (quedPackets > ((Main.netMode == 2 && !Main.dedServ) ? 2000 : tilePacketRate) || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS)
						{
							await Task.Delay(waitTime);
						}
					}
					try
					{
						markedTilesForLiquidAndDryBrush.TryRemove(keyValuePair2.Key, out value);
					}
					catch
					{
					}
					value = num3++;
				}
			}
			if (markedTilesForTilePaintBrush.Count > 0)
			{
				int num3 = 0;
				foreach (KeyValuePair<Point, int> keyValuePair2 in markedTilesForTilePaintBrush.ToList())
				{
					if (Main.gameMenu)
					{
						Main.NewText("A tile was null when sending data. Or you entered the game menu.", zazasFavoriteColor);
						return;
					}
					byte color = Main.tile[keyValuePair2.Key.X, keyValuePair2.Key.Y].color();
					byte paintCoating = 0;
					if (Main.tile[keyValuePair2.Key.X, keyValuePair2.Key.Y].fullbrightBlock())
					{
						paintCoating = 1;
					}
					if (Main.tile[keyValuePair2.Key.X, keyValuePair2.Key.Y].invisibleBlock())
					{
						paintCoating = 2;
					}
					byte wallColor = Main.tile[keyValuePair2.Key.X, keyValuePair2.Key.Y].wallColor();
					byte wallPaintCoating = 0;
					if (Main.tile[keyValuePair2.Key.X, keyValuePair2.Key.Y].fullbrightWall())
					{
						wallPaintCoating = 1;
					}
					if (Main.tile[keyValuePair2.Key.X, keyValuePair2.Key.Y].invisibleWall())
					{
						wallPaintCoating = 2;
					}
					FakeNetMessage13(new Vector2((float)((keyValuePair2.Key.X + 2) * 16), (float)(keyValuePair2.Key.Y * 16)), Main.LocalPlayer.selectedItem);
					if (keyValuePair2.Value == 1)
					{
						NetMessage.SendData(64, -1, -1, null, keyValuePair2.Key.X, keyValuePair2.Key.Y, (int)wallColor);
					}
					else if (keyValuePair2.Value == 2)
					{
						NetMessage.SendData(63, -1, -1, null, keyValuePair2.Key.X, keyValuePair2.Key.Y, (int)paintCoating, 1f);
					}
					else if (keyValuePair2.Value == 3)
					{
						NetMessage.SendData(64, -1, -1, null, keyValuePair2.Key.X, keyValuePair2.Key.Y, (int)wallPaintCoating, 1f);
					}
					else
					{
						NetMessage.SendData(63, -1, -1, null, keyValuePair2.Key.X, keyValuePair2.Key.Y, (int)color);
					}
					if (!tilePacketMaxMode)
					{
						if (num3 % ((Main.netMode == 2 && !Main.dedServ) ? 2000 : tilePacketRate) == 0 || IsLowFPS)
						{
							await Task.Delay(100);
						}
					}
					else
					{
						while (quedPackets > ((Main.netMode == 2 && !Main.dedServ) ? 2000 : tilePacketRate) || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS)
						{
							await Task.Delay(waitTime);
						}
					}
					try
					{
						markedTilesForTilePaintBrush.TryRemove(keyValuePair2.Key, out value);
					}
					catch
					{
					}
					value = num3++;
				}
			}
		}
		catch (Exception ex)
		{
			Main.NewText($"Handling Brush Packets Error!\n{ex}", errorColor);
			markedTilesForEraseBrush.Clear();
			markedTilesForLiquidAndDryBrush.Clear();
			markedTilesForTileBrush.Clear();
			markedTilesForWallBrush.Clear();
			markedTilesForTilePaintBrush.Clear();
			LogException(ex);
		}
		handlingPackets = false;
	}

	public static void PutTheEntireFuckingBeeMovieScriptIntoServerChat()
	{
		if (!Main.gameMenu)
		{
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("According to all known laws of aviation, there is no way a bee should be able to fly. Its wings are too small to get its fat little body off the ground. The bee, of course, flies anyway because bees don't care what humans think is impossible. Yellow, black. Yellow, black. Yellow, black. Yellow, black. Ooh, black and yellow! Let's shake it up a little. Barry! Breakfast is ready! Coming! Hang on a second."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Hello? - Barry? - Adam? - Can you believe this is happening? - I can't. I'll pick you up. Looking sharp. Use the stairs. Your father paid good money for those. Sorry. I'm excited. Here's the graduate. We're very proud of you, son. A perfect report card, all B's. Very proud."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Ma! I got a thing going here. - You got lint on your fuzz. - Ow! That's me! - Wave to us! We'll be in row 118,000. - Bye! Barry, I told you, stop flying in the house! Hey, Adam. - Hey, Barry. - Is that fuzz gel? - A little. Special day, graduation. Never thought I'd make it."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Three days grade school, three days high school. Those were awkward. Three days college. I'm glad I took a day and hitchhiked around the hive. You did come back different. - Hi, Barry. - Artie, growing a mustache? Looks good. - Hear about Frankie? - Yeah. - You going to the funeral?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("No, I'm not going. Everybody knows, sting someone, you die. Don't waste it on a squirrel. Such a hothead. I guess he could have just gotten out of the way. I love this incorporating an amusement park into our day. That's why we don't need vacations. Boy, quite a bit of pomp... under the circumstances."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Well, Adam, today we are men. - We are! - Bee-men. - Amen! Hallelujah! Students, faculty, distinguished bees, please welcome Dean Buzzwell. Welcome, New Hive City graduating class of... ...9:15. That concludes our ceremonies. And begins your career at Honex Industries!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Will we pick our job today? I heard it's just orientation. Heads up! Here we go. Keep your hands and antennas inside the tram at all times. - Wonder what it'll be like? - A little scary. Welcome to Honex, a division of Honesco and a part of the Hexagon Group. This is it! Wow. Wow."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("We know that you, as a bee, have worked your whole life to get to the point where you can work for your whole life. Honey begins when our valiant Pollen Jocks bring the nectar to the hive. Our top-secret formula is automatically color-corrected, scent-adjusted and bubble-contoured into this soothing sweet syrup with its distinctive golden glow you know as... Honey!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("That girl was hot. - She's my cousin! - She is? - Yes, we're all cousins. - Right. You're right. - At Honex, we constantly strive to improve every aspect of bee existence. These bees are stress-testing a new helmet technology. - What do you think he makes? - Not enough. Here we have our latest advancement, the Krelman."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("What does that do? - Catches that little strand of honey that hangs after you pour it. Saves us millions. Can anyone work on the Krelman? Of course. Most bee jobs are small ones. But bees know that every small job, if it's done well, means a lot. But choose carefully because you'll stay in the job you pick for the rest of your life."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("The same job the rest of your life? I didn't know that. What's the difference? You'll be happy to know that bees, as a species, haven't had one day off in 27 million years. So you'll just work us to death? We'll sure try. Wow! That blew my mind! 'What's the difference?' How can you say that? One job forever?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("That's an insane choice to have to make. I'm relieved. Now we only have to make one decision in life. But, Adam, how could they never have told us that? Why would you question anything? We're bees. We're the most perfectly functioning society on Earth. You ever think maybe things work a little too well here? Like what?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Give me one example. I don't know. But you know what I'm talking about. Please clear the gate. Royal Nectar Force on approach. Wait a second. Check it out. - Hey, those are Pollen Jocks! - Wow. I've never seen them this close. They know what it's like outside the hive."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Yeah, but some don't come back. - Hey, Jocks! - Hi, Jocks! You guys did great! You're monsters! You're sky freaks! I love it! I love it! - I wonder where they were. - I don't know. Their day's not planned. Outside the hive, flying who knows where, doing who knows what."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("You can't just decide to be a Pollen Jock. You have to be bred for that. Right. Look. That's more pollen than you and I will see in a lifetime. It's just a status symbol. Bees make too much of it. Perhaps. Unless you're wearing it and the ladies see you wearing it. "));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Those ladies? Aren't they our cousins too? Distant. Distant. Look at these two. - Couple of Hive Harrys. - Let's have fun with them. It must be dangerous being a Pollen Jock. Yeah. Once a bear pinned me against a mushroom! He had a paw on my throat, and with the other, he was slapping me!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Oh, my! - I never thought I'd knock him out. What were you doing during this? Trying to alert the authorities. I can autograph that. A little gusty out there today, wasn't it, comrades? Yeah. Gusty. We're hitting a sunflower patch six miles from here tomorrow. - Six miles, huh?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Barry! A puddle jump for us, but maybe you're not up for it. - Maybe I am. - You are not! We're going 0900 at J-Gate. What do you think, buzzy-boy? Are you bee enough? I might be. It all depends on what 0900 means. Hey, Honex! Dad, you surprised me. You decide what you're interested in?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Well, there's a lot of choices. - But you only get one. Do you ever get bored doing the same job every day? Son, let me tell you about stirring. You grab that stick, and you just move it around, and you stir it around. You get yourself into a rhythm. It's a beautiful thing."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("You know, Dad, the more I think about it, maybe the honey field just isn't right for me. You were thinking of what, making balloon animals? That's a bad job for a guy with a stinger. Janet, your son's not sure he wants to go into honey! - Barry, you are so funny sometimes."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("I'm not trying to be funny. You're not funny! You're going into honey. Our son, the stirrer! - You're gonna be a stirrer? - No one's listening to me! Wait till you see the sticks I have. I could say anything right now. I'm gonna get an ant tattoo! Let's open some honey and celebrate! Maybe I'll pierce my thorax. Shave my antennae. Shack up with a grasshopper. Get a gold tooth and call everybody 'dawg'! I'm so proud."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("We're starting work today! - Today's the day. Come on! All the good jobs will be gone. Yeah, right. Pollen counting, stunt bee, pouring, stirrer, front desk, hair removal... - Is it still available? - Hang on. Two left! One of them's yours! Congratulations! Step to the side. - What'd you get?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Picking crud out. Stellar! Wow! Oouple of newbies? Yes, sir! Our first day! We are ready! Make your choice. - You want to go first? - No, you go. Oh, my. What's available? Restroom attendant's open, not for the reason you think. - Any chance of getting the Krelman? - Sure, you're on. I'm sorry, the Krelman just closed out. Wax monkey's always open."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("The Krelman opened up again. What happened? A bee died. Makes an opening. See? He's dead. Another dead one. Deady. Deadified. Two more dead. Dead from the neck up. Dead from the neck down. That's life! Oh, this is so hard!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Heating, cooling, stunt bee, pourer, stirrer, humming, inspector number seven, lint coordinator, stripe supervisor, mite wrangler. Barry, what do you think I should... Barry? Barry! All right, we've got the sunflower patch in quadrant nine... What happened to you? Where are you? - I'm going out. - Out? Out where?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Out there. - Oh, no! I have to, before I go to work for the rest of my life. You're gonna die! You're crazy! Hello? Another call coming in. If anyone's feeling brave, there's a Korean deli on 83rd that gets their roses today."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Hey, guys. - Look at that. - Isn't that the kid we saw yesterday? Hold it, son, flight deck's restricted. It's OK, Lou. We're gonna take him up."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Really? Feeling lucky, are you? Sign here, here. Just initial that. - Thank you. - OK. You got a rain advisory today, and as you all know, bees cannot fly in rain. So be careful. As always, watch your brooms, hockey sticks, dogs, birds, bears and bats. Also, I got a couple of reports of root beer being poured on us. Murphy's in a home because of it, babbling like a cicada!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("That's awful. - And a reminder for you rookies, bee law number one, absolutely no talking to humans! All right, launch positions! Buzz, buzz, buzz, buzz! Buzz, buzz, buzz, buzz! Buzz, buzz, buzz, buzz! Black and yellow! Hello! You ready for this, hot shot? Yeah. Yeah, bring it on. Wind, check. - Antennae, check. - Nectar pack, check. - Wings, check. - Stinger, check. Scared out of my shorts, check."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("OK, ladies, let's move it out! Pound those petunias, you striped stem-suckers! All of you, drain those flowers! Wow! I'm out! I can't believe I'm out! So blue. I feel so fast and free! Box kite! Wow! Flowers! This is Blue Leader. We have roses visual. Bring it around 30 degrees and hold. Roses! 30 degrees, roger. Bringing it around. Stand to the side, kid. It's got a bit of a kick. That is one nectar collector!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Ever see pollination up close? - No, sir. I pick up some pollen here, sprinkle it over here. Maybe a dash over there, a pinch on that one. See that? It's a little bit of magic. That's amazing. Why do we do that? That's pollen power. More pollen, more flowers, more nectar, more honey for us. Cool. I'm picking up a lot of bright yellow. Could be daisies. Don't we need those?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Copy that visual. Wait. One of these flowers seems to be on the move. Say again? You're reporting a moving flower? Affirmative. That was on the line! This is the coolest. What is it? I don't know, but I'm loving this color. It smells good. Not like a flower, but I like it. Yeah, fuzzy. Chemical-y. Careful, guys. It's a little grabby."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("My sweet lord of bees! Candy-brain, get off there! Problem! - Guys! - This could be bad. Affirmative. Very close. Gonna hurt. Mama's little boy. You are way out of position, rookie! Coming in at you like a missile! Help me! I don't think these are flowers. - Should we tell him? - I think he knows. What is this?! Match point!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("You can start packing up, honey, because you're about to eat it! Yowser! Gross. There's a bee in the car! - Do something! - I'm driving! - Hi, bee. - He's back here! He's going to sting me! Nobody move. If you don't move, he won't sting you. Freeze! He blinked! Spray him, Granny!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("What are you doing?! Wow... the tension level out here is unbelievable. I gotta get home. Can't fly in rain. Can't fly in rain. Can't fly in rain. Mayday! Mayday! Bee going down! Ken, could you close the window please? Ken, could you close the window please? Check out my new resume. I made it into a fold-out brochure."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("You see? Folds out. Oh, no. More humans. I don't need this. What was that? Maybe this time. This time. This time. This time! This time! This... Drapes! That is diabolical. It's fantastic. It's got all my special skills, even my top-ten favorite movies. What's number one? Star Wars?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Nah, I don't go for that... ...kind of stuff. No wonder we shouldn't talk to them. They're out of their minds. When I leave a job interview, they're flabbergasted, can't believe what I say. There's the sun. Maybe that's a way out. I don't remember the sun having a big 75 on it."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("I predicted global warming. I could feel it getting hotter. At first I thought it was just me. Wait! Stop! Bee! Stand back. These are winter boots. Wait! Don't kill him! You know I'm allergic to them! This thing could kill me! Why does his life have less value than yours?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Why does his life have any less value than mine? Is that your statement? I'm just saying all life has value. You don't know what he's capable of feeling. My brochure! There you go, little guy. I'm not scared of him. It's an allergic thing. Put that on your resume brochure."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("My whole face could puff up. Make it one of your special skills. Knocking someone out is also a special skill. Right. Bye, Vanessa. Thanks. - Vanessa, next week? Yogurt night? - Sure, Ken. You know, whatever. - You could put carob chips on there. - Bye. - Supposed to be less calories. - Bye."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("I gotta say something. She saved my life. I gotta say something. All right, here it goes. Nah. What would I say? I could really get in trouble. It's a bee law. You're not supposed to talk to a human. I can't believe I'm doing this. I've got to. Oh, I can't do it. Come on! No. Yes. No. Do it. I can't. How should I start it? 'You like jazz?'"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("No, that's no good. Here she comes! Speak, you fool! Hi! I'm sorry. - You're talking. - Yes, I know. You're talking! I'm so sorry. No, it's OK. It's fine. I know I'm dreaming. But I don't recall going to bed. Well, I'm sure this is very disconcerting. This is a bit of a surprise to me. I mean, you're a bee! I am."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("And I'm not supposed to be doing this, but they were all trying to kill me. And if it wasn't for you... I had to thank you. It's just how I was raised. That was a little weird. - I'm talking with a bee. - Yeah. I'm talking to a bee. And the bee is talking to me! I just want to say I'm grateful. I'll leave now."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Wait! How did you learn to do that? - What? The talking thing. Same way you did, I guess. 'Mama, Dada, honey.' You pick it up. - That's very funny. - Yeah. Bees are funny. If we didn't laugh, we'd cry with what we have to deal with. Anyway... Can I... ...get you something? - Like what? I don't know."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("I mean... I don't know. Coffee? I don't want to put you out. It's no trouble. It takes two minutes. - It's just coffee. - I hate to impose. - Don't be ridiculous! - Actually, I would love a cup. Hey, you want rum cake? - I shouldn't. - Have some. - No, I can't. - Come on! I'm trying to lose a couple micrograms. - Where?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("These stripes don't help. You look great! I don't know if you know anything about fashion. Are you all right? No. He's making the tie in the cab as they're flying up Madison. He finally gets there. He runs up the steps into the church. The wedding is on. And he says, 'Watermelon? I thought you said Guatemalan. Why would I marry a watermelon?'"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Is that a bee joke? That's the kind of stuff we do. Yeah, different. So, what are you gonna do, Barry? About work? I don't know. I want to do my part for the hive, but I can't do it the way they want. I know how you feel. - You do? - Sure. My parents wanted me to be a lawyer or a doctor, but I wanted to be a florist."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Really? - My only interest is flowers. Our new queen was just elected with that same campaign slogan. Anyway, if you look... There's my hive right there. See it? You're in Sheep Meadow! Yes! I'm right off the Turtle Pond! No way! I know that area. I lost a toe ring there once."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Why do girls put rings on their toes? - Why not? - It's like putting a hat on your knee. - Maybe I'll try that. - You all right, ma'am? - Oh, yeah. Fine. Just having two cups of coffee! Anyway, this has been great. Thanks for the coffee. Yeah, it's no trouble."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Sorry I couldn't finish it. If I did, I'd be up the rest of my life. Are you...? Can I take a piece of this with me? Sure! Here, have a crumb. - Thanks! - Yeah. All right. Well, then... I guess I'll see you around. Or not. OK, Barry. And thank you so much again... for before. Oh, that?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("That was nothing. Well, not nothing, but... Anyway... This can't possibly work. He's all set to go. We may as well try it. OK, Dave, pull the chute. - Sounds amazing. - It was amazing! It was the scariest, happiest moment of my life. Humans! Humans! I can't believe you were with humans! Giant, scary humans!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("What were they like? Huge and crazy. They talk crazy. They eat crazy giant things. They drive crazy. - Do they try and kill you, like on TV? - Some of them. But some of them don't. - How'd you get back? - Poodle. You did it, and I'm glad. You saw whatever you wanted to see."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("You had your 'experience.' Now you can pick out your job and be normal. - Well... - Well? Well, I met someone. You did? Was she Bee-ish? - A wasp?! Your parents will kill you! - No, no, no, not a wasp. - Spider? - I'm not attracted to spiders."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("I know it's the hottest thing, with the eight legs and all. I can't get by that face. So who is she? She's... human. No, no. That's a bee law. You wouldn't break a bee law. - Her name's Vanessa. - Oh, boy. She's so nice. And she's a florist! Oh, no! You're dating a human florist! We're not dating."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("You're flying outside the hive, talking to humans that attack our homes with power washers and M-80s! One-eighth a stick of dynamite! She saved my life! And she understands me. This is over! Eat this. This is not over! What was that? - They call it a crumb. - It was so stingin' stripey! And that's not what they eat."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("That's what falls off what they eat! - You know what a Cinnabon is? - No. It's bread and cinnamon and frosting. They heat it up... Sit down! ...really hot! - Listen to me! We are not them! We're us. There's us and there's them! Yes, but who can deny the heart that is yearning? There's no yearning. Stop yearning."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Listen to me! You have got to start thinking bee, my friend. Thinking bee! - Thinking bee. - Thinking bee. Thinking bee! Thinking bee! Thinking bee! Thinking bee! There he is. He's in the pool. You know what your problem is, Barry? I gotta start thinking bee? How much longer will this go on? It's been three days!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Why aren't you working? I've got a lot of big life decisions to think about. What life? You have no life! You have no job. You're barely a bee! Would it kill you to make a little honey? Barry, come out. Your father's talking to you. Martin, would you talk to him? Barry, I'm talking to you! You coming?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Got everything? All set! Go ahead. I'll catch up. Don't be too long. Watch this! Vanessa! - We're still here. - I told you not to yell at him. He doesn't respond to yelling! - Then why yell at me? - Because you don't listen! I'm not listening to this. Sorry, I've gotta go. - Where are you going? - I'm meeting a friend. A girl?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Is this why you can't decide? Bye. I just hope she's Bee-ish. They have a huge parade of flowers every year in Pasadena? To be in the Tournament of Roses, that's every florist's dream! Up on a float, surrounded by flowers, crowds cheering. A tournament. Do the roses compete in athletic events? No."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("All right, I've got one. How come you don't fly everywhere? It's exhausting. Why don't you run everywhere? It's faster. Yeah, OK, I see, I see. All right, your turn. TiVo. You can just freeze live TV? That's insane! You don't have that? We have Hivo, but it's a disease. It's a horrible, horrible disease. Oh, my. Dumb bees! "));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("You must want to sting all those jerks. We try not to sting. It's usually fatal for us. So you have to watch your temper. Very carefully. You kick a wall, take a walk, write an angry letter and throw it out. Work through it like any emotion: Anger, jealousy, lust. Oh, my goodness! Are you OK? Yeah."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("What is wrong with you?! - It's a bug. He's not bothering anybody. Get out of here, you creep! What was that? A Pic 'N' Save circular? Yeah, it was. How did you know? It felt like about 10 pages. Seventy-five is pretty much our limit. You've really got that down to a science. - I lost a cousin to Italian Vogue. - I'll bet."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("What in the name of Mighty Hercules is this? How did this get here? Cute Bee, Golden Blossom, Ray Liotta Private Select? - Is he that actor? - I never heard of him. - Why is this here? - For people. We eat it. You don't have enough food of your own? - Well, yes. - How do you get it? - Bees make it. - I know who makes it!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("And it's hard to make it! There's heating, cooling, stirring. You need a whole Krelman thing! - It's organic. - It's our-ganic! It's just honey, Barry. Just what?! Bees don't know about this! This is stealing! A lot of stealing! You've taken our homes, schools, hospitals! This is all we have! And it's on sale?!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("I'm getting to the bottom of this. I'm getting to the bottom of all of this! Hey, Hector. - You almost done? - Almost. He is here. I sense it. Well, I guess I'll go home now and just leave this nice honey out, with no one around. You're busted, box boy! I knew I heard something. So you can talk! I can talk."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("And now you'll start talking! Where you getting the sweet stuff? Who's your supplier? I don't understand. I thought we were friends. The last thing we want to do is upset bees! You're too late! It's ours now! You, sir, have crossed the wrong sword! You, sir, will be lunch for my iguana, Ignacio! Where is the honey coming from?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Tell me where! Honey Farms! It comes from Honey Farms! Crazy person! What horrible thing has happened here? These faces, they never knew what hit them. And now they're on the road to nowhere! Just keep still. What? You're not dead? Do I look dead? They will wipe anything that moves. Where you headed?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("To Honey Farms. I am onto something huge here. I'm going to Alaska. Moose blood, crazy stuff. Blows your head off! I'm going to Tacoma. - And you? - He really is dead. All right. Uh-oh! - What is that?! - Oh, no! - A wiper! Triple blade! - Triple blade? Jump on! It's your only chance, bee! Why does everything have to be so doggone clean?!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("How much do you people need to see?! Open your eyes! Stick your head out the window! From NPR News in Washington, I'm Carl Kasell. But don't kill no more bugs! - Bee! - Moose blood guy!! - You hear something? - Like what? Like tiny screaming. Turn off the radio. Whassup, bee boy? Hey, Blood. Just a row of honey jars, as far as the eye could see."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Wow! I assume wherever this truck goes is where they're getting it. I mean, that honey's ours. - Bees hang tight. - We're all jammed in. It's a close community. Not us, man. We on our own. Every mosquito on his own. - What if you get in trouble? - You a mosquito, you in trouble. Nobody likes us. They just smack."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("See a mosquito, smack, smack! At least you're out in the world. You must meet girls. Mosquito girls try to trade up, get with a moth, dragonfly. Mosquito girl don't want no mosquito. You got to be kidding me! Mooseblood's about to leave the building! So long, bee! - Hey, guys! - Mooseblood! I knew I'd catch y'all down here."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Did you bring your crazy straw? We throw it in jars, slap a label on it, and it's pretty much pure profit. What is this place? A bee's got a brain the size of a pinhead. They are pinheads! Pinhead. –Check out the new smoker. - Oh, sweet. That's the one you want. The Thomas 3000! Smoker? Ninety puffs a minute, semi-automatic. Twice the nicotine, all the tar."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("A couple breaths of this knocks them right out. They make the honey, and we make the money. 'They make the honey, and we make the money'? Oh, my! What's going on? Are you OK? Yeah. It doesn't last too long."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Do you know you're in a fake hive with fake walls? Our queen was moved here. We had no choice. This is your queen? That's a man in women's clothes! That's a drag queen! What is this? Oh, no! There's hundreds of them! Bee honey. Our honey is being brazenly stolen on a massive scale!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("This is worse than anything bears have done! I intend to do something. Oh, Barry, stop. Who told you humans are taking our honey? That's a rumor. Do these look like rumors? That's a conspiracy theory. These are obviously doctored photos. How did you get mixed up in this? He's been talking to humans."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("What? - Talking to humans?! He has a human girlfriend. And they make out! Make out? Barry! We do not. - You wish you could. - Whose side are you on? The bees! I dated a cricket once in San Antonio. Those crazy legs kept me up all night. Barry, this is what you want to do with your life? I want to do it for all our lives."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Nobody works harder than bees! Dad, I remember you coming home so overworked your hands were still stirring. You couldn't stop. I remember that. What right do they have to our honey? We live on two cups a year. They put it in lip balm for no reason whatsoever! Even if it's true, what can one bee do? Sting them where it really hurts. In the face!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("The eye! - That would hurt. - No. Up the nose? That's a killer. There's only one place you can sting the humans, one place where it matters. Hive at Five, the hive's only full-hour action news source. No more bee beards! With Bob Bumble at the anchor desk. Weather with Storm Stinger. Sports with Buzz Larvi. And Jeanette Chung."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Good evening. I'm Bob Bumble. - And I'm Jeanette Chung. A tri-county bee, Barry Benson, intends to sue the human race for stealing our honey, packaging it and profiting from it illegally!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Tomorrow night on Bee Larry King, we'll have three former queens here in our studio, discussing their new book, Classy Ladies, out this week on Hexagon. Tonight we're talking to Barry Benson. Did you ever think, 'I'm a kid from the hive. I can't do this'? Bees have never been afraid to change the world."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("What about Bee Columbus? Bee Gandhi? Bejesus? Where I'm from, we'd never sue humans. We were thinking of stickball or candy stores. How old are you? The bee community is supporting you in this case, which will be the trial of the bee century. You know, they have a Larry King in the human world too."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("It's a common name. Next week... He looks like you and has a show and suspenders and colored dots... Next week... Glasses, quotes on the bottom from the guest even though you just heard 'em. Bear Week next week! They're scary, hairy and here live. Always leans forward, pointy shoulders, squinty eyes, very Jewish."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("In tennis, you attack at the point of weakness! It was my grandmother, Ken. She's 81. Honey, her backhand's a joke! I'm not gonna take advantage of that? Quiet, please. Actual work going on here. - Is that that same bee? - Yes, it is! I'm helping him sue the human race. - Hello. - Hello, bee. This is Ken."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Yeah, I remember you. Timberland, size ten and a half. Vibram sole, I believe. Why does he talk again? Listen, you better go 'cause we're really busy working. But it's our yogurt night! Bye-bye. Why is yogurt night so difficult?! You poor thing. You two have been at this for hours! Yes, and Adam here has been a huge help. - Frosting..."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("How many sugars? Just one. I try not to use the competition. So why are you helping me? Bees have good qualities. And it takes my mind off the shop. Instead of flowers, people are giving balloon bouquets now. Those are great, if you're three. And artificial flowers. - Oh, those just get me psychotic!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Yeah, me too. Bent stingers, pointless pollination. Bees must hate those fake things! Nothing worse than a daffodil that's had work done. Maybe this could make up for it a little bit. - This lawsuit's a pretty big deal. - I guess. You sure you want to go through with it? Am I sure?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("When I'm done with the humans, they won't be able to say, 'Honey, I'm home,' without paying a royalty! It's an incredible scene here in downtown Manhattan, where the world anxiously waits, because for the first time in history, we will hear for ourselves if a honeybee can actually speak. What have we gotten into here, Barry?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("It's pretty big, isn't it? I can't believe how many humans don't work during the day. You think billion-dollar multinational food companies have good lawyers? Everybody needs to stay behind the barricade. - What's the matter? - I don't know, I just got a chill. Well, if it isn't the bee team. You boys work on this?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("All rise! The Honorable Judge Bumbleton presiding. All right. Case number 4475, Superior Court of New York, Barry Bee Benson v. the Honey Industry is now in session. Mr. Montgomery, you're representing the five food companies collectively? A privilege. Mr. Benson... you're representing all the bees of the world?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("I'm kidding. Yes, Your Honor, we're ready to proceed. Mr. Montgomery, your opening statement, please. Ladies and gentlemen of the jury, my grandmother was a simple woman. Born on a farm, she believed it was man's divine right to benefit from the bounty of nature God put before us."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("If we lived in the topsy-turvy world Mr. Benson imagines, just think of what would it mean. I would have to negotiate with the silkworm for the elastic in my britches! Talking bee! How do we know this isn't some sort of holographic motion-picture-capture Hollywood wizardry?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("They could be using laser beams! Robotics! Ventriloquism! Cloning! For all we know, he could be on steroids! Mr. Benson? Ladies and gentlemen, there's no trickery here. I'm just an ordinary bee. Honey's pretty important to me. It's important to all bees. We invented it! We make it. And we protect it with our lives."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Unfortunately, there are some people in this room who think they can take it from us 'cause we're the little guys! I'm hoping that, after this is all over, you'll see how, by taking our honey, you not only take everything we have but everything we are! I wish he'd dress like that all the time. So nice! Call your first witness."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("So, Mr. Klauss Vanderhayden of Honey Farms, big company you have. I suppose so. I see you also own Honeyburton and Honron! Yes, they provide beekeepers for our farms. Beekeeper. I find that to be a very disturbing term. I don't imagine you employ any bee-free-ers, do you? - No. - I couldn't hear you. - No. - No. Because you don't free bees. You keep bees."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Not only that, it seems you thought a bear would be an appropriate image for a jar of honey. They're very lovable creatures. Yogi Bear, Fozzie Bear, Build-A-Bear. You mean like this? Bears kill bees! How'd you like his head crashing through your living room?! Biting into your couch! Spitting out your throw pillows! OK, that's enough. Take him away."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("So, Mr. Sting, thank you for being here. Your name intrigues me. - Where have I heard it before? - I was with a band called The Police. But you've never been a police officer, have you? No, I haven't. No, you haven't. And so here we have yet another example of bee culture casually stolen by a human for nothing more than a prance-about stage name."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Oh, please. Have you ever been stung, Mr. Sting? Because I'm feeling a little stung, Sting. Or should I say... Mr. Gordon M. Sumner! That's not his real name?! You idiots! Mr. Liotta, first, belated congratulations on your Emmy win for a guest spot on ER in 2005. Thank you. Thank you."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("I see from your resume that you're devilishly handsome with a churning inner turmoil that's ready to blow. I enjoy what I do. Is that a crime? Not yet it isn't. But is this what it's come to for you? Exploiting tiny, helpless bees so you don't have to rehearse your part and learn your lines, sir? Watch it, Benson! I could blow right now!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("This isn't a goodfella. This is a badfella! Why doesn't someone just step on this creep, and we can all go home?! - Order in this court! - You're all thinking it! Order! Order, I say! - Say it! - Mr. Liotta, please sit down! I think it was awfully nice of that bear to pitch in like that. I think the jury's on our side. Are we doing everything right, legally?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("I'm a florist. Right. Well, here's to a great team. To a great team! Well, hello. - Ken! - Hello. I didn't think you were coming. No, I was just late. I tried to call, but... the battery. I didn't want all this to go to waste, so I called Barry. Luckily, he was free. Oh, that was lucky. There's a little left. I could heat it up."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Yeah, heat it up, sure, whatever. So I hear you're quite a tennis player. I'm not much for the game myself. The ball's a little grabby. That's where I usually sit. Right... there. Ken, Barry was looking at your resume, and he agreed with me that eating with chopsticks isn't really a special skill. You think I don't see what you're doing?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("I know how hard it is to find the right job. We have that in common. Do we? Bees have 100 percent employment, but we do jobs like taking the crud out. That's just what I was thinking about doing. Ken, I let Barry borrow your razor for his fuzz. I hope that was all right. I'm going to drain the old stinger. Yeah, you do that. Look at that."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("You know, I've just about had it with your little mind games. - What's that? - Italian Vogue. Mamma mia, that's a lot of pages. A lot of ads. Remember what Van said, why is your life more valuable than mine? Funny, I just can't seem to recall that! I think something stinks in here! I love the smell of flowers. How do you like the smell of flames?!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Not as much. Water bug! Not taking sides! Ken, I'm wearing a Chapstick hat! This is pathetic! I've got issues! Well, well, well, a royal flush! - You're bluffing. - Am I? Surf's up, dude! Poo water! That bowl is gnarly. Except for those dirty yellow rings! Kenneth! What are you doing?! You know, I don't even like honey! I don't eat it! We need to talk!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("He's just a little bee! And he happens to be the nicest bee I've met in a long time! Long time? What are you talking about?! Are there other bugs in your life? No, but there are other things bugging me in life. And you're one of them! Fine! Talking bees, no yogurt night... My nerves are fried from riding on this emotional roller coaster!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Goodbye, Ken. And for your information, I prefer sugar-free, artificial sweeteners made by man! I'm sorry about all that. I know it's got an aftertaste! I like it! I always felt there was some kind of barrier between Ken and me. I couldn't overcome it. Oh, well. Are you OK for the trial? I believe Mr. Montgomery is about out of ideas."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("We would like to call Mr. Barry Benson Bee to the stand. Good idea! You can really see why he's considered one of the best lawyers... Yeah. Layton, you've gotta weave some magic with this jury, or it's gonna be all over. Don't worry."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("The only thing I have to do to turn this jury around is to remind them of what they don't like about bees. - You got the tweezers? - Are you allergic? Only to losing, son. Only to losing. Mr. Benson Bee, I'll ask you what I think we'd all like to know. What exactly is your relationship to that woman?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("We're friends. - Good friends? - Yes. How good? Do you live together? Wait a minute... Are you her little... ...bedbug? I've seen a bee documentary or two. From what I understand, doesn't your queen give birth to all the bee children? - Yeah, but... - So those aren't your real parents! - Oh, Barry... - Yes, they are! Hold me back!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("You're an illegitimate bee, aren't you, Benson? He's denouncing bees! Don't y'all date your cousins? - Objection! - I'm going to pincushion this guy! Adam, don't! It's what he wants! Oh, I'm hit!! Oh, lordy, I am hit! Order! Order! The venom! The venom is coursing through my veins!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("I have been felled by a winged beast of destruction! You see? You can't treat them like equals! They're striped savages! Stinging's the only thing they know! It's their way! - Adam, stay with me. - I can't feel my legs. What angel of mercy will come forward to suck the poison from my heaving buttocks? I will have order in this court. Order!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Order, please! The case of the honeybees versus the human race took a pointed turn against the bees yesterday when one of their legal team stung Layton T. Montgomery. - Hey, buddy. - Hey. - Is there much pain? - Yeah. I... I blew the whole case, didn't I? It doesn't matter. What matters is you're alive. You could have died. I'd be better off dead. Look at me."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("They got it from the cafeteria downstairs, in a tuna sandwich. Look, there's a little celery still on it. What was it like to sting someone? I can't explain it. It was all... All adrenaline and then... and then ecstasy! All right. You think it was all a trap? Of course. I'm sorry. I flew us right into this. What were we thinking? Look at us. We're just a couple of bugs in this world."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("What will the humans do to us if they win? I don't know. I hear they put the roaches in motels. That doesn't sound so bad. Adam, they check in, but they don't check out! Oh, my. Could you get a nurse to close that window? - Why? - The smoke. Bees don't smoke. Right. Bees don't smoke. Bees don't smoke! But some bees are smoking. That's it!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("That's our case! It is? It's not over? Get dressed. I've gotta go somewhere. Get back to the court and stall. Stall any way you can. And assuming you've done step correctly, you're ready for the tub. Mr. Flayman. Yes? Yes, Your Honor! Where is the rest of your team? Well, Your Honor, it's interesting."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Bees are trained to fly haphazardly, and as a result, we don't make very good time. I actually heard a funny story about... Your Honor, haven't these ridiculous bugs taken up enough of this court's valuable time? How much longer will we allow these absurd shenanigans to go on?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("They have presented no compelling evidence to support their charges against my clients, who run legitimate businesses. I move for a complete dismissal of this entire case! Mr. Flayman, I'm afraid I'm going to have to consider Mr. Montgomery's motion. But you can't! We have a terrific case. Where is your proof? Where is the evidence? Show me the smoking gun!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Hold it, Your Honor! You want a smoking gun? Here is your smoking gun. What is that? It's a bee smoker! What, this? This harmless little contraption? This couldn't hurt a fly, let alone a bee. Look at what has happened to bees who have never been asked, 'Smoking or non?' Is this what nature intended for us?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("To be forcibly addicted to smoke machines and man-made wooden slat work camps? Living out our lives as honey slaves to the white man? - What are we gonna do? - He's playing the species card. Ladies and gentlemen, please, free these bees! Free the bees! Free the bees! Free the bees! Free the bees! Free the bees!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("The court finds in favor of the bees! Vanessa, we won! I knew you could do it! High-five! Sorry. I'm OK! You know what this means? All the honey will finally belong to the bees. Now we won't have to work so hard all the time. This is an unholy perversion of the balance of nature, Benson. You'll regret this. Barry, how much honey is out there? All right. One at a time."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Barry, who are you wearing? My sweater is Ralph Lauren, and I have no pants. - What if Montgomery's right? - What do you mean? We've been living the bee way a long time, 27 million years. Congratulations on your victory. What will you demand as a settlement? First, we'll demand a complete shutdown of all bee work camps. Then we want back the honey that was ours to begin with, every last drop."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("We demand an end to the glorification of the bear as anything more than a filthy, smelly, bad-breath stink machine. We're all aware of what they do in the woods. Wait for my signal. Take him out. He'll have nauseous for a few hours, then he'll be fine. And we will no longer tolerate bee-negative nicknames... But it's just a prance-about stage name!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage(" ...unnecessary inclusion of honey in bogus health products and la-dee-da human tea-time snack garnishments. Can't breathe. Bring it in, boys! Hold it right there! Good. Tap it. Mr. Buzzwell, we just passed three cups, and there's gallons more coming! - I think we need to shut down!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Shut down? We've never shut down. Shut down honey production! Stop making honey! Turn your key, sir! What do we do now? Cannonball! We're shutting honey production! Mission abort. Aborting pollination and nectar detail. Returning to base. Adam, you wouldn't believe how much honey was out there. Oh, yeah? What's going on?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Where is everybody? - Are they out celebrating? - They're home. They don't know what to do. Laying out, sleeping in. I heard your Uncle Carl was on his way to San Antonio with a cricket. At least we got our honey back. Sometimes I think, so what if humans liked our honey? Who wouldn't? It's the greatest thing in the world! I was excited to be part of making it."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("This was my new desk. This was my new job. I wanted to do it really well. And now... Now I can't. I don't understand why they're not happy. I thought their lives would be better! They're doing nothing. It's amazing. Honey really changes people. You don't have any idea what's going on, do you? - What did you want to show me? - This. What happened here?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("That is not the half of it. Oh, no. Oh, my. They're all wilting. Doesn't look very good, does it? No. And whose fault do you think that is? You know, I'm gonna guess bees. Bees? Specifically, me. I didn't think bees not needing to make honey would affect all these things. It's not just flowers. Fruits, vegetables, they all need bees."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("That's our whole SAT test right there. Take away produce, that affects the entire animal kingdom. And then, of course... The human species? So if there's no more pollination, it could all just go south here, couldn't it? I know this is also partly my fault. How about a suicide pact? How do we do it? - I'll sting you, you step on me. – That just kills you twice."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Right, right. Listen, Barry... sorry, but I gotta get going. I had to open my mouth and talk. Vanessa? Vanessa? Why are you leaving? Where are you going? To the final Tournament of Roses parade in Pasadena. They've moved it to this weekend because all the flowers are dying. It's the last chance I'll ever have to see it. Vanessa, I just wanna say I'm sorry."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("I never meant it to turn out like this. I know. Me neither. Tournament of Roses. Roses can't do sports. Wait a minute. Roses. Roses? Roses! Vanessa! Roses?! Barry? - Roses are flowers! - Yes, they are. Flowers, bees, pollen! I know. That's why this is the last parade. Maybe not. Could you ask him to slow down? Could you slow down? Barry! OK, I made a huge mistake. This is a total disaster, all my fault."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Yes, it kind of is. I've ruined the planet. I wanted to help you with the flower shop. I've made it worse. Actually, it's completely closed down. I thought maybe you were remodeling. But I have another idea, and it's greater than my previous ideas combined. I don't want to hear it! All right, they have the roses, the roses have the pollen. I know every bee, plant and flower bud in this park."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("All we gotta do is get what they've got back here with what we've got. - Bees. - Park. - Pollen! - Flowers. - Repollination! - Across the nation! Tournament of Roses, Pasadena, California. They've got nothing but flowers, floats and cotton candy. Security will be tight. I have an idea. Vanessa Bloome, FTD. Official floral business. It's real. Sorry, ma'am."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Nice brooch. Thank you. It was a gift. Once inside, we just pick the right float. How about The Princess and the Pea? I could be the princess, and you could be the pea! Yes, I got it. - Where should I sit? - What are you? - I believe I'm the pea. - The pea? It goes under the mattresses. - Not in this fairy tale, sweetheart. - I'm getting the marshal. You do that!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("This whole parade is a fiasco! Let's see what this baby'll do. Hey, what are you doing?! Then all we do is blend in with traffic... ...without arousing suspicion. Once at the airport, there's no stopping us. Stop! Security. - You and your insect pack your float? - Yes. Has it been in your possession the entire time? Would you remove your shoes? - Remove your stinger. - It's part of me."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("I know. Just having some fun. Enjoy your flight. Then if we're lucky, we'll have just enough pollen to do the job. Can you believe how lucky we are? We have just enough pollen to do the job! I think this is gonna work. It's got to work. Attention, passengers, this is Captain Scott. We have a bit of bad weather in New York. It looks like we'll experience a couple hours delay."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Barry, these are cut flowers with no water. They'll never make it. I gotta get up there and talk to them. Be careful. Can I get help with the Sky Mall magazine? I'd like to order the talking inflatable nose and ear hair trimmer. Captain, I'm in a real situation. - What'd you say, Hal? - Nothing. Bee! Don't freak out! My entire species..."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("What are you doing? - Wait a minute! I'm an attorney! - Who's an attorney? Don't move. Oh, Barry. Good afternoon, passengers. This is your captain. Would a Miss Vanessa Bloome in 24B please report to the cockpit? And please hurry! What happened here? There was a DustBuster, a toupee, a life raft exploded. One's bald, one's in a boat, they're both unconscious!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Is that another bee joke? - No! No one's flying the plane! This is JFK control tower, Flight 356. What's your status? This is Vanessa Bloome. I'm a florist from New York. Where's the pilot? He's unconscious, and so is the copilot. Not good. Does anyone onboard have flight experience? As a matter of fact, there is. - Who's that? - Barry Benson. From the honey trial?!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Oh, great. Vanessa, this is nothing more than a big metal bee. It's got giant wings, huge engines. I can't fly a plane. - Why not? Isn't John Travolta a pilot? - Yes. How hard could it be? Wait, Barry! We're headed into some lightning. This is Bob Bumble. We have some late-breaking news from JFK Airport, where a suspenseful scene is developing."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Barry Benson, fresh from his legal victory... That's Barry! ...is attempting to land a plane, loaded with people, flowers and an incapacitated flight crew. Flowers?! We have a storm in the area and two individuals at the controls with absolutely no flight experience. Just a minute. There's a bee on that plane. I'm quite familiar with Mr. Benson and his no-account compadres."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("They've done enough damage. But isn't he your only hope? Technically, a bee shouldn't be able to fly at all. Their wings are too small... Haven't we heard this a million times? 'The surface area of the wings and body mass make no sense.' - Get this on the air! - Got it. - Stand by. - We're going live. The way we work may be a mystery to you."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Making honey takes a lot of bees doing a lot of small jobs. But let me tell you about a small job. If you do it well, it makes a big difference. More than we realized. To us, to everyone. That's why I want to get bees back to working together. That's the bee way! We're not made of Jell-O. We get behind a fellow. - Black and yellow! - Hello!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Left, right, down, hover. - Hover? - Forget hover. This isn't so hard. Beep-beep! Beep-beep! Barry, what happened?! Wait, I think we were on autopilot the whole time. - That may have been helping me. - And now we're not! So it turns out I cannot fly a plane. All of you, let's get behind this fellow! Move it out! Move out!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Our only chance is if I do what I'd do, you copy me with the wings of the plane! Don't have to yell. I'm not yelling! We're in a lot of trouble. It's very hard to concentrate with that panicky tone in your voice! It's not a tone. I'm panicking! I can't do this! Vanessa, pull yourself together. You have to snap out of it!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("You snap out of it. You snap out of it. - You snap out of it! - You snap out of it! - You snap out of it! - You snap out of it! - You snap out of it! - You snap out of it! - Hold it! - Why? Come on, it's my turn. How is the plane flying? I don't know. Hello?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Benson, got any flowers for a happy occasion in there? The Pollen Jocks! They do get behind a fellow. - Black and yellow. - Hello. All right, let's drop this tin can on the blacktop. Where? I can't see anything. Can you? No, nothing. It's all cloudy. Come on. You got to think bee, Barry. - Thinking bee. - Thinking bee. Thinking bee! Thinking bee! Thinking bee! Wait a minute. I think I'm feeling something."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("What? - I don't know. It's strong, pulling me. Like a 27-million-year-old instinct. Bring the nose down. Thinking bee! Thinking bee! Thinking bee! - What in the world is on the tarmac? - Get some lights on that! Thinking bee! Thinking bee! Thinking bee! - Vanessa, aim for the flower. - OK. Out the engines. We're going in on bee power. Ready, boys?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Affirmative! Good. Good. Easy, now. That's it. Land on that flower! Ready? Full reverse! Spin it around! - Not that flower! The other one! - Which one? - That flower. - I'm aiming at the flower! That's a fat guy in a flowered shirt. I mean the giant pulsating flower made of millions of bees! Pull forward. Nose down. Tail up. Rotate around it. - This is insane, Barry!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("This's the only way I know how to fly. Am I koo-koo-kachoo, or is this plane flying in an insect-like pattern? Get your nose in there. Don't be afraid. Smell it. Full reverse! Just drop it. Be a part of it. Aim for the center! Now drop it in! Drop it in, woman! Come on, already. Barry, we did it! You taught me how to fly! - Yes. No high-five! - Right. Barry, it worked!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Did you see the giant flower? What giant flower? Where? Of course I saw the flower! That was genius! - Thank you. - But we're not done yet. Listen, everyone! This runway is covered with the last pollen from the last flowers available anywhere on Earth. That means this is our last chance. We're the only ones who make honey, pollinate flowers and dress like this."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("If we're gonna survive as a species, this is our moment! What do you say? Are we going to be bees, or just Museum of Natural History keychains? We're bees! Keychain! Then follow me! Except Keychain. Hold on, Barry. Here. You've earned this. Yeah! I'm a Pollen Jock! And it's a perfect fit. All I gotta do are the sleeves. Oh, yeah. That's our Barry."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Mom! The bees are back! If anybody needs to make a call, now's the time. I got a feeling we'll be working late tonight! Here's your change. Have a great afternoon! Can I help who's next? Would you like some honey with that? It is bee-approved. Don't forget these. Milk, cream, cheese, it's all me. And I don't see a nickel! Sometimes I just feel like a piece of meat!"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("I had no idea. Barry, I'm sorry. Have you got a moment? Would you excuse me? My mosquito associate will help you. Sorry I'm late. He's a lawyer too? I was already a blood-sucking parasite. All I needed was a briefcase. Have a great afternoon! Barry, I just got this huge tulip order, and I can't get them anywhere. No problem, Vannie. Just leave it to me."));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("You're a lifesaver, Barry. Can I help who's next? All right, scramble, jocks! It's time to fly. Thank you, Barry! That bee is living my life! Let it go, Kenny. - When will this nightmare end?! - Let it all go. - Beautiful day to fly. - Sure is. Between you and me, I was dying to get out of that office. You have got to start thinking bee, my friend. - Thinking bee! - Me?"));
			ChatHelper.SendChatMessageFromClient(ChatManager.Commands.CreateOutgoingMessage("Hold it. Let's just stop for a second. Hold it. I'm sorry. I'm sorry, everyone. Can we stop here? I'm not making a major life decision during a production number! All right. Take ten, everybody. Wrap it up, guys. I had virtually no rehearsal for that."));
		}
	}

	public void HandleExtendedChatUIV2(Rectangle mouse, List<ChatMessageContainer> rawMessageArray, ChatMessageContainer chatMessageContainer, int loopForChatPos)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.drawingPlayerChat)
		{
			return;
		}
		Color baseColor = Color.White;
		float num = FontAssets.MouseText.Value.MeasureString("☰").X * 0.7f;
		float num2 = FontAssets.MouseText.Value.MeasureString("☰").Y * 0.7f;
		int snippetCount = ChatManager.ParseMessage(chatMessageContainer.OriginalText, Color.White).Count;
		Rectangle value = default(Rectangle);
		for (int i = 0; i < 255; i++)
		{
			if (Main.player[i].name.Length <= 0 || snippetCount < 1 || !chatMessageContainer.OriginalText.StartsWith(NameTagHandler.GenerateTag(Main.player[i].name)))
			{
				continue;
			}
			string nameTag = NameTagHandler.GenerateTag(Main.player[i].name);
			((Rectangle)(ref value))._002Ector(85 - (int)num, Main.screenHeight - 30 - 28 - loopForChatPos * 21, (int)num, (int)num2);
			if (((Rectangle)(ref mouse)).Intersects(value))
			{
				Main.LocalPlayer.mouseInterface = true;
				baseColor = Color.DarkGray;
				if (Main.mouseLeft && Main.mouseLeftRelease && ((onTshockServer && Main.chatText.Length + nameTag.Length < 500) || !onTshockServer))
				{
					ChatManager.AddChatText(FontAssets.MouseText.Value, chatMessageContainer.OriginalText.Substring(nameTag.Length + 1), Vector2.One);
				}
			}
			float adjustedY = (float)(Main.screenHeight - 30 - Main.lineCount * 28 - loopForChatPos * 21) + 2.1f;
			ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, "☰", new Vector2(85f - num, adjustedY), baseColor, 0f, Vector2.Zero, new Vector2(0.7f, 0.7f));
		}
	}

	public static async void LagOutServerPlayersUnlessTheyHaveCoolClient()
	{
		_ = Main.LocalPlayer;
		if (Main.gameMenu || Main.netMode == 0)
		{
			return;
		}
		Main.NewText("Can be used to force other players to load your desired chunk data. You can help them reveal the map.", zazasFavoriteColor);
		Main.NewText("Can be used to crash other players games if used maliciously with bad tile values.", zazasFavoriteColor);
		Main.NewText("Didn't finish.", zazasFavoriteColor);
		Vector2 vector = default(Vector2);
		for (int i = 0; i < 2; i++)
		{
			for (int j = 40; j < Main.maxTilesX; j += 200)
			{
				for (int k = 40; k < Main.maxTilesY; k += 150)
				{
					((Vector2)(ref vector))._002Ector((float)j * 16f, (float)k * 16f);
					Main.projectile[1000].whoAmI = Projectile.NewProjectile(Main.LocalPlayer.GetProjectileSource_Misc(602), vector, Vector2.Zero, 602, 0, 0f, Main.myPlayer);
					await Task.Delay(50);
				}
			}
		}
	}

	public void KillBubble(int radius)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < Main.npc.Length; i++)
		{
			NPC nPC = Main.npc[i];
			Main.LocalPlayer.position.ToTileCoordinates();
			Point entity = nPC.position.ToTileCoordinates();
			if (WithinPlayerRadius(radius, entity) && !nPC.friendly && nPC.active && nPC.type != 488)
			{
				int num = 1500;
				nPC.StrikeNPCNoInteraction(num, 0.5f, -nPC.direction, crit: true);
				if (Main.netMode != 0)
				{
					NetMessage.SendData(28, -1, -1, null, i, num, 0.5f, 0f - (float)nPC.direction, 1);
				}
			}
		}
	}

	public bool WithinPlayerRadius(int radius, Point entity)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Point point = Main.LocalPlayer.position.ToTileCoordinates();
		if (entity.X >= point.X - radius && entity.X <= point.X + radius && entity.Y >= point.Y - radius)
		{
			return entity.Y <= point.Y + radius;
		}
		return false;
	}

	public void PullItemsToPlayer(float speed, int acc)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Main.LocalPlayer;
		Vector2 vector = default(Vector2);
		for (int i = 0; i < 400; i++)
		{
			Item item = Main.item[i];
			Player.ItemSpaceStatus status = localPlayer.ItemSpace(item);
			int num = 600;
			if (item.active && item.shimmerTime == 0f && localPlayer.CanAcceptItemIntoInventory(item) && localPlayer.CanPullItem(item, status))
			{
				Rectangle val = new Rectangle((int)localPlayer.position.X - num, (int)localPlayer.position.Y - num, localPlayer.width + num * 2, localPlayer.height + num * 2);
				if (((Rectangle)(ref val)).Intersects(item.Hitbox))
				{
					item.beingGrabbed = true;
					((Vector2)(ref vector))._002Ector(item.position.X + (float)(item.width / 2), item.position.Y + (float)(item.height / 2));
					float num2 = localPlayer.position.X - vector.X;
					float num3 = localPlayer.position.Y - vector.Y;
					float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
					num4 = speed / num4;
					num2 *= num4;
					num3 *= num4;
					item.velocity.X = (item.velocity.X * (float)(acc - 1) + num2) / (float)acc;
					item.velocity.Y = (item.velocity.Y * (float)(acc - 1) + num3) / (float)acc;
				}
			}
		}
	}

	private void GetItemTypeFromWallType()
	{
		StreamWriter streamWriter = new StreamWriter("C:\\Users\\lucar\\Desktop\\WallsID to ItemID.txt");
		int num = 0;
		for (int i = 0; i < ItemID.Count; i++)
		{
			Item item = new Item();
			item.SetDefaults(i);
			if (item.type == 85 || item.createWall > 0 || (item.Name.Contains("Rope") && !item.Name.Contains("Coil")) || item.Name.Contains("Fire Block") || item.Name.Contains("Ichor Block") || ((item.Name.Contains("Beam") || item.Name.Contains("Column") || item.Name.Contains("Bubble")) && item.createTile > 0) || item.Name.Contains("Streamer") || item.Name.Contains("Cobweb") || item.Name.Contains("Active Stone") || item.Name.Contains("Smoke Block"))
			{
				string text = ((item.createWall > 0) ? "Wall" : "Tile");
				streamWriter.Write("if (!nonSolidTileIDToItemID.ContainsKey(" + num + "))  {nonSolidTileIDToItemID.Add(" + ((item.createWall > 0) ? item.createWall : item.createTile) + "," + item.type + "); } //\t\t" + text + " Name: " + ((item.createWall > 0) ? "FuckWall Names" : TileID.Search.GetName(item.createTile)) + " || Item Name: " + item.Name);
				num++;
			}
		}
		streamWriter.Close();
	}

	private static void EasyCommand1(string[] array, ref bool enable, ref int size, string commandName)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (array.Length <= 1)
		{
			enable = !enable;
			Main.NewText(commandName + (enable ? " En" : " Dis") + "abled" + (enable ? ("  Size: " + size) : ""), zazasFavoriteColor);
		}
		else if (int.TryParse(array[1], out size))
		{
			enable = true;
			Main.NewText(commandName + (enable ? " En" : " Dis") + "abled  Size: " + size, zazasFavoriteColor);
		}
		else
		{
			Main.NewText("Invalid Value ", errorColor);
		}
	}

	private void GetItemTypeFromWallTypeV2()
	{
		StreamWriter streamWriter = new StreamWriter("C:\\Users\\bigbootyfatjigglygiganticplanetsizeabsolutelybonkerslatinabitchesonmydick\\Desktop\\WallsID to ItemID V2.txt");
		for (int i = 0; i < ItemID.Count; i++)
		{
			Item item = new Item();
			item.SetDefaults(i);
			if (item.createWall > 0)
			{
				streamWriter.Write("wallIDToItemID.Add(" + item.createWall + "," + item.type + "); } //\t\tWall Name: FuckWall Names || Item Name: " + item.Name + "\n");
			}
		}
		streamWriter.Close();
	}

	public Vector2 SpectateAndFreecam()
	{
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			spectatingPlayer = false;
			freeCam = false;
			freeCam2 = false;
			spectatePlayerTarget = -1;
		}
		if (spectatePlayerTarget != -1 && !Main.player[spectatePlayerTarget].active)
		{
			spectatePlayerTarget = -1;
			showSpectatedInventory = false;
		}
		if (!freeCam)
		{
			Vector3 vector = default(Vector3);
			((Vector3)(ref vector))._002Ector(1f, 1f, 1f);
			Vector3 vector2 = Vector3.One / vector;
			int num = 21;
			savedFreeCam.X = Main.LocalPlayer.position.X + (float)Main.LocalPlayer.width * 0.5f - (float)Main.screenWidth * 0.5f * vector2.X + Main.cameraX;
			savedFreeCam.Y = Main.LocalPlayer.position.Y + (float)Main.LocalPlayer.height - (float)num - (float)Main.screenHeight * 0.5f * vector2.Y + Main.LocalPlayer.gfxOffY;
		}
		Vector2 result = default(Vector2);
		if (spectatingPlayer || (uiRework && spectatePlayerTarget != -1))
		{
			if (freeCam)
			{
				spectatingPlayer = false;
				spectatePlayerTarget = -1;
			}
			Player player = Main.player[(uiRework && spectatePlayerTarget != -1) ? spectatePlayerTarget : storePlrWhoAmI];
			if (player.position.X < Main.leftWorld || player.BottomRight.X > Main.rightWorld || player.position.Y < Main.topWorld || player.BottomRight.Y > Main.bottomWorld)
			{
				spectatingPlayer = false;
				spectatePlayerTarget = -1;
				Vector3 vector5 = default(Vector3);
				((Vector3)(ref vector5))._002Ector(1f, 1f, 1f);
				Vector3 vector6 = Vector3.One / vector5;
				int num4 = 21;
				Player localPlayer = Main.LocalPlayer;
				result.X = localPlayer.position.X + (float)localPlayer.width * 0.5f - (float)Main.screenWidth * 0.5f * vector6.X + Main.cameraX;
				result.Y = localPlayer.position.Y + (float)localPlayer.height - (float)num4 - (float)Main.screenHeight * 0.5f * vector6.Y + localPlayer.gfxOffY;
				return result;
			}
			savedFreeCam = player.Center;
			if (uiRework && spectatePlayerTarget != -1)
			{
				player = Main.player[spectatePlayerTarget];
			}
			if (Main.netMode == 1 && !loadingChunks)
			{
				LoadChunksWithGayAssDrone();
			}
			Vector3 vector7 = default(Vector3);
			((Vector3)(ref vector7))._002Ector(1f, 1f, 1f);
			Vector3 vector8 = Vector3.One / vector7;
			int num5 = 21;
			result.X = player.position.X + (float)player.width * 0.5f - (float)Main.screenWidth * 0.5f * vector8.X + Main.cameraX;
			result.Y = player.position.Y + (float)player.height - (float)num5 - (float)Main.screenHeight * 0.5f * vector8.Y + player.gfxOffY;
			Main.LocalPlayer.isOperatingAnotherEntity = true;
			if (freeCamDebug)
			{
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.TerraBlade, new ParticleOrchestraSettings
				{
					PositionInWorld = savedFreeCam
				});
			}
		}
		else
		{
			if (freeCam)
			{
				if (spectatingPlayer || (spectatePlayerTarget != -1 && uiRework))
				{
					freeCam = false;
				}
				_ = Main.player[Main.myPlayer];
				int num6 = (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) ? 9 : (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)160) ? 64 : 25));
				if (!Main.gamePaused && !Main.drawingPlayerChat && !Main.ingameOptionsWindow)
				{
					if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)87) || ((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)32))
					{
						savedFreeCam = new Vector2(savedFreeCam.X, savedFreeCam.Y - (float)num6);
					}
					if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)65))
					{
						savedFreeCam = new Vector2(savedFreeCam.X - (float)num6, savedFreeCam.Y);
					}
					if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)68))
					{
						savedFreeCam = new Vector2(savedFreeCam.X + (float)num6, savedFreeCam.Y);
					}
					if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)83))
					{
						savedFreeCam = new Vector2(savedFreeCam.X, savedFreeCam.Y + (float)num6);
					}
					Main.LocalPlayer.isOperatingAnotherEntity = true;
					if (worldBorderSize > -2)
					{
						Vector2 input = new Vector2(Main.leftWorld, Main.topWorld) - Main.GameViewMatrix.Translation;
						Vector2 input2 = new Vector2(Main.rightWorld - (float)Main.screenWidth / Main.GameViewMatrix.Zoom.X - 0f, Main.bottomWorld - (float)Main.screenHeight / Main.GameViewMatrix.Zoom.Y - 0f) - Main.GameViewMatrix.Translation;
						input = Utils.Round(input);
						input2 = Utils.Round(input2);
						savedFreeCam = Vector2.Clamp(savedFreeCam, input, input2);
					}
				}
				if (Main.netMode == 1 && !freeCam2 && !loadingChunks)
				{
					LoadChunksWithGayAssDrone();
				}
				if (freeCamDebug)
				{
					result.X = savedFreeCam.X + (float)Main.screenWidth * 0.5f + Main.cameraX;
					result.Y = savedFreeCam.Y + (float)Main.screenHeight * 0.5f + Main.LocalPlayer.gfxOffY;
					ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.TerraBlade, new ParticleOrchestraSettings
					{
						PositionInWorld = result
					});
				}
				return savedFreeCam;
			}
			Vector3 vector9 = default(Vector3);
			((Vector3)(ref vector9))._002Ector(1f, 1f, 1f);
			Vector3 vector10 = Vector3.One / vector9;
			int num7 = 21;
			Player localPlayer2 = Main.LocalPlayer;
			result.X = localPlayer2.position.X + (float)localPlayer2.width * 0.5f - (float)Main.screenWidth * 0.5f * vector10.X + Main.cameraX;
			result.Y = localPlayer2.position.Y + (float)localPlayer2.height - (float)num7 - (float)Main.screenHeight * 0.5f * vector10.Y + localPlayer2.gfxOffY;
		}
		return result;
	}

	private void ReadLineTesting()
	{
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		using StreamReader streamReader = new StreamReader("KeyBindsForZazaMod.txt");
		if (keyBindMaster != null && keyBindMaster.Count > 0)
		{
			keyBindMaster.Clear();
		}
		int num = 0;
		while (streamReader.Peek() >= 0)
		{
			string text = streamReader.ReadLine();
			int num2 = text.IndexOf(":");
			if (num2 <= 0)
			{
				continue;
			}
			string text2 = text.Substring(num2 + 1).Trim();
			Keys result;
			if (string.IsNullOrEmpty(text2))
			{
				if (!keyBindMaster.ContainsKey(num))
				{
					Tuple<Keys, object> value = Tuple.Create<Keys, object>((Keys)0, "");
					keyBindMaster.Add(num, value);
					Main.NewText("Keybind Master[" + num + "]: " + keyBindMaster[num]);
					num++;
				}
			}
			else if (Enum.TryParse<Keys>(text2, out result))
			{
				if (!keyBindMaster.ContainsKey(num))
				{
					Tuple<Keys, object> value2 = Tuple.Create<Keys, object>(result, "");
					keyBindMaster.Add(num, value2);
					Main.NewText("Keybind Master[" + num + "]: " + keyBindMaster[num]);
					num++;
				}
			}
			else if (text2.Contains("+") && text2.Contains("Button"))
			{
				string text3 = text2.Substring(0, text2.IndexOf("+") - 1);
				string text4 = text2.Substring(text2.IndexOf("+") + 1).Trim();
				bool flag = text4.Contains("Button");
				if (Enum.TryParse<Keys>(flag ? text3 : text4, out result) && !keyBindMaster.ContainsKey(num))
				{
					Tuple<Keys, object> value3 = Tuple.Create<Keys, object>(result, (!flag) ? text3 : text4);
					keyBindMaster.Add(num, value3);
					Main.NewText("Keybind Master[" + num + "]: " + keyBindMaster[num]);
					num++;
				}
			}
			else if (!text2.Contains("+") && text2.Contains("Button"))
			{
				if (!keyBindMaster.ContainsKey(num))
				{
					Tuple<Keys, object> value4 = Tuple.Create<Keys, object>((Keys)0, text2);
					keyBindMaster.Add(num, value4);
					Main.NewText("Keybind Master[" + num + "]: " + keyBindMaster[num]);
					num++;
				}
			}
			else if (text2.Contains("+") && !text2.Contains("Button"))
			{
				string value5 = text2.Substring(0, text2.IndexOf("+") - 1);
				string value6 = text2.Substring(text2.IndexOf("+") + 1);
				if (Enum.TryParse<Keys>(value5, out Keys result2) && Enum.TryParse<Keys>(value6, out Keys result3) && !keyBindMaster.ContainsKey(num))
				{
					Tuple<Keys, object> value7 = Tuple.Create<Keys, object>(result2, result3);
					keyBindMaster.Add(num, value7);
					Main.NewText("Keybind Master[" + num + "]: " + keyBindMaster[num]);
					num++;
				}
			}
			else if (text2.Contains("True") || text2.Contains("False"))
			{
				if (!keyBindMaster.ContainsKey(num))
				{
					bool flag2 = text2.Contains("True");
					Tuple<Keys, object> value8 = Tuple.Create<Keys, object>((Keys)0, flag2);
					keyBindMaster.Add(num, value8);
					Main.NewText("Keybind Master[" + num + "]: " + keyBindMaster[num]);
					num++;
				}
			}
			else
			{
				Main.NewText("Bind List Error Line[" + num + "]. Fix your .txt file ");
			}
		}
	}

	private bool PressingWithBind(int line)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Invalid comparison between Unknown and I4
		string text = keyBindMaster[line].Item2.ToString();
		bool flag = (int)keyBindMaster[line].Item1 == 0 || ((KeyboardState)(ref Main.keyState)).IsKeyDown(keyBindMaster[line].Item1);
		if ((text == "LeftButton") & flag)
		{
			return (int)((MouseState)(ref PlayerInput.MouseInfo)).LeftButton == 1;
		}
		if ((text == "RightButton") & flag)
		{
			return (int)((MouseState)(ref PlayerInput.MouseInfo)).RightButton == 1;
		}
		if ((text == "MiddleButton") & flag)
		{
			return (int)((MouseState)(ref PlayerInput.MouseInfo)).MiddleButton == 1;
		}
		return text == "";
	}

	public bool IsBindingActivated(int line)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Invalid comparison between Unknown and I4
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Invalid comparison between Unknown and I4
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Invalid comparison between Unknown and I4
		string text = keyBindMaster[line].Item2.ToString();
		if ((int)keyBindMaster[line].Item1 == 0 && text == "")
		{
			return false;
		}
		if (line == 10)
		{
			return (bool)keyBindMaster[line].Item2;
		}
		if (keyBindMaster[line].Item2 is Keys)
		{
			if ((line == 0 && !(bool)keyBindMaster[10].Item2) || line == 1 || line == 2 || line == 3 || line == 4 || line == 7)
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown(keyBindMaster[line].Item1) && ((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)keyBindMaster[line].Item2))
				{
					return ((KeyboardState)(ref Main.oldKeyState)).IsKeyUp((Keys)keyBindMaster[line].Item2);
				}
				return false;
			}
			if (((KeyboardState)(ref Main.keyState)).IsKeyDown(keyBindMaster[line].Item1))
			{
				return ((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)keyBindMaster[line].Item2);
			}
			return false;
		}
		bool flag = ((((line == 0 && !(bool)keyBindMaster[10].Item2) || line == 1 || line == 2 || line == 3 || line == 4 || line == 7) && (int)keyBindMaster[line].Item1 != 0) ? (((KeyboardState)(ref Main.keyState)).IsKeyDown(keyBindMaster[line].Item1) && ((KeyboardState)(ref Main.oldKeyState)).IsKeyUp(keyBindMaster[line].Item1)) : ((int)keyBindMaster[line].Item1 == 0 || ((KeyboardState)(ref Main.keyState)).IsKeyDown(keyBindMaster[line].Item1)));
		return text switch
		{
			"LeftButton" => ((int)((MouseState)(ref PlayerInput.MouseInfo)).LeftButton == 1) & flag, 
			"RightButton" => ((int)((MouseState)(ref PlayerInput.MouseInfo)).RightButton == 1) & flag, 
			"MiddleButton" => ((int)((MouseState)(ref PlayerInput.MouseInfo)).MiddleButton == 1) & flag, 
			_ => flag, 
		};
	}

	public static void ModToggler(string cmdName, ref bool boolean)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		boolean = !boolean;
		Main.NewText(cmdName + " " + (boolean ? "en" : "dis") + "abled.", zazasFavoriteColor);
	}

	public static void ModTogglerV2(ICheat cheat)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if (cheat is Cheats.SimpleCheat)
		{
			Cheats.SimpleCheat sCheat = (Cheats.SimpleCheat)cheat;
			sCheat.Enabled = !sCheat.Enabled;
			Main.NewText(sCheat.Name + " " + (sCheat.Enabled ? "en" : "dis") + "abled.", zazasFavoriteColor);
		}
	}

	public static void MouseTP(Vector2 pos)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (noclip)
		{
			Instance.OldCenterMod = pos;
		}
		else
		{
			Main.LocalPlayer.Center = pos;
		}
		if (Main.netMode != 0)
		{
			NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
		}
		Main.LocalPlayer.fallStart = (int)(Main.LocalPlayer.position.Y / 16f);
	}

	public static void TeleportWithSpawnPacket(Vector2? pos = null)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		int spawnX = Main.LocalPlayer.SpawnX;
		int spawnY = Main.LocalPlayer.SpawnY;
		if (!pos.HasValue)
		{
			Main.LocalPlayer.SpawnX = (int)(Main.LocalPlayer.Bottom.X / 16f);
			Main.LocalPlayer.SpawnY = (int)(Main.LocalPlayer.Bottom.Y / 16f);
		}
		else
		{
			Main.LocalPlayer.SpawnX = (int)(pos.Value.X / 16f);
			Main.LocalPlayer.SpawnY = (int)(pos.Value.Y / 16f);
		}
		NetMessage.SendData(12, -1, -1, null, Main.myPlayer);
		Main.LocalPlayer.SpawnX = spawnX;
		Main.LocalPlayer.SpawnY = spawnY;
	}

	public void HandleAndInterpretKeyBinds()
	{
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		using StreamReader streamReader = new StreamReader("KeyBindsForZazaMod.txt");
		if (keyBindMaster != null && keyBindMaster.Count > 0)
		{
			keyBindMaster.Clear();
		}
		int num = 0;
		while (streamReader.Peek() >= 0)
		{
			string text = streamReader.ReadLine();
			int num2 = text.IndexOf(":");
			if (num2 <= 0)
			{
				continue;
			}
			string text2 = text.Substring(num2 + 1).Trim();
			Keys result;
			if (string.IsNullOrEmpty(text2))
			{
				if (!keyBindMaster.ContainsKey(num))
				{
					Tuple<Keys, object> value = Tuple.Create<Keys, object>((Keys)0, "");
					keyBindMaster.Add(num, value);
					if (!Main.gameMenu)
					{
						Main.NewText("Keybind Master[" + num + "]: " + keyBindMaster[num]);
					}
					num++;
				}
			}
			else if (Enum.TryParse<Keys>(text2, out result))
			{
				if (!keyBindMaster.ContainsKey(num))
				{
					Tuple<Keys, object> value2 = Tuple.Create<Keys, object>(result, "");
					keyBindMaster.Add(num, value2);
					if (!Main.gameMenu)
					{
						Main.NewText("Keybind Master[" + num + "]: " + keyBindMaster[num]);
					}
					num++;
				}
			}
			else if (text2.Contains("+") && text2.Contains("Button"))
			{
				string text3 = text2.Substring(0, text2.IndexOf("+") - 1);
				string text4 = text2.Substring(text2.IndexOf("+") + 1).Trim();
				bool flag = text4.Contains("Button");
				if (Enum.TryParse<Keys>(flag ? text3 : text4, out result) && !keyBindMaster.ContainsKey(num))
				{
					Tuple<Keys, object> value3 = Tuple.Create<Keys, object>(result, (!flag) ? text3 : text4);
					keyBindMaster.Add(num, value3);
					if (!Main.gameMenu)
					{
						Main.NewText("Keybind Master[" + num + "]: " + keyBindMaster[num]);
					}
					num++;
				}
			}
			else if (!text2.Contains("+") && text2.Contains("Button"))
			{
				if (!keyBindMaster.ContainsKey(num))
				{
					Tuple<Keys, object> value4 = Tuple.Create<Keys, object>((Keys)0, text2);
					keyBindMaster.Add(num, value4);
					if (!Main.gameMenu)
					{
						Main.NewText("Keybind Master[" + num + "]: " + keyBindMaster[num]);
					}
					num++;
				}
			}
			else if (text2.Contains("+") && !text2.Contains("Button"))
			{
				string value5 = text2.Substring(0, text2.IndexOf("+") - 1);
				string value6 = text2.Substring(text2.IndexOf("+") + 1);
				if (Enum.TryParse<Keys>(value5, out Keys result2) && Enum.TryParse<Keys>(value6, out Keys result3) && !keyBindMaster.ContainsKey(num))
				{
					Tuple<Keys, object> value7 = Tuple.Create<Keys, object>(result2, result3);
					keyBindMaster.Add(num, value7);
					if (!Main.gameMenu)
					{
						Main.NewText("Keybind Master[" + num + "]: " + keyBindMaster[num]);
					}
					num++;
				}
			}
			else if (text2.Contains("True") || text2.Contains("False"))
			{
				if (!keyBindMaster.ContainsKey(num))
				{
					bool flag2 = !string.IsNullOrEmpty(text2) && text2.Contains("True");
					Tuple<Keys, object> value8 = Tuple.Create<Keys, object>((Keys)0, flag2);
					keyBindMaster.Add(num, value8);
					if (!Main.gameMenu)
					{
						Main.NewText("Keybind Master[" + num + "]: " + keyBindMaster[num]);
					}
					num++;
				}
			}
			else
			{
				Main.NewText("Bind List Error Line[" + num + "]. Fix your .txt file ", errorColor);
			}
		}
	}

	private void ActivateTileAndWallSearchDict()
	{
		for (int i = 0; i < ItemID.Count; i++)
		{
			Item item = new Item();
			item.SetDefaults(i);
			if (item.createWall > 0 && !wallToItemDictionarySearch.ContainsKey(item.createWall))
			{
				wallToItemDictionarySearch.Add(item.createWall, item.type);
			}
			else if (item.createTile >= 0 && !tileToItemDictionarySearch.ContainsKey(item.createTile))
			{
				tileToItemDictionarySearch.Add(item.createTile, item.type);
			}
		}
	}

	public void DisableMostModHacks()
	{
		godMode = false;
		antiDeathGod = false;
		shimmerGodMode = false;
		dodgeEverything = false;
		antiDebuff = false;
		deathItemDropMode = 0;
		persistBuffsMode = 0;
		unrestrictTeleportRod = false;
		infiniteMana = false;
		fullBright = false;
		journeyMenu = false;
		instantRespawn = false;
		extendedPickUpRange = false;
		extendedPlacementRange = false;
		infiniteMinions = false;
		infiniteAmmo = false;
		ignoreLiquids = false;
		ignoreDamageBlocks = false;
		dreamLuckMode = 0;
		fullBrightPercent = 0f;
		noSwapRestriction = false;
		LockOnHelper.ForceUsability = false;
		suppressCreativeShock = false;
		suppressHorrified = false;
		ignoreCC = false;
		cancelAnyBuff = false;
		notifyPacket4 = false;
		ModsVars.playerHacksButtonAllMelee = true;
		ModsVars.playerHacksButtonAllRanged = true;
		ModsVars.playerHacksButtonAllMagic = true;
		ModsVars.playerHacksButtonAllSummoner = true;
		ModsUICommandPanelButtons.ToggleAllMeleeAccs(ModsVars.playerHacksButtonAllMelee);
		ModsUICommandPanelButtons.ToggleAllRangedAccs(ModsVars.playerHacksButtonAllRanged);
		ModsUICommandPanelButtons.ToggleAllMagicAccs(ModsVars.playerHacksButtonAllMagic);
		ModsUICommandPanelButtons.ToggleAllSummonAccs(ModsVars.playerHacksButtonAllSummoner);
		ModsVars.playerHacksButtonAllMelee = false;
		ModsVars.playerHacksButtonAllRanged = false;
		ModsVars.playerHacksButtonAllMagic = false;
		ModsVars.playerHacksButtonAllSummoner = false;
		ModsVars.playerHacksButtonAllMelee = true;
		ModsVars.playerHacksButtonAllRanged = true;
		ModsVars.playerHacksButtonAllMagic = true;
		ModsVars.playerHacksButtonAllSummoner = true;
		infiniteReach = false;
		mapTeleportation = false;
		gotoMouse = false;
		noclip = false;
		bypassTeleportRequests = false;
		rgbPlayerOutline = false;
		noScreenClamp = false;
		playerTracker = false;
		autoButcher = false;
		killAura = false;
		crystalKillAura = false;
		playerVitalStatsUI = false;
		autoKillProjectiles = false;
		freeCam = false;
		freeCam2 = false;
		copyMode = false;
		pasteMode = false;
		tileBrush = false;
		eraseBrush = false;
		liquidBrush = false;
		selectionMode = false;
		tileExploitBrush = false;
		spectatingPlayer = false;
		spectatePlayerTarget = -1;
		shimmerWalkParticles = false;
		extraAccSlots = false;
		grappleMounted = false;
	}

	public async void FillTilesSelectionModeV2()
	{
		await Task.Run(delegate
		{
			//IL_0476: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			int num = Math.Min(selectionA.X, selectionB.X);
			int num2 = Math.Min(selectionA.Y, selectionB.Y);
			int num3 = Math.Abs(selectionA.X - selectionB.X);
			int num4 = Math.Abs(selectionA.Y - selectionB.Y);
			int selectedItem = Main.LocalPlayer.selectedItem;
			int createTile = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].createTile;
			int createWall = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].createWall;
			int num5 = 0;
			if (cancelAction)
			{
				cancelAction = false;
			}
			for (int i = num; i <= num + num3; i++)
			{
				int j;
				for (j = num2; j <= num2 + num4; j++)
				{
					if (cancelAction)
					{
						NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
						Main.NewText("Caneled filling tiles.", zazasFavoriteColor);
						cancelAction = false;
						return;
					}
					if (WorldGen.InWorld(i, j))
					{
						bool flag = false;
						Tile tile = Main.tile[i, j];
						if (Main.gameMenu)
						{
							Main.NewText("You can't do this here! (This should never appear)", errorColor);
							return;
						}
						if (tile == null)
						{
							tile = default(Tile);
						}
						if (createTile >= 0)
						{
							if (tile.type != createTile && tile.type >= 0 && createWall < 0)
							{
								tile.ClearTile();
							}
							if (!tile.active())
							{
								tile.active(active: true);
								tile.type = (ushort)createTile;
								Main.QueueMainThreadAction(delegate
								{
									WorldGen.SquareTileFrame(i, j, resetFrame: true, noBreak: true);
								});
								flag = true;
								if (Main.netMode != 0 && !selectionTileSyncingMode)
								{
									FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), selectedItem);
									NetMessage.SendData(17, -1, -1, null, 1, i, j, createTile);
								}
							}
						}
						else if (createWall > 0)
						{
							if (tile.wall != Main.LocalPlayer.HeldItem.createWall && tile.wall > 0)
							{
								tile.Clear(TileDataType.Wall);
							}
							if (tile.wall == 0)
							{
								tile.wall = (ushort)createWall;
								Main.QueueMainThreadAction(delegate
								{
									WorldGen.SquareWallFrame(i, j);
								});
								flag = true;
								if (Main.netMode != 0 && !selectionTileSyncingMode)
								{
									FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), selectedItem);
									NetMessage.SendData(17, -1, -1, null, 3, i, j, createWall);
								}
							}
						}
						if (flag)
						{
							if (!selectionTileSyncingMode)
							{
								if (!tilePacketMaxMode)
								{
									if ((Main.netMode != 0 && num5 % tilePacketRate == 0) || IsLowFPS)
									{
										Thread.Sleep(100);
									}
								}
								else
								{
									while ((Main.netMode != 0 && (quedPackets > tilePacketRate || Netplay.Connection.TimeOutTimer > 60)) || IsLowFPS)
									{
										Thread.Sleep(500);
									}
								}
							}
							Main.Map.UpdateLighting(i, j, byte.MaxValue);
							Main.refreshMap = true;
							num5++;
						}
					}
				}
			}
			if (Main.netMode != 0 && selectionTileSyncingMode)
			{
				for (int num6 = num; num6 <= num + num3; num6 += 10)
				{
					for (int num7 = num2; num7 <= num2 + num4; num7 += 10)
					{
						NetMessage.SendTileSquare(-1, num6, num7, 10, 10);
						while (quedPackets > tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS)
						{
							Thread.Sleep(500);
						}
					}
				}
			}
			Main.NewText(num5 + " Tiles Filled.", zazasFavoriteColor);
			NetMessage.SendData(13);
		});
	}

	public static async void WorldDownload()
	{
		try
		{
			if (savingTEDITSchematic)
			{
				Main.NewText("You are saving a schematic, please wait for that operation to finish.", errorColor);
				return;
			}
			if (worldDownload)
			{
				Main.NewText("You are already saving a world! Please wait for the operation to finish.", errorColor);
				return;
			}
			worldDownload = true;
			await Task.Run(delegate
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
				if (Main.gameMenu)
				{
					Main.NewText("Can't do this right now! (This should never appear)", errorColor);
					worldDownload = false;
				}
				else
				{
					if (Instance.DetermineUnloadedSections > 0)
					{
						Main.NewText($"{Instance.DetermineUnloadedSections} sections are unloaded. Your world downlad will be incomplete!", infoColor);
					}
					Guid uniqueId = Main.ActiveWorldFileData.UniqueId;
					Main.ActiveWorldFileData = WorldFile.CreateMetadata(Main.worldName, SocialAPI.Cloud != null && SocialAPI.Cloud.EnabledByDefault, Main.GameMode);
					Main.ActiveWorldFileData.SetSeed("0");
					Main.ActiveWorldFileData.UniqueId = uniqueId;
					Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
					WorldFile.SaveWorld();
					Main.NewText("World download complete!", zazasFavoriteColor);
					worldDownload = false;
				}
			});
			worldDownload = false;
		}
		catch (Exception ex)
		{
			Main.NewText($"An error occured during world download!\n{ex}", errorColor);
			LogException(ex);
			worldDownload = false;
		}
		worldDownload = false;
	}

	public static async void SaveMapImage()
	{
		if (Main.worldName == "" || Main.ActiveWorldFileData == null)
		{
			Main.NewText("There is no world data available, or the world name is blank.", errorColor);
			return;
		}
		if (MapHelper.colorLookup == null)
		{
			MapHelper.Initialize();
		}
		await Task.Run(delegate
		{
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			SKBitmap val = null;
			SKImage val2 = null;
			SKData val3 = null;
			FileStream fileStream = null;
			try
			{
				int maxTilesX = Main.maxTilesX;
				int maxTilesY = Main.maxTilesY;
				val = new SKBitmap(maxTilesX, maxTilesY, (SKColorType)6, (SKAlphaType)2);
				SKPixmap val4 = val.PeekPixels();
				Span<byte> pixelSpan = val4.GetPixelSpan();
				Main.NewText("Saving image of the world...", zazasFavoriteColor);
				for (int i = 0; i < maxTilesY; i++)
				{
					int num = i * val4.RowBytes;
					for (int j = 0; j < maxTilesX; j++)
					{
						int num2 = num + j * 4;
						MapTile tile = MapHelper.CreateMapTile(j, i, byte.MaxValue);
						Color mapTileXnaColor = MapHelper.GetMapTileXnaColor(ref tile, j, i);
						pixelSpan[num2] = ((Color)(ref mapTileXnaColor)).B;
						pixelSpan[num2 + 1] = ((Color)(ref mapTileXnaColor)).G;
						pixelSpan[num2 + 2] = ((Color)(ref mapTileXnaColor)).R;
						pixelSpan[num2 + 3] = byte.MaxValue;
					}
				}
				string text = Path.Combine(Main.SavePath, "Exported Maps");
				Directory.CreateDirectory(text);
				string worldName = Main.worldName;
				char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
				string text2 = "";
				string text3 = worldName;
				foreach (char c in text3)
				{
					text2 += ((!Enumerable.Contains(invalidFileNameChars, c)) ? ((c != ' ') ? c : '-') : '-');
				}
				text2 = text2.Replace(".", "_").Replace("*", "_");
				string path = Path.Combine(text, $"{text2}-{Main.ActiveWorldFileData.UniqueId}.png");
				val2 = SKImage.FromBitmap(val);
				val3 = val2.Encode((SKEncodedImageFormat)4, 100);
				fileStream = File.OpenWrite(path);
				val3.SaveTo((Stream)fileStream);
				Main.NewText($"Image of the world saved as {text2}-{Main.ActiveWorldFileData.UniqueId}.png in Terraria/Exported Maps.", zazasFavoriteColor);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An exception occured during map saving!\n{ex}", errorColor);
				LogException(ex);
			}
			finally
			{
				if (val != null)
				{
					((SKNativeObject)val).Dispose();
				}
				if (val2 != null)
				{
					((SKNativeObject)val2).Dispose();
				}
				if (val3 != null)
				{
					((SKNativeObject)val3).Dispose();
				}
				fileStream?.Dispose();
			}
		});
	}

	public static async void FillTilesSelectionModeV3()
	{
		await Task.Run(delegate
		{
			//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			if (concurrentFillOrDels >= 6)
			{
				Main.NewText("Cannot have more than 6 actions running at once!", errorColor);
			}
			else
			{
				concurrentFillOrDels++;
				int num = Math.Min(selectionA.X, selectionB.X);
				int num2 = Math.Min(selectionA.Y, selectionB.Y);
				int num3 = Math.Abs(selectionA.X - selectionB.X);
				int num4 = Math.Abs(selectionA.Y - selectionB.Y);
				int selectedItem = Main.LocalPlayer.selectedItem;
				int createTile = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].createTile;
				int createWall = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem].createWall;
				int num5 = 0;
				if (cancelAction)
				{
					cancelAction = false;
				}
				try
				{
					for (int i = num; i <= num + num3; i++)
					{
						int j;
						for (j = num2; j <= num2 + num4; j++)
						{
							if (cancelAction)
							{
								NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
								Main.NewText("Caneled filling tiles.", zazasFavoriteColor);
								cancelAction = false;
								concurrentFillOrDels--;
								return;
							}
							if (WorldGen.InWorld(i, j))
							{
								bool flag = false;
								Tile tile = Main.tile[i, j];
								if (Main.gameMenu)
								{
									Main.NewText("You can't do this here! (This should never appear)", errorColor);
									concurrentFillOrDels--;
									return;
								}
								if (tile == null)
								{
									tile = default(Tile);
								}
								if (createTile >= 0)
								{
									if (tile.type != createTile && tile.type >= 0 && createWall < 0)
									{
										tile.ClearTile();
									}
									if (!tile.active())
									{
										tile.active(active: true);
										tile.type = (ushort)createTile;
										Main.QueueMainThreadAction(delegate
										{
											WorldGen.SquareTileFrame(i, j, resetFrame: true, noBreak: true);
										});
										flag = true;
										if (Main.netMode != 0 && !selectionTileSyncingMode)
										{
											FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), 58);
											NetMessage.SendData(17, -1, -1, null, 1, i, j, createTile);
										}
									}
								}
								else if (createWall > 0)
								{
									if (tile.wall != Main.LocalPlayer.HeldItem.createWall && tile.wall > 0)
									{
										tile.Clear(TileDataType.Wall);
									}
									if (tile.wall == 0)
									{
										tile.wall = (ushort)createWall;
										Main.QueueMainThreadAction(delegate
										{
											WorldGen.SquareWallFrame(i, j);
										});
										flag = true;
										if (Main.netMode != 0 && !selectionTileSyncingMode)
										{
											FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), selectedItem);
											NetMessage.SendData(17, -1, -1, null, 3, i, j, createWall);
										}
									}
								}
								if (flag)
								{
									if (!selectionTileSyncingMode)
									{
										if (!tilePacketMaxMode)
										{
											if ((Main.netMode != 0 && num5 % tilePacketRate == 0) || IsLowFPS)
											{
												Thread.Sleep(100);
											}
										}
										else
										{
											while ((Main.netMode != 0 && (quedPackets > tilePacketRate || Netplay.Connection.TimeOutTimer > 60)) || IsLowFPS)
											{
												Thread.Sleep(500);
											}
										}
									}
									Main.Map.UpdateLighting(i, j, byte.MaxValue);
									Main.refreshMap = true;
									num5++;
								}
							}
						}
					}
					if (Main.netMode != 0 && selectionTileSyncingMode)
					{
						for (int num6 = num; num6 <= num + num3; num6 += 10)
						{
							for (int num7 = num2; num7 <= num2 + num4; num7 += 10)
							{
								NetMessage.SendTileSquare(-1, num6, num7, 10, 10);
								while (quedPackets > tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS)
								{
									Thread.Sleep(500);
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Main.NewText($"Fill tiles in selection error!\n{ex}", zazasFavoriteColor);
					LogException(ex);
				}
				Main.NewText(num5 + " Tiles Filled.", zazasFavoriteColor);
				NetMessage.SendData(13);
				concurrentFillOrDels--;
			}
		});
	}

	public static async void FillTilesSelectionModeV4(Item item)
	{
		await Task.Run(delegate
		{
			//IL_0572: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0596: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			if (concurrentFillOrDels >= 6)
			{
				Main.NewText("Cannot have more than 6 actions running at once!", errorColor);
			}
			else
			{
				concurrentFillOrDels++;
				int num = Math.Min(selectionA.X, selectionB.X);
				int num2 = Math.Min(selectionA.Y, selectionB.Y);
				int num3 = Math.Abs(selectionA.X - selectionB.X);
				int num4 = Math.Abs(selectionA.Y - selectionB.Y);
				int num5 = item.createTile;
				if (item.type == 4262)
				{
					num5 = 504;
				}
				if (item.type == 496)
				{
					num5 = 127;
				}
				int createWall = item.createWall;
				int num6 = 0;
				if (cancelAction)
				{
					cancelAction = false;
				}
				try
				{
					for (int i = num; i <= num + num3; i++)
					{
						int j;
						for (j = num2; j <= num2 + num4; j++)
						{
							if (cancelAction)
							{
								NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
								Main.NewText("Caneled filling tiles.", zazasFavoriteColor);
								cancelAction = false;
								concurrentFillOrDels--;
								return;
							}
							if (WorldGen.InWorld(i, j))
							{
								bool flag = false;
								Tile tile = Main.tile[i, j];
								if (Main.gameMenu)
								{
									Main.NewText("You can't do this here! (This should never appear)", errorColor);
									concurrentFillOrDels--;
									return;
								}
								if (tile == null)
								{
									tile = default(Tile);
								}
								if (num5 >= 0)
								{
									if (tile.type != num5 && tile.type >= 0 && createWall < 0)
									{
										tile.ClearTile();
									}
									if (!tile.active())
									{
										tile.active(active: true);
										tile.type = (ushort)num5;
										Main.QueueMainThreadAction(delegate
										{
											WorldGen.SquareTileFrame(i, j, resetFrame: true, noBreak: true);
										});
										flag = true;
										if (Main.netMode != 0 && !selectionTileSyncingMode)
										{
											ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, item.type);
											FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), 58);
											NetMessage.SendData(17, -1, -1, null, 1, i, j, num5);
											NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
										}
									}
								}
								else if (createWall > 0)
								{
									if (tile.wall != item.createWall && tile.wall > 0)
									{
										tile.Clear(TileDataType.Wall);
									}
									if (tile.wall == 0)
									{
										tile.wall = (ushort)createWall;
										Main.QueueMainThreadAction(delegate
										{
											WorldGen.SquareWallFrame(i, j);
										});
										flag = true;
										if (Main.netMode != 0 && !selectionTileSyncingMode)
										{
											ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, item.type);
											FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), 58);
											NetMessage.SendData(17, -1, -1, null, 3, i, j, createWall);
											NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
										}
									}
								}
								if (flag)
								{
									if (!selectionTileSyncingMode)
									{
										if (!tilePacketMaxMode)
										{
											if ((Main.netMode != 0 && num6 % tilePacketRate == 0) || IsLowFPS)
											{
												Thread.Sleep(100);
											}
										}
										else
										{
											while ((Main.netMode != 0 && (num6 > tilePacketRate || Netplay.Connection.TimeOutTimer > 60)) || IsLowFPS)
											{
												num6 = 0;
												Thread.Sleep(500);
											}
										}
									}
									Main.Map.UpdateLighting(i, j, byte.MaxValue);
									Main.refreshMap = true;
									num6++;
								}
							}
						}
					}
					if (Main.netMode != 0 && selectionTileSyncingMode)
					{
						for (int num7 = num; num7 <= num + num3; num7 += 10)
						{
							for (int num8 = num2; num8 <= num2 + num4; num8 += 10)
							{
								NetMessage.SendTileSquare(-1, num7, num8, 10, 10);
								while (num6 > tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS)
								{
									num6 = 0;
									Thread.Sleep(500);
								}
								num6++;
							}
						}
					}
				}
				catch (Exception ex)
				{
					Main.NewText($"Fill tiles in selection error!\n{ex}", zazasFavoriteColor);
					LogException(ex);
				}
				Main.NewText(num6 + " Tiles Filled.", zazasFavoriteColor);
				NetMessage.SendData(13);
				concurrentFillOrDels--;
			}
		});
	}

	public static async void BombFillSelection()
	{
		await Task.Run(delegate
		{
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (Main.rand == null)
				{
					Main.rand = new UnifiedRandom();
				}
				int bombFillType = GetBombFillType();
				if (!didTShockDisableYou)
				{
					if (selectionA == Point.Zero || selectionB == Point.Zero || bombAmount < 0)
					{
						Main.NewText("Invalid selection points.", zazasFavoriteColor);
						bombFiller = false;
					}
					else
					{
						int num = Math.Min(selectionA.X, selectionB.X);
						int num2 = Math.Min(selectionA.Y, selectionB.Y);
						int num3 = Math.Abs(selectionA.X - selectionB.X);
						int num4 = Math.Abs(selectionA.Y - selectionB.Y);
						UnifiedRandom unifiedRandom = new UnifiedRandom();
						Vector2 val = default(Vector2);
						for (int i = 0; i < bombAmount; i++)
						{
							float num5 = unifiedRandom.Next(num * 16, (num + num3) * 16);
							float num6 = unifiedRandom.Next(num2 * 16, (num2 + num4) * 16);
							((Vector2)(ref val))._002Ector(num5, num6);
							Point tileCoords = val.ToTileCoordinates();
							if (!Main.gameMenu && WorldGen.InWorld(tileCoords.X, tileCoords.Y, 1) && Main.tile[tileCoords.X, tileCoords.Y] != null && IsTile(Main.tile[tileCoords.X, tileCoords.Y]))
							{
								int num7 = Projectile.NewProjectile(Main.LocalPlayer.GetProjectileSource_Misc((bombFillerType != 5) ? bombFillType : Main.rand.Next(903, 907)), val.X, val.Y, 0f, 0f, (bombFillerType != 5) ? bombFillType : Main.rand.Next(903, 907), 0, 0f, Main.myPlayer);
								Main.projectile[num7].Kill();
								if (bombFillerType > 5)
								{
									int type = 904;
									if (bombFillerType == 7)
									{
										type = 905;
									}
									num7 = Projectile.NewProjectile(null, val.X, val.Y, 0f, 0f, type, 0, 0f, Main.myPlayer);
									Main.projectile[num7].Kill();
								}
								Main.QueueMainThreadAction(delegate
								{
									WorldGen.SquareTileFrame(tileCoords.X, tileCoords.Y);
								});
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Main.NewText($"Bomb Filler error!\n{ex}", zazasFavoriteColor);
				LogException(ex);
				bombFiller = false;
			}
		});
	}

	public static int GetBombFillType()
	{
		return bombFillerType switch
		{
			1 => 903, 
			2 => 904, 
			3 => 905, 
			4 => 906, 
			5 => Main.rand.Next(903, 906), 
			6 => 903, 
			7 => 903, 
			8 => 905, 
			_ => 910, 
		};
	}

	public static async void CopyTilesFromFillOrDelMode()
	{
		try
		{
			if (copyingFromFillOrDelMode)
			{
				Main.NewText("Already coying tiles, please wait for the operation to finish.", errorColor);
				return;
			}
			copyingFromFillOrDelMode = true;
			await Task.Run(delegate
			{
				//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
				int num = Math.Min(selectionA.X, selectionB.X);
				int num2 = Math.Min(selectionA.Y, selectionB.Y);
				schematicSizeX = Math.Abs(selectionA.X - selectionB.X) + 1;
				schematicSizeY = Math.Abs(selectionA.Y - selectionB.Y) + 1;
				copiedTiles.Clear();
				for (int i = num; i < num + schematicSizeX; i++)
				{
					for (int j = num2; j < num2 + schematicSizeY; j++)
					{
						if (WorldGen.InWorld(i, j))
						{
							LegacyTile item = new LegacyTile(Main.tile[i, j]);
							copiedTiles.Add(item);
						}
					}
				}
				Main.NewText("Copied " + copiedTiles.Count + " Tiles.", zazasFavoriteColor);
				copyingFromFillOrDelMode = false;
				Schematics.ResetPreview();
			});
		}
		catch (Exception ex)
		{
			Main.NewText($"Copy tiles error!\n{ex}", errorColor);
			copyingFromFillOrDelMode = false;
			LogException(ex);
		}
	}

	public static ushort ConvertUnsafeWall(ushort i)
	{
		return i switch
		{
			2 => 16, 
			3 => 246, 
			13 => 177, 
			14 => 20, 
			15 => 247, 
			28 => 248, 
			40 => 249, 
			48 => 250, 
			49 => 251, 
			50 => 252, 
			51 => 253, 
			52 => 254, 
			53 => 255, 
			54 => 256, 
			55 => 257, 
			56 => 258, 
			57 => 259, 
			58 => 260, 
			59 => 261, 
			61 => 262, 
			63 => 66, 
			64 => 67, 
			65 => 68, 
			69 => 264, 
			70 => 265, 
			71 => 266, 
			79 => 267, 
			80 => 74, 
			81 => 268, 
			83 => 269, 
			86 => 108, 
			170 => 270, 
			171 => 271, 
			178 => 183, 
			180 => 184, 
			185 => 274, 
			188 => 276, 
			189 => 277, 
			190 => 278, 
			191 => 279, 
			192 => 280, 
			193 => 281, 
			194 => 282, 
			195 => 283, 
			196 => 284, 
			197 => 285, 
			198 => 286, 
			199 => 287, 
			200 => 288, 
			201 => 289, 
			202 => 290, 
			203 => 291, 
			204 => 292, 
			205 => 293, 
			206 => 294, 
			207 => 295, 
			208 => 296, 
			209 => 297, 
			210 => 298, 
			211 => 299, 
			212 => 300, 
			213 => 301, 
			214 => 302, 
			215 => 303, 
			217 => 305, 
			218 => 306, 
			219 => 307, 
			220 => 308, 
			221 => 309, 
			222 => 310, 
			223 => 311, 
			244 => 78, 
			272 => 183, 
			273 => 184, 
			_ => i, 
		};
	}

	public static bool UrrmmmEquals(Tile tile, Tile tile2)
	{
		if (tile.type == tile2.type && tile.wall == tile2.wall && tile.liquid == tile2.liquid && tile.actuator() == tile2.actuator() && tile.slope() + 1 == tile2.slope() + 1 && tile.liquidType() == tile2.liquidType() && tile.inActive() == tile2.inActive() && tile.color() == tile2.color() && tile.wallColor() == tile2.wallColor() && tile.invisibleBlock() == tile2.invisibleBlock() && tile.invisibleWall() == tile2.invisibleWall() && tile.fullbrightBlock() == tile2.fullbrightBlock() && tile.fullbrightWall() == tile2.fullbrightWall() && tile.wire() == tile2.wire() && tile.wire2() == tile2.wire2() && tile.wire3() == tile2.wire3() && tile.wire4() == tile2.wire4() && tile.active() == tile2.active())
		{
			return tile.halfBrick() == tile2.halfBrick();
		}
		return false;
	}

	public static bool IsTileAir(LegacyTile t)
	{
		if (t.active() || t.wall != 0)
		{
			if (!pasteInTShock && Main.netMode == 1)
			{
				return t.liquid > 0;
			}
			return false;
		}
		return true;
	}

	public static async void SpamTheEntireFuckingBeeMovieScript()
	{
		await Task.Run(delegate
		{
			if (!Main.gameMenu)
			{
				while (spammingBeeMovieScript)
				{
					PutTheEntireFuckingBeeMovieScriptIntoServerChat();
					Thread.Sleep(50);
				}
			}
		});
	}

	public static void ApplyForcedAccessoryBuffs(Player player)
	{
		if (ModsVars.forceWarriorEmblem)
		{
			player.meleeDamage += 0.15f;
		}
		if (ModsVars.forceFeralClaws)
		{
			player.autoReuseGlove = true;
			player.meleeSpeed += 0.12f;
		}
		if (ModsVars.forceMagmaStone)
		{
			player.magmaStone = true;
		}
		if (ModsVars.forceTitanGlove)
		{
			player.kbGlove = true;
			player.meleeScaleGlove = true;
		}
		if (ModsVars.forceYoyoString)
		{
			player.yoyoString = true;
		}
		if (ModsVars.forceYoyoCounterweight)
		{
			player.counterWeight = 556 + Main.rand.Next(6);
		}
		if (ModsVars.forceYoyoGlove)
		{
			player.yoyoGlove = true;
		}
		if (ModsVars.forceRangerEmblem)
		{
			player.rangedDamage += 0.15f;
		}
		if (ModsVars.forceMagicQuiver)
		{
			player.magicQuiver = true;
			player.arrowDamageAdditiveStack += 0.1f;
		}
		if (ModsVars.forceMoltenQuiver)
		{
			player.magicQuiver = true;
			player.arrowDamageAdditiveStack += 0.1f;
			player.hasMoltenQuiver = true;
		}
		if (ModsVars.forceSorcererEmblem)
		{
			player.magicDamage += 0.15f;
		}
		if (ModsVars.forceBandOfStarpower)
		{
			player.statManaMax2 += 20;
		}
		if (ModsVars.forceManaRegenerationBand)
		{
			player.statManaMax2 += 20;
			player.manaRegenDelayBonus++;
			player.manaRegenBonus += 25;
		}
		if (ModsVars.forceMagicCuffs)
		{
			player.statManaMax2 += 20;
			player.magicCuffs = true;
		}
		if (ModsVars.forceNaturesGift)
		{
			player.manaCost -= 0.06f;
		}
		if (ModsVars.forceManaFlower)
		{
			player.manaFlower = true;
			player.manaCost -= 0.08f;
		}
		if (ModsVars.forceCelestialMagnet)
		{
			player.manaMagnet = true;
		}
		if (ModsVars.forceCelestialEmblem)
		{
			player.manaMagnet = true;
			player.magicDamage += 0.15f;
		}
		if (ModsVars.forceSummonerEmblem)
		{
			player.minionDamage += 0.15f;
		}
		if (ModsVars.forceHerculesBeetle)
		{
			player.minionKB += 2f;
			player.minionDamage += 0.15f;
		}
		if (ModsVars.forceNecromanticScroll)
		{
			player.minionDamage += 0.1f;
			player.maxMinions++;
		}
		if (ModsVars.forcePygmyNecklace)
		{
			player.maxMinions++;
		}
	}

	public static void EnableMostModHacks()
	{
		godMode = true;
		antiDeathGod = true;
		infiniteMana = true;
		journeyMenu = true;
		extendedPlacementRange = true;
		infiniteMinions = true;
		infiniteAmmo = true;
		ignoreLiquids = true;
		ignoreDamageBlocks = true;
		grappleMounted = true;
		deathItemDropMode = 2;
		persistBuffsMode = 1;
		unrestrictTeleportRod = true;
		ignoreCC = true;
		cancelAnyBuff = true;
		notifyPacket4 = true;
		noSwapRestriction = true;
		if (isCoolClient)
		{
			ModsVars.playerHacksButtonAllMelee = false;
			ModsVars.playerHacksButtonAllRanged = false;
			ModsVars.playerHacksButtonAllMagic = false;
			ModsVars.playerHacksButtonAllSummoner = false;
			ModsUICommandPanelButtons.ToggleAllMeleeAccs(ModsVars.playerHacksButtonAllMelee);
			ModsUICommandPanelButtons.ToggleAllRangedAccs(ModsVars.playerHacksButtonAllRanged);
			ModsUICommandPanelButtons.ToggleAllMagicAccs(ModsVars.playerHacksButtonAllMagic);
			ModsUICommandPanelButtons.ToggleAllSummonAccs(ModsVars.playerHacksButtonAllSummoner);
			ModsVars.playerHacksButtonAllMelee = true;
			ModsVars.playerHacksButtonAllRanged = true;
			ModsVars.playerHacksButtonAllMagic = true;
			ModsVars.playerHacksButtonAllSummoner = true;
		}
		mapTeleportation = true;
		rgbPlayerOutline = true;
		playerTracker = true;
		tilePacketMaxMode = false;
		fullBrightPercent = 0.5f;
		noScreenClamp = true;
		keepThinVanillaBorder = true;
		suppressCreativeShock = true;
		suppressHorrified = true;
		extraAccSlots = true;
		if (isCoolClient)
		{
			buffForceImmune[38] = true;
			buffForceImmune[68] = true;
			buffForceImmune[88] = true;
			buffForceImmune[145] = true;
			buffForceImmune[164] = true;
			buffForceImmune[194] = true;
			buffForceImmune[195] = true;
			buffForceImmune[196] = true;
			gotoMouse = true;
		}
	}

	public static void FreezePlayer(Player player)
	{
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		Random random = new Random(DateTime.Now.Millisecond);
		int num = 0;
		if (player.active && player.whoAmI != Main.myPlayer)
		{
			Instance.FalsePlayerPacket(player.position);
			Projectile.NewProjectile(player.GetProjectileSource_Misc(616), player.position, new Vector2(200f, 200f), 616, 0, 0f, -1, 10000f, 100000f, 10000f);
			Projectile obj = Main.projectile[Projectile.NewProjectile(player.GetProjectileSource_Misc(927), player.position, new Vector2((float)random.Next(-5, 5), (float)random.Next(-5, 5)), 927, 0, 0f, -1, 0f, 715000000f)];
			obj.aiStyle = 75;
			obj.scale = obj.ai[1];
			num++;
		}
		Instance.FalsePlayerPacket(player.position);
		NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
		if (num <= 0)
		{
			return;
		}
		Projectile[] projectile = Main.projectile;
		foreach (Projectile projectile3 in projectile)
		{
			if (projectile3.type == 927 && projectile3.owner == Main.myPlayer)
			{
				projectile3.timeLeft = 10;
			}
		}
	}

	public async void RegenerateTheWorld(string seed)
	{
		await Task.Run(delegate
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			if (WorldGen.gen)
			{
				Main.NewText("You can't do this right now.", errorColor);
			}
			else if (seed == "")
			{
				WorldUtils.DebugRegen();
			}
			else
			{
				WorldGen.noTrapsWorldGen = false;
				WorldGen.notTheBees = false;
				WorldGen.getGoodWorldGen = false;
				WorldGen.tenthAnniversaryWorldGen = false;
				WorldGen.dontStarveWorldGen = false;
				WorldGen.tempRemixWorldGen = false;
				WorldGen.tempTenthAnniversaryWorldGen = false;
				WorldGen.everythingWorldGen = false;
				if (seed.ToLower() == "no traps" || seed.ToLower() == "notraps")
				{
					WorldGen.noTrapsWorldGen = true;
				}
				if (seed.ToLower() == "not the bees" || seed.ToLower() == "not the bees!" || seed.ToLower() == "notthebees")
				{
					WorldGen.notTheBees = true;
				}
				if (seed.ToLower() == "for the worthy" || seed.ToLower() == "fortheworthy")
				{
					WorldGen.getGoodWorldGen = true;
				}
				if (seed.ToLower() == "don't dig up" || seed.ToLower() == "dont dig up" || seed.ToLower() == "dontdigup")
				{
					WorldGen.tempRemixWorldGen = true;
				}
				if (seed.ToLower() == "celebrationmk10")
				{
					WorldGen.tempTenthAnniversaryWorldGen = true;
				}
				if (seed.ToLower() == "constant" || seed.ToLower() == "theconstant" || seed.ToLower() == "the constant" || seed.ToLower() == "eye4aneye" || seed.ToLower() == "eyeforaneye")
				{
					WorldGen.dontStarveWorldGen = true;
				}
				if (seed.ToLower() == "get fixed boi" || seed.ToLower() == "getfixedboi")
				{
					WorldGen.noTrapsWorldGen = true;
					WorldGen.notTheBees = true;
					WorldGen.getGoodWorldGen = true;
					WorldGen.tempTenthAnniversaryWorldGen = true;
					WorldGen.dontStarveWorldGen = true;
					WorldGen.tempRemixWorldGen = true;
					WorldGen.everythingWorldGen = true;
				}
				WorldGen.currentWorldSeed = seed;
				string text = seed;
				int result = 0;
				if (!int.TryParse(text, out result))
				{
					result = Crc32.Calculate(text);
				}
				result = ((result == int.MinValue) ? int.MaxValue : Math.Abs(result));
				WorldGen.clearWorld();
				WorldGen.GenerateWorld(result);
				Main.NewText("World Regen Complete.", zazasFavoriteColor);
			}
		});
	}

	public static async void RejoinServer(string message = "Rejoining!")
	{
		if (Main.netMode == 1)
		{
			rejoining = true;
			if (rejoinRememberPos && !tempRejoin)
			{
				backupPlayer.SpawnX = Main.LocalPlayer.SpawnX;
				backupPlayer.SpawnY = Main.LocalPlayer.SpawnY;
				Main.LocalPlayer.SpawnX = (int)(Main.LocalPlayer.Bottom.X / 16f);
				Main.LocalPlayer.SpawnY = (int)(Main.LocalPlayer.Bottom.Y / 16f);
			}
			tempRejoin = true;
			Main.ToggleGameplayUpdates(state: false);
			string serverIp = Netplay.ServerIPText;
			_ = Netplay.ServerPassword;
			Netplay.InvalidateAllOngoingIPSetAttempts();
			Netplay.Disconnect = true;
			Netplay.Connection.Socket.Close();
			PlayerFileData localPlayerCopy = Main.ActivePlayerFileData;
			WorldGen.SaveAndQuitCallBack(null);
			Netplay.kickMessage = message;
			Main.menuMode = 14;
			await Task.Delay(250);
			SoundEngine.SoundPlayer.StopAll();
			Main.instance.AutoJoin(serverIp);
			Main.menuMultiplayer = true;
			Main.menuServer = false;
			Main.SelectPlayer(localPlayerCopy);
			Main.menuMode = 14;
			Main.statusText = Netplay.kickMessage;
		}
		else if (Main.netMode == 0 && !Main.CanUpdateGameplay)
		{
			Netplay.InvalidateAllOngoingIPSetAttempts();
			Netplay.Disconnect = true;
			Netplay.Connection.Socket.Close();
			Main.gameMenu = true;
			Main.menuMode = 14;
			Main.statusText = (Netplay.kickMessage = Language.GetTextValue("Net.LostConnection"));
		}
	}

	public async void JitterPlayerPosition()
	{
		await Task.Run(delegate
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			Vector2 plrPos = default(Vector2);
			while (jitterPos)
			{
				if (Main.gameMenu)
				{
					jitterPos = false;
					jitteringMyPosRn = false;
					return;
				}
				Random random = new Random(DateTime.Now.Millisecond);
				jitteringMyPosRn = true;
				int maxValue = jitterAmount;
				int minValue = jitterAmount * -1;
				float num = Main.LocalPlayer.position.X + (float)random.Next(minValue, maxValue);
				float num2 = Main.LocalPlayer.position.Y + (float)random.Next(minValue, maxValue);
				((Vector2)(ref plrPos))._002Ector(num, num2);
				FalsePlayerPacket(plrPos);
				Thread.Sleep(2);
			}
			jitteringMyPosRn = false;
		});
	}

	public static void TryPlayingLucyIdleMessage_AllPlayers()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		LucyAxeMessage.MessageSource messageSource = LucyAxeMessage.MessageSource.Idle;
		if (LucyAxeMessage._messageCooldownsByType[(int)messageSource] > 0)
		{
			return;
		}
		for (int i = 0; i < Main.player.Length; i++)
		{
			if (Main.player[i].active)
			{
				LucyAxeMessage.Create(messageSource, Main.player[i].Top, new Vector2(Main.rand.NextFloatDirection() * 7f, -2f + Main.rand.NextFloat() * -2f), forced: true, 99);
			}
		}
		LucyAxeMessage.PutMessageTypeOnCooldown(LucyAxeMessage.MessageSource.Idle, Main.rand.Next(3600, 7200));
	}

	public async void LockOrUnlockAllChests(bool unlock = false)
	{
		await Task.Run(delegate
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			if (Instance.DetermineUnloadedSections > 0)
			{
				Main.NewText("You should reveal the map for this to be more effective.", infoColor);
			}
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					if ((Main.tile[i, j].type == 21 || Main.tile[i, j].type == 467) && !unlock && Chest.Lock(i, j))
					{
						NetMessage.SendData(52, -1, -1, null, 0, 3f, i, j);
					}
					else if (((Main.tile[i, j].type == 21 || Main.tile[i, j].type == 467) & unlock) && Chest.Unlock(i, j))
					{
						NetMessage.SendData(52, -1, -1, null, 0, 1f, i, j);
					}
				}
			}
			Main.NewText((unlock ? "Unlocked all unlockable" : "Locked all lockable") + " chests in the world.", zazasFavoriteColor);
		});
	}

	public static async void SendInventoryManually(bool instant = false)
	{
		await Task.Run(delegate
		{
			//IL_034b: Unknown result type (might be due to invalid IL or missing references)
			if (!sendingBlockedSSC)
			{
				sendingBlockedSSC = true;
				if (instant)
				{
					NetMessage.SendData(16, -1, -1, null, Main.myPlayer);
					NetMessage.SendData(42, -1, -1, null, Main.myPlayer);
				}
				NetMessage.SendData(147, -1, -1, null, Main.myPlayer, Main.player[Main.myPlayer].CurrentLoadoutIndex);
				for (int i = 0; i < 59; i++)
				{
					NetMessage.SendData(5, -1, -1, null, Main.myPlayer, PlayerItemSlotID.Inventory0 + i, (int)Main.player[Main.myPlayer].inventory[i].prefix);
					if (!instant)
					{
						Thread.Sleep(40);
					}
				}
				for (int j = 0; j < Main.player[Main.myPlayer].armor.Length; j++)
				{
					NetMessage.SendData(5, -1, -1, null, Main.myPlayer, PlayerItemSlotID.Armor0 + j, (int)Main.player[Main.myPlayer].armor[j].prefix);
					if (!instant)
					{
						Thread.Sleep(40);
					}
				}
				for (int k = 0; k < Main.player[Main.myPlayer].dye.Length; k++)
				{
					NetMessage.SendData(5, -1, -1, null, Main.myPlayer, PlayerItemSlotID.Dye0 + k, (int)Main.player[Main.myPlayer].dye[k].prefix);
					if (!instant)
					{
						Thread.Sleep(40);
					}
				}
				NetMessage.SyncOnePlayer_ItemArray(Main.myPlayer, -1, -1, Main.player[Main.myPlayer].miscEquips, PlayerItemSlotID.Misc0);
				if (!instant)
				{
					Thread.Sleep(40);
				}
				NetMessage.SyncOnePlayer_ItemArray(Main.myPlayer, -1, -1, Main.player[Main.myPlayer].miscDyes, PlayerItemSlotID.MiscDye0);
				if (!instant)
				{
					Thread.Sleep(40);
				}
				NetMessage.SyncOnePlayer_ItemArray(Main.myPlayer, -1, -1, Main.player[Main.myPlayer].Loadouts[0].Armor, PlayerItemSlotID.Loadout1_Armor_0);
				if (!instant)
				{
					Thread.Sleep(40);
				}
				NetMessage.SyncOnePlayer_ItemArray(Main.myPlayer, -1, -1, Main.player[Main.myPlayer].Loadouts[0].Dye, PlayerItemSlotID.Loadout1_Dye_0);
				if (!instant)
				{
					Thread.Sleep(40);
				}
				NetMessage.SyncOnePlayer_ItemArray(Main.myPlayer, -1, -1, Main.player[Main.myPlayer].Loadouts[1].Armor, PlayerItemSlotID.Loadout2_Armor_0);
				if (!instant)
				{
					Thread.Sleep(40);
				}
				NetMessage.SyncOnePlayer_ItemArray(Main.myPlayer, -1, -1, Main.player[Main.myPlayer].Loadouts[1].Dye, PlayerItemSlotID.Loadout2_Dye_0);
				if (!instant)
				{
					Thread.Sleep(40);
				}
				NetMessage.SyncOnePlayer_ItemArray(Main.myPlayer, -1, -1, Main.player[Main.myPlayer].Loadouts[2].Armor, PlayerItemSlotID.Loadout3_Armor_0);
				if (!instant)
				{
					Thread.Sleep(40);
				}
				NetMessage.SyncOnePlayer_ItemArray(Main.myPlayer, -1, -1, Main.player[Main.myPlayer].Loadouts[2].Dye, PlayerItemSlotID.Loadout3_Dye_0);
				sendingBlockedSSC = false;
				if (notifyServerModifiedPlayer)
				{
					Main.NewText("Finished resending inventory.", zazasFavoriteColor);
				}
			}
		});
	}

	public static async void ScanWorldForSpecificTile(int tileId, bool teleport = false)
	{
		await Task.Run(delegate
		{
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			bool flag2 = false;
			int num = 0;
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.active() && tile.type == tileId)
					{
						flag = true;
						num++;
						if (num < 50 && !flag2)
						{
							Main.NewText($"Tile Located at - [ X:{i} Y:{j} ] with framing [ FrameX: {tile.frameX} ] | [ FrameY: {tile.frameY} ]", zazasFavoriteColor);
						}
						if (teleport && !flag2)
						{
							flag2 = true;
							Main.LocalPlayer.position = new Vector2((float)i * 16f - (float)(Main.LocalPlayer.width / 2) + 8f, (float)j * 16f - (float)Main.LocalPlayer.height);
							NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
						}
					}
				}
			}
			if (flag && num > 50 && !flag2)
			{
				Main.NewText("+ " + (num - 50) + " more... (Total " + num + ")", zazasFavoriteColor);
			}
			else if ((flag && num > 1) & flag2)
			{
				Main.NewText("+ " + (num - 1) + " more... (Total " + num + ")", zazasFavoriteColor);
			}
			if (!flag)
			{
				Main.NewText("Tile ID " + tileId + " not found in world and/or loaded sections.", zazasFavoriteColor);
			}
		});
	}

	public static async void RemoveTilesOfType(int tileId)
	{
		await Task.Run(delegate
		{
			//IL_0617: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_071b: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			int num = 0;
			int num2 = 0;
			if (concurrentFillOrDels >= 6)
			{
				Main.NewText("Cannot have more than 6 actions running at once!", errorColor);
			}
			else
			{
				concurrentFillOrDels++;
				if (cancelAction)
				{
					cancelAction = false;
				}
				int num3 = ((selectionA.X != 0 && selectionMode) ? Math.Min(selectionA.X, selectionB.X) : 0);
				int num4 = ((selectionA.Y != 0 && selectionMode) ? Math.Min(selectionA.Y, selectionB.Y) : 0);
				int num5 = ((selectionB.X != 0 && selectionMode) ? Math.Abs(selectionA.X - selectionB.X) : Main.maxTilesX);
				int num6 = ((selectionB.X != 0 && selectionMode) ? Math.Abs(selectionA.Y - selectionB.Y) : Main.maxTilesY);
				try
				{
					for (int i = num3; i <= num3 + num5; i++)
					{
						int j;
						for (j = num4; j <= num4 + num6; j++)
						{
							if (Main.gameMenu)
							{
								cancelAction = false;
								concurrentFillOrDels--;
								return;
							}
							if (cancelAction)
							{
								NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
								Main.NewText($"Caneled removing all {TileID.Search.GetName(tileId)} ({tileId}) tiles.", errorColor);
								cancelAction = false;
								concurrentFillOrDels--;
								return;
							}
							Tile tile = Main.tile[i, j];
							if (tile != null && tile.active() && tile.type == tileId)
							{
								flag = true;
								num++;
								if (Main.netMode != 0)
								{
									num2++;
									if (tile.type != 504)
									{
										tile.type = 504;
										FakeNetMessage13(new Vector2((float)i * 16f, (float)j * 16f), Main.LocalPlayer.selectedItem);
										NetMessage.SendData(17, -1, -1, null, 1, i, j, 504f);
										tile.active(active: false);
										tile.halfBrick(halfBrick: false);
										tile.frameX = -1;
										tile.frameY = -1;
										tile.ClearBlockPaintAndCoating();
										tile.frameNumber(0);
										if (tile.type == 419)
										{
											Wiring.PokeLogicGate(i, j + 1);
										}
										else if (TileID.Sets.BlocksWaterDrawingBehindSelf[tile.type])
										{
											Main.QueueMainThreadAction(delegate
											{
												WorldGen.SquareWallFrame(i, j);
											});
										}
										tile.type = 0;
										tile.inActive(inActive: false);
										Main.QueueMainThreadAction(delegate
										{
											WorldGen.SquareTileFrame(i, j);
										});
										NetMessage.SendData(17, -1, -1, null, 0, i, j);
										num2++;
									}
									else
									{
										tile.active(active: false);
										tile.halfBrick(halfBrick: false);
										tile.frameX = -1;
										tile.frameY = -1;
										tile.ClearBlockPaintAndCoating();
										tile.frameNumber(0);
										if (tile.type == 419)
										{
											Wiring.PokeLogicGate(i, j + 1);
										}
										else if (TileID.Sets.BlocksWaterDrawingBehindSelf[tile.type])
										{
											Main.QueueMainThreadAction(delegate
											{
												WorldGen.SquareWallFrame(i, j);
											});
										}
										tile.type = 0;
										tile.inActive(inActive: false);
										Main.QueueMainThreadAction(delegate
										{
											WorldGen.SquareTileFrame(i, j);
										});
										NetMessage.SendData(17, -1, -1, null, 0, i, j);
									}
									while (tilePacketMaxMode && (num2 > erasePacketRate || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS))
									{
										num2 = 0;
										Thread.Sleep(500);
									}
									if (!tilePacketMaxMode && (num % erasePacketRate == 0 || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS))
									{
										num2 = 0;
										Thread.Sleep(100);
									}
								}
								else
								{
									tile.active(active: false);
									tile.halfBrick(halfBrick: false);
									tile.frameX = -1;
									tile.frameY = -1;
									tile.ClearBlockPaintAndCoating();
									tile.frameNumber(0);
									if (tile.type == 419)
									{
										Wiring.PokeLogicGate(i, j + 1);
									}
									else if (TileID.Sets.BlocksWaterDrawingBehindSelf[tile.type])
									{
										Main.QueueMainThreadAction(delegate
										{
											WorldGen.SquareWallFrame(i, j);
										});
									}
									tile.type = 0;
									tile.inActive(inActive: false);
									Main.QueueMainThreadAction(delegate
									{
										WorldGen.SquareTileFrame(i, j);
									});
								}
								Main.Map.UpdateLighting(i, j, byte.MaxValue);
								Main.refreshMap = true;
							}
						}
					}
				}
				catch (Exception ex)
				{
					Main.NewText($"Remove specific tiles error!\n{ex}", zazasFavoriteColor);
					LogException(ex);
				}
				if (flag && num >= 1)
				{
					Main.NewText($"Removed {num} {TileID.Search.GetName(tileId)} ({tileId}) tiles.", zazasFavoriteColor);
					NetMessage.SendData(13);
				}
				if (!flag)
				{
					Main.NewText($"{TileID.Search.GetName(tileId)} ({tileId}) was not found in the world and/or loaded sections.", zazasFavoriteColor);
				}
				concurrentFillOrDels--;
			}
		});
	}

	public static async void ReplaceAllTilesWithType(int tileId)
	{
		await Task.Run(delegate
		{
			//IL_052a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_05df: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			int num2 = 0;
			if (concurrentFillOrDels >= 6)
			{
				Main.NewText("Cannot have more than 6 actions running at once!", errorColor);
			}
			else
			{
				concurrentFillOrDels++;
				if (cancelAction)
				{
					cancelAction = false;
				}
				int num3 = ((selectionA.X != 0 && selectionMode) ? Math.Min(selectionA.X, selectionB.X) : 0);
				int num4 = ((selectionA.Y != 0 && selectionMode) ? Math.Min(selectionA.Y, selectionB.Y) : 0);
				int num5 = ((selectionB.X != 0 && selectionMode) ? Math.Abs(selectionA.X - selectionB.X) : Main.maxTilesX);
				int num6 = ((selectionB.X != 0 && selectionMode) ? Math.Abs(selectionA.Y - selectionB.Y) : Main.maxTilesY);
				try
				{
					for (int i = num3; i <= num3 + num5; i++)
					{
						int j;
						for (j = num4; j <= num4 + num6; j++)
						{
							if (Main.gameMenu)
							{
								cancelAction = false;
								concurrentFillOrDels--;
								return;
							}
							if (cancelAction)
							{
								NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
								Main.NewText($"Caneled replacing all tiles with {TileID.Search.GetName(tileId)} ({tileId}.", errorColor);
								cancelAction = false;
								concurrentFillOrDels--;
								return;
							}
							Tile copy = Main.tile[i, j];
							if (copy.active() && copy.type != tileId && (Main.tileSolid[copy.type] || copy.type == 131) && !TileObjectData.CustomPlace(copy.type, 0) && copy.type != 137 && !TileID.Sets.Platforms[copy.type])
							{
								num++;
								if (Main.netMode != 0)
								{
									LegacyTile legacyTile = new LegacyTile(copy)
									{
										type = (ushort)tileId
									};
									Item itemDrop = Schematics.GetItemDrop(i, j, legacyTile, includeLargeObjectDrops: true);
									if (!itemDrop.IsAir)
									{
										if (itemDrop.type == 496)
										{
											itemDrop.createTile = 127;
										}
										if (itemDrop.type == 4262)
										{
											itemDrop.createTile = 504;
										}
										if (itemDrop.createTile != -1 && itemDrop.createTile == legacyTile.type)
										{
											copy.type = (ushort)tileId;
											FakeNetMessage13(new Vector2((float)((i + 2) * 16), (float)(j * 16)), 58);
											ModsNetMessages.SendModifiedData(5, -1, -1, null, Main.myPlayer, 58f, 0f, itemDrop.netID);
											NetMessage.SendData(17, -1, -1, null, 1, i, j, (int)copy.type, itemDrop.placeStyle);
											Main.QueueMainThreadAction(delegate
											{
												WorldGen.SquareTileFrame(i, j);
											});
											num2++;
											while (tilePacketMaxMode && (num2 > tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS))
											{
												num2 = 0;
												Thread.Sleep(500);
											}
											if (!tilePacketMaxMode && (num2 % tilePacketRate == 0 || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS))
											{
												num2 = 0;
												Thread.Sleep(100);
											}
											NetMessage.SendData(5, -1, -1, null, Main.myPlayer, 58f);
											NetMessage.SendData(13);
										}
									}
								}
								else
								{
									copy.type = (ushort)tileId;
									Main.QueueMainThreadAction(delegate
									{
										WorldGen.SquareTileFrame(i, j);
									});
								}
								Main.Map.UpdateLighting(i, j, byte.MaxValue);
								Main.refreshMap = true;
							}
						}
					}
				}
				catch (Exception ex)
				{
					Main.NewText($"Replace tiles error!\n{ex}", errorColor);
					LogException(ex);
				}
				if (num >= 1)
				{
					Main.NewText($"Replaced {num} tiles with {TileID.Search.GetName(tileId)} ({tileId}).", zazasFavoriteColor);
					NetMessage.SendData(13);
				}
				else
				{
					Main.NewText("There are no tiles to replace.", errorColor);
				}
				concurrentFillOrDels--;
			}
		});
	}

	public static void CrashMobilePlayersV2()
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			bool prevChannelState = Main.LocalPlayer.channel;
			for (int k = 0; k < Main.player.Length; k++)
			{
				if (Main.player[k].active && Main.player[k] != Main.LocalPlayer)
				{
					for (int j = 0; j < 3; j++)
					{
						SpawnMobileCrashingThing(Main.player[k].Center);
					}
				}
			}
			NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
			Main.LocalPlayer.channel = prevChannelState;
			Main.NewText("Attempted to crash all mobile players.", zazasFavoriteColor);
		}
		catch (Exception ex)
		{
			Main.NewText($"Crash mobile players error!\n{ex}", errorColor);
			LogException(ex);
		}
	}

	public static void SpawnMobileCrashingThing(Vector2 pos)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		Main.LocalPlayer.channel = true;
		FakeNetMessage13(pos, Main.LocalPlayer.selectedItem);
		Projectile obj = Main.projectile[Projectile.NewProjectile(null, pos, new Vector2(3f, 3f), 190, 0, 0f, Main.myPlayer, 0f, 0.2f)];
		obj.miscText = "ZazaMod Mobile Crashing Projectile";
		obj.timeLeft = 16;
		obj.netUpdate = true;
	}

	public static async void RotateTheWorld(bool fixChests = false)
	{
		Main.NewText("This is disabled until it can be fixed.", infoColor);
	}

	public static async void ConvertWorld(int conversionType = 0, bool selection = false)
	{
		await Task.Run(delegate
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			if (!purifyingWorld || selection)
			{
				if (!selection)
				{
					purifyingWorld = true;
				}
				else
				{
					if (concurrentConversions >= 3)
					{
						Main.NewText("Cannot have more than 3 conversions running at once!", errorColor);
						return;
					}
					concurrentConversions++;
				}
				if (cancelAction)
				{
					cancelAction = false;
				}
				int num = 0;
				int num2 = (selection ? Math.Min(selectionA.X, selectionB.X) : 10);
				int num3 = (selection ? Math.Min(selectionA.Y, selectionB.Y) : 10);
				int num4 = (selection ? (num2 + Math.Abs(selectionA.X - selectionB.X)) : (Main.maxTilesX - 11));
				int num5 = (selection ? (num3 + Math.Abs(selectionA.Y - selectionB.Y)) : (Main.maxTilesY - 11));
				try
				{
					for (int i = num2 + 1; i < num4; i++)
					{
						for (int j = num3 + 1; j < num5; j++)
						{
							if (cancelAction)
							{
								Main.NewText("Caneled world conversion.", errorColor);
								cancelAction = false;
								if (!selection)
								{
									purifyingWorld = false;
								}
								else
								{
									concurrentConversions--;
								}
								return;
							}
							if (Main.gameMenu)
							{
								if (!selection)
								{
									purifyingWorld = false;
								}
								else
								{
									concurrentConversions--;
								}
								return;
							}
							if (Main.tile[i, j].active() || Main.tile[i, j].wall > 0)
							{
								WorldGen.Convert(i, j, conversionType, 1);
								if (selection && Main.netMode != 0)
								{
									num++;
									while (selection && tilePacketMaxMode && (num / 2 > tilePacketRate || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS))
									{
										num = 0;
										Thread.Sleep(500);
									}
									if (selection && !tilePacketMaxMode && (num / 2 % tilePacketRate == 0 || Netplay.Connection.TimeOutTimer > 60 || IsLowFPS))
									{
										num = 0;
										Thread.Sleep(100);
									}
								}
							}
						}
					}
				}
				catch
				{
				}
				if (!selection)
				{
					purifyingWorld = false;
					Main.NewText("The world has been changed to " + ConversionType(conversionType) + ".", 50, byte.MaxValue, 130);
				}
				else
				{
					concurrentConversions--;
					Main.NewText("Comverted selection to " + ConversionType(conversionType) + ".", 50, byte.MaxValue, 130);
				}
			}
		});
	}

	public static string ConversionType(int type)
	{
		return type switch
		{
			1 => "Corruption", 
			2 => "Hallow", 
			3 => "Mushrooms", 
			4 => "Crimson", 
			5 => "Desert", 
			6 => "Snow", 
			7 => "Forest", 
			_ => "Purity", 
		};
	}

	public static void TeleportPlayers(Vector2 pos)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if (onTshockServer)
		{
			Main.NewText("This doesn't work on tShock servers.", errorColor);
			return;
		}
		for (int k = 0; k < Main.player.Length; k++)
		{
			if (Main.player[k].active && Main.player[k] != Main.LocalPlayer)
			{
				NetMessage.SendData(96, -1, -1, null, Main.player[k].whoAmI, pos.X, pos.Y);
			}
		}
		Main.NewText("Players teleported!", zazasFavoriteColor);
	}

	public static byte GetMaxVariationForLucy(LucyAxeMessage.MessageSource source, byte variation)
	{
		switch (source)
		{
		default:
			return 4;
		case LucyAxeMessage.MessageSource.PickedUp:
			return 2;
		case LucyAxeMessage.MessageSource.ChoppedTree:
			return 9;
		case LucyAxeMessage.MessageSource.ChoppedGemTree:
		case LucyAxeMessage.MessageSource.ChoppedCactus:
			return 0;
		}
	}

	public static void PossessNPC(Player player)
	{
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		if (controlledNpcType == 0 || Main.gameMenu)
		{
			return;
		}
		if ((controlledNpc != null && !controlledNpc.active) || (controlledNpc != null && controlledNpc.type != controlledNpcType))
		{
			controlledNpc = null;
		}
		NPC npc = controlledNpc;
		if (npc == null || npcCycleRequested)
		{
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == controlledNpcType && (!npcCycleRequested || (npcCycleRequested && Main.npc[i] != npc && Main.npc[i].whoAmI != lastControlledNpcIndex)))
				{
					npc = Main.npc[i];
					npcCycleRequested = false;
					lastControlledNpcIndex = npc.whoAmI;
					break;
				}
			}
		}
		if (npcCycleRequested)
		{
			npcCycleRequested = false;
			lastControlledNpcIndex = npc.whoAmI;
		}
		if (npc != null)
		{
			if (Main.netMode == 0)
			{
				npc.despawnEncouraged = false;
				npc.CanBeReplacedByOtherNPCs = false;
				if (!controlNpcAtMouse)
				{
					if (player.dead)
					{
						npc.shimmerTransparency = 1f;
					}
					if (!npc.noTileCollide)
					{
						npc.width = player.width;
						npc.height = player.height;
					}
				}
			}
			if (!npc.noTileCollide && !controlNpcAtMouse)
			{
				npc.Bottom = (controlNpcAtMouse ? Main.MouseWorld : player.Bottom);
			}
			else
			{
				npc.Center = (controlNpcAtMouse ? Main.MouseWorld : player.Center);
			}
			npc.position.X = (int)npc.position.X;
			npc.position.Y = (int)npc.position.Y;
			if (!controlNpcAtMouse)
			{
				npc.gfxOffY = player.gfxOffY;
			}
			else
			{
				npc.gfxOffY = 0f;
			}
			npc.velocity = (controlNpcAtMouse ? (mouseVelocity / 2f) : (player.velocity / 2f));
			if (!controlNpcAtMouse && !npc.noTileCollide && (npc.friendly || npc.townNPC || npc.isLikeATownNPC || (npc.aiStyle != 7 && npc.aiStyle != 37)))
			{
				npc.aiStyle = 0;
				npc.direction = player.direction;
				npc.spriteDirection = player.direction;
			}
			NPC nPC = npc;
			nPC.netOffset *= 0f;
		}
		controlledNpc = npc;
	}

	public static void ReplaceAllLiquids(byte newLiquid, int toReplace = -1)
	{
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		int startX = (selectionMode ? Math.Min(selectionA.X, selectionB.X) : 0);
		int startY = (selectionMode ? Math.Min(selectionA.Y, selectionB.Y) : 0);
		int endX = (selectionMode ? (startX + Math.Abs(selectionA.X - selectionB.X)) : (Main.maxTilesX - 1));
		int endY = (selectionMode ? (startY + Math.Abs(selectionA.Y - selectionB.Y)) : (Main.maxTilesY - 1));
		for (int i = startX; i <= endX; i++)
		{
			for (int j = startY; j <= endY; j++)
			{
				Tile tile = Main.tile[i, j];
				if ((tile.LiquidType == toReplace || toReplace == -1) && tile.LiquidAmount > 0 && tile.LiquidType != newLiquid)
				{
					tile.LiquidType = newLiquid;
				}
			}
		}
		Main.NewText($"Replaced all {((toReplace == -1) ? "liquids" : LiquidID.Search.GetName(toReplace))} with {LiquidID.Search.GetName((int)newLiquid)}.", zazasFavoriteColor);
	}

	public static string TileDataString(Tile tile)
	{
		List<string> tileData = new List<string>();
		if (tile.HasTile)
		{
			tileData.Add($"Tile Name: {TileID.Search.GetName((int)tile.TileType)} (ID: {tile.TileType})");
		}
		if (tile.WallType > 0)
		{
			tileData.Add($"Wall Name: {WallID.Search.GetName((int)tile.WallType)} (ID: {tile.WallType})");
		}
		if (tile.HasTile)
		{
			tileData.Add($"Tile FrameX: {tile.TileFrameX}, Tile FrameY: {tile.TileFrameY}");
			tileData.Add($"Tile Paint: {PaintID.Search.GetName((int)tile.TileColor)} (ID: {tile.TileColor})");
		}
		if (tile.WallType > 0)
		{
			tileData.Add($"Wall Paint: {PaintID.Search.GetName((int)tile.WallColor)} (ID: {tile.WallColor})");
		}
		if (tile.HasTile && tile.IsTileInvisible)
		{
			tileData.Add("Invisible Tile");
		}
		if (tile.WallType > 0 && tile.IsWallInvisible)
		{
			tileData.Add("Invisible Wall");
		}
		if (tile.HasTile && tile.IsTileFullbright)
		{
			tileData.Add("Fullbright Tile");
		}
		if (tile.WallType > 0 && tile.IsWallFullbright)
		{
			tileData.Add("Fullbright Wall");
		}
		if (tile.LiquidAmount > 0)
		{
			tileData.Add($"Liquid: {LiquidID.Search.GetName(tile.LiquidType)}, Amount: {tile.LiquidAmount}/255");
		}
		List<string> wires = new List<string>();
		string wiresString = "";
		if (tile.RedWire)
		{
			wires.Add("Red");
		}
		if (tile.BlueWire)
		{
			wires.Add("Blue");
		}
		if (tile.GreenWire)
		{
			wires.Add("Green");
		}
		if (tile.YellowWire)
		{
			wires.Add("Yellow");
		}
		if (wires.Count > 0)
		{
			wiresString = string.Join(", ", wires);
		}
		if (wiresString != "")
		{
			tileData.Add("Wires: " + wiresString);
		}
		if (tile.HasActuator)
		{
			tileData.Add($"Has Actuator: {tile.HasActuator}");
		}
		if (tile.HasTile)
		{
			tileData.Add($"Is Tile Actuated: {!tile.HasUnactuatedTile}");
		}
		return string.Join("\n", tileData);
	}

	public static string SelectionModeString()
	{
		if (!selectionModeInfo)
		{
			return "Selection Mode";
		}
		int topLeftX = Math.Min(selectionA.X, selectionB.X);
		int topLeftY = Math.Min(selectionA.Y, selectionB.Y);
		int bottomRightX = Math.Max(selectionA.X, selectionB.X);
		int bottomRightY = Math.Max(selectionA.Y, selectionB.Y);
		return $"Selection Mode\nTop Left: {topLeftX}, {topLeftY}\nBottom Right: {bottomRightX}, {bottomRightY}\nWidth: {bottomRightX - topLeftX + 1}\nHeight: {bottomRightY - topLeftY + 1}";
	}

	public static string PasteModeString()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		if (!selectionModeInfo)
		{
			return "";
		}
		string info = "Paste Mode";
		List<string> tileData = new List<string>();
		tileData.Add(info);
		Point mouseWorldPos = pasteImagePreviewPos;
		if (!motionlessPaste)
		{
			if (Main.mapFullscreen)
			{
				GetMapCoordinatesModV2(Main.mouseX, Main.mouseY, 0, out mouseWorldPos);
			}
			else
			{
				mouseWorldPos = Main.MouseWorld.ToTileCoordinates();
			}
		}
		else
		{
			tileData.Add("Paste Position Locked");
		}
		if (WorldGen.InWorld(mouseWorldPos.X, mouseWorldPos.Y))
		{
			tileData.Add($"Paste Origin: {mouseWorldPos.X}, {mouseWorldPos.Y}");
		}
		else
		{
			tileData.Add("Paste Origin Unavailable");
		}
		tileData.Add($"Width: {schematicSizeX}");
		tileData.Add($"Height: {schematicSizeY}");
		return info = string.Join("\n", tileData);
	}

	public static string NpcDataString(NPC npc)
	{
		List<string> npcData = new List<string>
		{
			"Internal Name: " + NPCID.Search.GetName(npc.netID),
			$"Index: {npc.whoAmI}",
			$"Defense: {npc.defense}",
			$"AI Style: {npc.aiStyle}"
		};
		if (npc.value > 0f)
		{
			npcData.Add($"Value (Money): {npc.value}");
		}
		if ((float)npc.extraValue > 0f)
		{
			npcData.Add($"Extra Value (Money): {npc.extraValue}");
		}
		npcData.Add($"AI[0]: {npc.ai[0]}");
		npcData.Add($"AI[1]: {npc.ai[1]}");
		npcData.Add($"AI[2]: {npc.ai[2]}");
		npcData.Add($"Friendly: {npc.friendly}");
		if (npc.HasPlayerTarget && Main.player.IndexInRange(npc.target) && npc.target != 255)
		{
			npcData.Add($"Target Player: {Main.player[npc.target].name} ({npc.target})");
		}
		if (npc.HasNPCTarget && Main.npc.IndexInRange(npc.target) && npc.target != 255)
		{
			npcData.Add($"Target NPC: {Main.npc[npc.target].FullName} ({npc.target})");
		}
		if (npc.dontTakeDamage)
		{
			npcData.Add($"Doesn't Take Damage: {npc.dontTakeDamage}");
		}
		return "\n" + string.Join("\n", npcData);
	}

	public static string FunnyQuirkyIndieGameLevelWriting()
	{
		List<string> values = new List<string>
		{
			"You tried to heal yourself... but you were already fully healed.",
			"You tried to heal yourself... but it refused.",
			"You tried to heal yourself... but nothing happened.",
			"You tried to heal yourself... but it didn't do anything.",
			"You tried to heal yourself... but you were unsuccessful.",
			"You tried to heal yourself... but this didn't accomplish anything.",
			"You tried to heal yourself... and amazingly, succeeded at doing nothing.",
			"You tried to heal yourself... but this would be wasting your time.",
			"You tried to heal yourself... but this text informed you that you did absolutely nothing.",
			"You tried to heal yourself... but you didn't need any healing.",
			"You yelled \"MEDIC!\". Nothing happened.",
			"You used the heal command. Nothing happened.",
			Main.LocalPlayer.name + " tried LifeUp! But it didn't work very well!",
			"OAK: " + Main.LocalPlayer.name + "! This isn't the time to use that!"
		};
		return values[Main.rand.Next(values.Count)];
	}

	public static void CopyInventoryFrom(Player from)
	{
		if (!isInventoryBackedUp)
		{
			Player playerClone = Main.LocalPlayer.SafeSerializedClone();
			isInventoryBackedUp = true;
			backupPlayer.inventory = playerClone.inventory;
			backupPlayer.armor = playerClone.armor;
			for (int i = 3; i < playerClone.hideVisibleAccessory.Length; i++)
			{
				backupPlayer.hideVisibleAccessory[i] = playerClone.hideVisibleAccessory[i];
			}
			backupPlayer.dye = playerClone.dye;
			backupPlayer.miscEquips = playerClone.miscEquips;
			for (int j = 0; j < 2; j++)
			{
				backupPlayer.hideMisc[j] = playerClone.hideMisc[j];
			}
			backupPlayer.miscDyes = playerClone.miscDyes;
			backupPlayer.statLifeMax = playerClone.statLifeMax;
			backupPlayer.statManaMax = playerClone.statManaMax;
			backupPlayer.Loadouts = playerClone.Loadouts;
			backupPlayer.CurrentLoadoutIndex = playerClone.CurrentLoadoutIndex;
			Player ourClone = from.SafeSerializedClone();
			Main.LocalPlayer.inventory = ourClone.inventory;
			Main.LocalPlayer.armor = ourClone.armor;
			for (int k = 3; k < ourClone.hideVisibleAccessory.Length; k++)
			{
				Main.LocalPlayer.hideVisibleAccessory[k] = ourClone.hideVisibleAccessory[k];
			}
			Main.LocalPlayer.dye = ourClone.dye;
			Main.LocalPlayer.miscEquips = ourClone.miscEquips;
			for (int l = 0; l < 2; l++)
			{
				Main.LocalPlayer.hideMisc[l] = ourClone.hideMisc[l];
			}
			Main.LocalPlayer.miscDyes = ourClone.miscDyes;
			Main.LocalPlayer.statLifeMax = ourClone.statLifeMax;
			Main.LocalPlayer.statManaMax = ourClone.statManaMax;
			Main.LocalPlayer.Loadouts = ourClone.Loadouts;
			Main.LocalPlayer.CurrentLoadoutIndex = ourClone.CurrentLoadoutIndex;
			SoundEngine.PlaySound(12);
			SendInventoryManually(instant: true);
		}
		else
		{
			Player backupClone = backupPlayer.SafeSerializedClone();
			isInventoryBackedUp = false;
			Main.LocalPlayer.inventory = backupClone.inventory;
			Main.LocalPlayer.armor = backupClone.armor;
			for (int m = 3; m < backupClone.hideVisibleAccessory.Length; m++)
			{
				Main.LocalPlayer.hideVisibleAccessory[m] = backupClone.hideVisibleAccessory[m];
			}
			Main.LocalPlayer.dye = backupClone.dye;
			Main.LocalPlayer.miscEquips = backupClone.miscEquips;
			for (int n = 0; n < 2; n++)
			{
				Main.LocalPlayer.hideMisc[n] = backupClone.hideMisc[n];
			}
			Main.LocalPlayer.miscDyes = backupClone.miscDyes;
			Main.LocalPlayer.statLifeMax = backupClone.statLifeMax;
			Main.LocalPlayer.statManaMax = backupClone.statManaMax;
			Main.LocalPlayer.Loadouts = backupClone.Loadouts;
			Main.LocalPlayer.CurrentLoadoutIndex = backupClone.CurrentLoadoutIndex;
			SoundEngine.PlaySound(12);
			SendInventoryManually(instant: true);
		}
	}

	public static void CopyAppearenceFrom(Player from)
	{
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		if (!isAppearanceBackedUp)
		{
			Player playerClone = Main.LocalPlayer.SafeSerializedClone();
			isAppearanceBackedUp = true;
			backupPlayer.hair = playerClone.hair;
			backupPlayer.hairColor = playerClone.hairColor;
			backupPlayer.skinColor = playerClone.skinColor;
			backupPlayer.eyeColor = playerClone.eyeColor;
			backupPlayer.shirtColor = playerClone.shirtColor;
			backupPlayer.underShirtColor = playerClone.underShirtColor;
			backupPlayer.pantsColor = playerClone.pantsColor;
			backupPlayer.shoeColor = playerClone.shoeColor;
			backupPlayer.hairDye = playerClone.hairDye;
			backupPlayer.hairDyeColor = playerClone.hairDyeColor;
			backupPlayer.hairDyeVar = playerClone.hairDyeVar;
			backupPlayer.skinVariant = playerClone.skinVariant;
			Player ourClone = from.SafeSerializedClone();
			Main.LocalPlayer.hair = ourClone.hair;
			Main.LocalPlayer.hairColor = ourClone.hairColor;
			Main.LocalPlayer.skinColor = ourClone.skinColor;
			Main.LocalPlayer.eyeColor = ourClone.eyeColor;
			Main.LocalPlayer.shirtColor = ourClone.shirtColor;
			Main.LocalPlayer.underShirtColor = ourClone.underShirtColor;
			Main.LocalPlayer.pantsColor = ourClone.pantsColor;
			Main.LocalPlayer.shoeColor = ourClone.shoeColor;
			Main.LocalPlayer.hairDye = ourClone.hairDye;
			Main.LocalPlayer.hairDyeColor = ourClone.hairDyeColor;
			Main.LocalPlayer.hairDyeVar = ourClone.hairDyeVar;
			Main.LocalPlayer.skinVariant = ourClone.skinVariant;
			SoundEngine.PlaySound(12);
			if (Main.netMode == 1)
			{
				NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
				NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
			}
		}
		else
		{
			Player backupClone = backupPlayer.SafeSerializedClone();
			isAppearanceBackedUp = false;
			Main.LocalPlayer.hair = backupClone.hair;
			Main.LocalPlayer.hairColor = backupClone.hairColor;
			Main.LocalPlayer.skinColor = backupClone.skinColor;
			Main.LocalPlayer.eyeColor = backupClone.eyeColor;
			Main.LocalPlayer.shirtColor = backupClone.shirtColor;
			Main.LocalPlayer.underShirtColor = backupClone.underShirtColor;
			Main.LocalPlayer.pantsColor = backupClone.pantsColor;
			Main.LocalPlayer.shoeColor = backupClone.shoeColor;
			Main.LocalPlayer.hairDye = backupClone.hairDye;
			Main.LocalPlayer.hairDyeColor = backupClone.hairDyeColor;
			Main.LocalPlayer.hairDyeVar = backupClone.hairDyeVar;
			Main.LocalPlayer.skinVariant = backupClone.skinVariant;
			SoundEngine.PlaySound(12);
			if (Main.netMode == 1)
			{
				NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
				NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
			}
		}
	}

	public static void SendDataM(int msgType, int amount = 1, bool batchWrite = false, int remoteClient = -1, int ignoreClient = -1, NetworkText text = null, int number = 0, float number2 = 0f, float number3 = 0f, float number4 = 0f, int number5 = 0, int number6 = 0, int number7 = 0)
	{
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_096b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_0974: Unknown result type (might be due to invalid IL or missing references)
		//IL_0979: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		int num = 256;
		if (text == null)
		{
			text = NetworkText.Empty;
		}
		if (Main.netMode == 2 && remoteClient >= 0)
		{
			num = remoteClient;
		}
		int length = 0;
		int realLength = 0;
		List<int> lengths = new List<int>();
		lock (NetMessage.buffer[num])
		{
			BinaryWriter writer = NetMessage.buffer[num].writer;
			if (writer == null)
			{
				NetMessage.buffer[num].ResetWriter();
				writer = NetMessage.buffer[num].writer;
			}
			writer.BaseStream.Position = 0L;
			if (batchWrite)
			{
				for (int i = 0; i < amount; i++)
				{
					lengths.Add(NetMessage.SendData_Inner(num, msgType, remoteClient, text, number, number2, number3, number4, number5, number6, number7));
					length += lengths[i];
				}
				realLength = length;
			}
			else
			{
				length = NetMessage.SendData_Inner(num, msgType, remoteClient, text, number, number2, number3, number4, number5, number6, number7);
				realLength = length * amount;
			}
			if (realLength > 131070)
			{
				Main.NewText("You're sending too much data for a vanilla server to handle. You may be disconnected.", errorColor);
			}
			if (length > 65535)
			{
				throw new Exception("Maximum packet length exceeded. id: " + msgType + " length: " + length);
			}
			if (Main.netMode != 0 && length < 1)
			{
				Main.NewText("Packet length of 0. id: " + msgType + " length: " + length, byte.MaxValue, 25, 25);
				throw new Exception("Packet length of 0. id: " + msgType + " length: " + length);
			}
			if (Main.netMode == 1)
			{
				if (Netplay.Connection.Socket.IsConnected())
				{
					try
					{
						for (int j = 0; j < amount; j++)
						{
							NetMessage.buffer[num].spamCount++;
							quedPackets++;
							Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, length);
							if (!batchWrite)
							{
								Netplay.Connection.Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Connection.ClientWriteCallBack);
							}
						}
						if (batchWrite)
						{
							Netplay.Connection.Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Connection.ClientWriteCallBack);
						}
					}
					catch
					{
					}
				}
			}
			else if (remoteClient == -1)
			{
				switch (msgType)
				{
				case 34:
				case 69:
				{
					for (int m = 0; m < 256; m++)
					{
						if (m == ignoreClient || !NetMessage.buffer[m].broadcast || !Netplay.Clients[m].IsConnected())
						{
							continue;
						}
						try
						{
							for (int n = 0; n < amount; n++)
							{
								NetMessage.buffer[m].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, length);
								if (!batchWrite)
								{
									Netplay.Clients[m].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[m].ServerWriteCallBack);
								}
							}
							if (batchWrite)
							{
								Netplay.Clients[m].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[m].ServerWriteCallBack);
							}
						}
						catch
						{
						}
					}
					break;
				}
				case 20:
				{
					for (int num8 = 0; num8 < 256; num8++)
					{
						if (num8 == ignoreClient || !NetMessage.buffer[num8].broadcast || !Netplay.Clients[num8].IsConnected() || !Netplay.Clients[num8].SectionRange((int)Math.Max(number3, number4), number, (int)number2))
						{
							continue;
						}
						try
						{
							for (int num9 = 0; num9 < amount; num9++)
							{
								NetMessage.buffer[num8].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, length);
								if (!batchWrite)
								{
									Netplay.Clients[num8].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[num8].ServerWriteCallBack);
								}
							}
							if (batchWrite)
							{
								Netplay.Clients[num8].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[num8].ServerWriteCallBack);
							}
						}
						catch
						{
						}
					}
					break;
				}
				case 23:
				{
					NPC nPC4 = Main.npc[number];
					for (int num10 = 0; num10 < 256; num10++)
					{
						if (num10 == ignoreClient || !NetMessage.buffer[num10].broadcast || !Netplay.Clients[num10].IsConnected())
						{
							continue;
						}
						bool flag6 = false;
						if (nPC4.boss || nPC4.netAlways || nPC4.townNPC || !nPC4.active)
						{
							flag6 = true;
						}
						else if (nPC4.netSkip <= 0)
						{
							Rectangle rect5 = Main.player[num10].getRect();
							Rectangle rect6 = nPC4.getRect();
							rect6.X -= 2500;
							rect6.Y -= 2500;
							rect6.Width += 5000;
							rect6.Height += 5000;
							if (((Rectangle)(ref rect5)).Intersects(rect6))
							{
								flag6 = true;
							}
						}
						else
						{
							flag6 = true;
						}
						if (!flag6)
						{
							continue;
						}
						try
						{
							for (int num11 = 0; num11 < amount; num11++)
							{
								NetMessage.buffer[num10].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, length);
								if (!batchWrite)
								{
									Netplay.Clients[num10].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[num10].ServerWriteCallBack);
								}
							}
							if (batchWrite)
							{
								Netplay.Clients[num10].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[num10].ServerWriteCallBack);
							}
						}
						catch
						{
						}
					}
					nPC4.netSkip++;
					if (nPC4.netSkip > 4)
					{
						nPC4.netSkip = 0;
					}
					break;
				}
				case 28:
				{
					NPC nPC3 = Main.npc[number];
					for (int num4 = 0; num4 < 256; num4++)
					{
						if (num4 == ignoreClient || !NetMessage.buffer[num4].broadcast || !Netplay.Clients[num4].IsConnected())
						{
							continue;
						}
						bool flag5 = false;
						if (nPC3.life <= 0)
						{
							flag5 = true;
						}
						else
						{
							Rectangle rect3 = Main.player[num4].getRect();
							Rectangle rect4 = nPC3.getRect();
							rect4.X -= 3000;
							rect4.Y -= 3000;
							rect4.Width += 6000;
							rect4.Height += 6000;
							if (((Rectangle)(ref rect3)).Intersects(rect4))
							{
								flag5 = true;
							}
						}
						if (!flag5)
						{
							continue;
						}
						try
						{
							for (int num5 = 0; num5 < amount; num5++)
							{
								NetMessage.buffer[num4].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, length);
								if (!batchWrite)
								{
									Netplay.Clients[num4].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[num4].ServerWriteCallBack);
								}
							}
							if (batchWrite)
							{
								Netplay.Clients[num4].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[num4].ServerWriteCallBack);
							}
						}
						catch
						{
						}
					}
					break;
				}
				case 13:
				{
					for (int num6 = 0; num6 < 256; num6++)
					{
						if (num6 == ignoreClient || !NetMessage.buffer[num6].broadcast || !Netplay.Clients[num6].IsConnected())
						{
							continue;
						}
						try
						{
							for (int num7 = 0; num7 < amount; num7++)
							{
								NetMessage.buffer[num6].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, length);
								if (!batchWrite)
								{
									Netplay.Clients[num6].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[num6].ServerWriteCallBack);
								}
							}
							if (batchWrite)
							{
								Netplay.Clients[num6].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[num6].ServerWriteCallBack);
							}
						}
						catch
						{
						}
					}
					Main.player[number].netSkip++;
					if (Main.player[number].netSkip > 2)
					{
						Main.player[number].netSkip = 0;
					}
					break;
				}
				case 27:
				{
					Projectile projectile2 = Main.projectile[number];
					for (int num2 = 0; num2 < 256; num2++)
					{
						if (num2 == ignoreClient || !NetMessage.buffer[num2].broadcast || !Netplay.Clients[num2].IsConnected())
						{
							continue;
						}
						bool flag4 = false;
						if (projectile2.type == 12 || Main.projPet[projectile2.type] || projectile2.aiStyle == 11 || projectile2.netImportant)
						{
							flag4 = true;
						}
						else
						{
							Rectangle rect = Main.player[num2].getRect();
							Rectangle rect2 = projectile2.getRect();
							rect2.X -= 5000;
							rect2.Y -= 5000;
							rect2.Width += 10000;
							rect2.Height += 10000;
							if (((Rectangle)(ref rect)).Intersects(rect2))
							{
								flag4 = true;
							}
						}
						if (!flag4)
						{
							continue;
						}
						try
						{
							for (int num3 = 0; num3 < amount; num3++)
							{
								NetMessage.buffer[num2].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, length);
								if (!batchWrite)
								{
									Netplay.Clients[num2].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[num2].ServerWriteCallBack);
								}
							}
							if (batchWrite)
							{
								Netplay.Clients[num2].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[num2].ServerWriteCallBack);
							}
						}
						catch
						{
						}
					}
					break;
				}
				default:
				{
					for (int k = 0; k < 256; k++)
					{
						if (k == ignoreClient || NetMessage.buffer[k] == null || Netplay.Clients[k] == null || (!NetMessage.buffer[k].broadcast && (Netplay.Clients[k].State < 3 || msgType != 10)) || !Netplay.Clients[k].IsConnected())
						{
							continue;
						}
						try
						{
							for (int l = 0; l < amount; l++)
							{
								NetMessage.buffer[k].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, length);
								if (!batchWrite)
								{
									Netplay.Clients[k].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[k].ServerWriteCallBack);
								}
							}
							if (batchWrite)
							{
								Netplay.Clients[k].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[k].ServerWriteCallBack);
							}
						}
						catch
						{
						}
					}
					break;
				}
				}
			}
			else if (Netplay.Clients[remoteClient].IsConnected())
			{
				try
				{
					for (int num12 = 0; num12 < amount; num12++)
					{
						NetMessage.buffer[remoteClient].spamCount++;
						Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, length);
						if (!batchWrite)
						{
							Netplay.Clients[remoteClient].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[remoteClient].ServerWriteCallBack);
						}
					}
					if (batchWrite)
					{
						Netplay.Clients[remoteClient].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, length, Netplay.Clients[remoteClient].ServerWriteCallBack);
					}
				}
				catch
				{
				}
			}
			if (Main.verboseNetplay)
			{
				for (int num13 = 0; num13 < length; num13++)
				{
				}
				for (int num14 = 0; num14 < length; num14++)
				{
					_ = NetMessage.buffer[num].writeBuffer[num14];
				}
			}
			NetMessage.buffer[num].writeLocked = false;
			if (msgType == 2 && Main.netMode == 2)
			{
				Netplay.Clients[num].PendingTermination = true;
				Netplay.Clients[num].PendingTerminationApproved = true;
			}
			if (NetMessage.sectionFailed)
			{
				NetMessage.sectionFailed = false;
				NetMessage.SendSignAndChestData_Section(remoteClient, number, (int)number2, (short)number3, (short)number4);
			}
		}
	}

	public static void FakeItemPacket(int itemId, short slot = 58, byte prefix = 0, short stack = 1)
	{
		if (Main.netMode == 0)
		{
			return;
		}
		lock (NetMessage.buffer[256])
		{
			BinaryWriter writer = NetMessage.buffer[256].writer;
			if (writer == null)
			{
				NetMessage.buffer[256].ResetWriter();
				writer = NetMessage.buffer[256].writer;
			}
			writer.BaseStream.Position = 0L;
			long position = writer.BaseStream.Position;
			writer.BaseStream.Position += 2L;
			writer.Write((byte)5);
			writer.Write((byte)Main.myPlayer);
			writer.Write(slot);
			writer.Write(stack);
			writer.Write(prefix);
			writer.Write((short)itemId);
			int num3 = (int)writer.BaseStream.Position;
			if (num3 > 65535)
			{
				throw new Exception("Maximum packet length exceeded. id: " + (byte)5 + " length: " + num3);
			}
			writer.BaseStream.Position = position;
			writer.Write((ushort)num3);
			writer.BaseStream.Position = num3;
			if (Main.netMode == 1 && Netplay.Connection.Socket.IsConnected())
			{
				try
				{
					NetMessage.buffer[256].spamCount++;
					Main.ActiveNetDiagnosticsUI.CountSentMessage(5, num3);
					Netplay.Connection.Socket.AsyncSend(NetMessage.buffer[256].writeBuffer, 0, num3, Netplay.Connection.ClientWriteCallBack);
				}
				catch
				{
				}
			}
			if (Main.verboseNetplay)
			{
				for (int j = 0; j < num3; j++)
				{
				}
				for (int k = 0; k < num3; k++)
				{
					_ = NetMessage.buffer[256].writeBuffer[k];
				}
			}
			NetMessage.buffer[256].writeLocked = false;
		}
	}

	public static void FakeNetMessage13(Vector2 plrPos, int heldItem)
	{
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 0)
		{
			return;
		}
		int num = 256;
		int num2 = 13;
		int whoAmI = Main.LocalPlayer.whoAmI;
		lock (NetMessage.buffer[num])
		{
			BinaryWriter writer = NetMessage.buffer[num].writer;
			if (writer == null)
			{
				NetMessage.buffer[num].ResetWriter();
				writer = NetMessage.buffer[num].writer;
			}
			writer.BaseStream.Position = 0L;
			long position = writer.BaseStream.Position;
			writer.BaseStream.Position += 2L;
			writer.Write((byte)num2);
			Player player = Main.player[whoAmI];
			writer.Write((byte)whoAmI);
			BitsByte bitsByte = (byte)0;
			bitsByte[0] = player.controlUp;
			bitsByte[1] = player.controlDown;
			bitsByte[2] = player.controlLeft;
			bitsByte[3] = player.controlRight;
			bitsByte[4] = player.controlJump;
			bitsByte[5] = player.controlUseItem;
			bitsByte[6] = player.direction == 1;
			writer.Write(bitsByte);
			BitsByte bitsByte2 = (byte)0;
			bitsByte2[0] = player.pulley;
			bitsByte2[1] = player.pulley && player.pulleyDir == 2;
			bitsByte2[2] = player.velocity != Vector2.Zero;
			bitsByte2[3] = player.vortexStealthActive;
			bitsByte2[4] = player.gravDir == 1f;
			bitsByte2[5] = player.shieldRaised;
			bitsByte2[6] = player.ghost;
			writer.Write(bitsByte2);
			BitsByte bitsByte3 = (byte)0;
			bitsByte3[0] = player.tryKeepingHoveringUp;
			bitsByte3[1] = player.IsVoidVaultEnabled;
			bitsByte3[2] = player.sitting.isSitting;
			bitsByte3[3] = player.downedDD2EventAnyDifficulty;
			bitsByte3[4] = player.isPettingAnimal;
			bitsByte3[5] = player.isTheAnimalBeingPetSmall;
			bitsByte3[6] = player.PotionOfReturnOriginalUsePosition.HasValue;
			bitsByte3[7] = player.tryKeepingHoveringDown;
			writer.Write(bitsByte3);
			BitsByte bitsByte4 = (byte)0;
			bitsByte4[0] = player.sleeping.isSleeping;
			bitsByte4[1] = player.autoReuseAllWeapons;
			bitsByte4[2] = player.controlDownHold;
			bitsByte4[3] = player.isOperatingAnotherEntity;
			writer.Write(bitsByte4);
			writer.Write((byte)heldItem);
			writer.WriteVector2(plrPos);
			if (bitsByte2[2])
			{
				writer.WriteVector2(Vector2.Zero);
			}
			if (bitsByte3[6])
			{
				writer.WriteVector2(player.PotionOfReturnOriginalUsePosition.Value);
				writer.WriteVector2(player.PotionOfReturnHomePosition.Value);
			}
			int num3 = (int)writer.BaseStream.Position;
			if (num3 > 65535)
			{
				throw new Exception("Maximum packet length exceeded. id: " + num2 + " length: " + num3);
			}
			writer.BaseStream.Position = position;
			writer.Write((ushort)num3);
			writer.BaseStream.Position = num3;
			if (Main.netMode == 1)
			{
				if (Netplay.Connection.Socket.IsConnected())
				{
					try
					{
						NetMessage.buffer[num].spamCount++;
						Main.ActiveNetDiagnosticsUI.CountSentMessage(num2, num3);
						Netplay.Connection.Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num3, Netplay.Connection.ClientWriteCallBack);
					}
					catch
					{
					}
				}
			}
			else if (num2 == 13)
			{
				for (int i = 0; i < 256; i++)
				{
					if (NetMessage.buffer[i].broadcast && Netplay.Clients[i].IsConnected())
					{
						try
						{
							NetMessage.buffer[i].spamCount++;
							Main.ActiveNetDiagnosticsUI.CountSentMessage(num2, num3);
							Netplay.Clients[i].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num3, Netplay.Clients[i].ServerWriteCallBack);
						}
						catch
						{
						}
					}
				}
				Main.player[whoAmI].netSkip++;
				if (Main.player[whoAmI].netSkip > 2)
				{
					Main.player[whoAmI].netSkip = 0;
				}
			}
			if (Main.verboseNetplay)
			{
				for (int j = 0; j < num3; j++)
				{
				}
				for (int k = 0; k < num3; k++)
				{
					_ = NetMessage.buffer[num].writeBuffer[k];
				}
			}
			NetMessage.buffer[num].writeLocked = false;
			if (num2 == 2 && Main.netMode == 2)
			{
				Netplay.Clients[num].PendingTermination = true;
				Netplay.Clients[num].PendingTerminationApproved = true;
			}
		}
	}

	public void FalsePlayerPacket(Vector2 plrPos)
	{
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 0)
		{
			return;
		}
		int num = 256;
		int num2 = 13;
		int whoAmI = Main.LocalPlayer.whoAmI;
		lock (NetMessage.buffer[num])
		{
			BinaryWriter writer = NetMessage.buffer[num].writer;
			if (writer == null)
			{
				NetMessage.buffer[num].ResetWriter();
				writer = NetMessage.buffer[num].writer;
			}
			writer.BaseStream.Position = 0L;
			long position = writer.BaseStream.Position;
			writer.BaseStream.Position += 2L;
			writer.Write((byte)num2);
			Player player = Main.player[whoAmI];
			writer.Write((byte)whoAmI);
			BitsByte bitsByte = (byte)0;
			bitsByte[0] = player.controlUp;
			bitsByte[1] = player.controlDown;
			bitsByte[2] = player.controlLeft;
			bitsByte[3] = player.controlRight;
			bitsByte[4] = player.controlJump;
			bitsByte[5] = player.controlUseItem;
			bitsByte[6] = player.direction == 1;
			writer.Write(bitsByte);
			BitsByte bitsByte2 = (byte)0;
			bitsByte2[0] = player.pulley;
			bitsByte2[1] = player.pulley && player.pulleyDir == 2;
			bitsByte2[2] = player.velocity != Vector2.Zero;
			bitsByte2[3] = player.vortexStealthActive;
			bitsByte2[4] = player.gravDir == 1f;
			bitsByte2[5] = player.shieldRaised;
			bitsByte2[6] = player.ghost;
			writer.Write(bitsByte2);
			BitsByte bitsByte3 = (byte)0;
			bitsByte3[0] = player.tryKeepingHoveringUp;
			bitsByte3[1] = player.IsVoidVaultEnabled;
			bitsByte3[2] = player.sitting.isSitting;
			bitsByte3[3] = player.downedDD2EventAnyDifficulty;
			bitsByte3[4] = player.isPettingAnimal;
			bitsByte3[5] = player.isTheAnimalBeingPetSmall;
			bitsByte3[6] = player.PotionOfReturnOriginalUsePosition.HasValue;
			bitsByte3[7] = player.tryKeepingHoveringDown;
			writer.Write(bitsByte3);
			BitsByte bitsByte4 = (byte)0;
			bitsByte4[0] = player.sleeping.isSleeping;
			bitsByte4[1] = player.autoReuseAllWeapons;
			bitsByte4[2] = player.controlDownHold;
			bitsByte4[3] = player.isOperatingAnotherEntity;
			writer.Write(bitsByte4);
			writer.Write((byte)player.selectedItem);
			writer.WriteVector2(plrPos);
			if (bitsByte2[2])
			{
				writer.WriteVector2(player.velocity);
			}
			if (bitsByte3[6])
			{
				writer.WriteVector2(player.PotionOfReturnOriginalUsePosition.Value);
				writer.WriteVector2(player.PotionOfReturnHomePosition.Value);
			}
			int num3 = (int)writer.BaseStream.Position;
			if (num3 > 65535)
			{
				throw new Exception("Maximum packet length exceeded. id: " + num2 + " length: " + num3);
			}
			writer.BaseStream.Position = position;
			writer.Write((ushort)num3);
			writer.BaseStream.Position = num3;
			if (Main.netMode == 1)
			{
				if (Netplay.Connection.Socket.IsConnected())
				{
					try
					{
						NetMessage.buffer[num].spamCount++;
						Main.ActiveNetDiagnosticsUI.CountSentMessage(num2, num3);
						Netplay.Connection.Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num3, Netplay.Connection.ClientWriteCallBack);
					}
					catch
					{
					}
				}
			}
			else if (num2 == 13)
			{
				for (int i = 0; i < 256; i++)
				{
					if (NetMessage.buffer[i].broadcast && Netplay.Clients[i].IsConnected())
					{
						try
						{
							NetMessage.buffer[i].spamCount++;
							Main.ActiveNetDiagnosticsUI.CountSentMessage(num2, num3);
							Netplay.Clients[i].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num3, Netplay.Clients[i].ServerWriteCallBack);
						}
						catch
						{
						}
					}
				}
				Main.player[whoAmI].netSkip++;
				if (Main.player[whoAmI].netSkip > 2)
				{
					Main.player[whoAmI].netSkip = 0;
				}
			}
			if (Main.verboseNetplay)
			{
				for (int j = 0; j < num3; j++)
				{
				}
				for (int k = 0; k < num3; k++)
				{
					_ = NetMessage.buffer[num].writeBuffer[k];
				}
			}
			NetMessage.buffer[num].writeLocked = false;
			if (num2 == 2 && Main.netMode == 2)
			{
				Netplay.Clients[num].PendingTermination = true;
				Netplay.Clients[num].PendingTerminationApproved = true;
			}
		}
	}
}
