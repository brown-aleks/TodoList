using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using ToDoList.WebAPI.Contracts;
using ToDoList.WebAPI.Models;
using ToDoList.WebAPI.Services;

namespace ToDoList.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class OpenLoopsController :  ControllerBase
    {
        private readonly ILogger<OpenLoopsController> _logger;
        private readonly IOpenLoopsAccess _openLoopsAccess;

        public OpenLoopsController(ILogger<OpenLoopsController> loger, IOpenLoopsAccess openCasesAccess)
        {
            _logger = loger;
            _openLoopsAccess = openCasesAccess;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetOpenLoopsResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOpenLoops()
        {
            var response = _openLoopsAccess.Get().Clone();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOpenLoop(Guid id)
        {
            var openLoop = _openLoopsAccess.Get(id);

            if (openLoop == null)
            {
                return NotFound();
            }

            return Ok(openLoop);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOpenLoopAsync([FromBody] CreateOpenLoopRequest request)
        {
            var openLoopId = await _openLoopsAccess.AddAsync(new OpenLoop()
            {
                Note = request.Note,
                Description = request.Description,
                —ompletDate = request.—ompletDate
            } );
            return Ok(openLoopId);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PutOpenLoopAsync(Guid id, [FromBody] UpdateOpenLoopRequest request)
        {
            if (request is null || request.Note is null || request.Description is null) // Í‡Í‡ˇ ÎË·Ó ‰Û„‡ˇ ÔÓ‚ÂÍ‡ ÔÓ‚ÂÍ‡ ‚ıÓ‰ˇ˘Ëı ‰‡ÌÌ˚ı.
            {
                return BadRequest();
            }

            var openLoopId = await _openLoopsAccess.UpdateAsync(new OpenLoop()
            {
                Id = id,
                Note = request.Note,
                Description = request.Description,
                —ompletDate = request.—ompletDate,
                —omplet = request.—omplet
            });

            if (openLoopId == Guid.Empty)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteOpenLoop(Guid id)
        {
            var openLoopId = await _openLoopsAccess.DeleteAsync(id);

            if (openLoopId == Guid.Empty)
            {
                return NotFound();
            }
            return NoContent();
        }

    }

}