using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using JetBrains.Annotations;
using ScriptBinding.Internals.Parser.ErrorListeners;
using ScriptBinding.Internals.Parser.Nodes;
using ScriptBinding.Internals.Parser.obj.Debug;

namespace ScriptBinding.Internals.Parser
{
    sealed class Parser : ScriptBaseVisitor<Node>
    {
        private readonly IScriptErrorListener _errorListener;

        public Parser([NotNull] IScriptErrorListener errorListener)
        {
            _errorListener = errorListener;
        }

        public Node Parse(string expression)
        {
            var charStream = new AntlrInputStream(expression);

            var lexer = new ScriptLexer(charStream);
            PrepareLexerErrorListener(lexer, new LexerErrorListenerAdapter(_errorListener));

            var tokenStream = new CommonTokenStream(lexer);

            var parser = new ScriptParser(tokenStream);
            PrepareParserErrorListener(parser, new ParserErrorListenerAdapter(_errorListener));

            var rootContext = parser.root();
            var root = rootContext.Accept(this);

            return root;
        }

        private void PrepareLexerErrorListener(ScriptLexer lexer, IAntlrErrorListener<int> listener)
        {
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(listener);
        }

        private void PrepareParserErrorListener(ScriptParser parser, IAntlrErrorListener<IToken> listener)
        {
            parser.RemoveErrorListeners();
            parser.AddErrorListener(listener);
        }

        #region Overrides of ScriptBaseVisitor<Node>

        /// <inheritdoc />
        public override Node VisitRoot(ScriptParser.RootContext context)
        {
            var singleNode = context.GetChild(0).Accept(this);
            return singleNode;
        }

        /// <inheritdoc />
        public override Node VisitStatement(ScriptParser.StatementContext context)
        {
            var singleNode = context.GetChild(0).Accept(this);
            return singleNode;
        }

        /// <inheritdoc />
        public override Node VisitConditional(ScriptParser.ConditionalContext context)
        {
            //  0: IF
            //  1: OPEN_PARENS
            //  2: statement
            //  3: CLOSE_PARENS
            //  4: THEN
            //  5: OPEN_PARENS
            //  6: statement
            //  7: CLOSE_PARENS
            //  8: ELSE
            //  9: OPEN_PARENS
            // 10: statement
            // 11: CLOSE_PARENS

            var ifNode = context.GetChild(2).Accept(this);
            if (ifNode != null)
            {
                var thenNode = context.GetChild(6).Accept(this);
                if (thenNode != null)
                {
                    var elseNode = context.GetChild(10).Accept(this);
                    if (elseNode != null)
                        return new ConditionalNode(context.Start.StartIndex, context.Stop.StopIndex, ifNode, thenNode, elseNode);
                }
            }

            return null;
        }

        /// <inheritdoc />
        public override Node VisitNotCondition(ScriptParser.NotConditionContext context)
        {
            // NOT OPEN_PARENS statement CLOSE_PARENS

            var node = context.GetChild(2).Accept(this);
            if (node != null)
                return new UnaryNode(context.Start.StartIndex, context.Stop.StopIndex, node, UnaryOperation.Not);

            return null;
        }

        /// <inheritdoc />
        public override Node VisitMemberAccess(ScriptParser.MemberAccessContext context)
        {
            // 0: member | parensStatement
            var firstNode = context.GetChild(0).Accept(this);

            if (context.ChildCount > 1 && firstNode != null)
            {
                // 1: DOT
                // 2: member
                // 3: DOT
                // 4: member...

                var members = new List<Node>(context.ChildCount / 2 + 1);
                var memberAccessNode = new MemberAccessNode(context.Start.StartIndex, context.Stop.StopIndex, members);

                members.Add(firstNode);

                for (int i = 2; i < context.ChildCount; i += 2)
                {
                    var nextNode = context.GetChild(i).Accept(this);
                    
                    if (nextNode == null)
                        return null;
                    
                    members.Add(nextNode);
                }

                return memberAccessNode;
            }

            return firstNode;
        }

        /// <inheritdoc />
        public override Node VisitInvoke(ScriptParser.InvokeContext context)
        {
            var identifierNode = (IdentifierNode)context.GetChild(0).Accept(this);
            if (identifierNode != null)
            {
                var parameters = GetInvokeParameters(context);

                var invokeNode = new InvokeNode(context.Start.StartIndex, context.Stop.StopIndex, identifierNode, parameters);
                return invokeNode;
            }

            return null;
        }

        /// <inheritdoc />
        public override Node VisitString(ScriptParser.StringContext context)
        {
            string text = context.GetChild<ITerminalNode>(0).GetText();
            text = EraseQuotes(text);

            var stringNode = new StringNode(context.Start.StartIndex, context.Stop.StopIndex, text);
            return stringNode;
        }

        /// <inheritdoc />
        public override Node VisitIdentifier(ScriptParser.IdentifierContext context)
        {
            string identifierText = context.GetChild<ITerminalNode>(0).GetText();
            var identifierNode = new IdentifierNode(context.Start.StartIndex, context.Stop.StopIndex, identifierText);

            return identifierNode;
        }

        /// <inheritdoc />
        public override Node VisitNumber(ScriptParser.NumberContext context)
        {
            string integerPart = context.GetChild<ITerminalNode>(0).GetText();

            if (context.ChildCount == 2)
            {
                // integer with a modifier
                string modifierPart = context.GetChild<ITerminalNode>(1).GetText();
                return CreateNumberNode(context.Start.StartIndex, context.Stop.StopIndex, integerPart, modifierPart);
            }

            if (context.ChildCount == 3)
            {
                // real number
                string fractionalPart = context.GetChild<ITerminalNode>(2).GetText();
                string value = integerPart + "." + fractionalPart;

                return new RealNode(context.Start.StartIndex, context.Stop.StopIndex, value);
            }

            if (context.ChildCount == 4)
            {
                // real number with a modifier
                string fractionalPart = context.GetChild<ITerminalNode>(2).GetText();
                string value = integerPart + "." + fractionalPart;

                string modifierPart = context.GetChild<ITerminalNode>(3).GetText();
                return CreateNumberNode(context.Start.StartIndex, context.Stop.StopIndex, value, modifierPart);
            }

            return new IntegerNode(context.Start.StartIndex, context.Stop.StopIndex, integerPart);
        }

        /// <inheritdoc />
        public override Node VisitAdditivePositive(ScriptParser.AdditivePositiveContext context)
        {
            return ParseBinaryOperation(context, BinaryOperation.Plus);
        }

        /// <inheritdoc />
        public override Node VisitSubtracting(ScriptParser.SubtractingContext context)
        {
            return ParseBinaryOperation(context, BinaryOperation.Minus);
        }

        /// <inheritdoc />
        public override Node VisitMultiplicative(ScriptParser.MultiplicativeContext context)
        {
            return ParseBinaryOperation(context, BinaryOperation.Multiply);
        }

        /// <inheritdoc />
        public override Node VisitDividing(ScriptParser.DividingContext context)
        {
            return ParseBinaryOperation(context, BinaryOperation.Divide);
        }

        /// <inheritdoc />
        public override Node VisitMod(ScriptParser.ModContext context)
        {
            return ParseBinaryOperation(context, BinaryOperation.Mod);
        }

        /// <inheritdoc />
        public override Node VisitOperandOr(ScriptParser.OperandOrContext context)
        {
            return ParseBinaryOperation(context, BinaryOperation.Or);
        }

        /// <inheritdoc />
        public override Node VisitOperandAnd(ScriptParser.OperandAndContext context)
        {
            return ParseBinaryOperation(context, BinaryOperation.And);
        }

        /// <inheritdoc />
        public override Node VisitCondition(ScriptParser.ConditionContext context)
        {
            var leftNode = context.GetChild(0).Accept(this);

            if (context.ChildCount == 3)
            {
                var rightNode = context.GetChild(2).Accept(this);
                if (rightNode != null)
                {
                    BinaryOperation operation;

                    var terminalChild = (ITerminalNode)context.GetChild(1);
                    var operatorSymbol = terminalChild.Symbol;
                    switch (operatorSymbol.Type)
                    {
                        case ScriptLexer.EQUALS:
                            operation = BinaryOperation.Equals;
                            break;
                        case ScriptLexer.NOT_EQUALS:
                            operation = BinaryOperation.NotEquals;
                            break;
                        case ScriptLexer.GREATER:
                            operation = BinaryOperation.Greater;
                            break;
                        case ScriptLexer.GREATER_EQUALS:
                            operation = BinaryOperation.GreaterOrEquals;
                            break;
                        case ScriptLexer.LESS:
                            operation = BinaryOperation.Less;
                            break;
                        case ScriptLexer.LESS_EQUALS:
                            operation = BinaryOperation.LessOrEquals;
                            break;
                        default:
                            _errorListener.SyntaxError(operatorSymbol.StartIndex, "Operator is not supported", null);
                            return null;
                    }

                    return new BinaryNode(context.Start.StartIndex, context.Stop.StopIndex, leftNode, rightNode, operation);
                }

                return null;
            }

            return leftNode;
        }

        /// <inheritdoc />
        public override Node VisitParensStatement(ScriptParser.ParensStatementContext context)
        {
            var node = context.GetChild(1).Accept(this);
            if (node != null)
            {
                var parensNode = new ParensNode(context.Start.StartIndex, context.Stop.StopIndex, node);
                return parensNode;
            }

            return null;
        }

        #endregion

        private Node ParseBinaryOperation(IParseTree context, BinaryOperation operation)
        {
            // 0: argument
            var currentNode = context.GetChild(0).Accept(this);

            if (currentNode != null)
            {
                // optional
                //
                // 1: binary operation
                // 2: argument
                // 3: binary operation
                // 4: argument
                // 5: ...
                for (int i = 2; i < context.ChildCount; i += 2)
                {
                    var node = context.GetChild(i).Accept(this);

                    if (node == null)
                        return null;

                    currentNode = new BinaryNode(currentNode.Start, node.End, currentNode, node, operation);
                }
            }

            return currentNode;
        }

        private Node CreateNumberNode(int start, int end, string value, string modifier)
        {
            switch (modifier)
            {
                case "l":
                case "L":
                    return new IntegerNode(start, end, value, IntegerModifiers.L);
                case "d":
                case "D":
                    return new RealNode(start, end, value, RealModifiers.D);
                case "f":
                case "F":
                    return new RealNode(start, end, value, RealModifiers.F);
                case "m":
                case "M":
                    return new RealNode(start, end, value, RealModifiers.M);
                default:
                    throw new NotSupportedException("Unknown number modifier");
            }
        }

        private IReadOnlyList<Node> GetInvokeParameters(ScriptParser.InvokeContext context)
        {
            // identifier OPEN_PARENS (invokeParameter (COMMA invokeParameter)*)? CLOSE_PARENS

            if (context.ChildCount > 3)
            {
                var parameters = new List<Node>(context.ChildCount / 2 - 1);
                for (int i = 2; i < context.ChildCount; i += 2)
                {
                    var parameterNode = context.GetChild(i).Accept(this);
                    
                    if (parameterNode == null)
                        return new List<Node>(0);
                    
                    parameters.Add(parameterNode);
                }

                return parameters;
            }

            return new List<Node>(0);
        }

        private string EraseQuotes(string text)
        {
            char[] newText = new char[text.Length - 2];
            for (int i = 1; i < text.Length - 1; i++)
                newText[i - 1] = text[i];

            return new string(newText);
        }
    }
}
