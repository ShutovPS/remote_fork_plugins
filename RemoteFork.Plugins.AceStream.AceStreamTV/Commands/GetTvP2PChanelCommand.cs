using RemoteFork.Network;
using RemoteFork.Settings;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.AceStream.Commands {
    public class GetTvP2PChanelCommand : ICommand {
        public const string KEY = "TvP2PChanel";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();
            var reGexLink = new Regex(PluginSettings.Settings.Regexp.LoadPlayer);
            var reGexName = new Regex(PluginSettings.Settings.Regexp.Title);

            string response = HTTPUtility.GetRequest(data[2]).Replace("\n", "");

            var item = new Item {
                Type = ItemType.FILE,
                Link = string.Format(PluginSettings.Settings.AceStreamApi.GetStreamManifest,
                    ProgramSettings.Settings.IpAddress, ProgramSettings.Settings.AceStreamPort,
                    reGexLink.Match(response).Groups[2].Value + "&.mp4"),
                Name = reGexName.Match(response).Value,
                ImageLink = PluginSettings.Settings.Icons.IcoVideo
            };

            items.Add(item);

            AceStreamTV.IsIptv = true;

            return items;
        }
    }
}
