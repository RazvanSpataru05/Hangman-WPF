using Hangman.Game;

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
            var viewModel = new GameViewModel(user, this);
            var window = new GameWindow(viewModel);
            window.Show();
        }
        public void ShowSignUpWindow()
        {
            var viewModel = new SignInViewModel(_userService, this);
            var window = new SignInWindow(viewModel);
            window.Show();
        }
        public bool? ShowGameOverWindow(string word)
        {
           var viewModel = new GameOverViewModel(word);
           var window = new GameOverWindow(viewModel);
           return window.ShowDialog();
        }
    }
}
