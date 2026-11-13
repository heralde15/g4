using System;

namespace SimpleHooks;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class GenerateHooksClassAttribute : Attribute
{
	public string HookClassName { get; }

	public GenerateHooksClassAttribute(string hookClassName)
	{
		HookClassName = hookClassName;
	}
}
