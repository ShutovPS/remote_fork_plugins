using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using RemoteFork.Items;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetVoiceListCommand : ICommand {
        public const string KEY = "voice";

        public const string URL_KEY = "url";

        public void GetItems(PlayList playList, IPluginContext context, Dictionary<string, string> data) {
            string url;

            data.TryGetValue(URL_KEY, out url);

            url = WebUtility.UrlDecode(url);

            var header = new Dictionary<string, string>() {
                {"Accept-Encoding", "gzip, deflate, lzma"},
                //{"Content-Type", "text/html; charset=UTF-8"}
            };

            string response = HTTPUtility.GetRequest(PluginSettings.Settings.Links.Site + url, header)
                .Replace("\n", " ");

            var match = Regex.Match(response, PluginSettings.Settings.Regexp.SecureMark);
            if (match.Success) {
                string secure = match.Groups[2].Value;
                string time = match.Groups[3].Value;

                match = Regex.Match(response, PluginSettings.Settings.Regexp.SerialData);
                if (match.Success) {
                    string serialId = match.Groups[1].Value;

                    match = Regex.Match(response, PluginSettings.Settings.Regexp.SeasonMiniData);
                    if (match.Success) {
                        string seasonId = match.Groups[1].Value;

                        var item = new GetSerialInfoCommand().GetItem(seasonId, nameof(FileItem.Title), response);

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

                        response = HTTPUtility
                            .PostRequest(PluginSettings.Settings.Links.Site + "/player.php", datastring, header)
                            .Replace("\n", "");

                        var regex = new Regex(PluginSettings.Settings.Regexp.PlayList);

                        var matches0 = regex.Matches(response);

                        regex = new Regex(PluginSettings.Settings.Regexp.Translate);

                        var matches = regex.Matches(response);

                        if (matches0.Count == 1 && matches.Count < 2) {
                            data[URL_KEY] = matches0[0].Groups[1].Value;
                            new GetSeriesListCommand().GetItems(playList, context, data);
                            return;
                        } else {
                            if (matches0.Count > 0) {
                                var itemM = new DirectoryItem() {
                                    Title = "Стандартный",
                                    Link = GetSeriesListCommand.CreateLink(matches0[0].Groups[1].Value),
                                    ImageLink = item.ImageLink,
                                    Description = item.Description.Replace(nameof(FileItem.Title), "Стандартный")
                                };
                                playList.Items.Add(itemM);
                            }

                            for (int i = 0; i < matches.Count; i++) {
                                string title = matches[i].Groups[2].Value.Trim();

                                var itemM = new DirectoryItem() {
                                    Title = title,
                                    Link = GetSeriesListCommand.CreateLink(matches[i].Groups[3].Value),
                                    ImageLink = item.ImageLink,
                                    Description = item.Description.Replace(nameof(FileItem.Title), title)
                                };

                                playList.Items.Add(itemM);
                            }
                        }
                    }
                }
            }
        }

        public static string CreateLink(string url) {
            var data = new Dictionary<string, object>() {
                {Seasonvar.KEY, KEY},
                {URL_KEY, WebUtility.UrlEncode(url)}
            };
            return Seasonvar.CreateLink(data);
        }
    }
}
