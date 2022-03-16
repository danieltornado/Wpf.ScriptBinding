using JetBrains.Annotations;

namespace ScriptBinding.Internals.Parser.Nodes
{
    sealed class RealNode : Node
    {
        [NotNull]
        public string Value { get; }
        
        public RealModifiers Modifier { get; }

        /// <inheritdoc />
        public RealNode(int start, int end, [NotNull] string value)
            : base(start, end)
        {
            Value = value;
            Modifier = RealModifiers.None;
        }

        /// <inheritdoc />
        public RealNode(int start, int end, [NotNull] string value, RealModifiers modifier)
            : base(start, end)
        {
            Value = value;
            Modifier = modifier;
        }

        #region Overrides of Node

        /// <inheritdoc />
        public override T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.VisitReal(this);
        }

        #endregion
    }
}
