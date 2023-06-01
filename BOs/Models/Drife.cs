using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class Drife
    {
        public Drife()
        {
            Cars = new HashSet<Car>();
        }

        public int DriveId { get; set; }
        public string DriveName { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
