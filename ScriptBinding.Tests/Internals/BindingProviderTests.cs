using System.Collections.Generic;
using System.Windows.Data;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals;
using ScriptBinding.Internals.Executor;

namespace ScriptBinding.Tests.Internals
{
    [TestClass]
    public class BindingProviderTests
    {
        [TestMethod]
        [DataRow("some value 1", "some value 2", "some value 3")]
        public void BindingProvider_GetByIndex_Test(params object[] values)
        {
            var bindingProvider = new BindingProvider();

            bindingProvider.SetValues(values);

            IBindingProvider provider = bindingProvider;

            using (new AssertionScope())
            {
                for (int index = 0; index < values.Length; index++)
                {
                    provider.TryGetValue(index, out object actualValue).Should().Be(true);

                    var expectedValue = values[index];
                    actualValue.Should().Be(expectedValue);
                }
            }
        }

        [TestMethod]
        [DynamicData(nameof(BindingProviderGetByPropertyPathTestData), DynamicDataSourceType.Method)]
        public void BindingProvider_GetByPropertyPath_Test(Binding[] bindings, object[] values)
        {
            var bindingProvider = new BindingProvider();

            bindingProvider.SetBindings(bindings);
            bindingProvider.SetValues(values);

            IBindingProvider provider = bindingProvider;

            using (new AssertionScope())
            {
                for (int i = 0; i < bindings.Length; i++)
                {
                    var propertyPath = bindings[i].Path.Path;
                    provider.TryGetValue(propertyPath, out object actualValue).Should().Be(true);

                    var expectedValue = values[i];
                    actualValue.Should().Be(expectedValue);
                }
            }
        }
        
        private static IEnumerable<object[]> BindingProviderGetByPropertyPathTestData()
        {
            yield return new object[]
            {
                // bindings
                new Binding[] { new GeneratedBinding("property1"), new GeneratedBinding("object.property2"), new GeneratedBinding("object.object.property3") },

                // values
                new object[] { "some value of the property1", "some value of the property 2", "some value of the property 3" }
            };
        }

        [TestMethod]
        [DynamicData(nameof(BindingProviderGetByElementNameTestData), DynamicDataSourceType.Method)]
        public void BindingProvider_GetByElementName_Test(Binding[] bindings, object[] values)
        {
            var bindingProvider = new BindingProvider();

            bindingProvider.SetBindings(bindings);
            bindingProvider.SetValues(values);

            IBindingProvider provider = bindingProvider;

            using (new AssertionScope())
            {
                for (int i = 0; i < bindings.Length; i++)
                {
                    var propertyPath = bindings[i].Path.Path;
                    var elementName = bindings[i].ElementName;
                    provider.TryGetValue(propertyPath, elementName, out object actualValue).Should().Be(true);

                    var expectedValue = values[i];
                    actualValue.Should().Be(expectedValue);
                }
            }
        }

        private static IEnumerable<object[]> BindingProviderGetByElementNameTestData()
        {
            yield return new object[]
            {
                // bindings
                new Binding[] { new GeneratedBinding("property1", "element1"), new GeneratedBinding("object.property2", "element2"), new GeneratedBinding("object.object.property3", "element3") },

                // values
                new object[] { "some value of the property1", "some value of the property 2", "some value of the property 3" }
            };
        }

        [TestMethod]
        [DynamicData(nameof(BindingProviderGetByMixedTestData), DynamicDataSourceType.Method)]
        public void BindingProvider_GetByMixed_Test(Binding[] bindings, object[] values)
        {
            var bindingProvider = new BindingProvider();

            bindingProvider.SetBindings(bindings);
            bindingProvider.SetValues(values);

            IBindingProvider provider = bindingProvider;

            using (new AssertionScope())
            {
                for (int i = 0; i < bindings.Length; i++)
                {
                    var index = i;
                    var binding = bindings[i];

                    object actualValue;

                    if (binding.Path != null)
                    {
                        var propertyPath = binding.Path.Path;

                        if (binding.ElementName != null)
                        {
                            var elementName = binding.ElementName;
                            provider.TryGetValue(propertyPath, elementName, out actualValue).Should().Be(true);
                        }
                        else
                        {
                            provider.TryGetValue(propertyPath, out actualValue).Should().Be(true);
                        }
                    }
                    else
                    {
                        provider.TryGetValue(index, out actualValue).Should().Be(true);
                    }
                    
                    var expectedValue = values[i];
                    actualValue.Should().Be(expectedValue);
                }
            }
        }

        private static IEnumerable<object[]> BindingProviderGetByMixedTestData()
        {
            yield return new object[]
            {
                // bindings
                new [] { new Binding(), new GeneratedBinding("property2"), new GeneratedBinding("property2", "element"), new Binding() },

                // values
                new object[] { "some value", "some value of the property 2", "some value of the property 2 and element", "some value 2" }
            };
        }
    }
}
