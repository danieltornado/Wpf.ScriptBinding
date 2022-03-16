namespace ScriptBinding.Internals.Parser.Nodes
{
    abstract class Node
    {
        public int Start { get; }
        public int End { get; }

        protected Node(int start, int end)
        {
            Start = start;
            End = end;
        }

        public abstract T Accept<T>(INodeVisitor<T> visitor);
    }
}
