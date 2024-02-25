using HelloJob.App.ViewModels;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HelloJob.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogService _blogService;

        public HomeController(ILogger<HomeController> logger, IBlogService blogService)
        {
            _logger = logger;
            _blogService = blogService;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int PageSize = 8)
        {
            HomeVM homeVM = new HomeVM
            {
                blogs = await _blogService.GetAllAsync(pageNumber, PageSize)
            };
            return View(homeVM);
        }


    }
}