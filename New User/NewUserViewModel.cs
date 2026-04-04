using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Hangman
{
    public class NewUserViewModel : INotifyPropertyChanged
    {
        private Window _window;
        private string _name;
        private int _currentIndex;
        private string _currentImage;
        private readonly List<string> _existingNames;

        public event PropertyChangedEventHandler? PropertyChanged;
        public List<string> Images = [];

        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand PreviousImageCommand { get; set; }
        public RelayCommand AddUserCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public NewUserViewModel(List<string> existingNames)
        {
            _existingNames = existingNames;

            string assetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
            Images = Directory.GetFiles(assetsPath, "*.jpg").ToList();
            if (Images.Count !=0)
                CurrentImage = Images[0];

            NextImageCommand = new(_ => NextImage());
            PreviousImageCommand = new(_ => PreviousImage());
            AddUserCommand = new(parameter =>
            {
                _window = parameter as Window;
                AddUser();
            });
            CancelCommand = new(parameter =>
            {
                _window = parameter as Window;
                Cancel();
            });
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
        public string CurrentImage
        {
            get => _currentImage;
            set
            {
                if (value != _currentImage)
                {
                    _currentImage = value;
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }

        private void NextImage()
        {
            if (Images.Count == 0) return;

            if (_currentIndex == Images.Count - 1)
            {
                _currentIndex = 0;
            }
            else
                _currentIndex++;
            CurrentImage = Images[_currentIndex];
        }
        private void PreviousImage()
        {
            if (Images.Count == 0) return;

            if (_currentIndex == 0)
            {
                _currentIndex = Images.Count - 1;
            }
            else
                _currentIndex--;
            CurrentImage = Images[_currentIndex];
        }
        private void AddUser()
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
            _window.DialogResult = true;
        }
        private void Cancel()
        {
            _window.DialogResult = false;
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
