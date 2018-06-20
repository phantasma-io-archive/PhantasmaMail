using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhantasmaMail.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComposeView : PopupPage
    {
        public ComposeView()
        {
            InitializeComponent();
        }

        private void ContentEditor_OnFocused(object sender, FocusEventArgs e)
        {
            IconGrid.IsVisible = false;
            ToEntry.IsVisible = false;
            SubjectEntry.IsVisible = false;
        }


        private void ContentEditor_OnUnfocused(object sender, FocusEventArgs e)
        {
            IconGrid.IsVisible = true;
            ToEntry.IsVisible = true;
            SubjectEntry.IsVisible = true;
        }

        protected override bool OnBackButtonPressed()
        {
            PopupNavigation.Instance.PopAsync();
            return true;
        }
    }
}