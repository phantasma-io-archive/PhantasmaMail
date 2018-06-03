using Android.App;
using Android.Content.PM;
using Android.OS;
using Acr.UserDialogs;
using CarouselView.FormsPlugin.Android;
using FFImageLoading.Forms.Platform;
using FFImageLoading.Svg.Forms;
using Xfx;

namespace PhantasmaMail.Droid
{
    [Activity(Label = "Phantasma Mail", Icon = "@drawable/phantasma_logo_vector", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            XfxControls.Init();
            Xamarin.Forms.Forms.Init(this, bundle);
            CarouselViewRenderer.Init();
            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, bundle);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            CachedImageRenderer.Init(true);
            var ignore = typeof(SvgCachedImage);
            LoadApplication(new App());
			XFGloss.Droid.Library.Init(this, bundle);
		}

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

