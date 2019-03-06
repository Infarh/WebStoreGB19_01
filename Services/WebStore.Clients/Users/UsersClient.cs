using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain.DTO.User;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Users
{
    public class UsersClient : BaseClient, IUsersClient
    {
        public UsersClient(IConfiguration configuration) : base(configuration) => 
            ServiceAddress = "api/users";

        #region Implementation of IUserStore<User>

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{ServiceAddress}/UserId", user, cancel);
            return await response.Content
                .ReadAsAsync<string>(cancel)
                .ConfigureAwait(false);
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/UserName", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetUserNameAsync(User user, string name, CancellationToken cancel)
        {
            user.UserName = name;
            await PostAsync($"{ServiceAddress}/UserName/{name}", user, cancel);
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/NormalUserName/", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetNormalizedUserNameAsync(User user, string name, CancellationToken cancel)
        {
            user.NormalizedUserName = name;
            await PostAsync($"{ServiceAddress}/NormalUserName/{name}", user, cancel)
                .ConfigureAwait(false);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/User", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancel)
        {
            return await (await PutAsync($"{ServiceAddress}/User", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/User/Delete", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<User> FindByIdAsync(string id, CancellationToken cancel)
        {
            return await GetAsync<User>($"{ServiceAddress}/User/Find/{id}", cancel);
        }

        public async Task<User> FindByNameAsync(string name, CancellationToken cancel)
        {
            return await GetAsync<User>($"{ServiceAddress}/User/Normal/{name}", cancel);
        }

        #endregion

        #region Implementation of IUserRoleStore<User>

        public async Task AddToRoleAsync(User user, string role, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/Role/{role}", user, cancel);
        }

        public async Task RemoveFromRoleAsync(User user, string role, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/Role/Delete/{role}", user, cancel);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/roles", user, cancel))
                .Content
                .ReadAsAsync<IList<string>>(cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> IsInRoleAsync(User user, string role, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/InRole/{role}", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel)
                .ConfigureAwait(false);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string role, CancellationToken cancel)
        {
            return await GetAsync<List<User>>($"{ServiceAddress}/UsersInRole/{role}", cancel);
        }

        #endregion

        #region Implementation of IUserPasswordStore<User>

        public async Task SetPasswordHashAsync(User user, string hash, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/SetPasswordHash", new PasswordHashDTO { Hash = hash, User = user }, cancel);
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetPasswordHash", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel).ConfigureAwait(false);
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/HasPassword", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel)
                .ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserTwoFactorStore<User>

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancel)
        {
            user.TwoFactorEnabled = enabled;
            await PostAsync($"{ServiceAddress}/SetTwoFactor/{enabled}", user, cancel);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetTwoFactorEnabled", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel)
                .ConfigureAwait(false);
        }


        #endregion

        #region Implementation of IUserEmailStore<User>

        public async Task SetEmailAsync(User user, string email, CancellationToken cancel)
        {
            user.Email = email;
            await PostAsync($"{ServiceAddress}/SetEmail/{email}", user, cancel);
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetEmail", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetEmailConfirmed", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            user.EmailConfirmed = confirmed;
            await PostAsync($"{ServiceAddress}/SetEmailConfirmed/{confirmed}", user, cancel);
        }

        public async Task<User> FindByEmailAsync(string email, CancellationToken cancel)
        {
            return await GetAsync<User>($"{ServiceAddress}/User/FindByEmail/{email}", cancel);
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/User/GetNormalizedEmail", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetNormalizedEmailAsync(User user, string email, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/SetnormalizedEmail/{email}", user, cancel);
        }

        #endregion

        #region Implementation of IUserPhoneNumberStore<User>

        public async Task SetPhoneNumberAsync(User user, string phone, CancellationToken cancel)
        {
            user.PhoneNumber = phone;
            await PostAsync($"{ServiceAddress}/SetPhoneNumber/{phone}", user, cancel);
        }

        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetPhoneNumber", user, cancel))
                .Content
                .ReadAsAsync<string>(cancel)
                .ConfigureAwait(false);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetPhoneNumberConfirmed", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel)
                .ConfigureAwait(false);
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            user.PhoneNumberConfirmed = confirmed;
            await PostAsync($"{ServiceAddress}/SetPhoneNumberConfirmed/{confirmed}", user, cancel);
        }

        #endregion

        #region Implementation of IUserClaimStore<User>

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetClaims", user, cancel))
                .Content
                .ReadAsAsync<List<Claim>>(cancel)
                .ConfigureAwait(false);
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/AddClaims", new AddClaimsDTO { User = user, Claims = claims }, cancel);
        }

        public async Task ReplaceClaimAsync(User user, Claim OldClaim, Claim NewClaim, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/ReplaceClaim",
                new ReplaceClaimDTO { User = user, OldClaim = OldClaim, NewClaim = NewClaim }, cancel);
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/RemoveClaims", new RemoveClaimsDTO { User = user, Claims = claims },
                cancel);
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancel)
        {
            var response = await PostAsync($"{ServiceAddress}/GetUsersForClaim", claim, cancel);
            return await response
                .Content
                .ReadAsAsync<List<User>>(cancel)
                .ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserLoginStore<User>

        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/AddLogin", new AddLoginDTO { User = user, UserLoginInfo = login }, cancel);
        }

        public async Task RemoveLoginAsync(User user, string LoginProvider, string ProviderKey, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/RemoveLogin/{LoginProvider}/{ProviderKey}", user, cancel);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetLogins", user, cancel))
                .Content
                .ReadAsAsync<List<UserLoginInfo>>(cancel);
        }

        public async Task<User> FindByLoginAsync(string LoginProvider, string ProviderKey, CancellationToken cancel)
        {
            return await GetAsync<User>($"{ServiceAddress}/User/FindByLogin/{LoginProvider}/{ProviderKey}", cancel);
        }

        #endregion

        #region Implementation of IUserLockoutStore<User>

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetLockoutEndDate", user, cancel))
                .Content
                .ReadAsAsync<DateTimeOffset?>(cancel);
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset? EndDate, CancellationToken cancel)
        {
            user.LockoutEnd = EndDate;
            await PostAsync($"{ServiceAddress}/SetLockoutEndDate",
                new SetLockoutDTO { User = user, LockoutEnd = EndDate }, cancel);
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/IncrementAccessFailedCount", user, cancel))
                .Content
                .ReadAsAsync<int>(cancel).ConfigureAwait(false);
        }

        public async Task ResetAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/ResetAccessFailedCont", user, cancel);
        }

        public async Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetAccessFailedCount", user, cancel))
                .Content
                .ReadAsAsync<int>(cancel);
        }

        public async Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{ServiceAddress}/GetLockoutEnabled", user, cancel))
                .Content
                .ReadAsAsync<bool>(cancel);
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancel)
        {
            await PostAsync($"{ServiceAddress}/SetLockoutEnabled/{enabled}", user, cancel);
        }

        #endregion
    }
}
