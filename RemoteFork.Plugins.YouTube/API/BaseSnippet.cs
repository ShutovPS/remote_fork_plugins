using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RemoteFork.Plugins.YouTubeAPI {
    [Serializable]
    public class Thumbnails {
        public string url { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int width { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int height { get; set; }
    }

    [Serializable]
    public class Snippet {
        public string channelTitle { get; set; }
        public string channelId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime publishedAt { get; set; }
        public Dictionary<string, Thumbnails> thumbnails { get; set; }
        public string liveBroadcastContent { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int position { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ResourceIdId resourceId { get; set; }
    }

    [Serializable]
    public class ResourceIdId {
        public string kind { get; set; }
        public string videoId { get; set; }
    }
}
