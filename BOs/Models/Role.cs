using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace BOs.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }
        /*[JsonIgnore]*/
        public virtual ICollection<User> Users { get; set; }
    }
}
