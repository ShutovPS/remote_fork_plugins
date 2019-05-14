using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
                    GetSerialTranslations(items, data);
                    break;
                case "playlist":
                    GetSerialSeries(items, data);
                    break;
            }

            return items;
        }

        private static void GetSerialTranslations(List<Item> items, params string[] data) {
            string url = string.Concat(PluginSettings.Settings.Links.Site, "/api/movies/player_data");
            string requestData = $"post_id={WebUtility.UrlDecode(data[3])}&showfull=true";

            var header = new Dictionary<string, string>() {
                {"X-Requested-With", "XMLHttpRequest"}
            };

            string response = HTTPUtility.PostRequest(url, requestData, header);

            GetSerialTranslationsData(items, response);
        }

        private static void GetSerialTranslationsData(List<Item> items, string response) {
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
                var video = filmModel.Message.Translations.Video;
                if (video != null && video.Count > 0) {
                    foreach (var translation in video) {
                        var item = new Item(baseItem) {
                            Name = translation.Key
                        };
                        if (filmModel.Message.Translations.Pl == "yes") {
                            item.Link =
                                $"{KEY}{PluginSettings.Settings.Separator}playlist{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(LinkDecode(translation.Value))}";
                        } else {
                            item.Link =
                                $"{GetEpisodeCommand.KEY}{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(LinkDecode(translation.Value))}";
                        }

                        items.Add(item);
                    }
                }
            }
        }

        private static void GetSerialSeries(List<Item> items, params string[] data) {
            string url = WebUtility.UrlDecode(data[3]);

            url = url.Substring(url.IndexOf("http", StringComparison.Ordinal));

            string response = HTTPUtility.GetRequest(url);

            GetSerialSeriesData(items, LinkDecode(response));
        }

        private static void GetSerialSeriesData(List<Item> items, string response) {
            SerialModel[] filmModel = null;

            try {
                filmModel = JsonConvert.DeserializeObject<SerialModel[]>(response);
            } catch (Exception exception) {
                Filmix.Logger.LogError(exception);
            }

            if (filmModel != null) {
                var baseItem = new Item() {
                    Type = ItemType.DIRECTORY,
                    ImageLink = PluginSettings.Settings.Icons.IcoFolder
                };

                foreach (var season in filmModel) {
                    foreach (var episode in season.Folder) {
                        var item = new Item(baseItem) {
                            Name = episode.Title,
                            Link =
                                $"{GetEpisodeCommand.KEY}{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(episode.File)}"
                        };
                        items.Add(item);
                    }
                }
            }
        }

        private static string LinkDecode(string data) {
            if (!BaseDecode(ref data)) {
                if (!Html5Decode(ref data)) {

                }
            }

            return data;
        }

        private static bool Html5Decode(ref string data) {
            if (!data.StartsWith(".")) {
                data = data.Substring(1);
                string tempString = string.Empty;
                for (int j = 0; j < data.Length; j += 3) {
                    tempString += "\\u0" + data.Substring(j, 3);
                }

                data = Tools.Tools.ReplaceUnicodeSymbols(tempString);

                return true;
            }

            return false;
        }

        private static bool BaseDecode(ref string data) {
            if (data.StartsWith("#2")) {
                data = data.Substring(2);
                for (int n = 0; n < 3; n++) {
                    foreach (string t in PluginSettings.Settings.Encryption.Tokens) {
                        data = data.Replace(t, string.Empty);
                    }
                }

                try {
                    data = Encoding.UTF8.GetString(Convert.FromBase64String(data));
                } catch (Exception exception) {
                    Filmix.Logger.LogError(exception);
                    if (UpdateTokens()) {
                        try {
                            data = Encoding.UTF8.GetString(Convert.FromBase64String(data));
                        } catch (Exception exception2) {
                            Filmix.Logger.LogError(exception2);
                        }
                    }
                }

                return true;
            }

            return false;
        }

        private static bool UpdateTokens() {
            bool result = false;
            string response = HTTPUtility.GetRequest(PluginSettings.Settings.Encryption.Url);

            try {
                PluginSettings.Settings.Encryption.Tokens = response.Split(",").ToList();
                result = true;
            } catch (Exception) {
            }

            if (result) {
                PluginSettings.Instance.Save();
            }

            return result;
        }
    }
}
