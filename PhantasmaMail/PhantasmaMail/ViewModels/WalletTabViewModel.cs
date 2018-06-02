using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Java.Lang;
using NeoModules.Rest.DTOs;
using PhantasmaMail.Models;
using PhantasmaMail.Services;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class WalletTabViewModel : ViewModelBase
    {
        private readonly WalletService _walletService;

        public WalletTabViewModel(WalletService walletService)
        {
            _walletService = walletService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            //todo get assets and nep5 balances
            await GetBalance();
            await SendExecute();
        }

        public ICommand SendCommand => new Command(async () => await SendExecute());

        private async Task SendExecute()
        {
            
        }

        private async Task GetNep5()
        {
            var test = await _walletService.GetAllNep5Tokens();
            foreach (var result in test.Results)
            {
                AssetsList.Add(new AssetModel
                {
                    TokenDetails = result.Token
                }
                );
            }
        }

        private async Task GetBalance() //todo
        {
            AssetsList = new ObservableCollection<AssetModel>();
            var balanceList = await _walletService.GetAccountBalance();
            foreach (var item in balanceList.Balance)
            {
                if (item.Asset == "NEO")
                {
                    NeoBalance = (decimal)item.Amount;
                    AssetsList.Add(new AssetModel
                    {
                        Amount = (decimal)item.Amount,
                        TokenDetails = new Token
                        {
                            Name = item.Asset
                        }
                    });

                }
                if (item.Asset == "GAS")
                {
                    GasBalance = (decimal)item.Amount;
                    AssetsList.Add(new AssetModel
                    {
                        Amount = (decimal)item.Amount,
                        TokenDetails = new Token
                        {
                            Name = item.Asset
                        }
                    });
                }
                else
                {
                    AssetsList.Add(new AssetModel
                    {
                        Amount = (decimal)item.Amount / 100000000,
                        TokenDetails = new Token
                        {
                            Name = item.Asset
                        }
                    });
                }
            }
            CurrentBalanceFiat = 100;
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
                    case "0x602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de":
                        GasBalance = decimal.Parse(asset.Value);
                        break;
                }
            }
        }

        #region Observable Properties

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
        #endregion
    }
}
