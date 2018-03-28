using RemoteFork.Network;
using RemoteFork.Settings;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RemoteFork.Plugins.AceStream.Commands {
    public class GetTvP2PChanelCommand : ICommand {
        public const string KEY = "TvP2PChanel";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();
            var reGexLink = new Regex("(loadPlayer\\(\')([\\d\\w]*?)(\')");
            var reGexName = new Regex("(?<=<title>).*?(?=&raquo;)");

            string response = HTTPUtility.GetRequest(data[2]).Replace("\n", "");

            var item = new Item {
                Type = ItemType.FILE,
                Link =
                    $"http://{ProgramSettings.Settings.IpAddress}:{ProgramSettings.Settings.AceStreamPort}/ace/getstream?id={reGexLink.Match(response).Groups[2].Value}&.mp4",
                Name = reGexName.Match(response).Value,
                ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291videofile.png"
            };

            items.Add(item);

            AceStreamTV.IsIptv = true;

            return items;
        }
    }
}
