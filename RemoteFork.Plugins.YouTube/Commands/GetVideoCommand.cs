using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteFork.Plugins.Settings;
using YoutubeExplode;
using YoutubeExplode.Models;

namespace RemoteFork.Plugins.Commands {
    public class GetVideoCommand : ICommand {
        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            var client = new YoutubeClient();

            var videoInfo = client.GetVideoAsync(data[2]).Result;

            string description = GetDescription(videoInfo);

            var streamInfoSet = client.GetVideoMediaStreamInfosAsync(data[2]).Result;

            foreach (var videoFile in streamInfoSet.Muxed) {
                if (!string.IsNullOrEmpty(videoFile.Url)) {
                    items.Add(new Item() {
                        Name = string.Format("{0}x{1}.{2}", videoFile.Resolution.Width, videoFile.Resolution.Height,videoFile.Container),
                        Description = description,
                        Link = videoFile.Url,
                        ImageLink = PluginSettings.Settings.Icons.IcoVideoFile,
                        Type = ItemType.FILE
                    });
                }
            }

            return items;
        }

        private static string GetDescription(Video video) {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(video.Thumbnails.MaxResUrl)) {
                sb.AppendLine(
                    $"<div id=\"poster\" style=\"float: left; padding: 4px; background-color: #eeeeee; margin: 0px 13px 1px 0px;\"><img style=\"width: 180px; float: left;\" src=\"{video.Thumbnails.MaxResUrl}\" /></div>");
            }

            sb.AppendLine($"<span style=\"color: #3366ff;\"><strong>{video.Title}</strong></span><br />");

            sb.AppendLine($"<span style=\"color: #339966;\"><strong>{video.Author}</strong></span><br />");

            if (video.Keywords.Any()) {
                sb.AppendLine(
                    $"<span style=\"color: #999999;\">{string.Join(", ", video)}</span><br />");
            }

            sb.AppendLine(
                $"<strong><span style=\"color: #ff9900;\">Дата загрузки:</span></strong> {video.UploadDate:dd-MM-YYYY}<br />");

            sb.AppendLine(
                $"<strong><span style=\"color: #ff9900;\">Продолжительность:</span></strong> {video.Duration:h'h 'm'm 's's'}<br />");

            sb.AppendLine(video.Description);

            string description = sb.ToString();

            return description;
        }
    }
}
