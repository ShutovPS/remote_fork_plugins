using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using RemoteFork.Items;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetSeriesListCommand : ICommand {
        public const string KEY = "series";

        public const string URL_KEY = "url";

        public void GetItems(PlayList playList, IPluginContext context, Dictionary<string, string> data) {
            string url;

            data.TryGetValue(URL_KEY, out url);

            url = WebUtility.UrlDecode(url);

            var header = new Dictionary<string, string>() {
                {"Accept-Encoding", "gzip, deflate, lzma"},
                //{"Content-Type", "text/html; charset=UTF-8"}
            };

            string response =
                HTTPUtility.GetRequest(PluginSettings.Settings.Links.Site +
                                       url.Replace("transСтандартный", "trans"), header);

            var matches = Regex.Matches(response,
                PluginSettings.Settings.Regexp.GetSeries,
                RegexOptions.Multiline);

            var match = Regex.Match(url, PluginSettings.Settings.Regexp.SerialInfo);

            var item = new GetSerialInfoCommand().GetItem(match.Groups[2].Value, nameof(FileItem.Title));

            for (int i = 0; i < matches.Count; i++) {
                string fileLink = matches[i].Groups[9].Value;
                if (fileLink.StartsWith("#2")) {
                    var regex = new Regex(PluginSettings.Settings.Regexp.FileLink);
                    fileLink = regex.Replace(fileLink, string.Empty);
                    byte[] linkData = Convert.FromBase64String(fileLink.Substring(2));
                    fileLink = Encoding.UTF8.GetString(linkData);
                    fileLink = fileLink.Split(" ")[0];
                }

                string title = string.Format("{0} Серия", matches[i].Groups[3].Value, matches[i].Groups[19].Value);

                var itemR = new FileItem() {
                    Title = title,
                    Link = fileLink,
                    ImageLink = PluginSettings.Settings.Icons.Video,
                    Description = item.Description.Replace(nameof(FileItem.Title), title)
                };

                playList.Items.Add(itemR);
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
