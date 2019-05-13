using System;

namespace RemoteFork.Plugins.Settings {
    [Serializable]
    public class Settings {
        public Version SettingsVersion = new Version(1, 12);

        public bool UseMp4 = true;

        public Encryption Encryption = new Encryption();

        public Links Links = new Links();

        public Regexp Regexp = new Regexp();

        public Icons Icons = new Icons();

        public char Separator = ';';

        public string PluginPath = "pluginPath";
    }

    [Serializable]
    public class Encryption {
        public string IV = "2ea2116c80fae4e90c1e2b2b765fcc45";
        public string Key = "9186a0bae4afec34c323aecb7754b7c848e016188eb8b5be677d54d3e70f9cbd";
        public string Url = "https://raw.githubusercontent.com/WendyH/PHP-Scripts/master/moon4crack.ini";
    }

    [Serializable]
    public class Regexp {
        public string Iframe = "(<iframe.*?src=\")(.*?iframe)(.*?\")";
        public string Translations = "(translations:\\s*)(\\[\\[.*?\\]\\])";
        public string Translation = "(\\[)(\")(.*?)(\")(,\")(.*?)(\"\\])";
        public string Seasons = "(seasons:\\s\\[)(.*?)(\\])";
        public string Episodes = "(episodes:\\s\\[)(.*?)(\\])";

        public string MetaTitle = "(<meta property=\"og:title\" content=\")(.*?)([\"\\s\\/]*?>)";
        public string MetaImage = "(<meta property=\"og:image\" content=\")(.*?)([\"\\s\\/]*?>)";
        public string MiniDescription = "(<div class=\"b-post__description_text\">\\s*)(.*?)(<\\s*\\/div>)";

        public string Categories =
            "(<div class=\"b-content__inline_item\" data-id=\"\\d+\")([\\s\\S]*?)(<\\/div><\\/div>)";

        public string FilmUrl =
            "(<a href=\")(https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\\+.~#?&\\/=]*))(\"><span class=\"b-navigation__next i-sprt\">(&nbsp;)?<\\/span><\\/a>)";

        public string FullDescription =
            "(data-url=\")(.*?)(\">.*?<img src=\")(.*?)(\".*?<i class=\"entity\">)(.*?)(<\\/i>)((.*?<span class=\"info\">)(.*?)(<\\/span>))?(.*?.html\">)(.*?)(<\\/a>\\s*?<div>)(.*?)(<\\/div>)";

        public string Script = "(<script src=\")(.*?)(\">)";
        public string Host = "(host:\\s?\')(.*?)(\')";
        public string Proto = "(proto:\\s?\')(.*?)(\')";

        public string VideoToken = "(video_token:\\s*\')(.*?)(\')";
        public string PartnerId = "(partner_id:\\s*)(\\d+)";
        public string DomainId = "(domain_id:\\s*)(\\d+)";
        public string WindowId = "(window\\[\'[\\d\\w]+\'\\]\\s?=\\s?\')(.*?)(\')";
        public string Ref = "(ref:\\s*\\\')(.*?)(\\\')";

        public string M3U8 = "(\"m3u8\":\\s*\")(.*?)(\")";
        public string ExtList = "(#EXT-X.*?=)(\\d+x\\d+)([\\s\\S]*?)(https?:.*)";

        public string MP4 = "(\"mp4\":\\s*\")(.*?)(\")";
        public string Mp4List = "(\\\")(\\d+)(\\\".*?\")(https?.*?)(\")";

        public string Redirect = "(<a href=\")(https?.+?iframe)(.*?\">)";

        public string InfoPoster = "(<img itemprop=\"image\" src=\")(.*?)(\")";
        public string InfoId = "(data-pid\\s*=\\s*\\\")(\\d+)";
        public string InfoTitleRu = "(<div class=\"b-post__title\">\\s*<h1 itemprop=\"name\">)(.*?)(<\\/h1>)";
        public string InfoTitleOriginal = "(<div class=\"b-post__origtitle\"\\s*itemprop=\"alternativeHeadline\">)(.*?)(<\\/div>)";
        public string InfoDescription = "(<div class=\"b-post__description_text\">)(.*?)(<\\/div>)";

        public string Ini = "({0}\\s*\\=\\s*\\\")([\\d\\w]+)(\\\")";
    }

    [Serializable]
    public class Links {
        public string Site = "https://hdrezka.name";
        public string Moonwalk  = "http://moonwalk.cc";
        public string QuickDescription  = "https://hdrezka.name/engine/ajax/quick_content.php";
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
