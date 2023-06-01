using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class ShowroomCar
    {
        public int CarId { get; set; }
        public int ShowroomId { get; set; }

        public virtual Car Car { get; set; }
        public virtual Showroom Showroom { get; set; }
    }
}
