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
        Task<Wishlist> GetWishList();
        Task AddToWishlist(int itemId, string itemtype);
        Task<WishlistItem> CreateWishlistItem(string itemType, int itemId);
        WishlistItem GetWishlistItem(List<WishlistItem> wishlistItems, string itemType, int itemId);
    }
}
