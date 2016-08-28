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
    public class HeartGroupsController : Controller
    {
        private readonly IHeartbeatService _service;

        public HeartGroupsController(IHeartbeatService service)
        {
            _service = service;
        }

        [HttpPost("name:{heartGroupName}/hearts/name:{heartName}/heartbeats")]
        public async Task<Heartbeat> CreatHeartbeatAsync(string heartGroupName, string heartName, CancellationToken token)
        {
            var heartbeat = await _service.CreateHeartbeatAsync(heartGroupName, heartName, token);

            return heartbeat;
        }
    }
}
