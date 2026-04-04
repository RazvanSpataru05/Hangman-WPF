using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{ 
    public interface IDialogService
    {
        bool? ShowNewUserDialog(List<string> existingNames, out string name, out string imagePath);

        void ShowGameWindow(User user);
    }
}
