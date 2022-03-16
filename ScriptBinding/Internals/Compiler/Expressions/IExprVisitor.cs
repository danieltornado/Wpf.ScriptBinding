namespace ScriptBinding.Internals.Compiler.Expressions
{
    interface IExprVisitor<out T>
    {
        T VisitBinary(Binary expression);
        T VisitCallBinding(CallBinding expression);
        T VisitCallDynamicMethod(CallDynamicMethod expression);
        T VisitCallDynamicProperty(CallDynamicProperty expression);
        T VisitCallElementBinding(CallElementBinding expression);
        T VisitCallEnum(CallEnum expression);
        T VisitCallMethod(CallMethod expression);
        T VisitCallProperty(CallProperty expression);
        T VisitCallPropertyBinding(CallPropertyBinding expression);
        T VisitCallType(CallType expression);
        T VisitConditional(Conditional expression);
        T VisitConstantBoolean(ConstantBoolean expression);
        T VisitConstantNull(ConstantNull expression);
        T VisitConstantNumber(ConstantNumber expression);
        T VisitConstantString(ConstantString expression);
        T VisitFailed(Failed expression);
        T VisitParens(Parens expression);
        T VisitUnary(Unary expression);
    }
}
