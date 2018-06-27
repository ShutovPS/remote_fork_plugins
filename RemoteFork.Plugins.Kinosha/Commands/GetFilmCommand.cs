using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;
using RemoteFork.Tools;

namespace RemoteFork.Plugins {
    public class GetFilmCommand : ICommand {
        public const string KEY = "episode";

        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            return GetFilm(WebUtility.UrlDecode(data[2]));
        }

        private static List<Item> GetFilm(string url) {
            string response = HTTPUtility.GetRequest(url);

            return GetFilmData(url, response);
        }

        private static List<Item> GetFilmData(string url, string response) {
            var items = new List<Item>();

            var regex = new Regex(PluginSettings.Settings.Regexp.IdAndType);

            if (regex.IsMatch(response)) {
                string type = regex.Match(response).Groups[2].Value;
                string id = regex.Match(response).Groups[4].Value;

                string data = $"key[id]={id}&pl_type={type}";
                string apiResponse = HTTPUtility.PostRequest($"{PluginSettings.Settings.Links.ApiSite}/getplay", data);

                FilmModel filmModel = null;

                try {
                    filmModel = JsonConvert.DeserializeObject<FilmModel>(apiResponse);
                } catch (Exception) {
                    // ignored
                }

                if (filmModel != null && filmModel.Status == "ok") {
                    switch (filmModel.Type) {
                        case 1:
                            items.AddRange(GetVideo(filmModel, GetDescription(response)));
                            break;
                        case 2:
                            items.AddRange(GetSerial(filmModel, GetDescription(response)));
                            break;
                    }
                }
            }

            return items;
        }

        private static IEnumerable<Item> GetVideo(FilmModel filmModel, string description) {
            var items = new List<Item>();

            if (filmModel.Sources != null && !string.IsNullOrEmpty(filmModel.Sources.Mp4)) {
                var baseItem = new Item() {
                    Type = ItemType.FILE,
                    ImageLink = PluginSettings.Settings.Icons.IcoVideo,
                    Description = string.Format(description, string.Empty)
                };

                var files = filmModel.Sources.Mp4.Split(',');
                foreach (string file in files) {
                    var item = new Item(baseItem) {
                        Link = file,
                        Name = Path.GetFileName(file)
                    };
                    items.Add(item);
                }
            }

            return items;
        }

        private static IEnumerable<Item> GetSerial(FilmModel filmModel, string description) {
            var items = new List<Item>();

            if (filmModel.Pl != null && filmModel.Pl.Mp4 != null && filmModel.Pl.Mp4.Playlist != null) {
                var baseItem = new Item() {
                    Type = ItemType.FILE,
                    ImageLink = PluginSettings.Settings.Icons.IcoVideo
                };

                foreach (var season in filmModel.Pl.Mp4.Playlist) {
                    if (season.Playlist != null) {
                        string title = season.Comment.ReplaceUnicodeSymbols();
                        foreach (var episode in season.Playlist) {
                            var item = new Item(baseItem) {
                                Link = episode.File,
                                Name = Path.GetFileName(episode.File),
                                Description = string.Format(description,
                                    $"{title}<br>{episode.Comment.ReplaceUnicodeSymbols()}")
                            };
                            items.Add(item);
                        }
                    } else if (!string.IsNullOrEmpty(season.File)) {
                        var item = new Item(baseItem) {
                            Link = season.File,
                            Name = Path.GetFileName(season.File),
                            Description =
                                string.Format(description, season.Comment.ReplaceUnicodeSymbols())
                        };
                        items.Add(item);
                    }

                }
            }

            return items;
        }
        private static string GetDescription(string response) {
            string image = string.Empty;
            string info = string.Empty;
            string quality = string.Empty;
            string title = string.Empty;

            var regex = new Regex(PluginSettings.Settings.Regexp.DescriptionNs);
            if (regex.IsMatch(response)) {
                info = regex.Match(response).Groups[2].Value;
            }

            regex = new Regex(PluginSettings.Settings.Regexp.QualitynNs);
            if (regex.IsMatch(response)) {
                quality = regex.Match(response).Groups[2].Value;
            }

            regex = new Regex(PluginSettings.Settings.Regexp.Poster);
            if (regex.IsMatch(response)) {
                image = regex.Match(response).Groups[2].Value;
            }

            regex = new Regex(PluginSettings.Settings.Regexp.TitleNs);
            if (regex.IsMatch(response)) {
                title = regex.Match(response).Groups[2].Value;
            }


            return
                $"<img src=\"{image}\" alt=\"\" align=\"left\" style=\"width:240px;float:left;\"/></div><span style=\"color:#3090F0\">{title}<br>{{0}}</span><br>{quality}<br>{info}";
        }
    }
}
