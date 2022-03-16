using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Debugger.ViewModels
{
    class ExprViewModel : ViewModelBase
    {
        public ExprViewModel(Expr expression)
        {
            switch (expression)
            {
                case null:
                    Display = "null";
                    break;
                case Binary binary:
                    Display = $"{expression.GetType().Name}: {ConvertBinaryType(binary.OperationType)}";

                    AddChild(binary.Argument1);
                    AddChild(binary.Argument2);
                    break;
                case CallBinding callBinding:
                    Display = $"{expression.GetType().Name}: {callBinding.Index}";
                    break;
                case CallDynamicMethod callDynamicMethod:
                    Display = $"{expression.GetType().Name}: {callDynamicMethod.MethodName}";

                    AddChild(callDynamicMethod.Target);

                    foreach (var parameter in callDynamicMethod.Parameters)
                    {
                        AddChild(parameter);
                    }

                    break;
                case CallDynamicProperty callDynamicProperty:
                    Display = $"{expression.GetType().Name}: {callDynamicProperty.PropertyName}";

                    AddChild(callDynamicProperty.Target);
                    break;
                case CallElementBinding callElementBinding:
                    Display = $"{expression.GetType().Name}: {callElementBinding.PropertyPath}, {callElementBinding.ElementName}";
                    break;
                case CallMethod callMethod:
                    Display = $"{expression.GetType().Name}: {callMethod.Method.Name}; {callMethod.Method.ReturnType.FullName}";

                    AddChild(callMethod.Target);

                    foreach (var parameter in callMethod.Parameters)
                    {
                        AddChild(parameter);
                    }

                    break;
                case CallProperty callProperty:
                    Display = $"{expression.GetType().Name}: {callProperty.Property.Name}; {callProperty.Property.PropertyType.FullName}";

                    AddChild(callProperty.Target);
                    break;
                case CallPropertyBinding callPropertyBinding:
                    Display = $"{expression.GetType().Name}: {callPropertyBinding.PropertyPath}";
                    break;
                case CallType callType:
                    Display = $"{expression.GetType().Name}: {callType.Type.FullName}";
                    break;
                case Conditional conditional:
                    Display = $"{expression.GetType().Name}";

                    AddChild(conditional.If);
                    AddChild(conditional.Then);
                    AddChild(conditional.Else);
                    break;
                case ConstantBoolean constantBoolean:
                    Display = $"{expression.GetType().Name}: {constantBoolean.Value}";
                    break;
                case ConstantNumber constantNumber:
                    Display = $"{expression.GetType().Name}: {constantNumber.Value}";
                    break;
                case ConstantNull _:
                    Display = $"{expression.GetType().Name}";
                    break;
                case ConstantString constantString:
                    Display = $"{expression.GetType().Name}: {constantString.Value}";
                    break;
                case Failed failed:
                    Display = $"{expression.GetType().Name}: {failed.Message}";
                    break;
                case Parens parens:
                    Display = $"{expression.GetType().Name}";

                    AddChild(parens.Expression);
                    break;
                case Unary unary:
                    Display = $"{expression.GetType().Name}: {ConvertUnaryType(unary.OperationType)}";

                    AddChild(unary.Argument);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(expression));
            }
        }

        private ExprViewModel(string display)
        {
            Display = display;
        }

        public string Display { get; }

        public ObservableCollection<ExprViewModel> Children { get; private set; }

        private void AddChild(Expr expression)
        {
            if (Children == null)
                Children = new ObservableCollection<ExprViewModel>();

            Children.Add(new ExprViewModel(expression));
        }

        private string ConvertBinaryType(BinaryType type)
        {
            switch (type)
            {
                case BinaryType.Plus:
                    return "+";
                case BinaryType.Minus:
                    return "-";
                case BinaryType.Multiply:
                    return "*";
                case BinaryType.Divide:
                    return "/";
                case BinaryType.Mod:
                    return "%";
                case BinaryType.Or:
                    return "OR";
                case BinaryType.And:
                    return "AND";
                case BinaryType.Greater:
                    return ">";
                case BinaryType.GreaterOrEquals:
                    return ">=";
                case BinaryType.Less:
                    return "<";
                case BinaryType.LessOrEquals:
                    return "<=";
                case BinaryType.Equals:
                    return "==";
                case BinaryType.NotEquals:
                    return "<>";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private string ConvertUnaryType(UnaryType type)
        {
            switch (type)
            {
                case UnaryType.Not:
                    return "not";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        #region Overrides of Object

        /// <inheritdoc />
        public override string ToString()
        {
            return Display;
        }

        #endregion
    }
}
