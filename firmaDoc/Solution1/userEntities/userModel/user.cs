using System;
using System.Collections.Generic;

namespace userEntities.userModel
{
    public partial class user
    {
        public user()
        {
            userPermits = new HashSet<userPermits>();
        }

        public int id { get; set; }
        public string nick { get; set; } = null!;
        public string email { get; set; } = null!;

        public virtual ICollection<userPermits> userPermits { get; set; }
    }
}
