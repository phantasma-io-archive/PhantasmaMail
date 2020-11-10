using FFImageLoading.Forms.Platform;
using FFImageLoading.Svg.Forms;
using Foundation;
using KeyboardOverlap.Forms.Plugin.iOSUnified;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfPicker.XForms.iOS;
using UIKit;
using Xfx;

namespace PhantasmaMail.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
			Rg.Plugins.Popup.Popup.Init();
            XfxControls.Init();
            Xamarin.Forms.Forms.Init();
            KeyboardOverlapRenderer.Init();
            SfPickerRenderer.Init();

            SfListViewRenderer.Init();
            CachedImageRenderer.Init();
            var ignore = typeof(SvgCachedImage);

            LoadApplication(new App());

            base.FinishedLaunching(app, options);


            // test
            //UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            //UINavigationBar.Appearance.ShadowImage = new UIImage();
            //UINavigationBar.Appearance.BackgroundColor = UIColor.Clear;
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.White });
            //UINavigationBar.Appearance.BarTintColor = UIColor.FromPatternImage(UIImage.FromFile("TopBar.png"));
            //UINavigationBar.Appearance.Translucent = true;

            return true;
        }
    }
}
