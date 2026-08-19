using Microsoft.AspNetCore.Mvc;
using back_end.Data;
using back_end.Database.DbAccess.Interfaces;
using back_end.Shared.Core;
using back_end.Models;

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChapterTranslationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IChapterTranslation _dbAccess;

        public ChapterTranslationController(AppDbContext context, IChapterTranslation dbAccess)
        {
            _context = context;
            _dbAccess = dbAccess;
        }

        [HttpGet]
        public async Task<ActionResult<List<DTOs.ChapterTranslation>>> GetChapterTranslation([FromQuery] int? id)
        {
            //? Verifications
            if (!id.HasValue)
                return BadRequest("You must provide a id");
            //? Variables
            Result<List<DTOs.ChapterTranslation>> result = await _dbAccess.GetChapterTranslation(id.Value);

            if (result.IsFailure)
                return StatusCode(500, "Server Failure");
            if (result.Value is null)
                return NotFound();

            return Ok(result.Value);
        }
    }
}
