using System;
using System.Diagnostics;
using System.Linq;
using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("io.phantasma.PhantasmaMail")]
[assembly: ExportEffect(typeof(PhantasmaMail.Droid.Effects.MaxLinesEffect), "MaxLinesEffect")]
namespace PhantasmaMail.Droid.Effects
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

                if (Control is TextView nativeTextView)
                {
                    if (maxLinesEffect.MaxLines <= 0)
                    {
                        nativeTextView.SetMaxLines(99999);
                    }
                    else if (maxLinesEffect.MaxLines == 1)
                    {
                        nativeTextView.SetSingleLine(true);
                        nativeTextView.Ellipsize = TextUtils.TruncateAt.End;
                    }
                    else if (maxLinesEffect.MaxLines > 1)
                    {
                        nativeTextView.SetMaxLines(maxLinesEffect.MaxLines);
                        nativeTextView.Ellipsize = TextUtils.TruncateAt.End;
                    }
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