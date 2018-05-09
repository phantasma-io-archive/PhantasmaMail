using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhantasmaMail.Services.Dialog
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel = "Ok");
        void ShowToast(string message, int duration = 5000);
        Task<bool> ShowConfirmAsync(string message, string title, string okLabel, string cancelLabel);
        Task<string> SelectActionAsync(string message, string title, IEnumerable<string> options);
        Task<string> SelectActionAsync(string message, string title, string cancelLabel, IEnumerable<string> options);
        void ShowLoading(string title = "Loading");
        void HideLoading();
    }
}
