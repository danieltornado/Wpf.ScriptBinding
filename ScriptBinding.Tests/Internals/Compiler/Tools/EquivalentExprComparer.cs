using System;
using System.Collections.Generic;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler.Tools
{
    class EquivalentExprComparer : IEqualityComparer<Expr>
    {
        private IEqualityComparer<Expr> ExplicitThis => this;

        #region Implementation of IEqualityComparer<in T>

        /// <inheritdoc />
        bool IEqualityComparer<Expr>.Equals(Expr x, Expr y)
        {
            return this.Equals((dynamic)x, (dynamic)y);
        }

        /// <inheritdoc />
        int IEqualityComparer<Expr>.GetHashCode(Expr obj)
        {
            return this.GetHashCode((dynamic)obj);
        }

        #endregion

        private bool Equals(Binary x, Binary y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.OperationType == y.OperationType
                && ExplicitThis.Equals(x.Argument1, y.Argument1)
                && ExplicitThis.Equals(x.Argument2, y.Argument2);
        }

        private bool Equals(CallBinding x, CallBinding y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.Index == y.Index;
        }

        private bool Equals(CallDynamicMethod x, CallDynamicMethod y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.MethodName == y.MethodName
                && ExplicitThis.Equals(x.Target, y.Target)
                && CollectionAreEquals(x.Parameters, y.Parameters);
        }

        private bool Equals(CallDynamicProperty x, CallDynamicProperty y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.PropertyName == y.PropertyName
                && ExplicitThis.Equals(x.Target, y.Target);
        }

        private bool Equals(CallElementBinding x, CallElementBinding y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.PropertyPath == y.PropertyPath
                && x.ElementName == y.ElementName;
        }

        private bool Equals(CallMethod x, CallMethod y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.Method == y.Method
                && ExplicitThis.Equals(x.Target, y.Target)
                && CollectionAreEquals(x.Parameters, y.Parameters);
        }

        private bool Equals(CallProperty x, CallProperty y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.Property == y.Property
                && ExplicitThis.Equals(x.Target, y.Target);
        }

        private bool Equals(CallPropertyBinding x, CallPropertyBinding y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.PropertyPath == y.PropertyPath;
        }

        private bool Equals(CallType x, CallType y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.Type == y.Type;
        }

        private bool Equals(Conditional x, Conditional y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && ExplicitThis.Equals(x.If, y.If)
                && ExplicitThis.Equals(x.Then, y.Then)
                && ExplicitThis.Equals(x.Else, y.Else);
        }

        private bool Equals(ConstantBoolean x, ConstantBoolean y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.Value == y.Value;
        }

        private bool Equals(ConstantNull x, ConstantNull y)
        {
            return x.Start == y.Start
                && x.End == y.End;
        }

        private bool Equals(ConstantNumber x, ConstantNumber y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && Equals(x.Value, y.Value);
        }

        private bool Equals(ConstantString x, ConstantString y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.Value == y.Value;
        }

        private bool Equals(Parens x, Parens y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && ExplicitThis.Equals(x.Expression, y.Expression);
        }

        private bool Equals(Unary x, Unary y)
        {
            return x.Start == y.Start
                && x.End == y.End
                && x.OperationType == y.OperationType
                && ExplicitThis.Equals(x.Argument, y.Argument);
        }

        private bool CollectionAreEquals(IReadOnlyList<Expr> x, IReadOnlyList<Expr> y)
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

        private bool Equals(Expr x, Expr y)
        {
            throw new NotSupportedException();
        }

        private int GetHashCode(Binary obj)
        {
            return new { obj.Start, obj.End, obj.OperationType }.GetHashCode();
        }

        private int GetHashCode(CallBinding obj)
        {
            return new { obj.Start, obj.End, obj.Index }.GetHashCode();
        }

        private int GetHashCode(CallDynamicMethod obj)
        {
            return new { obj.Start, obj.End, obj.MethodName }.GetHashCode();
        }

        private int GetHashCode(CallDynamicProperty obj)
        {
            return new { obj.Start, obj.End, obj.PropertyName }.GetHashCode();
        }

        private int GetHashCode(CallElementBinding obj)
        {
            return new { obj.Start, obj.End, obj.PropertyPath, obj.ElementName }.GetHashCode();
        }

        private int GetHashCode(CallMethod obj)
        {
            return new { obj.Start, obj.End }.GetHashCode();
        }

        private int GetHashCode(CallProperty obj)
        {
            return new { obj.Start, obj.End }.GetHashCode();
        }

        private int GetHashCode(CallPropertyBinding obj)
        {
            return new { obj.Start, obj.End, obj.PropertyPath }.GetHashCode();
        }

        private int GetHashCode(CallType obj)
        {
            return new { obj.Start, obj.End }.GetHashCode();
        }

        private int GetHashCode(Conditional obj)
        {
            return new { obj.Start, obj.End }.GetHashCode();
        }

        private int GetHashCode(ConstantBoolean obj)
        {
            return new { obj.Start, obj.End, obj.Value }.GetHashCode();
        }

        private int GetHashCode(ConstantNull obj)
        {
            return new { obj.Start, obj.End }.GetHashCode();
        }

        private int GetHashCode(ConstantNumber obj)
        {
            return new { obj.Start, obj.End, obj.Value }.GetHashCode();
        }

        private int GetHashCode(ConstantString obj)
        {
            return new { obj.Start, obj.End, obj.Value }.GetHashCode();
        }

        private int GetHashCode(Parens obj)
        {
            return new { obj.Start, obj.End }.GetHashCode();
        }

        private int GetHashCode(Unary obj)
        {
            return new { obj.Start, obj.End, obj.OperationType }.GetHashCode();
        }

        private int GetHashCode(Expr obj)
        {
            throw new NotSupportedException();
        }
    }
}
