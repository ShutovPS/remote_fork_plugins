using System;
using System.Collections.Generic;

namespace RemoteFork.Plugins {
    class NextPageCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            List<Item> items = new List<Item>();

            string lang = data.Length > 1 ? data[1] : string.Empty;
            string sort = data.Length > 2 ? data[2] : string.Empty;
            string page = data.Length > 3 ? data[3] : string.Empty;

            var tempSerials = Seasonvar.SERIAL_MATCHES[lang + sort];

            int id;
            if (int.TryParse(page, out id)) {
                for (int i = id; i < Math.Min(50 + id, Math.Min(tempSerials.Count, tempSerials.Count + id)); i++) {
                    var item = new GetSerialInfoCommand().GetItem(context, tempSerials[i].Groups[1].Value);
                    item.Name = tempSerials[i].Groups[3].Value.Trim();
                    item.Link = string.Format("{1}{0}{2}", Seasonvar.SEPARATOR, "list", tempSerials[i].Groups[2]);

                    items.Add(item);
                }

                if (tempSerials.Count - id > 50) {
                    Seasonvar.NextPageUrl = string.Format("{1}{0}{2}{0}{3}", Seasonvar.SEPARATOR, lang, sort, id + 50);
                }
            }

            return items;
        }
    }
}
