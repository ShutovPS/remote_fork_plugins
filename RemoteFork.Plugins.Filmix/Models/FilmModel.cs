using System.Collections.Generic;
using Newtonsoft.Json;

namespace RemoteFork.Plugins {
    class FilmModel {
        [JsonProperty("message")]
        public MessageModel Message { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class MessageModel {
        [JsonProperty("translations")]
        public TranslationsModel Translations { get; set; }
    }

    public class TranslationsModel {
        [JsonProperty("html5", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Html5 { get; set; }

        [JsonProperty("pl")]
        public string Pl { get; set; }

        [JsonProperty("ok")]
        public bool Ok { get; set; }
    }
}
