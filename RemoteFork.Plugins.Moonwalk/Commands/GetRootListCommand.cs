using System.Collections.Generic;
using System.Reflection;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            CheckUpdate(items, context);

            var baseItem = new Item() {
                Type = ItemType.DIRECTORY,
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };

            var item = new Item() {
                Name = "Поиск",
                Type = ItemType.DIRECTORY,
                Link = $"{SearchCommand.KEY}",
                SearchOn = "Поиск",
                ImageLink = PluginSettings.Settings.Icons.IcoSearch
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Зарубежные фильмы",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{PluginSettings.Settings.Links.Api}/movies_updates.json?api_token={PluginSettings.Settings.Key}"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Русские фильмы",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{PluginSettings.Settings.Links.Api}/movies_updates.json?api_token={PluginSettings.Settings.Key}&category=Russian"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Зарубежные сериалы",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{PluginSettings.Settings.Links.Api}/serials_updates.json?api_token={PluginSettings.Settings.Key}"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Русские сериалы",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{PluginSettings.Settings.Links.Api}/serials_updates.json?api_token={PluginSettings.Settings.Key}&category=Russian"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Аниме сериалы",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{PluginSettings.Settings.Links.Api}/serials_updates.json?api_token={PluginSettings.Settings.Key}&category=Anime"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Аниме фильмы",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{PluginSettings.Settings.Links.Api}/movies_updates.json?api_token={PluginSettings.Settings.Key}&category=Anime"
            };
            items.Add(item);

            return items;
        }

        private static void CheckUpdate(ICollection<Item> items, IPluginContext context) {
            if (context != null) {
                string latestVersion =
                    context.GetLatestVersionNumber(typeof(Moonwalk).GetCustomAttribute<PluginAttribute>().Id);
                if (!string.IsNullOrEmpty(latestVersion)) {
                    if (latestVersion != typeof(Moonwalk).GetCustomAttribute<PluginAttribute>().Version) {
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
