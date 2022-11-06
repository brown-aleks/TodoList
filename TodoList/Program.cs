namespace TodoList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             *  Тестирование авторизации юзера
             * */
            //var user = Authorization.AddUser();
            var user = Authorization.AuthorizeUser();
            


            /*  
             *  Тестирование консольного ввода через ConsoleHelper
             *  
            string[] strings = { "Первая комманда","Vtoraya kommanda","!@#*&%^(*&)(&*(" };
            ConsoleHelper.HidePassword = false;
            var str = ConsoleHelper.ReadLine(strings);
            Console.WriteLine("\n" + str +"\nEescapePressed = "+ ConsoleHelper.EescapePressed);
            Console.ReadLine();
            */
        }
    }
}