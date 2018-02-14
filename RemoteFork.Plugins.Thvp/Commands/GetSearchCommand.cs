using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;
using RemoteFork.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetSearchCommand : ICommand {
        public const string KEY = "search";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            string response =
                HTTPUtility.GetRequest(
                    $"{PluginSettings.Settings.TrackerServer}{data[2]}/{ProgramSettings.Settings.IpAddress}/{data[3]}");

            string searchData = context.GetRequestParams()["search"];
            if (!string.IsNullOrEmpty(searchData)) {
                searchData = searchData.ToLower();

                var regex = new Regex(PluginSettings.Settings.RegexFileInfo, RegexOptions.Multiline);
                var result = regex.Matches(response);

                if (result.Count > 0) {
                    foreach (Match match in result) {
                        if (!match.Groups[2].Value.ToLower().Contains(searchData)) {
                            response = response.Replace(match.Value, "");
                        }
                    }
                    Thvp.Source = response;
                } else {
                    items.Add(NonSearch());
                }
            }

            return items;
        }

        private static Item NonSearch() {

            var item = new Item {
                Link = string.Empty,
                ImageLink = PluginSettings.Settings.Icons.IcoNofile,
                Name = "<span style=\"color#F68648\">" + " - Ничего не найдено - " + "</span>",
                Description = "Поиск не дал результатов"
            };

            return item;
        }
    }
}
