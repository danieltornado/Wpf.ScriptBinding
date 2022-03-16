using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;

namespace ScriptBinding.Debugger.ViewModels.Base
{
    internal abstract class ValidatingViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private bool _hasErrors;
        private EventHandler<DataErrorsChangedEventArgs> _errorsChanged;
        private readonly Dictionary<string, List<object>> _errors = new Dictionary<string, List<object>>();

        protected void SetError(string propertyName, object error)
        {
            if (_errors.TryGetValue(propertyName, out var list))
            {
                list.Add(error);
            }
            else
            {
                list = new List<object>(1) { error };
                _errors.Add(propertyName, list);
            }

            SetHasError(true);
            RaiseErrorsChanged(propertyName);
        }

        protected void ClearError(string propertyName)
        {
            if (_errors.TryGetValue(propertyName, out var list))
            {
                list.Clear();

                if (_errors.All(e => e.Value.Count == 0))
                    SetHasError(false);

                RaiseErrorsChanged(propertyName);
            }
        }

        protected void ClearErrors()
        {
            SetHasError(false);
            
            foreach (var error in _errors)
            {
                error.Value.Clear();
                RaiseErrorsChanged(error.Key);
            }
        }

        private void SetHasError(bool value)
        {
            Set(ref _hasErrors, value, nameof(INotifyDataErrorInfo.HasErrors));
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            var errorsChanged = _errorsChanged;
            if (errorsChanged != null)
                errorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
            
        #region Implementation of INotifyDataErrorInfo

        /// <inheritdoc />
        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            if (_errors.TryGetValue(propertyName, out var list))
                return list;

            return Enumerable.Empty<object>();
        }

        /// <inheritdoc />
        bool INotifyDataErrorInfo.HasErrors => _hasErrors;

        /// <inheritdoc />
        event EventHandler<DataErrorsChangedEventArgs> INotifyDataErrorInfo.ErrorsChanged
        {
            add => _errorsChanged += value;
            remove => _errorsChanged -= value;
        }

        #endregion
    }
}
