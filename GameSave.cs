using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hangman
{
    public class GameSave
    {
        public int CurrentLevel { get; set; }
        public List<char> GuessedLetters { get; set; } = [];
        public int Mistakes {  get; set; }
        public string Category {  get; set; }
        public string Word { get; set; }
        public int TimeLeft { get; set; }
        public string SaveTime { get; set; }

        [JsonConstructor]
        public GameSave() { }

        public GameSave(int currentLevel, HashSet<char> guessedLetters, int mistakes, string category, string word, int timeLeft, DateTime saveTime)
        {
            CurrentLevel = currentLevel;
            GuessedLetters = guessedLetters.ToList();
            Mistakes = mistakes;
            Category = category;
            Word = word;
            TimeLeft = timeLeft;
            SaveTime = saveTime.ToString("dd/MM/yyyy HH:mm");
        }
    }
}
