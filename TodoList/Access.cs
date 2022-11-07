using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace TodoList
{
    public static class Access
    {
        private static readonly string _fileGroups = "Groups.dat";
        private static readonly string _directory = "./SecretDate/";
        private static readonly string _pathGroups = Path.Combine(_directory, _fileGroups);
        private static readonly JsonSerializerOptions optionsJson;

        private static List<Group>? _groups = null;

        public static List<Group>? GetGroups()
        {
            _groups ??= LoadGroupsAsync().Result;
            return _groups;
        }
        public static void AddGroup(Group group)
        {
            _groups ??= new();
            _groups.Add(group);
            var save = SaveGroupsAsync();
        }
        public static void AddRangeGroup(List<Group> groups)
        {
            _groups ??= new();
            _groups.AddRange(groups);
            var save = SaveGroupsAsync();
        }
        static Access()
        {
            optionsJson = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            //_groups.Clear();
            
            /*//
            

            var groups = LoadGroups();
            if (groups != null)
            {
                _groups.AddRange(groups);
            }//

            SaveGroups();
            */
        }
        private static async Task<List<Group>?> LoadGroupsAsync()
        {
            if (!File.Exists(_pathGroups)) { return new(); }

            List<Group>? groups = new();

            /*using (BinaryReader reader = new BinaryReader(File.Open(_pathGroups, FileMode.Open)))
            {
                while (reader.PeekChar() > -1)
                {
                    string crypt = reader.ReadString();

                    string json = Сryptography.Decrypt(crypt);
                    var group = JsonSerializer.Deserialize<Group>(json);

                    if (group != null)
                    {
                        groups.Add(group);
                    }
                }
            }*/

            using (StreamReader reader = new(_pathGroups))
            {
                    string json = await reader.ReadToEndAsync();

                    //string json = Сryptography.Decrypt(json);
                    groups = JsonSerializer.Deserialize<List<Group>>(json);
            }
            return groups;
        }
        private static async Task<bool> SaveGroupsAsync()
        {
            if (_groups == null || _groups.Count < 1) { return false; }

            Directory.CreateDirectory(_directory);
            
            /*using (BinaryWriter writer = new BinaryWriter(File.Open(_pathGroups, FileMode.Create)))
            {
                foreach (var group in _groups)
                {
                    var json = JsonSerializer.Serialize(group, optionsJson);

                    var crypt = Сryptography.Encrypt(json);

                    writer.Write(crypt);
                }
            }*/

            using (StreamWriter writer = new(_pathGroups, false))
            {
                var json = JsonSerializer.Serialize(_groups, optionsJson);

                //json = Сryptography.Encrypt(json);

                await writer.WriteLineAsync(json);
                
            }
            return true;
        }
    }
}
