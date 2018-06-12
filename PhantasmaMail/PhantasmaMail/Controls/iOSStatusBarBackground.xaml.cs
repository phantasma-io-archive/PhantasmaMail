using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhantasmaMail.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class iOSStatusBarBackground : ContentView
	{
	    public iOSStatusBarBackground()
	    {
	        InitializeComponent();
	        IsVisible = Device.RuntimePlatform == Device.iOS;
	    }

	    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
	    {
	        base.OnPropertyChanged(propertyName);

	        if (propertyName == IsVisibleProperty.PropertyName
	            && IsVisible
	            && Device.RuntimePlatform != Device.iOS)
	        {
	            IsVisible = false;
	        }
	    }
    }
}