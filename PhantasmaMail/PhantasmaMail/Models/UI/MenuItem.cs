using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PhantasmaMail.Models.UI
{
    public class MenuItem : BindableObject
    {
        private string _title;
        private MenuItemType _menuItemType;
        private Type _viewModelType;
        private bool _isEnabled;

        public string Title
        {
            get => _title;

            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public MenuItemType MenuItemType
        {
            get => _menuItemType;

            set
            {
                _menuItemType = value;
                OnPropertyChanged();
            }
        }

        public Type ViewModelType
        {
            get => _viewModelType;

            set
            {
                _viewModelType = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;

            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public Func<Task> AfterNavigationAction { get; set; }
    }
}