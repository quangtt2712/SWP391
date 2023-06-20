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
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
