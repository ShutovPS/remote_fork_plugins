using System.Collections.Generic;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;
using RemoteFork.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetCategoryCommand : ICommand {
        public const string KEY = "category";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            string response =
                HTTPUtility.GetRequest(
                    $"{PluginSettings.Settings.TrackerServer}{data[2]}/{ProgramSettings.Settings.IpAddress}/{data[3]}");
            Thvp.Source = response;

            return items;
        }
    }
}
