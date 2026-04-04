using Hangman.Dialog_Service;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Hangman
{
    public class SignInViewModel : INotifyPropertyChanged
    {
        private readonly IDialogService _dialogService;
        private readonly UserService _userService;
        private User _selectedUser;
        private ObservableCollection<User> _users;

        public event PropertyChangedEventHandler? PropertyChanged;
        public RelayCommand NewUserCommand { get; set; }
        public RelayCommand DeleteUserCommand { get; set; }
        public RelayCommand PlayCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                if (_users != value)
                {
                    _users = value;
                    OnPropertyChanged(nameof(Users));
                }
            }
        }
        public SignInViewModel(UserService userService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _userService = userService;
            _userService.LoadUsers();
            _users = new ObservableCollection<User>(_userService.GetUsers());

            NewUserCommand = new(_ => NewUser());
            DeleteUserCommand = new(_ => DeleteUser(), _ => SelectedUser != null);
            PlayCommand = new(parameter => Play(parameter), _ => SelectedUser != null);
            CancelCommand = new(_ => Cancel());
        }

        private void NewUser()
        {
            var existingNames = _users.Select(u => u.Name).ToList();
            var result = _dialogService.ShowNewUserDialog(existingNames, out string name, out string imagePath);
            if (result == true)
            {
                _userService.AddUser(name, imagePath);
                Users.Add(_userService.GetUsers().Last());
            }
        }
        private void DeleteUser()
        {
            if (SelectedUser == null) return;

            int selectedUserIndex = Users.IndexOf(SelectedUser);
            _userService.DeleteUser(SelectedUser.Name);
            Users.RemoveAt(selectedUserIndex);

            if (Users.Count == 0)
                SelectedUser = null;
            else
            {
                if (selectedUserIndex == Users.Count)
                    selectedUserIndex--;
                SelectedUser = Users[selectedUserIndex];
            }
        }
        private void Play(object? parameter)
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Please select an user first!");
                return;
            }
            var window = parameter as Window;
            _dialogService.ShowGameWindow(SelectedUser);
            window?.Close();
        }

        private void Cancel()
        {
            Application.Current.Shutdown();
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
