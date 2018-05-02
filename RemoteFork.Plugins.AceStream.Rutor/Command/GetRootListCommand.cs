using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            var item = new Item {
                Name = "Поиск",
                Link = "search",
                Type = ItemType.DIRECTORY,
                SearchOn = "Поик",
                ImageLink = PluginSettings.Settings.Icons.IcoSearch,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>Поиск</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" /> <p>"
            };
            items.Add(item);

            item = new Item {
                Name = "Новые раздачи",
                Link = $"category{PluginSettings.Settings.Separator}/new",
                Type = ItemType.DIRECTORY,
                ImageLink = PluginSettings.Settings.Icons.IcoFolder,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>Новые раздачи</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" /> <p>"
            };
            items.Add(item);

            string response = HTTPUtility.GetRequest(PluginSettings.Settings.TrackerServer + "/browse/0/0/0/0");

            var regex = new Regex("(<select name=\"category\")([\\s\\S]*?)(<\\/select>)");
            if (regex.IsMatch(response)) {
                string categories = regex.Match(response).Groups[2].Value;
                regex = new Regex("(<option value=\")(\\d+)(\">)(.*?)(<)");
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
