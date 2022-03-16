using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using ScriptBinding.Internals.Compiler.Expressions;
using ScriptBinding.Internals.Executor.ErrorListeners;

namespace ScriptBinding.Internals
{
    class ScriptConverter : IMultiValueConverter
    {
        private Expr _expression;
        private readonly Executor.Executor _executor;
        private readonly BindingProvider _bindingProvider;

        public ScriptConverter(IExecutingErrorListener errorListener)
        {
            var bindingProvider = new BindingProvider();

            _executor = new Executor.Executor(errorListener, bindingProvider);
            _bindingProvider = bindingProvider;
        }

        public void SetExpression(Expr expression, IReadOnlyList<BindingBase> bindings)
        {
            _expression = expression;
            _bindingProvider.SetBindings(bindings);
        }

        #region Implementation of IMultiValueConverter

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Executes expression with values

            _bindingProvider.SetValues(values);

            var value = _executor.Execute(_expression);
            return value;
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}