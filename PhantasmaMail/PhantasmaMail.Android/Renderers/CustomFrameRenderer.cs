using Android.Content;
using PhantasmaMail.Controls;
using PhantasmaMail.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

//[assembly: ExportRenderer(typeof(CustomFrame), typeof(CustomFrameRenderer))]
namespace PhantasmaMail.Droid.Renderers
{
    public class CustomFrameRenderer : FrameRenderer
    {
        public CustomFrameRenderer(Context ctx) : base(ctx)
        {

        }


        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                int pL = ViewGroup.PaddingLeft;
                int pT = ViewGroup.PaddingTop;
                int pR = ViewGroup.PaddingRight;
                int pB = ViewGroup.PaddingBottom;
                ViewGroup.SetBackgroundResource(Resource.Drawable.shadow_frame);
                //e.NewElement.Margin = new Thickness(20, 10);
                ViewGroup.SetPadding(pL, pT, pR, pB);
            }
        }
    }
}