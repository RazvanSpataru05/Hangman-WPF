using System.Text.Json.Serialization;

namespace Hangman
{
    public class Statistics
    {
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public string Category { get; set; }
        public string Username { get; set; }


        [JsonConstructor]
        public Statistics() { }
    }
}
