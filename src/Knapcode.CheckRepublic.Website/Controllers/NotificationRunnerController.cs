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
    [Authorize(Policy = AuthorizationConstants.WritePolicy)]
    public class NotificationRunnerController : Controller
    {
        private readonly INotificationRunnerService _service;

        public NotificationRunnerController(INotificationRunnerService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IEnumerable<CheckNotification>> RunAsync(CancellationToken token)
        {
            var notifications = await _service.RunAsync(token);

            return notifications;
        }
    }
}
