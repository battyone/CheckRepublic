using System.Linq;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Website.Controllers
{
    [Route("api/[controller]")]
    public class ChecksController : Controller
    {
        private readonly CheckContext _context;

        public ChecksController(CheckContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(int skip = 0, int take = 10)
        {
            if (skip < 0 || take < 0 || take > 100)
            {
                return BadRequest();
            }

            var checks = await _context
                .Checks
                .OrderBy(x => x.CheckId)
                .Skip(0)
                .Take(10)
                .ToListAsync();

            return new ObjectResult(checks);
        }
        
        [HttpGet("id:{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var check = await _context
                .Checks
                .FirstOrDefaultAsync(x => x.CheckId == id);

            if (check == null)
            {
                return NotFound();
            }

            return new ObjectResult(check);
        }

        [HttpGet("name:{name}")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            var check = await _context
                .Checks
                .FirstOrDefaultAsync(x => x.Name == name);

            if (check == null)
            {
                return NotFound();
            }

            return new ObjectResult(check);
        }
    }
}
