using System.Collections.Generic;
using System.Reflection;
using RemoteFork.Items;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetRootListCommand : ICommand {
        public const string KEY = "root";

        private static readonly Dictionary<string, string> _directories = new Dictionary<string, string>() {
            {
                "Зарубежные фильмы",
                GetCategoryCommand.CreateLink(
                    $"{PluginSettings.Settings.Links.Api}/movies_updates.json?api_token={PluginSettings.Settings.Api.Key}")
            }, {
                "Русские фильмы",
                GetCategoryCommand.CreateLink(
                    $"{PluginSettings.Settings.Links.Api}/movies_updates.json?api_token={PluginSettings.Settings.Api.Key}&category=Russian")
            }, {
                "Зарубежные сериалы",
                GetCategoryCommand.CreateLink(
                    $"{PluginSettings.Settings.Links.Api}/serials_updates.json?api_token={PluginSettings.Settings.Api.Key}")
            }, {
                "Русские сериалы",
                GetCategoryCommand.CreateLink(
                    $"{PluginSettings.Settings.Links.Api}/serials_updates.json?api_token={PluginSettings.Settings.Api.Key}&category=Russian")
            }, {
                "Аниме фильмы",
                GetCategoryCommand.CreateLink(
                    $"{PluginSettings.Settings.Links.Api}/movies_updates.json?api_token={PluginSettings.Settings.Api.Key}&category=Anime")
            }, {
                "Аниме сериалы",
                GetCategoryCommand.CreateLink(
                    $"{PluginSettings.Settings.Links.Api}/serials_updates.json?api_token={PluginSettings.Settings.Api.Key}&category=Anime")
            },
        };

        public void GetItems(PlayList playList, IPluginContext context, Dictionary<string, string> data) {
            CheckUpdate(playList, context);

            IItem item = new SearchItem() {
                Title = "Поиск",
                Link = SearchCommand.CreateLink(),
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
                Title = "Обновить ключи",
                Link = GetNewKeysCommand.CreateLink(),
                ImageLink = PluginSettings.Settings.Icons.IcoUpdate
            };
            playList.Items.Add(item);
        }

        private static void CheckUpdate(PlayList playList, IPluginContext context) {
            if (context != null) {
                string latestVersion =
                    context.GetLatestVersionNumber(typeof(Moonwalk).GetCustomAttribute<PluginAttribute>().Id);
                if (!string.IsNullOrEmpty(latestVersion)) {
                    if (latestVersion != typeof(Moonwalk).GetCustomAttribute<PluginAttribute>().Version) {
                        var updateItem = new FileItem() {
                            Title = $"Доступна новая версия: {latestVersion}",
                            Link = "http://newversion.m3u",
                            ImageLink = PluginSettings.Settings.Icons.NewVersion
                        };
                        playList.Items.Add(updateItem);
                    }
                }
            }
        }
    }
}
