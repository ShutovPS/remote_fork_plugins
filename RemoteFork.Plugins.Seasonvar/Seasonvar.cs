using RemoteFork.Plugins.Settings;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace RemoteFork.Plugins {
    [PluginAttribute(Id = "seasonvar", Version = "0.4.8", Author = "fd_crash&&forkplayer", Name = "Seasonvar",
        Description = "Сериалы ТУТ! Сериалы онлайн смотреть бесплатно. Смотреть онлайн",
        ImageLink = "http://cdn.seasonvar.ru/images/fav/apple-touch-icon-144x144.png")]
    public class Seasonvar : IPlugin {
        public static readonly Dictionary<string, List<Match>> SERIAL_MATCHES = new Dictionary<string, List<Match>>();
        public static readonly Dictionary<string, Item> SERIAL_ITEMS = new Dictionary<string, Item>();
        public static readonly string Cookie = GetCookie();

        // SEPARATOR служит для разделения команд при парсинге
        public const char SEPARATOR = ';';

        public const string PLUGIN_PATH = "pluginPath";
        public const string SITE_URL = "http://seasonvar.ru{0}";
        public const string IMAGE_URL = "http://cdn.seasonvar.ru/oblojka/{0}.jpg";
        public const string SMALL_IMAGE_URL = "http://cdn.seasonvar.ru/oblojka/small/{0}.jpg";
        //public const string PAGE = "СТРАНИЦА {0}";

        public static string NextPageUrl = string.Empty;

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

            var arg = path.Split(SEPARATOR);

            var items = new List<Item>();
            ICommand command = null;
            switch (arg.Length) {
                case 0:
                    break;
                case 1:
                    command = new GetRootListCommand();
                    break;
                default:
                    switch (arg[1]) {
                        case "eng":
                        case "rus":
                            command = new GetFilteringListCommand();
                            break;
                        case "series":
                            command = new GetSeriesListCommand();
                            break;
                        case "list":
                            command = new GetSerialListCommand();
                            break;
                        case "voise":
                            command = new GetVoiseListCommand();
                            break;
                        case "search":
                            command = new SearchSearialsCommand();
                            break;
                        case "update":
                            ClearList();
                            break;
                    }
                    break;
            }
            NextPageUrl = null;
            if (command != null) {
                string[] data = new string[4];
                for (int i = 0; i < arg.Length; i++) {
                    data[i] = arg[i];
                }
                items.AddRange(command.GetItems(context, data));
            }
            
            return CreatePlaylist(items, context);
        }

        private static Playlist CreatePlaylist(List<Item> items, IPluginContext context) {
            var playlist = new Playlist();

            if (!string.IsNullOrEmpty(NextPageUrl)) {
                var pluginParams = new NameValueCollection { [PLUGIN_PATH] = NextPageUrl };
                playlist.NextPageUrl = context.CreatePluginUrl(pluginParams);
            } else {
                playlist.NextPageUrl = null;
            }

            foreach (var item in items) {
                if (ItemType.DIRECTORY == item.Type) {
                    var pluginParams = new NameValueCollection {
                        [PLUGIN_PATH] = item.Link
                    };

                    item.Link = context.CreatePluginUrl(pluginParams);
                }
            }

            playlist.Items = items.ToArray();

            return playlist;
        }

        private static void ClearList() {
            SERIAL_MATCHES.Clear();
        }

        #region GetCookie
        static string GetCookie()
        {
            if (string.IsNullOrWhiteSpace(PluginSettings.Settings.Cookie) || DateTime.Now > PluginSettings.Settings.CookieExpires)
            {
                if (!string.IsNullOrWhiteSpace(PluginSettings.Settings.login) && !string.IsNullOrWhiteSpace(PluginSettings.Settings.password))
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            var postParams = new Dictionary<string, string>();
                            postParams.Add("login", PluginSettings.Settings.login);
                            postParams.Add("password", PluginSettings.Settings.password);

                            using (var postContent = new FormUrlEncodedContent(postParams))
                            {
                                using (HttpResponseMessage response = client.PostAsync("http://seasonvar.ru/?mod=login", postContent).Result)
                                {
                                    if (response.Headers.TryGetValues("Set-Cookie", out var cook))
                                    {
                                        if (cook.FirstOrDefault() is string line)
                                        {
                                            string svid1 = new Regex("svid1=([^;]+)(;|$)").Match(line).Groups[1].Value;
                                            if (!string.IsNullOrWhiteSpace(svid1))
                                            {
                                                PluginSettings.Settings.Cookie = $"svid1={svid1}; premAll=1";
                                                PluginSettings.Settings.CookieExpires = DateTime.Today.AddDays(28);
                                                PluginSettings.Instance.Save();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }
            }

            return string.IsNullOrWhiteSpace(PluginSettings.Settings.Cookie) ? string.Empty : PluginSettings.Settings.Cookie;
        }
        #endregion
    }
}
