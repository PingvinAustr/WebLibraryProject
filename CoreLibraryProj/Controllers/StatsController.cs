using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreLibraryProj.Controllers
{
    public class StatsController : Controller
    {
        // GET: StatsController
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated == false) { return Redirect("~/Home"); }
            return View();
        }

    }
}
