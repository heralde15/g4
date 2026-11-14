using System;
using System.Collections.Generic;

namespace Terraria.ZazaMod.CheatSystem;

public class Cheats
{
	public abstract class SimpleCheat : ICheat
	{
		private bool _enabled;

		public bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				_enabled = value;
				HandleToggled();
			}
		}

		public string Name { get; protected set; }

		public string Description { get; protected set; }

		public event Action OnEnabled;

		public event Action OnDisabled;

		public event Action OnTick;

		public SimpleCheat(string name = "", string description = "", bool enabledByDefault = false, Action onEnabled = null, Action onDisabled = null, Action onTick = null)
		{
			Name = name;
			Description = description;
			Enabled = enabledByDefault;
			if (onEnabled != null)
			{
				OnEnabled += onEnabled;
			}
			if (onDisabled != null)
			{
				OnDisabled += onDisabled;
			}
			if (onTick != null)
			{
				OnTick += onTick;
			}
		}

		public void Toggle()
		{
			_enabled = !_enabled;
			HandleToggled();
		}

		private void HandleToggled()
		{
			if (Enabled && OnEnabled != null)
			{
				OnEnabled();
			}
			else if (!Enabled && OnDisabled != null)
			{
				OnDisabled();
			}
		}

		public void Tick()
		{
			if (OnTick != null)
			{
				OnTick();
			}
		}
	}

	public abstract class ToggleCheat : ICheat
	{
		private int _state;

		private readonly int _minCycle;

		private readonly int _maxCycle = 3;

		public bool Enabled
		{
			get
			{
				return _state != 0;
			}
			set
			{
				throw new InvalidOperationException("Cannot set Enabled state of a ToggleCheat.");
			}
		}

		public int State
		{
			get
			{
				return _state;
			}
			set
			{
				if (value > _maxCycle)
				{
					throw new InvalidOperationException($"The value of {value} was greater than the maximum possible value of {_maxCycle} for the cheat.");
				}
				if (value < _minCycle)
				{
					throw new InvalidOperationException($"The value of {value} was less than the minimum possible value of {_minCycle} for the cheat.");
				}
				_state = value;
				HandleToggled();
			}
		}

		public string Name { get; protected set; }

		public string Description { get; protected set; }

		public event Action OnEnabled;

		public event Action OnDisabled;

		public event Action OnTick;

		public ToggleCheat(string name = "", string description = "", int defaultValue = 0, int minValue = 0, int maxValue = 3, Action onEnabled = null, Action onDisabled = null, Action onTick = null)
		{
			Name = name;
			Description = description;
			_state = defaultValue;
			if (minValue > maxValue)
			{
				throw new InvalidOperationException($"The minimum value of {minValue} for the cheat was greater than the maximum value of {maxValue}.");
			}
			if (defaultValue < minValue)
			{
				throw new InvalidOperationException($"The default value of {defaultValue} for the cheat was less than the minimum value of {minValue}.");
			}
			if (defaultValue > maxValue)
			{
				throw new InvalidOperationException($"The default value of {defaultValue} for the cheat was greater than the maximum value of {minValue}.");
			}
			_minCycle = minValue;
			_maxCycle = maxValue;
			if (onEnabled != null)
			{
				OnEnabled += onEnabled;
			}
			if (onDisabled != null)
			{
				OnDisabled += onDisabled;
			}
			if (onTick != null)
			{
				OnTick += onTick;
			}
		}

		public void CycleNext()
		{
			_state++;
			if (_state > _maxCycle)
			{
				_state = _minCycle;
			}
			HandleToggled();
		}

		public void CyclePrevious()
		{
			_state--;
			if (_state < _minCycle)
			{
				_state = _maxCycle;
			}
			HandleToggled();
		}

		private void HandleToggled()
		{
			if (Enabled && OnEnabled != null)
			{
				OnEnabled();
			}
			else if (!Enabled && OnDisabled != null)
			{
				OnDisabled();
			}
		}

		public void Tick()
		{
			if (OnTick != null)
			{
				OnTick();
			}
		}
	}

	public class NoArrowGravityCheat : SimpleCheat
	{
		public NoArrowGravityCheat()
		{
			base.Name = "No Arrow Gravity";
			base.Description = "Arrows fired by you have no gravity.";
		}
	}

	public class TerraDiscsCheat : SimpleCheat
	{
		public TerraDiscsCheat()
		{
			base.Name = "Terra Discs";
			base.Description = "Terra Beams will become Terra Discs.";
		}
	}

	public class DisablePillarShadersCheat : SimpleCheat
	{
		public DisablePillarShadersCheat()
		{
			base.Name = "Disable Pillar Shaders";
			base.Description = "Disables the shaders for the lunar pillars.";
		}
	}

	public class DisableGraveyardShaderCheat : SimpleCheat
	{
		public DisableGraveyardShaderCheat()
		{
			base.Name = "Disable Graveyard Shader";
			base.Description = "Disables the graveyard shader.";
		}
	}

	public class GodmodeCheat : SimpleCheat
	{
		public GodmodeCheat()
		{
			base.Name = "Godmode";
			base.Description = "You cannot die or take damage.";
		}
	}

	public class AntiDeathCheat : SimpleCheat
	{
		public AntiDeathCheat()
		{
			base.Name = "Anti-Death";
			base.Description = "You cannot die, but can still recieve damage.";
		}
	}

	public class DodgeDamageCheat : SimpleCheat
	{
		public DodgeDamageCheat()
		{
			base.Name = "Dodge All Damage";
			base.Description = "You cannot be hit.";
		}
	}

	public class InstantRegenCheat : SimpleCheat
	{
		public InstantRegenCheat()
		{
			base.Name = "Instant Regen";
			base.Description = "You will immediately regenerate health.";
		}
	}

	public static List<string> CheatNames = new List<string>
	{
		"Godmode", "Anti Death", "Dodge All Damage", "Instant Regen", "Infinite Mana", "Anti Debuff", "Journey Mode", "Instant Respawn", "Extended Pick-Up Range", "Extended Placement Range",
		"Infinite Minions", "Infinite Ammo", "Infinite Breath", "No Fall Damage", "Ignore Liquids", "Ignore Damaging Blocks", "Item Drop Mode", "Buff Loss Mode", "Unrestrict Rod Of Discord", "Enable Lock-On",
		"Suppress Horrified Debuff", "Suppress Creative Shock Debuff", "Ignore Crowd Control", "Cancel Any Buff", "Enable Extra Accessory Slots", "Equip Duplicate Accessories", "Grapple While Mounted", "Always PvP Deaths", "Free Crafting", "Craft Anywhere",
		"No Swap Restriction", "Hammer Anything", "Place Anywhere", "Always Explode on Hit", "Always Have Stardust Guardian", "Always Spirit Heal", "Always Spectre Wrath", "Always Nebula Boosters", "Force Dash Type", "Luck Override",
		"Disable & Reset All"
	};
}
