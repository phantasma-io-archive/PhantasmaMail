using PhantasmaMail.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhantasmaMail.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomNavigationPage : NavigationPage
	{
		public CustomNavigationPage()
		{
			InitializeComponent();
		}

		public CustomNavigationPage(Page root) : base(root)
		{
			InitializeComponent();
		}

		internal void ApplyNavigationTextColor(Page targetPage)
		{
			var color = NavigationBarAttachedProperty.GetTextColor(targetPage);
			BarTextColor = color == Color.Default
				? Color.White
				: color;
			BarBackgroundColor = Color.Transparent;
		}
	}
}