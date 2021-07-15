using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ordering.Calc.Api.Controllers
{
    public class HomeController : Controller
    {
        public JsonResult Index(int x=5,int y=5)
            => Json(new { result = x + y }, JsonRequestBehavior.AllowGet);
        
    }
}
