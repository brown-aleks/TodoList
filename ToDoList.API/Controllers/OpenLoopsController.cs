using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ToDoList.API.Models;

namespace ToDoList.API.Controllers
{
    /// <summary>
    /// ������ ������� CRUD ��� ������ � OpenLoops
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OpenLoopsController : Controller
    {
        private readonly ILogger<OpenLoopsController> _logger;
        private readonly OpenLoopDbContext _openLoopDbContext;
        /// <summary>
        /// ��������� �����������
        /// </summary>
        /// <param name="logger">������ �����������</param>
        /// <param name="openLoopDbContext">�������� ���� ������</param>
        public OpenLoopsController(ILogger<OpenLoopsController> logger, OpenLoopDbContext openLoopDbContext)
        {
            _logger = logger;
            _openLoopDbContext = openLoopDbContext;
        }

        /// <summary>
        /// ������ ���� ������� �� ��, �������� OpenLoop
        /// </summary>
        /// <returns>List ���������� �������� �������� OpenLoop</returns>
        [HttpGet]
        public ActionResult<IEnumerable<OpenLoop>> GetOpenLoops()
        {
            return Ok(_openLoopDbContext.OpenLoop.ToList());
        }

        /// <summary>
        /// ������ ����� ������ �� ��, �� ���������������� ��������������.
        /// </summary>
        /// <param name="id">������������� ������, ������� ����� ������� �� ��</param>
        /// <returns>������� ��������� ������� openLoop</returns>
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
        /// ��������� ����� ������ � ��.
        /// </summary>
        /// <param name="openLoopRequest">������ ������� JSON, �� ���������� ����� ����� ������.</param>
        /// <returns>����� ��������� ������ � ��</returns>
        [HttpPost]
        public ActionResult<OpenLoop> InsertOpenLoop(OpenLoopRequest openLoopRequest)
        {
            OpenLoop openLoop = new()
            {
                Id = Guid.NewGuid(),
                CreatedDateUtc = DateTime.UtcNow,
                Note = openLoopRequest.Note,
                Description = openLoopRequest.Description,
                �omplet = openLoopRequest.�omplet
            };

            bool successPars = DateTimeOffset.TryParse(openLoopRequest.�ompletDate, out DateTimeOffset dateTimeOffset);
            openLoop.�ompletDateUtc = successPars ? dateTimeOffset.UtcDateTime : DateTime.MinValue;

            _openLoopDbContext.OpenLoop.Add(openLoop);
            _openLoopDbContext.SaveChanges();
            return CreatedAtAction(nameof(GetOpenLoops), new { id = openLoop.Id }, openLoop);
        }

        /// <summary>
        /// ��������� ������������ ������ � ��.
        /// </summary>
        /// <param name="id">������������� ������ ������� ����� ��������.</param>
        /// <param name="openLoopRequest">������ ������� JSON, � ������ ���������� �����.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<OpenLoop> UpdateOpenLoop(string id, OpenLoopRequest openLoopRequest)
        {
            bool successParse = Guid.TryParse(id, out Guid guid);
            if (!successParse)
            {
                return BadRequest(id);
            }

            successParse = DateTimeOffset.TryParse(openLoopRequest.�ompletDate, out DateTimeOffset dateTimeOffset);
            if (!successParse)
            {
                return BadRequest(openLoopRequest.�ompletDate);
            }

            var openLoopToUpdate = _openLoopDbContext.OpenLoop.FirstOrDefault(a => a.Id.Equals(guid));
            if (openLoopToUpdate == null)
            {
                return NotFound();
            }

            openLoopToUpdate.Description = openLoopRequest.Description;
            openLoopToUpdate.Note = openLoopRequest.Note;
            openLoopToUpdate.�ompletDateUtc = dateTimeOffset.UtcDateTime;
            openLoopToUpdate.�omplet = openLoopRequest.�omplet;

            _openLoopDbContext.OpenLoop.Update(openLoopToUpdate);
            _openLoopDbContext.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// ������� ������ �� ��.
        /// </summary>
        /// <param name="id">������������� ������ ������� ������� �������</param>
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
