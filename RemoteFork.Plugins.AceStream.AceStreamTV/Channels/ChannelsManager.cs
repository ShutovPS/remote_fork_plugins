using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using RemoteFork.Network;

namespace RemoteFork.Plugins.AceStream.Channels {
    public static class ChannelsManager {
        private static readonly Dictionary<string, ChannelsModel> _channels = new Dictionary<string, ChannelsModel>();

        public static void SetChannelsJson(string key, string json) {
            var channels = JsonConvert.DeserializeObject<ChannelsModel>(json);
            if (_channels.ContainsKey(key)) {
                _channels[key] = channels;
            } else {
                _channels.Add(key, channels);
            }
        }

        public static ChannelsModel GetModel(string key) {
            if (!_channels.ContainsKey(key)) {
                string response = HTTPUtility.GetRequest("https://pomoyka.lib.emergate.net/trash/ttv-list/" + key);

                if (!string.IsNullOrEmpty(response)) {
                    try {
                        SetChannelsJson(key, response);
                    } catch {
                        // ignored
                    }
                }
            }

            if (!_channels.ContainsKey(key)) {
                return null;
            }

            return _channels[key];
        }

        public static List<ChannelsModel.ChannelModel> GetChannelsByCategory(string key, string category) {
            var result = new List<ChannelsModel.ChannelModel>();

            if (GetModel(key) != null) {
                result.AddRange(_channels[key].List
                    .FindAll(i => i.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(i => i.Name));
            }

            return result;
        }

        public static List<string> GetCategories(string key) {
            var result = new List<string>();

            if (GetModel(key) != null) {
                result.AddRange(_channels[key].List.Select(i => i.Category).Distinct().OrderBy(i => i));
            }

            return result;
        }
    }
}
