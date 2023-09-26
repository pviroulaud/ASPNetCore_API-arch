using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileDTO
{
    public class rabbitMqFileTransferDTO : userFileDTO
    {
        public int documentId { get; set; }
        public bool requireSign { get; set; }
      
    }

    public class syncDocumentInfoDTO
    {
        public int documentId { get; set; }
        public bool requireSign { get; set; }
        public int userId { get; set; }

        public bool cipher { get; set; }

    }
}
