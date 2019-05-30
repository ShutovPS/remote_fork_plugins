using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RemoteFork.Items;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetNewKeysCommand : ICommand {
        public const string KEY = "newkeys";

        public void GetItems(PlayList playList, IPluginContext context, Dictionary<string, string> data) {
            if (UpdateMoonwalkKeys()) {
                playList.IsIptv = true;
            }
        }

        private static bool UpdateMoonwalkKeys() {
            bool result = false;
            string response = HTTPUtility.GetRequest(PluginSettings.Settings.Api.DataUrl);

            try {
                var data = JsonConvert.DeserializeObject<ApiData>(response);
                PluginSettings.Settings.Api.DomainId = data.Domain;
                PluginSettings.Settings.Api.Key = data.Key;
            } catch (Exception exception) {
                Moonwalk.Logger.LogError(exception);
            }

            response = HTTPUtility.GetRequest(PluginSettings.Settings.Encryption.Url);

            try {
                var regex = new Regex(string.Format(PluginSettings.Settings.Regexp.Ini, "iv"));
                string iv = regex.Match(response).Groups[2].Value;
                if (!string.IsNullOrEmpty(iv)) {
                    PluginSettings.Settings.Encryption.IV = iv;
                    result = true;
                }
            } catch (Exception exception) {
                Moonwalk.Logger.LogError(exception);
            }

            try {
                var regex = new Regex(string.Format(PluginSettings.Settings.Regexp.Ini, "key"));
                string key = regex.Match(response).Groups[2].Value;
                if (!string.IsNullOrEmpty(key)) {
                    PluginSettings.Settings.Encryption.Key = key;
                    result = true;
                }
            } catch (Exception exception) {
                Moonwalk.Logger.LogError(exception);
            }

            if (result) {
                PluginSettings.Instance.Save();
            }

            return result;
        }

        public static string CreateLink() {
            var data = new Dictionary<string, object>() {
                {Moonwalk.KEY, KEY}
            };

            return Moonwalk.CreateLink(data);
        }

        [Serializable]
        private class ApiData {
            [JsonProperty("domain")] public int Domain;
            [JsonProperty("key")] public string Key;
        }
    }
}
