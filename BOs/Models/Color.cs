using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class Color
    {
        public Color()
        {
            CarColorInsideNavigations = new HashSet<Car>();
            CarColorNavigations = new HashSet<Car>();
        }

        public int ColorId { get; set; }
        public string ColorName { get; set; }

        public virtual ICollection<Car> CarColorInsideNavigations { get; set; }
        public virtual ICollection<Car> CarColorNavigations { get; set; }
    }
}
