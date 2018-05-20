using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.NEP6;
using NeoModules.NEP6.Models;
using NeoModules.RPC;
using PhantasmaMail.Services.Authentication;
using PhantasmaMail.ViewModels.Base;
using Helper = NeoModules.RPC.Helpers.Helper;

namespace PhantasmaMail.Services.Phantasma
{
    public class PrivatePhantasmaService : IPhantasmaService
    {
        public PrivatePhantasmaService()
        {
            _neoRpcClient = new NeoApiService(AppSettings.RpcClient);
        }

        public async Task<string> EstimateMessageCost(string boxName, string message)
        {
            var script = NeoModules.NEP6.Utils.GenerateScript(ContractScriptHashBytes, SendMessageOperation,
                new object[] {boxName, message});

            var result = await _neoRpcClient.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());
            return result?.GasConsumed;
        }

        #region READ Protocol Methods

        public async Task<string> GetMailboxFromAddress()
        {
            var script = NeoModules.NEP6.Utils.GenerateScript(ContractScriptHashBytes, GetInboxFromAddressOperation,
                new object[] {UserAddress});
            var result = await _neoRpcClient.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());

            var content = result.Stack[0].Value.ToString();

            if (string.IsNullOrEmpty(content)) return string.Empty;

            var nameBytes = Helper.HexToBytes(content);
            var name = Encoding.UTF8.GetString(nameBytes);

            return name;
        }

        public async Task<string> GetAddressFromMailbox(string boxName)
        {
            var script = NeoModules.NEP6.Utils.GenerateScript(ContractScriptHashBytes, GetAddressFromInboxOperation,
                new object[] {boxName});
            var result = await _neoRpcClient.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());

            var content = result.Stack[0].Value.ToString();
            if (string.IsNullOrEmpty(content)) return string.Empty;

            var addressScriptHash = Helper.HexToBytes(result.Stack[0].Value.ToString());
            var address = Wallet.ToAddress(new UInt160(addressScriptHash));

            return address;
        }


        public async Task<int> GetInboxCount()
        {
            var script = NeoModules.NEP6.Utils.GenerateScript(ContractScriptHashBytes, GetInboxCountOperation,
                new object[] {UserBoxName});
            var result = await _neoRpcClient.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());

            var count = result.Stack[0].Value.ToString();
            if (string.IsNullOrEmpty(count)) return 0;
            return int.Parse(count, NumberStyles.HexNumber);
        }

        public async Task<string> GetInboxContent(int index)
        {
            var script = NeoModules.NEP6.Utils.GenerateScript(ContractScriptHashBytes, GetInboxContentOperation,
                new object[] {UserBoxName, index});
            var result = await _neoRpcClient.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());

            var content = Helper.HexToBytes(result.Stack[0].Value.ToString());
            return Encoding.UTF8.GetString(content);
        }

        public async Task<int> GetOutboxCount()
        {
            var script = NeoModules.NEP6.Utils.GenerateScript(ContractScriptHashBytes, GetOutboxCountOperation,
                new object[] {UserBoxName});
            var result = await _neoRpcClient.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());

            var count = result.Stack[0].Value.ToString();
            if (string.IsNullOrEmpty(count)) return 0;
            return int.Parse(count, NumberStyles.HexNumber);
        }


        public async Task<string> GetOutboxContent(int index)
        {
            var script = NeoModules.NEP6.Utils.GenerateScript(ContractScriptHashBytes, GetOutboxContentOperation,
                new object[] {UserBoxName, index});
            var result = await _neoRpcClient.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());

            var content = Helper.HexToBytes(result.Stack[0].Value.ToString());
            return Encoding.UTF8.GetString(content);
        }


        public async Task<List<string>> GetAllInboxMessages(int count)
        {
            var emailHashList = new List<string>();
            for (var i = 1; i <= count; i++)
            {
                var mailHash = await GetInboxContent(i);
                emailHashList.Add(mailHash);
            }

            return emailHashList;
        }

        public async Task<List<string>> GetAllOutboxMessages(int count)
        {
            var emailHashList = new List<string>();
            for (var i = 1; i <= count; i++)
            {
                var mailHash = await GetOutboxContent(i);
                emailHashList.Add(mailHash);
            }

            return emailHashList;
        }

        #endregion

        #region WRITE Protocol Methods

        public async Task<string> RegisterMailbox(string name)
        {
            if (!(UserAccount.TransactionManager is AccountSignerTransactionManager accountsigner)) return string.Empty;
            var transaction = await accountsigner.CallContract(UserKeypair, ContractScriptHashBytes,
                RegisterInboxOperation,
                new object[] {UserAddress, name});
            if (transaction == null) return string.Empty;
            return transaction.Hash.ToString();
        }

        public async Task<string> UnregisterMailbox()
        {
            if (!(UserAccount.TransactionManager is AccountSignerTransactionManager accountsigner)) return string.Empty;
            var transaction = await accountsigner.CallContract(UserKeypair, ContractScriptHashBytes,
                UnregisterInboxOperation,
                new object[] {UserAddress});
            if (transaction == null) return string.Empty;
            return transaction.Hash.ToString();
        }


        public async Task<string> SendMessage(string destName, string mailHash)
        {
            if (!(UserAccount.TransactionManager is AccountSignerTransactionManager accountsigner)) return string.Empty;
            var transaction = await accountsigner.CallContract(UserKeypair, ContractScriptHashBytes,
                SendMessageOperation,
                new object[] {UserAddress, destName, mailHash});
            if (transaction == null) return string.Empty;
            return transaction.Hash.ToString();
        }

        public async Task<string> RemoveInboxMessage(int index)
        {
            if (!(UserAccount.TransactionManager is AccountSignerTransactionManager accountsigner)) return string.Empty;

            var transaction = await accountsigner.CallContract(UserKeypair, ContractScriptHashBytes,
                RemoveInboxMessageOperation,
                new object[] {UserAddress, index});
            if (transaction != null) return transaction.Hash.ToString();


            return string.Empty;
        }

        public async Task<string> RemoveOutboxMessage(int index)
        {
            if (!(UserAccount.TransactionManager is AccountSignerTransactionManager accountsigner)) return string.Empty;

            var transaction = await accountsigner.CallContract(UserKeypair, ContractScriptHashBytes,
                RemoveOutboxMessageOperation,
                new object[] {UserAddress, index, ""});
            if (transaction != null) return transaction.Hash.ToString();

            return string.Empty;
        }

        #endregion


        #region Private Properties

        private const string GetInboxFromAddressOperation = "getMailboxFromAddress";
        private const string GetAddressFromInboxOperation = "getAddressFromMailbox";
        private const string RegisterInboxOperation = "registerMailbox";
        private const string UnregisterInboxOperation = "unregisterMailbox";
        private const string SendMessageOperation = "sendMessage";
        private const string RemoveInboxMessageOperation = "removeInboxMessage";
        private const string RemoveOutboxMessageOperation = "removeOutboxMessage";
        private const string GetInboxCountOperation = "getInboxCount";
        private const string GetInboxContentOperation = "getInboxContent";
        private const string GetOutboxCountOperation = "getOutboxCount";
        private const string GetOutboxContentOperation = "getOutboxContent";

        private User ActiveUser => Locator.Instance.Resolve<IAuthenticationService>().AuthenticatedUser;
        private Account UserAccount => ActiveUser.GetDefaultAccount();
        private KeyPair UserKeypair => ActiveUser.GetKeypair();
        private string UserBoxName => ActiveUser.UserBox;
        private byte[] UserAddress => ActiveUser.GetDefaultAccount().Address.ToArray();
        private byte[] ContractScriptHashBytes => UInt160.Parse(ContractScriptHash).ToArray();

        //TODO: change to main net scripthash
        private const string ContractScriptHash = "4b4f63919b9ecfd2483f0c72ff46ed31b5bbb7a4";
        private readonly NeoApiService _neoRpcClient; // TODO: fix so the app only use one RpcClient

        #endregion
    }
}