using RemoteFork.Plugins.AceStream.Channels;
using System.Collections.Generic;
using RemoteFork.Settings;

namespace RemoteFork.Plugins.AceStream.Commands {
    public class GetAsChannelsCommand : ICommand {
        public const string KEY = "as_channels";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            var baseItem = new Item() {
                Type = ItemType.FILE,
                ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291videofile.png"
            };

            var channels = ChannelsManager.GetChannelsByCategory(data[2], data[3]);
            foreach (var channel in channels) {
                var item = new Item(baseItem) {
                    Name = channel.Name,
                    Description = channel.Name,
                    Link =
                        $"http://{ProgramSettings.Settings.IpAddress}:{ProgramSettings.Settings.AceStreamPort}/ace/getstream?id={channel.Url}&.mp4",
                };
                items.Add(item);
            }

            AceStreamTV.IsIptv = true;

            return items;
        }
    }
}
