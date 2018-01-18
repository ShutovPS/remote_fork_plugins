using System.Text.RegularExpressions;

namespace RemoteFork.Plugins {
    public class GetSerialInfoCommand {
        public Item GetItem(IPluginContext context = null, params string[] data) {
            Item item;

            string id = data[0];
            string text = data.Length > 1 ? data[1] : string.Empty;

            if (string.IsNullOrEmpty(text)) {
                if (Seasonvar.SERIAL_ITEMS.ContainsKey(id)) {
                    item = Seasonvar.SERIAL_ITEMS[id];
                } else {
                    item = new Item() {
                        ImageLink = string.Format(Seasonvar.SMALL_IMAGE_URL, id)
                    };
                    Seasonvar.SERIAL_ITEMS.Add(id, item);
                }
            } else {
                if (Seasonvar.SERIAL_ITEMS.ContainsKey(id)) {
                    item = Seasonvar.SERIAL_ITEMS[id];
                } else {
                    item = new Item() {
                        ImageLink = string.Format(Seasonvar.SMALL_IMAGE_URL, id)
                    };
                    Seasonvar.SERIAL_ITEMS.Add(id, item);
                }
                if (string.IsNullOrEmpty(item.Description)) {
                    var match = Regex.Match(text, "(data-id-season=\")(\\d+?)(\")");

                    if (match.Success) {
                        string seasonId = match.Groups[2].Value;

                        match = Regex.Match(text, "(<p\\s+itemprop=\"description\">\\s*)(.*?)(\\s*<\\/p>)");
                        if (match.Success) {
                            string descText = match.Groups[2].Value;

                            match = Regex.Match(text, "(itemprop=\"name\">\\s*)(.*?)(\\s*<\\/h1>)");
                            if (match.Success) {
                                string name = match.Groups[2].Value;
                                item.Description =
                                    string.Format(
                                        "<img src=\"{0}\" alt=\"\" align=\"left\"/><h3>{1}</h3>{2}",
                                        string.Format(Seasonvar.IMAGE_URL, seasonId), name.Trim(), descText);
                            }
                        }
                    }
                }
            }

            return item;
        }
    }
}
