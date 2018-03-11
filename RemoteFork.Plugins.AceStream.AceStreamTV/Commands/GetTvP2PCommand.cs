using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;

namespace RemoteFork.Plugins.AceStream.Commands {
    public class GetTvP2PCommand : ICommand {
        public const string KEY = "tvp2p";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();
            var reGexTop = new Regex("(<div class=\"modal-body category-body\">)([\\s\\S]+?)(<\\/div>)");
            var reGex = new Regex("(<a href=)(.*?)(\\/a>)");
            var reGexLinkName = new Regex("(\")(.*?)(\">)(.*?)(<)");
            string respose = HTTPUtility.GetRequest("http://tv-p2p.ru");
            string str = reGexTop.Match(respose).Value;
            foreach (Match match in reGex.Matches(str)) {
                var linkName = reGexLinkName.Match(match.Groups[2].Value);
                var item = new Item {
                    Type = ItemType.DIRECTORY,
                    Name = linkName.Groups[4].Value,
                    Link = $"TvP2PCategory{AceStreamTV.SEPARATOR}{linkName.Groups[2].Value}",
                    ImageLink = "http://torrent-tv.ru/images/all_channels.png"
                };
                items.Add(item);
            }
            
            return items;
        }
    }
}
