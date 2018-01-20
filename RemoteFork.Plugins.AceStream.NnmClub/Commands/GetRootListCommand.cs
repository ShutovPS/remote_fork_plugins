using System.Collections.Generic;

namespace RemoteFork.Plugins.Commands {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            var item = new Item {
                Name = "Поиск",
                Link = "search",
                Type = ItemType.DIRECTORY,
                SearchOn = "Поиск видео на NNM-Club",
                ImageLink = NnmClub.ICO_SEARCH,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" + NnmClub.LOGO_NO_NAME_CLUB + "\" />"
            };
            items.Add(item);

            item = new Item {
                Name = "Новинки кино",
                Link = $"page{NnmClub.SEPARATOR}{NnmClub.TrackerServerNnm}/forum/portal.php?c=10",
                ImageLink = NnmClub.ICO_FOLDER,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" + NnmClub.LOGO_NO_NAME_CLUB + "\" />"
            };

            items.Add(item);

            item = new Item {
                Name = "Наше кино",
                Link = $"page{NnmClub.SEPARATOR}{NnmClub.TrackerServerNnm}/forum/portal.php?c=13",
                ImageLink = NnmClub.ICO_FOLDER,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" + NnmClub.LOGO_NO_NAME_CLUB + "\" />"
            };
            items.Add(item);

            item = new Item {
                Name = "Зарубежное кино",
                Link = $"page{NnmClub.SEPARATOR}{NnmClub.TrackerServerNnm}/forum/portal.php?c=6",
                ImageLink = NnmClub.ICO_FOLDER,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" + NnmClub.LOGO_NO_NAME_CLUB + "\" />"
            };
            items.Add(item);

            item = new Item {
                Name = "HD (3D) Кино",
                Link = $"page{NnmClub.SEPARATOR}{NnmClub.TrackerServerNnm}/forum/portal.php?c=11",
                ImageLink = NnmClub.ICO_FOLDER,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" + NnmClub.LOGO_NO_NAME_CLUB + "\" />"
            };
            items.Add(item);

            item = new Item {
                Name = "Артхаус",
                Link = $"page{NnmClub.SEPARATOR}{NnmClub.TrackerServerNnm}/forum/portal.php?c=17",
                ImageLink = NnmClub.ICO_FOLDER,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" + NnmClub.LOGO_NO_NAME_CLUB + "\" />"
            };
            items.Add(item);

            item = new Item {
                Name = "Наши сериалы",
                Link = $"page{NnmClub.SEPARATOR}{NnmClub.TrackerServerNnm}/forum/portal.php?c=4",
                ImageLink = NnmClub.ICO_FOLDER,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + item.Name + "</font></b><p><img src=\"" + NnmClub.LOGO_NO_NAME_CLUB + "\" />"
            };
            items.Add(item);

            item = new Item {
                Name = "Зарубежные сериалы",
                Link = $"page{NnmClub.SEPARATOR}{NnmClub.TrackerServerNnm}/forum/portal.php?c=3",
                ImageLink = NnmClub.ICO_FOLDER,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" + NnmClub.LOGO_NO_NAME_CLUB + "\" />"
            };
            items.Add(item);

            item = new Item {
                Name = "Театр, МузВидео, Разное",
                Link = $"page{NnmClub.SEPARATOR}{NnmClub.TrackerServerNnm}/forum/portal.php?c=21",
                ImageLink = NnmClub.ICO_FOLDER,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" + NnmClub.LOGO_NO_NAME_CLUB + "\" />"
            };
            items.Add(item);

            item = new Item {
                Name = "Док. TV-бренды",
                Link = $"page{NnmClub.SEPARATOR}{NnmClub.TrackerServerNnm}/forum/portal.php?c=22",
                ImageLink = NnmClub.ICO_FOLDER,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" + NnmClub.LOGO_NO_NAME_CLUB + "\" />"
            };
            items.Add(item);

            item = new Item {
                Name = "Док. и телепередачи",
                Link = $"page{NnmClub.SEPARATOR}{NnmClub.TrackerServerNnm}/forum/portal.php?c=23",
                ImageLink = NnmClub.ICO_FOLDER,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" + NnmClub.LOGO_NO_NAME_CLUB + "\" />"
            };
            items.Add(item);

            item = new Item {
                Name = "Спорт и Юмор",
                Link = $"page{NnmClub.SEPARATOR}{NnmClub.TrackerServerNnm}/forum/portal.php?c=24",
                ImageLink = NnmClub.ICO_FOLDER,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" + NnmClub.LOGO_NO_NAME_CLUB + "\" />"
            };
            items.Add(item);

            item = new Item {
                Name = "Аниме и Манга",
                Link = $"page{NnmClub.SEPARATOR}{NnmClub.TrackerServerNnm}/forum/portal.php?c=1",
                ImageLink = NnmClub.ICO_FOLDER,
                Description = "<html><font face=\"Arial\" size=\"5\"><b>" + "</font></b><p><img src=\"" + NnmClub.LOGO_NO_NAME_CLUB + "\" />"
            };
            items.Add(item);

            NnmClub.IsIptv = false;
            return items;
        }
    }
}
