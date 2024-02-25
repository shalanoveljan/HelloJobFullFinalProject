using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelloJob.App.Controllers
{
    public class BlogController : Controller
    {
        readonly IBlogService _blogService;
        readonly ICategoryService _categoryService;
        readonly HelloJobDbContext _context;

        public BlogController(IBlogService blogService, ICategoryService categoryService, HelloJobDbContext context)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _context = context;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int PageSize = 6)
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(await _blogService.GetAllAsync(pageNumber,PageSize));
        }
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _blogService.GetAsync(id);

            await IncreaseCount(id);


            ViewBag.Blogs = await _blogService.GetAllAsync();
            ViewBag.Categories = await _categoryService.GetAllAsync();

            return View(res.Data);
        } 
        private async Task IncreaseCount(int id)
        {
            var res = await _blogService.GetAsync(id);

            res.Data.ViewCount++;
            await _context.SaveChangesAsync();
        }


    }
}
