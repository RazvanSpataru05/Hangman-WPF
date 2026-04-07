using Hangman.Game;

namespace Hangman
{ 
    public interface IDialogService
    {
        bool? ShowNewUserWindow(List<string> existingNames, out string name, out string imagePath);
        void ShowGameWindow(User user);
        void ShowSignUpWindow();
        bool? ShowGameOverWindow(string word, GameOverType type, bool timeExpired);
        void ShowAboutWindow();
        bool? ShowOpenSaveWindow(List<GameSave> saves, out GameSave selectedSave);
        void ShowStatisticsWindow();
    }
}
