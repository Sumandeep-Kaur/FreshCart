using FreshCart.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(int productId);
        Task<ReviewDto> AddReviewAsync(int userId, ReviewAddDto reviewDto);
        Task<ReviewDto> UpdateReviewAsync(int userId, int reviewId, ReviewAddDto reviewDto);
        Task DeleteReviewAsync(int userId, int reviewId);
    }
}
