using System.Linq;

namespace Terraria.Utilities.FileBrowser;

public class NativeFileDialog : IFileBrowser
{
	public string OpenFilePanel(string title, ExtensionFilter[] extensions, string requestedPath = null)
	{
		string[] value = extensions.SelectMany((ExtensionFilter x) => x.Extensions).ToArray();
		if (nativefiledialog.NFD_OpenDialog(string.Join(",", value), requestedPath, out var outPath) == nativefiledialog.nfdresult_t.NFD_OKAY)
		{
			return outPath;
		}
		return null;
	}

	public string OpenFileSavePanel(string title, ExtensionFilter[] extensions, string requestedPath = null)
	{
		string[] value = extensions.SelectMany((ExtensionFilter x) => x.Extensions).ToArray();
		if (nativefiledialog.NFD_SaveDialog(string.Join(",", value), requestedPath, out var outPath) == nativefiledialog.nfdresult_t.NFD_OKAY)
		{
			return outPath;
		}
		return null;
	}
}
