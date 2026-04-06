using System.Windows;

namespace Hangman.Game
{
    public partial class GameOverWindow : Window
    {
        public GameOverWindow(GameOverViewModel gameOverViewModel)
        {
            InitializeComponent();
            DataContext = gameOverViewModel;
        }
    }
}
