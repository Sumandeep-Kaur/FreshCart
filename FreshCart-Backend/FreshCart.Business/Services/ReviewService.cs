using AutoMapper;
using FreshCart.Business.DTOs;
using FreshCart.Business.Interfaces;
using FreshCart.Data.Entities;
using FreshCart.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(int productId)
        {
            var reviews = await _unitOfWork.Reviews.Query()
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<ReviewDto> AddReviewAsync(int userId, ReviewAddDto reviewDto)
        {
            var existingReview = await _unitOfWork.Reviews.Query()
                .FirstOrDefaultAsync(r => r.UserId == userId && r.ProductId == reviewDto.ProductId);

            if (existingReview != null)
            {
                throw new InvalidOperationException("You have already reviewed this product");
            }

            var product = await _unitOfWork.Products.GetByIdAsync(reviewDto.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            // Verify user has purchased the product
            var hasPurchased = await _unitOfWork.Orders.Query()
                .Include(o => o.Items)
                .AnyAsync(o => o.UserId == userId &&
                              o.Items.Any(i => i.ProductId == reviewDto.ProductId));

            if (!hasPurchased)
            {
                throw new InvalidOperationException("You can only review products you have purchased");
            }

            var review = _mapper.Map<Review>(reviewDto);
            review.UserId = userId;

            await _unitOfWork.Reviews.AddAsync(review);
            product.Reviews.Add(review);
            await _unitOfWork.SaveChangesAsync();

            await UpdateProductAverageRating(reviewDto.ProductId);

            // Fetch review with user info
            review = await _unitOfWork.Reviews.Query()
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == review.Id);

            return _mapper.Map<ReviewDto>(review);
        }

        public async Task<ReviewDto> UpdateReviewAsync(int userId, int reviewId, ReviewAddDto reviewDto)
        {
            var review = await _unitOfWork.Reviews.Query()
                .FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);

            if (review == null)
            {
                throw new KeyNotFoundException("Review not found");
            }

            review.Rating = reviewDto.Rating;
            review.Comment = reviewDto.Comment;
            review.ReviewDate = reviewDto.ReviewDate;

            await _unitOfWork.Reviews.UpdateAsync(review);
            await _unitOfWork.SaveChangesAsync();

            await UpdateProductAverageRating(reviewDto.ProductId);

            review = await _unitOfWork.Reviews.Query().Include(r => r.User).FirstOrDefaultAsync(r => r.Id == review.Id);
            return _mapper.Map<ReviewDto>(review);
        }

        public async Task DeleteReviewAsync(int userId, int reviewId)
        {
            var review = await _unitOfWork.Reviews.Query()
                .FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);

            if (review == null)
            {
                throw new KeyNotFoundException("Review not found");
            }

            await _unitOfWork.Reviews.DeleteAsync(reviewId);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task UpdateProductAverageRating(int productId)
        {
            var product = await _unitOfWork.Products.Query()
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product != null && product.Reviews.Any())
            {
                product.AverageRating = (int)product.Reviews.Average(r => r.Rating);
            }

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
