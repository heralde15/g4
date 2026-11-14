using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Terraria.ZazaMod;

public class ServerAutoFinder
{
	public struct ServerSearchResult
	{
		public string WorldName;

		public IPAddress ServerAddress;

		public int ServerPort;

		public Stopwatch TimeSinceLastRecieved;

		public ushort WorldXSize;

		public int NumberOfPlayers;

		public int MaxPlayers;

		public short GameMode;

		public bool Corruption;

		public bool HardMode;

		public string HostName;

		public ServerSearchResult(string worldN, IPAddress serverIp, int port, ushort tilesX, int numPlayers, int maxPlayers, short difficulty, bool corrupt, bool hard, string host)
		{
			WorldName = worldN;
			ServerAddress = serverIp;
			ServerPort = port;
			TimeSinceLastRecieved = new Stopwatch();
			TimeSinceLastRecieved.Start();
			WorldXSize = tilesX;
			NumberOfPlayers = numPlayers;
			MaxPlayers = maxPlayers;
			GameMode = difficulty;
			Corruption = corrupt;
			HardMode = hard;
			HostName = host;
		}
	}

	public static UdpClient broadcastPort;

	public static Thread broadcastThread;

	public static List<ServerSearchResult> TempResults = new List<ServerSearchResult>();

	public static Stopwatch TimeSinceLastListUpdate = new Stopwatch();

	public static List<ServerSearchResult> Results = new List<ServerSearchResult>();

	public static bool abortBroadcastThread;

	public static void BroadcastRecieveThread()
	{
		abortBroadcastThread = false;
		if (TempResults.Count > 0)
		{
			TempResults.Clear();
		}
		Thread.Sleep(1000);
		while (broadcastPort == null)
		{
			broadcastPort = new UdpClient(8888);
		}
		while (true)
		{
			try
			{
				if (abortBroadcastThread)
				{
					abortBroadcastThread = false;
					break;
				}
				UdpReceiveResult result = broadcastPort.ReceiveAsync().Result;
				BinaryReader reader = new BinaryReader(new MemoryStream(result.Buffer));
				if (reader.ReadInt32() != 1010)
				{
					continue;
				}
				IPAddress address = result.RemoteEndPoint.Address;
				int port = reader.ReadInt32();
				string worldName = reader.ReadString();
				string hostName = reader.ReadString();
				ushort tilesX = reader.ReadUInt16();
				bool crimson = reader.ReadBoolean();
				short gameMode = (short)reader.ReadInt32();
				byte maxPlayers = reader.ReadByte();
				byte numPlayers = reader.ReadByte();
				bool hardmode = reader.ReadBoolean();
				lock (TempResults)
				{
					ServerSearchResult searchResult = new ServerSearchResult(worldName, address, port, tilesX, numPlayers, maxPlayers, gameMode, !crimson, hardmode, hostName);
					if (!TempResults.Contains(searchResult))
					{
						TempResults.Add(searchResult);
					}
				}
			}
			catch
			{
			}
		}
	}

	public static void StartSearching()
	{
		if (broadcastThread != null)
		{
			StopSearching();
		}
		broadcastThread = new Thread(BroadcastRecieveThread);
		broadcastThread.IsBackground = true;
		broadcastThread.Start();
	}

	public static void StopSearching()
	{
		if (broadcastThread != null)
		{
			abortBroadcastThread = true;
		}
		if (broadcastPort != null)
		{
			broadcastPort.Close();
			broadcastPort = null;
		}
		TempResults.Clear();
		Results.Clear();
		TimeSinceLastListUpdate.Stop();
	}

	public static void UpdateResults()
	{
		if (!TimeSinceLastListUpdate.IsRunning)
		{
			TimeSinceLastListUpdate.Restart();
		}
		if (!(TimeSinceLastListUpdate.Elapsed.TotalSeconds > 0.25))
		{
			return;
		}
		lock (TempResults)
		{
			Results.Clear();
			for (int i = TempResults.Count - 1; i >= 0; i--)
			{
				ServerSearchResult result = TempResults[i];
				if (result.TimeSinceLastRecieved.Elapsed.TotalSeconds <= 10.0)
				{
					Results.Add(result);
				}
				else
				{
					TempResults.RemoveAt(i);
				}
			}
		}
		TimeSinceLastListUpdate.Restart();
	}
}
