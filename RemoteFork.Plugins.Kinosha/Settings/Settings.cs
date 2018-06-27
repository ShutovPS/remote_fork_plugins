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

        public static Settings DefaultSettings { get; } = new Settings() {
            SettingsVersion = 1.2f,
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
                Site = "http://kinosha.su",
                ApiSite = "http://api.kinosha.su",
            },

            Regexp = new Regexp() {
                NextUrl = "(<div class=\"next\"><a href=\")(.*?)(\"\\s?class=\"next\">)",
                Categories = "(<div class=\"col-xs-3-line item\")([\\s\\S]*?)(<div class=\"clearfix\">)",
                MiniDescription = "(<a href=\")(.*?)(\">)(.*?<img src=\")(.*?)(\")(.*?title=\')(.*?)(\'>)",
                SerialDescription = "(<em class=\'se\'>)(.*?)(<)(.*?<em class=\'ep\'>)(.*?)(<)",
                IdAndType = "(<div id=\"pl.*?data-type=\")(\\w+)(\".*?data-key=\")(\\d+)(\")",

                Quality = "(<span class=\"li link-cat\">)(.+?)(<\\/span>)",
                Description = "(<div class=\"discription\">)(.*?)(<\\/div>)",
                Title = "(class=\"title\">[\\s]*?<span>[\\s]*?)(\\S[\\s\\S]*?\\S)([\\s]*?<\\/span>)",

                Poster = "(<a href=\")(.*?)(\"\\s?class=\"hs\">)",
                DescriptionNs = "(<div class=\"film-disciption ns\">\\s*)(.*?)(\\s?<\\/div>)",
                QualitynNs = "(class=\"quality\">)(.*?)(<)",
                TitleNs = "(<h1 class=\"title\">[\\s]*?)(\\S[\\s\\S]*?\\S)([\\s]*?<\\/h1>)",
            }
        };
    }

    [Serializable]
    public class Regexp {
        public string Categories { get; set; }
        public string NextUrl { get; set; }
        public string MiniDescription { get; set; }
        public string SerialDescription { get; set; }
        public string IdAndType { get; set; }

        public string Quality { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }

        public string Poster { get; set; }
        public string DescriptionNs { get; set; }
        public string QualitynNs { get; set; }
        public string TitleNs { get; set; }
    }

    [Serializable]
    public class Links {
        public string Site { get; set; }
        public string ApiSite { get; set; }
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
