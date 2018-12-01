using System;

namespace RemoteFork.Plugins.Settings {
    [Serializable]
    public class Settings {
        public char Separator { get; set; }
        
        public float SettingsVersion { get; set; }
        
        public string PluginPath { get; set; }
        
        public Icons Icons { get; set; }
        
        public Links Links { get; set; }
        
        public Regexp Regexp { get; set; }

        public Encryption Encryption { get; set; }

        public static Settings DefaultSettings { get; } = new Settings() {
            SettingsVersion = 1.9f,
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
                NewVersion = "http://png.icons8.com/office/160/new.png",
            },

            Links = new Links() {
                Site = "http://hdrezka.ag",
                Moonwalk = "http://moonwalk.cc"
            },

            Encryption = new Encryption() {
                IV = "cdadf0b5b6373f1356240a050b885954",
                Key = "93deaf2d247d62b47376beb209f6128d03a60768198a2f4f7bd8e903ac5df65f",
                Url = "https://raw.githubusercontent.com/WendyH/PHP-Scripts/master/moon4crack.ini"
            },

            Regexp = new Regexp() {
                Iframe = "(<iframe.*?src=\")(.*?iframe)(.*?\")",
                Translations = "(translations:\\s*)(\\[\\[.*?\\]\\])",
                Translation = "(\\[)(\")(.*?)(\")(,\")(.*?)(\"\\])",
                Seasons = "(seasons:\\s\\[)(.*?)(\\])",
                Episodes = "(episodes:\\s\\[)(.*?)(\\])",

                MetaTitle = "(<meta property=\"og:title\" content=\")(.*?)([\"\\s\\/]*?>)",
                MetaImage = "(<meta property=\"og:image\" content=\")(.*?)([\"\\s\\/]*?>)",
                MiniDescription = "(<div class=\"b-post__description_text\">\\s*)(.*?)(<\\s*\\/div>)",

                Categories = "(<div class=\"b-content__inline_item\" data-id=\"\\d+\")([\\s\\S]*?)(<\\/div><\\/div>)",
                FilmUrl = "(<a href=\")(https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\\+.~#?&\\/=]*))(\"><span class=\"b-navigation__next i-sprt\">(&nbsp;)?<\\/span><\\/a>)",
                FullDescription = "(data-url=\")(.*?)(\">.*?<img src=\")(.*?)(\".*?<i class=\"entity\">)(.*?)(<\\/i>)((.*?<span class=\"info\">)(.*?)(<\\/span>))?(.*?.html\">)(.*?)(<\\/a>\\s*?<div>)(.*?)(<\\/div>)",

                Script = "(<script src=\")(.*?)(\">)",
                Host = "(host:\\s?\')(.*?)(\')",
                Proto = "(proto:\\s?\')(.*?)(\')",

                VideoToken = "(video_token:\\s*\')(.*?)(\')",
                PartnerId = "(partner_id:\\s*)(\\d+)",
                DomainId = "(domain_id:\\s*)(\\d+)",
                WindowId = "(window\\[\'[\\d\\w]+\'\\]\\s?=\\s?\')(.*?)(\')",
                Ref = "(ref:\\s*\\\')(.*?)(\\\')",

                M3U8 = "(\"m3u8\":\\s*\")(.*?)(\")",
                ExtList = "(#EXT-X.*?=)(\\d+x\\d+)([\\s\\S]*?)(http:.*)",

                Redirect = "(<a href=\")(http.+?iframe)(.*?\">)",

                Ini = "({0}\\s*\\=\\s*\\\")([\\d\\w]+)(\\\")",
            }
        };
    }

    [Serializable]
    public class Encryption {
        public string IV { get; set; }
        public string Key { get; set; }
        public string Url { get; set; }
    }

    [Serializable]
    public class Regexp {
        public string Iframe { get; set; }
        public string Translations { get; set; }
        public string Translation { get; set; }
        public string Seasons { get; set; }
        public string Episodes { get; set; }

        public string MetaTitle { get; set; }
        public string MetaImage { get; set; }
        public string MiniDescription { get; set; }

        public string Categories { get; set; }
        public string FilmUrl { get; set; }
        public string FullDescription { get; set; }

        public string Script { get; set; }
        public string Host { get; set; }
        public string Proto { get; set; }
        public string VideoToken { get; set; }
        public string PartnerId { get; set; }
        public string DomainId { get; set; }
        public string WindowId { get; set; }
        public string Ref { get; set; }

        public string M3U8 { get; set; }
        public string ExtList { get; set; }

        public string Redirect { get; set; }

        public string Ini { get; set; }
    }

    [Serializable]
    public class Links {
        public string Site { get; set; }
        public string Moonwalk { get; set; }
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
        public string NewVersion { get; set; }
    }
}
