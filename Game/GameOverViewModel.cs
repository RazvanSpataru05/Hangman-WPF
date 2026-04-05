using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hangman.Game
{
    public class GameOverViewModel
    {
        public string Word { get; set; }

        public RelayCommand TryAgainCommand { get; set; }
        public RelayCommand BackToMenuCommand { get; set; }
        public GameOverViewModel(string word)
        {
            Word = word;

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
