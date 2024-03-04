﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HelloJob.App.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "Employee,Owner")]
    public class ProfilimController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
