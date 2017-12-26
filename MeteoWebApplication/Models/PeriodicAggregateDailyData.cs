using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeteoWebApplication.Models
{
    public class PeriodicAggregateDailyData
    {
        public string measureType { get; set; }
        public string um { get; set; }
        public string max { get; set; }
        public string hourmax { get; set; }
        public string min { get; set; }
        public string hourmin { get; set; }
        public string mean { get; set; }
        public string escursion { get; set; }
        public string nmeasurement { get; set; }
    }
}