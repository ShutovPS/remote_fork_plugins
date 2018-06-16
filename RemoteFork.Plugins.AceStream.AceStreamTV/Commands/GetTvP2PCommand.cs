using RemoteFork.Network;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.AceStream.Commands {
    public class GetTvP2PCommand : ICommand {
        public const string KEY = "tvp2p";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();
            var reGexTop = new Regex(PluginSettings.Settings.Regexp.CategoryBody);
            var reGex = new Regex(PluginSettings.Settings.Regexp.Link);
            var reGexLinkName = new Regex(PluginSettings.Settings.Regexp.LinkName);
            string respose = HTTPUtility.GetRequest(PluginSettings.Settings.Links.TvP2P);
            string str = reGexTop.Match(respose).Value;
            foreach (Match match in reGex.Matches(str)) {
                var linkName = reGexLinkName.Match(match.Groups[2].Value);
                var item = new Item {
                    Type = ItemType.DIRECTORY,
                    Name = linkName.Groups[4].Value,
                    Link = $"{GetTvP2PCategoryCommand.KEY}{PluginSettings.Settings.Separator}{linkName.Groups[2].Value}",
                    ImageLink = "http://torrent-tv.ru/images/all_channels.png"
                };
                items.Add(item);
            }
            
            return items;
        }
    }
}
