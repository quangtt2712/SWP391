using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class Slot
    {
        public Slot()
        {
            Bookings = new HashSet<Booking>();
        }

        public int SlotId { get; set; }
        public TimeSpan? PickupDate { get; set; }
        public TimeSpan? ReturnDate { get; set; }
        public bool? IsBooked { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
