using System.Collections.Generic;

namespace RemoteFork.Plugins.AceStream.Commands {
    public class GetAceStreamNetTVCommand : ICommand {
        public const string KEY = "acestreamnettv";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            var baseItem = new Item() {
                Type = ItemType.DIRECTORY,
                ImageLink = "http://torrent-tv.ru/images/all_channels.png"
            };

            var item = new Item(baseItem) {
                Name = "РАЗВЛЕКАТЕЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.entertaining.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "ДЕТСКИЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.kids.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "ФИЛЬМЫ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.movies.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "СЕРИАЛЫ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.series.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "СПОРТИВНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.sport.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "МУЗЫКАЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.music.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "ОБРАЗОВАТЕЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.educational.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "ИНФОРМАЦИОННЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.informational.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "РЕГИОНАЛЬНЫЕ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.regional.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "ЭРОТИКА 18+",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.erotic_18_plus.iproxy"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "ВСЕ КАНАЛЫ",
                Link = $"iproxy{AceStreamTV.SEPARATOR}ace.all.iproxy"
            };
            items.Add(item);
            
            return items;
        }
    }
}
