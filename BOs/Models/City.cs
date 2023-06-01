using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class City
    {
        public City()
        {
            Districts = new HashSet<District>();
            Showrooms = new HashSet<Showroom>();
        }

        public string CityId { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string FullName { get; set; }
        public string FullNameEn { get; set; }
        public string CodeName { get; set; }
        public int? AdministrativeUnitId { get; set; }
        public int? AdministrativeRegionId { get; set; }

        public virtual ICollection<District> Districts { get; set; }
        public virtual ICollection<Showroom> Showrooms { get; set; }
    }
}
