using System.Windows;
using System.Windows.Input;

namespace Hangman.Game
{
    public partial class GameWindow : Window
    {
        public GameWindow(GameViewModel gameViewModel)
        {
            InitializeComponent();
            DataContext = gameViewModel;
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key >= Key.A && e.Key <= Key.Z)
            {
                var letter = (char)('A' + (e.Key - Key.A));
                var command = ((GameViewModel)DataContext).GuessLetterCommands[letter - 'A'];
                if (command.CanExecute(null))
                {
                    command.Execute(null);
                }
            }
        }
    }
}
