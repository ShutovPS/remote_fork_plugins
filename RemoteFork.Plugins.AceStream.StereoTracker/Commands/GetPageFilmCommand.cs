using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetPageFilmCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            var responseData = HTTPUtility.GetBytesRequest(data[2]);
            string response = Encoding.GetEncoding(1251).GetString(responseData);

            string torrentPath = null;
            var regex = new Regex(PluginSettings.Settings.Regexp.GetPageFilmTorrent);
            if (regex.IsMatch(response)) {
                torrentPath = regex.Match(response).Groups[2].Value;
            }
            if (!string.IsNullOrWhiteSpace(torrentPath)) {
                torrentPath = $"{PluginSettings.Settings.TrackerServer}/engine/download.php?id={torrentPath}";
                var header = new Dictionary<string, string>() {
                    {"Referer", PluginSettings.Settings.TrackerServer}
                };
                var torrent = HTTPUtility.GetBytesRequest(torrentPath, header);
                string id = string.Empty;
                var files = FileList.GetFileList(torrent, ref id);
                if (files.Count > 0) {
                    string stream = string.Format(PluginSettings.Settings.AceStreamApi.GetStream,
                        StereoTracker.GetAddress, id);
                    if (files.Count > 1) {
                        StereoTracker.Source = HTTPUtility.GetRequest(stream);
                    } else {
                        string name = Path.GetFileName(files.First().Value);
                        var item = new Item() {
                            Name = Path.GetFileName(name),
                            ImageLink = IconFile.GetIconFile(name),
                            Link = stream,
                            Type = ItemType.FILE,
                            Description = GetDescription(response)
                        };
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        public static string GetDescription(string html) {
            var data = new StringBuilder();

            var regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryMinitable);
            if (regex.IsMatch(html)) {
                string subText = regex.Match(html).Value;
                regex = new Regex(PluginSettings.Settings.Regexp.GetCategorySubCategory);
                var categories = regex.Matches(subText);
                regex = new Regex(PluginSettings.Settings.Regexp.GetCategorySize);
                var values = regex.Matches(subText);
                if (categories.Count == values.Count * 2) {
                    for (int i = 0; i < values.Count; i++) {
                        data.Append(categories[i].Groups[2].Value);
                        data.Append(": ");
                        data.Append(values[i].Groups[2].Value);
                        data.Append(" ");
                        data.Append(values[i].Groups[4].Value);
                        data.Append("<br>");
                    }
                }
            }
            return data.ToString();
        }
    }
}
