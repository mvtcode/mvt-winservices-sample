using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Sercurity
{
    public class Encrypt
    {
        public static string MD5User(string s)
        {
            string str1 = MD5(s);
            str1 = str1 + s;
            string str2 = MD5(str1);

            return str2;
        }

        public static string MD5Admin(string s)
        {
            string str1 = MD5(s);
            str1 = str1 + s;
            string str2 = MD5(str1);

            return str2;
        }

        private static string MD5(string s)
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(s);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes).ToLower().Replace("-", "");
        }

        //public static string EncryptConn(string str)
        //{
        //    Cryptography.RijndaelEnhanced rijndaelKey = new Cryptography.RijndaelEnhanced("#$%^&", "@#3$%^&*()12_@");
        //    return rijndaelKey.Encrypt(str);
        //}

        public static string DecryptConn(string str)
        {
            Cryptography.RijndaelEnhanced rijndaelKey = new Cryptography.RijndaelEnhanced("(*)adsas%^&efg", "@1B2c3&D4e5F6g7H8");
            return rijndaelKey.Decrypt(str);
        }

        ////////////// mã hóa AES
        
        //// Encrypt a byte array into a byte array using a key and an IV 
        //private static byte[] EncryptAES(byte[] clearData, byte[] Key, byte[] IV)
        //{

        //    // Create a MemoryStream that is going to accept the encrypted bytes 
        //    MemoryStream ms = new MemoryStream();

        //    Rijndael alg = Rijndael.Create();
        //    alg.Key = Key;

        //    alg.IV = IV;
        //    CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);

        //    cs.Write(clearData, 0, clearData.Length);
        //    cs.Close();
        //    byte[] encryptedData = ms.ToArray();
        //    return encryptedData;
        //}

        ///// <summary>
        ///// Returns an encrypted string using Rijndael (128,192,256 Bits).
        ///// </summary>
        //public static string EncryptAES(string Data, string Password, int Bits)
        //{

        //    byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(Data);


        //    PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,


        //        new byte[] { 0x00, 0x01, 0x02, 0x1C, 0x1D, 0x1E, 0x03, 0x04, 0x05, 0x0F, 0x20, 0x21, 0xAD, 0xAF, 0xA4 });


        //    if (Bits == 128)
        //    {
        //        byte[] encryptedData = EncryptAES(clearBytes, pdb.GetBytes(16), pdb.GetBytes(16));
        //        return Convert.ToBase64String(encryptedData);
        //    }
        //    else if (Bits == 192)
        //    {
        //        byte[] encryptedData = EncryptAES(clearBytes, pdb.GetBytes(24), pdb.GetBytes(16));
        //        return Convert.ToBase64String(encryptedData);
        //    }
        //    else if (Bits == 256)
        //    {
        //        byte[] encryptedData = EncryptAES(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
        //        return Convert.ToBase64String(encryptedData);
        //    }
        //    else
        //    {
        //        return string.Concat(Bits);
        //    }
        //}

        //// Decrypt a byte array into a byte array using a key and an IV 
        //private static byte[] DecryptAES(byte[] cipherData, byte[] Key, byte[] IV)
        //{

        //    MemoryStream ms = new MemoryStream();
        //    Rijndael alg = Rijndael.Create();
        //    alg.Key = Key;
        //    alg.IV = IV;
        //    CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
        //    cs.Write(cipherData, 0, cipherData.Length);
        //    cs.Close();
        //    byte[] decryptedData = ms.ToArray();
        //    return decryptedData;
        //}


        ///// <summary>
        ///// Returns a decrypted string.
        ///// </summary>
        //// Decrypt a string into a string using a password 
        //public static string DecryptAES(string Data, string Password, int Bits)
        //{

        //    byte[] cipherBytes = Convert.FromBase64String(Data);

        //    PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,

        //        new byte[] { 0x00, 0x01, 0x02, 0x1C, 0x1D, 0x1E, 0x03, 0x04, 0x05, 0x0F, 0x20, 0x21, 0xAD, 0xAF, 0xA4 });

        //    if (Bits == 128)
        //    {
        //        byte[] decryptedData = DecryptAES(cipherBytes, pdb.GetBytes(16), pdb.GetBytes(16));
        //        return System.Text.Encoding.Unicode.GetString(decryptedData);
        //    }
        //    else if (Bits == 192)
        //    {
        //        byte[] decryptedData = DecryptAES(cipherBytes, pdb.GetBytes(24), pdb.GetBytes(16));
        //        return System.Text.Encoding.Unicode.GetString(decryptedData);
        //    }
        //    else if (Bits == 256)
        //    {
        //        byte[] decryptedData = DecryptAES(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
        //        return System.Text.Encoding.Unicode.GetString(decryptedData);
        //    }
        //    else
        //    {
        //        return string.Concat(Bits);
        //    }
        //}
    }
}
