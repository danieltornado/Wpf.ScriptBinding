using System.Collections.Generic;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Parser.Nodes
{
    sealed class InvokeNode : Node
    {
        [NotNull]
        public IdentifierNode Identifier { get; }

        [NotNull]
        public IReadOnlyList<Node> Parameters { get; }

        /// <inheritdoc />
        public InvokeNode(int start, int end, [NotNull] IdentifierNode identifier, [NotNull] IReadOnlyList<Node> parameters)
            : base(start, end)
        {
            Identifier = identifier;
            Parameters = parameters;
        }

        #region Overrides of Node

        /// <inheritdoc />
        public override T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.VisitInvoke(this);
        }

        #endregion
    }
}
