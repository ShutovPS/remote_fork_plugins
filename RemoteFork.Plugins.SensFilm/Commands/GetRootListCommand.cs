using System.Collections.Generic;

namespace RemoteFork.Plugins {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            var item = new Item() {
                Name = "Поиск",
                Type = ItemType.DIRECTORY,
                Link = $"{SearchCommand.KEY}",
                SearchOn = "Поиск",
                ImageLink =
                    "http://icons.iconarchive.com/icons/graphicloads/colorful-long-shadow/256/Search-icon.png"
            };
            items.Add(item);

            items.AddRange(GetCategoryCommand.GetFilmsItems("http://sensfilm.xyz/serial/"));

            return items;
        }
    }
}
