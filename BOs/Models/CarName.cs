using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class CarName
    {
        public CarName()
        {
            Cars = new HashSet<Car>();
        }

        public int CarNameId { get; set; }
        public string CarName1 { get; set; }
        public int? ManufactoryId { get; set; }

        public virtual Manufactory Manufactory { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
