using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using ToDoList.API.Data;
using ToDoList.API.Models;
using ToDoList.API.Services;

namespace ToDoList.API.Controllers
{
    /// <summary>
    /// Группа методов CRUD для работы с OpenLoops
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OpenLoopsController : Controller
    {
        private readonly ILogger<OpenLoopsController> logger;
        private readonly OpenLoopService openLoopService;

        /// <summary>
        /// Внедрённые зависимости
        /// </summary>
        /// <param name="logger">Сервис логирования</param>
        /// <param name="openLoopService"></param>
        public OpenLoopsController(
            ILogger<OpenLoopsController> logger,
            OpenLoopService openLoopService)
        {
            this.logger = logger;
            this.openLoopService = openLoopService;
        }

        /// <summary>
        /// Запрос всех записей из БД, сущности OpenLoop
        /// </summary>
        /// <returns>List - элементами которого являются OpenLoop</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OpenLoop>>> GetOpenLoops()
        {
            var openLoops = await openLoopService.GetAsync();
            return Ok(openLoops);
        }

        /// <summary>
        /// Запрос одной записи из БД, по соответствующему идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор записи, которую нужно извлечь из БД</param>
        /// <returns>Возврат экземпляр объекта openLoop</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<OpenLoop>> GetOpenLoop(string id)
        {
            var guid = new Guid(id);
            var openLoop = await openLoopService.GetAsync(guid);
            if (openLoop == null)
            {
                return NotFound();
            }
            return Ok(openLoop);
        }

        /// <summary>
        /// Добавляет новую запись в БД.
        /// </summary>
        /// <param name="openLoopRequest">Строка формата JSON, со значениями полей новой задачи.</param>
        /// <returns>вновь созданная запись в БД</returns>
        [HttpPost]
        public async Task<IActionResult> CreateOpenLoop(OpenLoopRequest openLoopRequest)
        {
            var openLoop = await openLoopService.CreateAsync(openLoopRequest, User);
            return CreatedAtAction(nameof(GetOpenLoop), new { id = openLoop.Id }, openLoop);
        }

        /// <summary>
        /// Обновляет существующую запись в БД.
        /// </summary>
        /// <param name="id">Идентификатор записи которую нужно обновить.</param>
        /// <param name="openLoopRequest">Строка формата JSON, с новыми значениями полей.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOpenLoop(string id, OpenLoopRequest openLoopRequest)
        {
            bool successParse = Guid.TryParse(id, out Guid guid);
            if (!successParse)
            {
                return BadRequest(id);
            }

            successParse = DateTimeOffset.TryParse(openLoopRequest.CompleteDate, out DateTimeOffset dateTimeOffset);
            if (!successParse)
            {
                return BadRequest(openLoopRequest.CompleteDate);
            }

            var openLoopToUpdate = new OpenLoop()
            {
                Id = guid,
                Note = openLoopRequest.Note,
                Description = openLoopRequest.Description,
                CompleteDateUtc = dateTimeOffset.UtcDateTime,
                Complete = openLoopRequest.Complete
            };

            var result = await openLoopService.UpdateAsync(openLoopToUpdate, User);

            return result;
        }

        /// <summary>
        /// Удаляет запись из БД.
        /// </summary>
        /// <param name="id">Идентификатор задачи которую следует удалить</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOpenLoop(string id)
        {
            bool successParse = Guid.TryParse(id, out Guid guid);
            if (!successParse)
            {
                return BadRequest(id);
            }

            var result = await openLoopService.DeleteAsync(guid, User);

            return result;
        }
    }
}
