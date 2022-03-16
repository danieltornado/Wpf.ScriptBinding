using JetBrains.Annotations;

namespace ScriptBinding.Internals.Parser.Nodes
{
    sealed class IdentifierNode : Node
    {
        [NotNull]
        public string Name { get; }

        /// <inheritdoc />
        public IdentifierNode(int start, int end, [NotNull] string name)
            : base(start, end)
        {
            Name = name;
        }

        #region Overrides of Node

        /// <inheritdoc />
        public override T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.VisitIdentifier(this);
        }

        #endregion
    }
}
