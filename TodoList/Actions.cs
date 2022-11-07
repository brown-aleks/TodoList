namespace TodoList
{
    public static class Actions
    {
        private static readonly Dictionary<string, Act> _actions = new();
        public static Dictionary<string, Act> GetActions() { return _actions; }
        static Actions()
        {
            _actions.Add("C", new()
            {
                NameAct = "Добавить задачу",
                OperationAct = () =>
                {
                    Console.WriteLine("Действия по добавлению задачи");
                }
            });
            _actions.Add("R", new()
            {
                NameAct = "Посмотреть задачи",
                OperationAct = () =>
                {
                    Console.WriteLine("Вывод всех задач");
                }
            });
            _actions.Add("U", new()
            {
                NameAct = "Просмотр имеющихся задачь",
                OperationAct = () =>
                {
                    Console.WriteLine("Действия по удалению задачи");
                }
            });
            _actions.Add("D", new()
            {
                NameAct = "Удалить задачу",
                OperationAct = () =>
                {
                    Console.WriteLine("Действия по удалению задачи");
                }
            });
            _actions.Add("NoName", new() { });

        }
        private static void AddNote()
        { }
    }
    public class Act
    {
        public delegate void act();
        public string NameAct { get; set; } = "No Name Act";
        public act OperationAct { get; set; } = () =>
        {
            Console.WriteLine("Ни какого действия не назначено");
        };
    }
}
