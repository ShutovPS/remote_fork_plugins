using System;
using System.Text.RegularExpressions;
using YoutubeExplode.Internal;

namespace YoutubeExplode
{
    public partial class YoutubeClient
    {
        /// <summary>
        /// Verifies that the given string is syntactically a valid YouTube video ID.
        /// </summary>
        public static bool ValidateVideoId(string videoId)
        {
            if (videoId.IsNullOrWhiteSpace())
                return false;

            // Video IDs are always 11 characters
            if (videoId.Length != 11)
                return false;

            return !Regex.IsMatch(videoId, @"[^0-9a-zA-Z_\-]");
        }
    }
}