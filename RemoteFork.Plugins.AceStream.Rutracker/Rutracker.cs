using System.Collections.Generic;
using System.Collections.Specialized;
using RemoteFork.Plugins.Commands;
using RemoteFork.Plugins.Settings;
using RemoteFork.Settings;

namespace RemoteFork.Plugins {
    [Plugin(Id = "rutracker", Version = "0.1.3", Author = "fd_crash&ORAMAN", Name = "Rutracker (AceStream)",
        Description = "Воспроизведение Rutracker через меда-сервер Ace Stream",
        ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent2.png")]

    public class Rutracker : IPlugin {
        public static bool IsIptv = false;
        public static string NextPageUrl = null;
        public static string Source = null;

        public static string GetAddress => $"http://{ProgramSettings.Settings.IpAddress}:{ProgramSettings.Settings.AceStreamPort}";

        public Playlist GetList(IPluginContext context) {
            string path = context.GetRequestParams().Get(PluginSettings.Settings.PluginPath);

            path = path == null ? "plugin" : "plugin;" + path;

            var arg = path.Split(PluginSettings.Settings.Separator);

            var items = new List<Item>();
            ICommand command = null;
            var data = new string[arg.Length > 4 ? arg.Length : 4];
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
                        case "category":
                            command = new GetCategoryCommand();
                            break;
                        case "login":
                            command = new LoginCommand();
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
                var pluginParams = new NameValueCollection {[PluginSettings.Settings.PluginPath] = NextPageUrl };
                playlist.NextPageUrl = context.CreatePluginUrl(pluginParams);
            }
            playlist.Timeout = "60";

            playlist.Items = items.ToArray();

            foreach (var item in playlist.Items) {
                if (ItemType.DIRECTORY == item.Type) {
                    var pluginParams = new NameValueCollection {
                        [PluginSettings.Settings.PluginPath] = item.Link
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
