﻿using WeCodeCoffee.Data.Enum;
using WeCodeCoffee.Models;

namespace WeCodeCoffee.ViewModel
{
    public class EditClubViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string? URL { get; set;  }
        public int AddressId { get; set; }    
        public Address? Address { get; set; }
   

        public ClubCategory ClubCategory { get; set; }

    }
}
