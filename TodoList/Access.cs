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

        private static readonly List<Group> _groups = new();

        public static List<Group> Groups { get; } = _groups;
        static Access()
        {
            optionsJson = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            _groups = LoadGroups();

            //_groups.Clear();
            /*
            _groups.Add(new Group() {
                Id = Guid.NewGuid(),
                Name = "Guest",
                Description = "Группа гость. Обратитесь к адинистратору",
                UsrsId = new List<Guid> { new Guid("914b7127-1fb3-4e5b-9321-b0a3f54a4630"),
                                          new Guid("1b75186e-1549-4fb5-ab48-ee5334e9ea84"),
                                          new Guid("5ab8df46-0066-4c68-be30-5b2be16e0aaa") },
                ActionsKey = new List<string> { "C","R","U","D" } });
            

            var groups = LoadGroups();
            if (groups != null)
            {
                _groups.AddRange(groups);
            }//*/

            //SaveGroups();
            
        }
        private static List<Group> LoadGroups()
        {
            if (!File.Exists(_pathGroups)) { return new(); }

            List<Group> groups = new();

            using (BinaryReader reader = new BinaryReader(File.Open(_pathGroups, FileMode.Open)))
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
            }
            return groups;
        }
        private static bool SaveGroups()
        {
            if (_groups == null || _groups.Count < 1) { return false; }

            Directory.CreateDirectory(_directory);

            using (BinaryWriter writer = new BinaryWriter(File.Open(_pathGroups, FileMode.Create)))
            {
                foreach (var group in _groups)
                {
                    var json = JsonSerializer.Serialize(group, optionsJson);

                    var crypt = Сryptography.Encrypt(json);

                    writer.Write(crypt);
                }
            }

            return true;
        }

    }
}
