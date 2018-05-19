using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using RemoteFork.Network;

namespace RemoteFork.Plugins {
    public class GetSerialCommand : ICommand {
        public const string KEY = "getserial";

        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            string type = data[2];

            switch (type) {
                case "translations":
                    items.AddRange(GetSerialTranslations(data));
                    break;
                case "seasons":
                    items.AddRange(GetSerialSeasons(data));
                    break;
                case "series":
                    items.AddRange(GetSerialSeries(data));
                    break;
            }

            return items;
        }

        private static IEnumerable<Item> GetSerialTranslations(params string[] data) {
            var items = new List<Item>();

            string url = WebUtility.UrlDecode(data[3]);

            string response = HTTPUtility.GetRequest(url);

            var regex = new Regex("(<iframe.*?src=\")(.*?)(\")");
            if (regex.IsMatch((response))) {
                var header = new Dictionary<string, string>() {
                    {"Referer", url}
                };
                string moonwalkUrl = regex.Match(response).Groups[2].Value;
                string moonwalkResponse = HTTPUtility.GetRequest(moonwalkUrl, header);

                regex = new Regex("(translations:\\s*)(\\[\\[.*?\\]\\])");

                if (regex.IsMatch(moonwalkResponse)) {
                    string translations = regex.Match(moonwalkResponse).Groups[2].Value;

                    string description = GetDescription(response);

                    var baseItem = new Item() {
                        Type = ItemType.DIRECTORY,
                        Description = description,
                        ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246folder.png"
                    };

                    regex = new Regex("(\\[)(\")(.*?)(\")(,\")(.*?)(\"\\])");
                    if (regex.Matches(translations).Count > 1) {
                        foreach (Match match in regex.Matches(translations)) {
                            var item = new Item(baseItem) {
                                Name = match.Groups[6].Value,
                                Link = $"{KEY}{HDSerials.SEPARATOR}seasons{HDSerials.SEPARATOR}{match.Groups[3].Value}{HDSerials.SEPARATOR}{WebUtility.UrlEncode(url)}"
                            };
                            items.Add(item);
                        }
                    } else {
                        data[3] = url;
                        data[4] = WebUtility.UrlEncode(url);
                        items.AddRange(GetSerialSeasons(data));
                    }
                } else {
                    data[3] = url;
                    data[4] = WebUtility.UrlEncode(url);
                    items.AddRange(GetSerialSeasons(data));
                }
            }

            return items;
        }

        private static IEnumerable<Item> GetSerialSeasons(params string[] data) {
            var items = new List<Item>();

            var header = new Dictionary<string, string>() {
                {"Referer", WebUtility.UrlDecode(data[4])}
            };
            string url = WebUtility.UrlDecode(data[3]);
            if (!url.Contains("://")) {
                url = $"http://moonwalk.cc/serial/{data[3]}/iframe";
            }

            string response = HTTPUtility.GetRequest(url, header);

            var regex = new Regex("(seasons:\\s\\[)(.*?)(\\])");

            if (regex.IsMatch(response)) {
                var seasons = regex.Match(response).Groups[2].Value.Split(',');

                var baseItem = new Item() {
                    Type = ItemType.DIRECTORY,
                    ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246folder.png"
                };

                if (seasons.Length > 1) {
                    foreach (string season in seasons) {
                        string seasonUrl = $"{url}?season={season}";
                        var item = new Item(baseItem) {
                            Name = $"Сезон {season}",
                            Link = $"{KEY}{HDSerials.SEPARATOR}series{HDSerials.SEPARATOR}{WebUtility.UrlEncode(seasonUrl)}{HDSerials.SEPARATOR}{data[4]}"
                        };
                        items.Add(item);
                    }
                } else {
                    items.AddRange(GetSerialSeries(data));
                }
            } else {
                items.AddRange(GetSerialSeries(data));
            }

            return items;
        }

        private static IEnumerable<Item> GetSerialSeries(params string[] data) {
            var items = new List<Item>();

            var header = new Dictionary<string, string>() {
                {"Referer", WebUtility.UrlDecode(data[4])}
            };
            string url = WebUtility.UrlDecode(data[3]);

            string response = HTTPUtility.GetRequest(url, header);

            var regex = new Regex("(episodes:\\s\\[)(.*?)(\\])");

            if (regex.IsMatch(response)) {
                var episodes = regex.Match(response).Groups[2].Value.Split(',');

                var baseItem = new Item() {
                    Type = ItemType.DIRECTORY,
                    ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597246folder.png"
                };

                if (episodes.Length > 1) {
                    foreach (string episode in episodes) {
                        string episodeUrl = $"{url}{(url.Contains("?") ? "&" : "?")}episode={episode}";
                        var item = new Item(baseItem) {
                            Name = $"Серия {episode}",
                            Link =
                                $"{GetEpisodeCommand.KEY}{HDSerials.SEPARATOR}{GetEpisodeCommand.KEY}{HDSerials.SEPARATOR}{WebUtility.UrlEncode(episodeUrl)}{HDSerials.SEPARATOR}{data[4]}"
                        };
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        private static string GetDescription(string text) {
            string title = string.Empty;
            string image = string.Empty;
            string description = string.Empty;

            var regex = new Regex("(<meta property=\"og:title\" content=\")(.*?)(\">)");
            if (regex.IsMatch(text)) {
                title = regex.Match(text).Groups[2].Value;
            }
            regex = new Regex("(<meta property=\"og:image\" content=\")(.*?)(\">)");
            if (regex.IsMatch(text)) {
                image = regex.Match(text).Groups[2].Value;
            }
            regex = new Regex("(<div class=\"full-news-content\">.*?\"><\\/span>\\s*)(.*)");
            if (regex.IsMatch(text)) {
                description = regex.Match(text).Groups[2].Value;
            }

            description = $"<img src=\"{image}\" alt=\"\" align=\"left\" style=\"width:240px;float:left;\"/></div><span style=\"color:#3090F0\">{title}</span><br>{description}";

            return description;
        }
    }
}
