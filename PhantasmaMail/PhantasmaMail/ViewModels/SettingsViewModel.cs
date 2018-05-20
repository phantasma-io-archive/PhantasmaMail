using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NeoModules.RPC.Services;
using PhantasmaMail.Resources;
using PhantasmaMail.ViewModels.Base;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {

        public ICommand RefreshCommand => new Command(async () => await RefreshExecute());

        //public ICommand MintCommand => new Command(async () => await MintExecute());

        public ICommand TransferCommand => new Command(async () => await TransferExecute());

        public ICommand UnregisterInboxCommand => new Command(async () => await UnregisterInboxExecute());



        #region nep5 test

        private async Task RefreshExecute()
        {
            await InitializeAsync(null);
        }

        private Task TransferExecute()
        {
            throw new NotImplementedException();
        }
        #endregion



        public override async Task InitializeAsync(object navigationData)
        {
            var nep5Service = SetupNep5Service();
            var symbol = await nep5Service.GetSymbol();
            var name = await nep5Service.GetName();
            var decimals = await nep5Service.GetDecimals();
            var totalSupply = await nep5Service.GetTotalSupply(decimals);
            var balance = await nep5Service.GetBalance("0xbfe77ff8de50fb9db2525f7ffb73b72ec32d32ec", decimals);
            var balance2 = await nep5Service.GetBalance("ec322dc32eb773fb7f5f52b29dfb50def87fe7bf", decimals);
        }

        private async Task UnregisterInboxExecute()
        {
            try
            {
                var tx = await PhantasmaService.UnregisterMailbox();
                if (!string.IsNullOrEmpty(tx))
                {
                    await NavigationService.NavigateToAsync<LoginViewModel>();
                }
                await DialogService.ShowAlertAsync(tx, "TODO");
            }
            catch (Exception ex)
            {
                await DialogService.ShowAlertAsync(ex.Message, AppResource.Alert_Error);
            }
        }

        private static NeoNep5Service SetupNep5Service()
        {
            return new NeoNep5Service(AppSettings.RpcClient, "insertaddress");
        }

    }
}
