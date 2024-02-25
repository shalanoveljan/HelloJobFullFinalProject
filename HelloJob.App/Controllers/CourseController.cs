using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelloJob.App.Controllers
{
    public class CourseController : Controller
    {
        readonly ICourseService _CourseService;
        readonly ICategoryService _categoryService;
        readonly ITagService _tagService;

        public CourseController(ICourseService CourseService, ICategoryService categoryService, ITagService tagService)
        {
            _CourseService = CourseService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int PageSize = 6)
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(await _CourseService.GetAllAsync(pageNumber,PageSize));
        }
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _CourseService.GetAsync(id);
            ViewBag.Tags = await _tagService.GetAllAsync();
            ViewBag.Categories = await _categoryService.GetAllAsync();

            return View(res.Data);
        } 
        


    }
}
