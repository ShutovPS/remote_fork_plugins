using System;
using Newtonsoft.Json;

namespace RemoteFork.Plugins {
    public class FilmModel {
        [JsonProperty("serial")]
        public FilmModel Serial { get; set; }

        [JsonProperty("title_ru", NullValueHandling = NullValueHandling.Ignore)]
        public string TitleRu { get; set; }

        [JsonProperty("title_en", NullValueHandling = NullValueHandling.Ignore)]
        public string TitleEn { get; set; }

        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public int Year { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("camrip", NullValueHandling = NullValueHandling.Ignore)]
        public bool Camrip { get; set; }

        [JsonProperty("source_type", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceType { get; set; }

        [JsonProperty("instream_ads", NullValueHandling = NullValueHandling.Ignore)]
        public bool InstreamAds { get; set; }

        [JsonProperty("directors_version", NullValueHandling = NullValueHandling.Ignore)]
        public bool DirectorsVersion { get; set; }

        [JsonProperty("iframe_url", NullValueHandling = NullValueHandling.Ignore)]
        public string IframeUrl { get; set; }

        [JsonProperty("translator", NullValueHandling = NullValueHandling.Ignore)]
        public string Translator { get; set; }

        [JsonProperty("added_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset AddedAt { get; set; }

        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }

        [JsonProperty("season_episodes_count", NullValueHandling = NullValueHandling.Ignore)]
        public SeasonEpisodesCountModel[] SeasonEpisodesCount { get; set; }

        [JsonProperty("last_episode_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset LastEpisodeTime { get; set; }

        [JsonProperty("material_data", NullValueHandling = NullValueHandling.Ignore)]
        public MaterialDataModel MaterialData { get; set; }

        public class MaterialDataModel {
            [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
            public DateTimeOffset UpdatedAt { get; set; }

            [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
            public int Year { get; set; }

            [JsonProperty("tagline", NullValueHandling = NullValueHandling.Ignore)]
            public string Tagline { get; set; }

            [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
            public string Description { get; set; }

            [JsonProperty("countries", NullValueHandling = NullValueHandling.Ignore)]
            public string[] Countries { get; set; }

            [JsonProperty("genres", NullValueHandling = NullValueHandling.Ignore)]
            public string[] Genres { get; set; }

            [JsonProperty("actors", NullValueHandling = NullValueHandling.Ignore)]
            public string[] Actors { get; set; }

            [JsonProperty("directors", NullValueHandling = NullValueHandling.Ignore)]
            public string[] Directors { get; set; }

            [JsonProperty("studios", NullValueHandling = NullValueHandling.Ignore)]
            public string[] Studios { get; set; }
        }

        public class SeasonEpisodesCountModel {
            [JsonProperty("season_number", NullValueHandling = NullValueHandling.Ignore)]
            public int SeasonNumber { get; set; }

            [JsonProperty("episodes_count", NullValueHandling = NullValueHandling.Ignore)]
            public int EpisodesCount { get; set; }
        }
    }
}
