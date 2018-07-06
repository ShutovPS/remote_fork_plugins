using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetFilmCommand : ICommand {
        public const string KEY = "getfilm";

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

            var header = new Dictionary<string, string>() {
                {"Referer", url}
            };
            string moonwalkUrl = url;
            string moonwalkResponse = HTTPUtility.GetRequest(moonwalkUrl, header);

            items.AddRange(GetSerialTranslationsData(moonwalkUrl, data[3], moonwalkResponse));

            return items;
        }

        private static IEnumerable<Item> GetSerialTranslationsData(string url, string referer,
            string moonwalkResponse) {
            var items = new List<Item>();

            var regex = new Regex(PluginSettings.Settings.Regexp.Translations);

            if (regex.IsMatch(moonwalkResponse)) {
                string translations = regex.Match(moonwalkResponse).Groups[2].Value;

                var baseItem = new Item() {
                    Type = ItemType.DIRECTORY,
                    ImageLink = PluginSettings.Settings.Icons.IcoFolder
                };

                regex = new Regex(PluginSettings.Settings.Regexp.Translation);
                if (regex.Matches(translations).Count > 1) {
                    foreach (Match match in regex.Matches(translations)) {
                        var item = new Item(baseItem) {
                            Name = match.Groups[6].Value,
                            Link =
                                $"{KEY}{PluginSettings.Settings.Separator}seasons{PluginSettings.Settings.Separator}{match.Groups[3].Value}{PluginSettings.Settings.Separator}{referer}"
                        };
                        items.Add(item);
                    }

                    return items;
                }
            }

            return GetSerialSeasonsData(url, referer, moonwalkResponse);
        }

        private static IEnumerable<Item> GetSerialSeasons(params string[] data) {
            var header = new Dictionary<string, string>() {
                {"Referer", WebUtility.UrlDecode(data[4])}
            };
            string url = WebUtility.UrlDecode(data[3]);
            if (!url.Contains("://")) {
                url = $"{PluginSettings.Settings.Links.Site}/serial/{data[3]}/iframe";
            }

            string response = HTTPUtility.GetRequest(url, header);

            return GetSerialSeasonsData(url, data[4], response);
        }

        private static IEnumerable<Item> GetSerialSeasonsData(string url, string referer, string moonwalkResponse) {
            var items = new List<Item>();

            var regex = new Regex(PluginSettings.Settings.Regexp.Seasons);

            if (regex.IsMatch(moonwalkResponse)) {
                var seasons = regex.Match(moonwalkResponse).Groups[2].Value.Split(',');

                var baseItem = new Item() {
                    Type = ItemType.DIRECTORY,
                    ImageLink = PluginSettings.Settings.Icons.IcoFolder
                };

                if (seasons.Length > 0) {
                    foreach (string season in seasons) {
                        string seasonUrl = $"{url}?season={season}";
                        var item = new Item(baseItem) {
                            Name = $"Сезон {season}",
                            Link =
                                $"{KEY}{PluginSettings.Settings.Separator}series{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(seasonUrl)}{PluginSettings.Settings.Separator}{referer}"
                        };
                        items.Add(item);
                    }

                    return items;
                }
            }

            return GetSerialSeriesData(url, referer, moonwalkResponse);
        }

        private static IEnumerable<Item> GetSerialSeries(params string[] data) {
            var header = new Dictionary<string, string>() {
                {"Referer", WebUtility.UrlDecode(data[4])}
            };
            string url = WebUtility.UrlDecode(data[3]);

            string response = HTTPUtility.GetRequest(url, header);

            return GetSerialSeriesData(url, data[4], response);
        }

        private static IEnumerable<Item> GetSerialSeriesData(string url, string referer, string moonwalkResponse) {
            var items = new List<Item>();

            var regex = new Regex(PluginSettings.Settings.Regexp.Episodes);

            if (regex.IsMatch(moonwalkResponse)) {
                var episodes = regex.Match(moonwalkResponse).Groups[2].Value.Split(',');

                var baseItem = new Item() {
                    Type = ItemType.DIRECTORY,
                    ImageLink = PluginSettings.Settings.Icons.IcoFolder
                };

                if (episodes.Length > 0) {
                    foreach (string episode in episodes) {
                        string episodeUrl = $"{url}{(url.Contains("?") ? "&" : "?")}episode={episode}";
                        var item = new Item(baseItem) {
                            Name = $"Серия {episode}",
                            Link =
                                $"{GetEpisodeCommand.KEY}{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(episodeUrl)}{PluginSettings.Settings.Separator}{referer}"
                        };
                        items.Add(item);
                    }

                    return items;
                }
            }

            return GetEpisodeCommand.GetEpisodes(url, referer);
        }
    }
}
