using System;
using System.Collections.Generic;

namespace fileEntities.fileModel
{
    public partial class userFile
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string fileName { get; set; } = null!;
        public string contentType { get; set; } = null!;
        public int size { get; set; }
        public bool cipher { get; set; }
        public byte[] content { get; set; } = null!;
    }
}
