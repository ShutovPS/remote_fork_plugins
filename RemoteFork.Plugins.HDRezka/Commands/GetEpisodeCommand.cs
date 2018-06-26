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
                string scriptUrl = regex.Match(response).Groups[2].Value;
                var myUri = new Uri(url);

                scriptUrl = $"{myUri.Scheme}://{myUri.Host}{scriptUrl}";

                string scriptResponse = HTTPUtility.GetRequest(scriptUrl);

                regex = new Regex(PluginSettings.Settings.Regexp.VideoManifest);
                if (regex.IsMatch(scriptResponse)) {
                    string script = regex.Match(scriptResponse).Groups[2].Value;
                    regex = new Regex(PluginSettings.Settings.Regexp.Password);
                    string password = regex.Match(script).Groups[2].Value;
                    regex = new Regex(PluginSettings.Settings.Regexp.IV);
                    string iv = regex.Match(script).Groups[2].Value;
                    regex = new Regex(PluginSettings.Settings.Regexp.VideoToken);
                    string videoToken = regex.Match(response).Groups[2].Value;
                    regex = new Regex(PluginSettings.Settings.Regexp.PartnerId);
                    string partnerID = regex.Match(response).Groups[2].Value;
                    regex = new Regex(PluginSettings.Settings.Regexp.DomainId);
                    string domainID = regex.Match(response).Groups[2].Value;

                    var o = new {
                        a = int.Parse(partnerID),
                        b = int.Parse(domainID),
                        e = videoToken,
                        f = ProgramSettings.Settings.UserAgent
                    };
                    string q = JsonConvert.SerializeObject(o);
                    q = CryptoManager.Encrypt(q, password, iv);
                    q = WebUtility.UrlEncode(q);
                    q = $"q={q}";

                    response = HTTPUtility.PostRequest($"{PluginSettings.Settings.Links.Moonwalk}/vs", q);

                    regex = new Regex(PluginSettings.Settings.Regexp.M3U8);
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
            }

            return items;
        }
    }
}
