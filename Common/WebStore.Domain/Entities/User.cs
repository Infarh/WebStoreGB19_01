using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Entities
{
    public class User : IdentityUser
    {
        public const string UserRole = "User";
        public const string AdminRole = "Admin";
        public const string AdminUser = "Admin";
        public const string TestUser = "TestUser";
    }
}
