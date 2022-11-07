using System.Text.RegularExpressions;

namespace TodoList
{
    public static class Application
    {
        static public User? User { get; set; }
        static public void Run()
        {
            string? startStr = string.Empty;
            do
            {
                Console.Clear();
                Console.WriteLine("Авторизоваться нажмите\t\t- L");
                Console.WriteLine("Зарегистрироваться нажмите\t- R");
                Console.WriteLine("Выход\t\t\t\t- Q");
                startStr = ConsoleHelper.ReadLine();
                Console.WriteLine();

                if (startStr == "L" || startStr == "l" || startStr == "Д" || startStr == "д")
                {
                    User = Authorization.AuthorizeUser();
                }
                if (startStr == "R" || startStr == "r" || startStr == "К" || startStr == "к")
                {
                    User = Authorization.AddUser();
                }
                if (User != null)
                {
                    Console.WriteLine("\nВы успешно авторизованны!");
                    Console.WriteLine($"{User.Id}  {User.Name}  {User.Email}");
                    Console.WriteLine("Вы состоите в группах");

                    var groups = Access.GetGroups()?.Where(g => g.UsrsId.Any(a => a == User.Id)) ?? new List<Group>();

                    //  TODO: Тут реализовать выборку доступных комманд для юзера из groups

                    foreach (var group in groups)
                    {
                        Console.WriteLine($"{group.Id}  {group.Name}  {group.Description}");
                    }
                }

            } while (startStr == "Q" || startStr == "q" || ConsoleHelper.EescapePressed);
        }
    }
}
