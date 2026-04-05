using System.ComponentModel;
using System.Windows;

namespace Hangman
{
    public class NewUserViewModel : INotifyPropertyChanged
    {
        private string _name;
        private readonly List<string> _existingNames;

        public event PropertyChangedEventHandler? PropertyChanged;
        public ImageSelectorViewModel ImageSelector { get; set; } = new();

        public RelayCommand AddUserCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public NewUserViewModel(List<string> existingNames)
        {
            _existingNames = existingNames;
            AddUserCommand = new(parameter => AddUser(parameter));
            CancelCommand = new(parameter => Cancel(parameter));
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        private void AddUser(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Name cannot be empty!");
                return;
            }
            if (_existingNames.Contains(Name))
            {
                MessageBox.Show("Username already exists!");
                return;
            }
            var window = parameter as Window;
            window.DialogResult = true;
        }
        private void Cancel(object parameter)
        {
            var window = parameter as Window;
            window.DialogResult = false;
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
