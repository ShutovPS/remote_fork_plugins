using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Settings;

namespace RemoteFork.Plugins.AceStream.Commands {
    public class GetIproxyListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            AceStreamTV.Source =  HTTPUtility.GetRequest("http://pomoyka.lib.emergate.net/trash/ttv-list/" + data[2] +
                                                ".m3u?ip=" + ProgramSettings.Settings.IpAddress + ":" +
                                                ProgramSettings.Settings.AceStreamPort);
            


            AceStreamTV.IsIptv = true;
            return items;
        }
    }
}
