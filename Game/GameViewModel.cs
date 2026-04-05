using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using Hangman.Tools;
using System.Text;
using System.Security.Cryptography;

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
        private readonly int maxMistakes = 27;

        private GameState _state;
        private readonly IDialogService _dialogService;
        private readonly WordService _wordService = new();
        private string? _category;
        private int _currentLevel;
        private int _timeLeft;
        private int _mistakes;
        private bool _isGameActive;
        private string _word;
        private string _displayedWord;

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
        public GameViewModel(User currentUser, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _state = GameState.Ongoing;
            IsGameActive = false;
            CurrentUser = currentUser;
            GuessLetterCommands = new RelayCommand[26];

            NewGameCommand = new(_ => NewGame(), _ => Category != null);
            SaveGameCommand = new(_ => SaveGame(), _ => IsGameActive == true);
            SelectCategoryCommand = new(parameter => SelectCategory(parameter as string));
            CancelCommand = new(parameter => Cancel(parameter));

            for (int i = 0; i < GuessLetterCommands.Length; i++)
            {
                char letter = (char)('A' + i);
                GuessLetterCommands[i] = new(parameter => GuessLetter(letter),
                    _ => GuessedLetters.Contains(letter) == false);
            }
        }
        private void NewGame()
        {
            if (Category == null) return;

            IsGameActive = true;
            CurrentLevel = 1;
            SetupGame();
        }
        private void SaveGame()
        {

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
            var window = parameter as Window;
            _dialogService.ShowSignUpWindow();
            window?.Close();
        }
        private void GuessLetter(char letter)
        {
            if (!Word.Contains(letter))
                Mistakes++;

            GuessedLetters.Add(letter);
            UpdateDisplayedWord();
            CheckGameState();
            if (_state == GameState.Win)
            {
                CurrentLevel++;
                SetupGame();
            }
            else if (_state == GameState.Lose)
            {
                var result = _dialogService.ShowGameOverWindow(Word);
                if (result == true)
                {
                    CurrentLevel = 1;
                    SetupGame();
                }
                else
                {
                    IsGameActive = false;
                    CurrentLevel = 1;
                }
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
            if (Mistakes == maxMistakes)
            {
                _state = GameState.Lose;
                return;
            }
            for (int i= 0; i < Word.Length; i++)
            {
                if (Word[i] != DisplayedWord[i])
                    return;
            }
            _state = GameState.Win;
        }
        private void SetupGame()
        {
            if (Category == null) return;

            _state = GameState.Ongoing;
            Mistakes = 0;
            Word = _wordService.GetRandomWord(Category);
            TimeLeft = 60;
            GuessedLetters.Clear();
            StringBuilder sb = new();

            foreach (char c in Word)
            {
                sb.Append("_ ");
            }
            DisplayedWord = sb.ToString();
        }
    }
}
