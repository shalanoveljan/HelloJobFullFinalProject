using HelloJob.Entities.DTOS;
using HelloJob.Entities.Enums;
using HelloJob.Service.Services.Implementations;
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

        public async Task<IActionResult> Index(string userid=null,int page = 1, int pagesize = 6)
        {
            return View(await _ResumeService.GetAllAsync(userid,true,page, pagesize));
        }
        public async Task<IActionResult> Accept(int id)
        {
            var result = await _ResumeService.SetOrderStatus(id, Order.Accept);

            if (result.Success)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _ResumeService.SetOrderStatus(id, Order.Reject);

            if (result.Success)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
