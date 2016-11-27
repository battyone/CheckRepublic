using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business;
using Knapcode.CheckRepublic.Logic.Business.Models;
using Knapcode.CheckRepublic.Website.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Knapcode.CheckRepublic.Website.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = AuthorizationConstants.ReadPolicy)]
    public class CheckBatchesController : Controller
    {
        private readonly ICheckBatchService _service;

        public CheckBatchesController(ICheckBatchService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<CheckBatch>> GetCheckBatchesAsync(
            int skip = 0,
            int take = 10,
            bool asc = false,
            CancellationToken token = default(CancellationToken))
        {
            var checkBatches = await _service.GetCheckBatchesAsync(skip, take, asc, token);

            return checkBatches;
        }
        
        [HttpGet("id:{id}")]
        public async Task<IActionResult> GetCheckBatchByIdAsync(int id, CancellationToken token)
        {
            var checkBatch = await _service.GetCheckBatchByIdAsync(id, token);

            if (checkBatch == null)
            {
                return NotFound();
            }

            return new ObjectResult(checkBatch);
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestCheckBatchAsync(CancellationToken token)
        {
            var checkBatch = await _service.GetLatestCheckBatchAsync(token);

            if (checkBatch == null)
            {
                return NotFound();
            }

            return new ObjectResult(checkBatch);
        }
    }
}
