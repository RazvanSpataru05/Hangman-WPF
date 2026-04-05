namespace Hangman
{ 
    public interface IDialogService
    {
        bool? ShowNewUserWindow(List<string> existingNames, out string name, out string imagePath);
        void ShowGameWindow(User user);
        void ShowSignUpWindow();
        bool? ShowGameOverWindow(string word);
    }
}
