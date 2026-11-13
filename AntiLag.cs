using System;
using Terraria.Audio;
using Terraria.UtilClient.Chat;
using Terraria.UtilClient.Localization;
using Terraria.UtilClient.Tweaks;
using Terraria.UtilClient.Tweaks.Collections;

namespace Terraria.UtilClient.Features;

public static class AntiLag
{
	private static float AntiLagTimer;

	private static float LagCounter;

	private static bool DrawIsSlow => Main.RawDrawDeltaTime > (double)(FocusHelper.GameplayActive ? 1.05f : 1.075f);

	private static bool UpdateIsSlow => Main.UpdateDeltaTime > (double)(FocusHelper.GameplayActive ? 1.05f : 1.075f);

	public static bool Active => AntiLagTimer > 0f;

	public static bool StopProjectileDraw => AntiLagTimer > 2f;

	private static void AntiLagCheck()
	{
		if (Main.gameMenu)
		{
			if (UMain.TimeSinceWorldEntry.IsRunning)
			{
				UMain.TimeSinceWorldEntry.Reset();
			}
			LagCounter = 0f;
			return;
		}
		if (UMain.TimeSinceWorldEntry.Elapsed.TotalSeconds < 5.0)
		{
			return;
		}
		int num;
		float num2;
		if (!DrawIsSlow)
		{
			num = (UpdateIsSlow ? 1 : 0);
			if (num == 0)
			{
				num2 = 0f;
				goto IL_0070;
			}
		}
		else
		{
			num = 1;
		}
		num2 = Math.Max((float)Main.RawDrawDeltaTime, (float)Main.UpdateDeltaTime);
		goto IL_0070;
		IL_0070:
		float deltaAdd = num2;
		if (num != 0)
		{
			LagCounter += deltaAdd;
		}
		else
		{
			LagCounter -= 2f;
		}
		if (LagCounter < 0f)
		{
			LagCounter = 0f;
		}
		if (LagCounter > 20f && Main.netMode != 2)
		{
			TriggerAntiLag();
			LagCounter = 0f;
		}
	}

	public static void Tick()
	{
		if (TweaksCollectionExtensions.get_Current<PerformanceTweaks>().AntiLag)
		{
			AntiLagCheck();
			AntiLagTimer = MathF.Max(0f, AntiLagTimer - 1f / 60f);
		}
	}

	private static void TriggerAntiLag()
	{
		if (AntiLagTimer <= 0f)
		{
			ChatLogging.LogInfo(ULang.CommonVal("AntiLagTrigger"));
		}
		if (SoundEngine.SoundPlayer != null)
		{
			SoundEngine.SoundPlayer.StopAll();
		}
		for (int i = 0; i < Main.dust.Length; i++)
		{
			Main.dust[i].active = false;
		}
		for (int j = 0; j < Main.gore.Length; j++)
		{
			Main.gore[j].active = false;
		}
		AntiLagTimer = 5f;
	}
}
