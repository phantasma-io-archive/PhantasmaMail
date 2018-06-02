using System.Threading.Tasks;
using NeoModules.NEP6;
using NeoModules.NEP6.Models;

namespace PhantasmaMail.Services.Authentication
{
    public class User
    {
        private const string WalletLabel = "Phantasma wallet";
        private const string AddressLabel = "Phantasma address";
        public WalletManager WalletManager { get; set; }
        public string UserBox { get; set; }

        public async Task<Account> InitializeUserWallet(string encryptedKey, string password)
        {
            var localWallet = new Wallet(WalletLabel); //todo make this persistent
            WalletManager = new WalletManager(localWallet, AppSettings.RestService, AppSettings.RpcClient);
            return
                await WalletManager.ImportAccount(encryptedKey, password, AddressLabel);
        }

        public Account InitializeUserWallet(string wif)
        {
            var localWallet = new Wallet(WalletLabel); //todo make this persistent
            WalletManager = new WalletManager(localWallet, AppSettings.RestService, AppSettings.RpcClient);
            return WalletManager.ImportAccount(wif, AddressLabel);
        }

        public Account GetDefaultAccount()
        {
            return WalletManager?.GetDefaultAccount();
        }

        public string GetUserDefaultAddress()
        {
            if (WalletManager == null) return string.Empty;
            var account = WalletManager.GetDefaultAccount();
            var address = Wallet.ToAddress(account.Address);
            return address;
        }
    }
}