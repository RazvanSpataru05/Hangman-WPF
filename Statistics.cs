using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hangman
{
    public class Statistics
    {
        public uint GamesPlayed {  get; set; }
        public uint GamesWon {  get; set; }
        public string Category {  get; set; }


        [JsonConstructor]
        public Statistics() { }
    }
}
