using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;
using RemoteFork.Settings;
using RemoteFork.Tools;

namespace RemoteFork.Plugins {
    public class GetEpisodeCommand : ICommand {
        public const string KEY = "episode";

        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            return GetEpisodes(WebUtility.UrlDecode(data[2]), data[3]);
        }

        public static List<Item> GetEpisodes(string url, string referer) {
            var header = new Dictionary<string, string>() {
                {"Referer", WebUtility.UrlDecode(referer)}
            };

            string response = HTTPUtility.GetRequest(url, header);

            return GetEpisodesData(url, response);
        }

        private static List<Item> GetEpisodesData(string url, string response) {
            var items = new List<Item>();

            var regex = new Regex(PluginSettings.Settings.Regexp.Script);

            if (regex.IsMatch(response)) {
                regex = new Regex(PluginSettings.Settings.Regexp.Host);
                string scriptHost = regex.Match(response).Groups[2].Value;
                regex = new Regex(PluginSettings.Settings.Regexp.Proto);
                string scriptProto = regex.Match(response).Groups[2].Value;
                string moonwalkUrl = PluginSettings.Settings.Links.Site;
                if (!string.IsNullOrEmpty(scriptHost) && !string.IsNullOrEmpty(scriptProto)) {
                    moonwalkUrl = $"{scriptProto}{scriptHost}";
                }

                regex = new Regex(PluginSettings.Settings.Regexp.VideoToken);
                string videoToken = regex.Match(response).Groups[2].Value;
                regex = new Regex(PluginSettings.Settings.Regexp.PartnerId);
                string partnerId = regex.Match(response).Groups[2].Value;
                regex = new Regex(PluginSettings.Settings.Regexp.DomainId);
                string domainId = regex.Match(response).Groups[2].Value;
                regex = new Regex(PluginSettings.Settings.Regexp.WindowId);
                string windowId = regex.Match(response).Groups[2].Value;
                regex = new Regex(PluginSettings.Settings.Regexp.Ref);
                string scriptRef = regex.Match(response).Groups[2].Value;

                var o = new EncryptedData() {
                    PartnerId = int.Parse(partnerId),
                    DomainId = int.Parse(domainId),
                    Protected = false,
                    //                      WindowId = windowId,
                    WindowId = null,
                    VideoToken = videoToken,
                    UserAgent = ProgramSettings.Settings.UserAgent
                };

                ParseEpisodesData(items, moonwalkUrl, scriptRef, o);

                if (items.Count == 0) {
                    o.DomainId = PluginSettings.Settings.Encryption.DomainId;
                    ParseEpisodesData(items, moonwalkUrl, scriptRef, o);
                }
            }

            return items;
        }

        private static void ParseEpisodesData(List<Item> items, string moonwalkUrl, string scriptRef, EncryptedData o) {
            string q = JsonConvert.SerializeObject(o);

            string response = HTTPUtility.PostRequest($"{moonwalkUrl}/vs",
                string.Format("q={0}&ref={1}", EncryptQ(q), scriptRef));

            ParseEpisodesData(items, response);

            if (items.Count == 0) {
                if (new GetNewKeysCommand().GetItems() != null) {
                    response = HTTPUtility.PostRequest($"{moonwalkUrl}/vs",
                        string.Format("q={0}&ref={1}", EncryptQ(q), scriptRef));

                    ParseEpisodesData(items, response);
                }
            }
        }

        private static void ParseEpisodesData(List<Item> items, string response) {
            if (!string.IsNullOrEmpty(response)) {
                string url = ParseEpisodesLink(response, PluginSettings.Settings.UseMp4);

                if (!string.IsNullOrEmpty(url)) {
                    ParseEpisodesData(items, url, PluginSettings.Settings.UseMp4);
                } else {
                    url = ParseEpisodesLink(response, !PluginSettings.Settings.UseMp4);

                    if (!string.IsNullOrEmpty(url)) {
                        ParseEpisodesData(items, url, !PluginSettings.Settings.UseMp4);
                    }
                }
            }
        }

        private static string ParseEpisodesLink(string response, bool useMp4) {
            if (!string.IsNullOrEmpty(response)) {
                var regex = new Regex(useMp4
                    ? PluginSettings.Settings.Regexp.MP4
                    : PluginSettings.Settings.Regexp.M3U8);

                if (regex.IsMatch(response)) {
                    return regex.Match(response).Groups[2].Value;
                }
            }

            return string.Empty;
        }

        private static void ParseEpisodesData(List<Item> items, string url, bool useMp4) {
            if (!string.IsNullOrEmpty(url)) {
                url = url.ReplaceUnicodeSymbols();
                string response = HTTPUtility.GetRequest(url);

                var regex = new Regex(useMp4
                    ? PluginSettings.Settings.Regexp.Mp4List
                    : PluginSettings.Settings.Regexp.ExtList);

                var baseItem = new Item() {
                    Type = ItemType.FILE,
                    ImageLink = PluginSettings.Settings.Icons.IcoVideo
                };

                foreach (Match match in regex.Matches(response)) {
                    var item = new Item(baseItem) {
                        Name = match.Groups[2].Value,
                        Link = match.Groups[4].Value
                    };
                    items.Add(item);
                }
            }
        }

        private static string EncryptQ(string q) {
            string eq = CryptoManager.Encrypt(q, PluginSettings.Settings.Encryption.Key,
                PluginSettings.Settings.Encryption.IV);

            eq = WebUtility.UrlEncode(eq);

            return eq;
        }

        private class EncryptedData {
            [JsonProperty("a")] public int PartnerId;
            [JsonProperty("b")] public int DomainId;
            [JsonProperty("c")] public bool Protected;

            [JsonProperty("d", NullValueHandling = NullValueHandling.Ignore)]
            public string WindowId;

            [JsonProperty("e")] public string VideoToken;
            [JsonProperty("f")] public string UserAgent;
        }
    }
}
