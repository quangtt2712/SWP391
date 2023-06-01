using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            Cars = new HashSet<Car>();
        }

        public int VehiclesId { get; set; }
        public string VehiclesName { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
