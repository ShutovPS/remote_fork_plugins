using System.Collections.Generic;
using System.Collections.Specialized;
using RemoteFork.Plugins.AceStream.Commands;
using ICommand = RemoteFork.Plugins.AceStream.Commands.ICommand;

namespace RemoteFork.Plugins.AceStream {
    [PluginAttribute(Id = "acestreamtv", Version = "0.1", Author = "fd_crash&ORAMAN", Name = "AceStreamTV",
        Description = "Воспроизведение TORRENT IPTV через меда-сервер Ace Stream",
        ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent2.png")]

    public class AceStreamTV : IPlugin {
        public const char SEPARATOR = ';';
        public const string PLUGIN_PATH = "pluginPath";
        
        public static bool IsIptv = false;
        public static string NextPageUrl = string.Empty;
        public static string Source = string.Empty;

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
                        case "searchtvtoace":
                            data[2] = context.GetRequestParams()["search"];
                            command = new GetPageSearchStreamTVCommand();
                            break;
                        case "SEARCHTV":
                            command = new GetPageSearchStreamTVCommand();
                            break;
                        case "tv":
                            command = new GetRootListCommand();
                            break;
                        case "torrenttv":
                            command = new GetTorrentTVCommand();
                            break;
                        case "acestreamnettv":
                            command = new GetAceStreamNetTVCommand();
                            break;
                        case "tvp2p":
                            command = new GetTvP2PCommand();
                            break;
                        case "TvP2PCategory":
                            command = new GetTvP2PCategoryCommand();
                            break;
                        case "TvP2PChanel":
                            command = new GetTvP2PChanelCommand();
                            break;
                        case "iproxy":
                            command = new GetIproxyListCommand();
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
            } else {
                playlist.NextPageUrl = null;
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
