using System;
using System.Collections.Generic;

namespace documentEntities.documentModel
{
    public partial class document
    {
        public int id { get; set; }
        public string title { get; set; } = null!;
        public int userId { get; set; }
        public int? userFileId { get; set; }
        public DateTime? userFileStorageDate { get; set; }
        public int? signedUserFileId { get; set; }
        public DateTime? userFileSignDate { get; set; }
    }
}
