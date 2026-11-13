using System;
using System.CodeDom.Compiler;
using Terraria.UtilClient.Hooks;

namespace SimpleHooks;

[GeneratedCode("SimpleHooks", "1.0.0.0")]
public static class HookUtil
{
	public static bool InvokeHook<T0>(Action<HookResult, T0>? hook, T0 arg0)
	{
		HookResult result = new HookResult();
		hook?.Invoke(result, arg0);
		return result.Handled;
	}

	public static bool InvokeHook<T0, T1>(Action<HookResult, T0, T1>? hook, T0 arg0, T1 arg1)
	{
		HookResult result = new HookResult();
		hook?.Invoke(result, arg0, arg1);
		return result.Handled;
	}

	public static bool InvokeHook<T0, T1, T2>(Action<HookResult, T0, T1, T2>? hook, T0 arg0, T1 arg1, T2 arg2)
	{
		HookResult result = new HookResult();
		hook?.Invoke(result, arg0, arg1, arg2);
		return result.Handled;
	}

	public static bool InvokeHook<T0, T1, T2, T3>(Action<HookResult, T0, T1, T2, T3>? hook, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
	{
		HookResult result = new HookResult();
		hook?.Invoke(result, arg0, arg1, arg2, arg3);
		return result.Handled;
	}

	public static bool InvokeHook<T0, T1, T2, T3, T4, T5, T6, T7, T8>(Action<HookResult, T0, T1, T2, T3, T4, T5, T6, T7, T8>? hook, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
	{
		HookResult result = new HookResult();
		hook?.Invoke(result, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		return result.Handled;
	}

	public static bool InvokeHook<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<HookResult, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>? hook, T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
	{
		HookResult result = new HookResult();
		hook?.Invoke(result, arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
		return result.Handled;
	}
}
