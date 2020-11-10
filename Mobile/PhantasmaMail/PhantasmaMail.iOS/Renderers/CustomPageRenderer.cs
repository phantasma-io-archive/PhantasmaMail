using PhantasmaMail.iOS.Renderers;
using PhantasmaMail.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Page), typeof(CustomPageRenderer))]
namespace PhantasmaMail.iOS.Renderers
{
    public class CustomPageRenderer : PageRenderer
    {
        IElementController ElementController => Element as IElementController;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            // Force nav bar text color
            //var color = NavigationBarAttachedProperty.GetTextColor(Element);
            NavigationBarAttachedProperty.SetTextColor(Element, Color.White);
        }
    }
}