using System;
using System.Threading.Tasks;

namespace PhantasmaMail.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService //TODO
    {
        public bool IsAuthenticated { get; }

        public Task<bool> LoginAsync(string encryptedKey, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoginAsync(string wif)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            //IsAuthenticated = false;
            return Task.FromResult(true);
        }
    }
}