using System.ComponentModel.DataAnnotations;

namespace ToDoList.API.Models
{
    /// <summary>
    /// Модель для регистрации нового пользователя
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Обязательное поле. Должно соответствовать формату электронной почты. Должно быть уникальным в БД.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Обязательное поле. Имя пользователя (NickName). Длинна строки от 1 до 50 символов. Должно быть уникально в БД.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Username { get; set; }

        /// <summary>
        /// Обязательное поле. Длинна строки от 8 до 50 символов.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }

        /// <summary>
        /// Обязательное поле. Должно полностью совпадать с полем Password.
        /// </summary>
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Обязательное поле. Полное имя пользователя. Используется в диалогах с пользователем.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }
    }
}