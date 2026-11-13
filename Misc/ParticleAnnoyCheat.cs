using System;
using System.ComponentModel;
using System.Numerics;
using Terraria.GameContent.Drawing;
using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;
using Terraria.UtilClient.Common.Helpers;
using Terraria.UtilClient.Common.Utility.Game;
using Terraria.UtilClient.Net;
using Terraria.UtilClient.Net.Protocol.Packets.Modules;

namespace Terraria.UtilClient.Cheats.Categories.Misc;

public sealed class ParticleAnnoyCheat : PrimitiveTimerCheat<ParticleAnnoyCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		public enum AnnoyMode : byte
		{
			Shimmer,
			Gassy,
			Lightning,
			RainbowLightning
		}

		public static readonly AnnoyMode[] Modes = Enum.GetValues<AnnoyMode>();

		public DefaultPlayerSelector[] ModeSelectors = new DefaultPlayerSelector[Modes.Length];

		public CheatSettings()
		{
			for (int i = 0; i < ModeSelectors.Length; i++)
			{
				ModeSelectors[i] = new DefaultPlayerSelector();
			}
		}
	}

	public override string Name => "Particle Annoy";

	public override CheatCategory Category => CheatCategory.Misc;

	public override bool RateConfigurable => true;

	protected override uint DefaultUpdateRate => 15u;

	protected override void OnUpdate()
	{
		CheatSettings.AnnoyMode[] modes = CheatSettings.Modes;
		foreach (CheatSettings.AnnoyMode mode in modes)
		{
			foreach (Player selectedPlayer in base.Settings.ModeSelectors[(uint)mode].GetSelectedPlayers())
			{
				ApplyMode(selectedPlayer, mode);
			}
		}
	}

	private static void ApplyMode(Player player, CheatSettings.AnnoyMode mode)
	{
		ParticlesModule packet = new ParticlesModule
		{
			ParticleType = GetParticleType(mode),
			Settings = CreateSettings(player, mode)
		};
		packet.Send();
		packet.HandleInternal();
	}

	private static ParticleOrchestraType GetParticleType(CheatSettings.AnnoyMode mode)
	{
		switch (mode)
		{
		case CheatSettings.AnnoyMode.Shimmer:
			return ParticleOrchestraType.ShimmerTownNPC;
		case CheatSettings.AnnoyMode.Gassy:
			return ParticleOrchestraType.GasTrap;
		case CheatSettings.AnnoyMode.Lightning:
		case CheatSettings.AnnoyMode.RainbowLightning:
			return ParticleOrchestraType.StormLightning;
		default:
			throw new InvalidEnumArgumentException("mode", (int)mode, typeof(CheatSettings.AnnoyMode));
		}
	}

	private static ParticleOrchestraSettings CreateSettings(Player player, CheatSettings.AnnoyMode mode)
	{
		if (mode - 2 <= CheatSettings.AnnoyMode.Gassy)
		{
			ParticleOrchestraSettings settings = new ParticleOrchestraSettings
			{
				PositionInWorld = player.Center + new Vector2(UMain.rand.NextSingle(-600f, 600f), UMain.rand.NextSingle(-50f, 50f)),
				MovementVector = new Vector2(UMain.rand.Next(int.MinValue, int.MaxValue), 0f),
				IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer
			};
			if (mode == CheatSettings.AnnoyMode.RainbowLightning)
			{
				settings.UniqueInfoPiece = UMain.rand.Next(int.MinValue, int.MaxValue);
			}
			return settings;
		}
		return new ParticleOrchestraSettings
		{
			PositionInWorld = player.Center,
			IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer
		};
	}
}
