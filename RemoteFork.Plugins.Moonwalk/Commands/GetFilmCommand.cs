using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using RemoteFork.Items;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetFilmCommand : ICommand {
        public const string KEY = "getfilm";

        public const string TYPE_KEY = "type";
        public const string URL_KEY = "url";
        public const string REFERER_KEY = "referer";

        public const string TRANSLATIONS_KEY = "translations";
        public const string SEASONS_KEY = "seasons";
        public const string SERIES_KEY = "series";

        public void GetItems(PlayList playList, IPluginContext context, Dictionary<string, string> data) {
            string type;

            data.TryGetValue(TYPE_KEY, out type);

            switch (type) {
                case TRANSLATIONS_KEY:
                    GetSerialTranslations(playList, data);
                    break;
                case SEASONS_KEY:
                    GetSerialSeasons(playList, data);
                    break;
                case SERIES_KEY:
                    GetSerialSeries(playList, data);
                    break;
            }
        }

        private static void GetSerialTranslations(PlayList playList, Dictionary<string, string> data) {
            string url;

            data.TryGetValue(URL_KEY, out url);

            url = WebUtility.UrlDecode(url);

            var header = new Dictionary<string, string>() {
                {"Referer", url}
            };

            string moonwalkUrl = url;
            string moonwalkResponse = HTTPUtility.GetRequest(moonwalkUrl, header);

            GetSerialTranslationsData(playList, moonwalkUrl, url, moonwalkResponse);
        }

        private static void GetSerialTranslationsData(PlayList playList, string url, string referer,
            string moonwalkResponse) {
            var regex = new Regex(PluginSettings.Settings.Regexp.Translations);

            if (regex.IsMatch(moonwalkResponse)) {
                string translations = regex.Match(moonwalkResponse).Groups[2].Value;

                var baseItem = new DirectoryItem() {
                    ImageLink = PluginSettings.Settings.Icons.IcoFolder
                };

                regex = new Regex(PluginSettings.Settings.Regexp.Translation);
                if (regex.Matches(translations).Count > 1) {
                    foreach (Match match in regex.Matches(translations)) {
                        var item = new DirectoryItem(baseItem) {
                            Title = match.Groups[6].Value,
                            Link = CreateLink("seasons", match.Groups[3].Value, referer)
                        };
                        playList.Items.Add(item);
                    }

                    return;
                }
            }

            GetSerialSeasonsData(playList, url, referer, moonwalkResponse);
        }

        private static void GetSerialSeasons(PlayList playList, Dictionary<string, string> data) {
            string url;
            string referer;

            data.TryGetValue(URL_KEY, out url);
            data.TryGetValue(REFERER_KEY, out referer);

            url = WebUtility.UrlDecode(url);
            referer = WebUtility.UrlDecode(referer);

            var header = new Dictionary<string, string>() {
                {"Referer", referer}
            };

            if (!url.Contains("://")) {
                url = $"{PluginSettings.Settings.Links.Site}/serial/{url}/iframe";
            }

            string response = HTTPUtility.GetRequest(url, header);

            GetSerialSeasonsData(playList, url, referer, response);
        }

        private static void GetSerialSeasonsData(PlayList playList, string url, string referer,
            string moonwalkResponse) {
            var regex = new Regex(PluginSettings.Settings.Regexp.Seasons);

            if (regex.IsMatch(moonwalkResponse)) {
                var seasons = regex.Match(moonwalkResponse).Groups[2].Value.Split(',');

                var baseItem = new DirectoryItem() {
                    ImageLink = PluginSettings.Settings.Icons.IcoFolder
                };

                if (seasons.Length > 0) {
                    foreach (string season in seasons) {
                        string seasonUrl = $"{url}?season={season}";
                        var item = new DirectoryItem(baseItem) {
                            Title = $"Сезон {season}",
                            Link = CreateLink("series", seasonUrl, referer)
                        };
                        playList.Items.Add(item);
                    }

                    return;
                }
            }

            GetSerialSeriesData(playList, url, referer, moonwalkResponse);
        }

        private static void GetSerialSeries(PlayList playList, Dictionary<string, string> data) {
            string referer;
            string url;

            data.TryGetValue(REFERER_KEY, out referer);
            data.TryGetValue(URL_KEY, out url);

            url = WebUtility.UrlDecode(url);
            referer = WebUtility.UrlDecode(referer);

            var header = new Dictionary<string, string>() {
                {"Referer", referer}
            };

            string response = HTTPUtility.GetRequest(url, header);

            GetSerialSeriesData(playList, url, referer, response);
        }

        private static void GetSerialSeriesData(PlayList playList, string url, string referer,
            string moonwalkResponse) {
            var regex = new Regex(PluginSettings.Settings.Regexp.Episodes);

            if (regex.IsMatch(moonwalkResponse)) {
                var episodes = regex.Match(moonwalkResponse).Groups[2].Value.Split(',');

                var baseItem = new DirectoryItem() {
                    ImageLink = PluginSettings.Settings.Icons.IcoFolder
                };

                if (episodes.Length > 0) {
                    foreach (string episode in episodes) {
                        string episodeUrl = $"{url}{(url.Contains("?") ? "&" : "?")}episode={episode}";
                        var item = new DirectoryItem(baseItem) {
                            Title = $"Серия {episode}",
                            Link = GetEpisodeCommand.CreateLink(episodeUrl, referer)
                        };
                        playList.Items.Add(item);
                    }

                    return;
                }
            }

            GetEpisodeCommand.GetEpisodes(playList, url, referer);
        }

        public static string CreateLink(string type, string url, string referer) {
            var data = new Dictionary<string, object>() {
                {Moonwalk.KEY, KEY},
                {TYPE_KEY, type},
                {URL_KEY, WebUtility.UrlEncode(url)},
                {REFERER_KEY, WebUtility.UrlEncode(referer)},
            };

            return Moonwalk.CreateLink(data);
        }
    }
}
