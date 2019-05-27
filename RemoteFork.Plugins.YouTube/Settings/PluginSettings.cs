using System;
using System.IO;
using System.Reflection;
using RemoteFork.Settings;

namespace RemoteFork.Plugins.Settings {
    public class PluginSettings : AbstractSettings<PluginSettings, Settings> {
        static PluginSettings() {
            fileName = Path.Combine("Plugins", typeof(YouTube).GetCustomAttribute<PluginAttribute>().Id + ".json");
            defaultSettings = new Settings();
        }

        public PluginSettings() {
            if (new Version(defaultSettings.SettingsVersion) > new Version(settingsManager.Settings.SettingsVersion)) {
                settingsManager.Save(defaultSettings);
            }
        }
    }
}
