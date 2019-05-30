using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RemoteFork.Items;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;
using RemoteFork.Settings;
using RemoteFork.Tools;

namespace RemoteFork.Plugins {
    public class GetEpisodeCommand : ICommand {
        public const string KEY = "episode";

        public const string URL_KEY = "url";
        public const string REFERER_KEY = "referer";

        public void GetItems(PlayList playList, IPluginContext context, Dictionary<string, string> data) {
            string url;
            string referer;

            data.TryGetValue(URL_KEY, out url);
            data.TryGetValue(REFERER_KEY, out referer);

            url = WebUtility.UrlDecode(url);
            referer = WebUtility.UrlDecode(referer);

            GetEpisodes(playList, url, referer);
        }

        public static void GetEpisodes(PlayList playList, string url, string referer) {
            var header = new Dictionary<string, string>() {
                {"Referer", referer}
            };

            string response = HTTPUtility.GetRequest(url, header);

            GetEpisodesData(playList, url, response);
        }

        private static void GetEpisodesData(PlayList playList, string url, string response) {
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

                ParseEpisodesData(playList, moonwalkUrl, scriptRef, o);

                if (playList.Items.Count == 0) {
                    o.DomainId = PluginSettings.Settings.Api.DomainId;
                    ParseEpisodesData(playList, moonwalkUrl, scriptRef, o);
                }
            }
        }

        private static void ParseEpisodesData(PlayList playList, string moonwalkUrl, string scriptRef,
            EncryptedData o) {
            string q = JsonConvert.SerializeObject(o);

            string response = HTTPUtility.PostRequest($"{moonwalkUrl}/vs",
                string.Format("q={0}&ref={1}", EncryptQ(q), scriptRef));

            ParseEpisodesData(playList, response);

            if (playList.Items.Count == 0) {
                new GetNewKeysCommand().GetItems(playList, null, null);
                if (playList.IsIptv) {
                    playList.IsIptv = false;

                    response = HTTPUtility.PostRequest($"{moonwalkUrl}/vs",
                        string.Format("q={0}&ref={1}", EncryptQ(q), scriptRef));

                    ParseEpisodesData(playList, response);
                }
            }
        }

        private static void ParseEpisodesData(PlayList playList, string response) {
            if (!string.IsNullOrEmpty(response)) {
                string url = ParseEpisodesLink(response, PluginSettings.Settings.UseMp4);

                if (!string.IsNullOrEmpty(url)) {
                    ParseEpisodesData(playList, url, PluginSettings.Settings.UseMp4);
                } else {
                    url = ParseEpisodesLink(response, !PluginSettings.Settings.UseMp4);

                    if (!string.IsNullOrEmpty(url)) {
                        ParseEpisodesData(playList, url, !PluginSettings.Settings.UseMp4);
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

        private static void ParseEpisodesData(PlayList playList, string url, bool useMp4) {
            if (!string.IsNullOrEmpty(url)) {
                url = url.ReplaceUnicodeSymbols();
                string response = HTTPUtility.GetRequest(url);

                var regex = new Regex(useMp4
                    ? PluginSettings.Settings.Regexp.Mp4List
                    : PluginSettings.Settings.Regexp.ExtList);

                var baseItem = new FileItem() {
                    ImageLink = PluginSettings.Settings.Icons.IcoVideo
                };

                foreach (Match match in regex.Matches(response)) {
                    var item = new FileItem(baseItem) {
                        Title = match.Groups[2].Value,
                        Link = match.Groups[4].Value
                    };
                    playList.Items.Add(item);
                }
            }
        }

        public static string CreateLink(string url, string referer) {
            var data = new Dictionary<string, object>() {
                {Moonwalk.KEY, KEY},
                {URL_KEY, WebUtility.UrlEncode(url)},
                {REFERER_KEY, WebUtility.UrlEncode(referer)},
            };

            return Moonwalk.CreateLink(data);
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
