using System.Collections.Generic;

namespace RemoteFork.Plugins.AceStream.Commands {
    public class GetTorrentTVCommand : ICommand {
        public const string KEY = "torrenttv";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            var baseItem = new Item() {
                Type = ItemType.DIRECTORY,
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };

            var item = new Item(baseItem) {
                Name = "РАЗВЛЕКАТЕЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.ent.iproxy"
            };

            items.Add(item);

            item = new Item(baseItem) {
                Name = "ДЕТСКИЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.child.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "ПОЗНАВАТЕЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.discover.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "HD",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.HD.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "ОБЩИЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.common.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "ФИЛЬМЫ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.film.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "МУЖСКИЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.man.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "СПОРТИВНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.sport.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "МУЗЫКАЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.music.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "ИНФОРМАЦИОННЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.news.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "РЕГИОНАЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.region.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "РЕЛИГИОЗНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.relig.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "ЭРОТИКА 18+",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.porn.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "ВСЕ КАНАЛЫ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ttv.all.iproxy"
            };
            items.Add(item);
            
            return items;
        }
    }
}
