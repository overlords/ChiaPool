using ChiaPool.Models;

namespace ChiaPool.Extensions
{
    public static class UserExtensions
    {
        public static User WithoutSecrets(this User user)
        {
            user.PasswordHash = default;
            return user;
        }
    }
}
