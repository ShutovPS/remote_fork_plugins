using System.Collections.Specialized;

namespace RemoteFork.Plugins {
    public interface IPluginContext {
        NameValueCollection GetRequestParams();

        string CreatePluginUrl(NameValueCollection parameters);

        IHttpClient GetHttpClient();
        void ConsoleLog(string s);
        ILogger GetLogger();
    }
}
