using ReLogic.Reflection;

namespace Terraria.ZazaMod.ID;

public class PlatformUserID
{
	public const byte None = 0;

	public const byte Stadia = 1;

	public const byte XBO = 2;

	public const byte PSN = 3;

	public const byte Editor = 4;

	public const byte Nintendo = 5;

	public static readonly byte Count = 6;

	public static readonly IdDictionary Search = IdDictionary.Create<PlatformUserID, byte>();

	public static string PlatformNameFromId(byte ID)
	{
		return ID switch
		{
			1 => "Google Stadia", 
			2 => "Xbox", 
			3 => "PlayStation", 
			4 => "Unity Editor", 
			5 => "Nintendo Switch", 
			_ => "None", 
		};
	}
}
