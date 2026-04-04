using System.Windows;

namespace Hangman
{
    public partial class SignInWindow : Window
    {
        public SignInWindow(SignInViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}