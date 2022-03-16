using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals;
using ScriptBinding.Internals.Compiler;

namespace ScriptBinding.Tests.Internals
{
    [TestClass]
    public class ExpressionHolderTests
    {
        [TestMethod]
        [DataRow("PropertyPath1")]
        [DataRow("PropertyPath2")]
        public void ExpressionHolder_GeneratePropertyBinding_Test(string propertyPath)
        {
            var holder = new ExpressionHolder();
            IBindingGenerator bindingGenerator = holder;
            
            bindingGenerator.GenerateBinding(propertyPath);

            holder.GeneratedBindings.Should().HaveCount(1)
                .And.Subject.Should().Contain(e => e.Path != null && e.Path.Path == propertyPath && e.ElementName == null);
        }

        [TestMethod]
        [DataRow("PropertyPath1", "ElementName1")]
        [DataRow("PropertyPath2", "ElementName2")]
        public void ExpressionHolder_GenerateElementBinding_Test(string propertyPath, string elementName)
        {
            var holder = new ExpressionHolder();
            IBindingGenerator bindingGenerator = holder;

            bindingGenerator.GenerateBinding(propertyPath, elementName);

            holder.GeneratedBindings.Should().HaveCount(1)
                .And.Subject.Should().Contain(e => e.Path != null && e.Path.Path == propertyPath && e.ElementName == elementName);
        }
    }
}
