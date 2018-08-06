using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(PhantasmaMail.iOS.Effects.MaxLinesEffect), "MaxLinesEffect")]
namespace PhantasmaMail.iOS.Effects
{
    public class MaxLinesEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var maxLinesEffect = Element.Effects.FirstOfType<MaxLinesEffect>();
            if (maxLinesEffect == null)
            {
                return;
            }
            if (Control is UILabel nativeLabel)
            {
                nativeLabel.Lines = maxLinesEffect.MaxLines;
                nativeLabel.LineBreakMode = UILineBreakMode.TailTruncation;
            }
        }

        protected override void OnDetached() { }
    }
}