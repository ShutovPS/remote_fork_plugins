using System;

namespace RemoteFork.Plugins.Settings {
    [Serializable]
    public class Settings {
        public Version SettingsVersion = new Version(1, 10);

        public string Key = "997e626ac4d9ce453e6c920785db8f45";

        public bool UseMp4 = true;

        public Links Links = new Links();

        public Encryption Encryption = new Encryption();

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
        public int DomainId = 516746;

        public string DomainUrl =
            "https://gist.githubusercontent.com/ShutovPS/b2fa65ec20b5b5b180a89940979c3237/raw/moonwalk.domain";
    }

    [Serializable]
    public class Regexp {
        public string Translations = "(translations:\\s*)(\\[\\[.*?\\]\\])";
        public string Translation = "(\\[)(\")(.*?)(\")(,\")(.*?)(\"\\])";
        public string Seasons = "(seasons:\\s\\[)(.*?)(\\])";
        public string Episodes = "(episodes:\\s\\[)(.*?)(\\])";

        public string Script = "(<script src=\")(.*?)(\">)";
        public string Host = "(host:\\s?\')(.*?)(\')";
        public string Proto = "(proto:\\s?\')(.*?)(\')";
        public string VideoManifest = "(getVideoManifests:\\s*function)([\\s\\S]*?)(onGetManifestError)";

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

        public string Ini = "({0}\\s*\\=\\s*\\\")([\\d\\w]+)(\\\")";
    }

    [Serializable]
    public class Links {
        public string Api = "http://moonwalk.cc/api";
        public string Site = "http://moonwalk.cc";
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
