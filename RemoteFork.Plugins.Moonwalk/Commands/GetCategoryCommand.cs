using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RemoteFork.Items;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetCategoryCommand : ICommand {
        public const string KEY = "category";
        public const string URL_KEY = "url";

        public void GetItems(PlayList playList, IPluginContext context, Dictionary<string, string> data) {
            string url;
            data.TryGetValue(URL_KEY, out url);
            url = WebUtility.UrlDecode(url);

            GetFilmsItems(playList, url);
        }

        public static void GetFilmsItems(PlayList playList, string url) {
            string response = HTTPUtility.GetRequest(url);

            GetFilmsItemsFromHtml(playList, url, response);
        }

        public static void GetFilmsItemsFromHtml(PlayList playList, string url, string response, bool search = false) {
            response = Regex.Match(response, "\\[.*\\]").Value;

            var films = JsonConvert.DeserializeObject<FilmModel[]>(response);

            foreach (var film in films) {
                var item = GetItem(film);
                if (playList.Items.Any(i => i.GetLink() == item.GetLink())) {
                    continue;
                }

                playList.Items.Add(item);
            }

            if (!search) {
                var regex = new Regex("(\\&page\\=)(\\d+)");
                if (regex.IsMatch(url)) {
                    int page = int.Parse(regex.Match(url).Groups[2].Value);
                    url = regex.Replace(url, $"&page={page + 1}");
                } else {
                    url += "&page=2";
                }

                playList.NextPageUrl = CreateLink(url);
            }
        }

        private static IItem GetItem(FilmModel film) {
            if (film.Serial != null) {
                film = film.Serial;
            }

            var item = new DirectoryItem() {
                Title = film.GetTitle(),
                ImageLink = PluginSettings.Settings.Icons.IcoFolder,
                Link = GetFilmCommand.CreateLink(GetFilmCommand.TRANSLATIONS_KEY, film.IframeUrl, string.Empty),
                Description = film.ToString()
            };

            return item;
        }

        public static string CreateLink(string url) {
            var data = new Dictionary<string, object>() {
                {Moonwalk.KEY, KEY},
                {URL_KEY, WebUtility.UrlEncode(url)}
            };

            return Moonwalk.CreateLink(data);
        }
    }
}
