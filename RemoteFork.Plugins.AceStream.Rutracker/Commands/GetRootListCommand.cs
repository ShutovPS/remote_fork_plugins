using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            //var item = new Item {
            //    Name = "Поиск",
            //    Link = "search",
            //    Type = ItemType.DIRECTORY,
            //    SearchOn = "Поик",
            //    ImageLink = PluginSettings.Settings.Icons.IcoSearch,
            //    Description = "<html><font face=\"Arial\" size=\"5\"><b>Поиск</font></b><p><img src=\"" +
            //                  PluginSettings.Settings.Logo + "\" /> <p>"
            //};
            //items.Add(item);

            //var item = new Item {
            //    Name = "Торренты за сегодня",
            //    Link =
            //        $"page{PluginSettings.Settings.Separator}{PluginSettings.Settings.TrackerServer}/forum/tracker.php?f-1",
            //    Type = ItemType.DIRECTORY,
            //    ImageLink = PluginSettings.Settings.Icons.IcoFolder,
            //    Description = "<html><font face=\"Arial\" size=\"5\"><b>Торренты за сегодня</font></b><p><img src=\"" +
            //                  PluginSettings.Settings.Logo + "\" /> <p>"
            //};
            //items.Add(item);

            string response = HTTPUtility.PostRequest(PluginSettings.Settings.TrackerServer + "/forum/index.php",
                "nm=");

            var regex = new Regex("(<h3>Кино, Видео, ТВ<\\/h3>)([\\s\\S]*?)(<\\/div>)");
            if (regex.IsMatch(response)) {
                string categories = regex.Match(response).Groups[2].Value;
                regex = new Regex("(<a href=\")(.*?)(\">)(.*?)(<\\/a>)");
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
            regex = new Regex("(cx\" value=\")(.*)(\")");
            if (regex.IsMatch(response)) {
                Rutracker.CX = regex.Match(response).Groups[2].Value;
            }

            return items;
        }
    }
}
