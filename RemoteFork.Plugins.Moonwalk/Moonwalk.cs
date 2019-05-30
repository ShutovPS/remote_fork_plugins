using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using RemoteFork.Items;
using RemoteFork.Log;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    [PluginAttribute(Id = "moonwalk", Version = "0.1.0", Author = "fd_crash", Name = "Moonwalk",
        Description = "Cмотреть лучшие новинки фильмов онлайн в хорошем качестве и бесплатно.",
        ImageLink = "https://img.icons8.com/dusk/384/night-camera.png",
        Github = "ShutovPS/RemoteFork.Plugins/Moonwalk")]
    public class Moonwalk : IRemotePlugin {
        public const string KEY = "KEY";

        public static readonly Logger Logger = new Logger(typeof(Moonwalk));

        private static readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>() {
            {GetRootListCommand.KEY, new GetRootListCommand()},
            {SearchCommand.KEY, new SearchCommand()},
            {GetCategoryCommand.KEY, new GetCategoryCommand()},
            {GetFilmCommand.KEY, new GetFilmCommand()},
            {GetEpisodeCommand.KEY, new GetEpisodeCommand()},
            {GetNewKeysCommand.KEY, new GetNewKeysCommand()},
        };

        private static IPluginContext _context;

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

        public static Dictionary<string, string> ConvertToDictionary(string text) {
            if (text == null) {
                return new Dictionary<String,String>();
            }
            var dictionary = text
                .Split(';')
                .Select(part => part.Split('='))
                .Where(part => part.Length == 2)
                .ToDictionary(sp => sp[0], sp => sp[1]);

            return dictionary;
        }

        public static string ConvertToString(Dictionary<string, object> dictionary) {
            string text = string.Join(";", dictionary.Select(x => x.Key + "=" + x.Value).ToArray());

            return text;
        }
    }
}
