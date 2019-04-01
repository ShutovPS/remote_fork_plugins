using Newtonsoft.Json;
using System;

namespace RemoteFork.Plugins.Settings
{
    public class Settings
    {
        public string login { get; set; }
        public string password { get; set; }

        public string Cookie { get; set; }
        public DateTime CookieExpires { get; set; } = DateTime.Today;



        [JsonProperty("SETTINGS_VERSION")]
        public float SettingsVersion { get; set; }

        public static Settings DefaultSettings { get; } = new Settings();
    }
}
