using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class Fuel
    {
        public Fuel()
        {
            Cars = new HashSet<Car>();
        }

        public int FuelId { get; set; }
        public string FuelName { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
