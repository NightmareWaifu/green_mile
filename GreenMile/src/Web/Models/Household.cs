﻿using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace Web.Models
{
    public class Household
    {
        public int HouseholdId { get; set; }
        public string Name { get; set; } = "";
        public ICollection<User> Users { get; set; } = new Collection<User>();
        public User? Owner { get; set; } = null;
        public string? OwnerId { get; set; } = null;
        public string? Address { get; set;}
        public string? InviteLink { get; set; } = Guid.NewGuid().ToString();
        public ICollection<GroceryFoodItem> GroceryItems { get; set; } = new Collection<GroceryFoodItem>();

    }
}