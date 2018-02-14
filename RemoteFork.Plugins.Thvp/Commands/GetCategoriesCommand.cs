using System.Collections.Generic;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetCategoriesCommand : ICommand {
        public const string KEY = "categories";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            var item = new Item() {
                Name = "Поиск",
                SearchOn = " Поиск",
                Type = ItemType.DIRECTORY,
                ImageLink = PluginSettings.Settings.Icons.IcoSearch,
                Link =
                    $"{GetSearchCommand.KEY}{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}vse.m3u",
                Description = "<html><font face=\"Arial\" size=\"5\"><b>Поиск</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            var baseItem = new Item() {
                Type = ItemType.DIRECTORY,
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };

            item = new Item(baseItem) {
                Name = "Все",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}vse.m3u",
                Description = "<html><font face=\"Arial\" size=\"5\"><b>Все</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Сериалы",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}serial.m3u",
                Description = "<html><font face=\"Arial\" size=\"5\"><b>Сериалы</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Зарубежные фильмы",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}zarubej.m3u",
                Description = "<html><font face=\"Arial\" size=\"5\"><b>Зарубежные фильмы</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Наши фильмы",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}nashi.m3u",
                Description = "<html><font face=\"Arial\" size=\"5\"><b>Наши фильмы</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Телевизор",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}tv.m3u",
                Description = "<html><font face=\"Arial\" size=\"5\"><b>Телевизор</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Мультипликация",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}mult.m3u",
                Description = "<html><font face=\"Arial\" size=\"5\"><b>Мультипликация</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Аниме",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}anime.m3u",
                Description = "<html><font face=\"Arial\" size=\"5\"><b>Аниме</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Научно-популярные фильмы",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}nauka.m3u",
                Description =
                    "<html><font face=\"Arial\" size=\"5\"><b>Научно-популярные фильмы</font></b><p><img src=\"" +
                    PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Юмор",
                Link =
                    $"{GetCategoryCommand.KEY}{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}jumor.m3u",
                Description = "<html><font face=\"Arial\" size=\"5\"><b>Юмор</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            return items;
        }
    }
}
