using Newtonsoft.Json;

namespace Terraria.Utilities.Terraria.Utilities;

public struct FloatRange(float minimum, float maximum)
{
	[JsonProperty("Min")]
	public readonly float Minimum = minimum;

	[JsonProperty("Max")]
	public readonly float Maximum = maximum;

	public static FloatRange operator *(FloatRange range, float scale)
	{
		return new FloatRange(range.Minimum * scale, range.Maximum * scale);
	}

	public static FloatRange operator *(float scale, FloatRange range)
	{
		return range * scale;
	}

	public static FloatRange operator /(FloatRange range, float scale)
	{
		return new FloatRange(range.Minimum / scale, range.Maximum / scale);
	}

	public static FloatRange operator /(float scale, FloatRange range)
	{
		return range / scale;
	}
}
