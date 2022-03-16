using JetBrains.Annotations;

namespace ScriptBinding.Internals.Parser.Nodes
{
    sealed class StringNode : Node
    {
        [NotNull]
        public string Text { get; }

        /// <inheritdoc />
        public StringNode(int start, int end, [NotNull] string text)
            : base(start, end)
        {
            Text = text;
        }

        #region Overrides of Node

        /// <inheritdoc />
        public override T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.VisitString(this);
        }

        #endregion
    }
}
