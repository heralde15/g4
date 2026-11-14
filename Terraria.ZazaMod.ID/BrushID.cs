using ReLogic.Reflection;

namespace Terraria.ZazaMod.ID;

public class BrushID
{
	public const byte TileBrush = 0;

	public const byte EraseBrush = 1;

	public const byte WallBrush = 2;

	public const byte LiquidAndDryBrush = 3;

	public const byte TilePaintBrush = 4;

	public const byte TileExploitBrush = 5;

	public static readonly byte Count = 6;

	public static readonly IdDictionary Search = IdDictionary.Create<BrushID, byte>();
}
