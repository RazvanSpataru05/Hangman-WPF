using Hangman.About;
using Hangman.Game;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;

namespace Hangman.Dialog_Service
{
    public class DialogService : IDialogService
    {
        private readonly UserService _userService;

        public DialogService(UserService userService)
        {
            _userService = userService;
        }

        public bool? ShowNewUserWindow(List<string> existingNames, out string name, out string imagePath)
        {
            var viewModel = new NewUserViewModel(existingNames);
            var window = new NewUserWindow(viewModel);
            var result = window.ShowDialog();
            name = viewModel.Name;
            imagePath = viewModel.ImageSelector.CurrentImage;
            return result;
        }

        public void ShowGameWindow(User user)
        {
            var viewModel = new GameViewModel(user, this, _userService);
            var window = new GameWindow(viewModel);
            window.Show();
        }
        public void ShowSignUpWindow()
        {
            var viewModel = new SignInViewModel(_userService, this);
            var window = new SignInWindow(viewModel);
            window.Show();
        }
        public bool? ShowGameOverWindow(string word, GameOverType type, bool timeExpired)
        {
            var viewModel = new GameOverViewModel(word, type, timeExpired);
            var window = new GameOverWindow(viewModel);
            return window.ShowDialog();
        }
        public void ShowAboutWindow()
        {
            var window = new AboutWindow();
            window.ShowDialog();
        }
        public bool? ShowOpenSaveWindow(List<GameSave> saves, out GameSave selectedSave)
        {
            var viewModel = new OpenSaveViewModel(saves);
            var window = new OpenSaveWindow(viewModel);
            var result = window.ShowDialog();
            selectedSave = viewModel.SelectedSave;
            return result;
        }
        public void ShowStatisticsWindow()
        {
            var viewModel = new StatisticsViewModel(_userService);
            var window = new StatisticsWindow(viewModel);
            window.ShowDialog();
        }
    }
}
