//using HelloJob.Data.DBContexts.SQLSERVER;
//using HelloJob.Entities.Models;
//using HelloJob.Service.Services.Implementations;
//using HelloJob.Service.Services.Interfaces;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace HelloJob.App.Controllers
//{
//    public class LikeController : Controller
//    {
//        private readonly ILikedService _likeService;
//        private readonly UserManager<AppUser> _usermanager;
//        private readonly HelloJobDbContext _context;

//        public LikeController(ILikedService likeService, UserManager<AppUser> usermanager, HelloJobDbContext context)
//        {
//            _likeService = likeService;
//            _usermanager = usermanager;
//            _context = context;
//        }

//        public async Task<IActionResult> Index()
//        {
//            if (User.Identity.IsAuthenticated)
//            {
//                AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);

//                if (user is null)
//                {
//                    return RedirectToAction("Index", "Home");
//                }
//                Wishlist wishlist = await _likeService.GetWishList(user);
//                if(wishlist is null)
//                {
//                    return Json("Wishlist is null");
//                }

//                return View(wishlist);
//            }
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddToWishlist(int itemId, string itemType)
//        {
//            // Find User
//            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
//            if (user is null)
//            {
//                return RedirectToAction("Index", "Home");
//            }
//            // get wishlist of user or create
//            Wishlist wishlist = await _context.Wishlists
//                .Include(w => w.WishListItems)
//                .FirstOrDefaultAsync(w => w.AppUserId == user.Id);

//            if (wishlist is null)
//            {
//                wishlist = new Wishlist
//                {
//                    AppUserId = user.Id,
//                    AppUser = user,
//                    WishListItems = new List<WishlistItem>()
//                };

//                _context.Wishlists.Add(wishlist);
//                await _context.SaveChangesAsync();
//            }

//            // check wishlistItem
//            WishlistItem existingItem = wishlist.WishListItems.FirstOrDefault(item => item.ResumeId == itemId);
//            if (existingItem != null)
//            {
//                wishlist.WishListItems.Remove(existingItem);
//                await _context.SaveChangesAsync();
//                return Json("Removed from wishlist.");
//            }

//            // add wishlistitem in Wishlist
//            WishlistItem wishlistItem = await _likeService.CreateWishlistItem(itemType, itemId);
//            if (wishlistItem is null)
//            {
//                return Json("Failed to add to wishlist.");
//            }

//            wishlist.WishListItems.Add(wishlistItem);
//            await _context.SaveChangesAsync();

//            return Json("Added to wishlist.");
//        }

//    public async Task<IActionResult> RemoveFromWishlist(int itemId, string itemType)
//        {
//            AppUser user = await _usermanager.FindByNameAsync(User.Identity.Name);

//            if (user is null)
//            {
//                return RedirectToAction("Index", "Home");
//            }

//            Wishlist? wishlist = _context.Wishlists
//                .Include(w => w.WishListItems)
//                .FirstOrDefault(w => w.AppUserId == user.Id);

//            if (wishlist is null)
//            {
//                return Redirect(Request.Headers["Referer"].ToString());
//            }

//            WishlistItem wishlistItem = _likeService.GetWishlistItem(wishlist.WishListItems, itemType, itemId);
//            if (wishlistItem != null)
//            {
//                wishlist.WishListItems.Remove(wishlistItem);
//                _context.SaveChanges();
//            }
//            return Redirect(Request.Headers["Referer"].ToString());
//        }

//    }
//}
