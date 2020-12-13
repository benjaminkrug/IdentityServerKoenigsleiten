namespace IdentityServer.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
