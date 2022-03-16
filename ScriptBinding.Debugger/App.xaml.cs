using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using ScriptBinding.Debugger.ViewModels;

namespace ScriptBinding.Debugger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public new void Run()
        {
            SimpleIoc.Default.Register(() => new MainViewModel());

            var wnd = new MainWindow();
            wnd.DataContext = SimpleIoc.Default.GetInstance<MainViewModel>();

            Run(wnd);
        }
    }
}