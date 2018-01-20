using System;
using System.Collections.Generic;
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
        public static Dictionary<string, string> GetFileList(string pathTorrent) {
            string aceMadiaInfo =
                HTTPUtility.GetRequest(string.Format(PluginSettings.Settings.AceStreamApi.GetMediaFiles,
                    NnmClub.GetAddress, pathTorrent));
            if (!string.IsNullOrWhiteSpace(aceMadiaInfo)) {
                var magnet = JsonConvert.DeserializeObject<Magnet>(aceMadiaInfo);
                if (!string.IsNullOrEmpty(magnet.error)) {
                    throw new Exception(magnet.error);
                }
                return magnet.result;
            }

            return new Dictionary<string, string>();
        }
    }
}
