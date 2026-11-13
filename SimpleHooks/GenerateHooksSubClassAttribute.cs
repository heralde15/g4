using System;

namespace SimpleHooks;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class GenerateHooksSubClassAttribute : Attribute
{
	public string HookSubClassName { get; }

	public GenerateHooksSubClassAttribute(string hookSubClassName)
	{
		HookSubClassName = hookSubClassName;
	}
}
