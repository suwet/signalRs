using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using DemoIntegrateWithAspNetMvc.Models;

namespace DemoIntegrateWithAspNetMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<NotiHub> _notiHub;

        public HomeController(ILogger<HomeController> logger,IHubContext<NotiHub> notiHub)
        {
            _logger = logger;
            _notiHub = notiHub;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowNoti()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Buy(ItemModel model)
        {
            _notiHub.Clients.All.SendAsync("ReceiveMessage",model.ItemName,model.Price.ToString());
            return RedirectToAction("index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
