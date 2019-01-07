using Newtonsoft.Json;

namespace RemoteFork.Plugins {
    public partial class SerialModel {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("folder")]
        public Folder[] Folder { get; set; }
    }

    public partial class Folder {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("file")]
        public string File { get; set; }
    }
}
