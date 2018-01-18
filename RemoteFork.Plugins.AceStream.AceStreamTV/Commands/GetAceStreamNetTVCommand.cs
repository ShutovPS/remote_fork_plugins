using System.Collections.Generic;

namespace RemoteFork.Plugins.AceStream.Commands {
    public class GetAceStreamNetTVCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            var item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "РАЗВЛЕКАТЕЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.entertaining.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "ДЕТСКИЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.kids.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "ОБРАЗОВАТЕЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.educational.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "ФИЛЬМЫ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.movies.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "СЕРИАЛЫ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.series.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "ИНФОРМАЦИОННЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.informational.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "СПОРТИВНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.sport.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "МУЗЫКАЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.music.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "РЕГИОНАЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.regional.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "ЭРОТИКА 18+",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.erotic_18_plus.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "ВСЕ КАНАЛЫ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.all.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);
            
            AceStreamTV.IsIptv = false;
            return items;
        }
    }
}
