using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class AuthCommand : ICommand {
        public const string KEY = "auth";

        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            switch (data[2]) {
                case "login": {
                    string login = context.GetRequestParams()["search"];
                    if (!string.IsNullOrWhiteSpace(login)) {
                        PluginSettings.Settings.Auth.Login = login;

                        items.Add(
                            new Item() {
                                Name = "Пароль",
                                Link = $"{KEY}{PluginSettings.Settings.Separator}password",
                                Type = ItemType.DIRECTORY,
                                SearchOn = "Пароль",
                                ImageLink = PluginSettings.Settings.Icons.Password
                            }
                        );
                    }
                }
                    break;
                case "password": {
                    string password = context.GetRequestParams()["search"];
                    if (!string.IsNullOrWhiteSpace(password)) {
                        PluginSettings.Settings.Auth.Password = password;
                    }
                }
                    break;
                case "restore":
                    break;
                default:
                    if (!CheckLogin()) {
                        items.Add(
                            new Item() {
                                Name = "Повторить введенные данные",
                                Link = $"{KEY}{PluginSettings.Settings.Separator}restore",
                                Type = ItemType.DIRECTORY,
                                ImageLink = PluginSettings.Settings.Icons.User
                            }
                        );
                        items.Add(
                            new Item() {
                                Name = "Логин",
                                Link = $"{KEY}{PluginSettings.Settings.Separator}login",
                                Type = ItemType.DIRECTORY,
                                SearchOn = "Логин",
                                ImageLink = PluginSettings.Settings.Icons.Login
                            }
                        );
                    } else {
                        items.Add(OnAuth());
                    }

                    break;
            }

            if (items.Count == 0) {
                if (!string.IsNullOrWhiteSpace(PluginSettings.Settings.Auth.Login) &&
                    !string.IsNullOrWhiteSpace(PluginSettings.Settings.Auth.Password)) {
                    string response =
                        HTTPUtility.PostRequest(PluginSettings.Settings.Links.Site + "/engine/ajax/user_auth.php",
                            $"login_name={PluginSettings.Settings.Auth.Login}&login_password={PluginSettings.Settings.Auth.Password}&login_not_save=1&login=submit");

                    items.Add(CheckLogin(response));
                }
            }

            return items;
        }

        private static Item CheckLogin(string response) {
            var regex = new Regex(PluginSettings.Settings.Regexp.UserAuthorized);

            if (regex.IsMatch(response)) {
                PluginSettings.Instance.Save();
                return OnAuth();
            } else {
                if (CheckLogin()) {
                    return OnAuth();
                }
            }

            return OnError();
        }

        public static bool CheckLogin() {
            var response = HTTPUtility.GetRequest(PluginSettings.Settings.Links.Site);

            var regex = new Regex(PluginSettings.Settings.Regexp.UserLogout);

            return regex.IsMatch(response);
        }

        private static Item OnError() {
            var item = new Item {
                Link = string.Empty,
                ImageLink = PluginSettings.Settings.Icons.IcoError,
                Name = "<span style=\"color#F68648\">" + " - Ошибка - " + "</span>"
            };

            return item;
        }

        private static Item OnAuth() {
            var item = new Item {
                Link = string.Empty,
                ImageLink = PluginSettings.Settings.Icons.User,
                Name = "<span style=\"color#48f65a\">" + " - Авторизован - " + "</span>"
            };

            return item;
        }
    }
}
