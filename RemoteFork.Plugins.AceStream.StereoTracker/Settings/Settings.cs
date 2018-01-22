using Newtonsoft.Json;

namespace RemoteFork.Plugins.Settings {
    public class Settings {
        [JsonProperty(SettingsKey.SEPARATOR)]
        public char Separator { get; set; }

        [JsonProperty(SettingsKey.SETTINGS_VERSION)]
        public float SettingsVersion { get; set; }

        [JsonProperty(SettingsKey.PLUGIN_PATH)]
        public string PluginPath { get; set; }

        [JsonProperty(SettingsKey.TRACKER_SERVER_NNM)]
        public string TrackerServer { get; set; }

        [JsonProperty(SettingsKey.LOGO_NO_NAME_CLUB)]
        public string Logo { get; set; }

        [JsonProperty(SettingsKey.ICONS)]
        public Icons Icons { get; set; }

        [JsonProperty(SettingsKey.ACE_STREAM_API)]
        public AceStreamApi AceStreamApi { get; set; }

        [JsonProperty(SettingsKey.REGEXP)]
        public Regexp Regexp { get; set; }

        public static Settings DefaultSettings { get; } = new Settings() {
            SettingsVersion = 0,
            TrackerServer = "http://stereotraker.ru",
            PluginPath = "pluginPath",
            Separator = ';',

            Logo = "/templates/newfilmsx/images/logo.png",

            Icons = new Icons() {
                IcoError = "http://s1.iconbird.com/ico/0912/ToolbarIcons/w256h2561346685474SymbolError.png",
                IcoSearch = "http://s1.iconbird.com/ico/0612/MustHave/w256h2561339195991Search256x256.png",
                IcoFolder = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246folder.png",
                IcoNofile = "https://avatanplus.com/files/resources/mid/5788db3ecaa49155ee986d6e.png",
                IcoTorrentFile = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent2.png",
                IcoAduio = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597283musicfile.png",
                IcoVideo = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291videofile.png",
                IcoImage = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597278jpgfile.png",
                IcoOther = "http://s1.iconbird.com/ico/2013/6/364/w256h2561372348486helpfile256.png",
            },
            AceStreamApi = new AceStreamApi() {
                GetContentId = "http://api.torrentstream.net/upload/raw",
                GetMediaFiles = "{0}/server/api?method=get_media_files&content_id={1}",
                GetStream = "{0}/ace/getstream?id={1}",
            },

            Regexp = new Regexp() {
                GetPageFilmTorrent = "(id=)(\\d+)(\'>)",

                GetContentId = "({\"content_id\":\")(.*?)(\"})",

                GetSearchCenter = "(<div class=\"post-films\">)([\\s\\S]*?)(<div class=\"post-filmss\">)",
                GetSearchKinopoisk = "(?:<div class=\"left-btn-play\">)(\\s*?<img src=\")(.*?)(\\/>)",
                GetSearchName = "(title=\")(.*?)(\"\\s*\\/>)",
                GetSearchDataTopic = "(<a href=\"{full-link}\"><a href=\")(.*?)(\".*?>)(.*?)(<\\/a>)",
                GetSearchImage = "(<img src=\")(.*?)(\")",

                GetCategoryNextPage = "(<a href=\"{0})([\\w\\d\\/]*?page\\/\\d+\\/)(\">.....)(<\\/a>)",
                GetCategoryTopics = "(<div id=\'dle-content\'>)([\\s\\S]*?)(<div class=\"hblock\">)",
                GetCategoryTopic = "(<div class=\"post-films\">)([\\s\\S]*?)(<\\/div>\\s*<\\/div>)",
                GetCategoryTopicFilm = "(title=)(.*?)(\"\\s?\\/>[\\s\\S]*?<a href=\")(.*?)(\">)",

                GetCategoryMinitable = "(<table border=\"0\" id=\"atachment\")([\\s\\S]*?)(<\\/tr>)",
                GetCategorySubCategory = "(\\s*)(.*?)(<br>)",
                GetCategorySize = "(<strong>\\s*)(.*?)(<\\s*\\/strong>\\s*)(.*?)(<br>)",
                GetCategoryLeechers = "(title=\"Leechers\"><b>)(\\d+)(<)",
                GetCategorySeeders = "(title=\"Seeders\"><b>)(\\d+)(<)",
                GetCategoryTable = "(<table class=\"forumline forum\">)([\\s\\S]*?)(<\\/table>)"
            }
        };
    }

    public class AceStreamApi {
        [JsonProperty(SettingsKey.GET_CONTENT_ID)]
        public string GetContentId { get; set; }
        [JsonProperty(SettingsKey.GET_MEDIA_FILES)]
        public string GetMediaFiles { get; set; }
        [JsonProperty(SettingsKey.GET_STREAM)]
        public string GetStream { get; set; }
    }

    public class Regexp {
        [JsonProperty(SettingsKey.GET_PAGE_FILM_MAGNET)]
        public string GetPageFilmTorrent { get; set; }
        [JsonProperty(SettingsKey.GET_CONTENT_ID_TORRENT)]
        public string GetContentId { get; set; }

        [JsonProperty(SettingsKey.GET_PAGE_FILM_TORRENT)]
        public string GetSearchCenter { get; set; }
        [JsonProperty(SettingsKey.GET_SEARCH_DATA_TOPIC)]
        public string GetSearchDataTopic { get; set; }
        [JsonProperty(SettingsKey.GET_SEARCH_NAME)]
        public string GetSearchName { get; set; }
        [JsonProperty(SettingsKey.GET_SEARCH_KINOPOISK)]
        public string GetSearchKinopoisk { get; set; }
        [JsonProperty(SettingsKey.GET_SEARCH_IMAGE)]
        public string GetSearchImage { get; set; }

        [JsonProperty(SettingsKey.GET_CATEGORY_LEECHERS)]
        public string GetCategoryLeechers { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_SEEDERS)]
        public string GetCategorySeeders { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_SUB_CATEGORY)]
        public string GetCategorySubCategory { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_TABLE)]
        public string GetCategoryTable { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_NEXT_PAGE)]
        public string GetCategoryNextPage{ get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_TOPICS)]
        public string GetCategoryTopics { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_MINITABLE)]
        public string GetCategoryMinitable { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_TOPIC)]
        public string GetCategoryTopic { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_TOPIC_FILM)]
        public string GetCategoryTopicFilm { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_SIZE)]
        public string GetCategorySize{ get; set; }
    }

    public class Icons {
        [JsonProperty(SettingsKey.ICO_SEARCH)]
        public string IcoSearch { get; set; }
        [JsonProperty(SettingsKey.ICO_FOLDER)]
        public string IcoFolder { get; set; }
        [JsonProperty(SettingsKey.ICO_TORRENT_FILE)]
        public string IcoTorrentFile { get; set; }
        [JsonProperty(SettingsKey.ICO_NOFILE)]
        public string IcoNofile { get; set; }
        [JsonProperty(SettingsKey.ICO_ERROR)]
        public string IcoError { get; set; }
        [JsonProperty(SettingsKey.ICO_AUDIO)]
        public string IcoAduio { get; set; }
        [JsonProperty(SettingsKey.ICO_VIDEO)]
        public string IcoVideo { get; set; }
        [JsonProperty(SettingsKey.ICO_IMAGE)]
        public string IcoImage { get; set; }
        [JsonProperty(SettingsKey.ICO_OTHER)]
        public string IcoOther { get; set; }
    }
}
