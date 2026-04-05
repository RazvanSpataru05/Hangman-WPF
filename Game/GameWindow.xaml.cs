using System.Windows;

namespace Hangman.Game
{
    public partial class GameWindow : Window
    {
        public GameWindow(GameViewModel gameViewModel)
        {
            InitializeComponent();
            DataContext = gameViewModel;
        }
    }
}
