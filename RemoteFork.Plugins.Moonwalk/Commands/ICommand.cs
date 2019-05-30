using System.Collections.Generic;
using RemoteFork.Items;

namespace RemoteFork.Plugins {
    public interface ICommand {
        void GetItems(PlayList playList, IPluginContext context, Dictionary<string, string> data);
    }
}
