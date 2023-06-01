using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class Manufactory
    {
        public Manufactory()
        {
            CarNames = new HashSet<CarName>();
            Cars = new HashSet<Car>();
        }

        public int ManufactoryId { get; set; }
        public string ManufactoryName { get; set; }

        public virtual ICollection<CarName> CarNames { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
