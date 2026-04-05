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
            if (users != null)
                _users = users;
        }
        public void SaveUsers()
        {
            Directory.CreateDirectory(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "Data"));

            string jsonString = JsonSerializer.Serialize(_users);
            File.WriteAllText(_filepath, jsonString);
        }

        public void AddUser(string name, string profilePicturePath)
        {
            User user = new() { Name = name, ImagePath = profilePicturePath };
            _users.Add(user);
            SaveUsers();
        }
        public void DeleteUser(string name)
        {
            _users.RemoveAll(x => x.Name == name);
            SaveUsers();
        }
        public List<User> GetUsers()
        {
            return _users;
        }
    }
}
