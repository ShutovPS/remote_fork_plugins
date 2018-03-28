using RemoteFork.Plugins.AceStream.Channels;
using System.Collections.Generic;

namespace RemoteFork.Plugins.AceStream.Commands {
    class GetAsCategoriesCommand : ICommand {
        public const string KEY = "as_categories";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();
            
            var baseItem = new Item() {
                Type = ItemType.DIRECTORY,
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };

            var categories = ChannelsManager.GetCategories(data[2]);
            foreach (string category in categories) {
                var item = new Item(baseItem) {
                    Name = category,
                    Description = category,
                    Link = $"{GetAsChannelsCommand.KEY}{AceStreamTV.SEPARATOR}{data[2]}{AceStreamTV.SEPARATOR}{category}"
                };
                items.Add(item);
            }

            return items;
        }
    }
}
