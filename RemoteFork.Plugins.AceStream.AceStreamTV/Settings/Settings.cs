using System;

namespace RemoteFork.Plugins.Settings {
    [Serializable]
    public class Settings {
        public char Separator { get; set; }
        
        public float SettingsVersion { get; set; }
        
        public string PluginPath { get; set; }
        
        public Icons Icons { get; set; }
        
        public AceStreamApi AceStreamApi { get; set; }
        
        public Links Links { get; set; }
        
        public Regexp Regexp { get; set; }

        public static Settings DefaultSettings { get; } = new Settings() {
            SettingsVersion = 1.0f,
            PluginPath = "pluginPath",
            Separator = ';',

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
                GetStreamById = "http://{0}:{1}/ace/getstream?id={2}",
                GetStreamManifest = "http://{0}:{1}/ace/manifest.m3u8?&infohash={2}",
                Search = "https://search.acestream.net/?method=search&api_version=1.0&api_key=test_api_key&query={0}&page_size=50&page={1}",
            },

            Links = new Links() {
                TrashTTV = "http://91.92.66.82/trash/ttv-list/",
                TvP2P = "http://tv-p2p.ru",
                AllfonTv = "http://allfon-tv.com",
            },

            Regexp = new Regexp() {
                InfoHash = "(?<=\"infohash\":\").*?(?=\")",
                Name = "(?<=\"name\":\").*?(?=\")",
                LoadPlayer = "(loadPlayer\\(\')([\\d\\w]*?)(\')",
                Title = "(?<=<title>).*?(?=&raquo;)",
                CategoryBody = "(<div class=\"modal-body category-body\">)([\\s\\S]+?)(<\\/div>)",
                Link = "(<a href=)(.*?)(\\/a>)",
                LinkName = "(\")(.*?)(\">)(.*?)(<)",
            }
        };
    }

    [Serializable]
    public class AceStreamApi {
        public string GetStreamById { get; set; }
        public string GetStreamManifest { get; set; }
        public string Search { get; set; }
    }

    [Serializable]
    public class Regexp {
        public string InfoHash { get; set; }
        public string Name { get; set; }
        public string LoadPlayer { get; set; }
        public string Title { get; set; }
        public string CategoryBody { get; set; }
        public string Link { get; set; }
        public string LinkName { get; set; }
    }

    [Serializable]
    public class Links {
        public string TrashTTV { get; set; }
        public string TvP2P { get; set; }
        public string AllfonTv { get; set; }
    }

    [Serializable]
    public class Icons {
        public string IcoSearch { get; set; }
        public string IcoFolder { get; set; }
        public string IcoTorrentFile { get; set; }
        public string IcoNofile { get; set; }
        public string IcoError { get; set; }
        public string IcoAduio { get; set; }
        public string IcoVideo { get; set; }
        public string IcoImage { get; set; }
        public string IcoOther { get; set; }
    }
}
