using Microsoft.AspNetCore.Mvc;
using back_end.Data;
using back_end.Database.DbAccess.Interfaces;
using back_end.Shared.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITitle _dbAccess;

        public TitleController(AppDbContext context, ITitle dbAccess)
        {
            _context = context;
            _dbAccess = dbAccess;
        }

        [HttpGet]
        public async Task<ActionResult<List<DTOs.Title>>> GetTitle([FromQuery] int? id)
        {
            //? Verifications
            if (!id.HasValue)
                return BadRequest("You must provide a id");

            //? Variables
            Result<List<DTOs.Title>> result = await _dbAccess.GetTitleById(id.Value);

            if (result.IsFailure)
                return StatusCode(500, "Server Failure");

            if (result.Value!.Count <= 0 && id.HasValue)
                return NotFound();

            return Ok(result.Value);
        }
        [HttpGet("latestupdates")]
        public async Task<ActionResult<List<DTOs.Title>>> GetTitleLatestUpdates([FromQuery] int? limit, [FromQuery] bool compact = true)
        {
            //? Verifications
            if (!limit.HasValue)
                return BadRequest("You must provide a limit");

            //? Variables
            Result<List<DTOs.Title>> result = await _dbAccess.GetTitleLatestUpdates(limit.Value, compact);

            if (result.IsFailure)
                return StatusCode(500, "Server Failure");

            return Ok(result.Value);
        }
        [HttpGet("recentlyadded")]
        public async Task<ActionResult<List<DTOs.Title>>> GetTitleRecentlyAdded([FromQuery] int? limit, [FromQuery] bool compact = true)
        {
            //? Verifications
            if (!limit.HasValue)
                return BadRequest("You must provide a limit");

            //? Variables
            Result<List<DTOs.Title>> result = await _dbAccess.GetTitleRecentlyAdded(limit.Value, compact);

            if (result.IsFailure)
                return StatusCode(500, "Server Failure");

            return Ok(result.Value);
        }
        [HttpGet("featured")]
        public async Task<ActionResult<List<DTOs.Title>>> GetTitleFeatured([FromQuery] int? limit)
        {
            //? Verifications
            if (!limit.HasValue)
                return BadRequest("You must provide a limit");

            //? Variables
            Result<List<DTOs.Title>> result = await _dbAccess.GetFeaturedTitles(limit.Value);

            if (result.IsFailure)
                return StatusCode(500, "Server Failure");

            return Ok(result.Value);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<DTOs.Title>>> SearchTitle(
            [FromQuery] string? name,
            [FromQuery] string? author,
            [FromQuery] string? artist,

            [FromQuery] int[]? genresIds,
            [FromQuery] int[]? themesIds,
            [FromQuery] int[]? demographicIds,
            [FromQuery] int[]? statusIds,
            [FromQuery] int[]? contentRatingIds,
            [FromQuery] int? publicationYear,

            [FromQuery] int[]? excludeGenresIds,
            [FromQuery] int[]? excludeThemesIds
            )
        {
            Result<List<DTOs.Title>> result = await _dbAccess.GetTitlesByFilters(
                name,
                author,
                artist,

                genresIds,
                themesIds,
                demographicIds,
                statusIds,
                contentRatingIds,
                publicationYear,

                excludeGenresIds,
                excludeThemesIds
                );

            if (result.IsFailure)
                return StatusCode(500, "Server Failure");

            return result.Value;
        }
    }
}
