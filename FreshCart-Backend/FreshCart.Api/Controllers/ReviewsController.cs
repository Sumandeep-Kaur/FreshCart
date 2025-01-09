using FreshCart.Business.DTOs;
using FreshCart.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreshCart.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(IReviewService reviewService, ILogger<ReviewsController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetProductReviews(int productId)
        {
            try
            {
                var reviews = await _reviewService.GetProductReviewsAsync(productId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product reviews");
                return StatusCode(500, "An error occurred while retrieving reviews");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReviewDto>> AddReview(ReviewAddDto reviewDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var review = await _reviewService.AddReviewAsync(userId, reviewDto);
                return Ok(review);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding review");
                return StatusCode(500, "An error occurred while adding the review");
            }
        }

        [HttpPut("{reviewId}")]
        public async Task<ActionResult<ReviewDto>> UpdateReview(int reviewId, ReviewAddDto reviewDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var review = await _reviewService.UpdateReviewAsync(userId, reviewId, reviewDto);
                return Ok(review);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating review");
                return StatusCode(500, "An error occurred while updating the review");
            }
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _reviewService.DeleteReviewAsync(userId, reviewId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review");
                return StatusCode(500, "An error occurred while deleting the review");
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user token");
            }
            return userId;
        }
    }
}
