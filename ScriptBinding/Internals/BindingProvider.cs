using System.Collections.Generic;
using System.Windows.Data;
using ScriptBinding.Internals.Executor;

namespace ScriptBinding.Internals
{
    sealed class BindingProvider : IBindingProvider
    {
        private IReadOnlyList<BindingBase> _bindings;
        private object[] _values;

        public void SetBindings(IReadOnlyList<BindingBase> allBindings)
        {
            _bindings = allBindings;
        }

        public void SetValues(object[] values)
        {
            _values = values;
        }

        #region Implementation of IBindingProvider

        /// <inheritdoc />
        bool IBindingProvider.TryGetValue(int index, out object value)
        {
            if (index >= 0 && index < _values.Length)
            {
                value = _values[index];
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc />
        bool IBindingProvider.TryGetValue(string propertyPath, out object value)
        {
            int i = 0;
            while (i < _bindings.Count)
            {
                if (_bindings[i] is GeneratedBinding binding && binding.Path.Path == propertyPath && binding.ElementName is null)
                    return ExplicitThis.TryGetValue(i, out value);

                i++;
            }

            value = default;
            return false;
        }

        /// <inheritdoc />
        bool IBindingProvider.TryGetValue(string propertyPath, string elementName, out object value)
        {
            int i = 0;
            while (i < _bindings.Count)
            {
                if (_bindings[i] is GeneratedBinding binding && binding.Path.Path == propertyPath && binding.ElementName == elementName)
                    return ExplicitThis.TryGetValue(i, out value);

                i++;
            }

            value = default;
            return false;
        }

        #endregion

        private IBindingProvider ExplicitThis => this;
    }
}
