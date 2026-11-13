using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework.Input;
using Terraria.ID;
using Terraria.UtilClient.Chat;
using Terraria.UtilClient.Cheats.Settings;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.Misc;

public sealed class AutoFishBotCheat : TickingCheat<AutoFishBotCheat.CheatSettings>
{
	public sealed class CheatSettings : USettings
	{
		public bool CatchFish = true;

		public bool CatchCrates = true;

		public bool CatchMisc = true;

		public bool CatchEnemies;

		public bool RandomizeCasting = true;

		public int MinCastDelay = 30;

		public int MaxCastDelay = 90;

		public bool UseFishingPos;
	}

	private const int MinimumFishingPower = 5;

	private bool _setupComplete;

	private int _castTimer;

	private int? _fishingRodSlot;

	private Vector2? _fishingPos;

	public override string Name => "Auto Fish";

	public override CheatCategory Category => CheatCategory.Misc;

	public override bool SaveEnabled => false;

	protected override uint DefaultUpdateRate => 1u;

	protected override void OnEnabledCustom()
	{
		_setupComplete = false;
		SetCastTimer();
	}

	protected override void OnDisabled()
	{
		_setupComplete = false;
		_castTimer = 0;
		_fishingRodSlot = null;
		_fishingPos = null;
	}

	protected override void OnUpdate()
	{
		if (!_setupComplete)
		{
			if (!TrySetup() || Main.LocalPlayer.selectedItem != _fishingRodSlot)
			{
				return;
			}
			_setupComplete = true;
		}
		if (!CheckConditions(out string reason))
		{
			ChatLogging.LogInfo(Name + " stopped:\n   " + reason + ".");
			SetEnabled(value: false);
			return;
		}
		if (base.Settings.UseFishingPos && _fishingPos.HasValue)
		{
			SetCursorPosition(_fishingPos.Value);
		}
		if (TryFindBobber(out var bobber))
		{
			if (bobber.ai[1] < 0f && ShouldReel(bobber))
			{
				UseSelectedItem();
			}
		}
		else if (_castTimer > 0)
		{
			_castTimer--;
		}
		else
		{
			UseSelectedItem();
			SetCastTimer();
		}
	}

	private bool TrySetup()
	{
		if (Main.gameMenu)
		{
			return false;
		}
		if (!TryFindBestFishingPole(Main.LocalPlayer, out _fishingRodSlot))
		{
			ChatLogging.LogError("Auto Fish could not find a fishing pole in the hotbar.");
			SetEnabled(value: false);
			return false;
		}
		Main.LocalPlayer.changeItem = _fishingRodSlot.Value;
		return true;
	}

	private static void UseSelectedItem()
	{
		Main.LocalPlayer.controlUseItem = true;
		Main.LocalPlayer.releaseUseItem = true;
		Main.LocalPlayer.ItemCheck();
		Main.LocalPlayer.controlUseItem = false;
		Main.LocalPlayer.releaseUseItem = true;
	}

	private static bool TryFindBestFishingPole(Player player, [NotNullWhen(true)] out int? slot)
	{
		slot = null;
		int bestPower = 0;
		for (int i = 0; i < 10; i++)
		{
			Item item = player.inventory[i];
			if (!item.IsAir && item.fishingPole >= 5 && item.fishingPole > bestPower)
			{
				bestPower = item.fishingPole;
				slot = i;
			}
		}
		return slot.HasValue;
	}

	private bool CheckConditions([NotNullWhen(false)] out string? reason)
	{
		if (Main.LocalPlayer.dead)
		{
			reason = "The player is dead";
		}
		else if (Main.LocalPlayer.GetFishingConditions().BaitPower <= 0)
		{
			reason = "Bait not found";
		}
		else if (Main.LocalPlayer.selectedItem != _fishingRodSlot)
		{
			reason = "Selected item is not a fishing rod";
		}
		else if (base.Settings.UseFishingPos && !_fishingPos.HasValue)
		{
			reason = "Fishing position is not set";
		}
		else
		{
			reason = null;
		}
		return reason == null;
	}

	private static bool TryFindBobber([NotNullWhen(true)] out Projectile bobber)
	{
		bobber = Main.projectile.FirstOrDefault((Projectile proj) => proj.active && proj.owner == Main.myPlayer && proj.bobber);
		return bobber != null;
	}

	private bool ShouldReel(Projectile bobber)
	{
		int fishingResult = (int)bobber.localAI[1];
		if (fishingResult == 0)
		{
			return false;
		}
		if (fishingResult < 0)
		{
			return base.Settings.CatchEnemies;
		}
		if (fishingResult >= ItemID.Count)
		{
			return base.Settings.CatchMisc;
		}
		if (ItemID.Sets.IsFishingCrate[fishingResult])
		{
			return base.Settings.CatchCrates;
		}
		if (ItemID.Sets.IsBasicFish[fishingResult] || ItemID.Sets.IsQuestFish[fishingResult])
		{
			return base.Settings.CatchFish;
		}
		return base.Settings.CatchMisc;
	}

	public void SetFishingPos()
	{
		_fishingPos = Main.MouseWorld;
	}

	private static void SetCursorPosition(Vector2 worldPos)
	{
		Vector2 screenPos = worldPos - Main.screenPosition;
		Mouse.SetPosition((int)screenPos.X, (int)screenPos.Y);
	}

	private void SetCastTimer()
	{
		if (!base.Settings.RandomizeCasting)
		{
			_castTimer = 0;
		}
		int min = Math.Max(0, base.Settings.MinCastDelay);
		int max = Math.Max(min, base.Settings.MaxCastDelay);
		_castTimer = UMain.rand.Next(min, max + 1);
	}
}
