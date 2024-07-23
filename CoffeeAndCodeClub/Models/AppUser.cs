using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeCodeCoffee.Models
{
    public class AppUser : IdentityUser
    {
       

        public int? ContributionScore { get; set; }
        public int? YearJoined { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DeveloperType { get; set; }
        public string? Bio { get; set; }
      

        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public ICollection<Club>? Clubs { get; set; }
        public ICollection<Event>? Events { get; set; }

    }

}
