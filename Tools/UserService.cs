using System.IO;
using System.Text.Json;

namespace Hangman
{
    public class UserService
    {
        private readonly string _filepath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Data", "users.json");
        private List<User>? _users;
        public void LoadUsers()
        {
            if (!File.Exists(_filepath))
            {
                _users = [];
                return;
            }

            string jsonString = File.ReadAllText(_filepath);
            List<User>? users = JsonSerializer.Deserialize<List<User>>(jsonString);
            if (users != null) _users = users;

            foreach (var user in _users)
            {
                user.GameSaves = LoadSaves(user.Name);
            }
        }
        public void SaveUsers()
        {
            Directory.CreateDirectory(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "Data"));

            string jsonString = JsonSerializer.Serialize(_users);
            File.WriteAllText(_filepath, jsonString);
        }

        public void AddUser(string username, string profilePicturePath)
        {
            User user = new() { Name = username, ImagePath = profilePicturePath };
            _users?.Add(user);
            SaveUsers();
        }
        public void DeleteUser(string username)
        {
            _users?.RemoveAll(x => x.Name == username);
            DeleteSaves(username);
            DeleteStatistics(username);
            SaveUsers();
        }
        public List<User> GetUsers()
        {
            return _users;
        }

        // Save Services
        public void SaveGame(string username, GameSave save)
        {
            int index = _users.IndexOf(_users.FirstOrDefault(x => x.Name == username));
            if (index == -1) return;

            _users[index].GameSaves.Add(save);
            UpdateSaves(username, index);
        }
        private void UpdateSaves(string username, int index)
        {
            string fileName = username + "_save.json";
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Saves", fileName);
            string jsonString = JsonSerializer.Serialize(_users?[index].GameSaves);

            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Saves"));
            File.WriteAllText(filepath, jsonString);
        }
        public List<GameSave>? LoadSaves(string username)
        {
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Saves", username + "_save.json");
            if (!File.Exists(filepath)) return [];

            return JsonSerializer.Deserialize<List<GameSave>>(File.ReadAllText(filepath)) ?? [];
        }
        private void DeleteSaves(string username)
        {
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Saves", username + "_save.json");
            if (File.Exists(filepath)) File.Delete(filepath);
        }

        // Statistics Services
        public void UpdateStatistics(string username, string category, bool win, bool incrementPlayed)
        {
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Statistics", category + ".json");
            Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            List<Statistics> statistics = [];
            
            if (File.Exists(filepath))
            {
                statistics = JsonSerializer.Deserialize<List<Statistics>>(File.ReadAllText(filepath)) ?? [];
            }
            var userStat = statistics.FirstOrDefault(x => x.Username == username);
            if (userStat == null)
            {
                userStat = new Statistics() { Username = username, Category = category };
                statistics.Add(userStat);
            }
            if (incrementPlayed) userStat.GamesPlayed++;
            if (win) userStat.GamesWon++;
            File.WriteAllText(filepath, JsonSerializer.Serialize(statistics));
        }
        public List<Statistics> LoadStatistics(string category)
        {
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Statistics", category + ".json");
            Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            if (!File.Exists(filepath)) return [];

            return JsonSerializer.Deserialize<List<Statistics>>(File.ReadAllText(filepath)) ?? [];
        }
        public void DeleteStatistics(string username)
        {
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Statistics");
            if (!Directory.Exists(directory)) return;

            string[] files = Directory.GetFiles(directory, "*.json");
            foreach (var file in files)
            {
                List<Statistics> statistics = JsonSerializer.Deserialize<List<Statistics>>(File.ReadAllText(file)) ?? [];
                if (statistics.Count > 0)
                {
                    statistics.RemoveAll(x => x.Username == username);
                    File.WriteAllText(file, JsonSerializer.Serialize(statistics));
                }
            }
        }
        public List<string> LoadCategories()
        {
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Statistics");
            if (!Directory.Exists(directory)) return [];

            return Directory.GetFiles(directory, "*.json")
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }
    }
}
