using JetBrains.Annotations;

namespace ScriptBinding.Internals.Parser.Nodes
{
    sealed class IntegerNode : Node
    {
        [NotNull]
        public string Value { get; }

        public IntegerModifiers Modifier { get; }

        /// <inheritdoc />
        public IntegerNode(int start, int end, [NotNull] string value)
            : base(start, end)
        {
            Value = value;
            Modifier = IntegerModifiers.None;
        }

        /// <inheritdoc />
        public IntegerNode(int start, int end, [NotNull] string value, IntegerModifiers modifier)
            : base(start, end)
        {
            Value = value;
            Modifier = modifier;
        }

        public bool TryGetInt(out int value)
        {
            return int.TryParse(Value, out value);
        }

        #region Overrides of Node

        /// <inheritdoc />
        public override T Accept<T>(INodeVisitor<T> visitor)
        {
            return visitor.VisitInteger(this);
        }

        #endregion
    }
}
