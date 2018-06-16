using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using RemoteFork.Network;

namespace RemoteFork.Plugins {
    public class GetVoiseListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context, params string[] data) {
            List<Item> items = new List<Item>();

            string url = data.Length > 2 ? data[2] : string.Empty;

            var header = new Dictionary<string, string>() {
                {"Accept-Encoding", "gzip, deflate, lzma"},
                //{"Content-Type", "text/html; charset=UTF-8"}
            };

            //context.ConsoleLog("url=" + string.Format(Seasonvar.SITE_URL, url));
            string response = HTTPUtility.GetRequest(string.Format(Seasonvar.SITE_URL, url), header)
                .Replace("\n", " ");
            
            var match = Regex.Match(response, "'(secureMark)': '(.*?)'.*?'time': (\\d+)");
            if (match.Success) {
                string secure = match.Groups[2].Value;
                string time = match.Groups[3].Value;
                //context.ConsoleLog("secure=" + secure);
                //context.ConsoleLog("time=" + time);

                match = Regex.Match(response, "data-id-serial=\"(.*?)\"");
                if (match.Success) {
                    string serialId = match.Groups[1].Value;
                    //context.ConsoleLog("serialId=" + serialId);

                    match = Regex.Match(response, "data-id-season=\"(.*?)\"");
                    if (match.Success) {
                        string seasonId = match.Groups[1].Value;

                        //context.ConsoleLog("seasonId=" + seasonId);
                        Item item = new GetSerialInfoCommand().GetItem(context, seasonId,response);

                        var dataRequest = new Dictionary<string, string>() {
                            {"type", "html5"},
                            {"id", seasonId},
                            {"serial", serialId},
                            {"secure", secure},
                            {"time", time}
                        };
                        header = new Dictionary<string, string>() {
                            {"X-Requested-With", "XMLHttpRequest"},
                            {"Accept-Encoding", "gzip, deflate, lzma"},
                            //{"Content-Type", "text/html; charset=UTF-8"}
                        };
                        string datastring = "";
                        foreach (var k in dataRequest) {
                            if (datastring != "") datastring += "&";
                            datastring += WebUtility.UrlEncode(k.Key) + "=" +
                                          WebUtility.UrlEncode(k.Value);
                        }

                        //context.ConsoleLog(string.Format(Seasonvar.SITE_URL, "/player.php") + " datastring=" +
                        //                   datastring);
                        response = HTTPUtility
                            .PostRequest(string.Format(Seasonvar.SITE_URL, "/player.php"), datastring, header)
                            .Replace("\n", "");
                        //context.ConsoleLog("response=" + response.Substring(0, 1000));
                        var matches0 = Regex.Matches(response, " pl = {'0': \"(.*?)\"");

                        var matches = Regex.Matches(response,
                            "data-translate=\"([^0].*?)\">(.*?)</li.{1,30}>pl\\[.*?\"(.*?)\"");
                        //context.ConsoleLog("matches0=" + matches0.Count);
                        //context.ConsoleLog("matches=" + matches.Count);
                        if (matches0.Count == 1 && matches.Count < 2) {
                            data[2] = matches0[0].Groups[1].Value;
                            return new GetSeriesListCommand().GetItems(context, data);
                        } else {

                            if (matches0.Count > 0) {
                                var itemM = new Item() {
                                    Name = "Стандартный",
                                    Link =
                                        string.Format("{1}{0}{2}", Seasonvar.SEPARATOR, "series",
                                            HttpUtility.UrlDecode(matches0[0].Groups[1].Value)),
                                    ImageLink = item.ImageLink,
                                    Description = item.Description
                                };
                                items.Add(itemM);
                            }
                            for (int i = 0; i < matches.Count; i++) {
                                var itemM = new Item() {
                                    Name = matches[i].Groups[2].Value.Trim(),
                                    Link =
                                        string.Format("{1}{0}{2}", Seasonvar.SEPARATOR, "series",
                                            HttpUtility.UrlDecode(matches[i].Groups[3].Value)),
                                    ImageLink = item.ImageLink,
                                    Description = item.Description
                                };

                                items.Add(itemM);
                            }
                        }
                    }
                }
            }

            return items;
        }
    }
}
