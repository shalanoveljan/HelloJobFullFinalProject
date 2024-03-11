using HelloJob.Data.DAL.Interfaces;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

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
        public async Task<WishlistItem> CreateWishlistItem(string itemType, int itemId)
        {
            if (itemType == "Resume")
            {
                Resume? cv =await _resumeRepository.GetAsync(c => c.Id == itemId);
                if (cv is null)
                {
                    return null;
                }
                return new WishlistItem
                {
                    ResumeId = cv.Id,
                    IsLiked = true
                };
            }

            return null;
        }

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
                        wishlist.WishListItems.Remove(wishlistItem);
                        await _wishlistRepository.UpdateAsync(wishlist);
                    }

                    else
                    {
                        WishlistItem wishlist_item = await CreateWishlistItem(itemtype, itemId);
                        await _wishlistItemRepository.AddAsync(wishlist_item);
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
                List<WishlistItem>? wishlistitems = new List<WishlistItem>();

                var wishlistJson = _http.HttpContext.Request.Cookies["wishlist"];

                if (wishlistJson == null)
                {
                    WishlistItem wishlistitem = new WishlistItem()
                    {
                        Id = itemId,
                    };
                    wishlistitems.Add(wishlistitem);
                }
                else
                {
                    wishlistitems = JsonConvert.DeserializeObject<List<WishlistItem>>(wishlistJson);

                    WishlistItem? wishlistitemdto = wishlistitems.FirstOrDefault(x => x.Id == itemId);

                    if (wishlistitemdto == null)
                    {
                        wishlistitemdto = new WishlistItem()
                        {
                            Id = itemId,
                        };
                        wishlistitems.Add(wishlistitemdto);
                    }
                    else
                    {
                        wishlistitems.Remove(wishlistitemdto);
                    }

                }


                wishlistJson = JsonConvert.SerializeObject(wishlistitems);
                _http.HttpContext.Response.Cookies.Append("wishlist", wishlistJson);
            }
        }
        public WishlistItem GetWishlistItem(List<WishlistItem> wishlistItems, string itemType, int itemId)
        {
            if (itemType == "Resume")
            {
                return  wishlistItems.FirstOrDefault(item => item.ResumeId == itemId);
            }

            return null;
        }
        public async Task<Wishlist> GetWishList()
        {
            Wishlist wishlist = new Wishlist();

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
                    wishlist.WishListItems = wishlistquery.WishListItems.Where(X => !X.IsDeleted).ToList();
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
                            var wishlistItem = new WishlistItem
                            {
                                Id = resume.Data.Id,
                                
                            };
                            wishlist.WishListItems.Add(wishlistItem);
                        }
                    }
                }
            }



            return wishlist;
        }

    }
}
