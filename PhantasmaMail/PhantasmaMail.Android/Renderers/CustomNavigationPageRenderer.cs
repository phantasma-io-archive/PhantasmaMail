using Android.Content;
using Android.Widget;
using PhantasmaMail.Droid.Renderers;
using PhantasmaMail.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using AView = Android.Views.View;


//[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(CustomNavigationPageRenderer))]
namespace PhantasmaMail.Droid.Renderers
{
	public class CustomNavigationPageRenderer : NavigationPageRenderer
	{
		public CustomNavigationPageRenderer(Context ctx) : base(ctx)
		{

		}
		IPageController PageController => Element as IPageController;

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			base.OnLayout(changed, l, t, r, b);

			int containerHeight = b - t;

			PageController.ContainerArea = new Rectangle(0, 0, Context.FromPixels(r - l), Context.FromPixels(containerHeight));

			for (var i = 0; i < ChildCount; i++)
			{
				AView child = GetChildAt(i);

				if (child is Toolbar)
				{
					continue;
				}

				child.Layout(0, 0, r, b);
			}
		}
	}
}