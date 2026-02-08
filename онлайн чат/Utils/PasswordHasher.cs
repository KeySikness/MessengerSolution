using System.Security.Cryptography;
using System.Text;

namespace ChatClient.Utils
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return System.Convert.ToBase64String(bytes);
            }
        }
    }
}
