using System;
using System.Collections.Generic;

#nullable disable

namespace BOs.Models
{
    public partial class Ward
    {
        public Ward()
        {
            Showrooms = new HashSet<Showroom>();
        }

        public string WardId { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string FullName { get; set; }
        public string FullNameEn { get; set; }
        public string CodeName { get; set; }
        public string DistrictId { get; set; }
        public int? AdministrativeUnitId { get; set; }

        public virtual District District { get; set; }
        public virtual ICollection<Showroom> Showrooms { get; set; }
    }
}
