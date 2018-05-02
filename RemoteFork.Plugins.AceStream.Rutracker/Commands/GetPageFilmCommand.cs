using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;
using RemoteFork.Torrents;

namespace RemoteFork.Plugins.Commands {
    public class GetPageFilmCommand : ICommand {
        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            string responseFromServer = HTTPUtility.GetRequest(PluginSettings.Settings.TrackerServer + data[2]);

            string torrentPath = null;
            var regex = new Regex(PluginSettings.Settings.Regexp.GetPageFilmMagnet);
            if (regex.IsMatch(responseFromServer)) {
                torrentPath = regex.Match(responseFromServer).Groups[2].Value;
            }
            if (!string.IsNullOrWhiteSpace(torrentPath)) {
                var files = FileList.GetFileList(torrentPath, "magnet");
                if (files.Count > 0) {
                    string stream = string.Format(PluginSettings.Settings.AceStreamApi.GetStream, Rutracker.GetAddress,
                        torrentPath);
                    if (files.Count > 1) {
                        Rutracker.Source = HTTPUtility.GetRequest(stream);
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
