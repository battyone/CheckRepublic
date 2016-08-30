using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business;
using Knapcode.CheckRepublic.Logic.Entities;
using Knapcode.CheckRepublic.Website.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Knapcode.CheckRepublic.Website.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = AuthorizationConstants.ReadPolicy)]
    public class CheckResultsController : Controller
    {
        private readonly ICheckResultService _service;

        public CheckResultsController(ICheckResultService service)
        {
            _service = service;
        }

        [HttpGet("failures")]
        public async Task<IEnumerable<CheckResult>> GetFailedCheckResultsAsync(
            int skip = 0,
            int take = 10,
            bool asc = false,
            CancellationToken token = default(CancellationToken))
        {
            var checkResults = await _service.GetFailedCheckResultsAsync(skip, take, asc, token);

            return checkResults;
        }
    }
}
