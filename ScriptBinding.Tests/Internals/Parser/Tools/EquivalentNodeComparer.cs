using System;
using System.Collections.Generic;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Tests.Internals.Parser.Tools
{
    sealed class EquivalentNodeComparer : IEqualityComparer<Node>
    {
        private IEqualityComparer<Node> ExplicitThis => this;

        #region Implementation of IEqualityComparer<in T>

        /// <inheritdoc />
        bool IEqualityComparer<Node>.Equals(Node x, Node y)
        {
            return this.Equals((dynamic)x, (dynamic)y);
        }

        /// <inheritdoc />
        int IEqualityComparer<Node>.GetHashCode(Node obj)
        {
            return this.GetHashCode((dynamic)obj);
        }

        #endregion

        private bool Equals(BinaryNode x, BinaryNode y)
        {
            return x.Start == y.Start 
                && x.End == y.End 
                && x.OperationType == y.OperationType
                && ExplicitThis.Equals(x.Argument1, y.Argument1)
                && ExplicitThis.Equals(x.Argument2, y.Argument2);
        }

        private bool Equals(IdentifierNode x, IdentifierNode y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.Name == y.Name;
        }

        private bool Equals(RealNode x, RealNode y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.Value == y.Value
                && x.Modifier == y.Modifier;
        }

        private bool Equals(ParensNode x, ParensNode y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && ExplicitThis.Equals(x.Statement, y.Statement);
        }

        private bool Equals(IntegerNode x, IntegerNode y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.Value == y.Value
                && x.Modifier == y.Modifier;
        }

        private bool Equals(ConditionalNode x, ConditionalNode y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && ExplicitThis.Equals(x.If, y.If)
                && ExplicitThis.Equals(x.Then, y.Then)
                && ExplicitThis.Equals(x.Else, y.Else);
        }

        private bool Equals(MemberAccessNode x, MemberAccessNode y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && CollectionAreEquals(x.Members, y.Members);
        }

        private bool Equals(InvokeNode x, InvokeNode y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && ExplicitThis.Equals(x.Identifier, y.Identifier)
                && CollectionAreEquals(x.Parameters, y.Parameters);
        }

        private bool Equals(StringNode x, StringNode y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.Text == y.Text;
        }

        private bool Equals(UnaryNode x, UnaryNode y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.OperationType == y.OperationType
                && ExplicitThis.Equals(x.Argument, y.Argument);
        }

        private bool CollectionAreEquals(IReadOnlyList<Node> x, IReadOnlyList<Node> y)
        {
            if (x == null && y == null)
                return true;

            if (x != null && y != null)
            {
                if (x.Count == y.Count)
                {
                    for (int i = 0; i < x.Count; i++)
                    {
                        if (!ExplicitThis.Equals(x[i], y[i]))
                            return false;
                    }

                    return true;
                }
            }

            return false;
        }

        private bool Equals(Node x, Node y)
        {
            throw new NotSupportedException();
        }

        private int GetHashCode(BinaryNode obj)
        {
            return new { obj.Start, obj.End, obj.OperationType }.GetHashCode();
        }

        private int GetHashCode(IdentifierNode obj)
        {
            return new { obj.Start, obj.End, obj.Name }.GetHashCode();
        }

        private int GetHashCode(RealNode obj)
        {
            return new { obj.Start, obj.End, obj.Value, obj.Modifier }.GetHashCode();
        }

        private int GetHashCode(ParensNode obj)
        {
            return new { obj.Start, obj.End }.GetHashCode();
        }

        private int GetHashCode(IntegerNode obj)
        {
            return new { obj.Start, obj.End, obj.Value, obj.Modifier }.GetHashCode();
        }

        private int GetHashCode(ConditionalNode obj)
        {
            return new { obj.Start, obj.End }.GetHashCode();
        }

        private int GetHashCode(MemberAccessNode obj)
        {
            return new { obj.Start, obj.End }.GetHashCode();
        }

        private int GetHashCode(InvokeNode obj)
        {
            return new { obj.Start, obj.End }.GetHashCode();
        }

        private int GetHashCode(StringNode obj)
        {
            return new { obj.Start, obj.End, obj.Text }.GetHashCode();
        }

        private int GetHashCode(UnaryNode obj)
        {
            return new { obj.Start, obj.End, obj.OperationType }.GetHashCode();
        }

        private int GetHashCode(Node obj)
        {
            throw new NotSupportedException();
        }
    }
}
