using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
                regex = new Regex(PluginSettings.Settings.Regexp.Host);
                string scriptHost = regex.Match(response).Groups[2].Value;
                regex = new Regex(PluginSettings.Settings.Regexp.Proto);
                string scriptProto = regex.Match(response).Groups[2].Value;
                string moonwalkUrl = PluginSettings.Settings.Links.Moonwalk;
                if (!string.IsNullOrEmpty(scriptHost) && !string.IsNullOrEmpty(scriptProto)) {
                    moonwalkUrl = $"{scriptProto}{scriptHost}";
                }
                scriptUrl = $"{moonwalkUrl}{scriptUrl}";

                string scriptResponse = HTTPUtility.GetRequest(scriptUrl);

                regex = new Regex(PluginSettings.Settings.Regexp.VideoManifest);
                if (regex.IsMatch(scriptResponse)) {
                    string script = regex.Match(scriptResponse).Groups[2].Value;
                    regex = new Regex(PluginSettings.Settings.Regexp.Password);
                    string password = regex.Match(script).Groups[2].Value;

                    string iv = string.Empty;
                    regex = new Regex(PluginSettings.Settings.Regexp.IV0);
                    if (regex.IsMatch(script)) {
                        iv = regex.Match(script).Groups[4].Value;
                        if (!string.IsNullOrEmpty(iv)) {
                            regex = new Regex(string.Format(PluginSettings.Settings.Regexp.IV, iv));
                            iv = regex.Match(script).Groups[2].Value;
                        }
                    }

                    if (string.IsNullOrEmpty(iv)) {
                        regex = new Regex(PluginSettings.Settings.Regexp.Ncodes);
                        iv = regex.Match(script).Groups[2].Value;
                        regex = new Regex(PluginSettings.Settings.Regexp.Ncode);
                        var matches = regex.Matches(iv).Select(i => i.Groups[2].Value).Where(i => i.EndsWith('='));
                        matches = matches.OrderByDescending(i => i.Length);
                        iv = matches.First();
                        iv = Base64Decode(iv);
                    }

                    regex = new Regex(PluginSettings.Settings.Regexp.VideoToken);
                    string videoToken = regex.Match(response).Groups[2].Value;
                    regex = new Regex(PluginSettings.Settings.Regexp.PartnerId);
                    string partnerId = regex.Match(response).Groups[2].Value;
                    regex = new Regex(PluginSettings.Settings.Regexp.DomainId);
                    string domainId = regex.Match(response).Groups[2].Value;
                    regex = new Regex(PluginSettings.Settings.Regexp.WindowId);
                    string windowId = regex.Match(response).Groups[2].Value;

                    regex = new Regex(PluginSettings.Settings.Regexp.SecretWindow);
                    string secretWindow = regex.Match(scriptResponse).Groups[4].Value;
                    regex = new Regex(PluginSettings.Settings.Regexp.SecretArray);
                    string secretArray = regex.Match(scriptResponse).Groups[2].Value;
                    regex = new Regex(PluginSettings.Settings.Regexp.Ncode);
                    var secretNumbers = regex.Matches(secretArray).Select(i => i.Groups[2].Value).ToArray();
                    regex = new Regex(PluginSettings.Settings.Regexp.SecretValue);
                    secretArray = string.Empty;
                    foreach (Match match in regex.Matches(secretWindow)) {
                        if (!string.IsNullOrEmpty(match.Groups[3].Value)) {
                            secretArray += Base64Decode(secretNumbers[int.Parse(match.Groups[3].Value)]);
                        } else {
                            secretArray += match.Groups[5];
                        }
                    }

                    var o = new {
                        a = int.Parse(partnerId),
                        b = int.Parse(domainId),
                        d = windowId,
                        e = videoToken,
                        f = ProgramSettings.Settings.UserAgent
                    };
                    string q = JsonConvert.SerializeObject(o);
                    q = CryptoManager.Encrypt(q, secretArray + password, iv);
                    q = WebUtility.UrlEncode(q);
                    q = $"q={q}";

                    response = HTTPUtility.PostRequest($"{moonwalkUrl}/vs", q);

                    if (!string.IsNullOrEmpty(response)) {
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
            }

            return items;
        }

        private static string Base64Decode(string text) {
            byte[] data = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(data);
        }
    }
}
