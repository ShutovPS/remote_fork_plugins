using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExplode.Models;
using YoutubeExplode.Models.MediaStreams;

namespace YoutubeExplode
{
    /// <summary>
    /// Interface for <see cref="YoutubeClient"/>.
    /// </summary>
    public interface IYoutubeClient
    {
        #region Video

        /// <summary>
        /// Gets video information by ID.
        /// </summary>
        Task<Video> GetVideoAsync(string videoId);

        /// <summary>
        /// Gets a set of all available media stream infos for given video.
        /// </summary>
        Task<MediaStreamInfoSet> GetVideoMediaStreamInfosAsync(string videoId);

        #endregion

        #region MediaStream

        /// <summary>
        /// Gets the media stream associated with given metadata.
        /// </summary>
        Task<MediaStream> GetMediaStreamAsync(MediaStreamInfo info);

        /// <summary>
        /// Downloads the stream associated with given metadata to the output stream.
        /// </summary>
        Task DownloadMediaStreamAsync(MediaStreamInfo info, Stream output,
            IProgress<double> progress = null, CancellationToken cancellationToken = default);

#if NETSTANDARD2_0 || NET45

        /// <summary>
        /// Downloads the stream associated with given metadata to a file.
        /// </summary>
        Task DownloadMediaStreamAsync(MediaStreamInfo info, string filePath,
            IProgress<double> progress = null, CancellationToken cancellationToken = default);

#endif

        #endregion
    }
}