using System;
using Terraria.UtilClient.Common.Utility;
using Terraria.UtilClient.Config;
using Terraria.UtilClient.Config.Types;
using Terraria.UtilClient.Tweaks;
using Terraria.UtilClient.Tweaks.Collections;

namespace Terraria.UtilClient.Features;

public static class UuidProvider
{
	public enum UuidPrivacyMode
	{
		RandomAtStartup,
		RandomPerServer,
		RandomPerConnection
	}

	public static Guid StartupUUID { get; private set; } = Guid.NewGuid();

	public static Guid CurrentConnectionUUID { get; private set; } = Guid.NewGuid();

	public static void RegenerateStartupUuid()
	{
		StartupUUID = Guid.NewGuid();
	}

	public static void RegenerateConnectionUuid()
	{
		CurrentConnectionUUID = Guid.NewGuid();
	}

	public static string GetUUID()
	{
		if (!TweaksCollectionExtensions.get_Current<NetworkingTweaks>().EnableUUIDPrivacy)
		{
			return Main.clientUUID;
		}
		switch (TweaksCollectionExtensions.get_Current<NetworkingTweaks>().UUIDPrivacyType)
		{
		case UuidPrivacyMode.RandomPerServer:
		{
			if (!Netplay.Disconnect && Netplay.Connection.IsActive && Netplay.Connection.Socket.IsConnected() && ushort.TryParse(Main.getPort, out var port))
			{
				ServerEndPoint endpoint = new ServerEndPoint(Main.getIP, port);
				return UConfigExtensions.get_Current<ServerUuidsConfig>().GetOrCreateUuid(endpoint).ToString();
			}
			break;
		}
		case UuidPrivacyMode.RandomAtStartup:
			return StartupUUID.ToString();
		}
		return CurrentConnectionUUID.ToString();
	}
}
