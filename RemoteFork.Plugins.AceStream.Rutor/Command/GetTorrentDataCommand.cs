using System.Collections.Generic;
using System.IO;
using System.Linq;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetTorrentDataCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            string torrentPath = data[2];

            if (!string.IsNullOrWhiteSpace(torrentPath)) {
                Dictionary<string, string> files = null;
                string regex = string.Empty;

                if (torrentPath.Contains("magnet")) {
                    files = FileList.GetFileList(torrentPath);
                    regex = PluginSettings.Settings.AceStreamApi.GetStreamByMagnet;
                } else {
                    var torrent = HTTPUtility.GetBytesRequest(torrentPath);
                    torrentPath = string.Empty;
                    files = FileList.GetFileList(torrent, ref torrentPath);
                    regex = PluginSettings.Settings.AceStreamApi.GetStreamByTorrent;
                }
                if (files.Count > 0) {
                    string stream = string.Format(regex, Rutor.GetAddress,
                        torrentPath);
                    if (files.Count > 1) {
                        Rutor.Source = HTTPUtility.GetRequest(stream);
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
