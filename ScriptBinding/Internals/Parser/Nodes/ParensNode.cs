using JetBrains.Annotations;

namespace ScriptBinding.Internals.Parser.Nodes
{
    sealed class ParensNode : Node
    {
        [NotNull]
        public Node Statement { get; }

        /// <inheritdoc />
        public ParensNode(int start, int end, [NotNull] Node statement)
            : base(start, end)
        {
            Statement = statement;
        }

        #region Overrides of Node

        /// <inheritdoc />
        public override T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.VisitParens(this);
        }

        #endregion
    }
}
