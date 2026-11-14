using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.ZazaMod.CheatSystem;
using Terraria.ZazaMod.WorldEdit;

namespace Terraria.ZazaMod.UI.Legacy;

public class ModsUICommandPanelButtons : UIState
{
	public static UIList commandButtonsList;

	public static ModsUICommandPanelButtons uiStateCommandPanelButtons;

	public static UIElement commandPanelBackground;

	private static bool updatePlayerList;

	private static UITextPanel<LocalizedText>[] playerHacksButton;

	private static bool updateTravelList;

	private static bool updateMiscsList;

	private static bool updateBuilderMenuList;

	private static UITextPanel<LocalizedText>[] travelHacksButton;

	private static UITextPanel<LocalizedText>[] miscsHacksButton;

	private static UITextPanel<LocalizedText>[] builderMenuHacksButton;

	private static UITextPanel<LocalizedText> displaySize;

	private static UITextPanel<LocalizedText>[] displayBrushSize;

	private static bool specialButtons;

	private static bool plusButton;

	private static bool minusButton;

	private static List<UITextPanel<LocalizedText>> specialButtonsList;

	private static UITextPanel<LocalizedText>[] selectedItemHacksButton;

	private static int prevSelectedItem;

	public static int sharedPrevSelectedItem;

	private static UITextPanel<LocalizedText>[] zoneHacksButton;

	private static UITextPanel<LocalizedText>[] cosmeticHacksButton;

	public override void OnInitialize()
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Unknown result type (might be due to invalid IL or missing references)
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_082c: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0989: Unknown result type (might be due to invalid IL or missing references)
		//IL_098a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0997: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ded: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f67: Unknown result type (might be due to invalid IL or missing references)
		//IL_101e: Unknown result type (might be due to invalid IL or missing references)
		//IL_101f: Unknown result type (might be due to invalid IL or missing references)
		//IL_102c: Unknown result type (might be due to invalid IL or missing references)
		//IL_102e: Unknown result type (might be due to invalid IL or missing references)
		//IL_10be: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_1197: Unknown result type (might be due to invalid IL or missing references)
		//IL_1198: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_125c: Unknown result type (might be due to invalid IL or missing references)
		//IL_125d: Unknown result type (might be due to invalid IL or missing references)
		//IL_126a: Unknown result type (might be due to invalid IL or missing references)
		//IL_126c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1311: Unknown result type (might be due to invalid IL or missing references)
		//IL_1312: Unknown result type (might be due to invalid IL or missing references)
		//IL_131f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1321: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_148b: Unknown result type (might be due to invalid IL or missing references)
		//IL_148c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1499: Unknown result type (might be due to invalid IL or missing references)
		//IL_149b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1550: Unknown result type (might be due to invalid IL or missing references)
		//IL_1551: Unknown result type (might be due to invalid IL or missing references)
		//IL_155e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1560: Unknown result type (might be due to invalid IL or missing references)
		//IL_1615: Unknown result type (might be due to invalid IL or missing references)
		//IL_1616: Unknown result type (might be due to invalid IL or missing references)
		//IL_1623: Unknown result type (might be due to invalid IL or missing references)
		//IL_1625: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_16cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_16da: Unknown result type (might be due to invalid IL or missing references)
		//IL_177f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1780: Unknown result type (might be due to invalid IL or missing references)
		//IL_178d: Unknown result type (might be due to invalid IL or missing references)
		//IL_178f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1847: Unknown result type (might be due to invalid IL or missing references)
		//IL_1848: Unknown result type (might be due to invalid IL or missing references)
		//IL_1855: Unknown result type (might be due to invalid IL or missing references)
		//IL_1857: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_19cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a76: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a77: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d68: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f89: Unknown result type (might be due to invalid IL or missing references)
		//IL_202f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2030: Unknown result type (might be due to invalid IL or missing references)
		//IL_203d: Unknown result type (might be due to invalid IL or missing references)
		//IL_203f: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_219b: Unknown result type (might be due to invalid IL or missing references)
		//IL_219c: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_2251: Unknown result type (might be due to invalid IL or missing references)
		//IL_2252: Unknown result type (might be due to invalid IL or missing references)
		//IL_225f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2261: Unknown result type (might be due to invalid IL or missing references)
		//IL_2307: Unknown result type (might be due to invalid IL or missing references)
		//IL_2308: Unknown result type (might be due to invalid IL or missing references)
		//IL_2315: Unknown result type (might be due to invalid IL or missing references)
		//IL_2317: Unknown result type (might be due to invalid IL or missing references)
		//IL_23bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_23be: Unknown result type (might be due to invalid IL or missing references)
		//IL_23cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_23cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2473: Unknown result type (might be due to invalid IL or missing references)
		//IL_2474: Unknown result type (might be due to invalid IL or missing references)
		//IL_2481: Unknown result type (might be due to invalid IL or missing references)
		//IL_2483: Unknown result type (might be due to invalid IL or missing references)
		//IL_2529: Unknown result type (might be due to invalid IL or missing references)
		//IL_252a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2537: Unknown result type (might be due to invalid IL or missing references)
		//IL_2539: Unknown result type (might be due to invalid IL or missing references)
		//IL_25df: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_2695: Unknown result type (might be due to invalid IL or missing references)
		//IL_2696: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_274b: Unknown result type (might be due to invalid IL or missing references)
		//IL_274c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2759: Unknown result type (might be due to invalid IL or missing references)
		//IL_275b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2813: Unknown result type (might be due to invalid IL or missing references)
		//IL_2814: Unknown result type (might be due to invalid IL or missing references)
		//IL_2821: Unknown result type (might be due to invalid IL or missing references)
		//IL_2823: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_28c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_28c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_298d: Unknown result type (might be due to invalid IL or missing references)
		//IL_298e: Unknown result type (might be due to invalid IL or missing references)
		//IL_299b: Unknown result type (might be due to invalid IL or missing references)
		//IL_299d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a62: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bca: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c86: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c93: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c95: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d40: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d41: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e09: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ec4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3027: Unknown result type (might be due to invalid IL or missing references)
		//IL_3028: Unknown result type (might be due to invalid IL or missing references)
		//IL_3035: Unknown result type (might be due to invalid IL or missing references)
		//IL_3037: Unknown result type (might be due to invalid IL or missing references)
		//IL_30e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_30e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_319f: Unknown result type (might be due to invalid IL or missing references)
		//IL_31a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_31ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_31af: Unknown result type (might be due to invalid IL or missing references)
		//IL_325b: Unknown result type (might be due to invalid IL or missing references)
		//IL_325c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3269: Unknown result type (might be due to invalid IL or missing references)
		//IL_326b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3317: Unknown result type (might be due to invalid IL or missing references)
		//IL_3318: Unknown result type (might be due to invalid IL or missing references)
		//IL_3325: Unknown result type (might be due to invalid IL or missing references)
		//IL_3327: Unknown result type (might be due to invalid IL or missing references)
		//IL_33d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_33d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_348f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3490: Unknown result type (might be due to invalid IL or missing references)
		//IL_349d: Unknown result type (might be due to invalid IL or missing references)
		//IL_349f: Unknown result type (might be due to invalid IL or missing references)
		//IL_354b: Unknown result type (might be due to invalid IL or missing references)
		//IL_354c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3559: Unknown result type (might be due to invalid IL or missing references)
		//IL_355b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3601: Unknown result type (might be due to invalid IL or missing references)
		//IL_3602: Unknown result type (might be due to invalid IL or missing references)
		//IL_360f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3611: Unknown result type (might be due to invalid IL or missing references)
		//IL_36bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_36be: Unknown result type (might be due to invalid IL or missing references)
		//IL_36cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_36cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3779: Unknown result type (might be due to invalid IL or missing references)
		//IL_377a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3787: Unknown result type (might be due to invalid IL or missing references)
		//IL_3789: Unknown result type (might be due to invalid IL or missing references)
		//IL_3835: Unknown result type (might be due to invalid IL or missing references)
		//IL_3836: Unknown result type (might be due to invalid IL or missing references)
		//IL_3843: Unknown result type (might be due to invalid IL or missing references)
		//IL_3845: Unknown result type (might be due to invalid IL or missing references)
		//IL_3903: Unknown result type (might be due to invalid IL or missing references)
		//IL_3904: Unknown result type (might be due to invalid IL or missing references)
		//IL_3911: Unknown result type (might be due to invalid IL or missing references)
		//IL_3913: Unknown result type (might be due to invalid IL or missing references)
		//IL_39a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_39a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_39b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_39b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a91: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b77: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c13: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c14: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c21: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c23: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cce: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cde: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d89: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d97: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d99: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ef9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3efa: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f07: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_406f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4070: Unknown result type (might be due to invalid IL or missing references)
		//IL_407d: Unknown result type (might be due to invalid IL or missing references)
		//IL_407f: Unknown result type (might be due to invalid IL or missing references)
		//IL_412b: Unknown result type (might be due to invalid IL or missing references)
		//IL_412c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4139: Unknown result type (might be due to invalid IL or missing references)
		//IL_413b: Unknown result type (might be due to invalid IL or missing references)
		//IL_41e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_41e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_41ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_41f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_42c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_42c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_42d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_42d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4383: Unknown result type (might be due to invalid IL or missing references)
		//IL_4384: Unknown result type (might be due to invalid IL or missing references)
		//IL_4391: Unknown result type (might be due to invalid IL or missing references)
		//IL_4393: Unknown result type (might be due to invalid IL or missing references)
		//IL_443f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4440: Unknown result type (might be due to invalid IL or missing references)
		//IL_444d: Unknown result type (might be due to invalid IL or missing references)
		//IL_444f: Unknown result type (might be due to invalid IL or missing references)
		//IL_44fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_44fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4509: Unknown result type (might be due to invalid IL or missing references)
		//IL_450b: Unknown result type (might be due to invalid IL or missing references)
		//IL_45b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_45b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_45c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_45c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_466d: Unknown result type (might be due to invalid IL or missing references)
		//IL_466e: Unknown result type (might be due to invalid IL or missing references)
		//IL_467b: Unknown result type (might be due to invalid IL or missing references)
		//IL_467d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4753: Unknown result type (might be due to invalid IL or missing references)
		//IL_4754: Unknown result type (might be due to invalid IL or missing references)
		//IL_4761: Unknown result type (might be due to invalid IL or missing references)
		//IL_4763: Unknown result type (might be due to invalid IL or missing references)
		//IL_480f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4810: Unknown result type (might be due to invalid IL or missing references)
		//IL_481d: Unknown result type (might be due to invalid IL or missing references)
		//IL_481f: Unknown result type (might be due to invalid IL or missing references)
		//IL_48cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_48cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_48d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_48db: Unknown result type (might be due to invalid IL or missing references)
		//IL_4987: Unknown result type (might be due to invalid IL or missing references)
		//IL_4988: Unknown result type (might be due to invalid IL or missing references)
		//IL_4995: Unknown result type (might be due to invalid IL or missing references)
		//IL_4997: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a53: Unknown result type (might be due to invalid IL or missing references)
		//IL_4aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c97: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c99: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d47: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d48: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d55: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d57: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ec8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ed6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ed8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f79: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_500e: Unknown result type (might be due to invalid IL or missing references)
		//IL_500f: Unknown result type (might be due to invalid IL or missing references)
		//IL_501c: Unknown result type (might be due to invalid IL or missing references)
		//IL_501e: Unknown result type (might be due to invalid IL or missing references)
		//IL_50b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_50b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_50bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_50c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_5154: Unknown result type (might be due to invalid IL or missing references)
		//IL_5155: Unknown result type (might be due to invalid IL or missing references)
		//IL_5162: Unknown result type (might be due to invalid IL or missing references)
		//IL_5164: Unknown result type (might be due to invalid IL or missing references)
		//IL_51f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_51f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5205: Unknown result type (might be due to invalid IL or missing references)
		//IL_5207: Unknown result type (might be due to invalid IL or missing references)
		//IL_529a: Unknown result type (might be due to invalid IL or missing references)
		//IL_529b: Unknown result type (might be due to invalid IL or missing references)
		//IL_52a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_52aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_533d: Unknown result type (might be due to invalid IL or missing references)
		//IL_533e: Unknown result type (might be due to invalid IL or missing references)
		//IL_534b: Unknown result type (might be due to invalid IL or missing references)
		//IL_534d: Unknown result type (might be due to invalid IL or missing references)
		//IL_53e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_53e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_53ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_53f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_5495: Unknown result type (might be due to invalid IL or missing references)
		//IL_5496: Unknown result type (might be due to invalid IL or missing references)
		//IL_54a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_54a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_554b: Unknown result type (might be due to invalid IL or missing references)
		//IL_554c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5559: Unknown result type (might be due to invalid IL or missing references)
		//IL_555b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5601: Unknown result type (might be due to invalid IL or missing references)
		//IL_5602: Unknown result type (might be due to invalid IL or missing references)
		//IL_560f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5611: Unknown result type (might be due to invalid IL or missing references)
		base.OnInitialize();
		commandPanelBackground.Width.Set(515f, 0f);
		commandPanelBackground.Height.Set(585f, 0f);
		commandPanelBackground.SetPadding(10f);
		UISlicedImage uISlicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", (AssetRequestMode)1))
		{
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			HAlign = 0.5f,
			VAlign = 0.5f
		};
		uISlicedImage.SetPadding(10f);
		uISlicedImage.SetSliceDepths(10);
		uISlicedImage.Color = new Color(11, 84, 95) * 0.55f;
		commandPanelBackground.Append(uISlicedImage);
		commandButtonsList.Width.Set(0f, 1f);
		commandButtonsList.Height.Set(0f, 1f);
		commandButtonsList.HAlign = 0.5f;
		commandButtonsList.VAlign = 0.5f;
		commandButtonsList.ListPadding = 3f;
		commandButtonsList.ManualSortMethod = ManualIfnoSortingMethod;
		uISlicedImage.Append(commandButtonsList);
		UIScrollbar uIScrollbar = new UIScrollbar();
		uIScrollbar.SetView(100f, 1000f);
		uIScrollbar.Height.Set(0f, 1f);
		uIScrollbar.HAlign = 1f;
		uISlicedImage.Append(uIScrollbar);
		commandButtonsList.SetScrollbar(uIScrollbar);
		Color backgroundColor = new Color(9, 46, 89) * 0.75f;
		Color borderColor = new Color(0, 0, 0) * 0.75f;
		float precent = 0.945f;
		float pixels = 55f;
		float precent2 = 0.9f;
		float pixels2 = 40f;
		int num = 0;
		bool fakeBool = false;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("⮞ Player"), 0.45f, large: true);
		playerHacksButton[num].Width.Set(0f, precent);
		playerHacksButton[num].Height.Set(pixels, 0f);
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[0].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[0], ref Mods.commandPanelPlayerButton);
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.commandPanelPlayerButton);
		};
		commandButtonsList.Add(playerHacksButton[num]);
		num++;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Godmode"), 0.35f, large: true);
		playerHacksButton[num].MarginTop = 6f;
		playerHacksButton[num].Width.Set(0f, precent2);
		playerHacksButton[num].Height.Set(pixels2, 0f);
		playerHacksButton[num].HAlign = 0.245f;
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[1].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[1], CheatManager.GetCheat<Cheats.GodmodeCheat>());
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, CheatManager.GetCheat<Cheats.GodmodeCheat>());
		};
		num++;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Dodge Everything"), 0.35f, large: true);
		playerHacksButton[num].Width.Set(0f, precent2);
		playerHacksButton[num].Height.Set(pixels2, 0f);
		playerHacksButton[num].HAlign = 0.245f;
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[2].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[2], ref Mods.dodgeEverything);
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.dodgeEverything);
		};
		num++;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Anti-Death"), 0.35f, large: true);
		playerHacksButton[num].Width.Set(0f, precent2);
		playerHacksButton[num].Height.Set(pixels2, 0f);
		playerHacksButton[num].HAlign = 0.245f;
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[3].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[3], ref Mods.antiDeathGod);
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.antiDeathGod);
		};
		num++;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Infinite Mana"), 0.35f, large: true);
		playerHacksButton[num].Width.Set(0f, precent2);
		playerHacksButton[num].Height.Set(pixels2, 0f);
		playerHacksButton[num].HAlign = 0.245f;
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[4].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[4], ref Mods.infiniteMana);
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.infiniteMana);
		};
		num++;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Full Bright"), 0.35f, large: true);
		playerHacksButton[num].Width.Set(0f, precent2);
		playerHacksButton[num].Height.Set(pixels2, 0f);
		playerHacksButton[num].HAlign = 0.245f;
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[5].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[5], ref Mods.fullBright);
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.fullBright);
		};
		num++;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Journey Menu"), 0.35f, large: true);
		playerHacksButton[num].Width.Set(0f, precent2);
		playerHacksButton[num].Height.Set(pixels2, 0f);
		playerHacksButton[num].HAlign = 0.245f;
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[6].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[6], ref Mods.journeyMenu);
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.journeyMenu);
		};
		num++;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Instant Respawn"), 0.35f, large: true);
		playerHacksButton[num].Width.Set(0f, precent2);
		playerHacksButton[num].Height.Set(pixels2, 0f);
		playerHacksButton[num].HAlign = 0.245f;
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[7].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[7], ref Mods.instantRespawn);
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.instantRespawn);
		};
		num++;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Extended Pick-Up Range"), 0.35f, large: true);
		playerHacksButton[num].Width.Set(0f, precent2);
		playerHacksButton[num].Height.Set(pixels2, 0f);
		playerHacksButton[num].HAlign = 0.245f;
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[8].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[8], ref Mods.extendedPickUpRange);
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.extendedPickUpRange);
		};
		num++;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Extended Placement Range"), 0.35f, large: true);
		playerHacksButton[num].Width.Set(0f, precent2);
		playerHacksButton[num].Height.Set(pixels2, 0f);
		playerHacksButton[num].HAlign = 0.245f;
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[9].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[9], ref Mods.extendedPlacementRange);
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.extendedPlacementRange);
		};
		num++;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Infinite Minions"), 0.35f, large: true);
		playerHacksButton[num].Width.Set(0f, precent2);
		playerHacksButton[num].Height.Set(pixels2, 0f);
		playerHacksButton[num].HAlign = 0.245f;
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[10].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[10], ref Mods.infiniteMinions);
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.infiniteMinions);
		};
		num++;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Infinite Ammo"), 0.35f, large: true);
		playerHacksButton[num].Width.Set(0f, precent2);
		playerHacksButton[num].Height.Set(pixels2, 0f);
		playerHacksButton[num].HAlign = 0.245f;
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[11].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[11], ref Mods.infiniteAmmo);
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.infiniteAmmo);
		};
		num++;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Ignore Liquids"), 0.35f, large: true);
		playerHacksButton[num].Width.Set(0f, precent2);
		playerHacksButton[num].Height.Set(pixels2, 0f);
		playerHacksButton[num].HAlign = 0.245f;
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[12].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[12], ref Mods.ignoreLiquids);
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.ignoreLiquids);
		};
		num++;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Ignore Damage Blocks"), 0.35f, large: true);
		playerHacksButton[num].Width.Set(0f, precent2);
		playerHacksButton[num].Height.Set(pixels2, 0f);
		playerHacksButton[num].HAlign = 0.245f;
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[13].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[13], ref Mods.ignoreDamageBlocks);
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.ignoreDamageBlocks);
		};
		num++;
		playerHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Drop Entire Inventory"), 0.35f, large: true);
		playerHacksButton[num].Width.Set(0f, precent2);
		playerHacksButton[num].Height.Set(pixels2, 0f);
		playerHacksButton[num].HAlign = 0.245f;
		playerHacksButton[num].MarginBottom = 6f;
		playerHacksButton[num].BackgroundColor = backgroundColor;
		playerHacksButton[num].BorderColor = borderColor;
		playerHacksButton[14].OnUpdate += delegate
		{
			UpdateButtonVariables(playerHacksButton[14], ref fakeBool);
		};
		playerHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num = 0;
		travelHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("⮞ Travel"), 0.45f, large: true);
		travelHacksButton[num].Width.Set(0f, precent);
		travelHacksButton[num].Height.Set(pixels, 0f);
		travelHacksButton[num].BackgroundColor = backgroundColor;
		travelHacksButton[num].BorderColor = borderColor;
		travelHacksButton[0].OnUpdate += delegate
		{
			UpdateButtonVariables(travelHacksButton[0], ref Mods.commandPanelTravelButton);
		};
		travelHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.commandPanelTravelButton);
		};
		commandButtonsList.Add(travelHacksButton[num]);
		num++;
		travelHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Noclip"), 0.35f, large: true);
		travelHacksButton[num].MarginTop = 6f;
		travelHacksButton[num].Width.Set(0f, precent2);
		travelHacksButton[num].Height.Set(pixels2, 0f);
		travelHacksButton[num].HAlign = 0.245f;
		travelHacksButton[num].BackgroundColor = backgroundColor;
		travelHacksButton[num].BorderColor = borderColor;
		travelHacksButton[1].OnUpdate += delegate
		{
			UpdateButtonVariables(travelHacksButton[1], ref Mods.noclip);
		};
		travelHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.noclip);
		};
		num++;
		travelHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Mouse Teleportation"), 0.35f, large: true);
		travelHacksButton[num].Width.Set(0f, precent2);
		travelHacksButton[num].Height.Set(pixels2, 0f);
		travelHacksButton[num].HAlign = 0.245f;
		travelHacksButton[num].BackgroundColor = backgroundColor;
		travelHacksButton[num].BorderColor = borderColor;
		travelHacksButton[2].OnUpdate += delegate
		{
			UpdateButtonVariables(travelHacksButton[2], ref Mods.gotoMouse);
		};
		travelHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.gotoMouse);
		};
		num++;
		travelHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Map Teleportation"), 0.35f, large: true);
		travelHacksButton[num].Width.Set(0f, precent2);
		travelHacksButton[num].Height.Set(pixels2, 0f);
		travelHacksButton[num].HAlign = 0.245f;
		travelHacksButton[num].BackgroundColor = backgroundColor;
		travelHacksButton[num].BorderColor = borderColor;
		travelHacksButton[3].OnUpdate += delegate
		{
			UpdateButtonVariables(travelHacksButton[3], ref Mods.mapTeleportation);
		};
		travelHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.mapTeleportation);
		};
		num++;
		travelHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Teleport Item Bypass"), 0.35f, large: true);
		travelHacksButton[num].MarginBottom = 6f;
		travelHacksButton[num].Width.Set(0f, precent2);
		travelHacksButton[num].Height.Set(pixels2, 0f);
		travelHacksButton[num].HAlign = 0.245f;
		travelHacksButton[num].BackgroundColor = backgroundColor;
		travelHacksButton[num].BorderColor = borderColor;
		travelHacksButton[4].OnUpdate += delegate
		{
			UpdateButtonVariables(travelHacksButton[4], ref Mods.bypassTeleportRequests);
		};
		travelHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.bypassTeleportRequests);
		};
		num = 0;
		cosmeticHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("⮞ Cosmetics"), 0.45f, large: true);
		cosmeticHacksButton[num].Width.Set(0f, precent);
		cosmeticHacksButton[num].Height.Set(pixels, 0f);
		cosmeticHacksButton[num].BackgroundColor = backgroundColor;
		cosmeticHacksButton[num].BorderColor = borderColor;
		cosmeticHacksButton[0].OnUpdate += delegate
		{
			UpdateButtonVariables(cosmeticHacksButton[0], ref ModsVars.cosmeticHacksButton);
		};
		cosmeticHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.cosmeticHacksButton);
		};
		commandButtonsList.Add(cosmeticHacksButton[num]);
		num++;
		cosmeticHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Shimmer Walking Particles"), 0.35f, large: true);
		cosmeticHacksButton[num].MarginTop = 6f;
		cosmeticHacksButton[num].Width.Set(0f, precent2);
		cosmeticHacksButton[num].Height.Set(pixels2, 0f);
		cosmeticHacksButton[num].HAlign = 0.245f;
		cosmeticHacksButton[num].BackgroundColor = backgroundColor;
		cosmeticHacksButton[num].BorderColor = borderColor;
		cosmeticHacksButton[1].OnUpdate += delegate
		{
			UpdateButtonVariables(cosmeticHacksButton[1], ref Mods.shimmerWalkParticles);
		};
		cosmeticHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.shimmerWalkParticles);
		};
		num++;
		cosmeticHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Death Particle: " + GetDeathParticleName(Mods.deathParticleCosmeticId)), 0.35f, large: true);
		cosmeticHacksButton[num].Width.Set(0f, precent2);
		cosmeticHacksButton[num].Height.Set(pixels2, 0f);
		cosmeticHacksButton[num].HAlign = 0.245f;
		cosmeticHacksButton[num].BackgroundColor = backgroundColor;
		cosmeticHacksButton[num].BorderColor = borderColor;
		cosmeticHacksButton[2].OnUpdate += delegate
		{
			UpdateButtonVariables(cosmeticHacksButton[2], ref fakeBool);
		};
		cosmeticHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		cosmeticHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Death Particle For Everyone"), 0.35f, large: true);
		cosmeticHacksButton[num].Width.Set(0f, precent2);
		cosmeticHacksButton[num].Height.Set(pixels2, 0f);
		cosmeticHacksButton[num].HAlign = 0.245f;
		cosmeticHacksButton[num].BackgroundColor = backgroundColor;
		cosmeticHacksButton[num].BorderColor = borderColor;
		cosmeticHacksButton[3].OnUpdate += delegate
		{
			UpdateButtonVariables(cosmeticHacksButton[3], ref Mods.deathParticleCosmeticForAll);
		};
		cosmeticHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.deathParticleCosmeticForAll);
		};
		num++;
		cosmeticHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Death Effect: " + GetDeathEffectName(Mods.deathGoreCosmeticId)), 0.35f, large: true);
		cosmeticHacksButton[num].Width.Set(0f, precent2);
		cosmeticHacksButton[num].Height.Set(pixels2, 0f);
		cosmeticHacksButton[num].HAlign = 0.245f;
		cosmeticHacksButton[num].BackgroundColor = backgroundColor;
		cosmeticHacksButton[num].BorderColor = borderColor;
		cosmeticHacksButton[4].OnUpdate += delegate
		{
			UpdateButtonVariables(cosmeticHacksButton[4], ref fakeBool);
		};
		cosmeticHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		cosmeticHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Death Effect For Everyone"), 0.35f, large: true);
		cosmeticHacksButton[num].Width.Set(0f, precent2);
		cosmeticHacksButton[num].Height.Set(pixels2, 0f);
		cosmeticHacksButton[num].HAlign = 0.245f;
		cosmeticHacksButton[num].BackgroundColor = backgroundColor;
		cosmeticHacksButton[num].BorderColor = borderColor;
		cosmeticHacksButton[5].OnUpdate += delegate
		{
			UpdateButtonVariables(cosmeticHacksButton[5], ref Mods.deathGoreCosmeticForAll);
		};
		cosmeticHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.deathGoreCosmeticForAll);
		};
		num++;
		cosmeticHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Lucy Messages: " + GetLucyMessageModeName(Mods.lucyMessagesMode)), 0.35f, large: true);
		cosmeticHacksButton[num].Width.Set(0f, precent2);
		cosmeticHacksButton[num].Height.Set(pixels2, 0f);
		cosmeticHacksButton[num].HAlign = 0.245f;
		cosmeticHacksButton[num].BackgroundColor = backgroundColor;
		cosmeticHacksButton[num].BorderColor = borderColor;
		cosmeticHacksButton[6].OnUpdate += delegate
		{
			UpdateButtonVariables(cosmeticHacksButton[6], ref fakeBool);
		};
		cosmeticHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		cosmeticHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Lucy Message On Death: " + GetLucyDeathMessageModeName(Mods.lucyDeathMessageMode)), 0.35f, large: true);
		cosmeticHacksButton[num].Width.Set(0f, precent2);
		cosmeticHacksButton[num].Height.Set(pixels2, 0f);
		cosmeticHacksButton[num].HAlign = 0.245f;
		cosmeticHacksButton[num].BackgroundColor = backgroundColor;
		cosmeticHacksButton[num].BorderColor = borderColor;
		cosmeticHacksButton[7].OnUpdate += delegate
		{
			UpdateButtonVariables(cosmeticHacksButton[7], ref fakeBool);
		};
		cosmeticHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		cosmeticHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("RGB Outline For Self"), 0.35f, large: true);
		cosmeticHacksButton[num].Width.Set(0f, precent2);
		cosmeticHacksButton[num].Height.Set(pixels2, 0f);
		cosmeticHacksButton[num].HAlign = 0.245f;
		cosmeticHacksButton[num].BackgroundColor = backgroundColor;
		cosmeticHacksButton[num].BorderColor = borderColor;
		cosmeticHacksButton[8].OnUpdate += delegate
		{
			UpdateButtonVariables(cosmeticHacksButton[8], ref Mods.rgbPlayerOutline);
		};
		cosmeticHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.rgbPlayerOutline);
		};
		num++;
		cosmeticHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("RGB NPC Icons"), 0.35f, large: true);
		cosmeticHacksButton[num].Width.Set(0f, precent2);
		cosmeticHacksButton[num].Height.Set(pixels2, 0f);
		cosmeticHacksButton[num].HAlign = 0.245f;
		cosmeticHacksButton[num].BackgroundColor = backgroundColor;
		cosmeticHacksButton[num].BorderColor = borderColor;
		cosmeticHacksButton[9].OnUpdate += delegate
		{
			UpdateButtonVariables(cosmeticHacksButton[9], ref Mods.rgbNpcIcons);
		};
		cosmeticHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.rgbNpcIcons);
		};
		num++;
		cosmeticHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("RGB Boss Icons"), 0.35f, large: true);
		cosmeticHacksButton[num].Width.Set(0f, precent2);
		cosmeticHacksButton[num].Height.Set(pixels2, 0f);
		cosmeticHacksButton[num].MarginBottom = 6f;
		cosmeticHacksButton[num].HAlign = 0.245f;
		cosmeticHacksButton[num].BackgroundColor = backgroundColor;
		cosmeticHacksButton[num].BorderColor = borderColor;
		cosmeticHacksButton[10].OnUpdate += delegate
		{
			UpdateButtonVariables(cosmeticHacksButton[10], ref Mods.rgbBossIcons);
		};
		cosmeticHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.rgbBossIcons);
		};
		num = 0;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("⮞ Miscellaneous"), 0.45f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent);
		miscsHacksButton[num].Height.Set(pixels, 0f);
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[0].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[0], ref Mods.commandPanelMiscsButton);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.commandPanelMiscsButton);
		};
		commandButtonsList.Add(miscsHacksButton[num]);
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Reveal Map"), 0.35f, large: true);
		miscsHacksButton[num].MarginTop = 6f;
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[1].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[1], ref Mods.revealMapCooldown);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.revealMapCooldown);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Wipe Ground Items"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[2].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[2], ref fakeBool);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Track Players"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[3].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[3], ref Mods.playerTracker);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.playerTracker);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Show Dropped Item Owner"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[4].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[4], ref Main.showItemOwner);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Main.showItemOwner);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Force Soundtrack: " + GetForcedSoundtrackName(Mods.forceSoundtrackId)), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[5].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[5], ref fakeBool);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Protect Eternia Crystal From Foreign Attackers"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[6].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[6], ref Mods.crystalKillAura);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.crystalKillAura);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Kill Aura"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[7].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[7], ref Mods.killAura);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.killAura);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Refresh Binds"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[8].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[8], ref fakeBool);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Butcher Npcs"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[9].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[9], ref fakeBool);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Butcher Mobs"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[10].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[10], ref fakeBool);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Auto Butcher"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[11].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[11], ref Mods.autoButcher);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.autoButcher);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Enable Lock-On"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[12].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[12], ref LockOnHelper.ForceUsability);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref LockOnHelper.ForceUsability);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Lock-On Can Target Hostile Players"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[13].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[13], ref LockOnHelper.targetPlayers);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref LockOnHelper.targetPlayers);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Shimmer All Town NPCs"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[14].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[14], ref fakeBool);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Shimmer All Enemies"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[15].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[15], ref fakeBool);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Open Locked Chests"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[16].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[16], ref Mods.openLockedChests);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.openLockedChests);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Super Actuation Rod"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[17].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[17], ref Mods.actuatorRodPlus);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.actuatorRodPlus);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Free Cam"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[18].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[18], ref Mods.freeCam);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.freeCam);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Show ESP And Vitals"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[19].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[19], ref Mods.playerVitalStatsUI);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.playerVitalStatsUI);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Free Shops"), 0.35f, large: true);
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[20].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[20], ref Mods.freeShops);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.freeShops);
		};
		num++;
		miscsHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Hide UI Text"), 0.35f, large: true);
		miscsHacksButton[num].MarginBottom = 6f;
		miscsHacksButton[num].Width.Set(0f, precent2);
		miscsHacksButton[num].Height.Set(pixels2, 0f);
		miscsHacksButton[num].HAlign = 0.245f;
		miscsHacksButton[num].BackgroundColor = backgroundColor;
		miscsHacksButton[num].BorderColor = borderColor;
		miscsHacksButton[21].OnUpdate += delegate
		{
			UpdateButtonVariables(miscsHacksButton[21], ref Mods.showNetplayText);
		};
		miscsHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.showNetplayText);
		};
		num = 0;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("⮞ Biome Overrides"), 0.45f, large: true);
		zoneHacksButton[num].Width.Set(0f, precent);
		zoneHacksButton[num].Height.Set(pixels, 0f);
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[0].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[0], ref ModsVars.commandPanelZonesButton);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.commandPanelZonesButton);
		};
		commandButtonsList.Add(zoneHacksButton[num]);
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Overworld Biomes"), 0.35f, large: true);
		zoneHacksButton[num].MarginTop = 6f;
		zoneHacksButton[num].Width.Set(0f, precent2);
		zoneHacksButton[num].Height.Set(pixels2, 0f);
		zoneHacksButton[num].HAlign = 0.245f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[1].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[1], ref ModsVars.zoneHacksOB);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneHacksOB);
		};
		_ = Main.player[Main.myPlayer];
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Dungeon"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[2].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[2], ref ModsVars.zoneDungeon);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneDungeon);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Corrupt"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[3].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[3], ref ModsVars.zoneCorrupt);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneCorrupt);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Crimson"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[4].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[4], ref ModsVars.zoneCrimson);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneCrimson);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Hallow"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[5].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[5], ref ModsVars.zoneHallow);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneHallow);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Jungle"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[6].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[6], ref ModsVars.zoneJungle);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneJungle);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Snow"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[7].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[7], ref ModsVars.zoneSnow);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneSnow);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Desert"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[8].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[8], ref ModsVars.zoneDesert);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneDesert);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Underground Biomes"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, precent2);
		zoneHacksButton[num].Height.Set(pixels2, 0f);
		zoneHacksButton[num].HAlign = 0.245f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[9].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[9], ref ModsVars.zoneHacksUB);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneHacksUB);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Gem Cave"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[10].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[10], ref ModsVars.zoneGemCave);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneGemCave);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Glowshroom"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[11].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[11], ref ModsVars.zoneGlowshroom);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneGlowshroom);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Granite"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[12].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[12], ref ModsVars.zoneGranite);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneGranite);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Hive"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[13].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[13], ref ModsVars.zoneHive);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneHive);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Lihzahrd Temple"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[14].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[14], ref ModsVars.zoneLihzhardTemple);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneLihzhardTemple);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Marble"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[15].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[15], ref ModsVars.zoneMarble);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneMarble);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Underground Desert"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[16].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[16], ref ModsVars.zoneUndergroundDesert);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneUndergroundDesert);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Shimmer"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[17].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[17], ref ModsVars.zoneShimmer);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneShimmer);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Miscs"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, precent2);
		zoneHacksButton[num].Height.Set(pixels2, 0f);
		zoneHacksButton[num].HAlign = 0.245f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[18].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[18], ref ModsVars.zoneHacksMB);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneHacksMB);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Water Candle"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[19].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[19], ref ModsVars.zoneWaterCandle);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneWaterCandle);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Peace Candle"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[20].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[20], ref ModsVars.zonePeaceCandle);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zonePeaceCandle);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Shadow Candle"), 0.35f, large: true);
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[21].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[21], ref ModsVars.zoneShadowCandle);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneShadowCandle);
		};
		num++;
		zoneHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Meteor"), 0.35f, large: true);
		zoneHacksButton[num].MarginBottom = 6f;
		zoneHacksButton[num].Width.Set(0f, 0.75f);
		zoneHacksButton[num].Height.Set(25f, 0f);
		zoneHacksButton[num].HAlign = 0.39f;
		zoneHacksButton[num].BackgroundColor = backgroundColor;
		zoneHacksButton[num].BorderColor = borderColor;
		zoneHacksButton[22].OnUpdate += delegate
		{
			UpdateButtonVariables(zoneHacksButton[22], ref ModsVars.zoneMeteor);
		};
		zoneHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref ModsVars.zoneMeteor);
		};
		num = 0;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("⮞ World Edit"), 0.45f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, precent);
		builderMenuHacksButton[num].Height.Set(pixels, 0f);
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[0].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[0], ref Mods.commandPanelBuildMenuButton);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.commandPanelBuildMenuButton);
		};
		commandButtonsList.Add(builderMenuHacksButton[num]);
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText(Mods.tilePacketMaxMode ? "Packet Maximum" : "Packet 100/ms"), 0.35f, large: true);
		builderMenuHacksButton[num].MarginTop = 2f;
		builderMenuHacksButton[num].Width.Set(0f, 0.75f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.39f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[1].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[1], ref Mods.tilePacketMaxMode);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.tilePacketMaxMode);
		};
		CreateSpecialThreeButtons(builderMenuHacksButton[num], Mods.tilePacketRate.ToString(), 0.26f);
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText(Main.mapFullscreen ? "Placement Mode: Player" : "Placement Mode: Map"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.75f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.39f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[2].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[2], ref fakeBool);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Copy Tiles"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, precent2);
		builderMenuHacksButton[num].Height.Set(pixels2, 0f);
		builderMenuHacksButton[num].HAlign = 0.245f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[3].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[3], ref Mods.copyMode);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.copyMode);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Save Schematic"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[4].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[4], ref fakeBool);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Clear Selection"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[5].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[5], ref fakeBool);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Paste Tiles"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, precent2);
		builderMenuHacksButton[num].Height.Set(pixels2, 0f);
		builderMenuHacksButton[num].HAlign = 0.245f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[6].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[6], ref Mods.pasteMode);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.pasteMode);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Load Schematic"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[7].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[7], ref fakeBool);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Blend Tiles"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[8].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[8], ref Mods.blendAirTileMode);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.blendAirTileMode);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Attempt Paste In Tshock"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[9].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[9], ref Mods.pasteInTShock);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.pasteInTShock);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Cancel Paste"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[10].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[10], ref Mods.cancelPaste);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.cancelPaste);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("All Brushes"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, precent2);
		builderMenuHacksButton[num].Height.Set(pixels2, 0f);
		builderMenuHacksButton[num].HAlign = 0.245f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[11].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[11], ref Mods.brushMode);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.brushMode);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Brush Shape: " + ((!Mods.brushCircle) ? "Square" : "Circle")), 0.35f, large: true);
		builderMenuHacksButton[num].MarginBottom = 2f;
		builderMenuHacksButton[num].Width.Set(0f, 0.75f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.39f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[12].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[12], ref Mods.brushCircle);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.brushCircle);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Tile Brush"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[13].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[13], ref Mods.tileBrush);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.tileBrush);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Erase Brush"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[14].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[14], ref Mods.eraseBrush);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.eraseBrush);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Wall Brush"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[15].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[15], ref Mods.wallBrush);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.wallBrush);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Liquid Brush"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[16].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[16], ref Mods.liquidBrush);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.liquidBrush);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Selection Mode"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, precent2);
		builderMenuHacksButton[num].Height.Set(pixels2, 0f);
		builderMenuHacksButton[num].HAlign = 0.245f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[17].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[17], ref Mods.selectionMode);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.selectionMode);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Tile Packet Type: " + ((!Mods.selectionTileSyncingMode) ? "Singular" : "Chunk")), 0.35f, large: true);
		builderMenuHacksButton[num].MarginBottom = 2f;
		builderMenuHacksButton[num].Width.Set(0f, 0.75f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.39f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[18].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[18], ref Mods.selectionUseTileSync);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.selectionUseTileSync);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Fill Selection (All)"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[19].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[19], ref fakeBool);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Delete Selection"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[20].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[20], ref fakeBool);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Delete Selection (Tile)"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[21].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[21], ref fakeBool);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Delete Selection (Wall)"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[22].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[22], ref fakeBool);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Delete Selection (Liquid)"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[23].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[23], ref fakeBool);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Cancel Action"), 0.35f, large: true);
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[24].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[24], ref Mods.cancelAction);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.cancelAction);
		};
		num++;
		builderMenuHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Clear Selection"), 0.35f, large: true);
		builderMenuHacksButton[num].MarginBottom = 6f;
		builderMenuHacksButton[num].Width.Set(0f, 0.855f);
		builderMenuHacksButton[num].Height.Set(25f, 0f);
		builderMenuHacksButton[num].HAlign = 0.325f;
		builderMenuHacksButton[num].BackgroundColor = backgroundColor;
		builderMenuHacksButton[num].BorderColor = borderColor;
		builderMenuHacksButton[25].OnUpdate += delegate
		{
			UpdateButtonVariables(builderMenuHacksButton[25], ref fakeBool);
		};
		builderMenuHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		num = 0;
		Item item = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];
		selectedItemHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("⮞ Selected Item"), 0.45f, large: true);
		selectedItemHacksButton[num].Width.Set(0f, precent);
		selectedItemHacksButton[num].Height.Set(pixels, 0f);
		selectedItemHacksButton[num].BackgroundColor = backgroundColor;
		selectedItemHacksButton[num].BorderColor = borderColor;
		selectedItemHacksButton[0].OnUpdate += delegate
		{
			UpdateButtonVariables(selectedItemHacksButton[0], ref Mods.commandPanelSelectedItemButton);
		};
		selectedItemHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref Mods.commandPanelSelectedItemButton);
		};
		commandButtonsList.Add(selectedItemHacksButton[num]);
		num++;
		selectedItemHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Name: " + item.Name), 0.35f, large: true);
		selectedItemHacksButton[num].MarginTop = 6f;
		selectedItemHacksButton[num].Width.Set(0f, precent2);
		selectedItemHacksButton[num].Height.Set(pixels2, 0f);
		selectedItemHacksButton[num].HAlign = 0.245f;
		selectedItemHacksButton[num].BackgroundColor = backgroundColor;
		selectedItemHacksButton[num].BorderColor = borderColor;
		selectedItemHacksButton[1].OnUpdate += delegate
		{
			UpdateButtonVariables(selectedItemHacksButton[1], ref fakeBool);
		};
		num++;
		selectedItemHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Damage"), 0.35f, large: true);
		selectedItemHacksButton[num].Width.Set(0f, precent2);
		selectedItemHacksButton[num].Height.Set(pixels2, 0f);
		selectedItemHacksButton[num].HAlign = 0.245f;
		selectedItemHacksButton[num].BackgroundColor = backgroundColor;
		selectedItemHacksButton[num].BorderColor = borderColor;
		CreateSpecialThreeButtons(selectedItemHacksButton[num], item.damage.ToString(), 0.26f);
		num++;
		selectedItemHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Usetime"), 0.35f, large: true);
		selectedItemHacksButton[num].Width.Set(0f, precent2);
		selectedItemHacksButton[num].Height.Set(pixels2, 0f);
		selectedItemHacksButton[num].HAlign = 0.245f;
		selectedItemHacksButton[num].BackgroundColor = backgroundColor;
		selectedItemHacksButton[num].BorderColor = borderColor;
		CreateSpecialThreeButtons(selectedItemHacksButton[num], item.useTime.ToString(), 0.26f);
		num++;
		selectedItemHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Animation Speed"), 0.35f, large: true);
		selectedItemHacksButton[num].Width.Set(0f, precent2);
		selectedItemHacksButton[num].Height.Set(pixels2, 0f);
		selectedItemHacksButton[num].HAlign = 0.245f;
		selectedItemHacksButton[num].BackgroundColor = backgroundColor;
		selectedItemHacksButton[num].BorderColor = borderColor;
		CreateSpecialThreeButtons(selectedItemHacksButton[num], item.useAnimation.ToString(), 0.26f);
		num++;
		selectedItemHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Projectile"), 0.35f, large: true);
		selectedItemHacksButton[num].Width.Set(0f, precent2);
		selectedItemHacksButton[num].Height.Set(pixels2, 0f);
		selectedItemHacksButton[num].HAlign = 0.245f;
		selectedItemHacksButton[num].BackgroundColor = backgroundColor;
		selectedItemHacksButton[num].BorderColor = borderColor;
		CreateSpecialThreeButtons(selectedItemHacksButton[num], item.shoot.ToString(), 0.26f);
		num++;
		selectedItemHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Projectile Speed"), 0.35f, large: true);
		selectedItemHacksButton[num].Width.Set(0f, precent2);
		selectedItemHacksButton[num].Height.Set(pixels2, 0f);
		selectedItemHacksButton[num].HAlign = 0.245f;
		selectedItemHacksButton[num].BackgroundColor = backgroundColor;
		selectedItemHacksButton[num].BorderColor = borderColor;
		CreateSpecialThreeButtons(selectedItemHacksButton[num], item.shootSpeed.ToString(), 0.26f);
		num++;
		selectedItemHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Pick Power"), 0.35f, large: true);
		selectedItemHacksButton[num].Width.Set(0f, precent2);
		selectedItemHacksButton[num].Height.Set(pixels2, 0f);
		selectedItemHacksButton[num].HAlign = 0.245f;
		selectedItemHacksButton[num].BackgroundColor = backgroundColor;
		selectedItemHacksButton[num].BorderColor = borderColor;
		CreateSpecialThreeButtons(selectedItemHacksButton[num], item.pick.ToString(), 0.26f);
		num++;
		selectedItemHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Axe Power"), 0.35f, large: true);
		selectedItemHacksButton[num].Width.Set(0f, precent2);
		selectedItemHacksButton[num].Height.Set(pixels2, 0f);
		selectedItemHacksButton[num].HAlign = 0.245f;
		selectedItemHacksButton[num].BackgroundColor = backgroundColor;
		selectedItemHacksButton[num].BorderColor = borderColor;
		CreateSpecialThreeButtons(selectedItemHacksButton[num], item.axe.ToString(), 0.26f);
		num++;
		selectedItemHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Hammer Power"), 0.35f, large: true);
		selectedItemHacksButton[num].Width.Set(0f, precent2);
		selectedItemHacksButton[num].Height.Set(pixels2, 0f);
		selectedItemHacksButton[num].HAlign = 0.245f;
		selectedItemHacksButton[num].BackgroundColor = backgroundColor;
		selectedItemHacksButton[num].BorderColor = borderColor;
		CreateSpecialThreeButtons(selectedItemHacksButton[num], item.hammer.ToString(), 0.26f);
		num++;
		selectedItemHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Stack"), 0.35f, large: true);
		selectedItemHacksButton[num].Width.Set(0f, precent2);
		selectedItemHacksButton[num].Height.Set(pixels2, 0f);
		selectedItemHacksButton[num].HAlign = 0.245f;
		selectedItemHacksButton[num].BackgroundColor = backgroundColor;
		selectedItemHacksButton[num].BorderColor = borderColor;
		CreateSpecialThreeButtons(selectedItemHacksButton[num], item.stack.ToString(), 0.26f);
		num++;
		selectedItemHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Modifier: " + GetPrefixName(item.prefix)), 0.35f, large: true);
		selectedItemHacksButton[num].Width.Set(0f, precent2);
		selectedItemHacksButton[num].Height.Set(pixels2, 0f);
		selectedItemHacksButton[num].HAlign = 0.245f;
		selectedItemHacksButton[num].BackgroundColor = backgroundColor;
		selectedItemHacksButton[num].BorderColor = borderColor;
		selectedItemHacksButton[11].OnUpdate += delegate
		{
			UpdateButtonVariables(selectedItemHacksButton[11], ref fakeBool);
		};
		selectedItemHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		selectedItemHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Auto-Reuse: "), 0.35f, large: true);
		selectedItemHacksButton[num].Width.Set(0f, precent2);
		selectedItemHacksButton[num].Height.Set(pixels2, 0f);
		selectedItemHacksButton[num].HAlign = 0.245f;
		selectedItemHacksButton[num].BackgroundColor = backgroundColor;
		selectedItemHacksButton[num].BorderColor = borderColor;
		selectedItemHacksButton[12].OnUpdate += delegate
		{
			UpdateButtonVariables(selectedItemHacksButton[12], ref fakeBool);
		};
		selectedItemHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		selectedItemHacksButton[num] = new UITextPanel<LocalizedText>(Language.GetText("Restore Item Variables"), 0.35f, large: true);
		selectedItemHacksButton[num].Width.Set(0f, precent2);
		selectedItemHacksButton[num].Height.Set(pixels2, 0f);
		selectedItemHacksButton[num].HAlign = 0.245f;
		selectedItemHacksButton[num].BackgroundColor = backgroundColor;
		selectedItemHacksButton[num].BorderColor = borderColor;
		selectedItemHacksButton[13].OnUpdate += delegate
		{
			UpdateButtonVariables(selectedItemHacksButton[13], ref fakeBool);
		};
		selectedItemHacksButton[num].OnLeftClick += delegate(UIMouseEvent evt, UIElement element)
		{
			UpdateButtonOnClick(evt, element, ref fakeBool);
		};
		num++;
		Append(commandPanelBackground);
	}

	public override void Update(GameTime gameTime)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		base.Update(gameTime);
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)Main.screenWidth / 2f - (float)((int)Mods.panelWidth / 2) + 15f, (float)(Main.screenHeight / 2 - 40));
		commandPanelBackground.Left.Set(vector.X, 0f);
		commandPanelBackground.Top.Set(vector.Y - Mods.panelHeight / 2f + 75f, 0f);
		if (!Mods.tilePacketMaxMode && Mods.tilePacketRate <= 0)
		{
			Mods.tilePacketRate = 1;
		}
		if (PlayerPressedHotbarKeyV2())
		{
			sharedPrevSelectedItem = Main.LocalPlayer.selectedItem;
		}
		if (Mods.commandPannel && Main.LocalPlayer.mouseInterface)
		{
			Main.LocalPlayer.selectedItem = sharedPrevSelectedItem;
		}
		if (!Mods.tileBrush)
		{
			builderMenuHacksButton[13].RemoveAllChildren();
		}
		if (!Mods.eraseBrush)
		{
			builderMenuHacksButton[14].RemoveAllChildren();
		}
		if (!Mods.wallBrush)
		{
			builderMenuHacksButton[15].RemoveAllChildren();
		}
		if (!Mods.liquidBrush)
		{
			builderMenuHacksButton[16].RemoveAllChildren();
		}
		if (!Mods.killAura)
		{
			miscsHacksButton[7].RemoveAllChildren();
		}
		if (specialButtonsList == null || specialButtonsList.Count < 3)
		{
			return;
		}
		for (int i = 0; i < specialButtonsList.Count; i++)
		{
			string text = specialButtonsList[i].Text;
			if (text == "+" || text == "-")
			{
				continue;
			}
			Item item = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];
			if (specialButtonsList[i].Parent != null && (((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Packet Maximum" || ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Packet 100/ms"))
			{
				specialButtonsList[i].SetText(Language.GetText(Mods.tilePacketRate.ToString()));
			}
			if (Mods.killAura && specialButtonsList[i].Parent != null && ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Kill Aura")
			{
				specialButtonsList[i].SetText(Language.GetText(Mods.killAuraSize.ToString()));
			}
			if (Mods.brushMode)
			{
				if (Mods.tileBrush && specialButtonsList[i].Parent != null && ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Tile Brush")
				{
					specialButtonsList[i].SetText(Language.GetText(Mods.tileBrushSize.ToString()));
				}
				if (Mods.eraseBrush && specialButtonsList[i].Parent != null && ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Erase Brush")
				{
					specialButtonsList[i].SetText(Language.GetText(Mods.eraseBrushSize.ToString()));
				}
				if (Mods.wallBrush && specialButtonsList[i].Parent != null && ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Wall Brush")
				{
					specialButtonsList[i].SetText(Language.GetText(Mods.wallBrushSize.ToString()));
				}
				if (Mods.liquidBrush && specialButtonsList[i].Parent != null && ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Liquid Brush")
				{
					specialButtonsList[i].SetText(Language.GetText(Mods.liquidBrushSize.ToString()));
				}
			}
			if (!Mods.commandPanelSelectedItemButton || item.type <= 0)
			{
				continue;
			}
			if (specialButtonsList[i].Parent != null && ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Damage")
			{
				specialButtonsList[i].SetText(Language.GetText(item.damage.ToString()));
			}
			if (specialButtonsList[i].Parent != null && ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Usetime")
			{
				specialButtonsList[i].SetText(Language.GetText(item.useTime.ToString()));
			}
			if (specialButtonsList[i].Parent != null && ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Animation Speed")
			{
				specialButtonsList[i].SetText(Language.GetText(item.useAnimation.ToString()));
			}
			if (specialButtonsList[i].Parent != null && ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Projectile")
			{
				specialButtonsList[i].SetText(Language.GetText(item.shoot.ToString()));
				if (specialButtonsList[i].IsMouseHovering)
				{
					Main.instance.MouseText(ProjectileID.Search.GetName(item.shoot), 0, 0);
				}
			}
			if (specialButtonsList[i].Parent != null && ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Projectile Speed")
			{
				specialButtonsList[i].SetText(Language.GetText(item.shootSpeed.ToString()));
			}
			if (specialButtonsList[i].Parent != null && ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Pick Power")
			{
				specialButtonsList[i].SetText(Language.GetText(item.pick.ToString()));
			}
			if (specialButtonsList[i].Parent != null && ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Axe Power")
			{
				specialButtonsList[i].SetText(Language.GetText(item.axe.ToString()));
			}
			if (specialButtonsList[i].Parent != null && ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Hammer Power")
			{
				specialButtonsList[i].SetText(Language.GetText(item.hammer.ToString()));
			}
			if (specialButtonsList[i].Parent != null && ((UITextPanel<LocalizedText>)specialButtonsList[i].Parent).Text == "Stack")
			{
				specialButtonsList[i].SetText(Language.GetText(item.stack.ToString()));
			}
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
	}

	static ModsUICommandPanelButtons()
	{
		commandButtonsList = new UIList();
		uiStateCommandPanelButtons = new ModsUICommandPanelButtons();
		commandPanelBackground = new UIElement();
		playerHacksButton = new UITextPanel<LocalizedText>[15];
		travelHacksButton = new UITextPanel<LocalizedText>[8];
		miscsHacksButton = new UITextPanel<LocalizedText>[25];
		builderMenuHacksButton = new UITextPanel<LocalizedText>[40];
		displayBrushSize = new UITextPanel<LocalizedText>[40];
		specialButtonsList = new List<UITextPanel<LocalizedText>>();
		selectedItemHacksButton = new UITextPanel<LocalizedText>[20];
		zoneHacksButton = new UITextPanel<LocalizedText>[25];
		cosmeticHacksButton = new UITextPanel<LocalizedText>[16];
	}

	private void ManualIfnoSortingMethod(List<UIElement> list)
	{
	}

	private void UpdateButtonList()
	{
		specialButtonsList.Clear();
		commandButtonsList.Clear();
		ReInitializeSpecialButtonsForItemTab();
		commandButtonsList.Add(playerHacksButton[0]);
		if (Mods.commandPanelPlayerButton)
		{
			for (int i = 1; i < 15; i++)
			{
				commandButtonsList.Add(playerHacksButton[i]);
			}
		}
		commandButtonsList.Add(travelHacksButton[0]);
		if (Mods.commandPanelTravelButton)
		{
			for (int num = 1; num < 5; num++)
			{
				commandButtonsList.Add(travelHacksButton[num]);
			}
		}
		commandButtonsList.Add(cosmeticHacksButton[0]);
		if (ModsVars.cosmeticHacksButton)
		{
			for (int j = 1; j < 11; j++)
			{
				commandButtonsList.Add(cosmeticHacksButton[j]);
			}
		}
		commandButtonsList.Add(miscsHacksButton[0]);
		if (Mods.commandPanelMiscsButton)
		{
			for (int k = 1; k <= 21; k++)
			{
				commandButtonsList.Add(miscsHacksButton[k]);
				if (k == 7 && Mods.killAura)
				{
					CreateSpecialThreeButtons(miscsHacksButton[k], Mods.killAuraSize.ToString(), 0.26f);
				}
			}
		}
		commandButtonsList.Add(zoneHacksButton[0]);
		if (ModsVars.commandPanelZonesButton)
		{
			commandButtonsList.Add(zoneHacksButton[1]);
			if (ModsVars.zoneHacksOB)
			{
				for (int l = 2; l < 9; l++)
				{
					commandButtonsList.Add(zoneHacksButton[l]);
				}
			}
			commandButtonsList.Add(zoneHacksButton[9]);
			if (ModsVars.zoneHacksUB)
			{
				for (int m = 10; m < 18; m++)
				{
					commandButtonsList.Add(zoneHacksButton[m]);
				}
			}
			commandButtonsList.Add(zoneHacksButton[18]);
			zoneHacksButton[18].MarginBottom = (ModsVars.zoneHacksMB ? 0f : 6f);
			if (ModsVars.zoneHacksMB)
			{
				for (int n = 19; n < 23; n++)
				{
					commandButtonsList.Add(zoneHacksButton[n]);
				}
			}
		}
		commandButtonsList.Add(builderMenuHacksButton[0]);
		if (Mods.commandPanelBuildMenuButton)
		{
			for (int num2 = 1; num2 <= 25; num2++)
			{
				switch (num2)
				{
				case 18:
				case 19:
				case 20:
				case 21:
				case 22:
				case 23:
				case 24:
				case 25:
					if (Mods.selectionMode)
					{
						commandButtonsList.Add(builderMenuHacksButton[num2]);
					}
					break;
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
					if (Mods.brushMode)
					{
						commandButtonsList.Add(builderMenuHacksButton[num2]);
						if (num2 == 13 && Mods.tileBrush)
						{
							CreateSpecialThreeButtons(builderMenuHacksButton[num2], Mods.tileBrushSize.ToString(), 0.26f);
						}
						if (num2 == 14 && Mods.eraseBrush)
						{
							CreateSpecialThreeButtons(builderMenuHacksButton[num2], Mods.eraseBrushSize.ToString(), 0.26f);
						}
						if (num2 == 15 && Mods.wallBrush)
						{
							CreateSpecialThreeButtons(builderMenuHacksButton[num2], Mods.wallBrushSize.ToString(), 0.26f);
						}
						if (num2 == 16 && Mods.liquidBrush)
						{
							CreateSpecialThreeButtons(builderMenuHacksButton[num2], Mods.liquidBrushSize.ToString(), 0.26f);
						}
					}
					break;
				case 7:
				case 8:
				case 9:
				case 10:
					if (Mods.pasteMode)
					{
						commandButtonsList.Add(builderMenuHacksButton[num2]);
					}
					break;
				case 4:
				case 5:
					if (Mods.copyMode)
					{
						commandButtonsList.Add(builderMenuHacksButton[num2]);
					}
					break;
				case 1:
					builderMenuHacksButton[num2].RemoveAllChildren();
					commandButtonsList.Add(builderMenuHacksButton[num2]);
					CreateSpecialThreeButtons(builderMenuHacksButton[num2], Mods.tilePacketRate.ToString(), 0.26f);
					break;
				default:
					commandButtonsList.Add(builderMenuHacksButton[num2]);
					break;
				}
				builderMenuHacksButton[17].MarginBottom = (Mods.selectionMode ? 0f : 6f);
			}
		}
		commandButtonsList.Add(selectedItemHacksButton[0]);
		if (Mods.commandPanelSelectedItemButton)
		{
			for (int num3 = 1; num3 < 14; num3++)
			{
				commandButtonsList.Add(selectedItemHacksButton[num3]);
			}
		}
	}

	private void UpdateButtonVariables(UITextPanel<LocalizedText> theButtonInQuestion, ref bool inputBool)
	{
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
		bool flag = specialButtonsList != null && specialButtonsList.Count >= 3;
		Item item = Main.LocalPlayer.inventory[sharedPrevSelectedItem];
		if (theButtonInQuestion.Text.Contains("Death Particle: ") && ModsVars.cosmeticHacksButton)
		{
			theButtonInQuestion.SetText(Language.GetText("Death Particle: " + GetDeathParticleName(Mods.deathParticleCosmeticId)));
		}
		if (theButtonInQuestion.Text.Contains("Death Effect: ") && ModsVars.cosmeticHacksButton)
		{
			theButtonInQuestion.SetText(Language.GetText("Death Effect: " + GetDeathEffectName(Mods.deathGoreCosmeticId)));
		}
		if (theButtonInQuestion.Text.Contains("Lucy Messages: ") && ModsVars.cosmeticHacksButton)
		{
			theButtonInQuestion.SetText(Language.GetText("Lucy Messages: " + GetLucyMessageModeName(Mods.lucyMessagesMode)));
		}
		if (theButtonInQuestion.Text.Contains("Lucy Message On Death: ") && ModsVars.cosmeticHacksButton)
		{
			theButtonInQuestion.SetText(Language.GetText("Lucy Message On Death: " + GetLucyDeathMessageModeName(Mods.lucyDeathMessageMode)));
		}
		if (theButtonInQuestion.Text.Contains("Force Soundtrack: ") && Mods.commandPanelMiscsButton)
		{
			theButtonInQuestion.SetText(Language.GetText("Force Soundtrack: " + GetForcedSoundtrackName(Mods.forceSoundtrackId)));
		}
		if (theButtonInQuestion.Text.Contains("Name: ") && Mods.commandPanelSelectedItemButton)
		{
			theButtonInQuestion.SetText(Language.GetText("Name: " + item.Name));
			return;
		}
		if (theButtonInQuestion.Text.Contains("Modifier: ") && Mods.commandPanelSelectedItemButton)
		{
			theButtonInQuestion.SetText(Language.GetText("Modifier: " + GetPrefixName(item.prefix)));
		}
		else if (theButtonInQuestion.Text.Contains("Auto-Reuse: ") && Mods.commandPanelSelectedItemButton)
		{
			theButtonInQuestion.SetText(Language.GetText("Auto-Reuse: " + (item.autoReuse ? "True" : "False")));
		}
		else if (theButtonInQuestion.Text.Contains("Placement Mode: ") && Mods.commandPanelBuildMenuButton)
		{
			theButtonInQuestion.SetText(Language.GetText("Placement Mode: " + ((!Main.mapFullscreen) ? "Player" : "Map")));
			_ = theButtonInQuestion.IsMouseHovering;
			return;
		}
		if (Mods.commandPanelBuildMenuButton & flag)
		{
			for (int i = 0; i < specialButtonsList.Count; i++)
			{
				if (Mods.brushMode && specialButtonsList[i].IsMouseHovering && (theButtonInQuestion.Text == "Tile Brush" || theButtonInQuestion.Text == "Erase Brush" || theButtonInQuestion.Text == "Liquid Brush"))
				{
					theButtonInQuestion.BackgroundColor = (inputBool ? (new Color(9, 46, 66) * 0.75f) : (new Color(9, 46, 89) * 0.75f));
					theButtonInQuestion.BorderColor = new Color(0, 0, 0) * 0.75f;
					return;
				}
				if (specialButtonsList[i].IsMouseHovering && (theButtonInQuestion.Text == "Packet Maximum" || theButtonInQuestion.Text == "Packet 100/ms"))
				{
					theButtonInQuestion.BackgroundColor = (inputBool ? (new Color(9, 46, 66) * 0.75f) : (new Color(9, 46, 89) * 0.75f));
					theButtonInQuestion.BorderColor = new Color(0, 0, 0) * 0.75f;
					return;
				}
			}
		}
		if ((theButtonInQuestion.Text.Contains("Reveal Map") || theButtonInQuestion.Text.Contains("Chunks Left Unloaded: ")) && Mods.revealMapCooldown)
		{
			theButtonInQuestion.SetText(Language.GetText("Chunks Left Unloaded: " + Mods.Instance.DetermineUnloadedSections));
			theButtonInQuestion.BackgroundColor = (inputBool ? (new Color(9, 46, 66) * 0.75f) : (new Color(9, 46, 89) * 0.75f));
			theButtonInQuestion.BorderColor = new Color(0, 0, 0) * 0.75f;
			return;
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Drop Entire Inventory")
		{
			Main.instance.MouseText("Drops your entire inventory without deleting your items.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Toggle All Melee Accessories")
		{
			Main.instance.MouseText(ModsVars.playerHacksButtonAllMelee ? "Turn off all melee accessories (excluding Moon Charm)." : "Turn on all melee accessories (excluding Moon Charm).", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Toggle All Ranged Accessories")
		{
			Main.instance.MouseText(ModsVars.playerHacksButtonAllRanged ? "Turn off all ranged accessories." : "Turn on all ranged accessories.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Toggle All Magic Accessories")
		{
			Main.instance.MouseText(ModsVars.playerHacksButtonAllMagic ? "Turn off all magic accessories." : "Turn on all magic accessories.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Toggle All Summoner Accessories")
		{
			Main.instance.MouseText(ModsVars.playerHacksButtonAllSummoner ? "Turn off all summoner accessories." : "Turn on all summoner accessories.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Hide Werewolf")
		{
			Main.instance.MouseText("Hides werewolf form, useful if Moon Charm is enabled and you don't want to appear as one.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Werewolf Buff During Day")
		{
			Main.instance.MouseText("Forces werewolf buff during the day, so you get the effects at all times.\nOnly effective if you're wearing any Moon Charm accessory or forcing the Moon Charm.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Shimmer Walking Particles")
		{
			Main.instance.MouseText("Emit shimmer particles when you walk as if you're walking on an Aetherium Block.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text.Contains("Death Particle: "))
		{
			Main.instance.MouseText("Emit a particle on death. Visible and audible to others.\nThese are relatively short and may make sounds. Some are sounds only.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Death Particle For Everyone")
		{
			Main.instance.MouseText("Everyone will emit a particle on death.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text.Contains("Death Effect: "))
		{
			Main.instance.MouseText("Emit effects on death. Visible to others.\nThese are silent, but typically last for longer than particles.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Death Effect For Everyone")
		{
			Main.instance.MouseText("Everyone will emit an effect on death.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Lucy Messages: No Override")
		{
			Main.instance.MouseText("Cycle between forcing Lucy idle messages on, off, or not being overidden.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Lucy Messages: Forced Off")
		{
			Main.instance.MouseText("Lucy will remain quiet when in the inventory.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Lucy Messages: Forced On")
		{
			Main.instance.MouseText("Lucy will manifest herself and pretend she's in your inventory to annoy you.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Lucy Messages: Forced On (Everyone)")
		{
			Main.instance.MouseText("Everyone will randomly hear Lucy, aka free schizophrenia.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Lucy Message On Death")
		{
			Main.instance.MouseText("Lucy will speak when you die.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Lucy Message On Death: Disabled")
		{
			Main.instance.MouseText("Lucy will not speak when you die.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Lucy Message On Death: Self")
		{
			Main.instance.MouseText("Lucy will speak when you die.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Lucy Message On Death: Others")
		{
			Main.instance.MouseText("Lucy will haunt others and speak when they die.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Lucy Message On Death: Everyone")
		{
			Main.instance.MouseText("Lucy will haunt everyone who dies.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Protect Eternia Crystal From Foreign Attackers")
		{
			Main.instance.MouseText("A kill aura for the Eternia Crystal.\nInstantly kills any non-DD2 enemy that is nearby.", 0, 0);
		}
		if (theButtonInQuestion.Text.Contains("Chunks Left Unloaded: ") && !Mods.revealMapCooldown)
		{
			theButtonInQuestion.SetText(Language.GetText("Reveal Map"));
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text.Contains("Force Soundtrack: "))
		{
			Main.instance.MouseText("Force a specified soundtrack at all times. No talking to the Party Girl. Persists between worlds.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Attempt Paste In Tshock")
		{
			Main.instance.MouseText("Paste mode is designed to work seamlessly with tile syncing only.\nIf you're on a server where tile syncing is already enabled, do not enable this incomplete feature.\nThis feature is incomplete and it will remain so.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text.Contains("Tile Packet Type: Chunk"))
		{
			Main.instance.MouseText("Use this if server allows tile syncing. Far more superior.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text.Contains("Tile Packet Type: Singular"))
		{
			Main.instance.MouseText("Use this if server disabled tile syncing. Very inferior.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Packet 100/ms")
		{
			Main.instance.MouseText("After " + Mods.tilePacketRate + " packets have been sent a 100/ms delay will be invoked.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.Text == "Packet Maximum")
		{
			Main.instance.MouseText("After " + Mods.tilePacketRate + " packets have filled the socket a 500/ms delay will be invoked.", 0, 0);
		}
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.BackgroundColor != (inputBool ? (new Color(9, 46, 75) * 0.75f) : (new Color(9, 46, 110) * 0.75f)))
		{
			SoundEngine.PlaySound(12, 0);
			theButtonInQuestion.BackgroundColor = (inputBool ? (new Color(9, 46, 75) * 0.75f) : (new Color(9, 46, 110) * 0.75f));
			theButtonInQuestion.BorderColor = Main.OurFavoriteColor;
			return;
		}
		if (!theButtonInQuestion.IsMouseHovering && theButtonInQuestion.BackgroundColor != (inputBool ? (new Color(9, 46, 66) * 0.75f) : (new Color(9, 46, 89) * 0.75f)))
		{
			theButtonInQuestion.BackgroundColor = (inputBool ? (new Color(9, 46, 66) * 0.75f) : (new Color(9, 46, 89) * 0.75f));
			theButtonInQuestion.BorderColor = new Color(0, 0, 0) * 0.75f;
		}
		HandleSpecialButtonClicking(theButtonInQuestion);
	}

	private void UpdateButtonVariables(UITextPanel<LocalizedText> theButtonInQuestion, ICheat cheat)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		if (specialButtonsList != null)
		{
			_ = specialButtonsList.Count >= 3;
		}
		else
			_ = 0;
		_ = Main.LocalPlayer.inventory[sharedPrevSelectedItem];
		if (theButtonInQuestion.IsMouseHovering && theButtonInQuestion.BackgroundColor != (cheat.Enabled ? (new Color(9, 46, 75) * 0.75f) : (new Color(9, 46, 110) * 0.75f)))
		{
			SoundEngine.PlaySound(12, 0);
			theButtonInQuestion.BackgroundColor = (cheat.Enabled ? (new Color(9, 46, 75) * 0.75f) : (new Color(9, 46, 110) * 0.75f));
			theButtonInQuestion.BorderColor = Main.OurFavoriteColor;
			return;
		}
		if (!theButtonInQuestion.IsMouseHovering && theButtonInQuestion.BackgroundColor != (cheat.Enabled ? (new Color(9, 46, 66) * 0.75f) : (new Color(9, 46, 89) * 0.75f)))
		{
			theButtonInQuestion.BackgroundColor = (cheat.Enabled ? (new Color(9, 46, 66) * 0.75f) : (new Color(9, 46, 89) * 0.75f));
			theButtonInQuestion.BorderColor = new Color(0, 0, 0) * 0.75f;
		}
		HandleSpecialButtonClicking(theButtonInQuestion);
	}

	private void UpdateButtonOnClick(UIMouseEvent evt, UIElement listeningElement, ref bool boolean)
	{
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fe: Unknown result type (might be due to invalid IL or missing references)
		if (!(evt.Target is UITextPanel<LocalizedText>))
		{
			return;
		}
		string text = ((UITextPanel<LocalizedText>)evt.Target).Text;
		Item heldItem = Main.LocalPlayer.HeldItem;
		for (int i = 0; i < specialButtonsList.Count; i++)
		{
			if (specialButtonsList != null && specialButtonsList.Count >= 3 && specialButtonsList[i].IsMouseHovering)
			{
				return;
			}
		}
		if (text == "Track Players")
		{
			boolean = !boolean;
		}
		else if (text == "Noclip")
		{
			Mods.noclip = !Mods.noclip;
			NetMessage.SendData(50, -1, -1, null, Main.myPlayer);
		}
		else if (text.Contains("Death Particle: "))
		{
			CycleDeathParticle();
		}
		else if (text.Contains("Death Effect: "))
		{
			CycleDeathEffect();
		}
		else if (text.Contains("Lucy Messages: "))
		{
			CycleLucyMessageState();
		}
		else if (text.Contains("Lucy Message On Death: "))
		{
			CycleLucyDeathMessageState();
		}
		else if (text.Contains("Force Soundtrack: "))
		{
			CycleForcedSoundtrackId();
		}
		else
		{
			switch (text)
			{
			case "Drop Entire Inventory":
				Mods.forcedDropInventory = true;
				Main.LocalPlayer.DropItems();
				Mods.forcedDropInventory = false;
				break;
			case "Toggle All Melee Accessories":
				ToggleAllMeleeAccs(ModsVars.playerHacksButtonAllMelee);
				break;
			case "Toggle All Ranged Accessories":
				ToggleAllRangedAccs(ModsVars.playerHacksButtonAllRanged);
				break;
			case "Toggle All Magic Accessories":
				ToggleAllMagicAccs(ModsVars.playerHacksButtonAllMagic);
				break;
			case "Toggle All Summoner Accessories":
				ToggleAllSummonAccs(ModsVars.playerHacksButtonAllSummoner);
				break;
			case "Teleport Item Bypass":
				boolean = !boolean;
				if (!Mods.teleportBypassInfoShown && Mods.bypassTeleportRequests && Mods.Instance.DetermineUnloadedSections > 0)
				{
					Main.NewText("Teleportation Potions, Demon Conch, and Magic Conch (including Shellphone teleports) will have issues if the chunks aren't loaded.", byte.MaxValue, 240, 20);
					Main.NewText("Reveal the map for the best results.", byte.MaxValue, 240, 20);
					Mods.teleportBypassInfoShown = true;
				}
				break;
			case "Server Spy":
				PlayerVariantID.Sets.serverPeekEnabled = !PlayerVariantID.Sets.serverPeekEnabled;
				if (!PlayerVariantID.Sets.serverPeekWarningShown && PlayerVariantID.Sets.serverPeekEnabled)
				{
					Main.NewText("In server spy mode, you are not fully connected to a server, but can still talk in chat and explore the world.", byte.MaxValue, 240, 20);
					Main.NewText("No other players are visible, and their names in chat may appear incorrectly. You also cannot interact with or modify anything.", byte.MaxValue, 240, 20);
					Main.NewText("Use this carefully. If you send a chat message without having fully joined, you may expose yourself. It is still announced when you leave while spying, so also take that into caution.", byte.MaxValue, 240, 20);
					Main.NewText("Server spy mode only starts when you join a server. When you exit server spy, you will respawn and finish joining the server.", byte.MaxValue, 240, 20);
					PlayerVariantID.Sets.serverPeekWarningShown = true;
				}
				else if (PlayerVariantID.Sets.spyingOnServer)
				{
					PlayerVariantID.Sets.spyingOnServer = false;
					Main.NewText("Server spy ended. You have joined the server as normal. Be warned that it may expose you for cheating if you sent messages before joining.", byte.MaxValue, 240, 20);
					NetMessage.SendData(4);
					NetMessage.SendData(8);
					Main.LocalPlayer.Spawn(PlayerSpawnContext.SpawningIntoWorld);
				}
				break;
			case "Shimmer All Town NPCs":
			{
				for (int l = 0; l < Main.npc.Length; l++)
				{
					NPC nPC = Main.npc[l];
					if (NPCID.Sets.ShimmerTownTransform[nPC.type])
					{
						Main.npc[l].AddBuff(353, 100);
					}
				}
				Main.NewText("Shimmered all Town NPCs!", 31, 184, 155);
				break;
			}
			case "Shimmer All Enemies":
			{
				for (int k = 0; k < Main.npc.Length; k++)
				{
					if (!Main.npc[k].friendly && !Main.npc[k].CountsAsACritter)
					{
						Main.npc[k].AddBuff(353, 100);
					}
				}
				Main.NewText("Shimmered all enemies!", 31, 184, 155);
				break;
			}
			case "Shimmer Godmode":
				boolean = !boolean;
				if (!boolean)
				{
					Main.LocalPlayer.ClearBuff(353);
				}
				break;
			case "Reveal Map":
				if (!Mods.revealMapCooldown)
				{
					Mods.RevealTheMap();
					break;
				}
				goto default;
			default:
			{
				char c;
				switch (text)
				{
				case "Wipe Ground Items":
					Mods.DeleteGroundItemsCall(0);
					break;
				case "Butcher Mobs":
					Mods.ButcherMobsAndNpcs(0, 1);
					break;
				case "Butcher Npcs":
					Mods.ButcherMobsAndNpcs(0, 2);
					break;
				case "Save Blueprint":
					Mods.Instance.SaveTilesToFile();
					break;
				case "Load Blueprint":
					Mods.Instance.LoadTilesFromFile();
					break;
				case "Save Schematic":
					Schematics.SaveTEDITSchematic();
					break;
				case "Load Schematic":
					Schematics.LoadTEDITSchematic();
					break;
				case "Clear Selection":
					if (Mods.copyMode)
					{
						Mods.copyStartA = Point.Zero;
						Mods.copyStartB = Point.Zero;
					}
					else if (Mods.selectionMode)
					{
						Mods.selectionA = Point.Zero;
						Mods.selectionB = Point.Zero;
					}
					break;
				default:
					switch (text.Length)
					{
					case 10:
						break;
					case 11:
						goto IL_06e3;
					case 23:
						goto IL_0709;
					case 12:
						goto IL_07a8;
					case 14:
						goto IL_07bd;
					case 20:
						goto IL_07d2;
					case 16:
						goto IL_07e7;
					case 25:
						goto IL_0826;
					case 15:
						goto IL_083b;
					default:
						goto IL_0a08;
					}
					c = text[0];
					if (c != 'C')
					{
						if (c != 'T')
						{
							if (c == 'W' && text == "Wall Brush")
							{
								goto IL_08be;
							}
						}
						else if (text == "Tile Brush")
						{
							goto IL_08be;
						}
					}
					else if (text == "Copy Tiles")
					{
						Mods.pasteMode = false;
						Mods.brushMode = false;
						Mods.selectionMode = false;
						Mods.Instance.DisableBrushes();
						boolean = !boolean;
						break;
					}
					goto IL_0a08;
				case null:
					goto IL_0a08;
					IL_083b:
					if (!(text == "Clear Selection") || !Mods.selectionMode)
					{
						goto IL_0a08;
					}
					Mods.selectionA = Point.Zero;
					Mods.selectionB = Point.Zero;
					break;
					IL_07a8:
					if (text == "Liquid Brush")
					{
						goto IL_08be;
					}
					goto IL_0a08;
					IL_0826:
					if (!(text == "Delete Selection (Liquid)"))
					{
						goto IL_0a08;
					}
					if ((Main.LocalPlayer.HeldItem.type == 5303 || Main.LocalPlayer.HeldItem.type == 4872 || Main.LocalPlayer.HeldItem.type == 3032 || Main.LocalPlayer.HeldItem.type == 5304) && Main.netMode == 1)
					{
						Mods.EraseTileSelectionMode(3);
					}
					else
					{
						Main.NewText("Hold any type of sponge to erase liquids within selected area..");
					}
					break;
					IL_06e3:
					c = text[0];
					if (c != 'A')
					{
						if (c != 'E')
						{
							if (c == 'P' && text == "Paste Tiles")
							{
								Mods.copyMode = false;
								Mods.brushMode = false;
								Mods.selectionMode = false;
								Mods.Instance.DisableBrushes();
								boolean = !boolean;
								break;
							}
						}
						else if (text == "Erase Brush")
						{
							goto IL_08be;
						}
					}
					else if (text == "All Brushes")
					{
						Mods.selectionMode = false;
						Mods.copyMode = false;
						Mods.pasteMode = false;
						boolean = !boolean;
						break;
					}
					goto IL_0a08;
					IL_07e7:
					if (!(text == "Delete Selection"))
					{
						goto IL_0a08;
					}
					Mods.EraseTileSelectionMode(0);
					break;
					IL_07d2:
					if (!(text == "Fill Selection (All)"))
					{
						goto IL_0a08;
					}
					if (Main.LocalPlayer.HeldItem.createTile >= 0 || Main.LocalPlayer.HeldItem.createWall > 0)
					{
						Mods.FillTilesSelectionModeV3();
					}
					else
					{
						Main.NewText("Hold the block or wall you want to fill the selected area with.");
					}
					break;
					IL_07bd:
					if (!(text == "Selection Mode"))
					{
						goto IL_0a08;
					}
					Mods.copyMode = false;
					Mods.brushMode = false;
					Mods.pasteMode = false;
					Mods.Instance.DisableBrushes();
					boolean = !boolean;
					break;
					IL_0709:
					c = text[18];
					if (c != 'T')
					{
						if (c == 'W' && text == "Delete Selection (Wall)")
						{
							Mods.EraseTileSelectionMode(2);
							break;
						}
					}
					else if (text == "Delete Selection (Tile)")
					{
						Mods.EraseTileSelectionMode(1);
						break;
					}
					goto IL_0a08;
					IL_0a08:
					if (text == "Restore Item Variables" && heldItem.type > 0)
					{
						int stack = heldItem.stack;
						byte prefix = heldItem.prefix;
						heldItem.SetDefaults(heldItem.type);
						heldItem.stack = stack;
						heldItem.prefix = prefix;
						break;
					}
					if (text.Contains("Modifier:"))
					{
						for (int j = heldItem.prefix + 1; j < 85; j++)
						{
							if (heldItem.CanApplyPrefix(j))
							{
								heldItem.Prefix(j);
								break;
							}
							if (heldItem.prefix >= 80)
							{
								heldItem.ResetPrefix();
							}
						}
						break;
					}
					if (text.Contains("Auto-Reuse:"))
					{
						heldItem.autoReuse = !heldItem.autoReuse;
						break;
					}
					if (text.Contains("Packet Type:"))
					{
						Mods.selectionTileSyncingMode = !Mods.selectionTileSyncingMode;
						((UITextPanel<LocalizedText>)evt.Target).SetText(Language.GetText("Tile Packet Type: " + ((!Mods.selectionTileSyncingMode) ? "Singular" : "Chunk")));
						break;
					}
					if (text == "Refresh Binds")
					{
						Mods.keyBindMaster.Clear();
						break;
					}
					if (text.Contains("Brush Shape:"))
					{
						Mods.brushCircle = !Mods.brushCircle;
						((UITextPanel<LocalizedText>)evt.Target).SetText(Language.GetText("Brush Shape: " + ((!Mods.brushCircle) ? "Square" : "Circle")));
						break;
					}
					switch (text)
					{
					case "Packet Maximum":
					case "Packet 100/ms":
						Mods.tilePacketMaxMode = !Mods.tilePacketMaxMode;
						((UITextPanel<LocalizedText>)evt.Target).SetText(Language.GetText(Mods.tilePacketMaxMode ? "Packet Maximum" : "Packet 100/ms"));
						break;
					case "Cancel Paste":
						Mods.cancelPaste = true;
						break;
					default:
						if (!text.Contains("Toggle All"))
						{
							boolean = !boolean;
						}
						break;
					}
					break;
					IL_08be:
					boolean = !boolean;
					break;
				}
				break;
			}
			}
		}
		if (text.Contains("Modifier:"))
		{
			SoundEngine.PlaySound(2, 0, -1, 37);
		}
		else
		{
			SoundEngine.PlaySound(boolean ? 10 : 11, 0);
		}
		if (((UITextPanel<LocalizedText>)evt.Target).Text == "⮞ Player" || ((UITextPanel<LocalizedText>)evt.Target).Text == "⮟ Player")
		{
			((UITextPanel<LocalizedText>)evt.Target).SetText(Language.GetText((!Mods.commandPanelPlayerButton) ? "⮞ Player" : "⮟ Player"));
		}
		else if (((UITextPanel<LocalizedText>)evt.Target).Text == "⮞ Travel" || ((UITextPanel<LocalizedText>)evt.Target).Text == "⮟ Travel")
		{
			((UITextPanel<LocalizedText>)evt.Target).SetText(Language.GetText((!Mods.commandPanelTravelButton) ? "⮞ Travel" : "⮟ Travel"));
		}
		else if (((UITextPanel<LocalizedText>)evt.Target).Text == "⮞ Cosmetics" || ((UITextPanel<LocalizedText>)evt.Target).Text == "⮟ Cosmetics")
		{
			((UITextPanel<LocalizedText>)evt.Target).SetText(Language.GetText((!ModsVars.cosmeticHacksButton) ? "⮞ Cosmetics" : "⮟ Cosmetics"));
		}
		else if (((UITextPanel<LocalizedText>)evt.Target).Text == "⮞ Miscellaneous" || ((UITextPanel<LocalizedText>)evt.Target).Text == "⮟ Miscellaneous")
		{
			((UITextPanel<LocalizedText>)evt.Target).SetText(Language.GetText((!Mods.commandPanelMiscsButton) ? "⮞ Miscellaneous" : "⮟ Miscellaneous"));
		}
		else if (((UITextPanel<LocalizedText>)evt.Target).Text == "⮞ Biome Overrides" || ((UITextPanel<LocalizedText>)evt.Target).Text == "⮟ Biome Overrides")
		{
			((UITextPanel<LocalizedText>)evt.Target).SetText(Language.GetText((!ModsVars.commandPanelZonesButton) ? "⮞ Biome Overrides" : "⮟ Biome Overrides"));
		}
		else if (((UITextPanel<LocalizedText>)evt.Target).Text == "⮞ World Edit" || ((UITextPanel<LocalizedText>)evt.Target).Text == "⮟ World Edit")
		{
			((UITextPanel<LocalizedText>)evt.Target).SetText(Language.GetText((!Mods.commandPanelBuildMenuButton) ? "⮞ World Edit" : "⮟ World Edit"));
		}
		UpdateButtonList();
	}

	private void UpdateButtonOnClick(UIMouseEvent evt, UIElement listeningElement, ICheat cheat)
	{
		if (evt.Target is UITextPanel<LocalizedText>)
		{
			cheat.Enabled = !cheat.Enabled;
			SoundEngine.PlaySound(cheat.Enabled ? 10 : 11, 0);
			UpdateButtonList();
		}
	}

	private void HandleSpecialButtonClicking(UITextPanel<LocalizedText> theButtonInQuestion)
	{
		Item item = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];
		if (theButtonInQuestion.Text == "+" && theButtonInQuestion.IsMouseHovering)
		{
			if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Tile Brush")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					Mods.tileBrushSize++;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Mods.tileBrushSize++;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Erase Brush")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					Mods.eraseBrushSize++;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Mods.eraseBrushSize++;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Wall Brush")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					Mods.wallBrushSize++;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Mods.wallBrushSize++;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Liquid Brush")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					Mods.liquidBrushSize++;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Mods.liquidBrushSize++;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Damage")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.damage++;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.damage++;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Usetime")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.useTime++;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.useTime++;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Animation Speed")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.useAnimation++;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.useAnimation++;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Projectile")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					if (item.shoot >= ProjectileID.Count - 1)
					{
						item.shoot = 0;
					}
					else
					{
						item.shoot += 10;
					}
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					if (item.shoot >= ProjectileID.Count - 1)
					{
						item.shoot = 0;
					}
					else
					{
						item.shoot++;
					}
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Projectile Speed")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.shootSpeed++;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.shootSpeed++;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Pick Power")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.pick++;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.pick++;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Axe Power")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.axe++;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.axe++;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Hammer Power")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.hammer++;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.hammer++;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Stack")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.stack++;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.stack++;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Kill Aura")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					Mods.killAuraSize++;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Mods.killAuraSize++;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Packet Maximum" || ((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Packet 100/ms")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					Mods.tilePacketRate += ((Mods.tilePacketRate > 1000) ? 100 : 10);
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Mods.tilePacketRate++;
				}
			}
		}
		else
		{
			if (!(theButtonInQuestion.Text == "-") || !theButtonInQuestion.IsMouseHovering)
			{
				return;
			}
			if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Tile Brush" && Mods.tileBrushSize > 0)
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					Mods.tileBrushSize--;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Mods.tileBrushSize--;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Erase Brush" && Mods.eraseBrushSize > 0)
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					Mods.eraseBrushSize--;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Mods.eraseBrushSize--;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Wall Brush" && Mods.wallBrushSize > 0)
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					Mods.wallBrushSize--;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Mods.wallBrushSize--;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Liquid Brush" && Mods.liquidBrushSize > 0)
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					Mods.liquidBrushSize--;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Mods.liquidBrushSize--;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Damage" && item.damage > 0)
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.damage--;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.damage--;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Usetime" && item.useTime > 0)
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.useTime--;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.useTime--;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Animation Speed" && item.useAnimation > 0)
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.useAnimation--;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.useAnimation--;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Projectile")
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					if (item.shoot <= 0)
					{
						item.shoot = ProjectileID.Count - 1;
					}
					else
					{
						item.shoot -= 10;
					}
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					if (item.shoot <= 0)
					{
						item.shoot = ProjectileID.Count - 1;
					}
					else
					{
						item.shoot--;
					}
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Projectile Speed" && item.shootSpeed > 0f)
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.shootSpeed--;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.shootSpeed--;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Pick Power" && item.pick > 0)
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.pick--;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.pick--;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Axe Power" && item.axe > 0)
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.axe--;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.axe--;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Hammer Power" && item.hammer > 0)
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.hammer--;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.hammer--;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Stack" && item.stack > 1)
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					item.stack--;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					item.stack--;
				}
			}
			else if (((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Kill Aura" && Mods.killAuraSize > 0)
			{
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					Mods.killAuraSize--;
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Mods.killAuraSize--;
				}
			}
			else
			{
				if ((!(((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Packet Maximum") && !(((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Packet 100/ms")) || Mods.tilePacketRate <= ((((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Packet 100/ms") ? 1 : 0))
				{
					return;
				}
				if (((KeyboardState)(ref Main.keyState)).IsKeyDown((Keys)162) && Main.mouseLeft)
				{
					Mods.tilePacketRate -= ((Mods.tilePacketRate > 1000) ? 100 : 10);
					if (Mods.tilePacketRate < 0)
					{
						Mods.tilePacketRate = ((((UITextPanel<LocalizedText>)theButtonInQuestion.Parent).Text == "Packet 100/ms") ? 1 : 0);
					}
				}
				else if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Mods.tilePacketRate--;
				}
			}
		}
	}

	private void CreateSpecialThreeButtons(UITextPanel<LocalizedText> appendTo, string text, float fontSize)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		bool shitBool = false;
		float pixels;
		float pixels2;
		if (text == "Maximum Que")
		{
			pixels = 0.02f;
			pixels2 = 0.02f;
		}
		else
		{
			pixels = 1f;
			pixels2 = 1f;
		}
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText(text), fontSize, large: true);
		uITextPanel.Width.Set(pixels, 0f);
		uITextPanel.Height.Set(pixels2, 0f);
		uITextPanel.HAlign = 1f;
		uITextPanel.VAlign = 0.5f;
		uITextPanel.BackgroundColor = new Color(9, 46, 89) * 0.75f;
		uITextPanel.BorderColor = new Color(0, 0, 0) * 0.75f;
		specialButtonsList.Add(uITextPanel);
		appendTo.Append(specialButtonsList[specialButtonsList.Count - 1]);
		UITextPanel<LocalizedText> showDeets2 = new UITextPanel<LocalizedText>(Language.GetText("+"), fontSize, large: true);
		showDeets2.Width.Set(pixels, 0f);
		showDeets2.Height.Set(pixels2, 0f);
		showDeets2.HAlign = 0.84f;
		showDeets2.VAlign = 0.5f;
		showDeets2.OnUpdate += delegate
		{
			UpdateButtonVariables(showDeets2, ref shitBool);
		};
		specialButtonsList.Add(showDeets2);
		appendTo.Append(specialButtonsList[specialButtonsList.Count - 1]);
		UITextPanel<LocalizedText> showDeets3 = new UITextPanel<LocalizedText>(Language.GetText("-"), fontSize, large: true);
		showDeets3.Width.Set(pixels, 0f);
		showDeets3.Height.Set(pixels2, 0f);
		showDeets3.HAlign = 0.75f;
		showDeets3.VAlign = 0.5f;
		showDeets3.OnUpdate += delegate
		{
			UpdateButtonVariables(showDeets3, ref shitBool);
		};
		specialButtonsList.Add(showDeets3);
		appendTo.Append(specialButtonsList[specialButtonsList.Count - 1]);
	}

	public string GetPrefixName(int id)
	{
		return id switch
		{
			1 => "Large", 
			2 => "Massive", 
			3 => "Dangerous", 
			4 => "Savage", 
			5 => "Sharp", 
			6 => "Pointy", 
			7 => "Tiny", 
			8 => "Terrible", 
			9 => "Small", 
			10 => "Dull", 
			11 => "Unhappy", 
			12 => "Bulky", 
			13 => "Shameful", 
			14 => "Heavy", 
			15 => "Light", 
			16 => "Sighted", 
			17 => "Rapid", 
			18 => "Hasty", 
			19 => "Intimidating", 
			20 => "Deadly", 
			21 => "Staunch", 
			22 => "Awful", 
			23 => "Lethargic", 
			24 => "Awkward", 
			25 => "Powerful", 
			26 => "Mystic", 
			27 => "Adept", 
			28 => "Masterful", 
			29 => "Inept", 
			30 => "Ignorant", 
			31 => "Deranged", 
			32 => "Intense", 
			33 => "Taboo", 
			34 => "Celestial", 
			35 => "Furious", 
			36 => "Keen", 
			37 => "Superior", 
			38 => "Forceful", 
			39 => "Broken", 
			40 => "Damaged", 
			41 => "Shoddy", 
			42 => "Quick", 
			43 => "Deadly", 
			44 => "Agile", 
			45 => "Nimble", 
			46 => "Murderous", 
			47 => "Slow", 
			48 => "Sluggish", 
			49 => "Lazy", 
			50 => "Annoying", 
			51 => "Nasty", 
			52 => "Manic", 
			53 => "Hurtful", 
			54 => "Strong", 
			55 => "Unpleasant", 
			56 => "Weak", 
			57 => "Ruthless", 
			58 => "Frenzying", 
			59 => "Godly", 
			60 => "Demonic", 
			61 => "Zealous", 
			62 => "Hard", 
			63 => "Guarding", 
			64 => "Armored", 
			65 => "Warding", 
			66 => "Arcane", 
			67 => "Precise", 
			68 => "Lucky", 
			69 => "Jagged", 
			70 => "Spiked", 
			71 => "Angry", 
			72 => "Menacing", 
			73 => "Brisk", 
			74 => "Fleeting", 
			75 => "Hasty", 
			76 => "Quick", 
			77 => "Wild", 
			78 => "Rash", 
			79 => "Intrepid", 
			80 => "Violent", 
			81 => "Legendary", 
			82 => "Unreal", 
			83 => "Mythical", 
			84 => "Legendary", 
			_ => "Unmodified Item", 
		};
	}

	public bool PlayerPressedHotbarKey()
	{
		if (!PlayerInput.Triggers.Current.Hotbar10 && !PlayerInput.Triggers.Current.Hotbar1 && !PlayerInput.Triggers.Current.Hotbar2 && !PlayerInput.Triggers.Current.Hotbar3 && !PlayerInput.Triggers.Current.Hotbar4 && !PlayerInput.Triggers.Current.Hotbar5 && !PlayerInput.Triggers.Current.Hotbar6 && !PlayerInput.Triggers.Current.Hotbar7 && !PlayerInput.Triggers.Current.Hotbar8)
		{
			return PlayerInput.Triggers.Current.Hotbar9;
		}
		return true;
	}

	public static bool PlayerPressedHotbarKeyV2()
	{
		if (!PlayerInput.Triggers.Current.Hotbar10 && !PlayerInput.Triggers.Current.Hotbar1 && !PlayerInput.Triggers.Current.Hotbar2 && !PlayerInput.Triggers.Current.Hotbar3 && !PlayerInput.Triggers.Current.Hotbar4 && !PlayerInput.Triggers.Current.Hotbar5 && !PlayerInput.Triggers.Current.Hotbar6 && !PlayerInput.Triggers.Current.Hotbar7 && !PlayerInput.Triggers.Current.Hotbar8)
		{
			return PlayerInput.Triggers.Current.Hotbar9;
		}
		return true;
	}

	private void ReInitializeSpecialButtonsForItemTab()
	{
		Item item = Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem];
		for (int i = 2; i < 11; i++)
		{
			selectedItemHacksButton[i].RemoveAllChildren();
		}
		CreateSpecialThreeButtons(selectedItemHacksButton[2], item.damage.ToString(), 0.26f);
		CreateSpecialThreeButtons(selectedItemHacksButton[3], item.useTime.ToString(), 0.26f);
		CreateSpecialThreeButtons(selectedItemHacksButton[4], item.useAnimation.ToString(), 0.26f);
		CreateSpecialThreeButtons(selectedItemHacksButton[5], item.shoot.ToString(), 0.26f);
		CreateSpecialThreeButtons(selectedItemHacksButton[6], item.shootSpeed.ToString(), 0.26f);
		CreateSpecialThreeButtons(selectedItemHacksButton[7], item.pick.ToString(), 0.26f);
		CreateSpecialThreeButtons(selectedItemHacksButton[8], item.axe.ToString(), 0.26f);
		CreateSpecialThreeButtons(selectedItemHacksButton[9], item.hammer.ToString(), 0.26f);
		CreateSpecialThreeButtons(selectedItemHacksButton[10], item.stack.ToString(), 0.26f);
	}

	public static void ToggleAllMeleeAccs(bool toggleState)
	{
		ModsVars.forceWarriorEmblem = !toggleState;
		ModsVars.forceFeralClaws = !toggleState;
		ModsVars.forceMagmaStone = !toggleState;
		ModsVars.forceTitanGlove = !toggleState;
		ModsVars.forceYoyoString = !toggleState;
		ModsVars.forceYoyoCounterweight = !toggleState;
		ModsVars.forceYoyoGlove = !toggleState;
		ModsVars.playerHacksButtonAllMelee = !toggleState;
	}

	public static void ToggleAllRangedAccs(bool toggleState)
	{
		ModsVars.forceRangerEmblem = !toggleState;
		ModsVars.forceMagicQuiver = !toggleState;
		ModsVars.forceMoltenQuiver = !toggleState;
		ModsVars.forceSniperScope = !toggleState;
		ModsVars.playerHacksButtonAllRanged = !toggleState;
	}

	public static void ToggleAllMagicAccs(bool toggleState)
	{
		ModsVars.forceSorcererEmblem = !toggleState;
		ModsVars.forceBandOfStarpower = !toggleState;
		ModsVars.forceManaRegenerationBand = !toggleState;
		ModsVars.forceMagicCuffs = !toggleState;
		ModsVars.forceNaturesGift = !toggleState;
		ModsVars.forceManaFlower = !toggleState;
		ModsVars.forceCelestialMagnet = !toggleState;
		ModsVars.forceCelestialEmblem = !toggleState;
		ModsVars.playerHacksButtonAllMagic = !toggleState;
	}

	public static void ToggleAllSummonAccs(bool toggleState)
	{
		ModsVars.forceSummonerEmblem = !toggleState;
		ModsVars.forceDD2Accessory = !toggleState;
		ModsVars.forceHerculesBeetle = !toggleState;
		ModsVars.forceNecromanticScroll = !toggleState;
		ModsVars.forcePygmyNecklace = !toggleState;
		ModsVars.playerHacksButtonAllSummoner = !toggleState;
	}

	public static void ToggleAllCombatAccs(bool toggleState)
	{
	}

	public static void ToggleAllFishingAccs(bool toggleState)
	{
	}

	public static void ToggleAllMovementAccs(bool toggleState)
	{
	}

	public static void ToggleAllInfoAccs(bool toggleState)
	{
	}

	public static void ToggleAllMechanicalAccs(bool toggleState)
	{
	}

	public static void ToggleAllMiscAccs(bool toggleState)
	{
	}

	public string GetDeathParticleName(int id)
	{
		return id switch
		{
			-1 => "None", 
			0 => "Keybrand", 
			1 => "Flame Waders", 
			2 => "Stellar Tune", 
			3 => "WoF Goat Mount FLames", 
			4 => "Black Lightning", 
			5 => "Rainbow Rod Hit", 
			6 => "Black Lightning Small", 
			7 => "Stardust Punch", 
			8 => "Princess Weapon", 
			9 => "Paladin's Hammer", 
			10 => "Night's Edge", 
			11 => "Silver Bullet Sparkle", 
			12 => "True Night's Edge", 
			13 => "Excalibur", 
			14 => "True Excalibur", 
			15 => "Terra Blade", 
			16 => "Leaf Crystal Passive", 
			17 => "Leaf Crystal Shot", 
			18 => "Ash Tree Shake", 
			19 => "Pet Exchange", 
			20 => "Slap Hand SFX", 
			21 => "Flymeal SFX", 
			22 => "Gas Trap", 
			23 => "Item Transfer", 
			24 => "Shimmer Arrow", 
			25 => "Town Slime Transform", 
			26 => "Loadout Change", 
			27 => "Shimmer Block", 
			28 => "Digestion", 
			29 => "Waffle Iron SFX", 
			30 => "Poo Fly", 
			31 => "Shimmer Town NPC", 
			32 => "Shimmer Town NPC Send", 
			_ => "This Code Fucking Sucks ASS. I did all of this shit manually. If this message somehow appears, I fucked up.", 
		};
	}

	public string GetDeathEffectName(int id)
	{
		switch (id)
		{
		case -1:
			return "None";
		case 11:
		case 12:
		case 13:
			return "Cloud Smoke";
		case 16:
			return "Yellow Stars";
		case 17:
			return "Small Blue Stars";
		case -3:
			return "Yellow & Blue Stars";
		case 61:
		case 62:
		case 63:
			return "Smoke";
		case 99:
			return "Dark Smoke";
		case 276:
		case 277:
		case 278:
		case 279:
		case 280:
		case 281:
		case 282:
			return "Confetti";
		case 375:
		case 376:
		case 377:
			return "Blue Smoke";
		case 411:
			return "Tiny Bubbles";
		case 412:
			return "Small Bubbles";
		case 413:
			return "Medium Bubbles";
		case 414:
			return "Large Bubbles";
		case 415:
			return "Huge Bubbles";
		case 570:
		case 571:
		case 572:
			return "Music Notes";
		case 825:
		case 826:
		case 827:
			return "Lingering Smoke";
		case 861:
		case 862:
			return "Large Smoke";
		case 907:
			return "Pink Balloons";
		case 908:
			return "Blue Balloons";
		case 909:
			return "Green Balloons";
		case -2:
			return "All Balloons";
		case 1201:
			return "Chum";
		case 1218:
			return "Coins";
		default:
			return "This Code Fucking Sucks ASS. I did all of this shit manually. If this message somehow appears, I fucked up.";
		}
	}

	public static void CycleDeathParticle()
	{
		switch (Mods.deathParticleCosmeticId)
		{
		case -1:
			Mods.deathParticleCosmeticId = 0;
			break;
		case 0:
			Mods.deathParticleCosmeticId = 2;
			break;
		case 2:
			Mods.deathParticleCosmeticId = 4;
			break;
		case 4:
			Mods.deathParticleCosmeticId = 5;
			break;
		case 5:
			Mods.deathParticleCosmeticId = 10;
			break;
		case 10:
			Mods.deathParticleCosmeticId = 12;
			break;
		case 12:
			Mods.deathParticleCosmeticId = 13;
			break;
		case 13:
			Mods.deathParticleCosmeticId = 14;
			break;
		case 14:
			Mods.deathParticleCosmeticId = 15;
			break;
		case 15:
			Mods.deathParticleCosmeticId = 16;
			break;
		case 16:
			Mods.deathParticleCosmeticId = 17;
			break;
		case 17:
			Mods.deathParticleCosmeticId = 19;
			break;
		case 19:
			Mods.deathParticleCosmeticId = 20;
			break;
		case 20:
			Mods.deathParticleCosmeticId = 21;
			break;
		case 21:
			Mods.deathParticleCosmeticId = 22;
			break;
		case 22:
			Mods.deathParticleCosmeticId = 24;
			break;
		case 24:
			Mods.deathParticleCosmeticId = 25;
			break;
		case 25:
			Mods.deathParticleCosmeticId = 29;
			break;
		case 29:
			Mods.deathParticleCosmeticId = 31;
			break;
		case 31:
			Mods.deathParticleCosmeticId = 32;
			break;
		case 32:
			Mods.deathParticleCosmeticId = -1;
			break;
		default:
			Mods.deathParticleCosmeticId = 0;
			break;
		}
		if (Mods.deathParticleCosmeticId != -1)
		{
			Mods.deathParticleCosmeticEnabled = true;
		}
		else
		{
			Mods.deathParticleCosmeticEnabled = false;
		}
	}

	public static void CycleDeathEffect()
	{
		switch (Mods.deathGoreCosmeticId)
		{
		case -1:
			Mods.deathGoreCosmeticId = 11;
			break;
		case 11:
		case 12:
		case 13:
			Mods.deathGoreCosmeticId = 16;
			break;
		case 16:
			Mods.deathGoreCosmeticId = 17;
			break;
		case 17:
			Mods.deathGoreCosmeticId = -3;
			break;
		case -3:
			Mods.deathGoreCosmeticId = 61;
			break;
		case 61:
		case 62:
		case 63:
			Mods.deathGoreCosmeticId = 99;
			break;
		case 99:
			Mods.deathGoreCosmeticId = 276;
			break;
		case 276:
		case 277:
		case 278:
		case 279:
		case 280:
		case 281:
		case 282:
			Mods.deathGoreCosmeticId = 375;
			break;
		case 375:
		case 376:
		case 377:
			Mods.deathGoreCosmeticId = 411;
			break;
		case 411:
			Mods.deathGoreCosmeticId = 412;
			break;
		case 412:
			Mods.deathGoreCosmeticId = 413;
			break;
		case 413:
			Mods.deathGoreCosmeticId = 414;
			break;
		case 414:
			Mods.deathGoreCosmeticId = 415;
			break;
		case 415:
			Mods.deathGoreCosmeticId = 570;
			break;
		case 570:
		case 571:
		case 572:
			Mods.deathGoreCosmeticId = 825;
			break;
		case 825:
		case 826:
		case 827:
			Mods.deathGoreCosmeticId = 861;
			break;
		case 861:
		case 862:
			Mods.deathGoreCosmeticId = 907;
			break;
		case 907:
			Mods.deathGoreCosmeticId = 908;
			break;
		case 908:
			Mods.deathGoreCosmeticId = 909;
			break;
		case 909:
			Mods.deathGoreCosmeticId = -2;
			break;
		case -2:
			Mods.deathGoreCosmeticId = 1201;
			break;
		case 1201:
			Mods.deathGoreCosmeticId = 1218;
			break;
		case 1218:
			Mods.deathGoreCosmeticId = -1;
			break;
		default:
			Mods.deathGoreCosmeticId = 276;
			break;
		}
		if (Mods.deathGoreCosmeticId != -1)
		{
			Mods.deathGoreCosmeticEnabled = true;
		}
		else
		{
			Mods.deathGoreCosmeticEnabled = false;
		}
	}

	public static void CycleLucyMessageState()
	{
		switch (Mods.lucyMessagesMode)
		{
		case 0:
			Mods.lucyMessagesMode = 1;
			break;
		case 1:
			Mods.lucyMessagesMode = 2;
			break;
		case 2:
			Mods.lucyMessagesMode = 3;
			break;
		case 3:
			Mods.lucyMessagesMode = 0;
			break;
		default:
			Mods.lucyMessagesMode = 0;
			break;
		}
	}

	public static void CycleLucyDeathMessageState()
	{
		switch (Mods.lucyDeathMessageMode)
		{
		case 0:
			Mods.lucyDeathMessageMode = 1;
			break;
		case 1:
			Mods.lucyDeathMessageMode = 2;
			break;
		case 2:
			Mods.lucyDeathMessageMode = 3;
			break;
		case 3:
			Mods.lucyDeathMessageMode = 0;
			break;
		default:
			Mods.lucyDeathMessageMode = 0;
			break;
		}
	}

	public string GetLucyMessageModeName(int id)
	{
		return id switch
		{
			0 => "No Override", 
			1 => "Forced Off", 
			2 => "Forced On", 
			3 => "Forced On (Everyone)", 
			_ => "I Repeat, This Code Fucking Sucks ASS.", 
		};
	}

	public string GetLucyDeathMessageModeName(int id)
	{
		return id switch
		{
			0 => "Disabled", 
			1 => "Self", 
			2 => "Others", 
			3 => "Everyone", 
			_ => "I Repeat, This Code Fucking Sucks ASS.", 
		};
	}

	public string GetForcedSoundtrackName(int id)
	{
		return id switch
		{
			0 => "No Override", 
			1 => "Terraria", 
			2 => "Terraria: Otherworld", 
			_ => "I Repeat, This Code Fucking Sucks ASS.", 
		};
	}

	public static void CycleForcedSoundtrackId()
	{
		switch (Mods.forceSoundtrackId)
		{
		case 0:
			Mods.forceSoundtrackId = 1;
			break;
		case 1:
			Mods.forceSoundtrackId = 2;
			break;
		case 2:
			Mods.forceSoundtrackId = 0;
			break;
		default:
			Mods.forceSoundtrackId = 0;
			break;
		}
	}
}
