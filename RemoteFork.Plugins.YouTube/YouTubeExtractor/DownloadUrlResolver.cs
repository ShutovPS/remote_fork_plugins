using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace YoutubeExtractor {
    /// <summary>
    /// Provides a method to get the download link of a YouTube video.
    /// </summary>
    public static class DownloadUrlResolver {
        private const string RateBypassFlag = "ratebypass";
        private const string SignatureQuery = "signature";

        /// <summary>
        /// Decrypts the signature in the <see cref="VideoInfo.DownloadUrl" /> property and sets it
        /// to the decrypted URL. Use this method, if you have decryptSignature in the <see
        /// cref="GetDownloadUrls" /> method set to false.
        /// </summary>
        /// <param name="videoInfo">The video info which's downlaod URL should be decrypted.</param>
        /// <exception cref="YoutubeParseException">
        /// There was an error while deciphering the signature.
        /// </exception>
        public static void DecryptDownloadUrl(VideoInfo videoInfo) {
            IDictionary<string, string> queries = HttpHelper.ParseQueryString(videoInfo.DownloadUrl);

            if (queries.ContainsKey(SignatureQuery)) {
                string encryptedSignature = queries[SignatureQuery];

                string decrypted;

                try {
                    decrypted = GetDecipheredSignature(videoInfo.HtmlPlayerVersion, encryptedSignature);
                } catch (Exception ex) {
                    throw new YoutubeParseException("Could not decipher signature", ex);
                }

                videoInfo.DownloadUrl =
                    HttpHelper.ReplaceQueryStringParameter(videoInfo.DownloadUrl, SignatureQuery, decrypted);
                videoInfo.RequiresDecryption = false;
            }
        }

        public static IEnumerable<VideoInfo> GetDownloadUrlsFromID(string youtubeID, bool decryptSignature = true) {
            string url = string.Format("http://www.youtube.com/watch?v={0}", youtubeID);
            return GetDownloadUrls(url, decryptSignature);
        }

        /// <summary>
        /// Gets a list of <see cref="VideoInfo" />s for the specified URL.
        /// </summary>
        /// <param name="videoUrl">The URL of the YouTube video.</param>
        /// <param name="decryptSignature">
        /// A value indicating whether the video signatures should be decrypted or not. Decrypting
        /// consists of a HTTP request for each <see cref="VideoInfo" />, so you may want to set
        /// this to false and call <see cref="DecryptDownloadUrl" /> on your selected <see
        /// cref="VideoInfo" /> later.
        /// </param>
        /// <returns>A list of <see cref="VideoInfo" />s that can be used to download the video.</returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="videoUrl" /> parameter is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The <paramref name="videoUrl" /> parameter is not a valid YouTube URL.
        /// </exception>
        /// <exception cref="VideoNotAvailableException">The video is not available.</exception>
        /// <exception cref="WebException">
        /// An error occurred while downloading the YouTube page html.
        /// </exception>
        /// <exception cref="YoutubeParseException">The Youtube page could not be parsed.</exception>
        public static IEnumerable<VideoInfo> GetDownloadUrls(string videoUrl, bool decryptSignature = true) {
            if (videoUrl == null)
                throw new ArgumentNullException("videoUrl");
            string videoID;
            bool isYoutubeUrl = TryNormalizeYoutubeUrl(videoUrl, out videoUrl, out videoID);

            if (!isYoutubeUrl) {
                throw new ArgumentException("URL is not a valid youtube URL!");
            }

            try {
                var json = LoadJson(videoUrl);

                string videoTitle = GetVideoTitle(json);
                string sts = json["sts"].Value<string>();

                Dictionary<string, string> videoInfo = GetVideoInfo(videoID, sts);

                IEnumerable<ExtractionInfo> downloadUrls = ExtractDownloadUrls(videoInfo);

                IEnumerable<VideoInfo> infos = GetVideoInfos(downloadUrls, videoTitle).ToList();


                string htmlPlayerVersion = GetHtml5PlayerVersion(json);
                foreach (VideoInfo info in infos) {
                    info.HtmlPlayerVersion = htmlPlayerVersion;

                    if (decryptSignature && info.RequiresDecryption) {
                        DecryptDownloadUrl(info);
                    }
                }

                return infos;
            } catch (Exception ex) {
                if (ex is WebException || ex is VideoNotAvailableException) {
                    throw;
                }

                ThrowYoutubeParseException(ex, videoUrl);
            }

            return null; // Will never happen, but the compiler requires it
        }

#if PORTABLE
        public static System.Threading.Tasks.Task<IEnumerable<VideoInfo>> GetDownloadUrlsFromIDAsync(string youtubeID, bool decryptSignature
 = true)
        {
            string url = string.Format("http://www.youtube.com/watch?v={0}", youtubeID);
            return GetDownloadUrlsAsync(url, decryptSignature);
        }
        public static System.Threading.Tasks.Task<IEnumerable<VideoInfo>> GetDownloadUrlsAsync(string videoUrl, bool decryptSignature
 = true)
        {
            return System.Threading.Tasks.Task.Run(() => GetDownloadUrls(videoUrl, decryptSignature));
        }

#endif

        private static string GetVideoInfoRawAsync(string videoId, string el = "", string sts = "") {
            var url = $"https://www.youtube.com/get_video_info?video_id={videoId}&el={el}&sts={sts}&hl=en";
            string extractedJson = HttpHelper.DownloadString(url);
            return extractedJson;
        }

        private static Dictionary<string, string> GetVideoInfo(string videoID, string sts) {
            //string url = String.Format("https://www.youtube.com/get_video_info?video_id={0}&el={1}&sts={2}&hl=en", videoID, "embedded", sts);
            //string extractedJson = HttpHelper.DownloadString(url);
            string extractedJson = GetVideoInfoRawAsync(videoID, "embedded", sts);
            var videoInfo = SplitQuery(extractedJson);
            // If can't be embedded - try another value of el
            if (videoInfo.ContainsKey("errorcode")) {
                var errorReason = videoInfo["reason"];
                if (errorReason.Contains("&feature=player_embedded")) {
                    extractedJson = GetVideoInfoRawAsync(videoID, "detailpage", sts);
                    videoInfo = SplitQuery(extractedJson);
                }
            }

            // Check error
            if (videoInfo.ContainsKey("errorcode")) {
                var errorReason = videoInfo["reason"];

                throw new VideoNotAvailableException(errorReason);
            }

            return videoInfo;
        }

        private static Dictionary<string, string> SplitQuery(string query) {
            var dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var rawParams = query.Split('&');
            foreach (var rawParam in rawParams) {
                var param = WebUtility.UrlDecode(rawParam);

                // Look for the equals sign
                var equalsPos = param.IndexOf('=');
                if (equalsPos <= 0)
                    continue;

                // Get the key and value
                var key = param.Substring(0, equalsPos);
                var value = equalsPos < param.Length
                    ? param.Substring(equalsPos + 1)
                    : string.Empty;

                // Add to dictionary
                dic[key] = value;
            }

            return dic;
        }

        /// <summary>
        /// Normalizes the given YouTube URL to the format http://youtube.com/watch?v={youtube-id}
        /// and returns whether the normalization was successful or not.
        /// </summary>
        /// <param name="url">The YouTube URL to normalize.</param>
        /// <param name="normalizedUrl">The normalized YouTube URL.</param>
        /// <returns>
        /// <c>true</c>, if the normalization was successful; <c>false</c>, if the URL is invalid.
        /// </returns>
        public static bool TryNormalizeYoutubeUrl(string url, out string normalizedUrl, out string videoID) {
            string id = "";
            if (TryParseVideoId(url, out id)) {
                videoID = id;
                normalizedUrl = String.Format("https://www.youtube.com/embed/{0}?disable_polymer=true&hl=en", videoID);
                return true;
            } else {
                normalizedUrl = null;
                videoID = null;
                return false;
            }
        }

        /// <summary>
        /// Tries to parse video ID from a YouTube video URL.
        /// </summary>
        public static bool TryParseVideoId(string videoUrl, out string videoId) {
            videoId = default(string);

            if (String.IsNullOrEmpty(videoUrl))
                return false;

            // https://www.youtube.com/watch?v=yIVRs6YSbOM
            var regularMatch =
                Regex.Match(videoUrl, @"youtube\..+?/watch.*?v=(.*?)(?:&|/|$)").Groups[1].Value;
            if (!string.IsNullOrEmpty(regularMatch) && ValidateVideoId(regularMatch)) {
                videoId = regularMatch;
                return true;
            }

            // https://youtu.be/yIVRs6YSbOM
            var shortMatch =
                Regex.Match(videoUrl, @"youtu\.be/(.*?)(?:\?|&|/|$)").Groups[1].Value;
            if (!string.IsNullOrEmpty(shortMatch) && ValidateVideoId(shortMatch)) {
                videoId = shortMatch;
                return true;
            }

            // https://www.youtube.com/embed/yIVRs6YSbOM
            var embedMatch =
                Regex.Match(videoUrl, @"youtube\..+?/embed/(.*?)(?:\?|&|/|$)").Groups[1].Value;
            if (!string.IsNullOrEmpty(embedMatch) && ValidateVideoId(embedMatch)) {
                videoId = embedMatch;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Verifies that the given string is syntactically a valid YouTube video ID.
        /// </summary>
        public static bool ValidateVideoId(string videoId) {
            if (String.IsNullOrEmpty(videoId))
                return false;

            if (videoId.Length != 11)
                return false;

            return !Regex.IsMatch(videoId, @"[^0-9a-zA-Z_\-]");
        }

        /// <summary>
        /// Parses video ID from a YouTube video URL.
        /// </summary>
        public static string ParseVideoId(string videoUrl) {
            string result = "";
            if (TryParseVideoId(videoUrl, out result)) {
                return result;
            } else {
                throw new FormatException($"Could not parse video ID from given string [{videoUrl}].");
            }
        }

        private static IEnumerable<ExtractionInfo> ExtractDownloadUrls(Dictionary<string, string> values) {
            string[] splitByUrls = GetStreamMap(values).Split(',');
            string[] adaptiveFmtSplitByUrls = GetAdaptiveStreamMap(values).Split(',');
            splitByUrls = splitByUrls.Concat(adaptiveFmtSplitByUrls).ToArray();

            foreach (string s in splitByUrls) {
                IDictionary<string, string> queries = HttpHelper.ParseQueryString(s);
                string url;

                bool requiresDecryption = false;

                if (queries.ContainsKey("s") || queries.ContainsKey("sig")) {
                    requiresDecryption = queries.ContainsKey("s");
                    string signature = queries.ContainsKey("s") ? queries["s"] : queries["sig"];

                    url = string.Format("{0}&{1}={2}", queries["url"], SignatureQuery, signature);

                    string fallbackHost = queries.ContainsKey("fallback_host")
                        ? "&fallback_host=" + queries["fallback_host"]
                        : String.Empty;

                    url += fallbackHost;
                } else {
                    url = queries["url"];
                }

                url = HttpHelper.UrlDecode(url);
                url = HttpHelper.UrlDecode(url);

                IDictionary<string, string> parameters = HttpHelper.ParseQueryString(url);
                if (!parameters.ContainsKey(RateBypassFlag))
                    url += string.Format("&{0}={1}", RateBypassFlag, "yes");

                yield return new ExtractionInfo {RequiresDecryption = requiresDecryption, Uri = new Uri(url)};
            }
        }

        private static string GetAdaptiveStreamMap(Dictionary<string, string> values) {
            JToken streamMap = null;

            if (values.ContainsKey("adaptive_fmts")) {
                streamMap = values["adaptive_fmts"];
            }

            ;

            // bugfix: adaptive_fmts is missing in some videos, use url_encoded_fmt_stream_map instead
            if (streamMap == null) {
                streamMap = values["url_encoded_fmt_stream_map"];
            }

            return streamMap.ToString();
        }

        private static string GetDecipheredSignature(string htmlPlayerVersion, string signature) {
            return Decipherer.DecipherWithVersion(signature, htmlPlayerVersion);
        }

        private static string GetHtml5PlayerVersion(JObject json) {
            var regex = new Regex(@"player(?:[^-]+?)?-(.+?).js");

            string js = json["assets"]["js"].ToString();
            Match m = regex.Match(js);
            return m.Result("$1");
        }

        private static string GetStreamMap(Dictionary<string, string> values) {
            JToken streamMap = values["url_encoded_fmt_stream_map"];

            string streamMapString = streamMap == null ? null : streamMap.ToString();

            if (streamMapString == null || streamMapString.Contains("been+removed")) {
                throw new VideoNotAvailableException("Video is removed or has an age restriction.");
            }

            return streamMapString;
        }

        private static IEnumerable<VideoInfo> GetVideoInfos(IEnumerable<ExtractionInfo> extractionInfos,
            string videoTitle) {
            var downLoadInfos = new List<VideoInfo>();

            foreach (ExtractionInfo extractionInfo in extractionInfos) {
                string itag = HttpHelper.ParseQueryString(extractionInfo.Uri.Query)["itag"];

                int formatCode = int.Parse(itag);

                VideoInfo info = VideoInfo.Defaults.SingleOrDefault(videoInfo => videoInfo.FormatCode == formatCode);

                if (info != null) {
                    info = new VideoInfo(info) {
                        DownloadUrl = extractionInfo.Uri.ToString(),
                        Title = videoTitle,
                        RequiresDecryption = extractionInfo.RequiresDecryption
                    };
                } else {
                    info = new VideoInfo(formatCode) {
                        DownloadUrl = extractionInfo.Uri.ToString()
                    };
                }

                downLoadInfos.Add(info);
            }

            return downLoadInfos;
        }

        private static string GetVideoTitle(JObject json) {
            JToken title = json["args"]["title"];

            return title == null ? String.Empty : title.ToString();
        }

        private static bool IsVideoUnavailable(string pageSource) {
            const string unavailableContainer = "<div id=\"watch-player-unavailable\">";

            return pageSource.Contains(unavailableContainer);
        }

        private static JObject LoadJson(string url) {
            string pageSource = HttpHelper.DownloadString(url);

            if (IsVideoUnavailable(pageSource)) {
                throw new VideoNotAvailableException();
            }

            string extractedJson = pageSource.Between("yt.setConfig({'PLAYER_CONFIG': ", ",'");
            return JObject.Parse(extractedJson);
        }

        private static void ThrowYoutubeParseException(Exception innerException, string videoUrl) {
            throw new YoutubeParseException("Could not parse the Youtube page for URL " + videoUrl + "\n" +
                                            "This may be due to a change of the Youtube page structure.\n" +
                                            "Please report this bug at www.github.com/flagbug/YoutubeExtractor/issues",
                innerException);
        }

        private class ExtractionInfo {
            public bool RequiresDecryption { get; set; }

            public Uri Uri { get; set; }
        }

        /// <summary>
        /// Get string value between [first] a and [last] b.
        /// </summary>
        private static string Between(this string value, string a, string b) {
            int posA = value.IndexOf(a, StringComparison.Ordinal);
            int posB = value.LastIndexOf(b, StringComparison.Ordinal);
            if (posA == -1) {
                return "";
            }

            if (posB == -1) {
                return "";
            }

            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB) {
                return "";
            }

            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }
    }
}
