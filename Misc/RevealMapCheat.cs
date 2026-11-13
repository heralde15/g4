using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ReLogic.Threading;
using Terraria.UtilClient.Chat;
using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;
using Terraria.UtilClient.Common.Helpers.Game;
using Terraria.UtilClient.Localization;

namespace Terraria.UtilClient.Cheats.Categories.Misc;

public sealed class RevealMapCheat : AsyncRunActionCheat<RevealMapCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		[Range(1, 500)]
		public int SectionRequestDelayMs = 20;

		public float RestartTimeSeconds = 7.5f;
	}

	public override string Name => "Reveal Map";

	public override CheatCategory Category => CheatCategory.Misc;

	public override string StopMessage
	{
		get
		{
			if (Main.netMode != 1 || Props.UnloadedSectionCount < 1)
			{
				return ULang.CheatVal("MapRevealRefresh");
			}
			return ULang.CheatVal("MapRevealCancel");
		}
	}

	protected override async Task RunAction(CancellationToken cancellationToken)
	{
		if (Main.netMode != 1 || Props.UnloadedSectionCount <= 0)
		{
			UpdateAndRefreshMap();
			return;
		}
		ChatLogging.LogInfo(ULang.CheatVal("MapRevealBegin"));
		TimeSpan restartTime = TimeSpan.FromSeconds(base.Settings.RestartTimeSeconds);
		try
		{
			while (true)
			{
				cancellationToken.ThrowIfCancellationRequested();
				await NetHelper.RequestAllSections(base.Settings.SectionRequestDelayMs, cancellationToken);
				Stopwatch stopwatch = Stopwatch.StartNew();
				while (Props.UnloadedSectionCount > 0 && stopwatch.Elapsed < restartTime)
				{
					cancellationToken.ThrowIfCancellationRequested();
					await Task.Delay(10, cancellationToken);
				}
				if (Props.UnloadedSectionCount > 0)
				{
					UpdateAndRefreshMap(announceSuccess: false);
					continue;
				}
				break;
			}
		}
		catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
		{
		}
		UpdateAndRefreshMap();
	}

	private static void UpdateAndRefreshMap(bool announceSuccess = true)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		int mapEdge = Props.MapEdge;
		int bottom = Main.maxTilesY - mapEdge;
		FastParallel.For(mapEdge, Main.maxTilesX - mapEdge, (ParallelForAction)delegate(int xStart, int xEnd, object _)
		{
			for (int i = xStart; i < xEnd; i++)
			{
				if (Main.gameMenu)
				{
					break;
				}
				for (int j = mapEdge; j < bottom; j++)
				{
					Main.Map.UpdateLighting(i, j, byte.MaxValue);
				}
			}
		}, (object)null);
		Main.refreshMap = true;
		if (announceSuccess)
		{
			ChatLogging.LogInfo(ULang.CheatVal("MapRevealSuccess"));
		}
	}
}
