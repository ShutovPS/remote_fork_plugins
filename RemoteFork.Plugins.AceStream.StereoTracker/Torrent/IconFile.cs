using RemoteFork.Plugins.Settings;
using RemoteFork.Tools;

namespace RemoteFork.Plugins {
    public class IconFile {
        public static string GetIconFile(string name) {
            string type = MimeTypes.Get(name);
            if (type.Contains("audio")) {
                return PluginSettings.Settings.Icons.IcoAduio;
            } else if (type.Contains("video")) {
                return PluginSettings.Settings.Icons.IcoVideo;
            } else if (type.Contains("image")) {
                return PluginSettings.Settings.Icons.IcoImage;
            } else {
                return PluginSettings.Settings.Icons.IcoOther;
            }
        }
    }
}
