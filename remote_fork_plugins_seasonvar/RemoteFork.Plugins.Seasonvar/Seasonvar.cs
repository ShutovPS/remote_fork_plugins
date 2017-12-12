using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace RemoteFork.Plugins {
    [PluginAttribute(Id = "seasonvar", Version = "0.4.1", Author = "fd_crash&&forkplayer", Name = "Seasonvar",
        Description = "Сериалы ТУТ! Сериалы онлайн смотреть бесплатно. Смотреть онлайн",
        ImageLink = "http://cdn.seasonvar.ru/images/fav/apple-touch-icon-144x144.png")]
    public class Seasonvar : IPlugin {
        private static readonly Dictionary<string, List<Match>> serialMatches = new Dictionary<string, List<Match>>();
        private static readonly Dictionary<string, Item> serialItems = new Dictionary<string, Item>();

        private const char SEPARATOR = ';';
        private const string PLUGIN_PATH = "pluginPath";
        private const string SITE_URL = "http://seasonvar.ru{0}";
        private const string IMAGE_URL = "http://cdn.seasonvar.ru/oblojka/{0}.jpg";
        private const string SMALL_IMAGE_URL = "http://cdn.seasonvar.ru/oblojka/small/{0}.jpg";
        private const string NEXT_PAGE_IMAGE_URL = "http://www.rasputins.ca/images/right.png";

        // Item представляет собой класс, содержащащий следующие поля:
        //// Name - навзание
        //// Link - ссылка (если директория, то ссылка передается на обработку плагину, если файл, то ссылка открывается в проигрывателе)
        //// ImageLink - иконка
        //// Description - описание, поддерживает HTML формат
        //// Type - тип сущности: ItemType.FILE или ItemType.DIRECTORY (по умолчанию)

        // Главный метод для обработки запросов
        public Playlist GetList(IPluginContext context) {
            string path = context.GetRequestParams().Get(PLUGIN_PATH);

            path = path == null ? "plugin" : "plugin;" + path;
            context.ConsoleLog("path=" + path);
            // SEPARATOR служит для разделения команд при парсинге (char)
            var arg = path.Split(SEPARATOR);

            List<Item> items = new List<Item>();

            switch (arg.Length) {
                case 0:
                    break;
                case 1:
                    items.AddRange(GetRootListRequest());
                    break;
                default:
                    string lang = arg[1];
                    string sort = string.Empty;
                    string page = string.Empty;

                    switch (arg[1]) {
                        case "eng": {
                            if (arg.Length > 2) {
                                sort = arg[2];
                            }
                            if (arg.Length > 3) {
                                page = arg[3];
                            }

                            items.AddRange(GetFilteringListRequest(context, lang, sort, page));
                        }
                            break;
                        case "rus": {
                            if (arg.Length > 2) {
                                sort = arg[2];
                            }
                            if (arg.Length > 3) {
                                page = arg[3];
                            }

                            items.AddRange(GetFilteringListRequest(context, lang, sort, page));
                        }
                            break;
                        case "series":
                            items.AddRange(GetSeriesListRequest(context, arg[2]));
                            break;
                        case "list":
                            items.AddRange(GetSerialListRequest(context, arg[2]));
                            break;
                        case "voise":
                            items.AddRange(GetVoiseListRequest(context, arg[2]));
                            break;
                        case "update":
                            ClearList();
                            break;
                    }
                    break;
            }

            var playlist = new Playlist();

            foreach (var item in items) {
                if (ItemType.DIRECTORY == item.Type) {
                    var pluginParams = new NameValueCollection();

                    pluginParams[PLUGIN_PATH] = item.Link;

                    item.Link = context.CreatePluginUrl(pluginParams);
                }
            }

            playlist.Items = items.ToArray();

            return playlist;
        }

        #region GetRootListRequest

        private Item[] GetRootListRequest() {
            List<Item> items = new List<Item>();

            var item = new Item() {
                Name = "Зарубежные",
                Link = "eng"
            };
            items.Add(item);
            item = new Item() {
                Name = "Отечественные",
                Link = "rus"
            };
            items.Add(item);

            item = new Item() {
                Name = "Обновить список",
                Link = "update"
            };
            items.Add(item);

            return items.ToArray();
        }

        #endregion GetRootListRequest

        #region GetFilteringListRequest

        private Item[] GetFilteringListRequest(IPluginContext context, string lang, string sort, string page) {
            List<Item> items = new List<Item>();

            if (sort == "first") {
                if (!serialMatches.ContainsKey(lang + "name")) {
                    GetFilteringListRequest(context, lang, "name", page);
                }
                if (serialMatches.ContainsKey(lang + "name")) {
                    items.AddRange(FirstSymbolGroup(lang, sort, page));

                    return items.ToArray();
                } else {
                    sort = "name";
                }
            } else if (!string.IsNullOrEmpty(page)) {
                if (serialMatches.ContainsKey(lang + sort)) {
                    items.AddRange(NextPage(lang, sort, page));

                    return items.ToArray();
                }
            }

            if (string.IsNullOrEmpty(sort)) {
                sort = "view";

                var item = new Item() {
                    Name = "По популярности",
                    Link = string.Format("{1}{0}{2}", SEPARATOR, lang, "view")
                };
                items.Add(item);
                item = new Item() {
                    Name = "По названию",
                    Link = string.Format("{1}{0}{2}", SEPARATOR, lang, "name")
                };
                items.Add(item);
                item = new Item() {
                    Name = "По году",
                    Link = string.Format("{1}{0}{2}", SEPARATOR, lang, "god")
                };
                items.Add(item);
                item = new Item() {
                    Name = "По добавлению",
                    Link = string.Format("{1}{0}{2}", SEPARATOR, lang, "newest")
                };
                items.Add(item);
                item = new Item() {
                    Name = "По первому символу",
                    Link = string.Format("{1}{0}{2}", SEPARATOR, lang, "first")
                };
                items.Add(item);
            }

            List<Match> tempSerials;

            if (serialMatches.ContainsKey(lang + sort)) {
                tempSerials = serialMatches[lang + sort];
            } else {
                var data = new Dictionary<string, string>() {
                    {"filter[only]", lang},
                    {"filter[rait]", "kp"},
                    {"filter[sortTo][]", sort},
                    {"filter[block]", "yes"},
                };
                var header = new Dictionary<string, string>() {
                    {"Accept-Encoding", "gzip, deflate, lzma"},
                    {"Content-Type", "text/html; charset=UTF-8"}
                };
                string datastring = "";
                foreach (var k in data) {
                    if (datastring != "") datastring += "&";
                    datastring += System.Net.WebUtility.UrlEncode(k.Key) + "=" +
                                  System.Net.WebUtility.UrlEncode(k.Value);
                }
                string response = context.GetHttpClient()
                    .PostRequest(string.Format(SITE_URL, "/index.php"), datastring, header).Replace("\n", " ");
                context.ConsoleLog(string.Format(SITE_URL, "/index.php") + " datastring=" + datastring);
                //context.ConsoleLog("response=" + response.Substring(0,400));
                tempSerials = Regex.Matches(response,
                        "<a data-id=\"(.*?)\".*?href=\"(.*?)\".*?>(.*?)<",
                        RegexOptions.Multiline)
                    .Cast<Match>()
                    .ToList();

                context.ConsoleLog("tempSerials.Count=" + tempSerials.Count);

                if (tempSerials.Count > 0) {
                    serialMatches.Add((lang + sort), tempSerials);
                }
            }

            if (tempSerials != null) {
                for (int i = 0; i < Math.Min(50, tempSerials.Count); i++) {
                    var item = GetSerialInfoRequest(tempSerials[i].Groups[1].Value);
                    item.Name = tempSerials[i].Groups[3].Value.Trim();
                    item.Link = string.Format("{1}{0}{2}", SEPARATOR, "list", tempSerials[i].Groups[2]);

                    items.Add(item);
                }

                if (tempSerials.Count > 50) {
                    var item = new Item() {
                        Name = "СТРАНИЦА 2",
                        Link = string.Format("{1}{0}{2}{0}{3}", SEPARATOR, lang, sort, 50),
                        ImageLink = NEXT_PAGE_IMAGE_URL
                    };

                    items.Add(item);
                }
            }

            return items.ToArray();
        }

        private Item[] NextPage(string lang, string sort, string page) {
            List<Item> items = new List<Item>();

            var tempSerials = serialMatches[lang + sort];

            int id;
            if (int.TryParse(page, out id)) {
                for (int i = id; i < Math.Min(50 + id, Math.Min(tempSerials.Count, tempSerials.Count + id)); i++) {
                    var item = GetSerialInfoRequest(tempSerials[i].Groups[1].Value);
                    item.Name = tempSerials[i].Groups[3].Value.Trim();
                    item.Link = string.Format("{1}{0}{2}", SEPARATOR, "list", tempSerials[i].Groups[2]);

                    items.Add(item);
                }

                if (tempSerials.Count - id > 50) {
                    var item = new Item() {
                        Name = "СТРАНИЦА " + (id / 50 + 2),
                        Link = string.Format("{1}{0}{2}{0}{3}", SEPARATOR, lang, sort, id + 50),
                        ImageLink = NEXT_PAGE_IMAGE_URL
                    };

                    items.Add(item);
                }
            }

            return items.ToArray();
        }

        private Item[] FirstSymbolGroup(string lang, string sort, string page) {
            List<Item> items = new List<Item>();

            var tempSerials = serialMatches[lang + "name"];
            var group = tempSerials.GroupBy(i => i.Groups[3].Value.Trim().Replace("\"", "#").First());

            if (string.IsNullOrEmpty(page)) {
                foreach (var g in group) {
                    var item = new Item() {
                        Name = g.Key.ToString().Trim(),
                        Link = string.Format("{1}{0}{2}{0}{3}", SEPARATOR, lang, "first", g.Key)
                    };
                    items.Add(item);
                }
                items.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
            } else {
                var g = group.FirstOrDefault(i => i.Key == page.First());
                if (g != null) {
                    foreach (var gg in g) {
                        var item = GetSerialInfoRequest(gg.Groups[1].Value);
                        item.Name = gg.Groups[3].Value.Trim();
                        item.Link = string.Format("{1}{0}{2}", SEPARATOR, "list", gg.Groups[2]);

                        items.Add(item);
                    }
                }
            }

            return items.ToArray();
        }

        private static void ClearList() {
            serialMatches.Clear();
        }

        #endregion GetFilteringListRequest

        #region GetSerialListRequest

        private Item[] GetSerialListRequest(IPluginContext context, string url) {
            List<Item> items = new List<Item>();

            var header = new Dictionary<string, string>() {
                {"Accept-Encoding", "gzip, deflate, lzma"},
                {"Content-Type", "text/html; charset=UTF-8"}
            };

            string response = context.GetHttpClient().GetRequest(string.Format(SITE_URL, url), header);

            var matches = Regex.Matches(response,
                "(<h2>[.\\s^&]*?<a href=\")((\\S*?)(\\/serial-(\\d+))(\\S*?))(\">[\\s>]*?Сериал)(.+?)(\\s*?(<span>|<\\/a>))",
                RegexOptions.Multiline);

            if (matches.Count == 1) {
                return GetVoiseListRequest(context, url);
            } else {
                for (int i = 0; i < matches.Count; i++) {
                    Item item = GetSerialInfoRequest(matches[i].Groups[5].Value);
                    item.Name = matches[i].Groups[8].Value.Trim();
                    item.Link = string.Format("{1}{0}{2}", SEPARATOR, "voise", matches[i].Groups[2].Value);

                    items.Add(item);
                }
            }

            return items.ToArray();
        }

        #endregion GetSerialListRequest

        #region GetVoiseListRequest

        private Item[] GetVoiseListRequest(IPluginContext context, string url) {
            List<Item> items = new List<Item>();

            var header = new Dictionary<string, string>() {
                {"Accept-Encoding", "gzip, deflate, lzma"},
                {"Content-Type", "text/html; charset=UTF-8"}
            };

            context.ConsoleLog("url=" + string.Format(SITE_URL, url));
            string response = context.GetHttpClient().GetRequest(string.Format(SITE_URL, url), header)
                .Replace("\n", " ");

            //  context.ConsoleLog("response=" + response.Substring(0, 1000));
            var match = Regex.Match(response, "'(secureMark)': '(.*?)'.*?'time': (\\d+)");
            if (match.Success) {
                string secure = match.Groups[2].Value;
                string time = match.Groups[3].Value;
                context.ConsoleLog("secure=" + secure);
                context.ConsoleLog("time=" + time);

                match = Regex.Match(response, "data-id-serial=\"(.*?)\"");
                if (match.Success) {
                    string serialId = match.Groups[1].Value;
                    context.ConsoleLog("serialId=" + serialId);

                    match = Regex.Match(response, "data-id-season=\"(.*?)\"");
                    if (match.Success) {
                        string seasonId = match.Groups[1].Value;

                        context.ConsoleLog("seasonId=" + seasonId);
                        Item Item = GetSerialInfoRequest(seasonId, response);

                        var data = new Dictionary<string, string>() {
                            {"type", "html5"},
                            {"id", seasonId},
                            {"serial", serialId},
                            {"secure", secure},
                            {"time", time}
                        };
                        header = new Dictionary<string, string>() {
                            {"X-Requested-With", "XMLHttpRequest"},
                            {"Accept-Encoding", "gzip, deflate, lzma"},
                            {"Content-Type", "text/html; charset=UTF-8"}
                        };
                        string datastring = "";
                        foreach (var k in data) {
                            if (datastring != "") datastring += "&";
                            datastring += System.Net.WebUtility.UrlEncode(k.Key) + "=" +
                                          System.Net.WebUtility.UrlEncode(k.Value);
                        }

                        context.ConsoleLog(string.Format(SITE_URL, "/player.php") + " datastring=" + datastring);
                        response = context.GetHttpClient()
                            .PostRequest(string.Format(SITE_URL, "/player.php"), datastring, header).Replace("\n", "");
                        //context.ConsoleLog("response=" + response.Substring(0, 1000));
                        var matches0 = Regex.Matches(response, " pl = {'0': \"(.*?)\"");

                        var matches = Regex.Matches(response,
                            "data-translate=\"([^0].*?)\">(.*?)</li.{1,30}>pl\\[.*?\"(.*?)\"");
                        context.ConsoleLog("matches0=" + matches0.Count);
                        context.ConsoleLog("matches=" + matches.Count);
                        if (matches0.Count == 1 && matches.Count < 2) {
                            return GetSeriesListRequest(context, matches0[0].Groups[1].Value);
                        } else {

                            if (matches0.Count > 0) {
                                var item = new Item() {
                                    Name = "Стандартный",
                                    Link =
                                        string.Format("{1}{0}{2}", SEPARATOR, "series",
                                            Uri.UnescapeDataString(matches0[0].Groups[1].Value)),
                                    ImageLink = Item.ImageLink,
                                    Description = Item.Description
                                };
                                items.Add(item);
                            }
                            for (int i = 0; i < matches.Count; i++) {
                                var item = new Item() {
                                    Name = matches[i].Groups[2].Value.Trim(),
                                    Link =
                                        string.Format("{1}{0}{2}", SEPARATOR, "series",
                                            Uri.UnescapeDataString(matches[i].Groups[3].Value)),
                                    ImageLink = Item.ImageLink,
                                    Description = Item.Description
                                };

                                items.Add(item);
                            }
                        }
                    }
                }
            }

            return items.ToArray();
        }

        #endregion GetVoiseListRequest

        #region GetSeriesListRequest

        private Item[] GetSeriesListRequest(IPluginContext context, string url) {
            List<Item> items = new List<Item>();

            var header = new Dictionary<string, string>() {
                {"Accept-Encoding", "gzip, deflate, lzma"},
                {"Content-Type", "text/html; charset=UTF-8"}
            };
            context.ConsoleLog("url=" + string.Format(SITE_URL,
                                   url.Replace("transСтандартный", "trans")));
            string response =
                context.GetHttpClient().GetRequest(string.Format(SITE_URL,
                    url.Replace("transСтандартный", "trans")), header);

            var matches = Regex.Matches(response,
                "({)(\"title\"\\s*:\\s*\")(\\d+)(\\s+?)(.+?)(\")(.*?)(\"file\"\\s*:\\s*\")(.+?)(\")(.+?)(\"galabel\"\\s*:\\s*\")(.+?)(\")(.+?)(})",
                RegexOptions.Multiline);

            var match = Regex.Match(url, "(\\/)(\\d+)(\\/)");

            Item Item = GetSerialInfoRequest(match.Groups[2].Value);

            for (int i = 0; i < matches.Count; i++) {
                var item = new Item() {
                    Name = string.Format("{0} Серия", matches[i].Groups[3].Value, matches[i].Groups[19].Value),
                    Link = matches[i].Groups[9].Value,
                    Type = ItemType.FILE,
                    ImageLink = Item.ImageLink,
                    Description = Item.Description
                };

                items.Add(item);
            }

            return items.ToArray();
        }

        #endregion GetSeriesListRequest

        #region GetSerialInfoRequest

        private Item GetSerialInfoRequest(string id, string text = "") {
            Item item;

            if (string.IsNullOrEmpty(text)) {
                if (serialItems.ContainsKey(id)) {
                    item = serialItems[id];
                } else {
                    item = new Item() {
                        ImageLink = string.Format(SMALL_IMAGE_URL, id)
                    };
                    serialItems.Add(id, item);
                }
            } else {
                if (serialItems.ContainsKey(id)) {
                    item = serialItems[id];
                } else {
                    item = new Item() {
                        ImageLink = string.Format(SMALL_IMAGE_URL, id)
                    };
                    serialItems.Add(id, item);
                }
                if (string.IsNullOrEmpty(item.Description)) {
                    string description = string.Empty;

                    var match = Regex.Match(text, "(var\\s+id\\s*=\\s*\")(\\d+)(\";)");

                    if (match.Success) {
                        string seasonId = match.Groups[2].Value;

                        match = Regex.Match(text,
                            "(<div\\s+class\\s*=\\s*\"pg-s-rb\"\\s*>)([\\s\\S]*?)(serialId)");
                        if (match.Success) {
                            string descText = match.Groups[2].Value;

                            match = Regex.Match(descText,
                                "(<h1\\s+class\\s*=\\s*\"hname\"\\s*>)([\\s\\S]*?)(<\\/h1>)");
                            if (match.Success) {
                                string name = match.Groups[2].Value;
                                match = Regex.Match(descText, "(<p>)([\\s\\S]*?)(<\\/p>)");
                                if (match.Success) {
                                    description =
                                        string.Format(
                                            "<img src=\"{0}\" alt=\"\" align=\"left\"/><h3>{1}</h3>{2}",
                                            string.Format(IMAGE_URL, seasonId), name.Trim(), match.Groups[2].Value);
                                }
                            }
                        }

                        item.Description = description;
                    }
                }
            }

            return item;
        }

        #endregion GetSerialInfoRequest
    }
}
