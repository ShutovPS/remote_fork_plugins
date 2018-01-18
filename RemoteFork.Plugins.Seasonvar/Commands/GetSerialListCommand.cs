using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;

namespace RemoteFork.Plugins {
    internal class GetSerialListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context, params string[] data) {
            List<Item> items = new List<Item>();

            string url = data.Length > 2 ? data[2] : string.Empty;

            var header = new Dictionary<string, string>() {
                {"Accept-Encoding", "gzip, deflate, lzma"},
                {"Content-Type", "text/html; charset=UTF-8"}
            };

            string response = HTTPUtility.GetRequest(string.Format(Seasonvar.SITE_URL, url), header);

            var matches = Regex.Matches(response,
                "(<h2>[.\\s^&]*?<a href=\")((\\S*?)(\\/serial-(\\d+))(\\S*?))(\">[\\s>]*?Сериал)(.+?)(\\s*?(<span>|<\\/a>))",
                RegexOptions.Multiline);

            if (matches.Count == 1) {
                return new GetVoiseListCommand().GetItems(context, data);
            } else {
                for (int i = 0; i < matches.Count; i++) {
                    Item item = new GetSerialInfoCommand().GetItem(context, matches[i].Groups[5].Value);
                    item.Name = matches[i].Groups[8].Value.Trim();
                    item.Link = string.Format("{1}{0}{2}", Seasonvar.SEPARATOR, "voise", matches[i].Groups[2].Value);

                    items.Add(item);
                }
            }

            return items;
        }
    }
}
