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
      public Task<Wishlist> GetWishList();
     public  Task AddToWishlist(int itemId, string itemtype);
      public Task<WishlistItem> CreateWishlistItem(string itemType, int itemId);
      public WishlistItem GetWishlistItem(List<WishlistItem> wishlistItems, string itemType, int itemId);
    }
}
