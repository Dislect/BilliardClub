using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilliardClub.Controllers
{
    public class MenuController : Controller
    {
        public ActionResult Catalog()
        {
            return View();
        }
    }
}
