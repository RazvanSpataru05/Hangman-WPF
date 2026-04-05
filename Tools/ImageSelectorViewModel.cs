using System.ComponentModel;
using System.IO;

namespace Hangman
{
    public class ImageSelectorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private int _currentImageIndex;
        public int CurrentImageIndex
        {
            get => _currentImageIndex;
            set
            {
                if (_currentImageIndex != value)
                {
                    _currentImageIndex = value;
                    OnPropertyChanged(nameof(CurrentImageIndex));
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }

        public List<string> Images { get; set; } = [];
        public string? CurrentImage
        {
            get => Images.Count > 0 ? Images[CurrentImageIndex] : null;
            set { }
        }

        public RelayCommand NextImageCommand { get; set; }
        public RelayCommand PreviousImageCommand { get; set; }

        public ImageSelectorViewModel()
        {
            string assetsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
            Images = Directory.GetFiles(assetsPath, "*.png").ToList();
            if (Images.Count != 0)
                CurrentImage = Images[0];

            NextImageCommand = new(_ => NextImage());
            PreviousImageCommand = new(_ => PreviousImage());
        }

        public void NextImage()
        {
            if (CurrentImageIndex == Images.Count - 1)
            {
                CurrentImageIndex = 0;
            }
            else
            {
                CurrentImageIndex++;
            }
        }
        public void PreviousImage()
        {
            if (CurrentImageIndex == 0)
            {
                CurrentImageIndex = Images.Count - 1;
            }
            else
            {
                CurrentImageIndex--;
            }
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
