using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace IU.Services.Utilities
{
    public class PasswordHash
    {
        public static string HashPassword(string password){
            // Run the functions on the code, 
            string hashedPassword = Crypto.Hash(password, "MD5");
            return hashedPassword;
        }

        public static bool VerifyPassword(string hashedPassword, string password)
        {   
            // First parameter is the previously hashed string using a Salt
            return Crypto.VerifyHashedPassword(hashedPassword, password);
        }
    }
}
