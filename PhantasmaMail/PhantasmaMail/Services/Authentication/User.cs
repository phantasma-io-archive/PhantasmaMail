using System.Linq;
using System.Threading.Tasks;
using NeoModules.KeyPairs;
using NeoModules.NEP6;
using NeoModules.NEP6.Models;

namespace PhantasmaMail.Services.Authentication
{
    public class User
    {
        public WalletManager WalletManager { get; set; }

        public User()
        {
            TransactionInput.SendFromMobile = true;
        }

        public async Task<Account> InitializeUserWallet(string encryptedKey, string password)
        {
            var localWallet = new Wallet("Phantasma Wallet"); //todo make this persistent
            WalletManager = new WalletManager(localWallet, AppSettings.RestService, AppSettings.RpcClient);
            return
                await WalletManager.ImportAccount(encryptedKey, password, "Phantasma address"); //todo label
        }

        public Account InitializeUserWallet(string wif)
        {
            var localWallet = new Wallet("Phantasma Wallet"); //todo make this persistent
            WalletManager = new WalletManager(localWallet, AppSettings.RestService, AppSettings.RpcClient);
            return WalletManager.ImportAccount(wif, "Phantasma address"); //todo label
        }

        public Account GetDefaultAccount()
        {
            return WalletManager?.GetDefaultAccount();
        }

        public string GetUserDefaultAddress()
        {
            if (WalletManager == null) return string.Empty;
            var account =  WalletManager.GetDefaultAccount();
            var address = Wallet.ToAddress(account.Address);
            return address;
        }

        public byte[] GetCompressedPublicKey()
        {
            var account = WalletManager?.GetDefaultAccount();
            return account?.Key.PublicKey.EncodePoint(true).ToArray();
        }

        public KeyPair GetKeypair()
        {
            var account = WalletManager?.GetDefaultAccount();
            return account?.Key;
        }
    }
}