using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.UtilClient.Common.Helpers.Game;
using Terraria.UtilClient.Net;
using Terraria.UtilClient.Net.Protocol.Packets;

namespace Terraria.UtilClient.Cheats.Categories.Misc;

public static class ButcherHelpers
{
	public static int ButcherAll(bool auto, AutoButcherNPCsCheat.CheatSettings settings)
	{
		return ButcherAll(auto, settings.Filter, settings.IncludeInvincibleNPCs, settings.IncludeMultiPartNPCs);
	}

	public static int ButcherAll(bool auto, NPCHelper.NPCTypeFilter filter, bool allowInvincible, bool allowMultiPart)
	{
		int butcherCount = 0;
		HashSet<byte> butchered = new HashSet<byte>();
		for (int i = 0; i < Main.npc.Length; i++)
		{
			NPC npc = Main.npc[i];
			if (npc.IsAlive() && !butchered.Contains((byte)npc.whoAmI) && npc.MatchesFilter(filter, allowInvincible, allowMultiPart))
			{
				ButcherSingleNPC(npc, auto);
				butchered.Add((byte)npc.whoAmI);
				butcherCount++;
			}
		}
		if (butcherCount > 0)
		{
			NetHelper.SyncLocalPlayerControls();
		}
		return butcherCount;
	}

	internal static void ButcherSingleNPC(NPC npc, bool auto = false)
	{
		NetHelper.FakePlayerControlsPacket(npc);
		int loops = 0;
		int maxLoops = (auto ? 500 : 50);
		DamageNPCPacket packet = new DamageNPCPacket(npc);
		packet.Damage = 9999;
		packet.HitDirection = 0;
		packet.Crit = 1;
		using SerializedPacket<DamageNPCPacket> packet2 = packet.Serialized();
		while (npc.life > 0 && loops < maxLoops && !npc.immortal && ButcherStrike(npc, 9999, Main.LocalPlayer.direction))
		{
			if (Main.netMode == 1)
			{
				packet2.Send();
			}
			loops++;
		}
		if (auto)
		{
			if (npc.life <= 0)
			{
				npc.DeathSound = null;
			}
		}
		else
		{
			ButcherSound(npc);
		}
		if (npc.active && !auto)
		{
			npc.HitEffect();
		}
		if (npc.realLife >= 0)
		{
			Main.npc[npc.realLife].checkDead();
		}
		else
		{
			npc.checkDead();
		}
	}

	private static bool ButcherStrike(NPC npc, int Damage, int hitDirection, int owner = -1)
	{
		if (owner < 0)
		{
			owner = Main.myPlayer;
		}
		if (!npc.active || npc.life <= 0 || npc.immortal)
		{
			return false;
		}
		if (Main.onlyShimmerOceanWorlds && npc.type == 680 && npc.GivenName.Equals("Slimefors", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		NPC.SetPendingStrikeDamage(npc);
		if (Main.netMode == 0)
		{
			npc.PlayerInteraction(Main.myPlayer);
			NPCDamageTracker.AddDamage(npc, owner, npc.lifeMax);
			npc.life = -1;
			npc.HitEffect();
			int type = npc.type;
			if ((uint)(type - 396) <= 1u)
			{
				npc.ai[0] = -2f;
			}
			if (npc.type == 398)
			{
				npc.ai[0] = 2f;
			}
			bool flag;
			switch (npc.type)
			{
			case 422:
			case 493:
			case 507:
			case 517:
				flag = true;
				break;
			default:
				flag = false;
				break;
			}
			if (flag && npc.ai[2] != 1f)
			{
				npc.ai[2] = 1f;
			}
			if (npc.type == 548 && npc.ai[1] != 1f)
			{
				npc.ai[1] = 1f;
			}
			npc.checkDead();
			npc.active = false;
			NPC.ClearPendingStrikeDamage();
			return true;
		}
		npc.NetUpdateLowPriority();
		int actualDamage = Damage;
		actualDamage = (int)Main.CalculateDamageNPCsTake(actualDamage, npc.defense);
		actualDamage *= 2;
		if (npc.RedHatSkeletronAdjustmentsEnabled())
		{
			actualDamage = Math.Max(1, (int)((float)actualDamage * 0.7f));
		}
		if (npc.takenDamageMultiplier > 1f)
		{
			actualDamage = (int)((float)actualDamage * npc.takenDamageMultiplier);
		}
		npc.justHit = true;
		if (npc.townNPC)
		{
			bool flag = npc.aiStyle == 7;
			if (flag)
			{
				float num = npc.ai[0];
				bool flag2 = ((num == 3f || num == 4f || num == 16f || num == 17f) ? true : false);
				flag = flag2;
			}
			if (flag)
			{
				NPC npc2 = Main.npc[(int)npc.ai[2]];
				if (npc2.active)
				{
					npc2.ai[0] = 1f;
					npc2.ai[1] = 300 + UMain.rand.Next(300);
					npc2.ai[2] = 0f;
					npc2.localAI[3] = 0f;
					npc2.direction = hitDirection;
					npc2.netUpdate = true;
				}
			}
			npc.ai[0] = 1f;
			npc.ai[1] = 300 + UMain.rand.Next(300);
			npc.ai[2] = 0f;
			npc.localAI[3] = 0f;
			npc.direction = hitDirection;
			npc.netUpdate = true;
		}
		switch (npc.type)
		{
		case 371:
			actualDamage = 0;
			npc.ai[0] = 1f;
			npc.ai[1] = 4f;
			npc.dontTakeDamage = true;
			break;
		case 346:
			if ((double)npc.life >= (double)npc.lifeMax * 0.5 && (double)(npc.life - actualDamage) < (double)npc.lifeMax * 0.5)
			{
				Gore.NewGore(npc.position, npc.velocity, 517);
			}
			break;
		case 184:
		case 535:
			npc.localAI[0] = 60f;
			break;
		case 185:
			npc.localAI[0] = 1f;
			break;
		}
		if (npc.realLife >= 0)
		{
			NPC real = Main.npc[npc.realLife];
			NPC.AddStrikeDamage(real, actualDamage);
			real.life -= actualDamage;
			npc.life = real.life;
			npc.lifeMax = real.lifeMax;
		}
		else
		{
			NPC.AddStrikeDamage(npc, actualDamage);
			npc.life -= actualDamage;
		}
		NPC.ClearPendingStrikeDamage();
		return true;
	}

	private static void ButcherSound(NPC nPC)
	{
		if (nPC.type == 1)
		{
			switch ((int)nPC.ai[1])
			{
			case 5507:
				SoundEngine.PlaySound(SoundID.BalloonHurt, nPC.position);
				return;
			case 5502:
				SoundEngine.PlaySound(SoundID.CatHurt, nPC.position);
				return;
			case 5485:
				SoundEngine.PlaySound((UMain.rand.Next(5) == 0) ? SoundID.ChickenHurtRare : SoundID.ChickenHurt, nPC.position);
				return;
			case 5484:
				SoundEngine.PlaySound(SoundID.BellHurt, nPC.position);
				return;
			case 5506:
				SoundEngine.PlaySound(SoundID.CrowHurt, nPC.position);
				return;
			case 5503:
				SoundEngine.PlaySound(SoundID.DogHurt, nPC.position);
				return;
			case 5534:
				SoundEngine.PlaySound(SoundID.FairyHurt, nPC.position);
				return;
			case 5499:
				SoundEngine.PlaySound(SoundID.FrogHurt, nPC.position);
				return;
			case 5500:
				SoundEngine.PlaySound(SoundID.GoatHurt, nPC.position);
				return;
			case 5505:
				SoundEngine.PlaySound(SoundID.GoblinHurt, nPC.position);
				return;
			case 5501:
				SoundEngine.PlaySound(SoundID.RetroHurt, nPC.position);
				return;
			case 5504:
				SoundEngine.PlaySound(SoundID.TurkeyHurt, nPC.position);
				return;
			case 5508:
				SoundEngine.PlaySound(SoundID.UndeadHurt, nPC.position);
				return;
			case 5509:
				SoundEngine.PlaySound(SoundID.VampireHurt, nPC.position);
				return;
			}
			if (nPC.HitSound != null)
			{
				SoundEngine.PlaySound(nPC.HitSound, nPC.position);
			}
		}
		else if (nPC.HitSound != null)
		{
			SoundEngine.PlaySound(nPC.HitSound, nPC.position);
		}
	}
}
