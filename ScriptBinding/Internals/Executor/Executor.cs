using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using ScriptBinding.Internals.Common;
using ScriptBinding.Internals.Compiler.Expressions;
using ScriptBinding.Internals.Executor.ErrorListeners;

namespace ScriptBinding.Internals.Executor
{
    sealed class Executor : IExprVisitor<object>
    {
        private readonly IExecutingErrorListener _listener;
        private readonly IBindingProvider _bindingProvider;

        public Executor([NotNull] IExecutingErrorListener listener, [NotNull] IBindingProvider bindingProvider)
        {
            _listener = listener;
            _bindingProvider = bindingProvider;
        }

        public object Execute(Expr expression)
        {
            return expression.Accept(this);
        }

        #region Implementation of IExprVisitor<out object>

        /// <inheritdoc />
        object IExprVisitor<object>.VisitBinary(Binary expression)
        {
            switch (expression.OperationType)
            {
                case BinaryType.Plus:
                    return CalculateBinaryOperation(expression.Argument1, expression.Argument2, expression, Sum);
                case BinaryType.Minus:
                    return CalculateBinaryOperation(expression.Argument1, expression.Argument2, expression, Subtract);
                case BinaryType.Multiply:
                    return CalculateBinaryOperation(expression.Argument1, expression.Argument2, expression, Multiply);
                case BinaryType.Divide:
                    return CalculateBinaryOperation(expression.Argument1, expression.Argument2, expression, Divide);
                case BinaryType.Mod:
                    return CalculateBinaryOperation(expression.Argument1, expression.Argument2, expression, Mod);

                case BinaryType.And:
                    return CalculateBinaryOperation(expression.Argument1, expression.Argument2, expression, And);
                case BinaryType.Or:
                    return CalculateBinaryOperation(expression.Argument1, expression.Argument2, expression, Or);

                case BinaryType.Greater:
                    return CalculateBinaryOperation(expression.Argument1, expression.Argument2, expression, Greater);
                case BinaryType.GreaterOrEquals:
                    return CalculateBinaryOperation(expression.Argument1, expression.Argument2, expression, GreaterOrEquals);
                case BinaryType.Less:
                    return CalculateBinaryOperation(expression.Argument1, expression.Argument2, expression, Less);
                case BinaryType.LessOrEquals:
                    return CalculateBinaryOperation(expression.Argument1, expression.Argument2, expression, LessOrEquals);
                case BinaryType.Equals:
                    return CalculateBinaryOperation(expression.Argument1, expression.Argument2, expression, IsEquals);
                case BinaryType.NotEquals:
                    return CalculateBinaryOperation(expression.Argument1, expression.Argument2, expression, NotEquals);
                    
                default:
                    _listener.Error(expression.Start, expression.End, "Unknown binary operation type", null);
                    return null;
            }
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitCallBinding(CallBinding expression)
        {
            if (_bindingProvider.TryGetValue(expression.Index, out var result))
                return result;

            _listener.Error(expression.Start, expression.End, $"Binding[{expression.Index}] not found", null);
            return null;
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitCallDynamicMethod(CallDynamicMethod expression)
        {
            var target = expression.Target.Accept(this);
            if (target == null)
            {
                // Can't get target type, so can't get method
                return null;
            }

            var parameterValues = expression.Parameters.Select(e => e.Accept(this)).ToArray();
            var typedParameters = parameterValues.Select(e => e?.GetType()).ToArray();

            if (CommonHelper.TryGetMethod(expression.MethodName, typedParameters, target.GetType(), false, out MethodInfo method))
                return InvokeMethod(expression, method, target, parameterValues);

            _listener.Error(expression.Start, expression.End, "Method not found", null);
            return null;
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitCallDynamicProperty(CallDynamicProperty expression)
        {
            var target = expression.Target.Accept(this);
            if (target == null)
            {
                // Can't get target type, so can't get property
                return null;
            }

            if (CommonHelper.TryGetProperty(expression.PropertyName, target.GetType(), false, out PropertyInfo property))
                return InvokeProperty(expression, property, target);

            _listener.Error(expression.Start, expression.End, "Property not found", null);
            return null;
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitCallElementBinding(CallElementBinding expression)
        {
            if (_bindingProvider.TryGetValue(expression.PropertyPath, expression.ElementName, out var result))
                return result;

            _listener.Error(expression.Start, expression.End, $"'Binding path={expression.PropertyPath}, elementName={expression.ElementName}' not found", null);
            return null;
        }

        /// <inheritdoc />
        public object VisitCallEnum(CallEnum expression)
        {
            return expression.Value;
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitCallMethod(CallMethod expression)
        {
            object target = null; // null - default value for static invocation
            
            if (expression.Target is CallType)
            {
                // static invocation
            }
            else
            {
                target = expression.Target.Accept(this);
            }

            object[] parameters = PrepareInvocationParameters(expression.Parameters);

            return InvokeMethod(expression, expression.Method, target, parameters);
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitCallProperty(CallProperty expression)
        {
            object target = null; // null - default value for static invocation

            if (expression.Target is CallType)
            {
                // static invocation
            }
            else
            {
                target = expression.Target.Accept(this);
            }

            return InvokeProperty(expression, expression.Property, target);
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitCallPropertyBinding(CallPropertyBinding expression)
        {
            if (_bindingProvider.TryGetValue(expression.PropertyPath, out var result))
                return result;
            
            _listener.Error(expression.Start, expression.End, $"'Binding path={expression.PropertyPath}' not found", null);
            return null;
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitCallType(CallType expression)
        {
            // Do not process node

            _listener.Error(expression.Start, expression.End, "Unexpected expression", null);
            return null;
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitConditional(Conditional expression)
        {
            object resultIf = expression.If.Accept(this);
            if (resultIf is bool resultBool)
            {
                if (resultBool)
                    return expression.Then.Accept(this);
                
                return expression.Else.Accept(this);
            }
            
            _listener.Error(expression.Start, expression.End, "Expression 'if' returned non-boolean value", null);
            return null;
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitConstantBoolean(ConstantBoolean expression)
        {
            return expression.Value;
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitConstantNull(ConstantNull expression)
        {
            return null;
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitConstantNumber(ConstantNumber expression)
        {
            return expression.Value;
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitConstantString(ConstantString expression)
        {
            return expression.Value;
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitFailed(Failed expression)
        {
            _listener.Error(expression.Start, expression.End, expression.Message, null);
            return null;
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitParens(Parens expression)
        {
            return expression.Expression.Accept(this);
        }

        /// <inheritdoc />
        object IExprVisitor<object>.VisitUnary(Unary expression)
        {
            switch (expression.OperationType)
            {
                case UnaryType.Not:
                    return CalculateUnaryOperation(expression.Argument, expression, Not);
                default:
                    _listener.Error(expression.Start, expression.End, "Unknown unary operation type", null);
                    return null;
            }
        }

        #endregion

        private object[] PrepareInvocationParameters(IReadOnlyList<Expr> parameters)
        {
            object[] result = new object[parameters.Count];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = parameters[i].Accept(this);
            }

            return result;
        }

        private object InvokeMethod(Expr targetNode, MethodInfo method, object target, object[] parameters)
        {
            object result;
            try
            {
                result = method.Invoke(target, parameters);
            }
            catch (Exception e)
            {
                _listener.Error(targetNode.Start, targetNode.End, "Invocation exception", e);
                result = null;
            }

            return result;
        }

        private object InvokeProperty(Expr targetNode, PropertyInfo property, object target)
        {
            object result;
            try
            {
                result = property.GetValue(target);
            }
            catch (Exception e)
            {
                _listener.Error(targetNode.Start, targetNode.End, "Invocation exception", e);
                result = null;
            }

            return result;
        }

        private object CalculateBinaryOperation(Expr argument1, Expr argument2, Expr source, Func<object, object, object> calculateFunc)
        {
            object result1 = argument1.Accept(this);
            object result2 = argument2.Accept(this);
            
            try
            {
                return calculateFunc(result1, result2);
            }
            catch (Exception e)
            {
                _listener.Error(source.Start, source.End, "Cannot calculate binary operation", e);
                return null;
            }
        }

        private object CalculateUnaryOperation(Expr argument, Expr source, Func<object, object> calculateFunc)
        {
            object result = argument.Accept(this);
            
            try
            {
                return calculateFunc(result);
            }
            catch (Exception e)
            {
                _listener.Error(source.Start, source.End, "Cannot calculate unary operation", e);
                return null;
            }
        }

        private object Sum(object argument1, object argument2)
        {
            dynamic dynamic1 = argument1;
            dynamic dynamic2 = argument2;

            return dynamic1 + dynamic2;
        }

        private object Subtract(object argument1, object argument2)
        {
            dynamic dynamic1 = argument1;
            dynamic dynamic2 = argument2;

            return dynamic1 - dynamic2;
        }

        private object Multiply(object argument1, object argument2)
        {
            dynamic dynamic1 = argument1;
            dynamic dynamic2 = argument2;

            return dynamic1 * dynamic2;
        }

        private object Divide(object argument1, object argument2)
        {
            dynamic dynamic1 = argument1;
            dynamic dynamic2 = argument2;

            return dynamic1 / dynamic2;
        }

        private object Mod(object argument1, object argument2)
        {
            dynamic dynamic1 = argument1;
            dynamic dynamic2 = argument2;

            return dynamic1 % dynamic2;
        }

        private object And(object argument1, object argument2)
        {
            if (argument1 is bool value1 && argument2 is bool value2)
                return value1 && value2;

            throw new ArgumentException("Boolean values expected");
        }

        private object Or(object argument1, object argument2)
        {
            if (argument1 is bool value1 && argument2 is bool value2)
                return value1 || value2;

            throw new ArgumentException("Boolean values expected");
        }

        private object Greater(object argument1, object argument2)
        {
            dynamic dynamic1 = argument1;
            dynamic dynamic2 = argument2;

            return dynamic1 > dynamic2;
        }

        private object GreaterOrEquals(object argument1, object argument2)
        {
            dynamic dynamic1 = argument1;
            dynamic dynamic2 = argument2;

            return dynamic1 >= dynamic2;
        }

        private object Less(object argument1, object argument2)
        {
            dynamic dynamic1 = argument1;
            dynamic dynamic2 = argument2;

            return dynamic1 < dynamic2;
        }

        private object LessOrEquals(object argument1, object argument2)
        {
            dynamic dynamic1 = argument1;
            dynamic dynamic2 = argument2;

            return dynamic1 <= dynamic2;
        }

        private object IsEquals(object argument1, object argument2)
        {
            dynamic dynamic1 = argument1;
            dynamic dynamic2 = argument2;

            return dynamic1 == dynamic2;
        }

        private object NotEquals(object argument1, object argument2)
        {
            dynamic dynamic1 = argument1;
            dynamic dynamic2 = argument2;

            return dynamic1 != dynamic2;
        }

        private object Not(object argument)
        {
            if (argument is bool value)
                return value == false;

            throw new ArgumentException("Boolean value expected");
        }
    }
}
