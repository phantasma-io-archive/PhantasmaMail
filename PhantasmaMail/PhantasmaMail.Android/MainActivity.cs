using Android.App;
using Android.Content.PM;
using Android.OS;
using Acr.UserDialogs;
using FFImageLoading.Forms.Droid;
using FFImageLoading.Svg.Forms;

namespace PhantasmaMail.Droid
{
    [Activity(Label = "Phantasma Mail", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
			

			global::Xamarin.Forms.Forms.Init(this, bundle);
			UserDialogs.Init(this);
			Rg.Plugins.Popup.Popup.Init(this, bundle);
            CachedImageRenderer.Init(true);
            var ignore = typeof(SvgCachedImage);

            LoadApplication(new App());
			XFGloss.Droid.Library.Init(this, bundle);
		}
    }
}

