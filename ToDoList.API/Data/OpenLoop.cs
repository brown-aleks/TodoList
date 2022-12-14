namespace ToDoList.API.Data
{
    /// <summary>
    /// Модель представляющая сущность открытой задачи.
    /// </summary>
    public class OpenLoop
    {
        /// <summary>
        /// Уникальный идентификатор. Генерируется случайным образом при создании нового экземпляра.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Краткое наименование задачи.
        /// </summary>
        public string Note { get; set; } = string.Empty;
        /// <summary>
        /// Подробное описание задачи.
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Дата/время создания задачи. Устанавливается автоматически в момент создания экземпляра. Хранится в БД в формате UTC.
        /// </summary>
        public DateTime CreatedDateUtc { get; set; }
        /// <summary>
        /// Планируемая и/или фактическая дата завершения задачи. Устанавливается автором или пользователем имеющим право на редактирование.
        /// </summary>
        public DateTime CompleteDateUtc { get; set; }
        /// <summary>
        /// Состояние задачи на текущий момент. True - выполнена. False - выполняется, или не приступали к выполнению.
        /// </summary>
        public bool Complete { get; set; }
        /// <summary>
        /// Идентификатор пользователя, который создал задачу.
        /// </summary>
        public string CreatorId { get; set; } = string.Empty;
        /// <summary>
        /// Помечен на удаление.
        /// </summary>
        public bool IsDeleted { get; set; }

    }
}