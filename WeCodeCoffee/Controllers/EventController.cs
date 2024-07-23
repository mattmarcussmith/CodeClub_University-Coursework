using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeCodeCoffee.Data;
using WeCodeCoffee.Interface;
using WeCodeCoffee.Models;
using WeCodeCoffee.Repository;
using WeCodeCoffee.ViewModel;

namespace WeCodeCoffee.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IPhotoService _photoService;
    
        public EventController(IEventRepository eventRepository, IPhotoService photoService) 
        {
            _eventRepository = eventRepository;
            _photoService = photoService;
    

        }

        public async Task<IActionResult> Index()
        {
            var allEvents = await _eventRepository.GetAllEventsAsync();

            return View(allEvents);
        }

        public async Task<IActionResult> DetailEvent(int id)
        {
            Event eventDetails = await _eventRepository.GetEventByIdAsync(id);
            return View(eventDetails);
            
        }

        public IActionResult CreateEvent()
        {
            var currentUser = HttpContext.User.GetUserId();
            var createViewModel = new CreateEventViewModel { AppUserId = currentUser };
            return View(createViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventViewModel eventVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(eventVM.Image);

                var newEvent = new Event
                {
                    Title = eventVM.Title,
                    Description = eventVM.Description,
                    AppUserId = eventVM.AppUserId,
                    EventCategory = eventVM.EventCategory,
                    // convert FileFormat to string (the URL of the upload) for datbase storage
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        City = eventVM.Address.City,
                        Street = eventVM.Address.Street,
                        ZipCode = eventVM.Address.ZipCode,
                        State = eventVM.Address.State,
                    },
               
                };
                _eventRepository.Add(newEvent);
                return RedirectToAction("Index", "Dashboard");
            } 
            else
            {
                ModelState.AddModelError("", "Photo upload failed");

            }
            return View(eventVM);
          
        }


        public async Task<IActionResult> Edit(int id)
        {
            var techEvent = await _eventRepository.GetEventByIdAsync(id);
            if (techEvent == null)
            {
                return View("Error");
            }
            var techEventVM = new EditEventViewModel
            {
                Title = techEvent.Title,
                Description = techEvent.Description,
                URL = techEvent.Image,
                AddressId = techEvent.AddressId,
                Address = techEvent.Address,
                EventCategory = techEvent.EventCategory
            };
            return View(techEventVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent(int id, EditEventViewModel eventVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit Event");
                return View(eventVM);
            }
            var editedEvent = await _eventRepository.GetEventByIdAsyncNoTracking(id);
            if (editedEvent != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(editedEvent.Image);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Failed to delete photo: {ex.Message}");
                    return View(eventVM);
                }

                var newPhoto = await _photoService.AddPhotoAsync(eventVM.Image);

                var eventDetail = new Event
                {
                    Id = id,
                    Title = eventVM.Title,
                    Description = eventVM.Description,
                    Image = newPhoto.Url.ToString(),
                    AddressId = eventVM.AddressId,
                    Address = eventVM.Address,
                    EventCategory = eventVM.EventCategory



                };
                _eventRepository.Update(eventDetail);

              
            }
            else
            {

                ModelState.AddModelError("", "Event not found");
                return View(eventVM);
            }

            return RedirectToAction("Index", "Event");

        }

        public async Task<IActionResult> Delete(int id)
        {
            var eventDetails = await _eventRepository.GetEventByIdAsync(id);
            if (eventDetails == null)
            {
                return View("Error");
            }
            return View(eventDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventDetails = await _eventRepository.GetEventByIdAsync(id);
            if (eventDetails == null)
            {
                return View("Error");
            }

            _eventRepository.Delete(eventDetails);
            return RedirectToAction("Index", "Event");
        }

    }
}
