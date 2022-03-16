using JetBrains.Annotations;

namespace ScriptBinding.Internals.Parser.Nodes
{
    sealed class BinaryNode : Node
    {
        [NotNull]
        public Node Argument1 { get; }

        [NotNull]
        public Node Argument2 { get; }

        public BinaryOperation OperationType { get; }

        /// <inheritdoc />
        public BinaryNode(int start, int end, [NotNull] Node argument1, [NotNull] Node argument2, BinaryOperation operationType)
            : base(start, end)
        {
            Argument1 = argument1;
            Argument2 = argument2;
            OperationType = operationType;
        }

        #region Overrides of Node

        /// <inheritdoc />
        public override T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.VisitBinary(this);
        }

        #endregion
    }
}
