using Newtonsoft.Json;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace PruebaTecnica.Controllers.Utility
{
    public static class UtilityPT
    {
        private static readonly string keyString = "E546C8DF278CD5931069B522E695D4F2";
        public static readonly string UserId = "A100B";
        public static readonly string UserConf = "A200C";
        private static byte[] IV =
                                    {
                                        0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                                        0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
                                    };

        public static async Task<string> EncryptDataAsync(string plainText)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword();
            aes.IV = IV;
            
            using MemoryStream output = new();
            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(plainText));
            await cryptoStream.FlushFinalBlockAsync();

            return Convert.ToBase64String(output.ToArray());
        }

        public static async Task<string> DecryptAsync(string plainText)
        {
            byte[] encrypted = Convert.FromBase64String(plainText);
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword();
            aes.IV = IV;
            
            using MemoryStream input = new(encrypted);
            using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();
            
            await cryptoStream.CopyToAsync(output);
            return Encoding.Unicode.GetString(output.ToArray());
        }

        private static byte[] DeriveKeyFromPassword()
        {
            var emptySalt = Array.Empty<byte>();
            var iterations = 1000;
            var desiredKeyLength = 16; // 16 bytes equal 128 bits.
            var hashMethod = HashAlgorithmName.SHA384;
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(keyString),
                                             emptySalt,
                                             iterations,
                                             hashMethod,
                                             desiredKeyLength);
        }

        public static DataTable? ConvertToDataTable(dynamic objJson)
        {
            return JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(objJson));
        }
    }
}
