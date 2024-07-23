using WeCodeCoffee.Models;

namespace WeCodeCoffee.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Club> Clubs { get; set; }
        public IEnumerable<Event> Events { get; set; }

       public string City { get; set; }
       public string State { get; set; }

    }
}
