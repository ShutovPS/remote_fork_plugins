using System.Collections.Generic;
using System.Collections.Specialized;
using RemoteFork.Plugins.Commands;
using RemoteFork.Settings;

namespace RemoteFork.Plugins {
    [Plugin(Id = "nnmclub", Version = "0.1.0", Author = "fd_crash&ORAMAN", Name = "NNM-Club (AceStream)",
        Description = "Воспроизведение NNM-Club через меда-сервер Ace Stream",
        ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent2.png")]

    public class NnmClub : IPlugin {
        public const char SEPARATOR = ';';
        public const string PLUGIN_PATH = "pluginPath";
        public const string LOGO_NO_NAME_CLUB = "http://assets.nnm-club.ws/forum/images/logos/10let8.png";
        public const string ICO_SEARCH = "http://s1.iconbird.com/ico/0612/MustHave/w256h2561339195991Search256x256.png";
        public const string ICO_FOLDER = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246folder.png";
        public const string ICO_TORRENT_FILE = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent2.png";
        public const string ICO_NOFILE = "https://avatanplus.com/files/resources/mid/5788db3ecaa49155ee986d6e.png";
        public const string ICO_ERROR = "http://s1.iconbird.com/ico/0912/ToolbarIcons/w256h2561346685474SymbolError.png";
        
        public static bool IsIptv = false;
        public static string NextPageUrl = null;
        public static string Source = null;

        public static string TrackerServerNnm = "https://nnm-club.name";

        public static string GetAddress => $"http://{ProgramSettings.Settings.IpAddress}:{ProgramSettings.Settings.AceStreamPort}";

        public Playlist GetList(IPluginContext context) {
            string path = context.GetRequestParams().Get(PLUGIN_PATH);

            path = path == null ? "plugin" : "plugin;" + path;

            var arg = path.Split(SEPARATOR);

            var items = new List<Item>();
            ICommand command = null;
            var data = new string[4];
            switch (arg.Length) {
                case 0:
                    break;
                case 1:
                    command = new GetRootListCommand();
                    break;
                default:
                    switch (arg[1]) {
                        case "search":
                            data[2] = context.GetRequestParams()["search"];
                            command = new GetSearchCommand();
                            break;
                        case "pagefilm":
                            command = new GetPageFilmCommand();
                            break;
                        case "page":
                            command = new GetCategoryCommand();
                            break;
                    }
                    break;
            }

            NextPageUrl = Source = null;
            IsIptv = false;

            if (command != null) {
                for (int i = 0; i < arg.Length; i++) {
                    data[i] = arg[i];
                }
                items.AddRange(command.GetItems(context, data));
            }

            return CreatePlaylist(items, context);
        }

        public static Playlist CreatePlaylist(List<Item> items, IPluginContext context) {
            var playlist = new Playlist();

            if (!string.IsNullOrEmpty(NextPageUrl)) {
                var pluginParams = new NameValueCollection {[PLUGIN_PATH] = NextPageUrl };
                playlist.NextPageUrl = context.CreatePluginUrl(pluginParams);
            }
            playlist.Timeout = "60";

            playlist.Items = items.ToArray();

            foreach (var item in playlist.Items) {
                if (ItemType.DIRECTORY == item.Type) {
                    var pluginParams = new NameValueCollection {
                        [PLUGIN_PATH] = item.Link
                    };
                    item.Link = context.CreatePluginUrl(pluginParams);
                }
            }

            playlist.source = Source;
            playlist.IsIptv = IsIptv ? "True" : "False";

            return playlist;
        }
    }
}
