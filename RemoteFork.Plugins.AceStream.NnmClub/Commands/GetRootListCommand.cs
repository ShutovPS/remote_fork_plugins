using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            var baseItem = new Item() {
                ImageLink = PluginSettings.Settings.Icons.IcoFolder,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };

            var item = new Item() {
                Name = "Поиск",
                Link = "search",
                Type = ItemType.DIRECTORY,
                SearchOn = "Поиск видео на NNM-Club",
                ImageLink = PluginSettings.Settings.Icons.IcoSearch,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            string response = HTTPUtility.GetRequest(PluginSettings.Settings.TrackerServer + "/forum/portal.php");

            var regex = new Regex(PluginSettings.Settings.Regexp.GetRootCategories);
            if (regex.IsMatch(response)) {
                string categories = regex.Matches(response)[1].Groups[2].Value;
                regex = new Regex(PluginSettings.Settings.Regexp.GetRootCategory);
                foreach (Match category in regex.Matches(categories)) {
                    item = new Item {
                        Name = category.Groups[4].Value,
                        Link = $"category{PluginSettings.Settings.Separator}{category.Groups[2].Value}",
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
