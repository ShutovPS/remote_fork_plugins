using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using RemoteFork.Network;

namespace RemoteFork.Plugins {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            var baseItem = new Item() {
                Type = ItemType.DIRECTORY
            };

            var item = new Item() {
                Name = "Поиск",
                Type = ItemType.DIRECTORY,
                Link = $"{SearchCommand.KEY}",
                SearchOn = "Поиск",
                ImageLink =
                    "http://icons.iconarchive.com/icons/graphicloads/colorful-long-shadow/256/Search-icon.png"
            };
            items.Add(item);

            string response = HTTPUtility.GetRequest("http://godzfilm.net/");
            var regex = new Regex("(<div class=\"janr-block\"\\s*>)([\\s\\S]*?)(<div style=\"clear)");
            if (regex.IsMatch(response)) {
                response = regex.Match(response).Groups[2].Value;
                response = Regex.Replace(response, "(<\\/?FONT.*?>)", string.Empty);
                response = Regex.Replace(response, "&nbsp;", string.Empty);
                regex = new Regex("(<a href=\")(\\w*?)(\">.*?)([a-zA-Zа-яА-Я\\s\\/]+?)(<(.*?)\\/a>)");
                if (regex.IsMatch(response)) {
                    foreach (Match match in regex.Matches(response)) {
                        item = new Item(baseItem) {
                            Name = match.Groups[4].Value,
                            Link =
                                $"{GetCategoryCommand.KEY}{GodZfilm.SEPARATOR}{WebUtility.UrlEncode("http://godzfilm.net/" + match.Groups[2].Value)}",
                            ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246folder.png",
                            Description = match.Groups[4].Value

                        };
                        items.Add(item);
                    }
                }
            }

            return items;
        }
    }
}
