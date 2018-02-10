using System.Collections.Generic;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            var item = new Item() {
                Name = "Поиск",
                Link = "search",
                Type = ItemType.DIRECTORY,
                SearchOn = "Поиск",
                ImageLink = PluginSettings.Settings.Icons.IcoSearch,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>Поиск</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item() {
                Name = "По рейтингу",
                Link = "list" + PluginSettings.Settings.Separator + "rating",
                Type = ItemType.DIRECTORY,
                ImageLink = PluginSettings.Settings.Icons.IcoFolder,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>По рейтингу</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item() {
                Name = "По дате",
                Link = "list" + PluginSettings.Settings.Separator + "date",
                Type = ItemType.DIRECTORY,
                ImageLink = PluginSettings.Settings.Icons.IcoFolder,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>По дате</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item() {
                Name = "По названию",
                Link = "list" + PluginSettings.Settings.Separator + "title",
                Type = ItemType.DIRECTORY,
                ImageLink = PluginSettings.Settings.Icons.IcoFolder,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>По названию</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item() {
                Name = "По просмотрам",
                Link = "list" + PluginSettings.Settings.Separator + "viewCount",
                Type = ItemType.DIRECTORY,
                ImageLink = PluginSettings.Settings.Icons.IcoFolder,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>По просмотрам</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item() {
                Name = "По количеству видео",
                Link = "list" + PluginSettings.Settings.Separator + "videoCount",
                Type = ItemType.DIRECTORY,
                ImageLink = PluginSettings.Settings.Icons.IcoFolder,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>По количеству видео</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            return items;
        }
    }
}
