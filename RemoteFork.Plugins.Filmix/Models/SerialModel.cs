using System;
using Newtonsoft.Json;

namespace RemoteFork.Plugins {
    public partial class SerialModel {
        [JsonProperty("playlist")]
        public SerialModelPlaylist[] Playlist { get; set; }
    }

    public partial class SerialModelPlaylist {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("playlist")]
        public PlaylistPlaylist[] Playlist { get; set; }
    }

    public partial class PlaylistPlaylist {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("folderId")]
        public string FolderId { get; set; }

        [JsonProperty("serieId")]
        public string SerieId { get; set; }

        [JsonProperty("season")]
        public string Season { get; set; }

        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("isFlv")]
        public string IsFlv { get; set; }
    }
}
