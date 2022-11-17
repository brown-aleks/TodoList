using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using ToDoList.API.Models;

namespace ToDoList.API.Services
{
    public class OpenLoopsAccess : IOpenLoopsAccess
    {
        private const string _directoryName = "./data/";
        private readonly JsonSerializerOptions _optionsJson = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true
        };
        public async Task<Guid> AddAsync(OpenLoop newOpenLoops)
        {
            Directory.CreateDirectory(_directoryName);

            var guid = Guid.NewGuid();

            var openLoops = new OpenLoop()
            {
                Id = guid,
                Note = newOpenLoops.Note,
                Description = newOpenLoops.Description,
                CreatedDate = DateTimeOffset.Now,
                СompletDate = newOpenLoops.СompletDate,
                Сomplet = false
            };

            var json = JsonSerializer.Serialize(openLoops, _optionsJson);

            var fileName = $"{guid}.json";
            var path = Path.Combine(_directoryName, fileName);

            await File.WriteAllTextAsync(path, json);

            return openLoops.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var files = Directory.GetFiles(_directoryName);
            var filesFind = Path.Combine(_directoryName, id.ToString()) + ".json";

            var fileName = files.FirstOrDefault(f => f == filesFind);

            if (fileName == null) { return Guid.Empty; }

            File.Delete(fileName);

            return id;
        }

        public OpenLoop[] Get()
        {
            var files = Directory.GetFiles(_directoryName);

            var openLoops = new List<OpenLoop>();

            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var openLoop = JsonSerializer.Deserialize<OpenLoop>(json);

                if (openLoop == null)
                {
                    throw new Exception("openLoops cannot be deserialized.");
                }

                openLoops.Add(openLoop);
            }

            return openLoops.ToArray();
        }

        public OpenLoop? Get(Guid guid)
        {
            var openLoops = Get();
            var openLoop = openLoops.FirstOrDefault(loop => loop.Id == guid);

            return openLoop;
        }

        public async Task<Guid> UpdateAsync(OpenLoop openLoop)
        {
            var files = Directory.GetFiles(_directoryName);
            var filesFind = Path.Combine(_directoryName, openLoop.Id.ToString()) + ".json";

            var fileName = files.FirstOrDefault(f => f == filesFind);

            if (fileName == null) { return Guid.Empty; }

            var json = await File.ReadAllTextAsync(fileName);
            var openLoopFile = JsonSerializer.Deserialize<OpenLoop>(json);

            if (openLoopFile == null) { return Guid.Empty; }

            var newOpenLoop = new OpenLoop()
            { 
                Id = openLoopFile.Id, //openLoop.Id,
                Note = openLoop.Note,
                Description = openLoop.Description,
                CreatedDate = openLoopFile.CreatedDate, //openLoop.CreatedDate,
                СompletDate = openLoop.СompletDate,
                Сomplet = openLoop.Сomplet
            };

            json = JsonSerializer.Serialize(newOpenLoop, _optionsJson);
            await File.WriteAllTextAsync(fileName, json);

            return newOpenLoop.Id;
        }
    }
}
