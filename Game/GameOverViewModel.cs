using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hangman.Game
{
    public enum GameOverType
    {
        Win,
        Lose
    }
    public class GameOverViewModel
    {
        public string Word { get; set; }
        public GameOverType Type { get; set; }
        public string Title => Type == GameOverType.Win ? "Congratulations!" : "Game Over!";
        public string Message { get; set; }
         
        public RelayCommand TryAgainCommand { get; set; }
        public RelayCommand BackToMenuCommand { get; set; }
        public GameOverViewModel(string word, GameOverType type, bool timeExpired)
        {
            Word = word;
            Type = type;

            switch (Type)
            {
                case GameOverType.Win:
                    Message = "You've passed all levels!";
                    break;
                case GameOverType.Lose:
                    Message = timeExpired ? $"Time's up! The correct word was \"{Word}\"." : $"The correct word was: \"{Word}\".";
                    break;
            }

            TryAgainCommand = new(TryAgain);
            BackToMenuCommand = new(BackToMenu);
        }
        private void TryAgain(object parameter)
        {
            var window = parameter as Window;
            if (window != null) window.DialogResult = true;
        }
        private void BackToMenu(object parameter)
        {
            var window = parameter as Window;
            if (window != null) window.DialogResult = false;
        }
    }
}
