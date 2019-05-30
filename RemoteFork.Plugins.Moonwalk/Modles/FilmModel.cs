using System;
using System.Linq;
using System.Text;
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

        [JsonProperty("kinopoisk_id")]
        public string KinopoiskID { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("camrip", NullValueHandling = NullValueHandling.Ignore)]
        public bool CamRip { get; set; }

        [JsonProperty("source_type", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceType { get; set; }

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

        public string GetTitle() {
            string title = string.Empty;

            if (!string.IsNullOrEmpty(TitleRu)) {
                title = TitleRu;
            }
            if (!string.IsNullOrEmpty(TitleEn)) {
                if (string.IsNullOrEmpty(title)) {
                    title = TitleEn;
                } else {
                    title += $" / {TitleEn}";
                }
            }

            if (Year != 0) {
                title += $" ({Year})";
            }

            return title;
        }

        private string GetDescription() {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(MaterialData.Poster)) {
                sb.AppendLine(
                    $"<div id=\"poster\" style=\"float: left; padding: 4px; background-color: #eeeeee; margin: 0px 13px 1px 0px;\"><img style=\"width: 180px; float: left;\" src=\"{MaterialData.Poster}\" /></div>");
            }

            sb.AppendLine($"<span style=\"color: #3366ff;\"><strong>{GetTitle()}</strong></span><br>");

            if (!string.IsNullOrEmpty(MaterialData.Tagline) && MaterialData.Tagline.Length > 3) {
                sb.AppendLine(
                    $"<span style=\"color: #999999;\">{MaterialData.Tagline}</span><br>");
            }

            if (SeasonEpisodesCount != null) {
                sb.AppendLine($"<strong><span style=\"color: #ff9900;\">Сезоны:</span></strong> {SeasonEpisodesCount.Length}<br />");
            }

            if (!string.IsNullOrEmpty(SourceType) && SourceType.Length > 3) {
                sb.AppendLine($"<strong><span style=\"color: #ff9900;\">Качество:</span></strong> {SourceType}<br />");
            }

            if (MaterialData.Countries != null && MaterialData.Countries.Length != 0) {
                sb.AppendLine(
                    $"<span style=\"color: #339966;\"><strong>Страны:</strong></span> {string.Join(',', MaterialData.Countries.Take(3))}<br>");
            }

            if (MaterialData.Genres != null && MaterialData.Genres.Length != 0) {
                sb.AppendLine(
                    $"<span style=\"color: #339966;\"><strong>Жанры:</strong></span> {string.Join(',', MaterialData.Genres.Take(3))}<br>");
            }

            if (MaterialData.Actors != null && MaterialData.Actors.Length != 0) {
                sb.AppendLine(
                    $"<span style=\"color: #339966;\"><strong>Актеры:</strong></span> {string.Join(',', MaterialData.Actors.Take(3))}<br>");
            }

            if (MaterialData.Directors != null && MaterialData.Directors.Length != 0) {
                sb.AppendLine(
                    $"<span style=\"color: #339966;\"><strong>Режисеры:</strong></span> {string.Join(',', MaterialData.Directors.Take(3))}<br>");
            }

            if (!string.IsNullOrEmpty(KinopoiskID)) {
                sb.AppendLine(
                    $"<img style=\"padding: 5px;\" src=\"https://rating.kinopoisk.ru/{KinopoiskID}.gif\" align=\"absmiddle\" />");
            }

            if (!string.IsNullOrEmpty(MaterialData.Description)) {
                sb.AppendLine(
                    $"<p>{MaterialData.Description}</p>");
            }

            return sb.ToString();
        }

        private string GetMiniDescription() {
            var sb = new StringBuilder();

            sb.AppendLine($"<span style=\"color: #3366ff;\"><strong>{GetTitle()}</strong></span><br>");

            if (SeasonEpisodesCount != null) {
                sb.AppendLine($"<strong><span style=\"color: #ff9900;\">Сезоны:</span></strong> {SeasonEpisodesCount.Length}<br />");
            }

            if (!string.IsNullOrEmpty(KinopoiskID)) {
                sb.AppendLine(
                    $"<img style=\"padding: 5px;\" src=\"https://rating.kinopoisk.ru/{KinopoiskID}.gif\" align=\"absmiddle\" />");
            }

            return sb.ToString();
        }

        public override string ToString() {
            if (MaterialData != null) {
                return GetDescription();
            }

            return GetMiniDescription();
        }

        public class MaterialDataModel {
            [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
            public DateTimeOffset UpdatedAt { get; set; }

            [JsonProperty("poster", NullValueHandling = NullValueHandling.Ignore)]
            public string Poster { get; set; }

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

            [JsonProperty("kinopoisk_rating", NullValueHandling = NullValueHandling.Ignore)]
            public string KinopoiskRating { get; set; }

            [JsonProperty("kinopoisk_votes", NullValueHandling = NullValueHandling.Ignore)]
            public string KinopoiskVotes { get; set; }

            [JsonProperty("imdb_rating", NullValueHandling = NullValueHandling.Ignore)]
            public string ImdbRating { get; set; }

            [JsonProperty("imdb_votes", NullValueHandling = NullValueHandling.Ignore)]
            public string ImdbVotes { get; set; }

            [JsonProperty("mpaa_rating", NullValueHandling = NullValueHandling.Ignore)]
            public string MpaaRating { get; set; }

            [JsonProperty("mpaa_votes", NullValueHandling = NullValueHandling.Ignore)]
            public string MpaaVotes { get; set; }
        }

        public class SeasonEpisodesCountModel {
            [JsonProperty("season_number", NullValueHandling = NullValueHandling.Ignore)]
            public int SeasonNumber { get; set; }

            [JsonProperty("episodes_count", NullValueHandling = NullValueHandling.Ignore)]
            public int EpisodesCount { get; set; }
        }
    }
}
