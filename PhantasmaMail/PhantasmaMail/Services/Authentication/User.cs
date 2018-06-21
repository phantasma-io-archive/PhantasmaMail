using System.Linq;
using System.Threading.Tasks;
using NeoModules.KeyPairs;
using NeoModules.NEP6;
using NeoModules.NEP6.Models;
using NeoModules.RPC.Infrastructure;

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
            WalletManager = new WalletManager(AppSettings.RestService, AppSettings.RpcClient, localWallet);
            return
                await WalletManager.ImportAccount(encryptedKey, password, AddressLabel);
        }

        public Account InitializeUserWallet(string wif)
        {
            var localWallet = new Wallet(WalletLabel); //todo make this persistent
            WalletManager = new WalletManager(AppSettings.RestService, AppSettings.RpcClient, localWallet);
            return WalletManager.ImportAccount(wif, AddressLabel);
        }

        public Account InitializeUserWallet(byte[] privKey)
        {
            var localWallet = new Wallet(WalletLabel); //todo make this persistent
            WalletManager = new WalletManager(AppSettings.RestService, AppSettings.RpcClient,localWallet);
            return WalletManager.ImportAccount(privKey, AddressLabel);
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

        public byte[] GetPrivateKey()
        {
            var account = (IAccount) WalletManager?.GetDefaultAccount();
            return account?.PrivateKey;
        }

        public byte[] GetPublicKey()
        {
            var account = (IAccount)WalletManager?.GetDefaultAccount();
            var keyPair = new KeyPair(account?.PrivateKey);
            return keyPair.PublicKey.EncodePoint(false).Skip(1).ToArray();
        }
    }
}