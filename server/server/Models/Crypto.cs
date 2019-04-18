using System;
using System.IO;
using System.Security.Cryptography;

namespace server.Models
{
    public class Crypto
    {
        private static Aes myAes;
        public byte[] key;
        public byte[] iv;
        public Crypto()
        {
            // Create a new instance of the Aes
            // class.  This generates a new key and initialization 
            // vector (IV).
            myAes = Aes.Create();
            myAes.Padding = PaddingMode.PKCS7;
            key = myAes.Key;
            iv = myAes.IV;

        }

        public (byte[], byte[]) GetInfo()
        {
            return (myAes.Key, myAes.IV);
        }

        public string EncryptString(string plainText, byte[] key, byte[] iv)
        {

            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            byte[] encrypted;
            

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = myAes.CreateEncryptor(key, iv);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            // Convert the encrypted bytes from the memory stream.
            // Return the encrypted string
            return Convert.ToBase64String(encrypted);

        }

        public string DecryptString(string cipherText, byte[] k, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                return "";

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;


            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = myAes.CreateDecryptor(myAes.Key, myAes.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {

                    try
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            
                            plaintext = srDecrypt.ReadToEnd();

                        }
                    } catch (CryptographicException e)
                    {
                        // The ciphertext was not encrypted using the current key and iv
                        Console.WriteLine(e);
                        return "-1";
                    }
                    
                }
            }

            return plaintext;

        }
    }
}
