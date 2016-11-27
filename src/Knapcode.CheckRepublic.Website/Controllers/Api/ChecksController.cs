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
    public class ChecksController : Controller
    {
        private readonly ICheckService _checkService;
        private readonly ICheckResultService _checkResultService;

        public ChecksController(ICheckService checkService, ICheckResultService checkResultService)
        {
            _checkService = checkService;
            _checkResultService = checkResultService;
        }

        [HttpGet]
        public async Task<IEnumerable<Check>> GetChecksAsync(
            int skip = 0,
            int take = 10,
            bool asc = true,
            CancellationToken token = default(CancellationToken))
        {
            var checks = await _checkService.GetChecksAsync(skip, take, asc, token);

            return checks;
        }

        [HttpGet("id:{id}")]
        public async Task<IActionResult> GetCheckByIdAsync(int id, CancellationToken token)
        {
            var check = await _checkService.GetCheckByIdAsync(id, token);

            if (check == null)
            {
                return NotFound();
            }

            return new ObjectResult(check);
        }

        [HttpGet("name:{name}")]
        public async Task<IActionResult> GetCheckByNameAsync(string name, CancellationToken token)
        {
            var check = await _checkService.GetCheckByNameAsync(name, token);

            if (check == null)
            {
                return NotFound();
            }

            return new ObjectResult(check);
        }

        [HttpGet("id:{id}/checkresults")]
        public async Task<IEnumerable<CheckResult>> GetCheckResultsByCheckIdAsync(
            int id,
            int skip = 0,
            int take = 10,
            bool asc = false,
            CancellationToken token = default(CancellationToken))
        {
            var checkResults = await _checkResultService.GetCheckResultsByCheckIdAsync(id, skip, take, asc, token);

            return checkResults;
        }

        [HttpGet("name:{name}/checkresults")]
        public async Task<IEnumerable<CheckResult>> GetCheckResultsByCheckNameAsync(
            string name,
            int skip = 0,
            int take = 10,
            bool asc = false,
            CancellationToken token = default(CancellationToken))
        {
            var checkResults = await _checkResultService.GetCheckResultsByCheckNameAsync(name, skip, take, asc, token);

            return checkResults;
        }

        [HttpGet("name:{name}/checkresults/type:failure")]
        public async Task<IEnumerable<CheckResult>> GetFailureCheckResultsByCheckNameAsync(
            string name,
            int skip = 0,
            int take = 10,
            bool asc = false,
            CancellationToken token = default(CancellationToken))
        {
            var checkResults = await _checkResultService.GetFailureCheckResultsByCheckNameAsync(name, skip, take, asc, token);

            return checkResults;
        }

        [HttpGet("id:{id}/checkresults/type:failure")]
        public async Task<IEnumerable<CheckResult>> GetFailureCheckResultsByCheckIdAsync(
            int id,
            int skip = 0,
            int take = 10,
            bool asc = false,
            CancellationToken token = default(CancellationToken))
        {
            var checkResults = await _checkResultService.GetFailureCheckResultsByCheckIdAsync(id, skip, take, asc, token);

            return checkResults;
        }
    }
}
