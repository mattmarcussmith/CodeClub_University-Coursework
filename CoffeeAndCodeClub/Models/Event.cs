﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeCodeCoffee.Data.Enum;

namespace WeCodeCoffee.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }


        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address? Address { get; set; }


        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public EventCategory EventCategory { get; set; }




    }
}
