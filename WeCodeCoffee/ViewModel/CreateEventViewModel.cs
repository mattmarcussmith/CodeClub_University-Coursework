using WeCodeCoffee.Data.Enum;
using WeCodeCoffee.Models;

namespace WeCodeCoffee.ViewModel
{
    public class CreateEventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public IFormFile Image { get; set; }
        public EventCategory EventCategory { get; set; }

       public string AppUserId { get; set; }
       
    }
}
