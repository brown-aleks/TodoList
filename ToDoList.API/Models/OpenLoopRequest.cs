namespace ToDoList.API.Models
{
    /// <summary>
    /// Модель для запросов на добавление/изменение записи в БД.
    /// </summary>
    public class OpenLoopRequest
    {
        /// <summary>
        /// Краткое наименование задачи.
        /// </summary>
        public string Note { get; set; } = string.Empty;
        /// <summary>
        /// Подробное описание задачи.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Планируемая и/или фактическая дата завершения задачи. Устанавливается автором или пользователем имеющим право на редактирование.
        /// </summary>
        public string CompleteDate { get; set; } = DateTimeOffset.UtcNow.ToString();
        /// <summary>
        /// Состояние задачи на текущий момент. True - выполнена. False - выполняется, или не приступали к выполнению.
        /// </summary>
        public bool Complete { get; set; }
    }
}