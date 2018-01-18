using System;
using System.Collections.Generic;
using System.Linq;

namespace RemoteFork.Plugins {
    class FirstSymbolGroupCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            List<Item> items = new List<Item>();

            string lang = data.Length > 1 ? data[1] : string.Empty;
            string page = data.Length > 2 ? data[2] : string.Empty;

            var tempSerials = Seasonvar.SERIAL_MATCHES[lang + "name"];
            var group = tempSerials.GroupBy(i => i.Groups[3].Value.Trim().Replace("\"", "#").First());

            if (string.IsNullOrEmpty(page)) {
                foreach (var g in group) {
                    var item = new Item() {
                        Name = g.Key.ToString().Trim(),
                        Link = string.Format("{1}{0}{2}{0}{3}", Seasonvar.SEPARATOR, lang, "first", g.Key)
                    };
                    items.Add(item);
                }
                items.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
            } else {
                var g = group.FirstOrDefault(i => i.Key == page.First());
                if (g != null) {
                    foreach (var gg in g) {

                        var item = new GetSerialInfoCommand().GetItem(context, gg.Groups[1].Value);
                        item.Name = gg.Groups[3].Value.Trim();
                        item.Link = string.Format("{1}{0}{2}", Seasonvar.SEPARATOR, "list", gg.Groups[2]);

                        items.Add(item);
                    }
                }
            }

            return items;
        }
    }
}
