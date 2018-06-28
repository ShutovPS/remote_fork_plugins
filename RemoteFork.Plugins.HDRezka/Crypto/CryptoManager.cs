using System;
using System.Security.Cryptography;
using System.Text;

namespace RemoteFork.Plugins {
    public static class CryptoManager {
        public static string Encrypt(string text, string key, string iv) {
            var data = new UTF8Encoding().GetBytes(text);

            var aes = Aes.Create();
            if (aes != null) {
                var encryptor = aes.CreateEncryptor(HexStringToByteArray(key), HexStringToByteArray(iv));
                var encrData = encryptor.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(encrData);
            }

            return string.Empty;
        }

        public static string Decrypt(string text, string key, string iv) {
            var encrData = Convert.FromBase64String(text);
            
            var aes = Aes.Create();
            if (aes != null) {
                var encryptor = aes.CreateDecryptor(HexStringToByteArray(key), HexStringToByteArray(iv));
                var data = encryptor.TransformFinalBlock(encrData, 0, encrData.Length);
                return Encoding.UTF8.GetString(data);
            }

            return string.Empty;
        }

        public static byte[] HexStringToByteArray(string strHex) {
            dynamic r = new byte[strHex.Length / 2];
            for (int i = 0; i <= strHex.Length - 1; i += 2) {
                r[i / 2] = Convert.ToByte(Convert.ToInt32(strHex.Substring(i, 2), 16));
            }

            return r;
        }
    }
}
