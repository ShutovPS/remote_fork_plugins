using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RemoteFork.Plugins.AceStream {
    [Serializable]
    public class ChannelsModel {
        [JsonProperty("channels")]
        public List<ChannelModel> List { get; set; }

        [Serializable]
        public class ChannelModel {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("cat")]
            public string Category { get; set; }
        }
    }
}
