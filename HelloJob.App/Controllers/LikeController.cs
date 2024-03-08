using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Implementations;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloJob.App.Controllers
{
    public class LikeController : Controller
    {
        private readonly ILikedService _likeService;
        private readonly UserManager<AppUser> _usermanager;
        private readonly HelloJobDbContext _context;

        public LikeController(ILikedService likeService, UserManager<AppUser> usermanager, HelloJobDbContext context)
        {
            _likeService = likeService;
            _usermanager = usermanager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);

                if (user is null)
                {
                    return RedirectToAction("Index", "Home");
                }
                List<Wishlist> wishlists = await _likeService.GetWishLists(user);

                return View(wishlists);
            }
            return View();
        }

        public async Task<IActionResult> AddToWishlist(int itemId, string itemType)
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);

            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }

            Wishlist wishlist = _context.Wishlists
                .Include(w => w.WishListItems)
                .FirstOrDefault(w => w.AppUserId == user.Id);

            if (wishlist is null)
            {
                wishlist = new Wishlist
                {
                    AppUserId = user.Id,
                    AppUser = user,
                    WishListItems = new List<WishlistItem>()
                };

                _context.Wishlists.Add(wishlist);
            }

            WishlistItem existingItem = wishlist.WishListItems.FirstOrDefault(item => item.ResumeId == itemId);
            if (existingItem != null)
            {
                wishlist.WishListItems.Remove(existingItem);
                _context.SaveChanges();
                return Redirect(Request.Headers["Referer"].ToString());
            }

            WishlistItem wishlistItem =await _likeService.CreateWishlistItem(itemType, itemId);
            if (wishlistItem is null)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }

            wishlist.WishListItems.Add(wishlistItem);
            _context.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> RemoveFromWishlist(int itemId, string itemType)
        {
            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);

            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }

            Wishlist? wishlist = _context.Wishlists
                .Include(w => w.WishListItems)
                .FirstOrDefault(w => w.AppUserId == user.Id);

            if (wishlist is null)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }

            WishlistItem wishlistItem = _likeService.GetWishlistItem(wishlist.WishListItems, itemType, itemId);
            if (wishlistItem != null)
            {
                wishlist.WishListItems.Remove(wishlistItem);
                _context.SaveChanges();
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}
