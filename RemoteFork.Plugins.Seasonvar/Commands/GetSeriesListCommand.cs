using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RemoteFork.Network;

namespace RemoteFork.Plugins {
    public class GetSeriesListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            string url = data.Length > 2 ? data[2] : string.Empty;

            var header = new Dictionary<string, string>() {
                {"Accept-Encoding", "gzip, deflate, lzma"},
                //{"Content-Type", "text/html; charset=UTF-8"}
            };
            //context.ConsoleLog("url=" + string.Format(Seasonvar.SITE_URL,
            //                       url.Replace("transСтандартный", "trans")));
            string response =
                HTTPUtility.GetRequest(string.Format(Seasonvar.SITE_URL,
                    url.Replace("transСтандартный", "trans")), header);

            var matches = Regex.Matches(response,
                "({)(\"title\"\\s*:\\s*\")(\\d+)(\\s+?)(.+?)(\")(.*?)(\"file\"\\s*:\\s*\")(.+?)(\")(.+?)(\"galabel\"\\s*:\\s*\")(.+?)(\")(.+?)(})",
                RegexOptions.Multiline);

            var match = Regex.Match(url, "(\\/)(\\d+)(\\/)");

            var item = new GetSerialInfoCommand().GetItem(context, match.Groups[2].Value);

            for (int i = 0; i < matches.Count; i++) {
                string fileLink = matches[i].Groups[9].Value;
                if (fileLink.StartsWith("#2")) {
                    var regex = new Regex(@"(\\\/\\\/.*?=)");
                    fileLink = regex.Replace(fileLink, string.Empty);
                    byte[] linkData = Convert.FromBase64String(fileLink.Substring(2));
                    fileLink = Encoding.UTF8.GetString(linkData);
                }
                var itemR = new Item() {
                    Name = string.Format("{0} Серия", matches[i].Groups[3].Value, matches[i].Groups[19].Value),
                    Link = fileLink,
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
