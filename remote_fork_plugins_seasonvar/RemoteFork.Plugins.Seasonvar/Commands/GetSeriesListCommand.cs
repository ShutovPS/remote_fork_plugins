using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RemoteFork.Plugins {
    public class GetSeriesListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            List<Item> items = new List<Item>();

            string url = data.Length > 2 ? data[2] : string.Empty;

            var header = new Dictionary<string, string>() {
                {"Accept-Encoding", "gzip, deflate, lzma"},
                {"Content-Type", "text/html; charset=UTF-8"}
            };
            context.ConsoleLog("url=" + string.Format(Seasonvar.SITE_URL,
                                   url.Replace("transСтандартный", "trans")));
            string response =
                context.GetHttpClient().GetRequest(string.Format(Seasonvar.SITE_URL,
                    url.Replace("transСтандартный", "trans")), header);

            var matches = Regex.Matches(response,
                "({)(\"title\"\\s*:\\s*\")(\\d+)(\\s+?)(.+?)(\")(.*?)(\"file\"\\s*:\\s*\")(.+?)(\")(.+?)(\"galabel\"\\s*:\\s*\")(.+?)(\")(.+?)(})",
                RegexOptions.Multiline);

            var match = Regex.Match(url, "(\\/)(\\d+)(\\/)");

            Item item = new GetSerialInfoCommand().GetItem(context, match.Groups[2].Value);

            for (int i = 0; i < matches.Count; i++) {
                var itemR = new Item() {
                    Name = string.Format("{0} Серия", matches[i].Groups[3].Value, matches[i].Groups[19].Value),
                    Link = matches[i].Groups[9].Value,
                    Type = ItemType.FILE,
                    ImageLink = item.ImageLink,
                    Description = item.Description
                };

                items.Add(itemR);
            }

            return items;
        }
    }
}
