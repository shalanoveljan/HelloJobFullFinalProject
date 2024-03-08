using HelloJob.Core.Helper.MailHelper;
using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Enums;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Implementations;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        readonly IEmailHelper _emailHelper;
        public ResumeController(IResumeService ResumeService, ICategoryService categoryService, IEducationService educationService, ILanguageService languageService, IEmailHelper emailHelper)
        {
            _ResumeService = ResumeService;
            _categoryService = categoryService;
            _educationService = educationService;
            _languageService = languageService;
            _emailHelper = emailHelper;
        }

        public async Task<IActionResult> Index(string userid=null,int page = 1, int pagesize = 6)
        {
            return View(await _ResumeService.GetAllAsync(userid,true,page, pagesize));
        }

        public async Task<ResumeGetDto> GetResumeById(int resumeId)
        {
            var res = await _ResumeService.GetAsync(resumeId);
            return res.Data ;
        }
        public async Task<IActionResult> Accept(int resumeid)
        {
            var result = await ProcessOrderStatus(resumeid, "Muracietiniz qebul olundu");

            if (result.Success)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        public async Task<IActionResult> Reject(int resumeid)
        {
            var result = await ProcessOrderStatus(resumeid, "Muracietiniz red edildi");

            if (result.Success)
            {
                return Ok(result.Message);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        private async Task<HelloJob.Core.Utilities.Results.Abstract.IResult> ProcessOrderStatus(int resumeid, string emailSubject)
        {
            var resume = await GetResumeById(resumeid);

            if (resume == null)
            {
                return new ErrorResult("Resume tapilmadi");
            }

            var userEmail = resume.AppUser.Email;
            var result = await _ResumeService.SetOrderStatus(resumeid, emailSubject == "Muracietiniz qebul olundu" ? Order.Accept : Order.Reject);

            if (result.Success)
            {
                var notificationResult = await _emailHelper.SendNotificationEmailAsync(userEmail, emailSubject, emailSubject);

                if (notificationResult.Success)
                {
                    return new SuccessResult(result.Message);
                }
                else
                {
                    return new ErrorResult("Melumatlandirici e-postası gönderilmedi");
                }
            }
            else
            {
                return new ErrorResult(result.Message);
            }
        }

    }
}
