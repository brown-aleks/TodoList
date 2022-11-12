using System.Text.RegularExpressions;

namespace TodoList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             *  Тестирование авторизации
             * */
            //var user = Authorization.AddUser();
            //var user = Authorization.AuthorizeUser();

            //--------------------------------------------------------
            /*
            List<Group> groups = new List<Group>();
            groups.Add(new Group()
            {
                Id = Guid.NewGuid(),
                Name = "beginners",
                Description = "Группа начинающих.",
                UsersId = new List<Guid> { new Guid("914b7127-1fb3-4e5b-9321-b0a3f54a4630"),
                                          new Guid("1b75186e-1549-4fb5-ab48-ee5334e9ea84"),
                                          new Guid("5ab8df46-0066-4c68-be30-5b2be16e0aaa")
                },
                ActionsKey = new List<string> { "C", "R" }
            });
            groups.Add(new Group()
            {
                Id = Guid.NewGuid(),
                Name = "moderators",
                Description = "Группа модераторов.",
                UsersId = new List<Guid> { new Guid("914b7127-1fb3-4e5b-9321-b0a3f54a4630"),
                                          new Guid("1b75186e-1549-4fb5-ab48-ee5334e9ea84")
                },
                ActionsKey = new List<string> { "C", "R", "U" }
            });
            groups.Add(new Group()
            {
                Id = Guid.NewGuid(),
                Name = "administrators",
                Description = "Группа администраторов.",
                UsersId = new List<Guid> { new Guid("914b7127-1fb3-4e5b-9321-b0a3f54a4630")
                },
                ActionsKey = new List<string> { "C", "R", "U", "D" }
            });
            Access.AddRangeGroup(groups);
            */
            //--------------------------------------------------------

            Application.Run();

            /*  
             *  Тестирование консольного ввода через ConsoleHelper
             *  
            string[] strings = { "Первая команда","Vtoraya kommanda","!@#*&%^(*&)(&*(" };
            ConsoleHelper.HidePassword = false;
            var str = ConsoleHelper.ReadLine(strings);
            Console.WriteLine("\n" + str +"\nEescapePressed = "+ ConsoleHelper.EescapePressed);
            Console.ReadLine();
            */

            /*
             *  Тест доступа к группам и их содержимому.
             * 
            var groups = Access.Groups;
            foreach (var group in groups)
            {
                Console.WriteLine($"{group.Id}  {group.Name}  {group.Description}");
                foreach (var actionKey in group.ActionsKey)
                {
                    Console.WriteLine($"{actionKey}");
                }
                foreach (var userId in group.UsersId)
                {
                    Console.WriteLine($"{userId}");
                }
            }
            */
        }
    }
}