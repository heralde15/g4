using System.Collections.Generic;

namespace Terraria.ZazaMod.WorldEdit;

public static class ListExt
{
	public static void RemoveUnorderedAt<T>(this List<T> list, int index)
	{
		list[index] = list[list.Count - 1];
		list.RemoveAt(list.Count - 1);
	}
}
