using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Implementations;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelloJob.App.Controllers
{

    public class VacancyController : Controller
    {
        readonly IVacancyService _VacancyService;
        readonly ICategoryService _categoryService;
        readonly ICityService _cityService;
        readonly ICompanyService _companyService;
        readonly ILikedService _likeService;

        public VacancyController(IVacancyService VacancyService, ICategoryService categoryService, ILikedService likeService, ICompanyService companyService, ICityService cityService)
        {
            _VacancyService = VacancyService;
            _categoryService = categoryService;
            _likeService = likeService;
            _companyService = companyService;
            _cityService = cityService;
        }


        public async Task<IActionResult> Index(string userid = null, int page = 1, int pagesize = 6)
        {

            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.Cities = await _cityService.GetAllAsync();
        
                var result = await _VacancyService.GetAllAsync(userid,false,page,pagesize);
                return View(result.Datas);
        }
        public async Task<IActionResult> Detail(int id)
        {
            ViewBag.Vacancys = await _VacancyService.GetAllForVacancyPageInWebSiteAsync();
            var res = await _VacancyService.GetAsync(id);
            await _VacancyService.IncreaseCount(id);
            return View(res.Data);
        }

        public async Task<IActionResult> Wishlist()
        {
            
                Wishlist wishlist = await _likeService.GetWishList();
                if (wishlist is null)
                {
                    return Json("Wishlist is null");
                }

                return View(wishlist);
        }
        [HttpPost]
        public async Task<IActionResult> AddWishlist(int itemid, string itemtype)
        {
            await _likeService.AddToWishlist(itemid,itemtype);
            return Json(new { status = 200 });
        }

    }
}
