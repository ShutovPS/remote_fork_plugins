using System.Collections.Generic;

namespace RemoteFork.Plugins {
    public interface ICommand {
        List<Item> GetItems(IPluginContext context = null, params string[] data);
    }
}
