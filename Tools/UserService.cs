using System.IO;
using System.IO.Packaging;
using System.Text.Json;
using System.Windows;

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
            SaveUsers();
        }
        public List<User> GetUsers()
        {
            return _users;
        }

        // Save Services
        public void CreateSave(string username, GameSave save)
        {
            int index = _users.IndexOf(_users.FirstOrDefault(x => x.Name == username));
            if (index == -1) return;

            _users[index].GameSaves.Add(save);
            UpdateSaves(username, index);
        }
        private void UpdateSaves(string username, int index)
        {
            string fileName = username + "_save.json";
            string jsonString = JsonSerializer.Serialize(_users?[index].GameSaves);

            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Saves"));
            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Saves", fileName), jsonString);
        }
        public List<GameSave>? LoadSaves(string username)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Saves", username + "_save.json");
            if (!File.Exists(path)) return [];

            return JsonSerializer.Deserialize<List<GameSave>>(File.ReadAllText(path)) ?? [];
        }
        private void DeleteSaves(string username)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Saves", username + "_save.json");
            if (File.Exists(path)) File.Delete(path);
        }
    }
}
