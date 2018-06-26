using System.Collections.Generic;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            var item = new Item() {
                Name = "Поиск",
                Type = ItemType.DIRECTORY,
                Link = $"{SearchCommand.KEY}",
                SearchOn = "Поиск",
                ImageLink = PluginSettings.Settings.Icons.IcoSearch
            };
            items.Add(item);

            item = new Item() {
                Name = "Новинки",
                Type = ItemType.DIRECTORY,
                Link = $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{PluginSettings.Settings.Links.Site}/new/",
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };
            items.Add(item);

            item = new Item() {
                Name = "Фильмы",
                Type = ItemType.DIRECTORY,
                Link = $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{PluginSettings.Settings.Links.Site}/films/",
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };
            items.Add(item);

            item = new Item() {
                Name = "Сериалы",
                Type = ItemType.DIRECTORY,
                Link = $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{PluginSettings.Settings.Links.Site}/series/",
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };
            items.Add(item);

            item = new Item() {
                Name = "Мультфильмы",
                Type = ItemType.DIRECTORY,
                Link = $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{PluginSettings.Settings.Links.Site}/cartoons/",
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };
            items.Add(item);

            item = new Item() {
                Name = "Аниме",
                Type = ItemType.DIRECTORY,
                Link = $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{PluginSettings.Settings.Links.Site}/animation/",
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };
            items.Add(item);

            return items;
        }
    }
}
