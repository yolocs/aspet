using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dto = aspet.Models;
using dao = aspet.Daos;
using Microsoft.AspNetCore.Authorization;

namespace aspet.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private dao.ITasks tasks;

        public HomeController(dao.ITasks tasks, ILogger<HomeController> logger)
        {
            this.tasks = tasks;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var email = User.Claims.FirstOrDefault().Value;
            var tasks = this.tasks.List(email);
            return View(tasks);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new dto.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
