using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
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
                case "playlist":
                    items.AddRange(GetSerialSeries(data));
                    break;
            }

            return items;
        }

        private static IEnumerable<Item> GetSerialTranslations(params string[] data) {
            var items = new List<Item>();

            string url = string.Concat(PluginSettings.Settings.Links.Site, "/api/movies/player_data");
            string requestData = $"post_id={WebUtility.UrlDecode(data[3])}&showfull=true";

            var header = new Dictionary<string, string>() {
                {"X-Requested-With", "XMLHttpRequest"}
            };

            string response = HTTPUtility.PostRequest(url, requestData, header);

            items.AddRange(GetSerialTranslationsData(response));

            return items;
        }

        private static IEnumerable<Item> GetSerialTranslationsData(string response) {
            var items = new List<Item>();

            FilmModel filmModel = null;

            try {
                filmModel = JsonConvert.DeserializeObject<FilmModel>(response);
            } catch (Exception exception) {
                Filmix.Logger.LogError(exception);
            }

            var baseItem = new Item() {
                Type = ItemType.DIRECTORY,
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };

            if (filmModel != null) {
                var html5 = filmModel.Message.Translations.Html5;
                if (html5 != null && html5.Count > 0) {
                    if (html5.Count > 1) {
                        foreach (var translation in html5) {
                            var item = new Item(baseItem) {
                                Name = translation.Key
                            };
                            if (filmModel.Message.Translations.Pl == "yes") {
                                item.Link =
                                    $"{KEY}{PluginSettings.Settings.Separator}playlist{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(Html5Decode(translation.Value))}";
                            } else {
                                item.Link =
                                    $"{GetEpisodeCommand.KEY}{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(Html5Decode(translation.Value))}";
                            }
                            items.Add(item);
                        }
                    } else {
                        items.AddRange(
                            GetEpisodeCommand.GetEpisodes(Html5Decode(html5.First().Value)));
                    }
                }
            }

            return items;
        }

        private static IEnumerable<Item> GetSerialSeries(params string[] data) {
            string url = WebUtility.UrlDecode(data[3]);

            url = url.Substring(url.IndexOf("http", StringComparison.Ordinal));

            string response = HTTPUtility.GetRequest(url);

            return GetSerialSeriesData(Html5Decode(response));
        }

        private static IEnumerable<Item> GetSerialSeriesData(string response) {
            var items = new List<Item>();

            SerialModel filmModel = null;
            
            try {
                filmModel = JsonConvert.DeserializeObject<SerialModel>(response);
            } catch (Exception exception) {
                Filmix.Logger.LogError(exception);
            }

            if (filmModel != null) {
                var baseItem = new Item() {
                    Type = ItemType.DIRECTORY,
                    ImageLink = PluginSettings.Settings.Icons.IcoFolder
                };

                foreach (var seasons in filmModel.Playlist) {
                    foreach (var episode in seasons.Playlist) {
                        var item = new Item(baseItem) {
                            Name = string.Format("Сезон {0} Серия {1}", episode.Season, episode.SerieId),
                            Link =
                                $"{GetEpisodeCommand.KEY}{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(episode.File)}"
                        };
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        private static string Html5Decode(string data) {
            if (!data.StartsWith(".")) {
                data = data.Substring(1);
                string tempString = string.Empty;
                for (int j = 0; j < data.Length; j += 3) {
                    tempString += "\\u0" + data.Substring(j, 3);
                }
                data = ReplaceUnicodeSymbols(tempString);
            }
            return data;
        }

        private static string ReplaceUnicodeSymbols(string text) {
            return Regex.Replace(text, @"\\u([\dA-Fa-f]{4})", v => ((char)Convert.ToInt32(v.Groups[1].Value, 16)).ToString());
        }
    }
}
