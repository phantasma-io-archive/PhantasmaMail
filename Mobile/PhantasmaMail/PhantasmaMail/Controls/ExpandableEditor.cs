using Xamarin.Forms;

namespace PhantasmaMail.Controls
{
    public class ExpandableEditor : Editor
    {
        //public ExpandableEditor()
        //{
        //    //TextChanged += OnTextChanged;
        //}

        //~ExpandableEditor()
        //{
        //    TextChanged -= OnTextChanged;
        //}

        //private void OnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    InvalidateMeasure();
        //}

        public static BindableProperty PlaceholderProperty
            = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ExpandableEditor));

        public static BindableProperty PlaceholderColorProperty
            = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(ExpandableEditor), Color.LightGray);

        public static BindableProperty HasRoundedCornerProperty
            = BindableProperty.Create(nameof(HasRoundedCorner), typeof(bool), typeof(ExpandableEditor), false);

        public static BindableProperty IsExpandableProperty
            = BindableProperty.Create(nameof(IsExpandable), typeof(bool), typeof(ExpandableEditor), false);

        public bool IsExpandable
        {
            get { return (bool)GetValue(IsExpandableProperty); }
            set { SetValue(IsExpandableProperty, value); }
        }
        public bool HasRoundedCorner
        {
            get { return (bool)GetValue(HasRoundedCornerProperty); }
            set { SetValue(HasRoundedCornerProperty, value); }
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public Color PlaceholderColor
        {
            get { return (Color)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        public ExpandableEditor()
        {
            TextChanged += OnTextChanged;
        }

        ~ExpandableEditor()
        {
            TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsExpandable) InvalidateMeasure();
        }
    }
}
