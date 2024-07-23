using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using WeCodeCoffee.Data;
using WeCodeCoffee.Interface;
using WeCodeCoffee.Models;
using WeCodeCoffee.ViewModel;

namespace WeCodeCoffee.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IDashboardRepository _dashboardRepository;
        private readonly IPhotoService _photoService;
        public DashboardController(IDashboardRepository dashboardRepository, IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _photoService = photoService;
        }
        public void MapUserEdit(AppUser user, EditUserProfileViewModel editVM, ImageUploadResult photoResult)
        {

            user.FirstName = editVM.FirstName;
            user.UserName = editVM.UserName;
            user.LastName = editVM.LastName;
            user.DeveloperType = editVM.DeveloperType;
            user.Bio = editVM.Bio;
            user.Address = editVM.Address;
            user.ProfilePictureUrl = photoResult.Url.ToString();
              
        }

        public async Task<IActionResult> Index()
        {
            var userClubs = await _dashboardRepository.GetAllUserClubsAsync();
            var userEvents = await _dashboardRepository.GetAllUserEventsAsync();

            var dashboardViewModel = new DashboardViewModel()
            {
                Clubs = userClubs,
                Events = userEvents
            };

            return View(dashboardViewModel);

        }
   

        public async Task<IActionResult> EditUserProfile()
        {
            // Gets the current users ID from the HttpContext after the user has been authenticated
            // Then passes the ID to the GetUserByIdAsync method in the DashboardRepository

            var currentUserId = HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserByIdAsync(currentUserId);

            if (user == null) return View("Error");

            var editUserViewModel = new EditUserProfileViewModel()
            {
                Id = currentUserId,
                ProfilePictureUrl = user.ProfilePictureUrl,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DeveloperType = user.DeveloperType,
                Bio = user.Bio,
                Address = user.Address,
                UserName = user.UserName

            };
            return View(editUserViewModel);


        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserProfileViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editVM);
            }

            var user = await _dashboardRepository.GetUserByIdNoTrackingAsync(editVM.Id);

            if (user.ProfilePictureUrl == "" || user.ProfilePictureUrl == null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

          
                MapUserEdit(user, editVM, photoResult);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfilePictureUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Failed to delete photo: {ex.Message}");
                    return View(editVM);

                }

                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                MapUserEdit(user, editVM, photoResult);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
             


        }
    }
}
