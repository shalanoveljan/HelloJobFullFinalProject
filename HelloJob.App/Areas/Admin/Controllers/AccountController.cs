﻿using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloJob.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]

    public class AccountController : Controller
    {
        
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IAccountService accountService, UserManager<AppUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllAdmin(int page=1, int count=4)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var res = await _accountService.GetAllAdmin(page,count);
            return View(res.Datas);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllUsers(int page = 1, int count = 4)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var res = await _accountService.GetAllUsers(page, count);
            return View(res.Datas);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> VerifyEmail(string token, string email)
        {
            var result = await _accountService.VerifyEmail(token, email);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                return View();
            }
            TempData["Verify"] = "Succesfully SignUp";
            return RedirectToAction("index", "home");
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]

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
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> LogOut()
        {
            var result = await _accountService.LogOut();
            return RedirectToAction("index", "home");
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
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
        [Authorize(Roles = "SuperAdmin,Admin")]

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
        [Authorize(Roles = "SuperAdmin,Admin")]

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
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> Update()
        {
            var query = _userManager.Users.Where(x => x.UserName == User.Identity.Name);
            UpdateDto? updateDto = await query.Select(x => new UpdateDto { UserName = x.UserName })
                .FirstOrDefaultAsync();
            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "SuperAdmin,Admin")]


        public async Task<IActionResult> Update(UpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var res = await _accountService.Update(dto);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
                return View(dto);
            }
            TempData["update"] = "updated Account";


            return RedirectToAction(nameof(Update));

        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ChangeActivationStatus(string email, bool activate)
        {
            var result = await _accountService.ChangeUserActivationStatus(email, activate);

            if (result.Success)
            {
                TempData["update"] = "changed Activated status";
                return NoContent();

            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
