using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Orders = new List<Order>();
        }

        public Cart? Cart { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
