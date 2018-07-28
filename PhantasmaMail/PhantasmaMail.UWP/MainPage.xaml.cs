using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using PhantasmaMail;
using FFImageLoading.Forms.Platform;
using Syncfusion.ListView.XForms.UWP;
using Syncfusion.SfPicker.XForms.UWP;
using Syncfusion.SfPullToRefresh.XForms.UWP;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PhantasmaMail.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            CachedImageRenderer.Init();
            SfPullToRefreshRenderer.Init();
            SfListViewRenderer.Init();
            SfPickerRenderer.Init();
            LoadApplication(new PhantasmaMail.App());
            NativeCustomize();
        }

        private void NativeCustomize()
        {
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));

            // PC Customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.BackgroundColor = (Color)App.Current.Resources["NativeAccentColor"];
                    titleBar.ButtonBackgroundColor = (Color)App.Current.Resources["NativeAccentColor"];
                }
            }

            // Mobile Customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = (Color)App.Current.Resources["NativeAccentColor"];
                }
            }

            // Launch in Window Mode
            var currentView = ApplicationView.GetForCurrentView();
            if (currentView.IsFullScreenMode)
            {
                currentView.ExitFullScreenMode();
            }
        }
    }
}
