using System.Collections.Generic;

namespace RemoteFork.Plugins.AceStream.Commands {
    public interface ICommand {
        List<Item> GetItems(IPluginContext context = null, params string[] data);
    }
}
