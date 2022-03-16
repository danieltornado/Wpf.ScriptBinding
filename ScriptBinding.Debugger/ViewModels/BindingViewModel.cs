using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using ScriptBinding.Debugger.ViewModels.Base;
using ScriptBinding.Debugger.Views;

namespace ScriptBinding.Debugger.ViewModels
{
    [AssociatedView(typeof(BindingView))]
    class BindingViewModel : ValidatingViewModel
    {
        public BindingViewModel(string propertyPath)
            : this()
        {
            // Property binding
            PropertyPath = propertyPath;
        }

        private BindingViewModel()
        {
            AvailableTypes = new ObservableCollection<TypeViewModel>
            {
                new TypeViewModel(typeof(byte), new StringValueViewModel(e => byte.Parse(e))),
                new TypeViewModel(typeof(sbyte), new StringValueViewModel(e => sbyte.Parse(e))),
                new TypeViewModel(typeof(short), new StringValueViewModel(e => short.Parse(e))),
                new TypeViewModel(typeof(ushort), new StringValueViewModel(e => ushort.Parse(e))),
                new TypeViewModel(typeof(int), new StringValueViewModel(e => int.Parse(e))),
                new TypeViewModel(typeof(uint), new StringValueViewModel(e => uint.Parse(e))),
                new TypeViewModel(typeof(long), new StringValueViewModel(e => long.Parse(e))),
                new TypeViewModel(typeof(ulong), new StringValueViewModel(e => ulong.Parse(e))),
                new TypeViewModel(typeof(float), new StringValueViewModel(e => float.Parse(e, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture))),
                new TypeViewModel(typeof(double), new StringValueViewModel(e => double.Parse(e, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture))),
                new TypeViewModel(typeof(decimal), new StringValueViewModel(e => decimal.Parse(e, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture))),
                new TypeViewModel(typeof(bool), new StringValueViewModel(e => bool.Parse(e))),
                new TypeViewModel(typeof(string), new StringValueViewModel(e => e)),
                new TypeViewModel(typeof(char), new StringValueViewModel(e => char.Parse(e))),

                new TypeViewModel(typeof(System.Windows.Visibility), new EnumValueViewModel<System.Windows.Visibility>())
            };

            SelectedType = AvailableTypes.First(e => e.Type == typeof(int));
        }

        public string PropertyPath { get; }

        private TypeViewModel _selectedType;
        public TypeViewModel SelectedType
        {
            get => _selectedType;
            set 
            {
                if (Set(ref _selectedType, value, nameof(SelectedType)))
                    RaisePropertyChanged(nameof(ValueViewModel));
            }
        }

        public ObservableCollection<TypeViewModel> AvailableTypes { get; }

        public IBindingValueViewModel ValueViewModel
        {
            get
            {
                if (_selectedType == null)
                    return null; // maybe use something else?

                return _selectedType.BindingValueViewModel;
            }
        }
    }
}
