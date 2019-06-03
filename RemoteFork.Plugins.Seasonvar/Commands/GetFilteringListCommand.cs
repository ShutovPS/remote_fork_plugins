using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using RemoteFork.Items;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetFilteringListCommand : ICommand {
        public const string KEY = "filter";

        public const string LANG_KEY = "lang";
        public const string PAGE_KEY = "page";
        public const string SORT_KEY = "sort";

        private static readonly Dictionary<string, string> _filters = new Dictionary<string, string>() {
            {"По популярности", "view"},
            {"По названию", "name"},
            {"По году", "god"},
            {"По добавлению", "newest"}
        };

        public void GetItems(PlayList playList, IPluginContext context = null, Dictionary<string, string> data = null) {
            string lang;
            string page;
            string sort;

            data.TryGetValue(LANG_KEY, out lang);
            data.TryGetValue(PAGE_KEY, out page);
            data.TryGetValue(SORT_KEY, out sort);

            if (!string.IsNullOrEmpty(page)) {
                if (Seasonvar.SERIAL_MATCHES.ContainsKey(lang + sort)) {
                    new NextPageCommand().GetItems(playList, context, data);

                    return;
                }
            }

            if (string.IsNullOrEmpty(sort)) {
                sort = "view";

                var baseItem = new DirectoryItem() {
                    ImageLink = PluginSettings.Settings.Icons.IcoFolder
                };

                foreach (var filter in _filters) {
                    var item = new DirectoryItem(baseItem) {
                        Title = filter.Key,
                        Link = CreateLink(lang, filter.Value)
                    };
                    playList.Items.Add(item);
                }

                baseItem = new DirectoryItem() {
                    Title = "По первому символу",
                    Link = FirstSymbolGroupCommand.CreateLink(lang),

                    ImageLink = PluginSettings.Settings.Icons.IcoFolder
                };

                playList.Items.Add(baseItem);
            }

            List<Match> tempSerials;

            if (Seasonvar.SERIAL_MATCHES.ContainsKey(lang + sort)) {
                tempSerials = Seasonvar.SERIAL_MATCHES[lang + sort];
            } else {
                var dataRequest = new Dictionary<string, string>() {
                    {"filter[only]", lang},
                    {"filter[rait]", "kp"},
                    {"filter[sortTo][]", sort},
                    {"filter[block]", "yes"},
                };
                var header = new Dictionary<string, string>() {
                    {"Accept-Encoding", "gzip, deflate, lzma"},
                    //{"Content-Type", "text/html; charset=UTF-8"}
                };
                var datastring = new StringBuilder();
                foreach (var k in dataRequest) {
                    if (datastring.Length > 0) {
                        datastring.Append("&");
                    }

                    datastring.Append(WebUtility.UrlEncode(k.Key)).Append("=").Append(WebUtility.UrlEncode(k.Value));
                }

                string response = HTTPUtility
                    .PostRequest(PluginSettings.Settings.Links.Site + "/index.php", datastring.ToString(), header)
                    .Replace("\n", " ");

                tempSerials = Regex.Matches(response,
                        "<a data-id=\"(.*?)\".*?href=\"(.*?)\".*?>(.*?)<",
                        RegexOptions.Multiline)
                    .ToList();

                if (tempSerials.Count > 0) {
                    Seasonvar.SERIAL_MATCHES.Add((lang + sort), tempSerials);
                }
            }

            if (tempSerials != null) {
                for (int i = 0; i < Math.Min(50, tempSerials.Count); i++) {
                    var item = new GetSerialInfoCommand().GetItem(tempSerials[i].Groups[1].Value,
                        tempSerials[i].Groups[3].Value);

                    item.Link = GetSerialListCommand.CreateLink(tempSerials[i].Groups[2].Value);

                    playList.Items.Add(item);
                }

                if (tempSerials.Count > 50) {
                    playList.NextPageUrl = CreateLink(lang, sort, 50);
                }
            }
        }

        public static string CreateLink(string lang = default, string sort = default, int page = default) {
            var data = new Dictionary<string, object>() {
                {Seasonvar.KEY, KEY},
                {LANG_KEY, lang},
                {PAGE_KEY, page},
                {SORT_KEY, sort},
            };
            return Seasonvar.CreateLink(data);
        }
    }
}
