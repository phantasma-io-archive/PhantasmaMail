using PhantasmaMail.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhantasmaMail.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeServerView : ContentPage
    {
        public ChangeServerView()
        {
            InitializeComponent();
        }

        private async void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (BindingContext is ChangeServerViewModel vm)
            {
                await vm.ChangeServerExecute(e.Item as string);
            }
        }
    }
}