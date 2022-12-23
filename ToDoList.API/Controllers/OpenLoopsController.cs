using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ToDoList.API.Models;

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
        private readonly ILogger<OpenLoopsController> _logger;
        private readonly OpenLoopDbContext _openLoopDbContext;
        /// <summary>
        /// Внедрённые зависимости
        /// </summary>
        /// <param name="logger">Сервис логирования</param>
        /// <param name="openLoopDbContext">Контекст базы данных</param>
        public OpenLoopsController(ILogger<OpenLoopsController> logger, OpenLoopDbContext openLoopDbContext)
        {
            _logger = logger;
            _openLoopDbContext = openLoopDbContext;
        }

        /// <summary>
        /// Запрос всех записей из БД, сущности OpenLoop
        /// </summary>
        /// <returns>List элементами которого являются OpenLoop</returns>
        [HttpGet]
        public ActionResult<IEnumerable<OpenLoop>> GetOpenLoops()
        {
            return Ok(_openLoopDbContext.OpenLoop.ToList());
        }

        /// <summary>
        /// Запрос одной записи из БД, по соответствующему идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор записи, которую нужно извлечь из БД</param>
        /// <returns>Возврат экземпляр объекта openLoop</returns>
        [HttpGet("{id}")]
        public ActionResult<OpenLoop> GetOpenLoops(string id)
        {
            var guid = new Guid(id);
            var openLoop = _openLoopDbContext.OpenLoop.FirstOrDefault(a => a.Id.Equals(guid));
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
        public ActionResult<OpenLoop> InsertOpenLoop(OpenLoopRequest openLoopRequest)
        {
            OpenLoop openLoop = new()
            {
                Id = Guid.NewGuid(),
                CreatedDateUtc = DateTime.UtcNow,
                Note = openLoopRequest.Note,
                Description = openLoopRequest.Description,
                Сomplet = openLoopRequest.Сomplet
            };

            bool successPars = DateTimeOffset.TryParse(openLoopRequest.СompletDate, out DateTimeOffset dateTimeOffset);
            openLoop.СompletDateUtc = successPars ? dateTimeOffset.UtcDateTime : DateTime.MinValue;

            _openLoopDbContext.OpenLoop.Add(openLoop);
            _openLoopDbContext.SaveChanges();
            return CreatedAtAction(nameof(GetOpenLoops), new { id = openLoop.Id }, openLoop);
        }

        /// <summary>
        /// Обновляет существующую запись в БД.
        /// </summary>
        /// <param name="id">Идентификатор записи которую нужно обновить.</param>
        /// <param name="openLoopRequest">Строка формата JSON, с новыми значениями полей.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<OpenLoop> UpdateOpenLoop(string id, OpenLoopRequest openLoopRequest)
        {
            bool successParse = Guid.TryParse(id, out Guid guid);
            if (!successParse)
            {
                return BadRequest(id);
            }

            successParse = DateTimeOffset.TryParse(openLoopRequest.СompletDate, out DateTimeOffset dateTimeOffset);
            if (!successParse)
            {
                return BadRequest(openLoopRequest.СompletDate);
            }

            var openLoopToUpdate = _openLoopDbContext.OpenLoop.FirstOrDefault(a => a.Id.Equals(guid));
            if (openLoopToUpdate == null)
            {
                return NotFound();
            }

            openLoopToUpdate.Description = openLoopRequest.Description;
            openLoopToUpdate.Note = openLoopRequest.Note;
            openLoopToUpdate.СompletDateUtc = dateTimeOffset.UtcDateTime;
            openLoopToUpdate.Сomplet = openLoopRequest.Сomplet;

            _openLoopDbContext.OpenLoop.Update(openLoopToUpdate);
            _openLoopDbContext.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Удаляет запись из БД.
        /// </summary>
        /// <param name="id">Идентификатор задачи которую следует удалить</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteOpenLoop(string id)
        {
            bool successParse = Guid.TryParse(id, out Guid guid);
            if (!successParse)
            {
                return BadRequest(id);
            }

            var openLoopToDelete = _openLoopDbContext.OpenLoop.FirstOrDefault(a => a.Id.Equals(guid));

            if (openLoopToDelete == null)
            {
                return NotFound();
            }

            _openLoopDbContext.OpenLoop.Remove(openLoopToDelete);
            _openLoopDbContext.SaveChanges();

            return NoContent();
        }

    }
}
