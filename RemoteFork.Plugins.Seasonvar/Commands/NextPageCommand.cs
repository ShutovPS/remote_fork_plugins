using System;
using System.Collections.Generic;
using RemoteFork.Items;

namespace RemoteFork.Plugins {
    class NextPageCommand : ICommand {

        public const string LANG_KEY = "lang";
        public const string PAGE_KEY = "page";
        public const string SORT_KEY = "sort";

        public void GetItems(PlayList playList, IPluginContext context = null, Dictionary<string, string> data = null) {
            string lang;
            string page;
            string sort;

            data.TryGetValue(LANG_KEY, out lang);
            data.TryGetValue(PAGE_KEY, out page);
            data.TryGetValue(SORT_KEY, out sort);

            var tempSerials = Seasonvar.SERIAL_MATCHES[lang + sort];

            int id;

            if (int.TryParse(page, out id)) {
                for (int i = id; i < Math.Min(50 + id, Math.Min(tempSerials.Count, tempSerials.Count + id)); i++) {
                    var item = new GetSerialInfoCommand().GetItem(tempSerials[i].Id,
                        tempSerials[i].Title);

                    item.Link = GetSerialListCommand.CreateLink(tempSerials[i].Url);

                    playList.Items.Add(item);
                }

                if (tempSerials.Count - id > 50) {
                    playList.NextPageUrl = GetFilteringListCommand.CreateLink(lang, sort, id + 50);
                }
            }
        }
    }
}
