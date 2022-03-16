using System.Collections.Generic;
using ScriptBinding.Internals.Compiler;

namespace ScriptBinding.Tests.Internals.Compiler.Tools
{
    sealed class BindingGeneratorMock : IBindingGenerator
    {
        private readonly List<string> _propertyBindings = new List<string>();
        private readonly List<(string propertyPath, string elementName)> _elementBindings = new List<(string, string)>();
        
        public IReadOnlyList<string> PropertyBindings => _propertyBindings;
        public IReadOnlyList<(string propertyPath, string elementName)> ElementBindings => _elementBindings;

        #region Implementation of IBindingGenerator

        /// <inheritdoc />
        void IBindingGenerator.GenerateBinding(string propertyPath)
        {
            _propertyBindings.Add(propertyPath);
        }

        /// <inheritdoc />
        void IBindingGenerator.GenerateBinding(string propertyPath, string elementName)
        {
            _elementBindings.Add((propertyPath, elementName));
        }

        #endregion

        public BindingGeneratorMock AddBinding(string propertyPath)
        {
            _propertyBindings.Add(propertyPath);
            return this;
        }

        public BindingGeneratorMock AddBinding(string propertyPath, string elementName)
        {
            _elementBindings.Add((propertyPath, elementName));
            return this;
        }
    }
}
