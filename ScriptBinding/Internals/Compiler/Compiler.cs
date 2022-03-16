using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptBinding.Internals.Common;
using ScriptBinding.Internals.Compiler.Expressions;
using ScriptBinding.Internals.Parser.Nodes;

namespace ScriptBinding.Internals.Compiler
{
    sealed class Compiler : INodeVisitor<Expr>
    {
        private readonly IBindingGenerator _bindingGenerator;

        public Compiler(IBindingGenerator bindingGenerator)
        {
            _bindingGenerator = bindingGenerator;
        }

        public Expr Compile(Node expression)
        {
            var root = expression.Accept(this);
            return root;
        }

        #region Implementation of INodeVisitor<out Expr>

        /// <inheritdoc />
        Expr INodeVisitor<Expr>.VisitIdentifier(IdentifierNode node)
        {
            if (TryGetShortType(node.Name, out Type existingType))
                return new CallType(node.Start, node.End, existingType);

            switch (node.Name)
            {
                case "true":
                    return new ConstantBoolean(node.Start, node.End, true);
                case "false":
                    return new ConstantBoolean(node.Start, node.End, false);
                case "null":
                    return new ConstantNull(node.Start, node.End);
            }

            return new Failed(node.Start, node.End, "Type is not exist or unknown constant");
        }

        /// <inheritdoc />
        Expr INodeVisitor<Expr>.VisitInvoke(InvokeNode node)
        {
            // supports only binding functions
            return BuildBinding(node);
        }

        /// <inheritdoc />
        Expr INodeVisitor<Expr>.VisitMemberAccess(MemberAccessNode node)
        {
            Node firstNode = node.Members.First();

            switch (firstNode)
            {
                case InvokeNode invoke:
                    // Global context supports only binding functions
                    var dynamicExpr = BuildBinding(invoke);

                    if (dynamicExpr is Failed)
                        return dynamicExpr;

                    // Dynamic calling
                    foreach (var memberNode in node.Members.Skip(1))
                    {
                        dynamicExpr = BuildCalling(dynamicExpr, memberNode);
                    }

                    return dynamicExpr;
                case IdentifierNode identifier:
                    // Type expected
                    var typeStructure = BuildType(node, identifier, node.Members);
                    var staticExpr = typeStructure.expr;

                    if (staticExpr is Failed)
                        return staticExpr;

                    // Static-type calling
                    foreach (var memberNode in node.Members.Skip(typeStructure.count))
                    {
                        staticExpr = BuildCalling(staticExpr, memberNode);
                    }

                    return staticExpr;
                default:
                    // parensStatement expected
                    //return new Failed(firstNode.Start, firstNode.End, "Method or property expected");

                    Expr dynamicExpr2 = firstNode.Accept(this);

                    if (dynamicExpr2 is Failed)
                        return dynamicExpr2;

                    // Dynamic calling
                    foreach (var memberNode in node.Members.Skip(1))
                    {
                        dynamicExpr2 = BuildCalling(dynamicExpr2, memberNode);
                    }

                    return dynamicExpr2;
            }
        }

        /// <inheritdoc />
        Expr INodeVisitor<Expr>.VisitString(StringNode node)
        {
            return new ConstantString(node.Start, node.End, node.Text);
        }

        /// <inheritdoc />
        Expr INodeVisitor<Expr>.VisitConditional(ConditionalNode node)
        {
            var ifExpr = node.If.Accept(this);
            var thenExpr = node.Then.Accept(this);
            var elseExpr = node.Else.Accept(this);

            return new Conditional(node.Start, node.End, ifExpr, thenExpr, elseExpr);
        }

        /// <inheritdoc />
        Expr INodeVisitor<Expr>.VisitInteger(IntegerNode node)
        {
            if (CommonHelper.TryGetIntegerNumber(node.Value, node.Modifier, out object value))
                return new ConstantNumber(node.Start, node.End, value);

            return new Failed(node.Start, node.End, "Cannot parse to long value. Perhaps it is too big");
        }

        /// <inheritdoc />
        Expr INodeVisitor<Expr>.VisitReal(RealNode node)
        {
            if (CommonHelper.TryGetRealNumber(node.Value, node.Modifier, out object value))
                return new ConstantNumber(node.Start, node.End, value);

            return new Failed(node.Start, node.End, "Cannot parse to decimal value. Perhaps it is too big");
        }

        /// <inheritdoc />
        public Expr VisitBinary(BinaryNode node)
        {
            var argument1 = node.Argument1.Accept(this);
            var argument2 = node.Argument2.Accept(this);
            var binaryType = ConvertToBinaryType(node.OperationType);

            return new Binary(node.Start, node.End, argument1, argument2, binaryType);
        }

        /// <inheritdoc />
        public Expr VisitParens(ParensNode node)
        {
            Expr expression = node.Statement.Accept(this);
            return new Parens(node.Start, node.End, expression);
        }

        /// <inheritdoc />
        public Expr VisitUnary(UnaryNode node)
        {
            var argument = node.Argument.Accept(this);
            var unaryType = ConvertToUnaryType(node.OperationType);

            return new Unary(node.Start, node.End, argument, unaryType);
        }

        #endregion

        private Expr BuildBinding(InvokeNode invoke)
        {
            // Can create binding?
            if (invoke.Identifier.Name == "b")
            {
                // Can create binding?
                switch (invoke.Parameters.Count)
                {
                    case 1:
                        // CallBinding or CallPropertyBinding
                        return BuildBindingOrPropertyBinding(invoke, invoke.Parameters.First());
                    case 2:
                        // CallElementBinding
                        return BuildElementBinding(invoke, invoke.Parameters[0], invoke.Parameters[1]);
                    default:
                        return new Failed(invoke.Start, invoke.End, "Function 'b' can have only 1 or 2 parameters");
                }
            }

            return new Failed(invoke.Start, invoke.End, $"Unknown function name: {invoke.Identifier.Name}");
        }

        private Expr BuildBindingOrPropertyBinding(InvokeNode invoke, Node indexOrPropertyNode)
        {
            // Check parameters:
            switch (indexOrPropertyNode)
            {
                case IntegerNode integerNode:
                    if (integerNode.TryGetInt(out int index))
                        return new CallBinding(invoke.Start, invoke.End, index);

                    return new Failed(integerNode.Start, integerNode.End, "Cannot parse to int value. Perhaps it is too big");
                case MemberAccessNode memberAccessNode:
                    if (TryGetPropertyAccess(memberAccessNode.Members, out string propertyPath))
                    {
                        _bindingGenerator.GenerateBinding(propertyPath);
                        return new CallPropertyBinding(invoke.Start, invoke.End, propertyPath);
                    }

                    return new Failed(memberAccessNode.Start, memberAccessNode.End, "Property is expected");
                case IdentifierNode identifierNode:
                    _bindingGenerator.GenerateBinding(identifierNode.Name);
                    return new CallPropertyBinding(invoke.Start, invoke.End, identifierNode.Name);
                default:
                    return new Failed(indexOrPropertyNode.Start, indexOrPropertyNode.End, "Index or property is expected");
            }
        }

        private Expr BuildElementBinding(InvokeNode invoke, Node propertyNode, Node elementNameNode)
        {
            // Check parameters:
            string propertyPath;
            string elementName;

            switch (propertyNode)
            {
                case MemberAccessNode memberAccessNode:
                    if (TryGetPropertyAccess(memberAccessNode.Members, out propertyPath))
                    {
                        // good
                    }
                    else
                    {
                        return new Failed(memberAccessNode.Start, memberAccessNode.End, "Property is expected");
                    }
                    break;
                case IdentifierNode identifierNode:
                    propertyPath = identifierNode.Name;
                    break;
                default:
                    return new Failed(propertyNode.Start, propertyNode.End, "Property is expected");
            }

            switch (elementNameNode)
            {
                case StringNode stringNode:
                    elementName = stringNode.Text;
                    break;
                case IdentifierNode identifierNode:
                    elementName = identifierNode.Name;
                    break;
                default:
                    return new Failed(elementNameNode.Start, elementNameNode.End, "Element name is expected (string or identifier)");
            }

            // all is good
            _bindingGenerator.GenerateBinding(propertyPath, elementName);
            return new CallElementBinding(invoke.Start, invoke.End, propertyPath, elementName);
        }

        private bool TryGetPropertyAccess(IReadOnlyList<Node> access, out string propertyPath)
        {
            var firstNode = access.First();

            var propertyPathBuilder = new StringBuilder();

            if (firstNode is IdentifierNode identifierNode)
            {
                propertyPathBuilder.Append(identifierNode.Name);
            }
            else
            {
                propertyPath = null;
                return false;
            }

            foreach (var node in access.Skip(1))
            {
                if (node is IdentifierNode identifierNode2)
                {
                    propertyPathBuilder.Append(Type.Delimiter);
                    propertyPathBuilder.Append(identifierNode2.Name);
                }
                else
                {
                    propertyPath = null;
                    return false;
                }
            }

            propertyPath = propertyPathBuilder.ToString();
            return true;
        }

        private (Expr expr, int count) BuildType(Node parentNode, IdentifierNode firstMember, IReadOnlyList<Node> members)
        {
            var typeNameBuilder = new StringBuilder();
            typeNameBuilder.Append(firstMember.Name);

            if (TryGetType(typeNameBuilder.ToString(), out Type existingType))
                return Create(new CallType(firstMember.Start, firstMember.End, existingType), 1);

            for (int i = 1; i < members.Count; i++)
            {
                var member = members[i];

                // typeNameBuilder has not exist class
                if (!(member is IdentifierNode identifier))
                    return Create(new Failed(firstMember.Start, member.End, "Type is expected"), i + 1);

                typeNameBuilder.Append(Type.Delimiter);
                typeNameBuilder.Append(identifier.Name);

                if (TryGetType(typeNameBuilder.ToString(), out existingType))
                    return Create(new CallType(firstMember.Start, member.End, existingType), i + 1);
            }

            return Create(new Failed(parentNode.Start, parentNode.End, "Type is not exist"), members.Count);
        }

        private bool TryGetType(string typeName, out Type existingType)
        {
            // TODO: Implement optimization: lazy value for the domain's assemblies
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                existingType = assembly.GetType(typeName);
                if (existingType != null)
                    return true;
            }

            existingType = null;
            return false;
        }

        /// <summary>
        /// Gets types: byte, int, long, double, decimal...
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="existingType"></param>
        /// <returns></returns>
        private bool TryGetShortType(string typeName, out Type existingType)
        {
            return CommonHelper.ShortTypes.TryGetValue(typeName, out existingType);
        }

        private Expr BuildCalling(Expr target, Node member)
        {
            if (target is Failed)
                return target;

            Type targetType = target.GetExpressionType();
            switch (member)
            {
                case IdentifierNode identifier:
                    // Dynamic property access
                    if (targetType == null)
                    {
                        var expr = new CallDynamicProperty(target.Start, member.End, target, identifier.Name);
                        return expr;
                    }

                    // Enum access
                    if (CommonHelper.TryGetEnumValue(identifier.Name, targetType, out var enumValue))
                    {
                        var expr = new CallEnum(target.Start, member.End, enumValue);
                        return expr;
                    }

                    // Typed property access
                    if (CommonHelper.TryGetProperty(identifier.Name, targetType, target is CallType, out var property))
                    {
                        var expr = new CallProperty(target.Start, member.End, target, property);
                        return expr;
                    }

                    return new Failed(target.Start, member.End, $"Property was not found in the type: {targetType}");
                case InvokeNode invoke:
                    // Method access
                    var parameters = BuildInvokeParameters(invoke.Parameters);

                    if (targetType == null)
                    {
                        var expr = new CallDynamicMethod(target.Start, member.End, target, invoke.Identifier.Name, parameters);
                        return expr;
                    }

                    // Can be dynamic (reason: dynamic parameters)
                    var typedParameters = parameters.Select(e => e.GetExpressionType()).ToArray();
                    if (typedParameters.Any(e => e == null))
                    {
                        var expr = new CallDynamicMethod(target.Start, member.End, target, invoke.Identifier.Name, parameters);
                        return expr;
                    }

                    if (CommonHelper.TryGetMethod(invoke.Identifier.Name, typedParameters, targetType, target is CallType, out var method))
                    {
                        var expr = new CallMethod(target.Start, member.End, target, method, parameters);
                        return expr;
                    }
                    
                    return new Failed(target.Start, member.End, $"Method was not found in the type: {targetType}");
                default:
                    return new Failed(member.Start, member.End, "Method or property expected");
            }
        }
        
        private IReadOnlyList<Expr> BuildInvokeParameters(IReadOnlyList<Node> parameters)
        {
            var parameterExprs = new List<Expr>(parameters.Count);
            foreach (var parameter in parameters)
            {
                var parameterExpr = parameter.Accept(this);
                parameterExprs.Add(parameterExpr);
            }

            return parameterExprs;
        }

        private (Expr expr, int count) Create(Expr expr, int count)
        {
            return (expr, count);
        }

        private BinaryType ConvertToBinaryType(BinaryOperation binaryOperation)
        {
            switch (binaryOperation)
            {
                case BinaryOperation.Plus:
                    return BinaryType.Plus;
                case BinaryOperation.Minus:
                    return BinaryType.Minus;
                case BinaryOperation.Multiply:
                    return BinaryType.Multiply;
                case BinaryOperation.Divide:
                    return BinaryType.Divide;
                case BinaryOperation.Mod:
                    return BinaryType.Mod;

                case BinaryOperation.Or:
                    return BinaryType.Or;
                case BinaryOperation.And:
                    return BinaryType.And;
                
                case BinaryOperation.Greater:
                    return BinaryType.Greater;
                case BinaryOperation.GreaterOrEquals:
                    return BinaryType.GreaterOrEquals;
                case BinaryOperation.Less:
                    return BinaryType.Less;
                case BinaryOperation.LessOrEquals:
                    return BinaryType.LessOrEquals;
                case BinaryOperation.Equals:
                    return BinaryType.Equals;
                case BinaryOperation.NotEquals:
                    return BinaryType.NotEquals;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(binaryOperation), binaryOperation, null);
            }
        }

        private UnaryType ConvertToUnaryType(UnaryOperation unaryOperation)
        {
            switch (unaryOperation)
            {
                case UnaryOperation.Not:
                    return UnaryType.Not;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unaryOperation), unaryOperation, null);
            }
        }
    }
}
