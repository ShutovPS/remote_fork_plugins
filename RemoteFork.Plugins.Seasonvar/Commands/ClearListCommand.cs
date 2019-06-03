using System.Collections.Generic;
using RemoteFork.Items;

namespace RemoteFork.Plugins {
    public class ClearListCommand : ICommand {
        public const string KEY = "clear";

        public void GetItems(PlayList playList, IPluginContext context = null, Dictionary<string, string> data = null) {
            Seasonvar.SERIAL_MATCHES.Clear();
        }

        public static string CreateLink() {
            var data = new Dictionary<string, object>() {
                {Seasonvar.KEY, KEY}
            };

            return Seasonvar.CreateLink(data);
        }
    }
}
