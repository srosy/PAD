using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PAD.Data
{
    public class CryptographyService : ICryptographyService
    {
        private IConfiguration _configuration;
        private RijndaelManaged _symmetricKey;
        private byte[] _keyBytes;
        private byte[] _initVector;

        public CryptographyService(IConfiguration configuration)
        {
            _configuration = configuration;
            _symmetricKey = new RijndaelManaged();
            _keyBytes = new PasswordDeriveBytes(_configuration.GetValue<string>("CRYPT_KEY"), Encoding.UTF8.GetBytes(_configuration.GetValue<string>("CRYPT_SALT"))).GetBytes(32);
            _initVector = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("CRYPT_VECTOR"));
        }

        /// <summary>
        /// Returns encrypted credentials(for Client API authorization) using passed email and password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>string</returns>
        public string Encrypt(string email, string password)
        {
            var toEncrypt = Encoding.UTF8.GetBytes(email + "|" + password);

            var encryptor = _symmetricKey.CreateEncryptor(_keyBytes, _initVector);
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(toEncrypt, 0, toEncrypt.Length);
            cryptoStream.FlushFinalBlock();
            var encryptedCredentials = memoryStream.ToArray();
            return Convert.ToHexString(encryptedCredentials); //convert to hex to avoid issues with api endpoint urls
        }

        /// <summary>
        /// Decrypts the passed string and returns email and password if they could be parsed from the decrypted result
        /// </summary>
        /// <param name="encrypedString"></param>
        /// <returns>tuple(string, string)</returns>
        public (string email, string hashedpass) Decrypt(string encryptedString)
        {
            if (string.IsNullOrEmpty(encryptedString)) return (email: null, hashedpass: null);
            
            var toDecrypt = Convert.FromHexString(encryptedString);
            var decryptor = _symmetricKey.CreateDecryptor(_keyBytes, _initVector);
            using var memoryStream = new MemoryStream(toDecrypt);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var decryptedString = new byte[toDecrypt.Length];
            var decryptedByteCount = cryptoStream.Read(decryptedString, 0, decryptedString.Length);

            try
            {
                var credentials = Encoding.UTF8.GetString(decryptedString, 0, decryptedByteCount).Split("|");
                return (email: credentials[0], hashedpass: credentials[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return (email: null, hashedpass: null);
        }
    }
}
