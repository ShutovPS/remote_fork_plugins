using RemoteFork.Network;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.AceStream.Commands {
    public class GetTvP2PCategoryCommand : ICommand {
        public const string KEY = "TvP2PCategory";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            string response = HTTPUtility.GetRequest(PluginSettings.Settings.Links.TvP2P + data[2]).Replace("\n", "");
            var reGex = new Regex("(<div class=\"main_short\">)(.*?)(<div class=\"navigation\" align=\"center\"\\s?>)");
            var reGexDesc =
                new Regex("(href=\\\")(.*?)(\")(.*?title=\")(.*?)(\")(.*?<img src=\")(.*?)(\")(.*?)(<\\/a>)");

            if (reGex.IsMatch(response)) {
                LineGo:
                foreach (Match match in reGexDesc.Matches(reGex.Match(response).Groups[2].Value)) {
                    if (match.Success) {
                        var item = new Item {
                            Type = ItemType.DIRECTORY,
                            Name = match.Groups[5].Value,
                            Link = $"{GetTvP2PChanelCommand.KEY}{PluginSettings.Settings.Separator}{match.Groups[2]}",
                            ImageLink = PluginSettings.Settings.Links.TvP2P + match.Groups[7]
                        };
                        item.Description =
                            $"<html><font face=\"Arial\" size=\"5\"><b>{item.Name}</font></b><p><img src=\"{item.ImageLink}\"></html><p>";
                        items.Add(item);
                    }
                }

                var reGexNext = new Regex("(?<=<span class=\"pnext\"><a href=\").*?(?=\">)");
                if (reGexNext.IsMatch(response)) {
                    response = HTTPUtility.GetRequest(reGexNext.Match(response).Value).Replace("\n", "");
                    goto LineGo;
                }
            }

            return items;
        }
    }
}
