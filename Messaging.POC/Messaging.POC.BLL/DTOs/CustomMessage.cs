using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.POC.BLL.DTOs
{
    public class CustomMessage
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public string SendSubject { get; set; }
        public string ReplySubject { get; set; }
    }
}
