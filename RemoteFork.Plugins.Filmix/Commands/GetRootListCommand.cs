using System.Collections.Generic;
using System.Reflection;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            CheckUpdate(items, context);

            var item = new Item() {
                Name = "Поиск",
                Type = ItemType.DIRECTORY,
                Link = $"{SearchCommand.KEY}",
                SearchOn = "Поиск",
                ImageLink = PluginSettings.Settings.Icons.IcoSearch
            };
            items.Add(item);

            item = new Item() {
                Name = "Зарубежные фильмы",
                Type = ItemType.DIRECTORY,
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}films{PluginSettings.Settings.Separator}c996{PluginSettings.Settings.Separator}0",
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };
            items.Add(item);

            item = new Item() {
                Name = "Русские фильмы",
                Type = ItemType.DIRECTORY,
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}films{PluginSettings.Settings.Separator}c6{PluginSettings.Settings.Separator}0",
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };
            items.Add(item);

            item = new Item() {
                Name = "Зарубежные сериалы",
                Type = ItemType.DIRECTORY,
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}serialy{PluginSettings.Settings.Separator}c996{PluginSettings.Settings.Separator}0",
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };
            items.Add(item);

            item = new Item() {
                Name = "Русские сериалы",
                Type = ItemType.DIRECTORY,
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}serialy{PluginSettings.Settings.Separator}c6{PluginSettings.Settings.Separator}0",
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };
            items.Add(item);

            item = new Item() {
                Name = "Зарубежные мультфильмы",
                Type = ItemType.DIRECTORY,
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}multfilmy{PluginSettings.Settings.Separator}c996{PluginSettings.Settings.Separator}0",
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };
            items.Add(item);

            item = new Item() {
                Name = "Русские мультфильмы",
                Type = ItemType.DIRECTORY,
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}multfilmy{PluginSettings.Settings.Separator}c6{PluginSettings.Settings.Separator}0",
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };
            items.Add(item);

            HTTPUtility.GetRequest(PluginSettings.Settings.Links.Site);

            return items;
        }

        private static void CheckUpdate(ICollection<Item> items, IPluginContext context) {
            if (context != null) {
                string latestVersion =
                    context.GetLatestVersionNumber(typeof(Filmix).GetCustomAttribute<PluginAttribute>().Id);
                if (!string.IsNullOrEmpty(latestVersion)) {
                    if (latestVersion != typeof(Filmix).GetCustomAttribute<PluginAttribute>().Version) {
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
