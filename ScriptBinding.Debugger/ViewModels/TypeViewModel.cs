using System;
using GalaSoft.MvvmLight;
using ScriptBinding.Debugger.ViewModels.Base;

namespace ScriptBinding.Debugger.ViewModels
{
    class TypeViewModel : ViewModelBase
    {
        public Type Type { get; }

        public IBindingValueViewModel BindingValueViewModel { get; }

        /// <summary>
        /// Used as DisplayMemberPath
        /// </summary>
        public string DisplayPath => Type.Name;

        public TypeViewModel(Type type, IBindingValueViewModel bindingValueVm)
        {
            Type = type;
            BindingValueViewModel = bindingValueVm;
        }
    }
}
