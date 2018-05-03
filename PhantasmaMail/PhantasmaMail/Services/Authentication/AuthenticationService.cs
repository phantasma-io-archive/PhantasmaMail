using System;
using System.Threading.Tasks;

namespace PhantasmaMail.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService //TODO
    {
        public bool IsAuthenticated { get; }

        public Task<bool> LoginAsync(string encryptedKey, string password)
        {
            return Task.FromResult(!string.IsNullOrEmpty(encryptedKey) && !string.IsNullOrEmpty(password));
        }

        public Task<bool> LoginAsync(string wif)
        {
            return Task.FromResult(!string.IsNullOrEmpty(wif));
        }

        public Task LogoutAsync()
        {
            //IsAuthenticated = false;
            return Task.FromResult(true);
        }
    }
}