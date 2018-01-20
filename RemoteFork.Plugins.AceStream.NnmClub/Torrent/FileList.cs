using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RemoteFork.Network;

[Serializable]
public class Magnet {
    public Dictionary<string,string> result;
    public string error;
}

namespace RemoteFork.Plugins {
    public class FileList {
        public static Dictionary<string, string> GetFileList(string pathTorrent) {
            string aceMadiaInfo =
                HTTPUtility.GetRequest($"{NnmClub.GetAddress}/server/api?method=get_media_files&magnet={pathTorrent}");
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
