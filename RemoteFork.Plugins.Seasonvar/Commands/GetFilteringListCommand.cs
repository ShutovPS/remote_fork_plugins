using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using RemoteFork.Network;

namespace RemoteFork.Plugins {
    class GetFilteringListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            List<Item> items = new List<Item>();

            string lang = data.Length > 1 ? data[1] : string.Empty;
            string sort = data.Length > 2 ? data[2] : string.Empty;
            string page = data.Length > 3 ? data[3] : string.Empty;

            if (sort == "first") {
                if (!Seasonvar.SERIAL_MATCHES.ContainsKey(lang + "name")) {
                    data[2] = "name";
                    new GetFilteringListCommand().GetItems(context, data);
                }
                if (Seasonvar.SERIAL_MATCHES.ContainsKey(lang + "name")) {
                    items.AddRange(new FirstSymbolGroupCommand().GetItems(context, data));

                    return items;
                } else {
                    sort = "name";
                }
            } else if (!string.IsNullOrEmpty(page)) {
                if (Seasonvar.SERIAL_MATCHES.ContainsKey(lang + sort)) {
                    items.AddRange(new NextPageCommand().GetItems(context, data));

                    return items;
                }
            }

            if (string.IsNullOrEmpty(sort)) {
                sort = "view";

                var item = new Item() {
                    Name = "По популярности",
                    Link = string.Format("{1}{0}{2}", Seasonvar.SEPARATOR, lang, "view")
                };
                items.Add(item);
                item = new Item() {
                    Name = "По названию",
                    Link = string.Format("{1}{0}{2}", Seasonvar.SEPARATOR, lang, "name")
                };
                items.Add(item);
                item = new Item() {
                    Name = "По году",
                    Link = string.Format("{1}{0}{2}", Seasonvar.SEPARATOR, lang, "god")
                };
                items.Add(item);
                item = new Item() {
                    Name = "По добавлению",
                    Link = string.Format("{1}{0}{2}", Seasonvar.SEPARATOR, lang, "newest")
                };
                items.Add(item);
                item = new Item() {
                    Name = "По первому символу",
                    Link = string.Format("{1}{0}{2}", Seasonvar.SEPARATOR, lang, "first")
                };
                items.Add(item);
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
                    .PostRequest(string.Format(Seasonvar.SITE_URL, "/index.php"), datastring.ToString(), header)
                    .Replace("\n", " ");
                //context.ConsoleLog(string.Format(Seasonvar.SITE_URL, "/index.php") + " datastring=" + datastring);

                tempSerials = Regex.Matches(response,
                        "<a data-id=\"(.*?)\".*?href=\"(.*?)\".*?>(.*?)<",
                        RegexOptions.Multiline)
                    .ToList();

                //context.ConsoleLog("tempSerials.Count=" + tempSerials.Count);
                //
                if (tempSerials.Count > 0) {
                    Seasonvar.SERIAL_MATCHES.Add((lang + sort), tempSerials);
                }
            }

            if (tempSerials != null) {
                for (int i = 0; i < Math.Min(50, tempSerials.Count); i++) {
                    var item = new GetSerialInfoCommand().GetItem(context, tempSerials[i].Groups[1].Value);
                    item.Name = tempSerials[i].Groups[3].Value.Trim();
                    item.Link = string.Format("{1}{0}{2}", Seasonvar.SEPARATOR, "list", tempSerials[i].Groups[2]);

                    items.Add(item);
                }

                if (tempSerials.Count > 50) {
                    Seasonvar.NextPageUrl = string.Format("{1}{0}{2}{0}{3}", Seasonvar.SEPARATOR, lang, sort, 50);
                    //var item = new Item() {
                    //    Name = string.Format(Seasonvar.PAGE, 2),
                    //    Link = string.Format("{1}{0}{2}{0}{3}", Seasonvar.SEPARATOR, lang, sort, 50),
                    //    ImageLink = Seasonvar.NEXT_PAGE_IMAGE_URL
                    //};

                    //items.Add(item);
                }
            }

            return items;
        }
    }
}
