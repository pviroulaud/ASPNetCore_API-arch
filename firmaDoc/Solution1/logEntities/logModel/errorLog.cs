using System;
using System.Collections.Generic;

namespace logEntities.logModel
{
    public partial class errorLog
    {
        public int id { get; set; }
        public int? userId { get; set; }
        public string? location { get; set; }
        public DateTime errorDate { get; set; }
        public int errorCode { get; set; }
        public string? detail { get; set; }
        public string? _params { get; set; }
    }
}
