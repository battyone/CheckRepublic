using System.Linq;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Knapcode.CheckRepublic.Website.Controllers
{
    [Route("api/[controller]")]
    public class CheckBatchesController : Controller
    {
        private readonly CheckContext _context;

        public CheckBatchesController(CheckContext context)
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
                .CheckBatches
                .OrderByDescending(x => x.CheckBatchId)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new ObjectResult(checks);
        }
        
        [HttpGet("id:{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var checkBatch = await _context
                .CheckBatches
                .Include(x => x.CheckResults)
                .ThenInclude(x => x.Check)
                .FirstOrDefaultAsync(x => x.CheckBatchId == id);

            if (checkBatch == null)
            {
                return NotFound();
            }

            foreach (var checkResult in checkBatch.CheckResults)
            {
                checkResult.Check.CheckResults = null;
            }

            return new ObjectResult(checkBatch);
        }
    }
}
