using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Business;
using Microsoft.AspNetCore.Mvc;

namespace Knapcode.CheckRepublic.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICheckBatchService _service;

        public HomeController(ICheckBatchService service)
        {
            _service = service;
        }
        
        public async Task<IActionResult> Index(CancellationToken token)
        {
            var batch = await _service.GetLatestCheckBatchAsync(token);

            return View(batch);
        }
    }
}
