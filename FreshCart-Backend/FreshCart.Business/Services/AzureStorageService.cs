using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using FreshCart.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _productContainerName = "product-images";
        private readonly string _categoryContainerName = "category-images";
        private readonly ILogger<AzureStorageService> _logger;

        public AzureStorageService(
            BlobServiceClient blobServiceClient,
            ILogger<AzureStorageService> logger)
        {
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public async Task<string> UploadProductImageAsync(IFormFile file)
        {
            return await UploadImageAsync(file, _productContainerName);
        }

        public async Task<string> UploadCategoryImageAsync(IFormFile file)
        {
            return await UploadImageAsync(file, _categoryContainerName);
        }

        private async Task<string> UploadImageAsync(IFormFile file, string containerName)
        {
            if (file == null || file.Length == 0)
            {
                throw new InvalidOperationException("No image file provided");
            }

            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
            {
                throw new InvalidOperationException("Invalid file type. Only JPEG, PNG are allowed.");
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                throw new InvalidOperationException("File size exceeds maximum limit of 5MB");
            }

            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var blobClient = containerClient.GetBlobClient(fileName);

                await using var stream = file.OpenReadStream();
                await blobClient.UploadAsync(stream, new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                });

                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image to Azure Storage");
                throw new Exception("Failed to upload image", ex);
            }
        }

        public async Task DeleteProductImageAsync(string imageUrl)
        {
            await DeleteImageAsync(imageUrl, _productContainerName);
        }

        public async Task DeleteCategoryImageAsync(string imageUrl)
        {
            await DeleteImageAsync(imageUrl, _categoryContainerName);
        }

        private async Task DeleteImageAsync(string imageUrl, string containerName)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }

            try
            {
                var uri = new Uri(imageUrl);
                var blobName = Path.GetFileName(uri.LocalPath);
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(blobName);

                await blobClient.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image from Azure Storage");
                throw new Exception("Failed to delete image", ex);
            }
        }
    }
}
