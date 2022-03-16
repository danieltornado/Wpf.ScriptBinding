using System;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class CallPropertyBinding : Expr
    {
        [NotNull]
        public string PropertyPath { get; }

        /// <inheritdoc />
        public CallPropertyBinding(int start, int end, [NotNull] string propertyPath)
            : base(start, end)
        {
            PropertyPath = propertyPath;
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
            return visitor.VisitCallPropertyBinding(this);
        }

        #endregion
    }
}
