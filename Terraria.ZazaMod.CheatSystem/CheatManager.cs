using System.Collections.Generic;

namespace Terraria.ZazaMod.CheatSystem;

public static class CheatManager
{
	private class CheatStorage<T> where T : ICheat
	{
		public static ushort Id;

		public static string Name;

		public static T Cheat;
	}

	private static Dictionary<ushort, ICheat> _cheatsById = new Dictionary<ushort, ICheat>();

	private static Dictionary<string, ICheat> _cheatsByName = new Dictionary<string, ICheat>();

	private static ushort _cheatsCount;

	private static bool _initialized;

	public static bool Initialized
	{
		get
		{
			return _initialized;
		}
		private set
		{
			_initialized = value;
		}
	}

	public static void Register<T>() where T : ICheat, new()
	{
		T val = (CheatStorage<T>.Cheat = new T());
		if (!TryGetCheatByName(val.Name, out var _))
		{
			CheatStorage<T>.Id = _cheatsCount;
			CheatStorage<T>.Name = val.Name;
			_cheatsById[_cheatsCount] = val;
			_cheatsByName[val.Name] = val;
			_cheatsCount++;
		}
	}

	public static T GetCheat<T>() where T : ICheat
	{
		return CheatStorage<T>.Cheat;
	}

	public static ushort GetCheatId<T>() where T : ICheat
	{
		return CheatStorage<T>.Id;
	}

	public static bool TryGetCheat(ushort id, out ICheat cheat)
	{
		return _cheatsById.TryGetValue(id, out cheat);
	}

	public static bool TryGetCheatByName(string cheatName, out ICheat cheat)
	{
		return _cheatsByName.TryGetValue(cheatName, out cheat);
	}

	public static bool IsCheatActive(string cheatName)
	{
		if (TryGetCheatByName(cheatName, out var cheat))
		{
			return cheat.Enabled;
		}
		return false;
	}

	public static void TickAllCheats()
	{
		foreach (ICheat cheat in _cheatsById.Values)
		{
			if (cheat.Enabled)
			{
				cheat.Tick();
			}
		}
	}

	public static List<ICheat> GetAllCheats()
	{
		List<ICheat> cheats = new List<ICheat>();
		foreach (ICheat cheat in _cheatsById.Values)
		{
			cheats.Add(cheat);
		}
		return cheats;
	}

	public static void Initialize()
	{
		if (!Initialized)
		{
			RegisterPlayerCheats();
			RegisterMovementCheats();
			RegisterInformationCheats();
			RegisterRenderingCheats();
			RegisterCosmeticCheats();
			RegisterMiscCheats();
			RegisterWorldEditCheats();
			RegisterHeldItemCheats();
			Initialized = true;
		}
	}

	private static void RegisterPlayerCheats()
	{
		Register<Cheats.GodmodeCheat>();
		Register<Cheats.AntiDeathCheat>();
		Register<Cheats.DodgeDamageCheat>();
		Register<Cheats.InstantRegenCheat>();
	}

	private static void RegisterMovementCheats()
	{
	}

	private static void RegisterInformationCheats()
	{
	}

	private static void RegisterRenderingCheats()
	{
	}

	private static void RegisterCosmeticCheats()
	{
	}

	private static void RegisterMiscCheats()
	{
		Register<Cheats.DisablePillarShadersCheat>();
		Register<Cheats.DisableGraveyardShaderCheat>();
		Register<Cheats.NoArrowGravityCheat>();
		Register<Cheats.TerraDiscsCheat>();
	}

	private static void RegisterWorldEditCheats()
	{
	}

	private static void RegisterHeldItemCheats()
	{
	}

	public static void Reload()
	{
		_cheatsById.Clear();
		_cheatsByName.Clear();
		_cheatsCount = 0;
		Initialize();
	}
}
