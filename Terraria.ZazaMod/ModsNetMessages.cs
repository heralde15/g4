using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.ZazaMod;

public class ModsNetMessages
{
	public static ModsNetMessages declaration;

	static ModsNetMessages()
	{
		declaration = new ModsNetMessages();
	}

	public static void SenndPacket5(int whoAmI = 0, float inventorySlot = 0f, float perfix = 0f, float itemID = 0f)
	{
		if (Main.netMode == 0)
		{
			return;
		}
		int num = 256;
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
			writer.Write(5);
			writer.Write((byte)whoAmI);
			writer.Write((short)inventorySlot);
			_ = Main.player[whoAmI];
			writer.Write(69);
			writer.Write((byte)perfix);
			writer.Write((short)itemID);
			int num2 = (int)writer.BaseStream.Position;
			if (num2 > 65535)
			{
				throw new Exception("Maximum packet length exceeded. id: " + 5 + " length: " + num2);
			}
			writer.BaseStream.Position = position;
			writer.Write((ushort)num2);
			writer.BaseStream.Position = num2;
			if (Main.netMode == 1)
			{
				if (Netplay.Connection.Socket.IsConnected())
				{
					try
					{
						NetMessage.buffer[num].spamCount++;
						Main.ActiveNetDiagnosticsUI.CountSentMessage(5, num2);
						Netplay.Connection.Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num2, Netplay.Connection.ClientWriteCallBack);
					}
					catch
					{
					}
				}
			}
			else
			{
				for (int i = 0; i < 256; i++)
				{
					if (i == -1)
					{
						continue;
					}
					if (!NetMessage.buffer[i].broadcast)
					{
						if (Netplay.Clients[i].State >= 3)
						{
						}
					}
					else if (Netplay.Clients[i].IsConnected())
					{
						try
						{
							NetMessage.buffer[i].spamCount++;
							Main.ActiveNetDiagnosticsUI.CountSentMessage(5, num2);
							Netplay.Clients[i].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num2, Netplay.Clients[i].ServerWriteCallBack);
						}
						catch
						{
						}
					}
				}
			}
			if (Main.verboseNetplay)
			{
				for (int j = 0; j < num2; j++)
				{
				}
				for (int k = 0; k < num2; k++)
				{
					_ = NetMessage.buffer[num].writeBuffer[k];
				}
			}
			NetMessage.buffer[num].writeLocked = false;
		}
	}

	public static void Packet5FuckDNSPY(int whoAmI = 0, float inventorySlot = 0f, float perfix = 0f, float itemID = 0f)
	{
		if (Main.netMode == 0)
		{
			return;
		}
		int num = 256;
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
			writer.Write(5);
			writer.Write((byte)whoAmI);
			writer.Write((short)inventorySlot);
			_ = Main.player[whoAmI];
			writer.Write(69);
			writer.Write((byte)perfix);
			writer.Write((short)itemID);
			int num2 = (int)writer.BaseStream.Position;
			if (num2 > 65535)
			{
				throw new Exception("Maximum packet length exceeded. id: " + 5 + " length: " + num2);
			}
			writer.BaseStream.Position = position;
			writer.Write((ushort)num2);
			writer.BaseStream.Position = num2;
			if (Main.netMode == 1)
			{
				if (Netplay.Connection.Socket.IsConnected())
				{
					try
					{
						NetMessage.buffer[num].spamCount++;
						Main.ActiveNetDiagnosticsUI.CountSentMessage(5, num2);
						Netplay.Connection.Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num2, Netplay.Connection.ClientWriteCallBack);
					}
					catch
					{
					}
				}
			}
			else
			{
				for (int i = 0; i < 256; i++)
				{
					if (i == -1)
					{
						continue;
					}
					if (!NetMessage.buffer[i].broadcast)
					{
						if (Netplay.Clients[i].State >= 3)
						{
						}
					}
					else if (Netplay.Clients[i].IsConnected())
					{
						try
						{
							NetMessage.buffer[i].spamCount++;
							Main.ActiveNetDiagnosticsUI.CountSentMessage(5, num2);
							Netplay.Clients[i].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num2, Netplay.Clients[i].ServerWriteCallBack);
						}
						catch
						{
						}
					}
				}
			}
			if (Main.verboseNetplay)
			{
				for (int j = 0; j < num2; j++)
				{
				}
				for (int k = 0; k < num2; k++)
				{
					_ = NetMessage.buffer[num].writeBuffer[k];
				}
			}
			NetMessage.buffer[num].writeLocked = false;
		}
	}

	public static void SendModifiedData(int msgType, int remoteClient = -1, int ignoreClient = -1, NetworkText text = null, int number = 0, float number2 = 0f, float number3 = 0f, float number4 = 0f, int number5 = 0, int number6 = 0, int number7 = 0)
	{
		if (Main.netMode == 0)
		{
			return;
		}
		if (msgType == 21 && (Main.item[number].shimmerTime > 0f || Main.item[number].shimmered))
		{
			msgType = 145;
		}
		int num = 256;
		if (text == null)
		{
			text = NetworkText.Empty;
		}
		if (Main.netMode == 2 && remoteClient >= 0)
		{
			num = remoteClient;
		}
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
			writer.Write((byte)msgType);
			if (msgType == 5)
			{
				writer.Write((byte)number);
				writer.Write((short)number2);
				_ = Main.player[number];
				int num2 = 1;
				int num3 = (int)number4;
				if (num2 < 0)
				{
					num2 = 0;
				}
				writer.Write((short)num2);
				writer.Write((byte)number3);
				writer.Write((short)num3);
			}
			if (msgType == 147)
			{
				writer.Write((byte)Main.myPlayer);
				writer.Write((byte)69);
				NetMessage.WriteAccessoryVisibility(writer, Main.player[Main.myPlayer].hideVisibleAccessory);
			}
			int num4 = (int)writer.BaseStream.Position;
			if (num4 > 65535)
			{
				throw new Exception("Maximum packet length exceeded. id: " + msgType + " length: " + num4);
			}
			writer.BaseStream.Position = position;
			writer.Write((ushort)num4);
			writer.BaseStream.Position = num4;
			if (Main.netMode == 1)
			{
				if (Netplay.Connection.Socket.IsConnected())
				{
					try
					{
						NetMessage.buffer[num].spamCount++;
						Mods.quedPackets++;
						Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num4);
						Netplay.Connection.Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num4, Netplay.Connection.ClientWriteCallBack);
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
					for (int l = 0; l < 256; l++)
					{
						if (l != ignoreClient && NetMessage.buffer[l].broadcast && Netplay.Clients[l].IsConnected())
						{
							try
							{
								NetMessage.buffer[l].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num4);
								Netplay.Clients[l].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num4, Netplay.Clients[l].ServerWriteCallBack);
							}
							catch
							{
							}
						}
					}
					break;
				}
				case 20:
				{
					for (int j = 0; j < 256; j++)
					{
						if (j != ignoreClient && NetMessage.buffer[j].broadcast && Netplay.Clients[j].IsConnected() && Netplay.Clients[j].SectionRange((int)Math.Max(number3, number4), number, (int)number2))
						{
							try
							{
								NetMessage.buffer[j].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num4);
								Netplay.Clients[j].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num4, Netplay.Clients[j].ServerWriteCallBack);
							}
							catch
							{
							}
						}
					}
					break;
				}
				case 13:
				{
					for (int k = 0; k < 256; k++)
					{
						if (k != ignoreClient && NetMessage.buffer[k].broadcast && Netplay.Clients[k].IsConnected())
						{
							try
							{
								NetMessage.buffer[k].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num4);
								Netplay.Clients[k].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num4, Netplay.Clients[k].ServerWriteCallBack);
							}
							catch
							{
							}
						}
					}
					Main.player[number].netSkip++;
					if (Main.player[number].netSkip > 2)
					{
						Main.player[number].netSkip = 0;
					}
					break;
				}
				default:
				{
					for (int i = 0; i < 256; i++)
					{
						if (i != ignoreClient && (NetMessage.buffer[i].broadcast || (Netplay.Clients[i].State >= 3 && msgType == 10)) && Netplay.Clients[i].IsConnected())
						{
							try
							{
								NetMessage.buffer[i].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num4);
								Netplay.Clients[i].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num4, Netplay.Clients[i].ServerWriteCallBack);
							}
							catch
							{
							}
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
					NetMessage.buffer[remoteClient].spamCount++;
					Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num4);
					Netplay.Clients[remoteClient].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num4, Netplay.Clients[remoteClient].ServerWriteCallBack);
				}
				catch
				{
				}
			}
			if (Main.verboseNetplay)
			{
				for (int m = 0; m < num4; m++)
				{
				}
				for (int n = 0; n < num4; n++)
				{
					_ = NetMessage.buffer[num].writeBuffer[n];
				}
			}
			NetMessage.buffer[num].writeLocked = false;
			if (msgType == 2 && Main.netMode == 2)
			{
				Netplay.Clients[num].PendingTermination = true;
				Netplay.Clients[num].PendingTerminationApproved = true;
			}
		}
	}

	public static void SendModifiedDataProj(int msgType, int remoteClient = -1, int ignoreClient = -1, NetworkText text = null, int number = 0, Vector2 vector1 = default(Vector2), Vector2 vector2 = default(Vector2), float number4 = 0f, int number5 = 0, int number6 = 0, int number7 = 0)
	{
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 0)
		{
			return;
		}
		if (msgType == 21 && (Main.item[number].shimmerTime > 0f || Main.item[number].shimmered))
		{
			msgType = 145;
		}
		int num = 256;
		if (text == null)
		{
			text = NetworkText.Empty;
		}
		if (Main.netMode == 2 && remoteClient >= 0)
		{
			num = remoteClient;
		}
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
			writer.Write((byte)msgType);
			if (msgType == 27)
			{
				number = 1000;
				for (int i = 0; i < 1000; i++)
				{
					if (!Main.projectile[i].active)
					{
						number = i;
						break;
					}
				}
				if (num == 1000)
				{
					number = Projectile.FindOldestProjectile();
				}
				if (number4 == -1f)
				{
					number4 = Main.myPlayer;
				}
				writer.Write((short)number);
				writer.WriteVector2(vector1);
				writer.WriteVector2(vector2);
				writer.Write((byte)number4);
				writer.Write((short)number5);
				Projectile projectile = new Projectile();
				projectile.SetDefaults(1);
				BitsByte bitsByte = (byte)0;
				BitsByte bitsByte2 = (byte)0;
				bitsByte[0] = projectile.ai[0] != 0f;
				bitsByte[1] = projectile.ai[1] != 0f;
				bitsByte2[0] = projectile.ai[2] != 0f;
				if (projectile.bannerIdToRespondTo != 0)
				{
					bitsByte[3] = true;
				}
				if (projectile.damage != 0)
				{
					bitsByte[4] = true;
				}
				if (projectile.knockBack != 0f)
				{
					bitsByte[5] = true;
				}
				if (projectile.type > 0 && projectile.type < ProjectileID.Count && ProjectileID.Sets.NeedsUUID[projectile.type])
				{
					bitsByte[7] = true;
				}
				if (projectile.originalDamage != 0)
				{
					bitsByte[6] = true;
				}
				if ((byte)bitsByte2 != 0)
				{
					bitsByte[2] = true;
				}
				writer.Write(bitsByte);
				if (bitsByte[2])
				{
					writer.Write(bitsByte2);
				}
				if (bitsByte[0])
				{
					writer.Write(projectile.ai[0]);
				}
				if (bitsByte[1])
				{
					writer.Write(projectile.ai[1]);
				}
				if (bitsByte[3])
				{
					writer.Write((ushort)projectile.bannerIdToRespondTo);
				}
				if (bitsByte[4])
				{
					writer.Write((short)projectile.damage);
				}
				if (bitsByte[5])
				{
					writer.Write(projectile.knockBack);
				}
				if (bitsByte[6])
				{
					writer.Write((short)projectile.originalDamage);
				}
				if (bitsByte[7])
				{
					writer.Write((short)projectile.projUUID);
				}
				if (bitsByte2[0])
				{
					writer.Write(projectile.ai[2]);
				}
			}
			int num2 = (int)writer.BaseStream.Position;
			if (num2 > 65535)
			{
				throw new Exception("Maximum packet length exceeded. id: " + msgType + " length: " + num2);
			}
			writer.BaseStream.Position = position;
			writer.Write((ushort)num2);
			writer.BaseStream.Position = num2;
			if (Main.netMode == 1)
			{
				if (Netplay.Connection.Socket.IsConnected())
				{
					try
					{
						NetMessage.buffer[num].spamCount++;
						Mods.quedPackets++;
						Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num2);
						Netplay.Connection.Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num2, Netplay.Connection.ClientWriteCallBack);
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
					for (int k = 0; k < 256; k++)
					{
						if (k != ignoreClient && NetMessage.buffer[k].broadcast && Netplay.Clients[k].IsConnected())
						{
							try
							{
								NetMessage.buffer[k].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num2);
								Netplay.Clients[k].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num2, Netplay.Clients[k].ServerWriteCallBack);
							}
							catch
							{
							}
						}
					}
					break;
				}
				case 13:
				{
					for (int l = 0; l < 256; l++)
					{
						if (l != ignoreClient && NetMessage.buffer[l].broadcast && Netplay.Clients[l].IsConnected())
						{
							try
							{
								NetMessage.buffer[l].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num2);
								Netplay.Clients[l].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num2, Netplay.Clients[l].ServerWriteCallBack);
							}
							catch
							{
							}
						}
					}
					Main.player[number].netSkip++;
					if (Main.player[number].netSkip > 2)
					{
						Main.player[number].netSkip = 0;
					}
					break;
				}
				default:
				{
					for (int j = 0; j < 256; j++)
					{
						if (j != ignoreClient && (NetMessage.buffer[j].broadcast || (Netplay.Clients[j].State >= 3 && msgType == 10)) && Netplay.Clients[j].IsConnected())
						{
							try
							{
								NetMessage.buffer[j].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num2);
								Netplay.Clients[j].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num2, Netplay.Clients[j].ServerWriteCallBack);
							}
							catch
							{
							}
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
					NetMessage.buffer[remoteClient].spamCount++;
					Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num2);
					Netplay.Clients[remoteClient].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num2, Netplay.Clients[remoteClient].ServerWriteCallBack);
				}
				catch
				{
				}
			}
			if (Main.verboseNetplay)
			{
				for (int m = 0; m < num2; m++)
				{
				}
				for (int n = 0; n < num2; n++)
				{
					_ = NetMessage.buffer[num].writeBuffer[n];
				}
			}
			NetMessage.buffer[num].writeLocked = false;
			if (msgType == 2 && Main.netMode == 2)
			{
				Netplay.Clients[num].PendingTermination = true;
				Netplay.Clients[num].PendingTerminationApproved = true;
			}
		}
	}

	private static void DeezNuts(BinaryWriter writer, bool[] hideVisibleAccessory)
	{
		ushort num = 0;
		for (int i = 0; i < hideVisibleAccessory.Length; i++)
		{
			if (hideVisibleAccessory[i])
			{
				num |= (ushort)(1 << i);
			}
		}
		writer.Write(num);
	}

	public static void SendModifiedDataPlrSync(int msgType, int remoteClient = -1, int ignoreClient = -1, NetworkText text = null, int number = 0, Player player = null, string fakeName = "", float number4 = 0f, int number5 = 0, int number6 = 0, int number7 = 0)
	{
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 0)
		{
			return;
		}
		if (msgType == 21 && (Main.item[number].shimmerTime > 0f || Main.item[number].shimmered))
		{
			msgType = 145;
		}
		int num = 256;
		if (text == null)
		{
			text = NetworkText.Empty;
		}
		if (Main.netMode == 2 && remoteClient >= 0)
		{
			num = remoteClient;
		}
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
			writer.Write((byte)msgType);
			if (msgType == 4)
			{
				writer.Write((byte)number);
				writer.Write((byte)player.skinVariant);
				writer.Write((byte)player.hair);
				writer.Write((fakeName.Length > 0) ? fakeName : player.name);
				writer.Write(player.hairDye);
				DeezNuts(writer, player.hideVisibleAccessory);
				writer.Write(player.hideMisc);
				writer.WriteRGB(player.hairColor);
				writer.WriteRGB(player.skinColor);
				writer.WriteRGB(player.eyeColor);
				writer.WriteRGB(player.shirtColor);
				writer.WriteRGB(player.underShirtColor);
				writer.WriteRGB(player.pantsColor);
				writer.WriteRGB(player.shoeColor);
				BitsByte bitsByte = (byte)0;
				if (player.difficulty == 1)
				{
					bitsByte[0] = true;
				}
				else if (player.difficulty == 2)
				{
					bitsByte[1] = true;
				}
				else if (player.difficulty == 3)
				{
					bitsByte[3] = true;
				}
				bitsByte[2] = player.extraAccessory;
				writer.Write(bitsByte);
				BitsByte bitsByte2 = (byte)0;
				bitsByte2[0] = player.UsingBiomeTorches;
				bitsByte2[1] = player.happyFunTorchTime;
				bitsByte2[2] = player.unlockedBiomeTorches;
				bitsByte2[3] = player.unlockedSuperCart;
				bitsByte2[4] = player.enabledSuperCart;
				writer.Write(bitsByte2);
				BitsByte bitsByte3 = (byte)0;
				bitsByte3[0] = player.usedAegisCrystal;
				bitsByte3[1] = player.usedAegisFruit;
				bitsByte3[2] = player.usedArcaneCrystal;
				bitsByte3[3] = player.usedGalaxyPearl;
				bitsByte3[4] = player.usedGummyWorm;
				bitsByte3[5] = player.usedAmbrosia;
				bitsByte3[6] = player.ateArtisanBread;
				writer.Write(bitsByte3);
			}
			int num2 = (int)writer.BaseStream.Position;
			if (num2 > 65535)
			{
				throw new Exception("Maximum packet length exceeded. id: " + msgType + " length: " + num2);
			}
			writer.BaseStream.Position = position;
			writer.Write((ushort)num2);
			writer.BaseStream.Position = num2;
			if (Main.netMode == 1)
			{
				if (Netplay.Connection.Socket.IsConnected())
				{
					try
					{
						NetMessage.buffer[num].spamCount++;
						Mods.quedPackets++;
						Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num2);
						Netplay.Connection.Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num2, Netplay.Connection.ClientWriteCallBack);
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
					for (int j = 0; j < 256; j++)
					{
						if (j != ignoreClient && NetMessage.buffer[j].broadcast && Netplay.Clients[j].IsConnected())
						{
							try
							{
								NetMessage.buffer[j].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num2);
								Netplay.Clients[j].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num2, Netplay.Clients[j].ServerWriteCallBack);
							}
							catch
							{
							}
						}
					}
					break;
				}
				case 13:
				{
					for (int k = 0; k < 256; k++)
					{
						if (k != ignoreClient && NetMessage.buffer[k].broadcast && Netplay.Clients[k].IsConnected())
						{
							try
							{
								NetMessage.buffer[k].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num2);
								Netplay.Clients[k].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num2, Netplay.Clients[k].ServerWriteCallBack);
							}
							catch
							{
							}
						}
					}
					Main.player[number].netSkip++;
					if (Main.player[number].netSkip > 2)
					{
						Main.player[number].netSkip = 0;
					}
					break;
				}
				default:
				{
					for (int i = 0; i < 256; i++)
					{
						if (i != ignoreClient && (NetMessage.buffer[i].broadcast || (Netplay.Clients[i].State >= 3 && msgType == 10)) && Netplay.Clients[i].IsConnected())
						{
							try
							{
								NetMessage.buffer[i].spamCount++;
								Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num2);
								Netplay.Clients[i].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num2, Netplay.Clients[i].ServerWriteCallBack);
							}
							catch
							{
							}
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
					NetMessage.buffer[remoteClient].spamCount++;
					Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, num2);
					Netplay.Clients[remoteClient].Socket.AsyncSend(NetMessage.buffer[num].writeBuffer, 0, num2, Netplay.Clients[remoteClient].ServerWriteCallBack);
				}
				catch
				{
				}
			}
			if (Main.verboseNetplay)
			{
				for (int l = 0; l < num2; l++)
				{
				}
				for (int m = 0; m < num2; m++)
				{
					_ = NetMessage.buffer[num].writeBuffer[m];
				}
			}
			NetMessage.buffer[num].writeLocked = false;
			if (msgType == 2 && Main.netMode == 2)
			{
				Netplay.Clients[num].PendingTermination = true;
				Netplay.Clients[num].PendingTerminationApproved = true;
			}
		}
	}

	public static void Packet20Modded(BinaryWriter binaryWriter, int tileX = 0, float tileY = 0f, float xSize = 0f, float ySize = 0f, int changeType = 0)
	{
		Item heldItem = Main.LocalPlayer.HeldItem;
		int num = tileX;
		int num2 = (int)tileY;
		int num3 = (int)xSize;
		if (num3 < 0)
		{
			num3 = 0;
		}
		int num4 = (int)ySize;
		if (num4 < 0)
		{
			num4 = 0;
		}
		if (num < num3)
		{
			num = num3;
		}
		if (num >= Main.maxTilesX + num3)
		{
			num = Main.maxTilesX - num3 - 1;
		}
		if (num2 < num4)
		{
			num2 = num4;
		}
		if (num2 >= Main.maxTilesY + num4)
		{
			num2 = Main.maxTilesY - num4 - 1;
		}
		binaryWriter.Write((short)num);
		binaryWriter.Write((short)num2);
		binaryWriter.Write((byte)num3);
		binaryWriter.Write((byte)num4);
		binaryWriter.Write((byte)changeType);
		for (int i = num; i < num + num3; i++)
		{
			for (int j = num2; j < num2 + num4; j++)
			{
				BitsByte bitsByte = (byte)0;
				BitsByte bitsByte2 = (byte)0;
				BitsByte bitsByte3 = (byte)0;
				byte b = 0;
				byte b2 = 0;
				Tile tile = Main.tile[i, j];
				bitsByte[0] = tile.active();
				bitsByte[2] = tile.wall > 0;
				bitsByte[3] = tile.liquid > 0 && Main.netMode == 2;
				bitsByte[4] = tile.wire();
				bitsByte[5] = tile.halfBrick();
				bitsByte[6] = tile.actuator();
				bitsByte[7] = tile.inActive();
				bitsByte2[0] = tile.wire2();
				bitsByte2[1] = tile.wire3();
				if (tile.active() && tile.color() > 0)
				{
					bitsByte2[2] = true;
					b = tile.color();
				}
				if (tile.wall > 0 && tile.wallColor() > 0)
				{
					bitsByte2[3] = true;
					b2 = tile.wallColor();
				}
				bitsByte2 = (byte)((byte)bitsByte2 + (byte)(tile.slope() << 4));
				bitsByte2[7] = tile.wire4();
				bitsByte3[0] = tile.fullbrightBlock();
				bitsByte3[1] = tile.fullbrightWall();
				bitsByte3[2] = tile.invisibleBlock();
				bitsByte3[3] = tile.invisibleWall();
				binaryWriter.Write(bitsByte);
				binaryWriter.Write(bitsByte2);
				binaryWriter.Write(bitsByte3);
				if (b > 0)
				{
					binaryWriter.Write(b);
				}
				if (b2 > 0)
				{
					binaryWriter.Write(b2);
				}
				if (heldItem.type == 498)
				{
					if (tile.active())
					{
						if (i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1)
						{
							tile.type = 31;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 2)
						{
							tile.type = 31;
							tile.frameX = 0;
							tile.frameY = 18;
						}
						if (j == num2 + 1 && i == num + 1)
						{
							tile.type = 31;
							tile.frameX = 18;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num + 1)
						{
							tile.type = 31;
							tile.frameX = 18;
							tile.frameY = 18;
						}
					}
					binaryWriter.Write(tile.type);
					if (Main.tileFrameImportant[tile.type])
					{
						binaryWriter.Write(tile.frameX);
						binaryWriter.Write(tile.frameY);
					}
				}
				if (heldItem.type == 1989)
				{
					if (tile.active())
					{
						if (i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1)
						{
							tile.type = 31;
							tile.frameX = 36;
							tile.frameY = 0;
						}
						if (j == num2 + 2)
						{
							tile.type = 31;
							tile.frameX = 36;
							tile.frameY = 18;
						}
						if (j == num2 + 1 && i == num + 1)
						{
							tile.type = 31;
							tile.frameX = 54;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num + 1)
						{
							tile.type = 31;
							tile.frameX = 54;
							tile.frameY = 18;
						}
					}
					binaryWriter.Write(tile.type);
					if (Main.tileFrameImportant[tile.type])
					{
						binaryWriter.Write(tile.frameX);
						binaryWriter.Write(tile.frameY);
					}
				}
				else if (heldItem.type == 4951)
				{
					if (tile.active())
					{
						if (j == num2 + 1 && i == num)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 && i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1 && i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num)
						{
							tile.type = 26;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 3 && i == num)
						{
							tile.type = 26;
							tile.frameX = 0;
							tile.frameY = 18;
						}
						if (j == num2 + 2 && i == num + 1)
						{
							tile.type = 26;
							tile.frameX = 18;
							tile.frameY = 0;
						}
						if (j == num2 + 3 && i == num + 1)
						{
							tile.type = 26;
							tile.frameX = 18;
							tile.frameY = 18;
						}
						if (j == num2 + 2 && i == num + 2)
						{
							tile.type = 26;
							tile.frameX = 36;
							tile.frameY = 0;
						}
						if (j == num2 + 3 && i == num + 2)
						{
							tile.type = 26;
							tile.frameX = 36;
							tile.frameY = 18;
						}
					}
					binaryWriter.Write(tile.type);
					if (Main.tileFrameImportant[tile.type])
					{
						binaryWriter.Write(tile.frameX);
						binaryWriter.Write(tile.frameY);
					}
				}
				else if (heldItem.type == 4875)
				{
					if (tile.active())
					{
						if (j == num2 && i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1 && i == num)
						{
							tile.type = 231;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num)
						{
							tile.type = 231;
							tile.frameX = 0;
							tile.frameY = 18;
						}
						if (j == num2 + 3 && i == num)
						{
							tile.type = 231;
							tile.frameX = 0;
							tile.frameY = 36;
						}
						if (j == num2 + 1 && i == num + 1)
						{
							tile.type = 231;
							tile.frameX = 18;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num + 1)
						{
							tile.type = 231;
							tile.frameX = 18;
							tile.frameY = 18;
						}
						if (j == num2 + 3 && i == num + 1)
						{
							tile.type = 231;
							tile.frameX = 18;
							tile.frameY = 36;
						}
						if (j == num2 + 1 && i == num + 2)
						{
							tile.type = 231;
							tile.frameX = 36;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num + 2)
						{
							tile.type = 231;
							tile.frameX = 36;
							tile.frameY = 18;
						}
						if (j == num2 + 3 && i == num + 2)
						{
							tile.type = 231;
							tile.frameX = 36;
							tile.frameY = 36;
						}
					}
					binaryWriter.Write(tile.type);
					if (Main.tileFrameImportant[tile.type])
					{
						binaryWriter.Write(tile.frameX);
						binaryWriter.Write(tile.frameY);
					}
				}
				else if (heldItem.type == 4921)
				{
					if (tile.active())
					{
						if (j == num2 && i == num + 1)
						{
							tile.type = 597;
							tile.frameX = 396;
							tile.frameY = 0;
						}
						if (j == num2 && i == num + 2)
						{
							tile.type = 597;
							tile.frameX = 414;
							tile.frameY = 0;
						}
						if (j == num2 + 1 && i == num)
						{
							tile.type = 10;
							tile.frameX = 36;
							tile.frameY = 612;
						}
						if (j == num2 + 2 && i == num)
						{
							tile.type = 10;
							tile.frameX = 36;
							tile.frameY = 612;
						}
						if (j == num2 + 3 && i == num)
						{
							tile.type = 10;
							tile.frameX = 36;
							tile.frameY = 612;
						}
						if (j == num2 + 1 && i == num + 1)
						{
							tile.type = 10;
							tile.frameX = 36;
							tile.frameY = 612;
						}
						if (j == num2 + 2 && i == num + 1)
						{
							tile.type = 10;
							tile.frameX = 36;
							tile.frameY = 612;
						}
						if (j == num2 + 3 && i == num + 1)
						{
							tile.type = 10;
							tile.frameX = 36;
							tile.frameY = 612;
						}
						if (j == num2 + 1 && i == num + 2)
						{
							tile.type = 10;
							tile.frameX = 36;
							tile.frameY = 612;
						}
						if (j == num2 + 2 && i == num + 2)
						{
							tile.type = 10;
							tile.frameX = 36;
							tile.frameY = 612;
						}
						if (j == num2 + 3 && i == num + 2)
						{
							tile.type = 10;
							tile.frameX = 36;
							tile.frameY = 612;
						}
					}
					binaryWriter.Write(tile.type);
					if (Main.tileFrameImportant[tile.type])
					{
						binaryWriter.Write(tile.frameX);
						binaryWriter.Write(tile.frameY);
					}
				}
				else if (heldItem.type == 3202)
				{
					if (tile.active())
					{
						if (i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1)
						{
							tile.type = 238;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 2)
						{
							tile.type = 238;
							tile.frameX = 0;
							tile.frameY = 18;
						}
						if (j == num2 + 1 && i == num + 1)
						{
							tile.type = 238;
							tile.frameX = 18;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num + 1)
						{
							tile.type = 238;
							tile.frameX = 18;
							tile.frameY = 18;
						}
					}
					binaryWriter.Write(tile.type);
					if (Main.tileFrameImportant[tile.type])
					{
						binaryWriter.Write(tile.frameX);
						binaryWriter.Write(tile.frameY);
					}
				}
				else if (heldItem.type == 4917)
				{
					if (tile.active())
					{
						if (j == num2 + 1 && i == num)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 && i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1 && i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num)
						{
							tile.type = 26;
							tile.frameX = 54;
							tile.frameY = 0;
						}
						if (j == num2 + 3 && i == num)
						{
							tile.type = 26;
							tile.frameX = 54;
							tile.frameY = 18;
						}
						if (j == num2 + 2 && i == num + 1)
						{
							tile.type = 26;
							tile.frameX = 72;
							tile.frameY = 0;
						}
						if (j == num2 + 3 && i == num + 1)
						{
							tile.type = 26;
							tile.frameX = 72;
							tile.frameY = 18;
						}
						if (j == num2 + 2 && i == num + 2)
						{
							tile.type = 26;
							tile.frameX = 90;
							tile.frameY = 0;
						}
						if (j == num2 + 3 && i == num + 2)
						{
							tile.type = 26;
							tile.frameX = 90;
							tile.frameY = 18;
						}
					}
					binaryWriter.Write(tile.type);
					if (Main.tileFrameImportant[tile.type])
					{
						binaryWriter.Write(tile.frameX);
						binaryWriter.Write(tile.frameY);
					}
				}
				if (heldItem.type == 4916)
				{
					if (tile.active())
					{
						if (i == num && j == num2 + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (i == num && j == num2 + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (i == num && j == num2 + 3)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (i == num + 1 && j == num2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (i == num + 1 && j == num2 + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (i == num + 1 && j == num2 + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (i == num + 1 && j == num2 + 3)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (i == num + 2 && j == num2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (i == num + 2 && j == num2 + 1)
						{
							tile.type = 388;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (i == num + 2 && j == num2 + 2)
						{
							tile.type = 388;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (i == num + 2 && j == num2 + 3)
						{
							tile.type = 388;
							tile.frameX = 0;
							tile.frameY = 0;
						}
					}
					binaryWriter.Write(tile.type);
					if (Main.tileFrameImportant[tile.type])
					{
						binaryWriter.Write(tile.frameX);
						binaryWriter.Write(tile.frameY);
					}
				}
				else if (heldItem.type == 4919)
				{
					if (tile.active())
					{
						if (j == num2 + 1 && i == num)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 3 && i == num)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 && i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1 && i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num + 2)
						{
							tile.type = 125;
							tile.frameX = 72;
							tile.frameY = 0;
						}
						if (j == num2 + 3 && i == num + 2)
						{
							tile.type = 125;
							tile.frameX = 72;
							tile.frameY = 18;
						}
						if (j == num2 + 2 && i == num + 1)
						{
							tile.type = 215;
							tile.frameX = 0;
							tile.frameY = -6969;
						}
						if (j == num2 + 3 && i == num + 1)
						{
							tile.type = 215;
							tile.frameX = 18;
							tile.frameY = -6969;
						}
					}
					binaryWriter.Write(tile.type);
					if (Main.tileFrameImportant[tile.type])
					{
						binaryWriter.Write(tile.frameX);
						binaryWriter.Write(tile.frameY);
					}
				}
				else if (heldItem.type == 4920)
				{
					if (tile.active())
					{
						if (j == num2 + 1 && i == num)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 3 && i == num)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 && i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1 && i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 3 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 3 && i == num + 1)
						{
							tile.type = 144;
							tile.frameX = 72;
							tile.frameY = 18;
						}
					}
					binaryWriter.Write(tile.type);
					if (Main.tileFrameImportant[tile.type])
					{
						binaryWriter.Write(tile.frameX);
						binaryWriter.Write(tile.frameY);
					}
				}
				else if (heldItem.type == 4918)
				{
					if (tile.active())
					{
						if (j == num2 + 1 && i == num)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 && i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1 && i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 1 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num + 2)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 2 && i == num + 1)
						{
							tile.type = 3;
							tile.frameX = 0;
							tile.frameY = 0;
						}
						if (j == num2 + 3 && i == num)
						{
							tile.type = 423;
							tile.frameX = 0;
							tile.frameY = 36;
						}
						if (j == num2 + 3 && i == num + 1)
						{
							tile.type = 423;
							tile.frameX = 0;
							tile.frameY = 36;
						}
						if (j == num2 + 3 && i == num + 2)
						{
							tile.type = 423;
							tile.frameX = 0;
							tile.frameY = 36;
						}
					}
					binaryWriter.Write(tile.type);
					if (Main.tileFrameImportant[tile.type])
					{
						binaryWriter.Write(tile.frameX);
						binaryWriter.Write(tile.frameY);
					}
				}
				if (tile.wall > 0)
				{
					binaryWriter.Write(tile.wall);
				}
				if (tile.liquid > 0 && Main.netMode == 2)
				{
					binaryWriter.Write(tile.liquid);
					binaryWriter.Write(tile.liquidType());
				}
			}
		}
	}
}
