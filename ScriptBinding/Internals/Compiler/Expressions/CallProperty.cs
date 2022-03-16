using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class CallProperty : Expr
    {
        [NotNull]
        public Expr Target { get; }

        [NotNull]
        public PropertyInfo Property { get; }

        /// <inheritdoc />
        public CallProperty(int start, int end, [NotNull] Expr target, [NotNull] PropertyInfo property)
            : base(start, end)
        {
            Target = target;
            Property = property;
        }

        #region Overrides of Expr

        /// <inheritdoc />
        public override Type GetExpressionType()
        {
            return Property.PropertyType;
        }

        /// <inheritdoc />
        public override T Accept<T>(IExprVisitor<T> visitor)
        {
            return visitor.VisitCallProperty(this);
        }

        #endregion
    }
}
