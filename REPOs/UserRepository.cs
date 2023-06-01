using BOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOs
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(OldCarShowroomNetworkContext context) : base(context)
        {
        }
    }
}
