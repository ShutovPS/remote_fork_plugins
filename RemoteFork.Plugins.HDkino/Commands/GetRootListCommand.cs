using System.Collections.Generic;
using System.Net;

namespace RemoteFork.Plugins {
    public class GetRootListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            var baseItem = new Item() {
                Type = ItemType.DIRECTORY
            };

            var item = new Item(baseItem) {
                Name = "LostFilm",
                Link =
                    $"{GetCategoryCommand.KEY}{HDSerials.SEPARATOR}serials{HDSerials.SEPARATOR}{WebUtility.UrlEncode("http://lostfilm.hdkino.biz/")}",
                ImageLink = "http://hdkino.biz/templates/kin/images/logos/lost.jpg",
                Description =
                    "<img src=\"http://hdkino.biz/templates/kin/images/logos/lost.jpg\" alt=\"\" align=\"left\" style=\"width:240px;float:left;\"/>"

            };
            items.Add(item);
            item = new Item(baseItem) {
                Name = "ColdFilm",
                Link =
                    $"{GetCategoryCommand.KEY}{HDSerials.SEPARATOR}serials{HDSerials.SEPARATOR}{WebUtility.UrlEncode("http://coldfilm.hdkino.biz/")}",
                ImageLink = "http://hdkino.biz/templates/kin/images/logos/cold.jpg",
                Description =
                    "<img src=\"http://hdkino.biz/templates/kin/images/logos/cold.jpg\" alt =\"\" align=\"left\" style=\"width:240px;float:left;\"/>"
            };
            items.Add(item);

            return items;
        }
    }
}
