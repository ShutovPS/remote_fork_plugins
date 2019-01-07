using System;
using System.Collections.Generic;

namespace RemoteFork.Plugins.Settings {
    [Serializable]
    public class Settings {
        public char Separator { get; set; }
        
        public float SettingsVersion { get; set; }
        
        public string PluginPath { get; set; }

        public List<string> IgnoreQualities { get; set; }

        public Encryption Encryption { get; set; }
        
        public Icons Icons { get; set; }
        
        public Links Links { get; set; }
        
        public Regexp Regexp { get; set; }

        public static Settings DefaultSettings { get; } = new Settings() {
            SettingsVersion = 0.2f,
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
                Site = "http://filmix.cc"
            },

            IgnoreQualities = new List<string>() { },

            Encryption = new Encryption() {
                Tokens = new List<string>() { "\\", "//Y2VyY2EudHJvdmEuc2FnZ2V6emE=", "//a2lub2NvdmVyLnc5OC5uamJo", "//c2ljYXJpby4yMi5tb3ZpZXM=" },
                Url = "https://gist.githubusercontent.com/ShutovPS/79f88cfb66f96edc81970ebec74206cc/raw/filmix.tokens",
            },

            Regexp = new Regexp() {
                FullDescription = "(<div class=\"short\">)([\\s\\S]*?)(<div class=\"panel-wrap\">)",
                Title = "(itemprop=\"name\" content=\")(.*?)(\")",
                Quality = "(<div class=\"quality\">\\s*)(.*?)(\\s*<\\/div>)",
                Translation = "(<div class=\"item translate\">.*?class=\"item-content\">\\s*)(.*?)(\\s*<\\/)",
                Description = "(itemprop=\"description\">\\s*)(.*?)(\\s*<\\/)",
                Link = "(itemprop=\"url\" href=\"\\s*.*?\\/)(\\d+)(-.*?\\s*\")",
                Poster = "(<img src=\"\\s*)(.*?)(\\s*\")",
                Category = "(<div class=\"item category\">.*?class=\"item-content\">\\s*)(.*?)(\\s*<\\/)",
                AddInfo = "(<span class=\"added-info\">\\s*)(.*?)(\\s*<)",
                NextPage = "(<a data-number=\")(\\d+)(\"\\s*href=\")(.*?)(\"\\s*class=\"next icon-arowRight)",
                FileQualityArray = "(.*?)(\\[.*?\\])(.*)",
                FileQuality = "[\\d\\w]+",
            }
        };
    }

    [Serializable]
    public class Regexp {
        public string FullDescription { get; set; }
        public string Title { get; set; }
        public string Quality { get; set; }
        public string Translation { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Poster { get; set; }
        public string Category { get; set; }
        public string AddInfo { get; set; }
        public string NextPage { get; set; }
        public string FileQualityArray { get; set; }
        public string FileQuality { get; set; }
    }

    [Serializable]
    public class Links {
        public string Site { get; set; }
    }

    [Serializable]
    public class Encryption {
        public string Url { get; set; }
        public List<string> Tokens { get; set; }
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
