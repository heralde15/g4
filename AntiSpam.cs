using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Terraria.UtilClient.Chat;
using Terraria.UtilClient.Localization;

namespace Terraria.UtilClient.Features;

public static class AntiSpam
{
	private static readonly AntiSpamData?[] PlayerData = new AntiSpamData[255];

	internal static bool TemporarilyBlockProjDustAndLight = false;

	private static readonly FrozenDictionary<int, int> ProjectileWeights = new Dictionary<int, int> { [444] = 0 }.ToFrozenDictionary();

	private static AntiSpamData? Get(Player player)
	{
		if (!IsValidPlayer(player))
		{
			return null;
		}
		AntiSpamData data = PlayerData[player.whoAmI];
		if (data == null || data.Player != player)
		{
			return null;
		}
		return data;
	}

	private static AntiSpamData GetOrCreate(Player player)
	{
		ThrowIfInvalid(player);
		int index = player.whoAmI;
		AntiSpamData data = PlayerData[index];
		if (data != null && data.Player == player)
		{
			return data;
		}
		data = new AntiSpamData(player);
		PlayerData[index] = data;
		return data;
	}

	public static bool IsValidPlayer(Player player)
	{
		if (player == null)
		{
			return false;
		}
		int index = player.whoAmI;
		if (Main.player.IndexInRange(index) && index != Main.myPlayer && index != 255 && player.active && !player.InOpposingTeam(Main.LocalPlayer))
		{
			return Main.player[index] == player;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void ThrowIfInvalid(Player player)
	{
		ArgumentNullException.ThrowIfNull(player, "player");
		int index = player.whoAmI;
		if (!Main.player.IndexInRange(index))
		{
			throw new ArgumentOutOfRangeException("player", $"Player slot {index} is outside of Main.player");
		}
		if (index == Main.myPlayer)
		{
			throw new InvalidOperationException($"Player slot {index} is the local player");
		}
		if (index == 255)
		{
			throw new InvalidOperationException("Player slot 255 is not valid for AntiSpam.");
		}
		if (!player.active)
		{
			throw new InvalidOperationException($"Player slot {index} contains an inactive player");
		}
		if (Main.player[index] != player)
		{
			throw new InvalidOperationException($"Player object is not the current player at slot {index}");
		}
	}

	public static bool IsProjectileSpammer(Player player)
	{
		return Get(player)?.isProjectileSpammer ?? false;
	}

	public static bool IsSoundSpammer(Player player)
	{
		return Get(player)?.isSoundSpammer ?? false;
	}

	public static bool IsParticleSpammer(Player player)
	{
		return Get(player)?.isParticleSpammer ?? false;
	}

	private static int GetWeight(int projectileType)
	{
		return ProjectileWeights.GetValueOrDefault(projectileType, 1);
	}

	public static void AddProjectileSpam(Player player, int projectileType)
	{
		GetOrCreate(player).spammedProjectileCount += GetWeight(projectileType);
	}

	public static void AddSoundSpam(Player player, int amount = 1)
	{
		GetOrCreate(player).spammedSoundCount += amount;
	}

	public static void AddParticleSpam(Player player, int amount = 1)
	{
		GetOrCreate(player).spammedParticleCount += amount;
	}

	public static void Tick()
	{
		for (int i = 0; i < PlayerData.Length; i++)
		{
			AntiSpamData data = PlayerData[i];
			if (data != null)
			{
				Player player = data.Player;
				if (!IsValidPlayer(player))
				{
					PlayerData[i] = null;
				}
				else
				{
					Update(player, data);
				}
			}
		}
	}

	private static void Update(Player player, AntiSpamData data)
	{
		int projectileThreshold = Math.Max(4, 30 - data.projectileSpamFlags * 2);
		int soundThreshold = Math.Max(4, 30 - data.soundSpamFlags * 2);
		int particleThreshold = Math.Max(4, 30 - data.particleSpamFlags * 2);
		data.isProjectileSpammer = data.blockProjectileRecieveTime > 0 || data.spammedProjectileCount > projectileThreshold;
		data.isSoundSpammer = data.blockSoundRecieveTime > 0 || data.spammedSoundCount > soundThreshold;
		data.isParticleSpammer = data.blockParticleRecieveTime > 0 || data.spammedParticleCount > particleThreshold;
		data.spammedProjectileCount = Math.Max(0, data.spammedProjectileCount - 3);
		data.spammedSoundCount = Math.Max(0, data.spammedSoundCount - 3);
		data.spammedParticleCount = Math.Max(0, data.spammedParticleCount - 3);
		bool projectileOver = data.spammedProjectileCount > projectileThreshold;
		bool soundOver = data.spammedSoundCount > soundThreshold;
		bool particleOver = data.spammedParticleCount > particleThreshold;
		if (projectileOver && !data.wasProjectileOverThreshold)
		{
			data.projectileSpamFlags++;
			data.blockProjectileRecieveTime = 300 + data.projectileSpamFlags * 60;
			FlagMessage(player, "Projectile");
		}
		if (soundOver && !data.wasSoundOverThreshold)
		{
			data.soundSpamFlags++;
			data.blockSoundRecieveTime = 300 + data.soundSpamFlags * 60;
			FlagMessage(player, "Sound");
		}
		if (particleOver && !data.wasParticleOverThreshold)
		{
			data.particleSpamFlags++;
			data.blockParticleRecieveTime = 300 + data.particleSpamFlags * 60;
			FlagMessage(player, "Particle");
		}
		data.wasProjectileOverThreshold = projectileOver;
		data.wasSoundOverThreshold = soundOver;
		data.wasParticleOverThreshold = particleOver;
		if (data.blockProjectileRecieveTime > 0)
		{
			data.blockProjectileRecieveTime--;
		}
		if (data.blockSoundRecieveTime > 0)
		{
			data.blockSoundRecieveTime--;
		}
		if (data.blockParticleRecieveTime > 0)
		{
			data.blockParticleRecieveTime--;
		}
		data.flagDecayTimer = Math.Min(600, data.flagDecayTimer + 1);
		if (data.flagDecayTimer >= 600)
		{
			data.flagDecayTimer = 0;
			data.projectileSpamFlags = Math.Max(0, data.projectileSpamFlags - 1);
			data.soundSpamFlags = Math.Max(0, data.soundSpamFlags - 1);
			data.particleSpamFlags = Math.Max(0, data.particleSpamFlags - 1);
		}
	}

	private static void FlagMessage(Player player, string typeKey)
	{
		ChatLogging.LogInfo(ULang.CommonVal("AntiSpamTrigger", ULang.CommonVal(typeKey), player.name, ULang.TryGetPluralVal("UtilClientCommon." + typeKey)));
	}
}
