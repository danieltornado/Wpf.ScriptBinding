namespace ScriptBinding.Internals.Compiler
{
    interface IBindingGenerator
    {
        void GenerateBinding(string propertyPath);
        void GenerateBinding(string propertyPath, string elementName);
    }
}
