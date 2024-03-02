using HelloJob.Entities.DTOS;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HelloJob.App.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "Employee,Owner")]
    public class ResumeController : Controller
    {
        readonly IResumeService _ResumeService;
        readonly ICategoryService _categoryService;
        readonly IEducationService _educationService;
        readonly ILanguageService _languageService;
        public ResumeController(IResumeService ResumeService, ICategoryService categoryService, IEducationService educationService, ILanguageService languageService)
        {
            _ResumeService = ResumeService;
            _categoryService = categoryService;
            _educationService = educationService;
            _languageService = languageService;
        }

        public async Task<IActionResult> Index(string userid,int page = 1,int pagesize=6)
        {
            return View(await _ResumeService.GetAllAsync(userid,false,page,pagesize));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.Educations = await _educationService.GetAllAsync();
            ViewBag.Languages = await _languageService.GetAllAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResumePostDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.Educations = await _educationService.GetAllAsync();
                ViewBag.Languages = await _languageService.GetAllAsync();
                return View();
            }
            var response = await _ResumeService.CreateAsync(dto);


            if (!response.Success)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.Educations = await _educationService.GetAllAsync();
                ViewBag.Languages = await _languageService.GetAllAsync();
                ModelState.AddModelError("", response.Message);
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {

            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.Educations = await _educationService.GetAllAsync();
            ViewBag.Languages = await _languageService.GetAllAsync();
            var res = await _ResumeService.GetAsync(id);
            return View(res.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ResumePostDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.Educations = await _educationService.GetAllAsync();
                ViewBag.Languages = await _languageService.GetAllAsync();

                var res = await _ResumeService.GetAsync(id);
                return View(res.Data);
            }
            var response = await _ResumeService.UpdateAsync(id, dto);

            if (!response.Success)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.Educations = await _educationService.GetAllAsync();
                ViewBag.Languages = await _languageService.GetAllAsync();
                ModelState.AddModelError("", response.Message);
                var res = await _ResumeService.GetAsync(id);
                return View(res.Data);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            var res = await _ResumeService.RemoveAsync(id);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
