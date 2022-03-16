using System.Collections.Generic;
using ScriptBinding.Internals.Compiler;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Internals
{
    class ExpressionHolder : IBindingGenerator
    {
        public ExpressionHolder()
        {
            GeneratedBindings = new List<GeneratedBinding>();
        }

        public Expr Expression { get; private set; }
        public List<GeneratedBinding> GeneratedBindings { get; }

        public void SetExpression(Expr expression)
        {
            Expression = expression;
        }

        #region Implementation of IBindingGenerator

        /// <inheritdoc />
        void IBindingGenerator.GenerateBinding(string propertyPath)
        {
            var binding = new GeneratedBinding(propertyPath);
            GeneratedBindings.Add(binding);
        }

        /// <inheritdoc />
        void IBindingGenerator.GenerateBinding(string propertyPath, string elementName)
        {
            var binding = new GeneratedBinding(propertyPath, elementName);
            GeneratedBindings.Add(binding);
        }

        #endregion
    }
}