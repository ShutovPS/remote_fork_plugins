using System.Collections.Generic;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            var baseItem = new Item() {
                ImageLink = PluginSettings.Settings.Icons.IcoFolder,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" +
                              PluginSettings.Settings.LogoNoNameClub + "\" />"
            };

            var item = new Item() {
                Name = "Поиск",
                Link = "search",
                Type = ItemType.DIRECTORY,
                SearchOn = "Поиск видео на NNM-Club",
                ImageLink = PluginSettings.Settings.Icons.IcoSearch,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" +
                              PluginSettings.Settings.LogoNoNameClub + "\" />"
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Новинки кино",
                Link =
                    $"page{PluginSettings.Settings.Separator}{PluginSettings.Settings.TrackerServerNnm}/forum/portal.php?c=10",
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Наше кино",
                Link =
                    $"page{PluginSettings.Settings.Separator}{PluginSettings.Settings.TrackerServerNnm}/forum/portal.php?c=13",
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Зарубежное кино",
                Link =
                    $"page{PluginSettings.Settings.Separator}{PluginSettings.Settings.TrackerServerNnm}/forum/portal.php?c=6",
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "HD (3D) Кино",
                Link =
                    $"page{PluginSettings.Settings.Separator}{PluginSettings.Settings.TrackerServerNnm}/forum/portal.php?c=11",
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Артхаус",
                Link =
                    $"page{PluginSettings.Settings.Separator}{PluginSettings.Settings.TrackerServerNnm}/forum/portal.php?c=17",
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Наши сериалы",
                Link =
                    $"page{PluginSettings.Settings.Separator}{PluginSettings.Settings.TrackerServerNnm}/forum/portal.php?c=4",
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Зарубежные сериалы",
                Link =
                    $"page{PluginSettings.Settings.Separator}{PluginSettings.Settings.TrackerServerNnm}/forum/portal.php?c=3",
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Театр, МузВидео, Разное",
                Link =
                    $"page{PluginSettings.Settings.Separator}{PluginSettings.Settings.TrackerServerNnm}/forum/portal.php?c=21",
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Док. TV-бренды",
                Link =
                    $"page{PluginSettings.Settings.Separator}{PluginSettings.Settings.TrackerServerNnm}/forum/portal.php?c=22",
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Док. и телепередачи",
                Link =
                    $"page{PluginSettings.Settings.Separator}{PluginSettings.Settings.TrackerServerNnm}/forum/portal.php?c=23",
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Спорт и Юмор",
                Link =
                    $"page{PluginSettings.Settings.Separator}{PluginSettings.Settings.TrackerServerNnm}/forum/portal.php?c=24",
            };
            items.Add(item);

            item = new Item(baseItem) {
                Name = "Аниме и Манга",
                Link =
                    $"page{PluginSettings.Settings.Separator}{PluginSettings.Settings.TrackerServerNnm}/forum/portal.php?c=1",
            };
            items.Add(item);

            NnmClub.IsIptv = false;
            return items;
        }
    }
}
