﻿using System;
using System.Collections.Generic;

namespace Knapcode.CheckRepublic.Logic.Runner
{
    public class CheckBatch
    {
        public string MachineName { get; set; }
        public DateTimeOffset Time { get; set; }
        public TimeSpan Duration { get; set; }
        public List<CheckResult> CheckResults { get; set; }
    }
}
