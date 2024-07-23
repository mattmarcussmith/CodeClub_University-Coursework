using WeCodeCoffee.Models;

namespace WeCodeCoffee.ViewModel
{
    public class DetailUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? DeveloperType { get; set; }
        public string? ProfilePictureUrl { get; set; }
        
        public Address? Address { get; set; }

        public DateTime? LastLoginTime { get; set; }
    }
}
