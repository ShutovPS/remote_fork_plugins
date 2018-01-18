using System.Collections.Generic;

namespace RemoteFork.Plugins.AceStream.Commands {
    public class GetTorrentTVCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();
            var item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "РАЗВЛЕКАТЕЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.ent.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };

            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "ДЕТСКИЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.child.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "ПОЗНАВАТЕЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.discover.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "HD",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.HD.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "ОБЩИЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.common.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "ФИЛЬМЫ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.film.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "МУЖСКИЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.man.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "МУЗЫКАЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.music.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "ИНФОРМАЦИОННЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.news.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "РЕГИОНАЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.region.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "РЕЛИГИОЗНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.relig.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "СПОРТИВНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.sport.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "ЭРОТИКА 18+",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.porn.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            item = new Item {
                Type = ItemType.DIRECTORY,
                Name = "ВСЕ КАНАЛЫ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.all.iproxy",
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };
            items.Add(item);

            AceStreamTV.IsIptv =false;
            return items;
        }
    }
}
