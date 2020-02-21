using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoabCore.Models
{
    public class DigitalTwinEvent
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public long DigitalTwin { get; set; }
        public DateTime Timestamp { get; set; }
    }
}