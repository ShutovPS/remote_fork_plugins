namespace RemoteFork.Plugins.Settings {
    public class Settings {
        public string SettingsVersion = "0.4";

        public char Separator = ';';

        public string ApiServer = "https://www.googleapis.com/youtube/v3";

        public string ApiKey = "AIzaSyD6nuSKJVzCG4KI9yJ_ecHqhQpg3yTbJQg";

        public string PluginPath = "pluginPath";

        public string Logo = "http://pluspng.com/img-png/youtube-png-youtube-png-image-3567-300.png";

        public Icons Icons = new Icons();
    }

    public class Icons {
        public string IcoSearch = "http://s1.iconbird.com/ico/0612/MustHave/w256h2561339195991Search256x256.png";
        public string IcoFolder = "http://www.icons101.com/icon_png/size_128/id_80762/YouTube_Folder.png";
        public string IcoChannel = "http://www.icons101.com/icon_png/size_128/id_78792/Youtube.png";
        public string IcoVideo = "http://www.icons101.com/icon_png/size_128/id_78921/youtube.png";
        public string IcoVideoFile = "http://www.icons101.com/icon_png/size_128/id_52643/youtube.png";
        public string IcoPlaylist = "http://www.icons101.com/icon_png/size_128/id_73091/YoutubeIcon.png";
}
}
