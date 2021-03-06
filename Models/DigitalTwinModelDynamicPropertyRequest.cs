﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoabCore.Models
{
    public class DigitalTwinModelDynamicPropertyRequest
    {
        public string Name { get; set; }
        public long MeasurementType { get; set; }
        public long DataType { get; set; }
        public long UnitOfMeasure { get; set; }
        public long DigitalTwinModel { get; set; }
    }
}