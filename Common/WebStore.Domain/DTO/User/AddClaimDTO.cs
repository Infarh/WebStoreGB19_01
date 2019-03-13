using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.DTO.User
{
    public abstract class IdentityModelDto
    {
        public Entities.User User { get; }
    }
    
    public class AddClaimDto : IdentityModelDto
    {
        public IEnumerable<Claim> Claims { get; set; }
    }
    
    public class RemoveClaimDto : IdentityModelDto
    {
        public IEnumerable<Claim> Claims { get; set; }
    }

    public class ReplaceClaimDto : IdentityModelDto
    {
        public Claim OldClaims { get; set; }
        public Claim NewClaims { get; set; }
    }
    
    public class AddLoginDto : IdentityModelDto
    {
        public UserLoginInfo UserLoginInfo { get; set; }
    }

    public class PasswordHashDto
    {
        public string Hash { get; set; }
    }

    public class SetLockoutDto : IdentityModelDto
    {
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}