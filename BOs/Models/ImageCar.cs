using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class ImageCar
    {
        public int ImageId { get; set; }
        public bool? ImageMain { get; set; }
        public int? CarId { get; set; }
        public string Url { get; set; }

        public virtual Car Car { get; set; }
    }
}
