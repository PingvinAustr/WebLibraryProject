using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreLibraryProj.Controllers
{
    public class StatsController : Controller
    {
        // GET: StatsController
        public ActionResult Index()
        {
            return View();
        }

    }
}
