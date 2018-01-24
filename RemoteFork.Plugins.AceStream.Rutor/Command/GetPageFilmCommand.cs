using System.Collections.Generic;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetPageFilmCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            if (data.Length > 2 && !string.IsNullOrWhiteSpace(data[2])) {
                var item = new Item() {
                    Name = "Torrent",
                    ImageLink = PluginSettings.Settings.Icons.IcoTorrentFile,
                    Link = $"torrent{PluginSettings.Settings.Separator}{data[2]}",
                    Type = ItemType.DIRECTORY,
                    Description = string.Empty
                };
                items.Add(item);
            }
            if (data.Length > 3 && !string.IsNullOrWhiteSpace(data[3])) {
                var item = new Item() {
                    Name = "Magnet",
                    ImageLink = PluginSettings.Settings.Icons.IcoTorrentFile,
                    Link = $"torrent{PluginSettings.Settings.Separator}{data[3]}",
                    Type = ItemType.DIRECTORY,
                    Description = string.Empty
                };
                items.Add(item);
            }

            return items;
        }
    }
}
