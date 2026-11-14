using System;
using System.IO;
using Terraria.IO;
using Terraria.Localization;

namespace Terraria.ZazaMod;

public class SavedServerData : FileData
{
	public static Preferences SavedServersPref = new Preferences(Main.SavePath + System.IO.Path.DirectorySeparatorChar + "servers.json");

	public string ServerName = "";

	public string ServerIP = "";

	public int ServerPort;

	public ushort WorldSizeX;

	public short GameMode;

	public bool DrunkWorld;

	public bool NotTheBees;

	public bool ForTheWorthy;

	public bool Anniversary;

	public bool DontStarve;

	public bool RemixWorld;

	public bool NoTrapsWorld;

	public bool ZenithWorld;

	public bool HasCorruption = true;

	public bool IsHardMode;

	public bool DefeatedMoonlord;

	public bool IsValid;

	public bool Favorited;

	public LocalizedText WorldSizeName => WorldSizeX switch
	{
		1750 => Language.GetText("Legacy Tiny"), 
		1800 => Lang.prefix[7], 
		4200 => Language.GetText("UI.WorldSizeSmall"), 
		5000 => Language.GetText("Pre-Alpha"), 
		6400 => Language.GetText("UI.WorldSizeMedium"), 
		8400 => Language.GetText("UI.WorldSizeLarge"), 
		12800 => Language.GetText("Huge"), 
		_ => Language.GetText("UI.WorldSizeUnknown"), 
	};

	public SavedServerData()
		: base("ServerData")
	{
		IsValid = true;
	}

	public SavedServerData(string name, string ip, int port, ushort tilesX = 0, short gameMode = 0, bool corrupt = false, bool hard = false, bool downedMoonLord = false)
		: base("ServerData")
	{
		ServerName = name;
		ServerIP = ip;
		ServerPort = port;
		WorldSizeX = tilesX;
		GameMode = gameMode;
		HasCorruption = corrupt;
		IsHardMode = hard;
		DefeatedMoonlord = downedMoonLord;
		IsValid = true;
	}

	public override void SetAsActive()
	{
	}

	public override void MoveToCloud()
	{
	}

	public override void MoveToLocal()
	{
	}

	public static void TryMigrateRecentServerAndPort()
	{
		for (int i = 0; i < Main.recentIP.Length && !(Main.recentIP[i] == ""); i++)
		{
			Main.RecentServers.Add(new SavedServerData(Main.recentWorld[i], Main.recentIP[i], Main.recentPort[i], 0, 0));
		}
		SaveRecentServers();
	}

	public static void RefreshRecentServers()
	{
		try
		{
			Main.RecentServers.Clear();
			OpenRecentServers();
		}
		catch
		{
		}
	}

	public static void OpenRecentServers()
	{
		try
		{
			if (!SavedServersPref.Load())
			{
				TryMigrateRecentServerAndPort();
				return;
			}
			SavedServersPref.GetAllKeys().ForEach(delegate(string key)
			{
				if (uint.TryParse(key, out var _))
				{
					Main.RecentServers.Add(FromBase64(SavedServersPref.Get(key, "")));
				}
			});
		}
		catch
		{
		}
	}

	public static void SaveRecentServers()
	{
		Utils.TryCreatingDirectory(Main.SavePath);
		try
		{
			for (int i = 0; i < Main.RecentServers.Count; i++)
			{
				SavedServersPref.Put($"{i}", Main.RecentServers[i].AsBase64());
			}
			SavedServersPref.Save();
		}
		catch
		{
		}
	}

	protected string AsBase64()
	{
		MemoryStream memoryStream = new MemoryStream(new byte[1024]);
		BinaryWriter w = new BinaryWriter(memoryStream);
		Serialize(w);
		int pos = (int)w.BaseStream.Position;
		BinaryReader r = new BinaryReader(memoryStream);
		r.BaseStream.Position = 0L;
		byte[] bytes = new byte[pos];
		for (int j = 0; j < pos; j++)
		{
			bytes[j] = r.ReadByte();
		}
		return Convert.ToBase64String(bytes);
	}

	private static SavedServerData FromBase64(string b64)
	{
		SavedServerData data = new SavedServerData();
		try
		{
			BinaryReader reader = new BinaryReader(new MemoryStream(Convert.FromBase64String(b64)));
			data.ServerName = reader.ReadString();
			data.ServerIP = reader.ReadString();
			data.ServerPort = reader.ReadInt32();
			data.WorldSizeX = reader.ReadUInt16();
			data.GameMode = reader.ReadByte();
			BitsByte b65 = reader.ReadByte();
			data.DrunkWorld = b65[0];
			data.NotTheBees = b65[1];
			data.ForTheWorthy = b65[2];
			data.Anniversary = b65[3];
			data.DontStarve = b65[4];
			data.RemixWorld = b65[5];
			data.NoTrapsWorld = b65[6];
			data.ZenithWorld = b65[7];
			BitsByte b66 = reader.ReadByte();
			data.HasCorruption = b66[0];
			data.IsHardMode = b66[1];
			data.DefeatedMoonlord = b66[2];
			data.Favorited = reader.ReadBoolean();
			if (string.IsNullOrEmpty(data.ServerName) || string.IsNullOrEmpty(data.ServerIP))
			{
				data.IsValid = false;
			}
			return data;
		}
		catch
		{
			data.IsValid = false;
			return data;
		}
	}

	protected void Serialize(BinaryWriter writer)
	{
		writer.Write(ServerName);
		writer.Write(ServerIP);
		writer.Write(ServerPort);
		writer.Write(WorldSizeX);
		writer.Write((byte)GameMode);
		BitsByte b = (byte)0;
		b[0] = DrunkWorld;
		b[1] = NotTheBees;
		b[2] = ForTheWorthy;
		b[3] = Anniversary;
		b[4] = DontStarve;
		b[5] = RemixWorld;
		b[6] = NoTrapsWorld;
		b[7] = ZenithWorld;
		writer.Write(b);
		BitsByte b2 = (byte)0;
		b2[0] = HasCorruption;
		b2[1] = IsHardMode;
		b2[2] = DefeatedMoonlord;
		writer.Write(b2);
		writer.Write(Favorited);
	}

	private static SavedServerData FromReader(BinaryReader reader)
	{
		SavedServerData data = new SavedServerData();
		data.ServerName = reader.ReadString();
		data.ServerIP = reader.ReadString();
		data.ServerPort = reader.ReadInt32();
		data.WorldSizeX = reader.ReadUInt16();
		data.GameMode = reader.ReadByte();
		BitsByte b = reader.ReadByte();
		data.DrunkWorld = b[0];
		data.NotTheBees = b[1];
		data.ForTheWorthy = b[2];
		data.Anniversary = b[3];
		data.DontStarve = b[4];
		data.RemixWorld = b[5];
		data.NoTrapsWorld = b[6];
		data.ZenithWorld = b[7];
		BitsByte b2 = reader.ReadByte();
		data.HasCorruption = b2[0];
		data.IsHardMode = b2[1];
		data.DefeatedMoonlord = b2[2];
		data.Favorited = reader.ReadBoolean();
		if (string.IsNullOrEmpty(data.ServerName) || string.IsNullOrEmpty(data.ServerIP))
		{
			data.IsValid = false;
		}
		return data;
	}

	public void CopyToWorldFileData(WorldFileData data)
	{
		data.Name = ServerName;
		data.WorldSizeX = WorldSizeX;
		data.GameMode = GameMode;
		data.DrunkWorld = DrunkWorld;
		data.NotTheBees = NotTheBees;
		data.ForTheWorthy = ForTheWorthy;
		data.Anniversary = Anniversary;
		data.DontStarve = DontStarve;
		data.RemixWorld = RemixWorld;
		data.NoTrapsWorld = NoTrapsWorld;
		data.ZenithWorld = ZenithWorld;
		data.HasCorruption = HasCorruption;
		data.IsHardMode = IsHardMode;
		data.DefeatedMoonlord = DefeatedMoonlord;
	}

	public void FavoriteToggle()
	{
		Favorited = !Favorited;
		SaveRecentServers();
	}
}
