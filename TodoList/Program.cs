namespace TodoList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var openLoops = new List<string>();
            string? note;

            Console.WriteLine("Что вас беспокоит сейчас?");
            do
            {
                note = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(note));

            openLoops.Add(note);
        }
    }
}