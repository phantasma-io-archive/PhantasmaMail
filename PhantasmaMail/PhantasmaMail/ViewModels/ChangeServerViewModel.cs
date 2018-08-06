using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PhantasmaMail.ViewModels.Base;

namespace PhantasmaMail.ViewModels
{
    public class ChangeServerViewModel : ViewModelBase
    {
        private ObservableCollection<string> _serverList;

        public ObservableCollection<string> ServerList
        {
            get => _serverList;
            set
            {
                _serverList = value;
                OnPropertyChanged();
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            ServerList = new ObservableCollection<string>(AppSettings.RpcUrlList);
            return base.InitializeAsync(navigationData);
        }

        public async Task ChangeServerExecute(string url)
        {
            AppSettings.ChangeRpcServer(url);
            await NavigationService.NavigateBackAsync();
        }
    }
}
