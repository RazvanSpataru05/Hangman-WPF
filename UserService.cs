using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Hangman
{
    public class UserService
    {
        private readonly string _filepath = "Data/users.json";
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
            {
                _users = users;
            }
        }
        private void SaveUsers()
        {
            Directory.CreateDirectory("Data");

            string jsonString = JsonSerializer.Serialize(_users);
            File.WriteAllText(_filepath, jsonString);
        }

        public void AddUser(string name, string profilePicturePath)
        {
            User user = new() { Name = name, ProfilePicturePath = profilePicturePath };
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
