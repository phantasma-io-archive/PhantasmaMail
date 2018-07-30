using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NeoModules.JsonRpc.Client;
using NeoModules.KeyPairs;
using NeoModules.Rest.DTOs;
using NeoModules.Rest.DTOs.NeoNotifications;
using PhantasmaMail.Models;
using PhantasmaMail.Resources;
using PhantasmaMail.Services;
using PhantasmaMail.Utils;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class WalletTabViewModel : ViewModelBase
    {
        private readonly WalletService _walletService;

        private readonly string NeoScriptHash = "c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b";
        private readonly string GasScriptHash = "602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7";

        public WalletTabViewModel(WalletService walletService)
        {
            _walletService = walletService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await GetBalance();
            await GetTransactionHistory();
        }

        public ICommand SendCommand => new Command(async () => await SendExecute());
        public ICommand ClaimGasCommand => new Command(async () => await ClaimGasExecute());
        public ICommand OpenTxCommand => new Command(async () => await OpenTxExecute());
        public ICommand RefreshCommand => new Command(async () => await GetBalance());

        private async Task SendExecute()
        {
            string tx = string.Empty;
            try
            {
                if (IsBusy) return;
                IsBusy = true;
                DialogService.ShowLoading();
                if (!string.IsNullOrEmpty(ToAddress) && Quantity > 0 && SelectedItem != null) //todo address validation
                {
                    var scriptHash = ToAddress.ToScriptHash();// throws exception if address is not valid
                    if (_assetsPicker.TryGetValue(SelectedItem, out var contractHash))
                    {
                        tx = await _walletService.TransferNep5(ToAddress, Quantity, contractHash);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is RpcClientUnknownException || ex is RpcClientTimeoutException) //todo switch error message
                {
                    AppSettings.ChangeRpcServer();
                }
                await DialogService.ShowAlertAsync(ex.Message, AppResource.Alert_Error);
            }
            finally
            {
                IsBusy = false;
                DialogService.HideLoading();
                if (!string.IsNullOrEmpty(tx))
                {
                    await DialogService.ShowAlertAsync("Asset sent. It might take a few minutes for update balance.", "Success"); //todo localization
                    ToAddress = string.Empty;
                    Quantity = 0;
                }
                else
                {
                    await DialogService.ShowAlertAsync(AppResource.Alert_SomethingWrong, AppResource.Alert_Error); //todo localization
                }
            }
        }

        public async Task GetBalance() //todo
        {
            AssetsList = new ObservableCollection<AssetModel>();
            SelectedItem = null;

            CurrentBalanceFiat = 0;

            var balanceList = await _walletService.GetAccountBalance();
            foreach (var item in balanceList.Balance)
            {
                if (item.Amount <= 0)
                {
                    continue;
                }

                switch (item.Asset)
                {
                    case "NEO":
                        {
                            var priceInfo = await CoinInfoUtils.GetMarketPrice(CoinInfoUtils.NEO_ID);

                            NeoBalance = (decimal)item.Amount;
                            var fiatChange = CoinInfoUtils.CalculateChange(NeoBalance, priceInfo);

                            CurrentBalanceFiat += NeoBalance * priceInfo.price;

                            AssetsList.Add(new AssetModel
                            {
                                Amount = NeoBalance,
                                FiatValue = priceInfo.price,
                                TotalFiatValue = NeoBalance * priceInfo.price,
                                FiatChangePercentage = priceInfo.change,
                                FiatChange = fiatChange,
                                TokenDetails = new Token
                                {
                                    Name = item.Asset,
                                    Symbol = item.Asset
                                },
                                ImagePath = item.Asset + ".png"
                            });
                            break;
                        }
                    case "GAS":
                        {
                            var priceInfo = await CoinInfoUtils.GetMarketPrice(CoinInfoUtils.GAS_ID);

                            GasBalance = (decimal)item.Amount;
                            var fiatChange = CoinInfoUtils.CalculateChange(GasBalance, priceInfo);

                            CurrentBalanceFiat += GasBalance * priceInfo.price;

                            AssetsList.Add(new AssetModel
                            {
                                Amount = GasBalance,
                                FiatValue = priceInfo.price,
                                TotalFiatValue = GasBalance * priceInfo.price,
                                FiatChangePercentage = priceInfo.change,
                                FiatChange = fiatChange,
                                TokenDetails = new Token
                                {
                                    Name = item.Asset,
                                    Symbol = item.Asset
                                },
                                ImagePath = item.Asset + ".png"
                            });
                            break;
                        }
                    default: //NEP5 tokens
                        {
                            var tokenBalance = (decimal)item.Amount / 100000000; //todo decimals
                            var details = AppSettings.TokenList.Results.SingleOrDefault(result => result.Token.Name == item.Asset)?.Token;

                            var tokenId = CoinInfoUtils.GetIDForSymbol(details?.Symbol);
                            var priceInfo = await CoinInfoUtils.GetMarketPrice(tokenId);
                            var isListed = priceInfo != null;
                            var fiatChange = isListed ? CoinInfoUtils.CalculateChange(tokenBalance, priceInfo) : 0;

                            var model = new AssetModel
                            {
                                Amount = tokenBalance,
                                TokenDetails = details,
                            };

                            if (isListed)
                            {
                                CurrentBalanceFiat += tokenBalance * priceInfo.price;
                                model.TotalFiatValue = tokenBalance * priceInfo.price;
                                model.FiatValue = priceInfo.price;
                                model.FiatChangePercentage = priceInfo.change;
                                model.FiatChange = fiatChange;
                            }

                            model.ImagePath = model.TokenDetails.Symbol + ".png";
                            AssetsList.Add(model);
                            break;
                        }
                }

                if (_assetsPicker.ContainsKey(item.Asset)) // todo add to send picker
                {
                    PickerItemList.Add(item.Asset);
                }

                if (SelectedItem == null && PickerItemList.Any())
                {
                    SelectedItem = PickerItemList[0];
                }
            }
        }

        public async Task GetNativeAssetsBalance()
        {
            var balance = await _walletService.GetAssets();
            foreach (var asset in balance)
            {
                switch (asset.Asset)
                {
                    case "0xc56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b"://todo move this
                        NeoBalance = decimal.Parse(asset.Value);
                        break;
                    case "0x602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7":
                        GasBalance = decimal.Parse(asset.Value);
                        break;
                }
            }
        }

        public async Task GetTransactionHistory()
        {
            try
            {
                var transactionHistory = await _walletService.GetTransactionHistory();
                TransactionsList = new ObservableCollection<TransactionModel>();
                foreach (var transaction in transactionHistory)
                {
                    var txModel = new TransactionModel
                    {
                        ToAddress = transaction.AddressTo,
                        FromAddress = transaction.AddressFrom,
                        Asset = transaction.Asset,
                        TxHash = transaction.Txid
                    };
                    if (txModel.Asset == NeoScriptHash)
                    {
                        txModel.Symbol = "NEO";
                        txModel.Amount = decimal.Parse(transaction.Amount);
                    }
                    else if (txModel.Asset == GasScriptHash)
                    {
                        txModel.Symbol = "GAS";
                        txModel.Amount = decimal.Parse(transaction.Amount, NumberFormatInfo.InvariantInfo);
                    }
                    else
                    {
                        txModel.Symbol = AppSettings.TokenList.Results
                            .SingleOrDefault(result => result.Token.ScriptHash.Substring(2) == txModel.Asset)
                            ?.Token.Symbol;
                        txModel.Amount = decimal.Parse(transaction.Amount) / 100000000; //todo decimals
                    }
                    txModel.ImagePath = txModel.ToAddress == _walletService.GetUserAddress() ? "ic_receive.png" : "ic_send.png";

                    TransactionsList.Add(txModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task ClaimGasExecute()
        {
            await DialogService.ShowAlertAsync(AppResource.Alert_FeatureNotLive, AppResource.Alert_Error);
        }

        private async Task OpenTxExecute()
        {
            if (SelectedTransaction != null)
            {
                var uri = new Uri(AppSettings.NeoScanUrlTransactions + SelectedTransaction.TxHash);
                await Browser.OpenAsync(uri, BrowserLaunchType.SystemPreferred);
                SelectedTransaction = null;
            }
        }

        #region Observable Properties

        public string UserAddress => _walletService.GetUserAddress();

        private TransactionModel _selectedTransaction;

        public TransactionModel SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                _selectedTransaction = value;
                OnPropertyChanged();
                OpenTxCommand.Execute(_selectedTransaction);
            }
        }
        private ObservableCollection<AssetModel> _assetsList;

        public ObservableCollection<AssetModel> AssetsList
        {
            get => _assetsList;
            set
            {
                _assetsList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TransactionModel> _transactionsList;

        public ObservableCollection<TransactionModel> TransactionsList
        {
            get => _transactionsList;
            set
            {
                _transactionsList = value;
                OnPropertyChanged();
            }
        }

        private decimal _neoBalance;

        public decimal NeoBalance
        {
            get => _neoBalance;
            set
            {
                _neoBalance = value;
                OnPropertyChanged();
            }
        }

        private decimal _gasBalance;
        public decimal GasBalance
        {
            get => _gasBalance;
            set
            {
                _gasBalance = value;
                OnPropertyChanged();
            }
        }

        private decimal _currentBalanceFiat;
        public decimal CurrentBalanceFiat
        {
            get => _currentBalanceFiat;
            set
            {
                _currentBalanceFiat = value;
                OnPropertyChanged();
            }
        }

        private readonly Dictionary<string, string> _assetsPicker = new Dictionary<string, string>
        {
            { "NEO", "c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b" },
            { "GAS", "602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de" },
            { "DeepBrain Coin", "b951ecbbc5fe37a9c280a76cb0ce0014827294cf" },
            { "Red Pulse Token", "ecc6b20d3ccac1ee9ef109af5a7cdb85706b1df9" },
            { "Redeemable HashPuppy Token", "2328008e6f6c7bd157a342e789389eb034d9cbc4" },
            { "Qlink Token", "0d821bd7b6d53f5c2b40e217c6defc8bbe896cf5" },
            { "Narrative Token", "a721d5893480260bd28ca1f395f2c465d0b5b1c2" },
            { "Bridge Token", "891daf0e1750a1031ebe23030828ad7781d874d6" },
            { "Ontology Token", "ceab719b8baa2310f232ee0d277c061704541cfb" },
            { "Thor Token", "67a5086bac196b67d5fd20745b0dc9db4d2930ed" },
            { "Travala,", "de2ed49b691e76754c20fe619d891b78ef58e537" },
            { "Switcheo", "ab38352559b8b203bde5fddfa0b07d8b2525e132" },
            { "Effect.AI Token", "acbc532904b6b51b5ea6d19b803d78af70e7e6f9" },
            { "Master Contract Token", "a87cc2a513f5d8b4a42432343687c2127c60bc3f" },
            { "Guardium", "d1e37547d88bc9607ff9d73116ebd9381c156f79" },
            { "Pikcio Token", "af7c7328eee5a275a3bcaee2bf0cf662b5e739be" },
            { "Phantasma", "ed07cffad18f1308db51920d99a2af60ac66a7b3" },
            { "Asura World Coin", "a58b56b30425d3d1f8902034996fcac4168ef71d" },
            { "Loopring Neo Token", "06fa8be9b6609d963e8fc63977b9f8dc5f10895f" },
            { "Trinity Network Credit", "08e8c4400f1af2c20c28e0018f29535eb85d15b6" },
            { "Orbis", "0e86a40588f715fcaf7acd1812d50af478e6e917" },
            { "THEKEY Token", "132947096727c84c7f9e076c90f08fec3bc17f18" },
            { "Aphelion", "a0777c3ce2b169d4a23bcba4565e3225a0122d95" },
            { "NKN", "c36aee199dbba6c3f439983657558cfb67629599" },
            { "ACAT Token,ACAT", "7f86d61ff377f1b12e589a5907152b57e2ad9a7a" },
            { "Quarteria Token", "6eca2c4bd2b3ed97b2aa41b26128a40ce2bd8d1a" },
            { "Endorsit Shares", "81c089ab996fc89c468a26c0a88d23ae2f34b5c0" },
            { "Zeepin Token", "ac116d4b8d4ca55e6b6d4ecce2192039b51cccc5" },
            { "CPX Token", "45d493a6f73fa5f404244a5fb8472fc014ca5885" },
        };

        public ObservableCollection<string> PickerItemList { get; set; } = new ObservableCollection<string>();

        private string _selectedItem;
        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Send

        private string _toAddress;
        public string ToAddress
        {
            get => _toAddress;
            set
            {
                _toAddress = value;
                OnPropertyChanged();
            }
        }

        private decimal _quantity;
        public decimal Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }


        #endregion
    }
}
