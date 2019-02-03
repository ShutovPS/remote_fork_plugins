using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetNewKeysCommand : ICommand {
        public const string KEY = "newkeys";

        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            if (UpdateMoonwalkKeys()) {
                return new List<Item>();
            }
            return null;
        }

        private static bool UpdateMoonwalkKeys() {
            bool result = false;
            string response = HTTPUtility.GetRequest(PluginSettings.Settings.Encryption.DomainUrl);
            PluginSettings.Settings.Encryption.DomainId = response;

            response = HTTPUtility.GetRequest(PluginSettings.Settings.Encryption.Url);

            try {
                var regex = new Regex(string.Format(PluginSettings.Settings.Regexp.Ini, "iv"));
                string iv = regex.Match(response).Groups[2].Value;
                if (!string.IsNullOrEmpty(iv)) {
                    PluginSettings.Settings.Encryption.IV = iv;
                    result = true;
                }
            } catch (Exception) {
            }

            try {
                var regex = new Regex(string.Format(PluginSettings.Settings.Regexp.Ini, "key"));
                string key = regex.Match(response).Groups[2].Value;
                if (!string.IsNullOrEmpty(key)) {
                    PluginSettings.Settings.Encryption.Key = key;
                    result = true;
                }
            } catch (Exception) {
            }

            if (result) {
                PluginSettings.Instance.Save();
            }

            return result;
        }
    }
}
