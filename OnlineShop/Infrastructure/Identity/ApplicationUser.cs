using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Orders = new List<Order>();
        }

        public Cart? Cart { get; set; }
        public ICollection<Order> Orders {  get; set; }
    }
}
