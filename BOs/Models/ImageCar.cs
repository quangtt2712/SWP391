using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class ImageCar
    {
        public ImageCar()
        {
            Cars = new HashSet<Car>();
        }

        public int ImageId { get; set; }
        public string Url { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
