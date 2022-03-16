using System;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class CallElementBinding : Expr
    {
        [NotNull]
        public string PropertyPath { get; }

        [NotNull]
        public string ElementName { get; }

        /// <inheritdoc />
        public CallElementBinding(int start, int end, [NotNull] string propertyPath, [NotNull] string elementName)
            : base(start, end)
        {
            PropertyPath = propertyPath;
            ElementName = elementName;
        }

        #region Overrides of Expr

        /// <inheritdoc />
        public override Type GetExpressionType()
        {
            return null;
        }

        /// <inheritdoc />
        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitCallElementBinding(this);
        }

        #endregion
    }
}
