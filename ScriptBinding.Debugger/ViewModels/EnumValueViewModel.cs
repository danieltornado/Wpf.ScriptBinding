using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using ScriptBinding.Debugger.ViewModels.Base;
using ScriptBinding.Debugger.Views;

namespace ScriptBinding.Debugger.ViewModels
{
    [AssociatedView(typeof(EnumValueView))]
    class EnumValueViewModel<T> : ViewModelBase, IBindingValueViewModel
        where T : Enum
    {
        public EnumValueViewModel()
        {
            AvailableValues = new ObservableCollection<T>(GetEnumValues());
        }

        public ObservableCollection<T> AvailableValues { get; }

        private T _selectedValue;
        public T SelectedValue
        {
            get => _selectedValue;
            set => Set(ref _selectedValue, value, nameof(SelectedValue));
        }

        #region Overrides of BindingValueViewModel

        /// <inheritdoc />
        object IBindingValueViewModel.GetValue()
        {
            return _selectedValue;
        }

        /// <inheritdoc />
        public bool HasErrors()
        {
            return false;
        }

        #endregion

        private static IEnumerable<T> GetEnumValues()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
