using System.Collections.Generic;
using System.Net;
using System.Text;
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
                    GetSerialTranslations(items, data);
                    break;
                case "seasons":
                    GetSerialSeasons(items, data);
                    break;
                case "series":
                    GetSerialSeries(items, data);
                    break;
            }

            return items;
        }

        private static void GetSerialTranslations(List<Item> items, params string[] data) {
            string url = WebUtility.UrlDecode(data[3]);

            string response = HTTPUtility.GetRequest(url);

            var regex = new Regex(PluginSettings.Settings.Regexp.Iframe);
            if (regex.IsMatch((response))) {
                var header = new Dictionary<string, string>() {
                    {"Referer", url}
                };
                string moonwalkUrl = regex.Match(response).Groups[2].Value;
                string moonwalkResponse = HTTPUtility.GetRequest(moonwalkUrl, header);

                GetSerialTranslationsData(items, moonwalkUrl, data[3], response, moonwalkResponse);
            }
        }

        private static void GetSerialTranslationsData(List<Item> items, string url, string referer, string response,
            string moonwalkResponse) {
            var regex = new Regex(PluginSettings.Settings.Regexp.Translations);

            if (regex.IsMatch(moonwalkResponse)) {
                string translations = regex.Match(moonwalkResponse).Groups[2].Value;

                string description = GetDescription(response);

                var baseItem = new Item() {
                    Type = ItemType.DIRECTORY,
                    Description = description,
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

                    return;
                }
            }

            GetSerialSeasonsData(items, url, referer, moonwalkResponse);
        }

        private static void GetSerialSeasons(List<Item> items, params string[] data) {
            var header = new Dictionary<string, string>() {
                {"Referer", WebUtility.UrlDecode(data[4])}
            };
            string url = WebUtility.UrlDecode(data[3]);
            if (!url.Contains("://")) {
                url = $"{PluginSettings.Settings.Links.Moonwalk}/serial/{data[3]}/iframe";
            }

            string response = HTTPUtility.GetRequest(url, header);

            GetSerialSeasonsData(items, url, data[4], response);
        }

        private static void GetSerialSeasonsData(List<Item> items, string url, string referer, string moonwalkResponse) {
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

                    return;
                }
            }

            GetSerialSeriesData(items, url, referer, moonwalkResponse);
        }

        private static void GetSerialSeries(List<Item> items, params string[] data) {
            var header = new Dictionary<string, string>() {
                {"Referer", WebUtility.UrlDecode(data[4])}
            };
            string url = WebUtility.UrlDecode(data[3]);

            string response = HTTPUtility.GetRequest(url, header);

            GetSerialSeriesData(items, url, data[4], response);
        }

        private static void GetSerialSeriesData(List<Item> items, string url, string referer, string moonwalkResponse) {
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

                    return;
                }
            }

            GetEpisodeCommand.GetEpisodes(items, url, referer);
        }

        private static string GetDescription(string text) {
            string title = string.Empty;
            string image = string.Empty;
            string description = string.Empty;

            var regex = new Regex(PluginSettings.Settings.Regexp.MetaTitle);
            if (regex.IsMatch(text)) {
                title = regex.Match(text).Groups[2].Value;
            }

            regex = new Regex(PluginSettings.Settings.Regexp.MetaImage);
            if (regex.IsMatch(text)) {
                image = regex.Match(text).Groups[2].Value;
            }

            regex = new Regex(PluginSettings.Settings.Regexp.MiniDescription);
            if (regex.IsMatch(text)) {
                description = regex.Match(text).Groups[2].Value;
            }

            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(image)) {
                sb.AppendLine(
                    $"<div id=\"poster\" style=\"float: left; padding: 4px; background-color: #eeeeee; margin: 0px 13px 1px 0px;\"><img style=\"width: 180px; float: left;\" src=\"{image}\" /></div>");
            }

            sb.AppendLine($"<span style=\"color: #3366ff;\"><strong>{title}</strong></span><br>");
            sb.AppendLine(description);

            return sb.ToString();
        }
    }
}
