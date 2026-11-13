namespace Terraria.UtilClient.Features;

public sealed class AntiSpamData
{
	internal readonly Player Player;

	public int spammedProjectileCount;

	public int spammedSoundCount;

	public int spammedParticleCount;

	public int blockProjectileRecieveTime;

	public int blockSoundRecieveTime;

	public int blockParticleRecieveTime;

	public int projectileSpamFlags;

	public int soundSpamFlags;

	public int particleSpamFlags;

	public bool wasProjectileOverThreshold;

	public bool wasSoundOverThreshold;

	public bool wasParticleOverThreshold;

	public int flagDecayTimer;

	public bool isProjectileSpammer;

	public bool isSoundSpammer;

	public bool isParticleSpammer;

	public AntiSpamData(Player player)
	{
		Player = player;
	}
}
