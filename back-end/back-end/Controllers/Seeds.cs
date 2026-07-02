using back_end.Data;
using Microsoft.AspNetCore.Mvc;
using back_end.Database.DbAccess;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using back_end.Database.Seeds;

namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Seeds : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly DbSeeds _DbSeeds;
        private readonly Seeder _seeder;

        public Seeds(AppDbContext context)
        {
            _context = context;
            _seeder = new Seeder(context);
            _DbSeeds = new DbSeeds(context);
        }

        [HttpPost("Static")]
        public async Task<IActionResult> Static()
        {
            await _DbSeeds.Static();
            return Ok("Seed executed");
        }

        [HttpPost("Seed")]
        public async Task<IActionResult> Seed([FromBody] int rows = 500)
        {
            await _seeder.Run(rows);
            return Ok("Seed executed");
        }

        [HttpDelete]
        public async Task<IActionResult> Clear()
        {
            await _DbSeeds.Clear();
            return Ok("Successfully Cleaned");
        }
    }
}
