using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace EServices.Controllers
{
    [OutputCache(Duration = 86400, VaryByParam = "none", Location = OutputCacheLocation.Client)]
    public class LayoutSettingController : Controller
    {
        // GET: LayoutSetting
        public ActionResult Header()
        {
            return View();
        }
        public ActionResult SideBar()
        {
            return View();
        }
    }
}