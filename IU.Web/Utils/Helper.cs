using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

#pragma warning disable 1591
namespace KooCard.Web
{
    public class Helper
    {
        

        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
       
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static string GenerateResetPasswordToken(){
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            return token; 
        }

        public static bool CheckNeedResetPasswordToken(string token)
        {
            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (when < DateTime.UtcNow.AddHours(-24))
            {
                // too old
                return true;
            }
            return false;
        }


        public static string GenerateLoTo() {
            int check = 0;

            int[] lotto = new int[6];

            Random rand = new Random();

            for (int i = 0; i < lotto.Length; )
            {
                check = rand.Next(0, 9);

                if (!lotto.Contains(check))
                {
                    lotto[i] = check;
                    i++;
                }
            }
            return string.Join("", lotto);
        
        }

        public const string ALPHANUMERIC_CAPS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        public const string ALPHA_CAPS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string NUMERIC = "1234567890";
        public static string GetRandomString(int length, params char[] chars)
        {
            Random rand = new Random();
            string s = "";
            for (int i = 0; i < length; i++)
                s += chars[rand.Next() % chars.Length];

            return s;
        }

        public static string GenerateRandomId()
        {
            int maxSize = 36;
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - 1)]); }
            return result.ToString();
        }


    }
}