using System;
using System.Collections.Generic;

namespace userEntities.userModel
{
    public partial class permit
    {
        public permit()
        {
            userPermits = new HashSet<userPermits>();
        }

        public int id { get; set; }
        public string name { get; set; } = null!;

        public virtual ICollection<userPermits> userPermits { get; set; }
    }
}
