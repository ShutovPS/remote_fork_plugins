using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;
using RemoteFork.Settings;
using RemoteFork.Tools;

namespace RemoteFork.Plugins.AceStream.Commands {
    public class GetPageSearchCommand : ICommand {
        public const string KEY = "search";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();
            var reGexInfioHash = new Regex(PluginSettings.Settings.Regexp.InfoHash);
            var reGexName = new Regex(PluginSettings.Settings.Regexp.Name);

            if (string.IsNullOrEmpty(data[2])) {
                data[2] = context.GetRequestParams()["search"];
            }

            string url = data[2];
            string page = data[3];
            string response = HTTPUtility
                .GetRequest(string.Format(PluginSettings.Settings.AceStreamApi.Search, url, page))
                .ReplaceUnicodeSymbols();

            if (reGexInfioHash.IsMatch(response)) {
                var infoHashs = reGexInfioHash.Matches(response);
                var names = reGexName.Matches(response);
                for (int I = 0; I < infoHashs.Count; I++) {
                    var item = new Item {
                        Name = names[I].Value,
                        Link = string.Format(PluginSettings.Settings.AceStreamApi.GetStreamManifest,
                            ProgramSettings.Settings.IpAddress, ProgramSettings.Settings.AceStreamPort,
                            infoHashs[I].Value),
                        Type = ItemType.FILE,
                        ImageLink = PluginSettings.Settings.Icons.IcoVideo
                    };
                    item.Description = $"<html><font face=\"Arial\" size=\"5\"><b>{item.Name}</font></b>";
                    items.Add(item);
                }
            }

            int.TryParse(page, out int pageValue);

            AceStreamTV.NextPageUrl =
                $"{KEY}{PluginSettings.Settings.Separator}{url}{PluginSettings.Settings.Separator}{pageValue + 1}";
            AceStreamTV.IsIptv = true;

            return items;
        }
    }
}
