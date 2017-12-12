namespace RemoteFork.Plugins {
    public class Playlist {
        public static readonly Playlist EmptyPlaylist = new Playlist {
            Items = new Item[0]
        };

        public string source { get; set; }
        public string GetInfo { get; set; }
        public string NextPageUrl { get; set; }
        public string IsIptv { get; set; }
        public string Timeout { get; set; }
        public Item[] Items { get; set; }
    }
}
