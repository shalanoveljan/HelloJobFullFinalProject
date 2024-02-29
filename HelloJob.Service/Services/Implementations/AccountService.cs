﻿using AutoMapper;
using HelloJob.Core.Helper.MailHelper;
using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Core.Utilities.Results.Concrete;
using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.DTOS.User;
using HelloJob.Entities.Models;
using HelloJob.Service.Responses;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IEmailHelper _emailService;
        private readonly IHttpContextAccessor _http;
        private readonly IConfiguration _configuration;
        readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUrlHelper _helper;


        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IMapper mapper, IEmailHelper emailService, IHttpContextAccessor http, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IUrlHelper helper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailService = emailService;
            _http = http;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _helper = helper;
        }
        public async Task<Core.Utilities.Results.Abstract.IDataResult<string>> SignUp(RegisterDto dto, string role)
        {

            var isValidEmail = _emailService.IsValidEmail(dto.Email);
            if (!isValidEmail) return new ErrorDataResult<string>(message: "Invalid Email!");
            var checkUser = await _userManager.FindByEmailAsync(dto.Email);
            if (checkUser != null) return new ErrorDataResult<string>(message: "Email is already used!");
            AppUser appUser = new AppUser()
            {
                UserName = dto.Username,
                IsActivate = true
            };
            var result = await _userManager.CreateAsync(appUser, dto.Password);
            if (!result.Succeeded) return new ErrorDataResult<string>(message: string.Join('\n', result.Errors.Select(x => x.Description)));

            var hasUserRole = await _userManager.IsInRoleAsync(appUser, role);
            if (!hasUserRole)
                await _userManager.AddToRoleAsync(appUser, role);
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

            var url = $"{_http.HttpContext.Request.Scheme}://{_http.HttpContext.Request.Host}{_helper.Action("VerifyEmail", "Identity", new { email = appUser.Email, token = token })}";

            await _emailService.SendEmailAsync(appUser.Email, url, "Verify Email", token);

            return new SuccessDataResult<string>(
                message: "RegisterDto olundu"
                );
        }
        public async Task<Core.Utilities.Results.Abstract.IResult> Login(LoginDto dto)
        {
            try
            {
                AppUser checkUser = await _userManager.FindByNameAsync(dto.UserNameOrEmail);

                if (checkUser == null)
                {
                    checkUser = await _userManager.FindByEmailAsync(dto.UserNameOrEmail);
                }

                if (checkUser == null)
                    return new ErrorResult("User not Found!");

                if (!checkUser.EmailConfirmed)
                    return new ErrorResult("Please verify this email before sign in!");

                if (!checkUser.IsActivate)
                    return new ErrorResult("Your account is blocked! Please contact the administrator.");

                var result = await _signInManager.PasswordSignInAsync(checkUser, dto.Password, dto.RememberMe, true);

                if (!result.Succeeded)
                    return new ErrorResult("Email or Password is incorrect!");

                if (!result.IsLockedOut)
                    return new ErrorResult("User is locked out!");

                if (!result.IsNotAllowed)
                    return new ErrorResult("User is not allowed to sign in!");

                return new SuccessResult("Login Successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }


        }
        public async Task<Core.Utilities.Results.Abstract.IResult> VerifyEmail(string token, string email)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(email);
            if (appUser == null) return new ErrorResult("User tapilmadi");
            var res = await _userManager.ConfirmEmailAsync(appUser, token);
            if (!res.Succeeded)
            {
                var errors = string.Join(", ", res.Errors.Select(e => e.Description));
                return new ErrorResult($"Confirm Email is invalid : {errors}");
            }
            appUser.EmailConfirmed = true;
            await _signInManager.SignInAsync(appUser, true);
            return new SuccessResult("Email tesdiq olundu ve signin olundu");
        }

        public async Task<Core.Utilities.Results.Abstract.IResult> LogOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return new SuccessResult();
            }

            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        //private async Task FirstRegisterAsync()
        //{
        //        var users = _userManager.Users.ToList();
        //        if (users.Count != 1) return;
        //        var user = users[0];
        //        await _userManager.AddToRoleAsync(user, "SuperAdmin");
        //}
        public async Task<Core.Utilities.Results.Abstract.IResult> ForgetPassword(string email)
        {
            if (email is null)
            {
                return new ErrorResult("please enter email!");
            }
            var isValidEmail = _emailService.IsValidEmail(email);
            if (!isValidEmail) return new ErrorResult("Invalid Email!");
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null) return new ErrorResult("Email is already used!");
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (token is null) return new ErrorResult("token is not generated");
            var url = $"{_http.HttpContext.Request.Scheme}://{_http.HttpContext.Request.Host}{_helper.Action("ResetPassword", "Identity", new { email = user.Email, token = token })}";

            await _emailService.SendEmailAsync(user.Email, url, "Verify Email for resetpassword", token);
            return new SuccessResult();

        }
        public async Task<Core.Utilities.Results.Abstract.IDataResult<ResetPasswordDto>> ResetPasswordGet(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return new ErrorDataResult<ResetPasswordDto>("User not found");
            }
            ResetPasswordDto dto = new ResetPasswordDto()
            {
                Email = email,
                Token = token
            };

            return new SuccessDataResult<ResetPasswordDto>(dto, "get resetPassword");
        }

        public async Task<Core.Utilities.Results.Abstract.IResult> ResetPasswordPost(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return new ErrorResult("User not Found!");
            }

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password);
            if (!result.Succeeded)
            {
                string errors = string.Join("\n", result.Errors.Select(error => error.Description));
                return new ErrorResult("Reset password failed: " + errors);
            }

            return new SuccessResult("Reset password success");
        }

        public async Task<Core.Utilities.Results.Abstract.IResult> Update(UpdateDto dto)
        {
            var user = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);
            if (user == null)
            {
                return new ErrorResult("User not found!");
            }

            if (!string.IsNullOrEmpty(dto.Email))
            {
                user.Email = dto.Email;
            }

            if (!string.IsNullOrEmpty(dto.UserName))
            {
                user.UserName = dto.UserName;
            }

            if (!string.IsNullOrEmpty(dto.OldPassword) && !string.IsNullOrEmpty(dto.NewPassword))
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    string errors = string.Join("\n", changePasswordResult.Errors.Select(error => error.Description));
                    return new ErrorResult(errors);
                }
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                string errors = string.Join("\n", result.Errors.Select(error => error.Description));
                return new ErrorResult(errors);
            }

            return new SuccessResult("User updated successfully");
        }

        public async Task<Core.Utilities.Results.Abstract.IResult> ChangeUserActivationStatus(string email, bool activate)
        {
            var user = await _userManager.FindByIdAsync(email);
            if (user == null)
            {
                return new ErrorResult("User not found!");
            }

            if (activate)
            {
                user.IsActivate = true;
            }
            else
            {
                user.IsActivate = false;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                string errors = string.Join("\n", result.Errors.Select(error => error.Description));
                return new ErrorResult(errors);
            }

            return new SuccessResult("User activation status changed successfully");
        }



        public async Task<PagginatedResponse<AppUser>> GetAllUsers(int count, int page)
        {
            try
            {
                IQueryable<AppUser> query = _userManager.Users;

                if (count > 0 && page > 0)
                {
                    query = query.Where(user =>
                        _userManager.IsInRoleAsync(user, "Owner").Result ||
                        _userManager.IsInRoleAsync(user, "Employee").Result
                    );
                }

                int totalCount = await query.CountAsync();
                List<AppUser> users = await query.Skip((page - 1) * count).Take(count).ToListAsync();

                var response = new PagginatedResponse<AppUser>(
                    datas: users,
                    pageNumber: page,
                    pageSize: count,
                    totalCount: totalCount,
                    otherdatas: null
                );

                return response;
            }
            catch (Exception ex)
            {
                return new PagginatedResponse<AppUser>(
                    datas: null,
                    pageNumber: 0,
                    pageSize: 0,
                    totalCount: 0,
                    otherdatas: null
                );
            }
        }



        public async Task<PagginatedResponse<AppUser>> GetAllAdmin(int count, int page)
        {
            try
            {
                IQueryable<AppUser> query = _userManager.Users;

                if (count > 0 && page > 0)
                {
                    query = query.Where(user =>
                        _userManager.IsInRoleAsync(user, "Admin").Result
                    );
                }
                int totalCount = await query.CountAsync();
                List<AppUser> users = await query.Skip((page - 1) * count).Take(count).ToListAsync();

                var response = new PagginatedResponse<AppUser>(
                    datas: users,
                    pageNumber: page,
                    pageSize: count,
                    totalCount: totalCount,
                    otherdatas: null
                );

                return response;
            }
            catch (Exception ex)
            {
                return new PagginatedResponse<AppUser>(
                    datas: null,
                    pageNumber: 0,
                    pageSize: 0,
                    totalCount: 0,
                    otherdatas: null
                );
            }
        }


    }
}