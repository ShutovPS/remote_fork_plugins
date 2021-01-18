using System;

namespace RemoteFork.Plugins.Settings {
    [Serializable]
    public class Settings {
        public string SettingsVersion = "1.2";

        public string PluginPath = "pluginPath";

        public int CachingHours = 1;

        public Icons Icons = new Icons();

        public Links Links = new Links();

        public Regexp Regexp = new Regexp();

        public Authorization Authorization = new Authorization();
    }

    [Serializable]
    public class Regexp {
        public string GetAllSerials = "(<a data-id=\"(\\d+)\"\\s*?href=\"(.*?)\"\\s*?>)([\\s\\S]*?)(<\\/a>)";
        public string SerialWatched = "(<\\/i>)([\\s\\S]+)";

        public string PlayList = "\\s?pl = {'0': \"(.*?)\"";
        public string Translate = "data-translate=\"([^0].*?)\">(.*?)</li.{1,30}>pl\\[.*?\"(.*?)\"";

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
        public string SecureMark = "'(secureMark)': '(.*?)'.*?'time': '(\\d+)'";
        public string UserLogout = "(<a href=\"\\/\\?mod=logout\">)";
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
        public string Cookie = "";
        public DateTime CookieExpires;
    }

    [Serializable]
    public class Icons {
        public string Error = "https://img.icons8.com/dusk/64/000000/close-window.png";
        public string Search = "https://img.icons8.com/color/48/000000/search.png";
        public string Folder = "https://img.icons8.com/dusk/64/000000/opened-folder.png";
        public string Nofile = "https://img.icons8.com/dusk/64/000000/box-important.png";
        public string TorrentFile = "https://img.icons8.com/dusk/64/000000/utorrent.png";
        public string Audio = "https://img.icons8.com/bubbles/50/000000/music.png";
        public string Video = "https://img.icons8.com/dusk/64/000000/video-playlist.png";
        public string Image = "https://img.icons8.com/dusk/64/000000/picture.png";
        public string Other = "https://img.icons8.com/dusk/64/000000/new-file.png";
        public string Update = "https://img.icons8.com/dusk/64/000000/approve-and-update.png";
        public string NewVersion = "https://png.icons8.com/office/160/new.png";

        public string User = "https://img.icons8.com/dusk/64/000000/add-user-male.png";
        public string Login = "https://img.icons8.com/dusk/64/000000/enter-2.png";
        public string Password = "https://img.icons8.com/dusk/64/000000/password.png";
    }
}
