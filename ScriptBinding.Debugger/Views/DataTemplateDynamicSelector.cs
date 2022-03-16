using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ScriptBinding.Debugger.Services;

namespace ScriptBinding.Debugger.Views
{
    class DataTemplateDynamicSelector : DataTemplateSelector
    {
        private readonly DataTemplateBuilder _builder = new DataTemplateBuilder();
        private readonly Dictionary<Type, DataTemplate> _cache = new Dictionary<Type, DataTemplate>();

        public DataTemplate NullTemplate { get; set; }

        #region Overrides of DataTemplateSelector

        /// <inheritdoc />
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is null)
                return NullTemplate;

            Type itemType = item.GetType();

            if (_cache.TryGetValue(itemType, out DataTemplate result))
                return result;

            AssociatedViewAttribute associatedView = itemType.GetAttribute<AssociatedViewAttribute>();

            if (associatedView == null)
                throw new NotSupportedException($"{nameof(DataTemplateDynamicSelector)} supports only types with {nameof(AssociatedViewAttribute)}");
                
            result = _builder.Build(associatedView.ViewType);

            _cache.Add(itemType, result);

            return result;
        }

        #endregion
    }
}
