using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using RemoteFork.Items;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class AuthorizationCommand : ICommand {
        public const string KEY = "auth";

        public const string ACTION_KEY = "action";

        public void GetItems(PlayList playList, IPluginContext context, Dictionary<string, string> data) {
            string action;

            data.TryGetValue(ACTION_KEY, out action);

            switch (action) {
                case "login": {
                    string login = context.GetRequestParams()["search"];
                    if (!string.IsNullOrWhiteSpace(login)) {
                        PluginSettings.Settings.Authorization.Login = login;

                        playList.Items.Add(
                            new SearchItem() {
                                Title = "Пароль",
                                Link = CreateLink("password"),
                                Description = "Пароль",
                                ImageLink = PluginSettings.Settings.Icons.Password
                            }
                        );
                    }
                }
                    break;
                case "password": {
                    string password = context.GetRequestParams()["search"];

                    if (!string.IsNullOrWhiteSpace(password)) {
                        PluginSettings.Settings.Authorization.Password = password;
                    }
                }
                    break;
                case "restore":
                    break;
                default:
                    if (!CheckLogin(false)) {
                        playList.Items.Add(
                            new DirectoryItem() {
                                Title = "Повторить введенные данные",
                                Link = CreateLink("restore"),
                                ImageLink = PluginSettings.Settings.Icons.User
                            }
                        );
                        playList.Items.Add(
                            new SearchItem() {
                                Title = "Логин",
                                Link = CreateLink("login"),
                                Description = "Логин",
                                ImageLink = PluginSettings.Settings.Icons.Login
                            }
                        );
                    } else {
                        playList.Items.Add(OnAuth());
                    }

                    break;
            }

            if (playList.Items.Count == 0) {
                if (!string.IsNullOrWhiteSpace(PluginSettings.Settings.Authorization.Login) &&
                    !string.IsNullOrWhiteSpace(PluginSettings.Settings.Authorization.Password)) {
                    Authorization(PluginSettings.Settings.Authorization.Login,
                        PluginSettings.Settings.Authorization.Password);

                    playList.Items.Add(CheckLogin());
                }
            }
        }

        private static void Authorization(string login, string password) {
            try {
                using (var client = new HttpClient()) {
                    var postParams = new Dictionary<string, string> {
                        {"login", login},
                        {"password", password}
                    };

                    using (var postContent = new FormUrlEncodedContent(postParams)) {
                        using (var response = client
                            .PostAsync(PluginSettings.Settings.Links.Site + "/?mod=login", postContent).Result) {
                            if (response.Headers.TryGetValues("Set-Cookie", out var cook)) {
                                if (cook.FirstOrDefault() is string line) {
                                    var regex = new Regex("svid1=([^;]+)(;|$)");
                                    if (regex.IsMatch(line)) {
                                        string svid1 = regex.Match(line).Groups[1].Value;

                                        if (!string.IsNullOrWhiteSpace(svid1)) {
                                            PluginSettings.Settings.Authorization.Cookie = $"svid1={svid1}; premAll=1";
                                            PluginSettings.Settings.Authorization.CookieExpires =
                                                DateTime.UtcNow.AddDays(28);

                                            PluginSettings.Instance.Save();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } catch (Exception exception) {
                Seasonvar.Log.LogError(exception);
            }
        }

        private static DirectoryItem CheckLogin() {
            if (CheckLogin(true)) {
                PluginSettings.Instance.Save();
                return OnAuth();
            }

            return OnError();
        }

        public static bool CheckLogin(bool withCookie) {
            var header = new Dictionary<string, string>();

            if (withCookie) {
                header.Add("Cookie", PluginSettings.Settings.Authorization.Cookie);
            }

            var response = HTTPUtility.GetRequest(PluginSettings.Settings.Links.Site, header);

            var regex = new Regex(PluginSettings.Settings.Regexp.UserLogout);

            bool result = regex.IsMatch(response);

            if (!result && !withCookie && !string.IsNullOrEmpty(PluginSettings.Settings.Authorization.Cookie) &&
                PluginSettings.Settings.Authorization.CookieExpires > DateTime.UtcNow) {
                result = CheckLogin(true);
            }

            return result;
        }

        private static DirectoryItem OnError() {
            var item = new DirectoryItem {
                Link = string.Empty,
                ImageLink = PluginSettings.Settings.Icons.Error,
                Title = "<span style=\"color#F68648\">" + " - Ошибка - " + "</span>"
            };

            return item;
        }

        private static DirectoryItem OnAuth() {
            var item = new DirectoryItem {
                Link = string.Empty,
                ImageLink = PluginSettings.Settings.Icons.User,
                Title = "<span style=\"color#48f65a\">" + " - Авторизован - " + "</span>"
            };

            return item;
        }

        public static string CreateLink(string action = default) {
            var data = new Dictionary<string, object>() {
                {Seasonvar.KEY, KEY},
                {ACTION_KEY, action}
            };

            return Seasonvar.CreateLink(data);
        }
    }
}
