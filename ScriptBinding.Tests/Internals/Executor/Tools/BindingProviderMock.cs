using System.Collections.Generic;
using ScriptBinding.Internals.Executor;

namespace ScriptBinding.Tests.Internals.Executor.Tools
{
    class BindingProviderMock : IBindingProvider
    {
        #region nested

        public class Value
        {
            private object _value;
            private bool _isEmpty;
            private int _countOfReading;

            public void Set(object value)
            {
                _isEmpty = false;
                _value = value;
            }

            public void Clear()
            {
                _isEmpty = true;
                _value = null;
            }

            public bool TryGet(out object value)
            {
                if (_isEmpty)
                {
                    value = default;
                    return false;
                }

                value = _value;
                return true;
            }

            public int CountOfReading()
            {
                return _countOfReading;
            }

            public void ClearCountOfReading()
            {
                _countOfReading = 0;
            }

            internal void IncreaseCountOfReading()
            {
                _countOfReading++;
            }
        }

        #endregion

        private readonly Dictionary<int, Value> _values = new Dictionary<int, Value>();
        private readonly Dictionary<string, Value> _propertyValues = new Dictionary<string, Value>();
        private readonly Dictionary<(string propertyPath, string elementName), Value> _elementValues = new Dictionary<(string, string), Value>();

        public Value GetValue(int index)
        {
            var key = index;

            if (_values.TryGetValue(key, out var value))
                return value;

            value = new Value();
            _values.Add(key, value);

            return value;
        }

        public Value GetValue(string propertyPath)
        {
            var key = propertyPath;

            if (_propertyValues.TryGetValue(key, out var value))
                return value;

            value = new Value();
            _propertyValues.Add(key, value);

            return value;
        }

        public Value GetValue(string propertyPath, string elementName)
        {
            var key = (propertyPath, elementName);

            if (_elementValues.TryGetValue(key, out var value))
                return value;

            value = new Value();
            _elementValues.Add(key, value);

            return value;
        }

        private bool TryGetValue(Value container, out object value)
        {
            container.IncreaseCountOfReading();

            if (container.TryGet(out value))
                return true;

            value = default;
            return false;
        }

        #region Implementation of IBindingProvider

        /// <inheritdoc />
        bool IBindingProvider.TryGetValue(int index, out object value)
        {
            var container = GetValue(index);
            return TryGetValue(container, out value);
        }

        /// <inheritdoc />
        bool IBindingProvider.TryGetValue(string propertyPath, out object value)
        {
            var container = GetValue(propertyPath);
            return TryGetValue(container, out value);
        }

        /// <inheritdoc />
        bool IBindingProvider.TryGetValue(string propertyPath, string elementName, out object value)
        {
            var container = GetValue(propertyPath, elementName);
            return TryGetValue(container, out value);
        }

        #endregion
    }
}
