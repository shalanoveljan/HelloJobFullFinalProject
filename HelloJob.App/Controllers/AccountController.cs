using HelloJob.Core.Utilities.Results.Concrete;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.DTOS.User;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloJob.App.Controllers
{
    public class AccountController : Controller
    {
        readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;


        public AccountController(IAccountService accountService, UserManager<AppUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Register(RegisterDto dto, string role)
        {
            if(!ModelState.IsValid)
            {
                return PartialView("_RegisterPartialView",dto);
            }
           var res= await _accountService.SignUp(dto, role);
            if (!res.Success)
            {
                ViewData["Error"] = res.Message;
                return PartialView("_RegisterPartialView", dto);

            }
            TempData["Register"] = "Please verify your email";
            return RedirectToAction("index", "Home");
        }

        public async Task<IActionResult> VerifyEmail(string token, string email)
        {
            var result =await _accountService.VerifyEmail(token, email);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                return View();
            }
            TempData["Verify"] = "Succesfully SignUp";
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {

            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var result = await _accountService.Login(dto);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                return View(dto);
            }
            return RedirectToAction("index", "home");
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            var result = await _accountService.LogOut();
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(string mail)
        {
            if (!ModelState.IsValid)
            {
                return View(mail);
            }
            var result = await _accountService.ForgetPassword(mail);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(mail);
            }
            return RedirectToAction("index", "home");
        }


        [HttpGet]
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            if (!ModelState.IsValid)
            {
                return View(email);
            }

            var result = await _accountService.ResetPasswordGet(email, token);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(email);
            }
            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var result = await _accountService.ResetPasswordPost(dto);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(dto);
            }

            return RedirectToAction("login", "Account");
        }

        [Authorize]
        public async Task<IActionResult> Update()
        {
            var query = _userManager.Users.Where(x => x.UserName == User.Identity.Name);
            UpdateDto? updateDto = await query.Select(x => new UpdateDto { UserName = x.UserName })
                .FirstOrDefaultAsync();
            return View(updateDto);
        }

    }
}
