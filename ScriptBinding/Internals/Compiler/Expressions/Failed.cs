using System;

namespace ScriptBinding.Internals.Compiler.Expressions
{
    sealed class Failed : Expr
    {
        public string Message { get; }

        /// <inheritdoc />
        public Failed(int start, int end, string message)
            : base(start, end)
        {
            Message = message;
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
            return visitor.VisitFailed(this);
        }

        #endregion
    }
}
