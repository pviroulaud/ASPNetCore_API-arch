using System;
using System.Collections.Generic;

namespace logEntities.logModel
{
    public partial class operation
    {
        public operation()
        {
            operationLog = new HashSet<operationLog>();
        }

        public int id { get; set; }
        public string name { get; set; } = null!;
        public string? description { get; set; }

        public virtual ICollection<operationLog> operationLog { get; set; }
    }
}
