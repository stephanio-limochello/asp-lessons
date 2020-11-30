using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services.Identity;

namespace WebStore.Clients.Identity
{
    public class UsersClient : BaseClient, IUsersClient
    {
        public UsersClient(IConfiguration Configuration) : base(Configuration, WebAPI.Identity.Users) { }

        #region Implementation of IUserStore<User>

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/UserId", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel)
               .ConfigureAwait(false);
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/UserName", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel)
               .ConfigureAwait(false);
        }

        public async Task SetUserNameAsync(User user, string name, CancellationToken cancel)
        {
            var response = await PostAsync($"{_serviceAddress}/UserName/{name}", user, cancel);
            user.UserName = await response.Content.ReadAsAsync<string>(cancel);
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/NormalUserName/", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel)
               .ConfigureAwait(false);
        }

        public async Task SetNormalizedUserNameAsync(User user, string name, CancellationToken cancel)
        {
            var response = await PostAsync($"{_serviceAddress}/NormalUserName/{name}", user, cancel);
            user.NormalizedUserName = await response.Content.ReadAsAsync<string>(cancel);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancel)
        {
            var creation_success = await (await PostAsync($"{_serviceAddress}/User", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);

            return creation_success
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancel)
        {
            return await (await PutAsync($"{_serviceAddress}/User", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/User/Delete", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel)
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<User> FindByIdAsync(string id, CancellationToken cancel)
        {
            return await GetAsync<User>($"{_serviceAddress}/User/Find/{id}", cancel);
        }

        public async Task<User> FindByNameAsync(string name, CancellationToken cancel)
        {
            return await GetAsync<User>($"{_serviceAddress}/User/Normal/{name}", cancel);
        }

        #endregion

        #region Implementation of IUserRoleStore<User>

        public async Task AddToRoleAsync(User user, string role, CancellationToken cancel)
        {
            await PostAsync($"{_serviceAddress}/Role/{role}", user, cancel);
        }

        public async Task RemoveFromRoleAsync(User user, string role, CancellationToken cancel)
        {
            await PostAsync($"{_serviceAddress}/Role/Delete/{role}", user, cancel);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/roles", user, cancel))
               .Content
               .ReadAsAsync<IList<string>>(cancel);
        }

        public async Task<bool> IsInRoleAsync(User user, string role, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/InRole/{role}", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string role, CancellationToken cancel)
        {
            return await GetAsync<List<User>>($"{_serviceAddress}/UsersInRole/{role}", cancel);
        }

        #endregion

        #region Implementation of IUserPasswordStore<User>

        public async Task SetPasswordHashAsync(User user, string hash, CancellationToken cancel)
        {
            var response = await PostAsync(
                $"{_serviceAddress}/SetPasswordHash", new PasswordHashDTO { User = user, Hash = hash },
                cancel);
            user.PasswordHash = await response.Content.ReadAsAsync<string>(cancel);
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/GetPasswordHash", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel);
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/HasPassword", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);
        }

        #endregion

        #region Implementation of IUserEmailStore<User>

        public async Task SetEmailAsync(User user, string email, CancellationToken cancel)
        {
            var response = await PostAsync($"{_serviceAddress}/SetEmail/{email}", user, cancel);
            user.Email = await response.Content.ReadAsAsync<string>(cancel);
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/GetEmail", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel);
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/GetEmailConfirmed", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            var response = await PostAsync($"{_serviceAddress}/SetEmailConfirmed/{confirmed}", user, cancel);
            user.EmailConfirmed = await response.Content.ReadAsAsync<bool>(cancel);
        }

        public async Task<User> FindByEmailAsync(string email, CancellationToken cancel)
        {
            return await GetAsync<User>($"{_serviceAddress}/User/FindByEmail/{email}", cancel);
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/User/GetNormalizedEmail", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel);
        }

        public async Task SetNormalizedEmailAsync(User user, string email, CancellationToken cancel)
        {
            var response = await PostAsync($"{_serviceAddress}/SetNormalizedEmail/{email}", user, cancel);
            user.NormalizedEmail = await response.Content.ReadAsAsync<string>(cancel);
        }

        #endregion

        #region Implementation of IUserPhoneNumberStore<User>

        public async Task SetPhoneNumberAsync(User user, string phone, CancellationToken cancel)
        {
            var response = await PostAsync($"{_serviceAddress}/SetPhoneNumber/{phone}", user, cancel);
            user.PhoneNumber = await response.Content.ReadAsAsync<string>(cancel);
        }

        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/GetPhoneNumber", user, cancel))
               .Content
               .ReadAsAsync<string>(cancel);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/GetPhoneNumberConfirmed", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            var response = await PostAsync($"{_serviceAddress}/SetPhoneNumberConfirmed/{confirmed}", user, cancel);
            user.PhoneNumberConfirmed = await response.Content.ReadAsAsync<bool>(cancel);
        }

        #endregion

        #region Implementation of IUserLoginStore<User>

        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancel)
        {
            await PostAsync($"{_serviceAddress}/AddLogin", new AddLoginDTO { User = user, UserLoginInfo = login }, cancel);
        }

        public async Task RemoveLoginAsync(User user, string LoginProvider, string ProviderKey, CancellationToken cancel)
        {
            await PostAsync($"{_serviceAddress}/RemoveLogin/{LoginProvider}/{ProviderKey}", user, cancel);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/GetLogins", user, cancel))
               .Content
               .ReadAsAsync<List<UserLoginInfo>>(cancel);
        }

        public async Task<User> FindByLoginAsync(string LoginProvider, string ProviderKey, CancellationToken cancel)
        {
            return await GetAsync<User>($"{_serviceAddress}/User/FindByLogin/{LoginProvider}/{ProviderKey}", cancel);
        }

        #endregion

        #region Implementation of IUserLockoutStore<User>

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/GetLockoutEndDate", user, cancel))
               .Content
               .ReadAsAsync<DateTimeOffset?>(cancel);
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset? EndDate, CancellationToken cancel)
        {
            var response = await PostAsync(
                $"{_serviceAddress}/SetLockoutEndDate",
                new SetLockoutDTO { User = user, LockoutEnd = EndDate },
                cancel);
            user.LockoutEnd = await response.Content.ReadAsAsync<DateTimeOffset?>(cancel);
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/IncrementAccessFailedCount", user, cancel))
               .Content
               .ReadAsAsync<int>(cancel);
        }

        public async Task ResetAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            await PostAsync($"{_serviceAddress}/ResetAccessFailedCont", user, cancel);
        }

        public async Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/GetAccessFailedCount", user, cancel))
               .Content
               .ReadAsAsync<int>(cancel);
        }

        public async Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/GetLockoutEnabled", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancel)
        {
            var response = await PostAsync($"{_serviceAddress}/SetLockoutEnabled/{enabled}", user, cancel);
            user.LockoutEnabled = await response.Content.ReadAsAsync<bool>(cancel);
        }

        #endregion

        #region Implementation of IUserTwoFactorStore<User>

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancel)
        {
            var response = await PostAsync($"{_serviceAddress}/SetTwoFactor/{enabled}", user, cancel);
            user.TwoFactorEnabled = await response.Content.ReadAsAsync<bool>(cancel);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/GetTwoFactorEnabled", user, cancel))
               .Content
               .ReadAsAsync<bool>(cancel);
        }

        #endregion

        #region Implementation of IUserClaimStore<User>

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/GetClaims", user, cancel))
               .Content
               .ReadAsAsync<List<Claim>>(cancel);
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync(
                $"{_serviceAddress}/AddClaims",
                new AddClaimDTO { User = user, Claims = claims },
                cancel);
        }

        public async Task ReplaceClaimAsync(User user, Claim OldClaim, Claim NewClaim, CancellationToken cancel)
        {
            await PostAsync(
                $"{_serviceAddress}/ReplaceClaim",
                new ReplaceClaimDTO { User = user, Claim = OldClaim, NewClaim = NewClaim },
                cancel);
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync(
                $"{_serviceAddress}/RemoveClaims",
                new RemoveClaimDTO { User = user, Claims = claims },
                cancel);
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancel)
        {
            return await (await PostAsync($"{_serviceAddress}/GetUsersForClaim", claim, cancel))
               .Content
               .ReadAsAsync<List<User>>(cancel);
        }

        #endregion
    }
}
