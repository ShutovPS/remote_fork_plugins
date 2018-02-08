using System.Collections.Generic;
using System.IO;
using System.Linq;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetPageFilmCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            string torrentPath = data[2];
            if (!string.IsNullOrWhiteSpace(torrentPath)) {
                var files = FileList.GetFileList(torrentPath);
                if (files.Count > 0) {
                    string stream = string.Format(PluginSettings.Settings.AceStreamApi.GetStream, NnmClub.GetAddress,
                        torrentPath);
                    if (files.Count > 1) {
                        NnmClub.Source = HTTPUtility.GetRequest(stream);
                    } else {
                        string name = Path.GetFileName(files.First().Value);
                        var item = new Item() {
                            Name = Path.GetFileName(name),
                            ImageLink = IconFile.GetIconFile(name),
                            Link = stream,
                            Type = ItemType.FILE
                        };
                        items.Add(item);
                    }
                }
            }
            
            return items;
        }
    }
}
