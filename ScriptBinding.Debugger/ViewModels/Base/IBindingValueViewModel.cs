using System.ComponentModel;

namespace ScriptBinding.Debugger.ViewModels.Base
{
    interface IBindingValueViewModel : INotifyPropertyChanged
    {
        object GetValue();
        bool HasErrors();
    }
}
