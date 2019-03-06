using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.DTO.User
{
    public abstract class IdentityModelDTO
    {
        public Entities.User User { get; set; }
    }

    public class AddClaimsDTO : IdentityModelDTO
    {
        public IEnumerable<Claim> Claims { get; set; }
    }

    public class RemoveClaimsDTO : IdentityModelDTO
    {
        public IEnumerable<Claim> Claims { get; set; }
    }

    public class ReplaceClaimDTO : IdentityModelDTO
    {
        public Claim OldClaim { get; set; }
        public Claim NewClaim { get; set; }
    }

    public class AddLoginDTO : IdentityModelDTO
    {
         public UserLoginInfo UserLoginInfo { get; set; }
    }

    public class PasswordHashDTO : IdentityModelDTO
    {
        public string Hash { get; set; }
    }

    public class SetLockoutDTO : IdentityModelDTO
    {
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
