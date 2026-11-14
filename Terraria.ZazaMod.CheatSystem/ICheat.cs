using System;

namespace Terraria.ZazaMod.CheatSystem;

public interface ICheat
{
	string Name { get; }

	string Description { get; }

	bool Enabled { get; set; }

	event Action OnEnabled;

	event Action OnDisabled;

	event Action OnTick;

	void Tick();
}
