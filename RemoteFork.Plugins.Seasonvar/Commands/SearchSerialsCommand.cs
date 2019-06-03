using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using RemoteFork.Items;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class SearchSerialsCommand : ICommand {
        public const string KEY = "search";

        public void GetItems(PlayList playList, IPluginContext context, Dictionary<string, string> data) {
            string searchText = context.GetRequestParams()[KEY];

            var header = new Dictionary<string, string>() {
                {"Accept-Encoding", "gzip, deflate, lzma"},
                //{"Content-Type", "text/html; charset=windows-1251"}
            };

            string response =
                HTTPUtility.GetRequest(PluginSettings.Settings.Links.Site + "/autocomplete.php?query=" + searchText,
                    header);
            response = DecodeEncodedNonAsciiCharacters(response);
            var matchesIds = Regex.Matches(Regex.Match(response, "(\"id\":\\[)(\"(.*?),*\")+?\\]").Groups[2].Value,
                "(\"(\\d+?)\")\\,?");
            var matchesValues =
                Regex.Matches(Regex.Match(response, "(\"valu\":\\[)(\".*?\",?)\\]").Groups[2].Value,
                    "(\"(.+?)\"\\,?)");
            var matchesData =
                Regex.Matches(Regex.Match(response, "(\"data\":\\[)(\".*?\",?)\\]").Groups[2].Value,
                    "(\"(.+?\\.html)\"\\,?)");
            for (int i = 0; i < matchesData.Count; i++) {
                var item = new GetSerialInfoCommand().GetItem(matchesIds[i].Groups[2].Value,
                    matchesValues[i].Groups[2].Value);

                item.Link = GetVoiceListCommand.CreateLink('/' + matchesData[i].Groups[2].Value);

                playList.Items.Add(item);
            }
        }

        public static string CreateLink() {
            var data = new Dictionary<string, object>() {
                {Seasonvar.KEY, KEY}
            };

            return Seasonvar.CreateLink(data);
        }

        private static string DecodeEncodedNonAsciiCharacters(string value) {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => ((char) int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString());
        }
    }
}
