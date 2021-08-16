using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using PAD.Data.Models;
using System;
using System.Threading.Tasks;

namespace PAD.Data
{
    public class Session
    {
        private readonly ILocalStorageService _storage;
        private readonly AuthenticationStateProvider _auth;
        private readonly UserManager<IdentityUser> _user;
        private readonly IDbService _db;

        public Session(ILocalStorageService storage, AuthenticationStateProvider auth, UserManager<IdentityUser> user, IDbService db)
        {
            _storage = storage;
            _auth = auth;
            _user = user;
            _db = db;
        }

        /// <summary>
        /// Deletes the session from local storage.
        /// </summary>
        /// <returns>bool</returns>
        public async Task DeleteSession()
        {
            var existing = await _storage.ContainKeyAsync("session_pad");
            if (existing) await _storage.RemoveItemAsync("session_pad");
        }

        /// <summary>
        /// Gets the current session.
        /// </summary>
        /// <returns>Session</returns>
        public async Task<SessionObj> GetSession()
        {
            var session = await _storage.GetItemAsync<SessionObj>("session_pad");
            if (session?.ExpireDate == DateTime.MinValue)
            {
                await DeleteSession();
                return null;
            }

            if (session != null) return session;

            // try and make one
            var account = await GetAccount();
            if (account == null) return null; // can't get session
            session = await CreateSession(account.AccountId);

            return session;
        }

        /// <summary>
        /// Creates a session in local storage.
        /// </summary>
        /// <param name="acctId"></param>
        /// <returns>Session</returns>
        public async Task<SessionObj> CreateSession(int acctId)
        {
            await DeleteSession();
            var session = new SessionObj()
            {
                SessionId = Guid.NewGuid(),
                ExpireDate = DateTime.UtcNow.AddMinutes(30)
            };
            await _storage.SetItemAsync("session_pad", session); // create the session
            return session;
        }

        /// <summary>
        /// Gets the account from the Authentication Client.
        /// </summary>
        /// <returns>Account</returns>
        public async Task<Account> GetAccount()
        {
            var state = await _auth.GetAuthenticationStateAsync();
            if (state == null) return null;
            var user = await _user.GetUserAsync(state.User);
            if (user == null) return null;
            var account = await _db.GetAccountAsync(user.Id);
            return account;
        }
    }
}

