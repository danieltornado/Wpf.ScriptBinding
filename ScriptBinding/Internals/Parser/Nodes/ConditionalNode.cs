using Antlr4.Runtime.Misc;

namespace ScriptBinding.Internals.Parser.Nodes
{
    sealed class ConditionalNode : Node
    {
        [NotNull]
        public Node If { get; }

        [NotNull]
        public Node Then { get; }

        [NotNull]
        public Node Else { get; }

        /// <inheritdoc />
        public ConditionalNode(int start, int end, [NotNull] Node ifStatement, [NotNull] Node thenStatement, [NotNull] Node elseStatement)
            : base(start, end)
        {
            If = ifStatement;
            Then = thenStatement;
            Else = elseStatement;
        }

        #region Overrides of Node

        /// <inheritdoc />
        public override T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.VisitConditional(this);
        }

        #endregion
    }
}
