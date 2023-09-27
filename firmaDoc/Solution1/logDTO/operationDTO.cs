using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logDTO
{
    public class operationDTO
    {        
        public int id { get; set; }
        public int? userId { get; set; }
        public int operationId { get; set; }
        public DateTime operationDate { get; set; }
        public string? entity { get; set; }
        public string? description { get; set; }
    }
}
