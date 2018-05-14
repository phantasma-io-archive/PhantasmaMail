using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.NEP6;
using NeoModules.RPC;
using NeoModules.RPC.DTOs;
using PhantasmaMail.Services.Authentication;
using PhantasmaMail.ViewModels.Base;
using Helper = NeoModules.RPC.Helpers.Helper;

namespace PhantasmaMail.Services.Phantasma
{
    public class PhantasmaService : IPhantasmaService
    {
        public PhantasmaService()
        {
            _neoRpcClient = new NeoApiService(AppSettings.RpcClient);
        }

        public async Task<string> GetUserMailbox()
        {
            var compressedByteKey = ActiveUser.GetCompressedPublicKey().ToHexString();
            var parameterList = new List<InvokeParameter>
            {
                new InvokeParameter
                {
                    Type = "ByteArray",
                    Value = compressedByteKey
                }
            };
            var result = await _neoRpcClient.Contracts.InvokeFunction.SendRequestAsync(ContractScriptHash,
                GetMailboxFromAddressMethod, parameterList);

            var nameBytes = Helper.HexToBytes(result.Stack[0].Value.ToString());
            var name = Encoding.ASCII.GetString(nameBytes);
            return name;
        }

        public async Task<string> RegisterMailbox(string name)
        {
            var compressedPublicKey = ActiveUser.GetCompressedPublicKey(); //TODO switch this block to an unified call
            var account = ActiveUser.GetDefaultAccount();
            var keypair = ActiveUser.GetKeypair();
            if (compressedPublicKey == null || account == null || keypair == null) return string.Empty;
            var scriptBytes = UInt160.Parse(ContractScriptHash).ToArray();
            if (account.TransactionManager is AccountSignerTransactionManager accountsigner
            ) //todo refractor NeoModules account signer
            {
                var transaction = await accountsigner.CallContract(keypair, scriptBytes, RegisterMailboxMethod,
                    new object[] { compressedPublicKey, name });
                return transaction.Hash.ToString();
            }

            return string.Empty;
        }

        public async Task<string> SendMessage(string destName, string mailHash)
        {
            var compressedPublicKey = ActiveUser.GetCompressedPublicKey(); //TODO switch this block to an unified call
            var account = ActiveUser.GetDefaultAccount();
            var keypair = ActiveUser.GetKeypair();

            var scriptBytes = UInt160.Parse(ContractScriptHash).ToArray();
            if (account.TransactionManager is AccountSignerTransactionManager accountsigner)
            {
                var transaction = await accountsigner.CallContract(keypair, scriptBytes, SendMessageMethod,
                    new object[] { compressedPublicKey, destName, mailHash });
                if (transaction != null) return transaction.Hash.ToString();
            }

            return string.Empty;
        }

        public async Task<int> GetMailCount(string boxName)
        {
            var parameterList = new List<InvokeParameter>
            {
                new InvokeParameter
                {
                    Type = "String",
                    Value = boxName
                }
            };
            var result =
                await _neoRpcClient.Contracts.InvokeFunction.SendRequestAsync(ContractScriptHash, GetMailCountMethod,
                    parameterList);

            //TODO convert hex string to Int
            var count = result.Stack[0].Value.ToString();
            if (string.IsNullOrEmpty(count)) return 0;
            return int.Parse(count, NumberStyles.HexNumber);
        }

        public async Task<string> GetMailContent(string name, int index)
        {
            var parameterList = new List<InvokeParameter>
            {
                new InvokeParameter
                {
                    Type = "String",
                    Value = name
                },
                new InvokeParameter
                {
                    Type = "Integer",
                    Value = index.ToString()
                }
            };
            var result =
                await _neoRpcClient.Contracts.InvokeFunction.SendRequestAsync(ContractScriptHash, GetMailContentMethod,
                    parameterList);

            // TODO: return hash of message
            var content = Helper.HexToBytes(result.Stack[0].Value.ToString());
            return Encoding.ASCII.GetString(content);
        }

        public async Task<List<string>> GetAllMails(string name, int count)
        {
            var emailHashList = new List<string>();
            for (var i = 1; i <= count; i++)
            {
                var mailHash = await GetMailContent(name, i);
                emailHashList.Add(mailHash);
            }

            return emailHashList;
        }

        public async Task<List<string>> GetMailsFromRange(string name, int first, int last)
        {
            var emailHashList = new List<string>();
            for (var i = first; i <= last; i++)
            {
                var mailHash = await GetMailContent(name, i);
                emailHashList.Add(mailHash);
            }

            return emailHashList;
        }

        public async Task EstimateMessageCost(string message)
        {
            var compressedPublicKey = ActiveUser.GetCompressedPublicKey(); //TODO switch this block to an unified call
            var account = ActiveUser.GetDefaultAccount();
            var parameterList = new List<InvokeParameter>
            {
                new InvokeParameter
                {
                    Type = "ByteArray",
                    Value = compressedPublicKey.ToHexString()
                },
                new InvokeParameter
                {
                    Type = "String",
                    Value = "testNeoModules"
                },
                new InvokeParameter
                {
                    Type = "String",
                    Value = message
                }
            };
            if (account.TransactionManager is AccountSignerTransactionManager accountsigner)
            {
                var estimation = await accountsigner.EstimateGasAsync(ContractScriptHash, "sendMessage", parameterList);
                var e = estimation;
            }
        }

        public async Task<string> MintTokens(decimal amount)
        {
            var testScriptHash = "e6310d7a45131fc55e60ec9bfc016901184c88ad";
            var compressedPublicKey = ActiveUser.GetCompressedPublicKey(); //TODO switch this block to an unified call
            var account = ActiveUser.GetDefaultAccount();
            var keypair = ActiveUser.GetKeypair();

            if (compressedPublicKey == null || account == null || keypair == null) return string.Empty;
            var scriptBytes = UInt160.Parse(testScriptHash).ToArray();

            var output = new List<NeoModules.NEP6.Models.TransactionOutput>()
            {
                new NeoModules.NEP6.Models.TransactionOutput()
                {
                    AddressHash = account.Address.ToArray(),
                    Amount = 2,
                }
            };

            if (account.TransactionManager is AccountSignerTransactionManager accountsigner)
            {
                var transaction = await accountsigner.CallContract(keypair, scriptBytes, "mintTokens", null, "NEO", output);
                if (transaction != null) return transaction.Hash.ToString();
            }
            return string.Empty;
        }

        #region Private Properties

        private const string GetMailboxFromAddressMethod = "getMailboxFromAddress";
        private const string RegisterMailboxMethod = "registerMailbox";
        private const string SendMessageMethod = "sendMessage";
        private const string GetMailCountMethod = "getMailCount";
        private const string GetMailContentMethod = "getMailContent";

        private User ActiveUser => Locator.Instance.Resolve<IAuthenticationService>().AuthenticatedUser;

        private const string
            ContractScriptHash = "de1a53be359e8be9f3d11627bcca40548a2d5bc1"; //TODO: change to main net scripthash

        private readonly NeoApiService _neoRpcClient; // TODO: fix so the app only use one RpcClient

        #endregion
    }
}