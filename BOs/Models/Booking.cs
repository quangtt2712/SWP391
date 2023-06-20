using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class Booking
    {
        public string Username { get; set; }
        public int CarId { get; set; }
        public int Slot { get; set; }
        public int? Notification { get; set; }
        public string Note { get; set; }
        public DateTime? DayBooking { get; set; }

        public virtual Car Car { get; set; }
        public virtual Slot SlotNavigation { get; set; }
        public virtual User UsernameNavigation { get; set; }
    }
}
