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
        public HashSet<char> GuessedLetters { get; set; } = [];
        public int Mistakes {  get; set; }
        public string Category {  get; set; }
        public string Word { get; set; }
        public int TimeLeft { get; set; }

        [JsonConstructor]
        public GameSave() { }

        public GameSave(int currentLevel, HashSet<char> guessedLetters, int mistakes, string category, string word, int timeLeft)
        {
            CurrentLevel = currentLevel;
            GuessedLetters = guessedLetters;
            Mistakes = mistakes;
            Category = category;
            Word = word;
            TimeLeft = timeLeft;
        }
    }
}
