using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhantasmaMail.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterBoxView : ContentPage
	{
		public RegisterBoxView ()
		{
			InitializeComponent ();
		    NavigationPage.SetHasNavigationBar(this, false);
        }
	}
}