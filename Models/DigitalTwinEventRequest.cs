using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoabCore.Models
{
    public class DigitalTwinEventRequest
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public long DigitalTwin { get; set; }
    }
}