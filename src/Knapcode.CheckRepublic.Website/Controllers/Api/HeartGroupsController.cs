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
    public class HeartGroupsController : Controller
    {
        private readonly IHeartbeatService _heartbeatService;
        private readonly IHeartGroupService _heartGroupService;

        public HeartGroupsController(IHeartGroupService heartGroupService, IHeartbeatService heartbeatService)
        {
            _heartGroupService = heartGroupService;
            _heartbeatService = heartbeatService;
        }

        [HttpGet]
        public async Task<IEnumerable<HeartGroup>> GetHeartGroupsAsync(
            int skip = 0,
            int take = 10,
            bool asc = true,
            CancellationToken token = default(CancellationToken))
        {
            var heartGroups = await _heartGroupService.GetHeartGroupsAsync(skip, take, asc, token);

            return heartGroups;
        }

        [HttpGet("name:{heartGroupName}/heartbeats")]
        public async Task<IEnumerable<Heartbeat>> GetHeartbeatsByHeartGroupNameAsync(
            string heartGroupName,
            int skip = 0,
            int take = 1,
            bool asc = false,
            CancellationToken token = default(CancellationToken))
        {
            var heartbeats = await _heartGroupService.GetHeartbeatsByHeartGroupName(heartGroupName, skip, take, asc, token);

            return heartbeats;
        }

        [HttpPost("name:{heartGroupName}/hearts/name:{heartName}/heartbeats")]
        [Authorize(Policy = AuthorizationConstants.WritePolicy)]
        public async Task<Heartbeat> CreatHeartbeatAsync(string heartGroupName, string heartName, CancellationToken token)
        {
            var heartbeat = await _heartbeatService.CreateHeartbeatAsync(heartGroupName, heartName, token);

            return heartbeat;
        }
    }
}
