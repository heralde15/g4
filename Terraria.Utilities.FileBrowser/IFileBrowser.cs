namespace Terraria.Utilities.FileBrowser;

public interface IFileBrowser
{
	string OpenFilePanel(string title, ExtensionFilter[] extensions, string requestedPath = null);

	string OpenFileSavePanel(string title, ExtensionFilter[] extensions, string requestedPath = null);
}
