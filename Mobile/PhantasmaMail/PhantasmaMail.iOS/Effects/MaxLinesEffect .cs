using System;
using System.Diagnostics;
using UIKit;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(PhantasmaMail.iOS.Effects.MaxLinesEffect), "MaxLinesEffect")]
namespace PhantasmaMail.iOS.Effects
{
    public class MaxLinesEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var maxLinesEffect = Element.Effects?.OfType<PhantasmaMail.Effects.MaxLinesEffect>().First();
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
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
          
        }

        protected override void OnDetached() { }
    }
}