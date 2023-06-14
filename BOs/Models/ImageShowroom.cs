using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class ImageShowroom
    {
        public int ImageId { get; set; }
        public bool? ImageMain { get; set; }
        public int? ShowroomId { get; set; }
        public string Url { get; set; }

        public virtual Showroom Showroom { get; set; }
    }
}
