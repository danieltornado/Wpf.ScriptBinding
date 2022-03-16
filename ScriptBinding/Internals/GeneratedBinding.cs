using System.Windows.Data;

namespace ScriptBinding.Internals
{
    class GeneratedBinding : Binding
    {
        public GeneratedBinding(string propertyPath)
            : base(propertyPath)
        {
            Mode = BindingMode.OneWay;
        }

        public GeneratedBinding(string propertyPath, string elementName)
            : base(propertyPath)
        {
            ElementName = elementName;
            Mode = BindingMode.OneWay;
        }
    }
}