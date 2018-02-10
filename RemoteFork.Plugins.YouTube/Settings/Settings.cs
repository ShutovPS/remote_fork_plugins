using Newtonsoft.Json;

namespace RemoteFork.Plugins.Settings {
    public class Settings {
        [JsonProperty(SettingsKey.SEPARATOR)]
        public char Separator { get; set; }

        [JsonProperty(SettingsKey.SETTINGS_VERSION)]
        public float SettingsVersion { get; set; }

        [JsonProperty(SettingsKey.PLUGIN_PATH)]
        public string PluginPath { get; set; }

        [JsonProperty(SettingsKey.API_SERVER)]
        public string ApiServer { get; set; }

        [JsonProperty(SettingsKey.SITE_URL)]
        public string SiteUrl { get; set; }

        [JsonProperty(SettingsKey.LOGO)]
        public string Logo { get; set; }

        [JsonProperty(SettingsKey.ICONS)]
        public Icons Icons { get; set; }

        public static Settings DefaultSettings { get; } = new Settings() {
            SettingsVersion = 0.3f,
            ApiServer = "https://www.googleapis.com/youtube/v3",
            SiteUrl = "https://www.youtube.com",
            PluginPath = "pluginPath",
            Separator = ';',

            Logo = "http://pluspng.com/img-png/youtube-png-youtube-png-image-3567-300.png",

            Icons = new Icons() {
                IcoSearch = "http://s1.iconbird.com/ico/0612/MustHave/w256h2561339195991Search256x256.png",
                IcoFolder = "http://www.icons101.com/icon_png/size_128/id_80762/YouTube_Folder.png",
                IcoChannel = "http://www.icons101.com/icon_png/size_128/id_78792/Youtube.png",
                IcoVideo = "http://www.icons101.com/icon_png/size_128/id_78921/youtube.png",
                IcoVideoFile = "http://www.icons101.com/icon_png/size_128/id_52643/youtube.png",
                IcoPlaylist = "http://www.icons101.com/icon_png/size_128/id_73091/YoutubeIcon.png",
            },
        };
    }

    public class Icons {
        [JsonProperty(SettingsKey.ICO_SEARCH)]
        public string IcoSearch { get; set; }
        [JsonProperty(SettingsKey.ICO_FOLDER)]
        public string IcoFolder { get; set; }
        [JsonProperty(SettingsKey.ICO_CHANNEL)]
        public string IcoChannel { get; set; }
        [JsonProperty(SettingsKey.ICO_VIDEO)]
        public string IcoVideo { get; set; }
        [JsonProperty(SettingsKey.ICO_VIDEO_FILE)]
        public string IcoVideoFile { get; set; }
        [JsonProperty(SettingsKey.ICO_PLAYLIST)]
        public string IcoPlaylist { get; set; }
    }
}
