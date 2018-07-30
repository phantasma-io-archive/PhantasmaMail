using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.NEP6;
using NeoModules.NEP6.Models;
using NeoModules.RPC;
using PhantasmaMail.Services.Authentication;
using PhantasmaMail.ViewModels.Base;

namespace PhantasmaMail.Services.Phantasma
{
    public class PrivatePhantasmaService : IPhantasmaService
    {
        public NeoApiService ApiService { get; set; }

        public PrivatePhantasmaService()
        {
            ApiService = new NeoApiService(AppSettings.RpcClient);
        }

        public async Task<string> EstimateMessageCost(string boxName, string message)
        {
            var script = NeoModules.NEP6.Utils.GenerateScript(ContractScriptHashBytes, SendMessageOperation,
                new object[] { boxName, message });

            var result = await ApiService.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());
            return result?.GasConsumed;
        }

        private async Task<string> SendContractTransaction(string operation, object[] args)
        {
            if (AccountManager != null)
            {
                var transaction = await AccountManager?.CallContract(ContractScriptHashBytes,
                    operation,
                    args);

                if (transaction != null) return transaction.Hash.ToString();
            }

            return string.Empty;
        }

        #region READ Protocol Methods

        public async Task<string> GetMailboxFromAddress()
        {
            var script = NeoModules.NEP6.Utils.GenerateScript(ContractScriptHashBytes, GetInboxFromAddressOperation,
                new object[] { UserAddress });
            var result = await ApiService.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());

            if (result.Stack.Count > 1) return string.Empty;

            var content = result.Stack[0].Value.ToString();

            if (string.IsNullOrEmpty(content)) return string.Empty;

            var nameBytes = content.HexToBytes();
            var name = Encoding.UTF8.GetString(nameBytes);

            return name;
        }

        public async Task<string> GetAddressFromMailbox(string boxName)
        {
            var script = NeoModules.NEP6.Utils.GenerateScript(ContractScriptHashBytes, GetAddressFromInboxOperation,
                new object[] { boxName });
            var result = await ApiService.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());

            var content = result.Stack[0].Value.ToString();
            if (string.IsNullOrEmpty(content)) return string.Empty;

            var addressScriptHash = result.Stack[0].Value.ToString().HexToBytes();
            var address = new UInt160(addressScriptHash).ToAddress();

            return address;
        }


        public async Task<int> GetInboxCount()
        {
            var script = NeoModules.NEP6.Utils.GenerateScript(ContractScriptHashBytes, GetInboxCountOperation,
                new object[] { UserBoxName });
            var result = await ApiService.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());

            var count = result.Stack[0].Value.ToString();
            if (string.IsNullOrEmpty(count)) return 0;
            return int.Parse(count, NumberStyles.HexNumber);
        }

        public async Task<string> GetInboxContent(int index)
        {
            var script = NeoModules.NEP6.Utils.GenerateScript(ContractScriptHashBytes, GetInboxContentOperation,
                new object[] { UserBoxName, index });
            var result = await ApiService.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());

            var content = result.Stack[0].Value.ToString().HexToBytes();
            return Encoding.UTF8.GetString(content);
        }

        public async Task<int> GetOutboxCount()
        {
            var script = NeoModules.NEP6.Utils.GenerateScript(ContractScriptHashBytes, GetOutboxCountOperation,
                new object[] { UserBoxName });
            var result = await ApiService.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());

            var count = result.Stack[0].Value.ToString();
            if (string.IsNullOrEmpty(count)) return 0;
            return int.Parse(count, NumberStyles.HexNumber);
        }


        public async Task<string> GetOutboxContent(int index)
        {
            var script = NeoModules.NEP6.Utils.GenerateScript(ContractScriptHashBytes, GetOutboxContentOperation,
                new object[] { UserBoxName, index });
            var result = await ApiService.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());

            var content = result.Stack[0].Value.ToString().HexToBytes();
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
            return await SendContractTransaction(RegisterInboxOperation, new object[] { UserAddress, name });
        }

        public async Task<string> UnregisterMailbox()
        {
            return await SendContractTransaction(UnregisterInboxOperation, new object[] { UserAddress });
        }


        public async Task<string> SendMessage(string destName, string mailHash)
        {
            return await SendContractTransaction(SendMessageOperation, new object[] { UserAddress, destName, mailHash });
        }

        public async Task<string> RemoveInboxMessage(int index)
        {
            return await SendContractTransaction(RemoveInboxMessageOperation, new object[] { UserAddress, index });
        }

        public async Task<string> RemoveOutboxMessage(int index)
        {
            return await SendContractTransaction(RemoveOutboxMessageOperation, new object[] { UserAddress, index });
        }

        public async Task<string> RemoveInboxMessages(int[] indexes)
        {
            var args = new List<object> { UserAddress };
            var orderedIndex = indexes.OrderBy(ind => ind).ToArray();
            args.AddRange(orderedIndex.Cast<object>());

            return await SendContractTransaction(RemoveInboxMessagesOperation, args.ToArray());
        }

        public async Task<string> RemoveOutboxMessages(int[] indexes)
        {
            var args = new List<object> { UserAddress };
            var orderedIndex = indexes.OrderBy(ind => ind).ToArray();
            args.AddRange(orderedIndex.Cast<object>());

            return await SendContractTransaction(RemoveOutboxMessagesOperation, args.ToArray());
        }

        #endregion

        #region Private Properties

        private const string GetInboxFromAddressOperation = "getMailboxFromAddress";
        private const string GetAddressFromInboxOperation = "getAddressFromMailbox";
        private const string RegisterInboxOperation = "registerMailbox";
        private const string UnregisterInboxOperation = "unregisterMailbox";
        private const string SendMessageOperation = "sendMessage";
        private const string RemoveInboxMessageOperation = "removeInboxMessage";
        private const string RemoveInboxMessagesOperation = "removeInboxMessages";
        private const string RemoveOutboxMessageOperation = "removeOutboxMessage";
        private const string RemoveOutboxMessagesOperation = "removeOutboxMessages";
        private const string GetInboxCountOperation = "getInboxCount";
        private const string GetInboxContentOperation = "getInboxContent";
        private const string GetOutboxCountOperation = "getOutboxCount";
        private const string GetOutboxContentOperation = "getOutboxContent";

        private User ActiveUser => Locator.Instance.Resolve<IAuthenticationService>().AuthenticatedUser;
        public AccountSignerTransactionManager AccountManager => (AccountSignerTransactionManager)ActiveUser.GetDefaultAccount().TransactionManager;
        private string UserBoxName => ActiveUser.UserBox;
        private byte[] UserAddress => ActiveUser.GetDefaultAccount().Address.ToArray();
        private byte[] ContractScriptHashBytes => UInt160.Parse(ContractScriptHash).ToArray();

        //TODO: change to main net scripthash
        private string ContractScriptHash => AppSettings.ContractScriptHash;

        #endregion
    }
}