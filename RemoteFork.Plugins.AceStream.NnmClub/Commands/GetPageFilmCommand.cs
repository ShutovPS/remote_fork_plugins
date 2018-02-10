using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetPageFilmCommand : ICommand {
        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            string responseFromServer = HTTPUtility.GetRequest(PluginSettings.Settings.TrackerServer + "/forum/" + data[2]);

            string torrentPath = null;
            var regex = new Regex(PluginSettings.Settings.Regexp.GetPageFilmMagnet);
            if (regex.IsMatch(responseFromServer)) {
                torrentPath = regex.Match(responseFromServer).Groups[2].Value;
            }
            if (!string.IsNullOrWhiteSpace(torrentPath)) {
                data[2] = torrentPath;
                return new GetTorrentCommand().GetItems(context, data);
            }
            
            return items;
        }
    }
}
