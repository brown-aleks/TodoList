namespace AutomaticBroccoli.CLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.InputEncoding = Encoding.Unicode;
            //Console.OutputEncoding = Encoding.Unicode;

            var encoding = Console.OutputEncoding.EncodingName;

            // В терминале Visual Studio 2022 encoding = "Unicode (UTF-8)"
            // В терминале PowerShell encoding = "Codepage - 866"

            var openLoopsRepository = new OpenLoopsRepository();
            string? note;

            Console.WriteLine(encoding);
            Console.WriteLine("Что вас беспокоит сейчас?");
            do
            {
                note = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(note));

            openLoopsRepository.Add(new OpenLoop { Note = note, CreatedDate = DateTimeOffset.UtcNow });

            var openLoops = openLoopsRepository.Get();
            var group = openLoops.GroupBy(x => x.CreatedDate.Date);

            foreach (var groupOfOpenLoops in group)
            {
                Console.WriteLine($"Ваши заботы за: {groupOfOpenLoops.Key:D}");
                foreach (var openLoop in groupOfOpenLoops)
                {
                    Console.WriteLine(openLoop.Note);
                }
            }
        }
    }
}