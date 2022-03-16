using System.Linq;
using System.Windows.Data;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals;

namespace ScriptBinding.Tests
{
    [TestClass]
    public class ScriptBindingTests
    {
        [TestMethod]
        public void ScriptBinding_CheckGeneratedBindings()
        {
            const string propertyPath = "IsChecked";
            const string elementName = "Grid";

            Tools.ScriptBinding binding = new Tools.ScriptBinding
            {
                Expression = $"b(1) + b({propertyPath}) + b({propertyPath}, {elementName}) + b(0)"
            };

            using (new AssertionScope())
            {
                binding.Bindings.Should().HaveCount(2);
                binding.Bindings.OfType<GeneratedBinding>().Should().Contain(e => e.Path.Path == propertyPath && e.ElementName == null);
                binding.Bindings.OfType<GeneratedBinding>().Should().Contain(e => e.Path.Path == propertyPath && e.ElementName == elementName);
            }
        }

        [TestMethod]
        public void ScriptBinding_CheckGeneratedBindingsWithOther()
        {
            const string propertyPath = "IsChecked";
            const string elementName = "Grid";

            Tools.ScriptBinding binding = new Tools.ScriptBinding();

            // Has 2 bindings
            binding.Bindings.Add(new Binding());
            binding.Bindings.Add(new Binding());

            // Has 4 bindings
            binding.Expression = $"b(1) + b({propertyPath}) + b({propertyPath}, {elementName}) + b(0)";

            using (new AssertionScope())
            {
                binding.Bindings.Should().HaveCount(4);
                binding.Bindings.OfType<GeneratedBinding>().Should().Contain(e => e.Path.Path == propertyPath && e.ElementName == null);
                binding.Bindings.OfType<GeneratedBinding>().Should().Contain(e => e.Path.Path == propertyPath && e.ElementName == elementName);

                var isCheckedBinding = binding.Bindings.OfType<GeneratedBinding>().FirstOrDefault(e => e.Path.Path == propertyPath && e.ElementName == null);
                var isCheckedElementBinding = binding.Bindings.OfType<GeneratedBinding>().FirstOrDefault(e => e.Path.Path == propertyPath && e.ElementName == elementName);

                binding.Bindings.Should().HaveElementAt(2, isCheckedBinding);
                binding.Bindings.Should().HaveElementAt(3, isCheckedElementBinding);
            }
        }

        [TestMethod]
        public void ScriptBinding_CheckGeneratedBindingsWithOther_2()
        {
            const string propertyPath = "IsChecked";
            const string elementName = "Grid";

            Tools.ScriptBinding binding = new Tools.ScriptBinding();

            // Has 2 bindings
            binding.Expression = $"b(1) + b({propertyPath}) + b({propertyPath}, {elementName}) + b(0)";

            // Has 4 bindings
            binding.Bindings.Add(new Binding());
            binding.Bindings.Add(new Binding());

            using (new AssertionScope())
            {
                binding.Bindings.Should().HaveCount(4);
                binding.Bindings.OfType<GeneratedBinding>().Should().Contain(e => e.Path.Path == propertyPath && e.ElementName == null);
                binding.Bindings.OfType<GeneratedBinding>().Should().Contain(e => e.Path.Path == propertyPath && e.ElementName == elementName);

                var isCheckedBinding = binding.Bindings.OfType<GeneratedBinding>().FirstOrDefault(e => e.Path.Path == propertyPath && e.ElementName == null);
                var isCheckedElementBinding = binding.Bindings.OfType<GeneratedBinding>().FirstOrDefault(e => e.Path.Path == propertyPath && e.ElementName == elementName);

                binding.Bindings.Should().HaveElementAt(0, isCheckedBinding);
                binding.Bindings.Should().HaveElementAt(1, isCheckedElementBinding);
            }
        }
    }
}
