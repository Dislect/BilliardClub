using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BilliardClub.Controllers
{
    [Authorize(Roles = "user,employee")]
    public class LKController : Controller
    {
        [HttpGet]
        public ActionResult Info()
        {
            return View();
        }
    }
}