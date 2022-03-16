using JetBrains.Annotations;

namespace ScriptBinding.Internals.Parser.Nodes
{
    class UnaryNode : Node
    {
        [NotNull]
        public Node Argument { get; }

        public UnaryOperation OperationType { get; }

        /// <inheritdoc />
        public UnaryNode(int start, int end, [NotNull] Node argument, UnaryOperation operationType)
            : base(start, end)
        {
            Argument = argument;
            OperationType = operationType;
        }

        #region Overrides of Node

        /// <inheritdoc />
        public override T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.VisitUnary(this);
        }

        #endregion
    }
}
