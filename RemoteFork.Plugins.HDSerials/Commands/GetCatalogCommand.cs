using System.Collections.Generic;
using System.Net;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetCatalogCommand : ICommand {
        public const string KEY = "catalog";

        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            if (string.IsNullOrEmpty(data[3])) {
                items.AddRange(GetAlphabetItems(data));
            } else {
                items.AddRange(GetItems(data));
            }

            return items;
        }

        public List<Item> GetAlphabetItems(params string[] data) {
            var items = new List<Item>();

            var baseItem = new Item() {
                Type = ItemType.DIRECTORY
            };

            var alpha = "АБВГДЕЖЗИКЛМНОПРСТУФХЦЧШЩЭЮЯ".ToCharArray();
            foreach (var c in alpha) {
                var item = new Item(baseItem) {
                    Name = c.ToString(),
                    Link = $"{KEY}{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}{c}"
                };
                items.Add(item);
            }

            var itemNumbers = new Item(baseItem) {
                Name = "1-9",
                Link = $"{KEY}{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}1-9"
            };
            items.Add(itemNumbers);

            return items;
        }

        public List<Item> GetItems(params string[] data) {
            var items = new List<Item>();

            string url = WebUtility.UrlDecode(data[2]);

            string response = HTTPUtility.GetRequest(string.Concat(url, $"/{KEY}/{data[3]}"));

            items.AddRange(GetCategoryCommand.GetSerialsItems(response));

            return items;
        }
    }
}
