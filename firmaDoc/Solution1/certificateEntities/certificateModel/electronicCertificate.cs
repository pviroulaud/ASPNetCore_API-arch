using System;
using System.Collections.Generic;

namespace certificateEntities.certificateModel
{
    public partial class electronicCertificate
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string pfx { get; set; } = null!;
        public string cer { get; set; } = null!;
        public string pass { get; set; } = null!;
        public DateTime creationDate { get; set; }
        public DateTime expiratioDate { get; set; }
        public bool valid { get; set; }
    }
}
