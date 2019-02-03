using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;
using RemoteFork.Settings;

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
                //regex = new Regex(PluginSettings.Settings.Regexp.DomainId);
                //string domainId = regex.Match(response).Groups[2].Value;
                string domainId = PluginSettings.Settings.Encryption.DomainId;
                regex = new Regex(PluginSettings.Settings.Regexp.WindowId);
                string windowId = regex.Match(response).Groups[2].Value;
                regex = new Regex(PluginSettings.Settings.Regexp.Ref);
                string scriptRef = regex.Match(response).Groups[2].Value;

                var o = new {
                    a = int.Parse(partnerId),
                    b = int.Parse(domainId),
                    c = false,
                    //                      d = windowId,
                    e = videoToken,
                    f = ProgramSettings.Settings.UserAgent
                };
                string q = JsonConvert.SerializeObject(o);

                response = HTTPUtility.PostRequest($"{moonwalkUrl}/vs",
                    string.Format("q={0}&ref={1}", EncryptQ(q), scriptRef));

                items = ParseEpisodesData(response);

                if (items.Count == 0) {
                    if (new GetNewKeysCommand().GetItems() != null) {
                        response = HTTPUtility.PostRequest($"{moonwalkUrl}/vs",
                            string.Format("q={0}&ref={1}", EncryptQ(q), scriptRef));

                        items = ParseEpisodesData(response);
                    }
                }
            }

            return items;
        }

        private static string EncryptQ(string q) {
            string eq = CryptoManager.Encrypt(q, PluginSettings.Settings.Encryption.Key,
                PluginSettings.Settings.Encryption.IV);

            eq = WebUtility.UrlEncode(eq);

            return eq;
        }

        private static List<Item> ParseEpisodesData(string response) {
            var items = new List<Item>();

            if (!string.IsNullOrEmpty(response)) {
                var regex = new Regex(PluginSettings.Settings.Regexp.M3U8);
                if (regex.IsMatch(response)) {
                    response = HTTPUtility.GetRequest(regex.Match(response).Groups[2].Value);

                    regex = new Regex(PluginSettings.Settings.Regexp.ExtList);

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

            return items;
        }
    }
}
