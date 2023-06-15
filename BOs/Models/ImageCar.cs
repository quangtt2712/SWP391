
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace BOs.Models
{
    public partial class ImageCar
    {
        public int ImageId { get; set; }
        public bool? ImageMain { get; set; }
        public int? CarId { get; set; }
        public string Url { get; set; }
        [JsonIgnore]
        public virtual Car Car { get; set; }
    }
}
