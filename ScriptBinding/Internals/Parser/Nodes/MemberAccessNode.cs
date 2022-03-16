using System.Collections.Generic;
using JetBrains.Annotations;

namespace ScriptBinding.Internals.Parser.Nodes
{
    sealed class MemberAccessNode : Node
    {
        [NotNull]
        public IReadOnlyList<Node> Members { get; }

        /// <inheritdoc />
        public MemberAccessNode(int start, int end, [NotNull] IReadOnlyList<Node> members)
            : base(start, end)
        {
            Members = members;
        }

        #region Overrides of Node

        /// <inheritdoc />
        public override T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.VisitMemberAccess(this);
        }

        #endregion
    }
}
