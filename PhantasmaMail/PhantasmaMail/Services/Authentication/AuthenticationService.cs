using System.Threading.Tasks;
using NeoModules.NEP6;

namespace PhantasmaMail.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService //TODO
    {
        public bool IsAuthenticated { get; set; }
        public User AuthenticatedUser { get; set; }

        public async Task<bool> LoginAsync(string encryptedKey, string password)
        {
            if (!string.IsNullOrEmpty(encryptedKey) && !string.IsNullOrEmpty(password))
            {
                AuthenticatedUser = new User();
                var account = await AuthenticatedUser.InitializeUserWallet(encryptedKey, password);
                if (account == null) throw new WalletException("Error importing account"); //todo move to localization

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
                if (account == null) throw new WalletException("Error importing account");

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