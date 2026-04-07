using System.IO;
using System.Text.Json;

namespace Hangman.Tools
{
    public class WordService
    {
        public string GetRandomWord(string category)
        {
            if (category == "AllCategories")
            {
                List<string> allWords = new();
                string[] files = Directory.GetFiles("Words", "*.json");
                foreach (var file in files)
                {
                    List<string> fileWords = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(file));
                    if (fileWords != null)
                    {
                        allWords.AddRange(fileWords);
                    }
                }
                if (allWords != null && allWords.Count != 0)
                {
                    Random random = new();
                    return allWords[random.Next(allWords.Count)];
                }
                return "NULL";
            }
            string filePath = $"Words/{category}.json";
            string jsonString = File.ReadAllText(filePath);
            List<string> words = JsonSerializer.Deserialize<List<string>>(jsonString);
            if (words != null && words.Count != 0)
            {
                Random random = new();
                return words[random.Next(words.Count)];
            }
            return "NULL";
        }
    }
}
