using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Settings;
using RemoteFork.Tools;

namespace RemoteFork.Plugins.AceStream.Commands {
    public class GetPageSearchCommand : ICommand {
        public const string KEY = "search";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();
            var reGexInfioHash = new Regex("(?<=\"infohash\":\").*?(?=\")");
            var reGexName = new Regex("(?<=\"name\":\").*?(?=\")");

            if (string.IsNullOrEmpty(data[2])) {
                data[2] = context.GetRequestParams()["search"];
            }

            string url = data[2];
            string page = data[3];
            string response = HTTPUtility
                .GetRequest(
                    $"https://search.acestream.net/?method=search&api_version=1.0&api_key=test_api_key&query={url}&page_size=50&page={page}")
                .ReplaceUnicodeSymbols();

            if (reGexInfioHash.IsMatch(response)) {
                var infoHashs = reGexInfioHash.Matches(response);
                var names = reGexName.Matches(response);
                for (int I = 0; I < infoHashs.Count; I++) {
                    var item = new Item {
                        Name = names[I].Value,
                        Link = "http://" + ProgramSettings.Settings.IpAddress + ":" +
                               ProgramSettings.Settings.AceStreamPort + "/ace/manifest.m3u8?&infohash=" +
                               infoHashs[I].Value,
                        Type = ItemType.FILE,
                        ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291videofile.png"
                    };
                    item.Description = $"<html><font face=\"Arial\" size=\"5\"><b>{item.Name}</font></b>";
                    items.Add(item);
                }
            }

            int.TryParse(page, out int pageValue);

            AceStreamTV.NextPageUrl = $"{KEY}{AceStreamTV.SEPARATOR}{url}{AceStreamTV.SEPARATOR}{pageValue + 1}";
            AceStreamTV.IsIptv = true;

            return items;
        }
    }
}
