using HelloJob.Entities.DTOS;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HelloJob.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ExperienceController : Controller
    {
        readonly IExperienceService _ExperienceService;
        public ExperienceController(IExperienceService ExperienceService)
        {
            _ExperienceService = ExperienceService;
        }

        public async Task<IActionResult> Index(int page = 1,int pagesize=6)
        {

            return View(await _ExperienceService.GetAllAsync(page,pagesize));
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExperiencePostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var response = await _ExperienceService.CreateAsync(dto);

            if (!response.Success)
            {
                ModelState.AddModelError("", response.Message);
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var res = await _ExperienceService.GetAsync(id);
            return View(res.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ExperiencePostDto dto)
        {
            if (!ModelState.IsValid)
            {
                var res = await _ExperienceService.GetAsync(id);
                return View(res.Data);
            }
            var response = await _ExperienceService.UpdateAsync(id, dto);

            if (!response.Success)
            {
                ModelState.AddModelError("", response.Message);
                var res = await _ExperienceService.GetAsync(id);
                return View(res.Data);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            var res = await _ExperienceService.RemoveAsync(id);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
