using System.IO;
using System.Reflection;
using RemoteFork.Settings;

namespace RemoteFork.Plugins.Settings { 
    public class PluginSettings : AbstractSettings<PluginSettings, Settings> {
        static PluginSettings() {
            fileName = Path.Combine("Plugins", typeof(NnmClub).GetCustomAttribute<PluginAttribute>().Id + ".json");
            defaultSettings = Settings.DefaultSettings;
        }
    }
}
