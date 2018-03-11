using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Settings;

namespace RemoteFork.Plugins.AceStream.Commands {
    public class GetTvP2PChanelCommand : ICommand {
        public const string KEY = "TvP2PChanel";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();
            var item = new Item();
            var reGexLink = new Regex("(loadPlayer\\(\')([\\d\\w]*?)(\')");
            var reGexName = new Regex("(?<=<title>).*?(?=&raquo;)");

            string response = HTTPUtility.GetRequest(data[2]).Replace("\n", "");

            item.Type = ItemType.FILE;
            item.Link = $"http://{ProgramSettings.Settings.IpAddress}:{ProgramSettings.Settings.AceStreamPort}/ace/getstream?id={reGexLink.Match(response).Groups[2].Value}";
            item.Name = reGexName.Match(response).Value;
            item.ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291videofile.png";

            items.Add(item);

            AceStreamTV.IsIptv = true;

            return items;
        }
    }
}
