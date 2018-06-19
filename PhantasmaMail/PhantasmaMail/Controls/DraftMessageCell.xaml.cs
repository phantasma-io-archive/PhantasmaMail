using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhantasmaMail.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DraftMessageCell : Grid
	{
		public DraftMessageCell ()
		{
			InitializeComponent ();
		}
	}
}