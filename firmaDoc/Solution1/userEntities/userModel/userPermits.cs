using System;
using System.Collections.Generic;

namespace userEntities.userModel
{
    public partial class userPermits
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int permitId { get; set; }
        public bool enabled { get; set; }

        public virtual permit permit { get; set; } = null!;
        public virtual user user { get; set; } = null!;
    }
}
