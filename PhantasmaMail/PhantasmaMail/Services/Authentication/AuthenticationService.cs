using System.Threading.Tasks;
using NeoModules.NEP6;
using PhantasmaMail.Resources;

namespace PhantasmaMail.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        public bool IsAuthenticated { get; set; }
        public User AuthenticatedUser { get; set; }

        public async Task<bool> LoginAsync(string encryptedKey, string password)
        {
            if (!string.IsNullOrEmpty(encryptedKey) && !string.IsNullOrEmpty(password))
            {
                AuthenticatedUser = new User();
                var account = await AuthenticatedUser.InitializeUserWallet(encryptedKey, password);
                if (account == null) throw new WalletException(AppResource.Alert_Wallet);

                IsAuthenticated = true;
                return true;
            }

            IsAuthenticated = false;
            return false;
        }

        public Task<bool> LoginAsync(string wif)
        {
            if (!string.IsNullOrEmpty(wif))
            {
                AuthenticatedUser = new User();
                var account = AuthenticatedUser.InitializeUserWallet(wif);
                if (account == null) throw new WalletException(AppResource.Alert_Wallet);

                IsAuthenticated = true;
                return Task.FromResult(true);
            }

            IsAuthenticated = false;
            return Task.FromResult(false);
        }

        public Task LogoutAsync()
        {
            AuthenticatedUser = null;
            IsAuthenticated = false;
            return Task.FromResult(false);
        }
    }
}