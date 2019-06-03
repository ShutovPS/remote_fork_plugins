using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using RemoteFork.Items;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {

    [PluginAttribute(Id = "seasonvar",
        Version = "0.5.0",
        Author = "fd_crash&&forkplayer",
        Name = "Seasonvar",
        Description = "Сериалы ТУТ! Сериалы онлайн смотреть бесплатно. Смотреть онлайн",
        ImageLink = "http://cdn.seasonvar.ru/images/fav/apple-touch-icon-144x144.png",
        Github = "ShutovPS/RemoteFork.Plugins/Seasonvar")]

    public class Seasonvar : IRemotePlugin {
        public const string KEY = "KEY";

        public static readonly Dictionary<string, List<Match>> SERIAL_MATCHES = new Dictionary<string, List<Match>>();
        public static readonly Dictionary<string, IItem> SERIAL_ITEMS = new Dictionary<string, IItem>();

        private static IPluginContext _context;

        // Список доступных команд
        private static readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>() {
            {GetRootListCommand.KEY, new GetRootListCommand()},
            {GetFilteringListCommand.KEY, new GetFilteringListCommand()},
            {GetSeriesListCommand.KEY, new GetSeriesListCommand()},
            {GetSerialListCommand.KEY, new GetSerialListCommand()},
            {GetVoiceListCommand.KEY, new GetVoiceListCommand()},
            {SearchSerialsCommand.KEY, new SearchSerialsCommand()},
            {ClearListCommand.KEY, new ClearListCommand()},
            {FirstSymbolGroupCommand.KEY, new FirstSymbolGroupCommand()},
        };

        // Item представляет собой класс, содержащащий следующие поля:
        //// Title - назdание
        //// Link - ссылка (если директория, то ссылка передается на обработку плагину, если файл, то ссылка открывается в проигрывателе)
        //// ImageLink - иконка
        //// Description - описание, поддерживает HTML формат
        //// Type - тип сущности: ItemType.FILE или ItemType.DIRECTORY (по умолчанию)

        // Главный метод для обработки запросов
        public PlayList GetPlayList(IPluginContext context) {
            _context = context;

            string path = context.GetRequestParams().Get(PluginSettings.Settings.PluginPath);

            var playlist = new PlayList() {
                Items = new List<IItem>()
            };

            if (!string.IsNullOrEmpty(path)) {
                path = WebUtility.UrlDecode(path);
            }

            var data = ConvertToDictionary(path);

            ICommand command = null;

            if (data.Count == 0) {
                data[KEY] = GetRootListCommand.KEY;
            }

            string key;

            if (data.TryGetValue(KEY, out key)) {
                if (_commands.ContainsKey(key)) {
                    command = _commands[key];
                }

                if (command != null) {
                    command.GetItems(playlist, context, data);
                }
            }

            return playlist;
        }

        public static string CreateLink(Dictionary<string, object> data) {
            var url = ConvertToString(data);

            var pluginParams = new NameValueCollection {
                [PluginSettings.Settings.PluginPath] = WebUtility.UrlEncode(url)
            };

            url = _context.CreatePluginUrl(pluginParams);

            return url;
        }

        private static Dictionary<string, string> ConvertToDictionary(string text) {
            if (text == null) {
                return new Dictionary<string, string>();
            }
            var dictionary = text
                .Split(';')
                .Select(part => part.Split('='))
                .Where(part => part.Length == 2)
                .ToDictionary(sp => sp[0], sp => sp[1]);

            return dictionary;
        }

        private static string ConvertToString(Dictionary<string, object> dictionary) {
            string text = string.Join(";", dictionary.Select(x => x.Key + "=" + x.Value).ToArray());

            return text;
        }
    }
}
