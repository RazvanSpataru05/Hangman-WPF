using System.ComponentModel;
using System.Threading.Channels;
using System.Windows;

namespace Hangman.Game
{
    public class OpenSaveViewModel : INotifyPropertyChanged
    {
        private GameSave _selectedSave;
        public List<GameSave> Saves { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }

        public GameSave SelectedSave
        {
            get => _selectedSave;
            set
            {
                if (_selectedSave != value)
                {
                    _selectedSave = value;
                    OnPropertyChanged(nameof(SelectedSave));
                }
            }
        }

        public OpenSaveViewModel(List<GameSave> saves)
        {
            Saves = saves;
            CancelCommand = new RelayCommand(parameter => Close(parameter));
            LoadCommand = new RelayCommand(parameter => Load(parameter), _ => SelectedSave != null);
        }

        public void Close(object parameter)
        {
            var window = parameter as Window;
            if (window != null)
            {
                window.DialogResult = false;
            }
        }
        
        public void Load(object parameter)
        {
            var window = parameter as Window;
            if (window != null)
            {
                window.DialogResult = true;
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
