using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class Booking
    {
        public string Username { get; set; }
        public int CarId { get; set; }
        public DateTime PickupHour { get; set; }
        public DateTime CreationDateTime { get; set; }
        public int? ShowroomId { get; set; }

        public virtual Car Car { get; set; }
        public virtual Showroom Showroom { get; set; }
        public virtual User UsernameNavigation { get; set; }
    }
}
