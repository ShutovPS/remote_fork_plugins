using System;
using System.Collections.Generic;
using System.Linq;
using RemoteFork.Items;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class FirstSymbolGroupCommand : ICommand {
        public const string KEY = "first";

        public const string LANG_KEY = "lang";
        public const string PAGE_KEY = "page";

        public void GetItems(PlayList playList, IPluginContext context, Dictionary<string, string> data) {
            string lang;
            string page;

            data.TryGetValue(LANG_KEY, out lang);
            data.TryGetValue(PAGE_KEY, out page);

            if (!Seasonvar.SERIAL_MATCHES.ContainsKey(lang + "name")) {
                data[GetFilteringListCommand.SORT_KEY] = "name";
                new GetFilteringListCommand().GetItems(playList, context, data);

                playList.Items.Clear();
            }

            var tempSerials = Seasonvar.SERIAL_MATCHES[lang + "name"];
            var groups = tempSerials.GroupBy(i => i.Groups[3].Value.Trim().Replace("\"", "#").First());

            if (string.IsNullOrEmpty(page)) {
                foreach (var g in groups) {
                    var item = new DirectoryItem() {
                        Title = g.Key.ToString().Trim(),
                        Link = CreateLink(lang, page: g.Key.ToString()),

                        ImageLink = PluginSettings.Settings.Icons.IcoFolder
                    };
                    playList.Items.Add(item);
                }

                playList.Items.Sort((a, b) => string.Compare(a.Title, b.Title, StringComparison.Ordinal));
            } else {
                var group = groups.FirstOrDefault(i => i.Key == page.First());
                if (group != null) {
                    foreach (var i in group) {
                        var item = new GetSerialInfoCommand().GetItem(i.Groups[1].Value, i.Groups[3].Value);

                        item.Link = GetSerialListCommand.CreateLink(i.Groups[2].Value);

                        playList.Items.Add(item);
                    }
                }
            }
        }

        public static string CreateLink(string lang = default, string page = default) {
            var data = new Dictionary<string, object>() {
                {Seasonvar.KEY, KEY},
                {LANG_KEY, lang},
                {PAGE_KEY, page}
            };
            return Seasonvar.CreateLink(data);
        }
    }
}
