using Android.Content;
using Android.Graphics.Drawables;
using PhantasmaMail.Controls;
using PhantasmaMail.Droid.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExpandableEditor), typeof(ExpandableEditorRenderer))]
namespace PhantasmaMail.Droid.Renderers
{
    public class ExpandableEditorRenderer : EditorRenderer
    {
        public ExpandableEditorRenderer(Context ctx) : base(ctx) { }

        //public static void Init() { }

        //protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        //{
        //    base.OnElementChanged(e);
        //    if (e.OldElement == null)
        //    {
        //        Control.Background = null;

        //        var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
        //        layoutParams.SetMargins(0, 0, 0, 0);
        //        LayoutParameters = layoutParams;
        //        Control.LayoutParameters = layoutParams;
        //        Control.SetPadding(0, 0, 0, 0);
        //        SetPadding(0, 0, 0, 0);
        //    }
        //}
        bool initial = true;
        Drawable originalBackground;


        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                if (initial)
                {
                    originalBackground = Control.Background;
                    initial = false;
                }

            }

            if (e.NewElement != null)
            {
                var customControl = (ExpandableEditor)Element;
                if (customControl.HasRoundedCorner)
                {
                    ApplyBorder();
                }

                if (!string.IsNullOrEmpty(customControl.Placeholder))
                {
                    Control.Hint = customControl.Placeholder;
                    Control.SetHintTextColor(customControl.PlaceholderColor.ToAndroid());

                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var customControl = (ExpandableEditor)Element;

            if (ExpandableEditor.PlaceholderProperty.PropertyName == e.PropertyName)
            {
                Control.Hint = customControl.Placeholder;

            }
            else if (ExpandableEditor.PlaceholderColorProperty.PropertyName == e.PropertyName)
            {

                Control.SetHintTextColor(customControl.PlaceholderColor.ToAndroid());

            }
            else if (ExpandableEditor.HasRoundedCornerProperty.PropertyName == e.PropertyName)
            {
                if (customControl.HasRoundedCorner)
                {
                    ApplyBorder();

                }
                else
                {
                    this.Control.Background = originalBackground;
                }
            }
        }

        void ApplyBorder()
        {
            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(10);
            gd.SetStroke(2, Color.Black.ToAndroid());
            this.Control.Background = gd;
        }
    }
}