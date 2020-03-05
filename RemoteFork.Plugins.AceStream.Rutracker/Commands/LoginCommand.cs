using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class LoginCommand : ICommand {
        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var header = new Dictionary<string, string>();

            switch (data[2]) {
                case "root": {
                    return new List<Item>() {
                        new Item() {
                            Name = "Повторить введенные данные",
                            Link = "login;restore",
                            Type = ItemType.DIRECTORY,
                            ImageLink = PluginSettings.Settings.Icons.Login,
                            Description =
                                "<html><font face=\"Arial\" size=\"5\"><b>Повторить введенные данные</font></b><p><img src=\"" +
                                PluginSettings.Settings.Logo + "\" /> <p>"
                        },
                        new Item() {
                            Name = "Логин",
                            Link = "login;login",
                            Type = ItemType.DIRECTORY,
                            SearchOn = "Логин",
                            ImageLink = PluginSettings.Settings.Icons.Login,
                            Description =
                                "<html><font face=\"Arial\" size=\"5\"><b>Пользователь</font></b><p><img src=\"" +
                                PluginSettings.Settings.Logo + "\" /> <p>"
                        }
                    };
                }
                    break;
                case "login": {
                    string login = context.GetRequestParams()["search"];
                    if (!string.IsNullOrWhiteSpace(login)) {
                        PluginSettings.Settings.User.Login = login;

                        return new List<Item>() {
                            new Item() {
                                Name = "Пароль",
                                Link = "login;password",
                                Type = ItemType.DIRECTORY,
                                SearchOn = "Пароль",
                                ImageLink = PluginSettings.Settings.Icons.Password,
                                Description =
                                    "<html><font face=\"Arial\" size=\"5\"><b>Пароль</font></b><p><img src=\"" +
                                    PluginSettings.Settings.Logo + "\" /> <p>"
                            }
                        };
                    }
                }
                    break;
                case "password": {
                    string password = context.GetRequestParams()["search"];
                    if (!string.IsNullOrWhiteSpace(password)) {
                        PluginSettings.Settings.User.Password = password;
                    }
                }
                    break;
                case "captcha": {
                    string captcha = context.GetRequestParams()["search"];
                    string capSid = data[3];
                    string capCode = data[4];

                    if (!string.IsNullOrWhiteSpace(PluginSettings.Settings.User.Login) &&
                        !string.IsNullOrWhiteSpace(PluginSettings.Settings.User.Password)) {
                        string response =
                            HTTPUtility.PostRequest(PluginSettings.Settings.TrackerServer + "/forum/login.php",
                                $"redirect=tracker.php&login_username={PluginSettings.Settings.User.Login}&login_password={PluginSettings.Settings.User.Password}&cap_sid={capSid}&cap_code_{capCode}={captcha}&login==%C2%F5%EE%E4",
                                header);

                        return CheckLogin(response);
                    } else {
                        return NonSearch();
                    }
                }
                    break;
                case "restore":
                    break;
                default:
                    return NonSearch();
            }

            if (!string.IsNullOrWhiteSpace(PluginSettings.Settings.User.Login) &&
                !string.IsNullOrWhiteSpace(PluginSettings.Settings.User.Password)) {
                string response =
                    HTTPUtility.PostRequest(PluginSettings.Settings.TrackerServer + "/forum/login.php",
                        $"redirect=tracker.php&login_username={PluginSettings.Settings.User.Login}&login_password={PluginSettings.Settings.User.Password}&login=%C2%F5%EE%E4",
                        header);

                return CheckLogin(response);
            } else {
                return NonSearch();
            }
        }

        private static List<Item> CheckLogin(string response) {
            var regex = new Regex(PluginSettings.Settings.Regexp.UserLogout);

            if (regex.IsMatch(response)) {
                PluginSettings.Instance.Save();
                return new List<Item>() {
                    new Item {
                        Link = "root",
                        Name = "Успех",
                        ImageLink = PluginSettings.Settings.Icons.User
                    }
                };
            } else {
                regex = new Regex(PluginSettings.Settings.Regexp.LoginCaptcha);
                if (regex.IsMatch(response)) {
                    string text = regex.Match(response).Value;

                    regex = new Regex(PluginSettings.Settings.Regexp.LoginCaptchaImage);
                    string image = regex.Match(text).Groups[2].Value;
                    regex = new Regex(PluginSettings.Settings.Regexp.LoginCaptchaSid);
                    string capSid = regex.Match(text).Groups[2].Value;
                    regex = new Regex(PluginSettings.Settings.Regexp.LoginCaptchaCode);
                    string capCode = regex.Match(text).Groups[2].Value;

                    return new List<Item>() {
                        new Item() {
                            Name = "Защитный код",
                            Link = $"login;captcha;{capSid};{capCode}",
                            Type = ItemType.DIRECTORY,
                            SearchOn = "Защитный код",
                            ImageLink = PluginSettings.Settings.Icons.Password,
                            Description =
                                "<html><font face=\"Arial\" size=\"5\"><b>Защитный код</font></b><p><img src=\"" +
                                image + "\" /> <p>"
                        }
                    };
                } else {
                    return NonSearch();
                }
            }
        }

        private static List<Item> NonSearch() {
            var items = new List<Item>();
            var item = new Item {
                Link = string.Empty,
                ImageLink = PluginSettings.Settings.Icons.IcoNofile,
                Name = "<span style=\"color#F68648\">" + " - Ничего не найдено - " + "</span>",
                Description = "Поиск не дал результатов"
            };

            items.Add(item);

            return items;
        }
    }
}
