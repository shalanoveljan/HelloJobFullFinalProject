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

  
        public async Task<IActionResult> Index(List<CourseGetDto>? courses)
        {
       
            ViewBag.Categories = await _categoryService.GetAllAsync();
            if (courses.Count!=0)
            {

                return View(courses);
            }
            else
            { 
            var result = await _CourseService.GetAllForCoursePageInWebSiteAsync();
            return View(result.Data);
            }
        }
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _CourseService.GetAsync(id);
            ViewBag.Tags = await _tagService.GetAllAsync();
            ViewBag.Categories = await _categoryService.GetAllAsync();

            return View(res.Data);
        } 

        public async Task<IActionResult> SortCourses(int id)
        {
            var res = await _CourseService.SortCourses(id);
            Index(res.Data);
            return Json(res.Data);
        }
        //public async Task<IActionResult> FilterCourses(int id)
        //{
        //    var res = await _CourseService.FilterCourses(id);
        //    return Json(res.Data);
        //}



    }
}
