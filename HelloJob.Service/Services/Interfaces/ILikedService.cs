using HelloJob.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Interfaces
{
    public interface ILikedService
    {
        Task<List<Wishlist>> GetWishLists(AppUser user);
        Task<WishlistItem> CreateWishlistItem(string itemType, int itemId);
        WishlistItem GetWishlistItem(List<WishlistItem> wishlistItems, string itemType, int itemId);
    }
}
