using System;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class CallDynamicProperty : Expr
    {
        [NotNull]
        public Expr Target { get; }

        [NotNull]
        public string PropertyName { get; }

        /// <inheritdoc />
        public CallDynamicProperty(int start, int end, [NotNull] Expr target, [NotNull] string propertyName)
            : base(start, end)
        {
            Target = target;
            PropertyName = propertyName;
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
            return visitor.VisitCallDynamicProperty(this);
        }

        #endregion
    }
}
