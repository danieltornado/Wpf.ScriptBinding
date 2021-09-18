using System.Collections.Generic;
using System.Linq.Expressions;

namespace ScriptBinding.Internals
{
    class ExpressionHolder
    {
        public LambdaExpression Expression { get; }
        public List<GeneratedBinding> GeneratedBindings { get; }
    }
}
