using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Private_Gax_Log4Net_AspNetMvc.Controllers
{
    public class PeanutController : Controller
    {
        private static log4net.ILog s_logger = log4net.LogManager.GetLogger(typeof(PeanutController));

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
            for (int i = 0; i < count; ++i)
            {
                s_logger.Info($"Log a message {i} {DateTime.UtcNow.ToString("O")}");
            }

            return Content(count.ToString());
        }
    }
}