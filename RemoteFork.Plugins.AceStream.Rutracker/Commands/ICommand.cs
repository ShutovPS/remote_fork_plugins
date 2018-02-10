using System.Collections.Generic;

namespace RemoteFork.Plugins.Commands {
    public interface ICommand {
        List<Item> GetItems(IPluginContext context, params string[] data);
    }
}
