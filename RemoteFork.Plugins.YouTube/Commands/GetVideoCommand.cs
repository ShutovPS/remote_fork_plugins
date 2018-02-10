using System.Collections.Generic;
using System.Linq;
using RemoteFork.Plugins.Settings;
using YoutubeExtractor;

namespace RemoteFork.Plugins.Commands {
    public class GetVideoCommand : ICommand {
        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            var videos =
                DownloadUrlResolver.GetDownloadUrls(PluginSettings.Settings.SiteUrl + "/watch?v=" + data[2], false);

            var videoInfos = videos as IList<VideoInfo> ?? videos.ToList();
            foreach (var videoInfo in videoInfos) {
                if (videoInfo.VideoType != VideoType.Unknown && videoInfo.AudioType != AudioType.Unknown &&
                    videoInfo.Resolution > 0) {
                    if (videoInfo.RequiresDecryption) {
                        DownloadUrlResolver.DecryptDownloadUrl(videoInfo);
                    }
                    items.Add(new Item() {
                        Name = videoInfo.Title + " (" + videoInfo.Resolution + ") " + videoInfo.VideoType,
                        Link = videoInfo.DownloadUrl,
                        ImageLink = PluginSettings.Settings.Icons.IcoVideoFile,
                        Type = ItemType.FILE
                    });
                } else if (videoInfo.VideoType != VideoType.Unknown && videoInfo.Resolution > 0) {
                    var audio = videoInfos.FirstOrDefault(a => a.VideoType == videoInfo.VideoType &&
                                                           a.VideoExtension == videoInfo.VideoExtension &&
                                                           a.Resolution == 0);


                }
            }

            return items;
        }
    }
}
