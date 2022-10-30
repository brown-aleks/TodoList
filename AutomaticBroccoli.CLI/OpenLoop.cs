using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace AutomaticBroccoli.CLI
{
    public class OpenLoop
    {
        public string? Note { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }

    public class OpenLoopsRepository
    {
        private const string _directoryName = "./openLoops/";
        public bool Add(OpenLoop newOpenLoop)
        {
            Directory.CreateDirectory(_directoryName);

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(newOpenLoop, options);

            var fileName = $"{Guid.NewGuid()}.json";
            var path = Path.Combine( _directoryName, fileName );

            File.WriteAllText(path, json);

            return true;
        }
        public OpenLoop[] Get()
        {
            var files = Directory.GetFiles( _directoryName );

            var openLoops = new List<OpenLoop>();

            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var openLoop = JsonSerializer.Deserialize<OpenLoop>(json);

                if (openLoop == null)
                {
                    throw new Exception("OpenLoop cannot be deserialized.");
                }

                openLoops.Add(openLoop);
            }

            return openLoops.ToArray();
        }
    }
}