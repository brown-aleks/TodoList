using System.ComponentModel.DataAnnotations;

namespace ToDoList.API.Models
{
    /// <summary>
    /// Модель для аутентификации пользователя в системе.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Обязательное поле. Пользователь должен быть предварительно зарегистрирован в системе.
        /// </summary>
        [Required]
        public string Email { get; set; }
        
        /// <summary>
        /// Обязательное поле. Пароль должен соответствовать учётной записи.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}