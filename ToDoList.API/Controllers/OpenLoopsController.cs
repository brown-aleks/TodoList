using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ToDoList.API.Models;

namespace ToDoList.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OpenLoopsController : Controller
    {
        private readonly ILogger<OpenLoopsController> _logger;
        private readonly OpenLoopDbContext _openLoopDbContext;

        public OpenLoopsController(ILogger<OpenLoopsController> logger, OpenLoopDbContext openLoopDbContext)
        {
            _logger = logger;
            _openLoopDbContext = openLoopDbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OpenLoop>> GetOpenLoops()
        {
            return Ok(_openLoopDbContext.OpenLoop.ToList());
        }


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

        [HttpPost]
        public ActionResult<OpenLoop> InsertOpenLoop(OpenLoopRequest openLoopRequest)
        {
            OpenLoop openLoop = new()
            {
                Id = Guid.NewGuid(),
                CreatedDateUtc = DateTime.UtcNow,
                Note = openLoopRequest.Note,
                Description = openLoopRequest.Description,
                Ñomplet = openLoopRequest.Ñomplet
            };

            bool successPars = DateTimeOffset.TryParse(openLoopRequest.ÑompletDate, out DateTimeOffset dateTimeOffset);
            openLoop.ÑompletDateUtc = successPars ? dateTimeOffset.UtcDateTime : DateTime.MinValue;

            _openLoopDbContext.OpenLoop.Add(openLoop);
            _openLoopDbContext.SaveChanges();
            return CreatedAtAction(nameof(GetOpenLoops), new { id = openLoop.Id }, openLoop);
        }

        [HttpPut("{id}")]
        public ActionResult<OpenLoop> UpdateOpenLoop(string id, OpenLoopRequest openLoopRequest)
        {
            bool successParse = Guid.TryParse(id, out Guid guid);
            if (!successParse)
            {
                return BadRequest(id);
            }

            successParse = DateTimeOffset.TryParse(openLoopRequest.ÑompletDate, out DateTimeOffset dateTimeOffset);
            if (!successParse)
            {
                return BadRequest(openLoopRequest.ÑompletDate);
            }

            var openLoopToUpdate = _openLoopDbContext.OpenLoop.FirstOrDefault(a => a.Id.Equals(guid));
            if (openLoopToUpdate == null)
            {
                return NotFound();
            }

            openLoopToUpdate.Description = openLoopRequest.Description;
            openLoopToUpdate.Note = openLoopRequest.Note;
            openLoopToUpdate.ÑompletDateUtc = dateTimeOffset.UtcDateTime;
            openLoopToUpdate.Ñomplet = openLoopRequest.Ñomplet;

            _openLoopDbContext.OpenLoop.Update(openLoopToUpdate);
            _openLoopDbContext.SaveChanges();

            return NoContent();
        }

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
