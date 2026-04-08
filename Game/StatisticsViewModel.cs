using System.ComponentModel;

namespace Hangman.Game
{
    public class StatisticsViewModel : INotifyPropertyChanged
    {
        private readonly UserService _userService;

        private Statistics _selectedStatistic;
        private List<Statistics> _displayedStatistics;
        private string _category;
        private string _displayedCategory;
        
        public List<string> Categories { get; set; }
        public int CategoryIndex { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public Statistics SelectedStatistics
        {
            get => _selectedStatistic;
            set
            {
                if (_selectedStatistic != value)
                {
                    _selectedStatistic = value;
                    OnPropertyChanged(nameof(SelectedStatistics));
                }
            }
        }
        public List<Statistics> DisplayedStatistics
        {
            get => _displayedStatistics;
            set
            {
                if (_displayedStatistics != value)
                {
                    _displayedStatistics = value;
                    OnPropertyChanged(nameof(DisplayedStatistics));
                }
            }
        }
        public string DisplayedCategory
        {
            get => _displayedCategory;
            set
            {
                if (_displayedCategory != value)
                {
                    _displayedCategory = value;
                    OnPropertyChanged(nameof(DisplayedCategory));
                }
            }
        }
        public string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    if (_category == "AllCategories")
                        DisplayedCategory = "All Categories";
                    else
                        DisplayedCategory = _category; 

                    OnPropertyChanged(nameof(Category));
                }
            }
        }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PreviousPageCommand { get; set; }

        public StatisticsViewModel(UserService userService)
        {
            _userService = userService;
            Categories = _userService.LoadCategories();
            CategoryIndex = 0;

            if (Categories.Count > 0)
                Category = Categories[CategoryIndex];

            DisplayedStatistics = _userService.LoadStatistics(Category);
            NextPageCommand = new(_ => NextPage());
            PreviousPageCommand = new(_ => PreviousPage());
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void NextPage()
        {
            if (Categories.Count == 0) return;

            if (CategoryIndex < Categories.Count - 1)
                CategoryIndex++;
            else
                CategoryIndex = 0;
            Category = Categories[CategoryIndex];
            DisplayedStatistics = _userService.LoadStatistics(Category);
        }
        private void PreviousPage()
        {
            if (Categories.Count == 0) return;

            if (CategoryIndex > 0)
                CategoryIndex--;
            else
                CategoryIndex = Categories.Count - 1;
            Category = Categories[CategoryIndex];
            DisplayedStatistics = _userService.LoadStatistics(Category);
        }

    }
}
