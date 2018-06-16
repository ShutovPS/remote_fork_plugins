using System.Collections.Generic;
using System.Collections.Specialized;
using RemoteFork.Plugins.Commands;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    [Plugin(Id = "thvp", Version = "0.1.1", Author = "fd_crash", Name = "THVP",
        Description = "THVP предоставляет пользователю простой способ проигрывать любые торрент медиа-файлы",
        ImageLink = "http://thvp.ru/img/logo.png")]

    public class Thvp : IPlugin {
        public static bool IsIptv = false;
        public static string NextPageUrl = null;
        public static string Source = null;

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
                        case GetCategoriesCommand.KEY:
                            command = new GetCategoriesCommand();
                            break;
                        case GetCategoryCommand.KEY:
                            command = new GetCategoryCommand();
                            break;
                        case GetSearchCommand.KEY:
                            command = new GetSearchCommand();
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
                var pluginParams = new NameValueCollection { [PluginSettings.Settings.PluginPath] = NextPageUrl };
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
            playlist.IptvPlaylist = IsIptv;

            return playlist;
        }
    }
}
