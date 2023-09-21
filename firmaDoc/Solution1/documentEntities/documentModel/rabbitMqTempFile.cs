using System;
using System.Collections.Generic;

namespace documentEntities.documentModel
{
    public partial class rabbitMqTempFile
    {
        public int id { get; set; }
        public string guid { get; set; } = null!;
        public string fileName { get; set; } = null!;
        public string contentType { get; set; } = null!;
        public int size { get; set; }
        public byte[] content { get; set; } = null!;
    }
}
