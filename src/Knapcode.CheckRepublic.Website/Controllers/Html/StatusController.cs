using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business;
using Knapcode.CheckRepublic.Website.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Knapcode.CheckRepublic.Website.Controllers
{
    public class StatusController : Controller
    {
        private readonly ICheckBatchService _checkBatchService;
        private readonly ICheckResultService _checkResultService;
        private readonly ICheckService _checkService;

        public StatusController(
            ICheckBatchService checkBatchService,
            ICheckService checkService,
            ICheckResultService checkResultService)
        {
            _checkBatchService = checkBatchService;
            _checkService = checkService;
            _checkResultService = checkResultService;
        }
        
        public async Task<IActionResult> LatestCheckBatch(CancellationToken token)
        {
            var batch = await _checkBatchService.GetLatestCheckBatchAsync(token);

            return View(batch);
        }
        
        public async Task<IActionResult> Check(
            string name,
            int skip = 0,
            int take = 20,
            CancellationToken token = default(CancellationToken))
        {
            var check = await _checkService.GetCheckByNameAsync(name, token);
            var results = await _checkResultService.GetCheckResultsByCheckNameAsync(
                name,
                skip,
                take,
                asc: false,
                token: token);

            return View(new CheckStatus
            {
                CheckName = check.Name,
                Items = results.ToList()
            });
        }
    }
}
