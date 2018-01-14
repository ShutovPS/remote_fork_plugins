using System.Collections.Generic;

namespace RemoteFork.Plugins {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            List<Item> items = new List<Item>();

            var item = new Item() {
                Name = "Поиск",
                Link = "search",
                SearchOn = "Поиск",
                ImageLink = "http://icons.iconarchive.com/icons/graphicloads/colorful-long-shadow/256/Search-icon.png"
            };
            items.Add(item);
            item = new Item() {
                Name = "Зарубежные",
                Link = "eng"
            };
            items.Add(item);
            item = new Item() {
                Name = "Отечественные",
                Link = "rus"
            };
            items.Add(item);

            item = new Item() {
                Name = "Обновить список",
                Link = "update"
            };
            items.Add(item);

            return items;
        }
    }
}
