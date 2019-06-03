using System.Text;
using System.Text.RegularExpressions;
using RemoteFork.Items;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetSerialInfoCommand {
        public DirectoryItem GetItem(string serialId, string name, string text = default) {
            name = name.Trim();

            DirectoryItem item;

            if (string.IsNullOrEmpty(text)) {
                if (Seasonvar.SERIAL_ITEMS.ContainsKey(serialId)) {
                    item = new DirectoryItem(Seasonvar.SERIAL_ITEMS[serialId]);
                } else {
                    item = new DirectoryItem() {
                        ImageLink = string.Format(PluginSettings.Settings.Links.IconUrl, serialId)
                    };
                    Seasonvar.SERIAL_ITEMS.Add(serialId, item);
                }

                item = new DirectoryItem(item) {
                    Title = name,
                    Description = GetDescription(seasonId: serialId, name)
                };

            } else {
                if (Seasonvar.SERIAL_ITEMS.ContainsKey(serialId)) {
                    item = new DirectoryItem(Seasonvar.SERIAL_ITEMS[serialId]);
                } else {
                    item = new DirectoryItem() {
                        ImageLink = string.Format(PluginSettings.Settings.Links.IconUrl, serialId)
                    };
                    Seasonvar.SERIAL_ITEMS.Add(serialId, item);
                }

                if (string.IsNullOrEmpty(item.Description)) {
                    item.Description = GetDescription(text: text);
                }
            }

            return item;
        }

        private static string GetDescription(string seasonId = default, string name = default, string text = default) {
            var sb = new StringBuilder();

            string descText = string.Empty;

            if (!string.IsNullOrEmpty(text)) {
                var regex = Regex.Match(text, PluginSettings.Settings.Regexp.SeasonData);

                if (regex.Success) {
                    seasonId = regex.Groups[2].Value;
                }

                regex = Regex.Match(text, PluginSettings.Settings.Regexp.GetSerialDescription);
                if (regex.Success) {
                    descText = regex.Groups[2].Value;
                }

                regex = Regex.Match(text, PluginSettings.Settings.Regexp.GetSerialName);
                if (regex.Success) {
                    name = regex.Groups[2].Value.Trim();
                }
            }

            string poster = string.Format(PluginSettings.Settings.Links.PosterUrl, seasonId);

            if (!string.IsNullOrEmpty(poster)) {
                sb.AppendLine(
                    $"<div id=\"poster\" style=\"float: left; padding: 4px; background-color: #eeeeee; margin: 0px 13px 1px 0px;\"><img style=\"width: 180px; float: left;\" src=\"{poster}\" /></div>");
            }

            sb.AppendLine($"<span style=\"color: #3366ff;\"><strong>{name}</strong></span><br>");

            if (!string.IsNullOrEmpty(descText)) {
                sb.AppendLine(
                    $"<p>{descText}</p>");
            }

            return sb.ToString();
        }
    }
}
