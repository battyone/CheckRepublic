using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business;
using Knapcode.CheckRepublic.Logic.Entities;
using Knapcode.CheckRepublic.Website.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Knapcode.CheckRepublic.Website.Controllers
{
    [Route("api/[controller]")]
    [TypeFilter(typeof(ReadAuthorizationFilter))]
    public class ChecksController : Controller
    {
        private readonly ICheckService _service;

        public ChecksController(ICheckService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Check>> GetChecksAsync(
            int skip = 0,
            int take = 10,
            bool asc = true,
            CancellationToken token = default(CancellationToken))
        {
            var checks = await _service.GetChecksAsync(skip, take, asc, token);

            return checks;
        }

        [HttpGet("id:{id}")]
        public async Task<IActionResult> GetCheckByIdAsync(int id, CancellationToken token)
        {
            var check = await _service.GetCheckByIdAsync(id, token);

            if (check == null)
            {
                return NotFound();
            }

            return new ObjectResult(check);
        }

        [HttpGet("name:{name}")]
        public async Task<IActionResult> GetCheckByNameAsync(string name, CancellationToken token)
        {
            var check = await _service.GetCheckByNameAsync(name, token);

            if (check == null)
            {
                return NotFound();
            }

            return new ObjectResult(check);
        }

        [HttpGet("id:{id}/checkresults")]
        public async Task<IEnumerable<CheckResult>> GetCheckResultsByIdAsync(
            int id,
            int skip = 0,
            int take = 10,
            bool asc = false,
            CancellationToken token = default(CancellationToken))
        {
            var checkResults = await _service.GetCheckResultsByIdAsync(id, skip, take, asc, token);

            return checkResults;
        }

        [HttpGet("name:{name}/checkresults")]
        public async Task<IEnumerable<CheckResult>> GetCheckResultsByNameAsync(
            string name,
            int skip = 0,
            int take = 10,
            bool asc = false,
            CancellationToken token = default(CancellationToken))
        {
            var checkResults = await _service.GetCheckResultsByNameAsync(name, skip, take, asc, token);

            return checkResults;
        }
    }
}
