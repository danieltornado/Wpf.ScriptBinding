using ScriptBinding.Debugger.Views;

namespace ScriptBinding.Debugger.ViewModels
{
    [AssociatedView(typeof(ElementBindingView))]
    class ElementBindingViewModel : BindingViewModel
    {
        public string ElementName { get; }

        /// <inheritdoc />
        public ElementBindingViewModel(string propertyPath, string elementName)
            : base(propertyPath + "," + elementName)
        {
            ElementName = elementName;
        }
    }
}
