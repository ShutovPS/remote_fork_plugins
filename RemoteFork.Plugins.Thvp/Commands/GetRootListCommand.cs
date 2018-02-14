using System.Collections.Generic;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            var baseItem = new Item() {
                Type = ItemType.DIRECTORY,
                ImageLink = PluginSettings.Settings.Icons.IcoFolder
            };

            var item = new Item(baseItem) {
                Name = "За последние 24 часа",
                Link = $"{GetCategoriesCommand.KEY}{PluginSettings.Settings.Separator}",
                Description = "<html><font face=\"Arial\" size=\"5\"><b>За последние 24 часа</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "За последнюю неделю",
                Link = $"{GetCategoriesCommand.KEY}{PluginSettings.Settings.Separator}/w",
                Description = "<html><font face=\"Arial\" size=\"5\"><b>За последнюю неделю</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Топ 100 за месяц",
                Link = $"{GetCategoriesCommand.KEY}{PluginSettings.Settings.Separator}/t",
                Description = "<html><font face=\"Arial\" size=\"5\"><b>Топ 100 за месяц</font></b><p><img src=\"" +
                              PluginSettings.Settings.Logo + "\" />"
            };
            items.Add(item);

            return items;
        }
    }
}
