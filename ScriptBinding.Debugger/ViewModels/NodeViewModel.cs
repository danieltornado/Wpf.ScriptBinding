using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Debugger.ViewModels
{
    class NodeViewModel : ViewModelBase
    {
        public NodeViewModel(Node node)
        {
            switch (node)
            {
                case null:
                    Display = "null";
                    break;
                case BinaryNode binaryNode:
                    Display = $"{node.GetType().Name}: {ConvertBinaryOperation(binaryNode.OperationType)}";

                    AddChild(binaryNode.Argument1);
                    AddChild(binaryNode.Argument2);
                    break;
                case ConditionalNode conditionalNode:
                    Display = $"{node.GetType().Name}";

                    AddChild(conditionalNode.If);
                    AddChild(conditionalNode.Then);
                    AddChild(conditionalNode.Else);
                    break;
                case IdentifierNode identifierNode:
                    Display = $"{node.GetType().Name}: {identifierNode.Name}";
                    break;
                case IntegerNode integerNode:
                    Display = $"{node.GetType().Name}: {integerNode.Value}";
                    break;
                case InvokeNode invokeNode:
                    Display = $"{node.GetType().Name}";

                    AddChild(invokeNode.Identifier);

                    foreach (var parameter in invokeNode.Parameters)
                    {
                        AddChild(parameter);
                    }

                    break;
                case MemberAccessNode memberAccessNode:
                    Display = $"{node.GetType().Name}";

                    foreach (var parameter in memberAccessNode.Members)
                    {
                        AddChild(parameter);
                    }

                    break;
                case ParensNode parensNode:
                    Display = $"{node.GetType().Name}";

                    AddChild(parensNode.Statement);

                    break;
                case RealNode realNode:
                    Display = $"{node.GetType().Name}: {realNode.Value}";
                    break;
                case StringNode stringNode:
                    Display = $"{node.GetType().Name}: {stringNode.Text}";
                    break;
                case UnaryNode unaryNode:
                    Display = $"{node.GetType().Name}: {ConvertUnaryOperation(unaryNode.OperationType)}";

                    AddChild(unaryNode.Argument);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(node));
            }
        }

        private NodeViewModel(string display)
        {
            Display = display;
        }

        public string Display { get; }
        
        public ObservableCollection<NodeViewModel> Children { get; private set; }

        private void AddChild(Node node)
        {
            if (Children == null)
                Children = new ObservableCollection<NodeViewModel>();

            Children.Add(new NodeViewModel(node));
        }

        private string ConvertBinaryOperation(BinaryOperation operation)
        {
            switch (operation)
            {
                case BinaryOperation.Plus:
                    return "+";
                case BinaryOperation.Minus:
                    return "-";
                case BinaryOperation.Multiply:
                    return "*";
                case BinaryOperation.Divide:
                    return "/";
                case BinaryOperation.Mod:
                    return "%";
                case BinaryOperation.And:
                    return "AND";
                case BinaryOperation.Or:
                    return "OR";
                case BinaryOperation.Greater:
                    return ">";
                case BinaryOperation.GreaterOrEquals:
                    return ">=";
                case BinaryOperation.Less:
                    return "<";
                case BinaryOperation.LessOrEquals:
                    return "<=";
                case BinaryOperation.Equals:
                    return "==";
                case BinaryOperation.NotEquals:
                    return "<>";
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }
        }

        private string ConvertUnaryOperation(UnaryOperation operation)
        {
            switch (operation)
            {
                case UnaryOperation.Not:
                    return "not";
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
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
