using PhantasmaMail.Controls;
using PhantasmaMail.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExpandableEditor), typeof(ExpandableEditorRenderer))]
namespace PhantasmaMail.iOS.Renderers
{
    public class ExpandableEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
                Control.ScrollEnabled = false;
        }
    }
}