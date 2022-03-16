using System;

namespace ScriptBinding.Debugger.Views
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    class AssociatedViewAttribute : Attribute
    {
        public Type ViewType { get; }

        public AssociatedViewAttribute(Type viewType)
        {
            ViewType = viewType;
        }
    }
}
