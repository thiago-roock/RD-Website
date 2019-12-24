using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RdPodcastingWeb.Controllers
{
    [OutputCache(CacheProfile = "CacheLong")]
    public class HomeController : Controller
    {
       
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
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contato()
        {
            return View();
        }

        public ActionResult teste()
        {
            return View();
        }
    }
}