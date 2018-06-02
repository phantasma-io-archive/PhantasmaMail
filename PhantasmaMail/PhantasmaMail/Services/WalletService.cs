using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Android.Icu.Util;
using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.NEP6;
using NeoModules.NEP6.Models;
using NeoModules.Rest.DTOs;
using NeoModules.Rest.Services;
using NeoModules.RPC;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Services;
using Newtonsoft.Json;
using PhantasmaMail.Services.Authentication;
using PhantasmaMail.ViewModels.Base;

namespace PhantasmaMail.Services
{
    public class WalletService
    {
        public AccountSignerTransactionManager AccountManager => (AccountSignerTransactionManager)ActiveUser.GetDefaultAccount().TransactionManager;
        private User ActiveUser => Locator.Instance.Resolve<IAuthenticationService>().AuthenticatedUser;


        public WalletService()
        {
            //AccountManager.InitializeNep5Service("0xed07cffad18f1308db51920d99a2af60ac66a7b3"); //mainnet
            AccountManager.InitializeNep5Service(AppSettings.ContractScriptHash); //mainnet

        }

        public async Task<string> TransferNep5(string toAddress, decimal amount, string tokenScriptHash)
        {
            var tokenScriptHashBytes = UInt160.Parse(tokenScriptHash).ToArray();
            var toAddressBytes = toAddress.ToScriptHash().ToArray();
            var tx = await AccountManager.TransferNep5(toAddressBytes, amount, tokenScriptHashBytes);
            return tx;
        }

        public async Task<string> SendAsset(string toAddress, string symbol, decimal amount)
        {
            var tx = await AccountManager.SendAsset(toAddress, symbol, amount);
            return tx.Hash.ToString();
        }

        public async Task<List<AccountBalance>> GetAssets()
        {
            var assetService = new NeoApiAccountService(AppSettings.RpcClient);
            var assetBalance = await assetService.GetAccountState.SendRequestAsync(ActiveUser.GetUserDefaultAddress());
            return assetBalance.Balance;
        }

        public async Task<AddressBalance> GetAccountBalance()
        {
            var balance = await AppSettings.RestService.GetBalanceAsync(ActiveUser.GetUserDefaultAddress());
            return AddressBalance.FromJson(balance);
        }

        public async Task<TokenList> GetAllNep5Tokens()
        {
            var token = await AppSettings.RestService.GetAllTokens();
            return JsonConvert.DeserializeObject<TokenList>(token);
        }
    }
}
