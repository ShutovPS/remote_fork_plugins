using System.Collections.Generic;
using System.Reflection;
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

            items.AddRange(GetCategoryCommand.GetFilmsItems("http://sensfilm.xyz/serial/"));

            return items;
        }

        private static void CheckUpdate(ICollection<Item> items, IPluginContext context) {
            if (context != null) {
                string latestVersion =
                    context.GetLatestVersionNumber(typeof(SensFilm).GetCustomAttribute<PluginAttribute>().Id);
                if (!string.IsNullOrEmpty(latestVersion)) {
                    if (latestVersion != typeof(SensFilm).GetCustomAttribute<PluginAttribute>().Version) {
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
