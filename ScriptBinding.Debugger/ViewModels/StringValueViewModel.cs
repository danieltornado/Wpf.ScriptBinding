using System;
using System.ComponentModel;
using ScriptBinding.Debugger.ViewModels.Base;
using ScriptBinding.Debugger.Views;

namespace ScriptBinding.Debugger.ViewModels
{
    [AssociatedView(typeof(StringValueView))]
    class StringValueViewModel : ValidatingViewModel, IBindingValueViewModel
    {
        private readonly Converter<string, object> _converter;
        private object _convertedValue;

        public StringValueViewModel(Converter<string, object> converter)
        {
            _converter = converter;
        }

        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                if (Set(ref _value, value, nameof(Value)))
                    Validate();
            }
        }

        private void Validate()
        {
            ClearError(nameof(Value));

            // Is null-empty value checking needed?

            try
            {
                _convertedValue = _converter(_value);
            }
            catch
            {
                SetError(nameof(Value), "Incorrect value");
                _convertedValue = null;
            }
        }

        #region Overrides of BindingValueViewModel

        /// <inheritdoc />
        object IBindingValueViewModel.GetValue()
        {
            return _convertedValue;
        }

        /// <inheritdoc />
        public bool HasErrors()
        {
            return NotifyDataErrorInfo.HasErrors;
        }

        #endregion

        private INotifyDataErrorInfo NotifyDataErrorInfo => this;
    }
}
