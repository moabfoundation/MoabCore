using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoabCore.Models
{
    public class DigitalTwinModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Organization { get; set; }
        public long Version { get; set; }

    }
}
