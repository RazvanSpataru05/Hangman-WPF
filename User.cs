using System.Text.Json.Serialization;

namespace Hangman
{
    public class User
    {
        public string Name {  get; set; }
        public string ImagePath { get; set; }
        public List<GameSave> GameSaves { get; set; } = [];
        public List<Statistics> Statistics { get; set; } = [];

        [JsonConstructor]
        public User() {}
    }
}
