using System;
using System.IO;
using System.Reflection;
using RemoteFork.Settings;

namespace RemoteFork.Plugins.Settings { 
    public class PluginSettings : AbstractSettings<PluginSettings, Settings> {
        static PluginSettings() {
            fileName = Path.Combine("Plugins", typeof(Moonwalk).GetCustomAttribute<PluginAttribute>().Id + ".json");
            defaultSettings = new Settings();
        }

        public PluginSettings() {
            if (new Version(defaultSettings.Version) > new Version(settingsManager.Settings.Version)) {
                settingsManager.Save(defaultSettings);
            }
        }
    }
}
