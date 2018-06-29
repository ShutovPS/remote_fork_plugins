using System.Collections.Generic;
using System.Net;
using System.Reflection;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            CheckUpdate(items, context);

            var baseItem = new Item() {
                Type = ItemType.DIRECTORY
            };

            var item = new Item(baseItem) {
                Name = "LostFilm",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}serials{PluginSettings.Settings.Separator}{WebUtility.UrlEncode("http://lostfilm.hdkino.biz/")}",
                ImageLink = "http://hdkino.biz/templates/kin/images/logos/lost.jpg",
                Description =
                    "<img src=\"http://hdkino.biz/templates/kin/images/logos/lost.jpg\" alt=\"\" align=\"left\" style=\"width:240px;float:left;\"/>"

            };
            items.Add(item);
            item = new Item(baseItem) {
                Name = "ColdFilm",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}serials{PluginSettings.Settings.Separator}{WebUtility.UrlEncode("http://coldfilm.hdkino.biz/")}",
                ImageLink = "http://hdkino.biz/templates/kin/images/logos/cold.jpg",
                Description =
                    "<img src=\"http://hdkino.biz/templates/kin/images/logos/cold.jpg\" alt =\"\" align=\"left\" style=\"width:240px;float:left;\"/>"
            };
            items.Add(item);

            return items;
        }

        private static void CheckUpdate(ICollection<Item> items, IPluginContext context) {
            if (context != null) {
                string latestVersion =
                    context.GetLatestVersionNumber(typeof(HDSerials).GetCustomAttribute<PluginAttribute>().Id);
                if (!string.IsNullOrEmpty(latestVersion)) {
                    if (latestVersion != typeof(HDSerials).GetCustomAttribute<PluginAttribute>().Version) {
                        var updateItem = new Item() {
                            Name = $"Доступна новая версия: {latestVersion}",
                            Link = "http://newversion.m3u",
                            ImageLink = PluginSettings.Settings.Icons.NewVersion
                        };
                        items.Add(updateItem);
                    }
                }
            }
        }
    }
}
