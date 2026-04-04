using Hangman.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.Dialog_Service
{
    public class DialogService : IDialogService
    {
        public bool? ShowNewUserDialog(List<string> existingNames, out string name, out string imagePath)
        {
            var viewModel = new NewUserViewModel(existingNames);
            var window = new NewUserWindow(viewModel);
            var result = window.ShowDialog();
            name = viewModel.Name;
            imagePath = viewModel.CurrentImage;
            return result;
        }

        public void ShowGameWindow(User user)
        {
            var viewModel = new GameViewModel(user);
            var window = new GameWindow(viewModel);
            window.Show();
        }
    }
}
