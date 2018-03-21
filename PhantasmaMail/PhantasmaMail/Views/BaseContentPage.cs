using Xamarin.Forms;
using XFGloss;

namespace PhantasmaMail.Views
{
	public class BaseContentPage : ContentPage
	{
		public BaseContentPage()
		{
			// Manually construct a multi-color gradient at an angle of our choosing
			var bkgrndGradient = new Gradient()
			{
				Rotation = 348,
				StartColor = Color.FromHex("#FFEBBC"),
				EndColor = Color.FromHex("#A0C9D6"),
			};
			ContentPageGloss.SetBackgroundGradient(this, bkgrndGradient);
		}
	}
}
