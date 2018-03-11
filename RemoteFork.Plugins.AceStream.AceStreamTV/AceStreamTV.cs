using System.Collections.Generic;
using System.Collections.Specialized;
using RemoteFork.Plugins.AceStream.Commands;
using ICommand = RemoteFork.Plugins.AceStream.Commands.ICommand;

namespace RemoteFork.Plugins.AceStream {
    [PluginAttribute(Id = "acestreamtv", Version = "0.1.1", Author = "fd_crash&ORAMAN", Name = "AceStreamTV",
        Description = "Воспроизведение TORRENT IPTV через меда-сервер Ace Stream",
        ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291utorrent2.png")]

    public class AceStreamTV : IPlugin {
        public const char SEPARATOR = ';';
        public const string PLUGIN_PATH = "pluginPath";
        
        public static bool IsIptv = false;
        public static string NextPageUrl = null;
        public static string Source = null;

        public Playlist GetList(IPluginContext context) {
            string path = context.GetRequestParams().Get(PLUGIN_PATH);

            path = path == null ? "plugin" : "plugin;" + path;

            var arg = path.Split(SEPARATOR);

            var items = new List<Item>();
            ICommand command = null;
            var data = new string[4];
            switch (arg.Length) {
                case 0:
                    command = new GetRootListCommand();
                    break;
                case 1:
                    command = new GetRootListCommand();
                    break;
                default:
                    switch (arg[1]) {
                        case GetPageSearchStreamTVCommand.KEY:
                            command = new GetPageSearchStreamTVCommand();
                            break;
                        case GetRootListCommand.KEY:
                            command = new GetRootListCommand();
                            break;
                        case GetTorrentTVCommand.KEY:
                            command = new GetTorrentTVCommand();
                            break;
                        case GetAceStreamNetTVCommand.KEY:
                            command = new GetAceStreamNetTVCommand();
                            break;
                        case GetTvP2PCommand.KEY:
                            command = new GetTvP2PCommand();
                            break;
                        case GetTvP2PCategoryCommand.KEY:
                            command = new GetTvP2PCategoryCommand();
                            break;
                        case GetTvP2PChanelCommand.KEY:
                            command = new GetTvP2PChanelCommand();
                            break;
                        case GetIproxyListCommand.KEY:
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
