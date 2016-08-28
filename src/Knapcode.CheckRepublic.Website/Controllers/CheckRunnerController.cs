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
    [Authorize(Policy = AuthorizationConstants.WritePolicy)]
    public class CheckRunnerController : Controller
    {
        private readonly ICheckRunnerService _service;

        public CheckRunnerController(ICheckRunnerService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<CheckBatch> RunAsync(CancellationToken token)
        {
            var checkBatch = await _service.RunAsync(token);

            return checkBatch;
        }
    }
}
