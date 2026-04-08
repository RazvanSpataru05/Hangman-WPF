using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using Hangman.Tools;
using System.Text;
using System.Windows.Threading;
using System.IO;

namespace Hangman.Game
{
    public enum GameState
    {
        Ongoing,
        Win,
        Lose
    }
    public class GameViewModel : INotifyPropertyChanged
    {
        private readonly int MAX_MISTAKES = 7;

        private GameState _state;
        private readonly IDialogService _dialogService;
        private readonly WordService _wordService = new();
        private UserService _userService;

        private string? _category;
        private int _currentLevel;

        private DispatcherTimer _timer;
        private int _timeLeft;

        private bool _isGameActive;
        private string _word;
        private string _displayedWord;

        private string _displayedHangmanImage;
        private int _mistakes;
        public List<string> HangmanImages { get; set; } = [];

        public HashSet<char> GuessedLetters { get; set; } = new();
        public event PropertyChangedEventHandler? PropertyChanged;
        public User CurrentUser { get; set; }
        public bool IsGameActive
        {
            get => _isGameActive;
            set
            {
                if (_isGameActive != value)
                {
                    _isGameActive = value;
                    OnPropertyChanged(nameof(IsGameActive));
                    OnPropertyChanged(nameof(IsGameInactive));
                }
            }
        }
        public bool IsGameInactive => !IsGameActive;
        public RelayCommand NewGameCommand { get; set; }
        public RelayCommand OpenGameCommand { get; set; }
        public RelayCommand SaveGameCommand { get; set; }
        public RelayCommand StatisticsCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand SelectCategoryCommand { get; set; }
        public RelayCommand AboutCommand { get; set; }
        public RelayCommand[] GuessLetterCommands { get; set; }
        public string? Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
        public int CurrentLevel
        {
            get => _currentLevel;
            set
            {
                if (_currentLevel != value)
                {
                    _currentLevel = value;
                    OnPropertyChanged(nameof(CurrentLevel));
                }
            }
        }
        public int TimeLeft
        {
            get => _timeLeft;
            set
            {
                if (_timeLeft != value)
                {
                    _timeLeft = value;
                    OnPropertyChanged(nameof(TimeLeft));
                }
            }
        }
        public int Mistakes
        {
            get => _mistakes;
            set
            {
                if (_mistakes != value)
                {
                    _mistakes = value;
                    OnPropertyChanged(nameof(Mistakes));
                }
            }
        }
        public string Word
        {
            get => _word;
            set
            {
                if (_word != value)
                {
                    _word = value;
                    OnPropertyChanged(nameof(Word));
                }
            }
        }
        public string DisplayedWord
        {
            get => _displayedWord;
            set
            {
                if (_displayedWord != value)
                {
                    _displayedWord = value;
                    OnPropertyChanged(nameof(DisplayedWord));
                }
            }
        }
        public string DisplayedHangmanImage
        {
            get => _displayedHangmanImage;
            set
            {
                if (_displayedHangmanImage != value)
                {
                    _displayedHangmanImage = value;
                    OnPropertyChanged(nameof(DisplayedHangmanImage));
                }
            }
        }
        public GameViewModel(User currentUser, IDialogService dialogService, UserService userService)
        {
            _dialogService = dialogService;
            _userService = userService;
            _state = GameState.Ongoing;

            IsGameActive = false;
            CurrentUser = currentUser;

            _timer = new();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) =>
            {
                TimeLeft--;
                if (TimeLeft == 0)
                {
                    _timer.Stop();
                    _state = GameState.Lose;
                    DisplayedHangmanImage = HangmanImages[HangmanImages.Count - 2];
                    var result = _dialogService.ShowGameOverWindow(Word, GameOverType.Lose, true);
                    ResetGame(result);
                }
            };

            NewGameCommand = new(_ => NewGame(), _ => Category != null);
            SaveGameCommand = new(_ => SaveGame(), _ => IsGameActive == true);
            OpenGameCommand = new(_ => OpenGame());
            SelectCategoryCommand = new(parameter => SelectCategory(parameter as string));
            CancelCommand = new(parameter => Cancel(parameter));
            AboutCommand = new(_ => _dialogService.ShowAboutWindow());
            StatisticsCommand = new(_ => dialogService.ShowStatisticsWindow());
            GuessLetterCommands = new RelayCommand[26];

            for (int i = 0; i < GuessLetterCommands.Length; i++)
            {
                char letter = (char)('A' + i);
                GuessLetterCommands[i] = new(parameter => GuessLetter(letter),
                    _ => GuessedLetters.Contains(letter) == false);
            }
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Hangman");
            HangmanImages = Directory.GetFiles(path, "*.png").ToList();
            DisplayedHangmanImage = HangmanImages[Mistakes];
        }

        private void NewGame()
        {
            if (Category == null) return;

            IsGameActive = true;
            CurrentLevel = 1;
            _userService.UpdateStatistics(CurrentUser.Name, Category, false, true);
            SetupGame();
        }

        private void SaveGame()
        {
            GameSave save = new(CurrentLevel, GuessedLetters, Mistakes, Category, Word, TimeLeft, DateTime.Now);
            _userService.SaveGame(CurrentUser.Name, save);
            MessageBox.Show("Game saved successfully!", "Save Game", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenGame()
        {
            _timer.Stop();
            if (CurrentUser.GameSaves.Count == 0)
            {
                MessageBox.Show("No saved games found.", "Open Game", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var result = _dialogService.ShowOpenSaveWindow(CurrentUser.GameSaves, out GameSave selectedSave);
            if (result != null) _timer.Start();

            if (result == true)
            {
                LoadSave(selectedSave);
            }
        }
        private void SelectCategory(string category)
        {
            if (Category == category || category == null)
                Category = null;
            else
                Category = category;
        }
        private void Cancel(object? parameter)
        {
            _timer.Stop();
            var window = parameter as Window;
            _dialogService.ShowSignUpWindow();
            window?.Close();
        }
        private void GuessLetter(char letter)
        {
            if (!Word.Contains(letter))
            {
                Mistakes++;
                DisplayedHangmanImage = HangmanImages[Mistakes];
            }
            else
                TimeLeft += 3;

            GuessedLetters.Add(letter);
            UpdateDisplayedWord();
            CheckGameState();

            if (_state == GameState.Win)
            {
                _timer.Stop();
                DisplayedHangmanImage = HangmanImages[HangmanImages.Count-1];
                if (CurrentLevel == 3)
                {
                    _userService.UpdateStatistics(CurrentUser.Name, Category, true, false);
                    var result = _dialogService.ShowGameOverWindow(Word, GameOverType.Win, false);
                    ResetGame(result);
                }
                else
                {
                    CurrentLevel++;
                    MessageBox.Show($"Great job! Moving to level {CurrentLevel}.", "Level Completed",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    SetupGame();
                }
            }
            else if (_state == GameState.Lose)
            {
                _timer.Stop();
                _userService.UpdateStatistics(CurrentUser.Name, Category, false, false);
                var result = _dialogService.ShowGameOverWindow(Word, GameOverType.Lose, false);
                ResetGame(result);
            }
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void UpdateDisplayedWord()
        {
            StringBuilder sb = new();
            foreach (char c in Word)
            {
                if (GuessedLetters.Contains(c))
                    sb.Append(c);
                else
                    sb.Append("_ ");
            }
            DisplayedWord = sb.ToString();
        }

        private void CheckGameState()
        {
            if (Mistakes == MAX_MISTAKES)
            {
                _state = GameState.Lose;
                return;
            }
            for (int i = 0; i < Word.Length; i++)
            {
                if (Word[i] != DisplayedWord[i])
                    return;
            }
            _state = GameState.Win;
        }

        private void SetupGame()
        {
            if (Category == null) return;

            _timer.Start();
            _state = GameState.Ongoing;
            Mistakes = 0;
            DisplayedHangmanImage = HangmanImages[Mistakes];
            Word = _wordService.GetRandomWord(Category);
            TimeLeft = 30;
            GuessedLetters.Clear();
            StringBuilder sb = new();

            foreach (char c in Word)
            {
                sb.Append("_ ");
            }
            DisplayedWord = sb.ToString();
        }
        private void LoadSave(GameSave save)
        {
            _timer.Start();
            IsGameActive = true;
            Word = save.Word;
            TimeLeft = save.TimeLeft;
            CurrentLevel = save.CurrentLevel;
            Mistakes = save.Mistakes;
            Category = save.Category;
            GuessedLetters = save.GuessedLetters.ToHashSet<char>();
            DisplayedHangmanImage = HangmanImages[Mistakes];
            UpdateDisplayedWord();
        }

        private void ResetGame(bool? result)
        {
            _timer.Stop();
            CurrentLevel = 1;
            if (result == true)
            {
                SetupGame();
                _userService.UpdateStatistics(CurrentUser.Name, Category, false, true);
            }
            else
            {
                IsGameActive = false;
                Category = null;
            }
        }
    }
}
