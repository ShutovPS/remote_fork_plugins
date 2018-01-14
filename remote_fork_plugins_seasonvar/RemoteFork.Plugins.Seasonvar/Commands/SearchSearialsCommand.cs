using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RemoteFork.Plugins {
    public class SearchSearialsCommand : ICommand {
        public List<Item> GetItems(IPluginContext context, params string[] data) {
            List<Item> items = new List<Item>();

            string searchText = context.GetRequestParams()["search"];

            var header = new Dictionary<string, string>() {
                {"Accept-Encoding", "gzip, deflate, lzma"},
                {"Content-Type", "text/html; charset=windows-1251"}
            };

            string response = context.GetHttpClient()
                .GetRequest(string.Format(Seasonvar.SITE_URL, "/autocomplete.php?query=" + searchText), header);
            response = DecodeEncodedNonAsciiCharacters(response);
            var matchesIds = Regex.Matches(Regex.Match(response, "(\"id\":\\[)(\"(.*?),*\")+?\\]").Groups[2].Value,
                "(\"(\\d+?)\")\\,?");
            var matchesValues =
                Regex.Matches(Regex.Match(response, "(\"valu\":\\[)(\".*?\",?)\\]").Groups[2].Value,
                    "(\"(.+?)\"\\,?)");
            var matchesDatas =
                Regex.Matches(Regex.Match(response, "(\"data\":\\[)(\".*?\",?)\\]").Groups[2].Value,
                    "(\"(.+?\\.html)\"\\,?)");
            for (int i = 0; i < matchesDatas.Count; i++) {
                Item item = new GetSerialInfoCommand().GetItem(context, matchesIds[i].Groups[2].Value);
                item.Name = matchesValues[i].Groups[2].Value.Trim();
                item.Link = string.Format("{1}{0}/{2}", Seasonvar.SEPARATOR, "voise", matchesDatas[i].Groups[2].Value);

                items.Add(item);
            }

            return items;
        }

        private static string DecodeEncodedNonAsciiCharacters(string value) {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => {
                    return ((char) int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }
    }
}
