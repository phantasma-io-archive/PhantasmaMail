using Android.Graphics;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(PhantasmaMail.Droid.Effects.UnderlineEffect), "UnderlineEffect")]
namespace PhantasmaMail.Droid.Effects
{
    public class UnderlineEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var textView = (TextView)Control;
            textView.PaintFlags |= PaintFlags.UnderlineText;
        }

        protected override void OnDetached()
        {
            var textView = (TextView)Control;
            textView.PaintFlags &= ~PaintFlags.UnderlineText;
        }
    }
}