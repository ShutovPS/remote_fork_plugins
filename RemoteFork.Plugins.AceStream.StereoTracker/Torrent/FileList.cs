using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

[Serializable]
public class Magnet {
    public Dictionary<string,string> result;
    public string error;
}

namespace RemoteFork.Plugins {
    public class FileList {
        public static Dictionary<string, string> GetFileList(byte[] torrentData, ref string id) {
            string contentId = HTTPUtility.PostRequest(PluginSettings.Settings.AceStreamApi.GetContentId, Encoding.UTF8.GetBytes(Convert.ToBase64String(torrentData)));
            var regex = new Regex(PluginSettings.Settings.Regexp.GetContentId);
            if (regex.IsMatch(contentId)) {
                id = regex.Match(contentId).Groups[2].Value;
                string aceMadiaInfo =
                    HTTPUtility.GetRequest(string.Format(PluginSettings.Settings.AceStreamApi.GetMediaFiles,
                        StereoTracker.GetAddress, id));
                if (!string.IsNullOrWhiteSpace(aceMadiaInfo)) {
                    var magnet = JsonConvert.DeserializeObject<Magnet>(aceMadiaInfo);
                    if (!string.IsNullOrEmpty(magnet.error)) {
                        throw new Exception(magnet.error);
                    }
                    return magnet.result;
                }
            }

            return new Dictionary<string, string>();
        }
    }
}
