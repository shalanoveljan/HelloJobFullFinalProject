using HelloJob.Data.DAL.Interfaces;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public class LikedService : ILikedService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILikeDAL _likeRepository;
        private readonly IResumeDAL _resumeRepository;

        public LikedService(UserManager<AppUser> userManager, ILikeDAL likeRepository, IResumeDAL resumeRepository)
        {
            _userManager = userManager;
            _likeRepository = likeRepository;
            _resumeRepository = resumeRepository;
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
        public WishlistItem GetWishlistItem(List<WishlistItem> wishlistItems, string itemType, int itemId)
        {
            if (itemType == "Resume")
            {
                return  wishlistItems.FirstOrDefault(item => item.ResumeId == itemId);
            }

            return null;
        }

        public async Task<List<Wishlist>> GetWishLists(AppUser user)
        {
            List<Wishlist> wishLists = await _likeRepository.GetQuery(x => !x.IsDeleted)
               .Include(x => x.AppUser)
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
               .Where(w => w.AppUserId == user.Id)
               .ToListAsync();
            return wishLists;
        }
    }
}
