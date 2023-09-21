using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace usersDTO
{
    public class permitDTO
    {
        public int id { get; set; }
        public string name { get; set; } = null!;
    }
    public class userPermitDTO
    {
        public int userId { get; set; }
        public int permitId { get; set; }
        public bool enabled { get; set; }

    }
}
