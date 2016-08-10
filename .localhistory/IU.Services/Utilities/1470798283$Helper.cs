using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Web;

#pragma warning disable 1591
namespace IU.Services
{
    public class Helper
    {

        public DateTime[] GetDays(DateTime start, DateTime end)
        {
            var daysToChoose = new DayOfWeek[] { DayOfWeek.Thursday, DayOfWeek.Tuesday };

            var dates = Enumerable.Range(0, (int)(end - start).TotalDays + 1)
                                    .Select(d => start.AddDays(d))
                                    .Where(d => daysToChoose.Contains(d.DayOfWeek));
            return dates;
        }

        public static DateTime[] GetTwoDaysBefore()
        {
            var dt = DateTime.Now;
            var twoDaysBefore = dt.AddDays(-2);
            List<DateTime> ls = new List<DateTime>();
            ls.Add(dt.AddDays(-1));
            ls.Add(dt.AddDays(-2));
            return ls.ToArray();
        }

        public static DateTime GetTwoDaysNext()
        {
            var dt = DateTime.Now;
            var twoDaysNext = dt.AddDays(2);
            return twoDaysNext;
        }

        /// <summary>
        /// Pages the specified query.
        /// </summary>
        /// <typeparam name="T">Generic Type Object</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The Object query where paging needs to be applied.</param>
        /// <param name="pageNum">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderByProperty">The order by property.</param>
        /// <param name="isAscendingOrder">if set to <c>true</c> [is ascending order].</param>
        /// <param name="rowsCount">The total rows count.</param>
        /// <returns></returns>
        public static IQueryable<T> PagedResult<T, TResult>(IQueryable<T> query, int pageNum, int pageSize,
                        Expression<Func<T, TResult>> orderByProperty, bool isAscendingOrder, out int rowsCount)
        {
            if (pageSize <= 0) pageSize = 20;

            //Total result count
            rowsCount = query.Count();

            //If page number should be > 0 else set to first page
            if (rowsCount <= pageSize || pageNum <= 0) pageNum = 1;

            //Calculate nunber of rows to skip on pagesize
            int excludedRows = (pageNum - 1) * pageSize;

            query = isAscendingOrder ? query.OrderBy(orderByProperty) : query.OrderByDescending(orderByProperty);

            //Skip the required rows for the current page and take the next records of pagesize count
            return query.Skip(excludedRows).Take(pageSize);
        } 

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
            return Guid.NewGuid().ToString();
            //int maxSize = 36;
            //char[] chars = new char[62];
            //string a;
            //a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            //chars = a.ToCharArray();
            //int size = maxSize;
            //byte[] data = new byte[1];
            //RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            //crypto.GetNonZeroBytes(data);
            //size = maxSize;
            //data = new byte[size];
            //crypto.GetNonZeroBytes(data);
            //StringBuilder result = new StringBuilder(size);
            //foreach (byte b in data)
            //{ result.Append(chars[b % (chars.Length - 1)]); }
            //return result.ToString();
        }


    }
}