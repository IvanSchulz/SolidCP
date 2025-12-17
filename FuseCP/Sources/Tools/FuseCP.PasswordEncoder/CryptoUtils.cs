// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

namespace FuseCP.PasswordEncoder
{
    /// <summary>
    /// Summary description for CryptoUtils.
    /// </summary>
    public class CryptoUtils
    {
        public static string Encrypt(string InputText, string CryptoKey)
        {
            string Password = CryptoKey;

            if (InputText == null)
                return InputText;

            // We are now going to create an instance of the 
            // Rihndael class.
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            // First we need to turn the input strings into a byte array.
            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);


            // We are using salt to make it harder to guess our key
            // using a dictionary attack.
            byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());


            // The (Secret Key) will be generated from the specified 
            // password and salt.
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);


            // Create a encryptor from the existing SecretKey bytes.
            // We use 32 bytes for the secret key 
            // (the default Rijndael key length is 256 bit = 32 bytes) and
            // then 16 bytes for the IV (initialization vector),
            // (the default Rijndael IV length is 128 bit = 16 bytes)
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));


            // Create a MemoryStream that is going to hold the encrypted bytes 
            MemoryStream memoryStream = new MemoryStream();


            // Create a CryptoStream through which we are going to be processing our data. 
            // CryptoStreamMode.Write means that we are going to be writing data 
            // to the stream and the output will be written in the MemoryStream
            // we have provided. (always use write mode for encryption)
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

            // Start the encryption process.
            cryptoStream.Write(PlainText, 0, PlainText.Length);


            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memoryStream into a byte array.
            byte[] CipherBytes = memoryStream.ToArray();



            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();



            // Convert encrypted data into a base64-encoded string.
            // A common mistake would be to use an Encoding class for that. 
            // It does not work, because not all byte values can be
            // represented by characters. We are going to be using Base64 encoding
            // That is designed exactly for what we are trying to do. 
            string EncryptedData = Convert.ToBase64String(CipherBytes);



            // Return encrypted string.
            return EncryptedData;
        }


        public static string Decrypt(string InputText, string CryptoKey)
        {
            try
            {
                if (InputText == null || InputText == "")
                    return InputText;

                string Password = CryptoKey;
                RijndaelManaged RijndaelCipher = new RijndaelManaged();


                byte[] EncryptedData = Convert.FromBase64String(InputText);
                byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());


                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

                // Create a decryptor from the existing SecretKey bytes.
                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));


                MemoryStream memoryStream = new MemoryStream(EncryptedData);

                // Create a CryptoStream. (always use Read mode for decryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);


                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold EncryptedData;
                // DecryptedData is never longer than EncryptedData.
                byte[] PlainText = new byte[EncryptedData.Length];

                // Start decrypting.
                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);


                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string. 
                string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);


                // Return decrypted string.   
                return DecryptedData;
            }
            catch
            {
                return "";
            }
        }

        public static string SHA1(string plainText)
        {
            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            HashAlgorithm hash = new SHA1Managed(); ;

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);

            // Return the result.
            return Convert.ToBase64String(hashBytes);
        }

		public static string SHA256(string plainText)
		{
			// Convert plain text into a byte array.
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

			HashAlgorithm hash = new SHA256Managed(); ;

			// Compute hash value of our plain text with appended salt.
			byte[] hashBytes = hash.ComputeHash(plainTextBytes);

			// Return the result.
			return "SHA256:" + Convert.ToBase64String(hashBytes);
		}

	}
}
