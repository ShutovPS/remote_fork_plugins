using Newtonsoft.Json;

namespace RemoteFork.Plugins {
    class FilmModel {
        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("pl", NullValueHandling = NullValueHandling.Ignore)]
        public Pl Pl { get; set; }

        [JsonProperty("sources", NullValueHandling = NullValueHandling.Ignore)]
        public Sources Sources { get; set; }

        [JsonProperty("comment", NullValueHandling = NullValueHandling.Ignore)]
        public string Comment { get; set; }
    }

    public partial class Pl {
        [JsonProperty("mp4")]
        public Mp4 Mp4 { get; set; }
    }

    public partial class Mp4 {
        [JsonProperty("playlist")]
        public Mp4Playlist[] Playlist { get; set; }
    }

    public partial class Mp4Playlist {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        public string File { get; set; }

        [JsonProperty("playlist", NullValueHandling = NullValueHandling.Ignore)]
        public Mp4Playlist[] Playlist { get; set; }
    }

    public partial class Sources {
        [JsonProperty("mp4")]
        public string Mp4 { get; set; }
    }
}
