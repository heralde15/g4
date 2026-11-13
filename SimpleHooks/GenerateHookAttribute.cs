using System;

namespace SimpleHooks;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class GenerateHookAttribute : Attribute
{
}
