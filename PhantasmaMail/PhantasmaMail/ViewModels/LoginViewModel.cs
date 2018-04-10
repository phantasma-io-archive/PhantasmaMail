using PhantasmaMail.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PhantasmaMail.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        public ICommand LoginCommand => new Command(async () => await LoginExecute());

        private async Task LoginExecute()
        {
            // TODO LOGIN LOGIC
            //await Task.Delay(1);
            await NavigationService.NavigateToAsync<MainViewModel>();
        }
    }
}
