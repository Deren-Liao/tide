using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Template_AspNetMvc.Controllers
{
    public class PeanutController : Controller
    {
        // GET: Peanut
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Peanut/Log/1
        [HttpGet]
        public ActionResult LogDefault()
        {
            return Content("Yes, Log()");
        }

        // GET: /Peanut/Log/1
        [HttpGet]
        public ActionResult Log(int? count)
        {
            return Content(count.ToString());
        }
    }
}