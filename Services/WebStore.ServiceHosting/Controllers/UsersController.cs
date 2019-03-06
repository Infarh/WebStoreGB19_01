using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.User;
using WebStore.Domain.Entities;

namespace WebStore.ServiceHosting.Controllers
{
    [ApiController, Route("api/[controller]"), Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly UserStore<User> _UserStore;

        public UsersController(WebStoreContext db)
        {
            _UserStore = new UserStore<User>(db) {AutoSaveChanges = true};
        }

        [HttpPost("UserId")]
        public async Task<string> GetUserIdAsync([FromBody] User user)
        {
            return await _UserStore.GetUserIdAsync(user);
        }

        [HttpPost("UserName")]
        public async Task<string> GetUserNameAsync([FromBody] User user)
        {
            return await _UserStore.GetUserNameAsync(user);
        }

        [HttpPost("UserName/{name}")]
        public async Task SetUserNameAsync([FromBody] User user, string name)
        {
            await _UserStore.SetUserNameAsync(user, name);
        }

        [HttpPost("NormalUserName")]
        public async Task<string> GetNormalizedUserNameAsync([FromBody] User user)
        {
            return await _UserStore.GetNormalizedUserNameAsync(user);
        }

        [HttpPost("NormalUserName/{name}")]
        public Task SetNormalizedUserNameAsync([FromBody] User user, string name)
        {
            return _UserStore.SetNormalizedUserNameAsync(user, name);
        }

        [HttpPost("User")]
        public async Task<bool> CreateAsync([FromBody] User user)
        {
            return (await _UserStore.CreateAsync(user)).Succeeded;
        }

        [HttpPut("User")]
        public async Task<bool> UpdateAsync([FromBody] User user)
        {
            return (await _UserStore.UpdateAsync(user)).Succeeded;
        }

        [HttpPost("User/Delete")]
        public async Task<bool> DeleteAsync([FromBody] User user)
        {
            return (await _UserStore.DeleteAsync(user)).Succeeded;
        }

        [HttpGet("User/Find/{id}")]
        public async Task<User> FindByIdAsync(string id)
        {
            return await _UserStore.FindByIdAsync(id);
        }

        [HttpGet("User/Normal/{name}")]
        public async Task<User> FindByNameAsync(string name)
        {
            return await _UserStore.FindByNameAsync(name);
        }

        [HttpPost("Role/{role}")]
        public async Task AddToRoleAsync([FromBody] User user, string role)
        {
            await _UserStore.AddToRoleAsync(user, role);
        }

        [HttpPost("Role/Delete/{role}")]
        public async Task RemoveFromRoleAsync([FromBody] User user, string role)
        {
            await _UserStore.RemoveFromRoleAsync(user, role);
        }

        [HttpPost("Roles")]
        public async Task<IList<string>> GetRolesAsync([FromBody] User user)
        {
            return await _UserStore.GetRolesAsync(user);
        }

        [HttpPost("InRole/{role}")]
        public async Task<bool> IsInRoleAsync([FromBody] User user, string role)
        {
            return await _UserStore.IsInRoleAsync(user, role);
        }

        [HttpGet("UsersInRole/{role}")]
        public async Task<IList<User>> GetUsersInRoleAsync(string role)
        {
            return await _UserStore.GetUsersInRoleAsync(role);
        }

        [HttpPost("GetPasswordHash")]
        public async Task<string> GetPasswordHashAsync([FromBody] User user)
        {
            return await _UserStore.GetPasswordHashAsync(user);
        }

        [HttpPost("SetPasswordHash")]
        public async Task<string> SetPasswordHashAsync([FromBody] PasswordHashDTO hash)
        {
            await _UserStore.SetPasswordHashAsync(hash.User, hash.Hash);
            return hash.User.PasswordHash;
        }

        [HttpPost("HasPassword")]
        public async Task<bool> HasPasswordAsync([FromBody] User user)
        {
            return await _UserStore.HasPasswordAsync(user);
        }

        [HttpPost("GetClaims")]
        public async Task<IList<Claim>> GetClaimsAsync([FromBody] User user)
        {
            return await _UserStore.GetClaimsAsync(user);
        }

        [HttpPost("AddClaims")]
        public async Task AddClaimsAsync([FromBody] AddClaimsDTO ClaimInfo)
        {
            await _UserStore.AddClaimsAsync(ClaimInfo.User, ClaimInfo.Claims);
        }

        [HttpPost("ReplaceClaim")]
        public async Task ReplaceClaimAsync([FromBody] ReplaceClaimDTO ClaimInfo)
        {
            await _UserStore.ReplaceClaimAsync(ClaimInfo.User, ClaimInfo.OldClaim, ClaimInfo.NewClaim);
        }

        [HttpPost("RemoveClaim")]
        public async Task RemoveClaimsAsync([FromBody] RemoveClaimsDTO ClaimInfo)
        {
            await _UserStore.RemoveClaimsAsync(ClaimInfo.User, ClaimInfo.Claims);
        }

        [HttpPost("GetUsersForClaim")]
        public async Task<IList<User>> GetUsersForClaimAsync([FromBody] Claim claim)
        {
            return await _UserStore.GetUsersForClaimAsync(claim);
        }

        [HttpPost("GetTwoFactorEnabled")]
        public async Task<bool> GetTwoFactorEnabledAsync([FromBody] User user)
        {
            return await _UserStore.GetTwoFactorEnabledAsync(user);
        }

        [HttpPost("SetTwoFactor/{enable}")]
        public async Task SetTwoFactorEnabledAsync([FromBody] User user, bool enable)
        {
            await _UserStore.SetTwoFactorEnabledAsync(user, enable);
        }

        [HttpPost("GetEmail")]
        public async Task<string> GetEmailAsync([FromBody] User user)
        {
            return await _UserStore.GetEmailAsync(user);
        }

        [HttpPost("SetEmail/{email}")]
        public async Task SetEmailAsync([FromBody] User user, string email)
        {
            await _UserStore.SetEmailAsync(user, email);
        }

        [HttpPost("GetEmailConfirmed")]
        public async Task<bool> GetEmailConfirmedAsync([FromBody] User user)
        {
            return await _UserStore.GetEmailConfirmedAsync(user);
        }

        [HttpPost("SetEmailConfirmed/{enable}")]
        public async Task SetEmailConfirmedAsync([FromBody] User user, bool enable)
        {
            await _UserStore.SetEmailConfirmedAsync(user, enable);
        }

        [HttpGet("UserFindByEmail/{email}")]
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _UserStore.FindByEmailAsync(email);
        }

        [HttpPost("GetNormalizedEmail")]
        public async Task<string> GetNormalizedEmailAsync([FromBody] User user)
        {
            return await _UserStore.GetNormalizedEmailAsync(user);
        }

        [HttpPost("SetEmail/{email}")]
        public async Task SetNormalizedEmailAsync([FromBody] User user, string email)
        {
            await _UserStore.SetNormalizedEmailAsync(user, email);
        }

        [HttpPost("GetPhoneNumber")]
        public async Task<string> GetPhoneNumberAsync([FromBody] User user)
        {
            return await _UserStore.GetPhoneNumberAsync(user);
        }

        [HttpPost("SetPhoneNumber/{phone}")]
        public async Task SetPhoneNumberAsync([FromBody] User user, string phone)
        {
            await _UserStore.SetPhoneNumberAsync(user, phone);
        }

        [HttpPost("GetPhoneNumberConfirmed")]
        public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody] User user)
        {
            return await _UserStore.GetPhoneNumberConfirmedAsync(user);
        }

        [HttpPost("SetPhoneNumberConfirmed/{confirmed}")]
        public async Task SetPhoneNumberConfirmedAsync([FromBody] User user, bool confirmed)
        {
            await _UserStore.SetPhoneNumberConfirmedAsync(user, confirmed);
        }

        [HttpPost("AddLogin")]
        public async Task AddLoginAsync([FromBody] AddLoginDTO login)
        {
            await _UserStore.AddLoginAsync(login.User, login.UserLoginInfo);
        }

        [HttpPost("RemoveLogin/{LoginProvider}/{ProviderKey}")]
        public async Task RemoveLoginAsync([FromBody] User user, string LoginProvider, string ProviderKey)
        {
            await _UserStore.RemoveLoginAsync(user, LoginProvider, ProviderKey);
        }

        [HttpPost("GetLogins")]
        public async Task<IList<UserLoginInfo>> GetLoginsAsync([FromBody] User user)
        {
            return await _UserStore.GetLoginsAsync(user);
        }

        [HttpGet("User/FindByLogin/{LoginProvider}/{ProviderKey}")]
        public async Task<User> FindByLoginAsync(string LoginProvider, string ProviderKey)
        {
            return await _UserStore.FindByLoginAsync(LoginProvider, ProviderKey);
        }

        [HttpPost("GetLockoutEndDate")]
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync([FromBody] User user)
        {
            return await _UserStore.GetLockoutEndDateAsync(user);
        }

        [HttpPost("SetLockoutEndDate")]
        public async Task SetLockoutEndDateAsync([FromBody] SetLockoutDTO LocoutInfo)
        {
            await _UserStore.SetLockoutEndDateAsync(LocoutInfo.User, LocoutInfo.LockoutEnd);
        }

        [HttpPost("IncrementAccessFailedCount")]
        public async Task<int> IncrementAccessFailedCountAsync([FromBody] User user)
        {
            return await _UserStore.IncrementAccessFailedCountAsync(user);
        }

        [HttpPost("ResetAccessFailedCount")]
        public async Task ResetAccessFailedCountAsync([FromBody] User user)
        {
            await _UserStore.ResetAccessFailedCountAsync(user);
        }

        [HttpPost("GetAccessFailedCount")]
        public async Task<int> GetAccessFailedCountAsync([FromBody] User user)
        {
            return await _UserStore.GetAccessFailedCountAsync(user);
        }

        [HttpPost("GetLockoutEnabled")]
        public async Task<bool> GetLockoutEnabledAsync([FromBody] User user)
        {
            return await _UserStore.GetLockoutEnabledAsync(user);
        }

        [HttpPost("SetLockoutEnabled/{enable}")]
        public async Task SetLockoutEnabledAsync([FromBody] User user, bool enable)
        {
            await _UserStore.SetLockoutEnabledAsync(user, enable);
        }
    }
}