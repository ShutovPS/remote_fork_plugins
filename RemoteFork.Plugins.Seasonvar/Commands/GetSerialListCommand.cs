using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using RemoteFork.Items;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    internal class GetSerialListCommand : ICommand {
        public const string KEY = "list";

        public const string URL_KEY = "url";

        public void GetItems(PlayList playList, IPluginContext context = null, Dictionary<string, string> data = null) {
            string url;

            data.TryGetValue(URL_KEY, out url);

            url = WebUtility.UrlDecode(url);

            var header = new Dictionary<string, string>() {
                {"Accept-Encoding", "gzip, deflate, lzma"},
                //{"Content-Type", "text/html; charset=UTF-8"}
            };

            string response = HTTPUtility.GetRequest(PluginSettings.Settings.Links.Site + url, header);

            var matches = Regex.Matches(response,
                PluginSettings.Settings.Regexp.GetSerials,
                RegexOptions.Multiline);

            if (matches.Count == 1) {
                new GetVoiceListCommand().GetItems(playList, context, data);
            } else {
                for (int i = 0; i < matches.Count; i++) {
                    var item = new GetSerialInfoCommand().GetItem(matches[i].Groups[5].Value,
                        matches[i].Groups[8].Value);

                    item.Link = GetVoiceListCommand.CreateLink(matches[i].Groups[2].Value);

                    playList.Items.Add(item);
                }
            }
        }

        public static string CreateLink(string url = default) {
            var data = new Dictionary<string, object>() {
                {Seasonvar.KEY, KEY},
                {URL_KEY, WebUtility.UrlEncode(url)}
            };
            return Seasonvar.CreateLink(data);
        }
    }
}
