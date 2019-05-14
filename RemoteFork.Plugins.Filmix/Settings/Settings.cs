using System;
using System.Collections.Generic;

namespace RemoteFork.Plugins.Settings {
    [Serializable]
    public class Settings {
        public Version SettingsVersion = new Version(0, 5);

        public char Separator = ';';

        public string PluginPath = "pluginPath";

        public List<string> IgnoreQualities = new List<string>() { };

        public Encryption Encryption = new Encryption();

        public Autorization Auth = new Autorization();

        public Icons Icons = new Icons();

        public Links Links = new Links();

        public Regexp Regexp = new Regexp();
    }

    [Serializable]
    public class Regexp {
        public string FullDescription = "(<div class=\"short\">)([\\s\\S]*?)(<div class=\"panel-wrap\">)";

        public string Poster = "(<img src=\"\\s*)(.*?)(\\s*\")";
        public string Title = "(itemprop=\"name\"\\s*content=\")(.*?)(\")";
        public string TitleOriginal = "(itemprop=\"alternativeHeadline\"\\s*content=\")(.*?)(\")";
        public string Quality = "(<div class=\"quality\">\\s*)(.*?)(\\s*<\\/div>)";
        public string Translation = "(<div class=\"item translate\">.*?class=\"item-content\">\\s*)(.*?)(\\s*<\\/)";
        public string Description = "(itemprop=\"description\">\\s*)(.*?)(\\s*<\\/)";
        public string Categories = "(<div class=\"item category\">.*?class=\"item-content\">\\s*)(.*?)(\\s*<\\/div>)";
        public string Category = "([a-zA-Zà-ÿÀ-ß\\d\\s]+)(<\\/a>)?(,|<\\/span>)";
        public string AddInfo = "(<span class=\"added-info\">\\s*)(.*?)(\\s*<)";
        public string Year = "(<div class=\"item year\">.*?)(\\d{4})";

        public string Link = "(itemprop=\"url\" href=\"\\s*.*?\\/)(\\d+)(-.*?\\s*\")";

        public string NextPage = "(<a data-number=\")(\\d+)(\"\\s*href=\")(.*?)(\"\\s*class=\"next icon-arowRight)";
        public string FileQualityArray = "(\\[(.*?)\\])(.*)";
        public string FileQuality = "[\\d\\w]+";

        public string UserLogout = "\\?action\\=logout";
        public string UserAuthorized = "AUTHORIZED";
    }

    [Serializable]
    public class Links {
        public string Site = "http://filmix.cc";
    }

    [Serializable]
    public class Autorization {
        public string Login = "";
        public string Password = "";
    }

    [Serializable]
    public class Encryption {
        public List<string> Tokens = new List<string>()
            {"\\", "//Y2VyY2EudHJvdmEuc2FnZ2V6emE=", "//a2lub2NvdmVyLnc5OC5uamJo", "//c2ljYXJpby4yMi5tb3ZpZXM="};

        public string Url =
            "https://gist.githubusercontent.com/ShutovPS/79f88cfb66f96edc81970ebec74206cc/raw/filmix.tokens";
    }

    [Serializable]
    public class Icons {
        public string IcoError = "https://img.icons8.com/dusk/64/000000/close-window.png";
        public string IcoSearch = "https://img.icons8.com/color/48/000000/search.png";
        public string IcoFolder = "https://img.icons8.com/dusk/64/000000/opened-folder.png";
        public string IcoNofile = "https://img.icons8.com/dusk/64/000000/box-important.png";
        public string IcoTorrentFile = "https://img.icons8.com/dusk/64/000000/utorrent.png";
        public string IcoAduio = "https://img.icons8.com/bubbles/50/000000/music.png";
        public string IcoVideo = "https://img.icons8.com/dusk/64/000000/video-playlist.png";
        public string IcoImage = "https://img.icons8.com/dusk/64/000000/picture.png";
        public string IcoOther = "https://img.icons8.com/dusk/64/000000/new-file.png";
        public string IcoUpdate = "https://img.icons8.com/dusk/64/000000/approve-and-update.png";
        public string NewVersion = "https://png.icons8.com/office/160/new.png";
        public string User = "https://img.icons8.com/dusk/64/000000/add-user-male.png";
        public string Login = "https://img.icons8.com/dusk/64/000000/enter-2.png";
        public string Password = "https://img.icons8.com/dusk/64/000000/password.png";
    }
}
