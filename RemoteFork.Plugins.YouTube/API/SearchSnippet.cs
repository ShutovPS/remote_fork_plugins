using System;
using System.Collections.Generic;

namespace RemoteFork.Plugins.YouTubeAPI {
    [Serializable]
    public class SearchSnippet {
        public string kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }
        public string regionCode { get; set; }
        public PageInfo pageInfo { get; set; }
        public List<Item> items { get; set; }

        [Serializable]
        public class PageInfo {
            public string totalResults { get; set; }
            public string resultsPerPage { get; set; }
        }

        [Serializable]
        public class Item {
            public string kind { get; set; }
            public string etag { get; set; }
            public Id id { get; set; }
            public Snippet snippet { get; set; }
        }

        [Serializable]
        public class Id {
            public string kind { get; set; }
            public string videoId { get; set; }
            public string channelId { get; set; }
            public string playlistId { get; set; }
        }
    }
}
