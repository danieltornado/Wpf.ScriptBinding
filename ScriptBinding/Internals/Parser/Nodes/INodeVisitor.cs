namespace ScriptBinding.Internals.Parser.Nodes
{
    interface INodeVisitor<out T>
    {
        T VisitIdentifier(IdentifierNode node);
        T VisitInvoke(InvokeNode node);
        T VisitMemberAccess(MemberAccessNode node);
        T VisitString(StringNode node);
        T VisitConditional(ConditionalNode node);
        T VisitInteger(IntegerNode node);
        T VisitReal(RealNode node);
        T VisitBinary(BinaryNode node);
        T VisitParens(ParensNode node);
        T VisitUnary(UnaryNode node);
    }
}
