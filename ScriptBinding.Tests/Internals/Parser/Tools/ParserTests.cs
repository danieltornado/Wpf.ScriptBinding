using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Tests.Internals.Parser.Tools;

// ReSharper disable once CheckNamespace
namespace ScriptBinding.Tests.Internals.Parser
{
    [TestClass]
    public partial class ParserTests
    {
        private static void DefaultTest<TNode>(string expression, object expectedNode)
            where TNode : ScriptBinding.Internals.Parser.Nodes.Node
        {
            var expected = (TNode)expectedNode;
            var node = expression.Parse();

            node.Should().BeOfType<TNode>()
                .And.Subject.Should().BeEquivalentTo(expected, options => options.Using(new EquivalentNodeComparer()));
        }
    }
}
