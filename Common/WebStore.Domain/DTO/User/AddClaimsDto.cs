using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.DTO.User
{
    public abstract class IdentityModelDto
    {
        public Entities.User User { get; set; }
    }
    
    public class AddClaimsDto : IdentityModelDto
    {
        public IEnumerable<Claim> Claims { get; set; }
    }
    
    public class RemoveClaimDto : IdentityModelDto
    {
        public IEnumerable<Claim> Claims { get; set; }
    }

    public class ReplaceClaimsDto : IdentityModelDto
    {
        public Claim OldClaim { get; set; }
        public Claim NewClaim { get; set; }
    }
    
    public class AddLoginDto : IdentityModelDto
    {
        public UserLoginInfo UserLoginInfo { get; set; }
    }

    public class PasswordHashDto : IdentityModelDto
    {
        public string Hash { get; set; }
    }

    public class SetLockoutDto : IdentityModelDto
    {
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}