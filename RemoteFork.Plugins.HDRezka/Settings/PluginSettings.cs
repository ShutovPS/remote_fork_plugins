using System.IO;
using System.Reflection;
using RemoteFork.Settings;

namespace RemoteFork.Plugins.Settings { 
    public class PluginSettings : AbstractSettings<PluginSettings, Settings> {
        static PluginSettings() {
            fileName = Path.Combine("Plugins", typeof(HDRezka).GetCustomAttribute<PluginAttribute>().Id + ".json");
            defaultSettings = Settings.DefaultSettings;
        }

        public PluginSettings() {
            if (defaultSettings.SettingsVersion > settingsManager.Settings.SettingsVersion) {
                settingsManager.Save(defaultSettings);
            }
        }
    }
}
