using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeCodeCoffee.Data;
using WeCodeCoffee.Interface;
using WeCodeCoffee.Models;
using WeCodeCoffee.Repository;
using WeCodeCoffee.ViewModel;

namespace WeCodeCoffee.Controllers
{
    public class ClubController : Controller
    {

        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;


        public ClubController(IClubRepository clubRepository, IPhotoService photoService)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
  
        }

  
        public async Task<IActionResult> Index()
        {
            var clubs = await _clubRepository.GetAllClubsAsync();

            return View(clubs);
        }

        public async Task<IActionResult> DetailClub(int id)
        {
            var clubDetails = await _clubRepository.GetClubByIdAsync(id);

            return View(clubDetails);
        }

        public IActionResult CreateClub()
        {
            var currentUser =  HttpContext.User.GetUserId();
            var createViewModel = new CreateClubViewModel { AppUserId = currentUser};
            return View(createViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClub(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);

                var newclub = new Club
                {

                    Id = clubVM.Id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    AppUserId = clubVM.AppUserId,
                    ClubCategory = clubVM.ClubCategory,



                    // convert FileFormat to string (the URL of the upload) for datbase storage
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        City = clubVM.Address.City,
                        Street = clubVM.Address.Street,
                        ZipCode = clubVM.Address.ZipCode,
                        State = clubVM.Address.State,
                    },
                };

                _clubRepository.Add(newclub);
                return RedirectToAction("Index", "Dashboard");

            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
            return View(clubVM);
        }

        public async Task<IActionResult> EditClub(int id)
        {
            var club = await _clubRepository.GetClubByIdAsync(id);
            if (club == null)
            {
                return View("Error");
            }
            var clubVM = new EditClubViewModel
            {
               
                Title = club.Title,
                Description = club.Description,
                URL = club.Image,
                AddressId = club.AddressId,
                Address = club.Address,
                ClubCategory = club.ClubCategory
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditClub(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit Club");
                return View(clubVM);
            }
            var editedClub = await _clubRepository.GetClubByIdAsyncNoTracking(id);
            if (editedClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(editedClub.Image);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Failed to delete photo: {ex.Message}");
                    return View(clubVM);
                }

                var newPhoto = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = newPhoto.Url.ToString(),
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address,
                    ClubCategory = clubVM.ClubCategory



                };
                _clubRepository.Update(club);
               
           
            } 
            else
            {

               ModelState.AddModelError("", "Club not found");
                return View(clubVM);
            }

            return RedirectToAction("Index", "Club");

        } 

        public async Task<IActionResult> Delete(int id)
        {
            var club = await _clubRepository.GetClubByIdAsync(id);
            if (club == null)
            {
                return View("Error");
            }
            return View(club);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var club = await _clubRepository.GetClubByIdAsync(id);
            if (club == null)
            {
                return View("Error");
            }
       
            _clubRepository.Delete(club);
            return RedirectToAction("Index", "Club");
        }


       
    }
}
