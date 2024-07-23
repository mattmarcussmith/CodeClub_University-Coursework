using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using WeCodeCoffee.Helpers;
using WeCodeCoffee.Interface;
using WeCodeCoffee.ViewModel;

namespace WeCodeCoffee.Controllers
{
    public class HomeController : Controller
    {


        private readonly IClubRepository _clubRepository;

        public HomeController(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<IActionResult> IndexAsync()
        {
        
            var homeViewModel = new HomeViewModel();

            try
            {
                string url = "https://ipinfo.io/68.203.213.197/json?token=fe46981df9bb60";
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(url);
                    var ipInfo = JsonConvert.DeserializeObject<IPInfo>(response);
                    RegionInfo regionInfo = new RegionInfo(ipInfo.Country);
                    ipInfo.Country = regionInfo.EnglishName;
                    homeViewModel.City = ipInfo.City;
                    homeViewModel.State = ipInfo.Region;

                    if (!string.IsNullOrEmpty(homeViewModel.City))
                    {
                        homeViewModel.Clubs = await _clubRepository.GetClubsByCityAsync(homeViewModel.City);
                    }
                    else
                    {
                        homeViewModel.Clubs = null;
                    }
                    return View(homeViewModel);

                }
            }
            catch (Exception ex)
            {
                homeViewModel.Clubs = null;
            }
           

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }  
    }
}
