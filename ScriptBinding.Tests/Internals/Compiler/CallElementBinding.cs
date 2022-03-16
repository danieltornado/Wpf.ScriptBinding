using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(CallElementBindingTestData), DynamicDataSourceType.Method)]
        public void CallElementBindingTests(string expression, object expectedExpr)
        {
            DefaultTest<CallElementBinding>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> CallElementBindingTestData()
        {
            yield return new object[]
            {
                "b(IsChecked, 'Control')",
                new CallElementBinding(0, 22, "IsChecked", "Control")
            };

            yield return new object[]
            {
                "b(Editable.IsChecked, 'Control')",
                new CallElementBinding(0, 31, "Editable.IsChecked", "Control")
            };

            yield return new object[]
            {
                "b(Editable.Value.IsChecked, 'Control')",
                new CallElementBinding(0, 37, "Editable.Value.IsChecked", "Control")
            };

            yield return new object[]
            {
                "b(IsChecked, GridView)",
                new CallElementBinding(0, 21, "IsChecked", "GridView")
            };

            yield return new object[]
            {
                "b(Editable.IsChecked, GridView)",
                new CallElementBinding(0, 30, "Editable.IsChecked", "GridView")
            };

            yield return new object[]
            {
                "b(Editable.Value.IsChecked, GridView)",
                new CallElementBinding(0, 36, "Editable.Value.IsChecked", "GridView")
            };
        }
    }
}
