using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BOs.Models
{
    public partial class Showroom
    {
        public Showroom()
        {
            Bookings = new HashSet<Booking>();
            Cars = new HashSet<Car>();
        }

        public int ShowroomId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên.")]
        public string ShowroomName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập phone.")]
        public string Phone { get; set; }
        
        public string Website { get; set; }
    
        public string CityId { get; set; }
  
        public string DistrictId { get; set; }
        public string Wards { get; set; }
        
        public int? ImageId { get; set; }

        public virtual City City { get; set; }
        public virtual District District { get; set; }
        public virtual ImageShowroom Image { get; set; }
        public virtual Ward WardsNavigation { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
