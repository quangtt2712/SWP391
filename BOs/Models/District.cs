using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class District
    {
        public District()
        {
            Showrooms = new HashSet<Showroom>();
            Wards = new HashSet<Ward>();
        }

        public string DistrictId { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string FullName { get; set; }
        public string FullNameEn { get; set; }
        public string CodeName { get; set; }
        public string CityId { get; set; }
        public int? AdministrativeUnitId { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Showroom> Showrooms { get; set; }
        public virtual ICollection<Ward> Wards { get; set; }
    }
}
