using System.Text.Json.Serialization;

namespace Hangman
{
    public class User
    {
        public string Name {  get; set; }
        public string ImagePath { get; set; }

        [JsonIgnore]
        public List<GameSave> GameSaves { get; set; } = [];

        [JsonConstructor]
        public User() {}
    }
}
