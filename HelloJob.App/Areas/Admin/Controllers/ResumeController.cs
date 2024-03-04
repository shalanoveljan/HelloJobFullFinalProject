using HelloJob.Entities.DTOS;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HelloJob.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
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

        public async Task<IActionResult> Index(int page = 1,int pagesize=6)
        {

            return View(await _ResumeService.GetAllAsync(page,pagesize));
        }

    }
}
