using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Hashing
{
    public class HashingHelper
    {
        // Bir password verilcek ve hash ile salt dışarıya iletecek bir sistem || verilen password hash oluşturulur.
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                // Alt taraftaki key o anki algoritmanın oluşturduğu key'dir.
                passwordSalt = hmac.Key;
                // Alt tarafta password  öncesindeki kod sayesinde byte çevriliyor.
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        // Sonradan sisteme girmek istenilen password kaynaktaki hash ile ilgili salt göre eşleşip eşleşmediği sorgulanır.
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) // Password hash doğrula görevi yapar
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            // return true; // burayada yazılabilir.
        }
    }
}
