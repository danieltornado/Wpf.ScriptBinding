namespace ScriptBinding.Internals.Executor
{
    interface IBindingProvider
    {
        bool TryGetValue(int index, out object value);
        bool TryGetValue(string propertyPath, out object value);
        bool TryGetValue(string propertyPath, string elementName, out object value);
    }
}
