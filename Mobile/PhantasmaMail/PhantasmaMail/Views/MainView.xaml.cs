using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhantasmaMail.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : MasterDetailPage
    {
        public MainView()
        {
            InitializeComponent();
            var platform = Device.RuntimePlatform == Device.UWP;
            MasterBehavior = platform ? MasterBehavior.Split : MasterBehavior.Popover;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}