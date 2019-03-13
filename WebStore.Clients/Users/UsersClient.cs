using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain.DTO.User;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Users
{
    public class UsersClient : BaseClient, IUsersClient
    {
        public UsersClient(IConfiguration configuration) : base(configuration)
        {
            ServiceAddress = "api/users";
        }

        #region Implimentation of IUserStore

        public async Task<string> GetUserIdAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{ServiceAddress}/userId", user, cancel);
            return await response.Content
                .ReadAsAsync<string>(cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetUserNameAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/userName", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetUserNameAsync(Domain.Entities.User user, string name, CancellationToken cancel)
        {
            user.UserName = name;
            await PostAsync($"{ServiceAddress}/userName/{name}", user, cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedUserNameAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/normalUserName", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetNormalizedUserNameAsync(Domain.Entities.User user, string name, CancellationToken cancel)
        {
            user.NormalizedUserName = name;
            await PostAsync($"{ServiceAddress}/normalUserName/{name}", user, cancel)
                .ConfigureAwait(false);
        }

        public async Task<IdentityResult> CreateAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/user", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel) ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(Domain.Entities.User user,
            CancellationToken cancel)
        {
            return await (await PutAsync($"{ServiceAddress}/user", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel) ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(Domain.Entities.User user,
            CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/user/delete", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel) ? IdentityResult.Success : IdentityResult.Failed();
        }

        public Task<Domain.Entities.User> FindByIdAsync(string userId, CancellationToken cancel)
        {
            return GetAsync<Domain.Entities.User>($"{ServiceAddress}/user/find/{userId}", cancel);
        }

        public async Task<Domain.Entities.User> FindByNameAsync(string normalizedUserName, CancellationToken cancel)
        {
            return await GetAsync<Domain.Entities.User>($"{ServiceAddress}/user/normal/{normalizedUserName}", cancel);
        }

        #endregion

        #region IUserRoleStore

        public Task AddToRoleAsync(Domain.Entities.User user, string role, CancellationToken cancel)
        {
            return PostAsync($"{ServiceAddress}/role/{role}", user, cancel);
        }

        public Task RemoveFromRoleAsync(Domain.Entities.User user, string role, CancellationToken cancel)
        {
            return PostAsync($"{ServiceAddress}/role/delete/{role}", user, cancel);
        }

        public async Task<IList<string>> GetRolesAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/roles", user, cancel))
                .Content
                .ReadAsAsync<IList<string>>(cancel);
        }

        public async Task<bool> IsInRoleAsync(Domain.Entities.User user, string name, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/inRole/{name}", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel);
        }

        public async Task<IList<Domain.Entities.User>> GetUsersInRoleAsync(string name, CancellationToken cancel)
        {
            return (await GetAsync<List<Domain.Entities.User>>($"{ServiceAddress}/usersInRole/{name}", cancel)).ToList();
        }

        #endregion

        #region IUserPasswordStore

        public async Task SetPasswordHashAsync(Domain.Entities.User user, string hash, CancellationToken cancel)
        {
            user.PasswordHash = hash;
            await PostAsync($"{ServiceAddress}/setPasswordHash", new PasswordHashDto()
            {
                User = user, 
                Hash = hash
            }, cancel);
        }

        public async Task<string> GetPasswordHashAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/getPasswordHash", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel);
        }

        public async Task<bool> HasPasswordAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/hasPassword", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel);
        }

        #endregion

        #region IUserClaimStore

        public async Task<IList<Claim>> GetClaimsAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/getClaims", user, cancel))
                .Content
                .ReadAsAsync<List<Claim>>(cancel);
        }

        public Task AddClaimsAsync(Domain.Entities.User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            return PostAsync($"{ServiceAddress}/addClaims", new AddClaimsDto()
            {
                User = user, 
                Claims = claims
            }, cancel);
        }

        public Task ReplaceClaimAsync(Domain.Entities.User user, Claim oldClaim, Claim newClaim, CancellationToken cancel)
        {
            return PostAsync($"{ServiceAddress}/replaceClaim", new ReplaceClaimsDto()
            {
                User = user, 
                OldClaim = oldClaim,
                NewClaim = newClaim
            }, cancel);
        }

        public Task RemoveClaimsAsync(Domain.Entities.User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            return PostAsync($"{ServiceAddress}/removeClaims", new RemoveClaimDto()
            {
                User = user, 
                Claims = claims
            }, cancel);
        }

        public async Task<IList<Domain.Entities.User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/getUsersForClaim", claim, cancel))
                .Content
                .ReadAsAsync<List<Domain.Entities.User>>(cancel);
        }

        #endregion

        #region IUserTwoFactorStore

        public Task SetTwoFactorEnabledAsync(Domain.Entities.User user, bool enabled, CancellationToken cancel)
        {
            user.TwoFactorEnabled = enabled;
            return PostAsync($"{ServiceAddress}/setTwoFactor/{enabled}", user, cancel);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            var url = $"{ServiceAddress}/getTwoFactorEnabled";
            var result = await PostAsync(url, user, cancel);
            return await result.Content.ReadAsAsync<bool>(cancel);
        }

        #endregion

        #region IUserEmailStore

        public Task SetEmailAsync(Domain.Entities.User user, string email, CancellationToken cancel)
        {
            user.Email = email;
            return PostAsync($"{ServiceAddress}/setEmail/{email}", user, cancel);
        }

        public async Task<string> GetEmailAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/getEmail", user, cancel))
                .Content.
                ReadAsAsync<string>(cancel);
        }

        public async Task<bool> GetEmailConfirmedAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/getEmailConfirmed", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel);
        }

        public Task SetEmailConfirmedAsync(Domain.Entities.User user, bool confirmed, CancellationToken cancel)
        {
            user.EmailConfirmed = confirmed;
            return PostAsync($"{ServiceAddress}/setEmailConfirmed/{confirmed}", user, cancel);
        }

        public Task<Domain.Entities.User> FindByEmailAsync(string email, CancellationToken cancel)
        {
            return GetAsync<Domain.Entities.User>($"{ServiceAddress}/user/findByEmail/{email}", cancel);
        }

        public async Task<string> GetNormalizedEmailAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/getNormalizedEmail", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel);
        }
        
        public Task SetNormalizedEmailAsync(Domain.Entities.User user, string email, CancellationToken cancel)
        {
            user.NormalizedEmail = email;
            return PostAsync($"{ServiceAddress}/setEmail/{email}", user, cancel);
        }

        #endregion

        #region IUserPhoneNumberStore

        public Task SetPhoneNumberAsync(Domain.Entities.User user, string phone, CancellationToken cancel)
        {
            user.PhoneNumber = phone;
            return PostAsync($"{ServiceAddress}/setPhoneNumber/{phone}", user, cancel);
        }

        public async Task<string> GetPhoneNumberAsync(Domain.Entities.User user,
            CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/getPhoneNumber", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/getPhoneNumberConfirmed", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel);
        }

        public Task SetPhoneNumberConfirmedAsync(Domain.Entities.User user, bool confirmed, CancellationToken cancel)
        {
            user.PhoneNumberConfirmed = confirmed;
            return PostAsync($"{ServiceAddress}/setPhoneNumberConfirmed/{confirmed}", user, cancel);
        }

        #endregion

        #region IUserLoginStore

        public Task AddLoginAsync(Domain.Entities.User user, UserLoginInfo login, CancellationToken cancel)
        {
            var url = $"{ServiceAddress}/addLogin";
            return PostAsync(url, new AddLoginDto()
            {
                User = user, 
                UserLoginInfo = login
            }, cancel);
        }

        public Task RemoveLoginAsync(Domain.Entities.User user, string loginProvider, string providerKey, CancellationToken cancel)
        {
            return PostAsync($"{ServiceAddress}/removeLogin/{loginProvider}/{providerKey}", user, cancel);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/getLogins", user, cancel))
                .Content
                .ReadAsAsync<List<UserLoginInfo>>(cancel);
        }

        public Task<Domain.Entities.User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancel)
        {
            var url = $"{ServiceAddress}/user/FindByLogin/{loginProvider}/{providerKey}";
            return GetAsync<Domain.Entities.User>(url, cancel);
        }

        #endregion

        #region IUserLockoutStore

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/getLockoutEndDate", user, cancel)).Content.ReadAsAsync<DateTimeOffset?>(cancel);
        }

        public Task SetLockoutEndDateAsync(Domain.Entities.User user, DateTimeOffset? lockoutEnd, CancellationToken cancel)
        {
            user.LockoutEnd = lockoutEnd;
            return PostAsync($"{ServiceAddress}/setLockoutEndDate", new SetLockoutDto()
            {
                User = user, 
                LockoutEnd = lockoutEnd
            }, cancel);
        }

        public async Task<int> IncrementAccessFailedCountAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/IncrementAccessFailedCount", user, cancel))
                .Content
                .ReadAsAsync<int>(cancel);
        }

        public Task ResetAccessFailedCountAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return PostAsync($"{ServiceAddress}/ResetAccessFailedCount", user, cancel);
        }

        public async Task<int> GetAccessFailedCountAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetAccessFailedCount", user, cancel))
                .Content
                .ReadAsAsync<int>(cancel);
        }

        public async Task<bool> GetLockoutEnabledAsync(Domain.Entities.User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetLockoutEnabled", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel);
        }

        public async Task SetLockoutEnabledAsync(Domain.Entities.User user, bool enabled, CancellationToken cancel)
        {
            user.LockoutEnabled = enabled;
            await PostAsync($"{ServiceAddress}/SetLockoutEnabled/{enabled}", user, cancel);
        }

        #endregion
    }
}