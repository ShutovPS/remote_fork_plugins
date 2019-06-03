using System;

namespace RemoteFork.Plugins.Settings {
    [Serializable]
    public class Settings {
        public string SettingsVersion = "1.1";

        public string PluginPath = "pluginPath";

        public Icons Icons = new Icons();

        public Links Links = new Links();

        public Regexp Regexp = new Regexp();

        public Authorization Authorization = new Authorization();
    }

    [Serializable]
    public class Regexp {
        public string GetSerials =
            "(<h2>[.\\s^&]*?<a href=\")((\\S*?)(\\/serial-(\\d+))(\\S*?))(\">[\\s>]*?Сериал)(.+?)(\\s*?(<span>|<\\/a>))";

        public string GetSerialName = "(itemprop=\"name\">\\s*)(.*?)(\\s*<\\/h1>)";
        public string GetSerialDescription = "(<p\\s+itemprop=\"description\">\\s*)(.*?)(\\s*<\\/p>)";
        public string SeasonData = "(data-id-season=\")(\\d+?)(\")";
        public string SeasonMiniData = "data-id-season=\"(.*?)\"";
        public string SerialData = "data-id-serial=\"(.*?)\"";
        public string GetSeries = "({)(\"title\"\\s*:\\s*\")(\\d+)(\\s+?)(.+?)(\")(.*?)(\"file\"\\s*:\\s*\")(.+?)(\")(.+?)(\"galabel\"\\s*:\\s*\")(.+?)(\")(.+?)(})";
        public string FileLink = @"(\\\/\\\/.*?=)";
        public string SerialInfo = "(\\/)(\\d+)(\\/)";
        public string SecureMark = "'(secureMark)': '(.*?)'.*?'time': (\\d+)";
    }

    [Serializable]
    public class Links {
        public string Site = "http://seasonvar.ru";
        public string PosterUrl = "http://cdn.seasonvar.ru/oblojka/{0}.jpg";
        public string IconUrl = "http://cdn.seasonvar.ru/oblojka/small/{0}.jpg";
    }

    [Serializable]
    public class Authorization {
        public string Login = "";
        public string Password = "";
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
    }
}
