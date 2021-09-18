using System;
using System.Globalization;
using System.Windows.Data;

namespace ScriptBinding.Internals
{
    class ScriptConverter : IMultiValueConverter
    {
        public void SetExpression(ExpressionHolder holder)
        {
            throw new NotImplementedException();
        }

        #region Implementation of IMultiValueConverter

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Executes expression with values

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
