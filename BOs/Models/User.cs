using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            Cars = new HashSet<Car>();
        }

        public string Username { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public int? RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
