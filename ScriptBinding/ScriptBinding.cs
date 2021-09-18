using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using ScriptBinding.Internals;

namespace ScriptBinding
{
    [ContentProperty("Bindings")]
    public sealed class ScriptBinding : MarkupExtension, IAddChild
    {
        private readonly MultiBinding _decoratedBinding;
        private readonly ScriptConverter _scriptConverter;
        private readonly ExpressionBuilder _builder;
        private string _expression;
        
        public ScriptBinding()
        {
            _builder = new ExpressionBuilder();
            _scriptConverter = new ScriptConverter();

            _decoratedBinding = new MultiBinding
            {
                Mode = BindingMode.OneWay,
                Converter = _scriptConverter
            };
        }

        public ScriptBinding(string expression)
            : this()
        {
            _expression = expression;
            BuildExpression();
        }

        #region Implementation of IAddChild

        /// <summary>
        /// Called to Add the object as a Child.
        /// </summary>
        /// <param name="value">Object to add as a child - must have type BindingBase</param>
        void IAddChild.AddChild(object value)
        {
            ((IAddChild)_decoratedBinding).AddChild(value);
        }

        /// <summary>
        /// Called when text appears under the tag in markup
        /// </summary>
        /// <param name="text">Text to Add to the Object</param>
        void IAddChild.AddText(string text)
        {
            ((IAddChild)_decoratedBinding).AddText(text);
        }

        #endregion

        #region Overrides of MarkupExtension

        /// <summary>
        /// Return the value to set on the property for the target for this binding.
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _decoratedBinding.ProvideValue(serviceProvider);
        }

        #endregion

        [DefaultValue(null)]
        public string Expression
        {
            get => _expression;
            set
            {
                if (_expression != value)
                {
                    _expression = value;
                    BuildExpression();
                }
            } 
        }

        /// <summary>
        /// List of inner bindings
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Collection<BindingBase> Bindings => _decoratedBinding.Bindings;

        /// <summary>
        /// Update type
        /// </summary>
        [DefaultValue(UpdateSourceTrigger.PropertyChanged)]
        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get => _decoratedBinding.UpdateSourceTrigger;
            set => _decoratedBinding.UpdateSourceTrigger = value;
        }

        /// <summary>
        /// Raise SourceUpdated event whenever a value flows from target to source
        /// </summary>
        [DefaultValue(false)]
        public bool NotifyOnSourceUpdated
        {
            get => _decoratedBinding.NotifyOnSourceUpdated;
            set => _decoratedBinding.NotifyOnSourceUpdated = value;
        }

        /// <summary>
        /// Raise TargetUpdated event whenever a value flows from source to target
        /// </summary>
        [DefaultValue(false)]
        public bool NotifyOnTargetUpdated
        {
            get => _decoratedBinding.NotifyOnTargetUpdated;
            set => _decoratedBinding.NotifyOnTargetUpdated = value;
        }

        /// <summary>
        /// Raise ValidationError event whenever there is a ValidationError on Update
        /// </summary>
        [DefaultValue(false)]
        public bool NotifyOnValidationError
        {
            get => _decoratedBinding.NotifyOnValidationError;
            set => _decoratedBinding.NotifyOnValidationError = value;
        }

        /// <summary>
        ///     Collection&lt;ValidationRule&gt; is a collection of ValidationRule
        ///     instances on either a Binding or a MultiBinding.  Each of the rules
        ///     is checked for validity on update
        /// </summary>
        public Collection<ValidationRule> ValidationRules => _decoratedBinding.ValidationRules;

        /// <summary>
        /// called whenever any exception is encountered when trying to update
        /// the value to the source. The application author can provide its own
        /// handler for handling exceptions here. If the delegate returns
        ///     null - don’t throw an error or provide a ValidationError.
        ///     Exception - returns the exception itself, we will fire the exception using Async exception model.
        ///     ValidationError - it will set itself as the BindingInError and add it to the element’s Validation errors.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter
        {
            get => _decoratedBinding.UpdateSourceExceptionFilter;
            set => _decoratedBinding.UpdateSourceExceptionFilter = value;
        }

        /// <summary>
        /// True if an exception during source updates should be considered a validation error.
        /// </summary>
        [DefaultValue(false)]
        public bool ValidatesOnExceptions
        {
            get => _decoratedBinding.ValidatesOnExceptions;
            set => _decoratedBinding.ValidatesOnExceptions = value;
        }

        /// <summary>
        /// True if a data error in the source item should be considered a validation error.
        /// </summary>
        [DefaultValue(false)]
        public bool ValidatesOnDataErrors
        {
            get => _decoratedBinding.ValidatesOnDataErrors;
            set => _decoratedBinding.ValidatesOnDataErrors = value;
        }

        /// <summary>
        /// True if a data error from INotifyDataErrorInfo source item should be considered a validation error.
        /// </summary>
        [DefaultValue(true)]
        public bool ValidatesOnNotifyDataErrors
        {
            get => _decoratedBinding.ValidatesOnNotifyDataErrors;
            set => _decoratedBinding.ValidatesOnNotifyDataErrors = value;
        }

        /// <summary>
        /// Value to use when source cannot provide a value </summary>
        /// <remarks>
        ///     Initialized to DependencyProperty.UnsetValue; if FallbackValue is not set, BindingExpression
        ///     will return target property's default when Binding cannot get a real value.
        /// </remarks>
        public object FallbackValue
        {
            get => _decoratedBinding.FallbackValue;
            set => _decoratedBinding.FallbackValue = value;
        }

        /// <summary>
        /// Format string used to convert the data to type String.
        /// </summary>
        /// <remarks>
        ///     This property is used when the target of the binding has type
        ///     String and no Converter is declared.  It is ignored in all other
        ///     cases.
        /// </remarks>
        [DefaultValue(null)]
        public string StringFormat
        {
            get => _decoratedBinding.StringFormat;
            set => _decoratedBinding.StringFormat = value;
        }

        /// <summary>
        /// Value used to represent "null" in the target property.
        /// </summary>
        public object TargetNullValue
        {
            get => _decoratedBinding.TargetNullValue;
            set => _decoratedBinding.TargetNullValue = value;
        }

        /// <summary>
        /// Name of the <see cref="BindingGroup"/> this binding should join.
        /// </summary>
        [DefaultValue("")]
        public string BindingGroupName
        {
            get => _decoratedBinding.BindingGroupName;
            set => _decoratedBinding.BindingGroupName = value;
        }

        private void BuildExpression()
        {
            var holder = Build();
            
            SetExpression(holder);
            RemoveGeneratedBindings(holder);
            AppendGeneratedBindings(holder);
        }

        private ExpressionHolder Build()
        {
            return _builder.Build(_expression);
        }

        private void SetExpression(ExpressionHolder holder)
        {
            _scriptConverter.SetExpression(holder);
        }

        private void RemoveGeneratedBindings(ExpressionHolder holder)
        {
            foreach (var forceBinding in _decoratedBinding.Bindings.OfType<GeneratedBinding>().ToList())
            {
                _decoratedBinding.Bindings.Remove(forceBinding);
            }
        }

        private void AppendGeneratedBindings(ExpressionHolder holder)
        {
            foreach (var forceBinding in holder.GeneratedBindings)
            {
                _decoratedBinding.Bindings.Add(forceBinding);
            }
        }
    }
}
