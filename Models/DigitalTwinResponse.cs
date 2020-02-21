using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoabCore.Models
{
    public class DigitalTwinResponse
    {
        public long Id { get; set; }
        public long SecurityToken { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
