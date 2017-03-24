using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using StackdriverLogging;
using Google.Cloud.Logging.V2;

namespace GoogleAspNetCoreMvc_Test.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var msg = ApiRandomLog.WriteRandomEntry();
            return Content("msg");
            //return View();
        }

        public IActionResult About()
        {
            return Content("About");
        }

        public IActionResult Contact()
        {
            return Content("Contact");
        }

        public IActionResult Error()
        {
            return Content("Error");
        }
    }
}
