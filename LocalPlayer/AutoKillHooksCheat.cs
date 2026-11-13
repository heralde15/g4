using System;
using System.Collections.Generic;
using System.Numerics;
using Terraria.UtilClient.Cheats.Types;

namespace Terraria.UtilClient.Cheats.Categories.LocalPlayer;

public sealed class AutoKillHooksCheat : TickingCheat
{
	private static readonly Dictionary<int, float> HookSpeedCaps = new Dictionary<int, float>
	{
		{ 315, 14f },
		{ 487, 12f },
		{ 646, 16f },
		{ 647, 16f },
		{ 648, 16f },
		{ 649, 16f }
	};

	private static Vector2 _previousVelocity;

	public override string Name => "Auto Kill Hooks";

	public override CheatCategory Category => CheatCategory.LocalPlayer;

	public override bool SaveEnabled => false;

	protected override uint DefaultUpdateRate => 1u;

	protected override void OnUpdate()
	{
		Player player = Main.LocalPlayer;
		Vector2 velocity = player.velocity;
		float speed = velocity.Length();
		float acceleration = (velocity - _previousVelocity).Length();
		_previousVelocity = velocity;
		Projectile[] projectile = Main.projectile;
		foreach (Projectile proj in projectile)
		{
			if (proj.owner == player.whoAmI && proj.active && Main.projHook[proj.type])
			{
				float cap = HookSpeedCaps.GetValueOrDefault(proj.type, 11f);
				if ((Math.Abs(speed - cap) <= 0.2f && acceleration <= 0.001f && Vector2.Distance(player.Center, proj.Center) <= 48f) || Vector2.Distance(player.Center, proj.Center) <= 32f)
				{
					proj.Kill();
				}
			}
		}
	}
}
