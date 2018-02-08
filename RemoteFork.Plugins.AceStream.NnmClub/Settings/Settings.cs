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

        [JsonProperty(SettingsKey.ACE_STREAM_API)]
        public AceStreamApi AceStreamApi { get; set; }

        [JsonProperty(SettingsKey.REGEXP)]
        public Regexp Regexp { get; set; }

        public static Settings DefaultSettings { get; } = new Settings() {
            SettingsVersion = 1,
            TrackerServer = "https://nnmclub.to",
            PluginPath = "pluginPath",
            Separator = ';',

            Logo = "http://assets.nnm-club.ws/forum/images/logos/10let8.png",

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
                GetMediaFiles = "{0}/server/api?method=get_media_files&magnet={1}",
                GetStream = "{0}/ace/getstream?magnet={1}",
            },

            Regexp = new Regexp() {
                GetPageFilmMagnet = "(<a rel=\"nofollow\" href=\")(magnet.*?)(\")",

                GetRootCategories = "(<span class=\"genmed\" style=\"line-height)([\\s\\S]*?)(<\\/span>)",
                GetRootCategory = "(<a class=\"genmed\" href=\")(.*?)(\" title=\".*?\">)(.*?)(<\\/)",

                GetSearchProw = "(<tr class=\"prow)([\\s\\S]*?)(<\\/tr>)",
                GetSearchBB = "(\"><b>)(.*?)(</b>)",
                GetSearchBBA = "(\"><b>)(.*?)(<\\/b><\\/a>)",
                GetSearchGenmed = "(<a class=\"genmed \\w*\" href=\")(.*?)(\")",
                GetSearchSeedmed = "(class=\"seedmed\">)(.*?)(</td>)",
                GetSearchUTD = "(?<=</u>).*?(?=</td>)",
                GetSearchLeechmed = "(ass=\"leechmed\">)(.*?)(</td>)",

                GetCategoryHead = "(<td class=\"pcatHead\"><img class=\"picon\")([\\s\\S]*?)(><\\/table>)",
                GetCategoryLink = "(<a rel=\"nofollow\" href=\")(.*?)(\")",
                GetCategoryPortalImg = "(<var class=\"portalImg\".*?link=)(.*?)(\">)",
                GetCategoryNextPage = "(<a href=\")((portal\\.php\\?c=\\d+&)(?:amp;)(start=\\d+))(\">([а-яА-Я]*?.?)<\\/a><\\/span>)",
                GetCategoryTitle = "(<a class=\"pgenmed\".*?title=\")(.*?)(\">)",
                GetCategoryTitleA = "(<a class=\"pgenmed\".*?title=\")(.*?)(\">)",
                GetCategoryTitPims = "(<img)( class=\"tit-b pims\")( src=\")(.*?)(\".*?title=\")(.*?)(\".*?\"pcomm bold\">)(.*?)(<)",
                GetCategoryVarA = "(</var></a>)(.*?)(<br />)",
                GetCategoryBrB = "(<br \\/>)(<b>.*)(<\\/span>)"
            }
        };
    }

    public class AceStreamApi {
        [JsonProperty(SettingsKey.GET_MEDIA_FILES)]
        public string GetMediaFiles { get; set; }
        [JsonProperty(SettingsKey.GET_STREAM)]
        public string GetStream { get; set; }
    }

    public class Regexp {
        [JsonProperty(SettingsKey.GET_PAGE_FILM_MAGNET)]
        public string GetPageFilmMagnet { get; set; }

        [JsonProperty(SettingsKey.GET_ROOT_CATEGORIES)]
        public string GetRootCategories { get; set; }
        [JsonProperty(SettingsKey.GET_ROOT_CATEGORY)]
        public string GetRootCategory { get; set; }

        [JsonProperty(SettingsKey.GET_SEARCH_PROW)]
        public string GetSearchProw { get; set; }
        [JsonProperty(SettingsKey.GET_SEARCH_GENMED)]
        public string GetSearchGenmed { get; set; }
        [JsonProperty(SettingsKey.GET_SEARCH_BBA)]
        public string GetSearchBBA { get; set; }
        [JsonProperty(SettingsKey.GET_SEARCH_BB)]
        public string GetSearchBB { get; set; }
        [JsonProperty(SettingsKey.GET_SEARCH_UTD)]
        public string GetSearchUTD { get; set; }
        [JsonProperty(SettingsKey.GET_SEARCH_SEEDMED)]
        public string GetSearchSeedmed { get; set; }
        [JsonProperty(SettingsKey.GET_SEARCH_LEECHMED)]
        public string GetSearchLeechmed { get; set; }

        [JsonProperty(SettingsKey.GET_CATEGORY_HEAD)]
        public string GetCategoryHead { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_TITLE)]
        public string GetCategoryTitle { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_LINK)]
        public string GetCategoryLink { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_PORTAL_IMG)]
        public string GetCategoryPortalImg { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_NEXT_PAGE)]
        public string GetCategoryNextPage{ get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_TITLE_A)]
        public string GetCategoryTitleA { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_TIT_PIMS)]
        public string GetCategoryTitPims { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_VAR_A)]
        public string GetCategoryVarA { get; set; }
        [JsonProperty(SettingsKey.GET_CATEGORY_BR_B)]
        public string GetCategoryBrB{ get; set; }
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
