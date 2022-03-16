using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ScriptBinding.Example.Annotations;

namespace ScriptBinding.Example
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        public static MainViewModel Instance { get; } = new MainViewModel();

        private MainViewModel()
        {
            //System.Windows.Visibility.Visible
            //System.Windows.Visibility.Collapsed
            Enum v = System.Windows.Visibility.Collapsed;
        }

        public string Text => "Script binding sample text in the MainViewModel";
        public int Number => 10;
        public bool Mark => true;

        private string _twoWayBindingText;
        public string TwoWayBindingText
        {
            get => _twoWayBindingText;
            set
            {
                if (Equals(_twoWayBindingText, value))
                    return;

                _twoWayBindingText = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}