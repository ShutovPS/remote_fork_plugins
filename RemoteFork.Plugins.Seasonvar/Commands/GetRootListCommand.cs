using System.Collections.Generic;
using RemoteFork.Items;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetRootListCommand : ICommand {
        public const string KEY = "root";

        private static readonly Dictionary<string, string> _directories = new Dictionary<string, string>() {
            {"Зарубежные", GetFilteringListCommand.CreateLink("eng")},
            {"Отечественные", GetFilteringListCommand.CreateLink("rus")}
        };

        public void GetItems(PlayList playList, IPluginContext context, Dictionary<string, string> data) {
            playList.Items = new List<IItem>();

            IItem item = new SearchItem() {
                Title = "Поиск",
                Link = SearchSerialsCommand.CreateLink(),
                Description = "Поиск",
                ImageLink = PluginSettings.Settings.Icons.IcoSearch
            };
            playList.Items.Add(item);

            foreach (var directory in _directories) {
                item = new DirectoryItem() {
                    Title = directory.Key,
                    Link = directory.Value,

                    ImageLink = PluginSettings.Settings.Icons.IcoFolder
                };
                playList.Items.Add(item);
            }

            item = new DirectoryItem() {
                Title = "Обновить список",
                Link = ClearListCommand.CreateLink(),

                ImageLink = PluginSettings.Settings.Icons.IcoUpdate
            };
            playList.Items.Add(item);
        }
    }
}
