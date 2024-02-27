using AutoMapper;
using HelloJob.Core.Helper.MailHelper;
using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.DTOS.User;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public  class AccountService:IAccountService
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
        public async Task<Core.Utilities.Results.Abstract.IDataResult<string>> SignUp(RegisterDto dto)
        {
           
            var isValidEmail = _emailService.IsValidEmail(dto.Email);
            if (!isValidEmail)  return new ErrorDataResult<string>(message: "Invalid Email!");
            var checkUser = await _userManager.FindByEmailAsync(dto.Email);
            if (checkUser != null)    return new ErrorDataResult<string>(message: "Email is already used!");
            AppUser appUser = new AppUser()
            {
                UserName = dto.Username,
                IsActivate = dto.IsActivate,

            };
            var result = await  _userManager.CreateAsync(appUser, dto.Password);
            if (!result.Succeeded)   return new ErrorDataResult<string>(message: string.Join('\n', result.Errors.Select(x => x.Description)));
            
            await _userManager.AddToRoleAsync(appUser, "User");
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

            var url = $"{_http.HttpContext.Request.Scheme}://{_http.HttpContext.Request.Host}{_helper.Action("VerifyEmail", "Identity", new { email = appUser.Email, token = token })}";

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "Templates", "Verify.html");
 
            string body = string.Empty;

            body = System.IO.File.ReadAllText(path);

            body = body.Replace("{{url}}", url);

            await _emailService.SendEmailAsync(appUser.Email, "Verify Email", body,token);

            return new SuccessDataResult<string>(
                message: "RegisterDto olundu"
                );




        }
        public Task<Core.Utilities.Results.Abstract.IResult> Login(LoginDto dto, bool IsUserStatus)
        {
            throw new NotImplementedException();
        }
        public Task<Core.Utilities.Results.Abstract.IResult> VerifyEmail(string token, string email)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Utilities.Results.Abstract.IResult> LogOut()
        {
            throw new NotImplementedException();
        }

        public Task<Core.Utilities.Results.Abstract.IResult> ForgetPassword(string email)
        {
            throw new NotImplementedException();
        }
        public Task<Core.Utilities.Results.Abstract.IResult> ResetPasswordGet(string email, string token)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Utilities.Results.Abstract.IResult> ResetPasswordPost(ResetPasswordDto dto)
        {
            throw new NotImplementedException();
        }
        public Task<Core.Utilities.Results.Abstract.IResult> Update(UpdateDto dto, AppUser? updated)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Utilities.Results.Abstract.IResult> GetAllAdmin(int count, int page)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Utilities.Results.Abstract.IResult> GetAllUsers(int count, int page)
        {
            throw new NotImplementedException();
        }

        public Task<Core.Utilities.Results.Abstract.IResult> GetUser()
        {
            throw new NotImplementedException();
        }


        public Task<Core.Utilities.Results.Abstract.IResult> Remove(string id)
        {
            throw new NotImplementedException();
        }




    }
}
