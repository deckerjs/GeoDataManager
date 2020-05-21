using System;
using System.Collections.Generic;
using System.Text;

namespace CoordinateDataModels
{
    public class CoordinateDataSummary : CoordinateDataInfo
    {
        public long DataItemCount { get; set; }
        public Dictionary<string, string> SummaryData { get; set; }
    }
}
