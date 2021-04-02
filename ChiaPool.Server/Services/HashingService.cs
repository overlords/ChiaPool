using Common.Services;
using System.Security.Cryptography;
using System.Text;

namespace ChiaPool.Services
{
    public class HashingService : Service
    {
        private readonly SHA512 HashAlgorithm;

        public HashingService()
        {
            HashAlgorithm = new SHA512Managed();
        }

        public string HashString(string plain)
        {
            var bytes = Encoding.UTF8.GetBytes(plain);
            var hashedBytes = HashAlgorithm.ComputeHash(bytes);
            return Encoding.UTF8.GetString(hashedBytes);
        }
    }
}
