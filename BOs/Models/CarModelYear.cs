using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class CarModelYear
    {
        public CarModelYear()
        {
            Cars = new HashSet<Car>();
        }

        public int CarModelYearId { get; set; }
        public int? CarModelYear1 { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
