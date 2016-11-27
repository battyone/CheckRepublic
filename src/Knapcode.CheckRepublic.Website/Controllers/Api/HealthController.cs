using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business;
using Knapcode.CheckRepublic.Logic.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Knapcode.CheckRepublic.Website.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class HealthController : Controller
    {
        private readonly IHealthService _service;

        public HealthController(IHealthService service)
        {
            _service = service;
        }
        
        [HttpGet("runner")]
        public async Task<RunnerHealth> GetRunnerHealthAsync(CancellationToken token)
        {
            var runnerHealth = await _service.GetRunnerHealthAsync(token);

            Response.StatusCode = runnerHealth.IsHealthy ? 200 : 500;

            return runnerHealth;
        }
    }
}
