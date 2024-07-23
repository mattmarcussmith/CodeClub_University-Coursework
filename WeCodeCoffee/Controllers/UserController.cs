using Microsoft.AspNetCore.Mvc;
using WeCodeCoffee.Interface;
using WeCodeCoffee.Models;
using WeCodeCoffee.ViewModel;

namespace WeCodeCoffee.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index(string searchTerm)
        {
            IList<AppUser> users;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = await _userRepository.SearchUsersAsync(searchTerm);
            }
            else
            {
                users = await _userRepository.GetAllUsersAsync();
            }

            var userViewModels = users.Select(user => new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                LastLoginTime = user.LastLoginTime ?? DateTime.MinValue,
                DeveloperType = user.DeveloperType,
                Bio = user.Bio,
                Address = user.Address,

            }).ToList();

            return View(userViewModels);
        
        }


        [HttpGet("users/{id}")]
        public async Task<IActionResult> DetailUser(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userDetailViewModel = new DetailUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DeveloperType = user.DeveloperType,
                Bio = user.Bio,
                Address = user.Address,
                LastLoginTime = user.LastLoginTime ?? DateTime.MinValue
            };

            return View(userDetailViewModel);
        }
    }
}
