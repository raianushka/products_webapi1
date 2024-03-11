using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INV.Models
{
    public class ResultClassName
    {
        public MessageResult Result { get; set; } = new MessageResult();
        public dynamic Data { get; set; }
    }
     public class MessageResult
    {
        public bool Flag { get; set; }
        public string Message { get; set; }
        public bool IsZip { get; set; } = false;

    }
}
