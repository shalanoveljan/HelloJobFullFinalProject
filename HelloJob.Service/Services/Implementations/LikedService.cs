using HelloJob.Core.Utilities.Results.Concrete;
using HelloJob.Data.DAL.Interfaces;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using Newtonsoft.Json;
using System;

namespace HelloJob.Service.Services.Implementations
{
    public class LikedService : ILikedService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWishlistDAL _wishlistRepository;
        private readonly IWishlistItemDAL _wishlistItemRepository;
        private readonly IResumeDAL _resumeRepository;
        readonly IResumeService _resumeService;
        readonly IHttpContextAccessor _http;


        public LikedService(UserManager<AppUser> userManager, IResumeDAL resumeRepository, IHttpContextAccessor http, IWishlistDAL wishlistRepository, IWishlistItemDAL wishlistItemRepository, IResumeService resumeService)
        {
            _userManager = userManager;
            _resumeRepository = resumeRepository;
            _http = http;
            _wishlistRepository = wishlistRepository;
            _wishlistItemRepository = wishlistItemRepository;
            _resumeService = resumeService;
        }
        //public async Task<WishlistItem> CreateWishlistItem(string itemType, int itemId)
        //{
        //    if (itemType == "Resume")
        //    {
        //        Resume? cv =await _resumeRepository.GetAsync(c => c.Id == itemId);
        //        if (cv is null)
        //        {
        //            return null;
        //        }
        //        return new WishlistItem
        //        {
        //            Id = cv.Id,
                    
        //            IsLiked = true
        //        };
        //    }
        //    return null;
        //}

        public async Task AddToWishlist(int itemId,string itemtype)
        {
            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);
                var wishlist = await _wishlistRepository.GetAsync(x=>!x.IsDeleted&&x.AppUserId==appUser.Id, "WishListItems");

                if(wishlist != null)
                {
                    var wishlistItem = wishlist.WishListItems.Where(x => !x.IsDeleted).FirstOrDefault(x => x.ResumeId == itemId);
                    if (wishlistItem != null)
                    {
                        wishlistItem.IsLiked = false;
                        wishlist.WishListItems.Remove(wishlistItem);
                        await _wishlistRepository.UpdateAsync(wishlist);
                    }

                    else
                    {
                         wishlistItem =new WishlistItem 
                         {
                           ResumeId=itemId,
                           Wishlist=wishlist,
                           IsLiked=true,
                         } ;
                        await _wishlistItemRepository.AddAsync(wishlistItem);
                    }
                }

                else
                {
                    wishlist=new Wishlist { AppUserId= appUser.Id };
                    await _wishlistRepository.AddAsync(wishlist);
                    var item = new WishlistItem
                    {
                        Wishlist = wishlist,
                        ResumeId = itemId,
                        IsLiked = true,
                    };
                    await _wishlistItemRepository.AddAsync(item);
                }
                await _wishlistRepository.SaveChangesAsync();
            }
            else
            {
                List<WishlistPostDto>? WishlistDtos = new List<WishlistPostDto>();

                var wishlistJson = _http.HttpContext.Request.Cookies["wishlist"];

                if (wishlistJson == null)
                {
                    WishlistPostDto wishlistitem = new WishlistPostDto()
                    {
                        Id = itemId,
                        IsLiked= true,
                    };
                    WishlistDtos.Add(wishlistitem);
                }
                else
                {
                    WishlistDtos = JsonConvert.DeserializeObject<List<WishlistPostDto>>(wishlistJson);

                    WishlistPostDto? wishlistdto = WishlistDtos.FirstOrDefault(x => x.Id == itemId);

                    if (wishlistdto == null)
                    {
                        wishlistdto = new WishlistPostDto()
                        {
                            Id = itemId,
                            IsLiked= true,
                        };
                        WishlistDtos.Add(wishlistdto);
                    }
                    else
                    {
                        wishlistdto.IsLiked = false;
                        WishlistDtos.Remove(wishlistdto);
                    }

                }


                wishlistJson = JsonConvert.SerializeObject(WishlistDtos);
                _http.HttpContext.Response.Cookies.Append("wishlist", wishlistJson);
            }
        }
        public async Task<WishlistGetDto> GetWishList()
        {
            WishlistGetDto wishlistgetdto = new WishlistGetDto();
           

            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);

                var wishlistquery = await _wishlistRepository.GetQuery(x => !x.IsDeleted && x.AppUserId == appUser.Id)
                    .Include(x => x.WishListItems)
                    .ThenInclude(c => c.Resume)
                        .ThenInclude(v => v.Category)
                .Include(x => x.WishListItems)
                    .ThenInclude(c => c.Resume)
                        .ThenInclude(v => v.Education)
                .Include(x => x.WishListItems)
                    .ThenInclude(c => c.Resume)
                        .ThenInclude(v => v.Skills)
                .Include(x => x.WishListItems)
                    .ThenInclude(c => c.Resume)
                        .ThenInclude(v => v.City)
                .Include(x => x.WishListItems)
                    .ThenInclude(c => c.Resume)
                        .ThenInclude(v => v.Language)
                .Include(x => x.WishListItems)
                    .ThenInclude(c => c.Resume)
                        .ThenInclude(v => v.experiences)
                    .FirstOrDefaultAsync();

                if (wishlistquery != null)
                {
                    wishlistgetdto.wishlistItems = wishlistquery.WishListItems.Where(X => !X.IsDeleted)
                        .Select(cv=> new WishlistItemDto
                        {
                            Id = cv.Resume.Id,
                            Image = cv.Resume.Image,
                            Name = cv.Resume.Name,
                            Category = new CategoryGetDto { Id = cv.Resume.Category.Id, Name = cv.Resume.Category.Name, Image = cv.Resume.Category.Image, ParentId = cv.Resume.Category.ParentId },
                            Experience = cv.Resume.Experience,
                            Mode = cv.Resume.Mode,
                            Surname = cv.Resume.Surname,
                            ViewCount= cv.Resume.ViewCount,
                             EndDate= cv.Resume.EndDate,
                             Salary= (int)cv.Resume.Salary,
                             IsPremium= cv.Resume.IsPremium,
                            IsLiked = true
                        }).ToList();
                }
            }

            else
            {
                var wishlistJson = _http.HttpContext.Request.Cookies["wishlist"];

                if (wishlistJson != null)
                {
                    List<WishlistItem> wishlistItems = JsonConvert.DeserializeObject<List<WishlistItem>>(wishlistJson);

                    foreach (var item in wishlistItems)
                    {
                        var resume = await _resumeService.GetAsync(item.Id);
                        if (resume != null)
                        {
                            var wishlistItem = new WishlistItemDto
                            {
                                Id = resume.Data.Id,
                                Image = resume.Data.Image,
                                Name = resume.Data.Name,
                                Category = new CategoryGetDto { Id = resume.Data.Category.Id, Name = resume.Data.Category.Name, Image = resume.Data.Category.Image, ParentId = resume.Data.Category.ParentId },
                                Experience = resume.Data.Experience,
                                Mode = resume.Data.Mode,
                                Surname = resume.Data.Surname,
                                ViewCount = resume.Data.ViewCount,
                                EndDate = resume.Data.EndDate,
                                Salary=(int)resume.Data.Salary,
                                IsPremium=resume.Data.IsPremium,
                                IsLiked = true
                            };
                            wishlistgetdto.wishlistItems.Add(wishlistItem);
                        }
                    }
                }
            }
            return wishlistgetdto;
        }

        public WishlistItem GetWishlistItem(List<WishlistItem> wishlistItems, string itemType, int itemId)
        {
            if (itemType == "Resume")
            {
                return  wishlistItems.FirstOrDefault(item => item.ResumeId == itemId);
            }

            return null;
        }
    }
}
