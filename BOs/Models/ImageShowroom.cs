using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class ImageShowroom
    {
        public ImageShowroom()
        {
            Showrooms = new HashSet<Showroom>();
        }

        public int ImageId { get; set; }
        public string Url { get; set; }

        public virtual ICollection<Showroom> Showrooms { get; set; }
    }
}
