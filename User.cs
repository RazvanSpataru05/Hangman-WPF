using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hangman
{
    public class User
    {
        public string Name {  get; set; }
        public string ProfilePicturePath { get; set; }
        public List<GameSave> GameSaves { get; set; } = [];
        public List<Statistics> Statistics { get; set; } = [];

        [JsonConstructor]
        public User()
        {
        }
    }
}
