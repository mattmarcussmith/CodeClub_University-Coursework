using CloudinaryDotNet.Actions;

namespace WeCodeCoffee.Interface
{
    public interface IPhotoService
    {
        /// <summary>
        /// Purpose: Service to handle photo uploads and deletions
        /// Performance: Removes expensive database calls
        /// </summary>
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
