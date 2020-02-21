using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoabCore.Models
{
    public class DigitalTwinModelLimitedResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Version { get; set; }
    }
}