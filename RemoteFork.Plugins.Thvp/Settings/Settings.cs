using Newtonsoft.Json;

namespace RemoteFork.Plugins.Settings {
    public class Settings {
        [JsonProperty(SettingsKey.SEPARATOR)]
        public char Separator { get; set; }

        [JsonProperty(SettingsKey.SETTINGS_VERSION)]
        public float SettingsVersion { get; set; }

        [JsonProperty(SettingsKey.PLUGIN_PATH)]
        public string PluginPath { get; set; }

        [JsonProperty(SettingsKey.TRACKER_SERVER)]
        public string TrackerServer { get; set; }

        [JsonProperty(SettingsKey.LOGO)]
        public string Logo { get; set; }

        [JsonProperty(SettingsKey.ICONS)]
        public Icons Icons { get; set; }

        [JsonProperty(SettingsKey.REGEX_FILE_INFO)]
        public string RegexFileInfo { get; set; }

        public static Settings DefaultSettings { get; } = new Settings() {
            SettingsVersion = 0.1f,
            TrackerServer = "http://thvp.ru",
            PluginPath = "pluginPath",
            Separator = ';',

            Logo = "http://thvp.ru/img/logo.png",

            RegexFileInfo = "(#EXTINF:.*?,\\d*)(.*?)($)([\\s\\S]*?)(http:)([\\s\\S]*?)($)",

            Icons = new Icons() {
                IcoFolder = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246folder.png",
                IcoSearch = "http://s1.iconbird.com/ico/0612/MustHave/w48h481339195991Search48x48.png",
                IcoNofile = "http://s1.iconbird.com/ico/0512/48pxwebiconset/w48h481337349851Emptydocument.png",
            }
        };
    }
    
    public class Icons {
        [JsonProperty(SettingsKey.ICO_FOLDER)]
        public string IcoFolder { get; set; }
        [JsonProperty(SettingsKey.ICO_SEARCH)]
        public string IcoSearch { get; set; }
        [JsonProperty(SettingsKey.ICO_NOFILE)]
        public string IcoNofile { get; set; }
    }
}
