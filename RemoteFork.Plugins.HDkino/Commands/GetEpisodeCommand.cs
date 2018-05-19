using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RemoteFork.Network;
using RemoteFork.Settings;

namespace RemoteFork.Plugins {
    public class GetEpisodeCommand : ICommand {
        public const string KEY = "episode";

        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            var header = new Dictionary<string, string>() {
                {"Referer", WebUtility.UrlDecode(data[4])}
            };
            string url = WebUtility.UrlDecode(data[3]);

            string response = HTTPUtility.GetRequest(url, header);
            
            var regex = new Regex("(<script src=\")(.*?)(\">)");

            if (regex.IsMatch(response)) {
                string scriptUrl = regex.Match(response).Groups[2].Value;
                var myUri = new Uri(url);

                scriptUrl = $"{myUri.Scheme}://{myUri.Host}{scriptUrl}";

                string scriptResponse = HTTPUtility.GetRequest(scriptUrl);

                regex = new Regex("(getVideoManifests:\\s*function)([\\s\\S]*?)(o\\.done)");
                if (regex.IsMatch(scriptResponse)) {
                    string script = regex.Match(scriptResponse).Groups[2].Value;
                    regex = new Regex("(e\\s*=\\s*\")(.*?)(\")");
                    string password = regex.Match(script).Groups[2].Value;
                    regex = new Regex("(n\\s*=\\s*\")(.*?)(\")");
                    string iv = regex.Match(script).Groups[2].Value;
                    regex = new Regex("(video_token:\\s*\')(.*?)(\')");
                    string videoToken = regex.Match(response).Groups[2].Value;
                    regex = new Regex("(partner_id:\\s*)(\\d+)");
                    string partnerID = regex.Match(response).Groups[2].Value;
                    regex = new Regex("(domain_id:\\s*)(\\d+)");
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

                    response = HTTPUtility.PostRequest("http://moonwalk.cc/vs", q);

                    regex = new Regex("(\"m3u8\":\\s*\")(.*?)(\")");
                    if (regex.IsMatch(response)) {
                        response = HTTPUtility.GetRequest(regex.Match(response).Groups[2].Value);

                        regex = new Regex("(#EXT-X.*?=)(\\d+x\\d+)([\\s\\S]*?)(http:.*)");

                        var baseItem = new Item() {
                            Type = ItemType.FILE,
                            ImageLink = "http://s1.iconbird.com/ico/1012/AmpolaIcons/w256h2561350597291videofile.png"
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
