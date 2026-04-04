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
        public uint CurrentLevel { get; set; }
        public HashSet<char> GuessedLetters { get; set; } = [];
        public uint NumMistakes {  get; set; }
        public string Category {  get; set; }
        public string Word { get; set; }
        public uint TimeLeft { get; set; }

        [JsonConstructor]
        public GameSave() { }
    }
}
