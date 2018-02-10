using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();
            
            string response = HTTPUtility.PostRequest(PluginSettings.Settings.TrackerServer + "/forum/tracker.php",
                "nm=");

            var regex = new Regex(PluginSettings.Settings.Regexp.UserLogout);
            if (regex.IsMatch(response)) {
                var item = new Item {
                    Name = "Поиск",
                    Link = "search",
                    Type = ItemType.DIRECTORY,
                    SearchOn = "Поиcк",
                    ImageLink = PluginSettings.Settings.Icons.IcoSearch,
                    Description = "<html><font face=\"Arial\" size=\"5\"><b>Поиск</font></b><p><img src=\"" +
                                  PluginSettings.Settings.Logo + "\" /> <p>"
                };
                items.Add(item);
            } else {
                var item = new Item {
                    Name = "Авторизация",
                    Link = "login;root",
                    Type = ItemType.DIRECTORY,
                    ImageLink = PluginSettings.Settings.Icons.User,
                    Description = "<html><font face=\"Arial\" size=\"5\"><b>Авторизация</font></b><p><img src=\"" +
                                  PluginSettings.Settings.Logo + "\" /> <p>"
                };
                items.Add(item);
            }

            response = HTTPUtility.PostRequest(PluginSettings.Settings.TrackerServer + "/forum/index.php",
                "nm=");

            regex = new Regex(PluginSettings.Settings.Regexp.GetRootCategories);
            if (regex.IsMatch(response)) {
                string categories = regex.Match(response).Groups[2].Value;
                regex = new Regex(PluginSettings.Settings.Regexp.GetRootLink);
                foreach (Match category in regex.Matches(categories)) {
                    var item = new Item {
                        Name = category.Groups[4].Value,
                        Link = $"category{PluginSettings.Settings.Separator}/forum/{category.Groups[2].Value}",
                        Type = ItemType.DIRECTORY,
                        ImageLink = PluginSettings.Settings.Icons.IcoFolder,
                        Description = "<html><font face=\"Arial\" size=\"5\"><b>" + category.Groups[4].Value +
                                      "</font></b><p><img src=\"" + PluginSettings.Settings.Logo +
                                      "\" /> <p>"
                    };
                    items.Add(item);
                }
            }

            return items;
        }
    }
}
