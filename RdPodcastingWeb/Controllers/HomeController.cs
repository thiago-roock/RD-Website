
using System.Web.Mvc;
namespace RdPodcastingWeb.Controllers
{
    public class HomeController : Controller
    {

        [OutputCache(CacheProfile = "CacheLong")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sobre()
        {
            return View();
        }
        public ActionResult Episodios()
        {
            return View();
        }
        public ActionResult Contato()
        {
            return View();
        }

    }
}