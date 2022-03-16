using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScriptBinding.Internals.Compiler.Expressions;

namespace ScriptBinding.Tests.Internals.Compiler
{
    partial class CompilerTests
    {
        [TestMethod]
        [DynamicData(nameof(CallPropertyBindingTestData), DynamicDataSourceType.Method)]
        public void CallPropertyBindingTests(string expression, object expectedExpr)
        {
            DefaultTest<CallPropertyBinding>(expression, expectedExpr);
        }

        private static IEnumerable<object[]> CallPropertyBindingTestData()
        {
            yield return new object[]
            {
                "b(IsChecked)",
                new CallPropertyBinding(0, 11, "IsChecked")
            };

            yield return new object[]
            {
                "b(Editable.IsChecked)",
                new CallPropertyBinding(0, 20, "Editable.IsChecked")
            };

            yield return new object[]
            {
                "b(Editable.Value.IsChecked)",
                new CallPropertyBinding(0, 26, "Editable.Value.IsChecked")
            };
        }
    }
}
