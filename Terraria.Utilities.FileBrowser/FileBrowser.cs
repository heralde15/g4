namespace Terraria.Utilities.FileBrowser;

public class FileBrowser
{
	private static IFileBrowser _platformWrapper;

	static FileBrowser()
	{
		_platformWrapper = new NativeFileDialog();
	}

	public static string OpenFilePanel(string title, string extension, string requestedPath = null)
	{
		ExtensionFilter[] extensions = (string.IsNullOrEmpty(extension) ? null : new ExtensionFilter[1]
		{
			new ExtensionFilter("", extension)
		});
		return OpenFilePanel(title, extensions, requestedPath);
	}

	public static string OpenFilePanel(string title, ExtensionFilter[] extensions, string requestedPath = null)
	{
		return _platformWrapper.OpenFilePanel(title, extensions, requestedPath);
	}

	public static string OpenFileSavePanel(string title, string extension, string requestedPath = null)
	{
		ExtensionFilter[] extensions = (string.IsNullOrEmpty(extension) ? null : new ExtensionFilter[1]
		{
			new ExtensionFilter("", extension)
		});
		return OpenFileSavePanel(title, extensions, requestedPath);
	}

	public static string OpenFileSavePanel(string title, ExtensionFilter[] extensions, string requestedPath = null)
	{
		return _platformWrapper.OpenFileSavePanel(title, extensions, requestedPath);
	}
}
