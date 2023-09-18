using System;
using System.Collections.Generic;

namespace logEntities.logModel
{
    public partial class operationLog
    {
        public int id { get; set; }
        public int? userId { get; set; }
        public int operationId { get; set; }
        public DateTime operationDate { get; set; }
        public string? entity { get; set; }
        public string? description { get; set; }

        public virtual operation operation { get; set; } = null!;
    }
}
