using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.DTOS.User;
using HelloJob.Entities.Models;
using HelloJob.Core.Utilities.Results.Abstract;

namespace HelloJob.Service.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<IDataResult<string>> SignUp(RegisterDto dto);
        public Task<IResult> VerifyEmail(string token, string email);
        public Task<IResult> Login(LoginDto dto, bool IsUserStatus);
        public Task<IResult> LogOut();
        public Task<IResult> ForgetPassword(string email);
        public Task<IResult> ResetPasswordGet(string email, string token);
        public Task<IResult> ResetPasswordPost(ResetPasswordDto dto);
        public Task<IResult> GetUser();
        public Task<IResult> Update(UpdateDto dto, AppUser? updated);
        public Task<IResult> GetAllUsers(int count, int page);
        public Task<IResult> GetAllAdmin(int count, int page);
        public Task<IResult> Remove(string id);
    }
}
